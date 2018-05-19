using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AS.Messages
{
    public class MDSSignIn
    {
        public string pStrStatusRequest { set; get; }
        public string pStrIdCliente { set; get; }
        public string pStrIdSistema { set; get; }
        public string pStrUniqueId { set; get; }
        public string pStrErrorCode { set; get; }        
  
        public byte[] Message
        {  
            get{
                System.Text.Encoding enc = System.Text.Encoding.ASCII;
                return enc.GetBytes(pStrStatusRequest + pStrIdCliente + pStrIdSistema + pStrUniqueId + pStrErrorCode);
            }
        }
    }
}

