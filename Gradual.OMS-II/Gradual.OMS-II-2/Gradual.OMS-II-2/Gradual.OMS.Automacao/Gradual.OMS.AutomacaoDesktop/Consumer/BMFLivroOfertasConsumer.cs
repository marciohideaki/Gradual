using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.espertech.esper.client;
using com.espertech.esper.compat.collections;
using Gradual.OMS.AutomacaoDesktop.Events;
using System.Threading;
using log4net;
using Gradual.OMS.Automacao.Lib;
using System.IO;

namespace Gradual.OMS.AutomacaoDesktop.Consumer
{
    public class BMFLivroOfertasConsumer
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EPServiceProvider epService;
        private SortedDictionary<string, BMFLivroOfertas> todosLivrosBMF;
        private LinkedBlockingQueue<EventoBMF> filaMensagensLivroOfertas;
        private int maximoItens;
        private AutomacaoConfig parametros;
        private Thread _me = null;
        private DadosGlobais dadosGlobais;

        public BMFLivroOfertasConsumer(
                DadosGlobais dadosGlobais,
                LinkedBlockingQueue<EventoBMF> filaMensagensLivroOfertas,
                int maximoItens)
        {
            this.parametros = dadosGlobais.Parametros;
            this.epService = dadosGlobais.EpService;
            this.todosLivrosBMF = dadosGlobais.TodosLivrosBMF;
            this.filaMensagensLivroOfertas = filaMensagensLivroOfertas;
            this.maximoItens = maximoItens;
            this.dadosGlobais = dadosGlobais;
        }

        public void Start()
        {
            _me = new Thread(new ThreadStart(Run));
            _me.Start();
        }

        public void Stop()
        {
            if (_me != null)
            {
                while (_me.IsAlive)
                    Thread.Sleep(250);
            }
        }

        private void Run()
        {
            EventoBMF evento;
            string instrumento;
            string mensagem;
            string cabecalho;
            string tipo;
            long antes;
            long depois;

            //Calendar dataHoraCorrente = Calendar.getInstance();
            //SimpleDateFormat mascaraDataHora = new SimpleDateFormat("yyyyMMddHHmmssSSS");

            while (this.dadosGlobais.KeepRunning)
            {
                evento = null;
                try
                {
                    evento = filaMensagensLivroOfertas.Pop();
                }
                catch (Exception intExcept)
                {
                    logger.Error("InterruptedException na leitura da fila de mensagens:" + intExcept.Message, intExcept);
                    return;
                }
                antes = DateTime.Now.Ticks;

                instrumento = evento.Instrumento;
                tipo = evento.Tipo;
                mensagem = evento.Mensagem;

                // Inicializa os Livros de Ofertas de instrumentos em memoria, 
                // durante o recebimento das mensagens de instrumentos BMF 
                if (tipo.Equals(ConstantesMDS.TIPO_REQUISICAO_BMF_INSTRUMENTO))
                {
                    if (todosLivrosBMF.ContainsKey(instrumento))
                    {
                        todosLivrosBMF[instrumento].inicializaBMFLivrosOfertas();
                    }
                    continue;
                }

                contabilizaLivroOfertas(instrumento, tipo, mensagem);
                

                // Cabecalho da mensagem
                cabecalho = ConstantesMDS.TIPO_REQUISICAO_LIVRO;
                cabecalho = cabecalho + ConstantesMDS.DESCRICAO_DE_BOLSA_BMF;
                cabecalho = cabecalho + DateTime.Now.ToString("yyyyMMddHHmmssfff");;
                cabecalho = cabecalho + string.Format("%1$-20s", instrumento);

                logger.Info("LOF [" + instrumento + "]");

                geraEvento(instrumento,
                    cabecalho,
                    dadosGlobais.TodosLivrosBMF[instrumento].LivrosCompraBMF,
                    dadosGlobais.TodosLivrosBMF[instrumento].LivrosVendaBMF);

                depois = DateTime.Now.Ticks;
                TimeSpan duracao = new TimeSpan(depois - antes);
                logger.Debug("Duracao do processamento: " + duracao.TotalMilliseconds +
                        "ms (Mensagens na fila: " + filaMensagensLivroOfertas.Count + ")");

                if (instrumento.Equals("WINZ10"))
                {
                    FileStream fs = File.Open(@"c:\temp\WINZ10.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(fs, Encoding.ASCII);


                    writer.WriteLine("================================================================================");
                    writer.WriteLine(evento.Sequencia + "," + evento.Mensagem);
                    List<string> livroCompraSerializado = todosLivrosBMF[instrumento].serializarLivro(BovespaLivroOfertas.LIVRO_COMPRA);
                    List<string> livroVendaSerializado = todosLivrosBMF[instrumento].serializarLivro(BovespaLivroOfertas.LIVRO_VENDA);
                    for (int i = 0; (i < livroCompraSerializado.Count || i < livroVendaSerializado.Count); i++)
                    {
                        string linha = "";
                        if (i < livroCompraSerializado.Count)
                            linha += livroCompraSerializado[i].PadLeft(30);
                        else
                            linha += " ".PadLeft(30);

                        linha += "|";
                        if (i < livroVendaSerializado.Count)
                            linha += livroVendaSerializado[i].PadLeft(30);
                        else
                            linha += " ".PadLeft(30);

                        writer.WriteLine(linha);

                    }

                    writer.WriteLine("================================================================================");
                    writer.Close();
                    fs.Close();
                }

            }
        }

        private bool contabilizaLivroOfertas(string instrumento, string tipo, string mensagem)
        {
            string acao = "";
            int posicao = 0;
            string data = "";
            string dataOriginal = "";
            string hora = "";
            string horaOriginal = "";
            Decimal preco = new Decimal(0);
            long quantidade = 0;
            //DateFormat formatoDataHora = new SimpleDateFormat("yyyyMMddHHmmss");
            //Calendar dataHoraConvertido;

            try
            {
                acao = mensagem.Substring(
                        EventoBMF.MDUPDATEACTION_INI,
                        EventoBMF.MDUPDATEACTION_FIM - EventoBMF.MDUPDATEACTION_INI).Trim();

                string posicaoSemFormato = mensagem.Substring(
                        EventoBMF.MDENTRYPOSITIONNO_INI,
                        EventoBMF.MDENTRYPOSITIONNO_FIM - EventoBMF.MDENTRYPOSITIONNO_INI).Trim();
                if (posicaoSemFormato == null || posicaoSemFormato.Length==0)
                {
                    logger.Error("Instrumento[" + instrumento + "]: Posicao inexistente");
                    return false;
                }
                posicao = Convert.ToInt32(posicaoSemFormato) - 1;

                if (!acao.Equals(ConstantesMDS.TIPO_ACAO_BMF_EXCLUIR))
                {
                    dataOriginal = mensagem.Substring(
                            EventoBMF.MDENTRYDATE_INI,
                            EventoBMF.MDENTRYDATE_FIM-EventoBMF.MDENTRYDATE_INI);
                    horaOriginal = mensagem.Substring(
                            EventoBMF.MDENTRYTIME_INI,
                            EventoBMF.MDENTRYTIME_FIM - EventoBMF.MDENTRYTIME_INI).Replace(":", "");

                    //dataHoraConvertido = Calendar.getInstance(TimeZone.getTimeZone(
                    //        parametros.getFormatoDataHoraGMT()));
                    //dataHoraConvertido.set(
                    //        Integer.parseInt(dataOriginal.substring(0, 4)),
                    //        Integer.parseInt(dataOriginal.substring(4, 6)) - 1,
                    //        Integer.parseInt(dataOriginal.substring(6, 8)),
                    //        Integer.parseInt(horaOriginal.substring(0, 2)),
                    //        Integer.parseInt(horaOriginal.substring(2, 4)),
                    //        Integer.parseInt(horaOriginal.substring(4, 6)));

                    data = dataOriginal.Substring(0, 4);
                    data += dataOriginal.Substring(4, 2);
                    data += dataOriginal.Substring(6, 2);

                    hora = horaOriginal.Substring(0, 2);
                    hora += horaOriginal.Substring(2, 2);
                    hora += horaOriginal.Substring(4, 2);

                    string precoSemFormato = mensagem.Substring(
                            EventoBMF.MDENTRYPX_INI,
                            EventoBMF.MDENTRYPX_FIM - EventoBMF.MDENTRYPX_INI).Trim();
                    if (precoSemFormato.Length==0 )
                    {
                        logger.Error("Instrumento[" + instrumento + "]: Preco inexistente");
                        return false;
                    }

                    preco = Convert.ToDecimal(precoSemFormato);

                    string quantidadeSemFormato = mensagem.Substring(
                            EventoBMF.MDENTRYSIZE_INI,
                            EventoBMF.MDENTRYSIZE_FIM - EventoBMF.MDENTRYSIZE_INI).Trim();
                    if (quantidadeSemFormato.Length==0)
                    {
                        logger.Error("Instrumento[" + instrumento + "]: Quantidade inexistente");
                        return false;
                    }
                    quantidade = Convert.ToInt64(quantidadeSemFormato);
                }
            }
            catch (Exception e)
            {
                logger.Error("Falha na formatacao da mensagem[" + mensagem + "]: " + e.Message, e);
                return false;
            }

            if (!todosLivrosBMF.ContainsKey(instrumento))
            {
                todosLivrosBMF.Add(instrumento, new BMFLivroOfertas());
            }

            // Trata mensagem para livro de ofertas de compra
            if (tipo.Equals(ConstantesMDS.TIPO_REQUISICAO_BMF_LIVRO_OFERTAS_COMPRA))
            {
                string compradoraPorExtenso = mensagem.Substring(
                        EventoBMF.MDENTRYBUYER_INI,
                        EventoBMF.MDENTRYBUYER_FIM - EventoBMF.MDENTRYBUYER_INI).Replace("BM", "").Trim();

                if (compradoraPorExtenso.Length==0 )
                {
                    logger.Error("Instrumento[" + instrumento +
                            "]: Corretora Compradora nao encontrada na mensagem [" + mensagem + "]");
                }
                else
                {
                    //long compradora = Convert.ToInt64(compradoraPorExtenso);

                    try
                    {
                        if (acao.Equals(ConstantesMDS.TIPO_ACAO_BMF_INCLUIR))
                        {
                            todosLivrosBMF[instrumento].LivrosCompraBMF.Insert(
                                    posicao,
                                    new LivroOfertasEntry("BMF", instrumento, compradoraPorExtenso, data + hora, "A", quantidade, preco, "L"));
                            logger.Debug("Instrumento[" + instrumento +
                                    "]: Inclui oferta de compra - posicao[" + posicao +
                                    "] preco[" + string.Format("%013.2f", preco) +
                                    "] quantidade[" + quantidade +
                                    "] compradora[" + compradoraPorExtenso + "]");
                        }
                        else if (acao.Equals(ConstantesMDS.TIPO_ACAO_BMF_ALTERAR))
                        {
                            todosLivrosBMF[instrumento].LivrosCompraBMF[posicao] =
                                    new LivroOfertasEntry("BMF", instrumento, compradoraPorExtenso, data + hora, "A", quantidade, preco, "L");
                            logger.Debug("Instrumento[" + instrumento +
                                    "]: Altera oferta de compra - posicao[" + posicao +
                                    "] preco[" + string.Format("%013.2f", preco) +
                                    "] quantidade[" + quantidade +
                                    "] compradora[" + compradoraPorExtenso + "]");
                        }
                        else if (acao.Equals(ConstantesMDS.TIPO_ACAO_BMF_EXCLUIR))
                        {
                            try
                            {
                                todosLivrosBMF[instrumento].LivrosCompraBMF.RemoveAt(posicao);
                                logger.Debug("Instrumento[" + instrumento +
                                        "]: Exclui oferta de compra - posicao[" + posicao + "]");
                            }
                            catch (Exception e)
                            {
                                logger.Error("Instrumento[" + instrumento + "]: nao encontrado posicao " +
                                        posicao + "do livro de ofertas de compra" );
                                logger.Error("contabilizaLivroOferta:" + e.Message, e);
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        logger.Error("Instrumento[" + instrumento +
                                "]: Falha na manutencao do Livro Oferta de Compra - acao[" +
                                acao + "] - " + ex.Message,ex);
                    }
                }
            }

            // Trata mensagem para livro de ofertas de venda
            else if (tipo.Equals(ConstantesMDS.TIPO_REQUISICAO_BMF_LIVRO_OFERTAS_VENDA))
            {
                string vendedoraPorExtenso = mensagem.Substring(
                        EventoBMF.MDENTRYSELLER_INI,
                        EventoBMF.MDENTRYSELLER_FIM - EventoBMF.MDENTRYSELLER_INI).Replace("BM", "").Trim();

                if (vendedoraPorExtenso.Length==0)
                {
                    logger.Error("Instrumento[" + instrumento +
                            "]: Corretora Vendedora nao encontrada na mensagem [" + mensagem + "]");
                }
                else
                {
                    //long vendedora = Convert.ToInt64(vendedoraPorExtenso);

                    try
                    {
                        if (acao.Equals(ConstantesMDS.TIPO_ACAO_BMF_INCLUIR))
                        {
                            todosLivrosBMF[instrumento].LivrosVendaBMF.Insert(
                                    posicao,
                                    new LivroOfertasEntry("BMF", instrumento, vendedoraPorExtenso, data + hora, "V", quantidade, preco, "L"));
                            logger.Debug("Instrumento[" + instrumento +
                                    "]: Inclui oferta de venda - posicao[" + posicao +
                                    "] preco[" + string.Format("%013.2f", preco) +
                                    "] quantidade[" + quantidade +
                                    "] compradora[" + vendedoraPorExtenso + "]");
                        }
                        else if (acao.Equals(ConstantesMDS.TIPO_ACAO_BMF_ALTERAR))
                        {
                            todosLivrosBMF[instrumento].LivrosVendaBMF[posicao]=
                                    new LivroOfertasEntry("BMF", instrumento, vendedoraPorExtenso, data + hora, "V", quantidade, preco, "L");
                            logger.Debug("Instrumento[" + instrumento +
                                    "]: Altera oferta de venda - posicao[" + posicao +
                                    "] preco[" + string.Format("%013.2f", preco) +
                                    "] quantidade[" + quantidade +
                                    "] compradora[" + vendedoraPorExtenso + "]");
                        }
                        else if (acao.Equals(ConstantesMDS.TIPO_ACAO_BMF_EXCLUIR))
                        {
                            try
                            {
                                todosLivrosBMF[instrumento].LivrosVendaBMF.RemoveAt(posicao);
                                logger.Debug("Instrumento[" + instrumento +
                                        "]: Exclui oferta de venda - posicao[" + posicao + "]");
                            }
                            catch (Exception e)
                            {
                                logger.Error("Instrumento[" + instrumento + "]: nao encontrado posicao " +
                                        posicao + "do livro de ofertas de venda");
                                logger.Error("contabilizaLivroOfertas: " + e.Message, e);
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        logger.Error("Instrumento[" + instrumento +
                                "]: Falha na manutencao do Livro Oferta de Venda - acao[" +
                                acao + "] - " + ex.Message,ex);
                    }
                }
            }
            return true;
        }

        private void geraEvento(string instrumento, string cabecalho, List<LivroOfertasEntry> livroOfertasCompra, List<LivroOfertasEntry> livroOfertasVenda)
        {
            EventoAtualizacaoLivroOfertas mensagemEvento =
                new EventoAtualizacaoLivroOfertas(
                        instrumento,
                        ConstantesMDS.PLATAFORMA_TODAS,
                        cabecalho,
                        livroOfertasCompra,
                        livroOfertasVenda);

            epService.EPRuntime.SendEvent(mensagemEvento);
        }

    }
}
