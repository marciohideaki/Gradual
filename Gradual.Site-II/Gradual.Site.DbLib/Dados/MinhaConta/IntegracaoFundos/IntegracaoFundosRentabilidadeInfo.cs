using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosRentabilidadeInfo
    {
        public decimal Dia              { get ; set; }
        
        public decimal Semana           { get ; set; }
        
        public decimal Mes              { get ; set; }
        
        public decimal Trimestre        { get ; set; }
        
        public decimal Semestre         { get ; set; }
        
        public decimal Ano              { get ; set; }

        public decimal Ultimos12Meses   { get ; set; }
        
        public decimal Ultimos18Meses   { get ; set; }
        
        public decimal Historica        { get ; set; }
        
        public decimal Ultimos24Meses   { get ; set; }

        public DateTime Data            { get; set; }

        public IntegracaoFundosRentabilidadeInfo()
        {

        }

        ~IntegracaoFundosRentabilidadeInfo()
        {

        }
    }
}
