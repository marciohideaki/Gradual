using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Alavancagem.Operacional;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.OMS.CadastroPapeis.Lib;
using Gradual.OMS.CadastroPapeis;
using System.Collections;
using Gradual.OMS.CadastroPapeis.Lib.Info;
using Gradual.OMS.CadastroPapeis.Lib.Mensageria;
using System.Threading;

namespace Gradual.OMS.Alavancagem.ReCalculoAutomatico
{
    public class ReCalculaClienteAlavancagem : IServicoControlavel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.Threading.Timer gTimerRecalculaPosicao;

        #region IServicoControlavel Members

        private ServicoStatus _ServicoStatus { set; get; }
      

        #endregion

        private void RecalcularClientePosicaoIntradiaria(object state)
        {
            string CodigoCliente = string.Empty;

            try
            {
                logger.Info("Inicio Rotina: Horario de solicitacao: " + DateTime.Now.ToString());

                //TODO: COLOCAR NO WEBCONFIG HORARIOS E TEMPORIZADOR.
                int HorarioDe = 9;
                int HorarioAte = 20;

                if ((DateTime.Now.Hour >= HorarioDe) && (DateTime.Now.Hour <= HorarioAte))
                {
                    logger.Info("Método RecalcularClientePosicaoIntradiaria invocado com sucesso, horario : " + DateTime.Now.ToString());

                    DateTime dtIni = DateTime.Now;

                    string DataAtual = DateTime.Now.ToString("yyyy-MM-dd");

                    List<string> PosicaoCliente = new PersistenciaRisco().ObterClientesOperacaoIntraday();

                    logger.Info("recalcular a posicao de " + PosicaoCliente.Count + " clientes");

                    for (int i = 0; i <= PosicaoCliente.Count - 1; i++)
                    {
                        if (PosicaoCliente.Count > 0)
                        {
                            CodigoCliente = PosicaoCliente[i];

                            int _CodigoCliente = int.Parse(CodigoCliente);

                            RecalculaPosicaoCliente(_CodigoCliente, 12);
                            RecalculaPosicaoCliente(_CodigoCliente, 7);

                            RecalculaPosicaoCliente(_CodigoCliente, 5);
                            RecalculaPosicaoCliente(_CodigoCliente, 13);

                        }

                    }

                    TimeSpan DataFinal = (DateTime.Now - dtIni);
                    logger.Info("*********** Posicao recalculada com sucesso ****************");
                    logger.Info("Total de clientes recalculados: " + PosicaoCliente.Count.ToString());
                    logger.Info("Tempo total do processo: " + DataFinal.ToString());
                }
                else
                {
                    logger.Info("Operacao fora do horario permitido");
                }

            
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao recalcular a posicao do cliente: " + CodigoCliente);
                logger.Error("Descricao do erro: " + ex.Message, ex);
            }

        }

        private void RecalculaPosicaoCliente(int CodigoCliente, int CodigoParamentro)
        {
            #region Declaracoes

            decimal VolumePosicaoExecutadaCompra = 0;
            decimal VolumePosicaoExecutadaVenda = 0;
            decimal NetOperacoesRealizadas = 0;
            decimal VolumePosicaoAberturaDia = 0;
            decimal VolumeOrdensEmAberto = 0;

            List<PosicaoClienteLimiteInfo> lstPosicaoClienteLimiteInfo
                = new List<PosicaoClienteLimiteInfo>();

            #endregion

            #region Venda descoberta de ações

            if (CodigoParamentro == (int)(ParametroRiscoEnum.ParametroDescobertoAVista))
            {
                List<string> lstInstrumentos = new List<string>();

                int QuantidadeAberturaDia = 0;
                int QuantidadeAbertaPregao = 0;
                int QuantidadeExecutadaCompra = 0;
                int QuantidadeExecutadaVenda = 0;
                int NetQuantidade = 0;

                logger.Info("Limite para venda descoberta no mercado a vista.");

                string DataAtual = DateTime.Now.ToString("yyyy-MM-dd");
                string Ativo = string.Empty;

                logger.Info("Carrega a posicao de Abertura do inicio do dia");
                var PosicaoAberturaDia = new PersistenciaRisco().ObterCustodiaAbertura(CodigoCliente);

                logger.Info("Obtem a lista dos instrumentos movimentados no pregão atual");
                List<string> InstrumentosMovimentadosDia = new PersistenciaRisco().ObterInstrumentosMovimentados(CodigoCliente, DataAtual, "VIS");

                logger.Info("Obtem os negocios abertos no dia");
                var PosicaoNegociosAbertosDia = new PersistenciaRisco().ObterPosicaoCustodiaOfertas(CodigoCliente, DataAtual, "VIS");

                logger.Info("Obtem os negocios executados no mercado a vista para o dia.");
                var NegociosRealizados = new PersistenciaRisco().ObterPosicaoCustodia(CodigoCliente, DataAtual, "VIS");

                logger.Info("Obtem a custódia do cliente no Intraday");
                var PosicaoCustodiaIntraday = new PersistenciaRisco().ObterPosicaoCustodiaIntraday(CodigoCliente, DataAtual);

                List<string> ListaInstrumentos = ObtemListaInstrumentos(PosicaoAberturaDia, InstrumentosMovimentadosDia, "VIS");

                logger.Info("Verifica se existem negócios realizados para iniciar o calculo");

                if (ListaInstrumentos.Count > 0){
                    foreach (var ItemPosicao in ListaInstrumentos){
                        if (!lstInstrumentos.Contains(ItemPosicao.ToString()))
                        {

                            #region [ Posicao de abertura do dia ]

                            logger.Info("Instrumento encontrado: " + ItemPosicao.ToString());
                            var PosicaoAberturaInstrumento = from p in PosicaoAberturaDia
                                                             where p.INSTRUMENTO == ItemPosicao.ToString()
                                                             && p.COD_CARTEIRA == 21016
                                                             select p;

                            logger.Info("Calcula a posicao de abertura do Instrumento");
                            if (PosicaoAberturaInstrumento.Count() > 0)
                            {
                                foreach (var item in PosicaoAberturaInstrumento)
                                {
                                    // VolumePosicaoAberturaDia = (item.NET_QTDE_PROJ * item.VL_FECHAMENTO);
                                    VolumePosicaoAberturaDia = (item.QTDE_TOTAL_DIA * item.VL_FECHAMENTO);
                                    logger.Info("Posicao calculada. Valor: " + VolumePosicaoAberturaDia.ToString());
                                    QuantidadeAberturaDia = item.QTDE_TOTAL_DIA;
                                }
                            }
                            else
                            {
                                logger.Info("Cliente não possui posicao de abertura");
                            }


                            #endregion

                            #region [ Posicao de Ofertas em Aberto ]

                            logger.Info("Calcula a posicao dos negocios abertos ");

                            Hashtable PosicaoOrdem = new Hashtable();

                            var PosicaoEmAbertoInstrumento = from p in PosicaoNegociosAbertosDia
                                                             where p.NatOperacao == "V"
                                                             && p.Instrumento == ItemPosicao.ToString()
                                                             orderby p.TransactTime descending, p.OrderStatusID descending

                                                             select p;

                            if (PosicaoEmAbertoInstrumento.Count() > 0)
                            {

                                var PosicaoEmAbertoOrdemAlterada = from p in PosicaoEmAbertoInstrumento
                                                                   where p.OrderStatusID == 5
                                                                   && p.Instrumento == ItemPosicao.ToString()
                                                                   orderby p.TransactTime descending

                                                                   select p;

                                if (PosicaoEmAbertoOrdemAlterada.Count() > 0)
                                {
                                    foreach (var item in PosicaoEmAbertoOrdemAlterada)
                                    {
                                        if (!PosicaoOrdem.Contains(item.OrderID))
                                        {
                                            lock (PosicaoOrdem)
                                            {
                                                PosicaoOrdem.Add(item.OrderID, item.Instrumento);
                                            }

                                            VolumeOrdensEmAberto += (item.Preco * item.Quantidade);
                                            QuantidadeAbertaPregao += item.Quantidade;
                                        }

                                    }
                                }

                                var PosicaoEmAbertoOrdem = from p in PosicaoEmAbertoInstrumento
                                                           where p.OrderStatusID == 0
                                                           && p.Instrumento == ItemPosicao.ToString()
                                                           orderby p.TransactTime descending

                                                           select p;

                                if (PosicaoEmAbertoOrdem.Count() > 0)
                                {
                                    foreach (var item in PosicaoEmAbertoOrdem)
                                    {
                                        lock (PosicaoOrdem)
                                        {
                                            if (!PosicaoOrdem.Contains(item.OrderID))
                                            {
                                                lock (PosicaoOrdem)
                                                {
                                                    PosicaoOrdem.Add(item.OrderID, item.Instrumento);
                                                }

                                                VolumeOrdensEmAberto += (item.Preco * item.Quantidade);
                                                QuantidadeAbertaPregao += item.Quantidade;
                                            }
                                        }

                                    }
                                }

                                logger.Info("Posicao em aberto: " + VolumeOrdensEmAberto.ToString());
                            }

                            lock (PosicaoOrdem)
                            {
                                PosicaoOrdem.Clear();
                            }

                            #endregion

                            #region [ Posicao de compra executada no dia ]

                            //TODO: FILTRAR MERCADO A VISTA

                            logger.Info("Calcula a posicao dos negocios executados na compra");
                            var PosicaoComprada = from p in PosicaoCustodiaIntraday
                                                  where p.NatOperacao == "C"
                                                  && p.Instrumento == ItemPosicao.ToString()
                                                  && p.Mercado == "VIS"

                                                  group p by p.Instrumento into g

                                                  select new
                                                  {
                                                      Instrumento = g.Key,
                                                      Preco_Quantidade = g.Sum(p => p.Quantidade) + "|" + g.Average(p => p.Preco)
                                                  };

                            #endregion

                            #region [ Posicao de venda executada no dia ]

                            logger.Info("Calcula a posicao dos negocios executados na venda");
                            var PosicaoVendida = from p in PosicaoCustodiaIntraday
                                                 where p.NatOperacao == "V"
                                                 && p.Instrumento == ItemPosicao.ToString()
                                                 && p.Mercado == "VIS"
                                                 group p by p.Instrumento into g

                                                 select new
                                                 {
                                                     Instrumento = g.Key,
                                                     Preco_Quantidade = g.Sum(p => p.Quantidade) + "|" + g.Average(p => p.Preco)
                                                 };

                            #endregion

                            Ativo = ItemPosicao.ToString();

                            if (PosicaoComprada.Count() > 0)
                            {
                                // EXISTE POSICAO NA COMPRA E NA VENDA
                                foreach (var item in PosicaoComprada)
                                {
                                    object[] param = item.Preco_Quantidade.Split('|');

                                    int Quantidade = param[0].DBToInt32();
                                    decimal Preco = param[1].DBToDecimal();

                                    QuantidadeExecutadaCompra = Quantidade;
                                    VolumePosicaoExecutadaCompra = (Preco * Quantidade);
                                }

                                logger.Info("Total executado na compra: " + VolumePosicaoExecutadaCompra.ToString());
                            }
                            else
                            {
                                logger.Info("Não existe ordens de compra executadas no dia");
                            }

                            if (PosicaoVendida.Count() > 0)
                            {
                                foreach (var item in PosicaoVendida)
                                {
                                    object[] param = item.Preco_Quantidade.Split('|');

                                    int Quantidade = param[0].DBToInt32();
                                    decimal Preco = param[1].DBToDecimal();

                                    QuantidadeExecutadaVenda = Quantidade;
                                    VolumePosicaoExecutadaVenda = (Preco * Quantidade);
                                }

                                logger.Info("Total executado na venda: " + VolumePosicaoExecutadaVenda.ToString());
                            }
                            else
                            {
                                logger.Info("Não existe ordens de venda executadas no dia");
                            }


                            if (Math.Abs(QuantidadeAbertaPregao) > 0)
                            {
                                QuantidadeAbertaPregao = QuantidadeAbertaPregao * -1;
                                VolumeOrdensEmAberto = VolumeOrdensEmAberto * -1;
                            }


                            NetQuantidade = (((QuantidadeExecutadaCompra - QuantidadeExecutadaVenda) + QuantidadeAberturaDia) + QuantidadeAbertaPregao);

                            if (NetQuantidade < 0)
                            {
                                NetOperacoesRealizadas = (((VolumePosicaoExecutadaCompra - VolumePosicaoExecutadaVenda) + VolumePosicaoAberturaDia) + VolumeOrdensEmAberto);
                            }
                            else
                            {
                                NetQuantidade = 0;
                            }

                            logger.Info("VolumePosicaoExecutadaCompra:" + VolumePosicaoExecutadaCompra.ToString());
                            logger.Info("VolumePosicaoExecutadaVenda :" + VolumePosicaoExecutadaVenda.ToString());
                            logger.Info("VolumePosicaoAberturaDia    :" + VolumePosicaoAberturaDia.ToString());
                            logger.Info("VolumeOrdensEmAberto        :" + VolumeOrdensEmAberto.ToString());

                            logger.Info("Net das operações realizadas: " + NetOperacoesRealizadas.ToString());

                            // Utilizando Limite Operacional
                            if (NetOperacoesRealizadas < 0)
                            {
                                PosicaoClienteLimiteInfo info = new PosicaoClienteLimiteInfo();

                                info.Quantidade = 0;
                                info.Preco = 0;
                                info.Instrumento = Ativo;
                                info.Volume = Math.Abs(NetOperacoesRealizadas);

                                logger.Info("Instrumento: " + Ativo + " Volume R$" + NetOperacoesRealizadas.ToString());

                                lstPosicaoClienteLimiteInfo.Add(info);

                            }
                        }

                        lock (lstInstrumentos)
                        {
                            lstInstrumentos.Add(Ativo);
                        }

                        // Zerando variáveis de controle.
                        VolumePosicaoExecutadaCompra = 0;
                        VolumePosicaoExecutadaVenda = 0;
                        VolumePosicaoAberturaDia = 0;
                        VolumeOrdensEmAberto = 0;
                        NetOperacoesRealizadas = 0;
                        QuantidadeExecutadaCompra = 0;
                        QuantidadeExecutadaVenda = 0;
                        QuantidadeAberturaDia = 0;
                        QuantidadeAbertaPregao = 0;
                        VolumePosicaoExecutadaVenda = 0;
                        VolumePosicaoAberturaDia = 0;
                        VolumeOrdensEmAberto = 0;
                        QuantidadeAbertaPregao = 0;

                    }
                }
            }

            #endregion

            #region  Venda Descoberta de opções

            else if (CodigoParamentro == (int)(ParametroRiscoEnum.ParametroDescobertoOpcoes))
            {
                List<string> lstInstrumentos = new List<string>();

                int QuantidadePapelBase = 0;
                int QuantidadeCobertaOpcoes = 0;

                decimal VolumeQuantidadePapelBase = 0;
                decimal VolumeQuantidadeCoberta = 0;

                int QuantidadeAbertaOfertaPapelBase = 0;
                int QuantidadeAbertaOfertaOpcao = 0;

                Hashtable htPapelBase = new Hashtable();

                #region [ Declaracoes negócios Opcoes ]

                int QuantidadeOpcoesExecutadaCompraDia = 0;
                int QuantidadeOpcoesExecutadaVendaDia = 0;

                decimal VolumeOpcoesExecutadoCompraDia = 0;
                decimal VolumeOpcoesExecutadoVendaDia = 0;

                #endregion

                #region [ Declaracoes negócios Acoes ]

                int QuantidadeAcoessExecutadaCompraDia = 0;
                int QuantidadeAcoesExecutadaVendaDia = 0;

                decimal VolumeAcoesExecutadoCompraDia = 0;
                decimal VolumeAcoesExecutadoVendaDia = 0;

                #endregion


                string DataAtual = DateTime.Now.ToString("yyyy-MM-dd");
                string Ativo = string.Empty;

                logger.Info("Obtem a posicao da abertura do dia");
                var PosicaoAberturaDia = new PersistenciaRisco().ObterCustodiaAbertura(CodigoCliente);

                logger.Info("Obtem a lista dos instrumentos movimentados no pregão atual");
                List<string> InstrumentosMovimentadosDia = new PersistenciaRisco().ObterInstrumentosMovimentados(CodigoCliente, DataAtual, "OPC");

                logger.Info("Obtem as ofertas abertas abertas para o dia no mercado de opções");
                var PosicaoOfertasAbertasOpcoesDia = new PersistenciaRisco().ObterPosicaoCustodiaOfertas(CodigoCliente, DataAtual, "OPC");

                logger.Info("Obtem as ofertas abertas abertas para o dia no mercado de ações");
                var PosicaoOfertasAbertasAcoesDia = new PersistenciaRisco().ObterPosicaoCustodiaOfertas(CodigoCliente, DataAtual, "VIS");

                logger.Info("Obtem a custódia do cliente no Intraday");
                var PosicaoCustodiaIntraday = new PersistenciaRisco().ObterPosicaoCustodiaIntraday(CodigoCliente, DataAtual);

                List<string> ListaInstrumentos = ObtemListaInstrumentos(PosicaoAberturaDia, InstrumentosMovimentadosDia, "OPC");

                logger.Info("Verifica os movimentos diarios realizados no mercado de opções");
                if (ListaInstrumentos.Count > 0)
                {
                    foreach (var ItemPosicao in ListaInstrumentos)
                    {
                        if (!lstInstrumentos.Contains(ItemPosicao.ToString()))
                        {

                            #region [Cadastro Papeis]

                            logger.Info("Invoca o servico de cadastro de papeis");
                            CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis =
                            new ServicoCadastroPapeis().ObterInformacoesIntrumento(
                            new CadastroPapeisRequest()
                            {
                                Instrumento = ItemPosicao.ToString()
                            });

                            logger.Info("Papel base :  " + CadastroPapeis.Objeto.PapelObjeto.ToString());
                            logger.Info("Instrumento: " + CadastroPapeis.Objeto.Instrumento.ToString());


                            #endregion

                            #region [ Quantidade Coberta no Papel base  ]

                            #region [ Posicao Papel base na abertura do pregão ]

                            // Custodia do Papel Base na abertura do dia

                            logger.Info("Valida a custódia do papel base na abertura do dia");
                            var CustodiaAberturaPapelBase = from p in PosicaoAberturaDia
                                                            where p.INSTRUMENTO == CadastroPapeis.Objeto.PapelObjeto
                                                            select p;

                            if (CustodiaAberturaPapelBase.Count() > 0)
                            {
                                foreach (var itemCustodia in CustodiaAberturaPapelBase)
                                {
                                    QuantidadePapelBase = itemCustodia.QTDE_TOTAL_NEGOCIAVEL;
                                    VolumeQuantidadePapelBase = (itemCustodia.QTDE_TOTAL_NEGOCIAVEL * itemCustodia.VL_FECHAMENTO);
                                }

                                logger.Info("QuantidadePapelBase: " + QuantidadePapelBase.ToString());
                            }
                            else
                            {
                                logger.Info("Não existe posicao do papel base para a abertura do dia");
                            }

                            #endregion

                            #region [ Posicao de compra executada no dia para o papel base ]

                            // Custodia da opção no Intraday
                            logger.Info("Verifica a posicao comprada do papel objeto no Intraday");
                            var PosicaoComprada = from p in PosicaoCustodiaIntraday
                                                  where p.NatOperacao == "C"
                                                  && p.Instrumento == CadastroPapeis.Objeto.PapelObjeto
                                                  group p by p.Instrumento into g

                                                  select new
                                                  {
                                                      Instrumento = g.Key,
                                                      Preco_Quantidade = g.Sum(p => p.Quantidade) + "|" + g.Average(p => p.Preco)
                                                  };

                            if (PosicaoComprada.Count() > 0)
                            {
                                // EXISTE POSICAO NA COMPRA E NA VENDA
                                foreach (var item in PosicaoComprada)
                                {
                                    object[] param = item.Preco_Quantidade.Split('|');

                                    int Quantidade = param[0].DBToInt32();
                                    decimal Preco = param[1].DBToDecimal();

                                    QuantidadeAcoessExecutadaCompraDia = Quantidade;
                                    VolumeAcoesExecutadoCompraDia = (Preco * Quantidade);

                                    logger.Info("Quantidade comprada do papel objeto no Intraday " + QuantidadeAcoessExecutadaCompraDia.ToString());
                                }
                            }
                            else
                            {
                                logger.Info("Não existe posicao comprada do papel objeto no Intraday");
                            }

                            #endregion

                            #region [ Posica de venda executada no dia para o papel base ]

                            logger.Info("Verifica a posicao vendida do papel objeto no Intraday");
                            var PosicaoVendida = from p in PosicaoCustodiaIntraday
                                                 where p.NatOperacao == "V"
                                                 && p.Instrumento == CadastroPapeis.Objeto.PapelObjeto
                                                 group p by p.Instrumento into g

                                                 select new
                                                 {
                                                     Instrumento = g.Key,
                                                     Preco_Quantidade = g.Sum(p => p.Quantidade) + "|" + g.Average(p => p.Preco)
                                                 };

                            logger.Info("Verifica a posicao vendida do papel objeto no Intraday");
                            if (PosicaoVendida.Count() > 0)
                            {
                                foreach (var item in PosicaoVendida)
                                {
                                    object[] param = item.Preco_Quantidade.Split('|');

                                    int Quantidade = param[0].DBToInt32();
                                    decimal Preco = param[1].DBToDecimal();

                                    QuantidadeAcoesExecutadaVendaDia = Quantidade;
                                    VolumeAcoesExecutadoVendaDia = (Preco * Quantidade);

                                    logger.Info("QuantidadeAcoesExecutadaVendaDia :" + QuantidadeAcoesExecutadaVendaDia.ToString());
                                }
                            }
                            else
                            {
                                logger.Info("Não existe posicao vendida para o papel no intraday");
                            }


                            #endregion

                            #region [Quantidade em aberto no papel base]

                            #region [ Posicao de Ofertas em Aberto ]


                            logger.Info("Calcula a posicao dos negocios abertos ");

                            Hashtable PosicaoOrdem = new Hashtable();

                            var PosicaoEmAbertoInstrumento = from p in PosicaoOfertasAbertasAcoesDia
                                                             where p.NatOperacao == "V"
                                                             && p.Instrumento == CadastroPapeis.Objeto.PapelObjeto
                                                             orderby p.TransactTime descending, p.OrderStatusID descending

                                                             select p;

                            if (PosicaoEmAbertoInstrumento.Count() > 0)
                            {

                                var PosicaoEmAbertoOrdemAlterada = from p in PosicaoEmAbertoInstrumento
                                                                   where p.OrderStatusID == 5
                                                                   && p.Instrumento == CadastroPapeis.Objeto.PapelObjeto
                                                                   orderby p.TransactTime descending

                                                                   select p;

                                if (PosicaoEmAbertoOrdemAlterada.Count() > 0)
                                {
                                    foreach (var item in PosicaoEmAbertoOrdemAlterada)
                                    {
                                        lock (PosicaoOrdem)
                                        {
                                            if (!PosicaoOrdem.Contains(item.OrderID))
                                            {
                                                lock (PosicaoOrdem)
                                                {
                                                    PosicaoOrdem.Add(item.OrderID, item.Instrumento);
                                                }

                                                VolumeOrdensEmAberto += (item.Preco * item.Quantidade);
                                                QuantidadeAbertaOfertaPapelBase += item.Quantidade;
                                            }
                                        }

                                    }
                                }

                                var PosicaoEmAbertoOrdem = from p in PosicaoEmAbertoInstrumento
                                                           where p.OrderStatusID == 0
                                                           && p.Instrumento == CadastroPapeis.Objeto.PapelObjeto
                                                           orderby p.TransactTime descending

                                                           select p;

                                if (PosicaoEmAbertoOrdem.Count() > 0)
                                {
                                    foreach (var item in PosicaoEmAbertoOrdem)
                                    {
                                        lock (PosicaoOrdem)
                                        {
                                            if (!PosicaoOrdem.Contains(item.OrderID))
                                            {
                                                lock (PosicaoOrdem)
                                                {
                                                    PosicaoOrdem.Add(item.OrderID, item.Instrumento);
                                                }

                                                VolumeOrdensEmAberto += (item.Preco * item.Quantidade);
                                                QuantidadeAbertaOfertaPapelBase += item.Quantidade;
                                            }
                                        }

                                    }
                                }

                                logger.Info("Posicao em aberto: " + VolumeOrdensEmAberto.ToString());
                            }

                            lock (PosicaoOrdem)
                            {
                                PosicaoOrdem.Clear();
                            }

                            #endregion




                            #endregion

                            QuantidadePapelBase = ((QuantidadePapelBase + (QuantidadeAcoessExecutadaCompraDia - QuantidadeAcoesExecutadaVendaDia)) - QuantidadeAbertaOfertaPapelBase);

                            logger.Info("QuantidadePapelBase                :" + QuantidadePapelBase.ToString());
                            logger.Info("QuantidadeAcoessExecutadaCompraDia :" + QuantidadeAcoessExecutadaCompraDia.ToString());
                            logger.Info("QuantidadeAcoesExecutadaVendaDia   :" + QuantidadeAcoesExecutadaVendaDia.ToString());
                            logger.Info("QuantidadeAbertaOfertaPapelBase    :" + QuantidadeAbertaOfertaPapelBase.ToString());

                            logger.Info("Quantidade no papel base: " + QuantidadePapelBase.ToString());

                            if (QuantidadePapelBase >= CadastroPapeis.Objeto.LoteNegociacao.DBToInt32())
                            {
                                lock (htPapelBase)
                                {
                                    if (!(htPapelBase.Contains(CadastroPapeis.Objeto.PapelObjeto)))
                                    {
                                        htPapelBase.Add(CadastroPapeis.Objeto.PapelObjeto, QuantidadePapelBase);
                                    }
                                }
                            }

                            #endregion

                            #region [ Quantidade Coberta na Opção ]

                            #region [Quantidade coberta da opção na abertura do pregão]

                            // Custodia da opção na abertura do dia
                            var CustodiaAberturaDia = from p in PosicaoAberturaDia
                                                      where p.INSTRUMENTO == CadastroPapeis.Objeto.Instrumento
                                                      select p;

                            if (CustodiaAberturaDia.Count() > 0)
                            {
                                foreach (var item in CustodiaAberturaDia)
                                {
                                    QuantidadeCobertaOpcoes = item.QTDE_TOTAL_NEGOCIAVEL;
                                    VolumeQuantidadeCoberta = (item.QTDE_TOTAL_NEGOCIAVEL * item.VL_FECHAMENTO);
                                }
                            }

                            #endregion

                            if (ItemPosicao == "BVMFI10")
                            {
                                bool VALOR = true;
                            }

                            #region [ Posicao de compra executada no dia para a Opção ]
                            // Custodia da opção no Intraday
                            var PosicaoCompradaOpcao = from p in PosicaoCustodiaIntraday
                                                       where p.NatOperacao == "C"
                                                       && p.Instrumento == CadastroPapeis.Objeto.Instrumento
                                                       group p by p.Instrumento into g

                                                       select new
                                                       {
                                                           Instrumento = g.Key,
                                                           Preco_Quantidade = g.Sum(p => p.Quantidade) + "|" + g.Average(p => p.Preco)
                                                       };

                            // EXISTE POSICAO NA COMPRA E NA VENDA
                            foreach (var item in PosicaoCompradaOpcao)
                            {
                                object[] param = item.Preco_Quantidade.Split('|');

                                int Quantidade = param[0].DBToInt32();
                                decimal Preco = param[1].DBToDecimal();

                                QuantidadeOpcoesExecutadaCompraDia = Quantidade;
                                VolumeOpcoesExecutadoCompraDia = (Preco * Quantidade);
                            }

                            #endregion

                            #region [ Posicao de venda executada no dia para a opção ]

                            var PosicaoVendidaOpcao = from p in PosicaoCustodiaIntraday
                                                      where p.NatOperacao == "V"
                                                      && p.Instrumento == CadastroPapeis.Objeto.Instrumento
                                                      group p by p.Instrumento into g

                                                      select new
                                                      {
                                                          Instrumento = g.Key,
                                                          Preco_Quantidade = g.Sum(p => p.Quantidade) + "|" + g.Average(p => p.Preco)
                                                      };


                            foreach (var item in PosicaoVendidaOpcao)
                            {
                                object[] param = item.Preco_Quantidade.Split('|');

                                int Quantidade = param[0].DBToInt32();
                                decimal Preco = param[1].DBToDecimal();

                                QuantidadeOpcoesExecutadaVendaDia = Quantidade;
                                VolumeOpcoesExecutadoVendaDia = (Preco * Quantidade);
                            }

                            #endregion

                            #region [ Quantidade em aberto na opção ]


                            //var OfertasOpcaoDia = from p in PosicaoOfertasAbertasOpcoesDia
                            //                      where p.Instrumento == ItemPosicao.ToString()
                            //                      select p;

                            //if (OfertasOpcaoDia.Count() > 0){
                            //    foreach (var itemOferta in OfertasOpcaoDia){
                            //        QuantidadeAbertaOfertaOpcao += itemOferta.Quantidade;
                            //    }
                            //}


                            // 34 20 89

                            logger.Info("Calcula a posicao dos negocios abertos ");



                            Hashtable PosicaoOrdemOfertas = new Hashtable();

                            var OfertasOpcaoDia = from p in PosicaoOfertasAbertasOpcoesDia
                                                  where p.Instrumento == ItemPosicao.ToString()
                                                  && p.NatOperacao == "V"
                                                  orderby p.TransactTime descending, p.OrderStatusID descending

                                                  select p;

                            if (OfertasOpcaoDia.Count() > 0)
                            {

                                var PosicaoEmAbertoOrdemAlterada = from p in OfertasOpcaoDia
                                                                   where p.OrderStatusID == 5
                                                                   && p.Instrumento == ItemPosicao.ToString()
                                                                   && p.NatOperacao == "V"
                                                                   orderby p.TransactTime descending

                                                                   select p;

                                if (PosicaoEmAbertoOrdemAlterada.Count() > 0)
                                {
                                    foreach (var item in PosicaoEmAbertoOrdemAlterada)
                                    {
                                        if (!PosicaoOrdemOfertas.Contains(item.OrderID))
                                        {
                                            lock (PosicaoOrdemOfertas)
                                            {
                                                PosicaoOrdemOfertas.Add(item.OrderID, item.Instrumento);
                                            }

                                            QuantidadeAbertaOfertaOpcao += item.Quantidade;
                                        }

                                    }
                                }

                                var PosicaoEmAbertoOrdem = from p in OfertasOpcaoDia
                                                           where p.OrderStatusID == 0
                                                           && p.Instrumento == ItemPosicao.ToString()
                                                           && p.NatOperacao == "V"
                                                           orderby p.TransactTime descending

                                                           select p;

                                if (PosicaoEmAbertoOrdem.Count() > 0)
                                {
                                    foreach (var item in PosicaoEmAbertoOrdem)
                                    {
                                        if (!PosicaoOrdemOfertas.Contains(item.OrderID))
                                        {
                                            lock (PosicaoOrdemOfertas)
                                            {
                                                PosicaoOrdemOfertas.Add(item.OrderID, item.Instrumento);
                                            }

                                            QuantidadeAbertaOfertaOpcao += item.Quantidade;
                                        }

                                    }
                                }

                                logger.Info("Posicao em aberto: " + VolumeOrdensEmAberto.ToString());
                            }

                            lock (PosicaoOrdemOfertas)
                            {
                                PosicaoOrdemOfertas.Clear();
                            }

                            #endregion


                            string sPapel = ItemPosicao;
                            string sPapelBase = CadastroPapeis.Objeto.PapelObjeto;

                            if (QuantidadeCobertaOpcoes <= 0)
                            {
                                QuantidadeCobertaOpcoes = 0;
                            }



                            QuantidadeCobertaOpcoes = (QuantidadeCobertaOpcoes + (QuantidadeOpcoesExecutadaCompraDia - QuantidadeOpcoesExecutadaVendaDia) - QuantidadeAbertaOfertaOpcao);

                            string AtivoBase = CadastroPapeis.Objeto.PapelObjeto;
                            string AtivoDerivativo = CadastroPapeis.Objeto.Instrumento;



                            // Cliente com posição em opções menor que 0
                            if (htPapelBase.Contains(AtivoBase))
                            {
                                int RefQuantidadePapelBase = int.Parse(htPapelBase[AtivoBase].ToString());
                                int ContadorQuantidade = (RefQuantidadePapelBase + QuantidadeCobertaOpcoes);

                                if (QuantidadeCobertaOpcoes < 0)
                                {
                                    lock (htPapelBase)
                                    {
                                        if (ContadorQuantidade >= 0)
                                        {
                                            htPapelBase.Remove(AtivoBase);
                                            htPapelBase.Add(AtivoBase, ContadorQuantidade);
                                        }
                                        else
                                        {
                                            // htPapelBase.Remove(AtivoBase);

                                            PosicaoClienteLimiteInfo info = new PosicaoClienteLimiteInfo();

                                            info.Instrumento = ItemPosicao;
                                            info.Quantidade = ContadorQuantidade;
                                            decimal Cotacao = new PersistenciaRisco().ObterPrecoCotacao(ItemPosicao);
                                            info.Preco = Cotacao;

                                            info.Volume = (Cotacao * (info.Quantidade * -1));

                                            lstPosicaoClienteLimiteInfo.Add(info);

                                        }
                                    }

                                }

                            }
                            else
                            {
                                if (QuantidadeCobertaOpcoes < 0)
                                {
                                    PosicaoClienteLimiteInfo info = new PosicaoClienteLimiteInfo();

                                    info.Instrumento = ItemPosicao;
                                    info.Quantidade = QuantidadeCobertaOpcoes;
                                    decimal Cotacao = new PersistenciaRisco().ObterPrecoCotacao(ItemPosicao);
                                    info.Preco = Cotacao;

                                    info.Volume = (Cotacao * (info.Quantidade * -1));

                                    lstPosicaoClienteLimiteInfo.Add(info);
                                }

                            }

                            #endregion

                        }

                        #region [Limpando as varivaies]

                        QuantidadePapelBase = 0;
                        QuantidadeCobertaOpcoes = 0;
                        QuantidadeCobertaOpcoes = 0;
                        QuantidadeOpcoesExecutadaCompraDia = 0;
                        QuantidadeOpcoesExecutadaVendaDia = 0;
                        QuantidadeAbertaOfertaOpcao = 0;
                        VolumeQuantidadePapelBase = 0;
                        VolumeQuantidadeCoberta = 0;
                        QuantidadeAbertaOfertaPapelBase = 0;
                        QuantidadeAbertaOfertaOpcao = 0;
                        QuantidadeOpcoesExecutadaCompraDia = 0;
                        QuantidadeOpcoesExecutadaVendaDia = 0;
                        VolumeOpcoesExecutadoCompraDia = 0;
                        VolumeOpcoesExecutadoVendaDia = 0;
                        QuantidadeAcoessExecutadaCompraDia = 0;
                        QuantidadeAcoesExecutadaVendaDia = 0;
                        VolumeAcoesExecutadoCompraDia = 0;
                        VolumeAcoesExecutadoVendaDia = 0;

                        #endregion
                    }
                }
            }

            #endregion

            #region Compra a vista

            else if (CodigoParamentro == (int)(ParametroRiscoEnum.ParametroCompraAVista))
            {

                logger.Info("Limite para compra no mercado a vista.");

                decimal? SaldoContaCorrente = 0;
                decimal ValorProjetadoAcoes = 0;
                decimal TotalOfertasAberto = 0;
                decimal TotalParcialmenteCancelada = 0;

                decimal TotalExecutadoCompra = 0;
                decimal TotalExecutadoVenda = 0;
                decimal TotalOrdemAberta = 0;

                int QuantidadeAbertaPregao = 0;

                logger.Info("Obtem saldo em conta corrente do cliente");

                ContaCorrenteInfo _SaldoCorrenteInfo = new PersistenciaRisco().ObterSaldoAbertura(CodigoCliente);

                logger.Info("Projeta o conta corrente do cliente ate D+3");
                SaldoContaCorrente = ((_SaldoCorrenteInfo.SaldoD0) +
                                        (_SaldoCorrenteInfo.SaldoD1) +
                                        (_SaldoCorrenteInfo.SaldoD2) +
                                        (_SaldoCorrenteInfo.Liquidacoes)
                                     );

                logger.Info("Saldo projetado :" + SaldoContaCorrente.Value.ToString());
                logger.Info("SaldoD0 : " + _SaldoCorrenteInfo.SaldoD0.ToString());
                logger.Info("SaldoD1 : " + _SaldoCorrenteInfo.SaldoD1.ToString());
                logger.Info("SaldoD2 : " + _SaldoCorrenteInfo.SaldoD2.ToString());
                logger.Info("Liquidacoes : " + _SaldoCorrenteInfo.Liquidacoes.ToString());

                string DataAtual = DateTime.Now.ToString("yyyy-MM-dd");

                #region [ordens abertas]

                List<string> InstrumentosMovimentadosDiaAcoes = new PersistenciaRisco().ObterInstrumentosMovimentados(CodigoCliente, DataAtual, "VIS");

                logger.Info("Obtem a custódia do cliente no Intraday");
                var PosicaoNegociosAbertosDia = new PersistenciaRisco().ObterPosicaoCustodiaOfertas(CodigoCliente, DataAtual, "ALL");

                foreach (var ItemPosicao in InstrumentosMovimentadosDiaAcoes)
                {
                    #region [ Posicao de Ofertas em Aberto ]

                    logger.Info("Calcula a posicao dos negocios abertos ");

                    Hashtable PosicaoOrdem = new Hashtable();

                    var PosicaoEmAbertoInstrumento = from p in PosicaoNegociosAbertosDia
                                                     where p.NatOperacao == "C"
                                                     && p.Instrumento == ItemPosicao.ToString()
                                                     orderby p.TransactTime descending, p.OrderStatusID descending

                                                     select p;

                    if (PosicaoEmAbertoInstrumento.Count() > 0)
                    {

                        var PosicaoEmAbertoOrdemAlterada = from p in PosicaoEmAbertoInstrumento
                                                           where p.OrderStatusID == 5
                                                           && p.Instrumento == ItemPosicao.ToString()
                                                           orderby p.TransactTime descending

                                                           select p;

                        if (PosicaoEmAbertoOrdemAlterada.Count() > 0)
                        {
                            foreach (var item in PosicaoEmAbertoOrdemAlterada)
                            {
                                if (!PosicaoOrdem.Contains(item.OrderID))
                                {
                                    lock (PosicaoOrdem)
                                    {
                                        PosicaoOrdem.Add(item.OrderID, item.Instrumento);
                                    }

                                    VolumeOrdensEmAberto += (item.Preco * item.Quantidade);
                                    QuantidadeAbertaPregao += item.Quantidade;
                                }

                            }
                        }

                        var PosicaoEmAbertoOrdem = from p in PosicaoEmAbertoInstrumento
                                                   where p.OrderStatusID == 0
                                                   && p.Instrumento == ItemPosicao.ToString()
                                                   orderby p.TransactTime descending

                                                   select p;

                        if (PosicaoEmAbertoOrdem.Count() > 0)
                        {
                            foreach (var item in PosicaoEmAbertoOrdem)
                            {
                                if (!PosicaoOrdem.Contains(item.OrderID))
                                {
                                    lock (PosicaoOrdem)
                                    {
                                        PosicaoOrdem.Add(item.OrderID, item.Instrumento);
                                    }

                                    VolumeOrdensEmAberto += (item.Preco * item.Quantidade);
                                    QuantidadeAbertaPregao += item.Quantidade;
                                }

                            }
                        }

                        logger.Info("Posicao em aberto: " + VolumeOrdensEmAberto.ToString());
                    }

                    lock (PosicaoOrdem)
                    {
                        PosicaoOrdem.Clear();
                    }

                    #endregion
                }


                #endregion

                decimal ordensAbertas = VolumeOrdensEmAberto;

                //Obtendo a Posicao da Nota de Corretagem do dia para posição de D+3
                logger.Info("Obtendo a posição projetada das operações realizadas no dia (D+3)");
                var PosicaoProjetada = new PersistenciaRisco().ObterPosicaoSaldoProjetado(CodigoCliente, DataAtual);

                if (PosicaoProjetada.Count > 0){

                    TotalExecutadoCompra = PosicaoProjetada[0] * -1;
                    TotalExecutadoVenda = PosicaoProjetada[1];
                    TotalParcialmenteCancelada = PosicaoProjetada[2];

                    TotalOrdemAberta = VolumeOrdensEmAberto * -1;

                    logger.Info("TotalExecutadoCompra:" + TotalExecutadoCompra.ToString());
                    logger.Info("TotalExecutadoVenda :" + TotalExecutadoVenda.ToString());
                    logger.Info("TotalOrdemAberta    :" + TotalOrdemAberta.ToString());
                    logger.Info("TotalParcialmenteCancelada    :" + TotalParcialmenteCancelada.ToString());

                    ValorProjetadoAcoes = (TotalExecutadoCompra + TotalExecutadoVenda + TotalOrdemAberta + TotalParcialmenteCancelada);

                    logger.Info("ValorProjetadoAcoes    :" + ValorProjetadoAcoes.ToString());

                    SaldoContaCorrente += ValorProjetadoAcoes;
                    logger.Info("SaldoContaCorrente    :" + SaldoContaCorrente.ToString());
                }
                else
                {
                    logger.Info("Não existe operacoes projetadas para D+3");
                }

                decimal? LimiteUtilizado = 0;

                SaldoContaCorrente = (SaldoContaCorrente + (TotalOfertasAberto * -1));

                logger.Info("Verifica se o saldo total é negativo");
                if (SaldoContaCorrente < 0)
                {
                    LimiteUtilizado = SaldoContaCorrente;
                    logger.Info("LimiteUtilizado: " + LimiteUtilizado.ToString());

                    PosicaoClienteLimiteInfo info = new PosicaoClienteLimiteInfo();

                    info.Quantidade = 0;
                    info.Preco = 0;
                    info.Instrumento = "VALOR CC - A VISTA";
                    info.Volume = (LimiteUtilizado * -1);

                    logger.Info("Instrumento: " + info.Instrumento + " Volume R$" + info.Volume.ToString());

                    lstPosicaoClienteLimiteInfo.Add(info);
                }
                else
                {
                    logger.Info("Não é necessário utilizar limite");
                }

            }
            #endregion

            #region Compra de opções

            else if (CodigoParamentro == (int)(ParametroRiscoEnum.ParametroCompraOpcoes))
            {

                logger.Info("Limite para compra no mercado a vista.");

                decimal? SaldoContaCorrente = 0;
                decimal ValorProjetadoOpcoes = 0;
                decimal TotalOfertasAberto = 0;

                decimal TotalExecutadoCompra = 0;
                decimal TotalExecutadoVenda = 0;
                decimal TotalOrdemAberta = 0;

                int QuantidadeAbertaPregao = 0;

                logger.Info("Obtem saldo em conta corrente do cliente");

                ContaCorrenteInfo _SaldoCorrenteInfo = new PersistenciaRisco().ObterSaldoAbertura(CodigoCliente);

                logger.Info("Projeta o conta corrente do cliente ate D+3");
                SaldoContaCorrente = ((_SaldoCorrenteInfo.SaldoD0) +
                                       (_SaldoCorrenteInfo.SaldoD1) +
                                       (_SaldoCorrenteInfo.SaldoD2) +
                                       (_SaldoCorrenteInfo.Liquidacoes));

                string DataAtual = DateTime.Now.ToString("yyyy-MM-dd");

                logger.Info("Saldo projetado :" + SaldoContaCorrente.Value.ToString());
                logger.Info("SaldoD0 : " + _SaldoCorrenteInfo.SaldoD0.ToString());
                logger.Info("SaldoD1 : " + _SaldoCorrenteInfo.SaldoD1.ToString());
                logger.Info("SaldoD2 : " + _SaldoCorrenteInfo.SaldoD2.ToString());
                logger.Info("Liquidacoes : " + _SaldoCorrenteInfo.Liquidacoes.ToString());

                #region [ordens abertas]

                List<string> InstrumentosMovimentadosDiaAcoes = new PersistenciaRisco().ObterInstrumentosMovimentados(CodigoCliente, DataAtual, "VIS");

                logger.Info("Obtem a custódia do cliente no Intraday");
                var PosicaoNegociosAbertosDia = new PersistenciaRisco().ObterPosicaoCustodiaOfertas(CodigoCliente, DataAtual, "ALL");

                foreach (var ItemPosicao in InstrumentosMovimentadosDiaAcoes)
                {
                    #region [ Posicao de Ofertas em Aberto ]

                    logger.Info("Calcula a posicao dos negocios abertos ");

                    Hashtable PosicaoOrdem = new Hashtable();

                    var PosicaoEmAbertoInstrumento = from p in PosicaoNegociosAbertosDia
                                                     where p.NatOperacao == "C"
                                                     && p.Instrumento == ItemPosicao.ToString()
                                                     orderby p.TransactTime descending, p.OrderStatusID descending

                                                     select p;

                    if (PosicaoEmAbertoInstrumento.Count() > 0)
                    {

                        var PosicaoEmAbertoOrdemAlterada = from p in PosicaoEmAbertoInstrumento
                                                           where p.OrderStatusID == 5
                                                           && p.Instrumento == ItemPosicao.ToString()
                                                           orderby p.TransactTime descending

                                                           select p;

                        if (PosicaoEmAbertoOrdemAlterada.Count() > 0)
                        {
                            foreach (var item in PosicaoEmAbertoOrdemAlterada)
                            {
                                if (!PosicaoOrdem.Contains(item.OrderID))
                                {
                                    lock (PosicaoOrdem)
                                    {
                                        PosicaoOrdem.Add(item.OrderID, item.Instrumento);
                                    }

                                    VolumeOrdensEmAberto += (item.Preco * item.Quantidade);
                                    QuantidadeAbertaPregao += item.Quantidade;
                                }

                            }
                        }

                        var PosicaoEmAbertoOrdem = from p in PosicaoEmAbertoInstrumento
                                                   where p.OrderStatusID == 0
                                                   && p.Instrumento == ItemPosicao.ToString()
                                                   orderby p.TransactTime descending

                                                   select p;

                        if (PosicaoEmAbertoOrdem.Count() > 0)
                        {
                            foreach (var item in PosicaoEmAbertoOrdem)
                            {
                                if (!PosicaoOrdem.Contains(item.OrderID))
                                {
                                    lock (PosicaoOrdem)
                                    {
                                        PosicaoOrdem.Add(item.OrderID, item.Instrumento);
                                    }

                                    VolumeOrdensEmAberto += (item.Preco * item.Quantidade);
                                    QuantidadeAbertaPregao += item.Quantidade;
                                }

                            }
                        }

                        logger.Info("Posicao em aberto: " + VolumeOrdensEmAberto.ToString());
                    }

                    lock (PosicaoOrdem)
                    {
                        PosicaoOrdem.Clear();
                    }

                    #endregion
                }


                #endregion

                //Obtendo a Posicao da Nota de Corretagem do dia para posição de D+3
                var PosicaoProjetada = new PersistenciaRisco().ObterPosicaoSaldoProjetado(CodigoCliente, DataAtual);

                if (PosicaoProjetada.Count > 0)
                {

                    TotalExecutadoCompra = (PosicaoProjetada[0] * -1);
                    TotalExecutadoVenda = PosicaoProjetada[1];
                    TotalOrdemAberta = VolumeOrdensEmAberto * -1;

                    logger.Info("TotalExecutadoCompra:" + TotalExecutadoCompra.ToString());
                    logger.Info("TotalExecutadoVenda :" + TotalExecutadoVenda.ToString());
                    logger.Info("TotalOrdemAberta    :" + TotalOrdemAberta.ToString());

                    ValorProjetadoOpcoes = (TotalExecutadoCompra + TotalExecutadoVenda + TotalOrdemAberta);
                    logger.Info("ValorProjetadoOpcoes    :" + ValorProjetadoOpcoes.ToString());

                    SaldoContaCorrente += ValorProjetadoOpcoes;
                    logger.Info("SaldoContaCorrente    :" + SaldoContaCorrente.ToString());
                }
                else
                {
                    logger.Info("Não existe operacoes projetadas para D+3");
                }


                decimal? LimiteUtilizado = 0;

                SaldoContaCorrente = (SaldoContaCorrente + (TotalOfertasAberto * -1));

                logger.Info("Verifica se o saldo total é negativo");
                if (SaldoContaCorrente < 0)
                {
                    LimiteUtilizado = SaldoContaCorrente;

                    PosicaoClienteLimiteInfo info = new PosicaoClienteLimiteInfo();

                    info.Quantidade = 0;
                    info.Preco = 0;
                    info.Instrumento = "VALOR CC - OPCOES";
                    info.Volume = (LimiteUtilizado * -1);

                    logger.Info("Instrumento: " + info.Instrumento + " Volume R$" + info.Volume.ToString());

                    lstPosicaoClienteLimiteInfo.Add(info);
                }
                else
                {
                    logger.Info("Não é necessário utilizar limite");
                }
            }

            #endregion

            logger.Info("Efetua a chamada do metodo RecalcularLimiteClienteCustodia. Cliente: " + CodigoCliente.ToString() + " Código Parametro: " + CodigoParamentro.ToString());
            new PersistenciaRisco().RecalcularLimiteClienteCustodia(CodigoCliente, CodigoParamentro, lstPosicaoClienteLimiteInfo);

        }

        private List<string> ObtemListaInstrumentos(List<CustodiaAberturaInfo> PosicaoAbertura, List<string> InstrumentosMovimentadosDia, string TipoMercado)
        {

            try
            {
                List<string> lstInstrumentos = new List<string>();


                var PosicaoAberturaDia = from p in PosicaoAbertura
                                         where p.TIPO_MERCADO == TipoMercado
                                         select p;

                if (PosicaoAberturaDia.Count() > 0)
                {
                    foreach (var item in PosicaoAberturaDia)
                    {
                        lock (lstInstrumentos)
                        {
                            lstInstrumentos.Add(item.INSTRUMENTO);
                        }
                    }

                }

                foreach (string item in InstrumentosMovimentadosDia)
                {
                    lock (lstInstrumentos)
                    {
                        if (!lstInstrumentos.Contains(item))
                        {
                            lstInstrumentos.Add(item);
                        }
                    }
                }

                return lstInstrumentos;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObtemListaInstrumentos");
                logger.Info("Descrição do erro: " + ex.Message);
            }

            return null;
        }


        #region IServicoControlavel Members

        void IServicoControlavel.IniciarServico()
        {
            log4net.Config.XmlConfigurator.Configure();
            _ServicoStatus = ServicoStatus.EmExecucao;

            logger.Info("Servico Iniciado com sucesso");

            gTimerRecalculaPosicao = new System.Threading.Timer(new TimerCallback(RecalcularClientePosicaoIntradiaria), null, 0, (30 * 1000));

            logger.Info("Thread disparada com sucesso");
        }

        void IServicoControlavel.PararServico()
        {
            _ServicoStatus = ServicoStatus.Parado;
        }

        ServicoStatus IServicoControlavel.ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion
    }



}
