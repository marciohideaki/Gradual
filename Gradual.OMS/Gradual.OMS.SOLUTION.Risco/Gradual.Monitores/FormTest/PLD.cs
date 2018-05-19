using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Monitores.Risco.Info;
using Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Risco;
using Gradual.Monitores.Risco.Enum;
using System.Threading;
using Gradual.OMS.Library.Servicos;


namespace FormTest
{
    public partial class PLD : Form
    {
        ServerMonitor _Monitor = new ServerMonitor();
        System.Threading.Timer _TimerGrid;
       

        public PLD()
        {
            InitializeComponent();
        }

        IServicoMonitorRisco _gServico;
        private void PLD_Load(object sender, EventArgs e)
        {
            _Monitor.StartMonitor(null);

            // _gServico = Ativador.Get<IServicoMonitorRisco>(); 

           // _Monitor.StartMonitor(null);

            PreencheComboBox();
        }

     

        private void PreencheComboBox()
        {
            cbo.Items.Clear();
            cbo.Items.Add("ANALISE");
            cbo.Items.Add("APROVADO");
            cbo.Items.Add("REPROVADO");

        }

       


        private void Formatar()
        {

            for (int i = 0; i <= dgResumo.Rows.Count - 1; i++)
            {
                DataGridViewRow col = dgResumo.Rows[i];

                EnumCriticidadePLD _pld = (EnumCriticidadePLD)(col.Cells[4].Value);

                if (_pld == EnumCriticidadePLD.FOLGA)
                {
                    col.DefaultCellStyle.ForeColor = Color.Green;
                }

                if (_pld == EnumCriticidadePLD.CRITICO)
                {
                    col.DefaultCellStyle.ForeColor = Color.Yellow;
                    col.DefaultCellStyle.BackColor = Color.Red;
                }

                if (_pld == EnumCriticidadePLD.ALERTA)
                {
                    col.DefaultCellStyle.ForeColor = Color.Black;
                    col.DefaultCellStyle.BackColor = Color.Yellow;
                }

                if (_pld == EnumCriticidadePLD.PLDAPROVADO)
                {
                    col.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        

        private void PreenchePLD()
        {
            this.Invoke(new MethodInvoker(delegate()
             {
                 MonitorPLDRequest _request = new MonitorPLDRequest();

                 if (cbo.Text == "ANALISE"){
                     _request.EnumStatusPLD = EnumStatusPLD.EMANALISE;
                 }

                 if (cbo.Text == "APROVADO"){
                     _request.EnumStatusPLD = EnumStatusPLD.APROVADO;
                 }

                 if (cbo.Text == "REPROVADO"){
                     _request.EnumStatusPLD = EnumStatusPLD.REJEITADO;
                 }

                 MonitorPLDResponse _response = _Monitor.ObterMonitorPLD(_request);

                 int contador = _response.lstPLD.Count;

                 dgResumo.DataSource = _response.lstPLD;

                 lblatualizacao.Text = DateTime.Now.ToString();

             }));

        }

        private void button1_Click(object sender, EventArgs e)
        {
           // _TimerGrid = new System.Threading.Timer (new TimerCallback(IniciarTimer), null, 0, 3000);
            this.PreenchePLD();
            this.Formatar();
        }


        private void IniciarTimer(object valor)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                this.PreenchePLD();
                this.Formatar();
            }));



        }

    }
}
