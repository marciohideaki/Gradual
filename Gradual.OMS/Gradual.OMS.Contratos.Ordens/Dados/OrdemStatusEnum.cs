using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Enumerador de status de ordens
    /// </summary>
    public enum OrdemStatusEnum
    {
        NaoInformado,
        NaoImplementado,
        NovaPendente,
        Cancelada,
        CancelamentoPendente,
        Executada,
        Nova,
        ParcialmenteExecutada,
        Rejeitada,
        Substituida,
        Expirada,
        Suspenso
    }
}
