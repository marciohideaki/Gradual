using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using log4net;
using System.Globalization;

namespace Gradual.OMS.Cotacao
{
    /// <summary>
    /// Classe responsável por armazenas as estruturas de Índices e efetuar cálculo de valor e valorização.
    /// </summary>

    [Serializable]
    public class ComposicaoIndice
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");

        #region Propriedades (membros públicos)

        public class ItemComposicaoIndice
        {
            public string indice { get; set; }
            public string papel { get; set; }
            public double qtdeTeorica { get; set; }
            public double valor { get; set; }
            public double fechamento { get; set; }
            public DateTime dataCotacao { get; set; }
        }

        public class ItemIndice
        {
            public string indice { get; set; }
            public int codigoIndice { get; set; }
            public double valor { get; set; }
            public double fechamento { get; set; }
            public double oscilacao { get; set; }
            public DateTime dataCotacao { get; set; }

            public ItemIndice()
            { }

            public ItemIndice(string indice, int codigoIndice, double valor, double fechamento, double oscilacao, DateTime dataCotacao)
            {
                this.indice = indice;
                this.codigoIndice = codigoIndice;
                this.valor = valor;
                this.fechamento = fechamento;
                this.oscilacao = oscilacao;
                this.dataCotacao = dataCotacao;
            }
        }

        public class ItemQtdeTeorica
        {
            public string papel { get; set; }
            public double qtdeTeorica { get; set; }
            public double valor { get; set; }
            public double fechamento { get; set; }
            public DateTime dataCotacao { get; set; }

            public ItemQtdeTeorica(
                string papel, 
                double qtdeTeorica, 
                double valor, 
                double fechamento, 
                DateTime dataCotacao)
            {
                this.papel = papel;
                this.qtdeTeorica = qtdeTeorica;
                this.valor = valor;
                this.fechamento = fechamento;
                this.dataCotacao = dataCotacao;
            }
        }

        public bool EfetuarCalculosIndice
        {
            get
            {
                if (ConfigurationManager.AppSettings["EfetuarCalculosIndice"] == null)
                    return false;
                return bool.Parse(ConfigurationManager.AppSettings["EfetuarCalculosIndice"]);
            }
        }

        /// <summary>
        /// HashTable que armazena os Índices.
        /// </summary>
        public Hashtable hstIndices = new Hashtable();

        /// <summary>
        /// HashTable que armazena os Papéis previstos em um ou mais Índices.
        /// </summary>
        public Hashtable hstPapeis = new Hashtable();

        /// <summary>
        /// HashTable que armazena os papéis e respectivas quantidades teóricas que cada Índice compõe.
        /// </summary>
        public Hashtable hstComposicaoIndice = new Hashtable();

        public string INDICE_IBOV = "IBOV";
        public DateTime dataCotacaoIBOV = DateTime.MinValue;
        public int statusIBOV = 0;

        public ComposicaoIndice()
        {
            RecarregarComposicaoIndices();
        }

        public Hashtable GetListaIndices()
        {
            return hstIndices;
        }

        public void RecarregarComposicaoIndices()
        {
            if (EfetuarCalculosIndice)
            {
                logger.Info("*** Carregando Composição dos Índices...");

                DCotacoes _DCotacoes = new DCotacoes();

                List<ItemIndice> listaIndice = _DCotacoes.ObterDadosIndice();
                List<ItemComposicaoIndice> listaComposicaoIndice = _DCotacoes.ObterDadosComposicaoIndice();

                hstIndices.Clear();
                foreach (ItemIndice item in listaIndice)
                    hstIndices.Add(item.indice, 
                        new ItemIndice(
                            item.indice, 
                            item.codigoIndice, 
                            item.fechamento, 
                            item.fechamento, 
                            item.oscilacao, 
                            item.dataCotacao));

                hstPapeis.Clear();
                hstComposicaoIndice.Clear();
                foreach (ItemComposicaoIndice item in listaComposicaoIndice)
                {
                    if (!hstPapeis.Contains(item.papel))
                    {
                        List<string> listaNomeIndices = new List<string>();
                        listaNomeIndices.Add(item.indice);
                        hstPapeis.Add(item.papel, listaNomeIndices);
                    }
                    else
                    {
                        List<string> listaNomeIndices = (List<string>)hstPapeis[item.papel];
                        listaNomeIndices.Add(item.indice);
                    }

                    if (!hstComposicaoIndice.Contains(item.indice))
                    {
                        List<ItemQtdeTeorica> listaQtdeTeorica = new List<ItemQtdeTeorica>();
                        listaQtdeTeorica.Add(new ItemQtdeTeorica(item.papel, item.qtdeTeorica, item.valor, item.fechamento, item.dataCotacao));
                        hstComposicaoIndice.Add(item.indice, listaQtdeTeorica);
                    }
                    else
                    {
                        List<ItemQtdeTeorica> listaQtdeTeorica = (List<ItemQtdeTeorica>)hstComposicaoIndice[item.indice];
                        listaQtdeTeorica.Add(new ItemQtdeTeorica(item.papel, item.qtdeTeorica, item.valor, item.fechamento, item.dataCotacao));
                    }
                }
                hstPapeis.Add(INDICE_IBOV, null);

                IDictionaryEnumerator itensIndice = hstIndices.GetEnumerator();
                while (itensIndice.MoveNext())
                {
                    logger.Info("Índice [" + itensIndice.Key + "]: fechamento[" + ((ItemIndice)itensIndice.Value).fechamento + "] dataCotacao[" + ((ItemIndice)itensIndice.Value).dataCotacao + "]");
                }

                IDictionaryEnumerator itens = hstComposicaoIndice.GetEnumerator();
                while (itens.MoveNext())
                {
                    logger.Info("Índice [" + itens.Key + "] contém [" + ((List<ItemQtdeTeorica>)itens.Value).Count + "] papéis");
                    foreach (ItemQtdeTeorica item in (List<ItemQtdeTeorica>)itens.Value)
                        logger.Info("Papel[" + item.papel + "] qtd teorica[" + item.qtdeTeorica + "] valor[" + item.valor + "] fechamento[" + item.fechamento + "] dataCotacao[" + item.dataCotacao + "]");
                }

                logger.Info("*** Composição dos Índices carregados!");
            }
        }

        public void RecalcularIndice(string instrumento, string mensagem)
        {
            if (EfetuarCalculosIndice)
            {
                // Tratar apenas cotações existentes nos papéis dos Índices
                if (hstPapeis.Contains(instrumento))
                {
                    // Usar o índice IBOV para definir o momento do recarregamento de composição ou fechamento dos índices
                    if (instrumento.Equals(INDICE_IBOV))
                    {
                        string dataMsg = mensagem.Substring(41, 14);
                        DateTime dataCotacao = new DateTime(
                            int.Parse(dataMsg.Substring(0, 4)), 
                            int.Parse(dataMsg.Substring(4, 2)),
                            int.Parse(dataMsg.Substring(6, 2)), 
                            int.Parse(dataMsg.Substring(8, 2)), 
                            int.Parse(dataMsg.Substring(10, 2)), 
                            int.Parse(dataMsg.Substring(12, 2)));
                        int status = int.Parse(mensagem.Substring(155, 1));

                        // Se serviço ativado ou cotação já é do dia seguinte, recarrega a composição dos Índices
                        if (dataCotacaoIBOV.Equals(DateTime.MinValue) || dataCotacao.Date > dataCotacaoIBOV.Date)
                        {
                            statusIBOV = status;
                            dataCotacaoIBOV = dataCotacao;
                            logger.Info("Iniciando dia de negociações com dataCotação[" + dataCotacaoIBOV + "] e status[" + statusIBOV + "]");
                            RecarregarComposicaoIndices();
                        }

                        // Se cotação do IBOV não está mais em negociação, de um mesmo dia, grava fechamento dos Índices no SQL
                        else if (statusIBOV == 2 && status == 0 && dataCotacao.Date == dataCotacaoIBOV.Date)
                        {
                            statusIBOV = status;
                            DCotacoes _DCotacoes = new DCotacoes();
                            IDictionaryEnumerator itensIndice = hstIndices.GetEnumerator();
                            while (itensIndice.MoveNext())
                            {
                                logger.Info("Gravando Índice[" + ((ItemIndice)itensIndice.Value).indice + "] na tabela TB_ATIVO");
                                _DCotacoes.AtualizarTbAtivo((ItemIndice)itensIndice.Value);
                                logger.Info("Gravando Índice[" + ((ItemIndice)itensIndice.Value).indice + "] na tabela TB_HISTORICO_INDICE");
                                _DCotacoes.InserirTbHistoricoAtivo((ItemIndice)itensIndice.Value);
                            }
                        }
                    }
                    else
                    {
                        // Tratar apenas cotações de papéis em negociação
                        if (mensagem.Substring(155, 1).Equals("2"))
                        {
                            // Efetua o recálculo para todos os Índices onde o papel existe
                            foreach (string itemIndice in (List<string>)hstPapeis[instrumento])
                            {
                                double valorIndice = 0;
                                DateTime data = DateTime.Now;

                                foreach (ItemQtdeTeorica itemQtdeTeorica in (List<ItemQtdeTeorica>)hstComposicaoIndice[itemIndice])
                                {
                                    if (itemQtdeTeorica.papel.Equals(instrumento))
                                    {
                                        string dataMsg = mensagem.Substring(41, 14);
                                        data = new DateTime(
                                            int.Parse(dataMsg.Substring(0, 4)),
                                            int.Parse(dataMsg.Substring(4, 2)),
                                            int.Parse(dataMsg.Substring(6, 2)),
                                            int.Parse(dataMsg.Substring(8, 2)),
                                            int.Parse(dataMsg.Substring(10, 2)),
                                            int.Parse(dataMsg.Substring(12, 2)));
                                        double valor = Convert.ToDouble(mensagem.Substring(74, 13), ciBR);
                                        logger.Debug("Atualizando Cotação do papel[" + instrumento + "]: DataHora[" + data + "] valor[" + valor + "]");
                                        itemQtdeTeorica.dataCotacao = data;
                                        itemQtdeTeorica.valor = valor;
                                    }

                                    if (itemQtdeTeorica.dataCotacao.Date >= dataCotacaoIBOV.Date)
                                    {
                                        valorIndice += itemQtdeTeorica.qtdeTeorica * itemQtdeTeorica.valor;
                                        logger.Debug("Acumula para papel[" + itemQtdeTeorica.papel + "] o valor     [" + itemQtdeTeorica.valor + "]");
                                    }
                                    else
                                    {
                                        valorIndice += itemQtdeTeorica.qtdeTeorica * itemQtdeTeorica.fechamento;
                                        logger.Debug("Acumula para papel[" + itemQtdeTeorica.papel + "] o fechamento[" + itemQtdeTeorica.fechamento + "]");
                                    }
                                }

                                double oscilacao = 0;
                                double fechamento = ((ItemIndice)hstIndices[itemIndice]).fechamento;
                                if (fechamento != 0)
                                    oscilacao = ((valorIndice / fechamento) - 1) * 100;
                                logger.Debug("Indice[" + itemIndice + "]: Valor[" + valorIndice + "] Oscilacao[" + oscilacao + "]");

                                string mensagemIndice = "NEBV";                // Tipo msg + bolsa
                                mensagemIndice += "00000000000000000";         // Dthr
                                mensagemIndice += itemIndice.PadRight(20);     // Instrumento
                                mensagemIndice += mensagem.Substring(41, 8);   // Data
                                mensagemIndice += mensagem.Substring(49, 9);   // Hora
                                mensagemIndice += "0".PadLeft(8, '0');          // Corretora compradora
                                mensagemIndice += "0".PadLeft(8, '0');          // Corretora vendedora
                                mensagemIndice += valorIndice.ToString("0.00", ciBR).PadLeft(13, '0'); // Preco
                                mensagemIndice += "0".PadLeft(12, '0');         // Quantidade 
                                mensagemIndice += "0".PadLeft(9, '0') + ",000"; // Max 
                                mensagemIndice += "0".PadLeft(9, '0') + ",000"; // Min
                                mensagemIndice += "0".PadLeft(9, '0') + ",000"; // Volume
                                mensagemIndice += "1".PadLeft(8, '0');          // Num Neg
                                mensagemIndice += (oscilacao < 0) ? "-" : " ";  // Indic. Variacao
                                mensagemIndice += Math.Abs(oscilacao).ToString("0.00", ciBR).PadLeft(8, '0'); // Variacao
                                mensagemIndice += "2";                         // "2" - negociado
                                mensagemIndice += "0".PadLeft(12, '0');        // Qtde compra
                                mensagemIndice += "0".PadLeft(12, '0');        // Qtde venda

                                ((ItemIndice)hstIndices[itemIndice]).valor = valorIndice;
                                ((ItemIndice)hstIndices[itemIndice]).oscilacao = oscilacao;
                                ((ItemIndice)hstIndices[itemIndice]).dataCotacao = data;

                                List<string> lstCotacoes;

                                lstCotacoes = MemoriaCotacao.hstCotacoes.GetOrAdd(itemIndice, new List<string>());
                                lstCotacoes.Add(mensagemIndice);
                                logger.Debug("Mensagem indice[" + mensagemIndice + "]");
                                //lock (MemoriaCotacao.hstCotacoes)
                                //{
                                //    if (MemoriaCotacao.hstCotacoes[itemIndice] == null)
                                //    {
                                //        MemoriaCotacao.hstCotacoes.Add(itemIndice, new List<string>());
                                //    }

                                //    List<string> cotacoes = (List<string>)(MemoriaCotacao.hstCotacoes[itemIndice]);
                                //    cotacoes.Add(mensagemIndice);
                                //    logger.Debug("Mensagem indice[" + mensagemIndice + "]");
                                //}
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
