using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ArquivoContratoInfo : ICodigoEntidade
    {
        public int IdArquivoContrato { get; set; }

        public int IdContrato { get; set; }

        public byte[] Arquivo { get; set; }

        public string Extensao { get; set; }

        public string MIMEType { get; set; }

        public int Tamanho { get; set; }

        public string Nome { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
