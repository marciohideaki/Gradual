using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;


namespace Gradual.Intranet.Contratos.Dados.Relatorios.Risco
{
    public class RiscoClienteLimiteMovimentoRelInfo : ICodigoEntidade
    {
        #region | Propriedades de consulta

        public OpcoesBuscarPor ConsultaClienteTipo { get; set; }

        public string ConsultaClienteParametro { get; set; } 

        #endregion

        #region | Propriedades de dados

        public int IdClienteParametroValor { get; set; }

        public int IdClienteParametro { get; set; }

        public decimal ValorMovimento { get; set; }

        public decimal ValorAlocado { get; set; }

        public decimal ValorDisponivel { get; set; }

        public DateTime DataMovimento { get; set; }

        public string Historico { get; set; }

        #endregion

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
