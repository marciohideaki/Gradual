using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.IO;
using System.Globalization;
using Gradual.OMS.CarteiraRecomendada.lib;
using Gradual.OMS.CarteiraRecomendada.lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.Library.Servicos;

namespace ConsoleApplicationTesteCarteiraRecomendada
{
    public partial class FrmTeste : Form
    {
        public FrmTeste()
        {
            InitializeComponent();

            lstLista.Columns.Add("IdCarteira", 70, HorizontalAlignment.Center);
            lstLista.Columns.Add("IdProduto", 60, HorizontalAlignment.Center);
            lstLista.Columns.Add("IdTipoCarteira", 70, HorizontalAlignment.Center);
            lstLista.Columns.Add("DtCarteira", 120, HorizontalAlignment.Left);
            lstLista.Columns.Add("StAtiva", 50, HorizontalAlignment.Center);
            lstLista.Columns.Add("DsCarteira", 350, HorizontalAlignment.Left);

            lstListaComposicao.Columns.Add("IdCarteira", 70, HorizontalAlignment.Center);
            lstListaComposicao.Columns.Add("IdAtivo", 70, HorizontalAlignment.Center);
            lstListaComposicao.Columns.Add("Quantidade", 70, HorizontalAlignment.Center);

            lstListaCliente.Columns.Add("IdCarteira", 70, HorizontalAlignment.Center);
            lstListaCliente.Columns.Add("IdProduto", 60, HorizontalAlignment.Center);
            lstListaCliente.Columns.Add("DsCarteira", 330, HorizontalAlignment.Left);
            lstListaCliente.Columns.Add("DtCarteira", 120, HorizontalAlignment.Left);
            lstListaCliente.Columns.Add("Aderir ?", 70, HorizontalAlignment.Left);
            lstListaCliente.Columns.Add("Renovar ?", 70, HorizontalAlignment.Left);

            lstListaComposicaoClienteAtual.Columns.Add("IdCarteira", 70, HorizontalAlignment.Center);
            lstListaComposicaoClienteAtual.Columns.Add("IdAtivo", 70, HorizontalAlignment.Center);
            lstListaComposicaoClienteAtual.Columns.Add("Quantidade", 70, HorizontalAlignment.Center);

            lstListaComposicaoClienteNova.Columns.Add("IdCarteira", 70, HorizontalAlignment.Center);
            lstListaComposicaoClienteNova.Columns.Add("IdAtivo", 70, HorizontalAlignment.Center);
            lstListaComposicaoClienteNova.Columns.Add("Quantidade", 70, HorizontalAlignment.Center);

            lstListaAssessores.Columns.Add("IdAssessor", 70, HorizontalAlignment.Center);
            lstListaAssessores.Columns.Add("NomeAssessor", 400, HorizontalAlignment.Left);

            txtDataInicialAdesaoAcompanhamento.Text = "01/01/1900";
            txtDataFinalAdesaoAcompanhamento.Text = DateTime.Now.ToString("dd/MM/yyyy");
            chkOrdensExecutadasAcompanhamento.Checked = true;

            lstListaAcompanhamento.Columns.Add("IdCliente", 60, HorizontalAlignment.Center);
            lstListaAcompanhamento.Columns.Add("Cliente", 150, HorizontalAlignment.Left);
            lstListaAcompanhamento.Columns.Add("IdCarteira", 60, HorizontalAlignment.Center);
            lstListaAcompanhamento.Columns.Add("Carteira", 150, HorizontalAlignment.Left);
            lstListaAcompanhamento.Columns.Add("IdAssessor", 60, HorizontalAlignment.Center);
            lstListaAcompanhamento.Columns.Add("Adesao", 115, HorizontalAlignment.Left);
            lstListaAcompanhamento.Columns.Add("Renovacao", 115, HorizontalAlignment.Left);
            lstListaAcompanhamento.Columns.Add("Qtd.Renovacao", 65, HorizontalAlignment.Center);
            lstListaAcompanhamento.Columns.Add("Basket Disparada ?", 80, HorizontalAlignment.Center);

            txtDataOrdensDetalhesAcompanhamento.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lstListaDetalhesAcompanhamento.Columns.Add("IdOrdem", 100, HorizontalAlignment.Center);
            lstListaDetalhesAcompanhamento.Columns.Add("Ativo", 80, HorizontalAlignment.Center);
            lstListaDetalhesAcompanhamento.Columns.Add("Quantidade", 80, HorizontalAlignment.Center);
            lstListaDetalhesAcompanhamento.Columns.Add("IdStatus", 60, HorizontalAlignment.Center);
            lstListaDetalhesAcompanhamento.Columns.Add("Status", 200, HorizontalAlignment.Left);
            lstListaDetalhesAcompanhamento.Columns.Add("Data da Ordem", 120, HorizontalAlignment.Center);
        }

        private void btnLista_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                ListarResponse response = new ListarResponse();
                response = servico.SolicitarLista();
                if (!response.bSucesso)
                {
                    MessageBox.Show(response.DescricaoResposta);
                    return;
                }

                lstLista.Items.Clear();
                foreach (CarteiraRecomendadaInfo carteira in response.Lista)
                {
                    string[] item = new string[] {
                    carteira.IdCarteira.ToString(), 
                    carteira.IdProduto.ToString(),
                    carteira.IdTipoCarteira.ToString(),
                    carteira.DtCarteira.ToString(),
                    carteira.StAtiva.ToString(),
                    carteira.DsCarteira.ToString()};
                    lstLista.Items.Add(new ListViewItem(item));
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnListaComposicao_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                ListarComposicaoRequest request = new ListarComposicaoRequest();
                ListarComposicaoResponse response = new ListarComposicaoResponse();

                request.idCarteiraRecomendada = Int32.Parse(txtListaComposicaoIdCarteira.Text);
                response = servico.SolicitarListaComposicao(request);
                if (!response.bSucesso)
                {
                    MessageBox.Show(response.DescricaoResposta);
                    return;
                }

                lstListaComposicao.Items.Clear();
                foreach (CarteiraRecomendadaComposicaoInfo composicao in response.listaComposicao)
                {
                    string[] item = new string[] {
                    composicao.IdCarteiraRecomendada.ToString(), 
                    composicao.IdAtivo.ToString(),
                    composicao.Quantidade.ToString()};
                    lstListaComposicao.Items.Add(new ListViewItem(item));
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnInclusao_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                InserirRequest request = new InserirRequest();
                InserirResponse response = new InserirResponse();

                request.carteiraRecomendada.DsCarteira = txtDescricaoInclusao.Text;
                request.carteiraRecomendada.IdTipoCarteira = Int32.Parse(txtTipoCarteiraInclusao.Text);
                request.carteiraRecomendada.StAtiva = char.Parse(txtStatusInclusao.Text);

                CarteiraRecomendadaComposicaoInfo composicao = null;

                for (int i = 0; i < grdInclusao.RowCount; i++)
                {
                    if (grdInclusao.Rows[i].Cells["AtivoInclusao"].Value == null)
                        break;

                    composicao = new CarteiraRecomendadaComposicaoInfo();
                    composicao.IdAtivo = grdInclusao.Rows[i].Cells["AtivoInclusao"].Value.ToString();
                    composicao.Quantidade = Int32.Parse(grdInclusao.Rows[i].Cells["QuantidadeInclusao"].Value.ToString());
                    request.listaComposicao.Add(composicao);
                }

                response = servico.SolicitarInclusao(request);

                if (response.bSucesso == true)
                    MessageBox.Show("Sucesso !!");
                else
                {
                    string msg = "Falha: " + response.DescricaoResposta + (response.Exception != null ? " - " + response.Exception.Message : "");
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnAlteracao_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                AlterarRequest request = new AlterarRequest();
                AlterarResponse response = new AlterarResponse();

                request.IdCarteiraRecomendada = Int32.Parse(txtIdCarteiraAlteracao.Text);
                request.DsCarteira = txtDescricaoAlteracao.Text;
                request.StAtiva = char.Parse(txtStatusAlteracao.Text);

                response = servico.SolicitarAlteracao(request);

                if (response.bSucesso == true)
                    MessageBox.Show("Sucesso !!");
                else
                {
                    string msg = "Falha: " + response.DescricaoResposta + (response.Exception != null ? " - " + response.Exception.Message : "");
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnRenovar_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                RenovarRequest request = new RenovarRequest();
                RenovarResponse response = new RenovarResponse();

                request.idCarteiraRecomendada = Int32.Parse(txtIdCarteiraRenovacao.Text);
                request.dsRenovacao = txtDescricaoRenovacao.Text;

                CarteiraRecomendadaComposicaoInfo composicao = null;

                for (int i = 0; i < grdRenovacao.RowCount; i++)
                {
                    if (grdRenovacao.Rows[i].Cells["AtivoRenovacao"].Value == null)
                        break;

                    composicao = new CarteiraRecomendadaComposicaoInfo();
                    composicao.IdAtivo = grdRenovacao.Rows[i].Cells["AtivoRenovacao"].Value.ToString();
                    composicao.Quantidade = Int32.Parse(grdRenovacao.Rows[i].Cells["QuantidadeRenovacao"].Value.ToString());
                    request.listaComposicao.Add(composicao);
                }

                response = servico.SolicitarRenovacao(request);

                if (response.bSucesso == true)
                    MessageBox.Show("Sucesso !!");
                else
                {
                    string msg = "Falha: " + response.DescricaoResposta + (response.Exception != null ? " - " + response.Exception.Message : "");
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnAdesaoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                AdesaoClienteRequest request = new AdesaoClienteRequest();
                AdesaoClienteResponse response = new AdesaoClienteResponse();

                request.IdCliente = Int32.Parse(txtIdClienteAdesao.Text);
                request.IdProduto = Int32.Parse(txtIdProdutoAdesao.Text);
                request.IP = Dns.GetHostName();

                response = servico.SolicitarAdesaoCliente(request);

                if (response.bSucesso == true)
                    MessageBox.Show("Sucesso !!");
                else
                {
                    string msg = "Falha: " + response.DescricaoResposta + (response.Exception != null ? " - " + response.Exception.Message : "");
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnRenovacaoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                RenovarClienteRequest request = new RenovarClienteRequest();
                RenovarClienteResponse response = new RenovarClienteResponse();

                request.IdCliente = Int32.Parse(txtIdClienteRenovacaoCliente.Text);
                request.IdProduto = Int32.Parse(txtIdProdutoRenovacaoCliente.Text);
                request.IP = Dns.GetHostName();

                response = servico.SolicitarRenovacaoCliente(request);

                if (response.bSucesso == true)
                    MessageBox.Show("Sucesso !!");
                else
                {
                    string msg = "Falha: " + response.DescricaoResposta + (response.Exception != null ? " - " + response.Exception.Message : "");
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnCancelamento_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                CancelarRequest request = new CancelarRequest();
                CancelarResponse response = new CancelarResponse();

                request.IdCliente = Int32.Parse(txtIdClienteCancelamento.Text);
                request.IdProduto = Int32.Parse(txtIdProdutoCancelamento.Text);
                request.IP = Dns.GetHostName();

                response = servico.SolicitarCancelamento(request);

                if (response.bSucesso == true)
                    MessageBox.Show("Sucesso !!");
                else
                {
                    string msg = "Falha: " + response.DescricaoResposta + (response.Exception != null ? " - " + response.Exception.Message : "");
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnListaCliente_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                ListarClienteRequest request = new ListarClienteRequest();
                ListarClienteResponse response = new ListarClienteResponse();

                request.IdCliente = Int32.Parse(txtIdClienteListaCliente.Text);
                response = servico.SolicitarListaCliente(request);
                if (!response.bSucesso)
                {
                    MessageBox.Show(response.DescricaoResposta);
                    return;
                }

                lstListaCliente.Items.Clear();
                foreach (CarteiraRecomendadaClienteInfo lista in response.lista)
                {
                    string[] item = new string[] {
                    lista.IdCarteira.ToString(), 
                    lista.IdProduto.ToString(),
                    lista.DsCarteira.ToString(),
                    lista.DtCarteira.ToString(),
                    lista.PermiteAdesao.ToString(),
                    lista.PermiteRenovacao.ToString()};
                    lstListaCliente.Items.Add(new ListViewItem(item));
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnListaComposicaoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                ListarComposicaoClienteRequest request = new ListarComposicaoClienteRequest();
                ListarComposicaoClienteResponse response = new ListarComposicaoClienteResponse();

                request.idCliente = Int32.Parse(txtIdClienteComposicaoCliente.Text);
                request.idCarteiraRecomendada = Int32.Parse(txtIdCarteiraComposicaoCliente.Text);
                response = servico.SolicitarListaComposicaoCliente(request);
                if (!response.bSucesso)
                {
                    MessageBox.Show(response.DescricaoResposta);
                    return;
                }

                lstListaComposicaoClienteAtual.Items.Clear();
                foreach (CarteiraRecomendadaComposicaoInfo composicao in response.listaComposicaoAtual)
                {
                    string[] item = new string[] {
                    composicao.IdCarteiraRecomendada.ToString(), 
                    composicao.IdAtivo.ToString(),
                    composicao.Quantidade.ToString()};
                    lstListaComposicaoClienteAtual.Items.Add(new ListViewItem(item));
                }

                lstListaComposicaoClienteNova.Items.Clear();
                foreach (CarteiraRecomendadaComposicaoInfo composicao in response.listaComposicaoNova)
                {
                    string[] item = new string[] {
                    composicao.IdCarteiraRecomendada.ToString(), 
                    composicao.IdAtivo.ToString(),
                    composicao.Quantidade.ToString()};
                    lstListaComposicaoClienteNova.Items.Add(new ListViewItem(item));
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnListaAssessores_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                ListarAssessoresResponse response = new ListarAssessoresResponse();
                response = servico.SolicitarListaAssessores();
                if (!response.bSucesso)
                {
                    MessageBox.Show(response.DescricaoResposta);
                    return;
                }

                lstListaAssessores.Items.Clear();
                foreach (AssessorInfo assessor in response.Lista)
                {
                    string[] item = new string[] {
                    assessor.IdAssessor.ToString(), 
                    assessor.NomeAssessor.ToString()};
                    lstListaAssessores.Items.Add(new ListViewItem(item));
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnListaAcompanhamento_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                ListarAcompanhamentoRequest request = new ListarAcompanhamentoRequest();
                ListarAcompanhamentoResponse response = new ListarAcompanhamentoResponse();

                request.IdCliente = Int32.Parse(txtIdClienteAcompanhamento.Text);
                request.IdCarteiraRecomendada = Int32.Parse(txtIdCarteiraAcompanhamento.Text);
                request.IdAssessor = Int32.Parse(txtIdAssessorAcompanhamento.Text);
                request.DtAdesaoInicial = DateTime.Parse(txtDataInicialAdesaoAcompanhamento.Text, new CultureInfo("pt-BR", false));
                request.DtAdesaoFinal = DateTime.Parse(txtDataFinalAdesaoAcompanhamento.Text, new CultureInfo("pt-BR", false));
                if (txtDataInicialRenovacaoAcompanhamento.Text.Trim() == "")
                    request.DtRenovacaoInicial = null;
                else
                    request.DtRenovacaoInicial = DateTime.Parse(txtDataInicialRenovacaoAcompanhamento.Text, new CultureInfo("pt-BR", false));
                if (txtDataFinalRenovacaoAcompanhamento.Text.Trim() == "")
                    request.DtRenovacaoFinal = null;
                else
                    request.DtRenovacaoFinal = DateTime.Parse(txtDataFinalRenovacaoAcompanhamento.Text, new CultureInfo("pt-BR", false));
                request.StBasketAberto = chkBasketAbertoAcompanhamento.Checked;
                request.StOrdensExecutadas = chkOrdensExecutadasAcompanhamento.Checked;

                response = servico.SolicitarListaAcompanhamento(request);
                if (!response.bSucesso)
                {
                    MessageBox.Show(response.DescricaoResposta);
                    return;
                }

                lstListaAcompanhamento.Items.Clear();
                foreach (AcompanhamentoInfo acompanhamento in response.Lista)
                {
                    string[] item = new string[] {
                    acompanhamento.IdCliente.ToString(), 
                    acompanhamento.DsCliente.ToString(),
                    acompanhamento.IdCarteira.ToString(),
                    acompanhamento.DsCarteira.ToString(),
                    acompanhamento.IdAssessor.ToString(),
                    acompanhamento.DtAdesao.ToString(),
                    acompanhamento.DtRenovacao.ToString(),
                    acompanhamento.QtdRenovacoes.ToString(),
                    acompanhamento.StBasketDisparada.ToString()};
                    lstListaAcompanhamento.Items.Add(new ListViewItem(item));
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }

        private void btnDetalhesAcompanhamento_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoCarteiraRecomendada servico = Ativador.Get<IServicoCarteiraRecomendada>();

                OrdensEnviadasRequest request = new OrdensEnviadasRequest();
                OrdensEnviadasResponse response = new OrdensEnviadasResponse();

                request.IdCliente = Int32.Parse(txtIdClienteDetalhesAcompanhamento.Text);
                request.IdCarteiraRecomendada = Int32.Parse(txtIdCarteiraDetalhesAcompanhamento.Text);
                request.DtOrdensEnviadas = DateTime.Parse(txtDataOrdensDetalhesAcompanhamento.Text, new CultureInfo("pt-BR", false));

                response = servico.SolicitarListaDetalhesAcompanhamento(request);
                if (!response.bSucesso)
                {
                    MessageBox.Show(response.DescricaoResposta);
                    return;
                }

                lstListaDetalhesAcompanhamento.Items.Clear();
                foreach (OrdensEnviadasInfo ordens in response.Lista)
                {
                    string[] item = new string[] {
                    ordens.IdOrdem.ToString(), 
                    ordens.IdAtivo.ToString(),
                    ordens.Quantidade.ToString(),
                    ordens.IdOrdemStatus.ToString(),
                    ordens.DsOrdemStatus.ToString(),
                    ordens.DtOrdem.ToString()};
                    lstListaDetalhesAcompanhamento.Items.Add(new ListViewItem(item));
                }
            }
            catch (Exception ex)
            {
                string msg = "Falha: " + ex.Message;
                MessageBox.Show(msg);
            }
        }
    }
}
