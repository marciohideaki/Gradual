using System;
using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet
{
    public class PaginaBaseAutenticada : PaginaBase
    {
        #region Propriedades

        private string _IncIDjs = DateTime.Now.Millisecond.ToString();

        public string IncIDjs
        {
            get
            {
                return "_" + _IncIDjs;
            }
        }

        public bool ELocal
        {
            get
            {
                return Request.IsLocal;
            }
        }

        public string PrefixoDaRaiz
        {
            get
            {
                if (Request.Url.AbsoluteUri.ToLower().Contains("localhost"))
                {
                    return "/Gradual.Intranet";
                }
                else if (Request.Url.AbsoluteUri.ToLower().Contains(":4242"))
                {
                    return "/Intranet"; //no serivdor está configurado dentro de default web site, no diretorio /Intranet
                }
                else
                {
                    return "";
                }
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.UsuarioLogado == null)
            {
                if (string.IsNullOrEmpty(this.Acao))
                {
                    this.RedirecionarPara("Default.aspx");
                    //Server.Transfer("Login.aspx");
                }
                else
                {
                    this.Response.Clear();

                    this.Response.Write(base.RetornarErroAjax(RESPOSTA_SESSAO_EXPIRADA));

                    this.Response.End();
                }
            }
            else
            {
                ReceberSessaoResponse lResSessao = ServicoSeguranca.ReceberSessao(new ReceberSessaoRequest()
                {
                    CodigoSessao = this.CodigoSessao,
                    CodigoSessaoARetornar = this.CodigoSessao
                });

                if (!lResSessao.Sessao.EhSessaoDeAdministrador)
                {
                    object[] attrs = this.GetType().GetCustomAttributes(typeof(ValidarSegurancaAttribute), true);

                    if (attrs.Length > 0)
                    {
                        List<ItemSegurancaInfo> list = new List<ItemSegurancaInfo>();

                        list.Add(((ValidarSegurancaAttribute)attrs[0]).Seguranca);
                        ValidarItemSegurancaRequest lRequestSeguranca = new ValidarItemSegurancaRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            ItensSeguranca = list
                        };

                        try
                        {
                            ValidarItemSegurancaResponse lResponseSeguranca = this.ServicoSeguranca.ValidarItemSeguranca(lRequestSeguranca);

                            if (lResponseSeguranca.StatusResposta == MensagemResponseStatusEnum.OK)
                            {
                                if (!lResponseSeguranca.ItensSeguranca[0].Valido.Value)
                                {   //--> Acesso Negado
                                    this.Response.Clear();
                                    this.Response.End();
                                }
                            }
                            else
                            {
                                this.Response.Clear();
                                this.Response.End();
                            }
                        }
                        //catch (CommunicationObjectFaultedException)
                        //{
                        //    Ativador.AbortChannel(this.ServicoSeguranca);
                        //    this.ServicoSeguranca = Ativador.Get<IServicoSeguranca>();
                        //}
                        catch (System.Threading.ThreadAbortException)
                        {

                        }
                        catch (Exception ex)
                        {
                            this.Response.Clear();

                            this.Response.Write(base.RetornarErroAjax(ex.Message));

                            this.Response.End();
                        }
                    }
                }
            }
        }

        #endregion

        #region VerificaPermissoesPagina
        /// <summary>
        /// Retorna uma lista com as permissões de cada item CRUD da página
        /// </summary>
        /// <param name="pList">Lista de permissões dos itens da página</param>
        public List<ItemSegurancaInfo> VerificaPermissoesPagina(List<ItemSegurancaInfo> pList)
        {
            ValidarItemSegurancaRequest lRequestSeguranca = new ValidarItemSegurancaRequest()
            {
                CodigoSessao = this.CodigoSessao,
                ItensSeguranca = pList
            };

            ValidarItemSegurancaResponse lResponseSeguranca = this.ServicoSeguranca.ValidarItemSeguranca(lRequestSeguranca);

            if (lResponseSeguranca.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponseSeguranca.ItensSeguranca.Count > 0)
                {
                    return lResponseSeguranca.ItensSeguranca;
                }
                else
                {
                    string lItensDoRequest = string.Empty;

                    foreach (ItemSegurancaInfo lItem in pList)
                        lItensDoRequest += string.Format("{0}-{1}, ", lItem.Tag, lItem.PermissoesString);

                    lItensDoRequest = lItensDoRequest.TrimEnd(", ".ToCharArray());

                    throw new Exception(string.Format("Sem itens de segurança configurados para [{0}]", lItensDoRequest));
                }
            }
            else
            {
                throw new Exception(string.Format("Erro do Response do serviço de mensageria: [{0}]", lResponseSeguranca.StatusResposta));
            }
        }

        public bool UsuarioPodeMesmo(string pGuidPermissao)
        {
            List<ItemSegurancaInfo> lList = new List<ItemSegurancaInfo>();

            lList.Add(new ItemSegurancaInfo());

            lList[0].PermissoesString = pGuidPermissao;

            ValidarItemSegurancaRequest lRequestSeguranca = new ValidarItemSegurancaRequest()
            {
                CodigoSessao = this.CodigoSessao,
                ItensSeguranca = lList
            };

            try
            {
                ValidarItemSegurancaResponse lResponseSeguranca = this.ServicoSeguranca.ValidarItemSeguranca(lRequestSeguranca);

                if (lResponseSeguranca.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponseSeguranca.ItensSeguranca[0].Valido.Value)
                    {
                        return true;
                    }
                    else
                    {
                        // Acesso Negado
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            catch (System.Threading.ThreadAbortException)
            {

            }

            return false;
        }

        public bool UsuarioPode(string pTagDeSeguranca, params string[] pGuidDePermissoes)
        {
            if (pGuidDePermissoes.Length == 0)
                throw new Exception("Deve haver ao menos uma GUID de permissão");

            List<ItemSegurancaInfo> lLista = new List<ItemSegurancaInfo>();

            ItemSegurancaInfo lItemSegurancaSalvar = new ItemSegurancaInfo();

            lItemSegurancaSalvar.Tag = pTagDeSeguranca;
            lItemSegurancaSalvar.Permissoes = new List<string>(pGuidDePermissoes);
            lItemSegurancaSalvar.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;

            lLista.Add(lItemSegurancaSalvar);

            lLista = this.VerificaPermissoesPagina(lLista);

            foreach (ItemSegurancaInfo lItem in lLista) if (lItem.Tag == pTagDeSeguranca)
                    return lItem.Valido.Value;

            throw new Exception(string.Format("Tag [{0}] não encontrada na lista de segurança"));
        }

        public bool UsuarioPode(string pTagDeSeguranca, List<string> pPerfis, params string[] pGuidDePermissoes)
        {
            if (pGuidDePermissoes.Length == 0)
                throw new Exception("Deve haver ao menos uma GUID de permissão");

            List<ItemSegurancaInfo> lLista = new List<ItemSegurancaInfo>();

            ItemSegurancaInfo lItemSegurancaSalvar = new ItemSegurancaInfo();

            lItemSegurancaSalvar.Tag = pTagDeSeguranca;
            lItemSegurancaSalvar.Permissoes = new List<string>(pGuidDePermissoes);
            lItemSegurancaSalvar.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;
            lItemSegurancaSalvar.Perfis = pPerfis;
            lLista.Add(lItemSegurancaSalvar);

            lLista = this.VerificaPermissoesPagina(lLista);

            foreach (ItemSegurancaInfo lItem in lLista) if (lItem.Tag == pTagDeSeguranca)
                    return lItem.Valido.Value;

            throw new Exception(string.Format("Tag [{0}] não encontrada na lista de segurança"));
        }

        #endregion

        #region | Formatacao

        public string AbreviarNumero(object pNumero)
        {
            double lNumero;

            if (double.TryParse(pNumero.ToString(), out lNumero))
            {
                return lNumero.ToNumeroAbreviado();
            }
            else
            {
                return pNumero.ToString();
            }
        }

        public string AbreviarNumero(double pNumero)
        {
            return pNumero.ToNumeroAbreviado();
        }

        public string AbreviarNumero(int pNumero)
        {
            return pNumero.ToNumeroAbreviado();
        }

        #endregion
    }
}
