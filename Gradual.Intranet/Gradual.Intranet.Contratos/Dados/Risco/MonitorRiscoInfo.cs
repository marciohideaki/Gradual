using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Risco 
{
    public class MonitorRiscoInfo : ICodigoEntidade
    {
        public int? CodigoCliente { get; set; }

        public int? CodigoClienteBmf { get; set; }

        public int? CodigoAssessor { get; set; }

        public decimal ValorGarantiaDeposito { get; set; }

        public DateTime DataMovimentoGarantia { get; set; }

        public decimal ValorMargemTeorica { get; set; }  // 

        public decimal ValorMargemRequerida { get; set; } // PRC_SALDOCLIENTE_BMF

        public decimal ValorMargemRequeridaBovespa { get; set; }

        public string NomeCliente { set; get; }

        public string NomeAssessor { get; set; }
        
        public string Assessor { set; get; }

        public string StatusBovespa { get; set; }

        public string StatusBMF { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MonitorRiscoGarantiaBMFInfo : ICodigoEntidade
    {
        public List<MonitorRiscoGarantiaBMFInfo> ListaGarantias;

        public int? CodigoClienteBmf { get; set; }

        public decimal ValorGarantiaDeposito { get; set; }

        public string DescricaoGarantia { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MonitorRiscoGarantiaBMFOuroInfo : ICodigoEntidade
    {
        public List<MonitorRiscoGarantiaBMFOuroInfo> ListaGarantias;

        public int? CodigoClienteBmf { get; set; }

        public decimal ValorGarantiaDeposito { get; set; }

        public string DescricaoGarantia { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MonitorRiscoGarantiaBovespaInfo : ICodigoEntidade
    {
        public List<MonitorRiscoGarantiaBovespaInfo> ListaGarantias;

        public int? CodigoClienteBov { get; set; }

        public decimal ValorGarantiaDeposito { get; set; }

        public string DescricaoGarantia { get; set; }

        public DateTime DtDeposito { get; set; }

        public string FinalidadeGarantia { get; set; }

        public string CodigoAtividade { get; set; }

        public string CodigoIsin { get; set; }

        public int CodigoDistribuicao { get; set; }

        public string NomeEmpresa { get; set; }

        public int Quantidade { get; set; }
        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class MonitorRiscoFeriadosInfo : ICodigoEntidade
    {
        public List<DateTime> ListaFeriados;

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
