using Gradual.Intranet.Contratos.Mensagens;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.ControleDeOrdens;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Info;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Dados.Enum;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteResultadoOrdensStop
    {
        #region | Propriedades

        public string Id { get; set; }

        public string NumeroOrdem { get; set; }

        public string Tipo { get; set; }

        public string Papel { get; set; }

        public string Hora { get; set; }

        public string PrecoDisparo { get; set; }

        public string PrecoLancamento { get; set; }

        public string PrecoDisparoGain { get; set; }

        public string PrecoLancamentoGain { get; set; }

        public string PrecoEnvio { get; set; }

        public string Status { get; set; }

        public string LossGain { get; set; }

        public string Cliente { get; set; }

        public string Validade { get; set; }

        public string PrecoInicioMovel { get; set; }

        public string PrecoAjusteMovel { get; set; }

        public string Quantidade { get; set; }

        #endregion

        #region | Construtores

       public TransporteResultadoOrdensStop() { }

        public TransporteResultadoOrdensStop(OrdemStopStartInfo pInfo)
        {
            this.Id          = this.NumeroOrdem = pInfo.StopStartID.ToString();
            
            if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopSimultaneo)
                this.Tipo = "Simultaneo";
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopGain)
                this.Tipo = "Gain";
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StartCompra)
                this.Tipo = "Start";
            else
                this.Tipo = "Loss";

            this.Papel                 = pInfo.Symbol;
            this.Hora                  = (pInfo.RegisterTime.HasValue ) ? pInfo.RegisterTime.Value.ToString("HH:mm:ss") : "";
            
            if (pInfo.IdStopStartTipo == StopStartTipoEnum.StartCompra)
            {
                this.PrecoDisparo = (pInfo.StartPriceValue.HasValue) ? pInfo.StartPriceValue.Value.ToString("n2") : "0,00";
                this.PrecoLancamento = (pInfo.SendStartPrice.HasValue) ? pInfo.SendStartPrice.Value.ToString("n2") : "0,00";
            }
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopLoss)
            {
                this.PrecoDisparo = (pInfo.StopLossValuePrice.HasValue) ? pInfo.StopLossValuePrice.Value.ToString("n2") : "0,00";
                this.PrecoLancamento = (pInfo.SendStopLossValuePrice.HasValue) ? pInfo.SendStopLossValuePrice.Value.ToString("n2") : "0,00";
            }
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopGain)
            {
                this.PrecoDisparoGain = (pInfo.StopGainValuePrice.HasValue) ? pInfo.StopGainValuePrice.Value.ToString("n2") : "0,00";
                this.PrecoLancamentoGain = (pInfo.SendStopGainPrice.HasValue) ? pInfo.SendStopGainPrice.Value.ToString("n2") : "0,00";
            }
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopMovel)
            {
                this.PrecoDisparo = (pInfo.StopLossValuePrice.HasValue) ? pInfo.StopLossValuePrice.Value.ToString("n2") : "0,00";
                this.PrecoLancamento = (pInfo.StopGainValuePrice.HasValue) ? pInfo.StopGainValuePrice.Value.ToString("n2") : "0,00";
            }
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopSimultaneo)
            {

                this.PrecoDisparoGain    = (pInfo.SendStopGainPrice.HasValue) ? pInfo.SendStopGainPrice.Value.ToString("n2") : "0,00";
                this.PrecoLancamentoGain = (pInfo.StopGainValuePrice.HasValue) ? pInfo.StopGainValuePrice.Value.ToString("n2") : "0,00";
                this.PrecoDisparo        = (pInfo.StopLossValuePrice.HasValue) ? pInfo.StopLossValuePrice.Value.ToString("n2") : "0,00";
                this.PrecoLancamento     = (pInfo.SendStopLossValuePrice.HasValue) ? pInfo.SendStopLossValuePrice.Value.ToString("n2") : "0,00";
                
            }

            this.PrecoEnvio            = (pInfo.ReferencePrice.HasValue) ? pInfo.ReferencePrice.Value.ToString("n2") : "0,00";
            this.Status                = GetStatus(pInfo.StopStartStatusID);
            this.LossGain              = (pInfo.StopGainValuePrice.HasValue) ? pInfo.StopGainValuePrice.Value.ToString("n2") : "0,00";
            this.Cliente               = pInfo.Account.ToString();
            this.Validade              = (pInfo.ExpireDate.HasValue)? pInfo.ExpireDate.Value.ToString("dd/MM/yyyy") : "";
            this.PrecoInicioMovel      = (pInfo.InitialMovelPrice.HasValue)?pInfo.InitialMovelPrice.Value.ToString("n2") : "0,00";
            this.PrecoAjusteMovel      = (pInfo.AdjustmentMovelPrice.HasValue)? pInfo.AdjustmentMovelPrice.Value.ToString("n2") : "0,00";
            this.Quantidade            = pInfo.OrderQty.ToString();
            
        }

        #endregion

        #region | Métodos Apoio

        public List<TransporteResultadoOrdensStop> ToListTransporteResultadoOrdensStop(BuscarOrdensStopStartResponse pInfo)
        {
            var lRetorno = new List<TransporteResultadoOrdensStop>();

            pInfo.OrdensStartStop.ForEach(delegate(OrdemStopStartInfo osso)
            {
                lRetorno.Add(new TransporteResultadoOrdensStop(osso));
            });

            return lRetorno;
        }

        private string GetStatus(int StopStartStatusID)
        {
            string lRetorno = string.Empty;

            switch (StopStartStatusID)
            {
                case 1:
                case 2:
                    lRetorno = "Tentando enviar para a Bolsa";
                    break;
                case 3:
                    lRetorno = "Aberta";
                    break;
                case 4:
                    lRetorno = "Rejeitada";
                    break;
                case 6:
                case 7:
                case 8:
                case 12:
                    lRetorno = "Cancelada";
                    break;
                case 9:
                    lRetorno = "Cancelamento Rejeitado";
                    break;
                case 5:
                    lRetorno = "Disparada";
                    break;
                case 11:
                    lRetorno = "Executada";
                    break;
                case 10:
                    lRetorno = "Disparada";
                    break;
                case 13:
                    lRetorno = "Expirada";
                    break;
                default:
                    lRetorno = string.Format("{0} - {1}", StopStartStatusID.ToString(), "Não identificado");
                    break;
            }
            return lRetorno;
        }
        #endregion
    }
}
