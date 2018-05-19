using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Classe base para mensagens de requisição
    /// </summary>
    [Serializable]
    [DataContract]
    public class MensagemRequestBase : MensagemBase
    {
        ///// <summary>
        ///// Código do Sistema Cliente, por exemplo: HB (HomeBroker), Plataforma, Robo, etc.
        ///// </summary>
        //[DataMember]
        //public string CodigoSistemaCliente { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public MensagemRequestBase() { }

        /// <summary>
        /// Usuário Logado
        /// </summary>
        public int IdUsuarioLogado { get; set; }

        /// <summary>
        /// Usuário Logado
        /// </summary>
        public string DescricaoUsuarioLogado { get; set; }
    }
}
