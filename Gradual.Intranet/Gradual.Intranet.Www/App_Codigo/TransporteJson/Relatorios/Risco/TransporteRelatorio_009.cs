using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.OMS.Library;
using System;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_009 : ICodigoEntidade
    {
        #region | Propriedades

        public string CodigoBovespa { get; set; }

        public string NomeCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string Ativo { get; set; }

        public string TipoOrdem { get; set; }

        public string StatusOrdem { get; set; }

        public string Quantidade { get; set; }

        public string Preco { get; set; }

        public string BloqueadoSemiTotal { get; set; }

        public string BloqueadoTotalGeral { get; set; }

        public string Data { get; set; }

        #endregion

        #region | Métodos

        public List<TransporteRelatorio_009> TraduzirLista(List<RiscoClienteSaldoBloqueadoRelInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_009>();
            var lTotalBloqueado = default(decimal);

            if (null != pParametros && pParametros.Count > 0)
            {
                pParametros.ForEach(delegate(RiscoClienteSaldoBloqueadoRelInfo csb)
                {
                    lTotalBloqueado += csb.VlBloqueioOperacaoTotal;

                    lRetorno.Add(new TransporteRelatorio_009()
                    {
                        Ativo = csb.DsAtivo,
                        BloqueadoSemiTotal = csb.VlBloqueioOperacaoTotal.ToString("N2"),
                        CodigoBovespa = csb.CdBovespa.DBToString(),
                        CpfCnpj = csb.DsCpfCnpj.ToCpfCnpjString(),
                        Data = DateTime.MinValue.Equals(csb.DtTransacao) ? "-" : csb.DtTransacao.ToString("dd/MM/yyyy HH:mm"),
                        NomeCliente = csb.DsNome.ToStringFormatoNome(),
                        Preco = csb.VlPreco.ToString("N2"),
                        Quantidade = csb.QtOrdem.ToString(),
                        StatusOrdem = csb.DsStatusOrdem.ToStringFormatoNome(),
                        TipoOrdem = csb.TpOrdem
                    });
                });

                lRetorno[0].BloqueadoTotalGeral = lTotalBloqueado.ToString("N2");
            }

            return lRetorno;
        }

        #endregion

        #region | Implement Interface

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}