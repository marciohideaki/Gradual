using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using System.Configuration;
using Gradual.OMS.Email.Lib;
using log4net;
using Gradual.OMS.Library;
using Gradual.Monitores.Risco.Lib;

namespace Gradual.Monitores.Risco
{
    public class EmailInfo
    {
         private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EmailInfo()
        {
               //log4net.Config.XmlConfigurator.Configure();  
        }


        public void EnviarEmail(List<PLDOperacaoInfo> ListaPLD )
        {
           StringBuilder _EmailBuilder = new StringBuilder();

           try
           {

               _EmailBuilder.Append("<P> Segue abaixo relação de posições PLD que se encontram em estado de analise. </P>");

               foreach (var item in ListaPLD)
               {
                   string HoraNegocio = string.Format("{0:T}", item.HR_NEGOCIO).ToString();
                   string HoraAtual = string.Format("{0:T}", DateTime.Now).ToString();
                   string MinutosRestantes = string.Format("{0:T}", item.MinutosRestantesPLD).ToString();
                   string NumeroNegocio = item.NumeroNegocio.ToString();
                   string Instrumento = item.Intrumento;
                   string IntencaoPLD = item.IntencaoPLD;
                   string PrecoNegocio = item.PrecoNegocio.ToString();
                   string PrecoMercado = string.Format("{0:C}", item.PrecoMercado).ToString();
                   string LucroPrejuiso = string.Format("{0:C}", item.LucroPrejuiso).ToString();
                   string Quantidade = item.Quantidade.ToString();
                   string QuantidadeCasada = item.QT_CASADA.ToString();
                   string Sentido = item.Sentido.ToString();
                   string Status = item.STATUS.ToString();
                   string DescricaoCriticidade = item.DescricaoCriticidade;

                   _EmailBuilder.Append("<BR>");
                   _EmailBuilder.Append("-------------  [" + NumeroNegocio + "] ------------- <BR>");
                   _EmailBuilder.Append("<B> Hora do negócio</B>       : " + HoraNegocio + "<BR>");
                   _EmailBuilder.Append("<B> Hora Atual</B>            : " + HoraAtual + "<BR>");
                   _EmailBuilder.Append("<B> Minutos Restantes         : <font color='red'> " + MinutosRestantes + "</font></B> <BR>");
                   _EmailBuilder.Append("<B> Numero do negócio</B>     : " + NumeroNegocio + "<BR>");
                   _EmailBuilder.Append("<B> Instrumento</B>           : " + Instrumento + "<BR>");
                   _EmailBuilder.Append("<B> Intencao de PLD</B>       : " + IntencaoPLD + "<BR>");
                   _EmailBuilder.Append("<B> Preço Negócio</B>         : " + PrecoNegocio + "<BR>");
                   _EmailBuilder.Append("<B> Preço Mercado</B>         : " + PrecoMercado + "<BR>");
                   _EmailBuilder.Append("<B> Lucro Prejuiso</B>        : " + LucroPrejuiso + "<BR>");
                   _EmailBuilder.Append("<B> Quantidade</B>            : " + Quantidade + "<BR>");
                   _EmailBuilder.Append("<B> Quantidade Casada</B>     : " + QuantidadeCasada + "<BR>");
                   _EmailBuilder.Append("<B> Sentido Operacação</B>    : " + Sentido + "<BR>");
                   _EmailBuilder.Append("<B> Status do PLD</B>         : " + Status + "<BR>");
                   _EmailBuilder.Append("<B> Nível Criticidade</B>     : <font color='red'> " + DescricaoCriticidade + "</font></B> <BR>");
                   _EmailBuilder.Append("</BR>");

               }

               var lServico = Ativador.Get<IServicoEmail>();

               logger.InfoFormat("Conseguiu instanciar o serviço Ativador.Get<IServicoEmail>");

               string lEmailDestinatario = ConfigurationManager.AppSettings["EmailDestinatario"].ToString();

               var lEmail = new EnviarEmailRequest();

               lEmail.Objeto = new OMS.Email.Lib.EmailInfo();

               lEmail.Objeto.Assunto = "Alerta de PLD [ " + ListaPLD.Count + " ] posições a serem analisadas.";

               lEmail.Objeto.Destinatarios = new List<string>();

               lEmail.Objeto.Destinatarios.Add(lEmailDestinatario);

               logger.InfoFormat(string.Format("Enviando e-mail de notificação de ativação de plano para para {0}", lEmailDestinatario));

               lEmail.Objeto.Remetente = ConfigurationManager.AppSettings["EmailRemetenteNotificacao"].ToString();

               lEmail.Objeto.CorpoMensagem = _EmailBuilder.ToString();

               logger.InfoFormat("Entrou no método de EnviaEmailClienteEstornos");

               EnviarEmailResponse lResponse = lServico.Enviar(lEmail);

               if (lResponse.StatusResposta.Equals(MensagemResponseStatusEnum.OK))
               {
                   logger.Info("Email disparado com sucesso ( PLD ) ");
               }
               else
               {
                   logger.ErrorFormat("O e-mail não foi disparado - Descrição: {0} ");
               }
           }
           catch (Exception ex)
           {
               logger.Info("Ocorreu um erro ao enviar o E-MAIL de PLD para os destinatarios", ex);
           }

        }
    }
}
