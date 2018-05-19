using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Library;
using Gradual.OMS.AnaliseGrafica.Lib.Dados;
using Gradual.OMS.AnaliseGrafica.Lib;
using System.Globalization;
using System.Configuration;

namespace AppImportacaoCotacoesHistoricas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            DialogResult okClicked = openFileDialog1.ShowDialog();

            if (okClicked == System.Windows.Forms.DialogResult.OK)
            {
                // Open the selected file to read.
                System.IO.Stream fileStream = openFileDialog1.OpenFile();
                System.IO.StreamReader reader = new System.IO.StreamReader(fileStream);

                do
                {
                    string line = reader.ReadLine();

                    string regtype = line.Substring(0,2);

                    switch(regtype)
                    {
                        case "00":
                            CotaHist_Header header = Utilities.MarshalFromStringBlock<CotaHist_Header>(line);
                            break;
                        case "01":
                            CotaHist_DadosCotacao registro = Utilities.MarshalFromStringBlock<CotaHist_DadosCotacao>(line);

                            CotacaoANG cotacao = new CotacaoANG();
                            cotacao.A = registro.CodNegociacao.ByteArrayToString().Trim();
                            cotacao.Ab = Convert.ToDouble(registro.PrecoAbertura.ByteArrayToDecimal(2));
                            cotacao.Bo = "BOV";
                            string datapregao = registro.DataPregao.ByteArrayToString() + "000000";
                            cotacao.Dt = DateTime.ParseExact(datapregao,"yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                            cotacao.Fe = Convert.ToDouble(registro.PrecoUltimo.ByteArrayToDecimal(2));
                            cotacao.Id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            cotacao.Me = Convert.ToDouble(registro.PrecoMedio.ByteArrayToDecimal(2));
                            cotacao.Mi = Convert.ToDouble(registro.PrecoMinimo.ByteArrayToDecimal(2));
                            cotacao.Mx = Convert.ToDouble(registro.PrecoMaximo.ByteArrayToDecimal(2));
                            cotacao.OfC = Convert.ToDouble(registro.MelhorPrecoCompra.ByteArrayToDecimal(2));
                            cotacao.OfV = Convert.ToDouble(registro.MelhorPrecoVenda.ByteArrayToDecimal(2));
                            cotacao.Pr = Convert.ToDouble(registro.PrecoUltimo.ByteArrayToDecimal(2));
                            cotacao.Qt = Convert.ToDouble(registro.QuantidadeTotal.ByteArrayToString());
                            cotacao.To = Convert.ToDouble(registro.TotalNegocios.ByteArrayToString());
                            cotacao.Vl = Convert.ToDouble(registro.VolumeTotal.ByteArrayToDecimal(2));
                            if ( cotacao.Fe > cotacao.Ab )
                                cotacao.Os = (cotacao.Fe / cotacao.Ab);
                            else
                                if (cotacao.Fe < cotacao.Ab)
                                    cotacao.Os = ((1 - (cotacao.Fe / cotacao.Ab)) * 100) * -1;
                                else
                                    cotacao.Os = 0.0;

                            string msg = String.Format("Gravando Historico: A:[{0}] Dt [{1}] Ab[{2}] Max[{3}] Med[{4}] Min[{5}] Fe[{6}] Osc[{7}] Qt[{8}] Vl[{9}] Neg[{10}]",
                                cotacao.A,
                                cotacao.Dt.ToString("yyyy/MM/dd HH:mm:ss"),
                                cotacao.Ab,
                                cotacao.Mx,
                                cotacao.Me,
                                cotacao.Mi,
                                cotacao.Fe,
                                cotacao.Os,
                                cotacao.Qt,
                                cotacao.Vl,
                                cotacao.To);

                            txtResult.Text += msg + "\r\n";

                            ANGPersistenciaDB db = new ANGPersistenciaDB();
                            db.ConnectionString = ConfigurationManager.ConnectionStrings["MDS"].ConnectionString;

                            db.GravaSerieHistorica(cotacao);

                            Application.DoEvents();
                            break;
                        case "99":
                            CotaHist_Trailer trailer = Utilities.MarshalFromStringBlock<CotaHist_Trailer>(line);
                            break;
                    }
                }
                while(!reader.EndOfStream );


                reader.Close();
                fileStream.Close();
            }

        }
    }
}
