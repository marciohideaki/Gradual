using System;

namespace Gradual.OMS.ConsolidadorRelatorioCCLib.Enum
{
    /// <summary>
    /// Enumerador de direção da ordem: compra ou venda.
    /// </summary>
    [Serializable]
    public enum CriticaMensagemEnum
    {
        Exception,
        OK ,
        ErroNegocio
    }
}
