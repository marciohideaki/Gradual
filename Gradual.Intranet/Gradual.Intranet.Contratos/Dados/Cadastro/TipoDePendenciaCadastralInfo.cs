using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Entidade referente à tabela de tipo de pendencia cadastral
    /// </summary>
    /// <DataCriacao>05/05/2010</DataCriacao>
    /// <Autor>Rafael Sanches Garcia</Autor>
    /// <Alteracao>
    ///     <DataAlteracao></DataAlteracao>
    ///     <Autor></Autor>
    ///     <Motivo></Motivo>
    /// </Alteracao>
    public class TipoDePendenciaCadastralInfo : ICodigoEntidade
    {
        #region Propriedades

        /// <summary>
        /// Codigo da pendencia cadastral
        /// </summary>
        public Nullable<int> IdTipoPendencia { set; get; }

        /// <summary>
        /// Descrição da pendencia cadastral
        /// </summary>
        public string DsPendencia { set; get; }

        public Boolean StAutomatica { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
