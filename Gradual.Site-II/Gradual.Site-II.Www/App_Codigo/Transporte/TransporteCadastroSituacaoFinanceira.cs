using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using System.Globalization;

namespace Gradual.Site.Www
{
    public class TransporteCadastroSituacaoFinanceira
    {
        #region Propriedades

        public int IdCliente { get; set; }
        public int? IdSituacaoFinanceira { get; set; }

        public string VlTotalAplicacaoFinanceira    { get; set; }
        public string VlTotalBensImoveis            { get; set; }
        public string VlTotalBensMoveis             { get; set; }
        public string VlTotalOutrosRendimentos      { get; set; }
        public string VlTotalOutrosRendimentosDesc  { get; set; }
        public string VlTotalPatrimonioLiquido      { get; set; }
        public string VlTotalSalarioProLabore       { get; set; }
        public string VlTotalCapitalSocial          { get; set; }

        #endregion

        #region Construtores

        public TransporteCadastroSituacaoFinanceira() { }

        public TransporteCadastroSituacaoFinanceira(ClienteSituacaoFinanceiraPatrimonialInfo pInfo)
        {
            this.IdCliente = pInfo.IdCliente;

            if(pInfo.IdSituacaoFinanceiraPatrimonial.HasValue)
                this.IdSituacaoFinanceira = pInfo.IdSituacaoFinanceiraPatrimonial.Value;

            if(pInfo.VlTotalAplicacaoFinanceira.HasValue)
                this.VlTotalAplicacaoFinanceira = pInfo.VlTotalAplicacaoFinanceira.Value.ToString("N2");

            if(pInfo.VlTotalBensImoveis.HasValue)
                this.VlTotalBensImoveis         = pInfo.VlTotalBensImoveis.Value.ToString("N2");

            if(pInfo.VlTotalBensMoveis.HasValue)
                this.VlTotalBensMoveis          = pInfo.VlTotalBensMoveis.Value.ToString("N2");

            if(pInfo.VlTotalOutrosRendimentos.HasValue)
                this.VlTotalOutrosRendimentos   = pInfo.VlTotalOutrosRendimentos.Value.ToString("N2");

            if(pInfo.VlTotalPatrimonioLiquido.HasValue)
                this.VlTotalPatrimonioLiquido   = pInfo.VlTotalPatrimonioLiquido.Value.ToString("N2");

            if(pInfo.VlTotalSalarioProLabore.HasValue)
                this.VlTotalSalarioProLabore    = pInfo.VlTotalSalarioProLabore.Value.ToString("N2");

            if(pInfo.VTotalCapitalSocial.HasValue)
                this.VlTotalCapitalSocial       = pInfo.VTotalCapitalSocial.Value.ToString("N2");

            this.VlTotalOutrosRendimentosDesc = pInfo.DsOutrosRendimentos;
        }

        #endregion

        #region Métodos Públicos

        public ClienteSituacaoFinanceiraPatrimonialInfo ToClienteSituacaoFinanceiraPatrimonialInfo()
        {
            ClienteSituacaoFinanceiraPatrimonialInfo lRetorno = new ClienteSituacaoFinanceiraPatrimonialInfo();

            CultureInfo lInfo = new CultureInfo("pt-BR");

            lRetorno.IdCliente = this.IdCliente;

            if(this.IdSituacaoFinanceira.HasValue)
                lRetorno.IdSituacaoFinanceiraPatrimonial = this.IdSituacaoFinanceira.Value;
            
            if(!string.IsNullOrEmpty(this.VlTotalAplicacaoFinanceira))
                lRetorno.VlTotalAplicacaoFinanceira = Convert.ToDecimal(this.VlTotalAplicacaoFinanceira, lInfo);
            
            if(!string.IsNullOrEmpty(this.VlTotalBensImoveis))
                lRetorno.VlTotalBensImoveis         = Convert.ToDecimal(this.VlTotalBensImoveis,         lInfo);

            
            if(!string.IsNullOrEmpty(this.VlTotalBensMoveis))
                lRetorno.VlTotalBensMoveis          = Convert.ToDecimal(this.VlTotalBensMoveis,          lInfo);
            
            if(!string.IsNullOrEmpty(this.VlTotalOutrosRendimentos))
                lRetorno.VlTotalOutrosRendimentos   = Convert.ToDecimal(this.VlTotalOutrosRendimentos,   lInfo);
            
            if(!string.IsNullOrEmpty(this.VlTotalPatrimonioLiquido))
                lRetorno.VlTotalPatrimonioLiquido   = Convert.ToDecimal(this.VlTotalPatrimonioLiquido,   lInfo);
            
            if(!string.IsNullOrEmpty(this.VlTotalSalarioProLabore))
                lRetorno.VlTotalSalarioProLabore    = Convert.ToDecimal(this.VlTotalSalarioProLabore,    lInfo);
            
            if(!string.IsNullOrEmpty(this.VlTotalCapitalSocial))
                lRetorno.VTotalCapitalSocial        = Convert.ToDecimal(this.VlTotalCapitalSocial,       lInfo);
            
            if(!string.IsNullOrEmpty(this.VlTotalOutrosRendimentosDesc))
                lRetorno.DsOutrosRendimentos = this.VlTotalOutrosRendimentosDesc.ToUpper();

            return lRetorno;
        }

        public void TransferirClienteSituacaoFinanceiraPatrimonialInfo(ref ClienteSituacaoFinanceiraPatrimonialInfo pInfo)
        {
            CultureInfo lInfo = new CultureInfo("pt-BR");

            if(!string.IsNullOrEmpty(this.VlTotalAplicacaoFinanceira))
                pInfo.VlTotalAplicacaoFinanceira = Convert.ToDecimal(this.VlTotalAplicacaoFinanceira, lInfo);

            if(!string.IsNullOrEmpty(this.VlTotalBensImoveis))
                pInfo.VlTotalBensImoveis         = Convert.ToDecimal(this.VlTotalBensImoveis,         lInfo);

            if(!string.IsNullOrEmpty(this.VlTotalBensMoveis))
                pInfo.VlTotalBensMoveis          = Convert.ToDecimal(this.VlTotalBensMoveis,          lInfo);

            if(!string.IsNullOrEmpty(this.VlTotalOutrosRendimentos))
                pInfo.VlTotalOutrosRendimentos   = Convert.ToDecimal(this.VlTotalOutrosRendimentos,   lInfo);

            if(!string.IsNullOrEmpty(this.VlTotalPatrimonioLiquido))
                pInfo.VlTotalPatrimonioLiquido   = Convert.ToDecimal(this.VlTotalPatrimonioLiquido,   lInfo);

            if(!string.IsNullOrEmpty(this.VlTotalSalarioProLabore))
                pInfo.VlTotalSalarioProLabore    = Convert.ToDecimal(this.VlTotalSalarioProLabore,    lInfo);

            if(!string.IsNullOrEmpty(this.VlTotalCapitalSocial))
                pInfo.VTotalCapitalSocial        = Convert.ToDecimal(this.VlTotalCapitalSocial,       lInfo);

            if(!string.IsNullOrEmpty(this.VlTotalOutrosRendimentosDesc))
                pInfo.DsOutrosRendimentos = this.VlTotalOutrosRendimentosDesc.ToUpper();
        }

        #endregion
    }
}