using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteExtratoInfo : ICodigoEntidade
    {
        public string Titulo         { get; set; }
        public string Dia         { get; set; }
        public DateTime DataNegocio         { get; set; }
        public string Historico    { get; set; }
        public int Qtde                     { get; set; }
        public string Tipo                  { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }

    public class ClientePosicaoInfo : ICodigoEntidade
    {
        #region Posicao Avista
        public decimal ValorAtual   { get; set; }
        public decimal Variacao     { get; set; }
        #endregion

        #region Posição Opção
        public string Titulo            { get; set; }
        public decimal Exercicio        { get; set; }
        public int Quantidade           { get; set; }
        public decimal Custo            { get; set; }
        public decimal ValorExercicio   { get; set; }
        public decimal CustoTotal       { get; set; }
        public decimal ValorObjeto      { get; set; }
        #endregion

        #region Posicao Termo
        public DateTime DataRolagem     { get; set; }
        public decimal ValorTermo       { get; set; }
        #endregion

        #region Posicao Tesouro
        public DateTime DataVencimento  { get; set; }
        public decimal ValorOriginal    { get; set; }
        public decimal QuantidadeTesouro { get; set; }
        public string Tipo              { get; set; }
        #endregion
        //public Nullable<int> CodigoAssessor { get; set; }
        //public string CodigoNegocio         { get; set; }
        //public DateTime DataSistema         { get; set; }
        //public DateTime DataPosicao         { get; set; }
        //public int CodigoCliente            { get; set; }
        //public string NomeCliente           { get; set; }
        //public string NomeEmpresa           { get; set; }
        //public string NumeroDistribuicao    { get; set; }
        //public string CodigoMercado         { get; set; }
        //public string QtdeDisponivel        { get; set; }
        //public decimal PrecoLiquido         { get; set; }
        //public decimal PrecoBruto           { get; set; }
        //public decimal PrecoExercicio       { get; set; }
        //public DateTime DataUltMov          { get; set; }
        //public DateTime DataAbertPosicao    { get; set; }
        //public DateTime DataVencimento      { get; set; }
        //public DateTime DataRolagem         { get; set; }
        //public int Carteira                 { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }

    public class ClienteCustodiaFinanceiroInfo : ICodigoEntidade
    {
        public DateTime De { get; set; }
        public DateTime Ate { get; set; }
        public Nullable<int> CodigoAssessor { get; set; }
        public int CodigoCliente { get; set; }
        public string NomeCliente { get; set; }
        public List<ClienteExtratoInfo> ListaExtrato                { get; set; }
        public List<ClientePosicaoInfo> ListaPosicao                { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
