using System;
using System.ServiceModel;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.AcompanhamentoOrdens;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;
using System.Collections.Generic;
using System.Globalization;
using Gradual.OMS.AcompanhamentoOrdens.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Ordens.Lib;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.Ordens.Lib.Mensageria;
using System.Threading;
using System.Configuration;


namespace Gradual.OMS.AcompanhamentoOrdens.HosterTeste
{
    class Program
    {

        private static bool OrdensExpiradasAtualizadas { set; get; }
        private static int HorarioFechamentoBolsa { set; get; }

        private static void CancelarOrdensExperiradas(object state)
        {

            if (DateTime.Now.Hour < HorarioFechamentoBolsa){
                OrdensExpiradasAtualizadas = false;
            }

            if ((DateTime.Now.Hour >= HorarioFechamentoBolsa) && (OrdensExpiradasAtualizadas == false))
            {         
                    CamadaDeDados _CamadaDeDados = new CamadaDeDados();

                    _CamadaDeDados.AbrirConexao();
                    List<string> Ordens = _CamadaDeDados.BuscarOrdensValidasParaoDia();

                    IServicoOrdens ServicoOrdens = Ativador.Get<IServicoOrdens>();

                    for (int i = 0; i <= Ordens.Count - 1; i++){

                        ClienteCancelamentoInfo ClienteCancelamentoInfo = new ClienteCancelamentoInfo(){
                            OrderID = Ordens[i].ToString()
                        };

                        EnviarCancelamentoOrdemRequest request = new EnviarCancelamentoOrdemRequest() {
                            ClienteCancelamentoInfo = ClienteCancelamentoInfo
                        };

                        EnviarCancelamentoOrdemResponse response =
                         ServicoOrdens.CancelarOrdem(request);

                    }

                    OrdensExpiradasAtualizadas = true;                
            }
        }


        static System.Threading.Timer _TimerTicker;


        static void Main(string[] args)
        {

            HorarioFechamentoBolsa = 15;
            _TimerTicker = new System.Threading.Timer(new TimerCallback(CancelarOrdensExperiradas), null, 0, 10000);

            //CancelarOrdensExperiradas(null;);

            //ServiceHost lServico;

            //ServicoAcompanhamentoOrdens.MensagemDeAcompanhamento += new MensagemDeAcompanhamentoHandler(ServicoAcompanhamentoOrdens_MensagemDeAcompanhamento);

            //lServico = new ServiceHost(typeof(ServicoAcompanhamentoOrdens));

            //lServico.Open();

            //Console.WriteLine("Serviço de acompanhamento de ordens iniciado...");

            //string lInput = "";

            //while (lInput != "q")
            //{
            //    lInput = Console.ReadLine();

            //    if (lInput == "i")
            //    {
            //        ServicoAcompanhamentoOrdens.Iniciar();
            //    }
            //    else
            //    {
            //        Console.WriteLine("Comando [{0}] não reconhecido; utilize q para sair, i para iniciar", lInput);
            //    }
            //}
            //IServicoAcompanhamentoOrdens lServico = Ativador.Get<IServicoAcompanhamentoOrdens>();
            //ServicoAcompanhamentoOrdens lServico = new ServicoAcompanhamentoOrdens();

            //BuscarOrdensResponse lOrdens =            lServico.BuscarOrdensSinacor(
            //    new BuscarOrdensRequest()
            //    {
            //        ContaDoCliente = 42089
            //    }
            //    );

   
            
           //ServicoAcompanhamentoOrdens dados = new ServicoAcompanhamentoOrdens();

            //dados.
           //CamadaDeDados camada = new CamadaDeDados();

            //CamadaDeDados camada = new CamadaDeDados();

            //camada.AbrirConexao();

            //List<OrdemInfo> info =  camada..BuscarOrdensOnline(31940);

            CamadaDeDados camada = new CamadaDeDados();

            //camada.AbrirConexao();

            camada.BuscarOrdensSinacorBmf(
                new BuscarOrdensRequest()
                {
                    ContaDoCliente = 42089,
                    PaginaCorrente = 1,
                    QtdeLimiteRegistros = 20
                });

            //camada.BuscarOrdensSinacor(
            //    new BuscarOrdensRequest()
            //    {
            //        ContaDoCliente = 42089,
            //        PaginaCorrente = 1,
            //        QtdeLimiteRegistros = 20
            //    }
            //    );

            //List<OrdemInfo> info = camada.BuscarOrdens(null, null, null, null, null, null, null, 11);

            //BuscarOrdensResponse lst = camada.BuscarOrdens.BuscarOrdensSinacor(new BuscarOrdensRequest() 
            //{ 
            //    DataDe         = new DateTime(2010,11,03,10,00,00),
            //    DataAte        = new DateTime(2010,11,04,16,00,00),
            //    PaginaCorrente = 1,
            //    QtdeLimiteRegistros = 10,
            //    CodigoAssessor = 22
            //});

            //var info = lst.Ordens;
            //BuscarOrdensResponse lOrdens =
            //dados.BuscarOrdens(
            //    new BuscarOrdensRequest()
            //        {
            //            ContaDoCliente      = 31940,
            //            PaginaCorrente      = 2,
            //            QtdeLimiteRegistros = 20,
            //            TotalRegistros      = 0,
            //            //Instrumento         = "GGBR4",
            //            DataDe              = new DateTime(2010,10,19, 0, 0 ,0 ),
            //            DataAte             = new DateTime(2010,10,22, 0 ,0,0 )
            //        }
            //    );
            //dados.BuscarOrdens(
            //        null,
            //        new DateTime(2010, 08, 30),
            //        new DateTime(2010, 09, 03),null,null,null,null
            //    );

            //foreach (OrdemInfo ordem in info)
            //{
            //    Console.WriteLine(string.Format("CBLC: {0}, Chanel: {1}, ID ordem: {2}, Quant: {3}, Quantidade remanescente: {4}, Preço: {5}, DataTeste: {6}",
            //        ordem.Account,
            //        ordem.ChannelID,
            //        ordem.IdOrdem,
            //        ordem.OrderQty,
            //        ordem.OrderQtyRemmaining,
            //        ordem.Price.ToString("C", new CultureInfo("pt-BR")),
            //        DateTime.Now.Date.AddDays(1).AddSeconds(-1)
            //        ));
            //} 
            


            Console.Read();
        }

        static void ServicoAcompanhamentoOrdens_MensagemDeAcompanhamento(string pMensagem)
        {
            Console.WriteLine(pMensagem);
        }
    }
}
