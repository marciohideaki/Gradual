using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe que armazena os dados de ordem bmf do cliente
    /// </summary>
    [Serializable]
    public class PosicaoBmfInfo
    {
        /// <summary>
        /// Codigo do cliente BMF
        /// </summary>
        [DataMember]
        public int Cliente { set; get; }

        /// <summary>
        /// Sentido da ordem: Se é compra ou venda - C e V
        /// </summary>
        [DataMember]
        public string Sentido { set; get; }

        /// <summary>
        /// Contrato bmf
        /// </summary>
        [DataMember]
        public string Contrato { set; get; }

        /// <summary>
        /// Quantidade por Contrato
        /// </summary>
        [DataMember]
        public int QuantidadeContato { set; get; }

        /// <summary>
        /// Fator Multiplicador bmf 
        /// </summary>
        [DataMember]
        public decimal FatorMultiplicador { set; get; }

        /// <summary>
        /// Preço de aquisição do contrato bmf
        /// </summary>
        [DataMember]
        public decimal PrecoAquisicaoContrato { set; get; }

        /// <summary>
        /// Preço do contrato no mercado
        /// </summary>
        [DataMember]
        public decimal PrecoContatoMercado    { set; get; }

        /// <summary>
        /// Diferencial de pontos é a diferença entre o 
        /// preço de aquisição do contrato - o preço de mercado atual
        /// </summary>
        [DataMember]
        public decimal DiferencialPontos      { set; get; }

        /// <summary>
        /// Lucro e Prejuízo do contrato é o 
        /// (Diferencial de POntos vezes o Fator Multiplicador ) vezes Quantidade de contratos
        /// (OBS) O Calculo de DI é diferente
        /// </summary>
        [DataMember]
        public decimal LucroPrejuizoContrato  { set; get; }

        /// <summary>
        /// Data da operação de BMF
        /// </summary>
        [DataMember]
        public DateTime DataOperacao { set; get; }

        /// <summary>
        /// Contra parte é o comprador/vendedor da negociação de acordo com que 
        /// é essencialmente quem compra a ação quando alguém oferta a mesma ação a venda.
        /// </summary>
        [DataMember]
        public int Contraparte { set; get; }
        
        /// <summary>
        /// Codigo da série bmf
        /// </summary>
        [DataMember]
        public string CodigoSerie { get; set; }
    }
}
