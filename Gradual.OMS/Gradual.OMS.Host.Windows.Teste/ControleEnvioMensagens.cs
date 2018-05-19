using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.CanaisNegociacao;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop;

namespace Gradual.OMS.Host.Windows.Teste
{
    public partial class ControleEnvioMensagens : DevExpress.XtraEditors.XtraUserControl, IControle
    {
        private Controle _controle = null;
        private IServicoOrdens _servicoOrdens = null;
        private IServicoCanaisNegociacao _servicoCanais = null;
        private MensagemRequestBase _mensagemRequest = null;
        private List<Mensagem> _mensagens = new List<Mensagem>();
        private Dictionary<string, object> _parametros = null;
        
        public ControleEnvioMensagens()
        {
            InitializeComponent();

            this.lstMensagem.SelectedIndexChanged += new EventHandler(lstMensagem_SelectedIndexChanged);
            this.cmdEnviarMensagem.Click += new EventHandler(cmdEnviarMensagem_Click);

            if (!this.DesignMode)
                this.Load += new EventHandler(ControleEnvioMensagens_Load);
        }

        private void ControleEnvioMensagens_Load(object sender, EventArgs e)
        {
            // Referencia aos serviços
            _servicoOrdens = Ativador.Get<IServicoOrdens>();
            _servicoCanais = Ativador.Get<IServicoCanaisNegociacao>();

            // Carrega lista de mensagens de requisicao
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
                foreach (Type tipo in assembly.GetTypes())
                    if (tipo.IsSubclassOf(typeof(MensagemRequestBase)))
                        lstMensagem.Items.Add(tipo);

            // Carrega parametros
            if (_parametros != null)
            {
                splitter1.SplitPosition = (int)_parametros["splitter"];
            }
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

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


        #region IControle Members

        public void Inicializar(Controle controle)
        {
            _controle = controle;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("splitter", splitter1.SplitPosition);
            return parametros;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            Dictionary<string, object> parametros2 = parametros as Dictionary<string, object>;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion
    }
}
