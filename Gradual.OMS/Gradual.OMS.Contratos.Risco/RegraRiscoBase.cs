using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco
{
    /// <summary>
    /// Representa uma regra base de risco.
    /// </summary>
    public class RegraRiscoBase : RegraBase
    {
        /// <summary>
        /// Construtor principal
        /// </summary>
        /// <param name="regraInfo"></param>
        public RegraRiscoBase(RegraRiscoInfo regraInfo) : base((RegraInfo)regraInfo)
        {
        }

        /// <summary>
        /// Retorna o regraInfo como regraRiscoInfo
        /// </summary>
        public RegraRiscoInfo RegraInfo2 
        {
            get { return (RegraRiscoInfo)this.RegraInfo; }
        }

        public override string ToString()
        {
            return this.NomeRegra;
        }
    }
}
