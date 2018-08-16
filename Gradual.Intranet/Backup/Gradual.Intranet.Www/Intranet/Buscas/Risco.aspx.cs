using System;
using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Risco : PaginaBaseAutenticada
    {
        #region Propriedades

        //TODO: Aqui em vez de TransporteRiscoGrupo é o Tipo do objeto de perfil que vem direto do serviço
        private List<object> SessionUltimoResultadoDeBusca
        {
            get
            {
                return (List<object>)Session["UltimoResultadoDeBuscaDePerfisDeRisco"];
            }

            set
            {
                Session["UltimoResultadoDeBuscaDePerfisDeRisco"] = value;
            }
        }

        #endregion

        #region Métodos Private
        
        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            TransporteDeListaPaginada lRetorno = new TransporteDeListaPaginada();

            List<object> lLista = new List<object>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal =   (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            for (int a = lIndiceInicial; a < lIndiceFinal; a++) if (a < this.SessionUltimoResultadoDeBusca.Count)
                    lLista.Add(this.SessionUltimoResultadoDeBusca[a]);

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        private List<object> ListarGrupos(string pBusca)
        {
            List<object> lItens = new List<object>();
            ListarGruposResponse lRes = ServicoRegrasRisco.ListarGrupos(new ListarGruposRequest() { FiltroNomeGrupo = pBusca, DescricaoUsuarioLogado=base.UsuarioLogado.Nome, IdUsuarioLogado= base.UsuarioLogado.Id });
           
            foreach (GrupoInfo itemGrupo in lRes.Grupos)
            {
                lItens.Add(new TransporteRiscoGrupo()
                {
                    Descricao = itemGrupo.NomeDoGrupo,
                    Id = itemGrupo.CodigoGrupo.ToString()
                });
            };

            return lItens;
        }

        private List<object> ListarPermissoes(string pBusca, BolsaInfo pBolsa)
        {
            List<object> lItens = new List<object>();
            ListarPermissoesRiscoResponse lRes = ServicoRegrasRisco.ListarPermissoesRisco(
                new ListarPermissoesRiscoRequest() 
                { 
                    Bolsa = pBolsa,
                    FiltroNomePermissao = pBusca , DescricaoUsuarioLogado = base.UsuarioLogado.Nome, IdUsuarioLogado= base.UsuarioLogado.Id
                });

            foreach (PermissaoRiscoInfo itemPermissao in lRes.Permissoes)
            {
                lItens.Add(new TransporteRiscoPermissao()
                {
                    Descricao = itemPermissao.NomePermissao,
                    Id = itemPermissao.CodigoPermissao.ToString()
                });
            };

            return lItens;
        }

        private List<object> ListarParametros(string pBusca, BolsaInfo pBolsa)
        {
            List<object> lItens = new List<object>();
            ListarParametrosRiscoResponse lRes = ServicoRegrasRisco.ListarParametrosRisco(
                new ListarParametrosRiscoRequest() 
                { 
                    Bolsa = pBolsa,
                    FiltroNomeParamertro = pBusca,
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                    IdUsuarioLogado = base.UsuarioLogado.Id
                });

            foreach (ParametroRiscoInfo itemPermissao in lRes.ParametrosRisco)
            {
                lItens.Add(new TransporteRiscoParametro()
                {
                    Descricao = itemPermissao.NomeParametro,
                    Id = itemPermissao.CodigoParametro.ToString()
                });
            };

            return lItens;
        }

        private string ResponderBuscarItensParaSelecao()
        {
            string lRetorno = "Erro...";
            List<object> lListaRetorno = new List<object>();
            
            try
            {
                string lBuscarPor = Request["BuscarPor"];
                string lTermoDeBusca = Request["TermoDeBusca"];
                
                switch(lBuscarPor)
                {
                    case "grupo":
                        lListaRetorno = this.ListarGrupos(lTermoDeBusca);
                        break;
                    case "permissao":
                        lListaRetorno = this.ListarPermissoes(lTermoDeBusca, BolsaInfo.TODAS);
                        break;
                    case "parametro":
                        lListaRetorno = this.ListarParametros(lTermoDeBusca, BolsaInfo.TODAS);
                        break;
                }

                this.SessionUltimoResultadoDeBusca = lListaRetorno;

                // Fim da lista de teste

                TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

                lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontrados [{0}] itens", lListaRetorno.Count);

            }
            catch (Exception exBusca)
            {
                RetornarErroAjax("Erro durante a busca", exBusca);
            }

            return lRetorno;
        }

        private string ResponderPaginar()
        {
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

            //_search=false&nd=1275415807834&rows=10&page=2&sidx=invid&sord=desc

            if (this.SessionUltimoResultadoDeBusca != null)
            {
                int lPagina;

                if (int.TryParse(Request["page"], out lPagina))
                {
                    lLista = BuscarPaginaDeResultados(lPagina);

                }
            }
            //else
            //{
            //    lLista;
            //}

            lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }


        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                , "Paginar"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderBuscarItensParaSelecao
                                                , ResponderPaginar
                                                });
        }

        #endregion
    }
}
