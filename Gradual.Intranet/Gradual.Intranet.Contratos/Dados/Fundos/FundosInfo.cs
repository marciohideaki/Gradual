using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Fundos
{
    public class FundosInfo : ICodigoEntidade
    {
        public int          IdCliente           { get; set; }
        public string       Nome                { get; set; }
        public string       Apelido             { get; set; }
        public int          CodigoFundo         { get; set; }
        public string       CodigoFundoItau     { get; set; }
        public int          Administrador       { get; set; }
        public int          TipoCarteira        { get; set; }
        public int          CGC                 { get; set; }
        public string       NomeFundo           { get; set; }
        public string       Operacao            { get; set; }
        public decimal?     Cota                { get; set; }
        public decimal?     Quantidade          { get; set; }
        public decimal?     ValorBruto          { get; set; }
        public decimal?     IR                  { get; set; }
        public decimal?     IOF                 { get; set; }
        public decimal?     ValorLiquido        { get; set; }
        public DateTime?    DataInicioPesquisa  { get; set; }
        public DateTime?    DataFimPesquisa     { get; set; }
        public DateTime?    DataAtualizacao     { get; set; }
        public string       CodigoFundoAnbima   { get; set; }
        public string       CodigoCotistaItau   { get; set; }
        public string       Origem              { get; set; }
        public string       UsuarioLogado       { get; set; }
        public int          CodigoUsuarioLogado { get; set; }
        public int          CodigoFundoTermo    { get; set; }
        public int          CodigoClienteFundo  { get; set; }
        public DateTime     DataAdesao          { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
        
    }
}
