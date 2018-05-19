#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
#endregion

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Classe para preenchimento de pessoa vinculado
    /// </summary>
    public class ClientePessoaVinculadaInfo : ICodigoEntidade
    {
        #region Propriedades
        /// <summary>
        /// código da pessoa vinculada
        /// </summary>
        public int IdPessoaVinculada { get; set; }

        /// <summary>
        /// Nome da pessoa vinculada
        /// </summary>
        public string DsNome { get; set; }

        /// <summary>
        /// Pessoa vinculada Ativada/Inativada
        /// </summary>
        public bool StAtivo { get; set; }

        /// <summary>
        /// Cpf da pessoa Vinculada
        /// </summary>
        public string DsCpfCnpj { get; set; }

        /// <summary>
        /// Código da pessoa vinculada responsável
        /// </summary>
        public Nullable<int> IdPessoaVinculadaResponsavel { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>
        public Nullable<int> IdCliente { get; set; }

        /// <summary>
        /// Enum de Receber Pessoa Vinculada
        /// </summary>
        public eReceberPessoaVinculada ReceberPessoaVinculada { get; set; } 
        

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
