using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.Servico.ValidacaoRocket.Test
{
    public delegate void UpdateTextCallback(string pMessage);

    public partial class frmTestValidacaoRocket : Form
    {
        Gradual.Servico.ValidacaoRocket.Lib.ServicoValidacaoRocket gServico = new Lib.ServicoValidacaoRocket();

        #region Métodos Private
        
        private void MensagemDeStatusFormat(string pMensagem, params object[] pParams)
        {
            MensagemDeStatus(string.Format(pMensagem, pParams));
        }

        private void MensagemDeStatus(string pMensagem)
        {
            string lMensagem = string.Format("{0}\r\n{1}", pMensagem, txtStatus.Text);

            txtStatus.Text = lMensagem;

            Application.DoEvents();
        }

        private void AtualizarTextBox(string pMensagem)
        {
            txtStatus.Invoke(new UpdateTextCallback(this.MensagemDeStatus), new object[] { pMensagem });
        }

        #endregion

        #region Construtor

        public frmTestValidacaoRocket()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void frmTestValidacaoRocket_Load(object sender, EventArgs e)
        {
            gServico.OnMensagemDeLog += new Lib.MensagemDeLogEventHandler(lServico_OnMensagemDeLog);

            List<int> lLista = new List<int>();

            Random lRand = new Random();

            for (var a = 1; a <= 10; a++)
            {
                lLista.Add(lRand.Next(1000, 23000)); //random
            }

            //lLista.Add(237705); //bianca

            /*
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            lLista.Add(lRand.Next(1000, 23000)); //random
            */

            gServico.IniciarServico(lLista);
        }

        private void lServico_OnMensagemDeLog(string pTipo, string pMensagem)
        {
            AtualizarTextBox(string.Format(">{0}: {1}", pTipo, pMensagem));
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (btnIniciar.Text == "Pausar")
            {
                btnIniciar.Text = "Reiniciar";
            }
            else
            {
                btnIniciar.Text = "Pausar";
            }

            gServico.PausarReIniciarServico();
        }

        #endregion

    }
}
