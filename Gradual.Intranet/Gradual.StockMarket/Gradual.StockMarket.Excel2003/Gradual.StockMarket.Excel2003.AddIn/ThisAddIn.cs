using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Core;
using System.Collections.Generic;

namespace Gradual.StockMarket.Excel2003.AddIn
{
    public partial class ThisAddIn
    {
        #region Globais

        private const string gLabelRealizarLogin = "Realizar Login...";
        private const string gLabelAguarde = "Aguarde...";
        private const string gLabelRealizarLogout = "Realizar Logout";

        private int gCodigoCblcUsuario = -1;

        private List<string> gListaDeCarteiras = null;
        private List<string> gListaDeAtivos = null;

        Office.CommandBar gToolBarGradual;
        Office.CommandBarButton gToolBarGradual_btnLogin;

        Office.CommandBarButton gToolBarGradual_btnMontarCotacao;
        Office.CommandBarButton gToolBarGradual_btnMontarTickerCotacao;
        Office.CommandBarButton gToolBarGradual_btnMontarLivroDeOfertas;
        Office.CommandBarButton gToolBarGradual_btnImportarCarteira;

        Office.CommandBarButton gToolBarGradual_btnAplicarEstilo;

        Office.CommandBarPopup gToolBarGradual_mnuOpcoes;
        Office.CommandBarPopup gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao;
        Office.CommandBarButton gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Alta;
        Office.CommandBarButton gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Media;
        Office.CommandBarButton gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Baixa;
        Office.CommandBarButton gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa;

        Timer gTimerDeRecalculo;

        #endregion

        #region Métodos Private

        private void IniciarToolBar()
        {
            try
            {
                gToolBarGradual = Application.CommandBars["Gradual Stock Market"];
            }
            catch (ArgumentException)
            {
            }

            if (gToolBarGradual == null)
            {
                gToolBarGradual = Application.CommandBars.Add("Gradual Stock Market", 1, missing, true);
            }

            try
            {
                // Botão "Realizar Login" ----------------------------------------------------------

                gToolBarGradual_btnLogin = (Office.CommandBarButton)gToolBarGradual.Controls.Add(1, missing, missing, missing, missing);

                gToolBarGradual_btnLogin.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                gToolBarGradual_btnLogin.Caption = gLabelRealizarLogin;
                gToolBarGradual_btnLogin.Picture = CarregadorDeImagens.Carregar(Properties.Resources.IconeLogin);
                gToolBarGradual_btnLogin.Tag = "btnLogin";

                gToolBarGradual_btnLogin.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_btnLogin_Click);



                // Botão "Cotação"

                gToolBarGradual_btnMontarCotacao = (Office.CommandBarButton)gToolBarGradual.Controls.Add(1, missing, missing, missing, missing);

                gToolBarGradual_btnMontarCotacao.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                gToolBarGradual_btnMontarCotacao.Caption = "Cotação";
                gToolBarGradual_btnMontarCotacao.Picture = CarregadorDeImagens.Carregar(Properties.Resources.IconeCotacao);
                gToolBarGradual_btnMontarCotacao.Tag = "btnCotacao";
                gToolBarGradual_btnMontarCotacao.Enabled = false;
                gToolBarGradual_btnMontarCotacao.Visible = false;        // visible false porque o ícone fica feio
                gToolBarGradual_btnMontarCotacao.BeginGroup = true;

                gToolBarGradual_btnMontarCotacao.Click += new _CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_btnCotacao_Click);



                // Botão "Ticker Cotação"

                gToolBarGradual_btnMontarTickerCotacao = (Office.CommandBarButton)gToolBarGradual.Controls.Add(1, missing, missing, missing, missing);

                gToolBarGradual_btnMontarTickerCotacao.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                gToolBarGradual_btnMontarTickerCotacao.Caption = "Ticker Cotação";
                gToolBarGradual_btnMontarTickerCotacao.Picture = CarregadorDeImagens.Carregar(Properties.Resources.IconeTickerCotacao);
                gToolBarGradual_btnMontarTickerCotacao.Tag = "btnMontarTickerCotacao";
                gToolBarGradual_btnMontarTickerCotacao.Enabled = false;
                gToolBarGradual_btnMontarTickerCotacao.Visible = false;        // visible false porque o ícone fica feio

                gToolBarGradual_btnMontarTickerCotacao.Click += new _CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_btnMontarTickerCotacao_Click);



                // Botão "Livro de Ofertas" ----------------------------------------------------------

                gToolBarGradual_btnMontarLivroDeOfertas = (Office.CommandBarButton)gToolBarGradual.Controls.Add(1, missing, missing, missing, missing);

                gToolBarGradual_btnMontarLivroDeOfertas.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                gToolBarGradual_btnMontarLivroDeOfertas.Caption = "Livro de Ofertas";
                gToolBarGradual_btnMontarLivroDeOfertas.Picture = CarregadorDeImagens.Carregar(Properties.Resources.IconeLivroDeOfertas);
                gToolBarGradual_btnMontarLivroDeOfertas.Tag = "btnMontarLivroDeOfertas";
                gToolBarGradual_btnMontarLivroDeOfertas.Enabled = false;
                gToolBarGradual_btnMontarLivroDeOfertas.Visible = false;        // visible false porque o ícone fica feio

                gToolBarGradual_btnMontarLivroDeOfertas.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_btnMontarLivroDeOfertas_Click);



                // Botão "Importar Carteira" ----------------------------------------------------------

                gToolBarGradual_btnImportarCarteira = (Office.CommandBarButton)gToolBarGradual.Controls.Add(1, missing, missing, missing, missing);

                gToolBarGradual_btnImportarCarteira.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                gToolBarGradual_btnImportarCarteira.Caption = "Importar Carteira";
                gToolBarGradual_btnImportarCarteira.Picture = CarregadorDeImagens.Carregar(Properties.Resources.IconeCarteiras);
                gToolBarGradual_btnImportarCarteira.Tag = "btnImportarCarteira";
                gToolBarGradual_btnImportarCarteira.Enabled = false;
                gToolBarGradual_btnImportarCarteira.Visible = false;        // visible false porque o ícone fica feio

                gToolBarGradual_btnImportarCarteira.Click += new _CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_btnImportarCarteira_Click);



                // Botão "Aplicar Estilo" ----------------------------------------------------------

                gToolBarGradual_btnAplicarEstilo = (Office.CommandBarButton)gToolBarGradual.Controls.Add(1, missing, missing, missing, missing);

                gToolBarGradual_btnAplicarEstilo.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                gToolBarGradual_btnAplicarEstilo.Caption = "Aplicar Estilo...";
                gToolBarGradual_btnAplicarEstilo.Picture = CarregadorDeImagens.Carregar(Properties.Resources.IconeEstilo);
                gToolBarGradual_btnAplicarEstilo.Tag = "btnAplicarEstilo";
                gToolBarGradual_btnAplicarEstilo.Enabled = false;
                gToolBarGradual_btnAplicarEstilo.Visible = false;        // visible false porque o ícone fica feio
                gToolBarGradual_btnAplicarEstilo.BeginGroup = true;

                gToolBarGradual_btnAplicarEstilo.Click += new _CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_btnAplicarEstilo_Click);





                // Menu "Opções" ----------------------------------------------------------

                gToolBarGradual_mnuOpcoes = (Office.CommandBarPopup)gToolBarGradual.Controls.Add(MsoControlType.msoControlPopup, missing, missing, missing, missing);

                gToolBarGradual_mnuOpcoes.Caption = "Opções";
                gToolBarGradual_mnuOpcoes.Enabled = false;
                gToolBarGradual_mnuOpcoes.BeginGroup = true;

                // Menu "Opções > Frequencia de Atualização "

                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao = (Office.CommandBarPopup)gToolBarGradual_mnuOpcoes.Controls.Add(MsoControlType.msoControlPopup, missing, missing, missing, missing);
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao.Caption = "Frequência de Atualização";

                // Menu "Opções > Frequencia de Atualizacao > Alta / Média / baixa

                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Alta = (Office.CommandBarButton)gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao.Controls.Add(MsoControlType.msoControlButton, missing, missing, missing, missing);
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Alta.Caption = "Alta";
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Alta.TooltipText = "Atualiza todas as cotações Uma vez por segundo";
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Alta.State = MsoButtonState.msoButtonDown ;

                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Media = (Office.CommandBarButton)gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao.Controls.Add(MsoControlType.msoControlButton, missing, missing, missing, missing);
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Media.Caption = "Média";
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Media.TooltipText = "Atualiza todas as cotações Uma vez a cada dez segundos";

                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Baixa = (Office.CommandBarButton)gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao.Controls.Add(MsoControlType.msoControlButton, missing, missing, missing, missing);
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Caption = "Baixa";
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Baixa.TooltipText = "Atualiza todas as cotações Uma vez a cada minuto";

                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa = (Office.CommandBarButton)gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao.Controls.Add(MsoControlType.msoControlButton, missing, missing, missing, missing);
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Caption = "Muito Baixa";
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.TooltipText = "Atualiza todas as cotações Uma vez a cada dez minutos";

                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Alta.Click += new _CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Click);
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Media.Click += new _CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Click);
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Click += new _CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Click);
                gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Click += new _CommandBarButtonEvents_ClickEventHandler(gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Click);

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(string.Format("Erro ao criar a toolbar Gradual StockMarket: {0}", ex.Message));
            }
        }

        private void IniciarTimer()
        {
            gTimerDeRecalculo = new Timer();

            gTimerDeRecalculo.Interval = 1000;

            gTimerDeRecalculo.Tick += new EventHandler(gTimerDeRecalculo_Tick);

            gTimerDeRecalculo.Start();
        }

        private void RealizarLogin()
        {
            frmInputBox_Login lFormLogin = new frmInputBox_Login();

            if (lFormLogin.ShowDialog() == DialogResult.OK)
            {
                string lCodigoEmail, lSenha;

                lCodigoEmail = lFormLogin.CodigoEmail;
                lSenha = lFormLogin.Senha;


                //TODO: REMOVER !!!! SOMENTE PARA DEV !!!     ==============================================================

                if (lCodigoEmail == "" && lSenha == "")
                {
                    lCodigoEmail = "rafaelzinho22@hotmail.com";
                    lSenha = "abc1985";
                }

                // =========================================================================================================



                //ServicoSegurancaSoapClient lWebServiceSeguranca = new ServicoSegurancaSoapClient();

                WsAutenticacao.AutenticacaoSoapClient lServicoAutenticacao = new WsAutenticacao.AutenticacaoSoapClient();

                WsAutenticacao.AutenticarUsuarioRequest lRequest = new WsAutenticacao.AutenticarUsuarioRequest();
                WsAutenticacao.AutenticarUsuarioResponse lResponse;

                gToolBarGradual_btnLogin.Caption = gLabelAguarde;

                lRequest.CodigoOuEmailDoUsuario = lCodigoEmail;
                lRequest.Senha = lSenha;

                lResponse = lServicoAutenticacao.AutenticarUsuario(lRequest);

                if (lResponse.StatusResposta == "OK")
                {
                    gCodigoCblcUsuario = Convert.ToInt32(lResponse.CodigoCblcDoUsuario);

                    Interface_HabilitarBotoesAposLogin();

                    IniciarTimer();

                    gToolBarGradual_btnLogin.Caption = gLabelRealizarLogout;
                    gToolBarGradual_btnLogin.Picture = CarregadorDeImagens.Carregar(Properties.Resources.IconeLogout);
                }
                else
                {
                    MessageBox.Show(string.Format("Erro de login: {0}\r\n\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }

        }

        private void RealizarLogout()
        {
            //TODO: Logout  

            gToolBarGradual_btnLogin.Caption = gLabelRealizarLogin;
            gToolBarGradual_btnLogin.Picture = CarregadorDeImagens.Carregar(Properties.Resources.IconeLogin);

            Interface_DesabilitarBotoesAposLogout();
        }

        private void BuscarListaDeCarteirasEAtivos()
        {
            if (gCodigoCblcUsuario > 0 && gListaDeAtivos == null)
            {
                WsPlataforma.PlataformaSoapClient lServicoPlataforma = new WsPlataforma.PlataformaSoapClient();

                WsPlataforma.BuscarCarteirasComAtivosRequest lRequest = new WsPlataforma.BuscarCarteirasComAtivosRequest();
                WsPlataforma.BuscarCarteirasComAtivosResponse lResponse;

                lRequest.CodigoDoUsuario = gCodigoCblcUsuario.ToString();

                lResponse = lServicoPlataforma.BuscarCarteirasComAtivos(lRequest);

                if (lResponse.StatusResposta == "OK")
                {
                    gListaDeCarteiras = new List<string>(lResponse.Carteiras);
                    gListaDeAtivos = new List<string>(lResponse.Ativos);
                }
                else
                {
                    MessageBox.Show(string.Format("Erro ao buscar lista de carteiras: {0}\r\n\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
        }

        private bool CelulaAtualContemFormulaSM(out string pFormula)
        {
            pFormula = null;

            if (Application.ActiveSheet != null && Application.ActiveCell != null)
            {
                string lFormula = Convert.ToString(Application.ActiveCell.FormulaArray) + "";

                if (lFormula.StartsWith("=SM_COTACAO")
                 || lFormula.StartsWith("=SM_TICKER")
                 || lFormula.StartsWith("=SM_LIVROOFERTAS"))
                {
                    pFormula = lFormula.Substring(1, lFormula.IndexOf('(') - 1);

                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Métodos Private - Interface

        private void Interface_HabilitarBotoesAposLogin()
        {
            gToolBarGradual_btnMontarLivroDeOfertas.Enabled = true;
            gToolBarGradual_btnMontarLivroDeOfertas.Visible = true;

            gToolBarGradual_btnMontarCotacao.Enabled = true;
            gToolBarGradual_btnMontarCotacao.Visible = true;

            gToolBarGradual_btnMontarTickerCotacao.Enabled = true;
            gToolBarGradual_btnMontarTickerCotacao.Visible = true;

            gToolBarGradual_btnImportarCarteira.Enabled = true;
            gToolBarGradual_btnImportarCarteira.Visible = true;

            gToolBarGradual_btnAplicarEstilo.Enabled = true;
            gToolBarGradual_btnAplicarEstilo.Visible = true;

            gToolBarGradual_mnuOpcoes.Enabled = true;
        }

        private void Interface_DesabilitarBotoesAposLogout()
        {
            gToolBarGradual_btnMontarLivroDeOfertas.Enabled = false;
            gToolBarGradual_btnMontarLivroDeOfertas.Visible = false;

            gToolBarGradual_btnMontarCotacao.Enabled = false;
            gToolBarGradual_btnMontarCotacao.Visible = false;

            gToolBarGradual_btnMontarTickerCotacao.Enabled = false;
            gToolBarGradual_btnMontarTickerCotacao.Visible = false;

            gToolBarGradual_btnImportarCarteira.Enabled = false;
            gToolBarGradual_btnImportarCarteira.Visible = false;

            gToolBarGradual_btnAplicarEstilo.Enabled = false;
            gToolBarGradual_btnAplicarEstilo.Visible = false;

            gToolBarGradual_mnuOpcoes.Enabled = false;
        }

        private void Interface_MontarMonitorDeCotacoes(string pInstrumento)
        {
            int lRow, lCol;

            lRow = Application.ActiveCell.Row;
            lCol = Application.ActiveCell.Column;

            if (Application.ActiveSheet != null)
            {
                // monta o cabecalho se a célula de cima estiver vazia:

                object v2, f, fa;

                if (lRow > 1)
                {
                    v2 = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Value2;
                    f  = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Formula;
                    fa = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).FormulaArray;

                    if (v2 == null && f.ToString() == "" && fa.ToString() == "")
                    {
                        lRow--;

                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Value2 = "Papel";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 1]).Value2 = "Última";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 2]).Value2 = "Var (%)";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 3]).Value2 = "Cor. Comp.";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 4]).Value2 = "Vl. Comp";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 5]).Value2 = "Cor. Venda";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 6]).Value2 = "Vl. Venda";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 7]).Value2 = "Abertura";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 8]).Value2 = "Mínima";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 9]).Value2 = "Máxima";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 10]).Value2 = "Fech. Ant.";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 11]).Value2 = "N. Neg";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 12]).Value2 = "Volume";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).Value2 = "Data / Hora";

                        // seleciona o range do "corpo" da tabela e coloca a fórmula:

                        lRow++;
                    }
                }

                // coloca a última célula com formatação de data:
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).NumberFormat = "HH:mm:ss";

                Excel.Range lRangeMonitor;

                lRangeMonitor = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 0],
                                                                                     ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 13]);

                lRangeMonitor.FormulaArray = "=SM_COTACAO(\"" + pInstrumento + "\")";

                lRangeMonitor.Calculate();
            }
        }

        private void Interface_MontarTickerDeCotacao(string pInstrumento)
        {
            int lRow, lCol;

            lRow = Application.ActiveCell.Row;
            lCol = Application.ActiveCell.Column;

            if (Application.ActiveSheet != null)
            {
                // monta o cabecalho se a célula de cima estiver vazia:

                object v2, f, fa;

                if (lRow > 1)
                {
                    v2 = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Value2;
                    f =  ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Formula;
                    fa = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).FormulaArray;

                    if (v2 == null && f.ToString() == "" && fa.ToString() == "")
                    {
                        // Título "PAPEL"

                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Value2 = pInstrumento;

                        Excel.Range lRangeTitulo;

                        lRangeTitulo = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0],
                                                                                            ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 3]);

                        lRangeTitulo.Merge(false);
                        lRangeTitulo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    }
                }

                // seleciona o range do "corpo" da tabela e coloca a fórmula:

                Excel.Range lRangeTicker;

                lRangeTicker = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 0],
                                                                                    ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 5, lCol + 3]);

                lRangeTicker.FormulaArray = "=SM_TICKER(\"" + pInstrumento + "\")";

                //lRangeTicker.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(204, 67, 193));

                //lRangeTicker.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(204, 67, 193));

                lRangeTicker.Calculate();
            }
        }

        private void Interface_MontarLivroDeOfertas(string pInstrumento)
        {
            int lRow, lCol;

            lRow = Application.ActiveCell.Row;
            lCol = Application.ActiveCell.Column;

            if(Application.ActiveSheet != null)
            {
                // monta o cabecalho se as duas células acima estiverem vazias:

                object v2, f, fa;

                if (lRow > 2)
                {
                    v2 = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Value2;
                    f =  ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Formula;
                    fa = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).FormulaArray;

                    if (v2 == null && f.ToString() == "" && fa.ToString() == "")
                    {
                        v2 = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 2, lCol + 0]).Value2;
                        f =  ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 2, lCol + 0]).Formula;
                        fa = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 2, lCol + 0]).FormulaArray;

                        if (v2 == null && f.ToString() == "" && fa.ToString() == "")
                        {
                            // Título "Livro de Ofertas - PAPEL"

                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 2, lCol + 0]).Value2 = "Livro de Ofertas - " + pInstrumento;

                            Excel.Range lRangeTitulo;

                            lRangeTitulo = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 2, lCol + 0],
                                                                                                ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 2, lCol + 5]);

                            lRangeTitulo.Merge(false);
                            lRangeTitulo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                            // Cabeçalho da tabela:

                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Value2 = "Corr.";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 1]).Value2 = "Qtd.";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 2]).Value2 = "OFC";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 3]).Value2 = "OFV";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 4]).Value2 = "Qtd";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 5]).Value2 = "Corr.";
                        }
                    }
                }

                // seleciona o range do "corpo" da tabela e coloca a fórmula:

                Excel.Range lRangeLivro;

                lRangeLivro = ((Excel.Worksheet)Application.ActiveSheet).get_Range( ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 0],
                                                                                    ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 9, lCol + 5]);

                lRangeLivro.FormulaArray = "=SM_LIVROOFERTAS(\"" + pInstrumento + "\")";

                lRangeLivro.Calculate();
            }
        }

        private void Interface_MontarCarteira(string[] pAtivos)
        {
            int lRow, lCol;

            lRow = Application.ActiveCell.Row;
            lCol = Application.ActiveCell.Column;

            if (Application.ActiveSheet != null)
            {
                // monta os cabecalho da tabela:

                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Value2 = "Papel";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 1]).Value2 = "Última";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 2]).Value2 = "Var (%)";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 3]).Value2 = "Cor. Comp.";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 4]).Value2 = "Vl. Comp";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 5]).Value2 = "Cor. Venda";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 6]).Value2 = "Vl. Venda";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 7]).Value2 = "Abertura";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 8]).Value2 = "Mínima";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 9]).Value2 = "Máxima";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 10]).Value2 = "Fech. Ant.";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 11]).Value2 = "N. Neg";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 12]).Value2 = "Volume";
                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).Value2 = "Data / Hora";

                // seleciona o range do "corpo" da tabela e coloca a fórmula:

                foreach (string lAtivo in pAtivos)
                {
                    lRow++;

                    // coloca a última célula com formatação de data:
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).NumberFormat = "HH:mm:ss";

                    Excel.Range lRangeMonitor;

                    lRangeMonitor = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 0],
                                                                                         ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 13]);

                    lRangeMonitor.FormulaArray = "=SM_COTACAO(\"" + lAtivo + "\")";

                    lRangeMonitor.Calculate();
                }
            }
        }

        private void Interface_ImportarCarteira()
        {
            BuscarListaDeCarteirasEAtivos();

            frmInputBox_SelecionarCarteira lForm = new frmInputBox_SelecionarCarteira();

            lForm.ListaDeCarteiras = gListaDeCarteiras;

            if (lForm.ShowDialog() == DialogResult.OK)
            {
                int lIndice = lForm.IndiceDaCarteiraSelecionada;

                if (lIndice >= 0 && gListaDeAtivos.Count > lIndice)
                {
                    string[] lAtivos = gListaDeAtivos[lIndice].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    Interface_MontarCarteira(lAtivos);
                }
            }
        }

        private void Interface_AplicarEstilo()
        {
            // Verifica se a célula atual tem alguma das nossas fórmulas:

            string lFormulaAtual;

            if (CelulaAtualContemFormulaSM(out lFormulaAtual))
            {
                // acha a primeira célula (topo à esquerda) que tem a mesma fórmula
                int lRow, lCol;

                lRow = Application.ActiveCell.Row;
                lCol = Application.ActiveCell.Column;

                //vai pra esquerda:

                while (lRow > 1 && Convert.ToString(((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol]).FormulaArray).Contains(lFormulaAtual))
                {
                    lRow--;
                }

                //vai pra cima:

                while (lCol > 1 && Convert.ToString(((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol - 1]).FormulaArray).Contains(lFormulaAtual))
                {
                    lCol--;
                }

                frmInputBox_SelecionarEstilo lForm = new frmInputBox_SelecionarEstilo();

                if (lFormulaAtual == "SM_COTACAO")
                {
                    lForm.TipoDeEstiloParaSelecionar = "monitor";

                    if (lForm.ShowDialog() == DialogResult.OK)
                    {
                        Interface_AplicarEstilo_MonitorCotacao(lForm.EstiloSelecionado, lRow, lCol, lFormulaAtual);
                    }
                }
                else if (lFormulaAtual == "SM_TICKER")
                {
                    lForm.TipoDeEstiloParaSelecionar = "ticker";

                    if (lForm.ShowDialog() == DialogResult.OK)
                    {
                        Interface_AplicarEstilo_TickerCotacao(lForm.EstiloSelecionado, lRow, lCol, lFormulaAtual);
                    }
                }
                else if (lFormulaAtual == "SM_LIVROOFERTAS")
                {
                    lForm.TipoDeEstiloParaSelecionar = "livro";

                    if (lForm.ShowDialog() == DialogResult.OK)
                    {
                        Interface_AplicarEstilo_LivroOfertas(lForm.EstiloSelecionado, lRow, lCol, lFormulaAtual);
                    }
                }
            }
            else
            {
                MessageBox.Show("A célula atual não parece estar dentro de um range que contenha alguma fórmula Stock Market.\r\n\r\nPara aplicar um estilo, primeiro selecione uma célula que esteja dentro do corpo de um monitor de cotações / ticker ou livro de ofertas,\r\ndepois clique em 'Aplicar Estilo...'");
            }
        }

        private void Interface_AplicarEstilo_MonitorCotacao(byte pEstilo, int pRow, int pCol, string pFormula)
        {
            //MessageBox.Show(string.Format("Aplicando formatação de cotação a partir de {0}, {1}", pRow, pCol));

            // Cabeçalho:

            Excel.Range lRangeCabecalho, lRangeCorpo, lRangeColunaPapeis;

            lRangeCabecalho = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow - 1, pCol + 0],
                                                                                   ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow - 1, pCol + 13]);

            //Pra pegar o corpo, precisa ir verificando nas linhas abaixo até achar uma que não tenha a fórmula:

            int lQtdLinhas = 0;

            while (((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + lQtdLinhas, pCol + 0]).FormulaArray.ToString().Contains(pFormula))
                lQtdLinhas++;

            lRangeCorpo = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 0, pCol + 0],
                                                                               ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 13]);

            lRangeColunaPapeis = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 0,       pCol + 0],
                                                                               ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 0]);

            if (pEstilo == 1)
            {
                // Cabeçalho:

                lRangeCabecalho.Font.Size = 8;
                lRangeCabecalho.Font.Bold = true;
                lRangeCabecalho.Font.ColorIndex = 16;   // Cinza 50%   ==> http://www.mvps.org/dmcritchie/excel/colors.htm

                lRangeCabecalho.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;

                // linhas:

                lRangeCorpo.Font.ColorIndex = 1;   // Preto

                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;

                lRangeCorpo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // coluna que tem os nomes dos papéis:

                lRangeColunaPapeis.Font.Bold = true;

                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
            }
            else if (pEstilo == 2)
            {
                // Cabeçalho:

                lRangeCabecalho.Font.Size = 8;
                lRangeCabecalho.Font.Bold = true;
                lRangeCabecalho.Font.ColorIndex = 1;

                lRangeCabecalho.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;

                // linhas:

                lRangeCorpo.Font.ColorIndex = 1;   // Preto

                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;

                lRangeCorpo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // coluna que tem os nomes dos papéis:

                lRangeColunaPapeis.Font.Bold = true;

                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;

                // coluna de preço de compra:

                lRangeColunaPapeis = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 0, pCol + 4],
                                                                                          ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 4]);

                lRangeColunaPapeis.Font.Bold = true;

                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;
                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;

                // coluna de preço de venda:

                lRangeColunaPapeis = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 0, pCol + 6],
                                                                                          ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 6]);

                lRangeColunaPapeis.Font.Bold = true;

                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;
                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
            }
            else if (pEstilo == 3)
            {
                // Cabeçalho:

                lRangeCabecalho.Font.Size = 8;
                lRangeCabecalho.Font.Bold = false;
                lRangeCabecalho.Font.ColorIndex = 2;            // Branco
                lRangeCabecalho.Interior.ColorIndex = 16;       // Cinza 50%

                lRangeCabecalho.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;

                // linhas:

                lRangeCorpo.Font.ColorIndex = 1;                // Preto

                lRangeCorpo.Interior.ColorIndex = 15;           // Cinza 25%

                lRangeCorpo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // coluna que tem os nomes dos papéis:

                lRangeColunaPapeis.Font.Bold = true;
            }
            else if (pEstilo == 4)
            {
                // Cabeçalho:

                lRangeCabecalho.Font.Size = 8;
                lRangeCabecalho.Font.Bold = false;
                lRangeCabecalho.Font.ColorIndex = 2;            // Branco
                lRangeCabecalho.Interior.ColorIndex = 10;       // Verde

                lRangeCabecalho.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;

                // linhas:

                lRangeCorpo.Font.ColorIndex = 1;                // Preto

                lRangeCorpo.Interior.ColorIndex = 35;           // Verde Claro

                lRangeCorpo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;

                // coluna que tem os nomes dos papéis:

                lRangeColunaPapeis.Font.Bold = true;
            }
            else if (pEstilo == 5)
            {
                // Cabeçalho:

                lRangeCabecalho.Font.Size = 8;
                lRangeCabecalho.Font.Bold = false;
                lRangeCabecalho.Font.ColorIndex = 2;            // Branco
                lRangeCabecalho.Interior.ColorIndex = 9;        // Vermelho escuro

                lRangeCabecalho.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;

                // linhas:

                lRangeCorpo.Font.ColorIndex = 1;                // Preto

                lRangeCorpo.Interior.ColorIndex = 36;           // Amarelo Claro

                lRangeCorpo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;

                // coluna que tem os nomes dos papéis:

                lRangeColunaPapeis.Font.Bold = true;
            }
            else if (pEstilo == 6)
            {
                // Cabeçalho:

                lRangeCabecalho.Font.Size = 8;
                lRangeCabecalho.Font.Bold = true;
                lRangeCabecalho.Font.ColorIndex = 11;            // Azul Escuro
                lRangeCabecalho.Interior.ColorIndex = 33;        // Azul - céu

                lRangeCabecalho.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // linhas:

                lRangeCorpo.Font.ColorIndex = 47;               // Cinza-azulado

                lRangeCorpo.Interior.ColorIndex = 34;           // Turquesa claro

                lRangeCorpo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // coluna que tem os nomes dos papéis:

                lRangeColunaPapeis.Font.ColorIndex = 11;

                // coluna de preço de compra:

                lRangeColunaPapeis = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 0, pCol + 4],
                                                                                          ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 4]);

                lRangeColunaPapeis.Font.ColorIndex = 11;

                // coluna de preço de venda:

                lRangeColunaPapeis = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 0, pCol + 6],
                                                                                          ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 6]);

                lRangeColunaPapeis.Font.ColorIndex = 11;
            }
        }

        private void Interface_AplicarEstilo_TickerCotacao(byte pEstilo, int pRow, int pCol, string pFormula)
        {
            Excel.Range lRangeCabecalho, lRangeLabels1, lRangeLabels2, lRangeValores1, lRangeValores2;

            lRangeCabecalho = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow - 1, pCol + 0],
                                                                                   ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow - 1, pCol + 3]);

            lRangeLabels1 = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow,      pCol],
                                                                                 ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 5,  pCol]);

            lRangeValores1 = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow,     pCol + 1],
                                                                                  ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 5, pCol + 1]);

            lRangeLabels2 = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow,      pCol + 2],
                                                                                 ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 5,  pCol + 2]);

            lRangeValores2 = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow,     pCol + 3],
                                                                                  ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 5, pCol + 3]);

            // características comuns a todos os templates de ticker:

            //alinhamentos:

            lRangeCabecalho.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            lRangeLabels1.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            lRangeLabels2.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            lRangeValores1.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            lRangeValores2.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

            //tamanho da fonte pros labels:

            lRangeLabels1.Font.Size = 8;
            lRangeLabels2.Font.Size = 8;

            //bold pro header:

            lRangeCabecalho.Font.Bold = true;

            //bold para ultima e variacao:

            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[pRow, pCol + 1]).Font.Bold = true;
            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[pRow, pCol + 3]).Font.Bold = true;

            //borda ao redor do "quadrado" geral:

            lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
            lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
            lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
            lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;

            lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
            lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
            lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
            lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;

            lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
            lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
            lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
            lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;

            lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
            lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
            lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
            lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;

            // cor da fonte preto pros valores:

            lRangeValores1.Font.ColorIndex = 1;
            lRangeValores2.Font.ColorIndex = 1;

            if (pEstilo == 1)
            {
                // cor da fonte dos labels:
                lRangeLabels1.Font.ColorIndex = 16;  //gray 50%
                lRangeLabels2.Font.ColorIndex = 16;  //gray 50%

                //bold pros labels:

                lRangeLabels1.Font.Bold = true;
                lRangeLabels2.Font.Bold = true;
            }
            else if (pEstilo == 2)
            {
                // cor da fonte dos labels:
                lRangeLabels1.Font.ColorIndex = 56;  //gray 80%
                lRangeLabels2.Font.ColorIndex = 56;  //gray 80%

                //bold pros labels:

                lRangeLabels1.Font.Bold = true;
                lRangeLabels2.Font.Bold = true;

                //linha grossa do header:
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;

                //linhas internas pra todos:

                lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;
                lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;

                lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;
                lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;
            }
            else if (pEstilo == 3)
            {
                // cabeçalho branco de fundo cinza:

                lRangeCabecalho.Font.ColorIndex = 2;
                lRangeCabecalho.Interior.ColorIndex = 16;

                // cor da fonte dos labels:
                lRangeLabels1.Font.ColorIndex = 2;
                lRangeLabels2.Font.ColorIndex = 2;

                //fundo cinza dos labels:
                lRangeLabels1.Interior.ColorIndex = 16;
                lRangeLabels2.Interior.ColorIndex = 16;

                //fundo dos valores:
                lRangeValores1.Interior.ColorIndex = 15;    //cinza 25%
                lRangeValores2.Interior.ColorIndex = 15;    //cinza 25%

                //linhas internas dos valores:
                lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;
                lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;

            }
            else if (pEstilo == 4)
            {
                // cabeçalho branco de fundo verde:

                lRangeCabecalho.Font.ColorIndex = 2;
                lRangeCabecalho.Interior.ColorIndex = 10;

                // cor da fonte dos labels:
                lRangeLabels1.Font.ColorIndex = 2;
                lRangeLabels2.Font.ColorIndex = 2;

                //fundo dos labels:
                lRangeLabels1.Interior.ColorIndex = 50;
                lRangeLabels2.Interior.ColorIndex = 50;

                //fundo dos valores:
                lRangeValores1.Interior.ColorIndex = 35;    //verde claro
                lRangeValores2.Interior.ColorIndex = 35;    //verde claro

                //linhas internas dos valores:
                lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;
                lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;

            }
            else if (pEstilo == 5)
            {
                // cabeçalho branco de fundo vermelho:

                lRangeCabecalho.Font.ColorIndex = 2;
                lRangeCabecalho.Interior.ColorIndex = 9;

                // cor da fonte dos labels:
                lRangeLabels1.Font.ColorIndex = 2;
                lRangeLabels2.Font.ColorIndex = 2;

                //fundo dos labels:
                lRangeLabels1.Interior.ColorIndex = 53;
                lRangeLabels2.Interior.ColorIndex = 53;

                //fundo dos valores:
                lRangeValores1.Interior.ColorIndex = 36;    //amarelo claro
                lRangeValores2.Interior.ColorIndex = 36;    //amarelo claro

                //linhas internas dos valores:
                lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;
                lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.Constants.xlSolid;
            }
            else if (pEstilo == 6)
            {
                // cabeçalho azul:

                lRangeCabecalho.Font.ColorIndex = 11;
                lRangeCabecalho.Interior.ColorIndex = 33;

                // borda do cabeçalho:

                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;

                // cor da fonte dos labels:
                lRangeLabels1.Font.ColorIndex = 11;
                lRangeLabels2.Font.ColorIndex = 11;

                // cor da fonte dos valores:
                lRangeValores1.Font.ColorIndex = 11;
                lRangeValores2.Font.ColorIndex = 11;

                //fundo dos labels:
                lRangeLabels1.Interior.ColorIndex = 33;
                lRangeLabels2.Interior.ColorIndex = 33;

                //fundo dos valores:
                lRangeValores1.Interior.ColorIndex = 34;
                lRangeValores2.Interior.ColorIndex = 34;

                //tira linhas verticais internas:
                lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlNone;
                lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlNone;
                lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlNone;
            }
        }

        private void Interface_AplicarEstilo_LivroOfertas(byte pEstilo, int pRow, int pCol, string pFormula)
        {
            Excel.Range lRangeCabecalho, lRangeCabecalho2, lRangeValores;

            lRangeCabecalho = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow - 2, pCol + 0],
                                                                                   ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow - 2, pCol + 5]);

            lRangeCabecalho2 = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow - 1, pCol + 0],
                                                                                    ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow - 1, pCol + 5]);

            lRangeValores = ((Excel.Worksheet)Application.ActiveSheet).get_Range(((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 0, pCol + 0],
                                                                                 ((Excel.Worksheet)Application.ActiveSheet).Cells[pRow + 9, pCol + 5]);



            // características comuns a todos os templates de ticker:

            //alinhamentos:

            lRangeCabecalho.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            lRangeCabecalho2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            // cor da fonte dos cabecalho:
            lRangeCabecalho.Font.ColorIndex = 2;  //branco
            lRangeCabecalho2.Font.ColorIndex = 2;  //branco

            //tamanho da fonte pros labels:

            lRangeCabecalho2.Font.Size = 8;

            //bold pro header:

            lRangeCabecalho.Font.Bold = true;

            //borda ao redor do Cabecalho2

            lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
            lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
            lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
            lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;
            lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.Constants.xlSolid;

            //borda ao redor dos valores:
            lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
            lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
            lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
            lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;

            // cor da fonte preto pros valores:

            lRangeValores.Font.ColorIndex = 1;

            if (pEstilo == 1)
            {
                // cor da fonte dos cabecalho:
                lRangeCabecalho.Font.ColorIndex = 16;  //gray 50%

                // cor da fonte dos labels:
                lRangeCabecalho2.Font.ColorIndex = 16;  //gray 50%

            }
            else if (pEstilo == 2)
            {
                // cor da fonte dos cabecalho:
                lRangeCabecalho.Font.ColorIndex = 56;  //gray 80%

                // cor da fonte dos labels:
                lRangeCabecalho2.Font.ColorIndex = 56;  //gray 80%

                // borda grossa dos cabecalho2:

                lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
            }
            else if (pEstilo == 3)
            {
                //borda ao redor dos valores:
                lRangeValores.Borders.LineStyle = Excel.Constants.xlNone;

                //borda do cabecalho2:
                lRangeCabecalho2.Borders.LineStyle = Excel.Constants.xlNone;

                //borda do cabecalho1:
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;

                // cor de fundo do cabecalho1:
                lRangeCabecalho.Interior.ColorIndex = 16; //cinza 50%

                // cor de fundo dos cabecalho2:
                lRangeCabecalho2.Interior.ColorIndex = 48; //cinza 40%

                // cor de fundo dos valores:
                lRangeValores.Interior.ColorIndex = 15; //cinza 25%
            }
            else if (pEstilo == 4)
            {
                //borda no cabecalho1:
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;

                //borda vertical dos valores:
                lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.Constants.xlSolid;

                // cor de fundo do cabecalho1:
                lRangeCabecalho.Interior.ColorIndex = 10; //verde

                // cor de fundo dos cabecalho2:
                lRangeCabecalho2.Interior.ColorIndex = 50;

                // cor de fundo dos valores:
                lRangeValores.Interior.ColorIndex = 35;
            }
            else if (pEstilo == 5)
            {
                //borda no cabecalho1:
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;

                //borda vertical dos valores:
                lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.Constants.xlSolid;

                // cor de fundo do cabecalho1:
                lRangeCabecalho.Interior.ColorIndex = 9;

                // cor de fundo dos cabecalho2:
                lRangeCabecalho2.Interior.ColorIndex = 53;

                // cor de fundo dos valores:
                lRangeValores.Interior.ColorIndex = 36;
            }
            else if (pEstilo == 6)
            {
                //borda no cabecalho1:
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.Constants.xlSolid;

                //borda vertical dos valores:
                lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.Constants.xlSolid;

                // cor de fundo do cabecalho1:
                lRangeCabecalho.Interior.ColorIndex = 5;

                // cor de fundo dos cabecalho2:
                lRangeCabecalho2.Interior.ColorIndex = 41;

                // cor de fundo dos valores:
                lRangeValores.Interior.ColorIndex = 34;
            }
        }

        #endregion

        #region Event Handlers

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            #region VSTO generated code

            this.Application = (Excel.Application)Microsoft.Office.Tools.Excel.ExcelLocale1033Proxy.Wrap(typeof(Excel.Application), this.Application);

            #endregion

            IniciarToolBar();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

        }

        private void gTimerDeRecalculo_Tick(object sender, EventArgs e)
        {
            Excel.Range lRange = null;

            try
            {
                lRange = ((Excel.Worksheet)Application.ActiveSheet).Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeFormulas, missing);

                if (lRange != null)
                {
                    lRange.Dirty();

                    lRange.Calculate();
                }
            }
            catch { }
        }


        private void gToolBarGradual_btnLogin_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            if (gToolBarGradual_btnLogin.Caption == gLabelRealizarLogin)
            {
                RealizarLogin();
            }
            else if (gToolBarGradual_btnLogin.Caption == gLabelRealizarLogout)
            {
                RealizarLogout();
            }
        }

        private void gToolBarGradual_btnCotacao_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            string lInstrumento;

            lInstrumento = Convert.ToString(Application.InputBox("Indique o papel desejado:", "Buscar Cotações", missing, missing, missing, missing, missing, 2));

            if (!string.IsNullOrEmpty(lInstrumento) && lInstrumento != "False")
                Interface_MontarMonitorDeCotacoes(lInstrumento.ToUpper());
        }

        private void gToolBarGradual_btnMontarTickerCotacao_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            string lInstrumento;

            lInstrumento = Convert.ToString(Application.InputBox("Indique o papel desejado:", "Buscar Cotações", missing, missing, missing, missing, missing, 2));

            if (!string.IsNullOrEmpty(lInstrumento) && lInstrumento != "False")
                Interface_MontarTickerDeCotacao(lInstrumento.ToUpper());
        }

        private void gToolBarGradual_btnMontarLivroDeOfertas_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            string lInstrumento;

            lInstrumento = Convert.ToString( Application.InputBox("Indique o papel desejado:", "Criar Livro de Ofertas", missing, missing, missing, missing, missing, 2));

            if(!string.IsNullOrEmpty(lInstrumento) && lInstrumento != "False")
                Interface_MontarLivroDeOfertas(lInstrumento.ToUpper());
        }

        private void gToolBarGradual_btnImportarCarteira_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            Interface_ImportarCarteira();
        }

        private void gToolBarGradual_btnAplicarEstilo_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            Interface_AplicarEstilo();
        }


        private void gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Alta.State = MsoButtonState.msoButtonUp;
            gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Media.State = MsoButtonState.msoButtonUp;
            gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Baixa.State = MsoButtonState.msoButtonUp;
            gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.State = MsoButtonState.msoButtonUp;

            Ctrl.State = MsoButtonState.msoButtonDown;

            if (Ctrl.Caption == gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Alta.Caption)
            {
                gTimerDeRecalculo.Interval = 1000;
            }
            else if (Ctrl.Caption == gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Media.Caption)
            {
                gTimerDeRecalculo.Interval = 10000;
            }
            else if (Ctrl.Caption == gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Caption)
            {
                gTimerDeRecalculo.Interval = 60000;
            }
            else if (Ctrl.Caption == gToolBarGradual_mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Caption)
            {
                gTimerDeRecalculo.Interval = 600000;
            }
        }

        #endregion

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
