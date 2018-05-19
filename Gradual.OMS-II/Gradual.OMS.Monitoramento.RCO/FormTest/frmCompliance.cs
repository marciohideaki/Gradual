using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Monitores.Compliance;
using Gradual.Monitores.Compliance.Lib;
using Gradual.OMS.Library.Servicos;

namespace FormTest
{
    public partial class frmCompliance : Form
    {
        public frmCompliance()
        {
            InitializeComponent();
        }

        private void frmCompliance_Load(object sender, EventArgs e)
        {
            //IServicoMonitorCompliance _gServico = Ativador.Get<IServicoMonitorCompliance>();    

            //ServerMonitorCompliance _gServico = new ServerMonitorCompliance();

           // NegociosDiretosRequest _request = new NegociosDiretosRequest();            

           // var _response = _gServico.ObterNegociosDiretos(_request);

            var lServico = new ServerMonitorCompliance();

            lServico.StartLoadSuitability(null, false);
            //lServico.CarregarMonitoresMemoria(null);

            //OrdensAlteradasDayTradeRequest _request = new OrdensAlteradasDayTradeRequest();

            //OrdensAlteradasDayTradeResponse _response = _gServico.ObterOrdensAlteradasIntraday(_request);

            //EstatisticaDayTradeRequest _request = new EstatisticaDayTradeRequest();

            //_request.Assessor = 6;
            //_request.CodigoCliente = 3895;

            //EstatisticaDayTradeResponse _response = new EstatisticaDayTradeResponse();
            //var ITEM = _gServico.ObterEstatisticaDayTradeBovespa(_request);

        } 
    }
}
