using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Gradual.OMS.InvXX.Fundos.FINANCIAL;

namespace Gradual.OMS.InvXX.Fundos.Teste
{
    public partial class frmImportacaoFinancial : Form
    {
        public frmImportacaoFinancial()
        {
            InitializeComponent();
        }

        private void btnImportacaoFinancial_Click(object sender, EventArgs e)
        {
            try
            {
                //ImportacaoClientePosicaoServico lservico = new ImportacaoClientePosicaoServico();
                //lservico.ThreadImportacaoPosicao();//.ThreadImportacaoPosicao();
                PosicaoCotista.PosicaoCotistaWSSoapClient lServico = new PosicaoCotista.PosicaoCotistaWSSoapClient();
                PosicaoCotista.ValidateLogin lLogin = new PosicaoCotista.ValidateLogin();

                lLogin.Username = "brocha";
                lLogin.Password = "gradual12345*";

                //PosicaoCotista.PosicaoCotistaViewModel[] lPosicao = lServico.Exporta(lLogin, null, 204, null);

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        //ContaCorrente.ContaCorrenteWS lServicoCC = new ContaCorrente.ContaCorrenteWS();
        //PosicaoCotista.PosicaoCotista


        //ContaCorrente.ValidateLogin lLoginCC = new ContaCorrente.ValidateLogin();

        //lLoginCC.Username = "webservice";
        //lLoginCC.Password = "gradual123*";

        //lServicoCC.ValidateLoginValue = lLoginCC;


        //ContaCorrente.ContaCorrenteViewModel[] lResponseCC = lServicoCC.ExportaListaContaCorrentePorCpfcnpjPessoa("330.921.208-41");

        //if (lResponseCC.Count() > 0)
        //{
        //    MessageBox.Show("Teste--> ");
        //}

        //FundoCota.FundoWS lServico = new FundoCota.FundoWS();

        //FundoCota.ValidateLogin lLogin = new FundoCota.ValidateLogin();

        //lLogin.Username = "webservice";
        //lLogin.Password = "gradual123*";

        //lServico.ValidateLoginValue = lLogin;

        //FundoCota.CarteiraViewModel[] response = lServico.ExportaListaCarteira(new int[] { 500 });

        //int[] tipos = {1};

        //FundosCota.CarteiraViewModel[] lResponse = lServico.ExportaListaCarteira(lLogin, tipos);
        //33092120841


        //string lStringXML = "<itaumsg>"+
        //                    "<parameter>"+
        //                    "<param id=\"campo0\" value=\"XXXXX\" />"                   + //--> EBUSINESSID
        //                    "<param id=\"campo1\" value=\"XXXXXX\" />"                  + //--> SENHA
        //                    "<param id=\"campo2\" value=\"xxxxxx\" />"                  + //--> CDBANC
        //                    "<param id=\"campo3\" value=\"xxxxx\" />"                   + //--> CDFDO
        //                    "<param id=\"campo4\" value=\"xxxxxx\" />"                  + //--> CDBANCLI - Conta do cliente
        //                    "<param id=\"campo5\" value=\"xxxx\" />"                    + //--> AGENCIA 
        //                    "<param id=\"campo6\" value=\"xxxxxxxxx\" />" + //--> CDCTA
        //                    "<param id=\"campo7\" value=\"x\" />" + //-->  DAC10 
        //                    "<param id=\"campo8\" value=\"xxx\" />" + //--> SUBCONT 
        //                    "<param id=\"campo9\" value=\"xxx\" />" + //--> OPEMOV 
        //                    "<param id=\"campo10\" value=\"XXXXXXXXXXXXXXX\" />" + //--> VLIQSOL 
        //                    "<param id=\"campo11\" value=\"XXXXXXXXXXXXXXXXXXXXXXX\" />" + //--> BCOAGCT1 
        //                    "<param id=\"campo12\" value=\"XXXXXX\" />" + //--> IDOPEMAC
        //                    "<param id=\"campo13\" value=\"X\" />" + //--> CDTIPLIQ
        //                    "<param id=\"campo14\" value=\"XXXXXXXXXX\" />" + // --> CDAPL
        //                    "<param id=\"campo15\" value=\"X\" />" + //--> IDTIPCT1
        //                    "<param id=\"campo16\" value=\"XXXXXXXX\" />" + //--> DATAGEND
        //                    "<param id=\"campo17\" value=\"XXXXXXXX\" />" + //--> DTLANCT
        //                    "</parameter>"+
        //                    "</itaumsg>";
        //ImportacaoClientePosicaoServico lRetorno = new ImportacaoClientePosicaoServico();

        //lRetorno.ThreadImportacaoPosicao();

        //var html = new WebClient().DownloadString("https://www.itaucustodia.com.br/PassivoWebServices/xmlmva.jsp");

        //PosicaoCotista.PosicaoCotistaWSSoapClient lServico = new PosicaoCotista.PosicaoCotistaWSSoapClient();

        //PosicaoCotista.PosicaoCotista[] lResposta = lServico.Exporta(null, 53109, null);

        //OperacaoCotista.OperacaoCotistaWSSoapClient lServico = new OperacaoCotista.OperacaoCotistaWSSoapClient();

        //OperacaoCotista.OperacaoCotista[] lResposta = lServico.Exporta(null, 53109, null, new DateTime(2013, 10, 1));

        //HistoricoCota.HistoricoCotaWSSoapClient lServico = new HistoricoCota.HistoricoCotaWSSoapClient();

        //HistoricoCota.HistoricoCota[] lResposta = lServico.Exporta(new DateTime(2013, 11, 04), new DateTime(2013, 11, 05), 246360, false);

        //CadastroCotista.CadastroCotistaWSSoapClient lServico = new CadastroCotista.CadastroCotistaWSSoapClient();

        //CadastroCotista.ValidateLogin login                  = new CadastroCotista.ValidateLogin() { Username = "webservice", Password="gradual123*" };

        //CadastroCotista.Cotista lResponse = lServico.ExportaPorCpfcnpj(login, "15545054880");

        //OperacaoCotista.OperacaoCotistaWSSoapClient lServico = new OperacaoCotista.OperacaoCotistaWSSoapClient();

        //OperacaoCotista.OperacaoCotista[] lResponse = lServico.Exporta(null, 43731, null, null);

        //CadastroCotista.CadastroCotistaWSSoapClient lServico = new CadastroCotista.CadastroCotistaWSSoapClient();

        //CadastroCotista.ExportaRequest lRequest = new CadastroCotista.ExportaRequest();

        //lServico.Exporta()

        //PosicaoGerencialWS.PosicaoGerencialServiceClient lServico = new PosicaoGerencialWS.PosicaoGerencialServiceClient();

        //var html = new WebClient().DownloadString("https://www.itaucustodia.com.br/PassivoWebServices/services/DownloadArquivoService");
    }
}
