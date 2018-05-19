using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;


namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto
{
    public static class Formatador
    {
        
        public static ArmarStopSimplesResponse ArmarStopSimples(ArmarStopSimplesRequest req)
        {
            ArmarStopSimplesResponse resp = new ArmarStopSimplesResponse();
            
            try
            {

                SS_StopSimples _SS_StopSimples = new SS_StopSimples("BV");

                _SS_StopSimples.CodigoInstrumento = req.Instrumento;
                _SS_StopSimples.IdStopStart = req.IdStopStart;
                _SS_StopSimples.IdTipoOrdem = Convert.ToString(req.StopStartTipo);
                _SS_StopSimples.PrecoGain = Convert.ToString(req.PrecoGain);
                _SS_StopSimples.PrecoLoss = Convert.ToString(req.PrecoLoss);
                _SS_StopSimples.PrecoStart = Convert.ToString(req.PrecoStart);
                _SS_StopSimples.InicioMovel = Convert.ToString(req.InicioMovel);
                _SS_StopSimples.AjusteMovel = Convert.ToString(req.AjusteMovel);
                resp.StatusResposta = MensagemResponseStatusEnum.OK;
                Contexto.EnviarDados(_SS_StopSimples.getMessageSS());
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, req);
                resp.DescricaoResposta = ex.Message;
                resp.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return resp;
        }

        public static void CancelarOrdemStop(string Instrument ,string id_stopstart)
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
                Log.EfetuarLog(ex, string.Format("{0} - {1}, {2}", ex.Message, Instrument, id_stopstart));
            }
        }
    }
}
