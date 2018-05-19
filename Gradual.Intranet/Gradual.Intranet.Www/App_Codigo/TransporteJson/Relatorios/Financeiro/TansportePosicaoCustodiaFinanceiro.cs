using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.ContaCorrente.Lib.Info;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro
{
    public class TransporteFinaceiroExtratoMovimento
    {
        
        public string DataLiquidacao { get; set; }

        public string DataMovimento { get; set; }
        
        public string Historico { get; set; }

        public string ValorCredito { get; set; }

        public string ValorDebito { get; set; }

        public string ValorSaldo { get; set; }
    }

    public class TansporteFinanceiroExtrato
    {
        public string Titulo     { get; set; }
        
        public string Dia        { get; set; }

        public string DataNegocio { get; set; }

        public string Historico   { get; set; }
        
        public string Quantidade { get; set; }
    }

    public class TansporteFinanceiroPosicao
    {
        
        #region Posicao Avista
        public string ValorAtual { get; set; }
        public string Variacao { get; set; }
        #endregion

        #region Posição Opção
        public string Titulo { get; set; }
        public string Exercicio { get; set; }
        public string Quantidade { get; set; }
        public string Custo { get; set; }
        public string ValorExercicio { get; set; }
        public string CustoTotal { get; set; }
        public string ValorObjeto { get; set; }
        #endregion

        #region Posicao Termo
        public string DataRolagem { get; set; }
        public string ValorTermo { get; set; }
        #endregion

        #region Posicao Tesouro
        public string DataVencimento { get; set; }
        public string ValorOriginal { get; set; }
        public string Tipo { get; set; }
        #endregion
    }

    public class TansportePosicaoCustodiaFinanceiro
    {
        public string SaldoAnterior    { get; set; }
        public string SaldoDisponivel  { get; set; }
        public string SaldoTotal       { get; set; }

        #region Dados Cliente
        public string NomeCliente { get; set; }
        public string CodigoCliente { get; set; }
        #endregion

        public List<TansporteFinanceiroPosicao> ListaPosicaoAVista { get; set; }
        public List<TansporteFinanceiroExtrato> ListaExtratoAVista { get; set; }
        public List<TansporteFinanceiroPosicao> ListaPosicaoTermo { get; set; }
        public List<TansporteFinanceiroExtrato> ListaExtratoTermo { get; set; }
        public List<TansporteFinanceiroPosicao> ListaPosicaoOpcao { get; set; }
        public List<TansporteFinanceiroExtrato> ListaExtratoOpcao { get; set; }
        public List<TansporteFinanceiroPosicao> ListaPosicaoTesouro { get; set; }
        
        public List<TransporteFinaceiroExtratoMovimento>    ListaExtratoMovimento         { get; set; }
        

        public TansportePosicaoCustodiaFinanceiro()
        {
            ListaExtratoAVista   = new List<TansporteFinanceiroExtrato>();
            ListaPosicaoAVista   = new List<TansporteFinanceiroPosicao>();
            ListaPosicaoTermo    = new List<TansporteFinanceiroPosicao>();
            ListaExtratoTermo    = new List<TansporteFinanceiroExtrato>();
            ListaPosicaoOpcao    = new List<TansporteFinanceiroPosicao>();
            ListaExtratoOpcao    = new List<TansporteFinanceiroExtrato>();
            ListaPosicaoTesouro  = new List<TansporteFinanceiroPosicao>();

            ListaExtratoMovimento   = new List<TransporteFinaceiroExtratoMovimento>();
        }

        public TansportePosicaoCustodiaFinanceiro TraduzirLista( ClienteCustodiaFinanceiroInfo pLista, ContaCorrenteExtratoInfo pConta)
        {
            var lRetorno = new TansportePosicaoCustodiaFinanceiro();

            lRetorno.NomeCliente   = pLista.NomeCliente;
            lRetorno.CodigoCliente = pLista.CodigoCliente.ToString();

            pLista.ListaPosicao.ForEach(posicao => 
            {
                var lTrans = new TansporteFinanceiroPosicao();

                if (posicao.Tipo.Equals("VIS"))
                {
                    lTrans.Titulo     = posicao.Titulo;
                    lTrans.Quantidade = posicao.Quantidade.ToString();
                    lTrans.Custo      = posicao.Custo.ToString("N2");
                    lTrans.ValorAtual = posicao.ValorAtual.ToString("N2");
                    lTrans.Variacao   = posicao.Variacao.ToString("N2");

                    lRetorno.ListaPosicaoAVista.Add(lTrans);
                }

                if (posicao.Tipo.Equals("TER"))
                {
                    lTrans.Titulo         = posicao.Titulo;
                    lTrans.Quantidade     = posicao.Quantidade.ToString();
                    lTrans.DataVencimento = posicao.DataVencimento.ToString("dd/MM/yyyy");
                    lTrans.DataRolagem    = posicao.DataRolagem.ToString("dd/MM/yyyy");
                    lTrans.ValorTermo     = posicao.ValorTermo.ToString("N2");

                    lRetorno.ListaPosicaoTermo.Add(lTrans);
                }

                if (posicao.Tipo.Equals("TES"))
                {
                    lTrans.Titulo         = posicao.Titulo;
                    lTrans.DataVencimento = posicao.DataVencimento.ToString("dd/MM/yyyy");
                    lTrans.Quantidade     = posicao.QuantidadeTesouro.ToString("N2");
                    lTrans.ValorOriginal  = posicao.ValorOriginal.ToString("N2");

                    lRetorno.ListaPosicaoTesouro.Add(lTrans);
                }

                if (posicao.Tipo.Equals("OPC"))
                {
                    lTrans.Titulo         = posicao.Titulo;
                    lTrans.Exercicio      = posicao.Exercicio.ToString("N2");
                    lTrans.Quantidade     = posicao.Quantidade.ToString();
                    lTrans.Custo          = posicao.Custo.ToString("N2");
                    lTrans.ValorExercicio = posicao.ValorExercicio.ToString("N2");
                    lTrans.ValorObjeto    = posicao.ValorObjeto.ToString("N2");
                    lTrans.CustoTotal     = posicao.CustoTotal.ToString("N2");

                    lRetorno.ListaPosicaoOpcao.Add(lTrans);
                }
            });

            pLista.ListaExtrato.ForEach(extrato =>
            {
                var lExtrato = new TansporteFinanceiroExtrato();

                    lExtrato.Titulo                   = extrato.Titulo;
                    lExtrato.Dia                      = extrato.Dia;
                    lExtrato.DataNegocio              = extrato.DataNegocio.ToString("dd/MM/yyyy");
                    lExtrato.Historico                = extrato.Historico;
                    lExtrato.Quantidade               = extrato.Qtde.ToString();

                if (extrato.Tipo.Equals("VIS"))
                {
                    lRetorno.ListaExtratoAVista.Add(lExtrato);
                }

                if (extrato.Tipo.Equals("TER"))
                {
                    lRetorno.ListaExtratoTermo.Add(lExtrato);
                }

                if (extrato.Tipo.Equals("OPC"))
                {
                    lRetorno.ListaExtratoOpcao.Add(lExtrato);
                }
            });

            pConta.ListaContaCorrenteMovimento.ForEach(conta =>
            {
                var lEMovimento = new TransporteFinaceiroExtratoMovimento();

                lEMovimento.DataLiquidacao = conta.DataLiquidacao.ToString("dd/MM/yyyy");
                lEMovimento.DataMovimento  = conta.DataMovimento.ToString("dd/MM/yyyy");
                lEMovimento.Historico      = conta.Historico;
                lEMovimento.ValorCredito   = conta.ValorCredito.ToString("N2");
                lEMovimento.ValorDebito    = conta.ValorDebito.ToString("N2");
                lEMovimento.ValorSaldo     = conta.ValorSaldo.ToString("N2");

                lRetorno.ListaExtratoMovimento.Add(lEMovimento);
            });

            if (pConta != null)
            {
                lRetorno.SaldoAnterior   = pConta.SaldoAnterior.ToString("N2");
                lRetorno.SaldoDisponivel = pConta.SaldoDisponivel.ToString("N2");
                lRetorno.SaldoTotal      = pConta.SaldoTotal.ToString("N2");
            }

            return lRetorno;
        }
    }
}