using Gradual.Generico.Dados;

namespace Gradual.MinhaConta.Dados
{
    public class DBase
    {
        public AcessaDados AcessaDados = null;

        public string ConexaoTrade
        {
            get { return "Trade"; }
        }

        public string ConexaoOMS 
        {
            get { return "OMS"; }
        }

        public DBase()
        {
            AcessaDados = new AcessaDados();
        }
    }
}
