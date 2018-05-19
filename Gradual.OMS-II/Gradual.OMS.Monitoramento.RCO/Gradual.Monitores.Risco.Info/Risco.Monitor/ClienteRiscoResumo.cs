using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe responsável em armazenar e mostrar na intranet as operações intraday Bovespa
    /// É um resumo do que acontece com operações agrupadas pelo mesmo instrumento bovespa
    /// </summary>
    [Serializable]
    [DataContract]
    public class ClienteRiscoResumo
    {
        /// <summary>
        /// Código do cliente
        /// </summary>
        [DataMember]
        public int     Cliente             { set; get; }

        /// <summary>
        /// Instrumento 
        /// </summary>
        [DataMember]
        public string  Instrumento         { set; get; }

        /// <summary>
        /// Tipo de Mercado
        /// </summary>
        [DataMember]
        public string  TipoMercado         { set; get; }

        /// <summary>
        /// Lucro prejuízo do grupo de ordens
        /// </summary>
        [DataMember]
        public decimal LucroPrejuizo       { set; get; }

        /// <summary>
        /// Valor do Negocio de compra do grupo de ordem
        /// </summary>
        [DataMember]
        public decimal VLNegocioCompra     { set; get; }

        /// <summary>
        /// Valor do Negocio de venda do grupo das ordens do cliente
        /// </summary>
        [DataMember]
        public decimal VLNegocioVenda      { set; get; }

        /// <summary>
        /// Valor total de compra de mercado do grupo das ordens do cliente
        /// </summary>
        [DataMember]
        public decimal VLMercadoCompra      { set; get; }

        /// <summary>
        /// Valor total de compra de mercado do grupo das ordens do cliente
        /// </summary>
        [DataMember]
        public decimal VLMercadoVenda       { set; get; }

        /// <summary>
        /// Valor da cotação do instrumento do grupo
        /// </summary>
        [DataMember]
        public decimal Cotacao              { set; get; }

        /// <summary>
        /// Quantidade de abertura do papel do grupo do cliente
        /// </summary>
        [DataMember]
        public int     QtdeAber             { set; get; }

        /// <summary>
        /// Somatória da quantidade comprada do grupo do insturmento
        /// </summary>
        [DataMember]
        public int     QtdeComprada         { set; get; }

        /// <summary>
        /// Somatória da quantidade vendida do grupo do insturmento
        /// </summary>
        [DataMember]
        public int     QtdeVendida          { set; get; }

        /// <summary>
        /// Somatória da quantidade comprada do grupo do insturmento
        /// </summary>
        [DataMember]
        public decimal QtdeAtual            { set; get; }

        /// <summary>
        /// Financeiro de abertura é a quantidade do instrumento na abertura * a Cotação atual
        /// </summary>
        [DataMember]
        public decimal FinanceiroAbertura { set; get; }

        /// <summary>
        /// Financeiro comprado é a somatória do total negóciado em operações de compra
        /// </summary>
        [DataMember]
        public decimal FinanceiroComprado        { set; get; }

        /// <summary>
        /// Financeiro Vendido é a somatória do total negóciado em operações de venda
        /// </summary>
        [DataMember]
        public decimal FinanceiroVendido         { set; get; }

        /// <summary>
        /// QUantidade Reversão é o Lucro Prejuízo do grupo do instrumento / (dividido) pela cotação atual
        /// </summary>
        [DataMember]        
        public decimal QtReversao              { set; get; }
        [DataMember]
        public decimal PrecoReversao           { set; get; }
        [DataMember]
        public decimal NetOperacao              { set; get; }
    }
}
