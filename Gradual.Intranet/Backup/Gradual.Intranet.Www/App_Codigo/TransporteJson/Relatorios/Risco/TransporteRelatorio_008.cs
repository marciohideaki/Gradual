using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_008 : ICodigoEntidade
    {
        #region | Propriedade

        public string CodigoBovespa { get; set; }

        public string NomeCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string Parametro { get; set; }

        public string Grupo { get; set; }

        #endregion

        #region | Métodos

        public List<TransporteRelatorio_008> TraduzirLista(List<RiscoClienteBloqueioGrupoRelInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_008>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(delegate(RiscoClienteBloqueioGrupoRelInfo cbg)
                {
                    lRetorno.Add(new TransporteRelatorio_008()
                    {
                        CodigoBovespa = cbg.CdCodigo,
                        CpfCnpj = cbg.DsCpfCnpj.ToCpfCnpjString(),
                        Grupo = cbg.DsGrupo,
                        NomeCliente = cbg.DsNome.ToStringFormatoNome(),
                        Parametro = cbg.DsParametro,
                    });
                });

            return lRetorno;
        }

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}