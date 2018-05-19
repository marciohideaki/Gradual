using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class AplicacaoAgregadaInfo
    {
        public string CodigoAplicacao { get; set; }

        public string CodigoEspecie { get; set; }

        public string Prazo { get; set; }

        public DateTime Data { get; set; }

        public double Taxa { get; set; }

        public double Percental { get; set; }

        public DateTime DataHora { get; set; }
    }
}
