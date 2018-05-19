using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Classe de dados para informações da ordem.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OrdemInfo : ICloneable, ICodigoEntidade
    {
        /// <summary>
        /// Codigo local da ordem, atribuido pelo OMS.
        /// No caso de envio de ordem, o código da ordem será o mesmo código da mensagem
        /// </summary>
        [Description(
            "Codigo local da ordem, atribuido pelo OMS." +
            " No caso de envio de ordem, o código da ordem será o mesmo código da mensagem")]
        public string CodigoOrdem { get; set; }

        /// <summary>
        /// Codigo de ordem da bolsa, atribuido pela bolsa
        /// </summary>
        [Description("Codigo de ordem da bolsa, atribuido pela bolsa")]
        public string CodigoOrdemBolsa { get; set; }

        /// <summary>
        /// Codigo da bolsa a que a ordem se refere
        /// </summary>
        [Description("Codigo da bolsa a que a ordem se refere")]
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// Código da sessão que gerou a ordem.
        /// Será a sessão que irá receber automaticamente os eventos de execuções da ordem.
        /// </summary>
        [Description("Código da sessão que gerou a ordem")]
        public string CodigoSessao { get; set; }

        /// <summary>
        /// Código do cliente a que a ordem se refere
        /// </summary>
        [Description("Código do cliente a que a ordem se refere")]
        public string CodigoCliente { get; set; }

        /// <summary>
        /// Permite associar um código externo à ordem. Posteriormente a ordem poderá ser recuperada por este
        /// código externo.
        /// </summary>
        [Category("Outros")]
        [Description("Permite associar um código externo à ordem. Posteriormente a ordem poderá ser recuperada por este código externo.")]
        public string CodigoExterno { get; set; }

        /// <summary>
        /// Código do sistema cliente que solicitou a ordem
        /// </summary>
        [Category("Outros")]
        [Description("Código do sistema cliente que solicitou a ordem")]
        public string CodigoSistemaCliente { get; set; }

        /// <summary>
        /// Código do canal em que a ordem foi executada
        /// </summary>
        [Category("Outros")]
        [Description("Código do canal em que a ordem foi executada.")]
        public string CodigoCanal { get; set; }

        /// <summary>
        /// Permite que uma ordem seja executada em carater de exceção, através da emissão de um ticket de risco.
        /// Este ticket é emitido pela área de risco com algumas características possíveis da operação.
        /// </summary>
        [Category("Outros")]
        [Description("Permite que uma ordem seja executada em carater de exceção, através da emissão de um ticket de risco. Este ticket é emitido pela área de risco com algumas características possíveis da operação.")]
        public string CodigoTicketRisco { get; set; }

        /// <summary>
        /// Data da ordem
        /// </summary>
        [Description("Data da ordem")]
        public DateTime DataReferencia { get; set; }

        /// <summary>
        /// Indica a data que a ordem teve sua ultima alteração
        /// </summary>
        [Description("Indica a data que a ordem teve sua ultima alteração")]
        public DateTime DataUltimaAlteracao { get; set; }

        /// <summary>
        /// Indica ordem de compra ou venda
        /// </summary>
        [Description("Indica ordem de compra ou venda")]
        public OrdemDirecaoEnum Direcao { get; set; }

        /// <summary>
        /// Instrumento da ordem
        /// </summary>
        [Description("Instrumento da ordem")]
        public string Instrumento { get; set; }

        /// <summary>
        /// Preço da ordem
        /// </summary>
        [Description("Preço da ordem")]
        public double Preco { get; set; }

        /// <summary>
        /// Quantidade da ordem
        /// </summary>
        [Description("Quantidade da ordem")]
        public double Quantidade { get; set; }

        /// <summary>
        /// Indica a quantidade da ordem que já foi executada. Preenchida pelo OMS
        /// </summary>
        [Description("Indica a quantidade da ordem que já foi executada. Preenchida pelo OMS")]
        public double QuantidadeExecutada { get; set; }

        /// <summary>
        /// Indica a validade da ordem
        /// </summary>
        public OrdemValidadeEnum Validade { get; set; }

        /// <summary>
        /// Indica a data de validade da ordem caso o campo Validade seja ValidoAteDeterminadaData
        /// </summary>
        public DateTime? DataValidade { get; set; }

        /// <summary>
        /// Status da ordem
        /// </summary>
        [Description("Status da ordem")]
        public OrdemStatusEnum Status { get; set; }

        /// <summary>
        /// Historico de mensagens processadas
        /// </summary>
        [Description("Historico de mensagens processadas")]
        public List<SinalizarExecucaoOrdemRequest> Historico { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public OrdemInfo()
        {
            this.DataReferencia = DateTime.Now;
            this.DataUltimaAlteracao = DateTime.Now;
            this.Historico = new List<SinalizarExecucaoOrdemRequest>();
        }

        #region ICloneable Members

        /// <summary>
        /// Duplica o objeto
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            // Cria novo item
            OrdemInfo ordemInfo = (OrdemInfo)this.MemberwiseClone();

            // Retorna
            return ordemInfo;
        }

        #endregion

        public void Merge(OrdemInfo original)
        {
            this.CodigoBolsa = original.CodigoBolsa;
            this.CodigoOrdem = original.CodigoOrdem;
            this.CodigoOrdemBolsa = original.CodigoOrdemBolsa;
            this.CodigoSessao = original.CodigoSessao;
            this.CodigoCliente = original.CodigoCliente;
            this.DataReferencia = original.DataReferencia;
            this.DataUltimaAlteracao = original.DataUltimaAlteracao;
            this.Direcao = original.Direcao;
            this.Instrumento = original.Instrumento;
            this.Preco = original.Preco;
            this.Quantidade = original.Quantidade;
            this.QuantidadeExecutada = original.QuantidadeExecutada;
            this.Status = original.Status;
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoOrdem;
        }

        #endregion

        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
