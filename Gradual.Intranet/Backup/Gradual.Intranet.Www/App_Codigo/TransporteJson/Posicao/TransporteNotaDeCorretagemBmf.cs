using System.Collections.Generic;
using Gradual.OMS.ContaCorrente.Lib.Info;
using System;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Posicao
{
    public class TransporteNotaDeCorretagemBmf
    {
        #region | Propriedades
        public string Agencia               { get; set; }
        public string CepCidadeUFCliente    { get; set; }
        public string CodCliente            { get; set; }
        public string ContaCorrente         { get; set; }
        public string CpfCnpj               { get; set; }
        public string DvCliente             { get; set; }
        public string EnderecoCliente       { get; set; }
        public string NomeCliente           { get; set; }
        public string NrBanco               { get; set; }
        public string NrNota                { get; set; }
        public string DataPregao            { get; set; }
        public string VendaDisponivel       { get; set; }
        public string CompraDisponivel      { get; set; }
        public string VendaOpcoes           { get; set; }
        public string CompraOpcoes          { get; set; }
        public string ValorNegocios         { get; set; }
        public string IRRF                  { get; set; }
        public string IRRFDayTrade          { get; set; }
        public string TaxaOperacional       { get; set; }
        public string TaxaRegistroBmf       { get; set; }
        public string TaxaBmf               { get; set; }
        public string ISS                   { get; set; }
        public string AjustePosicao         { get; set; }
        public string AjusteDayTrade        { get; set; }
        public string TotalDespesas         { get; set; }
        public string IrrfCorretagem        { get; set; }
        public string TotalContaNormal      { get; set; }
        public string TotalLiquido          { get; set; }
        public string TotalLiquidoNota      { get; set; }

        public List<DetalhesNotaBmf> DetalhesDaNotaBmf { get; set; }

        #endregion

        #region | Estruturas

        public struct DetalhesNotaBmf
        {
            public string Sentido           { get; set; }
            public string Mercadoria        { get; set; }
            public string Mercadoria_Serie  { get; set; }
            public string Vencimento        { get; set; }
            public string Quantidade        { get; set; }
            public string PrecoAjuste       { get; set; }
            public string TipoNegocio       { get; set; }
            public string ValorOperacao     { get; set; }
            public string DC                { get; set; }
            public string TaxaOperacional   { get; set; }
            public string Observacao        { get; set; }
        }

        #endregion

        #region | Construtores

        public TransporteNotaDeCorretagemBmf()
        {
            this.DetalhesDaNotaBmf = new List<DetalhesNotaBmf>();
        }

        #endregion

        public List<TransporteNotaDeCorretagemBmf> TraduzirLista(List<Gradual.OMS.RelatoriosFinanc.Lib.Dados.NotaDeCorretagemExtratoBmfInfo> pParametro, string pNomeCliente, string pTipoMercado, string CodigoCliente, string DataPregao="")
        {
            var lRetorno = new List<TransporteNotaDeCorretagemBmf>();

            if (null != pParametro && pParametro.Count > 0)
            {
                var lTransporte = new TransporteNotaDeCorretagemBmf();

                foreach (var lNotaDeCorretagem in pParametro)
                //pParametro.ForEach(lNotaDeCorretagem =>
                {
                    lTransporte = new TransporteNotaDeCorretagemBmf();

                    if (lNotaDeCorretagem.Rodape.DataPregao.ToString("dd/MM/yyyy") == "01/01/0001") continue;

                    lTransporte.Agencia                  = lNotaDeCorretagem.CabecalhoCliente.Agencia;
                    lTransporte.CepCidadeUFCliente       = lNotaDeCorretagem.CabecalhoCliente.CepCidadeUFCliente;
                    lTransporte.CodCliente               = CodigoCliente.ToCodigoClienteFormatado();
                    lTransporte.ContaCorrente            = lNotaDeCorretagem.CabecalhoCliente.ContaCorrente;
                    lTransporte.CpfCnpj                  = lNotaDeCorretagem.Rodape.DsCpfCnpj.ToCpfCnpjString();
                    lTransporte.EnderecoCliente          = lNotaDeCorretagem.CabecalhoCliente.EnderecoCliente.ToStringFormatoNome();
                    lTransporte.NomeCliente              = pNomeCliente.ToStringFormatoNome();
                    lTransporte.NrBanco                  = lNotaDeCorretagem.CabecalhoCliente.NrBanco;
                    lTransporte.NrNota                   = lNotaDeCorretagem.Rodape.NumeroDaNota;
                    lTransporte.DataPregao               = lNotaDeCorretagem.Rodape.DataPregao.ToString("dd/MM/yyyy") == "01/01/0001" ? "01/01/4000" : lNotaDeCorretagem.Rodape.DataPregao.ToString("dd/MM/yyyy");
                    lTransporte.VendaDisponivel          = Math.Abs(lNotaDeCorretagem.Rodape.VendaDisponivel )  .ToString();
                    lTransporte.CompraDisponivel         = Math.Abs(lNotaDeCorretagem.Rodape.CompraDisponivel)  .ToString();
                    lTransporte.VendaOpcoes              = Math.Abs(lNotaDeCorretagem.Rodape.VendaOpcoes )      .ToString();
                    lTransporte.CompraOpcoes             = Math.Abs(lNotaDeCorretagem.Rodape.CompraOpcoes)      .ToString();
                    lTransporte.ValorNegocios            = Math.Abs(lNotaDeCorretagem.Rodape.ValorNegocios )    .ToString();
                    lTransporte.IRRF                     = Math.Abs(lNotaDeCorretagem.Rodape.IRRF)              .ToString();
                    lTransporte.IRRFDayTrade             = Math.Abs(lNotaDeCorretagem.Rodape.IRRFDayTrade )     .ToString();
                    lTransporte.TaxaOperacional          = Math.Abs(lNotaDeCorretagem.Rodape.TaxaOperacional )  .ToString();
                    lTransporte.TaxaRegistroBmf          = Math.Abs(lNotaDeCorretagem.Rodape.TaxaRegistroBmf )  .ToString();
                    lTransporte.TaxaBmf                  = Math.Abs(lNotaDeCorretagem.Rodape.TaxaBmf)           .ToString();
                    lTransporte.ISS                      = Math.Abs(lNotaDeCorretagem.Rodape.ISS)               .ToString();
                    lTransporte.AjustePosicao            = Math.Abs(lNotaDeCorretagem.Rodape.AjustePosicao)     .ToString();
                    lTransporte.AjusteDayTrade           = Math.Abs(lNotaDeCorretagem.Rodape.AjusteDayTrade)    .ToString();
                    lTransporte.TotalDespesas            = Math.Abs(lNotaDeCorretagem.Rodape.TotalDespesas)     .ToString();
                    lTransporte.IrrfCorretagem           = Math.Abs(lNotaDeCorretagem.Rodape.IrrfCorretagem)    .ToString();
                    lTransporte.TotalContaNormal         = Math.Abs(lNotaDeCorretagem.Rodape.TotalContaNormal ) .ToString();
                    lTransporte.TotalLiquido             = Math.Abs(lNotaDeCorretagem.Rodape.TotalLiquido)      .ToString();
                    lTransporte.TotalLiquidoNota         = Math.Abs(lNotaDeCorretagem.Rodape.TotalLiquidoNota ) .ToString();

                    //this.RecuperarDataLiquidoPara(pTipoMercado, lNotaDeCorretagem.CabecalhoCorretora.DataPregao);

                    if (lNotaDeCorretagem.ListaNotaDeCorretagemExtratoBmfInfo != null && lNotaDeCorretagem.ListaNotaDeCorretagemExtratoBmfInfo.Count > 0)
                        lNotaDeCorretagem.ListaNotaDeCorretagemExtratoBmfInfo.ForEach(lDetalhesNota =>
                        {
                            lTransporte.DetalhesDaNotaBmf.Add(new DetalhesNotaBmf()
                            {

                                Sentido            = lDetalhesNota.Sentido,
                                Mercadoria         = lDetalhesNota.Mercadoria,
                                Mercadoria_Serie   = lDetalhesNota.Mercadoria_Serie,
                                Vencimento         = lDetalhesNota.Vencimento.ToString("dd/MM/yyyy"),
                                Quantidade         = Math.Abs( lDetalhesNota.Quantidade).ToString(),
                                PrecoAjuste        = lDetalhesNota.PrecoAjuste.ToString("N4"),
                                TipoNegocio        = lDetalhesNota.TipoNegocio,
                                ValorOperacao      = Math.Abs(lDetalhesNota.ValorOperacao).ToString("N2"),
                                DC                 = lDetalhesNota.DC,
                                TaxaOperacional    = Math.Abs( lDetalhesNota.TaxaOperacional).ToString("N2"),
                                Observacao         = lDetalhesNota.Observacao
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