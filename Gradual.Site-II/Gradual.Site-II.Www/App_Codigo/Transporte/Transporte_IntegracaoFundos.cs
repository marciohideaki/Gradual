using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www.Transporte
{
    public class Transporte_IntegracaoFundos
    {
        public string IdProduto             { get; set; }

        public string IdCategoria           { get; set; }

        public string Fundo                 { get; set; }

        public string HorarioLimite         { get; set; }

        public string AplicacaoInicial { get; set; }

        public string AplicacaoAdicional { get; set; }

        public string SaldoMinimo { get; set; }

        public string ResgateMinimo { get; set; }

        public string PorcentagemCDI { get; set; }

        public string RentabilidadeDia      { get; set; }
        
        public string RentabilidadeMes      { get; set; }

        public string RentabilidadeAno      { get; set; }

        public string Rentabilidade12meses  { get; set; }

        public string DiasConversaoResgate  { get; set; }

        public string DiasConversaoResgateAntecipado { get; set; }

        public string DiasConversaoAplicacao { get; set; }

        public string MinimoAplicacaoAdicional { get; set; }

        public string DiasPagamentoResgate  { get; set; }

        public string TaxaAdministracao     { get; set; }

        public string TaxaAdministracaoMaxima { get; set; }

        public string TaxaPerformance       { get; set; }

        public string TaxaResgateAntecipado { get; set; }

        public string PatrimonioLiquido     { get; set; }

        public string NomeArquivoProspecto  { get; set; }

        public string Risco                 { get; set; }

        public string SaibaMais             { get; set; }

        public string Aplicar               { get; set; }

        public string PathTermoPF             { get; set; }

        public string FullPathTermoPF { get { return "/Resc/PDFs/AdesaoFundos/" + this.PathTermoPF; } }

        public string PathTermoPJ             { get; set; }

        public string CodigoAnbima          { get; set; }

        public string BotaoAplicar
        {
            get
            {
                try
                {
                    if (!ConfiguracoesValidadas.FundosInaplicaveis.Contains(Convert.ToInt32(this.IdProduto)))
                    {
                        return string.Format("<a class='botao btn-invista' href='/MinhaConta/Produtos/Fundos/Aplicar.aspx?idFundo={0}'>Aplicar</a>", this.IdProduto);
                    }
                    else
                    {
                        return "";
                    }
                }
                catch
                {
                    return "";
                }
            }
        }

        public Transporte_IntegracaoFundos()
        {
            Fundo
                = IdProduto
                = HorarioLimite
                = RentabilidadeAno
                = RentabilidadeMes
                = RentabilidadeDia
                = Risco
                = SaibaMais
                = Aplicar
                = NomeArquivoProspecto
                = PathTermoPF
                = PathTermoPJ
                = CodigoAnbima
                = string.Empty;
        }
    }
}