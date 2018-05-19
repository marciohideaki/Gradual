using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.InvXX.Fundos.DbLib;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos;
using Gradual.OMS.InvXX.Fundos;
using Gradual.OMS.InvXX.Fundos.FINANCIAL;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA;

namespace Gradual.OMS.InvXX.Fundos.Teste
{
    public partial class frmImportacaoANBIMA : Form
    {
        public frmImportacaoANBIMA()
        {
            InitializeComponent();
        }

        private void btnImportacao_Click(object sender, EventArgs e)
        {
            try
            {
                ImportacaoFundosServico lServico = new ImportacaoFundosServico();
                //lServico.ImportacaoAvulsaANBIMA();

                lServico.ThreadImportacaoAnbima(null, false);
            }
            catch (Exception ex)
            {
                
            }
        }

        

        private List<string> RetornaCodigoAnbima()
        {
            var lRetorno = new List<string> ();
            lRetorno.Add("331597");
            lRetorno.Add("340243"); 
            lRetorno.Add("342319"); 
            lRetorno.Add("070815"); 
            lRetorno.Add("341770"); 
            lRetorno.Add("337031"); 
            lRetorno.Add("047368");
            lRetorno.Add("072176");
            lRetorno.Add("246611");
            lRetorno.Add("333077");
            lRetorno.Add("296961");
            lRetorno.Add("314153"); 
            lRetorno.Add("206032");
            lRetorno.Add("316725"); 
            lRetorno.Add("343961");
            lRetorno.Add("325325");
            lRetorno.Add("271063");
            lRetorno.Add("240737"); 
            lRetorno.Add("288888"); 
            lRetorno.Add("313947"); 
            lRetorno.Add("316873"); 
            lRetorno.Add("273066"); 
            lRetorno.Add("191566"); 
            lRetorno.Add("221260");
            lRetorno.Add("283908"); 
            lRetorno.Add("272949");
            lRetorno.Add("282553"); 
            lRetorno.Add("198978"); 
            lRetorno.Add("296139");
            lRetorno.Add("200069");
            lRetorno.Add("222097");
            lRetorno.Add("349781");
            lRetorno.Add("248738");
            lRetorno.Add("246360"); 
            lRetorno.Add("245054");
            lRetorno.Add("185086");
            lRetorno.Add("022780"); 
            lRetorno.Add("031224");
            lRetorno.Add("217913");
            lRetorno.Add("231991");
            lRetorno.Add("196657");
            lRetorno.Add("109630"); 
            lRetorno.Add("260088"); 
            lRetorno.Add("214906");
            lRetorno.Add("115231"); 
            lRetorno.Add("305121"); 
            lRetorno.Add("215163"); 
            lRetorno.Add("284491");
            lRetorno.Add("314331"); 
            lRetorno.Add("296430");
            lRetorno.Add("213403"); 
            lRetorno.Add("143359"); 
            lRetorno.Add("071609"); 
            lRetorno.Add("217018");
            lRetorno.Add("245062");
            lRetorno.Add("108766");
            lRetorno.Add("065714"); 

            return lRetorno;
        }
        private void btnImportar_Click(object sender, EventArgs e)
        {

            List<string> ListaFundos = new List<string>();

            var lServico = new ImportacaoFundosServico();

            if (txtCodigoAnbima.Text.Equals(string.Empty))
            {
                MessageBox.Show("Codigo Anbima inválido");
            }
            else
            {
                string CodigoAnbima = txtCodigoAnbima.Text;
                DateTime lDataInicial = DateTime.Now.AddYears(-5);// DateTime.Parse(txtDate.Text);
                lServico.ImportarDadosFundos(CodigoAnbima, lDataInicial);
                
            }
        }

        private void btnImportarTodos_Click(object sender, EventArgs e)
        {
            var lServico = new ImportacaoFundosServico();
            
            var lLista = RetornaCodigoAnbima();

            foreach (string CodigoAnbima in lLista)
            {
                DateTime lDataInicial = DateTime.Now.AddYears(-5);// DateTime.Parse(txtDate.Text);
                lServico.ImportarDadosFundos(CodigoAnbima, lDataInicial);
            }
        }

        private void btnImportarAvulsos_Click(object sender, EventArgs e)
        {
            var lServico = new ImportacaoFundosServico();

            lServico.ImportacaoAvulsaANBIMA();
        }

        private void btnTestarHistorico_Click(object sender, EventArgs e)
        {
            DateTime lDataInicial = DateTime.Now.AddDays(-10);
            DateTime lDataFinal = DateTime.Now;

            HistoricoCota.HistoricoCotaWSSoapClient lServico = new HistoricoCota.HistoricoCotaWSSoapClient();
            HistoricoCota.HistoricoCotaRetorno[] lRetorno = lServico.Exporta(lDataInicial, null, 273589, null);
            
            
        }
    }
}
