#region Includes
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.ClubesFundos;
using Gradual.OMS.ClubesFundos.Lib;
using Gradual.OMS.Library;
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

        private void btnClubes_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoClubesFundos lServico = Ativador.Get<IServicoClubesFundos>();

                //ServicosClubesFundos lServico = new ServicosClubesFundos();

                ListarClubesRequest lRequest = new ListarClubesRequest()
                {
                    IdCliente = Convert.ToInt32(txtCodCliente.Text)
                };

                ListarClubesResponse lResponse = lServico.ConsultarClientesClubes(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    MessageBox.Show("Deu certo");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFundos_Click(object sender, EventArgs e)
        {
            try
            {
                IServicoClubesFundos lServico = Ativador.Get<IServicoClubesFundos>();

                ListarFundosRequest lRequest = new ListarFundosRequest()
                {
                    IdCliente = Convert.ToInt32(txtCodCliente.Text)
                };

                ListarFundosResponse lResponse = lServico.ConsultarClientesFundos(lRequest);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
