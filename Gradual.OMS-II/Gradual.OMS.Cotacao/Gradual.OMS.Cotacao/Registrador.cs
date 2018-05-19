using System;
using System.Collections;
using System.Collections.Generic;

namespace Gradual.OMS.Cotacao
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

        public static object SendMDSEventFactory(object key, object value)
        {
            lock (_arrListener)
            {
                for (int i = 0; i <= _arrListener.Count - 1; i++){
                    List<string> List = (List<string>)
                        (_hsEvents[_arrListener[i]]);

                    if (List != null)
                    {
                        if (List.Contains(key.ToString().Trim()))
                        {
                          

                        }
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
