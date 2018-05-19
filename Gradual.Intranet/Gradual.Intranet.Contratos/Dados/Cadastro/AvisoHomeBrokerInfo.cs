using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class AvisoHomeBrokerInfo: ICodigoEntidade
    {
        #region Propriedades

        public int IdAviso { get; set; }

        public int IdSistema { get; set; }

        public string DsAviso { get; set; }

        public string DsCBLCs { get; set; }

        public DateTime DtEntrada { get; set; }

        public DateTime DtSaida { get; set; }

        public string StAtivacaoManual { get; set; }

        #endregion

        #region Métodos Públicos

        public bool ContemCBLC(string pCBLC)
        {
            if (!string.IsNullOrEmpty(this.DsCBLCs))
            {
                string[] lCBLCs = this.DsCBLCs.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string lCBLC in lCBLCs)
                {
                    if (lCBLC.Trim() == pCBLC)
                    {
                        return true;
                    }
                }
            }

            return false;
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
