using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Recebidas;


namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos
{
    public class Registrador
    {
        private static ArrayList _arrListener = new ArrayList();
        private static Hashtable _hsEvents = new Hashtable();

        public static void AddEvent(object sender, List<string> args)
        {
            lock (_hsEvents){
                _hsEvents.Remove(sender);
                _hsEvents.Add(sender, args);
            }          
        }

        public static void RemoveEvent(object sender){
            lock (_hsEvents){
                _hsEvents.Remove(sender);
            }
        }

        public static bool PermissionToRemoveInstrument(object sender, string Instrument)
        {
            for (int i = 0; i <= _hsEvents.Count - 1; i++){
                List<string> lstInstrumentsToCompare = (List<string>)(_hsEvents[_arrListener[i]]);

                for (int j= 0; j <= lstInstrumentsToCompare.Count - 1; j++){

                    if (lstInstrumentsToCompare[j].ToString() == Instrument){
                        return false;
                    }
                }
            }

            return true;
        }

        public static List<string> GetInstruments(object sender){            
            return (List<string>)(_hsEvents[sender]);
        }

        public static object SendMDSEventFactory(object value)
        {
            lock (_arrListener)
            {
                for (int i = 0; i <= _arrListener.Count - 1; i++){
                    if (value.GetType() == typeof(RS_RespostaStop)){
                        ((Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos.MDSEventFactory)(_arrListener[i])).MDSSRespostaAutenticacaoEvent(value);
                    }
                    if (value.GetType() == typeof(SS_StopSimplesResposta)){
                        ((Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos.MDSEventFactory)(_arrListener[i])).MDSStopStartEvent(value);
                    }
                    if (value.GetType() == typeof(CR_CancelamentoStopResposta)){
                        ((Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos.MDSEventFactory)(_arrListener[i])).MDSSRespostaCancelamentoEvent(value);
                    }
                }
            }

            return null;
        }
       

        public static void AddListener(object sender)
        {
            lock (_arrListener){
                _arrListener.Add(sender);
            }               
        }

        public static void RemoveListener(object sender)
        {
            lock (_hsEvents){
                _hsEvents.Remove(sender);
            }

            lock (_arrListener){
                _arrListener.Remove(sender);             
            }
        }
     
    }
}
