using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    public class TransporteRelatorio_021
    {
        #region Propriedades

        public string CodigoCliente     { get; set; }
        public string NomeCliente       { get; set; }
        public string CodigoAssessor    { get; set; }
        public string NomeAssessor      { get; set; }
        public string Isin              { get; set; }
        public string Distribuicao      { get; set; }
        public string Carteira          { get; set; }
        public string Ativo             { get; set; }
        public string Quantidade        { get; set; }
        public string Valor             { get; set; }
        public string PercentualIR      { get; set; }
        public string ValorIR           { get; set; }
        public string ValorLiquido      { get; set; }
        public string TipoProvento      { get; set; }
        public string GrupoProvento     { get; set; }
        public string DataPagamento     { get; set; }
        public string Emitente          { get; set; }
        #endregion

        #region Construtores

        public TransporteRelatorio_021(ProventosClienteInfo info)
        {
            this.CodigoCliente  = info.CodigoCliente.ToString();
            this.CodigoAssessor = info.CodigoAssessor.ToString();
            this.NomeAssessor   = info.NomeAssessor;
            this.NomeCliente    = info.NomeCliente;
            this.PercentualIR   = info.PercentualIR.ToString("N2");
            this.Quantidade     = info.Quantidade.ToString();
            this.TipoProvento   = info.TipoProvento.ToString();
            this.Isin           = info.Isin;
            this.Valor          = info.Valor.ToString("N2");
            this.ValorIR        = info.ValorIR.ToString("N2");
            this.ValorLiquido   = info.ValorLiquido.ToString("N2");
            this.Ativo          = info.Ativo;
            this.Distribuicao   = info.Distribuicao.ToString();
            this.Carteira       = info.Carteira.ToString();
            this.DataPagamento  = info.DataPagamento.ToString("dd/MM/yyyy");
            this.Emitente       = info.Emitente;
            this.GrupoProvento  = info.GrupoProvento;
        }

        public TransporteRelatorio_021()
        {

        }
        #endregion

        #region Métodos
        public List<TransporteRelatorio_021> TraduzirLista(List<ProventosClienteInfo> pParametros)
        {
            List<TransporteRelatorio_021> lRetorno = new List<TransporteRelatorio_021>();
            
            TransporteRelatorio_021 lProventos = new TransporteRelatorio_021();
            
            pParametros.ForEach(proventos => 
            {
                lProventos.CodigoCliente  = proventos.CodigoCliente.ToString();
                lProventos.CodigoAssessor = proventos.CodigoAssessor.ToString();
                lProventos.NomeAssessor   = proventos.NomeAssessor;
                lProventos.NomeCliente    = proventos.NomeCliente;
                lProventos.PercentualIR   = proventos.PercentualIR.ToString("N3");
                lProventos.Quantidade     = proventos.Quantidade.ToString();
                lProventos.TipoProvento   = proventos.TipoProvento.ToString();
                lProventos.Isin           = proventos.Isin;
                lProventos.Valor          = proventos.Valor.ToString("N2");
                lProventos.ValorIR        = proventos.ValorIR.ToString("N2");
                lProventos.ValorLiquido   = proventos.ValorLiquido.ToString("N2");
                lProventos.Ativo          = proventos.Ativo;
                lProventos.Distribuicao   = proventos.Distribuicao.ToString();
                lProventos.Carteira       = proventos.Carteira.ToString();
                lProventos.DataPagamento  = proventos.DataPagamento.ToString("dd/MM/yyyy");
                lProventos.Emitente       = proventos.Emitente;
                lProventos.GrupoProvento  = proventos.GrupoProvento;

                lRetorno.Add(lProventos);
            });

            return lRetorno;
        }
        #endregion
    }
}