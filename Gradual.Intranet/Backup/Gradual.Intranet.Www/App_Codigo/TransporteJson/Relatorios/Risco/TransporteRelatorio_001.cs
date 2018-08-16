using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_001 : ICodigoEntidade
    {
        public string DsBolsa { get; set; }

        public string DsPermissao { get; set; }

        public List<TransporteRelatorio_001> TraduzirLista(List<RiscoPermissaoRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_001>();

            if (null != pParametro)
                pParametro.ForEach(delegate(RiscoPermissaoRelInfo pri) 
                {
                    lRetorno.Add(new TransporteRelatorio_001() 
                    {
                         DsPermissao = pri.DsPermissao,
                         DsBolsa = pri.DsBolsa,
                    });
                });

            lRetorno.Sort(delegate(TransporteRelatorio_001 tr1, TransporteRelatorio_001 tr2) { return Comparer<string>.Default.Compare(tr1.DsPermissao, tr2.DsPermissao); });

            return lRetorno;
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}