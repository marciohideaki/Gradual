using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.SaldoDevedor.Utils.Info
{
    [Serializable]
    public class ConfigMailInfo
    {
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
    }
}
