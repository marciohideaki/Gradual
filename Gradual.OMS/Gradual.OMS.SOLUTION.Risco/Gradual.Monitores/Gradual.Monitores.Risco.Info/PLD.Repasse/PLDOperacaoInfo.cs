using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Monitores.Risco.Enum;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [DataContract]
    [Serializable]
    public class PLDOperacaoInfo
    {
        [DataMember]
        public int Seq { set; get; }
        // Codigo do instrumento
         [DataMember]
        public string Intrumento { set; get; }
        // Numero do negócio
         [DataMember]
        public int NumeroNegocio { set; get; }
        // Minutos restantes para expirar o PLD
         [DataMember]
        public TimeSpan MinutosRestantesPLD { set; get; }
        // Criticidade do PLS
         [DataMember]
        public EnumCriticidadePLD Criticidade { set; get; }
        // Codigo do cliente
         [DataMember]
        public int CodigoCliente { set; get; }  
        // Sentido da operacao
         [DataMember]
        public string Sentido { set; get; }
        // Preco do negócio
         [DataMember]
        public decimal PrecoNegocio { set; get; }
        // Preco do mercado
         [DataMember]
        public decimal PrecoMercado { set; get; }
        // Quantidade de negocio
         [DataMember]
        public int Quantidade { set; get; }
        // Lucro Prejuiso
         [DataMember]
        public decimal LucroPrejuiso { set; get; }   
        // CODIGO DA CONTRAPARTE
         [DataMember]
        public int Contraparte { set; get; }
        // Tipo de registro
         [DataMember]
        public int TipoRegistro { set; get; }  
        // Quantidade casada
         [DataMember]
        public int QT_CASADA { set; get; } 
        // hora negocio
         [DataMember]
        public DateTime HR_NEGOCIO { set; get; }
        // hora negocio
         [DataMember]
        public DateTime UltimaAtualizacao { set; get; }
        // Indicador de Intencao de PLD
         [DataMember]
        public string IntencaoPLD { set; get; }
        // Tipo solicitacao
         [DataMember]
        public string STATUS { set; get; }
         [DataMember]
         public string DescricaoCriticidade { set; get; }
        // Criticidade do PLD      
    }
}
