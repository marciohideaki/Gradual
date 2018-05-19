using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Risco
{
    public class RiscoClienteParametroRelInfo : ICodigoEntidade
    {
        public int? ConsultaIdBolsa { get; set; }

        public int? ConsultaIdGrupo { get; set; }

        public int? ConsultaIdParametro { get; set; }

        public bool? ConsultaEstado { get; set; }

        public OpcoesBuscarPor ConsultaClienteTipo { get; set; }

        public string NomeCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string DsParametro { get; set; }

        public string DsBolsa { get; set; }

        public string DsGrupo { get; set; }

        public string ConsultaClienteParametro { get; set; } 

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
