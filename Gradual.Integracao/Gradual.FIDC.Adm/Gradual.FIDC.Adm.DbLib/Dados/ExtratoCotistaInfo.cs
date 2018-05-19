using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info para gerenciar as informações de Extrato de carteiras
    /// </summary>
    public class ExtratoCotistaInfo
    {
        public string CpfCnpj { get; set; }

        public string NomeCotista { get; set; }

        public int CodigoFundo { get; set; }

        public string NomeFundo { get; set; }

        public string Status { get; set; }

        public string DownloadLink { get; set; }
    }
}
