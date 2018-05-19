using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    /// <summary>
    /// Classe de transporte de fundos x categoria x subcategoria
    /// </summary>
    public class TransporteFundoCategoriaSubCategoria
    {
        
        #region Propriedades
        public int IdFundoCategoriaSubCategoria { get; set; }
        public int IdFundoCadastro { get; set; }
        public int IdFundoCategoria { get; set; }
        public int IdFundoSubCategoria { get; set; }
        #endregion

        #region Construtores
        public TransporteFundoCategoriaSubCategoria() { }

        public TransporteFundoCategoriaSubCategoria(FundoCategoriaInfo pInfo)
        {
            
        }
        #endregion

        #region Métodos
        public List<TransporteFundoCategoriaSubCategoria> TraduzirLista(List<FundoCategoriaSubCategoriaInfo> pInfo)
        {
            var lRetorno = new List<TransporteFundoCategoriaSubCategoria>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteFundoCategoriaSubCategoria()
                    {
                        IdFundoCategoriaSubCategoria = info.IdFundoCategoriaSubCategoria,
                        IdFundoCadastro = info.IdFundoCadastro,
                        IdFundoCategoria = info.IdFundoCategoria,
                        IdFundoSubCategoria = info.IdFundoSubCategoria
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}