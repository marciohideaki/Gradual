using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_002 : ICodigoEntidade
    {
        public string NomeCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string DescricaoPermissao { get; set; }

        public string Bolsa { get; set; }

        public string DescricaoGrupo { get; set; }

        public List<TransporteRelatorio_002> TraduzirLista(List<RiscoClientePermissaoRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_002>();

            if (null != pParametro)
                pParametro.ForEach(delegate(RiscoClientePermissaoRelInfo rcp)
                {
                    lRetorno.Add(new TransporteRelatorio_002()
                    {
                        Bolsa = rcp.Bolsa,
                        CpfCnpj = rcp.CpfCnpj.ToCpfCnpjString(),
                        DescricaoGrupo = rcp.DescricaoGrupo,
                        DescricaoPermissao = rcp.DescricaoPermissao,
                        NomeCliente = rcp.NomeCliente.ToStringFormatoNome(),
                    });
                });

            lRetorno.Sort(delegate(TransporteRelatorio_002 tr1, TransporteRelatorio_002 tr2) { return Comparer<string>.Default.Compare(tr1.NomeCliente, tr2.NomeCliente); });

            return lRetorno;
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}