using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Tamir.SharpSsh;
using System.Collections;
using System.Threading;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.SFTP
{
    public class SFtpClient
    {
        #region Atributos
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, string> ListaDir = new Dictionary<string,string>();
        #endregion

        private string _secureUser;
        private string _securePasswd;
        private string _secureHost;
        private int _securePort=22;

        #region Propriedades
        public string SFTPHost
        {
            get { return _secureHost; }
            set { _secureHost = value; }
        }


        public string SFTPUser
        {
            get { return _secureUser; }
            set { _secureUser = value; }
        }

        public string SFTPPassword
        {
            get { return _securePasswd; }
            set { _securePasswd = value; }
        }

        public int SFTPPort
        {
            get { return _securePort; }
            set { _securePort = value; }
        }

        #endregion

        #region Construtores
        public SFtpClient()
        {
        }
        #endregion

        //#region Structs
        //public struct SshConnectionInfo
        //{
        //    public string Host;
        //    public string User;
        //    public string Pass;
        //    public string IdentityFile;
        //}
        //#endregion

        #region Métodos
        public bool TransferirArquivos(string remoteDir, string localDir, string [] searchPattern )
        {
            bool bRet = false;

            Sftp secureClnt = null;

            try
            {
                logger.InfoFormat("Iniciando conexao SFTP: [{0}:{1}] User: [{2}] password: [{3}]", _secureHost, _securePort, _secureUser, _securePasswd);

                secureClnt = new Sftp(_secureHost, _secureUser, _securePasswd);

                if (_securePort != 22 )
                    secureClnt.Connect(_securePort);
                else
                    secureClnt.Connect();

                Thread.Sleep(2000);

                if ( !secureClnt.Connected)
                {
                    logger.ErrorFormat("Nao fechou conecao conexão SFTP com [{0}:{1}]", _secureHost, _securePort);
                    return bRet;
                }

                secureClnt.OnTransferStart    += new FileTransferEvent(sshCp_OnTransferStart);
                //secureClnt.OnTransferProgress += new FileTransferEvent(sshCp_OnTransferProgress);
                secureClnt.OnTransferEnd      += new FileTransferEvent(sshCp_OnTransferEnd);


                ArrayList list = secureClnt.GetFileList(remoteDir);

                foreach (string arquivo in list)
                {
                    logger.DebugFormat("Arquivo encontrado no ambiente remoto: [{0}] ", arquivo);

                    if (searchPattern.Any(arquivo.Contains))
                    {
                        logger.InfoFormat("Getting [{0}] -> [{1}]", remoteDir + "/" + arquivo, localDir);

                        secureClnt.Get(remoteDir + "/" + arquivo, localDir);
                    }
                }

                //foreach (var item in list)
                //{
                //    string lNome = item.ToString();

                //    if (lNome.Length > 2)
                //    {
                //        string[] lNomeSplitado = lNome.Split('_');

                //        int lAno = Convert.ToInt32(lNomeSplitado[2].Substring(0,4));

                //        int lMes = Convert.ToInt32(lNomeSplitado[2].Substring(4,2));

                //        int lDia = Convert.ToInt32(lNomeSplitado[2].Substring(6,2));

                //        DateTime lDtArquivo = new DateTime(lAno,lMes,lDia) ;

                //        if (lDtArquivo == lDtUil || lDir.Key == "IndicMes")
                //        {
                //            sshCp.Get(string.Concat(lDir.Value, lNome), lpath);
                //        }
                //    }
                //}

                secureClnt.Close();

                bRet = true;
            }
            catch (Exception ex)
            {
                logger.Error("TransferirArquivo: " + ex.Message, ex);
            }
            finally
            {
                if (secureClnt != null && secureClnt.Connected)
                    secureClnt.Close();
            }
            
            return bRet;
            
        }
        #endregion

        #region Eventos
        private static void sshCp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            try
            {
                logger.DebugFormat("Total de Bytes tranferidos [{0}] - [{1}]", totalBytes, message);

                logger.DebugFormat("Fechando Conexão SFTP");
            }
            catch (Exception ex)
            {
                logger.Error("sshCp_OnTransferEnd:" + ex.Message, ex);
            }
        }

        private static void sshCp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            try
            {
                logger.InfoFormat("Transferindo Arquivo [{0}] para [{1}] - [{2}]", src, dst, message);
            }
            catch (Exception ex)
            {
                logger.Error("sshCp_OnTransferProgress:" + ex.Message, ex);
            }
        }

        private static void sshCp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            try
            {
                logger.InfoFormat("Iniciando transferência do  Arquivo: [{0}] para [{1}] - [{2}]", src, dst, message);
            }
            catch (Exception ex)
            {
                logger.Error("sshCp_OnTransferStart:" + ex.Message, ex);
            }
        }
        #endregion

    }
}
