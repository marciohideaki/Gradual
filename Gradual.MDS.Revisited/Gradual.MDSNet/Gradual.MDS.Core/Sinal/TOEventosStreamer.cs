using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Sinal;

namespace Gradual.MDS.Core.Sinal
{
    public class TOEventosStreamer
    {
        public bool SendToStreamer { get; set; }
        public bool ShouldSendNegStreamer { get; set; }
        public string Instrumento { get; set; }
        public NegocioBase RegistroNegocio { get; set; }
        public bool ShouldSendNegHB { get; set; }
        public bool ShouldSendLivroNegocioHB { get; set; }
        public List<Dictionary<string, string>> StreamerLNG { get; set; }
        public TOEventosStreamer()
        {
            this.SendToStreamer = false;
            this.ShouldSendNegStreamer = false;
            this.Instrumento = string.Empty;
            this.RegistroNegocio = null;
            this.ShouldSendNegHB = false;
            this.StreamerLNG = null;
            this.ShouldSendLivroNegocioHB = false;
        }

    }
}
