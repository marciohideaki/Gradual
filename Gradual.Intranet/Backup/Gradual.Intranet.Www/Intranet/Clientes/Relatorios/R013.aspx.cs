using System;
using System.Collections.Generic;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R013 : PaginaBaseAutenticada
    {
        #region | Atributos

        public decimal gTotalClientes = 0;

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteDistribuicaoRegionalInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteDistribuicaoRegionalInfo>();

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteDistribuicaoRegionalInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count > 0)
                {
                    this.gTotalClientes = lResponse.Resultado.Count;
                    this.rptRelatorio.DataSource = new TransporteRelatorio_013().TraduzirLista(lResponse.Resultado);
                    this.rptRelatorio.DataBind();
                    this.rowLinhaDeNenhumItem.Visible = false;

                    //--> Contando as quantidades por assessor e por estado.
                    var lContadorPorAssessor = new Dictionary<string, int>(); 
                    var lContadorPorEstado = new Dictionary<string, int>();

                    foreach (ClienteDistribuicaoRegionalInfo lInfo in lResponse.Resultado)
                    {
                        if (!lContadorPorAssessor.ContainsKey(lInfo.NM_Assessor))
                            lContadorPorAssessor.Add(lInfo.NM_Assessor, 1);
                        else
                            lContadorPorAssessor[lInfo.NM_Assessor]++;

                        if (!lContadorPorEstado.ContainsKey(lInfo.SG_Estado))
                            lContadorPorEstado.Add(lInfo.SG_Estado, 1);
                        else
                            lContadorPorEstado[lInfo.SG_Estado]++;
                    }

                    //--> Contando as porcentagens por assessor e por estado (precisa estar "mastigado" pro gráfico).
                    var lPorcentagemPorAssessor = new Dictionary<string, double>();
                    var lPorcentagemPorEstado = new Dictionary<string, double>();

                    foreach (string lAssessor in lContadorPorAssessor.Keys)
                        lPorcentagemPorAssessor.Add(lAssessor, lContadorPorAssessor[lAssessor] * 100 / lResponse.Resultado.Count);

                    foreach (string lEstado in lContadorPorEstado.Keys)
                        lPorcentagemPorEstado.Add(lEstado, lContadorPorEstado[lEstado] * 100 / lResponse.Resultado.Count);

                    //--> "convertendo" os valores absolutos e de porcentagem pro label que vai aparecer no HTML.
                    var lDadosPorAssessor = new Dictionary<string, double>();
                    var lDadosPorEstado = new Dictionary<string, double>();

                    foreach (string lAssessor in lContadorPorAssessor.Keys)
                    {
                        lDadosPorAssessor.Add(string.Format("{0}% ({1}) - {2}"
                                                           , lPorcentagemPorAssessor[lAssessor]
                                                           , lContadorPorAssessor[lAssessor]
                                                           , lAssessor)
                                             , lPorcentagemPorAssessor[lAssessor]);
                    }

                    foreach (string lEstado in lContadorPorEstado.Keys)
                    {
                        lDadosPorEstado.Add(string.Format("{0}% ({1}) - {2}"
                                                         , lPorcentagemPorEstado[lEstado]
                                                         , lContadorPorEstado[lEstado]
                                                         , lEstado)
                                           , lPorcentagemPorEstado[lEstado]);
                    }

                    this.rptGrafico_PorAssessor.DataSource = lDadosPorAssessor;
                    this.rptGrafico_PorAssessor.DataBind();

                    this.rptGrafico_PorEstado.DataSource = lDadosPorEstado;
                    this.rptGrafico_PorEstado.DataBind();
                }

                else this.rowLinhaDeNenhumItem.Visible = true;
            }
        }

        private void ResponderArquivoCSV()
        {
            var lBuilder = new StringBuilder();

            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteDistribuicaoRegionalInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteDistribuicaoRegionalInfo>();

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteDistribuicaoRegionalInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.gTotalClientes = lResponse.Resultado.Count;

                lBuilder.AppendLine("CPF Cliente\tNome Cliente\tAssessor\tEstado\tCidade\tBairro\tLogradouro\tComplemento\tCEP\tTelefone\tRamal\tCelular1\tCelular2\te-mail\tData de Cadastro\r\n");

                foreach (TransporteRelatorio_013 lInfo in new TransporteRelatorio_013().TraduzirLista(lResponse.Resultado))
                    lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\r\n"
                        , lInfo.CPFCNPJ, lInfo.NomeCliente, lInfo.NomeAssessor, lInfo.UF, lInfo.Cidade, lInfo.Bairro, lInfo.Logradouro, lInfo.Complemento, lInfo.CEP, lInfo.Telefone, lInfo.Ramal, lInfo.Celular1, lInfo.Celular2, lInfo.Email, lInfo.DataCadastro);

                this.Response.Clear();

                this.Response.ContentType = "text/xls";

                this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                this.Response.Charset = "iso-8859-1";

                this.Response.AddHeader("content-disposition", "attachment;filename=RelatorioClientesDirect.xls");

                this.Response.Write(lBuilder.ToString());

                this.Response.End();
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                this.ResponderBuscarItensParaListagemSimples();
            }
            else if (this.Acao == "BuscarParte")
            {
                this.Response.Clear();

                //string lResponse = this.ResponderBuscarMaisDados();

                //this.Response.Write(lResponse);

                this.Response.End();
            }
            else if (this.Acao == "CarregarComoCSV")
            {
                this.ResponderArquivoCSV();
            }
        }

        #endregion
    }
}