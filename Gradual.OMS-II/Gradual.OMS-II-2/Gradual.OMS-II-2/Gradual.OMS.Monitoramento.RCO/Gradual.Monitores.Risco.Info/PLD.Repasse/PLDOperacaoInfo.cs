using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Monitores.Risco.Enum;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe de Operações para monitoramente de PLD
    /// </summary>
    [DataContract]
    [Serializable]
    public class PLDOperacaoInfo
    {
        /// <summary>
        /// Contador de Registro - Um sequencial somente usado como contador
        /// </summary>
        [DataMember]
        public int Seq { set; get; }

        /// <summary>
        /// Codigo do instrumento 
        /// </summary>
        [DataMember]
        public string Intrumento { set; get; }

        /// <summary>
        /// Numero do negócio
        /// </summary>
        [DataMember]
        public int NumeroNegocio { set; get; }

        /// <summary>
        /// Minutos restantes para expirar o PLD
        /// </summary>
        [DataMember]
        public TimeSpan MinutosRestantesPLD { set; get; }

        /// <summary>
        /// Minutos restantes para expirar o PLD
        /// </summary>
        [DataMember]
        public EnumCriticidadePLD Criticidade { set; get; }

        /// <summary>
        /// Codigo do cliente
        /// </summary>
        [DataMember]
        public int CodigoCliente { set; get; }  

        /// <summary>
        /// Sentido da operacao
        /// </summary>
        [DataMember]
        public string Sentido { set; get; }

        /// <summary>
        /// Preco do negócio
        /// </summary>
        [DataMember]
        public decimal PrecoNegocio { set; get; }

        /// <summary>
        /// Preco do mercado
        /// </summary>
        [DataMember]
        public decimal PrecoMercado { set; get; }

        /// <summary>
        /// Quantidade de negocio
        /// </summary>
        [DataMember]
        public int Quantidade { set; get; }

        /// <summary>
        /// Lucro Prejuiso
        /// </summary>
        [DataMember]
        public decimal LucroPrejuiso { set; get; }   

        /// <summary>
        /// CODIGO DA CONTRAPARTE
        /// </summary>
        [DataMember]
        public int Contraparte { set; get; }

        /// <summary>
        /// Tipo de registro
        /// </summary>
        [DataMember]
        public int TipoRegistro { set; get; }  
        
        /// <summary>
        /// Quantidade casada
        /// </summary>
        [DataMember]
        public int QT_CASADA { set; get; } 

        /// <summary>
        /// hora negocio
        /// </summary>
        [DataMember]
        public DateTime HR_NEGOCIO { set; get; }

        /// <summary>
        /// DAta da Ultima atualização
        /// </summary>
        [DataMember]
        public DateTime UltimaAtualizacao { set; get; }

        /// <summary>
        /// Indicador de Intencao de PLD
        /// </summary>
        [DataMember]
        public string IntencaoPLD { set; get; }
        
        /// <summary>
        /// Tipo Status solicitacao
        /// </summary>
        [DataMember]
        public string STATUS { set; get; }

        /// <summary>
        /// Descrição da criticidade
        /// </summary>
        [DataMember]
        public string DescricaoCriticidade { set; get; }
        
    }
}
