﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class VerificaNomeInfo : ICodigoEntidade
    {

        public string DsEmail { get; set; }

        public string DsNome { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
