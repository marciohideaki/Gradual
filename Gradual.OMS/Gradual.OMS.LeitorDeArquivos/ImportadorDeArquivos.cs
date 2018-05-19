using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

using System.Data.SqlClient;
using System.Globalization;

namespace Gradual.OMS.LeitorDeArquivos
{
    public delegate void MensagemDeImportacao(string pMensagem);

    public class ImportadorDeArquivos
    {
        #region Globais

        private List<ResultadoDeImportacao> gImportacoesParaRealizar = null;

        private string[] gLinhasDoArquivosDeDados;

        private string gCaminhoDoArquivoSendoImportado;

        private StreamWriter gLogStream = null;

        private StreamWriter gSaidaStream = null;

        private SqlConnection gConexaoComBanco = null;

        #endregion

        #region Propriedades

        public string ConteudoDoArquivoDeDefinicao { get; set; }

        public string CaminhoDoArquivoDeSaida { get; set; }

        public string StringDeConexao { get; set; }

        public string TabelaDeDestino { get; set; }

        public string ProcDeDestino { get; set; }

        public string ProcAnterior { get; set; }

        public string ProcPosterior { get; set; }

        public string ProcCasoErro { get; set; }

        public string ProcCasoSucesso { get; set; }

        public string NomeDoCampoDeData { get; set; }
        
        public string NomeDoCampoDeLinha { get; set; }

        public string NomeDoCampoDeArquivo { get; set; }

        public List<ConfiguracaoDeCampo> CamposConfigurados { get; set; }

        /// <summary>
        /// (get) Retorna true se todas as importações a realizar foram realizadas, com sucesso ou não.
        /// </summary>
        public bool ImportacaoFinalizada 
        {
            get
            {
                if (gImportacoesParaRealizar !=null)
                {
                    foreach (ResultadoDeImportacao lResultado in gImportacoesParaRealizar)
                    {
                        if (!lResultado.Finalizada) return false;
                    }

                    return true;
                }

                return false;
            }
        }

        #endregion

        #region Eventos

        public event MensagemDeImportacao Mensagem;

        private void OnMensagem(string pMensagem, params object[] pParams)
        {
            if (this.Mensagem != null)
                Mensagem(string.Format(pMensagem, pParams));

            MensagemProLog(pMensagem, pParams);
        }

        #endregion

        #region Construtor

        public ImportadorDeArquivos() 
        {
        }

        #endregion

        #region Métodos Private

        private void ExecutarProcedureCasoHaja(string pNomeDaProcedure, string pTipo)
        {
            if (!string.IsNullOrEmpty(pNomeDaProcedure))
            {
                SqlCommand lCommand = new SqlCommand(pNomeDaProcedure, gConexaoComBanco);

                OnMensagem("Executando procedure {0} [{1}]...", pTipo, this.ProcAnterior);

                lCommand.ExecuteNonQuery();

                OnMensagem("Procedure {0} executada com sucesso.", pTipo);
            }
        }


        private void LerLinhaDeConfiguracao(string pParametro, string pLinha)
        {
            pParametro = pParametro.ToLower();

            string lValor = pLinha.Substring(pLinha.IndexOf(':') + 1).TrimStart(' ');

            if(lValor.Contains("//"))
                lValor = lValor.Substring(0, lValor.IndexOf("//"));      //pra suportar comentários à direita da linha

            if (pParametro == "conexão" || pParametro == "conexao" || pParametro == "connectionstring" || pParametro == "connection string")
            {
                this.StringDeConexao = lValor;
            }
            else if (pParametro == "arquivo")
            {
                this.CaminhoDoArquivoDeSaida = lValor;
            }
            else if (pParametro == "tabela de destino" || pParametro == "tabela")
            {
                this.TabelaDeDestino = lValor;
            }
            else if (pParametro == "proc de destinno" || pParametro == "procedure de destino")
            {
                this.ProcDeDestino = lValor;
            }
            else if (pParametro == "proc anterior")
            {
                this.ProcAnterior = lValor;
            }
            else if (pParametro == "proc posterior")
            {
                this.ProcPosterior = lValor;
            }
            else if (pParametro == "proc caso erro")
            {
                this.ProcCasoErro = lValor;
            }
            else if (pParametro == "proc caso sucesso")
            {
                this.ProcCasoSucesso = lValor;
            }
            else if (pParametro == "campo de data de importação" || pParametro == "campo de data de importacao")
            {
                this.NomeDoCampoDeData = lValor;
            }
            else if (pParametro == "campo de linha do arquivo")
            {
                this.NomeDoCampoDeLinha = lValor;
            }
            else if (pParametro == "campo de nome do arquivo")
            {
                this.NomeDoCampoDeArquivo = lValor;
            }

            OnMensagem("Configuração [{0}] carregada com sucesso: [{1}]", pParametro, lValor);
        }

        private void LerLinhaDeCampo(string pCabecalhoDosCampos, string pLinha)
        {
            if (this.CamposConfigurados == null)
                this.CamposConfigurados = new List<ConfiguracaoDeCampo>();

            this.CamposConfigurados.Add(new ConfiguracaoDeCampo(pCabecalhoDosCampos, pLinha));

            OnMensagem("Campo [{0}] configurado com sucesso", this.CamposConfigurados[this.CamposConfigurados.Count - 1].NomeDoCampo);
        }


        private void MensagemProLog(string pMensagem, params object[] pParams)
        {
            if(gLogStream != null)
                gLogStream.WriteLine(string.Format(pMensagem, pParams));
        }

        private void MensagemPraSaida(string pMensagem, params object[] pParams)
        {
            if(gSaidaStream != null)
                gSaidaStream.WriteLine(string.Format(pMensagem, pParams));
        }


        private void Importacao_AbrirArquivoDeLog(string pArquivoDeDadosRelativo)
        {
            string lCaminhoDoLog;

            lCaminhoDoLog = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            lCaminhoDoLog = Path.Combine(lCaminhoDoLog, string.Format("Importacao-{0}.log", pArquivoDeDadosRelativo));

            OnMensagem("Abrindo arquivo de Log [{0}]", lCaminhoDoLog);

            gLogStream = File.AppendText(lCaminhoDoLog);
            
            OnMensagem("Arquivo de Log aberto com sucesso.");

            gLogStream.AutoFlush = true;
        }

        private void Importacao_AbrirArquivoDeSaida(string pArquivoDeDadosRelativo)
        {
            if (!string.IsNullOrEmpty(this.CaminhoDoArquivoDeSaida))
            {
                string lCaminhoDeSaida = this.CaminhoDoArquivoDeSaida;

                lCaminhoDeSaida.Insert(lCaminhoDeSaida.LastIndexOf('.'), string.Format("-{0}", pArquivoDeDadosRelativo));

                OnMensagem("Abrindo arquivo de Saída [{0}]", lCaminhoDeSaida);

                if (File.Exists(this.CaminhoDoArquivoDeSaida))
                {
                    try
                    {
                        OnMensagem("Arquivo de Saída já existe, renomeando...");

                        File.Move(lCaminhoDeSaida, string.Format("{0}.old_{1}", lCaminhoDeSaida, DateTime.Now.Ticks));

                        OnMensagem("Arquivo de Saída renomeado com sucesso.");
                    }
                    catch
                    { 
                        OnMensagem("Erro ao renomear arquivo de Saída: [{0}]. Será sobrescrito.", lCaminhoDeSaida);
                    }
                }

                gSaidaStream = File.AppendText(lCaminhoDeSaida);

                OnMensagem("Arquivo de Saída aberto com sucesso.");

                gLogStream.AutoFlush = true;
            }
            else
            {
                gSaidaStream = null;
            }
        }

        private bool Importacao_AbrirConexaoComBanco()
        {
            bool lRetorno = false;

            if (!string.IsNullOrEmpty(this.StringDeConexao))
            {
                gConexaoComBanco = new SqlConnection(this.StringDeConexao);

                OnMensagem("Tendando se conextar ao banco [{0}]", this.StringDeConexao);

                try
                {
                    gConexaoComBanco.Open();

                    lRetorno = true;
                }
                catch (Exception ex)
                {
                    OnMensagem("Erro ao conectar com o banco: {0}\r\n{1}", ex.Message, ex.StackTrace);
                }
            }
            else
            {
                OnMensagem("String de Conexão não configurada!");
            }

            return lRetorno;
        }


        private SqlParameter Importacao_ProcessarCampo(string pLinha, ConfiguracaoDeCampo pCampo)
        {
            string   lValorRemovido;
            int      lValorParseadoInt;
            double   lValorParseadoDouble;
            DateTime lValorParseadoDate;

            SqlParameter lParametro = new SqlParameter(pCampo.NomeDoCampo, DBNull.Value);

            lValorRemovido = pLinha.Substring(pCampo.PosicaoInicial, pCampo.Comprimento);

            try
            {
                if (string.IsNullOrEmpty(lValorRemovido))
                {
                    if (pCampo.Opcao_BrancoIgualAZero)
                    {
                        lParametro.Value = 0;
                    }
                    else
                    {
                        if (!pCampo.Opcao_BrancoIgualANulo)
                            throw new Exception(string.Format("    Campo [{0}] está em branco e não pode ser nulo. Utilizar a opção 'branco=nulo' caso possa.", pCampo.NomeDoCampo));
                    }
                }
                else
                {
                    if (pCampo.TipoDeDestino == typeof(int))
                    {
                        //pra suportar sinal de + ou - no final da string:
                        if (lValorRemovido.EndsWith("+") || lValorRemovido.EndsWith("-"))
                            lValorRemovido = string.Format("{0}{1}", lValorRemovido[lValorRemovido.Length - 1], lValorRemovido.Substring(0, lValorRemovido.Length - 1));

                        if (int.TryParse(lValorRemovido, out lValorParseadoInt))
                        {
                            lParametro.Value = lValorParseadoInt;
                        }
                        else
                        {
                            throw new Exception(string.Format("    Valor numérico [{1}] inválido para o campo [{0}].", pCampo.NomeDoCampo, lValorRemovido));
                        }
                    }
                    else if (pCampo.TipoDeDestino == typeof(double))
                    {
                        //pra suportar sinal de + ou - no final da string:
                        if (lValorRemovido.EndsWith("+") || lValorRemovido.EndsWith("-"))
                            lValorRemovido = string.Format("{0}{1}", lValorRemovido[lValorRemovido.Length - 1], lValorRemovido.Substring(0, lValorRemovido.Length - 1));

                        if (double.TryParse(lValorRemovido, out lValorParseadoDouble))
                        {
                            lParametro.Value = lValorParseadoDouble;
                        }
                        else
                        {
                            throw new Exception(string.Format("    Valor numérico [{1}] inválido para o campo [{0}].", pCampo.NomeDoCampo, lValorRemovido));
                        }
                    }
                    else if (pCampo.TipoDeDestino == typeof(DateTime))
                    {
                        string lFormatoDeOrigem = "dd/MM/YYYY";

                        if (!string.IsNullOrEmpty(pCampo.FormatoDeOrigem))
                            lFormatoDeOrigem = pCampo.FormatoDeOrigem;

                        if (DateTime.TryParseExact(lValorRemovido, lFormatoDeOrigem, new CultureInfo("pt-BR"), DateTimeStyles.None, out lValorParseadoDate))
                        {
                            lParametro.Value = lValorParseadoDate;
                        }
                        else
                        {
                            throw new Exception(string.Format("     Valor de data [{1}] inválido para o campo [{0}] (utilizando o formato [{2}].", pCampo.NomeDoCampo, lValorRemovido, lFormatoDeOrigem));
                        }
                    }
                    else
                    {
                        //string
                        //TODO: tirar o "formato de destino", não precisa

                        if(pCampo.Opcao_TrimNoValor && lValorRemovido != null)
                            lParametro.Value = lValorRemovido.Trim();
                        else
                            lParametro.Value = lValorRemovido;
                    }
                }
            }
            catch (Exception exGeral)
            {
                if (pCampo.Opcao_ErrosSaoNulos)
                {
                    OnMensagem("    Erro no processamento do campo [{0}], está configurado para passar nulo. Mensagem: [{1}]", pCampo.NomeDoCampo, exGeral.Message);

                    lParametro.Value = DBNull.Value;
                }
                else
                {
                    throw exGeral;
                }
            }

            return lParametro;
        }

        private void Importacao_ProcessarLinhaViaProcedure(string pLinha)
        {
            SqlCommand lCommand = new SqlCommand(this.ProcDeDestino, gConexaoComBanco);

            lCommand.CommandType = System.Data.CommandType.StoredProcedure;

            foreach (ConfiguracaoDeCampo lCampo in this.CamposConfigurados)
            {
                lCommand.Parameters.Add(Importacao_ProcessarCampo(pLinha, lCampo));
            }

            lCommand.ExecuteNonQuery();
        }

        private void Importacao_ProcessarLinhaViaInsert(string pLinha)
        {
            string lComando  = "INSERT INTO {0} ({1}) VALUES ({2})";

            string lCampos = "";

            string lValores = "";

            string lValor;

            string lFormatoDeData = null;

            SqlParameter lParametro;

            foreach (ConfiguracaoDeCampo lCampo in this.CamposConfigurados)
            {
                lParametro = Importacao_ProcessarCampo(pLinha, lCampo);

                lCampos += string.Format("{0}, ", lCampo.NomeDoCampo);

                if (lCampo.TipoDeDestino == typeof(string))
                {
                    lValor = string.Format("{0}, ", (lParametro.Value == DBNull.Value) ? "NULL" : "'" + lParametro.Value+ "'");
                }
                else if(lCampo.TipoDeDestino == typeof(DateTime))
                {
                    if(!string.IsNullOrEmpty( lCampo.FormatoDeDestino ))
                    {
                        lValor = string.Format("{0}, ", (lParametro.Value == DBNull.Value) ? "NULL" : "'" + ((DateTime)lParametro.Value).ToString(lCampo.FormatoDeDestino) + "'");

                        lFormatoDeData = lCampo.FormatoDeDestino;
                    }
                    else
                    {
                        lValor = string.Format("{0}, ", (lParametro.Value == DBNull.Value) ? "NULL" : "'" + lParametro.Value + "'");
                    }

                }
                else if (lCampo.TipoDeDestino == typeof(double))
                {
                    //separar em casas decimais
                    int lCasas = lCampo.Opcao_CasasDecimais;

                    if (lCasas > 0 && lParametro.Value != DBNull.Value)
                    {
                        string lValorOriginal = string.Format("{0}", lParametro.Value);

                        //pega a parte decimal:

                        if (lValorOriginal.Length > lCasas)
                        {
                            lValor = string.Format("{0}.{1}",
                                                    lValorOriginal.Substring(0, lValorOriginal.Length - lCasas), //parte inteira
                                                    lValorOriginal.Substring(lValorOriginal.Length - lCasas)    //parte decimal
                                                  );
                        }
                        else
                        {
                            lValor = lValorOriginal;
                        }

                        lValor = string.Format("{0}, ", lValor);
                    }
                    else
                    {
                        lValor = string.Format("{0}, ", (lParametro.Value == DBNull.Value) ? "NULL" : lParametro.Value);
                    }
                }
                else
                {
                    lValor = string.Format("{0}, ", (lParametro.Value == DBNull.Value) ? "NULL" : lParametro.Value);
                }


                if (lCampo.Opcao_Espacamento > 0)
                {
                    lValor = lValor.PadRight(lCampo.Opcao_Espacamento + 5, ' ');
                }

                lValores += lValor; 
            }

            if (!string.IsNullOrEmpty(this.NomeDoCampoDeData))
            {
                lCampos  += string.Format("{0}, ",   this.NomeDoCampoDeData);
                lValores += string.Format("'{0}', ", (string.IsNullOrEmpty(lFormatoDeData) ? DateTime.Now.ToString() : DateTime.Now.ToString(lFormatoDeData)));
            }

            if (!string.IsNullOrEmpty(this.NomeDoCampoDeLinha))
            {
                lCampos  += string.Format("{0}, ",   this.NomeDoCampoDeLinha);
                lValores += string.Format("'{0}', ", pLinha);
            }

            if (!string.IsNullOrEmpty(this.NomeDoCampoDeArquivo))
            {
                lCampos  += string.Format("{0}, ",   this.NomeDoCampoDeArquivo);
                lValores += string.Format("'{0}', ", gCaminhoDoArquivoSendoImportado);
            }

            lCampos  = lCampos.TrimEnd(' ', ',');
            lValores = lValores.TrimEnd(' ', ',');

            lComando = string.Format(lComando, this.TabelaDeDestino, lCampos, lValores);

            MensagemPraSaida(lComando);

            SqlCommand lCommand = new SqlCommand(lComando, gConexaoComBanco);

            lCommand.CommandType = System.Data.CommandType.Text;

            try
            {
                lCommand.ExecuteNonQuery();
            }
            catch (Exception exExecucao)
            {
                OnMensagem("Erro de insert: [{0}] [{1}]", exExecucao.Message, lComando);

                throw exExecucao;
            }
        }
        
        private void Importacao_FecharArquivosEConexao()
        {
            if (gSaidaStream != null)
            {
                OnMensagem("Fechando arquivo de Saída...");
                
                gSaidaStream.Close();
                
                gSaidaStream.Dispose();
            }

            if (gConexaoComBanco != null && gConexaoComBanco.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    OnMensagem("Fechando conexão com o banco...");
                    gConexaoComBanco.Close();
                }
                catch { }
                finally
                {
                    gConexaoComBanco.Dispose();
                }
            }

            if (gLogStream != null)
            {
                OnMensagem("Fechando arquivo de Log...");

                gLogStream.Close();

                gLogStream.Dispose();
            }

            gSaidaStream = null;
            gLogStream = null;
        }


        private void RealizarImportacao(ResultadoDeImportacao pResultado)
        {
            //System.Threading.Thread.Sleep(4000);

            string lArquivoRelativo = Path.GetFileName(pResultado.CaminhoDoArquivoDeDados);

            Importacao_AbrirArquivoDeLog(lArquivoRelativo);

            Importacao_AbrirArquivoDeSaida(lArquivoRelativo);

            if (Importacao_AbrirConexaoComBanco())
            {
                ExecutarProcedureCasoHaja(this.ProcAnterior, "pré-importação");

                int lContagemSucessos = 0;
                int lContagemErros = 0;
                int lTotal = gLinhasDoArquivosDeDados.Length;

                byte lPassosParaLogar = 0;

                byte lMaximoDeErrosEmSequencia = 100;

                bool lImportacaoComSucesso = true;

                for (int a = 0; a < gLinhasDoArquivosDeDados.Length; a++)
                {
                    lPassosParaLogar++;

                    if (lPassosParaLogar == 10)
                    {
                        lPassosParaLogar = 0;

                        OnMensagem("Importação em andamento: [{0}] com sucesso, [{1}] com erro.", lContagemSucessos, lContagemErros);
                    }


                    try
                    {
                        if (string.IsNullOrEmpty(this.ProcDeDestino))
                        {
                            Importacao_ProcessarLinhaViaInsert(gLinhasDoArquivosDeDados[a]);
                        }
                        else
                        {
                            Importacao_ProcessarLinhaViaProcedure(gLinhasDoArquivosDeDados[a]);
                        }

                        lContagemSucessos++;
                    }
                    catch (Exception ex)
                    {
                        OnMensagem("Erro ao importar a linha número [{0}]: [{1}]", (a + 1).ToString().PadLeft(9, '0'), ex.Message);

                        lContagemErros++;
                    }


                    if (lContagemErros >= lMaximoDeErrosEmSequencia && lContagemSucessos == 0)
                    {
                        OnMensagem("Passaram-se [{0}] erros sem nenhum sucesso, a importação será abortada.", lContagemErros);

                        lImportacaoComSucesso = false;

                        break;
                    }
                }

                ExecutarProcedureCasoHaja(this.ProcPosterior, "posterior");

                if (lImportacaoComSucesso)
                {
                    ExecutarProcedureCasoHaja(this.ProcCasoSucesso, "de sucesso");

                    OnMensagem("Importação realizada com sucesso, renomeando arquivo de dados");

                    File.Move(pResultado.CaminhoDoArquivoDeDados, pResultado.CaminhoDoArquivoDeDados + ".importado");

                    pResultado.MarcarComoFinalizada("Finalizada com Sucesso");
                }
                else
                {
                    ExecutarProcedureCasoHaja(this.ProcCasoErro, "de erro");

                    OnMensagem("Importação realizada com erro.");

                    pResultado.MarcarComoFinalizada("Finalizada com Erro");
                }
            }
            else
            {
                OnMensagem("Importação abortada por falta de conexão com o banco de dados.");

                pResultado.MarcarComoFinalizada("Finalizada com Erro");
            }

            Importacao_FecharArquivosEConexao();
        }

        #endregion

        #region Métodos Públicos

        public void LerArquivoDeDefinicao(string pCaminhoDoArquivo)
        {
            try
            {
                string[] lConteudo = File.ReadAllLines(pCaminhoDoArquivo);

                OnMensagem("Arquivo de definição aberto com sucesso, lendo as linhas...");

                string lParametro;
                string lCabecalhoDosCampos = "";

                byte lLendoCampos = 0;

                foreach (string lLinha in lConteudo)
                {
                    try
                    {
                        if(!string.IsNullOrEmpty(lLinha))
                        {
                            if(lLendoCampos == 0)
                            {
                                if (lLinha.Contains(':'))
                                {
                                    lParametro = lLinha.Substring(0, lLinha.IndexOf(':')).TrimStart();

                                    if (!lParametro.StartsWith("//"))
                                    {
                                        if (lParametro.ToLower() == "campos")
                                        {
                                            lLendoCampos = 1;
                                        }
                                        else
                                        {
                                            LerLinhaDeConfiguracao(lParametro, lLinha);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if(lLendoCampos == 1)
                                {
                                    lCabecalhoDosCampos = lLinha;

                                    lLendoCampos = 2;
                                }
                                else
                                {
                                    if(!lLinha.TrimStart().StartsWith("//"))
                                        LerLinhaDeCampo(lCabecalhoDosCampos, lLinha);
                                }
                            }
                        }
                    }
                    catch (Exception ex2)
                    {
                        OnMensagem("Erro ao ler uma linha de definição: {0}\r\n{1}", ex2.Message, ex2.StackTrace);

                        throw ex2;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao ler o arquivo em [{0}]: {1}\r\n\r\n{2}", pCaminhoDoArquivo, ex.Message, ex.StackTrace));
            }
        }

        private void RealizarImportacoes()
        {
            foreach (ResultadoDeImportacao lImportacao in gImportacoesParaRealizar)
            {
                OnMensagem("Iniciando importação do arquivo [{0}]", lImportacao.CaminhoDoArquivoDeDados);

                if (lImportacao.CaminhoDoArquivoDeDados.ToLower().StartsWith("http://") || lImportacao.CaminhoDoArquivoDeDados.ToLower().StartsWith("www"))
                {
                    OnMensagem("O arquivo de dados deve ser buscado da internet, tentando conexão...");

                    string lConteudo = "";

                    try
                    {
                        lConteudo = WGet.Get(lImportacao.CaminhoDoArquivoDeDados);
                    }
                    catch (Exception webException)
                    {
                        OnMensagem("Erro ao buscar dados de [{0}]: [{1}].\r\nA importação não pode continuar.", lImportacao.CaminhoDoArquivoDeDados, webException.Message);

                        lImportacao.MarcarComoFinalizada("Finalizada com erro");

                        break;
                    }

                    lImportacao.CaminhoDoArquivoDeDados = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    lImportacao.CaminhoDoArquivoDeDados = Path.Combine(lImportacao.CaminhoDoArquivoDeDados, string.Format("DadosDaInternet-{0}.temp", DateTime.Now.ToString("yyMMdd-HHmmss")));

                    OnMensagem("Dados buscados da internet com sucesso, salvando localmente em [{0}]", lImportacao.CaminhoDoArquivoDeDados);

                    File.WriteAllText(lImportacao.CaminhoDoArquivoDeDados, lConteudo);

                    OnMensagem("Arquivo salvado com sucesso, prosseguindo com a importação...");
                }

                gCaminhoDoArquivoSendoImportado = lImportacao.CaminhoDoArquivoDeDados;

                gLinhasDoArquivosDeDados = File.ReadAllLines(lImportacao.CaminhoDoArquivoDeDados);

                if (gLinhasDoArquivosDeDados.Length > 0)
                {
                    //gCaminhoDoArquivoSendoImportado = lImportacao.CaminhoDoArquivoDeDados;

                    OnMensagem("{0} linhas para importar...", gLinhasDoArquivosDeDados.Length);

                    RealizarImportacao(lImportacao);
                }
                else
                {
                    OnMensagem("Arquivo de dados vazio, terminando a importação.");

                    lImportacao.MarcarComoFinalizada("Arquivo de dados vazio");
                }
            }
        }

        public void RealizarImportacao(string pCaminhoDoArquivoDeDados)
        {
            gImportacoesParaRealizar = new List<ResultadoDeImportacao>();

            ResultadoDeImportacao lResultado;

            if (pCaminhoDoArquivoDeDados.EndsWith("\\"))
            {
                string[] lCaminhos = Directory.GetFiles(pCaminhoDoArquivoDeDados);

                foreach (string lCaminho in lCaminhos)
                {
                    if (!lCaminho.EndsWith(".importado"))       //arquivos ".importado" são os que já foram importados
                    {
                        lResultado = new ResultadoDeImportacao(lCaminho);

                        gImportacoesParaRealizar.Add(lResultado);
                    }
                }
            }
            else
            {
                gImportacoesParaRealizar.Add(new ResultadoDeImportacao(pCaminhoDoArquivoDeDados));
            }

            OnMensagem("[{0}] arquivo{1} para importar.", gImportacoesParaRealizar.Count, gImportacoesParaRealizar.Count > 0 ? "s" : "");
            
            ThreadStart lThread = new ThreadStart(RealizarImportacoes);

            lThread.BeginInvoke(null, null);

            OnMensagem("Thread de importações iniciado.");

        }

        #endregion
    }
}
