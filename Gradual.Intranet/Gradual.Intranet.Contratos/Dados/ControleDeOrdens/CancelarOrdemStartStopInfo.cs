using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;

namespace Gradual.Intranet.Contratos.Dados.ControleDeOrdens
{
    public class CancelarOrdemStartStopInfo : ICodigoEntidadeControleDeOrdens
    {
        public List<CancelarStartStopOrdensRequest> ListaCancelarStartStopOrdensRequest
        {
            get;
            set;
        }

        public CancelarStartStopOrdensResponse CancelarStartStopOrdensResponse
        {
            get;
            set;
        }

        #region ICodigoEntidadeControleDeOrdens Members

        public AutenticarUsuarioRequest AutenticarUsuarioRequest
        {
            get;
            set;
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
