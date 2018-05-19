using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using CelulaAtivacao;

namespace CelulaAtivacao
{
    public class CDBase
    {
        public string ConexaoCadastro = "Cadastro";

        public string ConexaoCadastroOracle = "CadastroOracle";
   
        public AcessaDados _AcessaDados = null ;

        public CDBase()
        {
            _AcessaDados = new AcessaDados();
        }
    }
}
