using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;


namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto
{
    public static class Formatador
    {

        public static void ArmarStopSimples(AutomacaoOrdensInfo _TOrder)
        {     
            try
            {
                SS_StopSimples _SS_StopSimples = new SS_StopSimples("BV");

                _SS_StopSimples.CodigoInstrumento = _TOrder.Instrumento;
                _SS_StopSimples.IdStopStart = Convert.ToString(_TOrder.IdStopStart);
                _SS_StopSimples.IdTipoOrdem = Convert.ToString(_TOrder.IdStopStartTipo);
                _SS_StopSimples.PrecoGain = Convert.ToString(_TOrder.PrecoGain);
                _SS_StopSimples.PrecoLoss = Convert.ToString(_TOrder.PrecoLoss);
                _SS_StopSimples.PrecoGain = Convert.ToString(_TOrder.PrecoGain);
                _SS_StopSimples.PrecoStart = Convert.ToString(_TOrder.PrecoStart);

                Contexto.EnviarDados(_SS_StopSimples.getMessageSS());
                
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao acessar o método ArmarStopLoss. Erro: " + ex.Message);
            }

        }      

        public static void CancelarOrdemStop(string Instrument ,int id_stopstart)
        {       
            try
            {
                CS_CancelamentoStop _CS_CancelamentoStop = new CS_CancelamentoStop();

                _CS_CancelamentoStop.IdStopStart = id_stopstart.ToString();
                _CS_CancelamentoStop.CodigoInstrumento = Instrument;
                Contexto.EnviarDados(_CS_CancelamentoStop.getMessageCE());
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao acessar o método CancelarOrdem. Erro: " + ex.Message);
            }

        }

    }
}
