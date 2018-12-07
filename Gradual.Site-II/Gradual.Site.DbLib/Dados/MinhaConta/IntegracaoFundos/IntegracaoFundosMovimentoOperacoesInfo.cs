using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosMovimentoOperacoesInfo
    {
        public IntegracaoFundosMovimentoOperacoesInfo()
        {
            Fundo = new IntegracaoFundosInfo();
        }

        public string CodigoAnbima                    { get; set; }

        public Nullable<DateTime>   DataAgendamento   { get; set; }

        public int                  CodCliente        { get; set; }

        public string               NomeCliente       { get; set; }

        public int                  IdMovimento       { get; set; }

        public IntegracaoFundosInfo Fundo             { get; set; }

        public string               Casa              { get; set; }

        public decimal              SaldoAtual        { get; set; }

        public decimal              ValorSolicitado   { get; set; }

        public decimal              SaldoCC           { get; set; }

        public IntegracaoFundosTipoOperacaoEnum TipoOperacao { get; set; }

        public IntegracaoFundosStatusOperacaoEnum Status { get; set; }

        public DateTime             DtHrSolicitacao   { get; set; }

        public string               Observacoes       { get; set; }

        public string               MotivoStatus      { get; set; }
    }
}
