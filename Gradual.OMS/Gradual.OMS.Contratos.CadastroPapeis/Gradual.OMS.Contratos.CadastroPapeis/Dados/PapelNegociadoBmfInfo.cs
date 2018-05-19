using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.CadastroPapeis.Dados
{
    /// <summary>
    /// Entidade de papeis negociados BMF
    /// </summary>
    [Serializable]
    public class PapelNegociadoBmfInfo
    {
        #region Properties
        /// <summary>
        /// Tipo de Registro
        /// </summary>
        public int TipoRegistro { get; set; }

        /// <summary>
        /// Tipo de negociação
        /// </summary>
        public string TipoNegociacao { get; set; }

        /// <summary>
        /// Código da Mercadoria
        /// </summary>
        public string CodMercadoria { get; set; }

        /// <summary>
        /// Código do mercado
        /// </summary>
        public char CodMercado { get; set; }

        /// <summary>
        /// Tipo da série
        /// </summary>
        public char TipoSerie { get; set; }

        /// <summary>
        /// Série (opç)/vencimento (fut)
        /// </summary>
        public string SerieVencimento { get; set; }

        /// <summary>
        /// Data de vencimento (fut/opç)
        /// </summary>
        public  Nullable<DateTime> DataVencimento { get; set; }

        /// <summary>
        /// Preço de exercício (opç)
        /// </summary>
        public double PrecoExercicio { get; set; }

        /// <summary>
        /// Qtd negocios efetuados no dia
        /// </summary>
        public int QuantidadeNegociadaDia { get; set; }

        /// <summary>
        /// Qtd contratos negociados no dia
        /// </summary>
        public int QuantidadeContratoNegociadaDia { get; set; }

        /// <summary>
        /// Sinal da cotação media dos negocios do dia
        /// </summary>
        public char SinalCotacaoMediaDia { get; set; }

        /// <summary>
        /// Cotacao media dos negocios do dia
        /// </summary>
        public int CotacaoMediaNegociadaDia { get; set; }

        /// <summary>
        /// Sinal de cotação do ultimo negocio do dia
        /// </summary>
        public char SinalCotacaoUltimaNegociacaoDia { get; set; }

        /// <summary>
        /// Cotacao do ultimo negocio do dia
        /// </summary>
        public int CotacaoUltimoNegocioDia { get; set; }

        /// <summary>
        /// Hora do ultimo negocio do dia
        /// </summary>
        public DateTime HoraUltimoNegocioDia { get; set; }

        /// <summary>
        /// Data do ultimo negocio do dia
        /// </summary>
        public Nullable<DateTime> DataUltimaNegociacao { get; set; }

        /// <summary>
        /// Sinal de cotação do fechamento do dia
        /// </summary>
        public char SinalCotacaoFechamentoDia { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double CotacaoFechamentoDia { get; set;}

        /// <summary>
        /// Sinal da cotação ajuste (fut)
        /// </summary>
        public char SinalCotacaoAjuste { get; set; }

        /// <summary>
        /// Cotação ajuste (fut)
        /// </summary>
        public double CotacaoAjusteFuturo { get; set; }

        /// <summary>
        /// Percentual de oscilação 
        /// </summary>
        public double PercentualOscilacao { get; set; }

        /// <summary>
        /// Sinal de Oscilação
        /// </summary>
        public char SinalOscilacao { get; set; }

        /// <summary>
        /// Qtd. dias corridos até data de vencimento
        /// </summary>
        public int QuantidadeDiasAteDataVencimento { get; set; }

        /// <summary>
        /// Qtd. dias úteis até data de vencimento
        /// </summary>
        public int QuantidadeDiasUteisAteDataVencimento { get; set; }

        /// <summary>
        /// Vencimento do contrato-objeto
        /// </summary>
        public string VencimentoContratoObjeto { get; set; }

        /// <summary>
        /// Valor de Diferença
        /// </summary>
        public double ValorDiferenca { get; set; }

        /// <summary>
        /// Sinal de diferença
        /// </summary>
        public char SinalDiferenca { get; set; }
        #endregion


    }
}
