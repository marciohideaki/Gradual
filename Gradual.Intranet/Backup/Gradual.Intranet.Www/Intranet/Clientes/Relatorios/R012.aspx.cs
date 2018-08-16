using System;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R012 : PaginaBaseAutenticada
    {
        #region | Atributos

        public decimal gTotalClientes = 0;

        public decimal gTotalRelatorio = 0;

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            ConsultarEntidadeCadastroRequest<ClienteMigradoCorretagemAnualInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteMigradoCorretagemAnualInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id };
            ConsultarEntidadeCadastroResponse<ClienteMigradoCorretagemAnualInfo> lResponse = new ConsultarEntidadeCadastroResponse<ClienteMigradoCorretagemAnualInfo>();

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteMigradoCorretagemAnualInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {

                //IEnumerable<TransporteRelatorio_011> lLista = from ClienteDirectInfo i in lResponse.Resultado select new TransporteRelatorio_011(i);

                //gTotalClientes = lLista.Count();

                if (lResponse.Resultado.Count > 0)
                {
                    gTotalClientes = lResponse.Resultado.Count;

                    rptRelatorio.DataSource = lResponse.Resultado;

                    rptRelatorio.DataBind();

                    rowLinhaDeNenhumItem.Visible = false;

                    foreach (ClienteMigradoCorretagemAnualInfo lInfo in lResponse.Resultado)
                    {
                        gTotalRelatorio += lInfo.Total;
                    }
                }
                else
                {
                    rowLinhaDeNenhumItem.Visible = true;
                }
            }
        }

        private void ResponderArquivoCSV()
        {
            StringBuilder lBuilder = new StringBuilder();

            ConsultarEntidadeCadastroRequest<ClienteMigradoCorretagemAnualInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteMigradoCorretagemAnualInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id };
            ConsultarEntidadeCadastroResponse<ClienteMigradoCorretagemAnualInfo> lResponse = new ConsultarEntidadeCadastroResponse<ClienteMigradoCorretagemAnualInfo>();

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteMigradoCorretagemAnualInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                gTotalClientes = lResponse.Resultado.Count;

                lBuilder.AppendLine("Nome Cliente\tAssessor\tData Cadastro\tData da Última Operação\tTotal\t\r\n");

                foreach (ClienteMigradoCorretagemAnualInfo lInfo in lResponse.Resultado)
                {
                    gTotalRelatorio += lInfo.Total;

                    lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t\r\n", lInfo.NM_Cliente, lInfo.NM_Assessor, lInfo.DT_Criacao, lInfo.DT_Ult_Oper, lInfo.Total);
                }

                lBuilder.AppendFormat("\t\t\t\t{0}\r\n", gTotalRelatorio);

                Response.Clear();

                Response.ContentType = "text/xls";

                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                Response.Charset = "iso-8859-1";

                Response.AddHeader("content-disposition", "attachment;filename=RelatorioClientesDirect.xls");

                Response.Write(lBuilder.ToString());

                Response.End();
            }
        }

        #endregion

        #region | Eventos

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

                //string lResponse = ResponderBuscarMaisDados();

                //Response.Write(lResponse);

                Response.End();
            }
            else if (this.Acao == "CarregarComoCSV")
            {
                ResponderArquivoCSV();
            }
        }

        #endregion
    }
}