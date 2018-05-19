using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de request de Carteiras do banco de dados
    /// </summary>
    public class CarteiraRequest : MensagemRequestBase
    {
        public int? CodigoFundo { get; set; }

        public string NomeFundo { get; set; }

        public DateTime DataDe { get; set; }

        public DateTime DataAte { get; set; }

        public int? CodigoLocalidade { get; set; }

        public char DownloadsPendentes { get; set; } 
    }
}
