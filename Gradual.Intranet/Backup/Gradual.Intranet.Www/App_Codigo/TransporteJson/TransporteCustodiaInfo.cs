using System;
using System.Collections.Generic;
using System.Globalization;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.OMS.Monitor.Custodia.Lib.Util;
namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteCustodiaInfo
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

        public TransporteCustodiaInfo()
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
        // Gradual.OMS.RelatoriosFinanc.Lib.Dados
        public TransporteCustodiaInfo(PosicaoCustodiaInfo.CustodiaMovimento pParametro) : this()
        {
            this.Empresa           = pParametro.NomeEmpresa;
            this.TipoMercado       = pParametro.TipoMercado;
            this.CodigoNegocio     = pParametro.CodigoInstrumento;
            this.QtdVenda          = pParametro.QtdeAExecVenda.ToString(gCultura);
            this.QtdAtual          = pParametro.QtdeAtual.ToString(gCultura);
            this.Carteira          = pParametro.DescricaoCarteira;
            this.QtdCompra         = pParametro.QtdeAExecCompra.ToString(gCultura);

            this.QtdAbertura       = pParametro.QtdeDisponivel.ToString(gCultura);
            this.CodigoCliente     = pParametro.IdCliente.ToString();
            this.TipoGrupo         = pParametro.TipoGrupo;
            this.ValorPosicao      = pParametro.ValorPosicao.ToString(gCultura);
            this.QtdD1             = pParametro.QtdeD1.ToString("N0");
            this.QtdD2             = pParametro.QtdeD2.ToString("N0");
            this.QtdD3             = pParametro.QtdeD3.ToString("N0");

        }

        public TransporteCustodiaInfo(MonitorCustodiaInfo.CustodiaPosicao pParametro)
            : this()
        {
            this.Empresa           = pParametro.NomeEmpresa;
            this.TipoMercado       = pParametro.TipoMercado;
            this.CodigoNegocio     = pParametro.CodigoInstrumento;
            this.QtdVenda          = pParametro.QtdeAExecVenda.ToString("N0");
            this.QtdAtual          = pParametro.QtdeAtual.ToString(gCultura);
            if (pParametro.CodigoCarteira == 0)
            {
                this.CodigoCarteira = "n/a";
                this.Carteira       = "n/a";
            }
            else
            {
                this.CodigoCarteira = pParametro.CodigoCarteira.ToString();
                this.Carteira       = pParametro.DescricaoCarteira;
            }
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

        
        

        public TransporteCustodiaInfo(MonitoramentoRiscoLucroCustodiaInfo.CustodiaMovimento pParametro)
            : this()
        {
            this.Empresa        = pParametro.NomeEmpresa;
            this.TipoMercado    = pParametro.TipoMercado;
            this.CodigoNegocio  = pParametro.CodigoInstrumento;
            this.QtdVenda       = String.Format(gCultura,"{0:#,0}",pParametro.QtdeAExecVenda);

            if (pParametro.TipoGrupo == "TEDI")
            {
                this.QtdAtual = String.Format(gCultura, "{0:f2}", pParametro.QtdeAtual);
                this.QtdAbertura = String.Format(gCultura, "{0:f2}", pParametro.QtdeDisponivel);
            }
            else
            {
                this.QtdAtual = String.Format(gCultura, "{0:#,0}", pParametro.QtdeAtual);
                this.QtdAbertura = String.Format(gCultura, "{0:#,0}", pParametro.QtdeDisponivel);
            }
            
            this.Carteira       = pParametro.DescricaoCarteira;
            this.CodigoCarteira = pParametro.CodigoCarteira.ToString();
            
            this.QtdCompra      = String.Format(gCultura,"{0:#,0}", pParametro.QtdeAExecCompra);
            
            this.CodigoCliente  = pParametro.IdCliente.ToString();
            this.TipoGrupo      = pParametro.TipoGrupo;
            this.ValorPosicao   = pParametro.ValorPosicao.ToString(gCultura);
            
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

            this.QtdLiquid     = pParametro.QtdeLiquidar.ToString("N0");
            this.CodigoSerie   = pParametro.CodigoSerie;
            this.FatorCotacao  = pParametro.FatorCotacao.DBToString();
            this.QtdDATotal    = pParametro.QtdeDATotal.ToString("N0");
        }

        #endregion

        #region | Métodos

        public static List<TransporteCustodiaInfo> TraduzirCustodiaInfo(List<PosicaoCustodiaInfo.CustodiaMovimento> pParametros)
        {
            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();

            if (pParametros != null)
            {
                foreach (PosicaoCustodiaInfo.CustodiaMovimento lInfo in pParametros)
                {
                    lRetorno.Add(new TransporteCustodiaInfo(lInfo));
                }
            }

            return lRetorno;
        }
        

        public static List<TransporteCustodiaInfo> TraduzirCustodiaInfo(List<MonitoramentoRiscoLucroCustodiaInfo.CustodiaMovimento> pParametros)
        {
            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();

            if (pParametros != null)
            {
                foreach (MonitoramentoRiscoLucroCustodiaInfo.CustodiaMovimento lInfo in pParametros)
                {
                    lRetorno.Add(new TransporteCustodiaInfo(lInfo));
                }
            }

            return lRetorno;
        }

        public static List<TransporteCustodiaInfo> TraduzirCustodiaInfo(List<MonitorCustodiaInfo.CustodiaPosicao> pParametros)
        {
            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();

            if (pParametros != null)
            {
                foreach (MonitorCustodiaInfo.CustodiaPosicao lInfo in pParametros)
                {
                    lRetorno.Add(new TransporteCustodiaInfo(lInfo));
                }
            }

            return lRetorno;
        }

        public static List<TransporteCustodiaInfo> TraduzirCustodiaInfo(List<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo.CustodiaPosicao> pParametros)
        {
            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();
            List<TransporteCustodiaInfo> lTempRetorno = new List<TransporteCustodiaInfo>();

            if (pParametros != null)
            {
                foreach (MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo.CustodiaPosicao lInfo in pParametros)
                {
                    TransporteCustodiaInfo lPosicao = new TransporteCustodiaInfo();

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
                        lTemp.QtdAtual           = (decimal.Parse(lTemp.QtdCompra, gCulturaStatic) - decimal.Parse(lTemp.QtdVenda, gCulturaStatic) ).ToString(gCulturaStatic);
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
                        lPosicao.QtdAbertura         = "0";
                        lPosicao.Empresa             = "n/a";
                        lPosicao.TipoMercado         = lInfo.TipoMercado;
                        lPosicao.CodigoNegocio       = lInfo.CodigoInstrumento;
                        lPosicao.QtdVenda            = lInfo.QtdeAExecVenda.ToString(gCulturaStatic);
                        lPosicao.QtdCompra           = lInfo.QtdeAExecCompra.ToString(gCulturaStatic);
                        lPosicao.QtdAtual            = (decimal.Parse(lPosicao.QtdCompra, gCulturaStatic) - decimal.Parse(lPosicao.QtdVenda, gCulturaStatic) ).ToString(gCulturaStatic);
                        lPosicao.Carteira            = "n/a";
                        lPosicao.CodigoCliente       = lInfo.IdCliente.ToString();
                        lPosicao.TipoGrupo           = lInfo.TipoGrupo;
                        lPosicao.ValorPosicao        = lInfo.ValorPosicao.ToString(gCulturaStatic);
                        lPosicao.CodigoSerie         = lInfo.CodigoSerie;
                        lPosicao.PrecoNegocioCompra  = (lInfo.PrecoNegocioCompra * lInfo.QtdeAExecCompra).ToString(gCulturaStatic);
                        lPosicao.PrecoNegocioVenda   = (lInfo.PrecoNegocioVenda * lInfo.QtdeAExecVenda).ToString(gCulturaStatic);
                        lPosicao.Sentido             = lInfo.Sentido;
                        lPosicao.EhPosicao           = true;

                        lTempRetorno.Add(lPosicao);
                    }
                }

                lRetorno = lTempRetorno;
            }

            return lRetorno;
        }

        public static List<TransporteCustodiaInfo> TraduzirCustodiaInfo(List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> pParametros)
        {
            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();
            List<TransporteCustodiaInfo> lTempRetorno = new List<TransporteCustodiaInfo>();

            if (pParametros != null)
            {
                foreach (MonitorCustodiaInfo.CustodiaPosicaoDiaBMF lInfo in pParametros)
                {
                    TransporteCustodiaInfo lPosicao = new TransporteCustodiaInfo();

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
                        lTemp.Sentido              = lInfo.Sentido;
                        lTemp.EhPosicao            = true;
                        lPosicao.Cotacao           = lInfo.Cotacao.ToString("N2");
                        lPosicao.ValorDeFechamento = lInfo.ValorFechamento.ToString("N2");
                        lPosicao.Resultado         = lInfo.Resultado.ToString("N2");
                        lPosicao.Variacao          = lInfo.Variacao.ToString("N2");
                        //lPosicao.ValorPU           = lInfo.ValorPU.ToString("N2");

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
                        lPosicao.Cotacao            = lInfo.Cotacao.ToString("N2");
                        lPosicao.ValorDeFechamento  = lInfo.ValorFechamento.ToString("N2");
                        lPosicao.Resultado          = lInfo.Resultado.ToString("N2");
                        lPosicao.Variacao           = lInfo.Variacao.ToString("N2");
                        //lPosicao.ValorPU            = lInfo.ValorPU.ToString("N2");
                        lTempRetorno.Add(lPosicao);
                    }
                }

                lRetorno = lTempRetorno;
            }

            return lRetorno;
        }
        #endregion
    }
}