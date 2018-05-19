using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Gradual.SerieHistorica.Dados;

namespace Gradual.SerieHistorica
{
    public partial class Form1 : Form
    {
        private OpenFileDialog arquivo;
        private List<String> arquivosImportacao = new List<string>();
        private List<Label> arquivosForm = new List<Label>();
        private List<ProgressBar> progressosForm = new List<ProgressBar>();
        private SerieHistoricaDados serieHistorica = null;
        private String servidorIP = null;
        private String bancoDados = null;
        private String usuario = null;
        private String senha = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Dados padrões para conexão com o Banco de Dados
            servidorIP = "192.168.254.14";
            bancoDados = "MDS";
            usuario = "analisegraf";
            senha = "gradual123*";

            arquivo = new OpenFileDialog();
            arquivo.Multiselect = true;
            btnImportar.Enabled = false;
            txtServidorIP.Text = servidorIP;
            txtBancoDados.Text = bancoDados;
            txtUsuario.Text = usuario;
            txtSenha.Text = senha;
            arquivosForm.Add(lblArquivo1);
            arquivosForm.Add(lblArquivo2);
            arquivosForm.Add(lblArquivo3);
            arquivosForm.Add(lblArquivo4);
            foreach (Label arquivoForm in arquivosForm)
                arquivoForm.Text = "";
            progressosForm.Add(progressBar1);
            progressosForm.Add(progressBar2);
            progressosForm.Add(progressBar3);
            progressosForm.Add(progressBar4);
            foreach (ProgressBar progresso in progressosForm)
            {
                progresso.Visible = false;
                progresso.Value = 0;
            }
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if ( MessageBox.Show("Confirma término do processo de Importação?", 
                "Importação Série Histórica", MessageBoxButtons.YesNo, MessageBoxIcon.Question, 
                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        private void btnSelecioneArquivo_Click(object sender, EventArgs e)
        {
            arquivosImportacao.Clear();
            foreach (Label arquivoForm in arquivosForm)
                arquivoForm.Text = "";
            foreach (ProgressBar progresso in progressosForm)
            {
                progresso.Visible = false;
                progresso.Value = 0;
            }

            if (arquivo.ShowDialog() == DialogResult.OK)
            {
                foreach (String nomeArquivo in arquivo.FileNames)
                {
                    arquivosImportacao.Add(nomeArquivo);
                    arquivosForm[arquivosImportacao.Count-1].Text = 
                        Path.GetFileName(arquivosImportacao[arquivosImportacao.Count-1]);
                    if (arquivosImportacao.Count == 4)
                        break;
                }
                btnImportar.Enabled = true;
            }
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            btnImportar.Enabled = false;
            txtProcessamento.Clear();
            txtProcessamento.ForeColor = System.Drawing.Color.Black;

            logBox("***** Iniciando Importação! *****", 1, false);

            if (AbrirConexaoBancoDados() == true)
            {
                ConsistirArquivos();
                //serieHistorica.IniciarTransacao("SerieHistorica");
                //if (ConsistirArquivos() == true)
                //    serieHistorica.FinalizarTransacao();
                //else
                //    serieHistorica.CancelarTransacao();
            }

            FecharConexaoBancoDados();
            logBox("***** Finalizando Importação! *****", 2, false);
        }

        private bool ConsistirArquivos()
        {
            foreach (String arquivo in arquivosImportacao)
            {
                int totalLinhas = 0;
                try
                {
                    totalLinhas = File.ReadLines(arquivo).Count();
                }
                catch (Exception ex)
                {
                    logBox("Falha ao acessar arquivo[" + arquivo + "]: " + ex.Message, 1, true);
                    return false;
                }

                StreamReader arq = null;
                try
                {
                    arq = File.OpenText(arquivo);
                }
                catch (Exception ex)
                {
                    logBox("Falha na abertura do arquivo[" + arquivo + "]: " + ex.Message, 1, true);
                    return false;
                }

                logBox("Lendo arquivo [" + Path.GetFileName(arquivo) + "]", 2, false);
                int totalColunas = 0;
                int contLinhas = 0;
                String linha = null;
                List<String> colunas = null;
                List<String> campos = null;
                while ((linha = arq.ReadLine()) != null)
                {
                    if (colunas == null)
                    {
                        colunas = new List<string>(linha.Split(';'));
                        totalColunas = colunas.Count;
                        switch (totalColunas)
                        {
                            // 15 colunas - Layout CotPapel
                            case 15:
                                logBox("Tratando arquivo com layout <CotPapel>", 1, false);
                                break;

                            // 54 colunas - Layout CotBMF
                            case 54:
                                logBox("Tratando arquivo com layout <CotBMF>", 1, false);
                                break;

                            // 34 colunas - Layout CotIndice
                            case 34:
                                logBox("Tratando arquivo com layout <CotIndice>", 1, false);
                                break;

                            // 11 colunas - Layout Proventos
                            case 11:
                                logBox("Tratando arquivo com layout <Proventos>", 1, false);
                                break;

                            default:
                                logBox("Arquivo com Layout indefinido!", 1, true);
                                return false;
                        }
                        progressosForm[arquivosImportacao.IndexOf(arquivo)].Visible = true;
                        continue;
                    }

                    campos = new List<string>(linha.Split(';'));
                    if (campos.Count != totalColunas)
                    {
                        logBox("Linha [" + (contLinhas + 1) + "]: linha com dados inválidos!", 1, true);
                        return false;
                    }
                    contLinhas++;
                    progressosForm[arquivosImportacao.IndexOf(arquivo)].Value = 
                        (int)(((float)contLinhas / totalLinhas) * 100);
                    progressosForm[arquivosImportacao.IndexOf(arquivo)].Refresh();

                    try
                    {
                        //serieHistorica.InserirHistorico(colunas, linha);
                        serieHistorica.AlterarHistorico(colunas, linha);
                    }
                    catch (Exception ex)
                    {
                        logBox("Linha [" + (contLinhas + 1) + "]: Falha na inserção no Banco: " + ex.Message, 1, true);
                        return false;
                    }
                }
                logBox("Arquivo OK! - Total de linhas lidas: " + contLinhas, 1, false);
            }
            return true;
        }

        private bool FecharConexaoBancoDados()
        {
            try
            {
                serieHistorica.FecharConexao();
                logBox("Conexão fechada OK!", 2, false);
                return true;
            }
            catch (Exception ex)
            {
                btnImportar.Enabled = true;
                logBox("Falha no fechamento da conexão [" + ex.Message + "]", 2, true);
                return false;
            }
        }

        private bool AbrirConexaoBancoDados()
        {
            // Efetuando conexão com Banco de Dados
            servidorIP = txtServidorIP.Text;
            bancoDados = txtBancoDados.Text;
            usuario = txtUsuario.Text;
            senha = txtSenha.Text;

            String connectionString =
                "Data Source=" + servidorIP + ";Initial Catalog=" + bancoDados +
                ";User Id=" + usuario + ";Password=" + senha + ";";

            serieHistorica = new SerieHistoricaDados();
            try
            {
                serieHistorica.AbrirConexao(connectionString);
                logBox("Conexão aberta com Banco OK!", 2, false);
                return true;
            }
            catch (Exception ex)
            {
                logBox("Falha na conexão [" + ex.Message + "]", 2, true);
                btnImportar.Enabled = true;
                return false;
            }
        }

        private void logBox(String mensagem, int proximasLinhas, bool falha)
        {
            if ( falha )
                txtProcessamento.ForeColor = System.Drawing.Color.Red;
            txtProcessamento.Text = txtProcessamento.Text + 
                new StringBuilder().Insert(0, Environment.NewLine, proximasLinhas).ToString() + 
                mensagem;
            
            txtProcessamento.Refresh();
        }
    }
}
