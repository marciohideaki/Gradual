using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.Monitores.Risco.Enum;

namespace Gradual.Monitores.Risco.Lib
{
    [Serializable]
    [DataContract]
    public class ExposicaoClienteInfo
    {
        [DataMember]
        public string Cliente { set; get; }

        [DataMember]
        public string Assessor { set; get; }

        [DataMember]
        public int CodigoBovespa { set; get; }

        [DataMember]
        public int CodigoBMF { set; get; }

        [DataMember]
        public decimal SaldoContaMargem { set; get; }

        [DataMember]
        public decimal TotalGarantias { set; get; }

        [DataMember]
        public decimal SaldoBMF { set; get; }

        [DataMember]
        public decimal ContaCorrenteAbertura { set; get; }

        [DataMember]
        public decimal CustodiaAbertura { set; get; }

        [DataMember]
        public decimal PLAberturaBovespa { set; get; }

        [DataMember]
        public decimal PLAberturaBMF { set; get; }

        [DataMember]
        public decimal LucroPrejuizoTermo { set; get; }

        [DataMember]
        public decimal SituacaoFinanceiraPatrimonial { set; get; }

        [DataMember]
        public decimal TotalFundos { set; get; }

        [DataMember]
        public List<ClienteFundoInfo> PosicaoFundos { set; get; }

        [DataMember]
        public decimal LucroPrejuizoBMF { set; get; }

        [DataMember]
        public decimal LucroPrejuizoBOVESPA { set; get; }

        [DataMember]
        public decimal LucroPrejuizoTOTAL { set; get; }

        [DataMember]
        public decimal TotalContaCorrenteTempoReal { set; get; }

        [DataMember]
        public decimal PatrimonioLiquidoTempoReal { set; get; }

        [DataMember]
        public decimal NetOperacoes { set; get; }

        [DataMember]
        public LimitesInfo LimitesOperacionais { set; get; }

        [DataMember]
        public List<BTCInfo> OrdensBTC { set; get; }

        [DataMember]
        public List<PosicaoTermoInfo> OrdensTermo { set; get; }

        [DataMember]
        public List<OperacoesInfo> OrdensExecutadas { set; get; }

        [DataMember]
        public List<PosicaoBmfInfo> OrdensBMF { set; get; }

        [DataMember]
        public DateTime DtAtualizacao { set; get; }

        [DataMember]
        public decimal PercentualPrejuiso { set; get; }

        [DataMember]
        public List<ClienteRiscoResumo> Operacoes { set; get; }

        [DataMember]
        public EnumSemaforo Semaforo { set; get; }

        [DataMember]
        public EnumProporcaoPrejuiso ProporcaoLucroPrejuiso { set; get; }

    }
}
