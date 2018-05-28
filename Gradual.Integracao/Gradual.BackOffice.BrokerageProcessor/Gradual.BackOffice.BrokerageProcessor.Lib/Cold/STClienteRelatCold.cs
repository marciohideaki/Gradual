using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Cold
{
    public struct STClienteRelatCold
    {
        public int Account { get; set; }
        public int IDGrupo { get; set; }
        public bool FlagBTC { get; set; }
        public bool FlagGarantia { get; set; }
        public bool FlagExigencia { get; set; }
        public bool FlagTermo { get; set; }
        public bool FlagLiqInvest { get; set; }
        public bool FlagPosDivBtc { get; set; }
        public bool FlagPosCliente { get; set; }
        public bool FlagCustodia { get; set; }
        public string EmailsTO { get; set; }
        public string EmailsCC { get; set; }
        public string EmailsBCC { get; set; }
        public bool FlagFolhaUnica { get; set; }
        public bool FlagAnexo { get; set; }
        public bool FlagPdf { get; set; }
        public bool FlagZip { get; set; }
        public bool FlagSendFlatImbarq { get; set; }
        public bool FlagConvertCold { get; set; }
    }
}
