using System;
using System.Collections.Generic;
using System.ComponentModel;

using Gradual.OMS.Library;

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Classe utilizada para armazenar o contexto de validação da mensagem.
    /// Este objeto será repassado de regra em regra, sendo que cada regra
    /// poderá consultar e alterar o conteúdo do contexto.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ContextoValidacaoInfo
    {
        /// <summary>
        /// Mensagem a ser validada
        /// </summary>
        public MensagemBase Mensagem { get; set; }

        /// <summary>
        /// Tipo da mensagem a ser validada
        /// </summary>
        public Type TipoMensagem { get; set; }

        /// <summary>
        /// Complementos utilizados na validação da mensagem
        /// </summary>
        public ColecaoTipoInstancia Complementos { get; set; }

        /// <summary>
        /// Lista de regras a serem validadas
        /// </summary>
        public List<RegraBase> RegrasAValidar { get; set; }

        /// <summary>
        /// Lista de regras já validadas
        /// </summary>
        public List<RegraBase> RegrasValidadas { get; set; }

        /// <summary>
        /// Resultados das validações das regras
        /// </summary>
        public List<CriticaInfo> Criticas { get; set; }

        /// <summary>
        /// Indica se a mensagem passou na validação
        /// </summary>
        public bool MensagemValida { get; set; }

        /// <summary>
        /// Codigo da sessao que iniciou a validação
        /// </summary>
        public string CodigoSessao { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ContextoValidacaoInfo()
        {
            this.Complementos = new ColecaoTipoInstancia();
            this.RegrasAValidar = new List<RegraBase>();
            this.RegrasValidadas = new List<RegraBase>();
            this.Criticas = new List<CriticaInfo>();
        }
    }
}
