using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.Lib.Mensagens;

namespace Gradual.Site.Lib.Dados.MinhaConta
{
    [Serializable]
    [DataContract]
    public class UltimasNegociacoesInfo
    {
        public DateTime? DtUltimasNegociacoes { get; set; }

        public string TipoBolsa { get; set; }

        public Int32 CdCliente { get; set; }

        public Int32 CdClienteBmf { get; set; }
    }
}
