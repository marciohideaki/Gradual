
namespace Gradual.Cadastro.Entidades
{
    public class EAssessorFilial
    {
        public int? ID_AssessorFilial { get; set; }
        public int? ID_Assessor { get; set; }
        public int? ID_Filial { get; set; }
        public int? ID_AssessorSinacor { get; set; }
        public string NomeAssessor { get; set; }
        public string NomeFilial { get; set; }
        public string NomeFilial_NomeAssessor { get; set; }
        public string NomeFilial_CodigoSinacorAssessor_NomeAssessor { get; set; }
    }
}
