#region Includes
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.ClubesFundos.Lib;
using Gradual.OMS.ClubesFundos;
using Gradual.OMS.Library.Servicos;
#endregion

namespace Gradual.OMS.ClubesFundos.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            IServicoClubesFundos lServico = Ativador.Get<IServicoClubesFundos>();

            ListarFundosResponse Fundos = lServico.ConsultarClientesFundos(
                new ListarFundosRequest()
                {
                    IdCliente = 36657
                });


            ListarClubesResponse Clubes = lServico.ConsultarClientesClubes(
                new ListarClubesRequest()
                {
                    IdCliente = 25461
                });
        }


    }
}
