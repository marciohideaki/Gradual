using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Monitores.Risco.Lib;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteCompliance
    {
        #region | Estruturas

        public struct EstatisticasDayTradeinfo
        {
            
            public string CodigoAssessor { get; set; }

            public string CodigoCliente { get; set; }

            public string Idade { get; set; }

            public string NET { get; set; }
            
            public string NomeAssessor { get; set; }
            
            public string NomeCliente { get; set; }

            public string PercentualPositivo { get; set; }
            
            public string PessoaVinculada { get; set; }

            public string QuantidadeDayTrade { get; set; }

            public string QuantidadeDayTradePositivo { get; set; }
            
            public string TipoBolsa { get; set; }
            
            public string ValorNegativo { get; set; }
            
            public string ValorPositivo { get; set; }

            public string DataNegocio { get; set; }
        }

        public struct NegociosDiretosInfo
        {
            public string DataNegocio { set; get; }

            public string CodigoClienteVendedor { set; get; }

            public string NomeClienteVendedor { set; get; }

            public string Sentido { set; get; }

            public string Instrumento { set; get; }

            public string PessoaVinculadaVendedor { set; get; }

            public string NumeroNegocio { set; get; }

            public string NomeClienteComprador { get; set; }

            public string CodigoClienteComprador { get; set; }

            public string PessoaVinculadaComprador { set; get; }
        }

        public struct OrdensAlteradasDayTradeCabecalhoInfo
        {
            public string Id { get; set; }

            public string DataHoraOrdem  { get; set; }
                                                     
            public string DayTrade       { get; set; }
                                                     
            public string Justificativa  { get; set; }
                                                     
            public string NumeroSeqOrdem { get; set; }
            
            public string TipoMercado    { get; set; }
        }

        public struct OrdensAlteradasInfo
        {
            public string Assessor	         { get; set; }
                                                         
            public string CodigoCliente      { get; set; }
                                                         
            public string ContaErro	         { get; set; }
                                                         
            public string DataAlteracao      { get; set; }
                                                         
            public string DescontoCorretagem { get; set; }
                                                         
            public string Instrumento	     { get; set; }
                                                         
            public string NomeCliente	     { get; set; }
                                                         
            public string NumeroSeqOrdem     { get; set; }
                                                         
            public string Quantidade	     { get; set; }
                                                         
            public string Sentido	         { get; set; }
                                                         
            public string TipoPessoa         { get; set; }
                                                         
            public string Usuario	         { get; set; }
                                                         
            public string UsuarioAlteracao   { get; set; }
                                                         
            public string Vinculado          { get; set; }
        }
        #endregion

        internal List<TransporteCompliance.OrdensAlteradasInfo> TraduzirLista(List<Monitores.Compliance.Lib.OrdensAlteradasDayTradeInfo> pParametros, string OrdensAlteradas= null)
        {
            var lRetorno = new List<TransporteCompliance.OrdensAlteradasInfo>();

            if (null != pParametros && pParametros.Count > 0)
            {
                pParametros.ForEach(plm =>
                {
                    plm.Corpo.ForEach(corpo =>
                        {
                            lRetorno.Add(new OrdensAlteradasInfo()
                            {
                                Assessor           = corpo.Assessor.ToString(),
                                CodigoCliente      = corpo.CodigoCliente.ToString(),
                                ContaErro          = corpo.ContaErro ? "Sim" : "Não",
                                DataAlteracao      = corpo.DataAlteracao.ToString("dd/MM/yyyy hh:mm:ss"),
                                DescontoCorretagem = corpo.DescontoCorretagem.ToString(),
                                Instrumento        = corpo.Instrumento.ToString(),
                                NomeCliente        = corpo.NomeCliente ,
                                NumeroSeqOrdem     = corpo.NumeroSeqOrdem.ToString(),
                                Quantidade         = corpo.Quantidade.ToString(),
                                Sentido            = corpo.Sentido.ToString(),
                                TipoPessoa         = corpo.TipoPessoa.ToString(),
                                Usuario            = corpo.Usuario.ToString(),
                                UsuarioAlteracao   = corpo.UsuarioAlteracao.ToString(),
                                Vinculado          = corpo.Vinculado ? "Sim" : "Não"
                            });
                        });
                });
            }
            return lRetorno;
        }
        internal List<TransporteCompliance.OrdensAlteradasDayTradeCabecalhoInfo> TraduzirLista(List<Monitores.Compliance.Lib.OrdensAlteradasDayTradeInfo> pParametros)
        {
            var lRetorno = new List<TransporteCompliance.OrdensAlteradasDayTradeCabecalhoInfo>();

            int lCount = 1;

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(plm =>
                {
                    lRetorno.Add(new OrdensAlteradasDayTradeCabecalhoInfo()
                    {
                        Id             = plm.Cabecalho.NumeroSeqOrdem.ToString(),
                        DataHoraOrdem  = plm.Cabecalho.DataHoraOrdem.ToString("dd/MM/yyyy hh:mm:ss"),
                        DayTrade       = plm.Cabecalho.DayTrade  ? "Sim" : "Não",
                        Justificativa  = plm.Cabecalho.Justificativa .ToString(),
                        NumeroSeqOrdem = plm.Cabecalho.NumeroSeqOrdem.ToString(),
                        TipoMercado    = plm.Cabecalho.TipoMercado   .ToString(),
                    });

                    lCount++;
                });

            return lRetorno;
        }

        internal List<TransporteCompliance.NegociosDiretosInfo> TraduzirLista(List<Monitores.Compliance.Lib.NegociosDiretosInfo> pParametros)
        {
            var lRetorno = new List<TransporteCompliance.NegociosDiretosInfo>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(plm =>
                {
                    
                    NegociosDiretosInfo lTemp =  lRetorno.Find(nd=>nd.NumeroNegocio == plm.NumeroNegocio.ToString());

                    var lNeg = new NegociosDiretosInfo();

                    if (string.IsNullOrEmpty(lTemp.NumeroNegocio))
                    {
                        lNeg.DataNegocio           = plm.DataNegocio.ToString("dd/MM/yyyy");

                        if (plm.Sentido == "V")
                        {
                            lNeg.CodigoClienteVendedor = plm.CodigoCliente.ToString();
                            lNeg.NomeClienteVendedor   = plm.NomeCliente;
                            lNeg.PessoaVinculadaVendedor = plm.PessoaVinculada.ToString();
                        }
                        else
                        {
                            lNeg.CodigoClienteComprador = plm.CodigoCliente.ToString();
                            lNeg.NomeClienteComprador = plm.NomeCliente;
                            lNeg.PessoaVinculadaComprador = plm.PessoaVinculada.ToString();
                        }

                        lNeg.Sentido               = plm.Sentido.ToString();
                        lNeg.Instrumento           = plm.Instrumento.ToString();
                        lNeg.NumeroNegocio         = plm.NumeroNegocio.ToString();

                        lRetorno.Add(lNeg);
                    }
                    else
                    {
                        lRetorno.Remove(lTemp);

                        lTemp.DataNegocio           = plm.DataNegocio.ToString("dd/MM/yyyy");

                        if (plm.Sentido == "C")
                        {
                            lTemp.CodigoClienteComprador = plm.CodigoCliente.ToString();
                            lTemp.NomeClienteComprador   = plm.NomeCliente;
                            lTemp.PessoaVinculadaComprador = plm.PessoaVinculada.ToString();
                        }
                        else
                        {
                            lTemp.CodigoClienteVendedor    = plm.CodigoCliente.ToString();
                            lTemp.NomeClienteVendedor      = plm.NomeCliente;
                            lTemp.PessoaVinculadaVendedor = plm.PessoaVinculada.ToString();
                        }

                        lTemp.Sentido               = plm.Sentido.ToString();
                        lTemp.Instrumento           = plm.Instrumento.ToString();
                        lTemp.NumeroNegocio         = plm.NumeroNegocio.ToString();

                        lRetorno.Add(lTemp);
                    }
                });

            return lRetorno;
        }

        internal List<TransporteCompliance.EstatisticasDayTradeinfo> TraduzirLista(List<Monitores.Compliance.EstatisticaDayTradeBovespaInfo> pParametros)
        {
            var lRetorno = new List<TransporteCompliance.EstatisticasDayTradeinfo>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(plm =>
                {
                    lRetorno.Add(new EstatisticasDayTradeinfo()
                    {
                        CodigoAssessor             =  plm.CodigoAssessor.ToString(),
                        CodigoCliente              =  plm.CodigoCliente.ToString(),
                        Idade                      =  plm.Idade        .ToString(),
                        NET                        =  plm.NET          .ToString("N2"),
                        NomeAssessor               =  plm.NomeAssessor .ToString(),
                        NomeCliente                =  plm.NomeCliente  .ToString(),
                        PercentualPositivo         = plm.PercentualPositivo         .ToString("N2"),
                        PessoaVinculada            = plm.PessoaVinculada            .ToString(),
                        QuantidadeDayTrade         = plm.QuantidadeDayTrade         .ToString(),
                        QuantidadeDayTradePositivo = plm.QuantidadeDayTradePositivo .ToString(),
                        TipoBolsa                  = plm.TipoBolsa.ToString(),
                        ValorNegativo              = plm.ValorNegativo.ToString("N2"),
                        ValorPositivo              = plm.ValorPositivo.ToString("N2"),
                        DataNegocio                = plm.DataNegocio.ToString("dd/MM/yyyy")
                    });
                });

            return lRetorno;
        }
    }
}