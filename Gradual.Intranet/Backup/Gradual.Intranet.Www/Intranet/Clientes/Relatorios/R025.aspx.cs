using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R025 : PaginaBaseAutenticada
    {
        #region Propriedades
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

        #region Métodos
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

                if (!base.CodigoAssessor.HasValue && !this.GetCdAssessor.HasValue)
                {
                    lRetorno = RetornarErroAjax("É necessário selecionar um assessor.");
                    this.rowLinhaDeNenhumItem.Visible = true;

                    return lRetorno;
                }
                else
                {
                    lCodigoAssessor = base.CodigoAssessor.HasValue ? base.CodigoAssessor.Value : this.GetCdAssessor.Value;
                }


                var lRequestRendaFixa = new ConsultarEntidadeCadastroRequest<RendaFixaInfo>();

                lRequestRendaFixa.EntidadeCadastro = new RendaFixaInfo();

                lRequestRendaFixa.EntidadeCadastro.CodigoAssessor = this.GetCdAssessor;

                var ListaTransporte = new List<TransporteRelatorioRendaFixa>();

                var lListaCliente = base.ReceberListaClientesAssessoresVinculados(lCodigoAssessor);

                decimal lTotalBruto    = 0.0M;
                decimal lTotalLiquido  = 0.0M;
                int lTotalAplicacoes   = 0;
                var ListaClientes      = new List<int>();

                foreach (ClienteResumidoInfo cliente in lListaCliente)
                {
                    var lListaRebate = new List<TransporteRelatorioRendaFixa>();

                    lRequestRendaFixa.EntidadeCadastro.CodigoCliente = int.Parse(cliente.CodBovespa);

                    var lPosicaoRendaFixa = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RendaFixaInfo>(lRequestRendaFixa);

                    var lTrans = new TransporteRelatorioRendaFixa().TraduzirLista(lPosicaoRendaFixa.Resultado, VencimentoRendaFixa);

                    ListaTransporte.AddRange(lTrans);

                    foreach (var renda in lPosicaoRendaFixa.Resultado)
                    {
                        if (!ListaClientes.Contains(renda.CodigoCliente.Value))
                        {
                            ListaClientes.Add(renda.CodigoCliente.Value);
                        }

                        lTotalAplicacoes++;
                        lTotalBruto   += renda.SaldoBruto;
                        lTotalLiquido += renda.SaldoLiquido;
                    }
                }

                this.lblTotalClientes.Text     = ListaClientes.Count.ToString();
                this.lblTotalLiquido.Text      = lTotalLiquido.ToString("N2");
                this.lblTotalBruto.Text        = lTotalBruto.ToString("N2");
                this.lblTotalAplicacoes.Text   = lTotalAplicacoes.ToString();
                this.lblDataProcessamento.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                this.rptRelatorioRendaFixa.DataSource = ListaTransporte;
                this.rptRelatorioRendaFixa.DataBind();

                this.rowLinhaDeNenhumItem.Visible = ListaTransporte.Count == 0;
            }
            catch (Exception exBusca)
            {
                throw exBusca;
            }

            return lRetorno;
        }
        #endregion
    }
}