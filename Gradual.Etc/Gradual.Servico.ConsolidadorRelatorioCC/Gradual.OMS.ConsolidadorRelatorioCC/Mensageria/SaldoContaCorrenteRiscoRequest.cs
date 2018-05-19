using System.Collections.Generic;

namespace Gradual.OMS.ConsolidadorRelatorioCCLib.Mensageria
{
    public class SaldoContaCorrenteRiscoRequest
    {
        /// <summary>
        /// Id. cliente no Sinacor [Código Bovespa].
        /// </summary>
        public int IdCliente { set; get; }

        /// <summary>
        /// Parâmetro de consulta de cliente por CPF/CNPJ.
        /// </summary>
        public List<string> ConsultaClientesCpfCnpj { get; set; }

        /// <summary>
        /// Parâmetro de consulta de cliente pelo código de assessor.
        /// </summary>
        public int? ConsultaIdAssessor { get; set; }

        /// <summary>
        /// Parâmetro de consulta de saldo na posição D+0 negativo.
        /// </summary>
        public bool ConsultaPosicaoD0 { get; set; }
        
        /// <summary>
        /// Parâmetro de consulta de saldo na posição D+1 negativo.
        /// </summary>
        public bool ConsultaPosicaoD1 { get; set; }

        /// <summary>
        /// Parâmetro de consulta de saldo na posição D+2 negativo.
        /// </summary>
        public bool ConsultaPosicaoD2 { get; set; }

        /// <summary>
        /// Parâmetro de consulta de saldo na posição D+3 negativo.
        /// </summary>
        public bool ConsultaPosicaoD3 { get; set; }

        /// <summary>
        /// Parâmetro de consulta de saldo na posição de Conta Margem negativo.
        /// </summary>
        public bool ConsultaContaMargem { get; set; }

        public SaldoContaCorrenteRiscoRequest()
        {
            this.ConsultaClientesCpfCnpj = new List<string>();
        }
    }
}
