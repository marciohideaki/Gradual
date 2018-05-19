using System;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Risco
{
    public class RiscoClienteSaldoBloqueadoRelInfo : Gradual.OMS.Library.ICodigoEntidade
    {
        #region | Propriedades de consulta

        public int? ConsultaIdCliente { get; set; }

        public string ConsultaDsNome { get; set; }

        public string ConsultaDsCpfCnpj { get; set; }

        public string ConsultaDsAtivo { get; set; }

        public string ConsultaTpOrdem { get; set; }

        public DateTime? ConsultaDtTransacaoDe { get; set; }

        public DateTime? ConsultaDtTransacaoAte { get; set; }

        public int ConsultaIdCanalBovespa { get; set; }

        #endregion

        #region | Propriedades de resultado

        public int CdBovespa { get; set; }

        public string DsNome { get; set; }

        public string DsCpfCnpj { get; set; }

        public string TpOrdem { get; set; }

        public int QtOrdem { get; set; }

        public decimal VlPreco { get; set; }

        public string DsAtivo { get; set; }

        public string DsStatusOrdem { get; set; }

        public decimal VlBloqueioOperacaoTotal { get; set; }

        public DateTime DtTransacao { get; set; }

        #endregion

        #region | Implement interface

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
