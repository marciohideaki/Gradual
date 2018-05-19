using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Ordens.Template;

namespace Gradual.Ordens.Interface  
{
    public interface IServicoOrdemStopStart
    {
         int EnviarOrdemStop(TOrdem pOrdemStop);
         bool CancelarOrdemStop(int id_startstop);
              
    }
}
