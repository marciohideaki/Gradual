using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Enum;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib.Info.Enum;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;
using System.Linq;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R018 : PaginaBaseAutenticada
    {
        #region | Atributos

        public TransporteRelatorio_018 gTransporteRelatorio = new TransporteRelatorio_018();

        private int gTamanhoDaParte = 1;

        #endregion

        #region | Propriedades

        public int? GetCodigoAssessor
        {
            get
            {
                //if (null != base.CodigoAssessor && base.CodigoAssessor >= 0)
                //    return base.CodigoAssessor.Value;
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["Assessor"], out lRetorno))
                    return null;

                return lRetorno;            
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

        public EnumTipoExtradoDeConta GetTipoDeExtrato
        {
            get
            {
                var lRetorno = default(bool);

                if (bool.TryParse(this.Request.Form["TipoMercado"], out lRetorno) && lRetorno)
                    return EnumTipoExtradoDeConta.Movimento;
                else
                    return EnumTipoExtradoDeConta.Liquidacao;
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

        public DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        public DateTime GetDataFim
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

                return lRetorno;
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
                        DsNome = this.GetNomeCliente
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
                    {
                        if (!this.Acao.Equals("CarregarClientes"))
                        {
                            lRetorno.AddRange(lResultado.FindAll(mov => { return mov.DsNome.Trim().IndexOf(this.GetLetraSelecionada, 0, 1) == 0; }));
                        }
                        else
                        {
                            lRetorno = lResultado;
                        }
                    }
                }

                return lRetorno;
            }
        }

        private List<TransporteRelatorio_018> ListaDeResultadosFiltrados
        {
            get
            {
                return (List<TransporteRelatorio_018>)this.Session["ListaDeResultadosFiltrados_Relatorio_018_ExtratoDeContaCorrente"];
            }
            set
            {
                this.Session["ListaDeResultadosFiltrados_Relatorio_018_ExtratoDeContaCorrente"] = value;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                this.ListaDeResultadosFiltrados = new List<TransporteRelatorio_018>();
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
            else if (this.Acao == "CarregarClientes")
            {
                this.Response.Clear();

                string lResponse = this.CarregarClientes();

                this.Response.Write(lResponse);

                this.Response.End();
            }

        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lListaExtratoDeContaCorrente = new List<ContaCorrenteExtratoInfo>();

            this.ListaDeResultadosFiltrados = new List<TransporteRelatorio_018>();

            string[] lClientes = new string[]{""};
            
            if (!String.IsNullOrEmpty(this.Request["cliente"]))
            {
                lClientes = this.Request["cliente"].Split(',');
            }
            //TODO: verificar no js onde é feita a troca da propriedade
            if (!String.IsNullOrEmpty(this.Request["cliente[]"]))
            {
                lClientes = this.Request["cliente[]"].Split(',');
            }

            var lClientesVinculados = this.GetListaClientesVinculados.Where(x => lClientes.Contains(x.CdCodigoBovespa.ToString()));

            foreach (ClientePorAssessorInfo lCliente in lClientesVinculados)
            {
                lListaExtratoDeContaCorrente.Add(this.BuscarExtratoDeContaCorrente(lCliente.CdCodigoBovespa, lCliente.DsNome));
            }
            this.ListaDeResultadosFiltrados = new TransporteRelatorio_018().TraduzirLista(lListaExtratoDeContaCorrente);

            if (null != this.ListaDeResultadosFiltrados)
                this.CarregarDados(0);
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

        private ContaCorrenteExtratoInfo BuscarExtratoDeContaCorrente(int? pCodCliente, string pNomeCliente)
        {
            try
            {
                var lRespostaBusca = Ativador.Get<IServicoExtratos>().ConsultarExtratoContaCorrente(
                    new ContaCorrenteExtratoRequest()
                    {
                        ConsultaTipoExtratoDeConta = this.GetTipoDeExtrato,
                        ConsultaCodigoCliente = pCodCliente,
                        ConsultaNomeCliente = pNomeCliente,
                        ConsultaDataInicio = this.GetDataInicial,
                        ConsultaDataFim = this.GetDataFim,
                    });

                if (lRespostaBusca.StatusResposta == CriticaMensagemEnum.OK)
                {
                    base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = pCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", pCodCliente) });

                    return lRespostaBusca.Relatorio;
                }
                else
                {
                    throw new Exception(string.Format("{0}-{1}", lRespostaBusca.StatusResposta, lRespostaBusca.StackTrace));
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao tentar gerar a nota de corretagem", ex);
                return new ContaCorrenteExtratoInfo();
            }
        }

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

            if (null != gTransporteRelatorio)
            {
                this.rptClientes_ExtratoContaCorrente_Movimento.DataSource = this.gTransporteRelatorio.ListaDetalhesExtrato;
                this.rptClientes_ExtratoContaCorrente_Movimento.DataBind();
            }
        }

        private TransporteRelatorio_018 BuscarPorParte(int pParte)
        {
            var lRetorno = new TransporteRelatorio_018();

            if (null != this.ListaDeResultadosFiltrados && this.ListaDeResultadosFiltrados.Count > pParte)
                lRetorno = this.ListaDeResultadosFiltrados[pParte];

            return lRetorno;
        }

        private string CarregarClientes()
        {
            return base.RetornarSucessoAjax(new TransporteRelatorio_018().TraduzirListaConsulta(this.GetListaClientesVinculados), "Fim");
        }

        #endregion
    }
}
