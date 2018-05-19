using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Ordens.StartStop.Lib.Enum
{
    public enum StopStartTipoEnum
    {
         StopLoss       = 1,
         StopGain       = 2,
         StopSimultaneo = 3,
         StopMovel      = 4,
        StartCompra     = 5,
    }

    public enum EnumStopStartTipo
    {
        StopLoss = 1,
        StopGain = 2,
        StopSimultaneo = 3,
        StopMovel = 4,
        StartCompra = 5,
    }

    public enum RespostaOrdem : int
    {
        Rejeitado = 0,
        Aceito    = 1,
    };

    public enum OrdemStopStatus : int
    {
        RegistradaAplicacao             = 1,
        EnviadaMDS                      = 2,
        AceitoMDS                       = 3,
        RejeitadoMDS                    = 4,
        ExecutadoMDS                    = 5,
        CancelamentoRegistradoAplicacao = 6,
        CancelamentoEnviadoMDS          = 7,
        CancelamentoAceitoMDS           = 8,
        CancelamentoRejeitadoMDS        = 9,
        Execucao                        = 10,
        Executado                       = 11,
    };
}
