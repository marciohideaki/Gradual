using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Risco.RegraLib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Risco.RegraLib.Mensagens;

namespace AplcacaoTesteDeRisco
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IServicoRegrasRisco lRegrasRisco = Ativador.Get<IServicoRegrasRisco>();

            ListarGruposResponse lREs = lRegrasRisco.ListarGrupos(new ListarGruposRequest());

            MessageBox.Show(lREs.Grupos.Count.ToString());
        }
    }
}
