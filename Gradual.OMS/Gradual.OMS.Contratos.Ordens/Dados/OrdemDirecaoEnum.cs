using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Enumerador de direção da ordem: compra ou venda.
    /// </summary>
    [Serializable]
    public enum OrdemDirecaoEnum
    {
        NaoInformado,
        NaoImplementado,
        Compra,
        Venda
    }
}
