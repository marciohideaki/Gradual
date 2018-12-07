using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using System.Globalization;

namespace Gradual.Site.Www.Transporte
{
    public class TransporteMovimentoOperacoes
    {
        #region Propriedades
        public int IdProduto { get; set; }

        public int IdMovimento { get; set; }

        public int CodCliente { get; set; }

        public string TipoOperacao { get; set; }

        public string NomeCliente { get; set; }

        public string NomeFundo { get; set; }

        public string Casa { get; set; }

        public decimal SaldoAtual { get; set; }

        public decimal ValorSolicitado { get; set; }

        public decimal SaldoCC { get; set; }

        public string Status { get; set; }

        public string DtAgendamento { get; set; }

        public string CaminhoArquivo { get; set; }

        public DateTime DtHrSolicitacaoDateTime { get; set; }

        public string DtHrSolicitacao { get; set; }

        public string Observacoes { get; set; }

        public string MotivoStatus { get; set; }
        #endregion

        public TransporteMovimentoOperacoes()
        {

        }

        public TransporteMovimentoOperacoes(IntegracaoFundosMovimentoOperacoesInfo pOperacao)
        {
            this.Casa                    = pOperacao.Casa;
            this.CodCliente              = pOperacao.CodCliente;
            this.IdMovimento             = pOperacao.IdMovimento;
            this.IdProduto               = pOperacao.Fundo.IdProduto;
            this.NomeCliente             = pOperacao.NomeCliente;
            this.NomeFundo               = pOperacao.Fundo.NomeProduto;
            this.CaminhoArquivo          = pOperacao.Fundo.NomeArquivoProspecto;
            this.SaldoAtual              = pOperacao.SaldoAtual;
            this.SaldoCC                 = pOperacao.SaldoCC;
            this.Status                  = Convert.ToString(pOperacao.Status);
            this.TipoOperacao            = pOperacao.TipoOperacao.ToString();
            this.ValorSolicitado         = pOperacao.ValorSolicitado;
            this.DtAgendamento           = pOperacao.DataAgendamento.Value.ToString("dd/MM/yyyy") != "01/01/0001" ? pOperacao.DataAgendamento.Value.ToString("dd/MM/yyyy") : string.Empty;
            this.DtHrSolicitacaoDateTime = pOperacao.DtHrSolicitacao;//.ToString("dd/MM/yyyy HH:mm");
            this.DtHrSolicitacao         = pOperacao.DtHrSolicitacao.ToString("dd/MM/yyyy HH:mm");
            this.Observacoes             = pOperacao.Observacoes;
            this.MotivoStatus            = pOperacao.MotivoStatus;
        }

        

        public List<TransporteMovimentoOperacoes> TraduzirLista(List<IntegracaoFundosMovimentoOperacoesInfo> pListaOperacoes, 
            decimal lSaldoCC, 
            List<Transporte_PosicaoCotista> PosicaoCotista)
        {
            var lRetorno = new List<TransporteMovimentoOperacoes>();

            Dictionary<string,decimal> lPosicao = new Dictionary<string,decimal>();

            foreach (Transporte_PosicaoCotista pos in PosicaoCotista)
            {
                decimal lValorLiquido = decimal.Parse(pos.ValorLiquido, new CultureInfo("pt-BR"));

                if (lPosicao.ContainsKey(pos.CodigoAnbima))
                {
                    lPosicao[pos.CodigoAnbima] += lValorLiquido;
                }
                else
                {
                    lPosicao.Add(pos.CodigoAnbima, lValorLiquido);
                }
            }

            pListaOperacoes.ForEach(operacao =>
            {
                var lOperacao = new TransporteMovimentoOperacoes();

                lOperacao.Casa                    = operacao.Casa;
                lOperacao.CodCliente              = operacao.CodCliente;
                lOperacao.IdMovimento             = operacao.IdMovimento;
                lOperacao.IdProduto               = operacao.Fundo.IdProduto;
                lOperacao.NomeCliente             = operacao.NomeCliente;
                lOperacao.NomeFundo               = operacao.Fundo.NomeProduto;
                lOperacao.CaminhoArquivo          = operacao.Fundo.NomeArquivoProspecto;
                lOperacao.SaldoAtual              = lPosicao.ContainsKey(operacao.CodigoAnbima)? lPosicao[operacao.CodigoAnbima].DBToDecimal() :0 ;
                lOperacao.SaldoCC                 = lSaldoCC;
                lOperacao.Status                  = Convert.ToString(operacao.Status);
                lOperacao.TipoOperacao            = operacao.TipoOperacao.ToString();
                lOperacao.ValorSolicitado         = operacao.ValorSolicitado;
                lOperacao.DtAgendamento           = operacao.DataAgendamento.Value.ToString("dd/MM/yyyy") != "01/01/0001" ? operacao.DataAgendamento.Value.ToString("dd/MM/yyyy") : string.Empty;
                lOperacao.DtHrSolicitacaoDateTime = operacao.DtHrSolicitacao;//.ToString("dd/MM/yyyy HH:mm");
                lOperacao.DtHrSolicitacao         = operacao.DtHrSolicitacao.ToString("dd/MM/yyyy HH:mm");
                lOperacao.Observacoes             = operacao.Observacoes;
                lOperacao.MotivoStatus            = operacao.MotivoStatus;

                lRetorno.Add(lOperacao);
            });

            return lRetorno;
        }
    }
}