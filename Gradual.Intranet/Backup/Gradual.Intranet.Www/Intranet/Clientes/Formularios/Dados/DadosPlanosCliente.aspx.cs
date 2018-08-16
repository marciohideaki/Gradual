using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.PlanoCliente.Lib;
using Gradual.OMS.PoupeDirect.Lib;
using Gradual.OMS.PoupeDirect.Lib.Dados;
using Gradual.OMS.PoupeDirect.Lib.Mensagens;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Contratos.Dados.Fundos;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class DadosPlanosCliente : PaginaBaseAutenticada
    {
        #region | Atributos

        private static ListarProdutosClienteResponse gListaProdutosCliente = new ListarProdutosClienteResponse();

        #endregion

        #region | Propriedades

        private int GetCodBovespa
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["CodBovespa"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["Id"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetCpfCnpj
        {
            get
            {
                return this.Request.Form["DsCpfCnpj"].ToString().Replace(".", "").Replace("-", "").Replace("/", "");
            }
        }


        private int GetCodigoFundo
        {
            get
            {
                return this.Request.Form["CodigoFundo"].DBToInt32();
            }
        }

        private string GetNomeFundo
        {
            get
            {
                return this.Request.Form["NomeFundo"].DBToString();
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                     , "SalvarDadosProdutos"
                                                     , "AderirTermo"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados
                                                     , ResponderSalvarDadosProdutos
                                                     , ResponderAderirTermo
                                                     });
        }

        protected void rptCliente_Produtos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.Controls != null
            && (e.Item.Controls.Count > 1)
            && (e.Item.Controls[1] is HtmlGenericControl))
            {
                ((HtmlGenericControl)e.Item.Controls[1]).Attributes.Add("for", e.Item.Controls[3].ClientID);

                if (e.Item.DataItem is Gradual.OMS.PlanoCliente.Lib.ProdutoInfo)
                {
                    var lContemRegistro = gListaProdutosCliente.LstPlanoCliente.Find(pci => pci.IdProdutoPlano.Equals(((Gradual.OMS.PlanoCliente.Lib.ProdutoInfo)e.Item.DataItem).IdProduto));

                    if (lContemRegistro != null && e.Item.Controls[3] is HtmlInputCheckBox)
                        ((HtmlInputCheckBox)e.Item.Controls[3]).Checked = true;

                    if (null != lContemRegistro)
                    {
                        //this.txtDataAdesao
                        ((HtmlGenericControl)e.Item.Controls[5]).InnerText = lContemRegistro.DtOperacao.ToString("dd/MM/yyyy");
                        //this.txtDataCobranca
                        ((HtmlGenericControl)e.Item.Controls[7]).InnerText = lContemRegistro.DtUltimaCobranca.ToString("yyyyMMdd") != "00010101" ? lContemRegistro.DtUltimaCobranca.ToString("dd/MM/yyyy") : "N/A";
                        //this.txtValorCobrado
                        ((HtmlGenericControl)e.Item.Controls[9]).InnerText = lContemRegistro.VlCobrado.ToString("N2");
                        //this.txtDataInicio
                        ((HtmlGenericControl)e.Item.Controls[11]).InnerText = lContemRegistro.DtAdesao != null ? lContemRegistro.DtAdesao.Value.ToString("dd/MM/yyyy") : "N/A";
                        //this.txtDataFim
                        ((HtmlGenericControl)e.Item.Controls[13]).InnerText = lContemRegistro.DtFimAdesao.Value.ToString("yyyyMMdd") != "00010101" ? lContemRegistro.DtFimAdesao.Value.ToString("dd/MM/yyyy") : "N/A";
                    }
                }
            }
        }

        #endregion

        #region | Métodos

        private string ResponderAderirTermo()
        {
            string lRetorno = string.Empty;

            string lNomeFundo = this.GetNomeFundo;

            var lRequest = new Gradual.Intranet.Contratos.Mensagens.SalvarEntidadeCadastroRequest<FundosInfo>();

            lRequest.EntidadeCadastro = new FundosInfo();

            lRequest.EntidadeCadastro.CodigoClienteFundo = GetCodBovespa;
            lRequest.EntidadeCadastro.CodigoFundo        = GetCodigoFundo;
            lRequest.EntidadeCadastro.CodigoCotistaItau  = "0";
            lRequest.EntidadeCadastro.UsuarioLogado      = base.UsuarioLogado.EmailLogin;
            lRequest.EntidadeCadastro.Origem             = "Intranet";
            lRequest.EntidadeCadastro.CodigoUsuarioLogado = base.UsuarioLogado.Id;

            var lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<FundosInfo>(lRequest);

            lRetorno = RetornarSucessoAjax(string.Concat("Adesão do termo do fundo ", lNomeFundo, " aderido com sucesso!"));

            return lRetorno;

        }

        private string ResponderSalvarDadosProdutos()
        {
            IServicoPlanoCliente lServicoPlano = Ativador.Get<IServicoPlanoCliente>();

            string lObjetoJson = this.Request.Form["ObjetoJson"];

            string lRetorno = string.Empty;

            TransporteListaProdutos lProdutos = JsonConvert.DeserializeObject<TransporteListaProdutos>(lObjetoJson);

            List<PlanoClienteInfo> lEnvio = new List<PlanoClienteInfo>();

            if (null != lProdutos.Lista && lProdutos.Lista.Count > 0)
                lProdutos.Lista.ForEach(delegate(string item)
                {
                    lEnvio.Add(new PlanoClienteInfo()
                    {
                        IdProdutoPlano = int.Parse(item),
                        StSituacao = 'C',
                        CdCblc = lProdutos.CodBovespa,
                        DsCpfCnpj = lProdutos.DsCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""),
                        DtFimAdesao = DateTime.Now,
                    });
                });

            if (lEnvio.Count > 0)
            {
                var lRequest = new AtualizarProdutosClienteRequest();

                lRequest.LstPlanoCliente = lEnvio;

                AtualizarProdutosClienteResponse lResponse = lServicoPlano.AtualizarPlanoCliente(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogAlteracao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = lProdutos.CodBovespa });

                    //ListarProdutosResponse lProdutosAtualizado = lServicoPlano.ListarProdutos();

                    //rptCliente_Produtos.DataSource = lProdutosAtualizado.LstProdutos;

                    //rptCliente_Produtos.DataBind();
                }

                lRetorno = RetornarSucessoAjax(string.Concat("Plano do cliente ", lProdutos.CodBovespa, " atualizado com sucesso."));
            }

            return lRetorno;
        }

        private string ResponderCarregarHtmlComDados()
        {
            var list = new List<ItemSegurancaInfo>();
            var lItemSegurancaSalvar = new ItemSegurancaInfo();

            lItemSegurancaSalvar.Permissoes   = new List<string>() { "7BEAEF45-0927-485D-B56A-F539C20511B8" };
            lItemSegurancaSalvar.Tag          = "Salvar";
            lItemSegurancaSalvar.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;

            list.Add(lItemSegurancaSalvar);

            base.VerificaPermissoesPagina(list).ForEach(delegate(ItemSegurancaInfo item)
            {
                if ("Salvar".Equals(item.Tag))
                {
                    btnCliente_Produtos.Visible = item.Valido.Value;
                }
            });

            list.Clear();

            var lItemSegurancaSalvarTermo = new ItemSegurancaInfo();

            lItemSegurancaSalvarTermo.Permissoes = new List<string>() { "35AD4C06-32D9-4F05-848B-F010C40857D0" };
            lItemSegurancaSalvarTermo.Tag = "Salvar";
            lItemSegurancaSalvarTermo.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;

            list.Add(lItemSegurancaSalvarTermo);

            base.VerificaPermissoesPagina(list).ForEach(delegate(ItemSegurancaInfo item)
            {
                if ("Salvar".Equals(item.Tag))
                {
                    btnCliente_AderirTermo.Visible = item.Valido.Value;
                }
            });

            var lServicoPlano = Ativador.Get<IServicoPlanoCliente>();

            {   //--> Exibir as datas do plano
                gListaProdutosCliente = lServicoPlano.ListarProdutosCliente(new ListarProdutosClienteRequest()
                {
                    DsCpfCnpj = this.GetCpfCnpj
                });

                if (null != gListaProdutosCliente.LstPlanoCliente && gListaProdutosCliente.LstPlanoCliente.Count > 0)
                    gListaProdutosCliente.LstPlanoCliente = gListaProdutosCliente.LstPlanoCliente.FindAll(info => { return info.StSituacao != 'C'; });

                if (null != gListaProdutosCliente.LstPlanoCliente && gListaProdutosCliente.LstPlanoCliente.Count > 0)
                {
                    var lPlanoCliente = (PlanoClienteInfo)gListaProdutosCliente.LstPlanoCliente[0];

                    this.lblClienteProdutosDataDeAdesao.Text = lPlanoCliente.DtAdesao.Value.ToString("dd/MM/yyyy");
                    this.lblClienteProdutosUltimoVencimento.Text = lPlanoCliente.DtUltimaOperacao.ToString("dd/MM/yyyy");

                    this.divCliente_Produtos_ClienteDesde.Visible = true;
                }
                else
                {
                    this.divCliente_Produtos_ClienteDesde.Visible = false;
                }
            }

            {   //--> Exibir a lista de produtos cadastrados
                ListarProdutosResponse lProdutos = lServicoPlano.ListarProdutos();

                Logger.InfoFormat("DadosPlanoCliente.aspx: [{0}] produtos encontrados", lProdutos.LstProdutos.Count);

                this.rptCliente_Produtos.DataSource = lProdutos.LstProdutos;
                this.rptCliente_Produtos.DataBind();
            }

            this.CarregarDadosPoupe();

            this.CarregarDadosTermoFundos();

            return string.Empty;
        }

        private void CarregarDadosTermoFundos()
        {
            var lRequest = new Gradual.Intranet.Contratos.Mensagens.ConsultarEntidadeCadastroRequest<FundosInfo>();

            lRequest.EntidadeCadastro = new FundosInfo();

            lRequest.EntidadeCadastro.CodigoClienteFundo = this.GetCodBovespa;

            var lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<FundosInfo>(lRequest);

            this.rptTermoFundoAderido.DataSource = lResponse.Resultado;
            this.rptTermoFundoAderido.DataBind();


            var lResponseFundos = ClienteDbLib.ListarFundos();

            this.rptFundos.DataSource = lResponseFundos;
            this.rptFundos.DataBind();
        }

        private void CarregarDadosPoupe()
        {
            var lServico = Ativador.Get<IServicoPoupeDirect>();

            var lRetorno = lServico.SelecionarProdutoCliente(new ProdutoClienteRequest()
            {
                ProdutoCliente = new ProdutoClienteInfo()
                {
                    CodigoCliente = this.GetCodBovespa,
                }
            });

            if (lRetorno.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.rptCliente_ProdutoPoupeDirect.DataSource = new TransporteListaProdutoDirect().TraduzirLista(lRetorno.ListaProdutoCliente);
                this.rptCliente_ProdutoPoupeDirect.DataBind();
            }
        }

        #endregion
    }

    public class TransporteListaProdutos
    {
        public List<string> Lista { get; set; }

        public int CodBovespa { get; set; }

        public int GetCodCliente { get; set; }

        public string DsCpfCnpj { get; set; }
    }

    public class TransporteListaProdutoDirect
    {
        public string Ativo { get; set; }

        public string DsProduto { get; set; }

        public string IdProduto { get; set; }

        public string DataSolicitacao { get; set; }

        public string DataVencimento { get; set; }

        public string DataTrocaAtivo { get; set; }

        public List<TransporteListaProdutoDirect> TraduzirLista(List<ProdutoClienteInfo> pParametro)
        {
            var lRetorno = new List<TransporteListaProdutoDirect>();

            if (pParametro != null && pParametro.Count > 0)
            {
                pParametro.ForEach(pci =>
                {
                    lRetorno.Add(new TransporteListaProdutoDirect()
                    {
                        Ativo = pci.CodigoAtivo.DBToString().ToUpper(),
                        DataSolicitacao = pci.DataSolicitacao == null ? "N/A" : pci.DataSolicitacao.Value.ToString("dd/MM/yyyy"),
                        DataTrocaAtivo = pci.DataRetroTrocaPlano == null ? "N/A" : pci.DataRetroTrocaPlano.Value.ToString("dd/MM/yyyy"),
                        DataVencimento = pci.DataVencimento == null ? "N/A" : pci.DataVencimento.Value.ToString("dd/MM/yyyy"),
                        DsProduto = pci.DescricaProduto,
                        IdProduto = pci.CodigoProduto.DBToString(),
                    });
                });
            }

            return lRetorno;
        }
    }
}