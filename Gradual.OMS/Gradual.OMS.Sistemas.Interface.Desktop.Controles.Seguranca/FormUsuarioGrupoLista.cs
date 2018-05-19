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
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    public partial class FormUsuarioGrupoLista : DevExpress.XtraEditors.XtraForm
    {
        public FormUsuarioGrupoLista()
        {
            InitializeComponent();

            this.Load += new EventHandler(FormUsuarioGrupoLista_Load);
        }

        private void FormUsuarioGrupoLista_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo de desenho
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Lista os grupos
                grd.DataSource = contexto.UsuarioGrupos.Values.ToList();
            }
        }

        public UsuarioGrupoInfo UsuarioGrupo { get; set; }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.UsuarioGrupo = (UsuarioGrupoInfo)grdv.GetFocusedRow();
            this.Close();
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}