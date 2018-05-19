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
using Gradual.OMS.Contratos.Comum.Permissoes;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    public partial class FormAssociarPermissao : XtraForm
    {
        public FormAssociarPermissao()
        {
            InitializeComponent();

            this.Load += new EventHandler(FormAssociarPermissao_Load);
        }

        private void FormAssociarPermissao_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Lista as permissoes ordenado pelo nome
                foreach (PermissaoBase permissao in                     
                            from p in contexto.Permissoes
                            orderby p.Value.PermissaoInfo.NomePermissao
                            select p.Value)
                    cmbPermissao.Properties.Items.Add(permissao);

                // Status de permissoes
                foreach (string item in Enum.GetNames(typeof(PermissaoAssociadaStatusEnum)))
                    cmbStatus.Properties.Items.Add(item);
            }
        }

        public PermissaoBase Permissao { get; set; }
        public PermissaoAssociadaStatusEnum PermissaoStatus { get; set; }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.Permissao = (PermissaoBase)cmbPermissao.EditValue;
            this.PermissaoStatus = 
                (PermissaoAssociadaStatusEnum)
                    Enum.Parse(
                        typeof(PermissaoAssociadaStatusEnum), 
                        (string)cmbStatus.EditValue);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}
