using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class AssessorSinacorInfo : ICodigoEntidade
    {
        public int CdAssessor { get; set; }

        public string DsNome { get; set; }

        public string DsNomeResumido { get; set; }

        public decimal PcAdiantamento { get; set; }

        public char InSituac { get; set; }

        public int CdEmpresa { get; set; }

        public int CdUsuario { get; set; }

        public char TpOcorrencia { get; set; }

        public int CdMunicipio { get; set; }

        public string DsEmail { get; set; }

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }
    }
}
