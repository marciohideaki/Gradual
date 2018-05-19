using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

using DevExpress.XtraBars;
using DevExpress.XtraEditors;

namespace Gradual.OMS.Sistemas.Interface.Desktop.SkinDevExpress
{
    public partial class JanelaForm : DevExpress.XtraEditors.XtraForm, IJanela
    {
        private Janela _item = null;
        
        public JanelaForm()
        {
            InitializeComponent();

            this.ShowInTaskbar = false;

            this.FormClosed += new FormClosedEventHandler(JanelaForm_FormClosed);
        }

        void JanelaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Pede para a interface retirar a janela
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            servicoInterface.RemoverJanela(_item.Info.Id);

            // Avisa do fechamento da janela
            servicoInterface.SinalizarJanelaFechando(_item.Info);
        }

        #region IInterfaceBase<Janela> Members

        public void Inicializar(Janela itemBase)
        {
            _item = itemBase;
        }

        public void MostrarJanela()
        {
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            //this.MdiParent = (Form)servicoInterface.Launcher.Instancia;
            this.Show((Form)servicoInterface.ReceberJanelaLauncher());
            //this.Show();
        }

        public void EsconderJanela()
        {
            this.Hide();
        }

        public void AdicionarControle(Controle controle)
        {
            ((Control)controle.Instancia).Dock = DockStyle.Fill;
            this.Controls.Add((Control)controle.Instancia);
            
            // O título da janela será o título do controle
            this.Text = controle.Info.Titulo;
        }

        public void AdicionarComando(Comando comando)
        {
            // Inclui na toolbar?
            if (comando.Info.RegistrarEmToolbar)
            {
                // Adiciona item na toolbar
                BarButtonItem button = new BarButtonItem(barManager, comando.Info.Titulo);
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
                
                // Adiciona item na toolbar
                toolbar.AddItem(button);
            }
        }

        public void SetarTitulo(string titulo)
        {
            this.Text = titulo;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Cria classe de parametros
            JanelaParametro janelaParametro = new JanelaParametro();
            janelaParametro.WindowState = this.WindowState;
            if (this.WindowState != FormWindowState.Normal) this.WindowState = FormWindowState.Normal;
            janelaParametro.Height = this.Height;
            janelaParametro.Width = this.Width;
            janelaParametro.Top = this.Top;
            janelaParametro.Left = this.Left;
            janelaParametro.Titulo = this.Text;
            janelaParametro.MostrarToolbar = toolbar.Visible;

            // Retorna
            return janelaParametro;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            JanelaParametro janelaParametro = parametros as JanelaParametro;
            if (janelaParametro != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Width = janelaParametro.Width;
                this.Height = janelaParametro.Height;
                this.Top = janelaParametro.Top;
                this.Left = janelaParametro.Left;
                this.WindowState = janelaParametro.WindowState;
                this.Text = janelaParametro.Titulo;
                toolbar.Visible = janelaParametro.MostrarToolbar;
            }
        }

        public void Fechar()
        {
            this.Close();
        }

        public void Ativar()
        {
            this.Focus();
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        public void MostrarConfiguracoes()
        {
            // Cria coleção dos objetos de configuração
            ColecaoTipoInstancia parametros = new ColecaoTipoInstancia();
            
            // Pede configuracoes da janela
            parametros.AdicionarItem(this.SalvarParametros(EventoManipulacaoParametrosEnum.Configuracao));

            // Pede configuracoes dos controles

            // Mostra tela de parametros

            // Caso ok...

            // Pede para janela carregar parametros

            // Pede para controles carregar parametros

        }

        #endregion
    }
}
