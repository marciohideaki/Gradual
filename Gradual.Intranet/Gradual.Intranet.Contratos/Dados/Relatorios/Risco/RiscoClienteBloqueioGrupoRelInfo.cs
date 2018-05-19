namespace Gradual.Intranet.Contratos.Dados.Relatorios.Risco
{
    public class RiscoClienteBloqueioGrupoRelInfo : Gradual.OMS.Library.ICodigoEntidade
    {
        public string CdCodigo { get; set; }

        public string DsNome { get; set; }

        public string DsCpfCnpj { get; set; }

        public int? CdAssessor { get; set; }

        public int? IdGrupo { get; set; }

        public int? IdParametro { get; set; }

        public string DsParametro { get; set; }

        public string DsGrupo { get; set; }

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }
    }
}
