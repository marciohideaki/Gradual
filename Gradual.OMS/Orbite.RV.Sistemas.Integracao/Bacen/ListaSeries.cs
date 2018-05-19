using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

using Orbite.RV.Contratos.Integracao.Bacen;

namespace Orbite.RV.Sistemas.Integracao.Bacen
{
    public class ListaSeries
    {
        private DsBCB _ds = null;
        private WebBrowser _webBrowser = null;
        private ListaSeriesProgress _progress = null;
        private Timer _timer = null;
        private Queue<string> _queue = new Queue<string>();
        private int _totalFontes = 0;

        public ListaSeries(DsBCB ds)
        {
            _ds = ds;
            _progress = new ListaSeriesProgress();
        }

        public ListaSeriesProgress Progress
        {
            get { return _progress; }
        }

        public void Start()
        {
            // Cria webbrowser
            _webBrowser = new WebBrowser();
            _webBrowser.Navigate("https://www3.bcb.gov.br/sgspub/");

            // Inicia timer para pegar lista de fontes
            _timer = new Timer();
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Interval = 400;
            _timer.Start();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            // Informa
            _progress.NotificaEtapa("Iniciando pesquisa de fontes", 0);

            if (!_webBrowser.IsBusy && _webBrowser.Document != null)
            {
                HtmlElement fonte = _webBrowser.Document.GetElementById("fonte");
                if (fonte != null)
                {
                    HtmlElementCollection options = fonte.GetElementsByTagName("option");
                    if (options.Count > 0)
                    {
                        _timer.Stop();
                        _timer.Tick -= new EventHandler(_timer_Tick);
                        _timer.Dispose();
                        _timer = null;

                        // Carrega a lista de fontes
                        _ds.TbFonte.Rows.Clear();
                        _ds.TbSerie.Rows.Clear();
                        foreach (HtmlElement option in options)
                        {
                            _ds.TbFonte.Rows.Add(option.GetAttribute("value"), option.InnerText);
                            _queue.Enqueue(option.GetAttribute("value"));
                        }

                        // Informa
                        _totalFontes = _ds.TbFonte.Rows.Count;
                        _progress.NotificaQuantidadeFontes(_totalFontes);

                        // Navega para o primeiro item
                        string primeiraSerie = _queue.Dequeue();
                        _webBrowser.Document.GetElementById("fonte").SetAttribute("value", primeiraSerie);
                        _webBrowser.Document.InvokeScript("localizarSeriesPorFonte", new object[] { "../localizarseries/localizarSeries.do?method=localizarSeriesPorFonte", 5 });

                        // Informa
                        string etapa = "Recebendo fonte {0}/{1} Página 1";
                        _progress.NotificaEtapa(string.Format(etapa, _totalFontes - _queue.Count, _totalFontes), _totalFontes - _queue.Count);

                        // Timer para tratar a serie
                        _timer = new Timer();
                        _timer.Tick += new EventHandler(_timer2_Tick);
                        _timer.Interval = 400;
                        _timer.Start();
                    }
                }
            }
        }

        void _timer2_Tick(object sender, EventArgs e)
        {
            if (!_webBrowser.IsBusy && _webBrowser.Document != null && _webBrowser.Document.Window.Frames["iCorpo"] != null)
            {
                // Verifica se tem serie publicada
                if (_webBrowser.Document.Window.Frames["iCorpo"].Document.Body.InnerHtml.IndexOf("Nenhuma") > 0 || _webBrowser.Document.Window.Frames["iCorpo"].Document.Body.InnerHtml.IndexOf("No public") > 0)
                {
                    _timer.Stop();
                    proximaSerie();
                }
                else
                {
                    HtmlElement tabela = _webBrowser.Document.Window.Frames["iCorpo"].Document.GetElementById("tabelaSeries");
                    if (tabela != null)
                    {
                        // Para o timer
                        _timer.Stop();

                        // Varre os elementos
                        foreach (HtmlElement element in tabela.GetElementsByTagName("TBODY")[0].GetElementsByTagName("TR"))
                        {
                            HtmlElementCollection tds = element.GetElementsByTagName("TD");
                            _ds.TbSerie.AddTbSerieRow(
                                tds[1].InnerText,
                                tds[2].InnerText,
                                (string)null,
                                tds[7].InnerText,
                                tds[3].InnerText,
                                tds[4].InnerText,
                                DateTime.Parse(tds[5].InnerText, System.Globalization.CultureInfo.GetCultureInfo("pt-br")),
                                tds[6].InnerText,
                                false);
                        }

                        // Verifica se tem proxima pagina
                        string html1 = _webBrowser.Document.Window.Frames["iCorpo"].Document.Body.InnerHtml;
                        int pos1 = html1.IndexOf(");\">Próximo");
                        if (pos1 == -1)
                            pos1 = html1.IndexOf(");\">Next");
                        if (pos1 > 0)
                        {
                            // Navega para a proxima pagina
                            string html2 = html1.Substring(0, pos1);
                            int pos2 = html2.LastIndexOf("javascript");
                            string html3 = html2.Substring(pos2);
                            html3 = html3.Replace("javascript:getPagina(", "");
                            _webBrowser.Document.Window.Frames["iCorpo"].Document.InvokeScript("getPagina", new object[] { int.Parse(html3) });

                            // Informa
                            string etapa = "Recebendo fonte {0}/{1} Página {2}";
                            _progress.NotificaEtapa(string.Format(etapa, _totalFontes - _queue.Count, _totalFontes, html3), _totalFontes - _queue.Count);

                            // Liga o timer
                            _timer.Start();
                        }
                        else
                        {
                            proximaSerie();
                        }
                    }
                }
            }
        }

        private void proximaSerie()
        {
            // Verifica se tem mais fontes
            if (_queue.Count > 0)
            {

                // Pega a proxima fonte
                string primeiraSerie = _queue.Dequeue();
                _webBrowser.Document.GetElementById("fonte").SetAttribute("value", primeiraSerie);
                _webBrowser.Document.InvokeScript("localizarSeriesPorFonte", new object[] { "../localizarseries/localizarSeries.do?method=localizarSeriesPorFonte", 5 });

                // Informa
                string etapa = "Recebendo fonte {0}/{1} Página 1";
                _progress.NotificaEtapa(string.Format(etapa, _totalFontes - _queue.Count, _totalFontes), _totalFontes - _queue.Count);

                // Liga o timer
                _timer.Start();

            }
            else
            {
                // Acabou
                _timer.Stop();
                _timer.Tick -= new EventHandler(_timer2_Tick);
                _timer.Dispose();
                _timer = null;

                // Informa
                _progress.NotificaFim();
            }

        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= new EventHandler(_timer2_Tick);
                _timer.Dispose();
            }
            _webBrowser.Stop();
            _webBrowser.Dispose();
            _webBrowser = null;
        }
    }
}
