using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Atributo para marcar uma classe de serviço e permitir uma futura localização ou montagem de lista.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited=false, AllowMultiple=true)]
    public sealed class ServicoAtributo : Attribute
    {
    }
}
