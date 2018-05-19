using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Gradual.OMS.Library.Servicos;
using Gradual.Core.OMS.SmartTrader.Lib;
using Gradual.Core.OMS.SmartTrader.Facade;
using Gradual.Core.OMS.SmartTrader.Lib.Mensagens;
using Gradual.Core.OMS.SmartTrader.Lib.Dados;


namespace WindowsFormSmartTraderTest
{
    //class Observable
    //{
    //    public event EventHandler SomethingHappened;

    //    public void DoSomething()
    //    {
    //        EventHandler handler = SomethingHappened;
    //        if (handler != null)
    //        {
    //            handler("rafael", EventArgs.Empty);
    //        }
    //    }



    //}

    //class Observer
    //{
    //    public void HandleEvent(object sender, EventArgs args)
    //    {
    //        Console.WriteLine("Something happened to " + sender);
    //    }
    //}



    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDummy_Click(object sender, EventArgs e)
        {
            ISmartTrader smartTrader = Ativador.Get<ISmartTrader>();
            //ReloadLimitRequest req = new ReloadLimitRequest();
            smartTrader.DummyFunction();
            
        }

        private void callback_SmartOrderFilled(object sender, EventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {               
            //Method in charge of handle a new smartorder request
            //Setting things up
            SmartTraderOrderProcessor Broker = new SmartTraderOrderProcessor();
            EnviarOrdemSmartRequest request = new EnviarOrdemSmartRequest();
            EnviarOrdemSmartResponse response = new EnviarOrdemSmartResponse();

            OrdemSmart OrdemSmart = new OrdemSmart();

            OrdemSmart.Account = 31940;
            OrdemSmart.Instrument = "OIBR4";
            OrdemSmart.Qty = 100;
            OrdemSmart.OperacaoInicio.PrecoOrdem = 3;
            OrdemSmart.OperacaoInicio.PrecoDisparo = 3.30;
            OrdemSmart.OperacaoInicio.PrecoOrdemTipo = TipoPreco.EntrarImediatamente;

     
            OrdemSmart.OperacaoLucro.PrecoOrdem = 3;
            OrdemSmart.OperacaoLucro.PrecoDisparo = 3.30;
            OrdemSmart.OperacaoLucro.PrecoOrdemTipo = TipoPreco.Indefinido;

            OrdemSmart.OperacaoPerda.PrecoOrdem = 3;
            OrdemSmart.OperacaoPerda.PrecoDisparo = 3.30;
            OrdemSmart.OperacaoPerda.PrecoOrdemTipo = TipoPreco.Indefinido;

            request.SmartOrder = OrdemSmart;
            response = Broker.EnviarOrdemSmartTrader(request);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SmartTraderCallback callback = new SmartTraderCallback();
            callback.StartRouterCallBack();

            callback.SmartOrderFilled += new EventHandler(callback_SmartOrderFilled);
               
        }
    }
}
