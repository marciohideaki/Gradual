using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Risco;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteSuitabilityLavagem
    {
        #region Propriedades
        public string NomeCliente { get; set; }

        public string CodigoCliente { get; set; }

        public string NomeAssessor { get; set; }

        public string CodigoAssessor { get; set; }

        public string CodigoLogin { get; set; }

        public string enumVolume { get; set; }

        public string PercentualVOLxSFP { get; set; }

        public string enumVOLxSFP { get; set; }

        public string DataDe { get; set; }

        public string DataAte { get; set; }

        public string SFP { get; set; }

        public string Volume { get; set; }

        public string Suitability { get; set; }

        public string Data { get; set; }
        
        public string ArquivoCiencia { get; set; }

        public string ArquivoCienciaLink
        {
            get
            {
                return string.IsNullOrEmpty(this.ArquivoCiencia) ? "" : string.Format("<a href='{0}' title='{1}' style='color:blue'>arquivo</a>", this.ArquivoCiencia, this.ArquivoCiencia.Substring(this.ArquivoCiencia.LastIndexOf('/') + 1));
            }
        }

        public string CodigoClienteBmf { get; set; }
        #endregion

        #region Métodos
        public List<TransporteSuitabilityLavagem> TraduzirLista(List<SuitabilityLavagemInfo> info)
        {
            var lRetorno = new List<TransporteSuitabilityLavagem>();

            info.ForEach( lav=> 
            {
                lRetorno.Add(new TransporteSuitabilityLavagem()
                    {
                        NomeCliente       = lav.NomeCliente,
                        CodigoCliente     = lav.CodigoCliente.ToString(),
                        NomeAssessor      = lav.NomeAssessor,
                        CodigoAssessor    = lav.CodigoAssessor.ToString(),
                        CodigoClienteBmf  = lav.CodigoClienteBmf.ToString(),
                        CodigoLogin       = lav.CodigoLogin.ToString(),
                        Data              = lav.Data.ToString("dd/MM/yyyy"),
                        enumVolume        = lav.enumVolume.ToString(),
                        enumVOLxSFP       = lav.enumVOLxSFP.ToString(),
                        PercentualVOLxSFP = lav.PercentualVOLxSFP.ToString("N2"),
                        SFP               = lav.SFP.ToString("N2"),
                        Volume            = lav.Volume.ToString("N2"),
                        Suitability       = lav.Suitability,
                        ArquivoCiencia    = lav.ArquivoCiencia
                    });
            });

            return lRetorno;
        }
        #endregion
    }
}