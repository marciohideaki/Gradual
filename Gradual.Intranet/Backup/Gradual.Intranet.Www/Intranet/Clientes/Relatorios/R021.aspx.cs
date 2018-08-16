using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Cliente;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R021 : PaginaBaseAutenticada
    {
        #region Properties
        public DateTime DataInicial ;
        public DateTime DataFinal ;
        private int? GetAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                return null;
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

        public DateTime GetDataInicial
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno);

                DataInicial = lRetorno;

                return lRetorno;
            }
        }

        public DateTime GetDataFinal
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

                DataFinal = lRetorno;

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

                ResponderBuscarItensParaListagemSimples();
                //Response.Write(lResponse);

                Response.End();
            }
        }
        #endregion

        #region Métodos
        
        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ProventosClienteInfo() 
            { 
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DataDe          = this.GetDataInicial,
                DataAte         = this.GetDataFinal,
                CodigoAssessor  = this.GetAssessor,
                CodigoCliente   = this.GetCdCliente,
            };

            try
            {
                ProventosClienteDbLib lServico = new ProventosClienteDbLib();

                var lResponse = lServico.ConsultarProventosCliente(lRequest);

                if (lResponse.Resultado != null && lResponse.Resultado.Count > 0)
                {
                    List<ProventosClienteInfo> lListaTemp = lResponse.Resultado;//.OrderBy(lp => lp.CodigoAssessor).ToList();  

                    //List<ProventosClienteInfo> lListaBusca = new List<ProventosClienteInfo>();

                    //lListaTemp.ForEach(result =>
                    //{
                    //    ProventosClienteInfo lCad = new ProventosClienteInfo();

                    //    lCad = result;

                    //    ProventosClienteInfo lCadBusca = lListaBusca.Find(busca => busca.CodigoAssessor == lCad.CodigoAssessor );

                    //    if (lCadBusca != null)
                    //    {
                    //        lListaBusca.Remove(lCadBusca);
                    //        lListaBusca.Add(lCadBusca);
                    //    }
                    //    else
                    //    {
                    //        lListaBusca.Add(lCad);
                    //    }
                    //});

                    IEnumerable<TransporteRelatorio_021> lLista = from ProventosClienteInfo i in lListaTemp select new TransporteRelatorio_021(i);

                    this.rptRelatorio.DataSource = lLista;

                    this.rptRelatorio.DataBind();

                    rowLinhaDeNenhumItem.Visible = false;
                }
                else
                {
                    rowLinhaDeNenhumItem.Visible = true;
                }
                
            }
            catch (Exception exBusca)
            {
                throw exBusca;
            }
        }
        #endregion
    }
}