using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteEmpresaColigada : ITransporteJSON
    {
        public string RazaoSocial { get; set; }

        public string CNPJ { get; set; }

        #region ITransporteJSON Members
        public int? Id
        {
            get;
            set;
        }

        public int ParentId
        {
            get;
            set;
        }

        public string TipoDeItem
        {
            get { return "EmpresaColigada"; }
        }
        #endregion

        public TransporteEmpresaColigada() { }

        public TransporteEmpresaColigada(ClienteControladoraInfo pClienteControladoraInfo)
        {
            this.Id = pClienteControladoraInfo.IdClienteControladora;
            this.ParentId = pClienteControladoraInfo.IdCliente;
            this.RazaoSocial = pClienteControladoraInfo.DsNomeRazaoSocial;
            this.CNPJ = pClienteControladoraInfo.DsCpfCnpj;
        }

        public ClienteControladoraInfo ToClienteControladoraInfo()
        {
            return new ClienteControladoraInfo()
            {
                DsCpfCnpj = this.CNPJ.Trim().Replace("/", "").Replace("-", "").Replace(".", ""),
                DsNomeRazaoSocial = this.RazaoSocial,
                IdCliente = this.ParentId,
                IdClienteControladora = this.Id
            };
        }
    }
}