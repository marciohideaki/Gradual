using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de response de Extrato de cotista do banco de dados
    /// </summary>
    public class ExtratoCotistaRequest : MensagemRequestBase
    {
        public int? CodigoFundo { get; set; }

        public string NomeFundo { get; set; }

        public string CpfCnpj { get; set; }

        public int? CodigoCotista { get; set; }

        public DateTime DataDe { get; set; }

        public DateTime DataAte { get; set; }

        public char DownloadPendentes { get; set; }
    }
}
