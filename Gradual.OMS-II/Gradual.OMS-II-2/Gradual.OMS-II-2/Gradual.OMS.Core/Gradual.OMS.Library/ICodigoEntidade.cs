using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Interface a ser implementada por entidades para informar o seu código
    /// </summary>
    public interface ICodigoEntidade
    {
        /// <summary>
        /// Informa o código da entidade
        /// </summary>
        /// <returns></returns>
        string ReceberCodigo();
    }
}
