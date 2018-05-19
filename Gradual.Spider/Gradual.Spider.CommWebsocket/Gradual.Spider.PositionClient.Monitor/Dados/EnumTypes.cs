using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Gradual.Spider.PositionClient.Monitor.Dados
{
    [ProtoContract]
    [Serializable]
    [Flags]
    public enum SegmentoMercadoEnum
    {
        AVISTA,
        TERMO,
        OPCAO,
        FUTURO,
        FRACIONARIO,
        INTEGRALFRACIONARIO,
        INDEFINIDO
    }
}
