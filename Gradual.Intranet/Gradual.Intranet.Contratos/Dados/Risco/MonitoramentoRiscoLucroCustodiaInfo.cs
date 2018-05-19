using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using System.Collections;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    public class    MonitoramentoRiscoLucroCustodiaInfo : ICodigoEntidade
    {
        #region | Propriedades
        
        public int? CdCodigo { get; set; }
        
        public string DsNomeCliente { get; set; }
        
        public List<CustodiaMovimento> ListaMovimento { get; set; }

        public int? ConsultaCdClienteBovespa { get; set; }

        public int? ConsultaCdClienteBMF { get; set; }

        public DateTime? ConsultaDtVencimentoTermo { get; set; }


        #endregion

        #region | Estruturas
        
        public struct CustodiaMovimento
        {
            public string DsTomador { get; set; }
            
            public string CdISIN { get; set; }
            
            public DateTime? DtOrigem { get; set; }
            
            public DateTime? DtAbertura { get; set; }
            
            public DateTime? DtVencimento { get; set; }
            
            public decimal? VlPrecoMedio { get; set; }
            
            public decimal? VlTaxaRemuneracao { get; set; }
            
            public decimal? VlBruto { get; set; }
            
            public decimal? VlLiquido { get; set; }
            
            public int BloqueadoPorLancamentoOpcao { get; set; }
            
            public int CodigoCarteira { get; set; }
            
            public string CodigoInstrumento { get; set; }
            
            public string DescricaoCarteira { get; set; }
            
            public int IdCliente { get; set; }
            
            public string NomeEmpresa { get; set; }
            
            public decimal QtdeAExecCompra { get; set; }
            
            public decimal QtdeAExecVenda { get; set; }
            
            public decimal QtdeAtual { get; set; }
            
            public decimal QtdeDisponivel { get; set; }
            
            public decimal QtdeLiquidar { get; set; }
            
            public string TipoGrupo { get; set; }
            
            public string TipoMercado { get; set; }

            public decimal ValorPosicao { get; set; }

            public decimal QtdeD1 { get; set; }

            public decimal QtdeD2 { get; set; }

            public decimal QtdeD3 { get; set; }

            public decimal QtdeDATotal { get; set; }

            public string CodigoSerie { get; set; }

            public decimal FatorCotacao { get; set; }

            public decimal QtdLiquidar { get; set; }
        }

        #endregion

        #region | Construtores

        public MonitoramentoRiscoLucroCustodiaInfo()
        {
            this.ListaMovimento = new List<CustodiaMovimento>();
        }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MonitoramentoRiscoLucroVencimentosDI : ICodigoEntidade
    {
        public Hashtable VencimentosDI { get; set; }
        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MonitoramentoRiscoLucroTaxaPTAXInfo : ICodigoEntidade
    {
        public decimal ValorPtax { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo : ICodigoEntidade
    {
        public int? CdCodigo { get; set; }

        public string DsNomeCliente { get; set; }


        public int? ConsultaCdClienteBovespa { get; set; }

        public int? ConsultaCdClienteBMF { get; set; }

        public DateTime? ConsultaDtVencimentoTermo { get; set; }

        public List<CustodiaPosicao> ListaPosicaoDia { get; set; }

        #region Structures
        public struct CustodiaPosicao
        {
            public string DsTomador { get; set; }

            public string CdISIN { get; set; }

            public DateTime? DtOrigem { get; set; }

            public DateTime? DtAbertura { get; set; }

            public DateTime? DtVencimento { get; set; }

            public decimal? VlPrecoMedio { get; set; }

            public decimal? VlTaxaRemuneracao { get; set; }

            public decimal? VlBruto { get; set; }

            public decimal? VlLiquido { get; set; }

            public int BloqueadoPorLancamentoOpcao { get; set; }

            public int CodigoCarteira { get; set; }

            public string CodigoInstrumento { get; set; }

            public string DescricaoCarteira { get; set; }

            public int IdCliente { get; set; }

            public string NomeEmpresa { get; set; }

            public decimal QtdeAExecCompra { get; set; }

            public decimal QtdeAExecVenda { get; set; }

            public decimal QtdeAtual { get; set; }

            public decimal QtdeDisponivel { get; set; }

            public string TipoGrupo { get; set; }

            public string TipoMercado { get; set; }

            public double ValorPosicao { get; set; }

            public decimal QtdeDATotal { get; set; }

            public string CodigoSerie { get; set; }

            public decimal PrecoNegocioCompra { get; set; }

            public decimal PrecoNegocioVenda { get; set; }

            public string Sentido { get; set; }

            public decimal Diferenca { get; set; }

        }
        #endregion

        #region Construtores
        public MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo()
        {
            this.ListaPosicaoDia = new List<CustodiaPosicao>();
        }
        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
