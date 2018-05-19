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
    /// Classe de response de Mec do banco de dados
    /// </summary>
    public class MecResponse : MensagemResponseBase
    {
        public List<MecInfo> ListaMec;

        public MecResponse()
        {
            ListaMec = new List<MecInfo>();
        }
    }
}
