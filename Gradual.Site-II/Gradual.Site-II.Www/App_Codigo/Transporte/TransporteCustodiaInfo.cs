using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Custodia.Lib.Info;
using System.Globalization;
using Gradual.OMS.Monitor.Custodia.Lib.Info;

namespace Gradual.Site.Www
{
    public class TransporteCustodiaInfo
    {
        #region Globais

        private IFormatProvider gCultura = new CultureInfo("pt-BR");

        #endregion

        #region Propriedades

        public string CodigoCliente { get; set; }

        public string CodigoNegocio { get; set; }

        public string Empresa { get; set; }

        public string TipoMercado { get; set; }

        public string ValorDeFechamento { get; set; }

        public string Carteira { get; set; }

        public string DsCarteira { get; set; }

        public string Cotacao { get; set; }

        public string QtdAbertura { get; set; }

        public string QtdCompra { get; set; }

        public string QtdVenda { get; set; }

        public string QtdAtual { get; set; }

        public string PrecoMedio { get; set; }

        public string ValorAtualizado { get; set; }

        public string Valor { get; set; }
        
        public string Variacao { get; set; }

        public string Resultado { get; set; }

        public string TipoGrupo { get; set; }

        public string ValorPosicao { get; set; }

        public List<TaxaCustodiaClienteInfo> Taxas { get; set; }

        #endregion

        #region Construtores

        public TransporteCustodiaInfo()
        {
            this.CodigoCliente      = "n/d";
            this.CodigoNegocio      = "n/d";
            this.Empresa            = "n/d";
            this.TipoMercado        = "n/d";
            this.ValorDeFechamento  = "n/d";
            this.Carteira           = "n/d";
            this.DsCarteira         = "n/d";
            this.Cotacao            = "n/d";
            this.QtdAbertura        = "n/d";
            this.QtdCompra          = "n/d";
            this.QtdVenda           = "n/d";
            this.QtdAtual           = "n/d";
            this.PrecoMedio         = "n/d";
            this.ValorAtualizado    = "n/d";
            this.Valor              = "n/d";
            this.Variacao           = "n/d";
            this.Resultado          = "n/d";
            this.TipoGrupo          = "n/d";
            this.ValorPosicao       = "n/d";
        }

        public TransporteCustodiaInfo(CustodiaClienteInfo pParametro) : this()
        {
            this.TipoMercado       = pParametro.TipoMercado;
            this.CodigoNegocio     = pParametro.CodigoInstrumento;
            this.QtdVenda          = pParametro.QtdeAExecVenda.ToString(gCultura);
            this.QtdAtual          = pParametro.QtdeAtual.ToString(gCultura);
            this.Carteira          = pParametro.CodigoCarteira.ToString();
            this.QtdCompra         = pParametro.QtdeAExecCompra.ToString(gCultura);
            this.QtdAbertura       = pParametro.QtdeDisponivel.ToString(gCultura);
            this.CodigoCliente     = pParametro.IdCliente.ToString();
            this.TipoGrupo         = pParametro.TipoGrupo;
            this.ValorPosicao      = pParametro.ValorPosicao.ToString(gCultura);
        }

        public TransporteCustodiaInfo(CustodiaClienteBMFInfo  pParametro) : this()
        {
            this.TipoMercado   = "BMF";
            this.CodigoNegocio = pParametro.CodigoInstrumento;
        }

        public TransporteCustodiaInfo(MonitorCustodiaInfo.CustodiaPosicao pParametro)
        {
            List<string> lstFatorCotacao1000 = new List<string>();

            lstFatorCotacao1000.Add("CEGR3");
            lstFatorCotacao1000.Add("CAFE3");
            lstFatorCotacao1000.Add("CAFE4");
            lstFatorCotacao1000.Add("CBEE3");
            lstFatorCotacao1000.Add("SGEN4");
            lstFatorCotacao1000.Add("PMET6");
            lstFatorCotacao1000.Add("EBTP3");
            lstFatorCotacao1000.Add("EBTP4");
            lstFatorCotacao1000.Add("TOYB3");
            lstFatorCotacao1000.Add("TOYB4");
            lstFatorCotacao1000.Add("FNAM11");
            lstFatorCotacao1000.Add("FNOR11");
            lstFatorCotacao1000.Add("NORD3");

            this.TipoMercado   = pParametro.TipoMercado;
            this.CodigoNegocio = pParametro.CodigoInstrumento;
            this.QtdVenda      = pParametro.QtdeAExecVenda.ToString(gCultura);
            this.QtdAtual      = pParametro.QtdeAtual.ToString(gCultura);
            this.Carteira      = pParametro.CodigoCarteira.ToString();
            this.QtdCompra     = pParametro.QtdeAExecCompra.ToString(gCultura);
            this.QtdAbertura   = pParametro.QtdeDisponivel.ToString(gCultura);
            this.CodigoCliente = pParametro.IdCliente.ToString();
            this.TipoGrupo     = pParametro.TipoGrupo;
            this.ValorPosicao  = pParametro.ValorPosicao.ToString(gCultura);

            if (lstFatorCotacao1000.Contains(pParametro.CodigoInstrumento))
            {
                this.QtdAtual = (pParametro.QtdeAtual/ 1000).ToString(gCultura);
                //this.ValorPosicao = (pParametro.ValorPosicao / 1000).ToString(gCultura);
            }
        }

        public TransporteCustodiaInfo(List<TaxaCustodiaClienteInfo> pParametro)
        {
            this.Taxas = pParametro;
        }

        #endregion

        #region Métodos

        public static List<TransporteCustodiaInfo> TraduzirCustodiaInfo(List<CustodiaClienteInfo> pParametros)
        {
            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();

            if (pParametros != null)
            {
                foreach (CustodiaClienteInfo lInfo in pParametros)
                {
                    lRetorno.Add(new TransporteCustodiaInfo(lInfo));
                }
            }

            return lRetorno;
        }

        public static List<TransporteCustodiaInfo> TraduzirCustodiaInfo(List<CustodiaClienteBMFInfo> pParametros)
        {
            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();

            if (pParametros != null)
            {
                foreach (CustodiaClienteBMFInfo lInfo in pParametros)
                {
                    lRetorno.Add(new TransporteCustodiaInfo(lInfo));
                }
            }

            return lRetorno;
        }

        public static List<TransporteCustodiaInfo> TraduzirCustodiaInfo(List<MonitorCustodiaInfo.CustodiaPosicao> pParametros)
        {
            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();

            if (pParametros != null)
            {
                foreach (MonitorCustodiaInfo.CustodiaPosicao lInfo in pParametros)
                {
                    lRetorno.Add(new TransporteCustodiaInfo(lInfo));
                }
            }

            return lRetorno;
        }

        #endregion
    }
}