using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class TransporteDetalhesUSPersonPJ
    {
        public bool Flag_USPersonNacional { get; set; }
        public string USPersonNacional_Nome { get; set; }
        public string USPersonNacional_Nacionalidades { get; set; }
        
        public bool Flag_USPersonResidente { get; set; }
        public string USPersonResidente_Nome { get; set; }

        public bool Flag_USPersonGreen { get; set; }
        public bool Flag_USPersonPresenca { get; set; }
        public bool Flag_USPersonNascido { get; set; }

        public string USPersonRenuncia_Motivo { get; set; }
        public string USPersonRenuncia_Documento { get; set; }
    }
}
