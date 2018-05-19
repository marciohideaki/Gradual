using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class FrasesInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código da Frase
        /// </summary>
        public Nullable<int> IdFrase { get; set; }

        /// <summary>
        /// Descrição da pergunta
        /// </summary>
        public string DsFrase { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
