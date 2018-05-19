using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ProventosClienteInfo : ICodigoEntidade
    {
        public Nullable<int> CodigoCliente          { get; set; }
        public string NomeCliente                   { get; set; }
        public Nullable<int> CodigoAssessor         { get; set; }
        public string NomeAssessor                  { get; set; }
        public string Isin                          { get; set; }
        public int Distribuicao                     { get; set; }
        public int Carteira                         { get; set; }
        public string Ativo                         { get; set; }
        public int Quantidade                       { get; set; }
        public decimal Valor                        { get; set; }
        public decimal PercentualIR                 { get; set; }
        public decimal ValorIR                      { get; set; }
        public decimal ValorLiquido                 { get; set; }
        public string TipoProvento                  { get; set; }
        public string GrupoProvento                 { get; set; }
        public DateTime DataPagamento               { get; set; }
        public string Emitente                      { get; set; }
        public int IdUsuarioLogado                  { get; set; }


        public DateTime DataDe { get; set; }
        public DateTime DataAte { get; set; }

        public List<ProventosClienteInfo> Resultado { get; set; }

        public ProventosClienteInfo()
        {
            Resultado = new List<ProventosClienteInfo>();
        }
        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
