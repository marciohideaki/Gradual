using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Mentalis.Security;
using log4net;
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch;
using System.Collections;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA;
using System.Configuration;
namespace Gradual.OMS.InvXX.Fundos.DbLib
{
    public class SharpSSH
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, string> ListaDir = new Dictionary<string,string>();
        #endregion

        #region Propriedades
        private string HostAnbimaSFTP
        {
            get { return ConfigurationManager.AppSettings["HostAnbimaSFTP"].ToString(); }
        }


        private string UserAnbimaSFTP
        {
            get { return ConfigurationManager.AppSettings["UserAnbimaSFTP"].ToString(); }
        }

        private string PasswordAnbimaSFTP
        {
            get { return ConfigurationManager.AppSettings["PasswordAnbimaSFTP"].ToString(); }
        }
        #endregion

        #region Construtores
        public SharpSSH()
        {
            ListaDir.Add("FundosMovCota", "/sianbima/anmovcot/");
            ListaDir.Add("FundosDia", "/sianbima/anvldia/");
            ListaDir.Add("FundosMes", "/sianbima/anvlmes/");
            //ListaDir.Add("IndicMes", "/sianbima/")
            //ListaArquivos.Add("Instituicoes","");
            //ListaArquivos.Add("FundosTipo","");
            //ListaArquivos.Add("Fundos","");
            ListaDir.Add("TaxaAdm", "/sianbima/antaxadm/");
            //ListaArquivos.Add("FundosMovCota","");
            //ListaArquivos.Add("FundosStatus","");
            //ListaArquivos.Add("NotaExpl","");
            //ListaArquivos.Add("FundosNE","");
            //ListaArquivos.Add("FundosDia","");
            //ListaArquivos.Add("FundosDia1","");
            //ListaArquivos.Add("FundosMes","");
            //ListaArquivos.Add("FundosConsDia","");
            //ListaArquivos.Add("FundosConsMes", "");
            //ListaArquivos.Add("PLInst","");
            //ListaArquivos.Add("AplicAgregada","");
            //ListaArquivos.Add("Especie","");
            //ListaArquivos.Add("Indicadores","");
            ListaDir.Add("IndicMes", "/sianbima/anvalind/");
            //ListaArquivos.Add("Queries","");
            //ListaArquivos.Add("ControleTransf","");
            //ListaArquivos.Add("Xanvaldia","");
            //ListaArquivos.Add("XFundosDia","");
        }
        #endregion

        #region Structs
        public struct SshConnectionInfo
        {
            public string Host;
            public string User;
            public string Pass;
            public string IdentityFile;
        }
        #endregion

        #region Métodos
        public bool TranferirArquivo()
        {
            bool lRetorno = false;

            Sftp sshCp = null;

            try
            {
                gLogger.InfoFormat("*******************************************************************");
                gLogger.InfoFormat("SharpSSH > Abrindo conexão com ANBIMA******************************");
                gLogger.InfoFormat("*******************************************************************");

                //SshTransferProtocolBase sshCp;

                SshConnectionInfo lInfo = new SshConnectionInfo();
                lInfo.Host = this.HostAnbimaSFTP;
                lInfo.User = this.UserAnbimaSFTP;
                lInfo.Pass = this.PasswordAnbimaSFTP;

                sshCp = new Sftp(lInfo.Host, lInfo.User);

                sshCp.Password = lInfo.Pass;

                gLogger.InfoFormat("Dados de conexão ANBIMA Host: [{0}] User: [{1}] password: [{2}]", lInfo.Host, lInfo.User, lInfo.Pass);

                sshCp.Connect();

                while (!sshCp.Connected)
                {
                    gLogger.InfoFormat("SharpSSH > Aguardando conexão SFTP com ANBIMA******************************");
                }

                sshCp.OnTransferStart    += new FileTransferEvent(sshCp_OnTransferStart);
                sshCp.OnTransferProgress += new FileTransferEvent(sshCp_OnTransferProgress);
                sshCp.OnTransferEnd      += new FileTransferEvent(sshCp_OnTransferEnd);

                string lpath = ConfigurationManager.AppSettings["ArquivosAnbima"];
                
                DateTime lDtUil = new ImportacaoDbLib().SelecionaUltimoPregao();

                foreach (KeyValuePair<string, string> lDir in ListaDir)
                {
                    ArrayList list = sshCp.GetFileList(lDir.Value);

                    foreach (string arquivo in list)
                    {
                        gLogger.InfoFormat("SharpSSH > Arquivo encontrado no anbiente na ANBIMA -> [{0}] ", arquivo);
                    }

                    foreach (var item in list)
                    {
                        string lNome = item.ToString();

                        if (lNome.Length > 2)
                        {
                            string[] lNomeSplitado = lNome.Split('_');

                            int lAno = Convert.ToInt32(lNomeSplitado[2].Substring(0,4));

                            int lMes = Convert.ToInt32(lNomeSplitado[2].Substring(4,2));

                            int lDia = Convert.ToInt32(lNomeSplitado[2].Substring(6,2));

                            DateTime lDtArquivo = new DateTime(lAno,lMes,lDia) ;

                            if (lDtArquivo == lDtUil || lDir.Key == "IndicMes")
                            {
                                sshCp.Get(string.Concat(lDir.Value, lNome), lpath);
                            }
                        }
                    }

                    gLogger.InfoFormat("SharpSSH > Arquivo [{0}] transferido com sucesso.", lpath);
                }

                sshCp.Close();

                lRetorno = true;
            }
            catch (Exception ex)
            {
                gLogger.Error("SharpSSH > TranferirArquivo -", ex);
            }
            finally
            {
                if (sshCp != null && sshCp.Connected) sshCp.Close();
            }

            
            return lRetorno;
            
        }
        #endregion

        #region Eventos
        private static void sshCp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            try
            {
                gLogger.InfoFormat("Total de Bytes tranferidos [{0}] - [{1}]", totalBytes, message);

                gLogger.InfoFormat("Fechando Conexão SFTP com ANBIMA");
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("SharpSSH > sshCp_OnTransferEnd - [{0}]", ex.StackTrace);
            }
        }

        private static void sshCp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            try
            {
                gLogger.InfoFormat("Transferindo Arquivo [{0}] para [{1}] - [{2}]", src, dst, message);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("SharpSSH > sshCp_OnTransferProgress - [{0}]", ex.StackTrace);
            }
        }

        private static void sshCp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            try
            {
                gLogger.InfoFormat("Iniciando transferência do  Arquivo: [{0}] para [{1}] - [{2}]", src, dst, message);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("SharpSSH > sshCp_OnTransferStart - [{0}]", ex.StackTrace);
            }
        }
        #endregion
    }
}
