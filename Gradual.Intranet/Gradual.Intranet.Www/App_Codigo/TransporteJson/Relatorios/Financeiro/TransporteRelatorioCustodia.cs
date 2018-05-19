using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.ContaCorrente.Lib.Info;
using System.Globalization;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro
{
    public class TransporteExtratoRelatorioCustodia
    {
        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        public string Mercado { get; set; }

        public string Ativo { get; set; }

        public string DescricaoCarteira { get; set; }

        public string CodigoCarteira { get; set; }

        public string NomeCarteira { get; set; }

        public string NomeEmpresa { get; set; }

        public string Quantidade { get; set; }

        public string Compra { get; set; }

        public string Venda { get; set; }

        public string QuantidadeAtual { get; set; }

        public string FechamentoAnterior { get; set; }

        public string ValorAtual { get; set; }

        public List<TransporteExtratoRelatorioCustodia> TraduzirLista(List<CustodiaExtratoInfo> pParametros)
        {
            var lRetorno = new List<TransporteExtratoRelatorioCustodia>();

            if (null != pParametros)
                pParametros.ForEach(delegate(CustodiaExtratoInfo cei)
                {
                    lRetorno.Add(new TransporteExtratoRelatorioCustodia()
                    {
                        Ativo = cei.Ativo,
                        CodigoCarteira = cei.CodigoCarteira.DBToString(),
                        DescricaoCarteira = cei.DescricaoCarteira,
                        Compra = cei.Compra.ToString("N0", gCultureInfo),
                        FechamentoAnterior = cei.FechamentoAnterior.ToString("N2", gCultureInfo),
                        Mercado = cei.Mercado,
                        NomeEmpresa = cei.NomeEmpresa,
                        Quantidade = cei.Quantidade.ToString("N0", gCultureInfo),
                        QuantidadeAtual = cei.QuantidadeAtual.ToString("N0", gCultureInfo),
                        ValorAtual = cei.ValorAtual.ToString("N2", gCultureInfo),
                        Venda = cei.Venda.ToString("N0", gCultureInfo),
                    });
                });

            //--> Ordenando pelo nome do ativo.
            lRetorno.Sort(delegate(TransporteExtratoRelatorioCustodia te1, TransporteExtratoRelatorioCustodia te2) { return Comparer<string>.Default.Compare(te1.NomeEmpresa, te2.NomeEmpresa); });

            return lRetorno;
        }
    }
}