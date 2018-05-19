#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
#endregion

namespace Gradual.Intranet.Contratos.Dados.Monitoramento
{
    public class MonitoramentoDesbloqueioCustodiaInfo : ICodigoEntidade
    {
        public int CodBovespa { get; set; }

        public int Quantidade { get; set; }

        public string Instrumento { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
