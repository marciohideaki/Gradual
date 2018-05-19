using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.Risco.Regras.Dados;

namespace Gradual.OMS.Contratos.Risco.Regras.Criticas
{
    /// <summary>
    /// Representa uma crítica de validação de emprestimo.
    /// Carrega a informação do emprestimo que realizou a validação
    /// </summary>
    public class CriticaRiscoEmprestimoInfo : CriticaRiscoInfo
    {
        /// <summary>
        /// Indica o empréstimo que foi utilizado na validação que originou a crítica
        /// </summary>
        public EmprestimoInfo EmprestimoInfo { get; set; }

        /// <summary>
        /// Informações de conta corrente utilizado para fazer a validação
        /// </summary>
        public ContaCorrenteInfo ContaCorrenteInfo { get; set; }
    }
}
