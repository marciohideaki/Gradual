using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Risco.Dados
{
    /// <summary>
    /// O ticket do risco agrupa o conjunto de alterações realizadas para 
    /// atender a uma alocação de risco.
    /// </summary>
    [Serializable]
    public class TicketRiscoInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código do ticket do risco. Gerado internamente.
        /// </summary>
        public string CodigoTicketRisco { get; set; }

        /// <summary>
        /// Código do usuário que emitiu o ticket
        /// </summary>
        public string CodigoUsuarioEmissor { get; set; }

        /// <summary>
        /// Código do usuário que tem a permissão de executar 
        /// a operação com este ticket.
        /// Caso seja nulo, vale para qualquer usuário
        /// </summary>
        public string CodigoUsuarioExecutor { get; set; }

        /// <summary>
        /// Permite direcionar a execução do ticket especificando, opcionalmente,
        /// usuário, bolsa, ativo, etc.
        /// </summary>
        public RiscoGrupoInfo Agrupamento { get; set; }
        
        /// <summary>
        /// Limite financeiro da operação.
        /// Caso seja nulo, não há limite financeiro para a operação
        /// </summary>
        public double? LimiteFinanceiro { get; set; }

        /// <summary>
        /// Limite de quantidade da operação.
        /// Caso seja nulo, não há limite de quantidade para a operação
        /// </summary>
        public double? LimiteQuantidade { get; set; }

        /// <summary>
        /// Data de vencimento do ticket.
        /// Caso seja nulo, não há vencimento do ticket.
        /// </summary>
        public DateTime? DataVencimento { get; set; }

        /// <summary>
        /// Indica o status do ticket
        /// </summary>
        public TicketRiscoStatusEnum Status { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public TicketRiscoInfo()
        {
            // Inicializa
            this.CodigoTicketRisco = Guid.NewGuid().ToString();
            this.Status = TicketRiscoStatusEnum.EmAberto;
            this.Agrupamento = new RiscoGrupoInfo();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoTicketRisco;
        }

        #endregion
    }
}
