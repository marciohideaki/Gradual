using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickFix;
using QuickFix.Fields;
namespace Gradual.Core.OMS.FixServerLowLatency.Dados
{
    /// <summary>
    /// Contains all TOs classes used in service
    /// TOMessage: message reference;
    /// TOMessageBackup: TO to backup information to persistence file;
    /// TOOrderSession: TO to control session id, order key and expiration control
    /// </summary>
    
    
    public class TOMessage
    {
        public Message MensagemQF{get;set;}
        public SessionID Sessao { get; set; }
        
        public TOMessage()
        {
            this.MensagemQF = null;
            this.Sessao = null;
        }

        ~TOMessage()
        {
            this.MensagemQF.Clear();
            this.MensagemQF = null;
            this.Sessao = null;
        }
    }

    [Serializable]
    public class PartyIDBackup
    {
        public string PartyID{get;set;}
        public char PartyIDSource{get;set;}
        public int PartyRole{get;set;}
    }

    [Serializable]
    public class TOMessageBackup
    {

        public string Key { get; set; }
        public string BeginString { get; set; }
        public string TargetCompID { get; set; }
        public string SenderCompID { get; set; }
        public string TipoExpiracao { get; set; }
        public string DataExpiracao { get; set; }
        public string DataEnvio { get; set; }
        
        public string MsgSeqNum { get; set; }

        public string OrigClOrdID { get; set; }
        public string ClOrdID { get; set; }
        public string Account { get; set; }

        public List<PartyIDBackup> PartyIDs;

        public TOMessageBackup()
        {
            this.Key = string.Empty;
            this.BeginString = string.Empty;
            this.TargetCompID = string.Empty;
            this.SenderCompID = string.Empty;
            this.TipoExpiracao = string.Empty;
            this.DataExpiracao = string.Empty;
            this.DataEnvio = string.Empty;
            
            this.MsgSeqNum = string.Empty;

            this.OrigClOrdID = string.Empty;
            this.ClOrdID = string.Empty;
            this.Account = string.Empty;
            this.PartyIDs = new List<PartyIDBackup>();
        }
    }

    public class TOOrderSession
    {
        public SessionID Sessao { get; set;}
        public string TipoExpiracao { get; set;}
        public string DataExpiracao { get; set;}
        public string DataEnvio { get; set; }

        public int MsgSeqNum { get; set; }

        public string OrigClOrdID { get; set; }
        public string ClOrdID { get; set; }
        public int Account { get; set; }
        public List<Group> PartyIDs;
        public TOOrderSession()
        {
            this.Sessao = null;
            this.TipoExpiracao = string.Empty;
            this.DataExpiracao = string.Empty;
            this.DataEnvio = string.Empty;
            // Used to find Business Reject and Reject messages
            this.MsgSeqNum = -1;
            // Used to find OrderCancel Reject messages
            this.OrigClOrdID = string.Empty;
            this.ClOrdID = string.Empty;
            this.Account = -1;

            // PartyIDs
            this.PartyIDs = new List<Group>();

        }
        ~TOOrderSession()
        {
            this.Sessao = null;
            int len = this.PartyIDs.Count;
            for (int i = 0; i < len; i++)
            {
                this.PartyIDs[i] = null;
            }
            this.PartyIDs.Clear();
            this.PartyIDs = null;

        }
    }
    
     
}
