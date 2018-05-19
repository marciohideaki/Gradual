using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    /// <summary>
    /// Classe de transporte de Carteiras do Robo Download
    /// </summary>
    public class TransporteRoboCarteira
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
        /// <summary>
        /// Construtor Robo carteira
        /// </summary>
        public TransporteRoboCarteira() { }

        /// <summary>
        /// Construtor Robo carteira
        /// </summary>
        /// <param name="pInfo">Info</param>
        public TransporteRoboCarteira(CarteirasInfo pInfo)
        { 
            this.CodigoFundo      = pInfo.CodigoFundo.ToString(); 
            
            this.NomeFundo        = pInfo.NomeFundo; 
            
            this.CodigoLocalidade = pInfo.CodigoLocalidade.ToString();
            
            this.Categoria        = pInfo.Categoria;
            
            this.DownloadHora     = pInfo.DownloadHora.ToString();
            
            this.Status           = pInfo.Status;
            
            this.DownloadLink     = pInfo.DownloadLink;
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Método para Tradução de lista para transporte de Carteiras
        /// </summary>
        /// <param name="pInfo">Info de Extrato de cotista</param>
        /// <returns>Retorna uma lista de Extrato de cotista</returns>
        public List<TransporteRoboCarteira> TraduzirLista(List<CarteirasInfo> pInfo)
        {
            var lRetorno = new List<TransporteRoboCarteira>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteRoboCarteira()
                    {
                        CodigoFundo      = info.CodigoFundo.ToString(),
                        
                        NomeFundo        = info.NomeFundo,
                        
                        CodigoLocalidade = info.CodigoLocalidade.ToString(),
                        
                        Categoria        = info.Categoria,
                        
                        DownloadHora     = info.DownloadHora.ToString("dd/MM/yyyy"),
                        
                        Status           = info.Status,
                        
                        DownloadLink     = info.DownloadLink

                    });
                });
            }

            return lRetorno;
        }

        #endregion
    }
}