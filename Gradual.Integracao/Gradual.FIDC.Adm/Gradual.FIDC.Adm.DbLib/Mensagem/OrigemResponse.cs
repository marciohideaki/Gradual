using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de resposta de Carteiras do banco de dados
    /// </summary>
    public class OrigemResponse: OMS.Library.MensagemResponseBase
    {
        public List<Dados.OrigemInfo> ListaOrigens;

        public OrigemResponse()
        {
            ListaOrigens = new List<Dados.OrigemInfo>();
        }
    }
}
