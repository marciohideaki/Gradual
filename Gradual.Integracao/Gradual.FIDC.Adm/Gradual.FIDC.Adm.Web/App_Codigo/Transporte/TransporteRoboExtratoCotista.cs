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
    public class TransporteRoboExtratoCotista
    {
        #region Propriedades
        public string CpfCnpj       { get; set; }

        public string NomeCotista   { get; set; }

        public string CodigoFundo   { get; set; }
        
        public string NomeFundo     { get; set; }
        
        public string Status        { get; set; }
        
        public string DownloadLink  { get; set; }
        #endregion

        #region Construtores
        public TransporteRoboExtratoCotista() { }

        public TransporteRoboExtratoCotista(ExtratoCotistaInfo pInfo)
        {
            this.CpfCnpj = pInfo.CpfCnpj;
            
            this.CodigoFundo   = pInfo.CodigoFundo.ToString() ;
            
            this.NomeFundo     = pInfo.NomeFundo   ;
            
            this.Status        = pInfo.Status      ;
            
            this.DownloadLink  = pInfo.DownloadLink;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para Tradução de lista para transporte de Extrato de Cotista
        /// </summary>
        /// <param name="pInfo">Info de Extrato de cotista</param>
        /// <returns>Retorna uma lista de Extrato de cotista</returns>
        public List<TransporteRoboExtratoCotista> TraduzirLista(List<ExtratoCotistaInfo> pInfo)
        {
            var lRetorno = new List<TransporteRoboExtratoCotista>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteRoboExtratoCotista()
                    {
                        CpfCnpj         = info.CpfCnpj,

                        NomeCotista     = info.NomeCotista,

                        CodigoFundo     = info.CodigoFundo.ToString(),
                        
                        NomeFundo       = info.NomeFundo,
                        
                        Status          = info.Status,
                        
                        DownloadLink    = info.DownloadLink
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}