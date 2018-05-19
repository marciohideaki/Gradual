using Gradual.Intranet.Contratos.Dados.Risco;
using System.Collections.Generic;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteManutencaoCliente
    {
        public string Cliente { get; set; }

        public string Assessor { get; set; }

        public string NomeDoGrupoGrupo { get; set; }

        public string DataInclusao { get; set; }


        public List<TransporteManutencaoCliente> TraduzirLista(List<ParametroAlavancagemConsultaInfo> pParametros)
        {
            var lRetorno = new List<TransporteManutencaoCliente>();

            if(null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(pac =>
                {
                    lRetorno.Add(new TransporteManutencaoCliente() 
                    {
                        Cliente = pac.CdCliente.DBToString(),
                        Assessor = pac.CdAssessor.DBToString(),
                        NomeDoGrupoGrupo = pac.DsGrupo,
                        DataInclusao = pac.DtInclusao.ToString("dd/MM/yyyy")
                    });
                });

            return lRetorno;
        }
    }
}