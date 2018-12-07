using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Site.Www
{
    public class TransporteCadastroPasso2
    {
        public string ClienteNome { get; set; }

        public string ClienteCPF { get; set; }

        public string ClienteEmailPessoal { get; set; }

        public List<ClienteTelefoneInfo> ListaDadosTelefonicos { get; set; }

        public TransporteCadastroPasso2()
        {
            this.ListaDadosTelefonicos = new List<ClienteTelefoneInfo>();
            this.ClienteNome = string.Empty;
            this.ClienteCPF = string.Empty;
            this.ClienteEmailPessoal = string.Empty;
        }
    }
}