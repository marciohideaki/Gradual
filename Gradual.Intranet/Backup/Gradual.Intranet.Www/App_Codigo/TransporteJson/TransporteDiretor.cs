using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteDiretor : TransporteRepresentantesBase, ITransporteJSON
    {
        public TransporteDiretor(){}

        public string TipoDeItem { get { return "Diretor"; } }

        public TransporteDiretor(ClienteDiretorInfo pDiretor, bool pExclusao)
        {
            this.Id             = pDiretor.IdClienteDiretor;
            this.Identidade     = pDiretor.DsIdentidade;
            this.ParentId       = pDiretor.IdCliente;
            this.UfOrgaoEmissor = pDiretor.CdUfOrgaoEmissor;
            this.OrgaoEmissor   = pDiretor.CdOrgaoEmissor;
            this.CPF            = pDiretor.NrCpfCnpj;
            this.Nome           = pDiretor.DsNome;
            this.Exclusao       = pExclusao;
        }

        public ClienteDiretorInfo ToClienteDiretorInfo()
        {
            ClienteDiretorInfo lDiretor = new ClienteDiretorInfo()
                {
                    CdOrgaoEmissor = this.OrgaoEmissor,
                    CdUfOrgaoEmissor = this.UfOrgaoEmissor,
                    DsIdentidade = this.Identidade,
                    DsNome = this.Nome,
                    IdCliente = this.ParentId,
                    IdClienteDiretor = this.Id,
                    NrCpfCnpj = this.CPF
                };
            return lDiretor;
        }
    }
}
