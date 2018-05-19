using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using QuickFix;
using QuickFix.Fields;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;
using System.Globalization;
using Gradual.Core.OMS.FixServerLowLatency.Database;
using Gradual.Core.OMS.DropCopy.Lib.Dados;

namespace Gradual.Core.OMS.FixServerLowLatency.Memory
{
    public class OrderSessionManager
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region Private Variables
        Dictionary <string, TOOrderSession> _dicMsgsCl;
        Dictionary<string, TOOrderSession> _dicMsgsOrd;
        bool _serializing;
        #endregion

        public QuickFix.DataDictionary.DataDictionary Dict { get; set; }
        public OrderSessionManager()
        {
            _dicMsgsCl = new Dictionary<string, TOOrderSession>();
            _dicMsgsOrd = new Dictionary<string, TOOrderSession>();
            _serializing = false;
        }

        ~OrderSessionManager()
        {
            this.Clear();
        }

        public void Clear()
        {
            try
            {
                if (null != _dicMsgsCl)
                {
                    lock (_dicMsgsCl)
                    {
                        _dicMsgsCl.Clear();
                        _dicMsgsCl = null;
                    }
                }

                if (null != _dicMsgsOrd)
                {
                    lock (_dicMsgsOrd)
                    {
                        _dicMsgsOrd.Clear();
                        _dicMsgsOrd = null;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro clear OrderSessionManager: " + ex.Message, ex);
            }
        }

        public Dictionary<string, TOOrderSession> GetDictionary(int op)
        {
            Dictionary<string, TOOrderSession> ret = null;
            switch (op)
            {
                case 1:
                    ret = _dicMsgsCl; break;
                case 2:
                    ret = _dicMsgsOrd; break;

            }
            return ret;
        }

        #region Management Functions
        

        // 1: return by chave
        // 2: return by chaveExchNumber
        public int GetOrder(string chave, out TOOrderSession to, string chaveExchNumber = "", int keyType = 1, string exchange = "")
        {
            try
            {
                //TOOrderSession ord = null;
                lock (_dicMsgsCl)
                {
                    if (_dicMsgsCl.TryGetValue(chave, out to))
                        return FindType.CLORDID;
                    else
                    {
                        //if (string.IsNullOrEmpty(chaveExchNumber))
                        //    return FindType.CLORDID;
                        
                        {
                            //KeyValuePair<string, TOOrderSession> item = new KeyValuePair<string, TOOrderSession>();
                            List<TOOrderSession> lst = _dicMsgsCl.Select(x => x.Value).Where(x => x.ExchangeNumberID == chaveExchNumber).ToList();

                            TOOrderSession item = null;
                            if (KeyType.CLORDID == keyType)
                                item = lst.OrderByDescending(x => x.ExchangeSeqNum).FirstOrDefault();
                            else
                                item = lst.OrderByDescending(x => x.ExchangeSeqNum).LastOrDefault();
                            
                            to = item;
                            if (null != to)
                            {
                                return FindType.EXCHANGE_NUMBER;
                            }
                            // Nao achou a ordem. Tentará busca a partir do banco de dados. //TODO[FF]: Fazer a busca e remontar o TO com as informacoes existentes
                            else
                            {

                                string[] arr = chave.Split('-');
                                string clord;
                                string strAcc = string.Empty;
                                int account;
                                string symbol;
                                if (arr.Length != 3)
                                    return FindType.UNDEFINED;
                                clord = arr[0];
                                if (exchange.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                                    strAcc = arr[1].Substring(0, arr[1].Length - 1);
                                else
                                    strAcc = arr[1];
                                account = Convert.ToInt32(strAcc);
                                symbol = arr[2];
                                DbFix db = new DbFix();
                                OrderDbInfo retOrd = db.BuscarOrdem(clord, account, symbol);
                                if (0 == retOrd.OrderID)
                                {
                                    to = null;
                                    return FindType.UNDEFINED;
                                }
                                else
                                {
                                    TOOrderSession toAux = this.OrderDbInfo2OrderSession(retOrd, chave);
                                    to = toAux;
                                    return FindType.DB;
                                }
                                
                            }
                        }
                    }
                }
                to = null;
                return FindType.UNDEFINED;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao buscar o TOOrderSession: " + ex.Message, ex);
                to = null;
                return FindType.UNDEFINED;
            }
        }

        public KeyValuePair<string, TOOrderSession> GetOrderBySeqNum(int seqnum)
        {
            KeyValuePair<string, TOOrderSession> item = new KeyValuePair<string, TOOrderSession>();
            try
            {
                lock (_dicMsgsCl)
                {
                    item = (KeyValuePair<string, TOOrderSession>)_dicMsgsCl.FirstOrDefault(x => x.Value.MsgSeqNum == seqnum);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao buscar o TOOrderSession por SeqNum: " + ex.Message, ex);
            }
            return item;
        }

        public KeyValuePair<string, TOOrderSession> GetOrderByClAndOrigCl(string cl, string origcl)
        {
            KeyValuePair<string, TOOrderSession> item = new KeyValuePair<string, TOOrderSession>();
            try
            {
                lock (_dicMsgsCl)
                {
                    item = (KeyValuePair<string, TOOrderSession>)_dicMsgsCl.FirstOrDefault(x => x.Value.ClOrdID == cl &&
                                                                                             x.Value.OrigClOrdID == origcl);  
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao buscar o TOOrderSession por Clr and OrigCl: " + ex.Message, ex);
            }
            return item;
        }

        public KeyValuePair<string, TOOrderSession> GetOrderByExchangeNumber(string chave)
        {
            KeyValuePair<string, TOOrderSession> item = new KeyValuePair<string, TOOrderSession>();
            try
            {
                lock (_dicMsgsCl)
                {
                    item = (KeyValuePair<string, TOOrderSession>)_dicMsgsCl.FirstOrDefault(x => x.Value.ExchangeNumberID == chave);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao buscar o TOOrderSession por ExchangeNumber: " + ex.Message, ex);
            }
            return item;
        }

        public bool AddOrder(string chave, TOOrderSession item)
        {
            try
            {
                lock (_dicMsgsCl)
                {
                    _dicMsgsCl.Add(chave, item);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao adicionar o TOOrderSession: " + ex.Message, ex);
                return false;
            }
        }

        public bool RemoveOrder(string chave)
        {
            try
            {
                bool ret = false;
                lock (_dicMsgsCl)
                {
                    ret = _dicMsgsCl.Remove(chave);
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao remover o TOOrderSession: " + ex.Message, ex);
                return false;
            }
        }
        #endregion

        #region "Backup Management"
        /*
        public void SaveSessionIDS(string filename)
        {
            FileStream fs = File.Open(filename, FileMode.Create, FileAccess.Write);
            try
            {
                // Serialize Dictionary to dat file
                List<TOMessageBackup> lst = new List<TOMessageBackup>();
                if (!_serializing)
                {
                    _serializing = true;
                    Dictionary<string, TOOrderSession> regs = new Dictionary<string, TOOrderSession>(_dicMsgsCl);

                    
                    foreach (KeyValuePair<string, TOOrderSession> item in regs)
                    {
                        TOMessageBackup to = new TOMessageBackup();

                        to.Key = item.Key;
                        to.BeginString = item.Value.Sessao.BeginString;
                        to.SenderCompID = item.Value.Sessao.SenderCompID;
                        to.SenderSubID = item.Value.Sessao.SenderSubID;
                        to.TargetCompID = item.Value.Sessao.TargetCompID;
                        to.TargetSubID = item.Value.Sessao.TargetSubID;
                        to.TipoExpiracao = item.Value.TipoExpiracao;
                        to.DataExpiracao = item.Value.DataExpiracao;
                        to.DataEnvio = item.Value.DataEnvio;
                        to.MsgSeqNum = item.Value.MsgSeqNum.ToString();
                        to.ClOrdID = item.Value.ClOrdID;
                        to.OrigClOrdID = item.Value.OrigClOrdID;
                        to.Account = item.Value.Account.ToString();
                        int lenPid = item.Value.PartyIDs.Count;
                        for (int i = 0; i < lenPid; i++)
                        {
                            PartyIDBackup pId = new PartyIDBackup();
                            pId.PartyID = item.Value.PartyIDs[i].GetField(Tags.PartyID);
                            pId.PartyIDSource = item.Value.PartyIDs[i].GetChar(Tags.PartyIDSource);
                            pId.PartyRole = item.Value.PartyIDs[i].GetInt(Tags.PartyRole);
                            to.PartyIDs.Add(pId);
                        }
                        // to.MensagemQF = item.Value.MensagemQF.ToString();
                        to.TipoLimite = (int)item.Value.TipoLimite;
                        to.Order = item.Value.Order;
                        to.MensagemQF = item.Value.MensagemQF.ToString();
                        to.ExchangeNumberID = item.Value.ExchangeNumberID;
                        to.ExchangeSeqNum = item.Value.ExchangeSeqNum;
                        //to.SecondaryOrderID = item.Value.SecondaryOrderID;
                        //to.TradeDate = item.Value.TradeDate;
                        lst.Add(to);
                        to = null;
                    }

                    BinaryFormatter bs = new BinaryFormatter();
                    bs.Serialize(fs, lst);
                    bs = null;
                    logger.InfoFormat("SaveSessionIDS(): Registros serializados: [{0}] [{1}]", lst.Count, filename);
                    // Efetuar limpeza da lista
                    int len = lst.Count;
                    for (int i = 0; i < len; i++)
                    {
                        TOMessageBackup aux = lst[i];
                        aux = null;
                    }

                    lst.Clear();
                    lst = null;
                    fs.Close();
                    fs = null;
                    regs.Clear();
                    regs = null;
                    _serializing = false;
                }
                else
                {
                    if (_serializing)
                        logger.Debug("SaveSessionIDS(): Processo de serializacao já em execucao!!!");
                }
            }
            catch (Exception ex)
            {
                logger.Error("SaveSessionIDS(): Erro na serializacao dos registros do dicionario: " + ex.Message, ex);
                _serializing = false; // mudar para false para tentar backupear no proximo "ciclo"
                fs.Close();
                fs = null;
            }
        }
        */
        /*
        public void LoadSessionIDs(string fileName, QuickFix.DataDictionary.DataDictionary dataDic)
        {
            string msgQF = string.Empty;
            try
            {
                if (File.Exists(fileName))
                {
                    List<TOMessageBackup> lst = new List<TOMessageBackup>();
                    FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    lst = (List<TOMessageBackup>)bformatter.Deserialize(fs);
                    int length = lst.Count;
                    if (lst.Count > 0)
                    {
                        lock (_dicMsgsCl)
                        {
                            for (int i = 0; i < length; i++)
                            {
                                TOMessageBackup to = lst[i];
                                TOOrderSession toOrder = new TOOrderSession();
                                SessionID ssID = null;
                                if (!string.IsNullOrEmpty(to.SenderSubID) && (!string.IsNullOrEmpty(to.TargetSubID)))
                                    ssID = new SessionID(to.BeginString, to.SenderCompID, to.SenderSubID, to.TargetCompID, to.TargetSubID);
                                else
                                    ssID = new SessionID(to.BeginString, to.SenderCompID, to.TargetCompID);
                                toOrder.Sessao = ssID;
                                toOrder.TipoExpiracao = to.TipoExpiracao;
                                toOrder.DataExpiracao = to.DataExpiracao;
                                toOrder.DataEnvio = to.DataEnvio;
                                toOrder.MsgSeqNum = Convert.ToInt32(to.MsgSeqNum);
                                toOrder.ClOrdID = to.ClOrdID;
                                toOrder.OrigClOrdID = to.OrigClOrdID;
                                toOrder.Account = Convert.ToInt32(to.Account);
                                int len = to.PartyIDs.Count;
                                for (int j = 0; j < len; j++)
                                {
                                    QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup grp = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                                    grp.Set(new PartyID(to.PartyIDs[j].PartyID));
                                    grp.Set(new PartyIDSource(to.PartyIDs[j].PartyIDSource));
                                    grp.Set(new PartyRole(to.PartyIDs[j].PartyRole));
                                    toOrder.PartyIDs.Add(grp);
                                }

                                toOrder.TipoLimite = (TipoLimiteEnum)to.TipoLimite;
                                toOrder.Order = to.Order;
                                toOrder.MensagemQF = new QuickFix.Message(to.MensagemQF, dataDic, true);
                                toOrder.ExchangeNumberID = to.ExchangeNumberID;
                                toOrder.ExchangeSeqNum = to.ExchangeSeqNum;
                                //toOrder.SecondaryOrderID = to.SecondaryOrderID;
                                //toOrder.TradeDate = to.TradeDate;
                                _dicMsgsCl.Add(to.Key, toOrder);
                            }
                        }
                        logger.Info("LoadSessionIDs(): Registros recuperados: " + lst.Count);
                        lst.Clear();
                        lst = null;
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("LoadSessionIDs(): Erro na deserializacao dos registros do dicionario: MsgQF: " + msgQF + " " + ex.Message, ex);
            }
        }
        */
        public void LoadOrderSessionIDsFromDB(int idSession, QuickFix.DataDictionary.DataDictionary dataDic)
        {
            try
            {
                DbFix dbFix = new DbFix();
                List<TOMessageBackup> lst = dbFix.BuscarOrderSessionIDs(idSession);
                
                int length = lst.Count;
                if (lst.Count > 0)
                {
                    lock (_dicMsgsCl)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            TOMessageBackup to = lst[i];
                            TOOrderSession toOrder = new TOOrderSession();
                            SessionID ssID = null;
                            if (!string.IsNullOrEmpty(to.SenderSubID) && (!string.IsNullOrEmpty(to.TargetSubID)))
                                ssID = new SessionID(to.BeginString, to.SenderCompID, to.SenderSubID, to.TargetCompID, to.TargetSubID);
                            else
                                ssID = new SessionID(to.BeginString, to.SenderCompID, to.TargetCompID);
                            toOrder.Sessao = ssID;
                            toOrder.TipoExpiracao = to.TipoExpiracao;
                            toOrder.DataExpiracao = to.DataExpiracao;
                            toOrder.DataEnvio = to.DataEnvio;
                            toOrder.MsgSeqNum = Convert.ToInt32(to.MsgSeqNum);
                            toOrder.ClOrdID = to.ClOrdID;
                            toOrder.OrigClOrdID = to.OrigClOrdID;
                            toOrder.Account = Convert.ToInt32(to.Account);
                            int len = to.PartyIDs.Count;
                            for (int j = 0; j < len; j++)
                            {
                                QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup grp = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                                grp.Set(new PartyID(to.PartyIDs[j].PartyID));
                                grp.Set(new PartyIDSource(to.PartyIDs[j].PartyIDSource));
                                grp.Set(new PartyRole(to.PartyIDs[j].PartyRole));
                                toOrder.PartyIDs.Add(grp);
                            }

                            toOrder.TipoLimite = (TipoLimiteEnum)to.TipoLimite;
                            toOrder.Order = to.Order;
                            toOrder.MensagemQF = new QuickFix.Message(to.MensagemQF, dataDic, true);
                            toOrder.ExchangeNumberID = to.ExchangeNumberID;
                            toOrder.ExchangeSeqNum = to.ExchangeSeqNum;
                            _dicMsgsCl.Add(to.Key, toOrder);
                        }
                    }
                    logger.Info("LoadOrderSessionIDsFromDB(): Registros recuperados: " + lst.Count);
                    lst.Clear();
                    lst = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("LoadOrderSessionIDsFromDB(): Erro na deserializacao dos registros a partir do banco de dados: " + ex.Message, ex);
            }
        }

        public void SaveOrderSessionIDsToDB(int idSessao)
        {
            try
            {
                
                // Serialize Dictionary to dat file
                if (!_serializing)
                {
                    _serializing = true;
                    Dictionary<string, TOOrderSession> regs = new Dictionary<string, TOOrderSession>(_dicMsgsCl);
                    DbFix dbFix = new DbFix();
                    logger.Info("Inicio da Gravacao");

                    // Efetuar a limpeza para "re-gravar" os registros
                    if (!dbFix.LimparOrderSessionIDs(idSessao))
                    {
                        logger.Info("Problemas na limpeza dos OrderSessionIDs");
                        return;
                    }

                    foreach (KeyValuePair<string, TOOrderSession> item in regs)
                    {
                        TOMessageBackup to = new TOMessageBackup();
                        
                        to.Key = item.Key;
                        to.BeginString = item.Value.Sessao.BeginString;
                        to.SenderCompID = item.Value.Sessao.SenderCompID;
                        to.SenderSubID = item.Value.Sessao.SenderSubID;
                        to.TargetCompID = item.Value.Sessao.TargetCompID;
                        to.TargetSubID = item.Value.Sessao.TargetSubID;
                        to.TipoExpiracao = item.Value.TipoExpiracao;
                        to.DataExpiracao = item.Value.DataExpiracao;
                        to.DataEnvio = item.Value.DataEnvio;
                        to.MsgSeqNum = item.Value.MsgSeqNum.ToString();
                        to.ClOrdID = item.Value.ClOrdID;
                        to.OrigClOrdID = item.Value.OrigClOrdID;
                        to.Account = item.Value.Account.ToString();
                        int lenPid = item.Value.PartyIDs.Count;
                        for (int i = 0; i < lenPid; i++)
                        {
                            PartyIDBackup pId = new PartyIDBackup();
                            pId.PartyID = item.Value.PartyIDs[i].GetField(Tags.PartyID);
                            pId.PartyIDSource = item.Value.PartyIDs[i].GetChar(Tags.PartyIDSource);
                            pId.PartyRole = item.Value.PartyIDs[i].GetInt(Tags.PartyRole);
                            to.PartyIDs.Add(pId);
                        }
                        to.TipoLimite = (int)item.Value.TipoLimite;
                        to.Order = item.Value.Order;
                        to.MensagemQF = item.Value.MensagemQF.ToString();
                        to.ExchangeNumberID = item.Value.ExchangeNumberID;
                        to.ExchangeSeqNum = item.Value.ExchangeSeqNum;
                        dbFix.InserirOrderSessionItem(idSessao, to);
                        to = null;
                    }
                    logger.InfoFormat("Gravando OrderSessions da Sessao: [{0}] Registros: [{1}]", idSessao, regs.Count);
                    //BinaryFormatter bs = new BinaryFormatter();
                    //bs.Serialize(fs, lst);
                    //bs = null;
                    //fs.Close();
                    //fs = null;
                    regs.Clear();
                    regs = null;
                    _serializing = false;
                }
                else
                {
                    if (_serializing)
                        logger.Debug("SaveSessionIDS(): Processo de serializacao já em execucao!!!");
                }
            }
            catch (Exception ex)
            {
                logger.Error("SaveSessionIDS(): Erro na serializacao dos registros do dicionario: " + ex.Message, ex);
                _serializing = false; // mudar para false para tentar backupear no proximo "ciclo"
                //fs.Close();
                //fs = null;
            }
        }

        public void ExpireOrderSessions()
        {
            try
            {
                List<string> lstToPurge = new List<string>();
                Dictionary<string, TOOrderSession> dicAux = _dicMsgsCl;
                lock (dicAux)
                {
                    if (dicAux.Count > 0)
                    {
                        foreach (KeyValuePair<string, TOOrderSession> item in dicAux)
                        {
                            TOOrderSession aux = item.Value;
                            DateTime dtSendingTime = DateTime.MinValue;
                            DateTime dtExpireDate;
                            switch (aux.TipoExpiracao)
                            {
                                // Validar a DateTimeNow com SendingTime
                                case "":  // Day
                                case "0": // Day
                                case "3": // Immediate or Cancel
                                case "4": // Fill Or Kill
                                case "7": // At the close
                                case "A": // Good For Auction
                                    {
                                        // Valida a data de envio para expiracao da ordem
                                        if (string.IsNullOrEmpty(aux.DataEnvio))
                                        {
                                            if (aux.MensagemQF != null)
                                            {
                                                string dtAux = aux.MensagemQF.Header.GetField(Tags.SendingTime);
                                                dtSendingTime = !string.IsNullOrEmpty(dtAux)? 
                                                    DateTime.ParseExact(dtAux, "yyyyMMdd-HH:mm:ss.fff", CultureInfo.InvariantCulture).ToLocalTime() : 
                                                    DateTime.MinValue;
                                            }
                                        }
                                        else
                                        {
                                            dtSendingTime = DateTime.ParseExact(aux.DataEnvio, "yyyyMMdd-HH:mm:ss.fff", CultureInfo.InvariantCulture).ToLocalTime();
                                        }
                                        if (dtSendingTime.Date < DateTime.Now.Date || dtSendingTime == DateTime.MinValue)
                                        {
                                            lstToPurge.Add(item.Key);
                                        }
                                    }
                                    break;
                                case "6": // Good till Date
                                    {

                                        if (!string.IsNullOrEmpty(aux.DataExpiracao))
                                        {
                                            dtExpireDate = DateTime.ParseExact(aux.DataExpiracao, "yyyyMMdd", CultureInfo.InvariantCulture);
                                            if (dtExpireDate.Date < DateTime.Now.Date)
                                                lstToPurge.Add(item.Key);
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    // Delete itens from dictionary
                    foreach (string it in lstToPurge)
                    {
                        TOOrderSession xx = null;
                        dicAux.TryGetValue(it, out xx);
                        xx = null;
                        dicAux.Remove(it);
                        logger.Info("Chave removida: " + it);
                    }
                    lstToPurge.Clear();
                    lstToPurge = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("ExpireOrderSessions(): " + ex.Message, ex);
            }
        }

        private TOOrderSession OrderDbInfo2OrderSession(OrderDbInfo ord, string chave)
        {
            try
            {
                TOOrderSession ret = new TOOrderSession();
                string ss = ord.SessionIDOriginal;
                string []aux = ss.Split( new string[] {":","/", "->"}, StringSplitOptions.RemoveEmptyEntries);

                string beginstr;
                string sender;
                string sendersub = string.Empty;
                string target;
                string targetsub = string.Empty;
                // Se tiver '/', existe sender sub e targetsub
                bool isSub = false;
                if (ss.IndexOf("/") > 0)
                {
                    beginstr = aux[0];
                    sender = aux[1];
                    sendersub = aux[2];
                    target = aux[3];
                    targetsub = aux[4];
                    isSub = true;
                }
                else
                {
                    beginstr = aux[0];
                    sender = aux[1];
                    target = aux[2];
                }
                if (isSub)
                    ret.Sessao = new QuickFix.SessionID(beginstr, sender, sendersub, target, targetsub);
                else
                    ret.Sessao = new QuickFix.SessionID(beginstr, sender, target);
                ret.TipoExpiracao = ord.TimeInForce;
                ret.DataExpiracao = ord.ExpireDate == DateTime.MinValue? "" : ord.ExpireDate.ToString("yyyyMMdd");
                ret.DataEnvio = ord.RegisterTime.ToString("yyyyMMdd-HH:mm:ss.fff");
                ret.MsgSeqNum = ord.FixMsgSeqNum;
                ret.OrigClOrdID = ord.OrigClOrdID;
                ret.ClOrdID = ord.ClOrdID;
                ret.Account = ord.Account;
                ret.ExchangeNumberID = ord.ExchangeNumberID;
                ret.ExchangeSeqNum = 1;
                ret.ChaveDicionario = chave;
                
                // Remontar a mensagem fix
                string msg = ord.MsgFix.Replace('|', '\x01');

                ret.MensagemQF = new QuickFix.Message(msg, this.Dict, true);
                // Remontar party id a partir da mensagem fix
                int lenPID = ret.MensagemQF.IsSetField(Tags.NoPartyIDs) ? ret.MensagemQF.GetInt(Tags.NoPartyIDs) : 0;
                for (int x = 1; x <= lenPID; x += 1)
                {
                    Group noPartyIds = new Group(ret.MensagemQF.GetGroup(x, Tags.NoPartyIDs));
                    ret.PartyIDs.Add(noPartyIds);
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("OrderDbInfo2OrderSession(): " + ex.Message, ex);
                return null;
            }
        }

        #endregion
    }
}
