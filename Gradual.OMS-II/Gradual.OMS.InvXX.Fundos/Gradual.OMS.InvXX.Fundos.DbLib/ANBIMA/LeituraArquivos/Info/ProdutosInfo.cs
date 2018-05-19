using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class ProdutosInfo
    {
        public string CpfCnpj { get; set; }

        public string IdCodigoAnbima { get; set; }

        public int IdProduto { get; set; }

        public string NomeProduto { get; set; }

        public string Risco { get; set; }

        public string HorarioLimite { get; set; }

        public DateTime DataInicio { get; set; }

        public Nullable<DateTime> DataFim { get; set; }

        public string NomeArquivoProspecto { get; set; }

        public ProdutosInfo()
        {
            this.NomeProduto = string.Empty;

            this.Risco = string.Empty;

            this.HorarioLimite = string.Empty;

            this.NomeArquivoProspecto = string.Empty;
        }
    }
}
