using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Classe para preenchimento de campos (Formulário) de controlador
    /// </summary>
    public class ClienteControladoraInfo : ICodigoEntidade
    {
        #region Propriedades
        /// <summary>
        /// Código do Controlador
        /// </summary>
        public Nullable<Int32> IdClienteControladora { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Nome ou razão social do Controlador
        /// </summary>
        public string DsNomeRazaoSocial { get; set; }


        /// <summary>
        /// Documento do Controlador
        /// </summary>
        public string DsCpfCnpj { get; set; }
        #endregion

        #region Construtor

        public ClienteControladoraInfo() { }
        
        public ClienteControladoraInfo(string pIdCliente) 
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
