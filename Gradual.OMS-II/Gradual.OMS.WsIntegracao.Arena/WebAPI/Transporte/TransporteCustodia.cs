using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using WebAPI_Test.Models;

namespace Gradual.OMS.WsIntegracao.Arena.Transporte
{
    public class TransporteCustodia
    {
        #region | Atributos

        private IFormatProvider gCultura = new CultureInfo("pt-BR");
        private static IFormatProvider gCulturaStatic = new CultureInfo("pt-BR");

        #endregion

        #region | Propriedades

        public string CodigoCliente { get; set; }

        public string CodigoNegocio { get; set; }

        public string Empresa { get; set; }

        public string TipoMercado { get; set; }

        public string ValorDeFechamento { get; set; }

        public string Carteira { get; set; }

        public string DsCarteira { get; set; }

        public string CodigoCarteira { get; set; }

        public string Cotacao { get; set; }

        public string QtdAbertura { get; set; }

        public string QtdCompra { get; set; }

        public string QtdVenda { get; set; }

        public string QtdAtual { get; set; }

        public string PrecoMedio { get; set; }

        public string ValorAtualizado { get; set; }

        public string Valor { get; set; }

        public string Variacao { get; set; }

        public string Resultado { get; set; }

        public string TipoGrupo { get; set; }

        public string ValorPosicao { get; set; }

        public string QtdD1 { get; set; }

        public string QtdD2 { get; set; }

        public string QtdD3 { get; set; }

        public string CodigoSerie { get; set; }

        public string FatorCotacao { get; set; }

        public string QtdDATotal { get; set; }

        public string PrecoNegocioCompra { get; set; }

        public string PrecoNegocioVenda { get; set; }

        public string Sentido { get; set; }

        public bool EhPosicao { get; set; }

        public string QtdLiquid { get; set; }

        public string ValorPU { get; set; }
        #endregion

        #region Construtores

        public TransporteCustodia()
        {
            this.CodigoCliente      = "n/d";
            this.CodigoNegocio      = "n/d";
            this.Empresa            = "n/d";
            this.TipoMercado        = "n/d";
            this.ValorDeFechamento  = "n/d";
            this.Carteira           = "n/d";
            this.DsCarteira         = "n/d";
            this.Cotacao            = "n/d";
            this.QtdAbertura        = "n/d";
            this.QtdCompra          = "n/d";
            this.QtdVenda           = "n/d";
            this.QtdAtual           = "n/d";
            this.PrecoMedio         = "n/d";
            this.ValorAtualizado    = "n/d";
            this.Valor              = "n/d";
            this.Variacao           = "n/d";
            this.Resultado          = "n/d";
            this.TipoGrupo          = "n/d";
            this.ValorPosicao       = "n/d";
            this.QtdD1              = "n/d";
            this.QtdD2              = "n/d";
            this.QtdD3              = "n/d";
            this.CodigoSerie        = "n/d";
            this.PrecoNegocioCompra = "n/d";
            this.PrecoNegocioVenda  = "n/d";
            this.Sentido            = "n/d";
            this.EhPosicao          = false;
            this.QtdLiquid          = "n/d";
            this.ValorPU            = "n/d";
        }
       

        public TransporteCustodia(MonitorCustodiaInfo.CustodiaPosicao pParametro)
            : this()
        {
            this.Empresa           = pParametro.NomeEmpresa;
            this.TipoMercado       = pParametro.TipoMercado;
            this.CodigoNegocio     = pParametro.CodigoInstrumento;
            this.QtdVenda          = pParametro.QtdeAExecVenda.ToString("N0");
            this.QtdAtual          = pParametro.QtdeAtual.ToString(gCultura);
            this.CodigoCarteira    = pParametro.CodigoCarteira.ToString();
            this.Carteira          = pParametro.DescricaoCarteira;
            this.QtdCompra         = pParametro.QtdeAExecCompra.ToString("N0");
            this.CodigoSerie       = pParametro.CodigoSerie;
            this.QtdAbertura       = pParametro.QtdeDisponivel.ToString(gCultura);
            this.CodigoCliente     = pParametro.IdCliente.ToString();
            this.TipoGrupo         = pParametro.TipoGrupo;
            this.ValorPosicao      = pParametro.ValorPosicao.ToString(gCultura);
            this.QtdLiquid         = pParametro.QtdeLiquidar.ToString("N0");
            this.QtdD1             = pParametro.QtdeD1.ToString("N0");
            this.QtdD2             = pParametro.QtdeD2.ToString("N0");
            this.QtdD3             = pParametro.QtdeD3.ToString("N0");

            if (pParametro.TipoGrupo == "TEDI")
            {
                this.QtdAtual    = String.Format(gCultura, "{0:f2}", pParametro.QtdeAtual);
                this.QtdAbertura = String.Format(gCultura, "{0:f2}", pParametro.QtdeDisponivel);
            }
            else
            {
                this.QtdAtual    = String.Format(gCultura, "{0:#,0}", pParametro.QtdeAtual);
                this.QtdAbertura = String.Format(gCultura, "{0:#,0}", pParametro.QtdeDisponivel);
            }


            if (this.TipoMercado.Equals("OPC") || this.TipoMercado.Equals("OPV"))
            {
                this.QtdD1 = pParametro.QtdeD3.ToString("N0");
                this.QtdD2 = 0.ToString("N0");
                this.QtdD3 = 0.ToString("N0");
            }
            else
            {
                this.QtdD1 = pParametro.QtdeD1.ToString("N0");
                this.QtdD2 = pParametro.QtdeD2.ToString("N0");
                this.QtdD3 = pParametro.QtdeD3.ToString("N0");
            }

            this.Variacao          = pParametro.Variacao.ToString("N2");
            this.ValorDeFechamento = pParametro.ValorFechamento.ToString("N2");
            this.Cotacao           = pParametro.Cotacao.ToString("N2");
            this.Resultado         = pParametro.Resultado.ToString("N2");
            //this.ValorPU           = pParametro.ValorPU.ToString("N2");

            if (pParametro.TipoGrupo == "TEDI")
            {
                this.Resultado = pParametro.ValorPosicao.ToString("N2");
            }
        }

        #endregion

        #region | Métodos
        /*
        public static List<TransporteCustodia> TraduzirCustodiaInfo(List<PosicaoCustodiaInfo.CustodiaMovimento> pParametros)
        {
            List<TransporteCustodia> lRetorno = new List<TransporteCustodia>();

            if (pParametros != null)
            {
                foreach (PosicaoCustodiaInfo.CustodiaMovimento lInfo in pParametros)
                {
                    lRetorno.Add(new TransporteCustodia(lInfo));
                }
            }

            return lRetorno;
        }
        */

        public static List<SaldoCustodiaBovespaCliente> TraduzirCustodiaInfo(List<MonitorCustodiaInfo.CustodiaPosicao> pParametros)
        {
            var lRetorno = new List<SaldoCustodiaBovespaCliente>();

            foreach (MonitorCustodiaInfo.CustodiaPosicao lInfo in pParametros)
            {
                var lCustodia = new SaldoCustodiaBovespaCliente();

                lCustodia.Ativo = lInfo.CodigoInstrumento;
                lCustodia.CodigoCarteira = lInfo.CodigoCarteira.ToString();
                lCustodia.SaldoAbertura = lInfo.QtdeDisponivel;
                lCustodia.SaldoD0 = lInfo.QtdeAtual;
                lCustodia.SaldoD1 = lInfo.QtdeD1;
                lCustodia.SaldoD2 = lInfo.QtdeD2;
                lCustodia.SaldoD3 = lInfo.QtdeD3;
                lCustodia.SaldoTotal = lInfo.QtdeDATotal;

                //lCustodia.Empresa              = lInfo.NomeEmpresa;
                //lCustodia.TipoMercado          = lInfo.TipoMercado;
                //lCustodia.quan                 = lInfo.QtdeAExecVenda
                //lCustodia.QtdAtual             = lInfo.QtdeAtual.ToString(gCultura);
                //lCustodia.CodigoCarteira       = lInfo.DescricaoCarteira;
                //lCustodia.QtdCompra            = lInfo.QtdeAExecCompra.ToString("N0");
                //lCustodia.CodigoSerie          = lInfo.CodigoSerie;
                //lCustodia.CodigoCliente        = lInfo.IdCliente.ToString();
                //lCustodia.TipoGrupo            = lInfo.TipoGrupo;
                //lCustodia.ValorPosicao         = lInfo.ValorPosicao.ToString(gCultura);
                //lCustodia.QtdLiquid            = lInfo.QtdeLiquidar.ToString("N0");
                
                if (lInfo.TipoGrupo == "TEDI")
                {
                    //this.QtdAtual = String.Format(gCultura, "{0:f2}", pParametro.QtdeAtual);
                    //this.QtdAbertura = String.Format(gCultura, "{0:f2}", pParametro.QtdeDisponivel);
                }
                else
                {
                    //this.QtdAtual = String.Format(gCultura, "{0:#,0}", pParametro.QtdeAtual);
                    //this.QtdAbertura = String.Format(gCultura, "{0:#,0}", pParametro.QtdeDisponivel);
                }



                if (lInfo.TipoMercado == "FUT" || lInfo.TipoMercado == "OPF" || lInfo.TipoMercado == "DIS")
                {

                }


                if (lInfo.TipoMercado.Equals("OPC") || lInfo.TipoMercado.Equals("OPV"))
                {
                    lCustodia.SaldoD1 = lInfo.QtdeD3;
                    lCustodia.SaldoD2 = 0;
                    lCustodia.SaldoD3 = 0;
                }
                else
                {
                    lCustodia.SaldoD1 = lInfo.QtdeD1;
                    lCustodia.SaldoD2 = lInfo.QtdeD2;
                    lCustodia.SaldoD3 = lInfo.QtdeD3;
                }
                /*
                this.Variacao = pParametro.Variacao.ToString("N2");
                this.ValorDeFechamento = pParametro.ValorFechamento.ToString("N2");
                this.Cotacao = pParametro.Cotacao.ToString("N2");
                this.Resultado = pParametro.Resultado.ToString("N2");
                //this.ValorPU           = pParametro.ValorPU.ToString("N2");

                if (pParametro.TipoGrupo == "TEDI")
                {
                    this.Resultado = pParametro.ValorPosicao.ToString("N2");
                }
                */
                lRetorno.Add(lCustodia);
            }
            return lRetorno;
        }

        //public static List<TransporteCustodia> TraduzirCustodiaInfo(List<MonitorCustodiaInfo.CustodiaPosicao> pParametros)
        //{
        //    List<TransporteCustodia> lRetorno = new List<TransporteCustodia>();

        //    if (pParametros != null)
        //    {
        //        foreach (MonitorCustodiaInfo.CustodiaPosicao lInfo in pParametros)
        //        {
        //            lRetorno.Add(new TransporteCustodia(lInfo));
        //        }
        //    }

        //    return lRetorno;
        //}
        /*
        public static List<TransporteCustodia> TraduzirCustodiaInfo(List<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo.CustodiaPosicao> pParametros)
        {
            List<TransporteCustodia> lRetorno = new List<TransporteCustodia>();
            List<TransporteCustodia> lTempRetorno = new List<TransporteCustodia>();

            if (pParametros != null)
            {
                foreach (MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo.CustodiaPosicao lInfo in pParametros)
                {
                    TransporteCustodia lPosicao = new TransporteCustodia();

                    var lTemp = lTempRetorno.Find(negocio => negocio.CodigoNegocio == lInfo.CodigoInstrumento);


                    if (lTemp != null)
                    {
                        lTempRetorno.Remove(lTemp);

                        lTemp.QtdAbertura        = "0";
                        lTemp.Empresa            = "n/a";
                        lTemp.TipoMercado        = lInfo.TipoMercado;
                        lTemp.CodigoNegocio      = lInfo.CodigoInstrumento;
                        lTemp.QtdVenda           = (decimal.Parse(lTemp.QtdVenda, gCulturaStatic) + lInfo.QtdeAExecVenda).ToString();
                        lTemp.QtdCompra          = (decimal.Parse(lTemp.QtdCompra, gCulturaStatic) + lInfo.QtdeAExecCompra).ToString();
                        lTemp.QtdAtual           = (decimal.Parse(lTemp.QtdCompra, gCulturaStatic) - decimal.Parse(lTemp.QtdVenda, gCulturaStatic)).ToString(gCulturaStatic);
                        lTemp.Carteira           = "n/a";
                        lTemp.CodigoCliente      = lInfo.IdCliente.ToString();
                        lTemp.TipoGrupo          = lInfo.TipoGrupo;
                        lTemp.ValorPosicao       = lInfo.ValorPosicao.ToString(gCulturaStatic);
                        lTemp.CodigoSerie        = lInfo.CodigoSerie;
                        lTemp.PrecoNegocioCompra = (decimal.Parse(lTemp.PrecoNegocioCompra, gCulturaStatic) + (lInfo.PrecoNegocioCompra * lInfo.QtdeAExecCompra)).ToString();
                        lTemp.PrecoNegocioVenda  = (decimal.Parse(lTemp.PrecoNegocioVenda, gCulturaStatic) + (lInfo.PrecoNegocioVenda * lInfo.QtdeAExecVenda)).ToString();
                        lTemp.Sentido            = lInfo.Sentido;
                        lTemp.EhPosicao          = true;

                        lTempRetorno.Add(lTemp);
                    }
                    else
                    {
                        lPosicao.QtdAbertura        = "0";
                        lPosicao.Empresa            = "n/a";
                        lPosicao.TipoMercado        = lInfo.TipoMercado;
                        lPosicao.CodigoNegocio      = lInfo.CodigoInstrumento;
                        lPosicao.QtdVenda           = lInfo.QtdeAExecVenda.ToString(gCulturaStatic);
                        lPosicao.QtdCompra          = lInfo.QtdeAExecCompra.ToString(gCulturaStatic);
                        lPosicao.QtdAtual           = (decimal.Parse(lPosicao.QtdCompra, gCulturaStatic) - decimal.Parse(lPosicao.QtdVenda, gCulturaStatic)).ToString(gCulturaStatic);
                        lPosicao.Carteira           = "n/a";
                        lPosicao.CodigoCliente      = lInfo.IdCliente.ToString();
                        lPosicao.TipoGrupo          = lInfo.TipoGrupo;
                        lPosicao.ValorPosicao       = lInfo.ValorPosicao.ToString(gCulturaStatic);
                        lPosicao.CodigoSerie        = lInfo.CodigoSerie;
                        lPosicao.PrecoNegocioCompra = (lInfo.PrecoNegocioCompra * lInfo.QtdeAExecCompra).ToString(gCulturaStatic);
                        lPosicao.PrecoNegocioVenda  = (lInfo.PrecoNegocioVenda * lInfo.QtdeAExecVenda).ToString(gCulturaStatic);
                        lPosicao.Sentido            = lInfo.Sentido;
                        lPosicao.EhPosicao          = true;

                        lTempRetorno.Add(lPosicao);
                    }
                }

                lRetorno = lTempRetorno;
            }

            return lRetorno;
        }
        */
        public static List<SaldoCustodiaBmfCliente> TraduzirCustodiaInfo(List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> pParametros, List<MonitorCustodiaInfo.CustodiaPosicao> pParametrosNormal )
        {
            var lRetorno = new List<SaldoCustodiaBmfCliente>();
            var lTempRetorno = new List<SaldoCustodiaBmfCliente>();

            if (pParametros != null)
            {
                foreach (MonitorCustodiaInfo.CustodiaPosicaoDiaBMF lInfo in pParametros)
                {
                    var lPosicao = new SaldoCustodiaBmfCliente();

                    var lTemp = lTempRetorno.Find(negocio => negocio.Ativo == lInfo.CodigoInstrumento);

                    if (lTemp != null)
                    {
                        lTempRetorno.Remove(lTemp);

                        lTemp.QuantidadeAbertura          =  0M;
                        lTemp.TipoMercadoria              = lInfo.TipoMercado;
                        lTemp.Ativo                       = lInfo.CodigoInstrumento;
                        lTemp.QuantidadeVendido           = lTemp.QuantidadeVendido + lInfo.QtdeAExecVenda;
                        lTemp.QuantidadeComprado          = lTemp.QuantidadeComprado + lInfo.QtdeAExecCompra;
                        lTemp.QuantidadeAtual             = lTemp.QuantidadeComprado - lTemp.QuantidadeVendido;
                        lTemp.Serie                       = lInfo.CodigoSerie;
                        lTemp.PU                          = lInfo.ValorPU;
                        lTemp.Ajuste                      = lInfo.ValorFechamento;
                        lTemp.EhPosicao                   = true;
                        //lTemp.Carteira             = "n/a";
                        //lTemp.CodigoCliente        = lInfo.IdCliente.ToString();
                        //lTemp.TipoGrupo            = lInfo.TipoGrupo;
                        //lTemp.ValorPosicao         = lInfo.ValorPosicao.ToString(gCulturaStatic);
                        //lTemp.PrecoNegocioCompra   = (decimal.Parse(lTemp.PrecoNegocioCompra, gCulturaStatic) + (lInfo.PrecoNegocioCompra * lInfo.QtdeAExecCompra)).ToString();
                        //lTemp.PrecoNegocioVenda    = (decimal.Parse(lTemp.PrecoNegocioVenda, gCulturaStatic) + (lInfo.PrecoNegocioVenda * lInfo.QtdeAExecVenda)).ToString();
                        //lTemp.Sentido              = lInfo.Sentido;
                        //lPosicao.Cotacao           = lInfo.Cotacao.ToString("N2");
                        //lPosicao.ValorDeFechamento = lInfo.ValorFechamento.ToString("N2");
                        //lPosicao.Resultado         = lInfo.Resultado.ToString("N2");
                        //lPosicao.Variacao          = lInfo.Variacao.ToString("N2");
                        //lPosicao.ValorPU           = lInfo.ValorPU.ToString("N2");

                        lTempRetorno.Add(lTemp);
                    }
                    else
                    {
                        lPosicao.QuantidadeAbertura          = 0M;
                        lPosicao.TipoMercadoria              = lInfo.TipoMercado;
                        lPosicao.Ativo                       = lInfo.CodigoInstrumento;
                        lPosicao.QuantidadeVendido           = lInfo.QtdeAExecVenda;
                        lPosicao.QuantidadeComprado          = lInfo.QtdeAExecCompra;
                        lPosicao.QuantidadeAtual             = lPosicao.QuantidadeComprado- lPosicao.QuantidadeVendido;
                        lPosicao.Serie                       = lInfo.CodigoSerie;
                        lPosicao.PU                          = lInfo.ValorPU;
                        lPosicao.Ajuste                      = lInfo.ValorFechamento;

                        //lPosicao.Carteira           = "n/a";
                        //lPosicao.CodigoCliente      = lInfo.IdCliente.ToString();
                        //lPosicao.TipoGrupo          = lInfo.TipoGrupo;
                        //lPosicao.ValorPosicao       = lInfo.ValorPosicao.ToString(gCulturaStatic);
                        //lPosicao.PrecoNegocioCompra = (lInfo.PrecoNegocioCompra * lInfo.QtdeAExecCompra).ToString(gCulturaStatic);
                        //lPosicao.PrecoNegocioVenda  = (lInfo.PrecoNegocioVenda * lInfo.QtdeAExecVenda).ToString(gCulturaStatic);
                        //lPosicao.Sentido            = lInfo.Sentido;
                        //lPosicao.EhPosicao          = true;
                        //lPosicao.Cotacao            = lInfo.Cotacao.ToString("N2");
                        //lPosicao.ValorDeFechamento  = lInfo.ValorFechamento.ToString("N2");
                        //lPosicao.Resultado          = lInfo.Resultado.ToString("N2");
                        //lPosicao.Variacao           = lInfo.Variacao.ToString("N2");
                        //lPosicao.ValorPU            = lInfo.ValorPU.ToString("N2");
                        lTempRetorno.Add(lPosicao);
                    }
                }

                lRetorno = lTempRetorno;
            }
            
            if (pParametrosNormal != null)
            {
                lTempRetorno = new List<SaldoCustodiaBmfCliente>();

                foreach (MonitorCustodiaInfo.CustodiaPosicao lInfo in pParametrosNormal)
                {
                    var lPosicao = new SaldoCustodiaBmfCliente();

                    var lTemp = lTempRetorno.Find(negocio => negocio.Ativo == lInfo.CodigoInstrumento);

                    if (lInfo.TipoMercado != "FUT") continue;

                    if (lTemp != null)
                    {
                        lTempRetorno.Remove(lTemp);

                        lTemp.QuantidadeAbertura = 0M;
                        lTemp.TipoMercadoria     = lInfo.TipoMercado;
                        lTemp.Ativo              = lInfo.CodigoInstrumento;
                        lTemp.QuantidadeVendido  = lTemp.QuantidadeVendido + lInfo.QtdeAExecVenda;
                        lTemp.QuantidadeComprado = lTemp.QuantidadeComprado + lInfo.QtdeAExecCompra;
                        lTemp.QuantidadeAtual    = lTemp.QuantidadeComprado - lTemp.QuantidadeVendido;
                        lTemp.Serie              = lInfo.CodigoSerie;
                        lTemp.PU                 = lInfo.ValorPU;
                        lTemp.Ajuste             = lInfo.ValorFechamento;
                        lTemp.EhPosicao          = true;

                        lTempRetorno.Add(lTemp);
                    }
                    else
                    {
                        lPosicao.QuantidadeAbertura = 0M;
                        lPosicao.TipoMercadoria     = lInfo.TipoMercado;
                        lPosicao.Ativo              = lInfo.CodigoInstrumento;
                        lPosicao.QuantidadeVendido  = lInfo.QtdeAExecVenda;
                        lPosicao.QuantidadeComprado = lInfo.QtdeAExecCompra;
                        lPosicao.QuantidadeAtual    = lPosicao.QuantidadeComprado - lPosicao.QuantidadeVendido;
                        lPosicao.Serie              = lInfo.CodigoSerie;
                        lPosicao.PU                 = lInfo.ValorPU;
                        lPosicao.Ajuste             = lInfo.ValorFechamento;

                        lTempRetorno.Add(lPosicao);
                    }
                }
            
                lRetorno.AddRange(lTempRetorno);
            }
            
            return lRetorno;
        }
        #endregion
    }
}