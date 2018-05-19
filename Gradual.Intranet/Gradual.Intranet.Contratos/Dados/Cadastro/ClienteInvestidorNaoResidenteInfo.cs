using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteInvestidorNaoResidenteInfo : ICodigoEntidade
    {
        #region Propriedades
        
        /// <summary>
        /// Id do Investidor não residente
        /// </summary>       
        public Nullable<int> IdInvestidorNaoResidente { get; set; }

        /// <summary>
        /// Código do Cliente
        /// </summary>       
        public int IdCliente { get; set; }

        /// <summary>
        /// Nome do Representante Legal
        /// </summary>
        public string DsRepresentanteLegal { get; set; }

        /// <summary>
        /// Custodiante no País
        /// </summary>
        public string DsCustodiante { get; set; }

        /// <summary>
        /// Retificação de Dados do Empregador
        /// </summary>
        public int DsRde { get; set; }

        /// <summary>
        /// Código Operacional CVM
        /// </summary>]
        public int DsCodigoCvm { get; set; }

        /// <summary>
        /// País de Origem
        /// </summary>       
        public string CdPaisOrigem { get; set; }

        /// <summary>
        /// Nome do administrador da carteira
        /// </summary>       
        public string DsNomeAdiministradorCarteira { get; set; }

        #endregion

        #region Construtor

        public ClienteInvestidorNaoResidenteInfo() { }
        
        public ClienteInvestidorNaoResidenteInfo(string pIdCliente)
        {
            this.IdCliente = int.Parse(pIdCliente);
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
