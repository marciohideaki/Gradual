using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class ClienteEnderecoDeCustodiaInfo : ClienteEnderecoInfo
    {
        public string ConsultaCpfCnpj { get; set; }

        public int ConsultaCondicaoDeDePendente { get; set; }

        public DateTime ConsultaDataDeNascimento { get; set; }
    }
}
