using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data.OracleClient;
using System.Configuration;
using System.IO;

namespace Gradual.Servico.ExportacaoGoldmanSachs.Teste
{
    public partial class frmPrincipal : Form
    {
        #region Globais

        ServicoExportacaoGoldmanSachs gServico = null;

        private DataTable gTabelaGsOpcao, gTabelaGsVista, gTabelaGsFuturo;

        private DataTable gTabelaGsOpcaoDaSimulacaoAtual, gTabelaGsVistaDaSimulacaoAtual, gTabelaGsFuturoDaSimulacaoAtual;

        private DateTime gDataHoraSimulacaoAtual;

        //private DateTime gDataHoraMaximaSimulacaoAtual;

        private double gIncrementoDeMinutosSimulacaoAtual;

        private bool gFlagRodarSimulacao;

        #endregion

        #region Constructors

        public frmPrincipal()
        {
            InitializeComponent();

            dtpHoraInicial.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
        }

        #endregion

        #region Métodos Private

        private void MensagemDeStatusFormat(string pMensagem, params object[] pParams)
        {
            MensagemDeStatus(string.Format(pMensagem, pParams));
        }

        private void MensagemDeStatus(string pMensagem)
        {
            txtStatus.Text = string.Format("{0}\r\n{1}", pMensagem, txtStatus.Text);

            Application.DoEvents();
        }

        private DataTable BuscarTabela(string pQuery, DateTime pData)
        {
            DataTable lRetorno = null;

            AcessaDados lDados = new AcessaDados();

            DbCommand lComando;

            string lQuery = string.Format(pQuery, pData.ToString("dd/MM/yyyy"), txtCodBovespa.Text.Trim());

            try
            {
                lDados.ConnectionStringName = "SINACOR"; //ConfigurationManager.AppSettings["ConexaoEmUso"];

                lComando = lDados.CreateCommand(System.Data.CommandType.Text, lQuery);

                lRetorno = lDados.ExecuteDbDataTable(lComando);
            }
            catch (Exception ex)
            {
                MensagemDeStatusFormat("Erro ao buscar tabela: [{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        private DataTable BuscarTabelaExecutandoProcedure(string pQuery, DateTime pData)
        {
            DataTable lRetorno = new DataTable();

            //AcessaDados lDados = new AcessaDados();
            //DbCommand lComando;
            //DbTransaction lTrans;

            OracleConnection lConnection = new OracleConnection( ConfigurationManager.ConnectionStrings[ ConfigurationManager.AppSettings["ConexaoEmUso"] ].ConnectionString );

            OracleTransaction lTrans;
            OracleCommand lCmd;

            OracleDataAdapter lAdpt;

            string lQuery = string.Format(pQuery, pData.ToString("dd/MM/yyyy"), txtCodBMF.Text.Trim());

            string lComando;

            lConnection.Open();

            lTrans = lConnection.BeginTransaction(); //check isolation level.

            try
            {
                lComando = string.Format(" corrwin.POMF_GERA_FAX.FAX ('{0}', {1}, {1}, NULL, NULL, NULL, 0, 'GSP-WKS-269', 0, 0,'C','NULL')", gDataHoraSimulacaoAtual.ToString("dd/MM/yyyy"), txtCodBMF.Text.Trim());

                lCmd = new OracleCommand( lComando, lConnection, lTrans);

                // será que daria pra fazer um count pra pelo menos tentar deduzir se a proc deveria voltar mais de um registro, pra comparar e tentar novamente? ha!

                lCmd.CommandType = CommandType.StoredProcedure;

                MensagemDeStatusFormat("Rodando procedure >> {0}", lComando);

                lCmd.ExecuteNonQuery();

                MensagemDeStatus("Procedure executada sem erro.");

                lCmd = new OracleCommand(lQuery, lConnection, lTrans);

                lAdpt = new OracleDataAdapter(lCmd);

                MensagemDeStatus("Preenchendo tabela com adapter Oracle...");

                lAdpt.Fill(lRetorno);

                lTrans.Commit();

                MensagemDeStatusFormat("Commit executado; [{0}] linhas retornadas.", lRetorno.Rows.Count);
            }
            catch (Exception ex)
            {
                MensagemDeStatusFormat("Erro! ROLLBACK!!");

                lTrans.Rollback();

                MensagemDeStatusFormat("Erro ao buscar tabela: [{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }
            finally
            {
                lConnection.Close();
            }

            return lRetorno;
        }

        private void CarregarTabelas()
        {
            MensagemDeStatus("Carregando tabelas...");

            gDataHoraSimulacaoAtual = dtpDiaDoTeste.Value;

            gTabelaGsOpcao  = BuscarTabela(SqlDasViews.V_GSOPCAO,       gDataHoraSimulacaoAtual);
            gTabelaGsVista  = BuscarTabela(SqlDasViews.V_GSVISTA,       gDataHoraSimulacaoAtual);
            gTabelaGsFuturo = BuscarTabela(SqlDasViews.V_GSFUTURO_NOVA, gDataHoraSimulacaoAtual);
            
            grdGsOpcao.DataSource = gTabelaGsOpcao;
            grdGsVista.DataSource = gTabelaGsVista;
            grdGsFuturo.DataSource = gTabelaGsFuturo;

            MensagemDeStatusFormat("Total de linhas: GsOpcao [{0}] GsVista [{1}] GsFuturo [{2}]"
                                    , gTabelaGsOpcao.Rows.Count
                                    , gTabelaGsVista.Rows.Count
                                    , gTabelaGsFuturo.Rows.Count);
        }

        private void FiltrarTabelasConformeDataHoraDaSimulacao()
        {
            DataView lView;

            string lFormula = "CONVERT(   SUBSTRING(HH_NEGOCIO, 1, 2) + SUBSTRING(HH_NEGOCIO, 4, 2),  'System.Int32') <= {0}";

            //string lFormula = "CONVERT(   SUBSTRING(HH_NEGOCIO, 0, 2),  Int32) <= {0}";

            lFormula = string.Format(lFormula, gDataHoraSimulacaoAtual.ToString("HHmm"));

            lView = new DataView(gTabelaGsFuturo, lFormula, null, DataViewRowState.CurrentRows);

            gTabelaGsFuturoDaSimulacaoAtual = lView.ToTable();

            lView = new DataView(gTabelaGsOpcao, lFormula, null, DataViewRowState.CurrentRows);

            gTabelaGsOpcaoDaSimulacaoAtual = lView.ToTable();

            lView = new DataView(gTabelaGsVista, lFormula, null, DataViewRowState.CurrentRows);

            gTabelaGsVistaDaSimulacaoAtual = lView.ToTable();
        }

        private void IniciarSimulacao()
        {
            MensagemDeStatus("Iniciando simulação...");

            gDataHoraSimulacaoAtual = new DateTime(gDataHoraSimulacaoAtual.Year, gDataHoraSimulacaoAtual.Month, gDataHoraSimulacaoAtual.Day, dtpHoraInicial.Value.Hour, dtpHoraInicial.Value.Minute, 0);

            gIncrementoDeMinutosSimulacaoAtual = Convert.ToDouble(cboIntervalo.Text);

            MensagemDeStatusFormat("Considerando início em [{0}], com verificação a cada [{1}] minutos"
                                    , gDataHoraSimulacaoAtual.ToString("dd/MM/yy HH:mm")
                                    , gIncrementoDeMinutosSimulacaoAtual);

            gFlagRodarSimulacao = true;

            tmrRodarSimulacao.Enabled = true;
            tmrRodarSimulacao.Start();
        }

        private void PararSimulacao()
        {
            MensagemDeStatus("Parando simulação...");

            gFlagRodarSimulacao = false;

            tmrRodarSimulacao.Enabled = false;
        }

        private void RodarUmPassoDaSimulacao()
        {
            gFlagRodarSimulacao = false;

            gDataHoraSimulacaoAtual = gDataHoraSimulacaoAtual.AddMinutes(gIncrementoDeMinutosSimulacaoAtual);

            MensagemDeStatusFormat("Considerando que a hora é [{0}]", gDataHoraSimulacaoAtual.ToString("HH:mm"));

            FiltrarTabelasConformeDataHoraDaSimulacao();

            gServico.AssumirDadosDeTeste(gDataHoraSimulacaoAtual, gTabelaGsOpcaoDaSimulacaoAtual, gTabelaGsVistaDaSimulacaoAtual, gTabelaGsFuturoDaSimulacaoAtual);

            gServico.RealizarUmaVerificacao();

            gFlagRodarSimulacao = true;
        }

        private void BuscarNasTabelas(string pNomeDoCampo, string pValor, DateTime pDataInicial, DateTime pDataFinal)
        {
            DateTime lDataAtual = pDataFinal;

            DataTable lOpcao, lVista, lFuturo;

            int lContador = 0;

            while (lDataAtual >= pDataInicial)
            {
                gServico_OnMensagemDeLog("B", string.Format("BUSCA em [{0}]==========================>", lDataAtual));

                gServico_OnMensagemDeLog("B", string.Format("Carregando busca em V_GSOPCAO em [{0}]", lDataAtual));

                lOpcao  = BuscarTabela(SqlDasViews.V_GSOPCAO,       lDataAtual);

                gServico_OnMensagemDeLog("B", string.Format("Carregando busca em V_GSVISTA em [{0}]", lDataAtual));

                lVista  = BuscarTabela(SqlDasViews.V_GSVISTA,       lDataAtual);

                gServico_OnMensagemDeLog("B", string.Format("Carregando busca em V_GSFUTURO em [{0}]", lDataAtual));

                lFuturo = BuscarTabela(SqlDasViews.V_GSFUTURO_NOVA, lDataAtual);

                foreach (DataRow lRow in lOpcao.Rows)
                {
                    try
                    {
                        if (lRow[pNomeDoCampo] != DBNull.Value)
                        {
                            if (lRow[pNomeDoCampo].ToString() == pValor)
                            {
                                gServico_OnMensagemDeLog("B", string.Format("ENCONTRADO [{1}][{2}] na tabela V_GSOPCAO em [{0}], parando."
                                                            , lDataAtual
                                                            , pNomeDoCampo
                                                            , pValor));

                                break;
                            }
                        }

                        lContador++;
                    }
                    catch (Exception ex1)
                    {
                        gServico_OnMensagemDeLog("B", string.Format("Erro ao buscar [{1}][{2}] na tabela V_GSFUTURO em [{0}], parando. [{3}]"
                                                    , lDataAtual
                                                    , pNomeDoCampo
                                                    , pValor
                                                    , ex1.Message));

                        break;
                    }
                }

                gServico_OnMensagemDeLog("B", string.Format("Pesquisados [{0}] de [{1}] registros em V_GSOPCAO dia [{2}]."
                                            , lContador
                                            , lOpcao.Rows.Count
                                            , lDataAtual));

                lContador = 0;

                foreach (DataRow lRow in lVista.Rows)
                {
                    try
                    {
                        if (lRow[pNomeDoCampo] != DBNull.Value)
                        {
                            if (lRow[pNomeDoCampo] == pValor)
                            {
                                gServico_OnMensagemDeLog("B", string.Format("ENCONTRADO [{1}][{2}] na tabela V_GSVISTA em [{0}], parando."
                                                            , lDataAtual
                                                            , pNomeDoCampo
                                                            , pValor));

                                break;
                            }
                        }

                        lContador++;
                    }
                    catch (Exception ex2)
                    {
                        gServico_OnMensagemDeLog("B", string.Format("Erro ao buscar [{1}][{2}] na tabela V_GSVISTA em [{0}], parando. [{3}]"
                                                    , lDataAtual
                                                    , pNomeDoCampo
                                                    , pValor
                                                    , ex2.Message));

                        break;
                    }
                }
                
                gServico_OnMensagemDeLog("B", string.Format("Pesquisados [{0}] de [{1}] registros em V_GSVISTA dia [{2}]."
                                            , lContador
                                            , lVista.Rows.Count
                                            , lDataAtual));

                lContador = 0;

                foreach (DataRow lRow in lFuturo.Rows)
                {
                    try
                    {
                        if (lRow[pNomeDoCampo] != DBNull.Value)
                        {
                            if (lRow[pNomeDoCampo] == pValor)
                            {
                                gServico_OnMensagemDeLog("B", string.Format("ENCONTRADO [{1}][{2}] na tabela V_GSOPCAO em [{0}], parando."
                                                            , lDataAtual
                                                            , pNomeDoCampo
                                                            , pValor));

                                break;
                            }
                        }

                        lContador++;
                    }
                    catch (Exception ex3)
                    {
                        gServico_OnMensagemDeLog("B", string.Format("Erro ao buscar [{1}][{2}] na tabela V_GSFUTURO em [{0}], parando. [{3}]"
                                , lDataAtual
                                , pNomeDoCampo
                                , pValor
                                , ex3.Message));

                        break;
                    }
                }

                gServico_OnMensagemDeLog("B", string.Format("Pesquisados [{0}] de [{1}] registros em V_GSFUTURO dia [{2}]."
                                            , lContador
                                            , lFuturo.Rows.Count
                                            , lDataAtual));

                lContador = 0;


                lDataAtual = lDataAtual.AddDays(-1);
            }


        }

        private void GerarArquivoOpcao(string pCaminho)
        {
            StringBuilder lBuilder = new StringBuilder();

            if (gTabelaGsOpcao == null)
            {
                MensagemDeStatus("Carregando tabela para buscar o formato dos campos...");
                CarregarTabelas();
            }

            MensagemDeStatus("Instanciando linha...");

            DataRow lRow = gTabelaGsOpcao.NewRow();

            lRow[0] = txtGA_O_NR_NEGOCIO.Text;
            lRow[1] = txtGA_O_DT_DATAORD.Text;
            lRow[2] = txtGA_O_HH_NEGOCIO.Text;
            lRow[3] = txtGA_O_CD_NEGOCIO.Text;
            lRow[4] = txtGA_O_LOTE.Text;
            lRow[5] = txtGA_O_CD_NATOPE.Text;
            lRow[6] = txtGA_O_DT_OPCOES.Text;
            lRow[7] = txtGA_O_QT_QTDESP.Text;
            lRow[8] = txtGA_O_VL_NEGOCIO.Text;
            lRow[9] = txtGA_O_PRECO_EXEC.Text;
            lRow[10] = txtGA_O_PAGTO_PREMIO.Text;
            lRow[11] = txtGA_O_DATA_LIQUID.Text;
            lRow[12] = txtGA_O_ESTILO.Text;
            lRow[13] = txtGA_O_PRECO_FECH.Text;
            lRow[14] = txtGA_O_DATA_EXPIRA.Text;
            lRow[15] = txtGA_O_NR_ORDEM.Text;
            lRow[16] = txtGA_O_ISIN.Text;
            lRow[17] = txtGA_O_ROBO.Text;
            lRow[18] = txtGA_O_CD_CLIENTE.Text;

            if (gServico == null)
            {
                MensagemDeStatus("Instanciando serviço para gerar arquivo...");

                gServico = new ServicoExportacaoGoldmanSachs();

                gServico.IniciarServico();
            }

            MensagemDeStatus("Serviço escrevendo conteúdo do arquivo...");

            

            gServico.EscreverConteudoDoArquivo(lRow, "O", ref lBuilder);
            
            MensagemDeStatus("Salvando arquivo...");

            File.WriteAllText(pCaminho, lBuilder.ToString());

            MensagemDeStatusFormat("Arquivo [{0}] salvo com sucesso!", pCaminho);
        }

        private void GerarArquivoVista(string pCaminho)
        {
            StringBuilder lBuilder = new StringBuilder();

            if (gTabelaGsVista == null)
            {
                MensagemDeStatus("Carregando tabela para buscar o formato dos campos...");
                CarregarTabelas();
            }

            MensagemDeStatus("Instanciando linha...");

            DataRow lRow = gTabelaGsVista.NewRow();

            lRow[0] = txtGA_V_NR_NEGOCIO.Text;
            lRow[1] = txtGA_V_DT_DATAORD.Text;
            lRow[2] = txtGA_V_HH_NEGOCIO.Text;
            lRow[3] = txtGA_V_CD_NEGOCIO.Text;
            lRow[4] = txtGA_V_LOTE.Text;
            lRow[5] = txtGA_V_CD_NATOPE.Text;
            lRow[6] = txtGA_V_DT_OPCOES.Text;
            lRow[7] = txtGA_V_QT_QTDESP.Text;
            lRow[8] = txtGA_V_VL_NEGOCIO.Text;
            lRow[9] = txtGA_V_PRECO_EXEC.Text;
            lRow[10] = txtGA_V_DATA_EXPIRACAO.Text;
            lRow[11] = txtGA_V_NR_ORDEM.Text;
            lRow[12] = txtGA_V_ISIN.Text;
            lRow[13] = txtGA_V_ROBO.Text;
            lRow[14] = txtGA_V_CD_CLIENTE.Text;

            if (gServico == null)
            {
                MensagemDeStatus("Instanciando serviço para gerar arquivo...");

                gServico = new ServicoExportacaoGoldmanSachs();

                gServico.IniciarServico();
            }

            MensagemDeStatus("Serviço escrevendo conteúdo do arquivo...");

            gServico.EscreverConteudoDoArquivo(lRow, "V", ref lBuilder);
            
            MensagemDeStatus("Salvando arquivo...");

            File.WriteAllText(pCaminho, lBuilder.ToString());

            MensagemDeStatusFormat("Arquivo [{0}] salvo com sucesso!", pCaminho);
        }

        private void GerarArquivoFuturo(string pCaminho)
        {
            StringBuilder lBuilder = new StringBuilder();

            if (gTabelaGsFuturo == null)
            {
                MensagemDeStatus("Carregando tabela para buscar o formato dos campos...");
                CarregarTabelas();
            }

            MensagemDeStatus("Instanciando linha...");

            DataRow lRow = gTabelaGsFuturo.NewRow();

            lRow[0] = txtGA_B_NR_NEGOCIO.Text;
            lRow[1] = txtGA_B_DT_DATAORD.Text;
            lRow[2] = txtGA_B_HH_NEGOCIO.Text;
            lRow[3] = txtGA_B_CD_NEGOCIO.Text;
            lRow[4] = txtGA_B_LOTE.Text;
            lRow[5] = txtGA_B_CD_NATOPE.Text;
            lRow[6] = txtGA_B_DT_OPCOES.Text;
            lRow[7] = txtGA_B_QT_QTDESP.Text;
            lRow[8] = txtGA_B_VL_NEGOCIO.Text;
            lRow[9] = txtGA_B_PRECO_EXEC.Text;
            lRow[10] = txtGA_B_PAGTO_PREMIO.Text;
            lRow[11] = txtGA_B_DATA_LIQUID.Text;
            lRow[12] = txtGA_B_ESTILO.Text;
            lRow[13] = txtGA_B_PRECO_EXEC.Text;
            lRow[14] = txtGA_B_DATA_EXPIRA.Text;
            lRow[15] = txtGA_B_NR_ORDEM.Text;

            if (gServico == null)
            {
                MensagemDeStatus("Instanciando serviço para gerar arquivo...");

                gServico = new ServicoExportacaoGoldmanSachs();

                gServico.IniciarServico();
            }

            MensagemDeStatus("Serviço escrevendo conteúdo do arquivo...");

            gServico.EscreverConteudoDoArquivo(lRow, "F", ref lBuilder);
            
            MensagemDeStatus("Salvando arquivo...");

            File.WriteAllText(pCaminho, lBuilder.ToString());

            MensagemDeStatusFormat("Arquivo [{0}] salvo com sucesso!", pCaminho);
        }

        #endregion

        #region Event Handlers

        private void gServico_OnMensagemDeLog(string pTipo, string pMensagem)
        {
            MensagemDeStatusFormat(">{0}: {1}", pTipo, pMensagem);
        }

        private void btnCarregarTabelas_Click(object sender, EventArgs e)
        {
            btnCarregarTabelas.Enabled = false;

            CarregarTabelas();

            btnCarregarTabelas.Enabled = true;

            btnIniciarServico.Enabled = true;
        }

        private void btnIniciarServico_Click(object sender, EventArgs e)
        {
            btnCarregarTabelas.Enabled = false;

            gServico = new ServicoExportacaoGoldmanSachs();

            gServico.EmModoDeControleExterno = true;
            
            gServico.AssumirDadosDeTeste(gDataHoraSimulacaoAtual, gTabelaGsOpcao, gTabelaGsVista, gTabelaGsFuturo);

            gServico.OnMensagemDeLog += new MensagemDeLogEventHandler(gServico_OnMensagemDeLog);

            gServico.IniciarServico();

            btnIniciarPararSimulacao.Enabled = true;
        }

        private void btnIniciarPararSimulacao_Click(object sender, EventArgs e)
        {
            if (btnIniciarPararSimulacao.Text == "Iniciar Simulação")
            {
                btnIniciarPararSimulacao.Text = "Parar Simulação";

                btnIniciarServico.Enabled = false;
                btnPausar.Enabled = true;

                IniciarSimulacao();
            }
            else
            {
                if (MessageBox.Show("Parar simulação?", "Confirmar", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    btnIniciarPararSimulacao.Text = "Iniciar Simulação";

                    PararSimulacao();

                    btnCarregarTabelas.Enabled = true;
                    btnIniciarServico.Enabled = true;
                    btnPausar.Enabled = false;
                }
            }
        }

        private void tmrRodarSimulacao_Tick(object sender, EventArgs e)
        {
            if (gFlagRodarSimulacao)
            {
                RodarUmPassoDaSimulacao();
            }
        }

        private void btnPausar_Click(object sender, EventArgs e)
        {
            if (btnPausar.Text == "Pausar Simulação")
            {
                btnPausar.Text = "Continuar Simulação";

                gFlagRodarSimulacao = false;
            }
            else
            {
                btnPausar.Text = "Pausar Simulação";

                gFlagRodarSimulacao = true;
            }
        }

        private void grdGsOpcao_DoubleClick(object sender, EventArgs e)
        {
            if (grdGsOpcao.SelectedRows.Count > 0)
            {
                txtGA_O_NR_NEGOCIO.Text = grdGsOpcao.SelectedRows[0].Cells[0].Value.ToString();
                txtGA_O_DT_DATAORD.Text = grdGsOpcao.SelectedRows[0].Cells[1].Value.ToString();
                txtGA_O_HH_NEGOCIO.Text = grdGsOpcao.SelectedRows[0].Cells[2].Value.ToString();
                txtGA_O_CD_NEGOCIO.Text = grdGsOpcao.SelectedRows[0].Cells[3].Value.ToString();
                txtGA_O_LOTE.Text = grdGsOpcao.SelectedRows[0].Cells[4].Value.ToString();
                txtGA_O_CD_NATOPE.Text = grdGsOpcao.SelectedRows[0].Cells[5].Value.ToString();
                txtGA_O_DT_OPCOES.Text = grdGsOpcao.SelectedRows[0].Cells[6].Value.ToString();
                txtGA_O_QT_QTDESP.Text = grdGsOpcao.SelectedRows[0].Cells[7].Value.ToString();
                txtGA_O_VL_NEGOCIO.Text = grdGsOpcao.SelectedRows[0].Cells[8].Value.ToString();
                txtGA_O_PRECO_EXEC.Text = grdGsOpcao.SelectedRows[0].Cells[9].Value.ToString();
                txtGA_O_PAGTO_PREMIO.Text = grdGsOpcao.SelectedRows[0].Cells[10].Value.ToString();
                txtGA_O_DATA_LIQUID.Text = grdGsOpcao.SelectedRows[0].Cells[11].Value.ToString();
                txtGA_O_ESTILO.Text = grdGsOpcao.SelectedRows[0].Cells[12].Value.ToString();
                txtGA_O_PRECO_FECH.Text = grdGsOpcao.SelectedRows[0].Cells[13].Value.ToString();
                txtGA_O_DATA_EXPIRA.Text = grdGsOpcao.SelectedRows[0].Cells[14].Value.ToString();
                txtGA_O_NR_ORDEM.Text = grdGsOpcao.SelectedRows[0].Cells[15].Value.ToString();
                txtGA_O_ISIN.Text = grdGsOpcao.SelectedRows[0].Cells[16].Value.ToString();
                txtGA_O_ROBO.Text = grdGsOpcao.SelectedRows[0].Cells[17].Value.ToString();
                txtGA_O_CD_CLIENTE.Text = grdGsOpcao.SelectedRows[0].Cells[18].Value.ToString();

                tabTabelas.SelectedIndex = 3;
                tbGerarArquivo.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Favor selecionar uma linha inteira");
            }
        }
        
        private void grdGsVista_DoubleClick(object sender, EventArgs e)
        {
            if (grdGsVista.SelectedRows.Count > 0)
            {
                txtGA_V_NR_NEGOCIO.Text = grdGsVista.SelectedRows[0].Cells[0].Value.ToString();
                txtGA_V_DT_DATAORD.Text = grdGsVista.SelectedRows[0].Cells[1].Value.ToString();
                txtGA_V_HH_NEGOCIO.Text = grdGsVista.SelectedRows[0].Cells[2].Value.ToString();
                txtGA_V_CD_NEGOCIO.Text = grdGsVista.SelectedRows[0].Cells[3].Value.ToString();
                txtGA_V_LOTE.Text = grdGsVista.SelectedRows[0].Cells[4].Value.ToString();
                txtGA_V_CD_NATOPE.Text = grdGsVista.SelectedRows[0].Cells[5].Value.ToString();
                txtGA_V_DT_OPCOES.Text = grdGsVista.SelectedRows[0].Cells[6].Value.ToString();
                txtGA_V_QT_QTDESP.Text = grdGsVista.SelectedRows[0].Cells[7].Value.ToString();
                txtGA_V_VL_NEGOCIO.Text = grdGsVista.SelectedRows[0].Cells[8].Value.ToString();
                txtGA_V_PRECO_EXEC.Text = grdGsVista.SelectedRows[0].Cells[9].Value.ToString();
                txtGA_V_DATA_EXPIRACAO.Text = grdGsVista.SelectedRows[0].Cells[10].Value.ToString();
                txtGA_V_NR_ORDEM.Text = grdGsVista.SelectedRows[0].Cells[11].Value.ToString();
                txtGA_V_ISIN.Text = grdGsVista.SelectedRows[0].Cells[12].Value.ToString();
                txtGA_V_ROBO.Text = grdGsVista.SelectedRows[0].Cells[13].Value.ToString();
                txtGA_V_CD_CLIENTE.Text = grdGsVista.SelectedRows[0].Cells[14].Value.ToString();

                tabTabelas.SelectedIndex = 3;
                tbGerarArquivo.SelectedIndex = 1;
            }
            else
            {
                MessageBox.Show("Favor selecionar uma linha inteira");
            }
        }
        
        private void grdGsFuturo_DoubleClick(object sender, EventArgs e)
        {
            if (grdGsFuturo.SelectedRows.Count > 0)
            {
                txtGA_B_NR_NEGOCIO.Text = grdGsFuturo.SelectedRows[0].Cells[0].Value.ToString();
                txtGA_B_DT_DATAORD.Text = grdGsFuturo.SelectedRows[0].Cells[1].Value.ToString();
                txtGA_B_HH_NEGOCIO.Text = grdGsFuturo.SelectedRows[0].Cells[2].Value.ToString();
                txtGA_B_CD_NEGOCIO.Text = grdGsFuturo.SelectedRows[0].Cells[3].Value.ToString();
                txtGA_B_LOTE.Text = grdGsFuturo.SelectedRows[0].Cells[4].Value.ToString();
                txtGA_B_CD_NATOPE.Text = grdGsFuturo.SelectedRows[0].Cells[5].Value.ToString();
                txtGA_B_DT_OPCOES.Text = grdGsFuturo.SelectedRows[0].Cells[6].Value.ToString();
                txtGA_B_QT_QTDESP.Text = grdGsFuturo.SelectedRows[0].Cells[7].Value.ToString();
                txtGA_B_VL_NEGOCIO.Text = grdGsFuturo.SelectedRows[0].Cells[8].Value.ToString();
                txtGA_B_PRECO_EXEC.Text = grdGsFuturo.SelectedRows[0].Cells[9].Value.ToString();
                txtGA_B_PAGTO_PREMIO.Text = grdGsFuturo.SelectedRows[0].Cells[10].Value.ToString();
                txtGA_B_DATA_LIQUID.Text = grdGsFuturo.SelectedRows[0].Cells[11].Value.ToString();
                txtGA_B_ESTILO.Text = grdGsFuturo.SelectedRows[0].Cells[12].Value.ToString();
                txtGA_B_PRECO_EXEC.Text = grdGsFuturo.SelectedRows[0].Cells[13].Value.ToString();
                txtGA_B_DATA_EXPIRA.Text = grdGsFuturo.SelectedRows[0].Cells[14].Value.ToString();
                txtGA_B_NR_ORDEM.Text = grdGsFuturo.SelectedRows[0].Cells[15].Value.ToString();

                tabTabelas.SelectedIndex = 3;
                tbGerarArquivo.SelectedIndex = 2;
            }
            else
            {
                MessageBox.Show("Favor selecionar uma linha inteira");
            }
        }

        private void btnBusca_Buscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBusca_NomeCampo.Text))
            {
                if (!string.IsNullOrEmpty(txtBusca_Valor.Text))
                {
                    DateTime lDataInicial = dtpBusca_DataInicial.Value;
                    DateTime lDataFinal   = dtpBusca_DataFinal.Value;

                    if (lDataInicial > lDataFinal)
                    {
                        DateTime d = lDataInicial;

                        lDataInicial = lDataFinal;
                        lDataFinal = d;
                        //forever not remembering how to do this; glad memory is cheap.
                    }

                    if (lDataInicial == lDataFinal)
                        lDataInicial = lDataInicial.AddDays(-1);

                    BuscarNasTabelas(txtBusca_NomeCampo.Text, txtBusca_Valor.Text, lDataInicial, lDataFinal);
                }
                else
                {
                    MessageBox.Show("Favor preencher o valor da busca.");
                }
            }
            else
            {
                MessageBox.Show("Favor preencher um nome de campo.");
            }
        }

        private void btnGerarArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                if (dlgSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (tbGerarArquivo.SelectedIndex == 0)
                    {
                        GerarArquivoOpcao(dlgSave.FileName);
                    }
                    else if (tbGerarArquivo.SelectedIndex == 1)
                    {
                        GerarArquivoVista(dlgSave.FileName);
                    }
                    else
                    {
                        GerarArquivoFuturo(dlgSave.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MensagemDeStatusFormat("ERRO!\r\n[{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }
        }

        #endregion

    }
}
