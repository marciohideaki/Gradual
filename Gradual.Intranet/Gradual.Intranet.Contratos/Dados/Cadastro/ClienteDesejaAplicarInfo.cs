using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class ClienteDesejaAplicarInfo : ICodigoEntidade
    {
        #region ICodigoEntidade Members

        public int IdCliente { get; set; }

        /// <summary>
        /// 'BOVESPA', 'FUNDOS', 'AMBOS'
        /// </summary>
        public string TpDesejaAplicar { get; set; }


        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
