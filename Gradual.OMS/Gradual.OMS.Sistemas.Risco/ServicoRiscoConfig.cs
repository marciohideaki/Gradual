using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Risco
{
    /// <summary>
    /// Classe de configuração para o serviço de risco
    /// </summary>
    [Serializable]
    public class ServicoRiscoConfig
    {
        /// <summary>
        /// Indica o namespace aonde estão contidas as regras de risco.
        /// O formado de cada item na coleção é namespace, assembly.
        /// </summary>
        public List<string> NamespacesRegras { get; set; }

        /// <summary>
        /// Indica se o timer de custodia deverá ser ligado
        /// </summary>
        public bool AtualizarCustodiaPeriodicamente { get; set; }

        /// <summary>
        /// Indica de quanto em quanto tempo a custódia será atualizada (em segundos)
        /// </summary>
        public int TempoAtualizacaoCustodia { get; set; }

        /// <summary>
        /// Indica se o timer de conta corrente deverá ser ligado
        /// </summary>
        public bool AtualizarContaCorrentePeriodicamente { get; set; }

        /// <summary>
        /// Indica de quanto em quanto tempo a conta corrente será atualizada (em segundos)
        /// </summary>
        public int TempoAtualizacaoContaCorrente { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoRiscoConfig()
        {
            this.NamespacesRegras = new List<string>();
            this.TempoAtualizacaoContaCorrente = 30;
            this.TempoAtualizacaoCustodia = 30;
        }
    }
}
