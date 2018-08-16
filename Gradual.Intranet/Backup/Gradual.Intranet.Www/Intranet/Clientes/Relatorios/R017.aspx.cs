using System;
using System.Collections.Generic;
using System.Globalization;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R017 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 1;

        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        public TransporteRelatorio_017 gTransporteRelatorio = new TransporteRelatorio_017();

        #endregion

        #region | Propriedades

        public int? GetCodigoAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0)
                    return base.CodigoAssessor.Value;

                return null;
            }
        }

        private int GetCodigoCorretora
        {
            get
            {
                return 2271;
            }
        }

        public int? GetCodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["cliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        public DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataInicial"], gCultureInfo, DateTimeStyles.None, out lRetorno);

                return lRetorno;
            }
        }

        public DateTime GetDataFim
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataFinal"], gCultureInfo, DateTimeStyles.None, out lRetorno);

                return lRetorno;
            }
        }

        public string GetTipoMercado
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Request.Form["NotaDeCorretagemTipoMercado"])
                && this.Request.Form["NotaDeCorretagemTipoMercado"].Contains("OPC"))
                    return "OPC";
                else
                    return "VIS";
            }
        }

        private List<ClientePorAssessorInfo> GetListaClientesVinculados
        {
            get
            {
                var lRetorno = new List<ClientePorAssessorInfo>();

                if (null != this.GetCodigoCliente)
                {
                    lRetorno.Add(new ClientePorAssessorInfo()
                    {
                        CdCodigoBovespa = this.GetCodigoCliente.Value
                    });
                }
                else
                {
                    lRetorno = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePorAssessorInfo>(
                        new ConsultarEntidadeCadastroRequest<ClientePorAssessorInfo>()
                        {
                            EntidadeCadastro = new ClientePorAssessorInfo()
                            {
                                ConsultaCdAssessor = this.GetCodigoAssessor
                            }
                        }).Resultado;
                }

                return lRetorno;
            }
        }

        private List<TransporteRelatorio_017> ListaDeResultados
        {
            get
            {
                return (List<TransporteRelatorio_017>)this.Session["ListaDeResultados_Relatorio_017_NotaDeCorretagem"];
            }
            set
            {
                this.Session["ListaDeResultados_Relatorio_017_NotaDeCorretagem"] = value;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                this.ResponderBuscarItensParaListagemSimples();
            }
            else if (this.Acao == "BuscarParte")
            {
                //this.Response.Clear();

                string lResponse = this.ResponderBuscarMaisDados();

                //this.Response.Write(lResponse);

                //this.Response.End();
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lListaNotaDeCorretagem = new List<NotaDeCorretagemExtratoInfo>();
            var lListaDataNegocioDoCliente = new List<DateTime>();
            int lIncrementoEmDias = default(int); //--> Lista o range de dia da pesquisa

            this.GetListaClientesVinculados.ForEach(lCai =>
            {
                lListaDataNegocioDoCliente = this.BuscarDatasUltimasNegociacoes(lCai.CdCodigoBovespa, this.GetDataInicial, this.GetDataFim ); //--> Recuperando os dias negociados por cliente.

                while (lListaDataNegocioDoCliente.Count > 0 && this.GetDataInicial.AddDays(lIncrementoEmDias) <= this.GetDataFim.AddDays(1))
                {   //--> Verificando se o range de data informado tem negociação para este cliente.
                    if (lListaDataNegocioDoCliente.Contains(this.GetDataInicial.AddDays(lIncrementoEmDias)))
                    {
                        var lNotaCliente = this.BuscarNotaPorCliente(lCai.CdCodigoBovespa, this.GetDataInicial.AddDays(lIncrementoEmDias));

                        if (lNotaCliente.ListaNotaDeCorretagemExtratoInfo !=null 
                            && lNotaCliente.ListaNotaDeCorretagemExtratoInfo.Count != 0)
                        {
                            lListaNotaDeCorretagem.Add(lNotaCliente);
                        }
                    }
                    lIncrementoEmDias++;
                }

                lIncrementoEmDias = 0;
            });

            this.ListaDeResultados = new TransporteRelatorio_017().TraduzirLista(lListaNotaDeCorretagem);

            if (null != this.ListaDeResultados)
            {
                this.CarregarDados(0);
            }
        }

        private string ResponderBuscarMaisDados()
        {
            string lRetorno = string.Empty;

            int lParte;

            if (int.TryParse(Request.Form["Parte"], out lParte))
            {
                if (null == this.ListaDeResultados || (lParte * gTamanhoDaParte) > this.ListaDeResultados.Count)
                {
                    this.divCarregandoMais.Visible = false;
                    this.divRelatorio.Visible = false;
                }
                else
                {
                    this.CarregarDados(lParte);
                }
            }
            else
            {
                lRetorno = base.RetornarSucessoAjax("Fim");
            }

            return lRetorno;
        }

        private NotaDeCorretagemExtratoInfo BuscarNotaPorCliente(int pCdCliente, DateTime pDataMovimento)
        {
            try
            {
                var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();

                var lResponse = lServicoAtivador.ConsultarNotaDeCorretagem(new OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest()
                {
                    ConsultaProvisorio = false,
                    ConsultaCodigoCliente = pCdCliente,
                    ConsultaDataMovimento = pDataMovimento,
                    ConsultaTipoDeMercado = this.GetTipoMercado,
                    ConsultaCodigoCorretora = this.GetCodigoCorretora,
                });

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodigoCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodigoCliente) });

                    return lResponse.Relatorio;
                }
                else
                {
                    throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao tentar gerar a nota de corretagem", ex);
                return new NotaDeCorretagemExtratoInfo();
            }
        }

        private List<DateTime> BuscarDatasUltimasNegociacoes(int pCdCliente, DateTime pDe, DateTime pAte)
        {
            var lRetorno = new List<DateTime>();
            var lResponse = new ConsultarObjetosResponse<UltimasNegociacoesInfo>();
            var lRequest = new ConsultarEntidadeRequest<UltimasNegociacoesInfo>();

            lRequest.Objeto              = new UltimasNegociacoesInfo();
            lRequest.Objeto.CdCliente    = pCdCliente;
            lRequest.Objeto.CdClienteBmf = pCdCliente;
            lRequest.Objeto.DataDe       = pDe;
            lRequest.Objeto.DataAte      = pAte;

            lResponse = new PersistenciaDbIntranet().ConsultarObjetos<UltimasNegociacoesInfo>(lRequest);

            if (null != lResponse.Resultado && lResponse.Resultado.Count > 0)
                lResponse.Resultado.ForEach(lUni =>
                {
                    lRetorno.Add(lUni.DtUltimasNegociacoes);
                });

            return lRetorno;
        }

        private TransporteRelatorio_017 BuscarPorParte(int pParte)
        {
            var lRetorno = new TransporteRelatorio_017();

            if (null != this.ListaDeResultados && this.ListaDeResultados.Count > pParte)
                lRetorno = this.ListaDeResultados[pParte];

            return lRetorno;
        }

        private void CarregarDados(int gParte)
        {
            if ((gParte + 1) == Math.Ceiling((double)(this.ListaDeResultados.Count / gTamanhoDaParte)) || this.ListaDeResultados.Count == 1)
            {
                this.divCarregandoMais.Visible = false;
            }
            else
            {
                this.divCarregandoMais.Visible = true;
                this.lblQuantoDeTanto.Text = string.Format("Parte {0} de {1}", (gParte + 1).ToString(), Math.Ceiling((double)(this.ListaDeResultados.Count / gTamanhoDaParte)));
            }

            this.gTransporteRelatorio = this.BuscarPorParte(gParte);

            if (null != gTransporteRelatorio)
            {
                this.rptLinhasDoRelatorio.DataSource = this.gTransporteRelatorio.NotaMovimento;
                this.rptLinhasDoRelatorio.DataBind();
            }
        }

        #endregion
    }
}