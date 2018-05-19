using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.AutomacaoDesktop
{
    /// <summary>
    /// Estrutura para conter informações de um parte envolvida no processo de negociação.
    /// Utilizado para montagem das mensagens fix.
    /// </summary>
    [Serializable]
    public class PartyInfo
    {
        public string PartyID { get; set; }
        public string PartyIDSource { get; set; }
        public int PartyRole { get; set; }
    }

    /// <summary>
    /// Estrutura de configuracao do canal de negociacao
    /// </summary>
    [Serializable]
    public class FixConfig
    {
        /// <summary>
        /// FIX Begin String
        /// </summary>
        public string BeginString { get; set; }

        /// <summary>
        /// FIX SenderCompID
        /// </summary>
        public string SenderCompID { get; set; }

        /// <summary>
        /// FIX TargetCompID
        /// </summary>
        public string TargetCompID { get; set; }

        public string PartyID { get; set; }
        public string PartyIDSource { get; set; }

        public int PartyRole { get; set; }
        public string SecurityIDSource { get; set; }


        public string LogonPassword { get; set; }

        /// <summary>
        /// Intervalo de Hearbeat em segundos
        /// </summary>
        public int HeartBtInt { get; set; }

        public List<PartyInfo> Parties { get; set; }

        /// <summary>
        /// Flag - indica se deve resetar numero de sequencia ao conectar
        /// </summary>
        public bool ResetSeqNum { get; set; }

        /// <summary>
        /// Flag - indica se deve persistir as mensagens recebidas
        /// </summary>
        public bool PersistMessages { get; set; }

        /// <summary>
        /// Numero da porta TCP para receber a conexao (se Initiator==false)
        /// </summary>
        public int SocketAcceptPort { get; set; }

        /// <summary>
        /// Numero da porta TCP a conectar no host definido em Host (se Initiator==true)
        /// </summary>
        public int SocketConnectPort { get; set; }

        /// <summary>
        /// Intervalo de tentativas de reconexao em segundos
        /// </summary>
        public int ReconnectInterval { get; set; }


        /// <summary>
        /// Endereco IP ou Hostname para conexao (se Initiator==true)
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Diretorio de gravacao da persistencia das mensagens
        /// </summary>
        public string FileStorePath { get; set; }

        /// <summary>
        /// Diretorio de gravacao do log das mensagens
        /// </summary>
        public string FileLogPath { get; set; }

        /// <summary>
        /// Horario de inicio das tentativas de conexao e login (HH:MM:SS)
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Horario de termino das tentativas de conexao e login (HH:MM:SS)
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Path completo do dicionario de dados a ser utilizado (FIX.n.n.xml)
        /// </summary>
        public string DataDictionary { get; set; }

        /// <summary>
        /// Initiator - booleano, indica se inicia (true) ou recebe a conexao
        /// </summary>
        public bool Initiator { get; set; }

        /// <summary>
        /// RawData - senha para BMF
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string RawData { get; set; }

        /// <summary>
        /// Nova senha (informar apenas se for trocar)
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string NewPassword { get; set; }

        /// <summary>
        /// Regular expression que define os papeis que serao tratados
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string FiltroListaInstrumentos { get; set; }

        /// <summary>
        /// Request ID ?
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string MdReqID { get; set; }



        public FixConfig()
        {
            this.Parties = new List<PartyInfo>();
            this.HeartBtInt = 30;
            this.Initiator = true;
            this.ReconnectInterval = 300;
            this.SocketAcceptPort = 0;
            this.StartTime = "00:00:00";
            this.EndTime = "23:59:59";
            this.DataDictionary = "FIX4.2.XML";
            this.PersistMessages = true;
            this.ResetSeqNum = false;
        }
    }
}
