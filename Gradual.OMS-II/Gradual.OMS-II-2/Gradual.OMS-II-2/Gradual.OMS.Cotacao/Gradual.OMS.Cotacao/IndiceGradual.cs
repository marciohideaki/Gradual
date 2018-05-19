using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using log4net;
using System.Globalization;

namespace Gradual.OMS.Cotacao
{
    /// <summary>
    /// Classe responsável por calcular o Indice Gradual, de acordo com a composição da carteira definida.
    /// </summary>

    [Serializable]
    public class IndiceGradual
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");

        private Dictionary<string, ItemIndice> listaIndice = null;
        private Dictionary<string, List<ItemComposicaoIndice>> listaComposicao = null;

        private DateTime dataCotacaoIBOV = DateTime.MinValue;
        private int statusIBOV = 0;

        #region Propriedades (membros públicos)

        public class ItemIndice
        {
            public string indice { get; set; }
            public int codigoIndice { get; set; }
            public double cotacaoAtual { get; set; }
            public double fechamentoAnterior { get; set; }
            public double variacao { get; set; }
            public DateTime dataCotacao { get; set; }
        }

        public class ItemComposicaoIndice
        {
            public string ativo { get; set; }
            public double cotacao { get; set; }
            public double variacao { get; set; }
            public double qtdeTeorica { get; set; }
            public double qtdeAjustada { get; set; }
            public DateTime dataCotacao { get; set; }
        }

        public bool CalcularIndiceGradual
        {
            get
            {
                if (ConfigurationManager.AppSettings["CalcularIndiceGradual"] == null)
                    return false;
                return bool.Parse(ConfigurationManager.AppSettings["CalcularIndiceGradual"]);
            }
        }

        public bool EnviarPriceLinkBloomberg
        {
            get
            {
                if (ConfigurationManager.AppSettings["EnviarPriceLinkBloomberg"] == null)
                    return false;
                return bool.Parse(ConfigurationManager.AppSettings["EnviarPriceLinkBloomberg"]);
            }
        }

        public string BloombergSecurityID
        {
            get
            {
                if (ConfigurationManager.AppSettings["BloombergSecurityID"] == null)
                    return "IGB30I";
                return ConfigurationManager.AppSettings["BloombergSecurityID"];
            }
        }

        public string BloombergTransactionType
        {
            get
            {
                if (ConfigurationManager.AppSettings["BloombergTransactionType"] == null)
                    return "Trade";
                return ConfigurationManager.AppSettings["BloombergTransactionType"];
            }
        }

        public string BloombergSecurityType
        {
            get
            {
                if (ConfigurationManager.AppSettings["BloombergSecurityType"] == null)
                    return "IDXOPTION";
                return ConfigurationManager.AppSettings["BloombergSecurityType"];
            }
        }

        public string BloombergSecurityIdType
        {
            get
            {
                if (ConfigurationManager.AppSettings["BloombergSecurityIdType"] == null)
                    return "Ticker";
                return ConfigurationManager.AppSettings["BloombergSecurityIdType"];
            }
        }

        public string INDICE_IBOV = "IBOV";

        public IndiceGradual()
        {
            RecarregarComposicaoIndices();
        }

        public void RecarregarComposicaoIndices()
        {
            try
            {
                if (CalcularIndiceGradual)
                {
                    logger.Info("*** Carregando Composição dos Índices Gradual...");

                    listaIndice = new Dictionary<string, ItemIndice>();
                    listaComposicao = new Dictionary<string, List<ItemComposicaoIndice>>();

                    DCotacoes _DCotacoes = new DCotacoes();
                    List<ItemIndice> lista = _DCotacoes.ObterListaIndicesGradual();

                    foreach (ItemIndice item in lista)
                    {
                        logger.Info("Indice Gradual[" +
                            item.indice + "]: cod[" +
                            item.codigoIndice + "] cotacao[" +
                            item.cotacaoAtual + "] fechamento[" +
                            item.fechamentoAnterior + "] variacao[" +
                            item.variacao + "] data[" +
                            item.dataCotacao + "]");
                        listaIndice.Add(item.indice, item);

                        List<ItemComposicaoIndice> listaComposicaoIndice = _DCotacoes.ObterListaComposicaoIndiceGradual(item.codigoIndice);
                        logger.Info("Indice Gradual[" + item.indice + "] com " + listaComposicaoIndice.Count + " ativos carregados");
                        foreach (ItemComposicaoIndice itemComposicao in listaComposicaoIndice)
                        {
                            logger.Info("Indice Gradual[" +
                                item.indice + "]: ativo[" +
                                itemComposicao.ativo + "] qtdeTeorica[" +
                                itemComposicao.qtdeTeorica + "] data[" +
                                itemComposicao.dataCotacao + "]");
                        }
                        listaComposicao.Add(item.indice, listaComposicaoIndice);
                    }

                    logger.Info("*** Composição dos Índices carregados!");
                }
            }
            catch (Exception ex)
            {
                logger.Error("RecarregarComposicaoIndices(): " + ex.Message, ex);
            }
        }

        public void AtualizarIndiceGradual(string instrumento, string mensagem)
        {
            try
            {
                if (CalcularIndiceGradual)
                {
                    DCotacoes _DCotacoes = new DCotacoes();

                    string dataHoraMensagem = mensagem.Substring(41, 14);
                    DateTime dataCotacao = new DateTime(
                        int.Parse(dataHoraMensagem.Substring(0, 4)),
                        int.Parse(dataHoraMensagem.Substring(4, 2)),
                        int.Parse(dataHoraMensagem.Substring(6, 2)),
                        int.Parse(dataHoraMensagem.Substring(8, 2)),
                        int.Parse(dataHoraMensagem.Substring(10, 2)),
                        int.Parse(dataHoraMensagem.Substring(12, 2)));

                    int status = int.Parse(mensagem.Substring(155, 1));

                    // Se serviço ativado ou cotação já é do dia seguinte, recarrega a composição dos Índices
                    if (dataCotacaoIBOV.Equals(DateTime.MinValue) || dataCotacao.Date > dataCotacaoIBOV.Date)
                    {
                        statusIBOV = 2;
                        dataCotacaoIBOV = dataCotacao;
                        logger.Info("Iniciando dia de negociações com dataCotação[" + dataCotacaoIBOV + "] e status[" + statusIBOV + "]");
                        RecarregarComposicaoIndices();

                        foreach (ItemIndice dadosIndice in listaIndice.Values)
                        {
                            dadosIndice.fechamentoAnterior = dadosIndice.cotacaoAtual;

                            logger.Info("Indice[" +
                                dadosIndice.indice + "]: Atualizando Fechamento Anterior[" +
                                dadosIndice.fechamentoAnterior + "] com a Cotação Atual[" +
                                dadosIndice.cotacaoAtual + "]");
                        }
                    }

                    // Se cotação do IBOV não está mais em negociação, de um mesmo dia, atualiza o fechamento dos Índices
                    else if (statusIBOV == 2 && status == 0 && dataCotacao.Date == dataCotacaoIBOV.Date)
                    {
                        foreach (ItemIndice dadosIndice in listaIndice.Values)
                        {
                            if (dataCotacao.Date == dadosIndice.dataCotacao)
                            {
                                dadosIndice.fechamentoAnterior = dadosIndice.cotacaoAtual;

                                _DCotacoes.AtualizarCotacaoIndice(dadosIndice);

                                logger.Info("Indice[" +
                                    dadosIndice.indice + "]: Atualizado Fechamento Anterior[" +
                                    dadosIndice.fechamentoAnterior + "] com a Cotação Atual[" +
                                    dadosIndice.cotacaoAtual + "] variacao[" +
                                    dadosIndice.variacao + "] data[" +
                                    dadosIndice.dataCotacao + "]");

                                statusIBOV = status;
                            }
                        }
                    }

                    if (status == 2 && statusIBOV == 2)
                    {
                        foreach (ItemIndice dadosIndice in listaIndice.Values)
                        {
                            double valorIndice = 0;
                            bool gravarComposicaoIndice = true;

                            foreach (ItemComposicaoIndice item in listaComposicao[dadosIndice.indice])
                            {
                                lock (MemoriaCotacao.hstCotacoesIndiceGradual)
                                {
                                    if (MemoriaCotacao.hstCotacoesIndiceGradual.ContainsKey(item.ativo))
                                    {
                                        string cotacao = (string)MemoriaCotacao.hstCotacoesIndiceGradual[item.ativo];

                                        item.cotacao = Convert.ToDouble(cotacao.Substring(74, 13), ciBR);

                                        string dataHora = cotacao.Substring(41, 14);
                                        DateTime data = new DateTime(
                                            int.Parse(dataHora.Substring(0, 4)),
                                            int.Parse(dataHora.Substring(4, 2)),
                                            int.Parse(dataHora.Substring(6, 2)),
                                            int.Parse(dataHora.Substring(8, 2)),
                                            int.Parse(dataHora.Substring(10, 2)),
                                            int.Parse(dataHora.Substring(12, 2)));

                                        if (item.cotacao == 0.0 || !DateTime.Now.ToString("yyyyMMdd").Equals(data.ToString("yyyyMMdd")))
                                        {
                                            logger.FatalFormat("Ativo [{0}] cotacao [{1}] ult neg [{2}] impedindo o calculo do indice",
                                                item.ativo,
                                                item.cotacao,
                                                data.ToString("dd/MM/yyyy HH:mm:ss.fff"));

                                            gravarComposicaoIndice = false;
                                            break;
                                        }
                                        item.variacao = Convert.ToDouble(cotacao.Substring(146, 9).Trim(), ciBR);

                                        item.qtdeAjustada = item.qtdeTeorica * item.cotacao;
                                        valorIndice += item.qtdeAjustada;

                                        item.dataCotacao = dataCotacao;
                                    }
                                    else
                                    {
                                        logger.FatalFormat("Ativo [{0}] sem cotacao na memoria, impedindo o calculo do indice", item.ativo);
                                        gravarComposicaoIndice = false;
                                        break;
                                    }
                                }
                            }

                            // Apenas grava as atualizações de composição do indice e as atualizações do indice,
                            // se todas as cotações dos ativos estiverem com valor > 0 e tiverem cotações no dia corrente
                            if (gravarComposicaoIndice)
                            {
                                foreach (ItemComposicaoIndice item in listaComposicao[dadosIndice.indice])
                                {
                                    _DCotacoes.AtualizarComposicaoIndiceGradual(dadosIndice.indice, item, false);

                                    logger.Info("Gravado Composicao de Indice[" +
                                        dadosIndice.indice + "]: ativo[" +
                                        item.ativo + "]: cotacao[" +
                                        item.cotacao + "] variacao[" +
                                        item.variacao + "] qtdeAjustada[" +
                                        item.qtdeAjustada + "] data[" +
                                        item.dataCotacao + "]");
                                }

                                double variacao = 0;
                                double fechamento = dadosIndice.fechamentoAnterior;
                                if (fechamento != 0)
                                    variacao = ((valorIndice / fechamento) - 1) * 100;

                                dadosIndice.cotacaoAtual = valorIndice;
                                dadosIndice.variacao = variacao;
                                dadosIndice.dataCotacao = dataCotacao;

                                _DCotacoes.AtualizarCotacaoIndice(dadosIndice);

                                logger.Info("Gravado Indice[" +
                                    dadosIndice.indice + "]: cotacao[" +
                                    dadosIndice.cotacaoAtual + "] fechamento[" +
                                    dadosIndice.fechamentoAnterior + "] variacao[" +
                                    dadosIndice.variacao + "] data[" +
                                    dadosIndice.dataCotacao + "]");

                                if (EnviarPriceLinkBloomberg)
                                {
                                    PriceLink.PriceLinkSession sessionPriceLink = new PriceLink.PriceLinkSession();

                                    string indiceAtual = String.Format("{0:0.00}", dadosIndice.cotacaoAtual).Replace(",", ".");

                                    sessionPriceLink.Contribute(
                                        BloombergSecurityID,
                                        indiceAtual,
                                        BloombergTransactionType,
                                        BloombergSecurityType,
                                        BloombergSecurityIdType);

                                    logger.Info("Enviado Bloomberg PriceLink SecurityID[" + BloombergSecurityID + 
                                        "]: cotacaoAtual[" + indiceAtual + 
                                        "] TransactionType[" + BloombergTransactionType + 
                                        "] SecurityType[" + BloombergSecurityType + 
                                        "] SecurityIdType[" + BloombergSecurityIdType + "]");

                                    sessionPriceLink = null;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("AtualizarIndiceGradual(): " + ex.Message, ex);
            }
        }

        #endregion
    }
}
