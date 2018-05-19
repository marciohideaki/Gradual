using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Ordens
{
    public partial class PainelCotacao : UserControl, IControle
    {

        private Controle _item = null;

        public PainelCotacao()
        {
            InitializeComponent();

        }

        void button1_Click(object sender, EventArgs e)
        {
        }

        void grdv_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
        }

        #region IInterfaceBase<Controle> Members

        public void Inicializar(Controle controle)
        {
            _item = controle;
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
