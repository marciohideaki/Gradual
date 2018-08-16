using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_007 : ICodigoEntidade
    {
        #region | Propriedades

        public string Direcao { get; set; }

        public string Ativo { get; set; }

        public string Assessor { get; set; }

        public string CpfCnpj { get; set; }

        public string NomeCliente { get; set; }

        public string CodigoBovespa { get; set; }

        #endregion

        #region | Métodos

        public List<TransporteRelatorio_007> TraduzirLista(List<RiscoClienteBloqueioInstrumentoRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_007>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(delegate(RiscoClienteBloqueioInstrumentoRelInfo cbi)
                {
                    lRetorno.Add(new TransporteRelatorio_007()
                    {
                        Assessor = cbi.CdAssessor.Value.DBToString(),
                        Ativo = cbi.CdAtivo.ToUpper(),
                        CodigoBovespa = cbi.CdCodigo.DBToString(),
                        CpfCnpj = cbi.DsCpfCnpj.ToCpfCnpjString(),
                        Direcao = "C".Equals(cbi.DsDirecao.ToUpper()) ? "Compra" : "V".Equals(cbi.DsDirecao.ToUpper()) ? "Venda" : "Compra/Venda",
                        NomeCliente = cbi.DsNome.ToStringFormatoNome(),
                    });
                });

            return lRetorno;
        }

        #endregion

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }
    }
}