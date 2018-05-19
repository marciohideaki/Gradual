using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente
{
    public class TransporteRelatorio_016
    {
        public string Cliente { get; set; }

        public string Ativo { get; set; }

        public string CodigoCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string Produto { get; set; }

        public string DataAdesao { get; set; }

        public string DataVencimento { get; set; }

        public string DataRetroativaTrocaAtivo { get; set; }

        public List<TransporteRelatorio_016> TraduzirLista(List<ClientePlanoPoupeInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_016>();

            if (null != pParametros && pParametros.Count > 0)
            {
                pParametros.ForEach(lClientePlano =>
                {
                    lRetorno.Add(new TransporteRelatorio_016()
                    {
                        Ativo = lClientePlano.DsAtivo.ToUpper(),
                        CodigoCliente = lClientePlano.CdCliente.ToCodigoClienteFormatado(),
                        CpfCnpj = lClientePlano.DsCpfCnpj.ToCpfCnpjString(),
                        DataAdesao = lClientePlano.DtAdesao.ToString("dd/MM/yyyy"),
                        DataRetroativaTrocaAtivo = lClientePlano.DtRetroTrocaAtivo.ToString("dd/MM/yyyy"),
                        DataVencimento = lClientePlano.DtVencimento.ToString("dd/MM/yyyy"),
                        Cliente = string.Concat(lClientePlano.CdCliente.ToCodigoClienteFormatado(), " ", lClientePlano.DsNome.ToStringFormatoNome()),
                        Produto = lClientePlano.DsProduto,
                    });
                });
            }

            return lRetorno;
        }
    }
}