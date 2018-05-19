/*****************************************************************************
 MainModule...: Acompanhamento
 SubModule....:
 Author.......: Hideaki
 Date.........: 01/10/2013
 Porpouse.....: 

 Modifications:
 Author               Date       Reason
 -------------------- ---------- ---------------------------------------------
 *****************************************************************************/
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace StockMarket.Excel2007.Classes
{
    public enum TipoSubstituicao
    {
        INALTERADA = 0,
        PRECO_QUANTIDADE = 1,
        QUANTIDADE = 2,
        PRECO = 3

    }

    public class Acompanhamento
    {
        private System.String instrumento;
        public System.String Instrumento
        {
            get
            {
                return this.instrumento;
            }
            set
            {
                this.instrumento = value;
                if (this.Posicao != null)
                {
                    this.Posicao.Instrumento = value;
                }
            }
        }

        public Dictionary<System.String, AcompanhamentoOrdemInfo> Ocorrencias { get; set; }
        public Posicao Posicao { get; set; }

        public Acompanhamento()
        {
            try
            {
                this.Instrumento = String.Empty;
                this.Ocorrencias = new Dictionary<string, AcompanhamentoOrdemInfo>();
                this.Posicao = new Posicao();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.Acompanhamento(): {0}", ex.Message));
            }
        }

        /// <summary>
        ///     Adiciona a ordem a lista de ordens negociadas no dia do cliente e instrumento discriminados separando os acompanhamentos relevantes a posição
        /// </summary>
        /// <param name="Ordem">Ordem recebida pela bolsa</param>
        /// <returns>Posição do cliente já calculada com base na ordem recebida</returns>
        public Posicao Add(OrdemInfo Ordem)
        {
            try
            {
                // Separa os acompanhamento relevantes
                var Ocorrencias = Ordem.Acompanhamentos.AsEnumerable();
                Ocorrencias =
                    from a in Ocorrencias
                    where
                    a.StatusOrdem.Equals(OrdemStatusEnum.NOVA) ||
                    a.StatusOrdem.Equals(OrdemStatusEnum.SUBSTITUIDA) ||
                    a.StatusOrdem.Equals(OrdemStatusEnum.PARCIALMENTEEXECUTADA) ||
                    a.StatusOrdem.Equals(OrdemStatusEnum.EXECUTADA) ||
                    a.StatusOrdem.Equals(OrdemStatusEnum.CANCELADA)
                    select a;

                // Verifica todos osacompanhamentos para identificar os que ainda não foram processados
                foreach (AcompanhamentoOrdemInfo acompanhamento in Ocorrencias)
                {
                    // Verifica se o acompanhamento em questão já foi processado através da chave composta pelo Código de Resposta da Bolsa e posição do acompanhamento dentro da ordem
                    if (!this.Ocorrencias.ContainsKey(String.Format("{0}:{1}", acompanhamento.CodigoResposta, Ordem.Acompanhamentos.IndexOf(acompanhamento).ToString())))
                    {
                        // Caso oa companhamento não tenha sido processado
                        this.Ocorrencias.Add(
                            String.Format("{0}:{1}", acompanhamento.CodigoResposta, Ordem.Acompanhamentos.IndexOf(acompanhamento).ToString()),
                            ((AcompanhamentoOrdemInfo)acompanhamento)
                        );

                        // Processa e efetua os calculos da posição com base no acompanhamento recem recebido
                        Calcular(((AcompanhamentoOrdemInfo)acompanhamento));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.Add(): {0}", ex.Message));
            }

            // Retorna a posição já calculada do cliente para o instrumento discriminado
            return this.Posicao;
        }

        /// <summary>
        ///     Calcula a posição do cliente
        /// </summary>
        /// <param name="Acompanhamento">Acompanhamento da ordem a ser utilizado para o calculo</param>
        private void Calcular(AcompanhamentoOrdemInfo Acompanhamento)
        {
            try
            {
                switch (Acompanhamento.StatusOrdem)
                {
                    case OrdemStatusEnum.NOVA:
                        Calcular_NOVA(Acompanhamento);
                        break;
                    case OrdemStatusEnum.SUBSTITUIDA:
                        Calcular_SUBSTITUIDA(Acompanhamento);
                        break;
                    case OrdemStatusEnum.PARCIALMENTEEXECUTADA:
                        Calcular_PARCIALMENTEEXECUTADA(Acompanhamento);
                        break;
                    case OrdemStatusEnum.EXECUTADA:
                        Calcular_EXECUTADA(Acompanhamento);
                        break;
                    case OrdemStatusEnum.CANCELADA:
                        Calcular_CANCELADA(Acompanhamento);
                        break;
                }

                CalcularPrecoExecucao(Acompanhamento);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.Calcular(): {0}", ex.Message));
            }
        }

        /// <summary>
        ///     Efetua os calculos com base no acompanhamento da ordem no momento da abertura da mesma na bolsa
        /// </summary>
        /// <param name="Acompanhamento">Acompanhamento de ordem base do calculo</param>
        private void Calcular_NOVA(AcompanhamentoOrdemInfo Acompanhamento)
        {
            try
            {
                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra))
                {
                    this.Posicao.QuantidadeAbertaCompra += Acompanhamento.QuantidadeSolicitada;
                    this.Posicao.FinanceiroTotalAbertaCompra += Acompanhamento.Preco * Acompanhamento.QuantidadeSolicitada;
                }

                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda))
                {
                    this.Posicao.QuantidadeAbertaVenda += Acompanhamento.QuantidadeSolicitada;
                    this.Posicao.FinanceiroTotalAbertaVenda += Acompanhamento.Preco * Acompanhamento.QuantidadeSolicitada;
                }

                this.Posicao.FinanceiroNetAbertas = this.Posicao.FinanceiroTotalAbertaVenda - this.Posicao.FinanceiroTotalAbertaCompra;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.Calcular_NOVA(): {0}", ex.Message));
            }
        }

        /// <summary>
        ///     Efetua os calculos com base no acompanhamento da ordem no momento da alteração da mesma na bolsa
        /// </summary>
        /// <param name="Acompanhamento">Acompanhamento de ordem base do calculo</param>
        private void Calcular_SUBSTITUIDA(AcompanhamentoOrdemInfo Acompanhamento)
        {
            //TODO: mudar o controle para carregar os acompanhamentos pois quando se trata de uma ordem VAC o CodigoResposta troca
            TipoSubstituicao Tipo = TipoSubstituicao.INALTERADA;

            try
            {
                var Ocorrencias = this.Ocorrencias.Values.AsEnumerable();
                var Ocorrencias_TODAS =
                    from a in Ocorrencias
                    where
                        a.CodigoResposta.Equals(Acompanhamento.CodigoResposta)
                    select a;

                var PenultimaOcorrencia = Ocorrencias_TODAS.ToList()[Ocorrencias_TODAS.Count() - 2];

                if (!PenultimaOcorrencia.Preco.Equals(Acompanhamento.Preco))
                {
                    Tipo = TipoSubstituicao.PRECO;
                    //Aplicacao.GravarLog("Acompanhamento.Calcular_SUBSTITUIDA()", String.Format("Ordem: {0} - Substituida por alteração de preço", Acompanhamento.NumeroControleOrdem.ToString()));
                }

                if (!PenultimaOcorrencia.QuantidadeRemanescente.Equals(Acompanhamento.QuantidadeRemanescente))
                {
                    Tipo = TipoSubstituicao.QUANTIDADE;
                    //Aplicacao.GravarLog("Acompanhamento.Calcular_SUBSTITUIDA()", String.Format("Ordem: {0} - Substituida por alteração de quantidade", Acompanhamento.NumeroControleOrdem.ToString()));
                }

                if (!PenultimaOcorrencia.Preco.Equals(Acompanhamento.Preco) && !PenultimaOcorrencia.QuantidadeRemanescente.Equals(Acompanhamento.QuantidadeRemanescente))
                {
                    Tipo = TipoSubstituicao.PRECO_QUANTIDADE;
                    //Aplicacao.GravarLog("Acompanhamento.Calcular_SUBSTITUIDA()", String.Format("Ordem: {0} - Substituida por alteração de quantidade e preço", Acompanhamento.NumeroControleOrdem.ToString()));
                }

                CalcularSubstituicao(Tipo, PenultimaOcorrencia, Acompanhamento);

                this.Posicao.FinanceiroNetAbertas = this.Posicao.FinanceiroTotalAbertaVenda - this.Posicao.FinanceiroTotalAbertaCompra;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.Calcular_SUBSTITUIDA(): {0}", ex.Message));
            }
        }

        private void CalcularSubstituicao(TipoSubstituicao Tipo, AcompanhamentoOrdemInfo UltimaOcorrenciaHistorica, AcompanhamentoOrdemInfo UltimaOcorrencia)
        {
            try
            {
                switch (Tipo)
                {
                    case TipoSubstituicao.PRECO:

                        if (UltimaOcorrencia.Direcao.Equals(OrdemDirecaoEnum.Compra))
                        {
                            this.Posicao.FinanceiroTotalAbertaCompra -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente * UltimaOcorrenciaHistorica.Preco);
                            this.Posicao.FinanceiroTotalAbertaCompra += (UltimaOcorrencia.QuantidadeRemanescente * UltimaOcorrencia.Preco);
                        }

                        if (UltimaOcorrencia.Direcao.Equals(OrdemDirecaoEnum.Venda))
                        {
                            this.Posicao.FinanceiroTotalAbertaVenda -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente * UltimaOcorrenciaHistorica.Preco);
                            this.Posicao.FinanceiroTotalAbertaVenda += (UltimaOcorrencia.QuantidadeRemanescente * UltimaOcorrencia.Preco);
                        }

                        break;
                    case TipoSubstituicao.QUANTIDADE:
                        if (UltimaOcorrencia.Direcao.Equals(OrdemDirecaoEnum.Compra))
                        {
                            this.Posicao.QuantidadeAbertaCompra -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente);
                            this.Posicao.QuantidadeAbertaCompra += (UltimaOcorrencia.QuantidadeRemanescente);
                            this.Posicao.FinanceiroTotalAbertaCompra -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente * UltimaOcorrenciaHistorica.Preco);
                            this.Posicao.FinanceiroTotalAbertaCompra += (UltimaOcorrencia.QuantidadeRemanescente * UltimaOcorrencia.Preco);
                        }
                        if (UltimaOcorrencia.Direcao.Equals(OrdemDirecaoEnum.Venda))
                        {
                            this.Posicao.QuantidadeAbertaVenda -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente);
                            this.Posicao.QuantidadeAbertaVenda += (UltimaOcorrencia.QuantidadeRemanescente);
                            this.Posicao.FinanceiroTotalAbertaVenda -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente * UltimaOcorrenciaHistorica.Preco);
                            this.Posicao.FinanceiroTotalAbertaVenda += (UltimaOcorrencia.QuantidadeRemanescente * UltimaOcorrencia.Preco);
                        }
                        break;
                    case TipoSubstituicao.PRECO_QUANTIDADE:
                        if (UltimaOcorrencia.Direcao.Equals(OrdemDirecaoEnum.Compra))
                        {
                            this.Posicao.FinanceiroTotalAbertaCompra -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente * UltimaOcorrenciaHistorica.Preco);
                            this.Posicao.FinanceiroTotalAbertaCompra += (UltimaOcorrencia.QuantidadeRemanescente * UltimaOcorrencia.Preco);
                            this.Posicao.QuantidadeAbertaCompra -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente);
                            this.Posicao.QuantidadeAbertaCompra += (UltimaOcorrencia.QuantidadeRemanescente);
                        }
                        if (UltimaOcorrencia.Direcao.Equals(OrdemDirecaoEnum.Venda))
                        {
                            this.Posicao.FinanceiroTotalAbertaVenda -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente * UltimaOcorrenciaHistorica.Preco);
                            this.Posicao.FinanceiroTotalAbertaVenda += (UltimaOcorrencia.QuantidadeRemanescente * UltimaOcorrencia.Preco);
                            this.Posicao.QuantidadeAbertaVenda -= (UltimaOcorrenciaHistorica.QuantidadeRemanescente);
                            this.Posicao.QuantidadeAbertaVenda += (UltimaOcorrencia.QuantidadeRemanescente);
                        }
                        break;

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.CalcularSubstituicao(): {0}", ex.Message));
            }
        }

        /// <summary>
        ///     Efetua os calculos com base no acompanhamento da ordem no momento da execucao parcial da mesma na bolsa
        /// </summary>
        /// <param name="Acompanhamento">Acompanhamento de ordem base do calculo</param>
        private void Calcular_PARCIALMENTEEXECUTADA(AcompanhamentoOrdemInfo Acompanhamento)
        {
            try
            {
                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra))
                {
                    this.Posicao.QuantidadeExecutadaCompra += Acompanhamento.QuantidadeNegociada;
                    this.Posicao.QuantidadeAbertaCompra -= Acompanhamento.QuantidadeNegociada;
                    this.Posicao.FinanceiroTotalExecutadaCompra += Acompanhamento.LastPx * Acompanhamento.QuantidadeNegociada;
                    this.Posicao.FinanceiroTotalAbertaCompra -= Acompanhamento.Preco * Acompanhamento.QuantidadeNegociada;
                }

                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda))
                {
                    this.Posicao.QuantidadeExecutadaVenda += Acompanhamento.QuantidadeNegociada;
                    this.Posicao.QuantidadeAbertaVenda -= Acompanhamento.QuantidadeNegociada;
                    this.Posicao.FinanceiroTotalExecutadaVenda += Acompanhamento.LastPx * Acompanhamento.QuantidadeNegociada;
                    this.Posicao.FinanceiroTotalAbertaVenda -= Acompanhamento.Preco * Acompanhamento.QuantidadeNegociada;
                }

                this.Posicao.FinanceiroNetAbertas = this.Posicao.FinanceiroTotalAbertaVenda - this.Posicao.FinanceiroTotalAbertaCompra;
                this.Posicao.FinanceiroNetExecutadas = this.Posicao.FinanceiroTotalExecutadaVenda - this.Posicao.FinanceiroTotalExecutadaCompra;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.Calcular_PARCIALMENTEEXECUTADA(): {0}", ex.Message));
            }
        }

        /// <summary>
        ///     Efetua os calculos com base no acompanhamento da ordem no momento da execucao da mesma na bolsa
        /// </summary>
        /// <param name="Acompanhamento">Acompanhamento de ordem base do calculo</param>
        private void Calcular_EXECUTADA(AcompanhamentoOrdemInfo Acompanhamento)
        {
            try
            {
                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra))
                {
                    this.Posicao.QuantidadeExecutadaCompra += Acompanhamento.QuantidadeNegociada;
                    this.Posicao.QuantidadeAbertaCompra -= Acompanhamento.QuantidadeNegociada;
                    this.Posicao.FinanceiroTotalExecutadaCompra += Acompanhamento.LastPx * Acompanhamento.QuantidadeNegociada;
                    this.Posicao.FinanceiroTotalAbertaCompra -= Acompanhamento.Preco * Acompanhamento.QuantidadeNegociada;
                }

                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda))
                {
                    this.Posicao.QuantidadeExecutadaVenda += Acompanhamento.QuantidadeNegociada;
                    this.Posicao.QuantidadeAbertaVenda -= Acompanhamento.QuantidadeNegociada;
                    this.Posicao.FinanceiroTotalExecutadaVenda += Acompanhamento.LastPx * Acompanhamento.QuantidadeNegociada;
                    this.Posicao.FinanceiroTotalAbertaVenda -= Acompanhamento.Preco * Acompanhamento.QuantidadeNegociada;
                }

                if (this.Posicao.QuantidadeAbertaCompra.Equals(0))
                {
                    this.Posicao.FinanceiroTotalAbertaCompra = 0;
                }

                if (this.Posicao.QuantidadeAbertaVenda.Equals(0))
                {
                    this.Posicao.FinanceiroTotalAbertaVenda = 0;
                }

                this.Posicao.FinanceiroNetAbertas = this.Posicao.FinanceiroTotalAbertaVenda - this.Posicao.FinanceiroTotalAbertaCompra;
                this.Posicao.FinanceiroNetExecutadas = this.Posicao.FinanceiroTotalExecutadaVenda - this.Posicao.FinanceiroTotalExecutadaCompra;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.Calcular_EXECUTADA(): {0}", ex.Message));
            }
        }

        /// <summary>
        ///     Efetua os calculos com base no acompanhamento da ordem no momento do cancelamento da mesma na bolsa
        /// </summary>
        /// <param name="Acompanhamento">Acompanhamento de ordem base do calculo</param>
        private void Calcular_CANCELADA(AcompanhamentoOrdemInfo Acompanhamento)
        {
            try
            {
                // Separa apenas a ocorrência com status nova, para que os valores acumulados possam ser zerados a posição anterior a substituição da ordem
                var Ocorrencias = this.Ocorrencias.Values.AsEnumerable();
                var Ocorrencias_TODAS =
                    from a in Ocorrencias
                    where
                        a.CodigoResposta.Equals(Acompanhamento.CodigoResposta)
                    select a;

                var PenultimaOcorrencia = Ocorrencias_TODAS.ToList()[Ocorrencias_TODAS.Count() - 2];

                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra))
                {
                    this.Posicao.QuantidadeAbertaCompra -= PenultimaOcorrencia.QuantidadeRemanescente;
                    this.Posicao.FinanceiroTotalAbertaCompra -= (PenultimaOcorrencia.QuantidadeRemanescente * Acompanhamento.LastPx);
                }

                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda))
                {
                    this.Posicao.QuantidadeAbertaVenda -= PenultimaOcorrencia.QuantidadeRemanescente;
                    this.Posicao.FinanceiroTotalAbertaVenda -= (PenultimaOcorrencia.QuantidadeRemanescente * Acompanhamento.LastPx);
                }

                if (this.Posicao.QuantidadeAbertaCompra.Equals(0))
                {
                    this.Posicao.FinanceiroTotalAbertaCompra = 0;
                }

                if (this.Posicao.QuantidadeAbertaVenda.Equals(0))
                {
                    this.Posicao.FinanceiroTotalAbertaVenda = 0;
                }

                this.Posicao.FinanceiroNetAbertas = this.Posicao.FinanceiroTotalAbertaVenda - this.Posicao.FinanceiroTotalAbertaCompra;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.Calcular_CANCELADA(): {0}", ex.Message));
            }
        }

        /// <summary>
        ///     Efetua o calculo do preco medio de execução com base no acompanhamento recebido agregado às execuções efetuadas anteriormente, contemplando o valor em aberto
        /// </summary>
        /// <param name="Acompanhamento">Acompanhamento de ordem base do calculo</param>
        private void CalcularPrecoExecucao(AcompanhamentoOrdemInfo Acompanhamento)
        {
            try
            {
                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra))
                {
                    if (!Acompanhamento.StatusOrdem.Equals(OrdemStatusEnum.CANCELADA))
                    {
                        if (this.Posicao.PrecoMedioCompra.Equals(0))
                        {
                            this.Posicao.PrecoMedioCompra = Acompanhamento.LastPx;
                        }
                        else
                        {
                            if (!this.Posicao.QuantidadeAbertaCompra.Equals(0) || !this.Posicao.QuantidadeExecutadaCompra.Equals(0))
                            {
                                this.Posicao.PrecoMedioCompra = (this.Posicao.FinanceiroTotalAbertaCompra + this.Posicao.FinanceiroTotalExecutadaCompra) / (this.Posicao.QuantidadeAbertaCompra + this.Posicao.QuantidadeExecutadaCompra);
                            }

                            if (this.Posicao.QuantidadeAbertaCompra.Equals(0))
                            {
                                if (!this.Posicao.QuantidadeExecutadaCompra.Equals(0))
                                {
                                    this.Posicao.PrecoMedioCompra = (this.Posicao.FinanceiroTotalExecutadaCompra) / (this.Posicao.QuantidadeExecutadaCompra);
                                }
                            }

                            if (this.Posicao.QuantidadeExecutadaCompra.Equals(0))
                            {
                                if (!this.Posicao.QuantidadeAbertaCompra.Equals(0))
                                {
                                    this.Posicao.PrecoMedioCompra = (this.Posicao.FinanceiroTotalAbertaCompra) / (this.Posicao.QuantidadeAbertaCompra);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.Posicao.QuantidadeAbertaCompra.Equals(0) && this.Posicao.QuantidadeExecutadaCompra.Equals(0))
                        {
                            this.Posicao.PrecoMedioCompra = 0;
                        }

                        if (this.Posicao.QuantidadeAbertaCompra.Equals(0))
                        {
                            if (!this.Posicao.QuantidadeExecutadaCompra.Equals(0))
                            {
                                this.Posicao.PrecoMedioCompra = (this.Posicao.FinanceiroTotalExecutadaCompra) / (this.Posicao.QuantidadeExecutadaCompra);
                            }
                        }

                        if (this.Posicao.QuantidadeExecutadaCompra.Equals(0))
                        {
                            if (!this.Posicao.QuantidadeAbertaCompra.Equals(0))
                            {
                                this.Posicao.PrecoMedioCompra = (this.Posicao.FinanceiroTotalAbertaCompra) / (this.Posicao.QuantidadeAbertaCompra);
                            }
                        }
                    }
                }

                if (Acompanhamento.Direcao.Equals(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda))
                {
                    if (!Acompanhamento.StatusOrdem.Equals(OrdemStatusEnum.CANCELADA))
                    {
                        if (this.Posicao.PrecoMedioVenda.Equals(0))
                        {
                            this.Posicao.PrecoMedioVenda = Acompanhamento.LastPx;
                        }
                        else
                        {
                            if (!this.Posicao.QuantidadeAbertaVenda.Equals(0) || !this.Posicao.QuantidadeExecutadaVenda.Equals(0))
                            {
                                this.Posicao.PrecoMedioVenda = (this.Posicao.FinanceiroTotalAbertaVenda + this.Posicao.FinanceiroTotalExecutadaVenda) / (this.Posicao.QuantidadeAbertaVenda + this.Posicao.QuantidadeExecutadaVenda);
                            }

                            if (this.Posicao.QuantidadeAbertaVenda.Equals(0))
                            {
                                if (!this.Posicao.QuantidadeExecutadaVenda.Equals(0))
                                {
                                    this.Posicao.PrecoMedioVenda = (this.Posicao.FinanceiroTotalExecutadaVenda) / (this.Posicao.QuantidadeExecutadaVenda);
                                }
                            }

                            if (this.Posicao.QuantidadeExecutadaVenda.Equals(0))
                            {
                                if (!this.Posicao.QuantidadeAbertaVenda.Equals(0))
                                {
                                    this.Posicao.PrecoMedioVenda = (this.Posicao.FinanceiroTotalAbertaVenda) / (this.Posicao.QuantidadeAbertaVenda);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.Posicao.QuantidadeAbertaVenda.Equals(0) && this.Posicao.QuantidadeExecutadaVenda.Equals(0))
                        {
                            this.Posicao.PrecoMedioVenda = 0;
                        }

                        if (this.Posicao.QuantidadeAbertaVenda.Equals(0))
                        {
                            if (!this.Posicao.QuantidadeExecutadaVenda.Equals(0))
                            {
                                this.Posicao.PrecoMedioVenda = (this.Posicao.FinanceiroTotalExecutadaVenda) / (this.Posicao.QuantidadeExecutadaVenda);
                            }
                        }

                        if (this.Posicao.QuantidadeExecutadaVenda.Equals(0))
                        {
                            if (!this.Posicao.QuantidadeAbertaVenda.Equals(0))
                            {
                                this.Posicao.PrecoMedioVenda = (this.Posicao.FinanceiroTotalAbertaVenda) / (this.Posicao.QuantidadeAbertaVenda);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Acompanhamento.CalcularPrecoExecucao(): {0}", ex.Message));
            }
        }
    }
}
