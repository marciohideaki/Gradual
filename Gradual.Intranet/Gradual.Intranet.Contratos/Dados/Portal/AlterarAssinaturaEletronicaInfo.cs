using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class AlterarAssinaturaEletronicaInfo : ICodigoEntidade
    {

        public int IdLogin { get; set; }
        public string CdAssinaturaAntiga { get; set; }
        public string CdAssinaturaNova { get; set; }
        public string CodigoPrincipal { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class AlterarAssinaturaEletronicaDinamicaInfo : AlterarAssinaturaEletronicaInfo
    {
        public Gradual.OMS.Seguranca.Lib.AssinaturaInfo AssinaturaDinamica { get; set; }
        public Gradual.OMS.Seguranca.Lib.AssinaturaInfo AssinaturaDinamicaNova { get; set; }
    }
}
