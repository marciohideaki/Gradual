using Gradual.Intranet.Contratos.Dados;
using System.Collections.Generic;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRelatorioClubes : TransporteRepresentantesBase, ITransporteJSON
    {
        #region | Propriedades

        public string TipoDeItem
        {
            get { return "Clubes"; }
        }

        public string IdCliente { get; set; }

        public string NomeClube { get; set; }

        public string Cota { get; set; }

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

        public TransporteRelatorioClubes() { }

        public TransporteRelatorioClubes(ClienteClubesInfo pParametro)
        {
            if (null != pParametro)
            {
                this.IdCliente          = pParametro.IdCliente.DBToString();
                this.NomeClube          = pParametro.NomeClube;
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

        #endregion

        #region | Métodos

        public List<TransporteRelatorioClubes> TraduzirListaParaTransporteRelatorioClubes(List<ClienteClubesInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorioClubes>();

            if (null != pParametro)
            {
                pParametro.ForEach(delegate(ClienteClubesInfo cci)
                {
                    lRetorno.Add(new TransporteRelatorioClubes(cci));
                });
            }

            return lRetorno;
        }

        public ClienteClubesInfo TraduzirParaClienteClubesInfo()
        {
            return new ClienteClubesInfo()
            {
                Cota            = this.Cota.DBToDecimal(),
                DataAtualizacao = this.DataAtualizacao.DBToDateTime(),
                IdCliente       = this.IdCliente.DBToInt32(),
                IOF             = this.IOF.DBToDecimal(),
                IR              = this.IR.DBToDecimal(),
                NomeClube       = this.NomeClube,
                Quantidade      = this.Quantidade.DBToInt32(),
                ValorBruto      = this.ValorBruto.DBToDecimal(),
                ValorLiquido    = this.ValorBruto.DBToDecimal(),
            };
        }

        #endregion
    }
}