using System;
using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class ParametroAlavancagem : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int GetIdGrupo
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["IdGrupo"], out lRetorno);

                return lRetorno;
            }
        }

        private decimal GetContaCorrente
        {
            get
            {
                var lRetorno = default(decimal);

                decimal.TryParse(this.Request.Form["CC"], out lRetorno);

                return lRetorno;
            }
        }

        private decimal GetCustodia
        {
            get
            {
                var lRetorno = default(decimal);

                decimal.TryParse(this.Request.Form["Custodia"], out lRetorno);

                return lRetorno;
            }
        }

        private decimal GetCompraAVista
        {
            get
            {
                var lRetorno = default(decimal);

                decimal.TryParse(this.Request.Form["CompraAVista"], out lRetorno);

                return lRetorno;
            }
        }

        private decimal GetVendaAVista
        {
            get
            {
                var lRetorno = default(decimal);

                decimal.TryParse(this.Request.Form["VendaAVista"], out lRetorno);

                return lRetorno;
            }
        }

        private decimal GetCompraOpcao
        {
            get
            {
                var lRetorno = default(decimal);

                decimal.TryParse(this.Request.Form["CompraOpcao"], out lRetorno);

                return lRetorno;
            }
        }

        private decimal GetVendaOpcao
        {
            get
            {
                var lRetorno = default(decimal);

                decimal.TryParse(this.Request.Form["VendaOpcao"], out lRetorno);

                return lRetorno;
            }
        }

        private char GetCarteira23
        {
            get
            {
                var lRetorno = default(bool);

                if (bool.TryParse(this.Request.Form["Carteira23"], out lRetorno) && lRetorno)
                    return 'S';
                else
                    return 'N';
            }
        }

        private char GetCarteira27
        {
            get
            {
                var lRetorno = default(bool);

                if (bool.TryParse(this.Request.Form["Carteira27"], out lRetorno) && lRetorno)
                    return 'S';
                else
                    return 'N';
            }
        }

        #endregion

        #region | Métodos

        private string ResponderCarregarHtmlComDados()
        {
            var lListaGrupoItem = new List<TransporteRiscoGrupo>();
            var lResponseGrupos = base.ServicoRegrasRisco.ListarGrupos(
                new ListarGruposRequest()
                {
                    FiltroTipoGrupo = EnumRiscoRegra.TipoGrupo.GrupoAlavancagem
                });

            if (null != lResponseGrupos && null != lResponseGrupos.Grupos && lResponseGrupos.Grupos.Count > 0)
                lResponseGrupos.Grupos.ForEach(lGrupoInfo =>
                {
                    lListaGrupoItem.Add(new TransporteRiscoGrupo(lGrupoInfo));
                });

            base.PopularComboComListaGenerica<TransporteRiscoGrupo>(lListaGrupoItem, this.rptGradIntra_ComboBox_Digitavel);

            return string.Empty;
        }

        private string ResponderSalvarParametrosDoGrupo()
        {
            var lRetorno = string.Empty;

            var lResponseSalvarParametroAlavancagem = base.ServicoRegrasRisco.SalvarParametroAlavancagem(new SalvarParametroAlavancagemRequest()
            {
                Objeto = new ParametroAlavancagemInfo()
                {
                    IdGrupo = this.GetIdGrupo,
                    PercentualContaCorrente = this.GetContaCorrente,
                    PercentualCustodia = this.GetCustodia,
                    PercentualAlavancagemCompraAVista = this.GetCompraAVista,
                    PercentualAlavancagemVendaAVista = this.GetVendaAVista,
                    PercentualAlavancagemCompraOpcao = this.GetCompraOpcao,
                    PercentualAlavancagemVendaOpcao = this.GetVendaOpcao,
                    StCarteiraGarantiaPrazo = this.GetCarteira23,
                    StCarteiraOpcao = this.GetCarteira27,
                }
            });

            if (lResponseSalvarParametroAlavancagem.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = base.RetornarSucessoAjax("Alterações salvas com sucesso.");
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Houve um erro ao tentar atualizar os parâmetros deste grupo", lResponseSalvarParametroAlavancagem.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderSelecionarParametrosDoGrupo()
        {
            var lRetorno = string.Empty;
            var lResponseSelecionarParametroGrupo = base.ServicoRegrasRisco.ListarParametroAlavancagem(new ListarParametroAlavancagemRequest()
                {
                    FiltroIdGrupo = this.GetIdGrupo
                });

            if (lResponseSelecionarParametroGrupo.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                TransporteGrupoDeAlavancagem lTransporteGrupoDeAlavancagem;

                if (lResponseSelecionarParametroGrupo.Objeto.Count > 0)
                    lTransporteGrupoDeAlavancagem = new TransporteGrupoDeAlavancagem(lResponseSelecionarParametroGrupo.Objeto[0]);
                else
                    lTransporteGrupoDeAlavancagem = new TransporteGrupoDeAlavancagem(new ParametroAlavancagemInfo());

                lRetorno = base.RetornarSucessoAjax(lTransporteGrupoDeAlavancagem, "Dados carregados com sucesso.");
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao tentar recuperar os dados deste Grupo de Alavancagem", lResponseSelecionarParametroGrupo.DescricaoResposta);
            }

            return lRetorno;
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if ("CarregarHtml".Equals(base.Acao))
            {
                this.ResponderCarregarHtmlComDados();
            }
            else
            {
                base.RegistrarRespostasAjax(new string[] { "SalvarParametrosDoGrupo"
                                                         , "SelecionarParametrosDoGrupo"
                                                         , "CarregarHtmlComDados"
                                                         },
                         new ResponderAcaoAjaxDelegate[] { this.ResponderSalvarParametrosDoGrupo
                                                         , this.ResponderSelecionarParametrosDoGrupo
                                                         , this.ResponderCarregarHtmlComDados
                                                         });
            }
        }

        #endregion
    }
}