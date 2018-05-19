using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    public class MonitoramentoRiscoLucroPrejuizoParametrosInfo :  ICodigoEntidade
    {
        public int IdJanela { get; set; }

        public int IdUsuario { get; set; }

        public string NomeJanela { get; set; }

        public string Consulta { get; set; }

        public string Colunas { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
