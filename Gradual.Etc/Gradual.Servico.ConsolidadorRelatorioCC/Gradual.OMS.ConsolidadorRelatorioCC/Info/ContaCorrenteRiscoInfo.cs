using System;
using System.Runtime.Serialization;

namespace Gradual.OMS.ConsolidadorRelatorioCCLib
{
    [Serializable]
    [DataContract]
    public class ContaCorrenteRiscoInfo
    {
        #region | Propriedades

        /// <summary>
        /// Id. cliente no Sinacor [Código Bovespa].
        /// </summary>
        [DataMember]
        public Nullable<int> IdClienteSinacor { get; set; }

        /// <summary>
        /// CNPJ do cliente.
        /// </summary>
        [DataMember]
        public string DsCpfCnpj { get; set; }
        
        /// <summary>
        /// Nome do cliente.
        /// </summary>
        [DataMember]
        public string NomeCliente { get; set; }

        /// <summary>
        /// Id. do assessor relativo ao cliente.
        /// </summary>
        [DataMember]
        public Nullable<int> IdAssessor { get; set; }
        
        /// <summary>
        /// Saldo do cliente em D0
        /// </summary>
        [DataMember]
        public decimal SaldoD0 { set; get; }

        /// <summary>
        /// Saldo do cliente D1
        /// </summary>
        [DataMember]
        public decimal SaldoD1 { set; get; }

        /// <summary>
        /// Saldo do cliente em D2
        /// </summary>
        [DataMember]
        public decimal SaldoD2 { set; get; }

        /// <summary>
        /// Saldo do cliente em D3
        /// </summary>
        [DataMember]
        public decimal SaldoD3 { set; get; }

        /// <summary>
        /// Saldo em conta margem do cliente
        /// </summary>
        [DataMember]
        public Nullable<decimal> SaldoContaMargem { set; get; }

        /// <summary>
        /// Valor total do limite liberado para o cliente.
        /// </summary>
        [DataMember]
        public Nullable<decimal> LimiteTotal { get; set; }

        /// <summary>
        /// Valor utilizado do limite pelo cliente.
        /// </summary>
        [DataMember]
        public Nullable<decimal> LimiteUtilizado { get; set; }

        /// <summary>
        /// Valor do limite utilizado pelo cliente no momento. (Limite total menos saldo devedor)
        /// </summary>
        [DataMember]
        public Nullable<decimal> SaldoCCLimiteDisponivel { get; set; }

        #endregion
    }
}
