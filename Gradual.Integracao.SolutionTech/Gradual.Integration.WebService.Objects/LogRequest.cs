using Gradual.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Integration.WebService.Messages
{
    [Serializable]
    public class LogRequest
    {
        public string logName       { get; set; }
        public string message       { get; set; }
        public string StackTrace    { get; set; }

        public LogRequest() { }
    }
}