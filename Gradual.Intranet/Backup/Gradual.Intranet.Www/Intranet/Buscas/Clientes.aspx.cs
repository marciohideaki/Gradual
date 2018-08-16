using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Clientes : PaginaBaseAutenticada
    {
        #region Propriedades

        private List<ClienteResumidoInfo> SessionUltimoResultadoDeBusca
        {
            get
            {
                return (List<ClienteResumidoInfo>)Session["UltimoResultadoDeBuscaDeClientes"];
            }

            set
            {
                Session["UltimoResultadoDeBuscaDeClientes"] = value;
            }
        }

        #endregion

        #region Métodos Private

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            var lRetorno = new TransporteDeListaPaginada();

            var lLista = new List<TransporteDadosResumidosCliente>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            for (int a = lIndiceInicial; a < lIndiceFinal; a++)
            {
                if (a < this.SessionUltimoResultadoDeBusca.Count)
                {
                    this.SessionUltimoResultadoDeBusca[a] =
                        VerificaContaInvestimento(this.SessionUltimoResultadoDeBusca[a]);

                    lLista.Add(new TransporteDadosResumidosCliente(this.SessionUltimoResultadoDeBusca[a]));
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        private ClienteResumidoInfo VerificaContaInvestimento(ClienteResumidoInfo pClienteResumido)
        {

            ClienteResumidoInfo lRetorno = pClienteResumido;
            if (pClienteResumido.Passo == "4")
            {
                try
                {
                    var lContas = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteContaInfo>(new ConsultarEntidadeCadastroRequest<ClienteContaInfo>()
                    {
                        IdUsuarioLogado = base.UsuarioLogado.Id,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        EntidadeCadastro = new ClienteContaInfo()
                        {
                            IdCliente = pClienteResumido.IdCliente
                        }
                    });

                    lRetorno.VerificarTiposDeContas(lContas.Resultado);

                    /*
                    foreach (ClienteContaInfo item in lContas.Resultado)
                    {
                        if (!string.IsNullOrWhiteSpace(pClienteResumido.CodBovespa))
                        {
                            if (int.Parse(pClienteResumido.CodBovespa.Replace("-CI", "")) == item.CdCodigo && item.CdSistema == eAtividade.BOL && item.StContaInvestimento)
                            {
                                lRetorno.CodBovespa = lRetorno.CodBovespa + "-CI";
                                lRetorno.CodGradual = lRetorno.CodGradual + "-CI";
                                lRetorno.CodBovespa = lRetorno.CodBovespa.Replace("-CI-CI", "-CI");
                                lRetorno.CodGradual = lRetorno.CodGradual.Replace("-CI-CI", "-CI");
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(pClienteResumido.CodBMF))
                        {
                            if (int.Parse(pClienteResumido.CodBMF.Replace("-CI", "")) == item.CdCodigo && item.CdSistema == eAtividade.BMF && item.StContaInvestimento)
                            {
                                lRetorno.CodBMF = lRetorno.CodBMF + "-CI";
                                lRetorno.CodBMF = lRetorno.CodBMF.Replace("-CI-CI", "-CI");
                            }
                        }

                    }*/
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return lRetorno;
        }

        private string ResponderBuscarItensParaSelecao()
        {
            string lRetorno = "Erro...";

            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteResumidoInfo>();
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteResumidoInfo>();
            var lDadosDeBusca = new ClienteResumidoInfo();

            string lTermoDeBusca, lBuscarPor, lTipo;

            bool lStatus_Ativo = false, lStatus_Inativo = false,
                 lPasso_Visitante = false, lPasso_Cadastrado = false, lPasso_ExportadoSinacor = false,
                 lPendencia_ComPendenciaCadastral = false, lPendencia_ComSolicitacaoAlteracao = false;

            lTermoDeBusca = Request.Form["TermoDeBusca"];
            lBuscarPor = Request.Form["BuscarPor"];
            lTipo = Request.Form["Tipo"];

            try
            {
                lStatus_Ativo = Convert.ToBoolean(Request.Form["Status_Ativo"]);
                lStatus_Inativo = Convert.ToBoolean(Request.Form["Status_Inativo"]);

                lPasso_Visitante = Convert.ToBoolean(Request.Form["Passo_Visitante"]);
                lPasso_Cadastrado = Convert.ToBoolean(Request.Form["Passo_Cadastrado"]);
                lPasso_ExportadoSinacor = Convert.ToBoolean(Request.Form["Passo_ExportadoSinacor"]);

                lPendencia_ComPendenciaCadastral = Convert.ToBoolean(Request.Form["Pendencia_ComPendenciaCadastral"]);
                lPendencia_ComSolicitacaoAlteracao = Convert.ToBoolean(Request.Form["Pendencia_ComSolicitacaoAlteracao"]);

            }
            catch { }

            lDadosDeBusca.TipoCliente = lTipo;

            lDadosDeBusca.TermoDeBusca = lTermoDeBusca;

            try
            {
                lDadosDeBusca.OpcaoBuscarPor = (OpcoesBuscarPor)Enum.Parse(typeof(OpcoesBuscarPor), lBuscarPor);
            }
            catch (Exception)
            {
                lDadosDeBusca.OpcaoBuscarPor = OpcoesBuscarPor.NomeCliente;
            }

            {   //--> Setando o status da consulta
                if (lStatus_Ativo && lStatus_Inativo)
                    lDadosDeBusca.OpcaoStatus = lDadosDeBusca.OpcaoStatus;

                else if (lStatus_Ativo)
                    lDadosDeBusca.OpcaoStatus = OpcoesStatus.Ativo;

                else if (lStatus_Inativo)
                    lDadosDeBusca.OpcaoStatus = OpcoesStatus.Inativo;
            }

            {   //--> Definindo o passo do cadastro

                if (lPasso_Visitante && lPasso_Cadastrado && lPasso_ExportadoSinacor)
                    lDadosDeBusca.OpcaoPasso = OpcoesPasso.Visitante | OpcoesPasso.Cadastrado | OpcoesPasso.Exportado;

                else if (lPasso_Visitante && lPasso_Cadastrado)
                    lDadosDeBusca.OpcaoPasso = OpcoesPasso.Visitante | OpcoesPasso.Cadastrado;

                else if (lPasso_Visitante && lPasso_ExportadoSinacor)
                    lDadosDeBusca.OpcaoPasso = OpcoesPasso.Visitante | OpcoesPasso.Exportado;

                else if (lPasso_Cadastrado && lPasso_ExportadoSinacor)
                    lDadosDeBusca.OpcaoPasso = OpcoesPasso.Cadastrado | OpcoesPasso.Exportado;

                else if (lPasso_Visitante)
                    lDadosDeBusca.OpcaoPasso = OpcoesPasso.Visitante;

                else if (lPasso_Cadastrado)
                    lDadosDeBusca.OpcaoPasso = OpcoesPasso.Cadastrado;

                else if (lPasso_ExportadoSinacor)
                    lDadosDeBusca.OpcaoPasso = OpcoesPasso.Exportado;
            }

            if (lPendencia_ComPendenciaCadastral && !lPendencia_ComSolicitacaoAlteracao)
            {
                lDadosDeBusca.OpcaoPendencia = OpcoesPendencia.ComPendenciaCadastral;
            }
            else if (!lPendencia_ComPendenciaCadastral && lPendencia_ComSolicitacaoAlteracao)
            {
                lDadosDeBusca.OpcaoPendencia = OpcoesPendencia.ComSolicitacaoAlteracao;
            }
            else if (!lPendencia_ComPendenciaCadastral && !lPendencia_ComSolicitacaoAlteracao)
            {
                lDadosDeBusca.OpcaoPendencia = 0;
            }

            lRequest.EntidadeCadastro = lDadosDeBusca;

            try
            {
                ReceberEntidadeCadastroRequest<LoginInfo> lEntradaLogin = new ReceberEntidadeCadastroRequest<LoginInfo>();

                lEntradaLogin.EntidadeCadastro = new LoginInfo() { IdLogin = base.UsuarioLogado.Id };

                ReceberEntidadeCadastroResponse<LoginInfo> lRetornoLogin = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<LoginInfo>(lEntradaLogin);

                if (lRetornoLogin.EntidadeCadastro.TpAcesso == eTipoAcesso.Assessor)
                    lRequest.EntidadeCadastro.CodAssessor = lRetornoLogin.EntidadeCadastro.CdAssessor;
                else
                    lRequest.EntidadeCadastro.CodAssessor = null;

                //--> Realizando a Consulta
                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteResumidoInfo>(lRequest);

                if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                {
                    if (lDadosDeBusca.OpcaoBuscarPor == OpcoesBuscarPor.CodBovespa)
                    {
                        if (lResponse.Resultado != null && lResponse.Resultado.Count == 1
                        && (lResponse.Resultado[0].CodGradual != lDadosDeBusca.TermoDeBusca))
                        {
                            var lContas = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteContaInfo>(
                                new ConsultarEntidadeCadastroRequest<ClienteContaInfo>()
                                {
                                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                    IdUsuarioLogado = base.UsuarioLogado.Id,
                                    EntidadeCadastro = new ClienteContaInfo()
                                    {
                                        IdCliente = lResponse.Resultado[0].IdCliente
                                    }
                                });

                            foreach (ClienteContaInfo item in lContas.Resultado)
                            {
                                if (item.CdCodigo == int.Parse(lDadosDeBusca.TermoDeBusca)
                                && (item.CdSistema == eAtividade.BOL)
                                && (item.StContaInvestimento))
                                {
                                    lResponse.Resultado[0].CodGradual = lDadosDeBusca.TermoDeBusca;
                                    lResponse.Resultado[0].CodBovespa = lDadosDeBusca.TermoDeBusca;
                                    if (item.StAtiva)
                                        lResponse.Resultado[0].CodBovespaAtiva = "A";
                                    else
                                        lResponse.Resultado[0].CodBovespaAtiva = "I";
                                }
                            }
                        }
                    }

                    this.SessionUltimoResultadoDeBusca = lResponse.Resultado;

                    TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

                    lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontrados [{0}] clientes", lResponse.Resultado.Count);
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro durante a busca.", string.Format("{0}\r\n\r\n{1}\r\n\r\n{2}", lResponse.StatusResposta, lResponse.StatusResposta, lResponse.DescricaoResposta));
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
            else
            {
                //lLista ;
            }

            lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax( new string[] { 
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
