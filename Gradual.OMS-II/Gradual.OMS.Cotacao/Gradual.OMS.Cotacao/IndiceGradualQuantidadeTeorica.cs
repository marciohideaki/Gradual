using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Net.Mail;

namespace Gradual.OMS.Cotacao
{
    /// <summary>
    /// Classe responsável por atualizar as Quantidades Teóricas do Indice Gradual no banco de dados, a partir de um arquivo texto lido periodicamente.
    /// Padrão no nome do arquivo: <indice>.txt (<indice> = nome do indice cadastrado na tabela tbCotacaoIndice)
    /// 
    /// Incluir no .config:
    /// <add key="EnviarEmailInformativo" value="true" />
    /// <add key="EmailAlertaHost" value="ironport.gradual.intra" />
    /// <add key="EmailInformativoReplyTo" value="<usuario>@gradualinvestimentos.com.br" />
    /// <add key="EmailInformativoDestinatarios" value="<usuario>@gradualinvestimentos.com.br" />
    /// <add key="IntervaloChecagemArquivo" value="10" />
    /// <add key="DiretorioChecagemArquivo" value="C:\temp" />
    /// 
    /// </summary>

    [Serializable]
    public class IndiceGradualQuantidadeTeorica
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");
        private CultureInfo ciUS = CultureInfo.CreateSpecificCulture("en-US");
        private NumberStyles numberStyles = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
        private Timer gTimer;
        private int gIntervalo = 10;
        private string gDiretorio = "C:\\temp";
        private List<IndiceGradual.ItemIndice> listaIndices = new List<IndiceGradual.ItemIndice>();

        #region Propriedades (membros públicos)

        public bool CalcularIndiceGradual
        {
            get
            {
                if (ConfigurationManager.AppSettings["CalcularIndiceGradual"] == null)
                    return false;
                return bool.Parse(ConfigurationManager.AppSettings["CalcularIndiceGradual"]);
            }
        }

        public bool EnviarEmailInformativo
        {
            get
            {
                if (ConfigurationManager.AppSettings["EnviarEmailInformativo"] == null)
                    return false;
                return bool.Parse(ConfigurationManager.AppSettings["EnviarEmailInformativo"]);
            }
        }

        public IndiceGradualQuantidadeTeorica()
        {
            if (CalcularIndiceGradual)
            {
                if (ConfigurationManager.AppSettings["IntervaloChecagemArquivo"] != null)
                    gIntervalo = int.Parse(ConfigurationManager.AppSettings["IntervaloChecagemArquivo"]);

                if (ConfigurationManager.AppSettings["DiretorioChecagemArquivo"] != null)
                    gDiretorio = ConfigurationManager.AppSettings["DiretorioChecagemArquivo"];

                listaIndices.Clear();
                DCotacoes _DCotacoes = new DCotacoes();
                listaIndices = _DCotacoes.ObterListaIndicesGradual();

                foreach (IndiceGradual.ItemIndice item in listaIndices)
                {
                    logger.Info("Preparado para tratar Indice Gradual[" + item.indice + "] codigo[" + item.codigoIndice + "]");
                }

                logger.Info("Ativando monitoramento de Arquivo de Quantidades Teoricas (Intervalo = " + gIntervalo + " segs) (Diretorio = " + gDiretorio + ")");
                gTimer = new Timer(new TimerCallback(VerificaArquivos), null, 0, gIntervalo * 1000);
            }
        }

        protected void VerificaArquivos(object state)
        {
            try
            {
                string[] arquivos = Directory.GetFiles(gDiretorio, "*.txt");

                foreach (string arquivo in arquivos)
                {
                    logger.Info("Arquivo encontrado [" + arquivo + "]");

                    foreach (IndiceGradual.ItemIndice item in listaIndices)
                    {
                        if (item.indice.Equals(Path.GetFileNameWithoutExtension(arquivo)))
                        {
                            logger.Info("Iniciando processamento no arquivo [" + Path.GetFileName(arquivo) + "]");

                            string arquivoTemporario = arquivo + ".tmp";
                            File.Move(arquivo, arquivoTemporario);
                            logger.Info("Renomeado para arquivo [" + Path.GetFileName(arquivoTemporario) + "]");

                            if (ProcessarQuantidadesTeoricas(item.indice, arquivoTemporario))
                            {
                                File.Delete(arquivoTemporario);
                                logger.Info("Removido arquivo [" + Path.GetFileName(arquivoTemporario) + "]");
                            }
                            else
                            {
                                string arquivoFalha = arquivo + ".ERRO";
                                File.Move(arquivoTemporario, arquivoFalha);
                                logger.Info("Renomeado para arquivo [" + Path.GetFileName(arquivoFalha) + "]");
                            }
                            DivulgarQuantidadesTeoricas(item.codigoIndice, item.indice);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Falha em VerificaArquivos(): " + ex.Message);
            }
        }

        private bool ProcessarQuantidadesTeoricas(string indice, string arquivo)
        {
            try
            {
                Dictionary<string, double> listaQtdTeoricas = new Dictionary<string, double>();

                string linha;
                StreamReader streamReader = new StreamReader(arquivo);
                while ((linha = streamReader.ReadLine()) != null)
                {
                    if (linha != null && linha.Trim().Length > 0)
                    {
                        string[] parametros = linha.Split('\t');

                        string ativo = parametros[0].Trim();

                        double qtdTeorica;
                        if (!double.TryParse(parametros[1].Replace(',', '.'), numberStyles, ciUS, out qtdTeorica))
                            qtdTeorica = 0;

                        if (ativo.Length > 0 && qtdTeorica > 0)
                            listaQtdTeoricas.Add(ativo, qtdTeorica);
                    }
                }
                streamReader.Close();

                if (listaQtdTeoricas.Count == 0)
                {
                    logger.Error("Não há linhas válidas no arquivo lido!");
                    return false;
                }

                logger.Info("Atualizando Quantidades Teoricas");

                DCotacoes _DCotacoes = new DCotacoes();

                logger.Info("Ativos lidos e tratados do arquivo: " + listaQtdTeoricas.Count);
                foreach (KeyValuePair<string, double> item in listaQtdTeoricas)
                {
                    logger.Info("Gravando ComposicaoIndice Ativo[" + item.Key + "] QtdTeorica[" + item.Value + "]");

                    IndiceGradual.ItemComposicaoIndice itemComposicaoIndice = new IndiceGradual.ItemComposicaoIndice();
                    itemComposicaoIndice.ativo = item.Key;
                    itemComposicaoIndice.qtdeTeorica = item.Value;
                    itemComposicaoIndice.dataCotacao = DateTime.Now;

                    _DCotacoes.AtualizarComposicaoIndiceGradual(indice, itemComposicaoIndice, true);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Falha em ProcessarQuantidadesTeoricas(): " + ex.Message);
                return false;
            }
            return true;
        }

        private void DivulgarQuantidadesTeoricas(int codigoIndice, string indice)
        {
            try
            {
                DCotacoes _DCotacoes = new DCotacoes();

                List<IndiceGradual.ItemComposicaoIndice> lista = _DCotacoes.ObterListaComposicaoIndiceGradual(codigoIndice);

                logger.Info("Montando e-mail informativo de atualização de Quantidades Teóricas Indice[" + indice + "]");

                string subject = "[" + indice + "] Atualização das Quantidades Teóricas";

                string body = "E-Mail Automático gerado pelo Sistema Gradual.OMS.Cotacao - Servidor [" + System.Environment.MachineName + "]";
                body += "\r\n";
                body += "\r\n";
                body += "Quantidades Teóricas atualizadas em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                body += "\r\n";
                body += "\r\n";
                body += "\r\n";

                foreach (IndiceGradual.ItemComposicaoIndice item in lista)
                {
                    body += String.Format("Ativo [{0,-14}] Qtd.Teórica [{1,12:######0.0000}]", item.ativo, item.qtdeTeorica);
                    body += "\r\n";
                }

                _enviarEmail(subject, body);
            }
            catch (Exception ex)
            {
                logger.Error("Falha em DivulgarQuantidadesTeoricas(): " + ex.Message);
            }
        }

        /// <summary>
        /// Envia e-mail
        /// </summary>
        private bool _enviarEmail(string subject, string body)
        {
            try
            {
                string[] destinatarios;

                if (!EnviarEmailInformativo)
                {
                    logger.Error("Envio informativo desabilitado!!!");
                    return false;
                }

                if (ConfigurationManager.AppSettings["EmailInformativoDestinatarios"] == null)
                {
                    logger.Error("Settings 'EmailInformativoDestinatarios' nao definido. Nao eh possivel enviar informativo");
                    return false;
                }

                char[] seps = { ';' };
                destinatarios = ConfigurationManager.AppSettings["EmailInformativoDestinatarios"].ToString().Split(seps);

                var lMensagem = new MailMessage("Gradual.OMS.Cotacao@gradualinvestimentos.com.br", destinatarios[0]);

                for (int i = 1; i < destinatarios.Length; i++)
                {
                    lMensagem.To.Add(destinatarios[i]);
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailInformativoReplyTo"].ToString()));
                lMensagem.Body = body;
                lMensagem.Subject = subject;

                new SmtpClient(ConfigurationManager.AppSettings["EmailAlertaHost"].ToString()).Send(lMensagem);

                logger.InfoFormat("Email [" + subject + "] enviado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("_enviarEmail(): " + ex.Message, ex);
                return false;
            }
            return true;
        }

        #endregion
    }
}
