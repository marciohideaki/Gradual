using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Alavancagem.Operacional;

namespace Gradual.OMS.Alavancagem.Operacional
{
    public class CustodiaAberturaInfo
    {

        public int ID_CLIENTE { set; get; }

        public string INSTRUMENTO { set; get; }

        public string TIPO_MERCADO { set; get; }

        public int QTDE_DISP { set; get; }

        public int COD_CARTEIRA { set; get; }

        public decimal VL_FECHAMENTO { set; get; }

        public int QTDE_PROJ_CPA_D0 { set; get; }

        public int QTDE_PROJ_CPA_D1 { set; get; }

        public int QTDE_PROJ_CPA_D2 { set; get; }

        public int QTDE_PROJ_CPA_D3 { set; get; }

        public int QTDE_PROJ_VDA_D0 { set; get; }

        public int QTDE_PROJ_VDA_D1 { set; get; }

        public int QTDE_PROJ_VDA_D2 { set; get; }

        public int QTDE_PROJ_VDA_D3 { set; get; }

        public int QTDE_PROJ_TOTAL_CPA { set; get; }

        public int QTDE_PROJ_TOTAL_VDA { set; get; }

        public int NET_QTDE_PROJ { set; get; }

        public int QTDE_TOTAL_DIA { set; get; }

        public int QTDE_TOTAL_NEGOCIAVEL { set; get; }

    }
}
