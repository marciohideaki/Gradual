using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace CelulaAtivacao
{
    public class BOCelulaAtivacao
    {

        /// <summary>
        /// Faz chamada ao método GetDataM1 da camada de acesso a dados
        /// </summary>
        /// <param name="DataInicial">Data de Inicio</param>
        /// <param name="DataFinal">Data Final</param>
        /// <param name="Bovespa">Código Bovespa</param>
        /// <param name="CodigoAssessor">Código do Assessor</param>
        /// <returns></returns>
        public DataTable GetDataM1(DateTime DataInicial, DateTime DataFinal, int? Bovespa ,int? CodigoAssessor)
        {
            return new DAOCelulaAtivacao().GetDataM1(DataInicial, DataFinal, Bovespa ,CodigoAssessor);      
        }


        /// <summary>
        /// Faz chamada ao método GetDataM2 da camada de acesso a dados
        /// </summary>
        /// <param name="DataInicial">Data de Inicio</param>
        /// <param name="DataFinal">Data Final</param>
        /// <param name="Bovespa">Código Bovespa</param>
        /// <param name="CodigoAssessor">Código do Assessor</param>
        /// <returns></returns>
        public DataTable GetDataM2(DateTime DataInicial, DateTime DataFinal, int? Bovespa, int? CodigoAssessor)
        {
            return new DAOCelulaAtivacao().GetDataM2(DataInicial, DataFinal, Bovespa, CodigoAssessor);
        }


        /// <summary>
        /// Faz chamada ao método GetDataM3 da camada de acesso a dados
        /// </summary>
        /// <param name="DataInicial">Data de Inicio</param>
        /// <param name="DataFinal">Data Final</param>
        /// <param name="Bovespa">Código Bovespa</param>
        /// <param name="CodigoAssessor">Código do Assessor</param>
        /// <returns></returns>
        public DataTable GetDataM3(DateTime DataInicial, DateTime DataFinal, int? Bovespa, int? CodigoAssessor)
        {
            return new DAOCelulaAtivacao().GetDataM3(DataInicial, DataFinal, Bovespa, CodigoAssessor);
        }

        /// <summary>
        /// Obtem os assessores cadastrados no sistema sinacor.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssessor()
        {
            return new DAOCelulaAtivacao().GetAssessor();
        }



    }
}
