using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento
{
    public class CustodiaInfo : Gradual.OMS.Library.ICodigoEntidade
    {
        public Nullable<int> CodigoAssessor         { get; set; }
        public Nullable<int> CodigoCliente          { get; set; }
        public System.String CodigoAtivo            { get; set; }
        public System.String CodigoMercado          { get; set; }
        public Nullable<int> CodigoCarteira         { get; set; }
        public Nullable<int> QuantidadeDisponivel   { get; set; }
        public Nullable<int> QuantidadeD1           { get; set; }
        public Nullable<int> QuantidadeD2           { get; set; }
        public Nullable<int> QuantidadeD3           { get; set; }
        public Nullable<int> QuantidadePendente     { get; set; }
        public Nullable<int> QuantidadeALiquidar    { get; set; }
        public Nullable<int> QuantidadeTotal        { get; set; }

        string OMS.Library.ICodigoEntidade.ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        public CustodiaInfo() { }
        
        public CustodiaInfo(CustodiaInfo custodia)
        {
            this.CodigoAssessor         = custodia.CodigoAssessor;
            this.CodigoCliente          = custodia.CodigoCliente;
            this.CodigoAtivo            = custodia.CodigoAtivo;
            this.CodigoMercado          = custodia.CodigoMercado;
            this.CodigoCarteira         = custodia.CodigoCarteira;
            this.QuantidadeDisponivel   = custodia.QuantidadeDisponivel;
            this.QuantidadeD1           = custodia.QuantidadeD1;
            this.QuantidadeD2           = custodia.QuantidadeD2;
            this.QuantidadeD3           = custodia.QuantidadeD3;
            this.QuantidadePendente     = custodia.QuantidadePendente;
            this.QuantidadeALiquidar    = custodia.QuantidadeALiquidar;
            this.QuantidadeTotal        = custodia.QuantidadeTotal;

        }
    }
}
