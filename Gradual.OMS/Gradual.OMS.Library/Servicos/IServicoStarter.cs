using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Interface para ser implementada por classes que fazem ativação de conjunto de serviços.
    /// </summary>
    public interface IServicoStarter
    {
        void Carregar();
    }
}
