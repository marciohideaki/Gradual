using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;

namespace Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados
{
    public partial class DadosDeVenda : PaginaBaseAutenticada
    {
        #region Métodos Private

        private string ResponderCarregarHtmlComDados()
        {
            string lMensagem;

            ConsultarEntidadeCadastroRequest<VendaDeFerramentaInfo>  lRequest = new ConsultarEntidadeCadastroRequest<VendaDeFerramentaInfo>();
            ConsultarEntidadeCadastroResponse<VendaDeFerramentaInfo> lResponse;

            lRequest.EntidadeCadastro = new VendaDeFerramentaInfo();
            lRequest.EntidadeCadastro.Busca_IdVenda = Convert.ToInt32(Request["Id"]);

            try
            {
                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<VendaDeFerramentaInfo>(lRequest);

                decimal lTotal = 0;

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    List<TransporteVendaDeFerramentaInfo> lLista = new List<TransporteVendaDeFerramentaInfo>();

                    foreach (VendaDeFerramentaInfo lVendaProduto in lResponse.Resultado)
                    {
                        lLista.Add(new TransporteVendaDeFerramentaInfo(lVendaProduto));

                        lTotal += lVendaProduto.VlPreco;
                    }

                    txtSolicitacoes_DadosDaVenda_ID.Text                   = lLista[0].Id;
                    txtSolicitacoes_DadosDaVenda_CodigoDeReferencia.Text   = lLista[0].CodigoDeReferencia;
                    txtSolicitacoes_DadosDaVenda_CBLC.Text                 = lLista[0].CBLC;
                    txtSolicitacoes_DadosDaVenda_CpfCnpj.Text              = lLista[0].CpfCnpj;
                    txtSolicitacoes_DadosDaVenda_Status.Text               = string.Format("{0} - ", lLista[0].Status);
                    txtSolicitacoes_DadosDaVenda_DescricaoStatus.Text      = lLista[0].DescricaoStatus;
                    txtSolicitacoes_DadosDaVenda_Data.Text                 = lLista[0].Data;
                    txtSolicitacoes_DadosDaVenda_Quantidade.Text           = lLista[0].Quantidade;
                    //txtSolicitacoes_DadosDaVenda_Preco.Text                = lLista[0].Preco;
                    //txtSolicitacoes_DadosDaVenda_IdProduto.Text            = lLista[0].IdProduto;
                    //txtSolicitacoes_DadosDaVenda_DescProduto.Text          = lLista[0].DescProduto;
                    txtSolicitacoes_DadosDaVenda_IdPagamento.Text          = lLista[0].IdPagamento;
                    txtSolicitacoes_DadosDaVenda_Tipo.Text                 = lLista[0].Tipo;
                    txtSolicitacoes_DadosDaVenda_MetodoTipo.Text           = lLista[0].MetodoTipo;
                    txtSolicitacoes_DadosDaVenda_MetodoDesc.Text           = lLista[0].MetodoDesc;
                    txtSolicitacoes_DadosDaVenda_ValorBruto.Text           = lTotal.ToString("N2");
                    txtSolicitacoes_DadosDaVenda_ValorDesconto.Text        = lLista[0].ValorDesconto;
                    txtSolicitacoes_DadosDaVenda_ValorTaxas.Text           = lLista[0].ValorTaxas;
                    txtSolicitacoes_DadosDaVenda_ValorLiquido.Text         = lLista[0].ValorLiquido;
                    txtSolicitacoes_DadosDaVenda_QuantidadeDeParcelas.Text = lLista[0].QuantidadeDeParcelas;

                    if(lResponse.Resultado[0].EnderecoDeEntrega != null)
                    {
                        lblEnderecoEntrega.Text = string.Format("{0} {1}, {2} - {3} {4}<br/>CEP {5}-{6}"
                                                                , lResponse.Resultado[0].EnderecoDeEntrega.DsLogradouro
                                                                , lResponse.Resultado[0].EnderecoDeEntrega.DsBairro
                                                                , string.Format("n. {0} {1}", lResponse.Resultado[0].EnderecoDeEntrega.DsNumero, lResponse.Resultado[0].EnderecoDeEntrega.DsComplemento)
                                                                , lResponse.Resultado[0].EnderecoDeEntrega.DsCidade
                                                                , lResponse.Resultado[0].EnderecoDeEntrega.CdUf
                                                                , lResponse.Resultado[0].EnderecoDeEntrega.NrCep.ToString().PadLeft(5, '0')
                                                                , lResponse.Resultado[0].EnderecoDeEntrega.NrCepExt.ToString().PadLeft(3, '0'));
                    }

                    lblTelefones.Text = string.Format("{0} / {1}", lResponse.Resultado[0].TelDeEntrega, lResponse.Resultado[0].CelDeEntrega);

                    if (lblTelefones.Text.TrimStart().StartsWith("/"))
                        lblTelefones.Text = lblTelefones.Text.TrimStart(" /".ToCharArray());

                    rptProdutosDaVenda.DataSource = lLista;
                    rptProdutosDaVenda.DataBind();
                }
                else
                {
                    lMensagem = string.Format("Resposta com erro do serviço");

                    Logger.Error(lMensagem);
                }
            }
            catch (Exception exBusca)
            {
                lMensagem = string.Format("Erro em DadosDeVenda.aspx > ResponderCarregarHtmlComDados() [{0}]\r\n{1}", exBusca.Message, exBusca.StackTrace);

                Logger.Error(lMensagem);
            }

            return string.Empty;
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] {
                                                  "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[] {
                                                  ResponderCarregarHtmlComDados
                                                });
        }

        #endregion
    }
}