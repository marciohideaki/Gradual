using System;

namespace Gradual.Migracao.PendenciasCadastrais.Entidades
{
    public class ClientePendenciaOracleInfo
    {
        public string DsCpfCnpj { get; set; }

        public bool PendenciaDocumento  { get; set; }

        public bool PendenciaCPF { get; set; }

        public bool PendenciaCertidaoCasamento { get; set; }

        public bool PendenciaComprovanteEndereco { get; set; }

        public bool PendenciaProcuracao { get; set; }

        public bool PendenciaComprovanteRenda { get; set; }

        public bool PendenciaContrato { get; set; }

        public bool PendenciaSerasa { get; set; }

        public DateTime PendenciaDataCadastro { get; set; }

        public DateTime PendenciaDataSolucao { get; set; }

        public string PendenciaDescricao { get; set; }
    }
}
