using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Dados
{
    /// <summary>
    /// Mantem informações de um instrumento.
    /// Um instrumento é um código negociável em algum local.
    /// Ele carrega informações da Origem, que é o nome do local onde ocorre a negociação.
    /// </summary>
    public class InstrumentoInfo
    {
        /// <summary>
        /// Código utilizado na negociação do instrumento
        /// </summary>
        public string CodigoNegociacao { get; set; }

        /// <summary>
        /// Data de inicio de negociacao do instrumento
        /// </summary>
        public DateTime? DataInicioNegociacao { get; set; }

        /// <summary>
        /// Data de fim de negociacao do instrumento
        /// </summary>
        public DateTime? DataFimNegociacao { get; set; }

        /// <summary>
        /// Indica mais algum detalhe do papel, que não é utilizado na negociação. Quando presente, 
        /// faz parte da chave primária.
        /// Por exemplo: numa listagem de papéis negociados em bolsa, o ativo PETR4 virá listado 
        /// apenas uma vez. Mas numa listagem de histórico cadastral de um ativo (para, por exemplo, 
        /// fazer uma valorização num período de tempo), o ativo PETR4 poderá vir listado mais de uma vez.
        /// </summary>
        public string Detalhe { get; set; }

        /// <summary>
        /// Indica a entidade que deu origem, ou onde é negociado, o instrumento
        /// </summary>
        public string Origem { get; set; }

        /// <summary>
        /// Indica o tipo do instrumento, se é uma ação, opção, futuro, etc.
        /// </summary>
        public InstrumentoTipoEnum Tipo { get; set; }

        /// <summary>
        /// Indica se o instrumento está habilitado para negociação ou se apenas faz parte de histórico
        /// </summary>
        public bool Habilitado { get; set; }
    }
}
