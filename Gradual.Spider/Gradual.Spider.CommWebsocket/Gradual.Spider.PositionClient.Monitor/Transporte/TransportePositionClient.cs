using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.PositionClient.Monitor.Dados;

namespace Gradual.Spider.PositionClient.Monitor.Transporte
{
    /// <summary>
    /// Classe de transporte para a info de position client
    /// Classe usada para passar para aplicação requisitante
    /// </summary>
    public class TransportePositionClient
    {
        /// <summary>
        /// Conta principal do cliente
        /// </summary>
        public string Account { get; set; }
        
        /// <summary>
        /// Ativo de custódia
        /// </summary>
        public string Ativo { get; set; }
        
        /// <summary>
        /// Variação da cotação do ativo
        /// </summary>
        public string Variacao { get; set; }
        
        /// <summary>
        /// ULtimo Preço de cotação do ativo
        /// </summary>
        public string UltPreco { get; set; }
        
        /// <summary>
        /// Preço de fechamento do ativo
        /// </summary>
        public string PrecoFechamento { get; set; }
        
        /// <summary>
        /// Quantidade de abertura
        /// </summary>
        public string QtdAbertura { get; set; }
        
        /// <summary>
        /// Quantidade executada de compra do ativo
        /// </summary>
        public string QtdExecC { get; set; }
        
        /// <summary>
        /// Quantidade executada de venda do ativo
        /// </summary>
        public string QtdExecV { get; set; }
        
        /// <summary>
        /// Net de quantidade executada do ativo
        /// </summary>
        public string NetExec { get; set; }
        
        /// <summary>
        /// Quantidade de Abertura de compra do ativo
        /// </summary>
        public string QtdAbC { get; set; }
        
        /// <summary>
        /// Quantidade deabertura de venda do ativo
        /// </summary>
        public decimal QtdAbV { get; set; }
        
        /// <summary>
        /// Net da quantidade de abertura do ativo
        /// </summary>
        public string NetAb { get; set; }
        
        /// <summary>
        /// Preço médio de compra do ativo
        /// </summary>
        public string PcMedC { get; set; }
        
        /// <summary>
        /// Preço médio de venda do ativo
        /// </summary>
        public string PcMedV { get; set; }
        
        /// <summary>
        /// Net de financeiro do ativo
        /// </summary>
        public string FinancNet { get; set; }
        
        /// <summary>
        /// Lucro Prejuízo Intraday
        /// </summary>
        public string LucroPrej { get; set; }
        
        /// <summary>
        /// Data da posição 
        /// </summary>
        public string DtPosicao { get; set; }
        
        /// <summary>
        /// Data de movimento da posição
        /// </summary>
        public string DtMovimento { get; set; }

        /// <summary>
        /// Quantidade de D1 do ativo
        /// </summary>
        public string QtdD1 { get; set; }

        /// <summary>
        /// Quantidade de D2 do ativo
        /// </summary>
        public string QtdD2 { get; set; }

        /// <summary>
        /// Quantidade de D3 do ativo
        /// </summary>
        public string QtdD3 { get; set; }

        /// <summary>
        /// Segmento de mercado de ativo
        /// </summary>
        public string SegmentoMercado { get; set; }



    }
}
