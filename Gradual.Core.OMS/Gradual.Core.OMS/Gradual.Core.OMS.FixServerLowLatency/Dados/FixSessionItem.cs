using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Core.OMS.FixServerLowLatency.Dados
{
    public class PartyIDItem
    {
        public int IdPartyID { get; set; }
        public int IdSessaoFIx { get; set; }
        public string PartyID { get; set; }
        public char PartyIDSource { get; set; }
        public int PartyRole { get; set; }
    }
    public class FixSessionItem
    {

        public int IdSessaoFix { get; set; }
        public int IdCliente { get; set; }
        public string Mnemonico { get; set; }
        public string Bolsa { get; set; }
        public int Operador { get; set; }
        public string BeginString { get; set; }
        public string SenderCompID { get; set; }
        public string SenderLocationID { get; set; }
        public string TargetCompID { get; set; }
        //public string PartyID { get; set; }
        //public string PartyIDSource { get; set; }
        //public int PartyRole { get; set; }
        public string SecurityIDSource { get; set; }
        public string LogonPassword { get; set; }
        public string NewPassword { get; set; }
        public int HeartBtInt { get; set; }
        public bool ResetSeqNum { get; set; }
        public bool PersistMessages { get; set; }
        public int SocketPort { get; set; }
        public int ReconnectInterval { get; set; }
        /// <summary>
        /// CancelOnDisconnect - define se as ordens devem ser automaticamente em caso de queda
        /// de conexao ou logout
        /// 0 - Do Not Cancel On Disconnect Or Logout
        /// 1 - Cancel On Disconnect Only
        /// 2 - Cancel On Logout Only
        /// 3 - Cancel On Disconnect Or Logout
        /// </summary>
        public int CancelOnDisconnect { get; set; }
        /// <summary>
        /// CODTimeout - tempo em segundos para reconectar antes que ordens sejam automaticamente canceladas
        /// max=60;
        /// </summary>
        public int CODTimeout { get; set; }
        public string Host { get; set; }
        public string FileStorePath { get; set; }
        public string FileLogPath { get; set; }
        public string DebugFileLogPath { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool UseDataDictionary {get;set;}
        public string DataDictionary { get; set; }
        public string ConnectionType { get; set; }
        public bool FinancialLimit { get; set; }

        public List<PartyIDItem> PartyIDs;

        public FixSessionItem()
        {
            this.PartyIDs = new List<PartyIDItem>();
        }

    }
}
