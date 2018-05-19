using Gradual.Spider.SupervisorRisco.Lib.Dados;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Gradual.Spider.PostTradingClientEngine.App_Codigo.TransporteJSon
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
        public string CodigoCliente         = string.Empty;

        /// <summary>
        /// Código de instrumento
        /// </summary>
        public string CodigoInstrumento     = string.Empty;

        /// <summary>
        /// Tipo de Mercado
        /// </summary>
        public string Mercado               = string.Empty;

        /// <summary>
        /// Vencimento do papel
        /// </summary>
        public string Vencimento            = string.Empty;

        /// <summary>
        /// Quantidade de abertura do papel do cliente
        /// </summary>
        public string QuantAbertura         = string.Empty;

        /// <summary>
        /// Quantidade total [quantAbertura + Net Total Operado]
        /// </summary>
        public string QuantTotal            = string.Empty;

        /// <summary>
        /// Lucro prejuízo calculado do cliente
        /// </summary>
        public string PL                    = string.Empty;

        /// <summary>
        /// Quantidade executada de compra do papel do cliente
        /// </summary>
        public string QuantExecutadaCompra  = string.Empty;

        /// <summary>
        /// Quantidade executada de venda do papel do cliente
        /// </summary>
        public string QuantExecutadaVenda   = string.Empty;
        public string QuantExecutadaNet     = string.Empty;
        public string QuantAbertaCompra     = string.Empty;
        public string QuantAbertaVenda      = string.Empty;
        public string QuantAbertaNet        = string.Empty;
        public string PrecoMedioCompra      = string.Empty;
        public string PrecoMedioVenda       = string.Empty;
        public string VolumeCompra          = string.Empty;
        public string VolumeVenda           = string.Empty;
        public string VolumeTotal           = string.Empty;
    
        public List<TransporteOperacoesIntraday> ListaTransporte = new List<TransporteOperacoesIntraday>();

        public TransporteOperacoesIntraday() { }


        public TransporteOperacoesIntraday(List<PosClientSymbolInfo> pLista)
        {
            TransporteOperacoesIntraday lTrans = new TransporteOperacoesIntraday();

            foreach (PosClientSymbolInfo pos in pLista)
            {
                lTrans = new TransporteOperacoesIntraday();

                lTrans.CodigoCliente         =  pos.Account.ToString();
                lTrans.CodigoInstrumento     =  pos.Ativo;
                lTrans.Mercado               =  pos.TipoMercado;
                lTrans.Vencimento            =  pos.DtVencimento.ToString("dd/MM/yyyy");
                lTrans.QuantAbertura         =  pos.QtdAbertura.ToString();
                lTrans.QuantTotal            =  (pos.QtdAbertura + pos.NetExec).ToString();
                lTrans.PL                    =  pos.LucroPrej.ToString("n",gCultura);
                lTrans.QuantExecutadaCompra  =  pos.QtdExecC.ToString();
                lTrans.QuantExecutadaVenda   =  pos.QtdExecV.ToString();
                lTrans.QuantExecutadaNet     =  pos.NetExec.ToString();
                lTrans.QuantAbertaCompra     =  pos.QtdAbC.ToString();
                lTrans.QuantAbertaVenda      =  pos.QtdAbV.ToString();
                lTrans.QuantAbertaNet        =  pos.NetAb.ToString();
                lTrans.PrecoMedioCompra      =  pos.PcMedC.ToString("n", gCultura);
                lTrans.PrecoMedioVenda       =  pos.PcMedV.ToString("n", gCultura);
                lTrans.VolumeCompra          =  pos.VolCompra.ToString("n", gCultura); //(pos.QtdExecC).ToString();
                lTrans.VolumeVenda           =  pos.VolVenda.ToString("n", gCultura); //pos.QtdExecV.ToString();
                lTrans.VolumeTotal           =  pos.VolTotal.ToString("n", gCultura); //(pos.QtdExecC + pos.QtdExecV).ToString();

                ListaTransporte.Add(lTrans);
            }
        }
    }
}