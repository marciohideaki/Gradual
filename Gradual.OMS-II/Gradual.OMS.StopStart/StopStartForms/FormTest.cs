using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Ordens.StartStop;
using Gradual.OMS.Ordens.StartStop.Geral;
using Gradual.OMS.Ordens.StartStop.Lib;
using Gradual.OMS.Ordens.StartStop.Lib.Enum;

//using System.Web;

namespace StopStartForms
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
        }
        
        public static bool ServicosCarregados = false;

        IServicoOrdemStopStart _Servico = null;

        private IList AdicionarCombo()
        {
            ArrayList Lista = new ArrayList();

            Lista.Add(new KeyValuePair<int, string>(1, "Stop Loss"));
            Lista.Add(new KeyValuePair<int, string>(2, "Stop Gain"));
            Lista.Add(new KeyValuePair<int, string>(3, "Stop Simultaneo"));
            Lista.Add(new KeyValuePair<int, string>(5, "Start Compra"));

            return Lista;

        }

        private void btCancelar_Click(object sender, EventArgs e) //--> Cancelar Ordem StartStop
        {
            try
            {
                _Servico = Ativador.Get<IServicoOrdemStopStart>();

                if (_Servico != null)
                {
                    CancelarStartStopOrdensRequest lRequestCancelamento = new CancelarStartStopOrdensRequest();

                    lRequestCancelamento.IdStopStart = int.Parse(txtIdStopStart.Text);
                    lRequestCancelamento.IdStopStartStatus = (int)OrdemStopStatus.CancelamentoEnviadoMDS;
                    lRequestCancelamento.Instrument = txtInstrumento.Text;

                    CancelarStartStopOrdensResponse lRespostaCancelamento = _Servico.CancelaOrdemStopStart(lRequestCancelamento);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDisparar_Click(object sender, EventArgs e) //--> Dispara Ordem StartStop
        {
            StopStartTipoEnum StopStartTipo = (StopStartTipoEnum)(int.Parse(cbTipoOrdem.SelectedValue.ToString()));

            ArmarStopLoss(int.Parse(txtCodigoCliente.Text), txtInstrumento.Text, decimal.Parse(txtPreco.Text), decimal.Parse(txtPreco2.Text), StopStartTipo);
        }

        private void btLimpar_Click(object sender, EventArgs e) //--> Limpar so campos
        {
            try
            {
                txtHistorico.Text = string.Empty;
            }
            catch
            {
                
            }
            
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            try
            {
                cbTipoOrdem.DataSource = AdicionarCombo();
                cbTipoOrdem.DisplayMember = "Value";
                cbTipoOrdem.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private T BuscarServicoDoAtivador<T>()
        {
            if (ServicosCarregados == false)
            {
                if (!ServicoHostColecao.Default.Servicos.ContainsKey(string.Format("{0}-", typeof(T))))
                    ServicoHostColecao.Default.CarregarConfig(ConfiguracoesValidadas.TipoDeObjetoAtivador);

                ServicosCarregados = true;
            }

            return Ativador.Get<T>();
        }

        private void ArmarStopLoss(int idCliente, string Instrumento, decimal preco, decimal preco2, StopStartTipoEnum StopStartTipo)
        {
            try
            {
                _Servico = Ativador.Get<IServicoOrdemStopStart>();

                if (_Servico != null)
                {

                    AutomacaoOrdensInfo _Order = new AutomacaoOrdensInfo();

                    ///*
                    //_Order.IdBolsa           = 1;
                    //_Order.IdCliente         = idCliente;
                    //_Order.IdMercadoTipo     = 1;
                    //_Order.IdStopstartStatus = (int)OrdemStopStatus.Registrado;
                    //_Order.IdStopStartTipo   = StopStartTipo;
                    //_Order.Instrumento       = Instrumento;
                    //_Order.DataValidade      = DateTime.Now;
                    //*/
                    _Order.IdBolsa = 1;
                    _Order.Symbol = Instrumento;
                    _Order.StopStartStatusID = (int)OrdemStopStatus.RegistradaAplicacao;
                    _Order.ExpireDate = DateTime.Now;
                    _Order.Account = idCliente;
                    _Order.OrdTypeID = 83; //--StopStart
                    _Order.IdStopStartTipo = StopStartTipo;
                    ////_Order.

                    if (StopStartTipo == StopStartTipoEnum.StopLoss)
                    {
                        _Order.SendStopGainPrice = null;
                        _Order.StopGainValuePrice = null;
                        _Order.StopLossValuePrice = preco;
                        _Order.SendStopLossValuePrice = preco2;
                        _Order.StartPriceValue = null;
                        _Order.OrderQty = 100;
                        _Order.AdjustmentMovelPrice = Conversao.ToDecimal(txtAdjMovelPrice.Text);
                        _Order.InitialMovelPrice = Conversao.ToDecimal(txtInitialMovelPrice.Text);
                        _Order.AdjustmentMovelPrice = null;
                        _Order.InitialMovelPrice = null;

                        ArmarStartStopResponse _Response = _Servico.ArmarStopStartGeral(
                              new ArmarStartStopRequest()
                              {
                                  _AutomacaoOrdensInfo = _Order
                              });

                        //_Servico.ArmarStopLoss(new ArmarStartStopRequest() { _AutomacaoOrdensInfo = _Order });

                        //Registrador.AddEvent(EventMds, Eventos);

                        //if (Contexto.SocketPrincipal != null)
                        //{
                        //    Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Cliente não esta conectado.");
                        //}
                    }

                    if (StopStartTipo == StopStartTipoEnum.StopGain)
                    {
                        _Order.StopGainValuePrice = preco;
                        _Order.SendStopGainPrice = preco2;
                        _Order.StopLossValuePrice = null;
                        _Order.SendStopLossValuePrice = null;
                        _Order.OrderQty = 90;
                        _Order.AdjustmentMovelPrice = null;
                        _Order.InitialMovelPrice = null;


                        ArmarStartStopResponse _Response = _Servico.ArmarStopStartGeral(
                              new ArmarStartStopRequest()
                              {
                                  _AutomacaoOrdensInfo = _Order
                              });

                        //Registrador.AddEvent(EventMds, Eventos);

                        txtHistorico.Text += _Response.DescricaoResposta + "\n";

                        //if (Contexto.SocketPrincipal != null)
                        //{
                        //    Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Cliente não esta conectado.");
                        //}
                    }

                    if (StopStartTipo == StopStartTipoEnum.StopSimultaneo)
                    {
                        _Order.StopLossValuePrice = preco;
                        _Order.StopGainValuePrice = preco2;
                        _Order.SendStopGainPrice = Convert.ToDecimal(txtSendGain.Text);
                        _Order.SendStopLossValuePrice = Convert.ToDecimal(txtSendLoss.Text);
                        _Order.OrderQty = 1;
                        //_Order.AdjustmentMovelPrice   = Convert.ToDecimal(txtAdjMovelPrice.Text);
                        //_Order.InitialMovelPrice      = Convert.ToDecimal(txtInitialMovelPrice.Text);


                        ArmarStartStopResponse _Response = _Servico.ArmarStopStartGeral(
                              new ArmarStartStopRequest()
                              {
                                  _AutomacaoOrdensInfo = _Order
                              });


                        txtHistorico.Text += string.Concat(_Response.DescricaoResposta, "\n");


                        //if (Contexto.SocketPrincipal != null)
                        //{
                        //    Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Cliente não esta conectado.");
                        //}
                    }


                    if (StopStartTipo == StopStartTipoEnum.StartCompra)
                    {

                        _Order.StartPriceValue = preco;
                        _Order.SendStartPrice = preco2;
                        _Order.StopLossValuePrice = null;
                        _Order.SendStopLossValuePrice = null;
                        _Order.OrderQty = 90;
                        _Order.AdjustmentMovelPrice = null;
                        _Order.InitialMovelPrice = null;

                        ArmarStartStopResponse _Response = _Servico.ArmarStopStartGeral(
                          new ArmarStartStopRequest()
                          {
                              _AutomacaoOrdensInfo = _Order
                          });

                        //Registrador.AddEvent(EventMds, Eventos);

                        //if (Contexto.SocketPrincipal != null)
                        //{
                        //    Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Cliente não esta conectado.");
                        //}


                    }

                    //if (StopStartTipo == StopStartTipoEnum.StopMovel)
                    //{

                    //    _Order.StartPriceValue = preco;
                    //    _Order.SendStartPrice = preco2;
                    //    _Order.StopLossValuePrice = null;
                    //    _Order.SendStopLossValuePrice = null;
                    //    _Order.OrderQty = 100;
                    //    _Order.AdjustmentMovelPrice = decimal.Parse(txtAdjMovelPrice.Text);
                    //    _Order.InitialMovelPrice = decimal.Parse(txtInitialMovelPrice.Text);

                    //    ArmarStartStopResponse _Response = _Servico.ArmarStartCompra(
                    //      new ArmarStartStopRequest()
                    //      {
                    //          _AutomacaoOrdensInfo = _Order
                    //      });

                    //    //Registrador.AddEvent(EventMds, Eventos);

                    //    if (Contexto.SocketPrincipal != null)
                    //    {
                    //        Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Cliente não esta conectado.");
                    //    }


                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao armar o StopLoss: ", ex.Message));
            }
        }

        private void btnSelecionarOrdem_Click(object sender, EventArgs e)
        {
            AutomacaoOrdensDados ordemStop = new AutomacaoOrdensDados();

            AutomacaoOrdensInfo lOrder = ordemStop.SelecionaOrdemStopStart(987);

            //Formatador.ArmarStopSimples(lOrder);
        }
    }
}
