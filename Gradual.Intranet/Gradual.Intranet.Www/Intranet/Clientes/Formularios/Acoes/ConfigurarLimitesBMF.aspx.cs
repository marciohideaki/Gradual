using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Newtonsoft.Json;
using Gradual.Intranet.Servicos.Mock;
using Gradual.OMS.Library;
using Gradual.Core.OMS.LimiteBMF;
using Gradual.Core.OMS.LimiteBMF.Lib;
using Gradual.Core.Ordens.Persistencia;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Core.OMS.LimiteManager;
using Gradual.Core.OMS.LimiteManager.Lib;
using Gradual.Core.OMS.LimiteManager.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Contratos.Mensagens;
namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class ConfigurarLimitesBMF : PaginaBaseAutenticada
    {
        #region Atributos
        private List<TransporteRelatorio_005> gDetalhesDoLimite = new List<TransporteRelatorio_005>();
        #endregion

        #region Propriedades
        private bool _PermissaoExcluir { get; set; }
        private string GetContrato
        {
            get
            {
                return this.Request.Form["Contrato"].ToString();
            }
        }
        private int GetCodBmf
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["CodBmf"], out lRetorno);

                return lRetorno;
            }
        }
        private string GetInstrumento
        {
            get
            {
                string lRetorno = this.Request.Form["instrumento"].ToString();

                return lRetorno;
            }
        }
        #endregion

        #region Eventos
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                     , "SalvarLimitesBmf"
                                                     , "CarregarDadosAtualizados"
                                                     , "CarregarLimiteBmfInstrumento"
                                                     , "InsereIntrumentoLimitesBmf"
                                                     , "RemoveIntrumentoLimitesBmf"
                                                     , "SelecionaIntrumentoLimitesBmf"
                                                     , "SelecionaContratoLimitesBmf"
                                                     , "AtualizarLimitesBmf"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderCarregarHtmlComDados
                                                     , this.ResponderSalvarLimitesBmf
                                                     , this.CarregarDadosAtualizados
                                                     , this.ResponderCarregarLimiteBmfInstrumento
                                                     , this.ResponderInsereIntrumentoLimitesBmf
                                                     , this.ResponderRemoveIntrumentoLimitesBmf
                                                     , this.ResponderSelecionaIntrumentoLimitesBmf
                                                     , this.ResponderSelecionaContratoLimitesBmf
                                                     , this.ResponderAtualizarLimitesBmf
                                                     });
        }
        #endregion
        
        #region Métodos
        private string ResponderAtualizarLimitesBmf()
        {
            var lServico = new ServicoLimiteBMF();

            ListarLimiteBMFRequest lRequest = new ListarLimiteBMFRequest();

            lRequest.Account = GetCodBmf;

            ListarLimiteBMFResponse lResponse = lServico.ObterLimiteBMFCliente(lRequest);

            Session["LimitesInstrumento_" + GetCodBmf] = Session["ListaLimitesInstrumentos_" + GetCodBmf] = null;
            Session["LimitesContrato_" + GetCodBmf] = Session["ListaLimites_" + GetCodBmf] = null;

            if (lResponse.ListaLimites != null && lResponse.ListaLimitesInstrumentos != null)
            {
                Session["LimitesContrato_" + GetCodBmf] = Session["ListaLimites_" + GetCodBmf] = lResponse.ListaLimites;
                Session["LimitesInstrumento_" + GetCodBmf] = Session["ListaLimitesInstrumentos_" + GetCodBmf] = lResponse.ListaLimitesInstrumentos;
            }

            return base.RetornarSucessoAjax("Dados de limites atualizados com sucesso.");
        }

        private string ResponderCarregarHtmlComDados()
        {
            List<ItemSegurancaInfo> list = new List<ItemSegurancaInfo>();
            ItemSegurancaInfo lItemSegurancaSalvar = new ItemSegurancaInfo();
            lItemSegurancaSalvar.Permissoes = new List<string>() { "9849F8B2-72BF-41B7-B61F-D1B0F6D8EF83" };
            lItemSegurancaSalvar.Tag = "Salvar";
            lItemSegurancaSalvar.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;
            list.Add(lItemSegurancaSalvar);

            base.VerificaPermissoesPagina(list).ForEach(delegate(ItemSegurancaInfo item)
            {
                if ("Salvar".Equals(item.Tag))
                {
                    btnClientes_Limites_BMF_Salvar.Visible = item.Valido.Value;
                }
            });

            var lServico = new ServicoLimiteBMF();

            ListarLimiteBMFRequest lRequest = new ListarLimiteBMFRequest();

            lRequest.Account = GetCodBmf;

            ListarLimiteBMFResponse lResponse = lServico.ObterLimiteBMFCliente(lRequest);

            Session["LimitesInstrumento_" + GetCodBmf] = Session["ListaLimitesInstrumentos_" + GetCodBmf] = null;
            Session["LimitesContrato_" + GetCodBmf]    = Session["ListaLimites_" + GetCodBmf]             = null;

            if (lResponse.ListaLimites != null && lResponse.ListaLimitesInstrumentos != null)
            {
                Session["LimitesContrato_" + GetCodBmf]    = Session["ListaLimites_" + GetCodBmf]             = lResponse.ListaLimites;
                Session["LimitesInstrumento_" + GetCodBmf] = Session["ListaLimitesInstrumentos_" + GetCodBmf] = lResponse.ListaLimitesInstrumentos;
            }

            List<ComboItemAux> lListaCommodities = new List<ComboItemAux>();
            
            var lRequestContrato = new ConsultarEntidadeCadastroRequest<ContratoBmfInfo>();

            lRequestContrato.EntidadeCadastro = new ContratoBmfInfo();

            var lResponseContrato = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ContratoBmfInfo>(lRequestContrato);

            lResponseContrato.Resultado.ForEach(contrato =>
            {
                lListaCommodities.Add(new ComboItemAux(contrato.CodigoContrato, contrato.DescricaoContrato));
            });
            /*
            lListaCommodities.Add(new ComboItemAux("DI1", "DI1.FUT"));
            lListaCommodities.Add(new ComboItemAux("DOL", "DOL.FUT"));
            lListaCommodities.Add(new ComboItemAux("IND", "IND.FUT"));
            lListaCommodities.Add(new ComboItemAux("WIN", "WIN.FUT"));
            lListaCommodities.Add(new ComboItemAux("WDL", "WDL.FUT"));
            lListaCommodities.Add(new ComboItemAux("WDO", "WDO.FUT"));
            lListaCommodities.Add(new ComboItemAux("BGI", "BGI.FUT"));
            lListaCommodities.Add(new ComboItemAux("WBG", "WBG.FUT"));
            lListaCommodities.Add(new ComboItemAux("EUR", "EUR.FUT"));
            lListaCommodities.Add(new ComboItemAux("WEU", "WEU.FUT"));
            lListaCommodities.Add(new ComboItemAux("ICF", "ICF.FUT"));
            lListaCommodities.Add(new ComboItemAux("WCF", "WCF.FUT"));
            lListaCommodities.Add(new ComboItemAux("ISU", "ISU.FUT"));
            lListaCommodities.Add(new ComboItemAux("ETH", "ETH.FUT"));
            lListaCommodities.Add(new ComboItemAux("ETN", "ETN.FUT"));
            lListaCommodities.Add(new ComboItemAux("CCM", "CCM.FUT"));
            lListaCommodities.Add(new ComboItemAux("SFI", "SFI.FUT"));
            lListaCommodities.Add(new ComboItemAux("OZ1", "OZ1.FUT"));
            lListaCommodities.Add(new ComboItemAux("DR1", "DR1.FUT"));
            lListaCommodities.Add(new ComboItemAux("IR1", "IR1.FUT"));
            lListaCommodities.Add(new ComboItemAux("BR1", "BR1.FUT"));
            lListaCommodities.Add(new ComboItemAux("CR1", "CR1.FUT"));
            lListaCommodities.Add(new ComboItemAux("MR1", "MR1.FUT"));
            lListaCommodities.Add(new ComboItemAux("SR1", "SR1.FUT"));
            lListaCommodities.Add(new ComboItemAux("ISP", "ISP.FUT"));
            */
            this.rptClientes_Contratos.DataSource = lListaCommodities;
            this.rptClientes_Contratos.DataBind();

            //this.CarregaInstrumentosLimiteBmf();

            //this.Response.Clear();

            return string.Empty; // só para obedecer assinatura
        }
        private string ResponderSelecionaContratoLimitesBmf()
        {
            string lRetorno = string.Empty;
            try
            {
                List<ClienteParametroLimiteBMFInfo> lListContrato = Session["ListaLimites_" + GetCodBmf] as List<ClienteParametroLimiteBMFInfo>;

                List<ClienteParametroBMFInstrumentoInfo> lListInstrumento = Session["ListaLimitesInstrumentos_" + GetCodBmf] as List<ClienteParametroBMFInstrumentoInfo>;

                lRetorno = base.RetornarSucessoAjax(new TransporteLimiteBMF().TraduzirParaTela(lListContrato, lListInstrumento, GetContrato), "Dados retornados com sucesso!");

            }
            catch (Exception ex)
            {
                RetornarErroAjax("Erro ao carregar limites bmf instrumento", ex);
            }

            return lRetorno;
        }

        private string ResponderSelecionaIntrumentoLimitesBmf()
        {
            string lRetorno = string.Empty;
            try
            {
                List<ClienteParametroLimiteBMFInfo> lListContrato = Session["ListaLimites_" + GetCodBmf] as List<ClienteParametroLimiteBMFInfo>;

                List<ClienteParametroBMFInstrumentoInfo> lListInstrumento = Session["ListaLimitesInstrumentos_" + GetCodBmf] as List<ClienteParametroBMFInstrumentoInfo>;

                lRetorno = base.RetornarSucessoAjax(new TransporteLimiteBMF().TraduzirInstrumentoParaTela(lListContrato, lListInstrumento, GetContrato, GetInstrumento), "Dados retornados com sucesso!"); 

            }
            catch (Exception ex)
            {
                RetornarErroAjax("Erro ao carregar limites bmf instrumento", ex);
            }

            return lRetorno;
        }

        private string ResponderRemoveIntrumentoLimitesBmf()
        {
            string lRetorno = string.Empty;

            try
            {
                List<ClienteParametroBMFInstrumentoInfo> listInstrumentos = Session["LimitesInstrumento_" + GetCodBmf] as List<ClienteParametroBMFInstrumentoInfo>;

                var lInstrumentoEncontrado = listInstrumentos.Find(instrumento => { return instrumento.Instrumento == GetInstrumento; });

                if (lInstrumentoEncontrado != null)
                {
                    listInstrumentos.Remove(lInstrumentoEncontrado);

                    lRetorno = RetornarSucessoAjax((object)GetInstrumento,"Instrumento removido com sucesso.");
                }
                else
                {
                    lRetorno = RetornarSucessoAjax("Erro ao Remover limites bmf instrumento");
                }
            }
            catch (Exception ex)
            {
                RetornarErroAjax("Erro ao Remover limites bmf instrumento", ex);
            }

            return lRetorno;
        }

        //private bool ValidaQuantidadeInstrumento(string pContrato, string pSentido, int quantidade)
        //{
        //    bool lRetorno = true;

        //    List<ClienteParametroLimiteBMFInfo> lListContrato = Session["LimitesContrato_" + GetCodBmf] as List<ClienteParametroLimiteBMFInfo>;

        //    var lContato = lListContrato.Find(contrato => { return contrato.Contrato == pContrato && contrato.Sentido == pSentido; });

        //    if ()

        //    return lRetorno;
        //}

        private string ResponderInsereIntrumentoLimitesBmf()
        {
            string lRetorno = string.Empty;

            try
            {
                ClienteParametroLimiteBMFInfo lContratoCompra = new ClienteParametroLimiteBMFInfo();
                ClienteParametroLimiteBMFInfo lContratoVenda = new ClienteParametroLimiteBMFInfo();

                if (VerificaContratoExiste(Request["lCliente_Contrato"].ToString(), GetCodBmf.ToString()))
                {
                    lContratoCompra.idClienteParametroBMF = GetIdClienteParametroBMF(Request["lCliente_Contrato"].ToString(),"C");
                    lContratoVenda.idClienteParametroBMF  = GetIdClienteParametroBMF(Request["lCliente_Contrato"].ToString(),"V");
                }

                lContratoVenda.Account                 = lContratoCompra.Account                = GetCodBmf;
                lContratoVenda.Contrato                = lContratoCompra.Contrato               = Request["lCliente_Contrato"];
                lContratoVenda.DataMovimento           = lContratoCompra.DataMovimento          = DateTime.Now;
                lContratoVenda.DataValidade            = lContratoCompra.DataValidade           = Convert.ToDateTime(Request["lCliente_Data_validade"]);
                lContratoVenda.QuantidadeMaximaOferta  = lContratoCompra.QuantidadeMaximaOferta = Convert.ToInt32(Request["lCliente_Qtde_Maxima_Contrato"]);
                lContratoCompra.QuantidadeTotal        = Convert.ToInt32(Request["lCliente_Qtde_Compra"]);
                lContratoVenda.QuantidadeTotal         = Convert.ToInt32(Request["lCliente_Qtde_Venda"]);
                lContratoCompra.QuantidadeDisponivel   = Convert.ToInt32(Request["lCliente_Qtde_Compra"]);
                lContratoVenda.QuantidadeDisponivel    = Convert.ToInt32(Request["lCliente_Qtde_Venda"]);
                
                lContratoCompra.Sentido                = "C";
                lContratoVenda.Sentido                 = "V";
                lContratoCompra.RenovacaoAutomatica    = 'S';
                lContratoVenda.RenovacaoAutomatica     = 'S';
                lContratoVenda.idClientePermissao      = base.UsuarioLogado.Id;
                lContratoCompra.idClientePermissao     = base.UsuarioLogado.Id;

                ClienteParametroBMFInstrumentoInfo lInstrumentoCompra = new ClienteParametroBMFInstrumentoInfo();
                ClienteParametroBMFInstrumentoInfo lInstrumentoVenda  = new ClienteParametroBMFInstrumentoInfo();
                
                if (VerificaInstrumentoExiste(Request["lCliente_Instrumento"].ToString().ToUpper(), GetCodBmf.ToString()))
                {
                    lInstrumentoCompra.IdClienteParametroInstrumento = GetIdClienteParametroInstrumentoBMF(Request["lCliente_Instrumento"].ToString().ToUpper(), "C");
                    lInstrumentoVenda.IdClienteParametroInstrumento  = GetIdClienteParametroInstrumentoBMF(Request["lCliente_Instrumento"].ToString().ToUpper(), "V");
                }

                lInstrumentoVenda.Instrumento            = lInstrumentoCompra.Instrumento            = Request["lCliente_Instrumento"].ToString().ToUpper();
                lInstrumentoVenda.QuantidadeMaximaOferta = lInstrumentoCompra.QuantidadeMaximaOferta = Convert.ToInt32(Request["lCliente_Qtde_Maxima_Instrumento"]);
                lInstrumentoVenda.dtMovimento            = lInstrumentoCompra.dtMovimento            = DateTime.Now;

                lInstrumentoVenda.Sentido             = 'V';
                lInstrumentoCompra.Sentido            = 'C';
                lInstrumentoCompra.QtTotalContratoPai = Convert.ToInt32(Request["lCliente_Qtde_Compra"]);
                lInstrumentoVenda.QtTotalContratoPai  = Convert.ToInt32(Request["lCliente_Qtde_Venda"]);
                lInstrumentoCompra.QtTotalInstrumento = Convert.ToInt32(Request["lCliente_Qtde_Compra_Instrumento"]);
                lInstrumentoVenda.QtTotalInstrumento  = Convert.ToInt32(Request["lCliente_Qtde_Venda_Instrumento"]);
                lInstrumentoCompra.QtDisponivel       = Convert.ToInt32(Request["lCliente_Qtde_Compra_Instrumento"]);
                lInstrumentoVenda.QtDisponivel        = Convert.ToInt32(Request["lCliente_Qtde_Venda_Instrumento"]);

                var listContrato = Session["LimitesContrato_" + GetCodBmf] as List<ClienteParametroLimiteBMFInfo>;
                var listInstrumento = Session["LimitesInstrumento_" + GetCodBmf] as List<ClienteParametroBMFInstrumentoInfo>;

                listContrato.Add(lContratoCompra);
                listContrato.Add(lContratoVenda);

                listInstrumento.Add(lInstrumentoCompra);
                listInstrumento.Add(lInstrumentoVenda);

                Session["LimitesContrato_" + GetCodBmf]    = listContrato;
                Session["LimitesInstrumento_" + GetCodBmf] = listInstrumento;

                lRetorno = base.RetornarSucessoAjax(new TransporteLimiteBMF().TraduzirInstrumentoNovoParaTela(listContrato,listInstrumento, Request["lCliente_Contrato"].ToString()), "Limite inserido com sucesso");
            }
            catch (Exception ex)
            {
                RetornarErroAjax("Erro ao Inserir limites bmf instrumento", ex);
            }

            return lRetorno;
        }

        private int GetIdClienteParametroInstrumentoBMF(string pInstrumento, string pSentido)
        {
            int lRetorno = 0;

            List<ClienteParametroBMFInstrumentoInfo> lListInstrumento = Session["LimitesInstrumento_" + GetCodBmf] as List<ClienteParametroBMFInstrumentoInfo>;

            var lInstrumento = lListInstrumento.Find(instrumento => { return instrumento.Instrumento == pInstrumento && instrumento.Sentido.ToString() == pSentido; });

            lRetorno = lInstrumento.IdClienteParametroInstrumento;

            return lRetorno;
        }

        private bool VerificaInstrumentoExiste(string pInstrumento, string CodigoBMF)
        {
            List<ClienteParametroBMFInstrumentoInfo> lListInstrumento = Session["LimitesInstrumento_" + GetCodBmf] as List<ClienteParametroBMFInstrumentoInfo>;

            var lInstrumento = lListInstrumento.Find(instrumento => { return instrumento.Instrumento == pInstrumento; });

            if (lInstrumento == null) return false; else return true;
        }

        private int GetIdClienteParametroBMF(string pContrato, string pSentido)
        {
            int lRetorno = 0;

            List<ClienteParametroLimiteBMFInfo> lListContrato = Session["LimitesContrato_" + GetCodBmf] as List<ClienteParametroLimiteBMFInfo>;

            var lContato = lListContrato.Find(contrato => { return contrato.Contrato == pContrato && contrato.Sentido == pSentido; });

            lRetorno = lContato.idClienteParametroBMF;

            return lRetorno;
        }

        private bool VerificaContratoExiste(string Contrato, string CodigoBMF)
        {
            List<ClienteParametroLimiteBMFInfo> lListContrato = Session["LimitesContrato_" + GetCodBmf] as List<ClienteParametroLimiteBMFInfo>;

            var lContato = lListContrato.Find(contrato=> { return contrato.Contrato == Contrato;});

            if (lContato == null) return false; else return true;
        }

        private string ResponderCarregarLimiteBmfInstrumento()
        {
            string lRetorno = string.Empty;

            try
            {
                

            }
            catch (Exception ex)
            {
                RetornarErroAjax("Erro ao carregar limites bmf instrumento", ex);
            }

            return lRetorno;
        }
        private class ContratoAuxIntrumento
        {
            public int IdClienteParametroBMF { get; set; }
            public string Contrato { get; set; }
            public string Sentido { get; set; }

        }

        private string ResponderSalvarLimitesBmf()
        {
            string lRetorno = string.Empty;

            try
            {
                var lServico = new ServicoLimiteBMF();

                var listAntigaInstrumentos = Session["ListaLimitesInstrumentos_" + GetCodBmf] as List<ClienteParametroBMFInstrumentoInfo>;
                var listAntigaContratos    = Session["ListaLimites_" + GetCodBmf]             as List<ClienteParametroLimiteBMFInfo>;

                string lAntigoStatusContratos = RetornarSucessoAjax(listAntigaContratos, "");
                string lAntigoStatusInstrumentos = RetornarSucessoAjax(listAntigaInstrumentos, "");

                List<ClienteParametroLimiteBMFInfo> lListContrato         = Session["LimitesContrato_" + GetCodBmf]    as List<ClienteParametroLimiteBMFInfo>;
                List<ClienteParametroBMFInstrumentoInfo> lListInstrumento = Session["LimitesInstrumento_" + GetCodBmf] as List<ClienteParametroBMFInstrumentoInfo>;

                InserirLimiteClienteBMFRequest lRequestContrato        = new InserirLimiteClienteBMFRequest();
                InserirLimiteBMFInstrumentoRequest lRequestInstrumento = new InserirLimiteBMFInstrumentoRequest();

                List<ContratoAuxIntrumento> llistAuxContrato = new List<ContratoAuxIntrumento>();

                //Atualiza os dados para salvar a quantidade 
                var lInstrumentoEncontradoCompra = lListInstrumento.Find(instru => { return instru.Sentido == 'C' && instru.Instrumento == GetInstrumento; });
                var lInstrumentoEncontradoVenda = lListInstrumento.Find(instru => { return instru.Sentido == 'V' && instru.Instrumento == GetInstrumento; });

                lListInstrumento.Remove(lInstrumentoEncontradoCompra);
                lListInstrumento.Remove(lInstrumentoEncontradoVenda);

                lInstrumentoEncontradoCompra.QtDisponivel = Convert.ToInt32(Request["lCliente_Qtde_Compra_Instrumento"].ToString());
                lInstrumentoEncontradoCompra.QtTotalInstrumento = Convert.ToInt32(Request["lCliente_Qtde_Compra_Instrumento"].ToString());

                lInstrumentoEncontradoVenda.QtDisponivel = Convert.ToInt32(Request["lCliente_Qtde_Venda_Instrumento"].ToString());
                lInstrumentoEncontradoVenda.QtTotalInstrumento = Convert.ToInt32(Request["lCliente_Qtde_Venda_Instrumento"].ToString());

                lInstrumentoEncontradoCompra.QuantidadeMaximaOferta = Convert.ToInt32(Request["lCliente_Qtde_Maxima_Instrumento"].ToString());
                lInstrumentoEncontradoVenda.QuantidadeMaximaOferta  = Convert.ToInt32(Request["lCliente_Qtde_Maxima_Instrumento"].ToString());

                lListInstrumento.Add(lInstrumentoEncontradoCompra);
                lListInstrumento.Add(lInstrumentoEncontradoVenda);

                var lContratoEncontrados = lListContrato.FindAll(contrato => { return contrato.Contrato == GetContrato; });

                foreach (ClienteParametroLimiteBMFInfo info in lContratoEncontrados)
                {
                    lListContrato.Remove(info);
                    
                    ClienteParametroLimiteBMFInfo infoAux = info;

                    infoAux.QuantidadeTotal = infoAux.QuantidadeDisponivel = (info.Sentido == "C") ? Convert.ToInt32(Request["lCliente_Qtde_Compra"].ToString()) : Convert.ToInt32(Request["lCliente_Qtde_Venda"].ToString());
                    infoAux.QuantidadeMaximaOferta = Convert.ToInt32(Request["lCliente_Qtde_Maxima_Contrato"].ToString());
                    lListContrato.Add(infoAux);
                }

                string lNovoStatusContratos    = RetornarSucessoAjax(lListContrato, "");
                string lNovoStatusInstrumentos = RetornarSucessoAjax(lListInstrumento, "");

                foreach (ClienteParametroLimiteBMFInfo info in lListContrato)
                {
                    lRequestContrato.ClienteParametroLimiteBMFInfo = info;

                    InserirLimiteClienteBMFResponse lResponseContrato =  lServico.AtualizarLimiteBMF(lRequestContrato);

                    ContratoAuxIntrumento lAux = new ContratoAuxIntrumento();

                    lAux.Contrato = info.Contrato;

                    lAux.IdClienteParametroBMF = lResponseContrato.IdClienteParametroBMF == 0 ? info.idClienteParametroBMF : lResponseContrato.IdClienteParametroBMF;

                    lAux.Sentido = info.Sentido;

                    llistAuxContrato.Add(lAux);
                }

                foreach (ClienteParametroBMFInstrumentoInfo info in lListInstrumento)
                {
                    lRequestInstrumento.LimiteBMFInstrumento = info;

                    var lAux = llistAuxContrato.Find(aux => { return aux.Sentido == info.Sentido.ToString() && aux.Contrato == info.Instrumento.Substring(0,3); });
                    
                    if (lAux != null)
                    {
                        lRequestInstrumento.LimiteBMFInstrumento.IdClienteParametroBMF = lAux.IdClienteParametroBMF;
                    }

                    lServico.AtualizarLimiteInstrumentoBMF(lRequestInstrumento);
                }

                base.RegistrarLogInclusao(new Contratos.Dados.Cadastro.LogIntranetInfo()
                {   //--> Registrando o Log.
                    CdBovespaClienteAfetado = this.GetCodBmf,
                    DsObservacao = string.Concat("Contratos Antigos: ", lAntigoStatusContratos, " Instrumentos Antigos: ", lAntigoStatusInstrumentos) +
                                   string.Concat(" Contratos Novos: "  , lNovoStatusContratos   , " Instrumentos Novos: ", lNovoStatusInstrumentos)
                });


                ListarLimiteBMFRequest lRequest = new ListarLimiteBMFRequest();

                lRequest.Account = GetCodBmf;

                ListarLimiteBMFResponse lResponse = lServico.ObterLimiteBMFCliente(lRequest);

                Session["LimitesInstrumento_" + GetCodBmf] = Session["ListaLimitesInstrumentos_" + GetCodBmf] = null;
                Session["LimitesContrato_" + GetCodBmf] = Session["ListaLimites_" + GetCodBmf] = null;

                if (lResponse.ListaLimites != null && lResponse.ListaLimitesInstrumentos != null)
                {
                    Session["LimitesContrato_" + GetCodBmf] = Session["ListaLimites_" + GetCodBmf] = lResponse.ListaLimites;
                    Session["LimitesInstrumento_" + GetCodBmf] = Session["ListaLimitesInstrumentos_" + GetCodBmf] = lResponse.ListaLimitesInstrumentos;
                }
                /*
                ILimiteManager lmtMng     = Ativador.Get<ILimiteManager>();
                ReloadLimitsResponse resp = new ReloadLimitsResponse();
                ReloadLimitsRequest req   = new ReloadLimitsRequest();
                req.ReloadSecurityList    = false; // true: para forçar o recarregamento de cadastro de papeis / false para carregar somente parâmetros e limites (mais rápido)
                lmtMng.ReloadLimitStructures(req);
                */
                lRetorno = RetornarSucessoAjax("Limites BMF configurados com sucesso.");
            }
            catch (Exception ex)
            {
                RetornarErroAjax("Erro ao Salvar limites BMF", ex);
            }

            return lRetorno;
        }

        public bool PermissaoExcluir()
        {
            return _PermissaoExcluir;
        }

        private string CarregarDadosAtualizados()
        {
            //this.CarregarDadosViaServico();

            return string.Empty; // base.RetornarSucessoAjax(new TransporteConfigurarLimites().TraduzirListaLimites(this.gRetornoLimitePorCliente.ParametrosRiscoCliente), "Dados atualizados com sucesso.");
        }

        

        private void CarregaInstrumentosLimiteBmf()
        {
            try
            {
                List<int> lIdsContratosEncontrados = new List<int>();

                List<ClienteParametroLimiteBMFInfo> lListContrato = Session["ListaLimites_" + GetCodBmf] as List<ClienteParametroLimiteBMFInfo>;

                List<ClienteParametroLimiteBMFInfo> lListContratoEncontrato = lListContrato.FindAll(contrato => { return contrato.Contrato == GetContrato; });

                lListContratoEncontrato.ForEach(contrato =>
                {
                    lIdsContratosEncontrados.Add(contrato.idClienteParametroBMF);
                });

                List<ClienteParametroBMFInstrumentoInfo> lListInstrumento = Session["ListaLimitesInstrumentos_" + GetCodBmf] as List<ClienteParametroBMFInstrumentoInfo>;

                IEnumerable<ClienteParametroBMFInstrumentoInfo> lListInstrumentoEncontratos = from a in lListInstrumento where lIdsContratosEncontrados.Contains(a.IdClienteParametroBMF) select a;

                List<ComboItemAux> lListaInstrumentos = new List<ComboItemAux>();

                List<string> lTempInstrumentos = new List<string>();

                lListInstrumentoEncontratos.ToList().ForEach(instrumento =>
                {
                    lListaInstrumentos.Add(new ComboItemAux(instrumento.Instrumento, instrumento.Instrumento +  " " + instrumento.QtTotalContratoPai.ToString() + instrumento.Sentido + " " + instrumento.QuantidadeMaximaOferta ));
                });

                var lContratoCompra = lListContrato.Find(contrato=> { return contrato.Sentido == "C"; });

                var lContratoVenda = lListContrato.Find(contrato => {return contrato.Sentido == "V"; });

                this.txtCliente_Qtde_Compra.Value = lContratoCompra.QuantidadeDisponivel.ToString();

                this.txtCliente_Qtde_Venda.Value = lContratoVenda.QuantidadeDisponivel.ToString();

                this.txtCliente_Qtde_Maxima_Ordem_Contrato.Value = lContratoCompra.QuantidadeMaximaOferta.ToString();

                this.txtCliente_Data_validade.Value = lContratoCompra.DataValidade.ToString("dd/MM/yyyy");


                //rptClientes_Instrumentos_Selecionadados.DataSource = lListaInstrumentos;
                //rptClientes_Instrumentos_Selecionadados.DataBind();

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }



    public class ComboItemAux
    {
        #region Prorpiedades
        public string Id { get; set; }
        public string Descricao { get; set; }
        #endregion

        #region Construtores
        public ComboItemAux()
        {

        }

        public ComboItemAux(string _Id, string _Descricao )
        {
            this.Id = _Id;
            this.Descricao = _Descricao;
        }
        #endregion
    }
}