using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cortex.OMS.ServidorFIXAdm.Lib.Dados;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;
using System.Configuration;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Cortex.OMS.ServidorFIX
{
    public class PersistenciaEstado
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SaveOrderSessions - Salvar a tabela de ordem x sessao fix 
        /// </summary>
        /// <param name="lista"></param>
        public static void SaveOrderSessions(Dictionary<string, OrderSessionInfo> lista)
        {
            Stream stream = null;

            try
            {
                string path = ConfigurationManager.AppSettings["OrderSessionsFile"].ToString();
                stream = File.Open(path, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();

                logger.Info("Salvando " + lista.Count + " ordens x sessoes");

                long qtdeitens = lista.Count;
                bformatter.Serialize(stream, qtdeitens);

                foreach (OrderSessionInfo order in lista.Values)
                {
                    bformatter.Serialize(stream, order.ClOrderID);
                    bformatter.Serialize(stream, order.OrdStatus);
                    bformatter.Serialize(stream, order.SenderCompID);
                    bformatter.Serialize(stream, order.TargetCompID);
                    bformatter.Serialize(stream, order.SessionID.BeginString);
                }
                stream.Flush();
            }
            catch (Exception ex)
            {
                logger.Error("SaveOrderSessions: " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }

            logger.Info("Ordens x sessoes gravados com sucesso");
        }


        public static Dictionary<string, OrderSessionInfo> LoadOrderSessions()
        {
            Stream stream = null;
            Dictionary<string, OrderSessionInfo> lista = new Dictionary<string, OrderSessionInfo>();
            try
            {
                logger.Info("Restaurando tabela ordem x sessoes ");
                string path = ConfigurationManager.AppSettings["OrderSessionsFile"].ToString();

                stream = File.Open(path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                long qtdeitens = (long)bformatter.Deserialize(stream);

                logger.Info("Restaurando " + qtdeitens + " itens");

                for (int i = 0; i < qtdeitens; i++)
                {
                    OrderSessionInfo info =  new OrderSessionInfo();

                    info.ClOrderID = (string) bformatter.Deserialize(stream);
                    info.OrdStatus = (OrdemStatusEnum)bformatter.Deserialize(stream);
                    info.SenderCompID = (string) bformatter.Deserialize(stream);
                    info.TargetCompID = (string) bformatter.Deserialize(stream);

                    string beginstr = (string)bformatter.Deserialize(stream);
                    info.SessionID = new QuickFix.SessionID(beginstr, info.SenderCompID, info.TargetCompID);

                    lista.Add(info.ClOrderID, info);
                }

                stream.Close();
                stream = null;

                logger.Info("Itens restaurados");
            }
            catch (Exception ex)
            {
                logger.Error("LoadOrderSessions: " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return lista;
        }

    }
}
