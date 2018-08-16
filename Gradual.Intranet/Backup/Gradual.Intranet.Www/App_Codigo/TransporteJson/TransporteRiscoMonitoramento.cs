using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Risco.Regra.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRiscoMonitoramento
    {
        #region | Atruturas

        public struct ListaExposicaoAoRisco
        {
            public string Criticidade { get; set; }

            public string Cliente { get; set; }

            public string Assessor { get; set; }

            public string Parametro { get; set; }

            public string Grupo { get; set; }

            public string ValorLimite { get; set; }

            public string ValorAlocado { get; set; }

            public string ValorDisponivel { get; set; }
        }

        #endregion

        #region | Propriedades

        public string DataHoraConsulta { get; set; }

        public List<ListaExposicaoAoRisco> ListaExposicaoRisco { get; set; }

        #endregion

        #region | Construtores

        public TransporteRiscoMonitoramento()
        {
            this.ListaExposicaoRisco = new List<ListaExposicaoAoRisco>();
        }

        #endregion

        #region | Métodos

        public TransporteRiscoMonitoramento TraduzirLista(List<MonitoramentoRiscoInfo> pParametros)
        {
            var lRetorno = new TransporteRiscoMonitoramento();

            if (null != pParametros && pParametros.Count > 0)
            {
                pParametros.ForEach(trm =>
                {
                    lRetorno.ListaExposicaoRisco.Add(new ListaExposicaoAoRisco()
                    {
                        Assessor = trm.CdAssessor.DBToString(),
                        Criticidade = this.DefinirCriticidade(trm),
                        Cliente = string.Concat(trm.IdCliente.ToCodigoClienteFormatado(), " ", trm.NmCliente.ToStringFormatoNome()),
                        Grupo = trm.DsGrupo.ToStringFormatoNome(),
                        Parametro = trm.DsParametro.ToStringFormatoNome(),
                        ValorAlocado = trm.VlAlocado.ToString("N2"),
                        ValorDisponivel = trm.VlDisponivel.ToString("N2"),
                        ValorLimite = trm.VlLimite.ToString("N2"),
                    });
                });
            }

            return lRetorno;
        }

        private string DefinirCriticidade(MonitoramentoRiscoInfo pParametro)
        {
            if (pParametro.VlLimite > 0M)
            {
                var lPercentualDeUsoDoLimite = 100M * pParametro.VlAlocado / pParametro.VlLimite;

                if (lPercentualDeUsoDoLimite > 75M && lPercentualDeUsoDoLimite <= 100M)
                    return "SemaforoVermelho";

                if (lPercentualDeUsoDoLimite > 50M && lPercentualDeUsoDoLimite <= 75M)
                    return "SemaforoAmarelo";
            }
            return "SemaforoVerde";
        }

        #endregion
    }
}