using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Account
{
    public class AccountParserInfo
    {
        public string CdBolsa { get; set;}
        public string CdMembro { get; set; }
        public int CdCliente { get; set; }
        public string CdClieOutrBolsa { get; set; }
        public string DvClieOutrBolsa { get; set; }
        public string InUtilCorresp { get; set; }
        public string InUtilArbitragem { get; set; }
        public int CdEmpresa { get; set; }
        public int CdUsuario { get; set; }
        public string TpOcorrencia { get; set; }


        public AccountParserInfo()
        {
            this.CdBolsa = string.Empty;
            this.CdMembro = string.Empty;
            this.CdCliente = 0;
            this.CdClieOutrBolsa = string.Empty;
            this.DvClieOutrBolsa = string.Empty;
            this.InUtilCorresp = string.Empty;
            this.InUtilArbitragem = string.Empty;
            this.CdEmpresa = 0;
            this.CdUsuario = 0;
            this.TpOcorrencia = string.Empty;
        }

    }
}
