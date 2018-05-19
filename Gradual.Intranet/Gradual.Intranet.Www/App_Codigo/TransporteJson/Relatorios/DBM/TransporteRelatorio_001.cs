using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM
{
    public class TransporteRelatorio_001_BreackDownAssessor
    {
        public string Tipo { get; set; }

        public string Nome { get; set; }

        public string CodigoAssessor { get; set; }

        public string Volume { get; set; }

        public string Corretagem { get; set; }

        public string Custodia { get; set; }

        public List<TransporteRelatorio_001_BreackDownAssessor> TraduzirLista(List<ResumoAssessorInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_001_BreackDownAssessor>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(rai =>
                {
                    lRetorno.Add(new TransporteRelatorio_001_BreackDownAssessor()
                    {
                        CodigoAssessor = rai.CodigoAssessor,
                        Corretagem = rai.Corretagem.ToString("N2"),
                        Custodia = rai.Custodia.ToString("N2"),
                        Nome = rai.NomeAssessor,
                        Tipo = rai.Tipo.DBToString().ToUpper(),
                        Volume = rai.Volume.ToString("N2"),
                    });
                });

            return lRetorno;
        }


        public List<TransporteRelatorio_001_BreackDownAssessor> TraduzirLista(List<DBMClienteInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_001_BreackDownAssessor>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(rai =>
                {
                    lRetorno.Add(new TransporteRelatorio_001_BreackDownAssessor()
                    {
                        Corretagem = rai.Corretagem.ToString("N2"),
                        Custodia = rai.Custodia.ToString("N2"),
                        Nome = rai.NomeAssessor,
                        Volume = rai.Volume.ToString("N2"),
                    });
                });

            return lRetorno;
        }

    }
}