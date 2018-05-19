using Gradual.Intranet.Contratos.Dados;
using System;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    /// <summary>
    /// Linha do Relatório Clientes > "Pendências Cadastrais"
    /// </summary>
    public class TransporteRelatorio_002
    {
        #region Propriedades

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Bovespa { get; set; }

        public string CpfCnpj { get; set; }

        public string Assessor { get; set; }

        public string DataDaPendencia { get; set; }

        public string DataDaResolucao { get; set; }
        
        public string TipoDePendencia { get; set; }

        public string DescricaoDaPendencia { get; set; }

        public string TipoPessoa { get; set; }

       

        #endregion

        #region Construtor

        public TransporteRelatorio_002() { }

        public TransporteRelatorio_002(ClientePendenciaCadastralRelInfo pInfo) 
        {
            this.Id                     = pInfo.IdCliente;
            this.Nome                   = pInfo.DsNomeCliente;
            this.CpfCnpj                = pInfo.DsCpfCnpj.DBToInt64(true).ToCpfCnpjString();
            this.DataDaPendencia        = pInfo.DtPendenciaCadastral.ToString("dd/MM/yyyy");
            this.Assessor               = pInfo.CodigoAssessor.DBToString();
            this.TipoDePendencia        = pInfo.DsTipoPendenciaCadastral;
            this.DescricaoDaPendencia   = pInfo.DsPendenciaCadastral;
            this.TipoPessoa             = pInfo.TipoPessoa.Equals("J") ? "Júridica" : "Física";
            this.Bovespa                = pInfo.CodigoBolsa.DBToString();
            this.DataDaResolucao        =    pInfo.DtResolucao==DateTime.MinValue?"":pInfo.DtResolucao.ToString("dd/MM/yyyy");
        }

        #endregion
    }
}
