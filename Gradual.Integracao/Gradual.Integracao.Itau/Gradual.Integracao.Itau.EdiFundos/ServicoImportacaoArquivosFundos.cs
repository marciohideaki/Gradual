using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Gradual.OMS.Library.Servicos;
using log4net;
using Ionic.Zip;
using Gradual.OMS.Library;

namespace Gradual.Integracao.Itau.EdiFundos
{
    public class ServicoImportacaoArquivosFundos : IServicoControlavel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ServicoStatus _servicoStatus = ServicoStatus.Parado;
        private Thread _thMonitorArquivosFundos;
        private string _dirArquivosRecebidos;
        private string _dirArquivosProcessadosBase;
        private string _dirArquivosErroBase;
        private string _dirArquivosProcessados;
        private string _dirArquivosErro;
        private int _intervaloPoll = 300;
        private bool _bKeepRunning = false;
        private Hashtable hstArqProc = new Hashtable();

        #region IServicoControlavel Members
        public void IniciarServico()
        {
            _bKeepRunning = true;
            logger.Info("Iniciando ServicoImportacaoArquivosFundos");

            if (ConfigurationManager.AppSettings["DiretorioArquivosRecebidos"] != null )
            {
                _dirArquivosRecebidos = ConfigurationManager.AppSettings["DiretorioArquivosRecebidos"].ToString();
            }

            if (ConfigurationManager.AppSettings["DiretorioArquivosProcessados"] != null)
            {
                _dirArquivosProcessadosBase = ConfigurationManager.AppSettings["DiretorioArquivosProcessados"].ToString();
            }

            if (ConfigurationManager.AppSettings["DiretorioArquivosErro"] != null)
            {
                _dirArquivosErroBase = ConfigurationManager.AppSettings["DiretorioArquivosErro"].ToString();
            }

            if (ConfigurationManager.AppSettings["IntervaloMonitoracao"] != null)
            {
                _intervaloPoll = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloMonitoracao"].ToString());
            }

            logger.Info("Parametros de inicializacao:");
            logger.Info("Diretorio de arquivos recebidos ....: " + _dirArquivosRecebidos);
            logger.Info("Diretorio de arquivos processados ..: " + _dirArquivosProcessadosBase);
            logger.Info("Diretorio de arquivos rejeitados ...: " + _dirArquivosErroBase);
            logger.Info("Intervalo de monitoracao ...........: " + _intervaloPoll + "s.");

            _intervaloPoll *= 1000;

            _thMonitorArquivosFundos = new Thread(new ThreadStart(thMonitorDiretorio));
            _thMonitorArquivosFundos.Name = "thMonitorDiretorio";
            _thMonitorArquivosFundos.Start();

            logger.Info("ServicoImportacaoArquivosFundos iniciado");
        }

        public void PararServico()
        {
            _bKeepRunning = false;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _servicoStatus;
        }
        #endregion // IServicoControlavel Members

        #region private methods
        private void thMonitorDiretorio()
        {
            List<string> arqRecebidos = new List<string>();
            DateTime lastCheck = DateTime.Now;

            while(_bKeepRunning)
            {
                try
                {
                    TimeSpan lastInterval = new TimeSpan(DateTime.Now.Ticks - lastCheck.Ticks);

                    if (lastInterval.TotalMilliseconds > _intervaloPoll)
                    {
                        lastCheck = DateTime.Now;

                        // Criando diretorios
                        _dirArquivosErro = _dirArquivosErroBase + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                        _dirArquivosProcessados = _dirArquivosProcessadosBase + "\\" + DateTime.Now.ToString("yyyy-MM-dd");

                        if (!Directory.Exists(_dirArquivosErro))
                        {
                            logger.Info("Criando diretorio [" + _dirArquivosErro + "]");
                            Directory.CreateDirectory(_dirArquivosErro);
                        }

                        if (!Directory.Exists(_dirArquivosProcessados))
                        {
                            logger.Info("Criando diretorio [" + _dirArquivosProcessados + "]");
                            Directory.CreateDirectory(_dirArquivosProcessados);
                        }


                        string [] diretorios = _dirArquivosRecebidos.Split(';');
                        arqRecebidos.Clear();
                        for (int i = 0; i < diretorios.Length; i++)
                        {
                            logger.Info("Verificando conteudo do diretorio [" + _dirArquivosRecebidos + "]");
                            arqRecebidos.AddRange(Directory.GetFiles(diretorios[i], "*.*"));
                        }

                        if (arqRecebidos.Count > 0)
                        {
                            logger.Info("Encontrados " + arqRecebidos.Count + " arquivos para serem verificados");

                            foreach (string arquivo in arqRecebidos)
                            {
                                _processarArquivo(arquivo);
                            }
                        }

                        
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("thMonitorDiretorio(): " + ex.Message, ex);
                }

                Thread.Sleep(250);

            }
        }

        private void _processarArquivo(string arquivo)
        {
            string filename = Path.GetFileName(arquivo).ToUpperInvariant();

            logger.Info("Processando arquivo [" + filename + "]");

            if (filename.Equals("ARQUIVOS_DOWNLOAD.ZIP"))
            {
                if ( _descompactarArquivo(arquivo) )
                    File.Move(arquivo, _dirArquivosProcessados + "\\" + filename + ".PRC." + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                else
                    File.Move(arquivo, _dirArquivosErro + "\\" + filename + ".ERR." + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                return;
            }

            // Verifica se o arquivo esta na lista dos esperados
            if (_verificaFilename(filename) == false)
            {
                logger.Error("Arquivo [" + filename + "] nao é um arquivo esperado, descartando.");
                File.Move(arquivo, _dirArquivosErro + "\\" + filename + ".ERR." + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                return;
            }

            // Verifica se eh uma nova versao ou se 
            // ja foi processado anteriormente
            string md5sum = Utils.MD5HashFile(arquivo);

            ControleArquivos ctrlArq = null;
            if (hstArqProc.ContainsKey(filename))
            {
                 ctrlArq = hstArqProc[filename] as ControleArquivos;
            }

            // efetuar a carga do arquivo
            ParserArquivosFundos parser = new ParserArquivosFundos();

            string subject = System.Environment.MachineName + " - Itau Importação de Posição de Fundos - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string msg = "Arquivo [" + filename + "] carregado e processado com sucesso";

            if (parser.Parse(arquivo))
            {
                logger.Info("Arquivo [" + filename + "] processado e carregado com sucesso");
                ctrlArq = new ControleArquivos();
                ctrlArq.MD5Sum = md5sum;
                ctrlArq.Filename = filename;
                ctrlArq.Path = arquivo;
                ctrlArq.Timestamp = DateTime.Now;

                if (hstArqProc.ContainsKey(filename))
                    hstArqProc[filename] = ctrlArq;
                else
                    hstArqProc.Add(filename, ctrlArq);

                // Move o arquivo para o diretorio de processados
                // Talvez seja interessante acrescentar data e hora do processamento
                // na extensao para permitir mais de um processamento por dia
                File.Move(arquivo, _dirArquivosProcessados + "\\" + filename + ".PRC." + DateTime.Now.ToString("yyyyMMddHHmmssfff"));

            }
            else
            {
                logger.Error("Falha no processamento do arquivo [" + filename + "], descartando");
                msg = "Falha no processamento do arquivo [" + filename + "], descartando";
                File.Move(arquivo, _dirArquivosErro + "\\" + filename + ".ERR." + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            }

            Utilities.EnviarEmail(subject, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _descompactarArquivo(string arquivo)
        {
            try
            {
                ZipFile zip = new ZipFile(arquivo);

                zip.ExtractAll(this._dirArquivosRecebidos, ExtractExistingFileAction.OverwriteSilently);

                zip.Dispose();

                zip = null;


            }
            catch (Exception ex)
            {
                logger.Error("_descompactarArquivo(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        private bool _verificaFilename(string filename)
        {
            switch (filename)
            {
                case "ARQAPD0.TXT":
                case "ARQATV.TXT":
                case "ARQCOT.TXT":
                case "ARQCTR.TXT":
                case "ARQOSM.TXT":
                case "ARQOTI.TXT":
                case "ARQPPC.TXT":
                case "ARQSDAD0.TXT":
                case "ARQSDOME.TXT":
                case "CADASI.TXT":
                case "CADFUN.TXT":
                case "CADGES.TXT":
                case "CADCOTCP.TXT":
                case "EMSER.TXT":
                case "EMSERSIM.TXT":
                case "RECTRIB.TXT":
                case "RECTRIBD.TXT":
                case "IMPORTACADCOT.CSV":
                case "IMPORTACADCOTAUGUSTO.CSV":
                case "RELACAOCOTISTASFUNDO.CSV":
                case ".ZIP":
                case "DOWNLOADARQUIVOS.XML":
                    return true;
                default:
                    if (filename.IndexOf("COTASPATRIMONIO") >= 0 ||
                        filename.IndexOf("CARTEIRASDOGESTOR") >= 0)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        #endregion //private methods
    }
}
