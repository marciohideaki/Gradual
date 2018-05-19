using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    /// <summary>
    /// Dados de Instrumento armazenados pelo MDS (Bovespa e BMF)
    /// </summary>
    [Serializable]
    public class InstrumentoInfo
    {
        /// <summary>
        /// Código de Negociação do Instrumento
        /// </summary>
        [DataMember]
        public string Instrumento { get; set; }

        /// <summary>
        /// Nome da empresa emitente do papel / Especificação do contrato
        /// </summary>
        [DataMember]
        public string RazaoSocial { get; set; }

        /// <summary>
        /// Grupo de cotação do Instrumento
        /// </summary>
        [DataMember]
        public string GrupoCotacao { get; set; }

        /// <summary>
        /// Forma de cotação do Instrumento
        /// </summary>
        [DataMember]
        public Nullable<int> FormaCotacao { get; set; }

        /// <summary>
        /// Data e hora do último negócio
        /// </summary>
        [DataMember]
        public Nullable<DateTime> DataUltimoNegocio { get; set; }

        /// <summary>
        /// Lote padrão do Instrumento para negociação
        /// </summary>
        [DataMember]
        public Nullable<int> LotePadrao { get; set; }

        /// <summary>
        /// Indicador de Opção (apenas BM&F)
        /// </summary>
        [DataMember]
        public string IndicadorOpcao { get; set; }

        /// <summary>
        /// Preço de Exercício
        /// </summary>
        [DataMember]
        public Nullable<double> PrecoExercicio { get; set; }

        /// <summary>
        /// Data de Vencimento (para Opções da Bovespa ou contratos da BM&F)
        /// </summary>
        [DataMember]
        public Nullable<DateTime> DataVencimento { get; set; }

        /// <summary>
        /// Código do Papel base do Instrumento
        /// </summary>
        [DataMember]
        public string CodigoPapelObjeto { get; set; }

        /// <summary>
        /// Segmento de mercado
        /// </summary>
        [DataMember]
        public string SegmentoMercado { get; set; }

        /// <summary>
        /// Coeficiente de Multiplicação
        /// </summary>
        [DataMember]
        public Nullable<double> CoeficienteMultiplicacao { get; set; }

        /// <summary>
        /// Data da última atualização do Instrumento
        /// </summary>
        [DataMember]
        public Nullable<DateTime> DataRegistro { get; set; }

        /// <summary>
        /// Código do Instrumento no sistema ISIN
        /// </summary>
        [DataMember]
        public string CodigoISIN { get; set; }

        /// <summary>
        /// Nomes dos indices que este instrumento está incluso
        /// </summary>
        [DataMember]
        public string ComposicaoIndice { get; set; }

        /// <summary>
        /// Numero de negocios ocorridos no dia para o Instrumento
        /// </summary>
        [DataMember]
        public Nullable<int> NumeroNegocios { get; set; }

        /// <summary>
        /// Volume financeiro ocorrido no dia para o Instrumento
        /// </summary>
        [DataMember]
        public Nullable<double> VolumeNegocios { get; set; }

        /// <summary>
        /// Quantidade de papeis negociados no dia para o Instrumento
        /// </summary>
        [DataMember]
        public Nullable<double> QuantidadePapeis { get; set; }

        /// <summary>
        /// Preço do último negócio
        /// </summary>
        [DataMember]
        public Nullable<double> Preco { get; set; }

        /// <summary>
        /// Variação do último negócio
        /// </summary>
        [DataMember]
        public Nullable<double> Variacao { get; set; }

        /// <summary>
        /// Dicionario das Corretoras Compradoras que mais negociaram (Negocios em Destaque)
        /// </summary>
        [DataMember]
        public SortedDictionary<RankingInfo, string> DictCompradorasMaisNegociadas { get; set; }
        public SortedDictionary<string, RankingInfo> DictCompradorasMaisNegociadasPorCorretora { get; set; }

        /// <summary>
        /// Dicionario das Corretoras Vendedoras que mais negociaram (Negocios em Destaque)
        /// </summary>
        [DataMember]
        public SortedDictionary<RankingInfo, string> DictVendedorasMaisNegociadas { get; set; }
        public SortedDictionary<string, RankingInfo> DictVendedorasMaisNegociadasPorCorretora { get; set; }

        /// <summary>
        /// Dicionario das Corretoras Compradoras com maiores volumes (Negocios em Destaque)
        /// </summary>
        [DataMember]
        public SortedDictionary<RankingInfo, string> DictCompradorasMaioresVolumes { get; set; }
        public SortedDictionary<string, RankingInfo> DictCompradorasMaioresVolumesPorCorretora { get; set; }

        /// <summary>
        /// Dicionario das Corretoras Vendedoras com maiores volumes (Negocios em Destaque)
        /// </summary>
        [DataMember]
        public SortedDictionary<RankingInfo, string> DictVendedorasMaioresVolumes { get; set; }
        public SortedDictionary<string, RankingInfo> DictVendedorasMaioresVolumesPorCorretora { get; set; }

        /// <summary>
        /// Dicionario dos maiores volumes de Corretoras (Resumo de Corretoras)
        /// </summary>
        [DataMember]
        public SortedDictionary<CorretorasInfo, string> DictMaioresVolumes { get; set; }
        public SortedDictionary<string, CorretorasInfo> DictMaioresVolumesPorCorretora { get; set; }
    }
}
