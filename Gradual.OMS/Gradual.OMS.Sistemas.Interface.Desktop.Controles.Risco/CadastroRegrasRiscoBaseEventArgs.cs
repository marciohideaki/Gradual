using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    /// <summary>
    /// EventArgs para os eventos do controle de regras de risco
    /// </summary>
    public class CadastroRegrasRiscoBaseEventArgs : EventArgs
    {
        /// <summary>
        ///  Referencia da regra de risco
        /// </summary>
        public RegraRiscoInfo RegraRisco { get; set; }
    }
}
