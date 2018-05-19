#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Info;
#endregion

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteOrdemStopDetalhe
    {
        public string Situacao { get; set; }

        public string DataSituacao { get; set; }

        public TransporteOrdemStopDetalhe()
        { }

        public TransporteOrdemStopDetalhe(OrdemStopStartInfoDetalhe info)
        {
            this.Situacao     = GetStatusHistorico(info.StopStartStatusID.Value, info.OrderStatusDescription);
            this.DataSituacao = (info.RegisterTime.HasValue) ? info.RegisterTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
        }

        #region Métodos de apoio
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


                default:
                    lRetorno = string.Format("{0} - {1}", StopStartStatusID.ToString(), "Não identificado");
                    break;
            }

            return (!string.IsNullOrEmpty(pCritica.Trim())) ? string.Format("{0} - {1}", lRetorno, pCritica) : lRetorno;
        }
        #endregion

    }
}