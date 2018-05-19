using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Timers;

using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Gradual.OMS.RoteadorOrdens.Mock
{
    [ServiceBehavior(InstanceContextMode= InstanceContextMode.Single)]
    public class ServicoRoteadorOrdens : IRoteadorOrdens
    {
        #region Globais

        private List<IRoteadorOrdensCallback> gListaDeCallBacks;

        private Timer gTimerDeAlteracaoDeOrdens;

        private static Queue<OrdemInfo> gAcompanhamentosParaEnviar;

        private List<string> gInstrumentos;

        private int gRefazerAListaQuantasVezes = 1;

        #endregion

        #region Construtor

        public ServicoRoteadorOrdens()
        {
            gAcompanhamentosParaEnviar = new Queue<OrdemInfo>();

            gInstrumentos = new List<string>();
            
            gInstrumentos.Add("PETR4");
            gInstrumentos.Add("VALE5");
            gInstrumentos.Add("TELB4");
            gInstrumentos.Add("TOYB3");
            gInstrumentos.Add("TENE5");
            gInstrumentos.Add("TIBR6");
            gInstrumentos.Add("BEEF3");
            gInstrumentos.Add("MMX10");
            gInstrumentos.Add("AMBV3");
            gInstrumentos.Add("BBDC3");
        }

        #endregion

        #region Métodos Private

        private OrdemInfo GerarAcompanhamentoAleatorio(ref Random pRand)
        {
            OrdemInfo lRetornoOrdem;

            AcompanhamentoOrdemInfo lRetorno = new AcompanhamentoOrdemInfo();

            lRetorno.CanalNegociacao = 1;

            lRetorno.DataAtualizacao = DateTime.Now;

            lRetorno.DataOrdemEnvio = DateTime.Now.AddSeconds(pRand.Next(-30, -1));

            lRetorno.DataValidade = DateTime.Now.AddDays(1);

            lRetorno.Direcao = (pRand.Next(0, 1) == 0) ? 
                                  OrdemDirecaoEnum.Compra
                                : OrdemDirecaoEnum.Venda;

            lRetorno.Instrumento = gInstrumentos[pRand.Next(0, 9)];

            lRetorno.NumeroControleOrdem = string.Format("{0}", gAcompanhamentosParaEnviar.Count + 1);

            lRetorno.CodigoResposta = string.Format("r-{0}", lRetorno.NumeroControleOrdem);

            lRetorno.CodigoTransacao = string.Format("t-{0}", lRetorno.NumeroControleOrdem);

            lRetorno.Preco = (decimal)pRand.Next(100, 500) * (decimal)pRand.NextDouble();

            lRetorno.QuantidadeSolicitada = pRand.Next(1, 50) * 100;

            lRetorno.QuantidadeExecutada = pRand.Next(20, 50) * 100;

            if (lRetorno.QuantidadeExecutada > lRetorno.QuantidadeSolicitada) lRetorno.QuantidadeExecutada = lRetorno.QuantidadeSolicitada;

            if (lRetorno.QuantidadeExecutada == lRetorno.QuantidadeSolicitada)
            {
                lRetorno.StatusOrdem = OrdemStatusEnum.EXECUTADA;
            }
            else
            {
                lRetorno.StatusOrdem = OrdemStatusEnum.PARCIALMENTEEXECUTADA;
            }

            //lRetornoOrdem = new OrdemInfo(lRetorno);

            lRetornoOrdem = new OrdemInfo();

            lRetornoOrdem.Acompanhamentos.Add(lRetorno);

            return lRetornoOrdem;
        }

        private void IniciarSimulacao()
        {
            if (gListaDeCallBacks == null)
            {
                gListaDeCallBacks = new List<IRoteadorOrdensCallback>();

                gTimerDeAlteracaoDeOrdens = new Timer(1000);

                gTimerDeAlteracaoDeOrdens.Elapsed += new ElapsedEventHandler(gTimerDeAlteracaoDeOrdens_Elapsed);

                gTimerDeAlteracaoDeOrdens.Start();
            }
        }

        /// <summary>
        /// Preenche a lista de acompanhamentos para enviar com dados mais estruturados do que randomicamente
        /// </summary>
        private void PreencherListaComAcompanhamentosOrganizados()
        {
            AcompanhamentoOrdemInfo lInfo1, lInfo2, lInfo3, lInfo4, lInfo1a, lInfo2a, lInfo3a, lInfo4a;

            OrdemInfo lDummyOrdem;

            lInfo1 = new AcompanhamentoOrdemInfo()
            {
                  CanalNegociacao = 1
                , CodigoDoCliente = 1111111111
                , NumeroControleOrdem = "1111111111010120101234"
                , CodigoResposta = "r-1111111111010120101234"
                , CodigoTransacao = "t1-1111111111010120101234"
                , Instrumento = "PETR4"
                , Preco = 28.8M
                , Direcao = OrdemDirecaoEnum.Compra
                , QuantidadeSolicitada = 1000
                , DataOrdemEnvio = DateTime.Now.AddSeconds(-1)
                , DataAtualizacao = DateTime.Now
                , DataValidade = DateTime.Now.AddDays(1)
                , StatusOrdem = OrdemStatusEnum.NOVA
            };

            //lDummyOrdem = new OrdemInfo(lInfo1);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo1);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);
            
            lInfo2 = new AcompanhamentoOrdemInfo()
            {
                  CanalNegociacao = 1
                , CodigoDoCliente = Convert.ToInt32(2222222222)
                , NumeroControleOrdem = "2222222222010120102222"
                , CodigoResposta = "r-2222222222010120102222"
                , CodigoTransacao = "t1-2222222222010120102222"
                , Instrumento = "VALE5"
                , Preco = 45.5M
                , Direcao = OrdemDirecaoEnum.Compra
                , QuantidadeSolicitada = 300
                , DataOrdemEnvio = DateTime.Now.AddSeconds(-1)
                , DataAtualizacao = DateTime.Now
                , DataValidade = DateTime.Now.AddDays(1)
                , StatusOrdem = OrdemStatusEnum.NOVA
            };
            
            //lDummyOrdem = new OrdemInfo(lInfo2);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo2);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);
            
            lInfo3 = new AcompanhamentoOrdemInfo()
            {
                  CanalNegociacao = 1
                , CodigoDoCliente = Convert.ToInt32(3333333333)
                , NumeroControleOrdem = "333333333301012013333"
                , CodigoResposta = "r-333333333301012013333"
                , CodigoTransacao = "t1-333333333301012013333"
                , Instrumento = "VALE5"
                , Preco = 43.0M
                , Direcao = OrdemDirecaoEnum.Venda
                , QuantidadeSolicitada = 2000
                , DataOrdemEnvio = DateTime.Now.AddSeconds(-1)
                , DataAtualizacao = DateTime.Now
                , DataValidade = DateTime.Now.AddDays(1)
                , StatusOrdem = OrdemStatusEnum.NOVA
            };
            
            //lDummyOrdem = new OrdemInfo(lInfo3);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo3);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);

            lInfo4 = new AcompanhamentoOrdemInfo()
            {
                  CanalNegociacao = 1
                , CodigoDoCliente = Convert.ToInt32(1111111111)
                , NumeroControleOrdem = "1111111111010120104444"
                , CodigoResposta = "r-1111111111010120104444"
                , CodigoTransacao = "t1-1111111111010120104444"
                , Instrumento = "VALE5"
                , Preco = 30.0M
                , Direcao = OrdemDirecaoEnum.Compra
                , QuantidadeSolicitada = 600
                , DataOrdemEnvio = DateTime.Now.AddSeconds(-1)
                , DataAtualizacao = DateTime.Now
                , DataValidade = DateTime.Now.AddDays(1)
                , StatusOrdem = OrdemStatusEnum.NOVA
            };
            
            //lDummyOrdem = new OrdemInfo(lInfo4);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo4);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);

            lInfo1a = new AcompanhamentoOrdemInfo(lInfo1);
            
            lInfo1a.QuantidadeExecutada = 1000;
            lInfo1a.CodigoTransacao = "t2-1111111111010120101234";
            lInfo1a.StatusOrdem = OrdemStatusEnum.EXECUTADA;
            
            //lDummyOrdem = new OrdemInfo(lInfo1a);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo1a);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);
            
            lInfo2a = new AcompanhamentoOrdemInfo(lInfo2);

            lInfo2a.CodigoTransacao = "t2-2222222222010120102222";
            lInfo2a.StatusOrdem = OrdemStatusEnum.REJEITADA;
            
            //lDummyOrdem = new OrdemInfo(lInfo2a);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo2a);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);
            
            lInfo3a = new AcompanhamentoOrdemInfo(lInfo3);

            lInfo3a.CodigoTransacao = "t2-333333333301012013333";
            lInfo3a.QuantidadeExecutada = 1000;
            lInfo3a.StatusOrdem = OrdemStatusEnum.PARCIALMENTEEXECUTADA;
            
            //lDummyOrdem = new OrdemInfo(lInfo3a);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo3a);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);
            
            lInfo3a = new AcompanhamentoOrdemInfo(lInfo3a);
            
            lInfo3a.CodigoTransacao = "t3-333333333301012013333";
            lInfo3a.QuantidadeExecutada = 500;
            lInfo3a.StatusOrdem = OrdemStatusEnum.PARCIALMENTEEXECUTADA;
            
            //lDummyOrdem = new OrdemInfo(lInfo3a);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo3a);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);
            
            lInfo3a = new AcompanhamentoOrdemInfo(lInfo3a);
            
            lInfo3a.CodigoTransacao = "t4-333333333301012013333";
            lInfo3a.QuantidadeExecutada = 500;
            lInfo3a.StatusOrdem = OrdemStatusEnum.EXECUTADA;
            
            //lDummyOrdem = new OrdemInfo(lInfo3a);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo3a);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);
            
            lInfo4a = new AcompanhamentoOrdemInfo(lInfo4);
            
            lInfo4a.CodigoTransacao = "t2-1111111111010120104444";
            lInfo4a.QuantidadeExecutada = 600;
            lInfo4a.StatusOrdem = OrdemStatusEnum.EXECUTADA;
            
            //lDummyOrdem = new OrdemInfo(lInfo4a);
            lDummyOrdem = new OrdemInfo();
            lDummyOrdem.Acompanhamentos.Add(lInfo4a);

            gAcompanhamentosParaEnviar.Enqueue(lDummyOrdem);

        }

        #endregion

        #region Métodos Públicos

        public static void AdicionarAcompanhamentoDeOrdem(string pIdDaOrdem, int pContaDoUsuario, string pPapel, decimal pPreco, int pQuantidade)
        {
            OrdemInfo lInfo;
            
            AcompanhamentoOrdemInfo lAcompanhamento = new AcompanhamentoOrdemInfo();

            lAcompanhamento.NumeroControleOrdem = pIdDaOrdem;
            lAcompanhamento.CodigoDoCliente = pContaDoUsuario;
            lAcompanhamento.Instrumento = pPapel;
            lAcompanhamento.Preco = pPreco;
            lAcompanhamento.QuantidadeExecutada = pQuantidade;
            lAcompanhamento.StatusOrdem = OrdemStatusEnum.EXECUTADA;

            lAcompanhamento.CodigoTransacao = "tX-" + pIdDaOrdem;

            //lInfo = new OrdemInfo(lAcompanhamento);
            lInfo = new OrdemInfo();

            lInfo.Acompanhamentos.Add(lAcompanhamento);

            gAcompanhamentosParaEnviar.Enqueue(lInfo);

            Console.WriteLine("Ordem em fila para envio");
        }

        #endregion

        #region IServicoRoteadorOrdens Members
        /*
        public AssinarExecucaoOrdemRequest AssinarExecucaoOrdem(AssinarExecucaoOrdemRequest pRequest)
        {
            //guarda o contexto dentro da lista de contextos, e depois vai gerando ordens e soltando callbacks

            IniciarSimulacao();

            IOrdemAlteradaCallback lCallBack = OperationContext.Current.GetCallbackChannel<IOrdemAlteradaCallback>();

            if (!gListaDeCallBacks.Contains(lCallBack))
                gListaDeCallBacks.Add(lCallBack);

            return new AbrirConexaoResponse();
        }
        */
        #endregion

        #region Event Handlers

        private void gTimerDeAlteracaoDeOrdens_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (gAcompanhamentosParaEnviar.Count == 0 && gRefazerAListaQuantasVezes > 0)
            {
                /*
                Random lRand = new Random();

                for (int i = 0; i < 100; i++)
                {
                    gAcompanhamentosParaEnviar.Enqueue(GerarAcompanhamentoAleatorio(ref lRand));
                }*/

                gRefazerAListaQuantasVezes--;

                //PreencherListaComAcompanhamentosOrganizados();
            }

            if (gAcompanhamentosParaEnviar.Count > 0)
            {
                foreach (IRoteadorOrdensCallback lCallBack in gListaDeCallBacks)
                {
                    lCallBack.OrdemAlterada(gAcompanhamentosParaEnviar.Dequeue());
                }
            }
        }

        #endregion

        #region IRoteadorOrdens Members

        public ExecutarCancelamentoOrdemResponse CancelarOrdem(ExecutarCancelamentoOrdemRequest request)
        {
            throw new NotImplementedException();
        }

        public ExecutarOrdemResponse ExecutarOrdem(ExecutarOrdemRequest request)
        {
            throw new NotImplementedException();
        }

        public ExecutarModificacaoOrdensResponse ModificarOrdem(ExecutarModificacaoOrdensRequest request)
        {
            throw new NotImplementedException();
        }

        public ExecutarOrdemCrossResponse ExecutarOrdemCross(ExecutarOrdemCrossRequest request)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region IRoteadorOrdens Members


        public PingResponse Ping(PingRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
