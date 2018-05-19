using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    /// <summary>
    /// Classe de transporte Robo titulos liquidados
    /// </summary>
    public class TransporteRoboTituloLiquidados
    {
        #region Propriedades

        public string CodigoFundo   { get; set; }

        public string NomeFundo     { get; set; }

        public string Status        { get; set; }

        public string Data          { get; set; }

        public string DownloadLink  { get; set; }
        #endregion

        #region Construtores
        public TransporteRoboTituloLiquidados() { }

        public TransporteRoboTituloLiquidados(TitulosLiquidadosInfo pInfo)
        {
            this.CodigoFundo  = pInfo.CodigoFundo.ToString();
            
            this.NomeFundo    = pInfo.NomeFundo;
            
            this.Status       = pInfo.Status;

            this.Data         = pInfo.Data.ToString("dd/MM/yyyy");

            this.DownloadLink = pInfo.DownloadLink;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para Tradução de lista para transporte de Títulos Liquidados
        /// </summary>
        /// <param name="pInfo">Info de Títulos Liquidados</param>
        /// <returns>Retorna uma lista de Títulos Liquidados</returns>
        public List<TransporteRoboTituloLiquidados> TraduzirLista(List<TitulosLiquidadosInfo> pInfo)
        {
            var lRetorno = new List<TransporteRoboTituloLiquidados>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteRoboTituloLiquidados()
                    {
                        CodigoFundo     = info.CodigoFundo.ToString(),
                        
                        NomeFundo       = info.NomeFundo,
                        
                        Status          = info.Status,
                        
                        Data            = info.Data.ToString("dd/MM/yyyy"),
                        
                        DownloadLink    = info.DownloadLink
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}