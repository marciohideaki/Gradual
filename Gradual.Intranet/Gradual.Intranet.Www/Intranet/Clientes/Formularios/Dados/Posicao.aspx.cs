using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class Posicao : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int CodigoBovespa
        {
            get
            {
                int lRetorno = 0;

                if (!string.IsNullOrWhiteSpace(Request.Form["CodBovespa"]))
                    lRetorno = Convert.ToInt32(Request.Form["CodBovespa"]);

                return lRetorno;
            }

        }

        private int CodigoBMF
        {
            get
            {
                int lRetorno = 0;

                if (!string.IsNullOrWhiteSpace(Request.Form["CodBMF"]))
                    lRetorno = Convert.ToInt32(Request.Form["CodBMF"]);

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BuscarUltimasDatasNegociacao();
                this.CarregarComboImpostoDeRenda();
            }
        }

        #endregion

        #region | Métodos

        private void BuscarUltimasDatasNegociacao()
        {
            ConsultarObjetosResponse<UltimasNegociacoesInfo> lResponse = new ConsultarObjetosResponse<UltimasNegociacoesInfo>();

            ConsultarEntidadeRequest<UltimasNegociacoesInfo> lRequest = new ConsultarEntidadeRequest<UltimasNegociacoesInfo>();

            lRequest.Objeto = new UltimasNegociacoesInfo();

            lRequest.Objeto.CdCliente = this.CodigoBovespa;

            lRequest.Objeto.CdClienteBmf = this.CodigoBMF;

            //lResponse = new PersistenciaDbIntranet().ConsultarObjetos<UltimasNegociacoesInfo>(lRequest);

            //this.rptItensDatasOperacaoInicial.DataSource = lResponse.Resultado;
            //this.rptItensDatasOperacaoInicial.DataBind();

            //this.rptItensDatasOperacaoFinal.DataSource = lResponse.Resultado;
            //this.rptItensDatasOperacaoFinal.DataBind();
        }

        private void CarregarComboImpostoDeRenda()
        {
            var lListaAnos = new List<KeyValuePair<string, string>>();

            for (int y = DateTime.Now.Year - 1; y > DateTime.Now.Year - 6; y--)
                lListaAnos.Add(new KeyValuePair<string, string>(y.ToString(), y.ToString()));

            this.rptGradIntra_Clientes_ImpostoDeRenda_AnoVigencia.DataSource = lListaAnos;
            this.rptGradIntra_Clientes_ImpostoDeRenda_AnoVigencia.DataBind();
        }

        #endregion
    }
}