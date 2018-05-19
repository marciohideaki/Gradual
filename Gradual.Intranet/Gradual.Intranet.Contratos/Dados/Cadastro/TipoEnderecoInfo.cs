using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Entidade referente à tabela de tipo de endereços
    /// </summary>
    /// <DataCriacao>22/05/2010</DataCriacao>
    /// <Autor>Bruno Varandas Ribeiro</Autor>
    /// <Alteracao>
    ///     <DataAlteracao></DataAlteracao>
    ///     <Autor></Autor>
    ///     <Motivo></Motivo>
    /// </Alteracao>
    public class TipoEnderecoInfo : ICodigoEntidade
    {
        #region Propriedades

        /// <summary>
        /// Codigo do tipo do endereço
        /// </summary>
        public Nullable<int> IdTipoEndereco { set; get; }

        /// <summary>
        /// Descrição do tipo de endereço
        /// </summary>
        public string DsEndereco { set; get; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
