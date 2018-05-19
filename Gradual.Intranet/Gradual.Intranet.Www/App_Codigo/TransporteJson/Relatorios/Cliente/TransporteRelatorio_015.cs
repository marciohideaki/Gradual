using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente
{
    public class TransporteRelatorio_015
    {
        #region | Propriedades

        public string CodBolsa { get; set; }

        public string NomeCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string Produto { get; set; }

        public string DataAdesao { get; set; }

        public string DataFimAdesao { get; set; }

        public string DataCobranca { get; set; }

        public string ValorCobranca { get; set; }

        public string StatusPlano { get; set; }

        public string CodAssessor { get; set; }

        public string Origem { get; set; }

        public string UsuarioLogado { get; set; }
        #endregion

        #region | Construtor

        public TransporteRelatorio_015() { }

        public TransporteRelatorio_015(ClienteProdutoInfo pInfo)
        {
            this.DataAdesao    = (pInfo.DtAdesao.HasValue && pInfo.DtAdesao.Value.ToString("dd/MM/yyyy") != "01/01/0001") ? pInfo.DtAdesao.Value.ToString("dd/MM/yyyy") : " - ";
            this.DataFimAdesao = (pInfo.DtFimAdesao.HasValue && pInfo.DtFimAdesao.Value.ToString("dd/MM/yyyy") != "01/01/0001") ? pInfo.DtFimAdesao.Value.ToString("dd/MM/yyyy") : " - ";
            this.DataCobranca  = (pInfo.DtUltima_cobranca.HasValue && pInfo.DtUltima_cobranca.Value.ToString("dd/MM/yyyy") != "01/01/0001") ? pInfo.DtUltima_cobranca.Value.ToString("dd/MM/yyyy") : " - ";
            this.ValorCobranca = pInfo.VlCobrado.ToString("N");
            this.Produto       = pInfo.NomeProduto;
            this.StatusPlano   = pInfo.StSituacao.ToString();
            this.CodAssessor   = pInfo.CdAssessor.DBToString();
            this.CodBolsa      = pInfo.CdCblc.DBToString();
            this.NomeCliente   = pInfo.NomeCliente.ToStringFormatoNome();
            this.CpfCnpj       = pInfo.DsCpfCnpj.ToCpfCnpjString();
            this.Produto       = pInfo.NomeProduto.DBToString();
            this.Origem        = pInfo.Origem;
            this.UsuarioLogado = pInfo.UsuarioLogado;
        }

        #endregion
    }
}
