using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Servico.FichaCadastral.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gradual.Servico.FichaCadastral.Teste
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            this.TesteFichaPF();
        }

        private void TesteFichaPF()
        {
            var lRetorno = new FichaCadastral_PJ().GerarFichaCadastral_PJ(new ReceberEntidadeRequest<FichaCadastralGradualInfo>()
            {
                Objeto = new FichaCadastralGradualInfo()
                {   //--> Rafael: 25022
                    //--> Bianca: 12611
                    IdCliente = 213193,
                    SitemaOrigem = SistemaOrigem.Intranet,
                }
            });

            /*
            var lRetorno = new FichaCadastral_PF().GerarFichaCadastral_PF(new ReceberEntidadeRequest<FichaCadastralGradualInfo>()
            {
                Objeto = new FichaCadastralGradualInfo()
                {   //--> Rafael: 25022
                    //--> Bianca: 12611
                    //IdCliente = 237697,
                    IdCliente = 238032,
                    SitemaOrigem = SistemaOrigem.Intranet,
                }
            });
            */

            if (null != lRetorno.Objeto)
            {

            }
        }
    }
}
