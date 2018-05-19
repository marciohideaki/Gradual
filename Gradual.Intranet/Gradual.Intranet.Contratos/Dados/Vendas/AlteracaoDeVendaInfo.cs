using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class AlteracaoDeVendaInfo : ICodigoEntidade
    {
        #region Propriedades

        public int Busca_IdVenda { get; set; }

        public int IdVendaAlteracao { get; set; }

        public int IdVenda { get; set; }

        public string DsPropriedades { get; set; }

        public string DsValoresAnteriores { get; set; }

        public string DsValoresModificados { get; set; }

        public DateTime DtData { get; set; }

        public string DsUsuario { get; set; }

        public string DsMotivo { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
