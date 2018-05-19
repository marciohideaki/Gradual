using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System.Threading;
using Gradual.Core.Ordens.Lib;
using Gradual.Core.Ordens.Persistencia;
using Gradual.Core.Ordens.Lib.Dados.Enum;
using Gradual.Core.OMS.LimiteBMF.Lib;
using Gradual.Core.OMS.LimiteBMF;
using System.Collections;
using System.Configuration;

namespace Gradual.Core.Ordens.Callback
{
    public class OrderCallback: IRoteadorOrdensCallback
    {
        IAssinaturasRoteadorOrdensCallback roteador;
        private bool _bKeepRunning = true;
        private bool bRemoveDigito = true;

        public OrderCallback()
        {
            if (ConfigurationManager.AppSettings["CodCorretora"] != null)
            {
                CORRETORA = ConfigurationManager.AppSettings["CodCorretora"].ToString();
            }

            if (ConfigurationManager.AppSettings["AccountStripDigit"] != null)
            {
                bRemoveDigito = ConfigurationManager.AppSettings["AccountStripDigit"].ToString().ToLower().Equals("true");

                logger.WarnFormat("REMOCAO DO DIGITO BOVESPA ESTA {0}", bRemoveDigito ? "HABILITADO" : "DESABILITADO");
            }

            roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);
        }

        #region IRoteadorOrdensCallback Members

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constantes

        private const string EXCHANGEBOVESPA = "BOVESPA";
        private const string EXCHANGEBMF = "BMF";

        #endregion

        #region Propriedades


        #endregion

        private Dictionary<string, bool> dctStatusBolsa = new Dictionary<string, bool>();
        private string CORRETORA = "227";

        public bool CanaisBolsaAtivos()
        {
            foreach (bool status in dctStatusBolsa.Values)
            {
                if (status == false)
                    return false;
            }

            return true;
        }

        public void OrdemAlterada(OrdemInfo report)
        {
            try
            {

                // - COLOCAR EM ARQUIVO DE CONFIG [ HARD CODED -> TESTES ]
                if ((report.ChannelID == 362) || (report.ChannelID == 0) || (report.ChannelID == 317) || (report.ChannelID == 370) || (report.ChannelID == 806) || (report.ChannelID == 371)){             
               //  if (report.ChannelID == 0){
                    if ((report.OrdStatus != OrdemStatusEnum.REJEITADA) &&
                        (report.OrdStatus != OrdemStatusEnum.EXPIRADA) &&
                        (report.OrdStatus != OrdemStatusEnum.SUSPENSA) &&
                        (report.OrdStatus != OrdemStatusEnum.ENVIADAPARAOCANAL) &&
                        (report.OrdStatus != OrdemStatusEnum.ENVIADAPARAABOLSA) &&
                        (report.OrdStatus != OrdemStatusEnum.ENVIADAPARAOROTEADORDEORDENS))
                    {


                        logger.Info("CALLBACK INICIALIZADO COM SUCESSO.");
                        logger.Info("INVOCA THREAD TratarNotificacaoExecucao");

                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(
                                TratarNotificacaoExecucao), report);


                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao receber o callback do acompanhamento de ordens...", ex);
                logger.Error("Descrição do erro..." + ex.Message);
            }
        }

        public void StatusConexaoAlterada(StatusConexaoBolsaInfo status)
        {
            string key = status.Bolsa + status.Operador;

            if (dctStatusBolsa.ContainsKey(key))
                dctStatusBolsa[key] = status.Conectado;
            else
                dctStatusBolsa.Add(key, status.Conectado);

            if (status.Conectado == true)
            {                       
                logger.Debug("StatusConexao...: True");
                logger.Debug("status.Bolsa...: " + status.Bolsa.ToString());
                logger.Debug("status.Porta...: " + status.Operador.ToString());
            }
            else
            {
                if (roteador == null)
                {
                    roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);                    
                }

                logger.Debug("StatusConexao...: False");
                logger.Debug("status.Bolsa...: " + status.Bolsa.ToString());
                logger.Debug("status.Porta...: " + status.Operador.ToString());
            }
        }


        private void TratarNotificacaoExecucao(object _report)
        {

            bool IsContaMaster;
            int CodigoContaMae;

            #region  TRATAMENTO DE CODIGO DE BOLSA

            OrdemInfo OrderInfo = (OrdemInfo)(_report);

            int Account = OrderInfo.Account;

            if ( bRemoveDigito )
                Account = OrderInfo.Account.ToString().Remove(OrderInfo.Account.ToString().Length - 1, 1).DBToInt32();

            #region Validacao de Cliente Conta Master

            logger.Info("VERIFICA SE O CLIENTE POSSUI CONTA MASTER ATRELADA AO CODIGO DE BOLSA");
            Dictionary<int, int> RelacaoContaMaster = new PersistenciaOrdens().ObterDadosContaMaster(Account);

            if (RelacaoContaMaster.Count > 0)
            {
                IsContaMaster = true;
                Account = RelacaoContaMaster[Account];

                logger.Info("CLIENTE POSSUI CONTA MASTER VINCULADA AO CODIGO DE BOLSA. CODIGO DA CONTA MASTER: " + Account.ToString());
            }
            else
            {
                IsContaMaster = false;
                logger.Info("CLIENTE NÃO POSSUI CONTA MASTER VINCULADA AO CODIGO DE BOLSA.");
            }


            #endregion

            #endregion

            #region CADASTRO DE PAPEIS

            //Obtem informacoes referentes ao cadastro de papel do instrumento selecionado
            EnviarCadastroPapelRequest ObjetoCadastroPapelRequest = new EnviarCadastroPapelRequest();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();

            string SYMBOL = OrderInfo.Symbol;

            if (SYMBOL.Substring(SYMBOL.Length - 1, 1) == "F")
            {
                OrderInfo.Symbol = SYMBOL.Remove(SYMBOL.Length - 1);
            }

            ObjetoCadastroPapelRequest.Instrumento = OrderInfo.Symbol.Trim();
            EnviarCadastroPapelResponse CadastroPapelResponse = ObjetoPersistenciaOrdens.ObterCadastroPapel(ObjetoCadastroPapelRequest);

            #endregion



            // SESSAO RESPONSAVEL POR REALOCAR OS LIMITES NA CONTA DO CLIENTE
            if ((OrderInfo.OrdStatus == OrdemStatusEnum.NOVA) || (OrderInfo.OrdStatus == OrdemStatusEnum.PARCIALMENTEEXECUTADA) || (OrderInfo.OrdStatus == OrdemStatusEnum.SUBSTITUICAOSOLICITADA) || (OrderInfo.OrdStatus == OrdemStatusEnum.SUBSTITUIDA))
            {
                this.CalcularLimite(Account, OrderInfo, CadastroPapelResponse.CadastroPapelInfo.SegmentoMercado);
            }

            // CANCELAMENTO DE ORDENS
            if (OrderInfo.OrdStatus == OrdemStatusEnum.CANCELADA)
            {
                this.CalcularLimiteCancelamento(Account, OrderInfo, CadastroPapelResponse.CadastroPapelInfo.SegmentoMercado);
            }


        }

        /// <summary>
        /// METODO RESPONSAVEL POR DEVOLVER O VOLUME ALOCADO DE UMA OFERTA QUE FOI CANCELADA.
        /// </summary>
        /// <param name="Account">CODIGO DO CLIENTE</param>
        /// <param name="OrderInfo">ORDEMINFO</param>
        /// <param name="SegmentoMercado"></param>
        private void CalcularLimiteCancelamento(int Account, OrdemInfo OrderInfo, SegmentoMercadoEnum SegmentoMercado)
        {
            Thread.Sleep(1000);

            #region CADASTRO DE PAPEIS

            //Obtem informacoes referentes ao cadastro de papel do instrumento selecionado
            EnviarCadastroPapelRequest ObjetoCadastroPapelRequest = new EnviarCadastroPapelRequest();
            EnviarCadastroPapelResponse ObjetoCadastroPapelResponse = new EnviarCadastroPapelResponse();
            PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();

            string SYMBOL = OrderInfo.Symbol;

            if (SYMBOL.Substring(SYMBOL.Length - 1, 1) == "F")
            {
                OrderInfo.Symbol = SYMBOL.Remove(SYMBOL.Length - 1);
            }

            ObjetoCadastroPapelRequest.Instrumento = OrderInfo.Symbol.Trim();
            ObjetoCadastroPapelResponse = PersistenciaOrdens.ObterCadastroPapel(ObjetoCadastroPapelRequest);

            #endregion

            #region VOLUME DO CANCELAMENTO DA ORDEM

            decimal PRICE = 0;

            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
            CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

            PRICE = CotacaoResponse.CotacaoInfo.Ultima - (CotacaoResponse.CotacaoInfo.Ultima * (10 / 100));

            decimal VOLUMEORDEM = (OrderInfo.OrderQty * PRICE);

            #endregion

            #region DECLARACOES

            int CODIGOPARAMETROCLIENTECOMPRA = 0;
            int CODIGOPARAMETROCLIENTEVENDA = 0;

            decimal LIMITEALOCADOCOMPRA = 0;
            decimal LIMITEALOCADOVENDA = 0;

            LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();
            LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();

            #endregion

            string idCliente = OrderInfo.Account.ToString();

            if ( bRemoveDigito )
                idCliente = OrderInfo.Account.ToString().Remove(OrderInfo.Account.ToString().Count() - 1, 1);

            int CODIGOCLIENTE = int.Parse(idCliente);

            switch (ObjetoCadastroPapelResponse.CadastroPapelInfo.SegmentoMercado)
            {
                case SegmentoMercadoEnum.AVISTA:

                    #region RECALCULO DE LIMITE NO MERCADO A VISTA

                    LimiteOperacionalRequest.CodigoCliente = CODIGOCLIENTE;

                    ObjetoLimitesOperacionais = PersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                    #region LIMITE DE COMPRA

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                            
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                            LIMITEALOCADOCOMPRA = ITEM.ValorAlocado;
                        }
                    }

                    #endregion

                    #region LIMITE VENDA


                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {
                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                            LIMITEALOCADOVENDA = ITEM.ValorAlocado;
                        }
                    }

                    #endregion

                    if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                    {
                        //if (LIMITEALOCADOCOMPRA >= VOLUMEORDEM)
                        //{

                            //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");
                        //}

                    }
                    else if (OrderInfo.Side == OrdemDirecaoEnum.Venda)
                    {
                        //if (LIMITEALOCADOVENDA >= VOLUMEORDEM)
                        //{LLLLL
                            //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);

                            logger.Info("Atualiza os limites de venda a vista");
                        //}
                    }

                    #endregion

                    break;
                case SegmentoMercadoEnum.FRACIONARIO:

                    #region  RECALCULO DE LIMITE NO MERCADO FRACIONARIO

                    LimiteOperacionalRequest.CodigoCliente = CODIGOCLIENTE;
                    ObjetoLimitesOperacionais = PersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                    #region LIMITE DE COMPRA

                    var LIMITECOMPRAFRACIONARIO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                                  where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                                  select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAFRACIONARIO.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAFRACIONARIO)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                            
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                            LIMITEALOCADOCOMPRA = ITEM.ValorAlocado;
                        }
                    }

                    #endregion

                    #region LIMITE VENDA


                    var LIMITEVENDAFRACIONARIO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                                 where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                                 select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAFRACIONARIO.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAFRACIONARIO)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                            LIMITEALOCADOVENDA = ITEM.ValorAlocado;
                        }
                    }

                    #endregion

                    if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                    {
                        //if (LIMITEALOCADOCOMPRA >= VOLUMEORDEM)
                        //{
                            //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");
                        //}

                    }
                    else if (OrderInfo.Side == OrdemDirecaoEnum.Venda)
                    {
                        //if (LIMITEALOCADOVENDA >= VOLUMEORDEM)
                        //{
                            //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                        //}
                    }

                    #endregion

                    break;
                case SegmentoMercadoEnum.OPCAO:

                    #region RECALCULO DE LIMITE NO MERCAO DE OPCOES

                    LimiteOperacionalRequest.CodigoCliente = CODIGOCLIENTE;
                    ObjetoLimitesOperacionais = PersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                    #region LIMITE DE COMPRA

                    var LIMITECOMPRAOPCAO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.COMPRAOPCOES
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAOPCAO.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAOPCAO)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                            
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                            LIMITEALOCADOCOMPRA = ITEM.ValorAlocado;
                        }
                    }

                    #endregion

                    #region LIMITE VENDA


                    var LIMITEVENDAOPCAO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                           where p.TipoLimite == TipoLimiteEnum.VENDAOPCOES
                                           select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAOPCAO.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAOPCAO)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                            LIMITEALOCADOVENDA = ITEM.ValorAlocado;
                        }
                    }

                    #endregion

                    if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                    {
                        //if (LIMITEALOCADOCOMPRA >= VOLUMEORDEM)
                        //{

                            //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");
                        //}

                    }
                    else if (OrderInfo.Side == OrdemDirecaoEnum.Venda)
                    {
                        //if (LIMITEALOCADOVENDA >= VOLUMEORDEM)
                        //{
                            //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");
                        //}

                    }

                    #endregion

                    break;

                case SegmentoMercadoEnum.FUTURO:

                    int QuantidadeOrdem = (OrderInfo.OrderQty - OrderInfo.CumQty);

                    AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                    AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                    int QuantidadeContrato = 0;
                    int QuantidadeInstrumento = 0;
                    int idClienteParametroBMF = 0;
                    char stUtilizacaoInstrumento = 'N';

                    ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();

                   // int accountRef = int.Parse(OrderInfo.Account.ToString().Remove(OrderInfo.Account.ToString().Length - 1));

                    requestListaLimite.Account = OrderInfo.Account;

                    ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                    string Instrumento = OrderInfo.Symbol;
                    string Contrato = OrderInfo.Symbol.Substring(0, 3);
                    string Sentido;

                    if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                    {
                        Sentido = "C";
                    }
                    else
                    {
                        Sentido = "V";
                    }

                    //CONTRATO BASE
                    var PosicaoContrato = from p in response.ListaLimites
                                          where p.Contrato == Contrato
                                          && p.Sentido == Sentido
                                          select p;

                    if (PosicaoContrato.Count() > 0)
                    {
                        foreach (var item in PosicaoContrato)
                        {
                            idClienteParametroBMF = item.idClienteParametroBMF;
                            QuantidadeContrato = item.QuantidadeDisponivel;

                            if (response.ListaLimitesInstrumentos.Count > 0)
                            {
                                // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                foreach (var vencimento in response.ListaLimitesInstrumentos)
                                {
                                    if (vencimento.Instrumento == Instrumento)
                                    {
                                        if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                        {
                                            QuantidadeInstrumento += vencimento.QtDisponivel;
                                            Instrumento = vencimento.Instrumento;
                                            stUtilizacaoInstrumento = 'S';
                                        }
                                    }
                                }
                            }
                        }

                        int QuantidadeSolicitada = OrderInfo.OrderQty;

                        requestBMF.account = requestListaLimite.Account;
                        requestBMF.idClienteParametroBMF = idClienteParametroBMF;
                        requestBMF.instrumento = Instrumento;
                        requestBMF.quantidadeSolicitada = QuantidadeOrdem * -1;
                        requestBMF.stUtilizaInstrumento = stUtilizacaoInstrumento;

                        responseBMF = new PersistenciaOrdens().AtualizaPosicaoLimiteBMF(requestBMF);

                    }
                    else
                    {
                        logger.Info("Não existe limite cadastrado");
                    }



                    break;

            }
        }

       

        /// <summary>
        /// METODO RESPONSAVEL POR RECALCULAR A POSICAO DE LIMIET DOS CLIENTES.
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="OrderInfo"></param>
        /// <param name="SegmentoMercado"></param>
        private void CalcularLimite(int Account, OrdemInfo OrderInfo, SegmentoMercadoEnum SegmentoMercado)
        {
            Thread.Sleep(500);

            #region DECLARACOES

            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();
            LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();

            decimal LIMITEOPERACIONAL = 0;
            decimal VOLUMEORDEM = 0;

            int CODIGOPARAMETROCLIENTECOMPRAOPCAO = 0;
            int CODIGOPARAMETROCLIENTECOMPRAAVISTA = 0;
            int CODIGOPARAMETROCLIENTEVENDAAVISTA = 0;
            int CODIGOPARAMETROCLIENTEVENDAOPCAO = 0;

            #endregion

            #region LIMITES OPERACIONAIS

            //**************************************************************************************************************
            //     TRECHO RESPONSAVEL POR CARREGAR OS LIMITES OPERACIOANIS DO CLIENTE.
            //**************************************************************************************************************   

            logger.Info("OBTEM INFORMACOES INICIAS DA POSICAO DO CLIENTE MANTIDA EM MEMORIA.");
            LimiteOperacionalRequest.CodigoCliente = Account;
            ObjetoLimitesOperacionais = ObjetoPersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

            #endregion

            logger.Info("*************** TRACE CALLBACK ***************");
            logger.Info("CLIENTE:.................. " + OrderInfo.Account.ToString());
            logger.Info("PAPEL :.................. " + OrderInfo.Symbol.ToString());
            logger.Info("QTDE  :.................. " + OrderInfo.OrderQty.ToString());
            logger.Info("PRECO :.................. " + OrderInfo.Price.ToString());

            switch (SegmentoMercado)
            {                  
                case SegmentoMercadoEnum.AVISTA:
                    logger.Info("RECALCULO DE POSICAO PARA O SEGMENTO A VISTA");

                    logger.Info("OBTEM LIMITES OPERACIONAIS");

                    #region LIMITE OPERACIONAL PARA COMPRA DE ACOES

                    var LIMITECOMPRAACOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAACOES.Count() > 0)
                    {
                        foreach (var ITEM in LIMITECOMPRAACOES)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRAAVISTA = ITEM.CodigoParametroCliente;
                        }
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA DE ACOES

                    var LIMITEVENDAACOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                           where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                           select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAACOES.Count() > 0)
                    {
                        foreach (var ITEM in LIMITEVENDAACOES)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDAAVISTA = ITEM.CodigoParametroCliente;
                        }
                    }

                    #endregion

                    logger.Info("RECALCULA A POSICAO DO CLIENTE");

                    #region NOVA OFERTA

                    if (OrderInfo.OrdStatus == OrdemStatusEnum.NOVA)
                    {
                        #region VALIDACAO DE PRECO POR ORDEM ENVIADA

                        decimal PRECO = decimal.Parse(OrderInfo.Price.ToString());
                        decimal QUANTIDADE = OrderInfo.OrderQty;

                        EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                        CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                        EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;


                        #endregion

                        VOLUMEORDEM = (PRECO * QUANTIDADE);

                        if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                        {
                            #region COMPRA DE ACOES

                            #region ATUALIZAÇÕES DOS LIMITES OPERACIONAIS

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("ATUALIZA LIMITES DE COMPRA NO MERCADO A VISTA");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("ATUALIZA LIMITES DE VENDA NO MERCADO A VISTA");

                            #endregion

                            #endregion
                        }
                        else
                        {
                            #region VENDA DE ACOES

                            #region ATUALIZAÇÕES DOS LIMITES OPERACIONAIS

                            //******************** DEBITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("ATUALIZA LIMITES DE COMPRA NO MERCADO A VISTA");

                            //******************** CREDITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("ATUALIZA LIMITES DE VENDA NO MERCADO A VISTA");

                            #endregion

                            #endregion
                        }
                    }

                    #endregion

                    #region ALTERACAO DE ORDENS

                    if (OrderInfo.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                    {

                        if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                        {
                            #region SUBSTITUICAO DE COMPRA DE ACOES

                            #region TRATAMENTO DA ROTINA DE ALTERACAO DE ORDENS

                            double VOLUMEORIGINAL = 0;
                            double VOLUMEMODIFICACAO = 0;
                            double DIFERENCIAL = 0;

                            double PRECO = 0;

                            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                            CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);
                            PRECO = double.Parse(CotacaoResponse.CotacaoInfo.Ultima.ToString());

                            EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                            InformacoesRequest.NumeroControleOrdem = OrderInfo.OrigClOrdID;

                            EnviarInformacoesOrdemResponse InformacoesOrdem =
                             this.ObterInformacoesOrdemRecalculo(InformacoesRequest);

                            OrderInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                            VOLUMEORIGINAL = (PRECO * InformacoesOrdem.OrdemInfo.OrderQty);
                            VOLUMEMODIFICACAO = (PRECO * OrderInfo.OrderQty);
                            DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                            ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                            ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(OrderInfo);

                            //**************************************************************************************************************
                            // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                            //**************************************************************************************************************                                     

                            #endregion

                            #region ATUALIZACAO DE LIMITES

                            #region DEVOLVE O LIMITE ATUAL

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion

                            #region DEBITA O NOVO LIMITE

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda1 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda2 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion

                            #endregion

                            #endregion

                        }
                        else
                        {
                            #region SUBSTITUICAO DE VENDA DE ACOES

                            #region TRATAMENTO DA ROTINA DE ALTERACAO DE ORDENS

                            double VOLUMEORIGINAL = 0;
                            double VOLUMEMODIFICACAO = 0;
                            double DIFERENCIAL = 0;

                            double PRECO = 0;

                            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                            CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);
                            PRECO = double.Parse(CotacaoResponse.CotacaoInfo.Ultima.ToString());

                            EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                            InformacoesRequest.NumeroControleOrdem = OrderInfo.OrigClOrdID;

                            EnviarInformacoesOrdemResponse InformacoesOrdem =
                           this.ObterInformacoesOrdemRecalculo(InformacoesRequest);

                            OrderInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                            VOLUMEORIGINAL = (PRECO * InformacoesOrdem.OrdemInfo.OrderQty);
                            VOLUMEMODIFICACAO = (PRECO * OrderInfo.OrderQty);
                            DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);


                            ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                            ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(OrderInfo);

                            //**************************************************************************************************************
                            // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                            //**************************************************************************************************************               

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
                                    CODIGOPARAMETROCLIENTEVENDAAVISTA = ITEM.CodigoParametroCliente;
                                }
                            }

                            #endregion

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
                                    LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                                    CODIGOPARAMETROCLIENTECOMPRAAVISTA = ITEM.CodigoParametroCliente;
                                }
                            }


                            #endregion

                            #endregion

                            #region ATUALIZACAO DE LIMITES

                            #region DEVOLVE O LIMITE ATUAL

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion

                            #region DEBITA O NOVO LIMITE

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda1 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda2 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion


                            #endregion

                            #endregion

                        }
                    }

                    #endregion

                    #region OFERTA EXECUTADA

                    if ((OrderInfo.OrdStatus == OrdemStatusEnum.EXECUTADA) || (OrderInfo.OrdStatus == OrdemStatusEnum.PARCIALMENTEEXECUTADA))
                    {
                        #region VALIDACAO DE PRECO POR ORDEM ENVIADA

                        decimal PRECO = decimal.Parse(OrderInfo.Price.ToString());
                        decimal QUANTIDADE = OrderInfo.OrderQty;

                        EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                        CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                        EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;


                        #endregion

                        VOLUMEORDEM = (PRECO * QUANTIDADE);

                        if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                        {
                            #region COMPRA DE ACOES

                            #region ATUALIZAÇÕES DOS LIMITES OPERACIONAIS

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("ATUALIZA LIMITES DE COMPRA NO MERCADO A VISTA");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("ATUALIZA LIMITES DE VENDA NO MERCADO A VISTA");

                            #endregion

                            #endregion
                        }
                        else
                        {
                            #region VENDA DE ACOES

                            #region ATUALIZAÇÕES DOS LIMITES OPERACIONAIS

                            //******************** DEBITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("ATUALIZA LIMITES DE COMPRA NO MERCADO A VISTA");

                            //******************** CREDITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("ATUALIZA LIMITES DE VENDA NO MERCADO A VISTA");

                            #endregion

                            #endregion
                        }
                    }

                    #endregion

                    break;

                case SegmentoMercadoEnum.OPCAO:
                    logger.Info("RECALCULO DE POSICAO PARA O SEGMENTO OPCOES");

                    logger.Info("OBTEM LIMITES OPERACIONAIS");
                    #region LIMITE OPERACIONAL PARA COMPRA DE OPCOES

                    var LIMITECOMPRAOPCOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAOPCOES
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAOPCOES.Count() > 0)
                    {
                        foreach (var ITEM in LIMITECOMPRAOPCOES)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRAOPCAO = ITEM.CodigoParametroCliente;
                        }
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA DE OPCOES

                    var LIMITEVENDAOPCAO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                           where p.TipoLimite == TipoLimiteEnum.VENDAOPCOES
                                           select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAOPCAO.Count() > 0)
                    {
                        foreach (var ITEM in LIMITEVENDAOPCAO)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDAOPCAO = ITEM.CodigoParametroCliente;
                        }
                    }

                    #endregion

                    logger.Info("RECALCULA A POSICAO DO CLIENTE");

                    #region NOVA OFERTA

                    if (OrderInfo.OrdStatus == OrdemStatusEnum.NOVA)
                    {
                        #region VALIDACAO DE PRECO POR ORDEM ENVIADA

                        decimal PRECO = decimal.Parse(OrderInfo.Price.ToString());
                        decimal QUANTIDADE = OrderInfo.OrderQty;

                        EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                        CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                        EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;

                        #endregion

                        VOLUMEORDEM = (PRECO * QUANTIDADE);

                        if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                        {
                            #region COMPRA DE OPCOES

                            #region ATUALIZACAO DE LIMITES OPERACIONAIS

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion

                            #endregion
                        }
                        else
                        {
                            #region VENDA DE OPCOES

                            #region ATUALIZACAO DE LIMITES OPERACIONAIS

                            //******************** DEBITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");


                            #endregion

                            #endregion
                        }
                    }

                    #endregion

                    #region ALTERACAO DE ORDENS

                    if (OrderInfo.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                    {
                        if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                        {
                            #region SUBSTITUICAO DE COMPRA DE OPCOES

                            #region TRATAMENTO DA ROTINA DE ALTERACAO DE ORDENS

                            double VOLUMEORIGINAL = 0;
                            double VOLUMEMODIFICACAO = 0;
                            double DIFERENCIAL = 0;

                            EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();

                            InformacoesRequest.NumeroControleOrdem = OrderInfo.OrigClOrdID;

                            EnviarInformacoesOrdemResponse InformacoesOrdem =
                                this.ObterInformacoesOrdemRecalculo(InformacoesRequest);

                            double PRECO = 0;

                            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                            CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);
                            PRECO = double.Parse(CotacaoResponse.CotacaoInfo.Ultima.ToString());

                            OrderInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                            VOLUMEORIGINAL = (PRECO * InformacoesOrdem.OrdemInfo.OrderQty);
                            VOLUMEMODIFICACAO = (PRECO * OrderInfo.OrderQty);
                            DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                            ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                            ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(OrderInfo);

                            //**************************************************************************************************************
                            // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                            //**************************************************************************************************************               
                            #endregion

                            #region ATUALIZACAO DE LIMITES

                            #region LIBERA O SALDO ALOCADO DA ORDEM ORIGINAL


                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra de opcoes");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda de opcoes");

                            #endregion

                            #region ALOCACAO DE LIMITES
                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/

                            AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponse2 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra de opcoes");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda3 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda de opcoes");

                            #endregion

                            #endregion

                            #endregion
                        }
                        else
                        {
                            #region SUBSTITUICAO DE VENDA DE OPCOES

                            #region TRATAMENTO DA ROTINA DE ALTERACAO DE ORDENS

                            double VOLUMEORIGINAL = 0;
                            double VOLUMEMODIFICACAO = 0;
                            double DIFERENCIAL = 0;

                            EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                            InformacoesRequest.NumeroControleOrdem = OrderInfo.OrigClOrdID;

                            EnviarInformacoesOrdemResponse InformacoesOrdem =
                                     this.ObterInformacoesOrdemRecalculo(InformacoesRequest);

                            double PRECO = 0;

                            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                            CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);
                            PRECO = double.Parse(CotacaoResponse.CotacaoInfo.Ultima.ToString());

                            OrderInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                            VOLUMEORIGINAL = (PRECO * InformacoesOrdem.OrdemInfo.OrderQty);
                            VOLUMEMODIFICACAO = (PRECO * OrderInfo.OrderQty);
                            DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                            ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                            ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(OrderInfo);

                            #endregion

                            #region ATUALIZACAO DE LIMITES

                            #region LIBERA O SALDO ALOCADO DA ORDEM ORIGINAL


                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra de opcoes");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda de opcoes");

                            #endregion

                            #region ALOCACAO DE LIMITES
                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/

                            AtualizarLimitesRequest = new AtualizarLimitesRequest();

                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponse2 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra de opcoes");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAOPCAO;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAOPCOES;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda3 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda de opcoes");

                            #endregion

                            #endregion

                            #endregion
                        }
                    }

                    #endregion

                    break;
                case SegmentoMercadoEnum.FRACIONARIO:
                    logger.Info("RECALCULO DE POSICAO PARA O SEGMENTO FRACIONARIO");

                    logger.Info("OBTEM LIMITES OPERACIONAIS");

                    #region LIMITE OPERACIONAL PARA COMPRA DE ACOES

                    var LIMITECOMPRAACOESFRA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                               where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                               select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAACOESFRA.Count() > 0)
                    {
                        foreach (var ITEM in LIMITECOMPRAACOESFRA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRAAVISTA = ITEM.CodigoParametroCliente;
                        }
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA DE ACOES

                    var LIMITEVENDAACOESFRA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                              where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                              select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAACOESFRA.Count() > 0)
                    {
                        foreach (var ITEM in LIMITEVENDAACOESFRA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDAAVISTA = ITEM.CodigoParametroCliente;
                        }
                    }

                    #endregion

                    logger.Info("RECALCULA A POSICAO DO CLIENTE");

                    #region NOVA OFERTA

                    if (OrderInfo.OrdStatus == OrdemStatusEnum.NOVA)
                    {
                        #region VALIDACAO DE PRECO POR ORDEM ENVIADA

                        decimal PRECO = decimal.Parse(OrderInfo.Price.ToString());
                        decimal QUANTIDADE = OrderInfo.OrderQty;

                        EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                        CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                        EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;


                        #endregion

                        VOLUMEORDEM = (PRECO * QUANTIDADE);

                        if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                        {
                            #region COMPRA DE ACOES

                            #region ATUALIZAÇÕES DOS LIMITES OPERACIONAIS

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion

                            #endregion
                        }
                        else
                        {
                            #region VENDA DE ACOES

                            #region ATUALIZAÇÕES DOS LIMITES OPERACIONAIS

                            //******************** DEBITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);

                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion

                            #endregion
                        }
                    }

                    #endregion

                    #region ALTERACAO DE ORDENS

                    if (OrderInfo.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                    {

                        if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                        {
                            #region SUBSTITUICAO DE COMPRA DE ACOES

                            #region TRATAMENTO DA ROTINA DE ALTERACAO DE ORDENS

                            double VOLUMEORIGINAL = 0;
                            double VOLUMEMODIFICACAO = 0;
                            double DIFERENCIAL = 0;

                            double PRECO = 0;

                            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                            CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);
                            PRECO = double.Parse(CotacaoResponse.CotacaoInfo.Ultima.ToString());

                            EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                            InformacoesRequest.NumeroControleOrdem = OrderInfo.OrigClOrdID;

                            EnviarInformacoesOrdemResponse InformacoesOrdem =
                             this.ObterInformacoesOrdemRecalculo(InformacoesRequest);

                            OrderInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                            VOLUMEORIGINAL = (PRECO * InformacoesOrdem.OrdemInfo.OrderQty);
                            VOLUMEMODIFICACAO = (PRECO * OrderInfo.OrderQty);
                            DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                            ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                            ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(OrderInfo);

                            //**************************************************************************************************************
                            // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                            //**************************************************************************************************************                                     

                            #endregion

                            #region ATUALIZACAO DE LIMITES

                            #region DEVOLVE O LIMITE ATUAL

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion

                            #region DEBITA O NOVO LIMITE

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda1 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda2 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion

                            #endregion

                            #endregion

                        }
                        else
                        {
                            #region SUBSTITUICAO DE VENDA DE ACOES

                            #region TRATAMENTO DA ROTINA DE ALTERACAO DE ORDENS

                            double VOLUMEORIGINAL = 0;
                            double VOLUMEMODIFICACAO = 0;
                            double DIFERENCIAL = 0;

                            double PRECO = 0;

                            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                            CotacaoRequest.Instrumento = OrderInfo.Symbol.Trim();
                            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);
                            PRECO = double.Parse(CotacaoResponse.CotacaoInfo.Ultima.ToString());

                            EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                            InformacoesRequest.NumeroControleOrdem = OrderInfo.OrigClOrdID;

                            EnviarInformacoesOrdemResponse InformacoesOrdem =
                           this.ObterInformacoesOrdemRecalculo(InformacoesRequest);

                            OrderInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                            VOLUMEORIGINAL = (PRECO * InformacoesOrdem.OrdemInfo.OrderQty);
                            VOLUMEMODIFICACAO = (PRECO * OrderInfo.OrderQty);
                            DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);


                            ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                            ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(OrderInfo);

                            //**************************************************************************************************************
                            // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                            //**************************************************************************************************************               

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
                                    CODIGOPARAMETROCLIENTEVENDAAVISTA = ITEM.CodigoParametroCliente;
                                }
                            }

                            #endregion

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
                                    LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                                    CODIGOPARAMETROCLIENTECOMPRAAVISTA = ITEM.CodigoParametroCliente;
                                }
                            }


                            #endregion

                            #endregion

                            #region ATUALIZACAO DE LIMITES

                            #region DEVOLVE O LIMITE ATUAL

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORIGINAL.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion

                            #region DEBITA O NOVO LIMITE

                            //******************** DEBITA O LIMITE DE COMPRA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal() * -1;
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda1 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de compra a vista");

                            //******************** CREDITA O LIMITE DE VENDA ************************************************/
                            AtualizarLimitesRequest = new AtualizarLimitesRequest();
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = Account;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                            AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEMODIFICACAO.DBToDecimal();
                            AtualizarLimitesResponse AtualizarLimitesResponseVenda2 = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                            logger.Info("Atualiza os limites de venda a vista");

                            #endregion


                            #endregion

                            #endregion

                        }
                    }

                    #endregion

                    break;

                case SegmentoMercadoEnum.FUTURO:


                    AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                    AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                    if (OrderInfo.Side == OrdemDirecaoEnum.Compra)
                    {
                        if (OrderInfo.OrdStatus == OrdemStatusEnum.NOVA)
                        {
                            #region NOVA OFERTA DE COMPRA

                            int QuantidadeContrato = 0;
                            int QuantidadeInstrumento = 0;
                            int idClienteParametroBMF = 0;
                            char stUtilizacaoInstrumento = 'N';

                            ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();
                            requestListaLimite.Account = OrderInfo.Account;

                            ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                            string Instrumento = OrderInfo.Symbol;
                            string Contrato = OrderInfo.Symbol.Substring(0, 3);

                            //CONTRATO BASE
                            var PosicaoContrato = from p in response.ListaLimites
                                                  where p.Contrato == Contrato

                                                  && p.Sentido == "C"
                                                  select p;

                            if (PosicaoContrato.Count() > 0)
                            {
                                foreach (var item in PosicaoContrato)
                                {
                                    idClienteParametroBMF = item.idClienteParametroBMF;
                                    QuantidadeContrato = item.QuantidadeDisponivel;

                                    if (response.ListaLimitesInstrumentos.Count > 0)
                                    {
                                        // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                        foreach (var vencimento in response.ListaLimitesInstrumentos)
                                        {
                                            if (vencimento.Instrumento == Instrumento)
                                            {
                                                if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                                {
                                                    QuantidadeInstrumento += vencimento.QtDisponivel;
                                                    Instrumento = vencimento.Instrumento;
                                                    stUtilizacaoInstrumento = 'S';
                                                }
                                            }
                                        }
                                    }
                                }

                                int QuantidadeSolicitada = OrderInfo.OrderQty;

                                // LIMITE SOMENTE PARA O CONTRATO
                                if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                                {
                                    requestBMF.account = requestListaLimite.Account;
                                    requestBMF.idClienteParametroBMF = idClienteParametroBMF;
                                    requestBMF.instrumento = Instrumento;
                                    requestBMF.quantidadeSolicitada = OrderInfo.OrderQty;
                                    requestBMF.stUtilizaInstrumento = stUtilizacaoInstrumento;

                                    responseBMF = new PersistenciaOrdens().AtualizaPosicaoLimiteBMF(requestBMF);

                                }

                                // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                                if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                                {
                                    requestBMF.account = requestListaLimite.Account;
                                    requestBMF.idClienteParametroBMF = idClienteParametroBMF;
                                    requestBMF.instrumento = Instrumento;
                                    requestBMF.quantidadeSolicitada = OrderInfo.OrderQty;
                                    requestBMF.stUtilizaInstrumento = stUtilizacaoInstrumento;

                                    responseBMF = new PersistenciaOrdens().AtualizaPosicaoLimiteBMF(requestBMF);
                                }

                            }
                            else
                            {
                                logger.Info("Não existe limite cadastrado");
                            }

                            #endregion

                        }

                        if (OrderInfo.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                        {
                            #region ALTERACAO DE ORDENS NA COMPRA

                            EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                            InformacoesRequest.NumeroControleOrdem = OrderInfo.OrigClOrdID;

                            EnviarInformacoesOrdemResponse InformacoesOrdem =
                             this.ObterInformacoesOrdemRecalculo(InformacoesRequest);

                            int QuantidadeOriginal = (InformacoesOrdem.OrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.CumQty);

                            int QuantidadeContrato = 0;
                            int QuantidadeInstrumento = 0;
                            int idClienteParametroBMF = 0;
                            char stUtilizacaoInstrumento = 'N';

                            ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();
                            requestListaLimite.Account = OrderInfo.Account;

                            ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                            string Instrumento = OrderInfo.Symbol;
                            string Contrato = OrderInfo.Symbol.Substring(0, 3);

                            //CONTRATO BASE
                            var PosicaoContrato = from p in response.ListaLimites
                                                  where p.Contrato == Contrato
                                                  && p.Sentido == "C"
                                                  select p;

                            if (PosicaoContrato.Count() > 0)
                            {
                                foreach (var item in PosicaoContrato)
                                {
                                    idClienteParametroBMF = item.idClienteParametroBMF;
                                    QuantidadeContrato = item.QuantidadeDisponivel;

                                    if (response.ListaLimitesInstrumentos.Count > 0)
                                    {
                                        // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                        foreach (var vencimento in response.ListaLimitesInstrumentos)
                                        {
                                            if (vencimento.Instrumento == Instrumento)
                                            {
                                                if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                                {
                                                    QuantidadeInstrumento += vencimento.QtDisponivel;
                                                    Instrumento = vencimento.Instrumento;
                                                    stUtilizacaoInstrumento = 'S';
                                                }
                                            }
                                        }
                                    }
                                }

                                int QuantidadeSolicitada = OrderInfo.OrderQty;

                                requestBMF.account = requestListaLimite.Account;
                                requestBMF.idClienteParametroBMF = idClienteParametroBMF;
                                requestBMF.instrumento = Instrumento;
                                requestBMF.quantidadeSolicitada = QuantidadeOriginal * -1;
                                requestBMF.stUtilizaInstrumento = stUtilizacaoInstrumento;

                                responseBMF = new PersistenciaOrdens().AtualizaPosicaoLimiteBMF(requestBMF);

                                requestBMF.account = requestListaLimite.Account;
                                requestBMF.idClienteParametroBMF = idClienteParametroBMF;
                                requestBMF.instrumento = Instrumento;
                                requestBMF.quantidadeSolicitada = OrderInfo.OrderQty;
                                requestBMF.stUtilizaInstrumento = stUtilizacaoInstrumento;

                                responseBMF = new PersistenciaOrdens().AtualizaPosicaoLimiteBMF(requestBMF);

                            }
                            else
                            {
                                logger.Info("Não existe limite cadastrado");
                            }

                            #endregion
                        }


                    }
                    else if (OrderInfo.Side == OrdemDirecaoEnum.Venda)
                    {
                        if (OrderInfo.OrdStatus == OrdemStatusEnum.NOVA)
                        {
                            #region  NOVA OFERTA DE VENDA

                            #region Controle de Pre-Risco BMF Venda

                            int QuantidadeContrato = 0;
                            int QuantidadeInstrumento = 0;
                            int idClienteParametroBMF = 0;
                            char stUtilizacaoInstrumento = 'N';

                            ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();
                            requestListaLimite.Account = OrderInfo.Account;

                            ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                            string Instrumento = OrderInfo.Symbol;
                            string Contrato = OrderInfo.Symbol.Substring(0, 3);

                            //CONTRATO BASE
                            var PosicaoContrato = from p in response.ListaLimites
                                                  where p.Contrato == Contrato
                                                  && p.Sentido == "V"
                                                  select p;

                            if (PosicaoContrato.Count() > 0)
                            {
                                foreach (var item in PosicaoContrato)
                                {
                                    idClienteParametroBMF = item.idClienteParametroBMF;
                                    QuantidadeContrato = item.QuantidadeDisponivel;

                                    if (response.ListaLimitesInstrumentos.Count > 0)
                                    {
                                        // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                        foreach (var vencimento in response.ListaLimitesInstrumentos)
                                        {
                                            if (item.idClienteParametroBMF == vencimento.IdClienteParametroBMF)
                                            {
                                                if (vencimento.Instrumento == Instrumento)
                                                {
                                                    QuantidadeInstrumento += vencimento.QtDisponivel;
                                                    Instrumento = vencimento.Instrumento;
                                                    stUtilizacaoInstrumento = 'S';
                                                }
                                            }
                                        }
                                    }
                                }

                                int QuantidadeSolicitada = OrderInfo.OrderQty;

                                // LIMITE SOMENTE PARA O CONTRATO
                                if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                                {

                                    requestBMF.account = requestListaLimite.Account;
                                    requestBMF.idClienteParametroBMF = idClienteParametroBMF;
                                    requestBMF.instrumento = Instrumento;
                                    requestBMF.quantidadeSolicitada = OrderInfo.OrderQty;
                                    requestBMF.stUtilizaInstrumento = stUtilizacaoInstrumento;

                                    responseBMF = new PersistenciaOrdens().AtualizaPosicaoLimiteBMF(requestBMF);

                                }

                                // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                                if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                                {

                                    requestBMF.account = requestListaLimite.Account;
                                    requestBMF.idClienteParametroBMF = idClienteParametroBMF;
                                    requestBMF.instrumento = Instrumento;
                                    requestBMF.quantidadeSolicitada = OrderInfo.OrderQty;
                                    requestBMF.stUtilizaInstrumento = stUtilizacaoInstrumento;

                                    responseBMF = new PersistenciaOrdens().AtualizaPosicaoLimiteBMF(requestBMF);
                                }
                            }

                            else
                            {
                                logger.Info("Cliente sem Limite BMF para venda");
                            }


                            #endregion

                            #endregion
                        }

                        if (OrderInfo.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                        {
                            #region ALTERACAO DE ORDEM DE VENDA

                            if (OrderInfo.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                            {
                                #region ALTERACAO DE ORDENS

                                EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                                InformacoesRequest.NumeroControleOrdem = OrderInfo.OrigClOrdID;

                                EnviarInformacoesOrdemResponse InformacoesOrdem =
                                 this.ObterInformacoesOrdemRecalculo(InformacoesRequest);

                                int QuantidadeOriginal = (InformacoesOrdem.OrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.CumQty);

                                int QuantidadeContrato = 0;
                                int QuantidadeInstrumento = 0;
                                int idClienteParametroBMF = 0;
                                char stUtilizacaoInstrumento = 'N';

                                ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();
                                requestListaLimite.Account = OrderInfo.Account;

                                ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                                string Instrumento = OrderInfo.Symbol;
                                string Contrato = OrderInfo.Symbol.Substring(0, 3);

                                //CONTRATO BASE
                                var PosicaoContrato = from p in response.ListaLimites
                                                      where p.Contrato == Contrato
                                                      && p.Sentido == "V"
                                                      select p;

                                if (PosicaoContrato.Count() > 0)
                                {
                                    foreach (var item in PosicaoContrato)
                                    {
                                        idClienteParametroBMF = item.idClienteParametroBMF;
                                        QuantidadeContrato = item.QuantidadeDisponivel;

                                        if (response.ListaLimitesInstrumentos.Count > 0)
                                        {
                                            // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                            foreach (var vencimento in response.ListaLimitesInstrumentos)
                                            {
                                                if (vencimento.Instrumento == Instrumento)
                                                {
                                                    if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                                    {
                                                        QuantidadeInstrumento += vencimento.QtDisponivel;
                                                        Instrumento = vencimento.Instrumento;
                                                        stUtilizacaoInstrumento = 'S';
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    int QuantidadeSolicitada = OrderInfo.OrderQty;

                                    requestBMF.account = requestListaLimite.Account;
                                    requestBMF.idClienteParametroBMF = idClienteParametroBMF;
                                    requestBMF.instrumento = Instrumento;
                                    requestBMF.quantidadeSolicitada = QuantidadeOriginal * -1;
                                    requestBMF.stUtilizaInstrumento = stUtilizacaoInstrumento;

                                    responseBMF = new PersistenciaOrdens().AtualizaPosicaoLimiteBMF(requestBMF);

                                    requestBMF.account = requestListaLimite.Account;
                                    requestBMF.idClienteParametroBMF = idClienteParametroBMF;
                                    requestBMF.instrumento = Instrumento;
                                    requestBMF.quantidadeSolicitada = OrderInfo.OrderQty;
                                    requestBMF.stUtilizaInstrumento = stUtilizacaoInstrumento;

                                    responseBMF = new PersistenciaOrdens().AtualizaPosicaoLimiteBMF(requestBMF);

                                }
                                else
                                {
                                    logger.Info("Não existe limite cadastrado");
                                }

                                #endregion
                            }

                            #endregion
                        }
                    }

                    break;
            }

        }

        private ExecutarModificacaoOrdensRequest PARSEARALTERACAOORDEM(OrdemInfo OrdemRequest)
        {
            PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
            ExecutarModificacaoOrdensRequest ModificaoOrdemRequest = new ExecutarModificacaoOrdensRequest();
            ModificaoOrdemRequest.info = new OrdemInfo();

            DateTime DataAux = DateTime.Now;

            try
            {
                logger.Info("Inicia a formatação da classe OrdemInfo");
                ModificaoOrdemRequest.info.ExecBroker = CORRETORA;

                ModificaoOrdemRequest.info.Exchange = EXCHANGEBOVESPA;
                ModificaoOrdemRequest.info.IdOrdem = OrdemRequest.IdOrdem;
                ModificaoOrdemRequest.info.OrdStatus = OrdemStatusEnum.SUBSTITUICAOSOLICITADA;
                ModificaoOrdemRequest.info.RegisterTime = DateTime.Now;
                ModificaoOrdemRequest.info.Side = OrdemRequest.Side;
                ModificaoOrdemRequest.info.Symbol = OrdemRequest.Symbol;
                ModificaoOrdemRequest.info.TimeInForce = OrdemRequest.TimeInForce;
                ModificaoOrdemRequest.info.Account = OrdemRequest.Account;
                ModificaoOrdemRequest.info.ChannelID = OrdemRequest.ChannelID;
                ModificaoOrdemRequest.info.ExecBroker = OrdemRequest.ExecBroker;
                ModificaoOrdemRequest.info.ExpireDate = OrdemRequest.ExpireDate;
                ModificaoOrdemRequest.info.MinQty = OrdemRequest.MinQty;
                ModificaoOrdemRequest.info.OrderQty = (OrdemRequest.CumQty + OrdemRequest.OrderQty);
                ModificaoOrdemRequest.info.SecurityID = OrdemRequest.SecurityID;
                ModificaoOrdemRequest.info.TransactTime = DateTime.Now;
                ModificaoOrdemRequest.info.OrdType = OrdemRequest.OrdType;
                ModificaoOrdemRequest.info.OrigClOrdID = OrdemRequest.OrigClOrdID;
                ModificaoOrdemRequest.info.ClOrdID = ContextoOMS.CtrlNumber;
                ModificaoOrdemRequest.info.Price = OrdemRequest.Price;

                #region VALIDADE DA ORDEM

                switch (OrdemRequest.TimeInForce)
                {

                    case  Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia:

                        ModificaoOrdemRequest.info.ExpireDate = new DateTime(DataAux.Year, DataAux.Month, DataAux.Day, 23, 59, 59);
                        break;

                    case Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada:

                        ModificaoOrdemRequest.info.ExpireDate = null;
                        break;

                    case Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidoAteDeterminadaData:

                        ModificaoOrdemRequest.info.ExpireDate = OrdemRequest.ExpireDate.Value;
                        break;
                }

                #endregion

                #region SECURITYID

                SecurityIDRequest SecurityIDRequest = new SecurityIDRequest();
                SecurityIDResponse SecurityIDResponse = new SecurityIDResponse();

                SecurityIDRequest.Instrumento = OrdemRequest.Symbol;
                SecurityIDResponse = PersistenciaOrdens.ObterSecurityID(SecurityIDRequest);

                ModificaoOrdemRequest.info.SecurityID = OrdemRequest.Symbol;

                #endregion

                return ModificaoOrdemRequest;

            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO PARSEAR A ORDEM SOLICITADA. NUMERO DE CONTROLE DA ORDEM: " + OrdemRequest.ClOrdID);
                throw (ex);
            }

        }

        /// <summary>
        /// Obter informações sobre a ordem
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private EnviarInformacoesOrdemResponse ObterInformacoesOrdem(EnviarInformacoesOrdemRequest request)
        {
            EnviarInformacoesOrdemResponse _ObterInformacoesOrdem = new EnviarInformacoesOrdemResponse();

            try
            {
                _ObterInformacoesOrdem = new PersistenciaOrdens().ObterInformacoesOrdem(request);

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao obter informações da oferta", ex);
            }

            return _ObterInformacoesOrdem;
        }

        /// <summary>
        /// Obter informações sobre a ordem
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private EnviarInformacoesOrdemResponse ObterInformacoesOrdemRecalculo(EnviarInformacoesOrdemRequest request)
        {
            EnviarInformacoesOrdemResponse _ObterInformacoesOrdem = new EnviarInformacoesOrdemResponse();

            try
            {
                _ObterInformacoesOrdem = new PersistenciaOrdens().ObterInformacoesOrdemRecalculo(request);

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao obter informações da oferta", ex);
            }

            return _ObterInformacoesOrdem;
        }

        /// <summary>
        /// Obter as cotacoes de um determinado instrumento
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private EnviarCotacaoResponse ObterCotacao(EnviarCotacaoRequest request)
        {
            try
            {
                return new PersistenciaOrdens().ObterCotacao(request);
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao obter a cotação do instrumento: " + request.Instrumento, ex);
                throw (ex);
            }

        }

        /// <summary>
        /// Metodo responsavel por atualizar o limite operacional no banco de dados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        private AtualizarLimitesResponse AtualizarLimiteOperacional(AtualizarLimitesRequest pRequest)
        {
            AtualizarLimitesResponse LimiteResponse = new AtualizarLimitesResponse();

            try
            {
                logger.Info("********************** LIMITE OPERACIONAL ************************************");

                logger.Info("CODIGO CLIENTE :.......................:" + pRequest.LimiteOperacionalInfo.CodigoCliente.ToString());
                logger.Info("CODIGO DO PARAMETRO :..................:" + pRequest.LimiteOperacionalInfo.CodigoParametroCliente.ToString());
                logger.Info("VALOR(R$) :............................:" + pRequest.LimiteOperacionalInfo.ValorAlocado.ToString());


                switch (pRequest.LimiteOperacionalInfo.TipoLimite)
                {
                    case TipoLimiteEnum.COMPRAAVISTA:
                        logger.Info("TIPO LIMITE :..........................:" + "COMPRA A VISTA");
                        break;

                    case TipoLimiteEnum.VENDAAVISTA:
                        logger.Info("TIPO LIMITE :..........................:" + "VENDA A VISTA");
                        break;

                    case TipoLimiteEnum.COMPRAOPCOES:
                        logger.Info("TIPO LIMITE :..........................:" + "COMPRA OPCOES");
                        break;

                    case TipoLimiteEnum.VENDAOPCOES:
                        logger.Info("TIPO LIMITE :..........................:" + "VENDA OPCOES");
                        break;
                }

                LimiteResponse = new PersistenciaOrdens().AtualizaLimiteCliente(pRequest);
                if (LimiteResponse.LimiteAtualizado)
                {
                    logger.Info("LIMITE ATUALIZADO COM SUCESSO.");
                }

                logger.Info("****************************************************************************");

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao atualizar o Limite operacional do cliente.", ex);
            }

            return LimiteResponse;
        }

        public void StartRouterCallBack()
        {
            IAssinaturasRoteadorOrdensCallback roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);

            logger.Info("Callback ativado com sucesso");
            AssinarExecucaoOrdemResponse resp = roteador.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());
            logger.Info("Assinatura de execução realizada com sucesso");
            AssinarStatusConexaoBolsaResponse cnxresp = roteador.AssinarStatusConexaoBolsa(new AssinarStatusConexaoBolsaRequest());
            logger.Info("Assinatura de execução realizada com sucesso");

            Thread thrMonitorRoteador = new Thread(new ThreadStart(RunMonitor));
            thrMonitorRoteador.Start();


        }


        #region Monitoramento

        /// <summary>
        ///MONITOR DE CONEXOES DO ROTEADOR
        /// </summary>
        private void RunMonitor()
        {
            try
            {                
                logger.Info("Iniciando thread de monitoracao do roteador de ordens");
                int _iMonitorConexoes = 0;

                if (roteador == null)
                    roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);

                while (_bKeepRunning)
                {
                    // 4 * 250 = 1 segundo 
                    if (_iMonitorConexoes == 30 * 4)
                    {
                        lock (roteador)
                        {
                            try
                            {
                                if (roteador == null)
                                    roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);                             
                            }
                            catch (Exception ex)
                            {
                                Ativador.AbortChannel(roteador);
                                roteador = null;
                            }
                        }
                        _iMonitorConexoes = 0;
                    }
                    else
                    {
                        _iMonitorConexoes++;
                    }

                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("RunMonitor(): " + ex.Message, ex);
            }

            logger.Info("THREAD DE MONITORAMENTO DO ROTEADOR FINALIZADA");
        }

        #endregion

        #endregion
    }
}
