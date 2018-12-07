using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosSimulacaoInfo
    {
        public IntegracaoFundosInfo Produto { get; set; }

        public decimal Valor            { get; set; }
        public decimal Variacao         { get; set; }
        public decimal VariacaoAno      { get; set; }
        public DateTime Data            { get; set; }

        public string Ativo             { get; set; }
        public decimal Retorno          { get; set; }
        public decimal Volume           { get; set; }
        public decimal Sharpe           { get; set; }
        public decimal Patrimonio       { get; set; }
        public decimal CDI              { get; set; }
        public decimal Resgate          { get; set; }
        public decimal AplicacaoMinima  { get; set; }
        public decimal Inicio           { get; set; }
        public decimal Ultimo12Meses    { get; set; }
        public decimal AcumuladoAno     { get; set; }
        public decimal MesAnterior      { get; set; }

        public IntegracaoFundosSimulacaoInfo()
        {
            this.Produto = new IntegracaoFundosInfo();
        }
    }
}
