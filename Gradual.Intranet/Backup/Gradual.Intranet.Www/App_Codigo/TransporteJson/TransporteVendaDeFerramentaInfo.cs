using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteVendaDeFerramentaInfo
    {
        #region Propriedades

        public string Id { get; set; }

        public string CodigoDeReferencia { get; set; }

        public string CBLC { get; set; }

        public string CpfCnpj { get; set; }
        
        public string Status { get; set; }
        
        public string DescricaoStatus { get; set; }

        public string Data { get; set; }

        public string Quantidade { get; set; }

        public string Preco { get; set; }

        public string IdProduto { get; set; }

        public string DescProduto { get; set; }

        public string IdPagamento { get; set; }

        public string Tipo { get; set; }

        public string MetodoTipo { get; set; }

        public string MetodoCodigo { get; set; }

        public string MetodoDesc { get; set; }

        public string ValorBruto { get; set; }

        public string ValorDesconto { get; set; }

        public string ValorTaxas { get; set; }

        public string ValorTaxasProduto { get; set; }

        public string ValorLiquido { get; set; }

        public string QuantidadeDeParcelas { get; set; }

        public string PrecoUnit { get; set; }
        
        public string Alias { get; set; }

        public string Obs { get; set; }

        #endregion

        #region Construtor

        public TransporteVendaDeFerramentaInfo(VendaDeFerramentaInfo pVenda)
        {
            this.Id              = pVenda.IdVenda.DBToString();
            this.CodigoDeReferencia   = pVenda.DsCodigoReferencia;
            this.CBLC                 = pVenda.CdCBLC.DBToString();
            this.CpfCnpj              = pVenda.DsCpfCnpj;
            this.Status               = pVenda.StStatus.DBToString();
            this.DescricaoStatus      = pVenda.DescricaoStatus;
            this.Data                 = pVenda.DtData.ToString("dd/MM/yyyy HH:mm");
            this.Quantidade           = pVenda.VlQuantidade.DBToString();
            this.Preco                = pVenda.VlPreco.ToString("N2");
            this.IdProduto            = pVenda.IdProduto.DBToString();
            this.DescProduto          = pVenda.DsProduto.DBToString();
            this.IdPagamento          = pVenda.IdPagamento.DBToString();
            this.Tipo                 = pVenda.CdTipo.DBToString();
            this.MetodoTipo           = pVenda.CdMetodoTipo.DBToString();
            this.MetodoCodigo         = pVenda.CdMetodoCodigo.DBToString();
            this.MetodoDesc           = pVenda.DsMetodoDesc.DBToString();
            this.ValorBruto           = pVenda.VlValorBruto.ToString("N2");
            this.ValorDesconto        = pVenda.VlValorDesconto.ToString("N2");
            this.ValorTaxas           = pVenda.VlValorTaxas.ToString("N2");
            this.ValorTaxasProduto    = pVenda.VlValorTaxaProduto.ToString("N2");
            this.ValorLiquido         = pVenda.VlValorLiquido.ToString("N2");
            this.QuantidadeDeParcelas = pVenda.VlQuantidadeParcelas.ToString("N0");
            this.Obs                  = pVenda.DsObservacoes;

            this.Alias = this.DescProduto;

            if (this.Alias.Contains("(") && this.Alias.Contains(")"))   //assumindo que qualquer coisa com [Nome da Moeda (SIGLA)] é câmbio.
                this.Alias = "Câmbio";
            if (pVenda.VlQuantidade != 0)
                this.PrecoUnit = Convert.ToDecimal((pVenda.VlPreco - pVenda.VlValorTaxaProduto) / pVenda.VlQuantidade).ToString("N2");
        }

        #endregion
    }
}