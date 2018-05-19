using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de execução de diretos
    /// Equivalente à mensagem NewOrderCross do Fix
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Mensagem(TipoMensagemResponse = typeof(ExecutarDiretoResponse))]
    public class ExecutarDiretoRequest : MensagemOrdemRequestBase
    {
        #region Bloco de Identificacao do Instrumento

        /// <summary>
        /// Símbolo. A BM&FBOVESPA exige que esse campo seja adequadamente preenchido. 
        /// Ele contém a forma inteligível do campo SecurityID, disponível na mensagem de 
        /// lista de instrumentos.
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("Símbolo. A BM&FBOVESPA exige que esse campo seja adequadamente preenchido. Ele contém a forma inteligível do campo SecurityID, disponível na mensagem de lista de instrumentos.")]
        public string Symbol { get; set; }

        /// <summary>
        /// Identificador do instrumento, conforme definido pela BM&FBOVESPA. Para a lista 
        /// de instrumentos, consulte a mensagem correspondente (Security List).
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("Identificador do instrumento, conforme definido pela BM&FBOVESPA. Para a lista de instrumentos, consulte a mensagem correspondente (Security List).")]
        public string SecurityID { get; set; }

        /// <summary>
        /// Classe ou fonte de identificação do instrumento. Campo condicionado a SecurityID 
        /// estar especificado. Valor aceito: 8 = símbolo da Bolsa (identificador BM&FBOVESPA 
        /// para instrumento)
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("Classe ou fonte de identificação do instrumento. Campo condicionado a SecurityID estar especificado. Valor aceito: 8 = símbolo da Bolsa (identificador BM&FBOVESPA para instrumento)")]
        public string SecurityIDSource { get; set; }

        /// <summary>
        /// Mercado ao qual o instrumento pertence (campo 48). Valor aceito: XBMF=BM&FBOVESPA. 
        /// Esse campo é opcional e sua ausência implica a atribuição automática do valor XBMF 
        /// (BM&FBOVESPA) para mercado (Market Center)
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("Mercado ao qual o instrumento pertence (campo 48). Valor aceito: XBMF=BM&FBOVESPA. Esse campo é opcional e sua ausência implica a atribuição automática do valor XBMF (BM&FBOVESPA) para mercado (Market Center)")]
        public string SecurityExchange { get; set; }

        #endregion

        #region Bloco NewOrderCross

        /// <summary>
        /// Identificador de ordem direta. Deve ser único durante determinado dia de pregão.
        /// </summary>
        [Category("NewOrderCross")]
        [Description("Identificador de ordem direta. Deve ser único durante determinado dia de pregão.")]
        public string CrossID { get; set; }

        /// <summary>
        /// Tipo de ordem direta inserida em um mercado. Valor aceito: 1 = negócio direto executado 
        /// total ou parcialmente. As duas pontas são tratadas da mesma maneira. Equivale a uma ordem 
        /// tudo ou nada (AON) (ordem a mercado ou limitada para execução total).
        /// </summary>
        [Category("NewOrderCross")]
        [Description("Tipo de ordem direta inserida em um mercado. Valor aceito: 1 = negócio direto executado total ou parcialmente. As duas pontas são tratadas da mesma maneira. Equivale a uma ordem tudo ou nada (AON) (ordem a mercado ou limitada para execução total).")]
        public int CrossType { get; set; }

        /// <summary>
        /// Indica se uma das pontas da ordem direta deve ser priorizada. Valor aceito: 0 = nenhuma
        /// </summary>
        [Category("NewOrderCross")]
        [Description("Indica se uma das pontas da ordem direta deve ser priorizada. Valor aceito: 0 = nenhuma")]
        public int CrossPriorization { get; set; }

        /// <summary>
        /// Tipo de ordem. Valor aceito para registro de operações: 2 = limitada
        /// </summary>
        [Category("NewOrderCross")]
        [Description("Tipo de ordem. Valor aceito para registro de operações: 2 = limitada")]
        public string OrdType { get; set; }

        /// <summary>
        /// Preço por ação ou contrato.
        /// </summary>
        [Category("NewOrderCross")]
        [Description("Preço por ação ou contrato.")]
        public double Price { get; set; }

        /// <summary>
        /// Horário de execução / geração da ordem, expresso em UTC.
        /// Faz referencia à data referencia da mensagem base.
        /// </summary>
        [Category("NewOrderCross")]
        [Description("Horário de execução / geração da ordem, expresso em UTC")]
        public DateTime TransactTime
        {
            get { return base.DataReferencia; }
            set { base.DataReferencia = value; }
        }

        /// <summary>
        /// Número de pontas (54) para grupos de repetição. Deve ser sempre igual a 2 (duas pontas).
        /// </summary>
        [Category("NewOrderCross")]
        [Description("Número de pontas (54) para grupos de repetição. Deve ser sempre igual a 2 (duas pontas).")]
        public List<OrdemDiretoPontaInfo> NoSides { get; set; }

        #endregion

        #region Construtores

        public ExecutarDiretoRequest()
        {
            this.NoSides = new List<OrdemDiretoPontaInfo>();
            this.Status = MensagemStatusEnum.SolicitacaoEfetuada;
        }

        #endregion
    }
}
