using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Views;
using Gradual.OMS.Termo.Lib.Info;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteObjetoDoSistema
    {
        #region Propriedades

        public string Id { get; set; }  //é string porque o paíd usa CodPais, que é string.

        public string Descricao { get; set; }

        #endregion

        #region Cosntrutor

        public TransporteObjetoDoSistema(){}

        public TransporteObjetoDoSistema(object pObjetoBase)
        {
            if (pObjetoBase is AtividadeIlicitaInfo)
            {
                this.Id        = ((AtividadeIlicitaInfo)pObjetoBase).IdAtividadeIlicita.ToString();
                this.Descricao = ((AtividadeIlicitaInfo)pObjetoBase).CdAtividade;
            }

            if (pObjetoBase is PaisesBlackListInfo)
            {
                this.Id        = ((PaisesBlackListInfo)pObjetoBase).IdPaisBlackList.ToString();
                this.Descricao = ((PaisesBlackListInfo)pObjetoBase).CdPais;
            }

            if(pObjetoBase is TipoDePendenciaCadastralInfo)
            {
                if(((TipoDePendenciaCadastralInfo)pObjetoBase).IdTipoPendencia.HasValue)
                    this.Id    = ((TipoDePendenciaCadastralInfo)pObjetoBase).IdTipoPendencia.Value.ToString();

                this.Descricao = ((TipoDePendenciaCadastralInfo)pObjetoBase).DsPendencia;
            }

            if(pObjetoBase is ContratoInfo)
            {
                if(((ContratoInfo)pObjetoBase).IdContrato.HasValue)
                    this.Id    = ((ContratoInfo)pObjetoBase).IdContrato.Value.ToString();

                this.Descricao = ((ContratoInfo)pObjetoBase).DsContrato;
            }

            if (pObjetoBase is TaxaTermoInfo)
            {
                TaxaTermoInfo lObjeto = ((TaxaTermoInfo)pObjetoBase);

                this.Id = lObjeto.IdTaxa.ToString();
                this.Descricao = string.Format("Valor: R$ {0:f2} Rolagem: R$ {1:f2}, para {2} dias a partir de {3}"
                                                , lObjeto.ValorTaxa
                                                , lObjeto.ValorRolagem
                                                , lObjeto.NumeroDias
                                                , lObjeto.DataReferencia.ToString("dd/MM/yyyy"));
            }
        }

        #endregion
    }
}