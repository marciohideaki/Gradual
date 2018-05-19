using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.WsIntegracao.Arena.Controllers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class ApiDocumentationAttribute : Attribute
    {
        public string Message { get; private set; }

        public ApiDocumentationAttribute(string pMessage)
        {
            this.Message = pMessage;
        }
    }
}
