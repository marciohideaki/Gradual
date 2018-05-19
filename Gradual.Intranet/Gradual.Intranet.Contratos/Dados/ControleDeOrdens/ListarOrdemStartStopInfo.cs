using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Seguranca.Lib;


namespace Gradual.Intranet.Contratos.Dados.ControleDeOrdens
{
    public class ListarOrdemStartStopInfo : ICodigoEntidadeControleDeOrdens
    {
        public List<OrdemStartStopResponse> OrdemStarStopResponse { get; set; }

        public ParametrosDeBuscaOrdemStarStop ParametrosDeBuscaOrdemStarStop { get; set; }

        #region ICodigoEntidadeControleDeOrdens Members

        public AutenticarUsuarioRequest AutenticarUsuarioRequest
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
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
