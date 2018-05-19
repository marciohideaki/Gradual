using Gradual.FIDC.Adm.DbLib.Dados;
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
    public class ExtratoCotistaResponse : MensagemResponseBase
    {
        public List<ExtratoCotistaInfo> ListaExtrato;

        public List<ListaFundosInfo> ListaFundos;

        public List<ListaCotistaInfo> ListaCotista;

        public ExtratoCotistaResponse()
        {
            ListaExtrato = new List<ExtratoCotistaInfo>();

            ListaFundos = new List<ListaFundosInfo>();

            ListaCotista = new List<ListaCotistaInfo>();
        }
    }
}
