using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;

namespace Gradual.Site.Www.Transporte
{
    public class TransportePosicaoConsolidada
    {
        #region Propriedades
        public int key                  { get; set; }

        public string IdProduto         { get; set; }

        public string CodCliente        { get; set; }

        public string NomeCliente       { get; set; }

        public string NomeFundo         { get; set; }

        public string Risco             { get; set; }

        public string QtdCotas          { get; set; }

        public string ValorCota         { get; set; }

        public string ValorBruto        { get; set; }

        public string IR                { get; set; }

        public string ValorLiquido      { get; set; }

        public string DataAtualizacao   { get; set; }

        public string IOF               { get; set; }
        #endregion


        public TransportePosicaoConsolidada()
        {

        }

        public List<TransportePosicaoConsolidada> TraduzirLista(List<IntegracaoFundosPosicaoClienteInfo> pParametros)
        {
            var lRetorno = new List<TransportePosicaoConsolidada>();

            pParametros.ForEach(posicao =>
                {
                    var lPosicao = new TransportePosicaoConsolidada();

                    //lPosicao. key           = posicao.
                    lPosicao.IdProduto       = posicao.Fundo.IdProduto.ToString();
                    lPosicao.CodCliente      = posicao.CodCliente.ToString();
                    lPosicao.NomeCliente     = posicao.NomeCliente;
                    lPosicao.NomeFundo       = posicao.Fundo.NomeProduto;
                    lPosicao.Risco           = posicao.Fundo.Risco;
                    lPosicao.QtdCotas        = posicao.QtdCotas.ToString();
                    lPosicao.ValorCota       = posicao.ValorCota.ToString("N2");
                    lPosicao.ValorBruto      = posicao.ValorBruto.ToString("N2");
                    lPosicao.IR              = posicao.IR.ToString("N2");
                    lPosicao.ValorLiquido    = posicao.ValorLiquido.ToString("N2");
                    lPosicao.DataAtualizacao = posicao.DataProcessamento.ToString("dd/MM/yyyy HH:mm");
                    lPosicao.IOF             = posicao.IOF.ToString("N2");

                    lRetorno.Add(lPosicao);
                });
            return lRetorno;
        }

    }
}