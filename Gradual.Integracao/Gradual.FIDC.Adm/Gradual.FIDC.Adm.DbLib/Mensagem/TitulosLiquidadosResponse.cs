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
    /// Classe de response de Titulos Liquidados do banco de dados
    /// </summary>
    public class TitulosLiquidadosResponse : MensagemResponseBase
    {
        public List<TitulosLiquidadosInfo> ListaTitulos;

        public TitulosLiquidadosResponse()
        {
            ListaTitulos = new List<TitulosLiquidadosInfo>();
        }
    }
}
