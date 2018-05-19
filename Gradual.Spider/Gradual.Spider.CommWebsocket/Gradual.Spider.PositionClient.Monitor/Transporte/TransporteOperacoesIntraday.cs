using Gradual.Spider.SupervisorRisco.Lib.Dados;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Gradual.Spider.PositionClient.Monitor.Transporte
{
    /// <summary>
    /// Classe responsável pela camada de transporte de operações intraday
    /// </summary>
    public class TransporteOperacoesIntraday
    {
        private CultureInfo gCultura = new CultureInfo("en-US");

        /// <summary>
        /// Código de cliente
        /// </summary>
        public int CodigoCliente = 0;

        /// <summary>
        /// Código de instrumento
        /// </summary>
        public string CodigoInstrumento = string.Empty;

        /// <summary>
        /// Tipo de Mercado
        /// </summary>
        public string Mercado = string.Empty;

        /// <summary>
        /// Vencimento do papel
        /// </summary>
        public DateTime Vencimento = DateTime.MinValue;

        /// <summary>
        /// Quantidade de abertura do papel do cliente
        /// </summary>
        public int QuantAbertura = 0;

        /// <summary>
        /// Quantidade total [quantAbertura + Net Total Operado]
        /// </summary>
        public decimal QuantTotal = 0;

        /// <summary>
        /// Lucro prejuízo calculado do cliente
        /// </summary>
        public decimal PL = 0;

        /// <summary>
        /// Quantidade executada de compra do papel do cliente
        /// </summary>
        public decimal QuantExecutadaCompra = 0;

        /// <summary>
        /// Quantidade executada de venda do papel do cliente
        /// </summary>
        public decimal QuantExecutadaVenda = 0;
        public decimal QuantExecutadaNet = 0;
        public decimal QuantAbertaCompra = 0;
        public decimal QuantAbertaVenda = 0;
        public decimal QuantAbertaNet = 0;
        public decimal PrecoMedioCompra = 0;
        public decimal PrecoMedioVenda = 0;
        public decimal VolumeCompra = 0;
        public decimal VolumeVenda = 0;
        public decimal VolumeTotal = 0;

        public List<TransporteOperacoesIntraday> ListaTransporte = new List<TransporteOperacoesIntraday>();

        public TransporteOperacoesIntraday() { }

        public TransporteOperacoesIntraday(List<PosClientSymbolInfo> pLista)
        {
            TransporteOperacoesIntraday lTrans = new TransporteOperacoesIntraday();

            foreach (PosClientSymbolInfo pos in pLista)
            {
                lTrans = new TransporteOperacoesIntraday();

                lTrans.CodigoCliente = pos.Account;
                lTrans.CodigoInstrumento = pos.Ativo;
                lTrans.Mercado = pos.TipoMercado;
                lTrans.Vencimento = pos.DtVencimento;
                lTrans.QuantAbertura = int.Parse( pos.QtdAbertura.ToString());
                lTrans.QuantTotal = int.Parse((pos.QtdAbertura + pos.NetExec).ToString());
                lTrans.PL = pos.LucroPrej;
                lTrans.QuantExecutadaCompra = pos.QtdExecC;
                lTrans.QuantExecutadaVenda = pos.QtdExecV;
                lTrans.QuantExecutadaNet = pos.NetExec;
                lTrans.QuantAbertaCompra = pos.QtdAbC;
                lTrans.QuantAbertaVenda = pos.QtdAbV;
                lTrans.QuantAbertaNet = pos.NetAb;
                lTrans.PrecoMedioCompra = pos.PcMedC;
                lTrans.PrecoMedioVenda = pos.PcMedV;
                lTrans.VolumeCompra = pos.VolCompra; //(pos.QtdExecC).ToString();
                lTrans.VolumeVenda = pos.VolVenda; //pos.QtdExecV.ToString();
                lTrans.VolumeTotal = pos.VolTotal; //(pos.QtdExecC + pos.QtdExecV).ToString();

                ListaTransporte.Add(lTrans);
            }
        }
    }
}
