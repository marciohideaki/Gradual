using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Integracao.IndiceGradual.Teste.localhost;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;

namespace Gradual.Integracao.IndiceGradual.Teste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                WSIndiceGradualInterface wsIndiceGradual = new WSIndiceGradualInterface();

                wsIndiceGradual.Url = cmbURL.Items[cmbURL.SelectedIndex] + "/WSIndiceGradualInterface.asmx";

                txtRetorno.Text = wsIndiceGradual.QueryIndiceGradualString(txtUsuario.Text, txtSenha.Text, 1, Int32.Parse(txtIdIndice.Text));

                XDocument doc = XDocument.Parse(txtRetorno.Text);
                IEnumerable<XElement> dadosXml = doc.Descendants("indices");

                if (dadosXml.ElementAt(0).Element("erro") != null)
                {
                    lblCotacao.Visible = false;
                    lblFechamento.Visible = false;
                    lblVariacao.Visible = false;
                    lblDataCotacao.Visible = false;
                    lblNomeIndice.Text = dadosXml.ElementAt(0).Element("erro").Value;
                }
                else if (dadosXml.ElementAt(0).Element("indice") != null)
                {
                    lblCotacao.Visible = true;
                    lblFechamento.Visible = true;
                    lblVariacao.Visible = true;
                    lblDataCotacao.Visible = true;
                    lblNomeIndice.Text = dadosXml.ElementAt(0).Element("indice").Element("nome-indice").Value;
                    lblCotacao.Text = dadosXml.ElementAt(0).Element("indice").Element("cotacao").Value;
                    lblFechamento.Text = dadosXml.ElementAt(0).Element("indice").Element("fechamento").Value;
                    lblVariacao.Text = dadosXml.ElementAt(0).Element("indice").Element("variacao").Value;
                    lblDataCotacao.Text = dadosXml.ElementAt(0).Element("indice").Element("data-cotacao").Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtUsuario.Text = "indicegradual";
            txtSenha.Text = "indgra2013*";
            txtIdIndice.Text = "1";

            cmbURL.Items.Add("http://localhost:4155");
            cmbURL.Items.Add("http://wsindicegradualinterface.gradualinvestimentos.com.br");
            cmbURL.SelectedIndex = 0;
        }
    }
}
