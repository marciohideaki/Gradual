using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteContratoInfo : ICodigoEntidade
    {
        #region Propriedades
        
        public Nullable<Int32> IdCliente { get; set; }
        
        public Nullable<Int32> CodigoBovespaCliente { get; set; }
        
        public Int32 IdContrato { get; set; }
    
        public DateTime DtAssinatura { get; set; }

        public List<int> LstIdContrato { get; set; }
        
        #endregion

        #region Construtor

        public ClienteContratoInfo() { }

        /// <summary>
        /// Construtor que espera um ID em forma de string, mas que deve ser um Int32
        /// </summary>
        /// <param name="pId"></param>
        public ClienteContratoInfo(string pId) 
        {
            this.IdCliente = int.Parse(pId);
        }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
