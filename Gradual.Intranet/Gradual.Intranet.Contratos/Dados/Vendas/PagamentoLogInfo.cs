using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class PagamentoLogInfo : ICodigoEntidade
    {
        #region Propriedades
        
        public int Busca_IdVenda { get; set; }

        public int IdPagamentoLog { get; set; }

        public DateTime DtData { get; set; }

        public string DsTransacao { get; set; }

        public string DsCodigoReferenciaVenda { get; set; }

        public string StDirecao { get; set; }

        public string DsMensagem { get; set; }

        public string DsXML { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
