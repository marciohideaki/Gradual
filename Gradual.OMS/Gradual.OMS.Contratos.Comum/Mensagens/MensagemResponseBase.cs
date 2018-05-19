using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Classe base para mensagens de resposta
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class MensagemResponseBase : MensagemBase
    {
        /// <summary>
        /// Código da mensagem que fez a requisição
        /// </summary>
        [Category("MensagemResponseBase")]
        [DataMember]
        public string CodigoMensagemRequest { get; set; }

        /// <summary>
        /// Indica o status da resposta
        /// </summary>
        [DataMember]
        public MensagemResponseStatusEnum StatusResposta { get; set; }

        /// <summary>
        /// Contem uma descrição da resposta. Caso o status indique algum erro,
        /// esta propriedade irá conter a descrição do erro.
        /// </summary>
        [DataMember]
        public string DescricaoResposta { get; set; }

        /// <summary>
        /// Lista de críticas da mensagem.
        /// As críticas geralmente são colocadas pelas regras do sistema de validação.
        /// </summary>
        [DataMember]
        public List<CriticaInfo> Criticas { get; set; }

        /// <summary>
        /// Data da Resposta
        /// </summary>
        [DataMember]
        public DateTime DataResposta { get; set; }

        /// <summary>
        /// Propriedade de uso geral
        /// </summary>
        public string ResponseTag { get; set; }

        /// <summary>
        /// Construtor default.
        /// </summary>
        public MensagemResponseBase() : base()
        {
            this.StatusResposta = MensagemResponseStatusEnum.OK;
            this.Criticas = new List<CriticaInfo>();
            this.DataResposta = DateTime.Now;
        }
    }
}
