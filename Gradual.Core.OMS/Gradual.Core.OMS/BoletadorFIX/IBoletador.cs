using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace BoletadorFIX
{
    public interface IBoletador
    {
        bool EnviarOrdem(OrdemInfo info);
        bool AlterarOrdem(OrdemInfo info);
        bool CancelarOrdem(OrdemCancelamentoInfo info);
        bool EnviarOrdemCross(OrdemCrossInfo info);
    }
}
