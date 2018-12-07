using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;

namespace Gradual.Site.Www.Transporte
{
    public class TransporteRentabilidadeFundo
    {
        public string NomeProduto           { get; set; }

        public int IdProduto                { get; set; }

        public decimal PatrimonioLiquido    { get; set; }

        public decimal RentabilidadeMes     { get; set; }

        public decimal RentabilidadeAno     { get; set; }

        public decimal rentabilidade12Meses { get; set; }

        public decimal ValorCota            { get; set; }

        public string DataInicioFundo       { get; set; }
        
        public TransporteRentabilidadeFundo()
        {

        }

        public TransporteRentabilidadeFundo(IntegracaoFundosRentabilidadeAcumuludaInfo pRentabilidade)
        {
            this.NomeProduto          = pRentabilidade.Produto.NomeProduto;
            
            this.IdProduto            = pRentabilidade.Produto.IdProduto;
            
            this.PatrimonioLiquido    = pRentabilidade.PatrimonioLiquido;
            
            this.RentabilidadeMes     = pRentabilidade.RentabilidadeMes;
            
            this.RentabilidadeAno     = pRentabilidade.RentabilidadeAno;
            
            this.rentabilidade12Meses = pRentabilidade.Rentabilidade12Meses;
            
            this.DataInicioFundo      = pRentabilidade.Produto.DataInicio.ToString("dd/MM/yyyy");
        }

        public List<TransporteRentabilidadeFundo> TraduzirLista(List<IntegracaoFundosRentabilidadeAcumuludaInfo> pParametros)
        {
            var lRetorno = new List<TransporteRentabilidadeFundo>();

            TransporteRentabilidadeFundo lRent = null;

            pParametros.ForEach(rent =>
            {
                lRent                      = new TransporteRentabilidadeFundo();
                lRent.NomeProduto          = rent.Produto.NomeProduto;
                lRent.IdProduto            = rent.Produto.IdProduto;
                lRent.PatrimonioLiquido    = rent.PatrimonioLiquido;
                lRent.RentabilidadeMes     = rent.RentabilidadeMes;
                lRent.RentabilidadeAno     = rent.RentabilidadeAno;
                lRent.rentabilidade12Meses = rent.Rentabilidade12Meses;
                lRent.DataInicioFundo      = rent.Produto.DataInicio.ToString("dd/MM/yyyy");

                lRetorno.Add(lRent);
            });

            return lRetorno;
        }
    }
}