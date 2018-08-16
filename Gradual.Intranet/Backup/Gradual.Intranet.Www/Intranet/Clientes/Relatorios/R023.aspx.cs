using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R023 : PaginaBaseAutenticada
    {

        #region Propriedades
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

                DataAplicacao = lRetorno;

                return lRetorno;
            }
        }

        public DateTime GetDataFinal
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

                DataResgate = lRetorno;

                return lRetorno;
            }
        }

        public string TotalRepasse { get; set; }
        #endregion

        #region Atributos
        public DateTime DataAplicacao;
        public DateTime DataResgate;
        #endregion

        #region Métodos
        private string ResponderBuscarItensParaListagemSimples()
        {
            string lRetorno = string.Empty;

            try
            {
                if (!base.CodigoAssessor.HasValue && !this.GetCdAssessor.HasValue)
                {
                    lRetorno = RetornarErroAjax("É necessário selecionar um assessor.");
                    this.rowLinhaDeNenhumItem.Visible = true;
                    this.rowsLinhaTotalRepasse.Visible = false;
                    return lRetorno;
                }

                int lCodigoAssessor = base.CodigoAssessor.HasValue ? base.CodigoAssessor.Value : this.GetCdAssessor.Value;

                var lListaCliente = base.ReceberListaClientesAssessoresVinculados(lCodigoAssessor);
                    
                var lListaRebate = new List<Transporte_PosicaoCotistaRebate>();

                decimal lTotalRepasse = 0.0M;

                foreach (ClienteResumidoInfo cliente in lListaCliente)
                {
                    int CodigoCliente     = cliente.CodBovespa.DBToInt32();
                    int CodigoAssessor    = cliente.CodAssessor.Value;
                    string CpfCnpjCliente = cliente.CPF;
                    string NomeCliente    = cliente.NomeCliente;
                    
                    try
                    {
                        List<Transporte_PosicaoCotista> lPosicao = base.PosicaoFundos(cliente.CodBovespa.DBToInt32(), cliente.CPF);

                        if (lPosicao.Count > 0)
                        {
                            var lTrans = new Transporte_PosicaoCotistaRebate().TraduzirLista(lPosicao, cliente, this.GetDataInicial, this.GetDataFinal);

                            lTrans.ForEach(trans =>
                            {
                                lTotalRepasse += trans.ValorRepasse.DBToDecimal();
                            });

                            lListaRebate.AddRange(lTrans);
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                this.TotalRepasse = lTotalRepasse.ToString("N2");
                this.rptRelatorioRebate.DataSource = lListaRebate;
                this.rptRelatorioRebate.DataBind();

                this.rowLinhaDeNenhumItem.Visible = lListaRebate.Count == 0;
                this.rowsLinhaTotalRepasse.Visible = lListaRebate.Count != 0;
            }
            catch (Exception exBusca)
            {
                throw exBusca;
            }

            return lRetorno;
        }
        #endregion

        #region Eventos
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

                //ResponderBuscarItensParaListagemSimples();
                ////Response.Write(lResponse);

                Response.End();
            }
        }
        #endregion
    }
}