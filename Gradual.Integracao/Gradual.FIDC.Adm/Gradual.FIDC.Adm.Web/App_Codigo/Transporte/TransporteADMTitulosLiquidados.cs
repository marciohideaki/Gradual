using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    /// <summary>
    /// Classe de transporte Mapa de evolução de Cotas
    /// </summary>
    public class TransporteADMTitulosLiquidados
    {
        #region Propriedades

        public string CodigoFundo   { get; set; }

        public string DataRef       { get; set; }

        public string NomeFundo     { get; set; }

        public string Valor         { get; set; }
        #endregion

        #region Construtores
        public TransporteADMTitulosLiquidados() { }

        public TransporteADMTitulosLiquidados(TitulosLiquidadosInfo pInfo)
        {
            this.CodigoFundo    = pInfo.CodigoFundo.ToString();

            this.DataRef        = pInfo.Data.ToString("dd/MM/yyyy");

            this.NomeFundo      = pInfo.NomeFundo;

            this.Valor          = pInfo.Valor.ToString("N2");
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para Tradução de lista para transporte de Títulos Liquidados
        /// </summary>
        /// <param name="pInfo">Info de Títulos Liquidados</param>
        /// <returns>Retorna uma lista de Títulos Liquidados</returns>
        public List<TransporteADMTitulosLiquidados> TraduzirLista(List<TitulosLiquidadosInfo> pInfo)
        {
            var lRetorno = new List<TransporteADMTitulosLiquidados>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteADMTitulosLiquidados()
                    {
                        CodigoFundo = info.CodigoFundo.ToString(),

                        DataRef     = info.Data.ToString("dd/MM/yyyy"),

                        NomeFundo   = info.NomeFundo,

                        Valor       = info.Valor.ToString("N2")
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}