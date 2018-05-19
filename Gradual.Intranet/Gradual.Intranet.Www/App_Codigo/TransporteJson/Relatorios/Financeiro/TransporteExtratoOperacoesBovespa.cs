using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro
{
    public class TransporteExtratoOperacoesBovespa
    {
        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        public string NomeCliente { get; set; }

        public string NomeOperador { get; set; }

        public string CodigoCliente { get; set; }

        public string DivCodigoCliente { get; set; }

        public string Mercado { get; set; }

        public string TipoOrdem { get; set; }

        public string Vencimento { get; set; }

        public string TipoBolsa { get; set; }

        public string PessoaVinculada { get; set; }

        public string NumeroOrdem { get; set; }

        public string Companhia { get; set; }

        public string TipoCompanhia { get; set; }

        public string Quantidade { get; set; }

        public string Preco { get; set; }

        public string CodigoAssessor { get; set; }

        public string Papel { get; set; }

        public string PrazoVencimento { get; set; }

        public string PrecoExercicio { get; set; }

        public string QuantidadeExecutada { get; set; }

        public string Sentido { get; set; }

        public string Saldo { get; set; }

        public string TipoMercado { get; set; }

        public string Status { get; set; }

        public List<TransporteExtratoOperacoesBovespaDetalhes> Detalhes;

        public List<TransporteExtratoOperacoesBovespa> TraduzirLista(ExtratoOrdemInfo pInfo)
        {
            var lRetorno = new List<TransporteExtratoOperacoesBovespa>();

            pInfo.Resultado.ForEach(ordem =>
                {
                    var lTrans = new TransporteExtratoOperacoesBovespa();

                    lTrans.NomeCliente         = ordem.NomeCliente;
                    lTrans.NomeOperador        = ordem.NomeOperador;
                    lTrans.CodigoCliente       = ordem.CodigoCliente.ToString();
                    lTrans.DivCodigoCliente    = ordem.DivCodigoCliente.ToString();
                    lTrans.Mercado             = ordem.Mercado;
                    lTrans.TipoOrdem           = ordem.TipoOrdem;
                    lTrans.Vencimento          = ordem.Vencimento.ToString("dd/MM/yyyy");
                    lTrans.TipoBolsa           = ordem.TipoBolsa;
                    lTrans.PessoaVinculada     = ordem.PessoaVinculada;
                    lTrans.NumeroOrdem         = ordem.NumeroOrdem.ToString();
                    lTrans.Companhia           = ordem.Companhia;
                    lTrans.TipoCompanhia       = ordem.TipoCompanhia;
                    lTrans.Quantidade          = ordem.Quantidade.ToString();
                    lTrans.Preco               = ordem.Preco.ToString("N2");
                    lTrans.CodigoAssessor      = ordem.CodigoAssessor.ToString();
                    lTrans.Papel               = ordem.Papel;
                    lTrans.PrazoVencimento     = ordem.PrazoVencimento;
                    lTrans.PrecoExercicio      = ordem.PrecoExercicio.ToString("N2");
                    lTrans.QuantidadeExecutada = ordem.QuantidadeExecutada.ToString("N");
                    lTrans.Sentido             = ordem.Sentido;
                    lTrans.Saldo               = ordem.Saldo.ToString();
                    lTrans.TipoMercado         = ordem.TipoMercado;
                    lTrans.Status              = ordem.Status;

                    lTrans.Detalhes = new List<TransporteExtratoOperacoesBovespaDetalhes>();
                    
                    TransporteExtratoOperacoesBovespaDetalhes lDet = null;
                    
                    ordem.Detalhes.ForEach(det =>
                        {
                            lDet = new TransporteExtratoOperacoesBovespaDetalhes();

                            lDet.CodigoContraParte = det.CodigoContraParte.ToString();
                            lDet.Data              = det.Data.ToString("dd/MM/yyyy HH:mm");
                            lDet.NumeroOperacao    = det.NumeroOperacao.ToString();
                            lDet.NumeroOrdem       = det.NumeroOrdem.ToString();
                            lDet.Preco             = det.Preco.ToString("N2");
                            lDet.Quantidade        = det.Quantidade.ToString();
                            lDet.Sentido           = det.Sentido.ToString();

                            lTrans.Detalhes.Add(lDet);
                        });

                    lRetorno.Add(lTrans);
                });

            return lRetorno;
        }
    }

    public class TransporteExtratoOperacoesBovespaDetalhes
    {
        public string Data { get; set; }

        public string NumeroOperacao { get; set; }

        public string Quantidade { get; set; }

        public string Preco { get; set; }

        public string CodigoContraParte { get; set; }

        public string Sentido { get; set; }

        public string NumeroOrdem { get; set; }
    }
}