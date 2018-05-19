using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    /// <summary>
    /// Classe de transporte de categorias de fundo
    /// </summary>
    public class TransporteFundoCategoria
    {
        
        #region Propriedades
        public int IdFundoCategoria { get; set; }
        public string DsFundoCategoria { get; set; }
        #endregion

        #region Construtores
        public TransporteFundoCategoria() { }

        public TransporteFundoCategoria(FundoCategoriaInfo pInfo)
        {
            this.DsFundoCategoria = pInfo.DsFundoCategoria.ToString();
        }
        #endregion

        #region Métodos
        public List<TransporteFundoCategoria> TraduzirLista(List<FundoCategoriaInfo> pInfo)
        {
            var lRetorno = new List<TransporteFundoCategoria>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteFundoCategoria()
                    {
                        IdFundoCategoria = info.IdFundoCategoria,
                        DsFundoCategoria = info.DsFundoCategoria,
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}