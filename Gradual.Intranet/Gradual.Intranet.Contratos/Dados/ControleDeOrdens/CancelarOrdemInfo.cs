using System;
using System.Collections.Generic;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;

namespace Gradual.Intranet.Contratos.Dados.ControleDeOrdens
{
    public class CancelarOrdemInfo : ICodigoEntidadeControleDeOrdens
    {


        public ExecutarCancelamentoOrdemResponse CancelarOrdemResponse { get; set; }

        public List<ExecutarCancelamentoOrdemRequest> ListaCancelarOrdemRequest { get; set; }
        
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
