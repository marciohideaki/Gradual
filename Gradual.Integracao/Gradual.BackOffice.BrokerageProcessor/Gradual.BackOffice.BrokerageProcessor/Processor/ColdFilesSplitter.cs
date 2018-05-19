using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.BackOffice.BrokerageProcessor.Lib.Txt;
using Gradual.BackOffice.BrokerageProcessor.Account;
using log4net;
using System.IO;
using System.Configuration;
using System.Globalization;
using Gradual.BackOffice.BrokerageProcessor.Lib.Cold;
using Gradual.BackOffice.BrokerageProcessor.Db;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

namespace Gradual.BackOffice.BrokerageProcessor.Processor
{
    public class ColdFilesSplitter
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private Dictionary<int, string> dctNome = new Dictionary<int, string>();


        private string dirSplitted = @"C:\Temp\ColdSplitted";
        public ColdFilesSplitter()
        {
            if (ConfigurationManager.AppSettings["DirColdSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirColdSplitted"].ToString();
            }

            if (!Directory.Exists(dirSplitted))
            {
                Directory.CreateDirectory(dirSplitted);
            }
            
            _CarregarListaNomes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toTxt"></param>
        /// <returns></returns>
        public bool SplitArquivoGarantias(TOArqTxt toTxt)
        {
            try
            {
                string[] allLines = File.ReadAllLines(toTxt.FileName);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                string clienteOrig = "";
                string clienteTmp = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    if (!String.IsNullOrEmpty(line))
                    {

                        int idxClient=line.IndexOf("CLIENTE :");
                        if (idxClient > 0 )
                        {
                            cabecalholido = true;
                            //int idxFimCliente = line.IndexOf("XXXXXXXXXXXXXXXXX");

                            clienteOrig = line.Substring(idxClient + 9, 12);
                            cliente = StripDigitAndThousand(clienteOrig.Trim());

                            logger.Info("Processando cliente BRP [" + cliente + "]");

                            clienteTmp = BuscarClienteGradual(cliente);

                            // Se houver quebra de pagina para o mesmo cliente
                            if (clienteTmp.Equals(clienteGradual))
                                continue;

                            clienteGradual = clienteTmp;

                            string newLine = line.Replace(clienteOrig, BuscarClienteGradualFormatado(cliente, true, true, 12, ' '));

                            string nomeCliente = BuscarNomeCliente(clienteGradual);
                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CCTVM S/A") > 0)
                                newLine = newLine.Replace("GRADUAL CCTVM S/A", nomeCliente);

                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CORRET DE CAMBIO") > 0)
                                newLine = newLine.Replace("GRADUAL CORRET DE CAMBIO", nomeCliente);

                            body = newLine + "\r\n";
                            continue;
                        }

                        int idxRodape = line.IndexOf("TOTAIS:");

                        if (idxRodape > 0)
                        {
                            body += line + "\r\n";

                            string filename = string.Format(@"{0}\{1}\GARANTIAS-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                            logger.Info("Gerando arquivo [" + filename + "]");

                            //Quebra de arquivo, split para novo cliente 
                            Directory.CreateDirectory(Path.GetDirectoryName(filename));

                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                            }


                            STClienteRelatCold stCold;
                            if (listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold))
                            {
                                bool flgCabecalhoGrupo = false;

                                if (cabecalhoGrupo.ContainsKey(stCold.IDGrupo))
                                    flgCabecalhoGrupo = true;

                                if (!stCold.FlagFolhaUnica || (stCold.FlagFolhaUnica && !flgCabecalhoGrupo))
                                {
                                    File.AppendAllText(filename, cabecalho + body);

                                    if (stCold.FlagFolhaUnica && !flgCabecalhoGrupo)
                                        cabecalhoGrupo.Add(stCold.IDGrupo, true);
                                }
                                else
                                    File.AppendAllText(filename, body);
                            }

                            continue;
                        }

                        if (!cabecalholido)
                        {
                            cabecalho += line + "\r\n";
                        }
                        else
                            body += line + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SplitArquivoGarantias: " + ex.Message, ex);
            }
            return true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="toTxt"></param>
        /// <returns></returns>
        public bool SplitArquivoMargem(TOArqTxt toTxt)
        {
            try
            {
                string[] allLines = File.ReadAllLines(toTxt.FileName);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                //string oldcliente = "";
                bool clienteLido = false;

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    if (!String.IsNullOrEmpty(line))
                    {
                        int idxCabecalho = line.IndexOf("DISCRIMINACAO");
                        if (idxCabecalho >= 0)
                        {
                            cabecalholido = true;
                            continue;
                        }

                        if (cabecalholido && !clienteLido)
                        {
                            cliente = line.Substring(4, 8);
                            cliente = cliente.Trim();
                            cliente = StripDigitAndThousand(Convert.ToInt32(cliente).ToString());

                            logger.Info("Processando cliente BRP [" + cliente + "]");

                            clienteGradual = BuscarClienteGradual(cliente);

                            line = line.Replace(cliente.PadLeft(7, '0'), BuscarClienteGradualFormatado(cliente, false, false, 7, '0'));

                            string nomeCliente = BuscarNomeCliente(clienteGradual);
                            if (!String.IsNullOrEmpty(nomeCliente) && line.IndexOf("GRADUAL CCTVM S/A") > 0)
                                line = line.Replace("GRADUAL CCTVM S/A", nomeCliente);

                            if (!String.IsNullOrEmpty(nomeCliente) && line.IndexOf("GRADUAL CORRET DE CAMBIO TIT E") > 0)
                                line = line.Replace("GRADUAL CORRET DE CAMBIO TIT E", nomeCliente);

                            body = line + "\r\n";

                            clienteLido = true;
                            continue;
                        }

                        int idxRodape = line.IndexOf("DISPONIVEL");
                        if (idxRodape >= 0)
                        {
                            body += line + "\r\n";

                            string filename = string.Format(@"{0}\{1}\EXIGENCIA-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                            logger.Info("Gerando arquivo [" + filename + "]");

                            //Quebra de arquivo, split para novo cliente 
                            Directory.CreateDirectory(Path.GetDirectoryName(filename));

                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                            }

                            STClienteRelatCold stCold;
                            if (listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold))
                            {
                                bool flgCabecalhoGrupo = false;

                                if (cabecalhoGrupo.ContainsKey(stCold.IDGrupo))
                                    flgCabecalhoGrupo = true;

                                if (!stCold.FlagFolhaUnica || (stCold.FlagFolhaUnica && !flgCabecalhoGrupo))
                                {
                                    File.AppendAllText(filename, cabecalho + body);

                                    if (stCold.FlagFolhaUnica && !flgCabecalhoGrupo)
                                        cabecalhoGrupo.Add(stCold.IDGrupo, true);
                                }
                                else
                                    File.AppendAllText(filename, body);
                            }

                            clienteLido = false;

                            continue;
                        }

                        if (!cabecalholido)
                        {
                            cabecalho += line + "\r\n";
                        }
                        else
                            body += line + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SplitArquivoMargem: " + ex.Message, ex);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toTxt"></param>
        /// <returns></returns>
        public bool SplitArquivoBTC(TOArqTxt toTxt)
        {
            try
            {
                string[] allLines = File.ReadAllLines(toTxt.FileName);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                string oldcliente = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();


                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    if (!String.IsNullOrEmpty(line))
                    {
                        int idxClient = line.IndexOf("Investidor ");
                        if (idxClient >= 0)
                        {
                            cabecalholido = true;
                            //int idxFimCliente = line.IndexOf("XXXXXXXXXXXXXXXXX");

                            cliente = line.Substring(idxClient + 11, 7);
                            cliente = cliente.Trim();
                            cliente = StripDigitAndThousand(Convert.ToInt32(cliente).ToString());

                            logger.Info("Processando cliente BRP [" + cliente + "]");

                            if (!String.IsNullOrEmpty(oldcliente) && !cliente.Equals(oldcliente))
                            {
                                string filename = string.Format(@"{0}\{1}\BTC-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                                logger.Info("Gerando arquivo [" + filename + "]");

                                //Quebra de arquivo, split para novo cliente 
                                Directory.CreateDirectory(Path.GetDirectoryName(filename));

                                if (File.Exists(filename))
                                {
                                    File.Delete(filename);
                                }

                                STClienteRelatCold stCold;
                                if (!String.IsNullOrEmpty(clienteGradual) && listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold))
                                {
                                    bool flgCabecalhoGrupo = false;

                                    if ( cabecalhoGrupo.ContainsKey(stCold.IDGrupo) )
                                        flgCabecalhoGrupo = true;

                                    if (!stCold.FlagFolhaUnica || (stCold.FlagFolhaUnica && !flgCabecalhoGrupo) )
                                    {
                                        File.AppendAllText(filename, cabecalho + body);

                                        if (stCold.FlagFolhaUnica && !flgCabecalhoGrupo)
                                            cabecalhoGrupo.Add(stCold.IDGrupo, true);
                                    }
                                    else
                                        File.AppendAllText(filename, body);
                                }

                                oldcliente = cliente;

                                clienteGradual = BuscarClienteGradual(cliente);
                            }

                            if (String.IsNullOrEmpty(oldcliente))
                            {
                                oldcliente = cliente;
                                clienteGradual = BuscarClienteGradual(cliente);
                            }

                            string newLine = line.Replace(cliente, BuscarClienteGradualFormatado(cliente, false, false,7,'0'));

                            string nomeCliente = BuscarNomeCliente(clienteGradual);
                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CCTVM S/A") > 0 )
                                newLine = newLine.Replace("GRADUAL CCTVM S/A", nomeCliente);

                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA") > 0)
                                newLine = newLine.Replace("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA", nomeCliente);

                            body = newLine + "\r\n";
                            continue;
                        }

                        if (!cabecalholido)
                        {
                            cabecalho += line + "\r\n";
                        }
                        else
                            body += line + "\r\n";
                    }

                }

                string filename1 = string.Format(@"{0}\{1}\BTC-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                logger.Info("Gerando arquivo [" + filename1 + "]");

                //Quebra de arquivo, split para novo cliente 
                Directory.CreateDirectory(Path.GetDirectoryName(filename1));

                if (File.Exists(filename1))
                {
                    File.Delete(filename1);
                }

                STClienteRelatCold stCold1;
                if (!String.IsNullOrEmpty(clienteGradual) && listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold1))
                {
                    bool flgCabecalhoGrupo = false;

                    if (cabecalhoGrupo.ContainsKey(stCold1.IDGrupo))
                        flgCabecalhoGrupo = true;

                    if (!stCold1.FlagFolhaUnica || (stCold1.FlagFolhaUnica && !flgCabecalhoGrupo))
                    {
                        File.AppendAllText(filename1, cabecalho + body);

                        if (stCold1.FlagFolhaUnica && !flgCabecalhoGrupo)
                            cabecalhoGrupo.Add(stCold1.IDGrupo, true);
                    }
                    else
                        File.AppendAllText(filename1, body);
                }
            }
            catch (Exception ex)
            {
                logger.Error("SplitArquivoBTC: " + ex.Message, ex);
            }
            return true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="toTxt"></param>
        /// <returns></returns>
        public bool SplitArquivoCustodia(TOArqTxt toTxt)
        {
            try
            {
                string[] allLines = File.ReadAllLines(toTxt.FileName);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                string clienteOrig = "";
                string oldcliente = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    if (!String.IsNullOrEmpty(line))
                    {
                        int idxClient = line.IndexOf("CLIENTE:");
                        if (idxClient >= 0)
                        {
                            cabecalholido = true;
                            //int idxFimCliente = line.IndexOf("XXXXXXXXXXXXXXXXX");

                            clienteOrig = line.Substring(idxClient + 9, 11);
                            cliente = StripDigitAndThousand(clienteOrig.Trim());

                            logger.Info("Processando cliente BRP [" + cliente + "]");


                            if (!String.IsNullOrEmpty(oldcliente) && !cliente.Equals(oldcliente))
                            {
                                string filename = string.Format(@"{0}\{1}\CUSTODIA-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                                logger.Info("Gerando arquivo [" + filename + "]");

                                //Quebra de arquivo, split para novo cliente 
                                Directory.CreateDirectory(Path.GetDirectoryName(filename));

                                if (File.Exists(filename))
                                {
                                    File.Delete(filename);
                                }

                                STClienteRelatCold stCold;
                                if (!String.IsNullOrEmpty(clienteGradual) && listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold))
                                {
                                    bool flgCabecalhoGrupo = false;

                                    if (cabecalhoGrupo.ContainsKey(stCold.IDGrupo))
                                        flgCabecalhoGrupo = true;

                                    if (!stCold.FlagFolhaUnica || (stCold.FlagFolhaUnica && !flgCabecalhoGrupo))
                                    {
                                        File.AppendAllText(filename, cabecalho + body);

                                        if (stCold.FlagFolhaUnica && !flgCabecalhoGrupo)
                                            cabecalhoGrupo.Add(stCold.IDGrupo, true);
                                    }
                                    else
                                        File.AppendAllText(filename, body);
                                }

                                oldcliente = cliente;

                                clienteGradual = BuscarClienteGradual(cliente);
                            }

                            if (String.IsNullOrEmpty(oldcliente))
                            {
                                oldcliente = cliente;
                                clienteGradual = BuscarClienteGradual(cliente);
                            }

                            string newLine = line.Replace(clienteOrig, BuscarClienteGradualFormatado(cliente, true, false, 11, ' '));

                            string nomeCliente = BuscarNomeCliente(clienteGradual);
                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CCTVM S/A") > 0)
                                newLine = newLine.Replace("GRADUAL CCTVM S/A", nomeCliente);

                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA") > 0)
                                newLine = newLine.Replace("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA", nomeCliente);

                            body = newLine + "\r\n";
                            continue;
                        }

                        if (!cabecalholido)
                        {
                            cabecalho += line + "\r\n";
                        }
                        else
                            body += line + "\r\n";
                    }
                }

                string filename1 = string.Format(@"{0}\{1}\CUSTODIA-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                logger.Info("Gerando arquivo [" + filename1 + "]");


                //Quebra de arquivo, split para novo cliente 
                Directory.CreateDirectory(Path.GetDirectoryName(filename1));

                if (File.Exists(filename1))
                {
                    File.Delete(filename1);
                }

                STClienteRelatCold stCold1;
                if (!String.IsNullOrEmpty(clienteGradual) && listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold1))
                {
                    bool flgCabecalhoGrupo = false;

                    if (cabecalhoGrupo.ContainsKey(stCold1.IDGrupo))
                        flgCabecalhoGrupo = true;

                    if (!stCold1.FlagFolhaUnica || (stCold1.FlagFolhaUnica && !flgCabecalhoGrupo))
                    {
                        File.AppendAllText(filename1, cabecalho + body);

                        if (stCold1.FlagFolhaUnica && !flgCabecalhoGrupo)
                            cabecalhoGrupo.Add(stCold1.IDGrupo, true);
                    }
                    else
                        File.AppendAllText(filename1, body);
                }
            }
            catch (Exception ex)
            {
                logger.Error("SplitArquivoCustodia: " + ex.Message, ex);
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="toTxt"></param>
        /// <returns></returns>
        public bool SplitArquivoLiquidacoes(TOArqTxt toTxt)
        {
            try
            {
                string[] allLines = File.ReadAllLines(toTxt.FileName);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                string clienteOrig = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    if (!String.IsNullOrEmpty(line))
                    {
                        int idxClient = line.IndexOf("Investidor:");
                        if (idxClient >= 0)
                        {
                            cabecalholido = true;
                            //int idxFimCliente = line.IndexOf("XXXXXXXXXXXXXXXXX");

                            clienteOrig = line.Substring(idxClient + 12, 6);
                            cliente = clienteOrig.Trim();

                            clienteGradual = BuscarClienteGradual(cliente);
                            string newLine = line.Replace(cliente, BuscarClienteGradualFormatado(cliente,false, false, 6, ' ') ).TrimEnd();

                            string nomeCliente = BuscarNomeCliente(clienteGradual);

                            body = newLine + " - " + nomeCliente + "\r\n";
                            continue;
                        }

                        int idxRodape = line.IndexOf("Saldo");

                        if (idxRodape >= 0)
                        {
                            body += line + "\r\n";

                            string filename = string.Format(@"{0}\{1}\LIQUIDACOES-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                            //Quebra de arquivo, split para novo cliente 
                            Directory.CreateDirectory(Path.GetDirectoryName(filename));

                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                            }

                            STClienteRelatCold stCold;
                            if (!String.IsNullOrEmpty(clienteGradual) && listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold))
                            {
                                bool flgCabecalhoGrupo = false;

                                if (cabecalhoGrupo.ContainsKey(stCold.IDGrupo))
                                    flgCabecalhoGrupo = true;

                                if (!stCold.FlagFolhaUnica || (stCold.FlagFolhaUnica && !flgCabecalhoGrupo))
                                {
                                    File.AppendAllText(filename, cabecalho + body);

                                    if (stCold.FlagFolhaUnica && !flgCabecalhoGrupo)
                                        cabecalhoGrupo.Add(stCold.IDGrupo, true);
                                }
                                else
                                    File.AppendAllText(filename, body);
                            }

                            continue;
                        }

                        if (!cabecalholido)
                        {
                            cabecalho += line + "\r\n";
                        }
                        else
                            body += line + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SplitArquivoLiquidacoes: " + ex.Message, ex);
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="toTxt"></param>
        /// <returns></returns>
        public bool SplitArquivoPosicaoCliente(TOArqTxt toTxt)
        {
            try
            {
                string[] allLines = File.ReadAllLines(toTxt.FileName);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                string oldcliente = "";
                string clienteOrig = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    if (!String.IsNullOrEmpty(line))
                    {
                        int idxClient = line.IndexOf("NOME DO CLIENTE:");
                        if (idxClient >= 0)
                        {
                            cabecalholido = true;
                            //int idxFimCliente = line.IndexOf("XXXXXXXXXXXXXXXXX");

                            clienteOrig = line.Substring(0, idxClient).Trim();
                            cliente = StripDigitAndThousand(clienteOrig);

                            logger.Info("Processando cliente BRP [" + cliente  + "]");

                            if (!String.IsNullOrEmpty(oldcliente) && !cliente.Equals(oldcliente) && !String.IsNullOrEmpty(clienteGradual))
                            {
                                string filename = string.Format(@"{0}\{1}\POSCLIENTE-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                                logger.Info("Gerando arquivo [" + filename + "]");

                                //Quebra de arquivo, split para novo cliente 
                                Directory.CreateDirectory(Path.GetDirectoryName(filename));

                                if (File.Exists(filename))
                                {
                                    File.Delete(filename);
                                }

                                STClienteRelatCold stCold;
                                if (!String.IsNullOrEmpty(clienteGradual) && listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold))
                                {
                                    bool flgCabecalhoGrupo = false;

                                    if (cabecalhoGrupo.ContainsKey(stCold.IDGrupo))
                                        flgCabecalhoGrupo = true;

                                    if (!stCold.FlagFolhaUnica || (stCold.FlagFolhaUnica && !flgCabecalhoGrupo))
                                    {
                                        File.AppendAllText(filename, cabecalho + body);

                                        if (stCold.FlagFolhaUnica && !flgCabecalhoGrupo)
                                            cabecalhoGrupo.Add(stCold.IDGrupo, true);
                                    }
                                    else
                                        File.AppendAllText(filename, body);
                                }


                                oldcliente = cliente;

                                clienteGradual = BuscarClienteGradual(cliente);
                            }

                            if (String.IsNullOrEmpty(oldcliente))
                            {
                                oldcliente = cliente;
                                clienteGradual = BuscarClienteGradual(cliente);
                            }

                            string newLine = line.Replace(clienteOrig.PadLeft(8, ' '), BuscarClienteGradualFormatado(cliente, true, false, 8, ' '));

                            string nomeCliente = BuscarNomeCliente(clienteGradual);
                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CCTVM S/A") > 0)
                                newLine = newLine.Replace("GRADUAL CCTVM S/A", nomeCliente);

                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA") > 0)
                                newLine = newLine.Replace("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA", nomeCliente);

                            body = newLine + "\r\n";
                            continue;
                        }

                        if (!cabecalholido)
                        {
                            cabecalho += line + "\r\n";
                        }
                        else
                            body += line + "\r\n";
                    }
                }

                string filename1 = string.Format(@"{0}\{1}\POSCLIENTE-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                logger.Info("Gerando arquivo [" + filename1 + "]");

                //Quebra de arquivo, split para novo cliente 
                Directory.CreateDirectory(Path.GetDirectoryName(filename1));

                if (File.Exists(filename1))
                {
                    File.Delete(filename1);
                }

                STClienteRelatCold stCold1;
                if (!String.IsNullOrEmpty(clienteGradual) && listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold1))
                {
                    bool flgCabecalhoGrupo = false;

                    if (cabecalhoGrupo.ContainsKey(stCold1.IDGrupo))
                        flgCabecalhoGrupo = true;

                    if (!stCold1.FlagFolhaUnica || (stCold1.FlagFolhaUnica && !flgCabecalhoGrupo))
                    {
                        File.AppendAllText(filename1, cabecalho + body);

                        if (stCold1.FlagFolhaUnica && !flgCabecalhoGrupo)
                            cabecalhoGrupo.Add(stCold1.IDGrupo, true);
                    }
                    else
                        File.AppendAllText(filename1, body);
                }
            }
            catch (Exception ex)
            {
                logger.Error("SplitArquivoPosicaoCliente: " + ex.Message, ex);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toTxt"></param>
        /// <returns></returns>
        public bool SplitArquivoTermo(TOArqTxt toTxt)
        {
            try
            {
                string[] allLines = File.ReadAllLines(toTxt.FileName);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                string clienteOrig = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    if (!String.IsNullOrEmpty(line))
                    {

                        int idxClient = line.IndexOf("CLIENTE");
                        int idxRodape = line.IndexOf("TOTAL CLIENTE");

                        if (idxClient > 0 && idxRodape==-1)
                        {
                            cabecalholido = true;
                            //int idxFimCliente = line.IndexOf("XXXXXXXXXXXXXXXXX");

                            clienteOrig = line.Substring(idxClient + 7, 12);
                            cliente = StripDigitAndThousand(clienteOrig.Trim());

                            logger.Info("Processando cliente BRP [" + cliente + "]");

                            clienteGradual = BuscarClienteGradual(cliente);

                            string newLine = line.Replace(clienteOrig, BuscarClienteGradualFormatado(cliente, true, false, 12, ' '));

                            string nomeCliente = BuscarNomeCliente(clienteGradual);
                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CCTVM S/A") > 0)
                                newLine = newLine.Replace("GRADUAL CCTVM S/A", nomeCliente);

                            if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA") > 0)
                                newLine = newLine.Replace("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA", nomeCliente);

                            body = newLine + "\r\n";
                            continue;
                        }


                        if (idxRodape > 0)
                        {
                            body += line + "\r\n";

                            string filename = string.Format(@"{0}\{1}\TERMO-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                            logger.Info("Gerando arquivo [" + filename + "]");

                            //Quebra de arquivo, split para novo cliente 
                            Directory.CreateDirectory(Path.GetDirectoryName(filename));

                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                            }


                            STClienteRelatCold stCold;
                            if (listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold))
                            {
                                bool flgCabecalhoGrupo = false;

                                if (cabecalhoGrupo.ContainsKey(stCold.IDGrupo))
                                    flgCabecalhoGrupo = true;

                                if (!stCold.FlagFolhaUnica || (stCold.FlagFolhaUnica && !flgCabecalhoGrupo))
                                {
                                    File.AppendAllText(filename, cabecalho + body);

                                    if (stCold.FlagFolhaUnica && !flgCabecalhoGrupo)
                                        cabecalhoGrupo.Add(stCold.IDGrupo, true);
                                }
                                else
                                    File.AppendAllText(filename, body);
                            }

                            continue;
                        }

                        if (!cabecalholido)
                        {
                            cabecalho += line + "\r\n";
                        }
                        else
                            body += line + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SplitArquivoTermo: " + ex.Message, ex);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toTxt"></param>
        /// <returns></returns>
        public bool SplitArquivoDividendo(TOArqTxt toTxt)
        {
            try
            {
                string[] allLines = File.ReadAllLines(toTxt.FileName);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                string clienteOrig = "";
                string oldcliente = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();
                
                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');
                    
                    // Leitura do cabecalho
                    if (!cabecalholido)
                    {
                        cabecalho += line + "\r\n";
                        if (line.IndexOf("Reembolso de Proventos em Dinheiro para o", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            cabecalholido = true;
                            cabecalho += "\r\n";
                        }
                        continue;
                    }
                    
                    // Inicio de um novo registro
                    if (line.IndexOf("GRADUAL CCTVM S/A") >= 0 || line.IndexOf("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA") >=0 )
                    {
                        clienteOrig = line.Substring(0, 8);
                        cliente = StripDigitAndThousand(clienteOrig.Trim());
                        logger.Info("Processando cliente BRP [" + cliente + "]");

                        if (!String.IsNullOrEmpty(oldcliente) && !cliente.Equals(oldcliente) && !String.IsNullOrEmpty(clienteGradual))
                        {
                            string filename = string.Format(@"{0}\{1}\DIVIDENDOS-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                            logger.Info("Gerando arquivo [" + filename + "]");

                            //Quebra de arquivo, split para novo cliente 
                            Directory.CreateDirectory(Path.GetDirectoryName(filename));

                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                            }

                            STClienteRelatCold stCold;
                            if (!String.IsNullOrEmpty(clienteGradual) && listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold))
                            {
                                bool flgCabecalhoGrupo = false;

                                if (cabecalhoGrupo.ContainsKey(stCold.IDGrupo))
                                    flgCabecalhoGrupo = true;

                                if (!stCold.FlagFolhaUnica || (stCold.FlagFolhaUnica && !flgCabecalhoGrupo))
                                {
                                    File.AppendAllText(filename, cabecalho + body);

                                    if (stCold.FlagFolhaUnica && !flgCabecalhoGrupo)
                                        cabecalhoGrupo.Add(stCold.IDGrupo, true);
                                }
                                else
                                    File.AppendAllText(filename, body);
                            }


                            oldcliente = cliente;

                            clienteGradual = BuscarClienteGradual(cliente);
                        }

                        if (String.IsNullOrEmpty(oldcliente))
                        {
                            oldcliente = cliente;
                            clienteGradual = BuscarClienteGradual(cliente);
                        }

                        string newLine = line.Replace(clienteOrig.PadLeft(8, ' '), BuscarClienteGradualFormatado(cliente, false, false, 8, ' '));

                        string nomeCliente = BuscarNomeCliente(clienteGradual);
                        if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CCTVM S/A") > 0 )
                            newLine = newLine.Replace("GRADUAL CCTVM S/A", nomeCliente);

                        if (!String.IsNullOrEmpty(nomeCliente) && newLine.IndexOf("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA") > 0)
                            newLine = newLine.Replace("GRADUAL CORRET DE CAMBIO TIT E VALS MOB SA", nomeCliente);

                        body = newLine + "\r\n";
                        continue;
                    }

                    if (!cabecalholido)
                    {
                        cabecalho += line + "\r\n";
                    }
                    else
                        body += line + "\r\n";
                }

                string filename1 = string.Format(@"{0}\{1}\DIVIDENDOS-{2}.txt", dirSplitted, clienteGradual, clienteGradual);

                logger.Info("Gerando arquivo [" + filename1 + "]");

                //Quebra de arquivo, split para novo cliente 
                Directory.CreateDirectory(Path.GetDirectoryName(filename1));

                if (File.Exists(filename1))
                {
                    File.Delete(filename1);
                }

                STClienteRelatCold stCold1;
                if (!String.IsNullOrEmpty(clienteGradual) && listaClientesCold.TryGetValue(Convert.ToInt32(clienteGradual), out stCold1))
                {
                    bool flgCabecalhoGrupo = false;

                    if (cabecalhoGrupo.ContainsKey(stCold1.IDGrupo))
                        flgCabecalhoGrupo = true;

                    if (!stCold1.FlagFolhaUnica || (stCold1.FlagFolhaUnica && !flgCabecalhoGrupo))
                    {
                        File.AppendAllText(filename1, cabecalho + body);

                        if (stCold1.FlagFolhaUnica && !flgCabecalhoGrupo)
                            cabecalhoGrupo.Add(stCold1.IDGrupo, true);
                    }
                    else
                        File.AppendAllText(filename1, body);
                }
            }
            catch (Exception ex)
            {
                logger.Error("SplitArquivoDividendo: " + ex.Message, ex);
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public string BuscarClienteGradual(string cliente)
        {
            AccountDigit acDig = new AccountDigit();

            int accountGradual = AccountParser.Instance.GetAccountParsed(Convert.ToInt32(cliente));

            return accountGradual.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="retornaComDigito"></param>
        /// <param name="retornaComSepMilhar"></param>
        /// <param name="padLeft"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        public  string BuscarClienteGradualFormatado(string cliente, bool retornaComDigito = false, bool retornaComSepMilhar=false, int padLeft=0, char paddingChar=' ')
        {
            AccountDigit acDig = new AccountDigit();
            string account = "";

            int accountGradual = AccountParser.Instance.GetAccountParsed(Convert.ToInt32(cliente));

            if ( !retornaComSepMilhar )
            {
                if (!retornaComDigito)
                    account = accountGradual.ToString();
                else
                {
                    account = acDig.GetAccountWithDigit(accountGradual, AccountDigit.BOVESPA).ToString();
                    account = account.Insert(account.Length - 1, "-");
                }
            }
            else
            {
                if (!retornaComDigito)
                    account = accountGradual.ToString("N0", CultureInfo.CreateSpecificCulture("pt-BR"));
                else
                {
                    account = accountGradual.ToString("N0", CultureInfo.CreateSpecificCulture("pt-BR"));
                    account += "-" + acDig.GetAccountWithDigit(accountGradual, AccountDigit.BOVESPA).ToString().Substring(account.Length - 1, 1);
                }
            }

            if (padLeft > 0)
            {
                account = account.PadLeft(padLeft, paddingChar);
            }

            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public string BuscarClienteBRP(string cliente)
        {
            AccountDigit acDig = new AccountDigit();

            int accountBRP = AccountParser.Instance.GetAccountBRP(Convert.ToInt32(cliente));

            return accountBRP.ToString();
        }


        public  string StripDigitAndThousand(string account)
        {
            string strippedAccount = account;

            int idxMinus = account.IndexOf('-');

            if (idxMinus > 0)
            {
                strippedAccount=account.Remove(idxMinus);
            }

            idxMinus = strippedAccount.IndexOf('.');

            if (idxMinus > 0)
            {
                strippedAccount = strippedAccount.Remove(idxMinus,1);
            }

            return strippedAccount;
        }

        public void _CarregarListaNomes()
        {
            try
            {
                DbAccOracle db = new DbAccOracle();

                ConcurrentDictionary<int, string> dct = db.CarregarNomeClientes();

                foreach (KeyValuePair<int, string> item in dct)
                {
                    if (!dctNome.ContainsKey(item.Key))
                        dctNome.Add(item.Key, item.Value);
                }

                logger.Info("Carregou lista de nomes com " + dctNome.Count + " itens.");
            }
            catch (Exception ex)
            {
                logger.Error("_CarregarListaNomes: " + ex.Message, ex);
            }
        }

        public string BuscarNomeCliente(string clienteGradual)
        {
            string ret = String.Empty;

            if (dctNome.ContainsKey(Convert.ToInt32(clienteGradual)) ) 
                ret = dctNome[Convert.ToInt32(clienteGradual)];

            return ret;
        }
    }
}
