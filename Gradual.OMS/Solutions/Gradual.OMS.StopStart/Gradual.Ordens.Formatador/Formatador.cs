using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.PacoteComunicacao.Mensagens.Enviadas;
using Gradual.Ordens.Comunicacao.Contexto;
using Gradual.Ordens.Template;


namespace Gradual.Ordens.Formatador
{
    public static class Formatador
    {

        public static void ArmarStopLoss(TOrdem _TOrder)
        {     
            try
            {
                SS_StopSimples _SS_StopSimples = new SS_StopSimples("BV");

                _SS_StopSimples.CodigoInstrumento = _TOrder.instrumento;
                _SS_StopSimples.IdStopStart = Convert.ToString(_TOrder.id_stopstart);
                _SS_StopSimples.IdTipoOrdem = Convert.ToString(_TOrder.id_stopstart_tipo);
                _SS_StopSimples.PrecoGain = Convert.ToString(_TOrder.preco_gain);
                _SS_StopSimples.PrecoLoss = Convert.ToString(_TOrder.preco_loss);
                _SS_StopSimples.PrecoGain = Convert.ToString(_TOrder.preco_gain);
                _SS_StopSimples.PrecoStart = Convert.ToString(_TOrder.preco_start);

                Contexto.EnviarDados(_SS_StopSimples.getMessageSS());
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao acessar o método ArmarStopLoss. Erro: " + ex.Message);
            }

        }


        public static void CancelarOrdemStop(int id_stopstart)
        {       
            try
            {
                CS_CancelamentoStop _CS_CancelamentoStop = new CS_CancelamentoStop();

                _CS_CancelamentoStop.IdStopStart = id_stopstart.ToString();
                Contexto.EnviarDados(_CS_CancelamentoStop.getMessageCS());
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao acessar o método CancelarOrdem. Erro: " + ex.Message);
            }

        }

    }
}
