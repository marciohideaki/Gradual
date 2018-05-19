using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info para gerenciar as informações de carteiras
    /// </summary>
    public class CarteirasInfo
    {
        public int CodigoFundo { get; set; }

        public string NomeFundo { get; set; }

        public int CodigoLocalidade { get; set; }

        public string Categoria { get; set; }

        public DateTime DownloadHora { get; set; }

        public string Status { get; set; }

        public string DownloadLink { get; set; }
    }
}
