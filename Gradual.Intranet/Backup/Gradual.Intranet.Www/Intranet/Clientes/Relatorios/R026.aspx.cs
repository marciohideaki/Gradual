using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.ContaCorrente.Lib.Info.Enum;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R026 : PaginaBaseAutenticada
    {

        #region Propriedades
        
        public string gSaldoAnterior    { get; set; }
        public string gSaldoAtual       { get; set; }
        public string gSaldoTotal       { get; set; }
        public string gNomeCliente      { get; set; }
        public string gCodigoCliente    { get; set; }

        private int? VencimentoRendaFixa
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["VencimentoRendaFixa"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }
        private int? GetCdAssessor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Assessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CodCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCodigoIndice
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CodigoIndice"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        public DateTime GetDataInicial
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        public DateTime GetDataFinal
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

                return lRetorno;
            }
        }

        #endregion

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

                Response.End();
            }
        }

        

        private string ResponderBuscarItensParaListagemSimples()
        {
            string lRetorno = string.Empty;

            try
            {
                int lCodigoAssessor = 0;

                var lListaCliente = new List<ClienteResumidoInfo>();

                if (!GetCdCliente.HasValue)
                {
                    lRetorno = RetornarErroAjax("É necessário inserir um cliente.");

                    return lRetorno;
                }

                if (base.CodigoAssessor.HasValue )
                {
                    lCodigoAssessor = base.CodigoAssessor.HasValue ? base.CodigoAssessor.Value : this.GetCdAssessor.Value;
                }

                var lRequestPosicao                             = new ReceberEntidadeCadastroRequest<ClienteCustodiaFinanceiroInfo>();

                lRequestPosicao.EntidadeCadastro                = new ClienteCustodiaFinanceiroInfo();

                lRequestPosicao.EntidadeCadastro.CodigoAssessor = this.GetCdAssessor;

                lRequestPosicao.EntidadeCadastro.De = this.GetDataInicial;

                lRequestPosicao.EntidadeCadastro.Ate = new DateTime(this.GetDataFinal.Year, this.GetDataFinal.Month, 01);

                var ListaTransporte                             = new List<TansportePosicaoCustodiaFinanceiro>();

                var lServicoAtivador = Ativador.Get<IServicoExtratos>();

                var lRespostaBuscaExtrato = lServicoAtivador.ConsultarExtratoContaCorrente(
                    new ContaCorrenteExtratoRequest()
                    {
                        ConsultaTipoExtratoDeConta = EnumTipoExtradoDeConta.Liquidacao,
                        ConsultaCodigoCliente = this.GetCdCliente.Value,
                        //ConsultaNomeCliente = this.GetNomeCliente,
                        ConsultaDataInicio = this.GetDataInicial,
                        ConsultaDataFim = this.GetDataFinal,
                    });


                if (lCodigoAssessor != 0)
                {
                    lListaCliente = base.ReceberListaClientesAssessoresVinculados(lCodigoAssessor);

                    bool lEncontrouCliente = false;

                    foreach (ClienteResumidoInfo cliente in lListaCliente)
                    {
                        if (cliente.CodBovespa == GetCdCliente.Value.ToString())
                        {
                            lEncontrouCliente = true;

                            continue;
                        }
                    }

                    if (!lEncontrouCliente)
                    {
                        lRetorno = RetornarErroAjax("Cliente não encontrado.");

                        return lRetorno;
                    }
                }

                var lListaRebate                                = new List<TansportePosicaoCustodiaFinanceiro>();

                lRequestPosicao.EntidadeCadastro.CodigoCliente  = GetCdCliente.Value;

                var lPosicaoFinanceira                          = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteCustodiaFinanceiroInfo>(lRequestPosicao);

                var lTrans                                      = new TansportePosicaoCustodiaFinanceiro().TraduzirLista(lPosicaoFinanceira.EntidadeCadastro, lRespostaBuscaExtrato.Relatorio);
                gSaldoAnterior = lTrans.SaldoAnterior;
                gSaldoAtual    = lTrans.SaldoDisponivel;
                gSaldoTotal    = lTrans.SaldoTotal;

                gNomeCliente = lTrans.NomeCliente;
                gCodigoCliente = lTrans.CodigoCliente;
                

                this.rptRelatorioPosicaoVista.DataSource = lTrans.ListaPosicaoAVista;
                this.rptRelatorioPosicaoVista.DataBind();

                this.rptRelatorioPosicaoTermo.DataSource = lTrans.ListaPosicaoTermo;
                this.rptRelatorioPosicaoTermo.DataBind();

                this.rptRelatorioPosicaoOpcao.DataSource = lTrans.ListaPosicaoOpcao;
                this.rptRelatorioPosicaoOpcao.DataBind();

                this.rptRelatorioPosicaoTesouro.DataSource = lTrans.ListaPosicaoTesouro;
                this.rptRelatorioPosicaoTesouro.DataBind();

                this.rptRelatorioExtratoVista.DataSource = lTrans.ListaExtratoAVista;
                this.rptRelatorioExtratoVista.DataBind();

                this.rptRelatorioExtratoTermo.DataSource = lTrans.ListaExtratoTermo;
                this.rptRelatorioExtratoTermo.DataBind();

                this.rptRelatorioExtratoOpcao.DataSource = lTrans.ListaExtratoOpcao;
                this.rptRelatorioExtratoOpcao.DataBind();

                this.rptRelatorioExtratoMovimento.DataSource = lTrans.ListaExtratoMovimento;
                this.rptRelatorioExtratoMovimento.DataBind();

                this.rowLinhaDeNenhumItemPosicaoVista.Visible   = lTrans.ListaPosicaoAVista.Count == 0;
                this.rowLinhaDeNenhumItemPosicaoOpcao.Visible   = lTrans.ListaPosicaoOpcao.Count == 0;
                this.rowLinhaDeNenhumItemPosicaoTermo.Visible   = lTrans.ListaPosicaoTermo.Count == 0;
                this.rowLinhaDeNenhumItemPosicaoTesouro.Visible = lTrans.ListaPosicaoTesouro.Count == 0;

                this.rowLinhaDeNenhumItemExtratoTermo.Visible = lTrans.ListaExtratoTermo.Count == 0;
                this.rowLinhaDeNenhumItemExtratoOpcao.Visible = lTrans.ListaExtratoOpcao.Count == 0;
                this.rowLinhaDeNenhumItemExtratoVista.Visible = lTrans.ListaExtratoAVista.Count == 0;
                this.rowLinhaDeNenhumItemExtratoMovimento.Visible = lTrans.ListaExtratoMovimento.Count == 0;

                
            }
            catch (Exception exBusca)
            {
                throw exBusca;
            }

            return lRetorno;
        }
    }
}