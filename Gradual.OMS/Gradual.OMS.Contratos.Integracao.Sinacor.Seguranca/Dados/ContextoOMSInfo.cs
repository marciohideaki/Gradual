using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados
{
    /// <summary>
    /// Classe para conter informações do usuário relativas ao sinacor.
    /// Uma instância desta classe preenchida é associada ao usuário no
    /// momento do login, pela classe de complemento de usuários do sinacor.
    /// </summary>
    [Serializable]
    public class ContextoOMSInfo
    {
        /// <summary>
        /// Código CBLC do usuário
        /// </summary>
        [Description("Código CBLC do usuário. Deve ser preenchido no cadastro para fazer a associação entre o usuário do sistema e o cliente cblc.")]
        public string CodigoCBLC { get; set; }

        /// <summary>
        /// Código CBLC de conta de investimento do usuário, caso ele possua
        /// </summary>
        [Description("Código CBLC de conta de investimento do usuário, caso ele possua")]
        public string CodigoCBLCInvestimento { get; set; }

        /// <summary>
        /// Código CBLC do Assessor
        /// </summary>
        [Description("Código do assessor do cliente CBLC. Este campo é preenchido automaticamente pelo sistema a cada login, através de integração com o sistema Sinacor.")]
        public string CodigoCBLCAssessor { get; set; }

        /// <summary>
        /// Código da custódia do usuário.
        /// Indica qual o objeto de custódia mantem as posições de custódia para o usuário.
        /// </summary>
        [Description("Código da custódia do usuário. Indica qual o objeto de custódia mantem as posições de custódia para o usuário.")]
        public string CodigoCustodia { get; set; }

        /// <summary>
        /// Código da conta corrente do usuário.
        /// Indica qual o objeto de conta corrente que mantém as posições de conta corrente para o usuário.
        /// </summary>
        [Description("Código da conta corrente do usuário. Indica qual o objeto de conta corrente que mantém as posições de conta corrente para o usuário.")]
        public string CodigoContaCorrente { get; set; }

        /// <summary>
        /// Indica a data em que a consulta do sinacor foi feita
        /// </summary>
        [Description("Indica a data da última consulta do sistema ao sinacor. Este campo é preenchido automaticamente pelo sistema.")]
        public DateTime DataConsultaSinacor { get; set; }

        /// <summary>
        /// Indica se o sinacor já foi verificado neste contexto (usuário, sessão, etc)
        /// </summary>
        [Description("Indica se o sinacor já foi verificado para este contexto. Preenchido automaticamente pelo sistema.")]
        public bool SinacorVerificado { get; set; }

        /// <summary>
        /// Código do perfil de risco a ser associado ao cliente.
        /// </summary>
        [Description("Código do perfil de risco a ser associado ao cliente.")]
        public string CodigoPerfilRisco { get; set; }

        /// <summary>
        /// Representação em string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
