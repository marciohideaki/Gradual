using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Gradual.Core.OMS.FixServerLowLatency.Dados
{

    /// <summary>
    /// Fix Config classes
    /// </summary>
    
    [Serializable]
    public class FixConfigInfo
    {
        

        public FixConfigInfo()
        {

        }
    }

    [Serializable]
    public class FixConfig
    {
        public string SenderLocationID { get; set; }
        public string PartyID { get; set; }
        public string PartyIDSource { get; set; }
        public int PartyRole { get; set; }
        public string SecurityIDSource { get; set; }
        public int HeartBtInt { get; set; }
        
        public int SocketAcceptPort { get; set; }
        public int ReconnectInterval { get; set; }
        public string FileStorePath { get; set; }
        public string FileLogPath { get; set; }
        public string DebugFileLogPath { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ConnectionType { get; set; }

        public FixConfig()
        {
        
        }
    }


}
