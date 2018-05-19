using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Possíveis status de serviços. Utilizado por quem implementa IServicoControlavel.
    /// </summary>
    public enum ServicoStatus
    {
        Indefinido,
        EmExecucao,
        Parado,
        Erro
    }
}
