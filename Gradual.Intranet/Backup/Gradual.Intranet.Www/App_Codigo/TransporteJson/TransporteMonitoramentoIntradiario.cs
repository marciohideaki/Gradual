using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Monitores.Risco.Lib;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteMonitoramentoIntradiario
    {
        public string CodigoCliente { get; set; }

        public string NomeCliente { get; set; }

        public string CodigoAssessor { get; set; }

        public string NomeAssessor { get; set; }

        public string Net { get; set; }

        public string SFP { get; set; }

        public string PercentualSFPxNET { get; set; }

        public string PercentualFINANxExposicao { get; set; }

        public string Exposicao { get; set; }

        public string Posicao { get; set; }

        public string Data { get; set; }

        public string CodigoClienteBmf { get; set; }
        #region Construtores
        public TransporteMonitoramentoIntradiario() { }

        public TransporteMonitoramentoIntradiario(ExposicaoClienteInfo info) 
        {
            decimal lFinanceiro                =  0 ;
            decimal lExposicao                 = info.LucroPrejuizoTOTAL;
            decimal lPercentualFINANxExposicao = lFinanceiro/ lExposicao;


            this.CodigoCliente             = info.CodigoBovespa.ToString();
            this.NomeCliente               = info.Cliente;
            this.CodigoAssessor            = info.Assessor;
            this.NomeAssessor              = info.NomeAssessor;
            this.Net                       = info.NetOperacoes.ToString("N2");
            this.SFP                       = info.SituacaoFinanceiraPatrimonial.ToString("N2");
            this.PercentualSFPxNET         = (info.NetOperacoes / (info.SituacaoFinanceiraPatrimonial == 0 ? 1 : info.SituacaoFinanceiraPatrimonial)).ToString();
            this.PercentualFINANxExposicao = lFinanceiro.ToString("N2");
                
        }

        public List<TransporteMonitoramentoIntradiario> TraduzirLista( List<MonitoramentoIntradiarioInfo> pInfo)
        {
            var lRetorno = new List<TransporteMonitoramentoIntradiario>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteMonitoramentoIntradiario()
                    {
                        CodigoAssessor            = info.CodigoAssessor.Value.ToString(),
                        CodigoCliente             = info.CodigoCliente.Value.ToString(),
                        NomeAssessor              = info.NomeAssessor,
                        NomeCliente               = info.NomeCliente,
                        Exposicao                 = info.Exposicao.ToString("N2"),
                        Net                       = info.Net.ToString("N2"),
                        SFP                       = info.SFP.ToString("N2"),
                        PercentualSFPxNET         = Math.Abs(info.NETxSFP).ToString("N2"),
                        PercentualFINANxExposicao = Math.Abs(info.EXPxPosicao).ToString("N2"),
                        Posicao                   = info.Posicao.ToString("N2"),
                        Data                      = info.Data.ToString("dd/MM/yyyy hh:mm"),
                        CodigoClienteBmf          = info.CodigoClienteBmf.HasValue ? info.CodigoClienteBmf.Value.ToString() : "0"
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}