using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net.Sockets;
using AS.Sockets;
using System.Configuration;

namespace AuthenticationServerHoster
{
    public partial class ASHoster : ServiceBase
    {
        public ASHoster()
        {
            InitializeComponent();
        }

        internal SocketPackage SockPkg = new SocketPackage();

        protected override void OnStart(string[] args)
        {
            try{        
                int PortNumber = int.Parse (ConfigurationManager.AppSettings["ASPortLissen"].ToString());             
                SockPkg.StartListen(PortNumber);
                EventLog.WriteEntry("Servidor inicializado com sucesso.", EventLogEntryType.Information);

            }
            catch (SocketException se){
                EventLog.WriteEntry(string.Format("Código: {0} - MsgErro: {1} ", se.ErrorCode.ToString(), se.Message), EventLogEntryType.Error);
            }
            catch (Exception ex){
                EventLog.WriteEntry(string.Format("Source: {0} - MsgErro: {1} ", ex.Source.ToString(), ex.Message), EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            try{
                SockPkg.StopListen();
                EventLog.WriteEntry("Serviço parado com sucesso", EventLogEntryType.Information);
            }
            catch (SocketException se){
                EventLog.WriteEntry(string.Format("Código: {0} - MsgErro: {1} ", se.ErrorCode.ToString(), se.Message), EventLogEntryType.Error);
            }
            catch (Exception ex){
                EventLog.WriteEntry(string.Format("Source: {0} - MsgErro: {1} ", ex.Source.ToString(), ex.Message), EventLogEntryType.Error);
            }
        }
    }
}
