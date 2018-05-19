using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Recebidas;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto;
using Gradual.OMS.Comunicacao.Automacao.Ordens;



namespace StopStart
{
    class Program
    {

        //#region ["Declarações"]

        //static MDSEventFactory EventMds = new MDSEventFactory();
        //static List<string> Eventos = new List<string>();
        //static NOrdem _Ordens = new NOrdem();

        //#endregion

        //#region ["Enumeração"]

        //private enum RespostaOrdem : int{
        //    Rejeitado = 0,
        //    Aceito = 1, 
        //};

        //public enum OrdemStopStatus : int
        //{
        //    Registrado = 1,
        //    Aceito = 2,
        //    Rejeitado = 3,
        //    Enviado = 4,
        //    Executado = 5,
        //    Cancelado = 6
        //};

        //#endregion

        //#region ["Eventos"]

        //private static void Initialize()
        //{

        //    Event._MDSAuthenticationResponse +=
        //        new Event._onMDSAuthenticationResponse(Event__MDSAuthenticationResponse);

        //    OMSEventHandlerClass omsEHC =
        //        new OMSEventHandlerClass(EventMds);

        //    EventMds.OnMDSStopStartEvent +=
        //        new MDSEventFactory._OnMDSStopStartEvent(EventMds_OnMDSStopStartEvent);

        //    EventMds.OnMDSSRespostaAutenticacaoEvent
        //        += new MDSEventFactory._OnMDSSRespostaAutenticacaoEvent(EventMds_OnMDSSRespostaAutenticacaoEvent);

        //    Registrador.AddListener(EventMds);

        //}   
 

        //static void EventMds_OnMDSSRespostaAutenticacaoEvent(object sender, MDSEventArgs e)
        //{
        //    try{
        //        RS_RespostaStop _RS_RespostaStop
        //            = (RS_RespostaStop)(sender);

        //        int id_stopstart = int.Parse(_RS_RespostaStop.pStrIdStopstart);
        //        int id_status    = int.Parse(_RS_RespostaStop.pStrStatus);                
                
        //        if (id_status == (int)RespostaOrdem.Rejeitado){                    

        //            new DOrdem().AtualizaOrdemStop(
        //              id_stopstart,
        //              (int)OrdemStopStatus.Rejeitado
        //              );

        //            Console.WriteLine("Stop rejeitado pelo MDS");
        //        }
        //        else if (id_status == (int)RespostaOrdem.Aceito){
                    
        //            new DOrdem().AtualizaOrdemStop(
        //              id_stopstart,
        //              (int)OrdemStopStatus.Aceito
        //              );

        //            Console.WriteLine("Stop aceito pelo MDS");
        //        } 
                
        //    }
        //    catch (Exception ex){
        //        throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao acessar o método: EventMds_OnMDSSRespostaAutenticacaoEvent: ", ex.Message));
        //    }

        //}

        //static void EventMds_OnMDSStopStartEvent(object sender, MDSEventArgs e)
        //{
        //     try{

        //        ThreadPool.QueueUserWorkItem(new WaitCallback(

        //                                delegate(object required)
        //                                {
        //                                    ProcessarEventoMDS(
        //                                        (object)(sender)
        //                                        );
        //                                }));
        //    }
        //    catch (Exception ex){
        //        throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao acessar o método: EventMds_OnMDSBussinesReceveive:", ex.Message));
        //    }
        //}

        //static void Event__MDSAuthenticationResponse(object Response, System.Net.Sockets.Socket _ClientSocket)
        //{
        //    try
        //    {
        //        if (Response.ToString().Trim() != string.Empty)
        //        {
        //            switch (int.Parse(Response.ToString().Trim()))
        //            {
        //                case 0:
        //                    Console.WriteLine("Usuário Logado no sistema");
        //                    break;
        //                case 1:
        //                    Console.WriteLine("Login efetuado com sucesso.");
        //                    Contexto.SocketPrincipal = _ClientSocket;
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //#endregion

        //#region ["Funções"]

        //static void ProcessarEventoMDS(object sender){
        //    try{
        //          SS_StopSimplesResposta _SS_StopSimplesResposta = 
        //              (SS_StopSimplesResposta)(sender);

        //          //Formatador.CancelarOrdemStop(
        //          //    _SS_StopSimplesResposta.pStrInstrument.ToString().Trim(),
        //          //    int.Parse(_SS_StopSimplesResposta.pStrIdStopStart)
        //          //    );
        
        //        new DOrdem().AtualizaOrdemStop(
        //            int.Parse(_SS_StopSimplesResposta.pStrIdStopStart),
        //            (int)OrdemStopStatus.Executado,
        //           decimal.Parse(_SS_StopSimplesResposta.pStrPrecoReferencia)
        //            );

        //        Console.WriteLine("Stop Executado.");
                
        //    }
        //    catch (Exception ex){
        //        throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao acessar o método ProcessarEventoMDS:", ex.Message));
        //    }
                 
        //}

        //static void EnviarMensagemAutenticacao()
        //{

        //    A1_SignIn A1 = new A1_SignIn("BV");

        //    A1.idCliente = "00007";
        //    A1.idSistema = "3";

        //    try
        //    {

        //        ASSocketConnection _Client =
        //        new ASSocketConnection();

        //        _Client.ASSocketOpen();
        //        _Client.SendData(A1.getMessageA1());
        //        _Client = null;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //}

        //private static void CancelarOrdemStop()
        //{
        //    _Ordens.CancelaOrdemStopStart(40, (int)OrdemStopStatus.Cancelado);
        //    Console.WriteLine("Ordem cancelada com sucesso !");
        //}

        //private static void ArmarStopLoss(int idCliente , string Instrumento , decimal preco)
        //{
        //    try
        //    {
        //        TOrdem _TOrder = new TOrdem();

        //        _TOrder.id_bolsa = 1;
        //        _TOrder.id_cliente = idCliente;
        //        _TOrder.id_mercado_tipo = 1;
        //        _TOrder.id_stopstart_status = (int)OrdemStopStatus.Registrado;
        //        _TOrder.id_stopstart_tipo = 1;
        //        _TOrder.instrumento = Instrumento;
        //        _TOrder.data_validade = DateTime.Now;
        //        _TOrder.preco_envio_gain = null;
        //        _TOrder.preco_gain = null;
        //        _TOrder.preco_loss = preco;
        //        _TOrder.preco_envio_loss = 35.60M;
        //        _TOrder.quantidade = 100;
        //        _TOrder.valor_ajuste_movel = null;
        //        _TOrder.valor_inicio_movel = null;

        //        _TOrder.id_stopstart = _Ordens.ArmarStopLoss(_TOrder);
  
        //        Registrador.AddEvent(EventMds, Eventos);

        //        if (Contexto.SocketPrincipal != null){
        //            Formatador.ArmarStopSimples(_TOrder);
        //            Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _TOrder.instrumento, " - " + _TOrder.id_stopstart.ToString(), " - " + _TOrder.preco_loss.ToString()));
        //        }
        //        else{
        //            Console.WriteLine("Cliente não esta conectado.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao armar o StopLoss: ", ex.Message));
        //    }

        //}

        //#endregion

        static void Main(string[] args)
        {         
            //Initialize();
            //EnviarMensagemAutenticacao();

            Thread.Sleep(500);

            //new ThreadStart(DispararOrdem).Invoke();
      

            Console.Read();
        }

        //private static void DispararOrdem()
        //{

        //    while (true)
        //    {
        //        Console.WriteLine("Informe o codigo do cliente");
        //        string codigoCliente = Console.ReadLine();

        //        Console.WriteLine("Informe o papel");
        //        string Papel = Console.ReadLine();

        //        Console.WriteLine("Informe o preco");
        //        string Preco = Console.ReadLine();

        //        ArmarStopLoss(int.Parse(codigoCliente), Papel, decimal.Parse(Preco));
        //    }
        //}

    }
}
