using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos
{
    public partial class OperacoesSaldo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var lBase = (PaginaBase)this.Page;

                if (string.IsNullOrEmpty(lBase.SessaoClienteLogado.CodigoPrincipal))
                {
                    if (!lBase.JavascriptParaRodarOnLoad.Contains("Você ainda não possui código de conta Gradual"))
                    {
                        lBase.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                    }
                    return;
                }

                this.CarregaDados();
            }
        }

        #region Métodos
        private void CarregaDados()
        {
            ContaCorrenteExtratoInfo lSaldo = BuscarDadosSaldo();

            decimal lSaldoCC = lSaldo.SaldoTotal;

            var lBase = this.Page as PaginaFundos;

            var ListaContaCorrente = lBase.ObterDadosContaCorrente( lBase.SessaoClienteLogado.CodigoPrincipal.DBToInt32());

            var ListaSaldo = new List<ListaSaldoClienteAux>();

            foreach (var corrente in ListaContaCorrente)
            {
                var saldo= new ListaSaldoClienteAux();

                saldo.SaldoDisponivel = (corrente.SaldoD0 + corrente.SaldoD1 + corrente.SaldoD2 + corrente.SaldoD3).ToString("N2");
                saldo.SaldoBloqueado  = corrente.SaldoBloqueado.HasValue ? corrente.SaldoBloqueado.Value.ToString("N2") : "0,00";
                saldo.SaldoTotal      = (decimal.Parse(saldo.SaldoDisponivel) - decimal.Parse(saldo.SaldoBloqueado)).ToString("N2");

                ListaSaldo.Add(saldo);
            }

            this.rptListaDeSaldos.DataSource = ListaSaldo;
            this.rptListaDeSaldos.DataBind();

            this.trNenhumSaldos.Visible = ListaSaldo.Count == 0;
        }
        
        public struct ListaSaldoClienteAux
        {
            public string SaldoDisponivel { get; set; }
            public string SaldoBloqueado { get; set; }
            public string SaldoTotal { get; set; }
        }

        private ContaCorrenteExtratoInfo BuscarDadosSaldo()
        {
            ContaCorrenteExtratoInfo lRetorno = null;

            var lBase = (PaginaBase)this.Page;

            IServicoExtratos lServico = lBase.InstanciarServicoDoAtivador<IServicoExtratos>();

            ContaCorrenteExtratoRequest lRequest = new ContaCorrenteExtratoRequest();
            ContaCorrenteExtratoResponse lResponse;

            lRequest.ConsultaCodigoCliente      = lBase.SessaoClienteLogado.CodigoPrincipal.DBToInt32();
            lRequest.ConsultaDataInicio         = DateTime.Now.AddDays(-1);
            lRequest.ConsultaDataFim            = DateTime.Now;
            lRequest.ConsultaNomeCliente        = lBase.SessaoClienteLogado.Nome;
            lRequest.ConsultaTipoExtratoDeConta = OMS.ContaCorrente.Lib.Info.Enum.EnumTipoExtradoDeConta.Movimento;

            lResponse = lServico.ConsultarExtratoContaCorrente(lRequest);

            if (lResponse.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
            {
                //lRetorno = new TransporteRelatorioExtratoContaCorrente( lResponse.Relatorio );

                lRetorno = lResponse.Relatorio;
            }
            else
            {
                throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.StackTrace));
            }

            return lRetorno;
        }
        #endregion
    }
}