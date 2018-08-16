using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.PlanoCliente.Lib;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class Filtros : PaginaBaseAutenticada
    {
        #region | Atributos

        private static Hashtable LstPaises = new Hashtable();
        private static Hashtable LstAtividades = new Hashtable();

        public static string gIdAsessorLogado;

        #endregion

        #region | Propriedades

        private int? GetCdAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0)
                    return base.CodigoAssessor.Value;
                
                return null;
            }
        }

        private List<PoupeDirectProdutoInfo> GetProdutosPoupe
        {
            get
            {
                var lServico = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<PoupeDirectProdutoInfo>(new ConsultarEntidadeCadastroRequest<PoupeDirectProdutoInfo>());

                return lServico.Resultado;
            }
        }

        private List<Gradual.OMS.PlanoCliente.Lib.ProdutoInfo> GetProdutosCliente
        {
            get
            {
                IServicoPlanoCliente lServico = Ativador.Get<IServicoPlanoCliente>();

                ListarProdutosResponse lResponse = lServico.ListarProdutos();

                return lResponse.LstProdutos;
            }
        }

        private List<AtividadeIlicitaInfo> GetAtividadesIlicitasCadastradas
        {
            get
            {
                var lRetorno =
                    base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<AtividadeIlicitaInfo>
                    (
                        new Contratos.Mensagens.ConsultarEntidadeCadastroRequest<AtividadeIlicitaInfo>()
                    );

                lRetorno.Resultado.ForEach(
                    delegate(AtividadeIlicitaInfo aii)
                    {   //Caro Programador, esta instrução traduz o nome da atividade.
                        aii.IdAtividadeIlicita = aii.CdAtividade.DBToInt32();
                        aii.CdAtividade = LstAtividades[aii.CdAtividade].DBToString();
                    });

                return lRetorno.Resultado;
            }
        }

        private List<PaisesBlackListInfo> GetPaisesBlackListCadastrados
        {
            get
            {
                var lRetorno =
                    base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<PaisesBlackListInfo>
                    (
                        new ConsultarEntidadeCadastroRequest<PaisesBlackListInfo>()
                    );

                lRetorno.Resultado.ForEach(
                    delegate(PaisesBlackListInfo pbl)
                    {   //Caro Programador, esta instrução traduz o nome do país.
                        pbl.DsNomePais = LstPaises[pbl.CdPais].DBToString();
                    });

                return lRetorno.Resultado;
            }
        }

        private List<TipoDePendenciaCadastralInfo> GetTiposPendenciaCadastral
        {
            get
            {
                ConsultarEntidadeCadastroResponse<TipoDePendenciaCadastralInfo> lRetornoPendencias = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<TipoDePendenciaCadastralInfo>(new ConsultarEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>()
                {
                    IdUsuarioLogado = base.UsuarioLogado.Id,
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                    EntidadeCadastro = new TipoDePendenciaCadastralInfo()
                });

                return lRetornoPendencias.Resultado;
            }
        }

        private List<ClientePorAssessorInfo> GetListaClientesVinculados
        {
            get
            {
                var lRetorno = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePorAssessorInfo>(
                    new ConsultarEntidadeCadastroRequest<ClientePorAssessorInfo>()
                    {
                        EntidadeCadastro = new ClientePorAssessorInfo()
                        {
                            ConsultaCdAssessor = this.GetCdAssessor
                        }
                    });

                return lRetorno.Resultado;
            }        
        }

        #endregion

        #region | Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!Page.IsPostBack)
            {
                var lReceberSessao = ServicoSeguranca.ReceberSessao(
                    new ReceberSessaoRequest()
                    {
                        CodigoSessao = base.CodigoSessao,
                        CodigoSessaoARetornar = base.CodigoSessao,
                    });

                if (lReceberSessao.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    this.litRelAssessor.Visible = !lReceberSessao.Usuario.Perfis.Contains("6");

                    //--> Defindo o valor para posicionamento da grid
                    gIdAsessorLogado = !this.litRelAssessor.Visible ? lReceberSessao.Usuario.CodigoAssessor.ToString() : string.Empty;
                }

                this.PreenchePaisesAtividadesSinacor();

                base.PopularComboComListaGenerica<TipoDePendenciaCadastralInfo>(this.GetTiposPendenciaCadastral, this.rptClientes_FiltroRelatorio_TipoDePendencia);
                base.PopularComboComListaGenerica<PaisesBlackListInfo>(this.GetPaisesBlackListCadastrados, this.rptClientes_FiltroRelatorio_Pais);
                base.PopularComboComListaGenerica<AtividadeIlicitaInfo>(this.GetAtividadesIlicitasCadastradas, this.rptClientes_FiltroRelatorio_AtividadeIlicita);
                base.PopularComboComListaGenerica<PoupeDirectProdutoInfo>(this.GetProdutosPoupe, this.rptClientes_FiltroRelatorio_ProdutosPoupe);

                base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptRisco_FiltroRelatorio_Assessor); //--> Combo Assessores Sinacor

                this.rptClientes_FiltroRelatorio_Produtos.DataSource = this.GetProdutosCliente;
                this.rptClientes_FiltroRelatorio_Produtos.DataBind();

                this.rptClientes_FiltroAssessor.DataSource = new TransporteRelatorio_017().TraduzirListaConsulta(this.GetListaClientesVinculados);
                this.rptClientes_FiltroAssessor.DataBind();

                this.rptRelatorio018_ClientesAssessor.DataSource = new TransporteRelatorio_017().TraduzirListaConsulta(this.GetListaClientesVinculados);
                this.rptRelatorio018_ClientesAssessor.DataBind();

                base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptRelatorio018_Assessores); //--> Combo Assessores Sinacor
            }
        }

        #endregion

        #region | Métodos de apoio

        /// <summary>
        /// Método de apoio para preenchimento das listas de atividades e países
        /// </summary>
        private void PreenchePaisesAtividadesSinacor()
        {
            LstAtividades.Clear();
            LstPaises.Clear();

            var lRespostaAtividade =
                base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>
                (
                    new ConsultarEntidadeCadastroRequest<SinacorListaInfo>()
                    {
                        EntidadeCadastro = new SinacorListaInfo()
                        {
                            Informacao = eInformacao.AtividadePFePJ,
                        }
                    }
                );

            var lRespostaPaises =
                base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>
                (
                    new ConsultarEntidadeCadastroRequest<SinacorListaInfo>()
                    {
                        EntidadeCadastro = new SinacorListaInfo()
                        {
                            Informacao = eInformacao.Pais,
                        }
                    }
                );

            lRespostaAtividade.Resultado.ForEach(delegate(SinacorListaInfo item) { LstAtividades.Add(item.Id, item.Value); });
            lRespostaPaises.Resultado.ForEach(delegate(SinacorListaInfo item) { LstPaises.Add(item.Id, item.Value); });
        }

        #endregion
    }
}
