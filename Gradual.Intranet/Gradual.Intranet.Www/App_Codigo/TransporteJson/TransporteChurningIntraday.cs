using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Monitores.Risco.Lib;
using Gradual.Intranet.Contratos.Dados.Compliance;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteChurningIntraday
    {
        public string CodigoCliente { get; set; }

        public string NomeCliente { get; set; }

        public string CodigoAssessor { get; set; }

        public string NomeAssessor { get; set; }

        public string ValorVendas { get; set; }

        public string ValorCompras { get; set; }
        
        public string ValorCarteira { get; set; }
        
        public string PercentualTRnoDia { get; set; }
        
        public string PercentualTRnoPeriodo { get; set; }
        
        public string PercentualCEnoDia { get; set; }
        
        public string PercentualCEnoPeriodo { get; set; }

        public string Data { get; set; }

        public string CarteiraMedia { get; set; }

        public string ValorCarteiraDia { get; set; }

        public string ValorL1 { get; set; }

        public string Portas { get; set; }

        public string TipoPessoa { get; set; }

        public string Corretagem { get; set; }

        #region Construtores
        public TransporteChurningIntraday() { }

        public TransporteChurningIntraday(ExposicaoClienteInfo info) 
        {
            //decimal lFinanceiro                =  0 ;
            //decimal lExposicao                 = info.LucroPrejuizoTOTAL;
            //decimal lPercentualFINANxExposicao = lFinanceiro/ lExposicao;


            //this.CodigoCliente             = info.CodigoBovespa.ToString();
            //this.NomeCliente               = info.Cliente;
            //this.CodigoAssessor            = info.Assessor;
            //this.NomeAssessor              = info.NomeAssessor;
            //this.Net                       = info.NetOperacoes.ToString("N2");
            //this.SFP                       = info.SituacaoFinanceiraPatrimonial.ToString("N2");
            //this.PercentualSFPxNET         = (info.NetOperacoes / (info.SituacaoFinanceiraPatrimonial == 0 ? 1 : info.SituacaoFinanceiraPatrimonial)).ToString();
            //this.PercentualFINANxExposicao = lFinanceiro.ToString("N2");
                
        }

        public List<TransporteChurningIntraday> TraduzirLista(List<ChurningIntradayInfo> pInfo)
        {
            var lRetorno = new List<TransporteChurningIntraday>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteChurningIntraday()
                    {
                        CodigoAssessor            = info.CodigoAssessor.Value.ToString(),
                        CodigoCliente             = info.CodigoCliente.Value.ToString(),
                        NomeAssessor              = info.NomeAssessor,
                        NomeCliente               = info.NomeCliente,
                        Data                      = info.Data.ToString("dd/MM/yyyy hh:mm"),
                        ValorCarteira             = info.ValorCarteira.ToString("N2"),
                        ValorCompras              = info.ValorCompras.ToString("N2"),
                        ValorVendas               = info.ValorVendas.ToString("N2"),
                        PercentualCEnoDia         = info.PercentualCEnoDia.ToString("N5"),
                        PercentualCEnoPeriodo     = info.PercentualCEnoPeriodo.ToString("N5"),
                        PercentualTRnoDia         = info.PercentualTRnoDia.ToString("N5"),
                        PercentualTRnoPeriodo     = info.PercentualTRnoPeriodo.ToString("N5"),
                        CarteiraMedia             = info.ValorCarteiraMedia.ToString("N2"),
                        ValorCarteiraDia          = info.ValorCarteiraDia.ToString("N2"),
                        ValorL1                   = info.ValorL1.ToString("N2"),
                        Portas                    = info.Porta,
                        TipoPessoa                = info.TipoPessoa,
                        Corretagem                = info.ValorCorretagem.ToString("N2")
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}