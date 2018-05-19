using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Classe de Contas do banco de cliente
    /// </summary>
    public class ClienteBancoInfo :  ICodigoEntidade
    {
        #region Propriedades

        /// <summary>
        /// Código do banco
        /// </summary>
        public Nullable<Int32> IdBanco { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>
        public Int32 IdCliente { get; set; }

        /// <summary>
        /// Código da agencia
        /// </summary>
        public string DsAgencia { get; set; }

        /// <summary>
        /// Dígito da agencia
        /// </summary>
        public string DsAgenciaDigito { get; set; }

        /// <summary>
        /// Código da Conta
        /// </summary>
        public string DsConta { get; set; }

        /// <summary>
        /// Tipo de conta
        /// </summary>
        public string TpConta { get; set; }
        
        /// <summary>
        /// Código da agencia
        /// </summary>
        public string DsCpfTitular { get; set; }

        /// <summary>
        /// Código da agencia
        /// </summary>
        public string DsNomeTitular { get; set; }

        private string dsContaDigito;
        /// <summary>
        /// Digito da Conta
        /// </summary>
        public string DsContaDigito
        {
            set 
            {
                if (value.ToLower().Contains("x"))
                    this.dsContaDigito = value.Replace('x', '0');

                this.dsContaDigito = value;
            }
            get
            {
                if (null != this.dsContaDigito && this.dsContaDigito.ToLower().Contains("x"))
                    return this.dsContaDigito.Replace('x', '0');

                return this.dsContaDigito;
            }
        }

        /// <summary>
        ///indicador de conta Principal do cliente
        /// </summary>
        public bool StPrincipal { get; set; }

        /// <summary>
        /// Código do banco
        /// </summary>
        public string CdBanco { get; set; }

        #endregion

        #region Construtor

        public ClienteBancoInfo() { }
        
        public ClienteBancoInfo(string pIdCliente) 
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
