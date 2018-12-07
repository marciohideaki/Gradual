using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace Gradual.Site.Www
{
    public class TransporteTickerDeDestaquesMensagem
    {
        #region Propriedades

        public string CodigoDoInstrumento { get; set; }

        public string UltimoPrecoDeNegocio { get; set; }

        public string IndicadorDeVariacaoFechamentoAnterior { get; set; }

        public string VariacaoFechamentoAnterior { get; set; }

        public string VolumeAcumulado { get; set; }

        public string VolumeMilhar
        {
            get
            {
                if (!string.IsNullOrEmpty(this.VolumeAcumulado))
                {
                    double lVolume;

                    if (double.TryParse(this.VolumeAcumulado.Substring(0, this.VolumeAcumulado.IndexOf(',')), out lVolume))
                    {
                        string lRetorno;

                        if (lVolume >= 1000000000)
                        {
                            lRetorno = string.Format("{0:N2}B", (lVolume / 1000000000));
                        }
                        else if (lVolume >= 1000000)
                        {
                            lRetorno = string.Format("{0:N2}M", (lVolume / 1000000));
                        }
                        else if (lVolume >= 1000)
                        {
                            lRetorno = string.Format("{0:N2}K", (lVolume / 1000));
                        }
                        else
                        {
                            lRetorno = this.VolumeAcumulado;
                        }

                        return lRetorno;
                    }
                }

                return "";
            }
        }

        public string NumeroDeNegocios { get; set; }

        public string NumeroDeNegociosFormatado
        {
            get
            {
                double lValor;

                if (double.TryParse(this.NumeroDeNegocios, out lValor))
                {
                    return string.Format("{0:N0}", lValor);
                }

                return this.NumeroDeNegocios;
            }
        }

        public string HoraDaUltimaAtualizacao { get; set; }

        public string TipoDeRanking { get; set; }

        #endregion

        #region Métodos Públicos

        public void ProcessarMensagem(string pMensagem)
        {
            try
            {
                this.CodigoDoInstrumento = pMensagem.Substring(0, 20).Trim();
                this.UltimoPrecoDeNegocio = pMensagem.Substring(20, 12).TrimStart('0');         //O substring original era 20, 13 mas estou cortando pra ficar com 2 casas depois da vírgual
                this.IndicadorDeVariacaoFechamentoAnterior = pMensagem.Substring(33, 1);
                this.VariacaoFechamentoAnterior = pMensagem.Substring(34, 8).TrimStart('0');
                this.VolumeAcumulado = pMensagem.Substring(42, 13).TrimStart('0');
                this.NumeroDeNegocios = pMensagem.Substring(55, 8).TrimStart('0');
                this.HoraDaUltimaAtualizacao = pMensagem.Substring(63, 5);


                if (this.VariacaoFechamentoAnterior.StartsWith(","))
                    this.VariacaoFechamentoAnterior = "0" + this.VariacaoFechamentoAnterior;

                this.VariacaoFechamentoAnterior = string.Format("{0}{1}", this.IndicadorDeVariacaoFechamentoAnterior, this.VariacaoFechamentoAnterior);

            }
            catch (Exception ex)
            {
                ILog lLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                lLogger.ErrorFormat("Erro [{0}] em TransporteTickerDeDestaquesMensagem.ProcessarMensagem()\r\n    >>Parâmetro:\r\n[{1}]\r\n    >>Stack:\r\n{2}"
                                    , ex.Message
                                    , pMensagem
                                    , ex.StackTrace);
            }
            //this.TipoDeRanking = pMensagem.Substring(68, 1);
        }

        #endregion

        #region Construtor

        public TransporteTickerDeDestaquesMensagem()
        {
        }

        public TransporteTickerDeDestaquesMensagem(string pMensagem)
        {
            this.ProcessarMensagem(pMensagem);
        }

        #endregion
    }
}