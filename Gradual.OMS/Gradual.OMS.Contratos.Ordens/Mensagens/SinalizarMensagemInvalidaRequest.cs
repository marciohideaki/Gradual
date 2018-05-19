using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Utilizada, basicamente, pelos canais para sinalizar alguma mensagem 
    /// inválida que foi enviada. 
    /// Por exemplo, o sistema de ordens pode fazer a solicitação de uma 
    /// execução de ordem enviando uma mensagem com formato inválido. 
    /// Como retorno, será enviado uma mensagem de mensagem inválida e o 
    /// serviço de canais irá informar o servidor de ordens através desta 
    /// mensagem.
    /// 
    /// Utilizada para retornar mensagens do tipo Reject para a aplicação
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Mensagem(TipoMensagemResponse = typeof(SinalizarMensagemInvalidaResponse))]
    public class SinalizarMensagemInvalidaRequest : MensagemSinalizacaoBase
    {
        public string CodigoCanal { get; set; }
        public string CodigoMensagemOrigem { get; set; }
        public string ClOrdID { get; set; }
        public string OrderID { get; set; }
        public object Dados { get; set; }
        public string Descricao { get; set; }
        public Exception Erro { get; set; }

        #region Bloco Mensagem Reject

        /// <summary>
        /// Número de sequencia da mensagem de referência rejeitada
        /// </summary>
        [Category("Bloco Reject")]
        [Description("Número de sequencia da mensagem de referência rejeitada")]
        public int RefSeqNum { get; set; }

        /// <summary>
        /// Número do campo FIX da referência
        /// </summary>
        [Category("Bloco Reject")]
        [Description("Número do campo FIX da referência")]
        public int RefTagID { get; set; }

        /// <summary>
        /// Tipo de mensagem FIX de referência
        /// </summary>
        [Category("Bloco Reject")]
        [Description("Tipo de mensagem FIX de referência")]
        public string RefMsgType { get; set; }

        /// <summary>
        /// Código para identificar o motivo da mensagem de rejeição no nível de sessão. Valores aceitos:
        /// 0 = número de campo inválido; 1 = falta campo obrigatório; 2 = campo não definido para este tipo de mensagem;
        /// 3 = campo não definido; 4 = campo especificado sem valor; 5 = valor incorreto (fora da faixa) para campo;
        /// 6 = formato incorreto dos dados para valor; 9 = problemas com CompID; 10 = problema de precisão no horário de envio;
        /// 11 = tipo de mensagem inválido; 13 = campo aparece mais de uma vez; 14 = campo especificado fora da ordem necessária;
        /// 15 = campos de grupo de repetição fora de ordem; 16 = contagem incorreta do número de registros (NumInGroup) para grupo de repetição;
        /// 17 = valor de dados não-numéricos inclui delimitador de campo (delimitador SOH); 99 = outros
        /// </summary>
        [Category("Bloco Reject")]
        [Description("Código para identificar o motivo da mensagem de rejeição no nível de sessão. Valores aceitos: 0 = número de campo inválido; 1 = falta campo obrigatório; 2 = campo não definido para este tipo de mensagem; 3 = campo não definido; 4 = campo especificado sem valor; 5 = valor incorreto (fora da faixa) para campo; 6 = formato incorreto dos dados para valor; 9 = problemas com CompID; 10 = problema de precisão no horário de envio; 11 = tipo de mensagem inválido; 13 = campo aparece mais de uma vez; 14 = campo especificado fora da ordem necessária; 15 = campos de grupo de repetição fora de ordem; 16 = contagem incorreta do número de registros (NumInGroup) para grupo de repetição; 17 = valor de dados não-numéricos inclui delimitador de campo (delimitador SOH); 99 = outros")]
        public int SessionRejectReason { get; set; }

        /// <summary>
        /// Sempre que possível, mensagem para explicar o motivo da rejeição
        /// </summary>
        [Category("Bloco Reject")]
        [Description("Sempre que possível, mensagem para explicar o motivo da rejeição")]
        public string Text { get; set; }

        #endregion
    }
}
