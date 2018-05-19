using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R024 : PaginaBaseAutenticada
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
                    
                    return lRetorno;
                }

                int lCodigoAssessor = base.CodigoAssessor.HasValue ? base.CodigoAssessor.Value : this.GetCdAssessor.Value;

                var lListaCliente = base.ReceberListaClientesDoAssessor(lCodigoAssessor);

                var lListaRebate = new List<TransporteClienteAssessor>();

                var lTrans = new TransporteClienteAssessor().TraduzirLista(lListaCliente);

                this.rptRelatorioRebate.DataSource = lTrans;
                this.rptRelatorioRebate.DataBind();

                this.rowLinhaDeNenhumItem.Visible = lTrans.Count == 0;
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