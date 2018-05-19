using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

using Gradual.OMS.Contratos.CanaisNegociacao;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.CanaisNegociacao;
using Gradual.OMS.Sistemas.Comum;
using Gradual.OMS.Sistemas.Ordens;
using Gradual.OMS.Sistemas.Risco;

namespace Gradual.OMS.Host.Windows.Teste
{
    public partial class frmPrincipal : Form
    {
        private IServicoOrdens _servicoOrdens = null;
        private IServicoCanaisNegociacao _servicoCanais = null;
        private MensagemRequestBase _mensagemRequest = null;
        private List<Mensagem> _mensagens = new List<Mensagem>();
        
        public frmPrincipal()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            // Repassa mensagem
            base.OnLoad(e);

            // Referencia aos serviços
            _servicoOrdens = Ativador.Get<IServicoOrdens>();
            _servicoCanais = Ativador.Get<IServicoCanaisNegociacao>();

            // Carrega lista de mensagens de requisicao
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
                foreach (Type tipo in assembly.GetTypes())
                    if (tipo.IsSubclassOf(typeof(MensagemRequestBase)))
                        lstMensagem.Items.Add(tipo);

            // Monitora mensagens recebidas
            _servicoOrdens.EventoSinalizacao += new EventHandler<SinalizarEventArgs>(_servicoOrdens_EventoSinalizacao);

            // Status
            lblStatusCanais.Text = _servicoCanais.ReceberStatusServico().ToString();
        }

        void _servicoOrdens_EventoSinalizacao(object sender, SinalizarEventArgs e)
        {
            // Inclui linha de detalhe
            _mensagens.Add(
                new Mensagem()
                {
                    Sinalizacao = e.Mensagem
                });

            // Atualiza o grid
            grdMensagens.DataSource = _mensagens;
            grdMensagens.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Finaliza canais, caso ainda esteja rodando
            if (_servicoCanais.ReceberStatusServico() == ServicoStatus.EmExecucao)
                _servicoCanais.PararServico();
            
            // Repassa chamada
            base.OnClosing(e);
        }

        private void cmdSair_Click(object sender, EventArgs e)
        {
            // Para os canais clientes
            Ativador.Get<IServicoCanaisNegociacao>().PararServico();

            // Fecha o formulario
            this.Close();
        }

        private void lstMensagem_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cria instancia da mensagem
            if (lstMensagem.SelectedItem != null)
            {
                _mensagemRequest = (MensagemRequestBase)Activator.CreateInstance((Type)lstMensagem.SelectedItem);
                comporMensagem(_mensagemRequest);
                ppg.SelectedObject = _mensagemRequest;
            }
        }

        private void cmdListarInstrumentos_Click(object sender, EventArgs e)
        {
            // Pede lista de ativos
            ListarInstrumentosResponse listarInstrumentosResponse =
                _servicoOrdens.ListarInstrumentos(
                    new ListarInstrumentosRequest()
                    {
                        CodigoMensagem = "reqi01",
                        CodigoBolsa = "BOVESPA",
                        CodigoCliente = "123",
                        DataReferencia = DateTime.Now
                    });
            var t = from s in listarInstrumentosResponse.Instrumentos
                    select new InstrumentoInfo() { Symbol = s };
            grdInstrumentos.DataSource = new List<InstrumentoInfo>(t);
            
            // Seleciona tab de instrumentos
            tab.SelectedTab = tabInstrumentos;
        }

        private void cmdCanaisIniciar_Click(object sender, EventArgs e)
        {
            _servicoCanais.IniciarServico();
            lblStatusCanais.Text = _servicoCanais.ReceberStatusServico().ToString();
        }

        private void cmdCanaisParar_Click(object sender, EventArgs e)
        {
            _servicoCanais.PararServico();
            lblStatusCanais.Text = _servicoCanais.ReceberStatusServico().ToString();
        }

        private void cmdEnviarMensagem_Click(object sender, EventArgs e)
        {
            try
            {
                // Envia mensagem ao servico de ordens
                MensagemResponseBase response = _servicoOrdens.ProcessarMensagem(_mensagemRequest);

                // Inclui linha de detalhe
                _mensagens.Add(
                    new Mensagem() 
                    { 
                        Request = _mensagemRequest,
                        Response = response
                    });

                // Seleciona tab de mensagens
                tab.SelectedTab = tabMensagens;
                
                // Atualiza o grid
                grdMensagens.DataSource = null;
                grdMensagens.DataSource = _mensagens;
                grdMensagens.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void grdMensagensEnviadas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                popupMensagens.Show(Cursor.Position);
        }

        private void grdInstrumentos_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                popupInstrumentos.Show(Cursor.Position);
        }

        private void popupInstrumentosEnviarOrdem_Click(object sender, EventArgs e)
        {
            if (grdInstrumentos.SelectedRows.Count > 0)
            {
                InstrumentoInfo instrumento = (InstrumentoInfo)grdInstrumentos.SelectedRows[0].DataBoundItem;
                ExecutarOrdemRequest executarOrdem = new ExecutarOrdemRequest();
                executarOrdem.Symbol = instrumento.Symbol;
                mostrarMensagem(executarOrdem);
            }
        }

        private void popupMensagensCriarMensagem_Click(object sender, EventArgs e)
        {
            if (grdMensagens.SelectedRows.Count > 0)
            {
                Mensagem mensagem = (Mensagem)grdMensagens.SelectedRows[0].DataBoundItem;
                BinaryFormatter serializer = new BinaryFormatter();

                MemoryStream ms = new MemoryStream();
                serializer.Serialize(ms, mensagem.Request);
                ms.Position = 0;
                MensagemRequestBase mensagemRequest = (MensagemRequestBase)serializer.Deserialize(ms);

                mostrarMensagem(mensagemRequest);
            }
        }

        private void mostrarMensagem(MensagemRequestBase mensagem)
        {
            lstMensagem.SelectedItem = mensagem.GetType();
            _mensagemRequest = mensagem;
            comporMensagem(_mensagemRequest);
            ppg.SelectedObject = _mensagemRequest;
            tab.SelectedTab = tabEnviarMensagem;
        }

        private void mnuAtualizaLista_Click(object sender, EventArgs e)
        {
            // Seleciona tab de mensagens
            tab.SelectedTab = tabMensagens;

            // Atualiza o grid
            grdMensagens.DataSource = null;
            grdMensagens.Refresh();
            grdMensagens.DataSource = _mensagens;
            grdMensagens.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void comporMensagem(MensagemRequestBase mensagem)
        {
            if (mensagem.GetType() == typeof(ExecutarOrdemRequest))
            {
                ExecutarOrdemRequest msg = (ExecutarOrdemRequest)mensagem;
                msg.ClOrdID = _servicoOrdens.GerarCodigoMensagem("ORD");
                msg.CodigoCliente = "123";
                msg.DataReferencia = DateTime.Now;
                msg.CodigoBolsa = "BMF";
                msg.TransactTime = DateTime.Now.Date;
                msg.Price = 100;
                msg.OrderQty = 10;
            } 
            else if (mensagem.GetType() == typeof(CancelarOrdemRequest))
            {
                CancelarOrdemRequest msg = (CancelarOrdemRequest)mensagem;
                msg.ClOrdID = _servicoOrdens.GerarCodigoMensagem("CNC");
                msg.CodigoBolsa = "BMF";
                msg.CodigoCliente = "123";
            }
            else
            {
                mensagem.CodigoMensagem = _servicoOrdens.GerarCodigoMensagem("MSG");
            }
        }

        private void grdMensagens_DoubleClick(object sender, EventArgs e)
        {
            if (grdMensagens.SelectedRows.Count > 0)
            {
                Mensagem mensagem = (Mensagem)grdMensagens.SelectedRows[0].DataBoundItem;
                new frmObjeto(mensagem).Show();
            }
        }

        private void testeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IServicoOrdens servicoOrdens = Ativador.Get<IServicoOrdens>();
            ExecutarOrdemResponse response =
                servicoOrdens.ExecutarOrdem(
                new ExecutarOrdemRequest()
                {
                    Account = "1",
                    ClOrdID = "o1",
                    CodigoBolsa = "BOVESPA",
                    CodigoCliente = "123",
                    DataReferencia = DateTime.Now,
                    OrderQty = 100,
                    OrdType = OrdemTipoEnum.Limitada,
                    Price = 85,
                    Side = OrdemDirecaoEnum.Compra,
                    Symbol = "USIM5",
                    TimeInForce = OrdemValidadeEnum.ValidaParaODia
                });
        }
    }
}
