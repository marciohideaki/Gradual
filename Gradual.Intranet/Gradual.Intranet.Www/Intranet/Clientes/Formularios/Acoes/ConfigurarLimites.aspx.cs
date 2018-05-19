using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class ConfigurarLimites : PaginaBaseAutenticada
    { 
        #region | Atributos

        private ListarPermissoesRiscoClienteResponse gRetornoPermissoesRiscoCliente;

        private ListarBloqueiroInstrumentoResponse gRetornoBloqueioPorCliente;

        private ListarParametrosRiscoClienteResponse gRetornoLimitePorCliente;

        private ListarPermissoesRiscoResponse gRetornoParametrosRisco;

        private ListarClienteParametroGrupoResponse gRetornoClienteParametroGrupo;

        private List<TransporteRelatorio_005> gDetalhesDoLimite;

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

        private bool _PermissaoExcluir { get; set; }

        public int GetIdParametroLimiteDescobertoMercadoAVista { get { return 5; } }

        public int GetIdParametroLimiteDescobertoMercadoOpcoes { get { return 7; } }

        public int GetIdParametroLimiteMaximoDaOrdem { get { return 8; } }

        public int GetIdParametroLimiteCompraMercadoAVista { get { return 12; } }

        public int GetIdParametroLimiteCompraMercadoOpcoes { get { return 13; } }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            this.pnlLinksParametroLimiteOperaAVista.Visible
                = this.pnlLinksParametroLimiteOperaAVistaDescoberto.Visible
                = this.pnlLinksParametroLimiteOperaOpcao.Visible
                = this.pnlLinksParametroLimiteOperaOpcaoDescoberto.Visible
                = this.pnlLinksParametroLimiteValorMaximoDaOrdem.Visible
                = base.UsuarioPode("Salvar", "54f77b3b-ac85-42be-b5d9-92a4fa03b056");

            base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                     , "SalvarLimitesBovespa"
                                                     , "SalvarDadosPermissoes"
                                                     , "AtualizarLimitesBloqueados"
                                                     , "CarregarDadosAtualizados"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderCarregarHtmlComDados
                                                     , this.ResponderSalvarLimitesBovespa
                                                     , this.ResponderSalvarDadosPermissoes
                                                     , this.ResponderCarregarLimitesBloqueados
                                                     , this.CarregarDadosAtualizados
                                                     });
        }

        protected void rptCliente_Permissoes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (null != gRetornoPermissoesRiscoCliente)
            {
                var lContemRegistro = gRetornoPermissoesRiscoCliente.PermissoesAssociadas.Find(
                    delegate(PermissaoRiscoAssociadaInfo pri)
                    {
                        return pri.PermissaoRisco.CodigoPermissao == ((TransporteConfigurarLimites)(e.Item.DataItem)).CodigoPermissao.DBToInt32();
                    });

                if (e.Item.Controls != null && e.Item.Controls.Count > 1)
                {
                    if (lContemRegistro != null && e.Item.Controls[3] is HtmlInputCheckBox)
                        ((HtmlInputCheckBox)e.Item.Controls[3]).Checked = true;

                    if (e.Item.Controls[1] is HtmlGenericControl)
                    {
                        ((HtmlGenericControl)e.Item.Controls[1]).Attributes.Add("for", e.Item.Controls[3].ClientID);
                        ((HtmlGenericControl)e.Item.Controls[1]).Attributes.Add("CodigoPermissao", ((TransporteConfigurarLimites)(e.Item.DataItem)).CodigoPermissao);
                    }
                }
            }
        }

        protected void rptCliente_ResticoesPorGrupo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var lRadioButton = new HtmlInputRadioButton();
            var lCodigoGrupo = default(int);
            var lIdParametro = default(int);
            var lGrupoCadastrado = new ClienteParametroGrupoInfo();

            foreach (Control controle in e.Item.Controls)
            {
                if (controle is HtmlInputRadioButton)
                    lRadioButton = (HtmlInputRadioButton)controle;

                if (controle is HtmlInputHidden)
                    lIdParametro = ((HtmlInputHidden)controle).Value.DBToInt32();

                if (controle is HtmlGenericControl)
                {
                    lCodigoGrupo = ((GrupoInfo)(e.Item.DataItem)).CodigoGrupo;

                    if (null != this.gRetornoClienteParametroGrupo)
                        lGrupoCadastrado = this.gRetornoClienteParametroGrupo.ListaObjeto.Find(delegate(ClienteParametroGrupoInfo cpg) { return cpg.IdGrupo == lCodigoGrupo && cpg.IdParametro == lIdParametro; });

                    if (null != lGrupoCadastrado && lRadioButton.ID.Contains("rdbResticaoPorGrupoRestringe"))
                    {
                        lRadioButton.Checked = true;
                    }

                    //--> Setando o relacionamento do atributo 'for' à label do Checkbox.
                    ((HtmlGenericControl)controle).Attributes.Add("for", lRadioButton.ClientID);
                }
            }
        }

        #endregion

        #region | Métodos

        public bool PermissaoExcluir()
        {
            return _PermissaoExcluir;
        }

        private string ResponderCarregarLimitesBloqueados()
        {
            this.CarregarDadosViaServico();

            return RetornarSucessoAjax(this.gDetalhesDoLimite, "Saldo bloqueado carregado com sucesso!");
        }

        private string CarregarDadosAtualizados()
        {
            this.CarregarDadosViaServico();

            return base.RetornarSucessoAjax(new TransporteConfigurarLimites().TraduzirListaLimites(this.gRetornoLimitePorCliente.ParametrosRiscoCliente), "Dados atualizados com sucesso.");
        }

        private string ResponderCarregarHtmlComDados()
        {
            List<ItemSegurancaInfo> list = new List<ItemSegurancaInfo>();
            ItemSegurancaInfo lItemSegurancaSalvar = new ItemSegurancaInfo();
            lItemSegurancaSalvar.Permissoes = new List<string>() { "54f77b3b-ac85-42be-b5d9-92a4fa03b056" };
            lItemSegurancaSalvar.Tag = "Salvar";
            lItemSegurancaSalvar.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;
            list.Add(lItemSegurancaSalvar);

            base.VerificaPermissoesPagina(list).ForEach(delegate(ItemSegurancaInfo item)
            {
                if ("Salvar".Equals(item.Tag))
                {
                    btnClientes_Limites_Bovespa.Visible = item.Valido.Value;
                    btnCliente_Restricoes.Visible = item.Valido.Value;
                    btnGradIntra_Clientes_Restricoes_RestricaoPorAtivos_Add.Visible = item.Valido.Value;
                    btnGradIntra_Clientes_Restricoes_RestricaoPorAtivos_Remover_click.Visible = item.Valido.Value;
                    _PermissaoExcluir = item.Valido.Value;
                }
            });

            this.CarregarDadosViaServico();

            this.ConfigurarLimitesNaTela(new TransporteConfigurarLimites().TraduzirListaLimites(this.gRetornoLimitePorCliente.ParametrosRiscoCliente));

            this.ConfigurarLimitesBloqueadosNaTela(this.gDetalhesDoLimite);

            this.ConfigurarPermissoesNaTela(new TransporteConfigurarLimites().TraduzirListaPermissoes(this.gRetornoParametrosRisco.Permissoes));

            this.ConfigurarRestricoesNaTela();

            this.Response.Clear();

            return string.Empty; // só para obedecer assinatura
        }

        private string ResponderSalvarLimitesBovespa()
        {
            string lRetorno = string.Empty;

            TransporteLimiteBovespa lLimites;

            try
            {
                string lObjetoJson = this.Request.Form["ObjetoJson"];

                lLimites = JsonConvert.DeserializeObject<TransporteLimiteBovespa>(lObjetoJson);

                this.SalvarExpirarParametrosRisco(lLimites);
                this.SalvarIncluirRenovarParametroRisco(lLimites);
                this.SalvarPermissoes(lLimites);

                SalvarFatFingerClienteRequest lRequest = new SalvarFatFingerClienteRequest();
                
                lRequest.FatFinger                     = new FatFingerClienteInfo();

                if (!string.IsNullOrEmpty(lLimites.VencimentoMaximoDaOrdem))
                {
                    lRequest.FatFinger.CodigoCliente = lLimites.CodBovespa;
                    lRequest.FatFinger.DataVencimento = Convert.ToDateTime(lLimites.VencimentoMaximoDaOrdem);
                    lRequest.FatFinger.ValorMaximo = lLimites.ValorMaximoDaOrdem;

                    var lResponse = new ServicoRegrasRisco().SalvarFatFingerCliente(lRequest);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        string lRetornoFatFinger = RetornarSucessoAjax(lResponse, "Fat Finger incluído com sucesso");

                        base.RegistrarLogInclusao(new Contratos.Dados.Cadastro.LogIntranetInfo()
                        {
                            CdBovespaClienteAfetado = lLimites.CodBovespa,
                            DsObservacao = string.Concat("Alteração de Fat Finger : Log = ", lRetornoFatFinger)
                        });
                    }
                }
                else
                {
                    RemoverFatFingerClienteRequest lRequestRemoverFat = new RemoverFatFingerClienteRequest();

                    lRequestRemoverFat.FatFinger = new FatFingerClienteInfo();

                    lRequestRemoverFat.FatFinger.CodigoCliente = lLimites.CodBovespa;

                    var lResponseRemoverFat = new ServicoRegrasRisco().RemoverFatFingerCliente(lRequestRemoverFat);

                    if (lResponseRemoverFat.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        string lRetornoFatFinger = RetornarSucessoAjax(lResponseRemoverFat, "Fat Finger incluído com sucesso");

                        base.RegistrarLogExclusao(lRetornoFatFinger);
                    }
                }

                base.RegistrarLogInclusao(new Contratos.Dados.Cadastro.LogIntranetInfo() 
                {   //--> Registrando o Log.
                    CdBovespaClienteAfetado = lLimites.CodBovespa, 
                    DsObservacao = string.Concat("Inclusão de limite para o cliente: cd_codigo = ", lLimites.CodBovespa.ToString()) 
                }); 

                lRetorno = RetornarSucessoAjax("Limites Bovespa configurados com sucesso.");
            }
            catch (Exception ex)
            {
                RetornarErroAjax("Erro ao Salvar limites", ex);
            }

            return lRetorno;
        }

        private string ResponderSalvarDadosPermissoes()
        {
            try
            {
                var lCdCodigo = default(int);

                {   //--> Salvando as restrições por Grupo.
                    var lClienteParametroGrupo = JsonConvert.DeserializeObject<TransporteClienteParametroGrupo[]>(this.Request.Form["ObjetoJSonGrupos"]);
                    this.SalvarRestricoesPorGrupo(lClienteParametroGrupo);
                    lCdCodigo = lClienteParametroGrupo.Length > 0 ? lClienteParametroGrupo[0].IdCliente : default(int);
                }

                {   //--> Salvando as restrições por Ativo.
                    var lClienteParametroAtivo = JsonConvert.DeserializeObject<TransporteBloqueioInstrumento>(this.Request.Form["ObjetoJSonAtivos"]);
                    this.SalvarRestricoesPorAtivo(lClienteParametroAtivo);
                }

                base.RegistrarLogInclusao(new Contratos.Dados.Cadastro.LogIntranetInfo() 
                {   //--> Registrando o Log.
                    IdClienteAfetado = lCdCodigo,
                    DsObservacao = string.Concat("Inclusão de Restrição por Grupo e Ativo para o cliente: id_cliente = ", lCdCodigo.ToString())
                });

                return base.RetornarSucessoAjax("Restrições salvas com sucesso!");
            }
            catch (Exception ex)
            {
                return base.RetornarErroAjax(ex.ToString());
            }
        }

        #endregion

        #region | Métodos de apoio

        private void CarregarDadosViaServico()
        {
            {   //--> Definindo a aprensentação dos limites.
                this.gRetornoLimitePorCliente = new ServicoRegrasRisco().ListarLimitePorCliente(
                    new ListarParametrosRiscoClienteRequest
                    {
                        CodigoCliente = this.GetCodBovespa,
                    });

                var lConsultaSaldo =
                    base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoLimiteAlocadoInfo>(
                        new ConsultarEntidadeCadastroRequest<RiscoLimiteAlocadoInfo>(
                            new RiscoLimiteAlocadoInfo()
                            {
                                ConsultaIdCliente = this.GetCodBovespa 
                            }));

                this.gDetalhesDoLimite = new TransporteRelatorio_005().TraduzirListaSaldo(lConsultaSaldo.Resultado);
            }

            {   //--> Definindo a apresenteção das permissões.
                this.gRetornoParametrosRisco = new ServicoRegrasRisco().ListarPermissoesRisco(new ListarPermissoesRiscoRequest());

                this.gRetornoPermissoesRiscoCliente = new ServicoRegrasRisco().ListarPermissoesRiscoCliente(new ListarPermissoesRiscoClienteRequest() { CodigoCliente = this.GetCodBovespa });
            }

            {   //--> Definindo a carga de dados dos Grupos de Risco do cliente.
                this.gRetornoBloqueioPorCliente = new ServicoRegrasRisco().ListarBloqueioPorCliente(
                    new ListarBloqueiroInstrumentoRequest()
                    {
                        Objeto = new BloqueioInstrumentoInfo()
                        {
                            IdCliente = this.GetCodBovespa
                        }
                    });
            }

            {   //--> Definindo carga de dados dos grupos por cliente e parâmetro.
                this.gRetornoClienteParametroGrupo = new ServicoRegrasRisco().ListarClienteParametroGrupo(
                    new ListarClienteParametroGrupoRequest()
                    {
                        Objeto = new ClienteParametroGrupoInfo()
                        {
                            IdCliente = this.GetCodBovespa,
                        }
                    });
            }

            {   //--> Definindo os dados de segurança
               this.btnCliente_Restricoes.Visible =
               this.btnClientes_Limites_Bovespa.Visible = base.UsuarioPode("Salvar", "54f77b3b-ac85-42be-b5d9-92a4fa03b056");
            }
        }

        private void ConfigurarLimitesNaTela(List<TransporteConfigurarLimites> pParametrosLimites)
        {
            pParametrosLimites.ForEach(delegate(TransporteConfigurarLimites tcl)
            {
                switch (tcl.TipoLimite)
                {
                    case Gradual.Intranet.Contratos.Dados.Enumeradores.eTipoLimite.OperarCompraAVista:
                        this.hddCliente_Limites_Bosvespa_AVista_DataDeVencimento.Value = DateTime.Today.ToString("dd/MM/yyyy");
                        this.txtCliente_Limites_Bosvespa_AVista_DataDeVencimento.Value = tcl.Vencimento;
                        this.hddCliente_Limites_Bosvespa_AVista_Limite.Value =
                        this.txtCliente_Limites_Bosvespa_AVista_Limite.Value = tcl.Limite;
                        this.chkCliente_Limites_Bosvespa_AVista_Opera.Checked = tcl.StAtivo;
                        break;
                    case Gradual.Intranet.Contratos.Dados.Enumeradores.eTipoLimite.OperarCompraOpcao:
                        this.hddCliente_Limites_Bovespa_Opcao_DataDeVencimento.Value = DateTime.Today.ToString("dd/MM/yyyy");
                        this.txtCliente_Limites_Bovespa_Opcao_DataDeVencimento.Value = tcl.Vencimento;
                        this.hddCliente_Limites_Bovespa_Opcao_Limite.Value =
                        this.txtCliente_Limites_Bovespa_Opcao_Limite.Value = tcl.Limite;
                        this.chkCliente_Limites_Bovespa_Opcao_Opera.Checked = tcl.StAtivo;
                        break;
                    case Gradual.Intranet.Contratos.Dados.Enumeradores.eTipoLimite.OperarDescobertoAvista:
                        this.hddCliente_Limites_Bosvespa_AVista_DataDeVencimentoDescoberto.Value = DateTime.Today.ToString("dd/MM/yyyy");
                        this.txtCliente_Limites_Bosvespa_AVista_DataDeVencimentoDescoberto.Value = tcl.Vencimento;
                        this.hddCliente_Limites_Bosvespa_AVista_LimiteDescoberto.Value =
                        this.txtCliente_Limites_Bosvespa_AVista_LimiteDescoberto.Value = tcl.Limite;
                        this.chkCliente_Limites_Bosvespa_AVista_OperaDescoberto.Checked = tcl.StAtivo;
                        break;
                    case Gradual.Intranet.Contratos.Dados.Enumeradores.eTipoLimite.OperarDescobertoOpcao:
                        this.hddCliente_Limites_Bovespa_Opcao_DataDeVencimentoDescoberto.Value = DateTime.Today.ToString("dd/MM/yyyy");
                        this.txtCliente_Limites_Bovespa_Opcao_DataDeVencimentoDescoberto.Value = tcl.Vencimento;
                        this.hddCliente_Limites_Bovespa_Opcao_LimiteDescoberto.Value =
                        this.txtCliente_Limites_Bovespa_Opcao_LimiteDescoberto.Value = tcl.Limite;
                        this.chkCliente_Limites_Bovespa_Opcao_OperaDescoberto.Checked = tcl.StAtivo;
                        break;
                    case Gradual.Intranet.Contratos.Dados.Enumeradores.eTipoLimite.ValorMaximoOrdem:
                        this.hddCliente_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento.Value = DateTime.Today.ToString("dd/MM/yyyy");
                        this.txtCliente_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento.Value = tcl.Vencimento;
                        this.hddCliente_Limites_Bovespa_ValorMaximoDaOrdem.Value =
                        this.txtCliente_Limites_Bovespa_ValorMaximoDaOrdem.Value = tcl.Limite;
                        this.chkCliente_Limites_Bovespa_ValorMaximoDaOrdem.Checked = tcl.StAtivo;
                        break;
                }
            });

            ReceberFatFingerClienteRequest lRequest = new ReceberFatFingerClienteRequest();

            lRequest.FatFinger = new FatFingerClienteInfo();

            lRequest.FatFinger.CodigoCliente = this.GetCodBovespa;

            var lResponse = new ServicoRegrasRisco().ReceberFatFingerCliente(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.FatFinger != null && lResponse.FatFinger.CodigoCliente != 0)
                {
                    this.hddCliente_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento.Value = lResponse.FatFinger.DataVencimento.ToString("dd/MM/yyyy");// DateTime.Today.ToString("dd/MM/yyyy");
                    this.txtCliente_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento.Value = lResponse.FatFinger.DataVencimento.ToString("dd/MM/yyyy");
                    this.hddCliente_Limites_Bovespa_ValorMaximoDaOrdem.Value                   =
                    this.txtCliente_Limites_Bovespa_ValorMaximoDaOrdem.Value                   = lResponse.FatFinger.ValorMaximo.ToString("N2");
                    this.chkCliente_Limites_Bovespa_ValorMaximoDaOrdem.Checked                 = true;
                }
            }
        }

        private void ConfigurarLimitesBloqueadosNaTela(List<TransporteRelatorio_005> pParametrosDetalhesDoLimite)
        {
            {
                this.txtCliente_Detalhes_AVista_Alocado.Value =
                this.txtCliente_Detalhes_AVista_Disponivel.Value =
                this.txtCliente_Detalhes_AVista_Limite.Value =

                this.txtCliente_Detalhes_Opcoes_Alocado.Value =
                this.txtCliente_Detalhes_Opcoes_Disponivel.Value =
                this.txtCliente_Detalhes_Opcoes_Limite.Value =

                this.txtCliente_Detalhes_AVista_Descoberto_Alocado.Value =
                this.txtCliente_Detalhes_AVista_Descoberto_Disponivel.Value =
                this.txtCliente_Detalhes_AVista_Descoberto_Limite.Value =

                this.txtCliente_Detalhes_Opcoes_Descoberto_Alocado.Value =
                this.txtCliente_Detalhes_Opcoes_Descoberto_Disponivel.Value =
                this.txtCliente_Detalhes_Opcoes_Descoberto_Limite.Value = "0,00";
            }

            pParametrosDetalhesDoLimite.ForEach(delegate(TransporteRelatorio_005 tr5)
            {
                if (tr5.Parametro.Contains("compra"))
                {
                    if (tr5.Parametro.Contains("vista"))
                    {
                        this.txtCliente_Detalhes_AVista_Alocado.Value = tr5.ValorAlocado;
                        this.txtCliente_Detalhes_AVista_Disponivel.Value = tr5.ValorDisponivel;
                        this.txtCliente_Detalhes_AVista_Limite.Value = tr5.ValorLimite;
                    }
                    else
                    {
                        this.txtCliente_Detalhes_Opcoes_Alocado.Value = tr5.ValorAlocado;
                        this.txtCliente_Detalhes_Opcoes_Disponivel.Value = tr5.ValorDisponivel;
                        this.txtCliente_Detalhes_Opcoes_Limite.Value = tr5.ValorLimite;
                    }
                }
                else
                {
                    if (tr5.Parametro.Contains("vista"))
                    {
                        this.txtCliente_Detalhes_AVista_Descoberto_Alocado.Value = tr5.ValorAlocado;
                        this.txtCliente_Detalhes_AVista_Descoberto_Disponivel.Value = tr5.ValorDisponivel;
                        this.txtCliente_Detalhes_AVista_Descoberto_Limite.Value = tr5.ValorLimite;
                    }
                    else
                    {
                        this.txtCliente_Detalhes_Opcoes_Descoberto_Alocado.Value = tr5.ValorAlocado;
                        this.txtCliente_Detalhes_Opcoes_Descoberto_Disponivel.Value = tr5.ValorDisponivel;
                        this.txtCliente_Detalhes_Opcoes_Descoberto_Limite.Value = tr5.ValorLimite;
                    }
                }
            });
        }

        private void ConfigurarPermissoesNaTela(List<TransporteConfigurarLimites> pPermissoesTodas)
        {
            this.rptCliente_Permissoes.DataSource = pPermissoesTodas;
            this.rptCliente_Permissoes.DataBind();
        }

        private SalvarParametroRiscoClienteResponse SalvarExpirarParametrosRisco(TransporteLimiteBovespa pParametro)
        {
            var lServicoRegrasRisco = new ServicoRegrasRisco();
            var lListaParametrosExpiradosIds = new List<int>();

            {   //--> Expirar Limite
                if (pParametro.FlagOperaAVistaDescobertoExpirarLimite)
                {
                    lListaParametrosExpiradosIds.Add(this.GetIdParametroLimiteDescobertoMercadoAVista);
                }
                if (pParametro.FlagOperaAVistaExpirarLimite)
                {
                    lListaParametrosExpiradosIds.Add(this.GetIdParametroLimiteCompraMercadoAVista);
                }
                if (pParametro.FlagOperaOpcaoDescobertoExpirarLimite)
                {
                    lListaParametrosExpiradosIds.Add(this.GetIdParametroLimiteDescobertoMercadoOpcoes);
                }
                if (pParametro.FlagOperaOpcaoExpirarLimite)
                {
                    lListaParametrosExpiradosIds.Add(this.GetIdParametroLimiteCompraMercadoOpcoes);
                }
                if (pParametro.FlagMaximoDaOrdemExpirarLimite)
                {
                    lListaParametrosExpiradosIds.Add(this.GetIdParametroLimiteMaximoDaOrdem);
                }

                lListaParametrosExpiradosIds.ForEach(delegate(int idParametro)
                {
                    lServicoRegrasRisco.SalvarExpirarLimite(
                        new SalvarParametroRiscoClienteRequest()
                        {
                            ParametroRiscoCliente = new ParametroRiscoClienteInfo()
                            {
                                CodigoCliente = pParametro.CodBovespa,
                                CodigoParametroCliente = idParametro
                            }
                        });
                });
            }

            return new SalvarParametroRiscoClienteResponse() { StatusResposta = MensagemResponseStatusEnum.OK };
        }

        private SalvarParametroRiscoClienteResponse SalvarIncluirRenovarParametroRisco(TransporteLimiteBovespa pParametro)
        {
            var lServicoRegrasRisco = new ServicoRegrasRisco();

            if (pParametro.FlagOperaAVistaDescobertoIncluirLimite || pParametro.FlagOperaAVistaDescobertoRenovarLimite)
            {
                var lParametro = new ParametroRiscoInfo()
                {
                    CodigoParametro = this.GetIdParametroLimiteDescobertoMercadoAVista,
                };

                lServicoRegrasRisco.SalvarParametroRiscoCliente(
                    new SalvarParametroRiscoClienteRequest()
                    {
                        ParametroRiscoCliente = new ParametroRiscoClienteInfo()
                        {
                            CodigoCliente = pParametro.CodBovespa,
                            Parametro = lParametro,
                            Valor = pParametro.LimiteAVistaDescoberto,
                            DataValidade = pParametro.VencimentoAVistaDescoberto.DBToDateTime(),
                        }
                    });
            }
            if (pParametro.FlagOperaAVistaIncluirLimite || pParametro.FlagOperaAVistaRenovarLimite)
            {
                var lParametro = new ParametroRiscoInfo()
                {
                    CodigoParametro = this.GetIdParametroLimiteCompraMercadoAVista,
                };

                lServicoRegrasRisco.SalvarParametroRiscoCliente(
                    new SalvarParametroRiscoClienteRequest()
                    {
                        ParametroRiscoCliente = new ParametroRiscoClienteInfo()
                        {
                            CodigoCliente = pParametro.CodBovespa,
                            Parametro = lParametro,
                            Valor = pParametro.LimiteAVista,
                            DataValidade = pParametro.VencimentoAVista.DBToDateTime(),
                        }
                    });
            }
            if (pParametro.FlagOperaOpcaoDescobertoIncluirLimite || pParametro.FlagOperaOpcaoDescobertoRenovarLimite)
            {
                var lParametro = new ParametroRiscoInfo()
                {
                    CodigoParametro = this.GetIdParametroLimiteDescobertoMercadoOpcoes,
                };

                lServicoRegrasRisco.SalvarParametroRiscoCliente(
                    new SalvarParametroRiscoClienteRequest()
                    {
                        ParametroRiscoCliente = new ParametroRiscoClienteInfo()
                        {
                            CodigoCliente = pParametro.CodBovespa,
                            Parametro = lParametro,
                            Valor = pParametro.LimiteOpcaoDescoberto,
                            DataValidade = pParametro.VencimentoOpcaoDescoberto.DBToDateTime(),
                        }
                    });
            }
            if (pParametro.FlagOperaOpcaoIncluirLimite || pParametro.FlagOperaOpcaoRenovarLimite)
            {
                var lParametro = new ParametroRiscoInfo()
                {
                    CodigoParametro = this.GetIdParametroLimiteCompraMercadoOpcoes,
                };

                var lRetornoSalvarParametroClienteRisco = lServicoRegrasRisco.SalvarParametroRiscoCliente(
                    new SalvarParametroRiscoClienteRequest()
                    {
                        ParametroRiscoCliente = new ParametroRiscoClienteInfo()
                        {
                            CodigoCliente = pParametro.CodBovespa,
                            Parametro = lParametro,
                            Valor = pParametro.LimiteOpcao,
                            DataValidade = pParametro.VencimentoOpcao.DBToDateTime(),
                        }
                    });
            }
            if (pParametro.FlagMaximoDaOrdem || pParametro.FlagMaximoDaOrdemRenovarLimite)
            {
                var lParametro = new ParametroRiscoInfo()
                {
                    CodigoParametro = this.GetIdParametroLimiteMaximoDaOrdem,
                };

                lServicoRegrasRisco.SalvarParametroRiscoCliente(
                    new SalvarParametroRiscoClienteRequest()
                    {
                        ParametroRiscoCliente = new ParametroRiscoClienteInfo()
                        {
                            CodigoCliente = pParametro.CodBovespa,
                            Parametro = lParametro,
                            Valor = pParametro.ValorMaximoDaOrdem,
                            DataValidade = pParametro.VencimentoMaximoDaOrdem.DBToDateTime(),
                        }
                    });
            }

            return new SalvarParametroRiscoClienteResponse() { StatusResposta = MensagemResponseStatusEnum.OK };
        }

        private MensagemResponseBase SalvarPermissoes(TransporteLimiteBovespa pParametro)
        {
            List<PermissaoRiscoAssociadaInfo> lItesnPermissoes = new List<PermissaoRiscoAssociadaInfo>();
            PermissaoRiscoAssociadaInfo lPrai;

            foreach (string item in pParametro.Permissoes)
            {
                if (item != null)
                {
                    lPrai = new PermissaoRiscoAssociadaInfo();
                    lPrai.PermissaoRisco = new PermissaoRiscoInfo()
                    {
                        CodigoPermissao = int.Parse(item)
                    };

                    lPrai.CodigoCliente = pParametro.CodBovespa;

                    lItesnPermissoes.Add(lPrai);
                }
            }

            SalvarPermissoesRiscoAssociadasRequest lreqSalvar = new SalvarPermissoesRiscoAssociadasRequest()
            {
                PermissoesAssociadas = lItesnPermissoes,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            MensagemResponseBase lresPer = ServicoRegrasRisco.SalvarPermissoesRiscoAssociadas(lreqSalvar);
            return lresPer;
        }

        private void ConfigurarRestricoesNaTela()
        {
            {   //--> Configurando o combo com as restrições por grupo.
                var lListaRestricoesGrupo = new ServicoRegrasRisco().ListarGrupos(new ListarGruposRequest() { });

                this.rptCliente_ResticoesPorGrupoOpcoes.DataSource =
                this.rptCliente_ResticoesPorGrupoOpcoesDescoberto.DataSource =
                this.rptCliente_ResticoesPorGrupoAVistaDescoberto.DataSource =
                this.rptCliente_ResticoesPorGrupoAVista.DataSource = lListaRestricoesGrupo.Grupos;

                this.rptCliente_ResticoesPorGrupoOpcoesDescoberto.DataBind();
                this.rptCliente_ResticoesPorGrupoAVistaDescoberto.DataBind();
                this.rptCliente_ResticoesPorGrupoOpcoes.DataBind();
                this.rptCliente_ResticoesPorGrupoAVista.DataBind();
            }

            if (this.gRetornoBloqueioPorCliente != null && this.gRetornoBloqueioPorCliente.Resultado != null)
            {   //--> Configurando a grid com os papeis por cliente.
                this.rptCliente_RestricaoPorAtivo.DataSource = new TransporteBloqueioInstrumento().TraduzirLista(this.gRetornoBloqueioPorCliente.Resultado);
                this.rptCliente_RestricaoPorAtivo.DataBind();
            }
        }

        private void SalvarRestricoesPorGrupo(TransporteClienteParametroGrupo[] pParametros)
        {
            if (null != pParametros && pParametros.Length > 0)
            {
                DbTransaction lDbTransaction;
                {   //--> Criando a transação.
                    var lConexao = new Conexao();
                    lConexao._ConnectionStringName = "RISCO";
                    var lDbConnection = lConexao.CreateIConnection();
                    lDbConnection.Open();
                    lDbTransaction = lDbConnection.BeginTransaction();
                }

                var lRetornoRemoverGrupo = new RemoverClienteParametroGrupoResponse();
                var lRetornoSalvamentoGrupo = new SalvarClienteParametroGrupoResponse();
                var lParametrosGrupos = new List<TransporteClienteParametroGrupo>(pParametros);
                var lServicoRegrasRisco = new ServicoRegrasRisco();
                int lIdCliente = this.Request.Form["Id"].DBToInt32();
                var lListaParametros = new List<int>();
                lListaParametros.Add(GetIdParametroLimiteCompraMercadoOpcoes);
                lListaParametros.Add(GetIdParametroLimiteCompraMercadoAVista);
                lListaParametros.Add(GetIdParametroLimiteDescobertoMercadoOpcoes);
                lListaParametros.Add(GetIdParametroLimiteDescobertoMercadoAVista);

                //--> Removendo as incidencias de grupos para determinado parâmetro.
                lListaParametros.ForEach(delegate(int idParametro)
                {
                    lRetornoRemoverGrupo = lServicoRegrasRisco.RemoverClienteParametroGrupo(lDbTransaction, new RemoverClienteParametroGrupoRequest()
                        {
                            Objeto = new ClienteParametroGrupoInfo()
                            {
                                IdCliente = lIdCliente,
                                IdParametro = idParametro,
                            }
                        });

                    if (!MensagemResponseStatusEnum.OK.Equals(lRetornoRemoverGrupo.StatusResposta))
                    {
                        lDbTransaction.Rollback();
                        base.RetornarErroAjax("Houve um erro durante o salvamento dos dados. Tente novamente por favor.");
                        return;
                    }
                });

                lParametrosGrupos.ForEach(delegate(TransporteClienteParametroGrupo cpg)
                {
                    //--> Atrelando os grupos aos parametros.
                    cpg.ListaGrupos.ForEach(delegate(int idGrupo)
                    {
                        lRetornoSalvamentoGrupo = lServicoRegrasRisco.SalvarClienteParametroGrupo(lDbTransaction, new SalvarClienteParametroGrupoRequest()
                        {
                            Objeto = new ClienteParametroGrupoInfo()
                            {
                                IdCliente = cpg.IdCliente,
                                IdGrupo = idGrupo,
                                IdParametro = cpg.IdParametro,
                            }
                        });

                        if (!MensagemResponseStatusEnum.OK.Equals(lRetornoSalvamentoGrupo.StatusResposta))
                        {
                            lDbTransaction.Rollback();
                            base.RetornarErroAjax("Houve um erro durante o salvamento dos dados. Tente novamente por favor.");
                            return;
                        }
                    });
                });

                if (MensagemResponseStatusEnum.OK.Equals(lRetornoSalvamentoGrupo.StatusResposta))
                    lDbTransaction.Commit();
            }
        }

        private void SalvarRestricoesPorAtivo(TransporteBloqueioInstrumento pParametro)
        {
            if (null != pParametro)
            {
                DbTransaction lDbTransaction;
                {   //--> Criando a transação.
                    var lConexao = new Conexao();
                    lConexao._ConnectionStringName = "RISCO";
                    var lDbConnection = lConexao.CreateIConnection();
                    lDbConnection.Open();
                    lDbTransaction = lDbConnection.BeginTransaction();
                }

                var lRetornoSalvamentoBloqueio = new SalvarBloqueioInstrumentoResponse();

                int lIdCliente = this.Request.Form["Id"].DBToInt32();
                var lRetornoExclusaoBloqueio = new ServicoRegrasRisco().RemoverBloqueioPorCliente(lDbTransaction, new RemoverBloqueioInstrumentoRequest()
                    {
                        Objeto = new BloqueioInstrumentoInfo()
                        {
                            IdCliente = lIdCliente
                        }
                    });

                if (!MensagemResponseStatusEnum.OK.Equals(lRetornoExclusaoBloqueio.StatusResposta))
                {
                    lDbTransaction.Rollback();
                    base.RetornarErroAjax("Houve um erro durante o salvamento dos dados. Tente novamente por favor.");
                    return;
                }

                for (int i = 0; i < pParametro.Ativos.Length; i++)
                {
                    lRetornoSalvamentoBloqueio = new ServicoRegrasRisco().SalvarBloqueioInstrumento(
                        lDbTransaction,
                        new SalvarBloqueioInstrumentoRequest()
                        {
                            Objeto = new BloqueioInstrumentoInfo()
                            {
                                CdAtivo = pParametro.Ativos[i],
                                Direcao = pParametro.Direcoes[i].Substring(0, 1),
                                IdCliente = lIdCliente
                            }
                        });

                    if (!MensagemResponseStatusEnum.OK.Equals(lRetornoSalvamentoBloqueio.StatusResposta))
                    {
                        lDbTransaction.Rollback();
                        base.RetornarErroAjax("Houve um erro durante o salvamento dos dados. Tente novamente por favor.");
                        return;
                    }
                }

                if (MensagemResponseStatusEnum.OK.Equals(lRetornoSalvamentoBloqueio.StatusResposta))
                    lDbTransaction.Commit();
            }
        }

        #endregion
    }
}
