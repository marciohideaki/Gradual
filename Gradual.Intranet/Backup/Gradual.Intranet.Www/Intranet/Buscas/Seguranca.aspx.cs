using System;
using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Buscas
{

    [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
    public partial class Seguranca : PaginaBaseAutenticada
    {
        #region Propriedades

        private List<object> SessionUltimoResultadoDeBusca
        {
            get
            {
                if (Session["SessionUltimoResultadoDeBusca"] == null)
                    Session["SessionUltimoResultadoDeBusca"] = new List<object>();
                return (List<object>)Session["SessionUltimoResultadoDeBusca"];
            }
            set
            {
                Session["SessionUltimoResultadoDeBusca"] = value;
            }
        }


        #endregion

        #region Métodos Private

        private string ResponderBuscarUsuarios()
        {
            string lRetorno = "Erro...";
            
            ListarUsuariosRequest lRequest = new ListarUsuariosRequest();
            lRequest.CodigoSessao = this.CodigoSessao;

            if (Request.Params["BuscarCampo"].ToLower().Trim() == "descricao")
            {
                lRequest.FiltroNomeOuEmail = Request.Params["TermoDeBusca"];
            }

            if (Request["BuscarCampo"].ToLower().Trim() == "codigo")
            {
                lRequest.FiltroCodigoUsuario = Request.Params["TermoDeBusca"];
            }

            ListarUsuariosResponse lResponse = ServicoSeguranca.ListarUsuarios(lRequest);

            SessionUltimoResultadoDeBusca.Clear();

            foreach (UsuarioInfo lUserInfo in lResponse.Usuarios)
            {
                this.SessionUltimoResultadoDeBusca.Add(new TransporteSegurancaUsuario()
                {
                    Id = lUserInfo.CodigoUsuario,
                    Nome = lUserInfo.Nome,
                    Email = lUserInfo.Email
                });
            }

            TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

            lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontrados [{0}] clientes", SessionUltimoResultadoDeBusca.Count);

            return lRetorno;
        }

        private string ResponderBuscarGrupos()
        {

            string lRetorno = "Erro...";

            ListarUsuarioGruposRequest lRequest = new ListarUsuarioGruposRequest();
            lRequest.CodigoSessao = this.CodigoSessao;

            if (Request.Params["TermoDeBusca"] != null && Request.Params["TermoDeBusca"] != string.Empty)
            {
                if (Request.Params["BuscarCampo"].ToLower().Trim() == "descricao")
                {
                    lRequest.FiltroNomeUsuarioGrupo = Request.Params["TermoDeBusca"];
                }

                if (Request["BuscarCampo"].ToLower().Trim() == "codigo")
                {
                    lRequest.FiltroCodigoUsuarioGrupo = Request.Params["TermoDeBusca"];
                }
            }

            ListarUsuarioGruposResponse lResponse = ServicoSeguranca.ListarUsuarioGrupos(lRequest);

            SessionUltimoResultadoDeBusca.Clear();

            foreach (UsuarioGrupoInfo lUserInfo in lResponse.UsuarioGrupos)
            {
                this.SessionUltimoResultadoDeBusca.Add(new TransporteSegurancaGrupo()
                {
                    Id = lUserInfo.CodigoUsuarioGrupo,
                    Nome = lUserInfo.NomeUsuarioGrupo
                });
            }

            TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

            lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontrados [{0}] grupos", SessionUltimoResultadoDeBusca.Count);

            return lRetorno;
        }

        private string ResponderBuscarPerfis()
        {
            string lRetorno = "Erro...";

            ListarPerfisRequest lRequest = new ListarPerfisRequest();
            lRequest.CodigoSessao = this.CodigoSessao;

            if (Request.Params["TermoDeBusca"] != null && Request.Params["TermoDeBusca"] != string.Empty)
            {

                if (Request.Params["BuscarCampo"].ToLower().Trim() == "descricao")
                {
                    lRequest.FiltroNomePerfil = Request.Params["TermoDeBusca"];
                }

                if (Request["BuscarCampo"].ToLower().Trim() == "codigo")
                {
                    lRequest.FiltroCodigoPerfil = Request.Params["TermoDeBusca"];
                }
            }
            ListarPerfisResponse lResponse = ServicoSeguranca.ListarPerfis(lRequest);

            SessionUltimoResultadoDeBusca.Clear();

            foreach (PerfilInfo lPerfilInfo in lResponse.Perfis)
            {
                this.SessionUltimoResultadoDeBusca.Add(new TransporteSegurancaPerfil()
                {
                    Id = lPerfilInfo.CodigoPerfil,
                    Nome = lPerfilInfo.NomePerfil
                });
            }

            TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

            lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontrados [{0}] perfis", SessionUltimoResultadoDeBusca.Count);

            return lRetorno;
        }

        private string ResponderBuscarPermissoes()
        {
            string lRetorno = "Erro...";

            ListarPermissoesRequest lRequest = new ListarPermissoesRequest();
            lRequest.CodigoSessao = this.CodigoSessao;

            if (Request.Params["TermoDeBusca"] != null && Request.Params["TermoDeBusca"] != string.Empty)
            {

                if (Request.Params["BuscarCampo"].ToLower().Trim() == "descricao")
                {
                    lRequest.FiltroNomePermissao = Request.Params["TermoDeBusca"];
                }

                if (Request["BuscarCampo"].ToLower().Trim() == "codigo")
                {
                    lRequest.FiltroCodigoPermissao= Request.Params["TermoDeBusca"];
                }
            }
            ListarPermissoesResponse lResponse = ServicoSeguranca.ListarPermissoes(lRequest);

            SessionUltimoResultadoDeBusca.Clear();

            foreach (PermissaoInfo lPermissaoInfo in lResponse.Permissoes)
            {
                this.SessionUltimoResultadoDeBusca.Add(new TransporteSegurancaPermissaoSeguranca()
                {
                    Id = lPermissaoInfo.CodigoPermissao,
                    Nome = lPermissaoInfo.NomePermissao,
                    DescricaoPermissao = lPermissaoInfo.DescricaoPermissao
                });
            }

            TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

            lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontrados [{0}] perfis", SessionUltimoResultadoDeBusca.Count);

            return lRetorno;
        }

        private string ResponderBuscarItensParaSelecao()
        {
            string lRetorno = "Erro...";

            try
            {
                switch (Request.Form["BuscarPor"])
                {
                    case "Usuario": lRetorno = ResponderBuscarUsuarios(); break;
                    case "Grupo": lRetorno = ResponderBuscarGrupos(); break;
                    case "Perfil": lRetorno = ResponderBuscarPerfis(); break;
                    case "Permissao": lRetorno = ResponderBuscarPermissoes(); break;

                    default:

                        lRetorno = RetornarErroAjax("Sem busca implementada para objeto [{0}]", Request.Form["BuscarPor"]);

                        break;
                }
            }
            catch (Exception exBusca)
            {
                lRetorno = RetornarErroAjax("Erro durante a busca", exBusca);
            }

            return lRetorno;
        }

        private string ResponderPaginar()
        {
            string lRetorno = string.Empty;

            //_search=false&nd=1275415807834&rows=10&page=2&sidx=invid&sord=desc
            if (this.SessionUltimoResultadoDeBusca != null)
            {
                int lPagina;

                if (int.TryParse(Request["page"], out lPagina))
                {
                    TransporteDeListaPaginada lLista;

                    lLista = BuscarPaginaDeResultados(lPagina);

                    lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado
                }
            }

            return lRetorno;
        }

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
 
            TransporteDeListaPaginada lRetorno = new TransporteDeListaPaginada();

            List<object> lLista = new List<object>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            for (int a = lIndiceInicial; a < lIndiceFinal; a++)
            {
                if (a < this.SessionUltimoResultadoDeBusca.Count)
                {
                    lLista.Add(this.SessionUltimoResultadoDeBusca[a]);
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }
        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            RegistrarRespostasAjax(new string[] { 
                                                    "BuscarItensParaSelecao"
                                                  , "Paginar"
                                                },
                new ResponderAcaoAjaxDelegate[] { 
                                                    ResponderBuscarItensParaSelecao
                                                  , ResponderPaginar
                                                });
        }

        #endregion
    }
}
