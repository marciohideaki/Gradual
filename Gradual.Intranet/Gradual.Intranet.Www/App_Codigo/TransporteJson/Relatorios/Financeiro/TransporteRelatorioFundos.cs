using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRelatorioFundos : TransporteRepresentantesBase, ITransporteJSON
    {
        #region | Propriedades

        public string TipoDeItem
        {
            get { return "Clubes"; }
        }

        public string IdCliente { get; set; }

        public string NomeFundo { get; set; }

        public string Cota { get; set; }

        public string CodigoAnbima { get; set; }

        public string Quantidade { get; set; }

        public string ValorBruto { get; set; }

        public string IR { get; set; }

        public string IOF { get; set; }

        public string ValorLiquido { get; set; }

        public string DataInicioPesquisa { get; set; }

        public string DataFimPesquisa { get; set; }

        public string DataAtualizacao { get; set; }

        public string TipoMercado { get; set; }
        #endregion

        #region | Construtores

        public TransporteRelatorioFundos() { }

        public TransporteRelatorioFundos(ClienteFundosInfo pParametro)
        {
            if (null != pParametro)
            {
                this.IdCliente          = pParametro.IdCliente.DBToString();
                this.NomeFundo          = pParametro.NomeFundo;
                this.Cota               = pParametro.Cota.Value.ToString("N2");
                this.Quantidade         = pParametro.Quantidade.Value.ToString("N0");
                this.ValorBruto         = pParametro.ValorBruto.Value.ToString("N2");
                this.IR                 = pParametro.IR.Value.ToString("N2");
                this.IOF                = pParametro.IOF.Value.ToString("N2");
                this.ValorLiquido       = pParametro.ValorLiquido.Value.ToString("N2");
                this.DataInicioPesquisa = pParametro.DataInicioPesquisa.HasValue ?  pParametro.DataInicioPesquisa.Value.ToString("dd/MM/yyyy") : "";
                this.TipoMercado        = "FUN";
            }
        }

        public TransporteRelatorioFundos(ClienteClubesInfo pParametro)
        {
            if (null != pParametro)
            {
                this.IdCliente          = pParametro.IdCliente.DBToString();
                this.NomeFundo          = pParametro.NomeClube;
                this.Cota               = pParametro.Cota.ToString("N2");
                this.Quantidade         = pParametro.Quantidade.ToString("N0");
                this.ValorBruto         = pParametro.ValorBruto.ToString("N2");
                this.IR                 = pParametro.IR.ToString("N2");
                this.IOF                = pParametro.IOF.ToString("N2");
                this.ValorLiquido       = pParametro.ValorLiquido.ToString("N2");
                this.DataInicioPesquisa = pParametro.DataInicioPesquisa.ToString("dd/MM/yyyy");
                this.TipoMercado        = "CLU";
            }
        }

        public TransporteRelatorioFundos(Transporte_PosicaoCotista pParametro)
        {
            if (null != pParametro)
            {
                //this.IdCliente          = pParametro..IdCliente.DBToString();
                this.CodigoAnbima = pParametro.CodigoAnbima;
                this.NomeFundo          = pParametro.NomeFundo;
                this.Cota               = pParametro.ValorCota;
                this.Quantidade         = pParametro.QtdCotas;
                this.ValorBruto         = pParametro.ValorBruto;
                this.IR                 = pParametro.IR;
                this.IOF                = pParametro.IOF;
                this.ValorLiquido       = pParametro.ValorLiquido;
                //this.DataInicioPesquisa = pParametro.DataInicioPesquisa.ToString("dd/MM/yyyy");
                this.TipoMercado        = "FUN";
            }
        }

        
        #endregion

        #region | Métodos

        public List<TransporteRelatorioFundos> TraduzirListaParaTransporteRelatorioFundos(List<ClienteFundosInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorioFundos>();

            if (null != pParametro)
            {
                pParametro.ForEach(delegate(ClienteFundosInfo cci)
                {
                    lRetorno.Add(new TransporteRelatorioFundos(cci));
                });
            }

            return lRetorno;
        }

        public List<TransporteRelatorioFundos> TraduzirListaParaTransporteRelatorioFundos(List<ClienteClubesInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorioFundos>();

            if (null != pParametro)
            {
                pParametro.ForEach(delegate(ClienteClubesInfo cci)
                {
                    lRetorno.Add(new TransporteRelatorioFundos(cci));
                });
            }

            return lRetorno;
        }

        public List<TransporteRelatorioFundos> TraduzirListaParaTransporteRelatorioFundos(List<Transporte_PosicaoCotista> pParametro)
        {
            var lRetorno = new List<TransporteRelatorioFundos>();

            if (null != pParametro)
            {
                pParametro.ForEach(delegate(Transporte_PosicaoCotista cci)
                {
                    lRetorno.Add(new TransporteRelatorioFundos(cci));
                });
            }

            return lRetorno;
        }
        public ClienteFundosInfo TraduzirParaClienteFundosInfo()
        {
            return new ClienteFundosInfo()
            {
                Cota            = this.Cota.DBToDecimal(),
                DataAtualizacao = this.DataAtualizacao.DBToDateTime(),
                IdCliente       = this.IdCliente.DBToInt32(),
                IOF             = this.IOF.DBToDecimal(),
                IR              = this.IR.DBToDecimal(),
                NomeFundo       = this.NomeFundo,
                Quantidade      = this.Quantidade.DBToInt32(),
                ValorBruto      = this.ValorBruto.DBToDecimal(),
                ValorLiquido    = this.ValorBruto.DBToDecimal(),
            };
        }

        #endregion
    }
}