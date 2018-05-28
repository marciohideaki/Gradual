using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Email
{
    public class EmailNotaCorretagemInfo
    {
        public int IdEmail { get; set; }
        public int IdCliente { get; set; }
        public DateTime DtRegistroEmail { get; set; }
        public string EmailOrigem { get; set; }
        public string EmailDestinatario { get; set; }
        public string EmailDestinatarioCc { get; set; }
        public string EmailDestinatarioCco { get; set; }
        public string Assunto { get; set; }
        public string Body { get; set; }
        public string ArquivoNota { get; set; }
        public string Bolsa { get; set; }
        public string Status { get; set; }
        public string DescStatus { get; set; }

        public EmailNotaCorretagemInfo()
        {
            this.IdEmail = 0;
            this.IdCliente = 0;
            this.DtRegistroEmail = DateTime.MinValue;
            this.EmailOrigem = string.Empty;
            this.EmailDestinatario = string.Empty;
            this.EmailDestinatarioCc = string.Empty;
            this.EmailDestinatarioCco = string.Empty;
            this.Assunto = string.Empty;
            this.ArquivoNota = string.Empty;
            this.Bolsa = string.Empty;
            this.Body = string.Empty;
            this.Status = string.Empty;
            this.DescStatus = string.Empty;
        }



    }
}
