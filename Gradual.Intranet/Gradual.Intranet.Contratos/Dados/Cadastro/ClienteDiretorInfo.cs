using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteDiretorInfo : ICodigoEntidade
    {
        #region Propriedades

        public Nullable<Int32> IdClienteDiretor { get; set; }

        public int IdCliente { get; set; }

        public string DsIdentidade { get; set; }

        public string NrCpfCnpj { get; set; }

        /// <summary>
        /// Nome do Diretor
        /// </summary>
        public string DsNome { get; set; }

        /// <summary>
        /// Orgão emissor do documento 
        /// </summary>
        public string CdOrgaoEmissor { get; set; }

        /// <summary>
        /// Código da UF do orgão emissor
        /// </summary>
        public string CdUfOrgaoEmissor { get; set; }

        #endregion

        #region Construtor

        public ClienteDiretorInfo() { }

        public ClienteDiretorInfo(string pIdCliente)
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
