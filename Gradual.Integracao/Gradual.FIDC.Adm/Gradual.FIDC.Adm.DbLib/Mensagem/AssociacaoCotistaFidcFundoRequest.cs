using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class AssociacaoCotistaFidcFundoRequest : MensagemRequestBase
    {
        public int IdCotistaFidcFundo { get; set; }
        public int IdCotistaFidc { get; set; }
        public int IdFundoCadastro { get; set; }
        public DateTime DtInclusao { get; set; }
    }
}
