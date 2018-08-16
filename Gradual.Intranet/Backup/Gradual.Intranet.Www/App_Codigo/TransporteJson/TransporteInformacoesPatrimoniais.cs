using System;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using System.Globalization;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteInformacoesPatrimoniais : ITransporteJSON
    {
        #region | Propriedades

        public System.Nullable<int> Id { get; set; }

        public int ParentId { get; set; }

        public string TotalBensImoveis { get; set; }

        public string TotalBensMoveis { get; set; }

        public string TotalAplicacoesFinanceiras { get; set; }

        public string TotalSalarioProLabore { get; set; }

        public string TotalOutrosRendimentos { get; set; }

        public string DescricaoOutrosRendimentos { get; set; }

        public string TotalCapitalSocial { get; set; }

        public string TotalPatrimonioLiquino { get; set; }

        public string DtCapitalSocial { get; set; }

        public string DtPatrimonioLiquido { get; set; }

        public string DtAtualizacao { get; set; }

        #endregion

        #region Construtor

        public TransporteInformacoesPatrimoniais() { }

        public TransporteInformacoesPatrimoniais(ClienteSituacaoFinanceiraPatrimonialInfo pInfo)
        {
            CultureInfo lInfo = new CultureInfo("pt-BR");

            this.Id       = pInfo.IdSituacaoFinanceiraPatrimonial;
            this.ParentId = pInfo.IdCliente;

            if (pInfo.VlTotalBensImoveis.HasValue)
                this.TotalBensImoveis = pInfo.VlTotalBensImoveis.Value.ToString("n", lInfo);

            if (pInfo.VlTotalBensMoveis.HasValue)
                this.TotalBensMoveis = pInfo.VlTotalBensMoveis.Value.ToString("n", lInfo);

            if(pInfo.VlTotalAplicacaoFinanceira.HasValue)
                this.TotalAplicacoesFinanceiras = pInfo.VlTotalAplicacaoFinanceira.Value.ToString("n", lInfo); 

            if(pInfo.VlTotalSalarioProLabore.HasValue)
                this.TotalSalarioProLabore = pInfo.VlTotalSalarioProLabore.Value.ToString("n", lInfo);

            if(pInfo.VlTotalOutrosRendimentos.HasValue)
                this.TotalOutrosRendimentos = pInfo.VlTotalOutrosRendimentos.Value.ToString("n", lInfo);

            if(pInfo.VlTotalPatrimonioLiquido.HasValue)
                this.TotalPatrimonioLiquino = pInfo.VlTotalPatrimonioLiquido.Value.ToString("n", lInfo);

            if(pInfo.VTotalCapitalSocial.HasValue)
                this.TotalCapitalSocial = pInfo.VTotalCapitalSocial.Value.ToString("n", lInfo);

            this.DescricaoOutrosRendimentos = pInfo.DsOutrosRendimentos.DBToString();

            this.DtCapitalSocial            = (pInfo.DtCapitalSocial.Equals(DateTime.MinValue) || pInfo.DtCapitalSocial == null) ? string.Empty : DateTime.Parse(pInfo.DtCapitalSocial.ToString()).ToString("dd/MM/yyyy");

            this.DtPatrimonioLiquido        = (pInfo.DtPatrimonioLiquido.Equals(DateTime.MinValue) || pInfo.DtPatrimonioLiquido == null) ? string.Empty : DateTime.Parse(pInfo.DtPatrimonioLiquido.ToString()).ToString("dd/MM/yyyy");
            this.DtAtualizacao              = pInfo.DtAtualizacao.DBToString();
        }

        #endregion

        #region | Métodos

        public ClienteSituacaoFinanceiraPatrimonialInfo ToClienteSituacaoFinanceiraPatrimonialInfo()
        {
            ClienteSituacaoFinanceiraPatrimonialInfo lRetorno = new ClienteSituacaoFinanceiraPatrimonialInfo();

            lRetorno.IdSituacaoFinanceiraPatrimonial = this.Id;

            lRetorno.IdCliente                  = this.ParentId.DBToInt32();
            lRetorno.VlTotalBensImoveis         = this.TotalBensImoveis.DBToDecimal();
            lRetorno.VlTotalBensMoveis          = this.TotalBensMoveis.DBToDecimal();
            lRetorno.VlTotalAplicacaoFinanceira = this.TotalAplicacoesFinanceiras.DBToDecimal();
            lRetorno.VlTotalSalarioProLabore    = this.TotalSalarioProLabore.DBToDecimal();
            lRetorno.VlTotalOutrosRendimentos   = this.TotalOutrosRendimentos.DBToDecimal();
            lRetorno.VTotalCapitalSocial        = this.TotalCapitalSocial.DBToDecimal();
            lRetorno.VlTotalPatrimonioLiquido   = this.TotalPatrimonioLiquino.DBToDecimal();
            lRetorno.DtCapitalSocial            = this.DtCapitalSocial.DBToDateTime(eDateNull.Permite);
            lRetorno.DtPatrimonioLiquido        = this.DtPatrimonioLiquido.DBToDateTime(eDateNull.Permite);
            lRetorno.DsOutrosRendimentos        = this.DescricaoOutrosRendimentos;
            lRetorno.DtAtualizacao              = DateTime.Now;

            return lRetorno;
        }

        #endregion

        #region ITransporteJSON Members


        public string TipoDeItem
        {
            get { return "InformacoesPatrimoniais"; }
        }

        #endregion
    }
}
