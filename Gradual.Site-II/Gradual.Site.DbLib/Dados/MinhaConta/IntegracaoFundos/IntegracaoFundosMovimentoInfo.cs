using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosMovimentoInfo
    {
        public int IdProduto                        { get; set; }

        public DateTime Data                        { get; set; }

        public DateTime DataAtualizacao             { get; set; }

        public decimal AplicacaoMinimaInicial       { get; set; }

        public decimal ValorMinimoResgate           { get; set; }

        public decimal AplicacaoMinimaAdicional     { get; set; }

        public decimal SaldoMinimoAplicado          { get; set; }
        
        public string DsDiasConvAplicacao           { get; set; }
		                                            
        public string DsDiasConvResgate             { get; set; }
		                                            
        public string DsDiasConvResgateAntecipado   { get; set; }
		                                            
        public string DsDiasPagResgate              { get; set; }
		
        public decimal VlTaxaAdmin                  { get; set; }
		                                            
        public decimal VlTaxaAdminMaxima            { get; set; }
		                                            
        public string VlTaxaPerformance            { get; set; }
		                                            
        public decimal VlTaxaResgateAntecipado      { get; set; }
		                                            
        public decimal VlPatrimonioLiquido          { get; set; }
        
        public IntegracaoFundosMovimentoInfo()
        {

        }
    }
}
