using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    public partial class FormUsuarioLista : XtraForm
    {
        public FormUsuarioLista()
        {
            InitializeComponent();

            this.Load += new EventHandler(FormUsuarioLista_Load);
        }

        private void FormUsuarioLista_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo de desenho
            if (!this.DesignMode)
            {
                // Referencia aos servicos
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();
                IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

                // Pede a lista de usuarios
                List<UsuarioInfo> usuarios =
                    ((ListarUsuariosResponse)
                        servicoAutenticador.ProcessarMensagem(
                            new ListarUsuariosRequest() 
                            { 
                                CodigoSessao = contexto.SessaoInfo.CodigoSessao
                            })).Usuarios;

                // Lista os grupos
                grd.DataSource = usuarios;
            }
        }

        public UsuarioInfo Usuario { get; set; }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Usuario = (UsuarioInfo)grdv.GetFocusedRow();
            this.Close();
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
