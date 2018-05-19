using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    public class CotistaFidcFundoInfo
    {
        public int IdCotistaFidcFundo { get; set; }
        public int IdCotistaFidc { get; set; }
        public int IdFundoCadastro { get; set; }
        public DateTime DtInclusao { get; set; }
        public string NomeCotista { get; set; }
        public string NomeFundo { get; set; }
        public string EmailCotista { get; set; }
    }
}
