using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Cotacao.Lib.Mensageria;
using Gradual.OMS.Risco.Persistencia.Lib;
 

namespace Gradual.OMS.Cotacao
{
    public class ServicoCotacaoOMS:IServicoCotacaoOMS
    {

        #region IServicoCotacaoOMS Members

        public Lib.Mensageria.EnviarCotacaoResponse ObterCotacaoInstrumento(Lib.Mensageria.EnviarCotacaoRequest request)
        {
            PersistenciaCotacao PersistenciaCotacao 
                = new PersistenciaCotacao();

            try{
                return PersistenciaCotacao.ObterCotacaoInstrumento(request);
            }
            catch (Exception ex){
                throw (ex);
            }

        }

        #endregion
    }
}
