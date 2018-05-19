using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R019 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 1;

        private IServicoRelatoriosFinanceiros lServicoRelatoriosFinanceiros = null;

        public TransporteRelatorio_019 gTransporteRelatorio = new TransporteRelatorio_019();

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

        public string GetLetraSelecionada
        {
            get
            {
                var lRetorno = "A";

                if (!string.IsNullOrWhiteSpace(this.Request.Form["LetraSelecionada"]))
                    lRetorno = this.Request.Form["LetraSelecionada"].ToString();

                return lRetorno;
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

        public string GetNomeCliente
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request.Form["ClienteDesc"]) && this.Request.Form["ClienteDesc"].Length > 8)
                    return this.Request.Form["ClienteDesc"].Substring(8);

                return string.Empty;
            }
        }

        public string GetBolsa
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request.Form["Bolsa"]))
                    return this.Request.Form["Bolsa"];

                return null;
            }
        }

        public string GetMercado
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request.Form["CustodiaMercado"]))
                    return this.Request.Form["CustodiaMercado"];

                return null;
            }
        }

        public DateTime? GetDataVencimentoTermo
        {
            get
            {
                var lRetorno = default(DateTime);

                if ("TER".Equals(this.GetMercado)
                && (DateTime.TryParse(this.Request.Form["DataVencimentoTermo"], out lRetorno)))
                    return lRetorno;

                return null;
            }
        }

        private List<TransporteRelatorio_019> ListaDeResultadosFiltrados
        {
            get
            {
                return (List<TransporteRelatorio_019>)this.Session["ListaDeResultadosFiltrados_Relatorio_019_PosicaoCustodia"];
            }
            set
            {
                this.Session["ListaDeResultadosFiltrados_Relatorio_019_PosicaoCustodia"] = value;
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
                        CdCodigoBovespa = this.GetCodigoCliente.Value,
                        DsNome = this.GetNomeCliente,
                    });
                }
                else
                {
                    var lResultado = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePorAssessorInfo>(
                        new ConsultarEntidadeCadastroRequest<ClientePorAssessorInfo>()
                        {
                            EntidadeCadastro = new ClientePorAssessorInfo()
                            {
                                ConsultaCdAssessor = this.GetCodigoAssessor
                            }
                        }).Resultado;

                    lRetorno.Clear();

                    if (null != lResultado)
                        lRetorno.AddRange(lResultado.FindAll(mov => { return mov.DsNome.Trim().IndexOf(this.GetLetraSelecionada, 0, 1) == 0; }));
                }

                return lRetorno;
            }
        }

        private IServicoRelatoriosFinanceiros ServicoRelatoriosFinanceiros
        {
            get
            {
                if (null == this.lServicoRelatoriosFinanceiros)
                    this.lServicoRelatoriosFinanceiros = Ativador.Get<IServicoRelatoriosFinanceiros>();

                return this.lServicoRelatoriosFinanceiros;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                this.ListaDeResultadosFiltrados = new List<TransporteRelatorio_019>();
                this.ResponderBuscarItensParaListagemSimples();
            }
            else if (this.Acao == "BuscarParte")
            {
                this.ResponderBuscarMaisDados();
            }
            else if (this.Acao == "BuscarPorLetra")
            {
                this.ResponderBuscarItensParaListagemSimples();
            }
        }

        #endregion

        #region | Métodos

        private void CarregarDados(int gParte)
        {
            if ((gParte + 1) == Math.Ceiling((double)(this.ListaDeResultadosFiltrados.Count / gTamanhoDaParte)))
            {
                this.divCarregandoMais.Visible = false;
            }
            else
            {
                this.divCarregandoMais.Visible = true;
                this.lblQuantoDeTanto.Text = string.Format("Parte {0} de {1}", (gParte + 1).ToString(), Math.Ceiling((double)(this.ListaDeResultadosFiltrados.Count / gTamanhoDaParte)));
            }

            this.gTransporteRelatorio = this.BuscarPorParte(gParte);

            if (null != gTransporteRelatorio && gTransporteRelatorio.ListaConteudoRelatorio.Count > 0)
            {
                this.rptClientes_Custodia_Detalhes.DataSource = this.gTransporteRelatorio.ListaConteudoRelatorio;
                this.rptClientes_Custodia_Detalhes.DataBind();
            }
            else
            {
                this.trClienteSemCustodia.Visible = true;
            }
        }

        private string ResponderBuscarMaisDados()
        {
            string lRetorno = string.Empty;

            int lParte;

            if (int.TryParse(Request.Form["Parte"], out lParte))
            {
                if (null == this.ListaDeResultadosFiltrados || (lParte * gTamanhoDaParte) >= this.ListaDeResultadosFiltrados.Count || this.ListaDeResultadosFiltrados.Count == 1)
                {
                    this.divCarregandoMais.Visible = false;
                    this.divRelatorio.Visible = false;
                    this.ListaDeResultadosFiltrados = null;
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

            return base.RetornarSucessoAjax("Funciou");
        }

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lListaPosicaoTBC = new List<PosicaoCustodiaInfo>();

            this.ListaDeResultadosFiltrados = new List<TransporteRelatorio_019>();

            this.GetListaClientesVinculados.ForEach(lZaz =>
            {
                lListaPosicaoTBC.Add(this.BuscarCustodia(lZaz.CdCodigoBovespa, lZaz.CdCodigoBMF, lZaz.DsNome));
            });

            //--> Filtra o resultado por Mercado e executa a tradução para expor na tela.
            this.ListaDeResultadosFiltrados = new TransporteRelatorio_019().TraduzirLista(lListaPosicaoTBC, this.GetMercado);

            if (null != this.ListaDeResultadosFiltrados)
                this.CarregarDados(0);
        }

        private TransporteRelatorio_019 BuscarPorParte(int pParte)
        {
            var lRetorno = new TransporteRelatorio_019();

            if (null != this.ListaDeResultadosFiltrados && this.ListaDeResultadosFiltrados.Count > pParte)
                lRetorno = this.ListaDeResultadosFiltrados[pParte];

            return lRetorno;
        }

        private PosicaoCustodiaInfo BuscarCustodia(int pCdClienteBovespa, int pCdClienteBMF, string pDsNome)
        {
            var lRetorno = this.ServicoRelatoriosFinanceiros.ConsultarCustodia(new PosicaoCustodiaRequest()
            {
                ConsultaCdClienteBMF = pCdClienteBMF,
                ConsultaCdClienteBovespa = pCdClienteBovespa,
                ConsultaDtVencimentoTermo = this.GetDataVencimentoTermo
            });

            if (lRetorno.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lRetorno.Objeto.DsNomeCliente = pDsNome;
                lRetorno.Objeto.CdCodigo = pCdClienteBovespa;

                lRetorno.Objeto.ListaMovimento = this.FiltrarLista(lRetorno.Objeto);

                return lRetorno.Objeto;
            }

            return new PosicaoCustodiaInfo();
        }

        private List<PosicaoCustodiaInfo.CustodiaMovimento> FiltrarLista(PosicaoCustodiaInfo pParametro)
        {
            var lRetorno = new List<PosicaoCustodiaInfo.CustodiaMovimento>();

            if (null != pParametro.ListaMovimento && pParametro.ListaMovimento.Count > 0)
            {
                switch (this.GetBolsa)
                {
                    case "bmf":
                        lRetorno = pParametro.ListaMovimento.FindAll(cci => "BMF".Equals(cci.TipoGrupo.DBToString().ToUpper()));
                        break;

                    default:
                        lRetorno = pParametro.ListaMovimento.FindAll(cci => !"BMF".Equals(cci.TipoGrupo.DBToString().ToUpper()));
                        break;
                }
            }

            return lRetorno;
        }

        #endregion
    }
}