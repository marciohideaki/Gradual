using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.Comum;
using Orbite.RV.Contratos.MarketData.Mensagens;
using Orbite.RV.Contratos.MarketData.Bolsa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Mensagens
{
    /// <summary>
    /// Mensagem de request da série de cotações
    /// </summary>
    [SerieMarketData(
        CodigoSerie = "964AECFA-77C0-4868-BD28-D96FCC4402A7",
        NomeSerie = "Cotações",
        DescricaoSerie = "Lista de cotações de ativos",
        TipoMensagemResponse = typeof(ReceberSerieCotacaoResponse))]
    public class ReceberSerieCotacaoRequest : ReceberSerieItensRequest
    {
        /// <summary>
        /// Indica a data inicial dos elementos da série a retornar.
        /// Caso não seja informado, indica desde o início da série.
        /// </summary>
        public DateTime? DataInicial { get; set; }

        /// <summary>
        /// Indica a data final dos elementos da série a retornar.
        /// Caso não seja informado, indica desde o fim da série.
        /// </summary>
        public DateTime? DataFinal { get; set; }

        /// <summary>
        /// Indica o período de retorno desejado
        /// </summary>
        public PeriodoEnum Periodo { get; set; }

        /// <summary>
        /// Contem informações para se localizar a série desejada.
        /// </summary>
        public InstrumentoInfo Instrumento { get; set; }

        /// <summary>
        /// Indica que se não for encontrado nenhuma cotação para o período informado,
        /// retorna a cotação encontrada imediatamente anterior à data inicial
        /// </summary>
        public bool SeNaoEncontradoTrazerUltimo { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ReceberSerieCotacaoRequest()
        {
            this.Periodo = PeriodoEnum.Natural;
        }
    }
}
