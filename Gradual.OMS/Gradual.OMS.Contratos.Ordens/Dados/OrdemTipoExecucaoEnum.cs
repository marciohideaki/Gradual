using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    public enum OrdemTipoExecucaoEnum
    {
        NaoInformado,
        NaoImplementado,
        CancelamentoNegocio,
        CancelamentoOferta,
        CancelamentoPendente,
        Negocio,
        NovaPendente,
        Nova,
        Preenchimento,
        Reconfirmacao,
        Rejeicao,
        Substituicao,
        TerminoValidade,
        Suspenso
    }
}
