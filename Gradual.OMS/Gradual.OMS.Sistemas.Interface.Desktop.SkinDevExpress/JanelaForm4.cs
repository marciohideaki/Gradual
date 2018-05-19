using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

using DevExpress.XtraBars.Docking;

namespace Gradual.OMS.Sistemas.Interface.Desktop.SkinDevExpress
{
    public class JanelaForm4 : IJanela
    {
        private DockPanel _panel = null;
        private Janela _item = null;
        private DockVisibility _visibility = DockVisibility.Visible;

        #region IJanela Members

        public void Inicializar(Janela janela)
        {
            // Inicializa
            _item = janela;
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            JanelaLauncher launcher = (JanelaLauncher)servicoInterface.ReceberJanelaLauncher();
            
            // Cria o panel
            _panel = launcher.dockManager.AddPanel(DockingStyle.Float);
            _panel.ClosedPanel += new DockPanelEventHandler(_panel_ClosedPanel);
            _panel.Visibility = DockVisibility.Visible;
        }

        void _panel_ClosedPanel(object sender, DockPanelEventArgs e)
        {
            // Pede para a interface retirar a janela
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            servicoInterface.RemoverJanela(_item.Info.Id);

            // Avisa do fechamento da janela
            servicoInterface.SinalizarJanelaFechando(_item.Info);
        }

        public void AdicionarControle(Controle controle)
        {
            ((Control)controle.Instancia).Dock = DockStyle.Fill;
            _panel.Controls.Add((Control)controle.Instancia);

            // O título da janela será o título do controle
            _panel.Text = controle.Info.Titulo;
        }

        public void AdicionarComando(Comando comando)
        {
        }

        public void MostrarJanela()
        {
            _panel.Visibility = _visibility;
        }

        public void EsconderJanela()
        {
            if (_panel.Visibility != DockVisibility.Hidden)
                _visibility = _panel.Visibility;
            else
                _visibility = DockVisibility.Visible;
            _panel.Visibility = DockVisibility.Hidden;
        }

        public void MostrarConfiguracoes()
        {
        }

        public void SetarTitulo(string titulo)
        {
            _panel.Text = titulo;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Cria classe de parametros
            JanelaParametro janelaParametro = new JanelaParametro();
            janelaParametro.Height = _panel.Height;
            janelaParametro.Width = _panel.Width;
            janelaParametro.Top = _panel.Top;
            janelaParametro.Left = _panel.Left;
            janelaParametro.Titulo = _panel.Text;

            // Retorna
            return janelaParametro;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            JanelaParametro janelaParametro = parametros as JanelaParametro;
            if (janelaParametro != null)
            {
                _panel.Width = janelaParametro.Width;
                _panel.Height = janelaParametro.Height;
                _panel.Top = janelaParametro.Top;
                _panel.Left = janelaParametro.Left;
                _panel.Text = janelaParametro.Titulo;
            }
        }

        public void Fechar()
        {
            _panel.DockManager.RemovePanel(_panel);
        }

        public void Ativar()
        {
            _panel.DockManager.ActivePanel = _panel;
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion
    }
}
