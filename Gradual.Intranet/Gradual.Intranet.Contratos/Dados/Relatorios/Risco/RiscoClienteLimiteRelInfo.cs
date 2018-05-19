using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Risco
{
    public class RiscoClienteLimiteRelInfo : ICodigoEntidade
    {
        #region | Propriedades de consulta

        public OpcoesBuscarPor ConsultaClienteTipo { get; set; }

        public string ConsultaClienteParametro { get; set; } 

        public int? ConsultaIdParametro { get; set; }

        public DateTime ConsultaInicio { get; set; }

        public DateTime ConsultaFim { get; set; }

        public string ConsultaHistorico { get; set; }

        #endregion

        #region | Propriedades de dados

        public int? IdClienteParametro { get; set; }

        public string DsNome { get; set; }

        public string DsCpfCnpj { get; set; }

        public string DsParametro { get; set; }

        public decimal VlAlocado { get; set; }

        public decimal VlLimite { get; set; }

        public DateTime? DtValidade { get; set; }

        #endregion

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
