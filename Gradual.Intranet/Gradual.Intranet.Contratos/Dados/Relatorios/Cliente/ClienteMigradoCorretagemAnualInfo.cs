using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteMigradoCorretagemAnualInfo : ICodigoEntidade
    {
        #region Propriedades

        /// <summary>
        /// Nome do Cliente
        /// </summary>
        public string NM_Cliente { get; set; }

        /// <summary>
        /// Nome do Assessor
        /// </summary>
        public string NM_Assessor { get; set; }

        /// <summary>
        /// Data de cadastro
        /// </summary>
        public DateTime DT_Criacao { get; set; }

        /// <summary>
        /// Data da última operação
        /// </summary>
        public DateTime DT_Ult_Oper { get; set; }

        public decimal Total { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
