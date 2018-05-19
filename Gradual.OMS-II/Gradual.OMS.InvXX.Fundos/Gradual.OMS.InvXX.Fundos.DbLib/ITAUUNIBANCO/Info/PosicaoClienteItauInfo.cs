using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO.Info
{
    public class PosicaoClienteItauInfo
    {
        public int CodigoCliente { get; set; }

        public int IdMovimento { get; set; }

        public int IdProcessamento { get; set; }

        public string DsCpfCnpj { get; set; }

        public string Banco { get; set; }

        public string Angencia { get; set; }

        public string Conta { get; set; }

        public string DigitoConta { get; set; }

        public string SubConta { get; set; }

        public string IdCotista { get; set; }

        public int IdFundo { get; set; }

        public decimal QuantidadeCotas {get;set;}

        public decimal ValorCota { get; set; }

        public decimal ValorBruto { get; set; }

        public decimal ValorIR { get; set; }

        public decimal ValorIOF { get; set; }

        public decimal ValorLiquido { get; set; }

        public DateTime DtReferencia { get; set; }

        public DateTime DtProcessamento { get; set; }
    }
}
