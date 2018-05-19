using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Gradual.Intranet.Contratos;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Risco;
using log4net;
using System.Configuration;
using System.IO;
using Gradual.OMS.Email.Lib;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;

namespace Gradual.OMS.WsIntegracao
{
    /// <summary>
    /// Web Service de integração com o Cadastro
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Cadastro : System.Web.Services.WebService
    {
        #region Propriedades

        private static ILog _Logger = null;

        private static ILog Logger
        {
            get
            {
                if (_Logger == null)
                {
                    log4net.Config.XmlConfigurator.Configure();

                    _Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }

                return _Logger;
            }
        }

        #endregion

        #region Métodos Private

        public void EnviarEmail(string pDestinatario, string pAssunto, string pNomeArquivo, Dictionary<string, string> pVariaveisEmail, eTipoEmailDisparo pTipoEmailDisparo, List<Gradual.OMS.Email.Lib.EmailAnexoInfo> pAnexos = null)
        {
            try
            {
                string lCorpoDoEmail = File.ReadAllText(Server.MapPath(string.Format("Resc/{0}", pNomeArquivo)));

                List<string> lDestinatarios = new List<string>();

                lDestinatarios.Add(pDestinatario);

                IServicoEmail lServico = Ativador.Get<IServicoEmail>();

                EnviarEmailRequest lRequest = new EnviarEmailRequest();
                EnviarEmailResponse lResponse;

                lRequest.Objeto               = new EmailInfo();
                lRequest.Objeto.Assunto       = pAssunto;
                lRequest.Objeto.Destinatarios = lDestinatarios;
                lRequest.Objeto.Remetente     = ConfigurationManager.AppSettings["EmailRemetenteGradual"];
                lRequest.Objeto.CorpoMensagem = lCorpoDoEmail;

                if (pAnexos != null)
                {
                    foreach (var lItem in pAnexos)
                        lRequest.Objeto.Anexos.Add(lItem);
                }

                lResponse = lServico.Enviar(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    Logger.InfoFormat("Email enviado com sucesso para [{0}]", pDestinatario);
                }
                else
                {
                    Logger.ErrorFormat("Resposta com erro do serviço de email [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Erro ao enviar email [{0}]\r\n{1}", ex.Message, ex.StackTrace);

            }
        }

        private void EnviarEmailComPerfilDoInvestidor(string pEmailDestinatario)
        {
            try
            {
                List<EmailAnexoInfo> lAnexos = new List<EmailAnexoInfo>();

                lAnexos.Add(new EmailAnexoInfo()
                {
                    Nome = "Resultado_Arrojado.pdf",
                    Arquivo = File.ReadAllBytes(Server.MapPath("Resc/Resultado_Arrojado.pdf")),
                });

                EnviarEmail(   pEmailDestinatario
                            , "Perfil do Investidor | Confira o seu portfólio recomendado"
                            , "EmailSuitability.html"
                            , new Dictionary<string, string>()
                            , Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos
                            , lAnexos);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Erro ao montar email [{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Métodos Públicos

        [WebMethod]
        public BuscarListaDeContratosResponse BuscarListaDeContratos(BuscarListaDeContratosRequest pRequest)
        {
            BuscarListaDeContratosResponse lResponse = new BuscarListaDeContratosResponse();

            try
            {
                ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

                ConsultarEntidadeRequest<ClienteContratoInfo> lRequestCadastro = new ConsultarEntidadeRequest<ClienteContratoInfo>();
                ConsultarObjetosResponse<ClienteContratoInfo> lResponseCadastro;

                lRequestCadastro.Objeto = new ClienteContratoInfo() { CodigoBovespaCliente = pRequest.CodigoBovespaDoCliente };

                Logger.InfoFormat("Executando ClienteDbLib.ConsultarClienteContrato({0})", pRequest.CodigoBovespaDoCliente);

                lResponseCadastro = Gradual.Intranet.Servicos.BancoDeDados.Persistencias.ClienteDbLib.ConsultarClienteContrato(lRequestCadastro);

                lResponse.Contratos = lResponseCadastro.Resultado;

                Logger.InfoFormat("[{0}] contratos retornados", lResponseCadastro.Resultado.Count);

                /*
                lResponse.IDsDosContratos = new List<int>();

                foreach (ClienteContratoInfo lInfo in lResponse.Contratos)
                {
                    lResponse.IDsDosContratos.Add(lInfo.IdContrato);
                }
                */

                lResponse.StatusResposta = "OK";
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Erro executando ClienteDbLib.ConsultarClienteContrato: [{0}] [{1}]", ex.Message, ex.StackTrace);

                lResponse.StatusResposta = "ERRO";

                lResponse.DescricaoResposta = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        [WebMethod]
        public string TestarBuscarListaDeContratos(string pCodigoBovespaDoCliente)
        {
            string lRetorno;

            try
            {
                BuscarListaDeContratosRequest lRequest = new BuscarListaDeContratosRequest();
                BuscarListaDeContratosResponse lResponse;

                string lLista = "";

                lRequest.CodigoBovespaDoCliente = Convert.ToInt32(pCodigoBovespaDoCliente);

                lResponse = BuscarListaDeContratos(lRequest);

                if (lResponse.StatusResposta == "OK")
                {
                    foreach (ClienteContratoInfo lContrato in lResponse.Contratos)
                    {
                        lLista += string.Format("{0}, ", lContrato.IdContrato);
                    }

                    lLista = lLista.TrimEnd().TrimEnd(',');
                }

                lRetorno = string.Format("Response: [{0}] [{1}] [{2}]", lResponse.StatusResposta, lResponse.DescricaoResposta, lLista);
            }
            catch (Exception ex)
            {
                lRetorno = string.Format("Deu exception! [{0}]\r\n[{1}]", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        [WebMethod]
        public CadastrarAdesaoAoContratoResponse CadastrarAdesaoAoContrato(CadastrarAdesaoAoContratoRequest pRequest)
        {
            CadastrarAdesaoAoContratoResponse lResponse = new CadastrarAdesaoAoContratoResponse();

            try
            {
                ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

                SalvarEntidadeRequest<ClienteContratoInfo> lRequestCadastro = new SalvarEntidadeRequest<ClienteContratoInfo>();
                SalvarEntidadeResponse<ClienteContratoInfo> lResponseCadastro;

                lRequestCadastro.Objeto = new ClienteContratoInfo();

                lRequestCadastro.Objeto.LstIdContrato = new List<int>();
                lRequestCadastro.Objeto.LstIdContrato.Add(pRequest.IdDoContratoNoCadastro);

                lRequestCadastro.Objeto.CodigoBovespaCliente = pRequest.CodigoBovespaDoCliente;
                lRequestCadastro.Objeto.IdContrato = pRequest.IdDoContratoNoCadastro;
                lRequestCadastro.Objeto.DtAssinatura = DateTime.Now;

                Logger.InfoFormat("Executando ClienteDbLib.SalvarClienteContrato(CodigoBovespaCliente: [{0}], IdContrato: [{1}])", pRequest.CodigoBovespaDoCliente, pRequest.IdDoContratoNoCadastro);

                lResponseCadastro = Gradual.Intranet.Servicos.BancoDeDados.Persistencias.ClienteDbLib.SalvarClienteContrato(lRequestCadastro);

                int lIdContratoAlavancagem = Convert.ToInt32(ConfigurationManager.AppSettings["IdContratoTermoAlavancagemFinanceira"]);

                if (pRequest.IdDoContratoNoCadastro == lIdContratoAlavancagem)   //verificar se é contrato de adesão termo alavancagem
                {
                    Logger.InfoFormat("Atualizando suitability pelo contrato de adesão (ID [{0}])", lIdContratoAlavancagem);

                    Gradual.Intranet.Contratos.IServicoPersistenciaCadastro lServicoPersistenciaCadastro = Ativador.Get<Gradual.Intranet.Contratos.IServicoPersistenciaCadastro>();

                    SalvarEntidadeCadastroRequest<ClienteSuitabilityInfo> lRequestSuitability = new SalvarEntidadeCadastroRequest<ClienteSuitabilityInfo>();
                    SalvarEntidadeCadastroResponse lResponseSuitability;

                    lRequestSuitability.EntidadeCadastro = new ClienteSuitabilityInfo();

                    lRequestSuitability.EntidadeCadastro.ds_fonte                  = "Termo-Alavancagem";
                    lRequestSuitability.EntidadeCadastro.ds_loginrealizado         = pRequest.NomeDoCliente;
                    lRequestSuitability.EntidadeCadastro.ds_perfil                 = "Arrojado";
                    lRequestSuitability.EntidadeCadastro.ds_respostas              = "";
                    lRequestSuitability.EntidadeCadastro.ds_status                 = "Finalizado";
                    lRequestSuitability.EntidadeCadastro.dt_realizacao             = DateTime.Now;
                    //lRequestSuitability.EntidadeCadastro.IdCliente                 = Convert.ToInt32(pRequest.IdDoClienteNoBanco);
                    lRequestSuitability.EntidadeCadastro.CdCblc                    = Convert.ToInt32(pRequest.CodigoBovespaDoCliente);
                    lRequestSuitability.EntidadeCadastro.st_preenchidopelocliente  = true;

                    lResponseSuitability = lServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteSuitabilityInfo>(lRequestSuitability);

                    if (lResponseSuitability.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        Logger.InfoFormat("Resposta ok do IServicoPersistenciaCadastro ao cadastrar suitability para [{0}] [{1}]", pRequest.IdDoClienteNoBanco, pRequest.CodigoBovespaDoCliente);

                        this.EnviarEmailComPerfilDoInvestidor(pRequest.EmailDoCliente);

                        Logger.Info("Resposta OK");

                        lResponse.StatusResposta = "OK";
                    }
                    else
                    {
                        Logger.ErrorFormat("Resposta com erro do IServicoPersistenciaCadastro ao cadastrar suitability para [{0}] [{1}]: [{2}]\r\n{3}"
                                    , pRequest.IdDoClienteNoBanco
                                    , pRequest.CodigoBovespaDoCliente
                                    , lResponseSuitability.StatusResposta
                                    , lResponseSuitability.DescricaoResposta);

                        lResponse.StatusResposta = "ERRO";

                        lResponse.DescricaoResposta = "Resposta com erro do IServicoPersistenciaCadastro ao cadastrar suitability";
                    }
                }
                else
                {
                    lResponse.StatusResposta = "OK";

                    lResponse.DescricaoResposta = "OK";
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Erro executando ClienteDbLib.SalvarClienteContrato: [{0}] [{1}]", ex.Message, ex.StackTrace);

                lResponse.StatusResposta = "ERRO";

                lResponse.DescricaoResposta = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
            }


            return lResponse;
        }

        [WebMethod]
        public string TestarCadastrarAdesaoAoContratoStop(string pCodigoBovespaDoCliente, string pIdDoContrato)
        {
            string lRetorno;

            try
            {
                CadastrarAdesaoAoContratoRequest lRequest = new CadastrarAdesaoAoContratoRequest();
                CadastrarAdesaoAoContratoResponse lResponse;

                lRequest.CodigoBovespaDoCliente = Convert.ToInt32(pCodigoBovespaDoCliente);
                lRequest.IdDoContratoNoCadastro = Convert.ToInt32(pIdDoContrato);

                lResponse = CadastrarAdesaoAoContrato(lRequest);

                lRetorno = string.Format("Response: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }
            catch (Exception ex)
            {
                lRetorno = string.Format("Deu exception! [{0}]\r\n[{1}]", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        [WebMethod]
        public string TestarCadastrarAdesaoAoContrato(string pIdDoClienteNoBanco, string pCodigoBovespaDoCliente, string pEmailDoCliente, string pNomeDoCliente)
        {
            string lRetorno;

            try
            {
                CadastrarAdesaoAoContratoRequest lRequest = new CadastrarAdesaoAoContratoRequest();
                CadastrarAdesaoAoContratoResponse lResponse;

                lRequest.CodigoBovespaDoCliente = Convert.ToInt32(pCodigoBovespaDoCliente);
                lRequest.EmailDoCliente         = pEmailDoCliente;
                lRequest.IdDoClienteNoBanco     = pIdDoClienteNoBanco;
                lRequest.IdDoContratoNoCadastro = Convert.ToInt32(ConfigurationManager.AppSettings["IdContratoTermoAlavancagemFinanceira"]);
                lRequest.NomeDoCliente          = pNomeDoCliente;

                lResponse = CadastrarAdesaoAoContrato(lRequest);

                lRetorno = string.Format("Response: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }
            catch (Exception ex)
            {
                lRetorno = string.Format("Deu exception! [{0}]\r\n[{1}]", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        [WebMethod]
        public BuscarContratoResponse BuscarContrato(BuscarContratoRequest pRequest)
        {
            BuscarContratoResponse lResponse = new BuscarContratoResponse();

            try
            {
                ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

                ReceberEntidadeRequest<ArquivoContratoInfo> lRequestArquivo = new ReceberEntidadeRequest<ArquivoContratoInfo>();
                ReceberObjetoResponse<ArquivoContratoInfo> lResponseArquivo;

                lRequestArquivo.Objeto = new ArquivoContratoInfo() { IdArquivoContrato = pRequest.IdDoContrato};

                Logger.InfoFormat("Executando ClienteDbLib.ReceberArquivosContratos(IdArquivoContrato: [{0}])", pRequest.IdDoContrato);

                lResponseArquivo = Gradual.Intranet.Servicos.BancoDeDados.Persistencias.ClienteDbLib.ReceberArquivosContratos(lRequestArquivo);

                lResponse.TextoDoContrato = System.Text.UTF8Encoding.UTF8.GetString(lResponseArquivo.Objeto.Arquivo);

                Logger.Info("Resposta OK");

                lResponse.StatusResposta = "OK";
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Erro executando ClienteDbLib.ReceberArquivosContratos: [{0}] [{1}]", ex.Message, ex.StackTrace);

                lResponse.StatusResposta = "ERRO";

                lResponse.DescricaoResposta = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        [WebMethod]
        public BuscarLimitesDoClienteResposta BuscarLimitesDoCliente(BuscarLimitesDoClienteRequest pRequest)
        {
            BuscarLimitesDoClienteResposta lResponse = new BuscarLimitesDoClienteResposta();

            try
            {
                ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

                IServicoPersistenciaCadastro lServico;

                RiscoLimiteAlocadoInfo lInfo = new RiscoLimiteAlocadoInfo();

                lInfo.ConsultaIdCliente = pRequest.CodigoBovespaDoCliente;

                ConsultarEntidadeCadastroRequest<RiscoLimiteAlocadoInfo> lRequestServico = new ConsultarEntidadeCadastroRequest<RiscoLimiteAlocadoInfo>(lInfo);
                ConsultarEntidadeCadastroResponse<RiscoLimiteAlocadoInfo> lResponseServico;

                lServico = Ativador.Get<IServicoPersistenciaCadastro>();

                Logger.InfoFormat("Executando ConsultarEntidadeCadastro<RiscoLimiteAlocadoInfo>(CodigoBovespaDoCliente: [{0}])", pRequest.CodigoBovespaDoCliente);

                lResponseServico = lServico.ConsultarEntidadeCadastro<RiscoLimiteAlocadoInfo>(lRequestServico);

                if (lResponseServico.StatusResposta == Library.MensagemResponseStatusEnum.OK)
                {
                    lResponse.ReceberDadosDeLimite(lResponseServico.Resultado);

                    Logger.Info("Resposta OK");

                    lResponse.StatusResposta = "OK";
                }
                else
                {
                    Logger.ErrorFormat("Resposta com erro do serviço: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);

                    lResponse.StatusResposta = "ERRO";
                    lResponse.DescricaoResposta = lResponseServico.DescricaoResposta;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Erro executando ConsultarEntidadeCadastro<RiscoLimiteAlocadoInfo>: [{0}] [{1}]", ex.Message, ex.StackTrace);

                lResponse.StatusResposta = "ERRO";

                lResponse.DescricaoResposta = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        #endregion
    }
}
