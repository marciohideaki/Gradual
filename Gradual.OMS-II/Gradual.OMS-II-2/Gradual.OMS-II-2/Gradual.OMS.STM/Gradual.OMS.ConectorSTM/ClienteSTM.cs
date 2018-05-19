using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading;
using System.Configuration;
using Gradual.OMS.ConectorSTM.Eventos;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Gradual.OMS.ConectorSTM
{
    public class ClienteSTM
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread _me;
        private bool _bKeepRunning;
        //private string ultimoMsgId = "20110602A0000007676";
        private string ultimoMsgId = " ";
        private STMCom.TSTMCom stmclient;

        public const int TAM_CABECALHO_STM = 6;
        public const int MSG_MEGA_FUNCTIONCODE_OFFSET = 16;
        public const int TAM_MSG_MEGA_FUNCTIONCODE = 4;


        #region STMCom Event Handlers
        void stmclient_OnStarted()
        {
            logger.Info("OnStarted(): ");

        }

        void stmclient_OnReceiveData(string LastMsgId, string SPF_Header, string DataPtr, int DataSize)
        {
            logger.Debug("stmclient_OnReceiveData():");

            string cabecalho;
            string corpo;
            string tipo;
            string instrumento;
            long antesSendEvent;
            long depoisSendEvent;

            ultimoMsgId = LastMsgId;
            try
            {
                if (DataPtr.Length < TAM_CABECALHO_STM)
                {
                    logger.Error("Tamanho da mensagem invalida [" + DataPtr + "]");
                }

                antesSendEvent = DateTime.Now.Ticks;
                cabecalho = DataPtr.Substring(0, TAM_CABECALHO_STM);
                tipo = EventoSTM.TIPO_MSG_CBLC;

                if (this.isMsgCBLC(cabecalho))
                {
                    corpo = DataPtr.Substring(TAM_CABECALHO_STM);
                }
                else
                {
                    cabecalho = DataPtr.Substring(MSG_MEGA_FUNCTIONCODE_OFFSET, TAM_MSG_MEGA_FUNCTIONCODE);
                    corpo = DataPtr.ToString();
                    tipo = EventoSTM.TIPO_MSG_MEGA;
                }

                EventoSTM evento = new EventoSTM(tipo, cabecalho, corpo, ultimoMsgId);
                ServicoConectorSTM.epService.EPRuntime.SendEvent(evento);
                depoisSendEvent = DateTime.Now.Ticks;

                TimeSpan duracaoSendEvent = new TimeSpan(depoisSendEvent - antesSendEvent);
                logger.Debug("OnReceiveData(): msg [" + ultimoMsgId + "] [" + tipo + ":" + cabecalho + "] processado em " + duracaoSendEvent.TotalMilliseconds + " ms");

                stmclient.Ack = true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em OnDataReceived():" + ex.Message);
                logger.Error(ex);
            }
        }

        void stmclient_OnError(int error, string msg, string description)
        {
            logger.ErrorFormat("stmclient_OnError({0},{1},{2})", error, msg, description);

            string body = "";
            string subject = System.Environment.MachineName + " STM: Desconectado da bolsa";
            body += System.Environment.MachineName + " Conector STM: ";
            body += "\r\n";
            body += "Desconectado da  BOLSA: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            body += "\r\n";
            body += "Erro: " + error.ToString() + "\r\n";
            body += "Msg : " + msg + "\r\n";
            body += "Desc: " + description + "\r\n";

            Gradual.OMS.Library.Utilities.EnviarEmail(subject, body);

            stmclient.Disconnect();

            stmclient = null;
        }

        void stmclient_OnDisconnect(string Desc)
        {
            logger.Info("OnDisconnect(): " + Desc);

            string body = "";
            string subject = System.Environment.MachineName + " STM: Desconectado da bolsa";
            body += System.Environment.MachineName + " Conector STM: ";
            body += "\r\n";
            body += "Desconectado da  BOLSA: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            body += "\r\n";
            body += "Desc: " + Desc + "\r\n";

            Gradual.OMS.Library.Utilities.EnviarEmail(subject, body);

            stmclient = null;
        }

        void stmclient_OnConnect()
        {
            logger.Info("OnConnect(): ");

            stmclient.StartRequest(this.ultimoMsgId);
        }

        void stmclient_OnAcceptStart()
        {
            logger.Info("OnAcceptStart(): ");
        }

        #endregion //STMCom Event Handlers

        public void Start()
        {
            _loadUltimoMsgID();

            _bKeepRunning = true;
            _me = new Thread(new ThreadStart(Run));
            _me.Start();
        }

        public void Stop()
        {
            _saveUltimoMsgID();

            _bKeepRunning = false;
            while (_me != null && _me.IsAlive)
            {
                Thread.Sleep(250);
            }
        }

        public void Run()
        {
            try
            {
                int lastTrial = 600;

                logger.Info("Iniciando conexao com STM");

                byte[] senhaDecoded = Convert.FromBase64String(ConfigurationManager.AppSettings["STMPasswd"].ToString());
                byte[] userDecoded = Convert.FromBase64String(ConfigurationManager.AppSettings["STMUser"].ToString());

                while (_bKeepRunning)
                {
                    if (stmclient == null && lastTrial > 600)
                    {
                        logger.Info("Tentando conectar com STM");

                        stmclient = new STMCom.TSTMCom();

                        stmclient.Cas_Funcionario = Encoding.UTF8.GetString(userDecoded); 
                        stmclient.Cas_Usuario = "227";
                        stmclient.Cas_Senha = Encoding.UTF8.GetString(senhaDecoded);
                        stmclient.Compressed = true;
                        //stmclient.Filter = "<AN  >+;<ANF >+;";
                        //                     0123456
                        stmclient.Ack = true;

                        // Caso efetue troca de senha
                        if (ConfigurationManager.AppSettings["STMNewPasswd"] != null)
                        {
                            string novasenha = ConfigurationManager.AppSettings["STMNewPasswd"].ToString();
                            byte[] novasenhaDecoded = Convert.FromBase64String(novasenha);


                            logger.Info("Efetuando troca de senha [" + Encoding.UTF8.GetString(senhaDecoded) +
                                "] -> [" + Encoding.UTF8.GetString(novasenhaDecoded) + "]");


                            stmclient.Cas_NovaSenha = Encoding.UTF8.GetString(novasenhaDecoded);

                            // Remove a nova senha, no arquivo de configuracao inclusive
                            Configuration stmconfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                            stmconfig.AppSettings.Settings.Remove("STMNewPasswd");
                            stmconfig.AppSettings.Settings.Remove("STMPasswd");
                            stmconfig.AppSettings.Settings.Add("STMPasswd", novasenha);

                            stmconfig.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                        }

                        stmclient.OnAcceptStart += new STMCom.ISTMComEvents_OnAcceptStartEventHandler(stmclient_OnAcceptStart);
                        stmclient.OnConnect += new STMCom.ISTMComEvents_OnConnectEventHandler(stmclient_OnConnect);
                        stmclient.OnDisconnect += new STMCom.ISTMComEvents_OnDisconnectEventHandler(stmclient_OnDisconnect);
                        stmclient.OnError += new STMCom.ISTMComEvents_OnErrorEventHandler(stmclient_OnError);
                        stmclient.OnStarted += new STMCom.ISTMComEvents_OnStartedEventHandler(stmclient_OnStarted);
                        stmclient.OnReceiveData += new STMCom.ISTMComEvents_OnReceiveDataEventHandler(stmclient_OnReceiveData);

                        logger.Info("Chamando connect()");

                        bool bConnect = stmclient.Connect();

                        if (!bConnect)
                        {
                            logger.Error("Connect voltou false");
                            stmclient = null;
                        }

                        lastTrial = 0;
                    }
                    else
                        lastTrial++;

                    Thread.Sleep(100);
                }

                if ( stmclient != null )
                    stmclient.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error("Run(): " + ex.Message, ex);
            }
        }


        private bool isMsgCBLC( string cabecalho )
        {
            if (cabecalho.Substring(0, 1).Equals("<") && cabecalho.Substring(5, 1).Equals(">"))
                return true;

            return false;
        }

        private void _saveUltimoMsgID()
        {
            Stream stream = null;
            string path = ConfigurationManager.AppSettings["MSGIDFile"].ToString();

            try
            {
                logger.Info("Salvando ultimo MSGID processado: " + this.ultimoMsgId);

                stream = File.Open(path, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, ultimoMsgId);

                stream.Close();
                stream = null;
            }
            catch (Exception ex)
            {
                logger.Error("_saveUltimoMsgID(): " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

        }


        private bool _loadUltimoMsgID()
        {
            Stream stream = null;
            string path = ConfigurationManager.AppSettings["MSGIDFile"].ToString();

            try
            {

                stream = File.Open(path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                ultimoMsgId = (string) bformatter.Deserialize(stream);

                logger.Info("Carregando ultimo MSGID processado: " + this.ultimoMsgId);

                stream.Close();
                stream = null;

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("_loadUltimoMsgID(): " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            ultimoMsgId = " ";
            return false;
        }
    }
}
