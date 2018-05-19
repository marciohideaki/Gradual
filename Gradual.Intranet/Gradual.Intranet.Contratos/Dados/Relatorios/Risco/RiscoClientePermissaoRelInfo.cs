using Gradual.Intranet.Contratos.Dados.Enumeradores;
using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios
{
    public class RiscoClientePermissaoRelInfo : ICodigoEntidade
    {
        public Nullable<int> ConsultaIdBolsa { get; set; }

        public Nullable<int> ConsultaIdGrupo { get; set; }

        public OpcoesBuscarPor ConsultaClienteTipo { get; set; }

        public string ConsultaClienteParametro { get; set; } 

        public string NomeCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string DescricaoPermissao { get; set; }

        public string Bolsa { get; set; }

        public string DescricaoGrupo { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
