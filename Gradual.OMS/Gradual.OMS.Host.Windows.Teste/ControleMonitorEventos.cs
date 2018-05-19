using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.CanaisNegociacao;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop;

namespace Gradual.OMS.Host.Windows.Teste
{
    public partial class ControleMonitorEventos : DevExpress.XtraEditors.XtraUserControl, IControle
    {
        private Controle _controle = null;
        private IServicoOrdens _servicoOrdens = null;
        private IServicoCanaisNegociacao _servicoCanais = null;
        private List<Mensagem> _mensagens = new List<Mensagem>();

        public ControleMonitorEventos()
        {
            InitializeComponent();

            if (!this.DesignMode)
                this.Load += new EventHandler(ControleMonitorEventos_Load);
        }

        void ControleMonitorEventos_Load(object sender, EventArgs e)
        {
            // Referencia aos serviços
            _servicoOrdens = Ativador.Get<IServicoOrdens>();
            _servicoCanais = Ativador.Get<IServicoCanaisNegociacao>();

            // Monitora mensagens recebidas
            _servicoOrdens.EventoSinalizacao += new EventHandler<SinalizarEventArgs>(_servicoOrdens_EventoSinalizacao);

            // Detalhe das linhas do grid
            grdMensagens.DoubleClick += new EventHandler(grdMensagens_DoubleClick);
        }

        private void grdMensagens_DoubleClick(object sender, EventArgs e)
        {
            if (grdMensagens.SelectedRows.Count > 0)
            {
                Mensagem mensagem = (Mensagem)grdMensagens.SelectedRows[0].DataBoundItem;
                new frmObjeto(mensagem).Show();
            }
        }

        private delegate void InvokeDelegate();
        void _servicoOrdens_EventoSinalizacao(object sender, SinalizarEventArgs e)
        {
            // Inclui linha de detalhe
            _mensagens.Add(
                new Mensagem()
                {
                    Sinalizacao = e.Mensagem
                });

            // Ajusta para execução entre threads diferentes
            grdMensagens.Invoke(
                new InvokeDelegate(
                    delegate() 
                    {
                        // Atualiza o grid
                        grdMensagens.DataSource = null;
                        grdMensagens.DataSource = _mensagens;
                        grdMensagens.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    }
                ));
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
            _controle = controle;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            return null;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion
    }
}
