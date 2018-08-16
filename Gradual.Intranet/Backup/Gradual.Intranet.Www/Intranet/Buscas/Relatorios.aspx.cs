using Gradual.OMS.Seguranca.Lib;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Relatorios : PaginaBaseAutenticada
    {
        public static string gIdAsessorLogado;

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(Relatorios_Load);
        }

        void Relatorios_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ReceberSessaoRequest lReq = new ReceberSessaoRequest();
                lReq.CodigoSessao = base.CodigoSessao;
                lReq.CodigoSessaoARetornar = base.CodigoSessao;
                ReceberSessaoResponse lRes = base.ServicoSeguranca.ReceberSessao(lReq);
                if (lRes.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    this.litRelAssessor.Visible = !lRes.Usuario.Perfis.Contains("6");

                    if (!this.litRelAssessor.Visible)
                        gIdAsessorLogado = lRes.Usuario.CodigoAssessor.ToString();
                }

                this.BindCombos();
            }
        }

        private void BindCombos()
        {
            {   //--> Combo Assessores Sinacor
                base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptRisco_FiltroRelatorioRisco_Assessor);
            }
        }

        //#region | Atributos

        //private static Hashtable LstPaises = new Hashtable();
        //private static Hashtable LstAtividades = new Hashtable();

        //#endregion

        //#region | Propriedades

        //private List<AtividadeIlicitaInfo> GetAtividadesIlicitasCadastradas
        //{
        //    get
        //    {
        //        var lRetorno =
        //            base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<AtividadeIlicitaInfo>
        //            (
        //                new Contratos.Mensagens.ConsultarEntidadeCadastroRequest<AtividadeIlicitaInfo>()
        //            );

        //        lRetorno.Resultado.ForEach(
        //            delegate(AtividadeIlicitaInfo aii)
        //            {   //Caro Programador, esta instrução traduz o nome da atividade.
        //                aii.IdAtividadeIlicita = aii.CdAtividade.DBToInt32();
        //                aii.CdAtividade = LstAtividades[aii.CdAtividade].DBToString();
        //            });

        //        return lRetorno.Resultado;
        //    }
        //}

        //private List<PaisesBlackListInfo> GetPaisesBlackListCadastrados
        //{
        //    get
        //    {
        //        var lRetorno =
        //            base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<PaisesBlackListInfo>
        //            (
        //                new ConsultarEntidadeCadastroRequest<PaisesBlackListInfo>()
        //            );

        //        lRetorno.Resultado.ForEach(
        //            delegate(PaisesBlackListInfo pbl)
        //            {   //Caro Programador, esta instrução traduz o nome do país.
        //                pbl.DsNomePais = LstPaises[pbl.CdPais].DBToString();
        //            });

        //        return lRetorno.Resultado;
        //    }
        //}

        //private List<TipoDePendenciaCadastralInfo> GetTiposPendenciaCadastral
        //{
        //    get
        //    {
        //        ConsultarEntidadeCadastroResponse<TipoDePendenciaCadastralInfo> lRetornoPendencias = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<TipoDePendenciaCadastralInfo>(new ConsultarEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>()
        //        {
        //            IdUsuarioLogado = base.UsuarioLogado.Id,
        //            DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
        //            EntidadeCadastro = new TipoDePendenciaCadastralInfo()
        //        });

        //        return lRetornoPendencias.Resultado;
        //    }
        //}

        //#endregion

        //#region | Eventos

        //protected new void Page_Load(object sender, EventArgs e)
        //{
        //    if (!this.Page.IsPostBack)
        //    {
        //        this.PreenchePaisesAtividadesSinacor();

        //        base.PopularComboComListaGenerica<TipoDePendenciaCadastralInfo>(GetTiposPendenciaCadastral, rptClientes_FiltroRelatorio_TipoDePendencia);
        //        base.PopularComboComListaGenerica<PaisesBlackListInfo>(this.GetPaisesBlackListCadastrados, rptClientes_FiltroRelatorio_Pais);
        //        base.PopularComboComListaGenerica<AtividadeIlicitaInfo>(this.GetAtividadesIlicitasCadastradas, rptClientes_FiltroRelatorio_AtividadeIlicita);
        //    }
        //}

        //#endregion

        //#region | Métodos de apoio

        ///// <summary>
        ///// Método de apoio para preenchimento das listas de atividades e países
        ///// </summary>
        //private void PreenchePaisesAtividadesSinacor()
        //{
        //    LstAtividades.Clear();
        //    LstPaises.Clear();

        //    var lRespostaAtividade =
        //        base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>
        //        (
        //            new ConsultarEntidadeCadastroRequest<SinacorListaInfo>()
        //            {
        //                EntidadeCadastro = new SinacorListaInfo()
        //                {
        //                    Informacao = eInformacao.AtividadePFePJ,
        //                }
        //            }
        //        );

        //    var lRespostaPaises =
        //        base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>
        //        (
        //            new ConsultarEntidadeCadastroRequest<SinacorListaInfo>()
        //            {
        //                EntidadeCadastro = new SinacorListaInfo()
        //                {
        //                    Informacao = eInformacao.Pais,
        //                }
        //            }
        //        );

        //    lRespostaAtividade.Resultado.ForEach(delegate(SinacorListaInfo item) { LstAtividades.Add(item.Id, item.Value); });
        //    lRespostaPaises.Resultado.ForEach(delegate(SinacorListaInfo item) { LstPaises.Add(item.Id, item.Value); });
        //}

        //#endregion
    }
}