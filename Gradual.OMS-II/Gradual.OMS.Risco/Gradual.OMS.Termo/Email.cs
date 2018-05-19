using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.OMS.Termo.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;
using System.Configuration;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Library;

namespace Gradual.OMS.Termo
{
    public class Email
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Email()
        {
            log4net.Config.XmlConfigurator.Configure();
        }


        public void EnviarEmail(ClienteGarantiaRequest request)
        {
            StringBuilder _EmailBuilder = new StringBuilder();

            try
            {

                _EmailBuilder.Append("<P> SEGUE ABAIXO E-MAIL REFERENTE A ALOCAÇÃO DE ATIVO PARA COBERTURA DE OPERAÇÃO A TERMO </P>");

                    
                _EmailBuilder.Append("<BR>");
                _EmailBuilder.Append("-------------  [SOLICITACAO DE TRANSFERENCIA DE CARTEIRA PARA COBERTURA DE OPERAÇÃO A TERMO] ------------- <BR>");
                _EmailBuilder.Append("<B> Horario da solicitação </B>       : " + request.ClienteGarantiaInfo.DataSolicitacao + "<BR>");
                _EmailBuilder.Append("<B> Hora Atual</B>            : " + DateTime.Now + "<BR>");
                _EmailBuilder.Append("<B> Código do Cliente </B>    : " + request.ClienteGarantiaInfo.IdCliente.ToString() + "<BR>");
                _EmailBuilder.Append("<B> Instrumento</B>           : " + request.ClienteGarantiaInfo.Instrumento + "<BR>");
                _EmailBuilder.Append("<B> Quantidade </B>           : " + request.ClienteGarantiaInfo.Quantidade + "<BR>");
                _EmailBuilder.Append("<B> Função </B>               : Cobertura de termo ");    
                _EmailBuilder.Append("</BR>");

              

                var lServico = Ativador.Get<IServicoEmail>();

                logger.InfoFormat("Conseguiu instanciar o serviço Ativador.Get<IServicoEmail>");

                string lEmailDestinatario = ConfigurationManager.AppSettings["EmailDestinatario"].ToString();

                var lEmail = new EnviarEmailRequest();

                lEmail.Objeto = new OMS.Email.Lib.EmailInfo();

                lEmail.Objeto.Assunto = "Notificação de solicitação de transferencia de custódia [TERMO]";

                lEmail.Objeto.Destinatarios = new List<string>();

                lEmail.Objeto.Destinatarios.Add(lEmailDestinatario);

                logger.InfoFormat(string.Format("Enviando e-mail de notificação de ativação de plano para para {0}", lEmailDestinatario));

                lEmail.Objeto.Remetente = ConfigurationManager.AppSettings["EmailRemetenteNotificacao"].ToString();

                lEmail.Objeto.CorpoMensagem = _EmailBuilder.ToString();

                logger.InfoFormat("Entrou no método de EnviaEmailClienteEstornos");

                EnviarEmailResponse lResponse = lServico.Enviar(lEmail);

                if (lResponse.StatusResposta.Equals(MensagemResponseStatusEnum.OK))
                {
                    logger.Info("Email disparado com sucesso (Garantia de Termo) ");
                }
                else
                {
                    logger.ErrorFormat("O e-mail não foi disparado - Descrição: {0} ");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar o E-MAIL de solicitação de alocação para garantia de termo para os destinatarios", ex);
            }

        }
    }
}
