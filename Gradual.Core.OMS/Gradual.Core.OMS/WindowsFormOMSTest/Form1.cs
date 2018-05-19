using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Core.Ordens.Lib;
using Gradual.Core.Ordens.Lib.Dados.Enum;
using Gradual.Core.Ordens.Persistencia;
using Gradual.Core.Ordens;
using Gradual.OMS.Library;
using Gradual.Core.Ordens.Callback;
using Gradual.Core.OMS.LimiteBMF.Lib;
using Gradual.Core.OMS.LimiteBMF;
using System.Text.RegularExpressions;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.Core.OrdensMonitoracao.ADM.Lib;
using Gradual.Core.OrdensMonitoracao.ADM.Lib.Mensagens;
using HelloService.Lib;

namespace WindowsFormOMSTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void InserirLimiteInstrumento()
        {
            ClienteParametroBMFInstrumentoInfo info     = new ClienteParametroBMFInstrumentoInfo();
            InserirLimiteBMFInstrumentoRequest request  = new InserirLimiteBMFInstrumentoRequest();
            InserirLimiteBMFInstrumentoResponse response = new InserirLimiteBMFInstrumentoResponse();

            info.IdClienteParametroBMF = 16;
            info.Instrumento = "INDZ13";              
            info.QtDisponivel = 5;
            info.QtTotalInstrumento = 5;
            info.QtTotalContratoPai = 10;

            request.LimiteBMFInstrumento = info;

            response = new ServicoLimiteBMF().AtualizarLimiteInstrumentoBMF(request);


        }

        private void InserirParametroBMF()
        {
            ClienteParametroLimiteBMFInfo ClienteParametroBMFInfo = new ClienteParametroLimiteBMFInfo();
            
            InserirLimiteClienteBMFRequest  request  = new InserirLimiteClienteBMFRequest();
            InserirLimiteClienteBMFResponse response = new InserirLimiteClienteBMFResponse();

            ServicoLimiteBMF ServicoLimiteBMF = new ServicoLimiteBMF();

            ClienteParametroBMFInfo.Account = 31217;
            ClienteParametroBMFInfo.DataValidade = DateTime.Now.AddDays(5);
            ClienteParametroBMFInfo.Contrato = "DOL";
            ClienteParametroBMFInfo.idClientePermissao = 30;
            ClienteParametroBMFInfo.QuantidadeDisponivel = 10;
            ClienteParametroBMFInfo.QuantidadeTotal = 10;
            ClienteParametroBMFInfo.RenovacaoAutomatica = 'S';
            ClienteParametroBMFInfo.Sentido = "V";

            request.ClienteParametroLimiteBMFInfo = ClienteParametroBMFInfo;
            response = ServicoLimiteBMF.AtualizarLimiteBMF(request);           

        }


        private void LimparCampos()
        {
            TXTVISTACOMPRADISPONIVEL.Text = string.Empty;
            TXTVISTACOMPRAALOCADO.Text    = string.Empty;
            TXTVISTACOMPRATOTAL.Text      = string.Empty;
            TXTVISTAVENDADISPONIVEL.Text  = string.Empty;
            TXTVISTAVENDAALOCADO.Text     = string.Empty;
            TXTVISTAVENDATOTAL.Text       = string.Empty;
            TXTOPCAOVENDADISPONIVEL.Text  = string.Empty;
            TXTOPCAOVENDAALOCADO.Text     = string.Empty;
            TXTOPCAOVENDATOTAL.Text       = string.Empty;
            TXTOPCAOCOMPRADISPONIVEL.Text = string.Empty;
            TXTOPCAOCOMPRAALOCADO.Text    = string.Empty;
            TXTOPCAOCOMPRATOTAL.Text      = string.Empty;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CalularLimites();
          
        }


        private void CalularLimites()
        {

            this.LimparCampos();

            LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();

            LimiteOperacionalRequest.CodigoCliente = int.Parse(textBox1.Text);
            ObjetoLimitesOperacionais = ObjetoPersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);


            #region LIMITE OPERACIONAL PARA COMPRA A VISTA

            var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                     where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                     select p;

            //SALDO DE COMPRA NO MERCADO A VISTA.
            if (LIMITECOMPRAAVISTA.Count() > 0)
            {

                foreach (var ITEM in LIMITECOMPRAAVISTA)
                {
                    // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                    TXTVISTACOMPRADISPONIVEL.Text = ITEM.ValorDisponivel.ToString();
                    TXTVISTACOMPRAALOCADO.Text = ITEM.ValorAlocado.ToString();
                    TXTVISTACOMPRATOTAL.Text = ITEM.ValotTotal.ToString();

                }
            }


            #endregion

            #region LIMITE OPERACIONAL PARA VENDA A VISTA

            var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                    where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                    select p;

            //SALDO DE COMPRA NO MERCADO A VISTA.
            if (LIMITEVENDAAVISTA.Count() > 0)
            {

                foreach (var ITEM in LIMITEVENDAAVISTA)
                {
                    // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.    
                    TXTVISTAVENDADISPONIVEL.Text = ITEM.ValorDisponivel.ToString();
                    TXTVISTAVENDAALOCADO.Text = ITEM.ValorAlocado.ToString();
                    TXTVISTAVENDATOTAL.Text = ITEM.ValotTotal.ToString();

                }
            }


            #endregion

            #region LIMITE OPERACIONAL PARA COMPRA OPCOES

            var LIMITECOMPRAOPCOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                     where p.TipoLimite == TipoLimiteEnum.COMPRAOPCOES
                                     select p;

            //SALDO DE COMPRA NO MERCADO A VISTA.
            if (LIMITECOMPRAOPCOES.Count() > 0)
            {

                foreach (var ITEM in LIMITECOMPRAOPCOES)
                {
                    // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                    TXTOPCAOCOMPRADISPONIVEL.Text = ITEM.ValorDisponivel.ToString();
                    TXTOPCAOCOMPRAALOCADO.Text = ITEM.ValorAlocado.ToString();
                    TXTOPCAOCOMPRATOTAL.Text = ITEM.ValotTotal.ToString();

                }
            }


            #endregion  

            #region LIMITE OPERACIONAL PARA VENDA OPCOES

            var LIMITEVENDAOPCOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                    where p.TipoLimite == TipoLimiteEnum.VENDAOPCOES
                                    select p;

            //SALDO DE COMPRA NO MERCADO A VISTA.
            if (LIMITEVENDAOPCOES.Count() > 0)
            {

                foreach (var ITEM in LIMITEVENDAOPCOES)
                {
                    // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                    TXTOPCAOVENDADISPONIVEL.Text = ITEM.ValorDisponivel.ToString();
                    TXTOPCAOVENDAALOCADO.Text = ITEM.ValorAlocado.ToString();
                    TXTOPCAOVENDATOTAL.Text = ITEM.ValotTotal.ToString();

                }
            }

            #endregion
        }

        EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
        PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
        LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();

        private void Form1_Load(object sender, EventArgs e)
        {
        
            DateTime qq = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));

            //NewOrderCallback OrderCallback = new NewOrderCallback();
           // OrderCallback.StartRouterCallBack();

            txtData.Text = DateTime.Today.ToString("dd/MM/yyyy");

            //txtData.Text = DateTime.Now.ToShortDateString();
            //txtCliente.Focus();
            //cboValidade.Text = "DIA";
            //cboTipoOrdem.Text = "LIMITADA";

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void btnVenda_Click(object sender, EventArgs e)
        {

        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            EnviarOrdem();
            this.CalularLimites();       
        }

        private void btnNova_Click(object sender, EventArgs e)
        {

        }

        private void InativarLimiteOperacional()
        {
            ExcluirLimiteBMFRequest  request  = new ExcluirLimiteBMFRequest();
            ExcluirLimiteBMFResponse response = new ExcluirLimiteBMFResponse();

            request.IdClienteParametroInstrumento = 20;

            response = new ServicoLimiteBMF().InativarLimiteInstrumento(request);
        }

        private void InativarLimiteContrato()
        {
            InativarLimiteContratoRequest  request  = new InativarLimiteContratoRequest();
            InativarLimiteContratoResponse response = new InativarLimiteContratoResponse();

            request.IdClienteParametroBMF = 24;

            response = new ServicoLimiteBMF().InativarLimiteContrato(request);
        }

        private void InserirLimiteInstrumentoCliente()
        {
            #region Instrumento

            ClienteParametroBMFInstrumentoInfo ContratoInstrumento = new ClienteParametroBMFInstrumentoInfo();
            InserirLimiteBMFInstrumentoRequest RequestInstrumento = new InserirLimiteBMFInstrumentoRequest();
            InserirLimiteBMFInstrumentoResponse ResponseInstrumento = new InserirLimiteBMFInstrumentoResponse();

            ContratoInstrumento.IdClienteParametroBMF = 16;
            ContratoInstrumento.ContratoBase = "IND";
            ContratoInstrumento.Instrumento = "INDZ13";
            ContratoInstrumento.QtDisponivel = 5;
            ContratoInstrumento.QtTotalInstrumento =5;
            ContratoInstrumento.QtTotalContratoPai = 50;

            RequestInstrumento.LimiteBMFInstrumento = ContratoInstrumento;

            ResponseInstrumento = new ServicoLimiteBMF().AtualizarLimiteInstrumentoBMF(RequestInstrumento);

            #endregion
        }

        private void EnviarOrdem()
        {

            EnviarOrdemRequest request = new EnviarOrdemRequest();
            EnviarOrdemResponse response = new EnviarOrdemResponse();


            ServicoOrdens ServicoOrdens = new ServicoOrdens();

           // IServicoOrdens ServicoOrdens = Ativador.Get<IServicoOrdens>();

            request.ClienteOrdemInfo.Account = txtCliente.Text.DBToInt32();

            request.ClienteOrdemInfo.Symbol = txtPapel.Text.ToString();

            #region Validade

            if (cboValidade.Text == "ValidaParaoDia")
            {
                request.ClienteOrdemInfo.ExpireDate = DateTime.Parse(txtData.Text);
                request.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;
            }

            if (cboValidade.Text == "ValidaAteDeterminadaData")
            {
                request.ClienteOrdemInfo.ExpireDate = DateTime.Parse(txtData.Text);
                request.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;
            }

            if (cboValidade.Text == "ExecutaIntegralouCancela")
            {
                request.ClienteOrdemInfo.ExpireDate = DateTime.Parse(txtData.Text);
                request.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ExecutaIntegralOuCancela;
            }

            if (cboValidade.Text == "ExecutaIntegralParcialouCancela")
            {
                request.ClienteOrdemInfo.ExpireDate = DateTime.Parse(txtData.Text);
                request.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ExecutaIntegralParcialOuCancela;
            }

            if (cboValidade.Text == "FechamentoDoMercado")
            {
                request.ClienteOrdemInfo.ExpireDate = DateTime.Parse(txtData.Text);
                request.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.FechamentoDoMercado;
            }

            if (cboValidade.Text == "BoaParaLeilao")
            {
                request.ClienteOrdemInfo.ExpireDate = DateTime.Parse(txtData.Text);
                request.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.BoaParaLeilao;
            }

            if (cboValidade.Text == "ValidaParaAberturaDoMercado")
            {
                request.ClienteOrdemInfo.ExpireDate = DateTime.Parse(txtData.Text);
                request.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaAberturaDoMercado;
            }

            #endregion
 
            #region Tipo Ordem

           // request.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada;

            #endregion


            if (cboSentido.SelectedItem == "COMPRA")
            {
                request.ClienteOrdemInfo.Side = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra;
            }
            else
            {
                request.ClienteOrdemInfo.Side = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda;
            }



            request.ClienteOrdemInfo.Price = double.Parse(cboPreco.Text.ToString());
            request.ClienteOrdemInfo.OrdType = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemTipoEnum.Limitada;
            request.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;
           // request.ClienteOrdemInfo.ExecBroker = "156";            

            request.ClienteOrdemInfo.OrderQty = int.Parse(txtQuantidade.Text);
            request.ClienteOrdemInfo.MaxFloor = int.Parse(txtQuantidadeAparente.Text);
            request.ClienteOrdemInfo.MinQty = int.Parse(txtQuantidadeMinima.Text);
            request.ClienteOrdemInfo.ChannelID = int.Parse(txtChannelID.Text);
            request.ClienteOrdemInfo.OrigClOrdID = txtCLordID.Text;
            //request.ClienteOrdemInfo.StopPrice = double.Parse(txtStopStart.Text);

            if (txtStopStart.Text != string.Empty)
            {
                request.ClienteOrdemInfo.StopPrice = double.Parse(txtStopStart.Text);
            }

            request.ClienteOrdemInfo.CompIDOMS = "HB";

            response = ServicoOrdens.EnviarOrdem(request);           

            MessageBox.Show(response.CriticaInfo[0].Critica);

        }

        private void LimparCamposOrdem()
        {
            txtPapel.Text = string.Empty;
    
            txtQuantidade.Text = string.Empty;
            txtQuantidadeAparente.Text = string.Empty;  
            txtQuantidadeMinima.Text = string.Empty;
            txtData.Text = string.Empty;
            cboSentido.SelectedItem  = "";
      
            cboValidade.SelectedItem  = "";
            cboValidade.Text = "DIA";
       
            txtCliente.Focus();

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            ExecutarCancelamentoOrdemResponse EnviarCancelamentoOrdemResponse = new ExecutarCancelamentoOrdemResponse();
            EnviarCancelamentoOrdemRequest  EnviarCancelamentoOrdemRequest = new EnviarCancelamentoOrdemRequest();
            ServicoOrdens ServicoOrdens = new ServicoOrdens();

            //IServicoOrdens ServicoOrdens = Ativador.Get<IServicoOrdens>();

            EnviarCancelamentoOrdemRequest.ClienteCancelamentoInfo.OrigClOrdID = txtNumeroControle.Text;
            EnviarCancelamentoOrdemRequest.ClienteCancelamentoInfo.Account = int.Parse(txtCliente.Text);
            EnviarCancelamentoOrdemRequest.ClienteCancelamentoInfo.ChannelID = int.Parse(txtChannelID.Text);
            EnviarCancelamentoOrdemRequest.ClienteCancelamentoInfo.Symbol = txtPapel.Text;


            EnviarCancelamentoOrdemResponse = ServicoOrdens.CancelarOrdem(EnviarCancelamentoOrdemRequest);

            MessageBox.Show(EnviarCancelamentoOrdemResponse.DadosRetorno.Ocorrencias[0].Ocorrencia);
            MessageBox.Show("Ordem cancelada com sucesso.");

            this.CalularLimites();
            
        }

        private void btnCallback_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //InserirParametroBMF();
            InserirParametroBMF();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            ListarLimites();

        }


        private void ListarLimites()
        {

            ListarLimiteBMFRequest  request = new ListarLimiteBMFRequest();
            ListarLimiteBMFResponse response = new ListarLimiteBMFResponse();
            ServicoLimiteBMF ServicoLimiteBMF = new ServicoLimiteBMF();

            request.Account = 31217;

            ServicoLimiteBMF.ObterLimiteBMFCliente(request);

            


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

