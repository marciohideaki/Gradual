using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Risco.Regras.Dados;

namespace Gradual.OMS.Contratos.Risco.Regras
{
    /// <summary>
    /// Classe de configurações para a regra de validação de conta corrente e empréstimos
    /// </summary>
    [Serializable]
    public class RegraContaCorrenteEmprestimoConfig
    {
        /// <summary>
        /// Informações do emprestimo concedido
        /// </summary>
        public EmprestimoInfo Emprestimo { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public RegraContaCorrenteEmprestimoConfig()
        {
            this.Emprestimo = new EmprestimoInfo();
        }
    }
}
