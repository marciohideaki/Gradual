using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de response de Carteiras do banco de dados
    /// </summary>
    public class CarteiraResponse : MensagemResponseBase
    {
        public List<CarteirasInfo> ListaCarteira;

        public CarteiraResponse()
        {
            ListaCarteira = new List<CarteirasInfo>();
        }
    }
}
