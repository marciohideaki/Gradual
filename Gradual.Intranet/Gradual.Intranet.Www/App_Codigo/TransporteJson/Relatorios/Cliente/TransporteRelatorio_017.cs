using System;
using System.Collections.Generic;
using System.Globalization;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente
{
    public class TransporteRelatorio_017
    {
        #region | Atributos

        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        #endregion

        #region | Propriedades

        public string codigo { get; set; }

        public string nome { get; set; }

        public string TipoMercado { get; set; }

        public CabecalhoCliente NotaCabecalhoCliente { get; set; }

        public CabecalhoCorretora NotaCabecalhoCorretora { get; set; }

        public RodapeCorretora NotaRodapeCorretora { get; set; }

        public List<MovimentoNota> NotaMovimento { get; set; }

        #endregion

        #region | Estruturas

        public struct CabecalhoCliente
        {
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
        }

        public struct CabecalhoCorretora
        {
            public string CpfCnpj { get; set; }

            public string DataPregao { get; set; }

            public string EmailCorretora { get; set; }

            public string EnderecoCorretora { get; set; }

            public string NomeCorretora { get; set; }

            public string NumeroNota { get; set; }

            public string SiteCorretora { get; set; }

            public string TelefonesCorretora { get; set; }
        }

        public struct RodapeCorretora
        {
            public string CompraOpcoes { get; set; }

            public string CompraVista { get; set; }

            public string Corretagem { get; set; }

            public string Corretagem_DC { get; set; }

            public string CorretagemPosNeg { get; set; }

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

            public string TaxaTerOpcFutPosNeg { get; set; }

            public string Total_123_A { get; set; }

            public string Total_123_A_DC { get; set; }

            public string Total_123_APosNeg { get; set; }

            public string TotalBolsaB { get; set; }

            public string TotalBolsaB_DC { get; set; }

            public string TotalBolsaBPosNeg { get; set; }

            public string TotalBolsaBPosNeg_DC { get; set; }

            public string ValorAjusteFuturo { get; set; }

            public string ValorDasOperacoes { get; set; }

            public string ValorLiquidoNota { get; set; }

            public string ValorLiquidoNota_DC { get; set; }

            public string ValorLiquidoOperacoes { get; set; }

            public string ValorLiquidoOperacoes_DC { get; set; }

            public string ValorLiquidoOperacoesPosNeg { get; set; }

            public string VendaOpcoes { get; set; }

            public string VendaVista { get; set; }

            public string VLBaseOperacoesIRRF { get; set; }

            public string VLBaseOperacoesIRRF_DC { get; set; }

            public string TaxaDeLiquidacao_DC { get; set; }

            public string LiquidoPara { get; set; }

            public string IRRFSobreDayTradeBase { get; set; }

            public string IRRFSobreDayTradeProjecao { get; set; }

            public string IRRFSemOperacoesBase { get; set; }

            public string IRRFSemOperacoesValor { get; set; }
        }

        public struct MovimentoNota
        {
            public string CodigoCliente { get; set; }

            public string CodigoNegocio { get; set; }

            public string DC { get; set; }

            public string EspecificacaoTitulo { get; set; }

            public string NomeBolsa { get; set; }

            public string NomeEmpresa { get; set; }

            public string Observacao { get; set; }

            public string Quantidade { get; set; }

            public string TipoMercado { get; set; }

            public string TipoOperacao { get; set; }

            public string ValorNegocio { get; set; }

            public string ValorTotal { get; set; }

            public string ValorNegocioPosNeg { get; set; }

            public string ValorTotalPosNeg { get; set; }
        }

        #endregion

        #region | Construtores

        public TransporteRelatorio_017()
        {
            this.NotaCabecalhoCliente = new CabecalhoCliente();

            this.NotaCabecalhoCorretora = new CabecalhoCorretora();

            this.NotaRodapeCorretora = new RodapeCorretora();

            this.NotaMovimento = new List<MovimentoNota>();
        }

        public TransporteRelatorio_017(string pTipoMercado)
        {
            this.NotaCabecalhoCliente = new CabecalhoCliente();

            this.NotaCabecalhoCorretora = new CabecalhoCorretora();

            this.NotaRodapeCorretora = new RodapeCorretora();

            this.NotaMovimento = new List<MovimentoNota>();

            this.TipoMercado = pTipoMercado;
        }

        #endregion

        #region | Métodos

        public List<TransporteRelatorio_017> TraduzirListaConsulta(List<ClientePorAssessorInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_017>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(lcpi =>
                {
                    lRetorno.Add(new TransporteRelatorio_017()
                    {
                        codigo = lcpi.CdCodigoBovespa.DBToString(),
                        nome = string.Format("{0} {1}", lcpi.CdCodigoBovespa.ToCodigoClienteFormatado(), lcpi.DsNome.ToStringFormatoNome()),
                    });
                });

            return lRetorno;
        }

        public List<TransporteRelatorio_017> TraduzirLista(List<NotaDeCorretagemExtratoInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_017>();
            var lTransporteRelatorio_017 = new TransporteRelatorio_017();

            if (null != pParametros && pParametros.Count > 0)
                foreach (NotaDeCorretagemExtratoInfo lNotaDeCorretagemInfo in pParametros)
                {
                    if (null != lNotaDeCorretagemInfo.CabecalhoCliente.CpfCnpj)
                    {
                        lTransporteRelatorio_017 = new TransporteRelatorio_017();

                        //--> Carregando os dados de cabeçalho do cliente.
                        lTransporteRelatorio_017.NotaCabecalhoCliente = new CabecalhoCliente()
                        {
                            Agencia = lNotaDeCorretagemInfo.CabecalhoCliente.Agencia,
                            CepCidadeUFCliente = lNotaDeCorretagemInfo.CabecalhoCliente.CepCidadeUFCliente,
                            CodCliente = lNotaDeCorretagemInfo.CabecalhoCliente.CodCliente.DBToString(),
                            ContaCorrente = lNotaDeCorretagemInfo.CabecalhoCliente.ContaCorrente,
                            CpfCnpj = lNotaDeCorretagemInfo.CabecalhoCliente.CpfCnpj.ToCpfCnpjString(),
                            DvCliente = lNotaDeCorretagemInfo.CabecalhoCliente.DvCliente.DBToString(),
                            EnderecoCliente = lNotaDeCorretagemInfo.CabecalhoCliente.EnderecoCliente.ToStringFormatoNome(),
                            NomeCliente = lNotaDeCorretagemInfo.CabecalhoCliente.NomeCliente.ToStringFormatoNome(),
                            NrBanco = lNotaDeCorretagemInfo.CabecalhoCliente.NrBanco,
                            NrNota = lNotaDeCorretagemInfo.CabecalhoCliente.NrNota.DBToString(),
                        };

                        //--> Carregando os dados de cabeçalho da corretora.
                        lTransporteRelatorio_017.NotaCabecalhoCorretora = new CabecalhoCorretora()
                        {
                            CpfCnpj = lNotaDeCorretagemInfo.CabecalhoCorretora.CpfCnpj.ToCpfCnpjString(),
                            DataPregao = lNotaDeCorretagemInfo.CabecalhoCorretora.DataPregao.ToString("dd/MM/yyyy", gCultureInfo),
                            EmailCorretora = lNotaDeCorretagemInfo.CabecalhoCorretora.EmailCorretora.ToLower(),
                            EnderecoCorretora = lNotaDeCorretagemInfo.CabecalhoCorretora.EnderecoCorretora.ToStringFormatoNome(),
                            NomeCorretora = lNotaDeCorretagemInfo.CabecalhoCorretora.NomeCorretora,
                            NumeroNota = lNotaDeCorretagemInfo.CabecalhoCorretora.NumeroNota,
                            SiteCorretora = lNotaDeCorretagemInfo.CabecalhoCorretora.SiteCorretora,
                            TelefonesCorretora = lNotaDeCorretagemInfo.CabecalhoCorretora.TelefonesCorretora,
                        };

                        //--> Carregando os dados de rodapé.
                        lTransporteRelatorio_017.NotaRodapeCorretora = new RodapeCorretora()
                        {
                            Debentures = lNotaDeCorretagemInfo.Rodape.Debentures.ToString("N2", gCultureInfo),
                            ValorLiquidoOperacoes = Math.Abs(lNotaDeCorretagemInfo.Rodape.ValorLiquidoOperacoes).ToString("N2", gCultureInfo),
                            VendaVista = lNotaDeCorretagemInfo.Rodape.VendaVista.ToString("N2", gCultureInfo),
                            TaxaDeRegistro = Math.Abs(lNotaDeCorretagemInfo.Rodape.TaxaDeRegistro).ToString("N2", gCultureInfo),
                            CompraVista = lNotaDeCorretagemInfo.Rodape.CompraVista.ToString("N2", gCultureInfo),
                            TaxaLiquidacao = Math.Abs(lNotaDeCorretagemInfo.Rodape.TaxaLiquidacao).ToString("N2", gCultureInfo),
                            CompraOpcoes = lNotaDeCorretagemInfo.Rodape.CompraOpcoes.ToString("N2", gCultureInfo),
                            Total_123_A = Math.Abs(lNotaDeCorretagemInfo.Rodape.Total_123_A).ToString("N2", gCultureInfo),

                            VendaOpcoes = lNotaDeCorretagemInfo.Rodape.VendaOpcoes.ToString("N2", gCultureInfo),
                            TaxaTerOpcFut = Math.Abs(lNotaDeCorretagemInfo.Rodape.TaxaTerOpcFut).ToString("N2", gCultureInfo),
                            OperacoesTermo = lNotaDeCorretagemInfo.Rodape.OperacoesTermo.ToString("N2", gCultureInfo),
                            TaxaANA = Math.Abs(lNotaDeCorretagemInfo.Rodape.TaxaANA).ToString("N2", gCultureInfo),

                            OperacoesFuturo = lNotaDeCorretagemInfo.Rodape.OperacoesFuturo.ToString("N2", gCultureInfo),
                            Emolumentos = Math.Abs(lNotaDeCorretagemInfo.Rodape.Emolumentos).ToString("N2", gCultureInfo),
                            OperacoesTitulosPublicos = lNotaDeCorretagemInfo.Rodape.OperacoesTitulosPublicos.ToString("N2", gCultureInfo),
                            TotalBolsaB = Math.Abs(lNotaDeCorretagemInfo.Rodape.TotalBolsaB).ToString("N2", gCultureInfo),
                            ValorDasOperacoes = lNotaDeCorretagemInfo.Rodape.ValorDasOperacoes.ToString("N2", gCultureInfo),
                            Corretagem = Math.Abs(lNotaDeCorretagemInfo.Rodape.Corretagem).ToString("N2", gCultureInfo),

                            ValorAjusteFuturo = lNotaDeCorretagemInfo.Rodape.ValorAjusteFuturo.ToString("N2", gCultureInfo),
                            ISS = Math.Abs(lNotaDeCorretagemInfo.Rodape.ISS).ToString("N2", gCultureInfo),
                            IRSobreCorretagem = lNotaDeCorretagemInfo.Rodape.IRSobreCorretagem.ToString("N2", gCultureInfo),
                            VLBaseOperacoesIRRF = lNotaDeCorretagemInfo.Rodape.VLBaseOperacoesIRRF.ToString("N2", gCultureInfo),
                            IRRFOperacoes = Math.Abs(lNotaDeCorretagemInfo.Rodape.IRRFOperacoes).ToString("N2", gCultureInfo),
                            IRRFSobreDayTrade = lNotaDeCorretagemInfo.Rodape.IRRFSobreDayTrade.ToString("N2", gCultureInfo),

                            Outras = Math.Abs(lNotaDeCorretagemInfo.Rodape.Outras).ToString("N2", gCultureInfo),
                            Outras_DC = lNotaDeCorretagemInfo.Rodape.Outras > 0 ? "C" : "D",

                            ValorLiquidoNota = Math.Abs(lNotaDeCorretagemInfo.Rodape.ValorLiquidoNota).ToString("N2", gCultureInfo),
                            ValorLiquidoNota_DC = lNotaDeCorretagemInfo.Rodape.ValorLiquidoNota > 0 ? "C" : "D",

                            ValorLiquidoOperacoes_DC = lNotaDeCorretagemInfo.Rodape.ValorLiquidoOperacoes > 0 ? "C" : "D",
                            TaxaDeRegistro_DC = lNotaDeCorretagemInfo.Rodape.TaxaDeRegistro > 0 ? "C" : "D",
                            TaxaDeLiquidacao_DC = lNotaDeCorretagemInfo.Rodape.TaxaLiquidacao > 0 ? "C" : "D",
                            Total_123_A_DC = lNotaDeCorretagemInfo.Rodape.Total_123_A > 0 ? "C" : "D",
                            TaxaTerOpcFut_DC = lNotaDeCorretagemInfo.Rodape.TaxaTerOpcFut > 0 ? "C" : "D",
                            TaxaANA_DC = lNotaDeCorretagemInfo.Rodape.TaxaANA > 0 ? "C" : "D",
                            TaxaLiquidacao_DC = lNotaDeCorretagemInfo.Rodape.TaxaLiquidacao > 0 ? "C" : "D",
                            Emolumentos_DC = "D",

                            TotalBolsaBPosNeg = lNotaDeCorretagemInfo.Rodape.TotalBolsaBPosNeg.ToString("N2", gCultureInfo),
                            TotalBolsaBPosNeg_DC = lNotaDeCorretagemInfo.Rodape.TotalBolsaBPosNeg > 0 ? "C" : "D",

                            Corretagem_DC = "D",
                            ISS_DC = lNotaDeCorretagemInfo.Rodape.ISS > 0 ? "C" : "D",
                            IRRFOperacoes_DC = lNotaDeCorretagemInfo.Rodape.IRRFOperacoes > 0 ? "C" : "D",

                            LiquidoPara = lNotaDeCorretagemInfo.Rodape.DataLiquidoPara.ToString("dd/MM/yyyy", gCultureInfo),

                            ValorLiquidoOperacoesPosNeg = lNotaDeCorretagemInfo.Rodape.ValorLiquidoOperacoesPosNeg.ToString("N2", gCultureInfo),
                            TaxaDeRegistroPosNeg = lNotaDeCorretagemInfo.Rodape.TaxaDeRegistroPosNeg.ToString("N2", gCultureInfo),
                            TaxaLiquidacaoPosNeg = lNotaDeCorretagemInfo.Rodape.TaxaLiquidacaoPosNeg.ToString("N2", gCultureInfo),
                            Total_123_APosNeg = lNotaDeCorretagemInfo.Rodape.Total_123_APosNeg.ToString("N2", gCultureInfo),
                            TaxaTerOpcFutPosNeg = lNotaDeCorretagemInfo.Rodape.TaxaTerOpcFutPosNeg.ToString("N2", gCultureInfo),
                            TaxaANAPosNeg = lNotaDeCorretagemInfo.Rodape.TaxaANAPosNeg.ToString("N2", gCultureInfo),
                            EmolumentosPosNeg = lNotaDeCorretagemInfo.Rodape.EmolumentosPosNeg.ToString("N2", gCultureInfo),
                            CorretagemPosNeg = lNotaDeCorretagemInfo.Rodape.CorretagemPosNeg.ToString("N2", gCultureInfo),
                            ISSPosNeg = lNotaDeCorretagemInfo.Rodape.ISSPosNeg.ToString("N2", gCultureInfo),
                            IRRFOperacoesPosNeg = lNotaDeCorretagemInfo.Rodape.IRRFOperacoesPosNeg.ToString("N2", gCultureInfo),
                            IRRFSemOperacoesBase = lNotaDeCorretagemInfo.Rodape.IRRFSemOperacoesBase.ToString("N2", gCultureInfo),
                            IRRFSemOperacoesValor = lNotaDeCorretagemInfo.Rodape.IRRFSemOperacoesValor.ToString("N2", gCultureInfo),
                            IRRFSobreDayTradeBase = lNotaDeCorretagemInfo.Rodape.IRRFSobreDayTradeBase.ToString("N2", gCultureInfo),
                            IRRFSobreDayTradeProjecao = lNotaDeCorretagemInfo.Rodape.IRRFSobreDayTradeProjecao.ToString("N2", gCultureInfo),
                        };

                        if (null != lNotaDeCorretagemInfo.ListaNotaDeCorretagemExtratoInfo && lNotaDeCorretagemInfo.ListaNotaDeCorretagemExtratoInfo.Count > 0)
                            lNotaDeCorretagemInfo.ListaNotaDeCorretagemExtratoInfo.ForEach(lMovimento =>
                            {
                                lTransporteRelatorio_017.NotaMovimento.Add(new MovimentoNota()
                                {
                                    CodigoCliente = lMovimento.CodigoCliente.DBToString(),
                                    CodigoNegocio = lMovimento.CodigoNegocio,
                                    DC = lMovimento.DC,
                                    EspecificacaoTitulo = string.Concat(lMovimento.CodigoNegocio, " ", lMovimento.EspecificacaoTitulo),
                                    NomeBolsa = lMovimento.NomeBolsa,
                                    NomeEmpresa = lMovimento.NomeEmpresa,
                                    Observacao = string.IsNullOrWhiteSpace(lMovimento.Observacao) ? string.Empty : lMovimento.Observacao.Replace("N", string.Empty),
                                    Quantidade = lMovimento.Quantidade.ToString("N0", gCultureInfo),
                                    TipoMercado = lMovimento.TipoMercado,
                                    TipoOperacao = lMovimento.TipoOperacao,
                                    ValorNegocio = lMovimento.ValorNegocio.ToString("N2", gCultureInfo),
                                    ValorTotal = lMovimento.ValorTotal.ToString("N2", gCultureInfo),
                                    ValorNegocioPosNeg = lMovimento.ValorNegocio >= 0 ? "ValorPositivo" : "ValorNegativo",
                                    ValorTotalPosNeg = lMovimento.ValorTotal >= 0 ? "ValorPositivo" : "ValorNegativo",
                                });
                            });

                        lRetorno.Add(lTransporteRelatorio_017);
                    }
                }

            return lRetorno;
        }

        #endregion
    }
}