using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info utilizada para gerencias dados referentes a Cadastro de Fundos
    /// </summary>
    public class ConsultaFundosConstituicaoInfo
    {
        public int IdFundoCadastro { get; set; }
        public string NomeFundo { get; set; }
        public string Grupo { get; set; }
        public string Etapa { get; set; }
        public string StatusEtapa { get; set; }
        public string StatusGeral { get; set; }
    }
}
