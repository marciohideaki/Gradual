using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R001 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 50;

        #endregion

        #region | Propriedades

        private string GetCpfCnpj
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["CpfCnpj"]))
                    return null;

                return this.Request.Form["CpfCnpj"].Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            }
        }

        private DateTime GetDataInicial
        {
            get 
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get 
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                var lRetorno = default(int);

                int.TryParse(this.Request.Form["Assessor"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetTipoPessoa
        {
            get
            { 
                string lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request.Form["TipoPessoa"]))
                    lRetorno = this.Request.Form["TipoPessoa"];

                return lRetorno;
            }
        }

        private IEnumerable<TransporteRelatorio_001> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_001>)Session["ListaDeResultados_Relatorio_001"];
            }
            
            set
            {
                Session["ListaDeResultados_Relatorio_001"] = value;
            }
        }

        #endregion

        #region | Métodos

        private List<TransporteRelatorio_001> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_001> lRetorno = new List<TransporteRelatorio_001>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = (pParte - 1) * gTamanhoDaParte;
            lIndiceFinal   = lIndiceInicial + gTamanhoDaParte;

            if (null != this.ListaDeResultados)
            {
                for (int a = lIndiceInicial; a < this.ListaDeResultados.Count(); a++)
                {
                    lRetorno.Add(this.ListaDeResultados.ElementAt(a));

                    if (a == lIndiceFinal) break;
                }
            }
            return lRetorno;
        }

        private string ResponderBuscarMaisDados()
        {
            string lRetorno;

            int lParte;

            if (int.TryParse(Request.Form["Parte"], out lParte))
            {
                string lMensagemFim;

                if (null == this.ListaDeResultados || (lParte * gTamanhoDaParte) > this.ListaDeResultados.Count())
                {
                    lMensagemFim = "Fim";
                }
                else
                {
                    lMensagemFim = string.Format("TemMais:Parte {0} de {1}" , lParte, Math.Ceiling((double)(this.ListaDeResultados.Count() / gTamanhoDaParte)));
                }
                

                lRetorno = RetornarSucessoAjax(BuscarParte(lParte) , lMensagemFim);
            }
            else
            {
                lRetorno = RetornarSucessoAjax("Fim");
            }

            return lRetorno;
        }

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteCadastradoPeriodoInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteCadastradoPeriodoInfo>();

            try
            {
                ClienteCadastradoPeriodoInfo lInfo = new ClienteCadastradoPeriodoInfo() 
                {
                    DtDe           = this.GetDataInicial,
                    DtAte          = this.GetDataFinal.AddDays(1D),
                    CodigoAssessor = this.GetAssessor,
                    TipoPessoa     = this.GetTipoPessoa,
                    DsCpfCnpj      = this.GetCpfCnpj
                    

                };


                lRequest.EntidadeCadastro = lInfo;

                Stopwatch lContador = new Stopwatch();

                Logger.InfoFormat("Request de Relatório: R-001 (Clientes cadastrados por período). DataDe: [{0}], DataAte: [{1}], CodigoAssessor: [{2}]"
                                    , lInfo.DtDe
                                    , lInfo.DtAte
                                    , lInfo.CodigoAssessor);

                lContador.Start();

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteCadastradoPeriodoInfo>(lRequest);

                lContador.Stop();

                Logger.InfoFormat("Request de Relatório: [{0}] ms para retornar do serviço. [{1}] linhas."
                                    , lContador.ElapsedMilliseconds
                                    , lResponse.Resultado.Count);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        lContador.Restart();

                        IEnumerable<TransporteRelatorio_001> lLista = from ClienteCadastradoPeriodoInfo i in lResponse.Resultado select new TransporteRelatorio_001(i);

                        lContador.Stop();

                        Logger.InfoFormat("Request de Relatório: [{0}] ms para converter a lista para IEnumerable<TransporteRelatorio_001>."
                                            , lContador.ElapsedMilliseconds);

                        lContador.Restart();

                        if (lLista.Count() >= gTamanhoDaParte)
                        {
                            this.ListaDeResultados = lLista;

                            this.rptRelatorio.DataSource = BuscarParte(1);

                            rowLinhaCarregandoMais.Visible = true;
                        }
                        else
                        {
                            this.rptRelatorio.DataSource = lLista;
                        }

                        this.rptRelatorio.DataBind();
                        
                        lContador.Stop();

                        Logger.InfoFormat("Request de Relatório: [{0}] ms para realizar o databind."
                                            , lContador.ElapsedMilliseconds);

                        rowLinhaDeNenhumItem.Visible = false;
                    }
                    else
                    {
                        rowLinhaDeNenhumItem.Visible = true;
                    }
                }
            }
            catch (Exception exBusca)
            {
                throw exBusca;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                ResponderBuscarItensParaListagemSimples();
            }
            else if (this.Acao == "BuscarParte")
            {
                Response.Clear();

                string lResponse = ResponderBuscarMaisDados();

                Response.Write(lResponse);

                Response.End();
            }
        }

        #endregion
    }
}
