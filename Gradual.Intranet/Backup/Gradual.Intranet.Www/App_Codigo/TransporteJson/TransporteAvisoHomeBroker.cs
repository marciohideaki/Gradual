using System;
using System.Globalization;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteAvisoHomeBroker
    {
        #region Propriedades

        public int CodigoAviso { get; set; }
        
        public string DataEntrada { get; set; }

        public string DataSaida { get; set; }

        public string HoraEntrada { get; set; }

        public string HoraSaida { get; set; }

        public string TextoTruncado { get; set; }

        public string Texto { get; set; }
        
        public string CBLCs { get; set; }

        public string FlagAtivacaoManual { get; set; }

        public string FlagEstaSendoExibido { get; set; }

        public string MotivoExibicao { get; set; }

        public string IdSistema { get; set; }

        #endregion

        #region Construtor
        
        public TransporteAvisoHomeBroker()
        {
        }

        public TransporteAvisoHomeBroker(AvisoHomeBrokerInfo pAviso)
        {
            this.CodigoAviso = pAviso.IdAviso;

            this.DataEntrada = pAviso.DtEntrada.ToString("dd/MM/yyyy");

            this.DataSaida = pAviso.DtSaida.ToString("dd/MM/yyyy");

            this.HoraEntrada = pAviso.DtEntrada.ToString("HH:mm");

            this.HoraSaida = pAviso.DtSaida.ToString("HH:mm");

            this.CBLCs = pAviso.DsCBLCs;

            this.Texto = pAviso.DsAviso;

            this.IdSistema = pAviso.IdSistema.ToString();

            this.AtualizarTextoTruncado();

            this.FlagAtivacaoManual = pAviso.StAtivacaoManual;

            this.FlagEstaSendoExibido = "N";

            if (DateTime.Now >= pAviso.DtEntrada && DateTime.Now <= pAviso.DtSaida)
            {
                this.FlagEstaSendoExibido = "S";
                this.MotivoExibicao = "está dentro da data de entrada e saída";
            }
            else
            {
                if(pAviso.StAtivacaoManual.ToUpper() == "S")
                {
                    this.FlagEstaSendoExibido = "S";
                    this.MotivoExibicao = "está marcado para exibição manual";
                }
            }
        }

        #endregion

        #region Métodos Públicos

        public AvisoHomeBrokerInfo ToAvisoHomeBrokerInfo()
        {
            AvisoHomeBrokerInfo lRetorno = new AvisoHomeBrokerInfo();

            CultureInfo lCulture = new CultureInfo("pt-BR");

            lRetorno.IdAviso = this.CodigoAviso;

            lRetorno.IdSistema = int.Parse(this.IdSistema);

            lRetorno.DsCBLCs = this.CBLCs;

            DateTime lData;

            try
            {
                if(!DateTime.TryParseExact(this.DataEntrada + " " + this.HoraEntrada, "dd/MM/yyyy HH:mm", lCulture, DateTimeStyles.None, out lData))
                {
                    if(!DateTime.TryParseExact(this.DataEntrada, "dd/MM/yyyy", lCulture, DateTimeStyles.None, out lData))
                    {
                        lData = DateTime.Now.AddDays(1);
                    }
                }

                lRetorno.DtEntrada = lData;
            }
            catch { }

            try
            {
                if(!DateTime.TryParseExact(this.DataSaida + " " + this.HoraSaida, "dd/MM/yyyy HH:mm", lCulture, DateTimeStyles.None, out lData))
                {
                    if(!DateTime.TryParseExact(this.DataSaida, "dd/MM/yyyy", lCulture, DateTimeStyles.None, out lData))
                    {
                        lData = DateTime.Now.AddDays(1);
                    }
                }

                lRetorno.DtSaida = lData;

                if (lRetorno.DtSaida < lRetorno.DtEntrada)
                    lRetorno.DtSaida = lRetorno.DtEntrada.AddDays(1);
            }
            catch { }

            lRetorno.DsAviso = this.Texto;

            lRetorno.StAtivacaoManual = (this.FlagAtivacaoManual == "S") ? "S" : "N";

            return lRetorno;
        }

        #endregion

        internal void AtualizarTextoTruncado()
        {
            this.TextoTruncado = this.Texto;

            if (this.TextoTruncado.Length > 20) 
                this.TextoTruncado = this.TextoTruncado.Substring(0, 20) + "(...)";
        }
    }
}