using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Sistemas.CanaisNegociacao
{
    [Serializable]
    public class CanalNegociacaoBVMFConfig
    {
        public string ArquivoConfig { get; set; }
        public string BeginString { get; set; }
        public string SenderCompID { get; set; }
        public string TargetCompID { get; set; }
        public string PartyID { get; set; }
        public string PartyIDSource { get; set; }
        public int PartyRole { get; set; }
        public string SecurityIDSource { get; set; }
        public string LogonPassword { get; set; }
        public int HeartBtInt { get; set; }
        public List<PartyInfo> Parties { get; set; }
        public bool RessetarSeqNum { get; set; }

        public CanalNegociacaoBVMFConfig()
        {
            this.Parties = new List<PartyInfo>();
            this.HeartBtInt = 30;
        }
    }
}
