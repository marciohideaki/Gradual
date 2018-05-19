using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using MdsBayeuxClient;
using System.Threading;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.CadastroCliente;
using Gradual.OMS.CadastroCliente.Lib;

namespace StockMarket.Excel2007
{
    public delegate void UsuarioLogadoEventHandler(string cliente);
    
    public delegate void UsuarioDeslogadoEventHandler();

    public delegate void RealizarAcaoEventHandler(string pAcao, string pParametro);

    public partial class ribStockMarket
    {
        #region Globais

        private List<string> gListaDeCarteiras = null;
        private List<string> gListaDeAtivos = null;

        private int gCodigoDeAcessoDoUsuario = -1;

        private const string gGuidDePermissaoDeAcessoAoStockMarket = "5DB513D0-C88F-4461-8FBA-63ABDAABFFD4";

        #endregion

        #region Eventos

        public event UsuarioLogadoEventHandler UsuarioLogado;

        private void OnUsuarioLogado(string cliente)
        {
            if (UsuarioLogado != null)
                UsuarioLogado(cliente);
        }

        public event UsuarioDeslogadoEventHandler UsuarioDeslogado;

        private void OnUsuarioDeslogado()
        {
            if (UsuarioDeslogado != null)
                UsuarioDeslogado();
        }

        public event RealizarAcaoEventHandler RealizarAcao;

        private void OnRealizarAcao(string pAcao, string pParametro)
        {
            if (RealizarAcao != null)
                RealizarAcao(pAcao, pParametro);
        }

        #endregion

        #region Métodos Private
        
        private WsAutenticacao.AutenticacaoSoapClient InstanciarClientAutenticacao()
        {
            string lUrl;

            WsAutenticacao.AutenticacaoSoapClient lRetorno;

            System.ServiceModel.BasicHttpBinding lBinding;

            System.ServiceModel.EndpointAddress lAddress;


            if (Debugger.IsAttached)
            {
                //"desenvolvimento":
                lUrl = System.Configuration.ConfigurationManager.AppSettings["UrlWSIntegracaoAutenticacaoDesenv"];

                //lUrl = "http://gsp-srv-wnhb01:8080/Gradual.WsIntegracao/Autenticacao.asmx";
                //lUrl = "http://10.0.11.100:8080/Gradual.WsIntegracao/Autenticacao.asmx";
                //http://10.0.11.100:8080/Gradual.WsIntegracao/Autenticacao.asmx
            }
            else
            {
                //produção
                lUrl = System.Configuration.ConfigurationManager.AppSettings["UrlWSIntegracaoAutenticacaoProd"];

                //lUrl = "http://localhost/Gradual.OMS.WsIntegracao/Autenticacao.asmx";
                //lUrl = "http://10.0.11.100:8080/Gradual.WsIntegracao/Autenticacao.asmx";
                //lUrl = "http://wsplataforma.gradualinvestimentos.com.br:8080/Gradual.WsIntegracao/Autenticacao.asmx";
            }

            lBinding = new System.ServiceModel.BasicHttpBinding();

            lAddress = new System.ServiceModel.EndpointAddress(lUrl);

            lRetorno = new WsAutenticacao.AutenticacaoSoapClient(lBinding, lAddress);

            return lRetorno;
        }
        
        private WsPlataforma.PlataformaSoapClient InstanciarClientPlataforma()
        {
            string lUrl;

            WsPlataforma.PlataformaSoapClient lRetorno;

            System.ServiceModel.BasicHttpBinding lBinding;

            System.ServiceModel.EndpointAddress lAddress;


            if (Debugger.IsAttached)
            {
                //"desenvolvimento":
                lUrl = System.Configuration.ConfigurationManager.AppSettings["UrlWSIntegracaoPlataformaDesenv"];

                //lUrl = "http://gsp-srv-wnhb01:8080/Gradual.WsIntegracao/Plataforma.asmx";
                //lUrl = "http://10.0.11.100:8080/Gradual.WsIntegracao/Autenticacao.asmx";
                //http://10.0.11.100:8080/Gradual.WsIntegracao/Autenticacao.asmx
            }
            else
            {
                //produção
                lUrl = System.Configuration.ConfigurationManager.AppSettings["UrlWSIntegracaoPlataformaProd"];

                //lUrl = "http://localhost/Gradual.OMS.WsIntegracao/Autenticacao.asmx";
                //lUrl = "http://10.0.11.100:8080/Gradual.WsIntegracao/Plataforma.asmx";
                //lUrl = "http://wsplataforma.gradualinvestimentos.com.br:8080/Gradual.WsIntegracao/Plataforma.asmx";
            }

            lBinding = new System.ServiceModel.BasicHttpBinding();

            lAddress = new System.ServiceModel.EndpointAddress(lUrl);

            lRetorno = new WsPlataforma.PlataformaSoapClient(lBinding, lAddress);

            return lRetorno;
        }

        private void RealizarLogin()
        {
            try
            {
                frmInputBox_Login lFormLogin = new frmInputBox_Login();

                if (lFormLogin.ShowDialog() == DialogResult.OK)
                {
                    string lCodigoEmail, lSenha;

                    lCodigoEmail = lFormLogin.CodigoEmail;
                    lSenha = lFormLogin.Senha;

                    btnAutenticacao_RealizarLogin.Visible = false;

                    lblMensagensDeAutenticacao.Label = "Realizando login, favor aguardar...";

                    Application.DoEvents();

                    System.Threading.Thread.Sleep(2000);

                    Application.DoEvents();

                    //TODO: REMOVER !!!! SOMENTE PARA DEV !!!     ==============================================================
                    /*
                    if (lCodigoEmail == "" && lSenha == "")
                    {
                        lCodigoEmail = "31940";
                        lSenha = "abc123";
                    }
                    */
                    // =========================================================================================================



                    //ServicoSegurancaSoapClient lWebServiceSeguranca = new ServicoSegurancaSoapClient();

                    WsAutenticacao.AutenticacaoSoapClient lServicoAutenticacao = InstanciarClientAutenticacao();

                    WsAutenticacao.AutenticarUsuarioRequest lRequest = new WsAutenticacao.AutenticarUsuarioRequest();
                    WsAutenticacao.AutenticarUsuarioResponse lResponse;

                    //gToolBarGradual_btnLogin.Caption = gLabelAguarde;

                    lRequest.CodigoOuEmailDoUsuario = lCodigoEmail;
                    lRequest.Senha = lSenha;
                    lRequest.Token = Criptografia.Criptografar(string.Format("{0}{1}", Environment.MachineName, new Random().Next(100, 999)), true);

                    lResponse = lServicoAutenticacao.AutenticarUsuario(lRequest);

                    if (lResponse.StatusResposta == "OK")
                    {
                        if(ClienteTemPermissaoDeAcesso(lResponse.IdLogin, lResponse.CodigoDaSessao))
                        {
                            lblMensagensDeAutenticacao.Label = "Login realizado com sucesso, buscando lista de carteiras...";

                            Application.DoEvents();

                            System.Threading.Thread.Sleep(1000);

                            gCodigoDeAcessoDoUsuario = Convert.ToInt32(lResponse.CodigoDeAcessoDoUsuario);

                            BuscarListaDeCarteirasEAtivos();

                            Interface_PreencherListaDeCarteiras();

                            Interface_HabilitarBotoesAposLogin();

                            OnUsuarioLogado(lResponse.CodigoDeAcessoDoUsuario);

                            lFormLogin.GravarConfiguracao();
                        }
                        else
                        {
                            MessageBox.Show("Você não tem permissão de acesso para utilizar o StockMarket.");

                            lblMensagensDeAutenticacao.Visible = false;

                            btnAutenticacao_RealizarLogin.Visible = true;
                        }
                    }
                    else
                    {
                        string lMensagem = string.Format("Erro de login: {0}\r\n\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta);

                        EventLogger.WriteFormat(EventLogEntryType.Warning, lMensagem);

                        MessageBox.Show(lMensagem);

                        lblMensagensDeAutenticacao.Visible = false;

                        btnAutenticacao_RealizarLogin.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erro ao realizar login: {0}\r\n\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private bool ClienteTemPermissaoDeAcesso(string pCodigoDoCliente, string pCodigoDaSessao)
        {
            try
            {
                WsAutenticacao.AutenticacaoSoapClient lServicoAutenticacao = InstanciarClientAutenticacao();

                WsAutenticacao.BuscarPermissoesDoUsuarioRequest lRequest = new WsAutenticacao.BuscarPermissoesDoUsuarioRequest();
                WsAutenticacao.BuscarPermissoesDoUsuarioResponse lResponse;

                //gToolBarGradual_btnLogin.Caption = gLabelAguarde;

                lRequest.CodigoDoUsuario = pCodigoDoCliente;
                lRequest.CodigoDaSessao = pCodigoDaSessao;

                lResponse = lServicoAutenticacao.BuscarPermissoesDoUsuario(lRequest);

                if (lResponse.StatusResposta == "OK")
                {
                    foreach (WsAutenticacao.PermissaoAssociadaInfo lPermissao in lResponse.Permissoes)
                    {
                        if (lPermissao.CodigoPermissao == gGuidDePermissaoDeAcessoAoStockMarket)
                            return true;
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("Erro de verificação de permissões: {0}\r\n\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erro ao verificar permissões de acesso: {0}\r\n\r\n{1}", ex.Message, ex.StackTrace));
            }

            return false;
        }

        private void BuscarListaDeCarteirasEAtivos()
        {
            try
            {
                if (gListaDeAtivos == null)
                {
                    WsPlataforma.PlataformaSoapClient lServicoPlataforma = InstanciarClientPlataforma();

                    WsPlataforma.BuscarCarteirasComAtivosRequest lRequest = new WsPlataforma.BuscarCarteirasComAtivosRequest();
                    WsPlataforma.BuscarCarteirasComAtivosResponse lResponse;

                    lRequest.CodigoDoUsuario = gCodigoDeAcessoDoUsuario.ToString();

                    lResponse = lServicoPlataforma.BuscarCarteirasComAtivos(lRequest);

                    if (lResponse.StatusResposta == "OK")
                    {
                        lblMensagensDeAutenticacao.Label = "Lista de carteiras carregada com sucesso.";

                        Application.DoEvents();

                        System.Threading.Thread.Sleep(1000);

                        gListaDeCarteiras = new List<string>(lResponse.Carteiras);
                        gListaDeAtivos = new List<string>(lResponse.Ativos);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Erro ao buscar lista de carteiras: {0}\r\n\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erro ao buscar lista de carteiras: {0}\r\n\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private bool CelulaAtualContemFormulaSM(out string pFormula)
        {
            pFormula = null;

            try
            {
                if (Globals.ThisAddIn.Application.ActiveSheet != null && Globals.ThisAddIn.Application.ActiveCell != null)
                {
                    string lFormula = Convert.ToString(Globals.ThisAddIn.Application.ActiveCell.FormulaArray) + "";

                    if (lFormula.StartsWith("=" + Funcoes.FUNCAO_COTACAO_LINHA)
                     || lFormula.StartsWith("=" + Funcoes.FUNCAO_COTACAO_RAPIDA)
                     || lFormula.StartsWith("=" + Funcoes.FUNCAO_LIVRO_OFERTAS))
                    {
                        pFormula = lFormula.Substring(1, lFormula.IndexOf('(') - 1);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em CelulaAtualContemFormulaSM: {0}", ex.Message));
            }

            return false;
        }

        #endregion

        #region Métodos Private - Interface

        private void Interface_HabilitarBotoesAposLogin()
        {
            btnAutenticacao_RealizarLogin.Visible = false;

            lblMensagensDeAutenticacao.Visible = false;

            mnuOpcoes.Visible = true;

            grpCotacao.Visible = true;
            grpCarteiras.Visible = true;
            grpEstilos.Visible = true;
            grpOrdens.Visible = true;
        }

        private void Interface_PreencherListaDeCarteiras()
        {
            cboCarteiras_Carteiras.Items.Clear();

            if (gListaDeCarteiras != null)
            {
                foreach (string lCarteira in gListaDeCarteiras)
                {
                    cboCarteiras_Carteiras.Items.Add(this.Factory.CreateRibbonDropDownItem());

                    cboCarteiras_Carteiras.Items[cboCarteiras_Carteiras.Items.Count - 1].Label = lCarteira;
                }

                if (cboCarteiras_Carteiras.Items.Count > 0)
                {
                    lblCarteiras_ImportandoLista.Visible = false;

                    cboCarteiras_Carteiras.Visible = true;
                    btnCarteira_ImportarAtivos.Visible = true;
                }
                else
                {
                    lblCarteiras_ImportandoLista.Visible = true;
                    lblCarteiras_ImportandoLista.Label = "(sem carteiras cadastradas)";

                    cboCarteiras_Carteiras.Visible = false;
                    btnCarteira_ImportarAtivos.Visible = false;
                }
            }
        }
        
        private void Interface_AplicarEstilo()
        {
            // Verifica se a célula atual tem alguma das nossas fórmulas:

            string lFormulaAtual;

            if (CelulaAtualContemFormulaSM(out lFormulaAtual))
            {
                // acha a primeira célula (topo à esquerda) que tem a mesma fórmula
                int lRow, lCol;

                lRow = Globals.ThisAddIn.Application.ActiveCell.Row;
                lCol = Globals.ThisAddIn.Application.ActiveCell.Column;
                
                //vai pra esquerda:

                while (lRow > 1 && Convert.ToString(((Microsoft.Office.Interop.Excel.Range)((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[lRow - 1, lCol]).FormulaArray).Contains(lFormulaAtual))
                {
                    lRow--;
                }

                //vai pra cima:

                while (lCol > 1 && Convert.ToString(((Microsoft.Office.Interop.Excel.Range)((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[lRow, lCol - 1]).FormulaArray).Contains(lFormulaAtual))
                {
                    lCol--;
                }

                frmInputBox_SelecionarEstilo lForm = new frmInputBox_SelecionarEstilo();

                if (lFormulaAtual == Funcoes.FUNCAO_COTACAO_LINHA)
                {
                    lForm.TipoDeEstiloParaSelecionar = "monitor";

                    if (lForm.ShowDialog() == DialogResult.OK)
                    {
                        Interface_AplicarEstilo_MonitorCotacao(lForm.EstiloSelecionado, lRow, lCol, lFormulaAtual);
                    }
                }
                else if (lFormulaAtual == Funcoes.FUNCAO_COTACAO_RAPIDA)
                {
                    lForm.TipoDeEstiloParaSelecionar = "ticker";

                    if (lForm.ShowDialog() == DialogResult.OK)
                    {
                        Interface_AplicarEstilo_TickerCotacaoRapida(lForm.EstiloSelecionado, lRow + 1, lCol, lFormulaAtual);
                    }
                }
                else if (lFormulaAtual == Funcoes.FUNCAO_LIVRO_OFERTAS)
                {
                    lForm.TipoDeEstiloParaSelecionar = "livro";

                    if (lForm.ShowDialog() == DialogResult.OK)
                    {
                        Interface_AplicarEstilo_LivroOfertas(lForm.EstiloSelecionado, lRow + 2, lCol, lFormulaAtual);
                    }
                }
            }
            else
            {
                MessageBox.Show("A célula atual não parece estar dentro de um range que contenha alguma fórmula Stock Market.\r\n\r\nPara aplicar um estilo, primeiro selecione uma célula que esteja dentro do corpo de um monitor de cotações / ticker ou livro de ofertas,\r\ndepois clique em 'Aplicar Estilo...'");
            }
        }

        private void Interface_AplicarEstilo_MonitorCotacao(byte pEstilo, int pRow, int pCol, string pFormula)
        {
            //MessageBox.Show(string.Format("Aplicando formatação de cotação a partir de {0}, {1}", pRow, pCol));

            // Cabeçalho:

            Microsoft.Office.Interop.Excel.Range lRangeCabecalho, lRangeCorpo, lRangeColunaPapeis;

            Color lCorCabecalho, lCorInteriorCabecalho, lCorInteriorCelulas;

            lRangeCabecalho = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow - 1, pCol + 0],
                                                                                   ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow - 1, pCol + 21]];

            //Pra pegar o corpo, precisa ir verificando nas linhas abaixo até achar uma que não tenha a fórmula:

            int lQtdLinhas = 0;

            while (((Microsoft.Office.Interop.Excel.Range)((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + lQtdLinhas, pCol + 0]).FormulaArray.ToString().Contains(pFormula))
                lQtdLinhas++;

            lRangeCorpo = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 0, pCol + 0],
                                                                               ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 21]];

            lRangeColunaPapeis = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 0, pCol + 0],
                                                                               ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 0]];

            lCorCabecalho = Color.White;
            lCorInteriorCabecalho = Color.FromArgb(165, 165, 165);
            lCorInteriorCelulas = Color.White;

            if (pEstilo == 1)
            {
                // Cabeçalho:

                lRangeCabecalho.Font.Size = 8;
                lRangeCabecalho.Font.Bold = true;
                lRangeCabecalho.Font.Color = Color.Black;

                lRangeCabecalho.Interior.Color = Color.White;

                lRangeCabecalho.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                // linhas:

                lRangeCorpo.Font.Color = Color.Black;

                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle         = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle        = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle       = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

                lRangeCorpo.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                lRangeCorpo.Interior.Color = Color.White;

                // coluna que tem os nomes dos papéis:

                lRangeColunaPapeis.Font.Bold = true;

                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            }
            else if (pEstilo == 2)
            {
                lCorCabecalho = Color.White;
                lCorInteriorCabecalho = Color.FromArgb(165, 165, 165);
                lCorInteriorCelulas = Color.White;
            }
            else if (pEstilo == 3)
            {
                lCorCabecalho = Color.White;
                lCorInteriorCabecalho = Color.FromArgb( 74,  69,  42);
                lCorInteriorCelulas   = Color.FromArgb(238, 236, 225);
            }
            else if (pEstilo == 4)
            {
                lCorCabecalho = Color.White;
                lCorInteriorCabecalho = Color.FromArgb( 79,  98,  40);
                lCorInteriorCelulas   = Color.FromArgb(234, 241, 221);
            }
            else if (pEstilo == 5)
            {
                lCorCabecalho = Color.White;
                lCorInteriorCabecalho = Color.FromArgb( 23,  55,  93);
                lCorInteriorCelulas   = Color.FromArgb(219, 229, 241);
            }
            else if (pEstilo == 6)
            {
                lCorCabecalho = Color.White;
                lCorInteriorCabecalho = Color.FromArgb( 99,  37,  35);
                lCorInteriorCelulas   = Color.FromArgb(238, 236, 225);
            }

            if (pEstilo != 1)
            {
                // Cabeçalho:

                lRangeCabecalho.Font.Size = 8;
                lRangeCabecalho.Font.Bold = true;
                lRangeCabecalho.Font.Color = lCorCabecalho;
                
                lRangeCabecalho.Interior.Color = lCorInteriorCabecalho;

                lRangeCabecalho.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle    = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle  = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle   = Microsoft.Office.Interop.Excel.Constants.xlSolid;

                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                // linhas:

                lRangeCorpo.Font.Color = Color.Black;
                lRangeCorpo.Interior.Color = lCorInteriorCelulas;

                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle         = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle        = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle       = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle   = Microsoft.Office.Interop.Excel.Constants.xlSolid;

                lRangeCorpo.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle       = -4119;

                lRangeCorpo.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // coluna que tem os nomes dos papéis:

                lRangeColunaPapeis.Font.Bold = true;

                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

                // coluna de preço de compra:

                lRangeColunaPapeis = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 0, pCol + 4],
                                                                                                                    ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 4]];

                lRangeColunaPapeis.Font.Bold = true;

                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

                // coluna de preço de venda:

                lRangeColunaPapeis = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 0, pCol + 6],
                                                                                                                    ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + lQtdLinhas - 1, pCol + 6]];

                lRangeColunaPapeis.Font.Bold = true;

                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeColunaPapeis.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            }
        }

        private void Interface_AplicarEstilo_TickerCotacaoRapida(byte pEstilo, int pRow, int pCol, string pFormula)
        {
            Microsoft.Office.Interop.Excel.Range lRangeCabecalho = null;
            Microsoft.Office.Interop.Excel.Range lRangeLabels1, lRangeLabels2, lRangeValores1, lRangeValores2;

            Color lCorDosLabes, lCorInteriorDosLabels, lCorDoCabecalho, lCorInteriorDoCabecalho;

            if(pRow > 1)
                lRangeCabecalho = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow - 1, pCol + 0],
                                                                                       ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow - 1, pCol + 3]];

            lRangeLabels1 = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow,      pCol],
                                                                                 ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 9,  pCol]];

            lRangeValores1 = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow,     pCol + 1],
                                                                                  ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 9, pCol + 1]];

            lRangeLabels2 = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow,      pCol + 2],
                                                                                 ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 9,  pCol + 2]];

            lRangeValores2 = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow,     pCol + 3],
                                                                                  ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 9, pCol + 3]];

            // características comuns a todos os templates de ticker:

            //alinhamentos:

            if(lRangeCabecalho != null)
                lRangeCabecalho.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            lRangeLabels1.HorizontalAlignment  = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            lRangeLabels2.HorizontalAlignment  = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            lRangeValores1.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            lRangeValores2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

            //tamanho da fonte pros labels:

            lRangeLabels1.Font.Size = 8;
            lRangeLabels2.Font.Size = 8;

            //bold pro header:
            if(lRangeCabecalho != null)
                lRangeCabecalho.Font.Bold = true;

            //bold para ultima e variacao:

            ((Microsoft.Office.Interop.Excel.Range)((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow, pCol + 1]).Font.Bold = true;
            ((Microsoft.Office.Interop.Excel.Range)((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow, pCol + 3]).Font.Bold = true;

            //borda ao redor do "quadrado" geral:

            lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle     = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle   = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle  = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle    = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle     = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle   = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle  = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle    = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle    = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle  = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle   = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle    = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle  = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle   = Microsoft.Office.Interop.Excel.Constants.xlSolid;


            // cor da fonte preto pros valores:

            lRangeValores1.Font.ColorIndex = 1;
            lRangeValores2.Font.ColorIndex = 1;
            
            lCorDosLabes = Color.DarkGray;
            lCorInteriorDosLabels = Color.FromArgb(242, 242, 242);
            
            lCorDoCabecalho = Color.Black;
            lCorInteriorDoCabecalho = Color.Transparent;

            if (pEstilo == 1)
            {
                //linha dupla na borda inferior do cabeçalho:
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = -4119;

                lRangeCabecalho.Interior.Color = lCorInteriorDoCabecalho;
                lRangeCabecalho.Font.Color     = lCorDoCabecalho;

                // cor da fonte dos labels:
                lRangeLabels1.Font.ColorIndex = 16;  //gray 50%
                lRangeLabels2.Font.ColorIndex = 16;  //gray 50%

                //bold pros labels:

                lRangeLabels1.Font.Bold = true;
                lRangeLabels2.Font.Bold = true;
            }
            else if (pEstilo == 2)
            {
                lCorDoCabecalho = Color.White;
                lCorInteriorDoCabecalho = Color.FromArgb(165, 165, 165);

                lCorDosLabes = Color.FromArgb(127, 127, 127);
                lCorInteriorDosLabels = Color.FromArgb(242, 242, 242);
            }
            else if (pEstilo == 3)
            {
                lCorDoCabecalho = Color.White;
                lCorInteriorDoCabecalho = Color.FromArgb(74, 69, 42);

                lCorDosLabes = Color.FromArgb(123, 68, 43);
                lCorInteriorDosLabels = Color.FromArgb(238, 236, 225);
            }
            else if (pEstilo == 4)
            {
                lCorDoCabecalho = Color.White;
                lCorInteriorDoCabecalho = Color.FromArgb(79, 98, 40);

                lCorDosLabes = Color.FromArgb(90, 90, 90);
                lCorInteriorDosLabels = Color.FromArgb(234, 241, 221);
            }
            else if (pEstilo == 5)
            {
                lCorDoCabecalho = Color.White;
                lCorInteriorDoCabecalho = Color.FromArgb(23, 55, 93);

                lCorDosLabes = Color.FromArgb(90, 90, 90);
                lCorInteriorDosLabels = Color.FromArgb(219, 229, 241);
            }
            else if (pEstilo == 6)
            {
                lCorDoCabecalho = Color.White;
                lCorInteriorDoCabecalho = Color.FromArgb(99, 37, 35);

                lCorDosLabes = Color.FromArgb(130, 90, 90);
                lCorInteriorDosLabels = Color.FromArgb(238, 236, 225);
            }

            if (pEstilo != 1)
            {
                //o estilo "geral" de cores é igual pra todos:

                // cor da fonte dos labels:
                lRangeLabels1.Font.Color = lCorDosLabes;
                lRangeLabels2.Font.Color = lCorDosLabes;

                //bold e fundo cinza-claro pros labels:
                
                lRangeLabels1.Font.Bold = true;
                lRangeLabels2.Font.Bold = true;

                lRangeLabels1.Interior.Color = lCorInteriorDosLabels;
                lRangeLabels2.Interior.Color = lCorInteriorDosLabels;

                //linha grossa, bordas e fundo do header:
                if (lRangeCabecalho != null)
                {
                    lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                    lRangeCabecalho.Interior.Color = lCorInteriorDoCabecalho;
                    lRangeCabecalho.Font.Color     = lCorDoCabecalho;

                    lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle   = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                    lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                    lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle  = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                }

                //linhas internas pra todos:

                lRangeLabels1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeLabels2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

                lRangeValores1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                lRangeValores2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            }
        }

        private void Interface_AplicarEstilo_LivroOfertas(byte pEstilo, int pRow, int pCol, string pFormula)
        {
            Microsoft.Office.Interop.Excel.Range lRangeCabecalho, lRangeCabecalho2, lRangeValores;

            Color lCorCabecalho1, lCorCabecalho1Fundo, lCorCabecalho2, lCorCabecalho2Fundo, lCorValores, lCorValoresFundo;

            lRangeCabecalho = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow - 2, pCol + 0],
                                                                                   ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow - 2, pCol + 5]];

            lRangeCabecalho2 = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow - 1, pCol + 0],
                                                                                    ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow - 1, pCol + 5]];

            lRangeValores = ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Range[((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 0, pCol + 0],
                                                                                 ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet).Cells[pRow + 9, pCol + 5]];

            lCorCabecalho1 = lCorCabecalho2 = lCorValores = Color.Black;
            lCorCabecalho1Fundo = lCorCabecalho2Fundo = lCorValoresFundo = Color.White;

            // características comuns a todos os templates de ticker:

            //alinhamentos:

            lRangeCabecalho.HorizontalAlignment  = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            lRangeCabecalho2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            lRangeValores.HorizontalAlignment    = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            //tamanho da fonte pros labels:

            lRangeCabecalho2.Font.Size = 8;

            //bold pro header:

            lRangeCabecalho.Font.Bold = true;
            
            //borda ao redor do Cabecalho

            lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle        = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle      = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle       = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            //borda ao redor do Cabecalho2

            lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle        = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle      = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle     = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle       = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            //borda ao redor dos valores:
            lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle    = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle  = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle   = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            //borda vertical nos valores:
            lRangeValores.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            if (pEstilo == 1)
            {
                //tira as bordas da parte de cima:
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle   = Microsoft.Office.Interop.Excel.Constants.xlNone;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;
                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle  = Microsoft.Office.Interop.Excel.Constants.xlNone;

                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            }
            else if (pEstilo == 2)
            {
                lCorCabecalho1 = Color.White;
                lCorCabecalho1Fundo = Color.FromArgb(165, 165, 165);

                lCorCabecalho2 = Color.Black;
                lCorCabecalho2Fundo = Color.FromArgb(242, 242, 242);

                lCorValores = Color.Black;
                lCorValoresFundo = Color.White;

                lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

                lRangeCabecalho.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = -4119;
            }
            else if (pEstilo == 3)
            {
                lCorCabecalho1 = Color.White;
                lCorCabecalho1Fundo = Color.FromArgb( 74,  69,  42);

                lCorCabecalho2 = Color.Black;
                lCorCabecalho2Fundo = Color.FromArgb(221, 217, 195);

                lCorValores = Color.Black;
                lCorValoresFundo = Color.FromArgb(238, 236, 225);

                lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = -4119;
            }
            else if (pEstilo == 4)
            {
                lCorCabecalho1 = Color.White;
                lCorCabecalho1Fundo = Color.FromArgb( 79,  98,  40);

                lCorCabecalho2 = Color.Black;
                lCorCabecalho2Fundo = Color.FromArgb(194, 214, 154);

                lCorValores = Color.Black;
                lCorValoresFundo = Color.FromArgb(234, 241, 221);

                lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = -4119;
            }
            else if (pEstilo == 5)
            {
                lCorCabecalho1 = Color.White;
                lCorCabecalho1Fundo = Color.FromArgb( 23,  55,  93);

                lCorCabecalho2 = Color.White;
                lCorCabecalho2Fundo = Color.FromArgb( 55,  96, 145);

                lCorValores = Color.Black;
                lCorValoresFundo = Color.FromArgb(219, 229, 241);

                lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = -4119;
            }
            else if (pEstilo == 6)
            {
                lCorCabecalho1 = Color.White;
                lCorCabecalho1Fundo = Color.FromArgb( 99,  37,  35);

                lCorCabecalho2 = Color.White;
                lCorCabecalho2Fundo = Color.FromArgb(149,  55,  53);

                lCorValores = Color.Black;
                lCorValoresFundo = Color.FromArgb(242, 221, 220);

                lRangeCabecalho2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = -4119;
            }

            // cor da fonte dos cabecalhos:
            lRangeCabecalho.Font.Color  = lCorCabecalho1;
            lRangeCabecalho2.Font.Color = lCorCabecalho2;
            lRangeValores.Font.Color    = lCorValores;

            // cor do fundo dos cabecalhos:
            lRangeCabecalho.Interior.Color  = lCorCabecalho1Fundo;
            lRangeCabecalho2.Interior.Color = lCorCabecalho2Fundo;
            lRangeValores.Interior.Color    = lCorValoresFundo;
        }

        #endregion

        #region Event Handlers

        private void ribGradualStockMarket_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btnAutenticacao_RealizarLogin_Click(object sender, RibbonControlEventArgs e)
        {
            RealizarLogin();
        }

        private void txtCotacao_Instrumento_TextChanged(object sender, RibbonControlEventArgs e)
        {
            /*
            if (txtCotacao_Ativo.Text != txtCotacao_Ativo.Text.ToUpper())
                txtCotacao_Ativo.Text = txtCotacao_Ativo.Text.ToUpper();
            
            btnCotacao_AdicionarCotacao.Enabled = (txtCotacao_Ativo.Text.Length > 2);
            btnCotacao_AdicionarTicker.Enabled  = (txtCotacao_Ativo.Text.Length > 2);
            btnCotacao_AdicionarLivroDeOfertas.Enabled  = (txtCotacao_Ativo.Text.Length > 2);
            */
        }

        private void mnuOpcoes_FrequenciaDeAtualizacao_Alta_Click(object sender, RibbonControlEventArgs e)
        {
            mnuOpcoes_FrequenciaDeAtualizacao_Alta.Checked = true;
            mnuOpcoes_FrequenciaDeAtualizacao_Media.Checked = false;
            mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Checked = false;
            mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Checked = false;
            
            OnRealizarAcao(Funcoes.ACAO_AJUSTAR_FREQUENCIA, "1000");
        }

        private void mnuOpcoes_FrequenciaDeAtualizacao_Media_Click(object sender, RibbonControlEventArgs e)
        {
            mnuOpcoes_FrequenciaDeAtualizacao_Alta.Checked = false;
            mnuOpcoes_FrequenciaDeAtualizacao_Media.Checked = true;
            mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Checked = false;
            mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Checked = false;

            OnRealizarAcao(Funcoes.ACAO_AJUSTAR_FREQUENCIA, "10000");
        }

        private void mnuOpcoes_FrequenciaDeAtualizacao_Baixa_Click(object sender, RibbonControlEventArgs e)
        {
            mnuOpcoes_FrequenciaDeAtualizacao_Alta.Checked = false;
            mnuOpcoes_FrequenciaDeAtualizacao_Media.Checked = false;
            mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Checked = true;
            mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Checked = false;

            OnRealizarAcao(Funcoes.ACAO_AJUSTAR_FREQUENCIA, "6000");
        }

        private void mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa_Click(object sender, RibbonControlEventArgs e)
        {
            mnuOpcoes_FrequenciaDeAtualizacao_Alta.Checked = false;
            mnuOpcoes_FrequenciaDeAtualizacao_Media.Checked = false;
            mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Checked = false;
            mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Checked = true;

            OnRealizarAcao(Funcoes.ACAO_AJUSTAR_FREQUENCIA, "60000");
        }

        private void btnCotacao_AdicionarCotacao_Click(object sender, RibbonControlEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCotacao_Ativo.Text) && txtCotacao_Ativo.Text.Length > 2)
            {
                OnRealizarAcao(Funcoes.ACAO_MONTAR_COTACAO, txtCotacao_Ativo.Text.ToUpper());
            }
            else
            {
                MessageBox.Show("Favor indicar um ativo válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCotacao_AdicionarTicker_Click(object sender, RibbonControlEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCotacao_Ativo.Text) && txtCotacao_Ativo.Text.Length > 2)
            {
                OnRealizarAcao(Funcoes.ACAO_MONTAR_TICKERCOTACAO, txtCotacao_Ativo.Text.ToUpper());
            }
            else
            {
                MessageBox.Show("Favor indicar um ativo válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCotacao_AdicionarLivroDeOfertas_Click(object sender, RibbonControlEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCotacao_Ativo.Text) && txtCotacao_Ativo.Text.Length > 2)
            {
                OnRealizarAcao(Funcoes.ACAO_MONTAR_LIVROOFERTAS, txtCotacao_Ativo.Text.ToUpper());
            }
            else
            {
                MessageBox.Show("Favor indicar um ativo válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCarteira_ImportarAtivos_Click(object sender, RibbonControlEventArgs e)
        {
            if (cboCarteiras_Carteiras.SelectedItemIndex >= 0)
            {
                OnRealizarAcao(Funcoes.ACAO_MONTAR_CARTEIRA, gListaDeAtivos[cboCarteiras_Carteiras.SelectedItemIndex]);
            }
        }

        private void btnEstilos_Aplicar_Click(object sender, RibbonControlEventArgs e)
        {
            Interface_AplicarEstilo();
        }

        private void btnOrdens_Acompanhamento_Click(object sender, RibbonControlEventArgs e)
        {
            OnRealizarAcao(Funcoes.ACAO_MONTAR_POSICAO_NET, gCodigoDeAcessoDoUsuario.ToString());
        }

        #endregion

    }
}
