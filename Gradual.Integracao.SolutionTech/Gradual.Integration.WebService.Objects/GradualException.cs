using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Integration.WebService.Objects
{
    [Serializable]
    public class GradualException : Exception
    {
        private string message      = String.Empty;
        private string stackTrace   = String.Empty;

        public GradualException() { }

        public GradualException(String Message, String StackTrace)
        {
            message     = Message;
            stackTrace  = StackTrace;

        }

        public override string Message
        {
            get
            {
                return message;
            }
        }

        public override string StackTrace
        {
            get
            {
                return stackTrace;
            }
        }
    }
}
