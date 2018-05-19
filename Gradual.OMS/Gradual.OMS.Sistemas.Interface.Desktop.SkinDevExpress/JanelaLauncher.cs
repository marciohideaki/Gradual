using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library.Servicos;

using DevExpress.XtraBars;
using DevExpress.XtraEditors;

namespace Gradual.OMS.Sistemas.Interface.Desktop.SkinDevExpress
{
    public partial class JanelaLauncher : DevExpress.XtraEditors.XtraForm, IJanela
    {
        private Janela _item = null;
        private List<BarButtonItem> _desktopButtons = new List<BarButtonItem>();

        public JanelaLauncher()
        {
            InitializeComponent();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();

            foreach (DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins)
            {
                BarButtonItem item = new BarButtonItem(barManager, skin.SkinName);
                menuSkins.AddItem(item);
                item.ItemClick += delegate(object sender, ItemClickEventArgs e)
                {
                    defaultLookAndFeel.LookAndFeel.SkinName = e.Item.Caption;
                };
            }

            // Quando fechar o launcher, salva as configurações antes
            this.FormClosing += new FormClosingEventHandler(JanelaLauncher_FormClosing);

            // Após o launcher ser lançado, carrega as configurações iniciais
            this.Load += new EventHandler(JanelaLauncher_Load);
        }

        void JanelaLauncher_Load(object sender, EventArgs e)
        {
            // Carrega configurações default
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            servicoInterface.CarregarConfiguracoesDefault();
            servicoInterface.CarregarConfiguracoes();

            // Verifica se tem algum desktop selecionado
            if (servicoInterface.DesktopAtivo == null)
            {
                // Se nao tem nenhum desktop, cria um
                if (servicoInterface.Desktops.Count == 0)
                    servicoInterface.Desktops.Add(new DesktopInfo() { Nome = "Default" });

                // Seta desktop inicial
                servicoInterface.MostrarDesktop(servicoInterface.Desktops[0].Id);
            }

            // Carrega lista de desktops
            listarDesktops();

            // Inicia o timer
            timer.Enabled = true;

            this.BackColor = Color.White;
        }

        void JanelaLauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Salva configurações
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            servicoInterface.SalvarConfiguracoes();
            servicoInterface.SalvarConfiguracoesDefault();

            // Avisa do fechamento da janela
            servicoInterface.SinalizarJanelaFechando(_item.Info);
        }

        private void listarDesktops()
        {
            // Inicializa
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();

            // Remove itens anteriores
            toolbarDesktops.ItemLinks.Clear();
            for (int i = 0; i < menuDesktops.ItemLinks.Count; i++)
            {
                if (menuDesktops.ItemLinks[i].Item.Tag != null)
                {
                    int removerAte = menuDesktops.ItemLinks.Count;
                    for (int i2 = i; i2 < removerAte; i2++)
                        menuDesktops.ItemLinks.RemoveAt(i);
                    break;
                }
            }

            // Adiciona novamente
            foreach (DesktopInfo desktopInfo in servicoInterface.Desktops)
            {
                BarButtonItem item = new BarButtonItem(barManager, desktopInfo.Nome);
                _desktopButtons.Add(item);
                item.ButtonStyle = BarButtonStyle.Check;
                item.ImageIndex = 0;
                item.Tag = desktopInfo;
                item.ItemClick += delegate(object sender, ItemClickEventArgs e)
                {
                    foreach (BarItemLink link in toolbarDesktops.ItemLinks)
                        if (e.Item != link.Item)
                            ((BarButtonItem)link.Item).Down = false;
                    servicoInterface.MostrarDesktop(((DesktopInfo)e.Item.Tag).Id);
                    statusDesktopAtivo.Caption = ((DesktopInfo)e.Item.Tag).Nome;
                };
                BarItemLink menuLink = menuDesktops.AddItem(item);
                if (toolbarDesktops.ItemLinks.Count == 0)
                    menuLink.BeginGroup = true;
                toolbarDesktops.AddItem(item);

                // Se for o desktop ativo, seleciona o botão
                if (servicoInterface.DesktopAtivo.Id == desktopInfo.Id)
                    item.Down = true;
            }
        }

        #region IInterfaceBase<Janela> Members

        public void Inicializar(Janela itemBase)
        {
            _item = itemBase;
        }

        public void MostrarJanela()
        {
            this.Show();
        }

        public void EsconderJanela()
        {
            this.Hide();
        }

        public void Ativar()
        {
            this.Focus();
        }

        public void AdicionarControle(Controle controle)
        {
            Control control = (Control)controle.Instancia;
            control.Dock = DockStyle.Fill;
            this.Controls.Add(control);
        }

        public void AdicionarComando(Comando comando)
        {
            // Cria o botao
            BarButtonItem button = new BarButtonItem(barManager, comando.Info.Titulo);
            button.Hint = comando.Info.Titulo;
            button.ItemClick += delegate(object sender, ItemClickEventArgs e)
            {
                comando.Instancia.Executar();
            };

            // Carrega a imagem, caso exista
            if (comando.Info.ImagemArquivo != null && System.IO.File.Exists(comando.Info.ImagemArquivo))
            {
                ((DevExpress.Utils.ImageCollection)button.Images).AddImage(Image.FromFile(comando.Info.ImagemArquivo));
                button.ImageIndex = ((DevExpress.Utils.ImageCollection)button.Images).Images.Count - 1;
            }

            // Adiciona nos locais necessários
            if (comando.Info.RegistrarEmMenu)
                menuAplicacoes.AddItem(button);
            if (comando.Info.RegistrarEmToolbar)
                toolbarAplicacoes.AddItem(button);
        }

        public void SetarTitulo(string titulo)
        {
            this.Text = titulo;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Cria classe de parametros
            JanelaLauncherParametro janelaParametro = new JanelaLauncherParametro();
            janelaParametro.WindowState = this.WindowState;
            if (this.WindowState != FormWindowState.Normal) this.WindowState = FormWindowState.Normal;
            janelaParametro.Height = this.Height;
            janelaParametro.Width = this.Width;
            janelaParametro.Top = this.Top;
            janelaParametro.Left = this.Left;
            janelaParametro.SkinName = defaultLookAndFeel.LookAndFeel.SkinName;

            // Retorna
            return janelaParametro;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            bool visible = this.Visible;

            JanelaLauncherParametro janelaParametro = parametros as JanelaLauncherParametro;
            if (janelaParametro != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Width = janelaParametro.Width;
                this.Height = janelaParametro.Height;
                this.Top = janelaParametro.Top;
                this.Left = janelaParametro.Left;
                this.WindowState = janelaParametro.WindowState;
                this.defaultLookAndFeel.LookAndFeel.SkinName = janelaParametro.SkinName;
            }
        }

        public void Fechar()
        {
            this.Close();
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        public void MostrarConfiguracoes()
        {
        }

        #endregion

        private void menuAdicionarDesktop_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Cria o desktop
            DesktopInfo desktopInfo =
                new DesktopInfo()
                {
                    Nome = "(novo desktop)"
                };

            // Pede informacoes da novo desktop
            JanelaLauncherConfigurarDesktop configurarDesktop = new JanelaLauncherConfigurarDesktop(desktopInfo);
            if (configurarDesktop.ShowDialog(this) != DialogResult.OK)
                return;
            desktopInfo.Nome = configurarDesktop.DesktopInfo.Nome;
            
            // Inicializa
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            
            // Adiciona no servico da interface
            servicoInterface.Desktops.Add(desktopInfo);

            // Mostra o desktop recem criado
            servicoInterface.MostrarDesktop(desktopInfo.Id);

            // Atualiza a lista de desktops
            listarDesktops();
        }

        private void menuSair_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void menuRemoverDesktop_ItemClick(object sender, ItemClickEventArgs e)
        {
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            if (servicoInterface.Desktops.Count > 1)
            {
                servicoInterface.RemoverDesktop(servicoInterface.DesktopAtivo.Id);
                servicoInterface.MostrarDesktop(servicoInterface.Desktops[0].Id);
                listarDesktops();
            }
            else
            {
                MessageBox.Show("Não é possível remover todos os desktops.", "Remover Desktop", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuConfigurarDesktop_ItemClick(object sender, ItemClickEventArgs e)
        {
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            DesktopInfo desktopAtivo = servicoInterface.DesktopAtivo;
            JanelaLauncherConfigurarDesktop janelaConfigurar = new JanelaLauncherConfigurarDesktop(desktopAtivo);
            if (janelaConfigurar.ShowDialog(this) == DialogResult.OK)
            {
                // Copia as alterações no desktop atual
                desktopAtivo.Nome = janelaConfigurar.DesktopInfo.Nome;

                // Atualiza elementos da tela

            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            statusMemory.Caption = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 ^ 2).ToString();
        }

        private void menuSalvarConfiguracoes_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                servicoInterface.SalvarConfiguracoes(saveFileDialog.FileName);
            }
        }

        private void menuCarregarConfiguracoes_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                servicoInterface.CarregarConfiguracoes(openFileDialog.FileName);

                listarDesktops();
            }
        }
    }
}