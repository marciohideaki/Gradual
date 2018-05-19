using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_004 : ICodigoEntidade
    {
        public string NomeCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string DescricaoParametro { get; set; }

        public string Bolsa { get; set; }

        public string DescricaoGrupo { get; set; }

        public List<TransporteRelatorio_004> TraduzirLista(List<RiscoClienteParametroRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_004>();

            if (null != pParametro)
                pParametro.ForEach(delegate(RiscoClienteParametroRelInfo cpi)
                {
                    lRetorno.Add(new TransporteRelatorio_004()
                    {
                        NomeCliente = cpi.NomeCliente.ToStringFormatoNome(),
                        DescricaoParametro = cpi.DsParametro,
                        DescricaoGrupo = cpi.DsGrupo,
                        Bolsa = cpi.DsBolsa,
                        CpfCnpj = cpi.CpfCnpj.ToCpfCnpjString(),
                    });
                });

            return lRetorno;
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}