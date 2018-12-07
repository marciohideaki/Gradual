using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Info;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Dados.Enum;

namespace Gradual.Site.Www
{
    public class TransporteStartStop
    {
        #region Propriedades

        public string Id { get; set; }

        public string Conta { get; set; }

        public string DataOrdemEnvio { get; set; }

        public string Codigo { get; set; } 

        public string CodigoDoPapel { get; set; }

        public string Quantidade { get; set; }

        public string PrecoDeDisparo { get; set; }

        public string PrecoLimite { get; set; }

        public string InicioMovel { get; set; }

        public string AjusteMovel { get; set; }

        public string Enviado { get; set; }

        public string Cancelado { get; set; }

        public string Status { get; set; }

        public string LossOuGain { get; set; }

        public string Data { get; set; }

        public string Validade { get; set; }

        public string DataDeValidade { get; set; }

        public string DataCanceladaOuEnviada { get; set; }

        public List<TransporteStartStopHistorico> Historico { get; set; }

        #endregion

        #region Construtor
        
        public TransporteStartStop() { }
        
        public TransporteStartStop(OrdemStopStartInfo pInfo)
        {
            
            this.Codigo                 = pInfo.StopStartID.ToString();
            this.Id                     = pInfo.StopStartID.ToString();
            this.CodigoDoPapel          = pInfo.Symbol;
            this.Data                   = (pInfo.RegisterTime.HasValue ) ? pInfo.RegisterTime.Value.ToString("HH:mm:ss") : "";

            if (pInfo.IdStopStartTipo == StopStartTipoEnum.StartCompra)
            {
                this.PrecoDeDisparo = (pInfo.StartPriceValue.HasValue) ? pInfo.StartPriceValue.Value.ToString("n2") : "0,00";
                this.PrecoLimite    = (pInfo.SendStartPrice.HasValue) ? pInfo.SendStartPrice.Value.ToString("n2") : "0,00";
            }
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopLoss)
            {
                this.PrecoDeDisparo = (pInfo.StopLossValuePrice.HasValue) ? pInfo.StopLossValuePrice.Value.ToString("n2") : "0,00";
                this.PrecoLimite    = (pInfo.SendStopLossValuePrice.HasValue) ? pInfo.SendStopLossValuePrice.Value.ToString("n2") : "0,00";
            }
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopGain)
            {
                this.PrecoDeDisparo = (pInfo.StopGainValuePrice.HasValue) ? pInfo.StopGainValuePrice.Value.ToString("n2") : "0,00";
                this.PrecoLimite    = (pInfo.SendStopGainPrice.HasValue) ? pInfo.SendStopGainPrice.Value.ToString("n2") : "0,00";
            }
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopMovel)
            {
                this.PrecoDeDisparo = (pInfo.StopLossValuePrice.HasValue) ? pInfo.StopLossValuePrice.Value.ToString("n2") : "0,00";
                this.PrecoLimite    = (pInfo.StopGainValuePrice.HasValue) ? pInfo.StopGainValuePrice.Value.ToString("n2") : "0,00";
            }
            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopSimultaneo)
            {
                if ( pInfo.StopGainValuePrice > 0 )
                {
                    this.PrecoDeDisparo =  pInfo.StopGainValuePrice.Value.ToString("n2") ;
                    this.PrecoLimite    =  pInfo.SendStopGainPrice.Value.ToString("n2")  ;
                }
                else if (pInfo.StopLossValuePrice > 0 )
                {
                    this.PrecoDeDisparo = (pInfo.StopLossValuePrice.HasValue)     ? pInfo.StopLossValuePrice.Value.ToString("n2") : "0,00";
                    this.PrecoLimite    = (pInfo.SendStopLossValuePrice.HasValue) ? pInfo.SendStopLossValuePrice.Value.ToString("n2") : "0,00";
                }
            }

            this.Enviado            = (pInfo.ReferencePrice.HasValue) ? pInfo.ReferencePrice.Value.ToString("n2") : "0,00";
            this.Status             = GetStatus(pInfo.StopStartStatusID);

            if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopSimultaneo)

                this.LossOuGain = "Simultaneo";

            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StopGain)

                this.LossOuGain = "Gain";

            else if (pInfo.IdStopStartTipo == StopStartTipoEnum.StartCompra)

                this.LossOuGain = "Start";

            else
                this.LossOuGain = "Loss";

            this.Conta                  = pInfo.Account.ToString();
            this.InicioMovel            = (pInfo.InitialMovelPrice.HasValue) ? pInfo.InitialMovelPrice.Value.ToString("n2") : "0,00";
            this.AjusteMovel            = (pInfo.AdjustmentMovelPrice.HasValue) ? pInfo.AdjustmentMovelPrice.Value.ToString("n2") : "0,00";
            this.Quantidade             = pInfo.OrderQty.ToString();
            this.DataCanceladaOuEnviada = (pInfo.ExecutionTime.HasValue) ? pInfo.ExecutionTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
            
            this.DataDeValidade         = (pInfo.ExpireDate.HasValue) ? pInfo.ExpireDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";

            this.Historico = new List<TransporteStartStopHistorico>();

            TransporteStartStopHistorico lHistorico;
            
            foreach (OrdemStopStartInfoDetalhe lDetalhe in pInfo.Details)
            {
                lHistorico = new TransporteStartStopHistorico();

                //Historico.Situacao     = lDetalhe.OrderStatusDescription;
                lHistorico.Situacao     = GetStatusHistorico(lDetalhe.StopStartStatusID.Value,  lDetalhe.OrderStatusDescription);
                lHistorico.DataSituacao = (lDetalhe.RegisterTime.HasValue)? lDetalhe.RegisterTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;

                this.Historico.Add(lHistorico);
            }
        }

        #endregion

        #region Métodos Private
        
        
        private string GetStatusHistorico(int StopStartStatusID, string pCritica)
        {
            string lRetorno = string.Empty;

            switch (StopStartStatusID)
            {
                case 1:
                    lRetorno = "Tentando Registrar";
                    break;

                case 2:
                    lRetorno = "Registrada";
                    break;

                case 3:
                    lRetorno = "Aguardando disparo";
                    break;

                case 4:
                    lRetorno = "Rejeitada pela corretora";
                    break;

                case 6:
                    lRetorno = "Houve críticas no envio da ordem";
                    break;

                case 7:
                    lRetorno = "Requisitando remoção da monitoração";
                    break;

                case 10:
                    lRetorno = "Disparada";
                    break;

                case 12:
                    lRetorno = "Cancelada";
                    break;

                case 8:
                    lRetorno = "Ordem Stop/Start removido da monitoração";
                    break;

                case 9:
                    lRetorno = "Cancelamento Rejeitado";
                    break;

                case 5:
                    lRetorno = "Enviada para a bolsa";
                    break;

                case 11:
                    lRetorno = "Executada";
                    break;

                case 13:
                    lRetorno = "Expirada";
                    break;

                default:
                    lRetorno = string.Format("{0} - {1}", StopStartStatusID.ToString(), "Não identificado");
                    break;
            }

            return (!string.IsNullOrEmpty(pCritica.Trim())) ? string.Format("{0} - {1}", lRetorno, pCritica) : lRetorno ;
        }


        private string GetStatus(int StopStartStatusID)
        {
            string lRetorno = string.Empty;

            switch (StopStartStatusID)
            {
                case 1:
                case 2:
                    lRetorno = "Tentando Cadastrar";
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

    public class TransporteStartStopHistorico
    {
        public string Situacao { get; set; }
        
        public string DataSituacao { get; set; }
    }
}