using System;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;

namespace Gradual.Intranet.Contratos.Dados.ControleDeOrdens
{
    public class ListarOrdensInfo : ICodigoEntidadeControleDeOrdens
    {
        public BuscarOrdensRequest ListarOrdensRequest { get; set; }
        public BuscarOrdensResponse ListarOrdensResponse { get; set; }

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
