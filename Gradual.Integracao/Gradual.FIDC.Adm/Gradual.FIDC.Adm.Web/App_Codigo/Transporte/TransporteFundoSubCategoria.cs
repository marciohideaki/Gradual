using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    /// <summary>
    /// Classe de transporte de subcategoria de fundos
    /// </summary>
    public class TransporteFundoSubCategoria
    {
        
        #region Propriedades
        public int IdFundoSubCategoria { get; set; }
        public string DsFundoSubCategoria { get; set; }
        #endregion

        #region Construtores
        public TransporteFundoSubCategoria() { }

        public TransporteFundoSubCategoria(FundoSubCategoriaInfo pInfo)
        {
            this.DsFundoSubCategoria = pInfo.DsFundoSubCategoria.ToString();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para Tradução de lista para transporte de Títulos Liquidados
        /// </summary>
        /// <param name="pInfo">Info de Títulos Liquidados</param>
        /// <returns>Retorna uma lista de Títulos Liquidados</returns>
        public List<TransporteFundoSubCategoria> TraduzirLista(List<FundoSubCategoriaInfo> pInfo)
        {
            var lRetorno = new List<TransporteFundoSubCategoria>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteFundoSubCategoria()
                    {
                        IdFundoSubCategoria = info.IdFundoSubCategoria,
                        DsFundoSubCategoria = info.DsFundoSubCategoria,
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}