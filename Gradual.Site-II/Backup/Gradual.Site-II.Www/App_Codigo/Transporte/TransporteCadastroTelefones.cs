using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Site.Www
{
    public class TransporteCadastroTelefones
    {
        public struct DadosTelefonicos
        {
            public string IdTelefone { get; set; }

            public string DsDdd { get; set; }

            public string DsTipoTelefone { get; set; }

            public string IdTipoTelefone { get; set; }

            public string DsNumero { get; set; }

            public string DsRamal { get; set; }

            public string StPrincipal { get; set; }
        }

        public List<DadosTelefonicos> TraduzirLista(List<ClienteTelefoneInfo> pParametro)
        {
            var lRetorno = new List<DadosTelefonicos>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(cti =>
                {
                    lRetorno.Add(new DadosTelefonicos()
                    {
                        IdTelefone = cti.IdTelefone.DBToString(),
                        DsTipoTelefone = this.DefinirTipoTelefone(cti.IdTipoTelefone),
                        IdTipoTelefone = cti.IdTipoTelefone.DBToString(),
                        DsDdd = cti.DsDdd.DBToString(),
                        DsNumero = cti.DsNumero.DBToString(),
                        DsRamal = cti.DsRamal.DBToString(),
                        StPrincipal = cti.StPrincipal ? "Sim" : "Não"
                    });
                });

            return lRetorno;
        }

        private string DefinirTipoTelefone(int pIdTipoTelefone)
        {
            switch (pIdTipoTelefone)
            {
                case 1: return "Residencial";

                case 2: return "Comercial";

                case 3: return "Celular";

                default: return "Fax";
            }
        }
    }
}