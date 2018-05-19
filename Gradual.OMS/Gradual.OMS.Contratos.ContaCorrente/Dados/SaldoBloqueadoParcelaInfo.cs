using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.ContaCorrente.Dados
{
    /// <summary>
    /// Contém informações sobre uma parcela do saldo bloqueado
    /// </summary>
    [Serializable]
    public class SaldoBloqueadoParcelaInfo
    {
        /// <summary>
        /// Data em que o saldo foi inserido
        /// </summary>
        public DateTime DataInsercao { get; set; }

        /// <summary>
        /// Data da última atualização do saldo
        /// </summary>
        public DateTime DataUltimaAtualizacao { get; set; }

        /// <summary>
        /// Valor da parcela do bloqueio
        /// </summary>
        public double ValorParcelaBloqueio { get; set; }

        /// <summary>
        /// Evento gerador do bloqueio
        /// </summary>
        public SaldoBloqueadoParcelaTipoEnum TipoParcela { get; set; }

        /// <summary>
        /// Código de referencia ao objeto do evento gerador
        /// </summary>
        public string CodigoReferencia { get; set; }
    }
}
