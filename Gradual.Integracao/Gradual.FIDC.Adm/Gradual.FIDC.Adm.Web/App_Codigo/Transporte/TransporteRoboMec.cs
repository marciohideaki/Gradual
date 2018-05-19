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
    public class TransporteRoboMec
    {
        #region Propriedades
        public string CodigoFundo       { get; set; }
        
        public string NomeFundo         { get; set; }
        
        public string CodigoLocalidade  { get; set; }
        
        public string Categoria         { get; set; }
        
        public string DownloadHora      { get; set; }
        
        public string Status            { get; set; }
        
        public string DownloadLink      { get; set; }
        #endregion

        #region Construtores
        public TransporteRoboMec() { }

        public TransporteRoboMec(MecInfo pInfo)
        {
            this.CodigoFundo      = pInfo.CodigoFundo.ToString();        
            
            this.NomeFundo        = pInfo.NomeFundo;          
            
            this.CodigoLocalidade = pInfo.CodigoLocalidade.ToString();
            
            this.Categoria        = pInfo.Categoria;          
            
            this.DownloadHora     = pInfo.DownloadHora.ToString("dd/MM/yyyy");
            
            this.Status           = pInfo.Status;
            
            this.DownloadLink     = pInfo.DownloadLink;
        }
        #endregion

        #region Métodos
        
        /// <summary>
        /// Método para Tradução de lista para transporte de Mec
        /// </summary>
        /// <param name="pInfo">Info de Mapa de evolução de cotas</param>
        /// <returns>Retorna uma lista de Mapa de evolução de cotas</returns>
        public List<TransporteRoboMec> TraduzirLista(List<MecInfo> pInfo)
        {
            var lRetorno = new List<TransporteRoboMec>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteRoboMec()
                    {
                        CodigoFundo = info.CodigoFundo.ToString(),

                        NomeFundo = info.NomeFundo,

                        CodigoLocalidade = info.CodigoLocalidade.ToString(),

                        Categoria = info.Categoria,

                        DownloadHora = info.DownloadHora.ToString("dd/MM/yyyy"),

                        Status = info.Status,

                        DownloadLink = info.DownloadLink

                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}