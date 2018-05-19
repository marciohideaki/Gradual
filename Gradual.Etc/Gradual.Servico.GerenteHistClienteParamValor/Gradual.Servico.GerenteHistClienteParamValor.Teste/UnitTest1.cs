using Gradual.Servico.GerenteHistClienteParamValor.Lib;
using Gradual.Servico.GerenteHistClienteParamValor.Lib.Mensagem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gradual.Servico.GerenteHistClienteParamValor.Teste
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var lRetorno = new ServicoGerenteHistoricoRiscoClienteParametroValor().IniciarProcessoGuadarHistorico(new GerenteHistoricoRiscoClienteParametroValorRequest()
            {
                TipoRequisitante = TipoRequisitante.ServicoGerenteHistorico
            });
        }
    }
}
