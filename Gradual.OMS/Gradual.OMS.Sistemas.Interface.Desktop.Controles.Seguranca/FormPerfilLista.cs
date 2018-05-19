using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    public partial class FormPerfilLista : DevExpress.XtraEditors.XtraForm
    {
        public FormPerfilLista()
        {
            InitializeComponent();

            this.Load += new EventHandler(FormPerfilLista_Load);
        }

        private void FormPerfilLista_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo de desenho
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Lista os grupos
                grd.DataSource = contexto.Perfis.Values.ToList();
            }
        }

        public PerfilInfo Perfil { get; set; }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Perfil = (PerfilInfo)grdv.GetFocusedRow();
            this.Close();
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}