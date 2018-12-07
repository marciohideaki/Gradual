using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Dados.MinhaConta
{
    [Serializable]
    [DataContract]
    public class SinacorEnderecoInfo
    {
        public string Rua { get; set; }//a.nm_logradouro

        public string Numero { get; set; }//a.nr_predio, 

        public string Complemento { get; set; }//a.nm_comp_ende, 

        public string Bairro { get; set; }//a.nm_bairro,

        public string Cidade { get; set; }//a.nm_cidade,

        public string UF { get; set; }//a.sg_estado, 

        public string Cep { get; set; }//substr( to_char( a.cd_cep + 100000 ), 2, 5 )||'-'||substr( to_char( a.cd_cep_ext + 1000 ), 2, 3 ) cep 

        public Int64 CPF { get; set; }

        public DateTime DataNascimento { get; set; }

        public int CondicaoDependente { get; set; }
    }
}
