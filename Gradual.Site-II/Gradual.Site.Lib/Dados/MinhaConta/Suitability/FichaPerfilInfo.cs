using System;
using Gradual.Site.Lib.Mensagens;

namespace Gradual.Site.Lib.Dados.MinhaConta.Suitability
{
    [Serializable]
    [DataContract]
    public class FichaPerfilInfo
    {
        [DataMember]
        public int ConsultaIdCliente { get; set; }

        [DataMember]
        public int IdFichaPerfil { get; set; }

        [DataMember]
        public int IdCliente { get; set; }

        [DataMember]
        public string DsFaixaEtaria { get; set; }

        [DataMember]
        public string DsOcupacao { get; set; }

        [DataMember]
        public string DsConhecimentoCapitais { get; set; }

        [DataMember]
        public string TpInvestidor { get; set; }

        [DataMember]
        public string TpInvestimento { get; set; }

        [DataMember]
        public string TpInstituicao { get; set; }

        [DataMember]
        public string DsRendaFamiliar { get; set; }
    }
}
