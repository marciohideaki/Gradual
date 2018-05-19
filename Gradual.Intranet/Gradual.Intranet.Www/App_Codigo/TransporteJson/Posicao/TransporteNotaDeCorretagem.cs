using System.Collections.Generic;
using Gradual.OMS.ContaCorrente.Lib.Info;
using System;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Posicao
{
    public class TransporteNotaDeCorretagem
    {
        #region | Propriedades

        public string Agencia { get; set; }

        public string CepCidadeUFCliente { get; set; }

        public string CodCliente { get; set; }

        public string ContaCorrente { get; set; }

        public string CpfCnpj { get; set; }

        public string DvCliente { get; set; }

        public string EnderecoCliente { get; set; }

        public string NomeCliente { get; set; }

        public string NrBanco { get; set; }

        public string NrNota { get; set; }

        public string CompraOpcoes { get; set; }

        public string CompraVista { get; set; }

        public string Corretagem { get; set; }

        public string Corretagem_DC { get; set; }

        public string Debentures { get; set; }

        public string DescOutras { get; set; }

        public string Emolumentos { get; set; }

        public string Emolumentos_DC { get; set; }

        public string EmolumentosPosNeg { get; set; }

        public string IRRFOperacoes { get; set; }

        public string IRRFOperacoes_DC { get; set; }

        public string IRRFOperacoesPosNeg { get; set; }

        public string IRRFSobreDayTrade { get; set; }

        public string IRSobreCorretagem { get; set; }

        public string ISS { get; set; }

        public string ISS_DC { get; set; }

        public string ISSPosNeg { get; set; }

        public string OperacoesFuturo { get; set; }

        public string OperacoesTermo { get; set; }

        public string OperacoesTitulosPublicos { get; set; }

        public string Outras { get; set; }

        public string Outras_DC { get; set; }

        public string TaxaANA { get; set; }

        public string TaxaANA_DC { get; set; }

        public string TaxaANAPosNeg { get; set; }

        public string TaxaDeRegistro { get; set; }

        public string TaxaDeRegistro_DC { get; set; }

        public string TaxaDeRegistroPosNeg { get; set; }

        public string TaxaLiquidacao { get; set; }

        public string TaxaLiquidacao_DC { get; set; }

        public string TaxaLiquidacaoPosNeg { get; set; }

        public string TaxaTerOpcFut { get; set; }

        public string TaxaTerOpcFut_DC { get; set; }

        public string DataEmissaoNota { get; set; }

        public string TaxaTerOpcFutPosNeg { get; set; }

        public string Total_123_A { get; set; }

        public string Total_123_A_DC { get; set; }

        public string TotalBolsaB { get; set; }

        public string TotalBolsaB_DC { get; set; }

        public string ValorAjusteFuturo { get; set; }

        public string ValorDasOperacoes { get; set; }

        public string ValorLiquidoNota { get; set; }

        public string ValorLiquidoNota_DC { get; set; }

        public string ValorLiquidoOperacoes { get; set; }

        public string ValorLiquidoOperacoes_DC { get; set; }

        public string VendaOpcoes { get; set; }

        public string VendaVista { get; set; }

        public string VLBaseOperacoesIRRF { get; set; }

        public string VLBaseOperacoesIRRF_DC { get; set; }

        public string LiquidoPara { get; set; }

        public List<DetalhesNota> DetalhesDaNota { get; set; }

        #endregion

        #region | Estruturas

        public struct DetalhesNota
        {
            public string NomeBolsa { get; set; }

            public string TipoOperacao { get; set; }

            public string TipoMercado { get; set; }

            public string EspecificacaoTitulo { get; set; }

            public string Observacao { get; set; }

            public string Quantidade { get; set; }

            public string ValorNegocioPosNeg { get; set; }

            public string ValorNegocio { get; set; }

            public string ValorTotalPosNeg { get; set; }

            public string ValorTotal { get; set; }

            public string DC { get; set; }
        }

        #endregion

        #region | Construtores

        public TransporteNotaDeCorretagem()
        {
            this.DetalhesDaNota = new List<DetalhesNota>();
        }

        #endregion

        public List<TransporteNotaDeCorretagem> TraduzirLista(List<Gradual.OMS.RelatoriosFinanc.Lib.Dados.NotaDeCorretagemExtratoInfo> pParametro, string pNomeCliente, string pTipoMercado)
        {
            var lRetorno = new List<TransporteNotaDeCorretagem>();

            if (null != pParametro && pParametro.Count > 0)
            {
                var lTransporte = new TransporteNotaDeCorretagem();

                foreach (var lNotaDeCorretagem in pParametro)
                //pParametro.ForEach(lNotaDeCorretagem =>
                {
                    lTransporte = new TransporteNotaDeCorretagem();
                    
                    if (lNotaDeCorretagem.CabecalhoCorretora.DataPregao.ToString("dd/MM/yyyy") == "01/01/0001") 
                        continue;

                    lTransporte.Agencia                  = lNotaDeCorretagem.CabecalhoCliente.Agencia;
                    lTransporte.CepCidadeUFCliente       = lNotaDeCorretagem.CabecalhoCliente.CepCidadeUFCliente;
                    lTransporte.CodCliente               = lNotaDeCorretagem.CabecalhoCliente.CodCliente.ToCodigoClienteFormatado();
                    lTransporte.ContaCorrente            = lNotaDeCorretagem.CabecalhoCliente.ContaCorrente;
                    lTransporte.CpfCnpj                  = lNotaDeCorretagem.CabecalhoCliente.CpfCnpj.ToCpfCnpjString();
                    lTransporte.EnderecoCliente          = lNotaDeCorretagem.CabecalhoCliente.EnderecoCliente.ToStringFormatoNome();
                    lTransporte.NomeCliente              = pNomeCliente.ToStringFormatoNome();
                    lTransporte.NrBanco                  = lNotaDeCorretagem.CabecalhoCliente.NrBanco;
                    lTransporte.NrNota                   = lNotaDeCorretagem.CabecalhoCliente.NrNota.DBToString();
                    lTransporte.DataEmissaoNota          = lNotaDeCorretagem.CabecalhoCorretora.DataPregao.ToString("dd/MM/yyyy") == "01/01/0001" ? "01/01/4000" : lNotaDeCorretagem.CabecalhoCorretora.DataPregao.ToString("dd/MM/yyyy");
                    lTransporte.CompraOpcoes             = Math.Abs(lNotaDeCorretagem.Rodape.CompraOpcoes).ToString("N2");
                    lTransporte.CompraVista              = Math.Abs(lNotaDeCorretagem.Rodape.CompraVista).ToString("N2");
                    lTransporte.Corretagem               = Math.Abs(lNotaDeCorretagem.Rodape.Corretagem).ToString("N2");
                    lTransporte.Corretagem_DC            = "D";
                    lTransporte.Debentures               = Math.Abs(lNotaDeCorretagem.Rodape.Debentures).ToString("N2");
                    lTransporte.DescOutras               = lNotaDeCorretagem.Rodape.DescOutras;
                    lTransporte.DvCliente                = "";
                    lTransporte.Emolumentos              = Math.Abs(lNotaDeCorretagem.Rodape.Emolumentos).ToString("N2");
                    lTransporte.Emolumentos_DC           = "D";
                    lTransporte.IRRFOperacoes            = Math.Abs(lNotaDeCorretagem.Rodape.IRRFOperacoes).ToString("N2");
                    lTransporte.IRRFOperacoes_DC         = lNotaDeCorretagem.Rodape.IRRFOperacoes > 0 ? "C" : "D";
                    lTransporte.IRRFSobreDayTrade        = Math.Abs(lNotaDeCorretagem.Rodape.IRRFSobreDayTrade).ToString("N2");
                    lTransporte.IRSobreCorretagem        = Math.Abs(lNotaDeCorretagem.Rodape.IRSobreCorretagem).ToString("N2");
                    lTransporte.ISS                      = Math.Abs(lNotaDeCorretagem.Rodape.ISS).ToString("N2");
                    lTransporte.ISS_DC                   = lNotaDeCorretagem.Rodape.ISS_DC;
                    lTransporte.OperacoesFuturo          = Math.Abs(lNotaDeCorretagem.Rodape.OperacoesFuturo).ToString("N2");
                    lTransporte.OperacoesTermo           = Math.Abs(lNotaDeCorretagem.Rodape.OperacoesTermo).ToString("N2");
                    lTransporte.OperacoesTitulosPublicos = Math.Abs(lNotaDeCorretagem.Rodape.OperacoesTitulosPublicos).ToString("N2");
                    lTransporte.Outras                   = Math.Abs(lNotaDeCorretagem.Rodape.Outras).ToString("N2");
                    lTransporte.Outras_DC                = lNotaDeCorretagem.Rodape.Outras > 0 ? "C" : "D";
                    lTransporte.TaxaANA                  = Math.Abs(lNotaDeCorretagem.Rodape.TaxaANA).ToString("N2");
                    lTransporte.TaxaANA_DC               = lNotaDeCorretagem.Rodape.TaxaANA > 0 ? "C" : "D";
                    lTransporte.TaxaDeRegistro           = Math.Abs(lNotaDeCorretagem.Rodape.TaxaDeRegistro).ToString("N2");
                    lTransporte.TaxaDeRegistro_DC        = lNotaDeCorretagem.Rodape.TaxaDeRegistro_DC;
                    lTransporte.TaxaLiquidacao           = Math.Abs(lNotaDeCorretagem.Rodape.TaxaLiquidacao).ToString("N2");
                    lTransporte.TaxaLiquidacao_DC        = lNotaDeCorretagem.Rodape.TaxaLiquidacao_DC;
                    lTransporte.TaxaTerOpcFut            = Math.Abs(lNotaDeCorretagem.Rodape.TaxaTerOpcFut).ToString("N2");
                    lTransporte.TaxaTerOpcFut_DC         = lNotaDeCorretagem.Rodape.TaxaTerOpcFut > 0 ? "C" : "D";
                    lTransporte.Total_123_A              = Math.Abs(lNotaDeCorretagem.Rodape.Total_123_A).ToString("N2");
                    lTransporte.Total_123_A_DC           = lNotaDeCorretagem.Rodape.Total_123_A > 0 ? "C" : "D";
                    lTransporte.TotalBolsaB              = Math.Abs(lNotaDeCorretagem.Rodape.TotalBolsaB).ToString("N2");
                    lTransporte.TotalBolsaB_DC           = lNotaDeCorretagem.Rodape.TotalBolsaB > 0 ? "C" : "D";
                    lTransporte.ValorAjusteFuturo        = Math.Abs(lNotaDeCorretagem.Rodape.ValorAjusteFuturo).ToString("N2");
                    lTransporte.ValorDasOperacoes        = Math.Abs(lNotaDeCorretagem.Rodape.ValorDasOperacoes).ToString("N2");
                    lTransporte.ValorLiquidoNota         = Math.Abs(lNotaDeCorretagem.Rodape.ValorLiquidoNota).ToString("N2");
                    lTransporte.ValorLiquidoNota_DC      = lNotaDeCorretagem.Rodape.ValorLiquidoNota > 0 ? "C" : "D";
                    lTransporte.ValorLiquidoOperacoes    = Math.Abs(lNotaDeCorretagem.Rodape.ValorLiquidoOperacoes).ToString("N2");
                    lTransporte.ValorLiquidoOperacoes_DC = lNotaDeCorretagem.Rodape.ValorLiquidoOperacoes > 0 ? "C" : "D";
                    lTransporte.VendaOpcoes              = Math.Abs(lNotaDeCorretagem.Rodape.VendaOpcoes).ToString("N2");
                    lTransporte.VendaVista               = Math.Abs(lNotaDeCorretagem.Rodape.VendaVista).ToString("N2");
                    lTransporte.VLBaseOperacoesIRRF      = Math.Abs(lNotaDeCorretagem.Rodape.VLBaseOperacoesIRRF).ToString("N2");
                    lTransporte.VLBaseOperacoesIRRF_DC   = lNotaDeCorretagem.Rodape.VLBaseOperacoesIRRF > 0 ? "C" : "D";
                    lTransporte.LiquidoPara              = lNotaDeCorretagem.Rodape.DataLiquidoPara.ToString("dd/MM/yyyy"); //this.RecuperarDataLiquidoPara(pTipoMercado, lNotaDeCorretagem.CabecalhoCorretora.DataPregao);

                    if (null != lNotaDeCorretagem.ListaNotaDeCorretagemExtratoInfo && lNotaDeCorretagem.ListaNotaDeCorretagemExtratoInfo.Count > 0)
                        lNotaDeCorretagem.ListaNotaDeCorretagemExtratoInfo.ForEach(lDetalhesNota =>
                        {
                            lTransporte.DetalhesDaNota.Add(new DetalhesNota()
                            {
                                DC                  = lDetalhesNota.DC,
                                EspecificacaoTitulo = string.Concat(lDetalhesNota.NomeEmpresa, " ", lDetalhesNota.EspecificacaoTitulo),
                                NomeBolsa           = lDetalhesNota.NomeBolsa,
                                Observacao          = lDetalhesNota.Observacao,
                                Quantidade          = Math.Abs(lDetalhesNota.Quantidade).ToString("N0"),
                                TipoMercado         = lDetalhesNota.TipoMercado,
                                TipoOperacao        = lDetalhesNota.TipoOperacao,
                                ValorNegocio        = lDetalhesNota.ValorNegocio.ToString("N2"),
                                ValorTotal          = lDetalhesNota.ValorTotal.ToString("N2"),
                            });
                        });

                    lRetorno.Add(lTransporte);
                }
            }

            return lRetorno;
        }

        private string RecuperarDataLiquidoPara(string pTipoMercado, DateTime pDataNota)
        {
            var lRetorno = "OPC".Equals(pTipoMercado) ? pDataNota.AddDays(1) : pDataNota.AddDays(3);

            if (DayOfWeek.Wednesday.Equals(pDataNota)
            || (DayOfWeek.Thursday.Equals(pDataNota.DayOfWeek))
            || (DayOfWeek.Friday.Equals(pDataNota.DayOfWeek)))
                lRetorno = lRetorno.AddDays(2); //--> Adicionando o período do final de semana.

            return lRetorno.ToString("dd/MM/yyyy");
        }
    }
}