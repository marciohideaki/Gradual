using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundosInfo
    {
        public string CodigoFundo { get; set; }

        public string CodInst { get; set; }

        public string NomeFantasia { get; set; }

        public int CodigoTipo { get; set; }

        public DateTime DataInicioFundo { get; set; }

        public DateTime DataFimFundo { get; set; }

        public DateTime DataInfo { get; set; }

        public char PerfilCota { get; set; }

        public DateTime DataDiv { get; set; }

        public string RazaoSocial { get; set; }

        public string DsCnpj { get; set; }

        public char Aberto { get; set; }

        public char Exclusivo { get; set; }

        public string PrazoConvCotas { get; set; }

        public string PrazoConvResg { get; set; }

        public string PrazoPgtoResg { get; set; }

        public int CarenciaUniversal { get; set; }

        public int CarenciaCiclica { get; set; }

        public char CotaAbertura { get; set; }

        public int PeriodoDivulg { get; set; }

        public DateTime DataHora { get; set; }
    }
}
