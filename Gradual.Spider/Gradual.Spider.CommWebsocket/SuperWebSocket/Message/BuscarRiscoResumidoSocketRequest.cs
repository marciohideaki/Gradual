using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.PositionClient.Monitor.Lib.Message
{
    /// <summary>
    /// Classe de request do socket para efetuar o filtro 
    /// da descida dos objeto no socket da aplicação conectada
    /// </summary>
    public class BuscarRiscoResumidoSocketRequest
    {
        /// <summary>
        /// Código do cliente que o usuario inserio no filtro
        /// </summary>
        public int CodigoCliente { get; set; }

        /// <summary>
        /// Opção de PL somente com lucro
        /// </summary>
        public bool OpcaoPLSomenteComLucro { get; set; }

        /// <summary>
        /// OPção de PL Somente no negativo
        /// </summary>
        public bool OpcaoPLSomentePLnegativo { get; set; }

        /// <summary>
        /// Opção de SFP(Situação financeira patrimonial) Atingido até 25 %
        /// </summary>
        public bool OpcaoSFPAtingidoAte25 { get; set; }

        /// <summary>
        /// Opção de SFP(Situação financeira patrimonial) Atingido entre 25 % e 50 %
        /// </summary>
        public bool OpcaoSFPAtingidoEntre25e50 { get; set; }

        /// <summary>
        /// Opção de SFP(Situação financeira patrimonial) Atingido entre 50 % e 75 %
        /// </summary>
        public bool OpcaoSFPAtingidoEntre50e75 { get; set; }

        /// <summary>
        /// Opção de SFP(Situação financeira patrimonial) Atingido acima de 75 %
        /// </summary>
        public bool OpcaoSFPAtingidoAcima75 { get; set; }

        /// <summary>
        /// Opção de Prejuizo Atingido até 2.000
        /// </summary>
        public bool OpcaoPrejuizoAtingidoAte2K { get; set; }

        /// <summary>
        /// Opção de Prejuizo Atingido entre 2.000 e 5.000
        /// </summary>
        public bool OpcaoPrejuizoAtingidoEntre2Ke5K { get; set; }

        /// <summary>
        /// Opção de Prejuizo Atingido entre 5.000 e 10.000
        /// </summary>
        public bool OpcaoPrejuizoAtingidoEntre5Ke10K { get; set; }

        /// <summary>
        /// Opção de Prejuizo Atingido entre 10.000 e 20.000
        /// </summary>
        public bool OpcaoPrejuizoAtingidoEntre10Ke20K { get; set; }

        /// <summary>
        /// Opção de Prejuizo Atingido entre 20.000 e 50.000
        /// </summary>
        public bool OpcaoPrejuizoAtingidoEntre20Ke50K { get; set; }

        /// <summary>
        /// Opção de Prejuizo Atingido acima de 50.000
        /// </summary>
        public bool OpcaoPrejuizoAtingidoAcima50K { get; set; }

        /// <summary>
        /// Lista de Clientes de request
        /// </summary>
        public List<int> ListaClientes { get; set; }
    }
}
