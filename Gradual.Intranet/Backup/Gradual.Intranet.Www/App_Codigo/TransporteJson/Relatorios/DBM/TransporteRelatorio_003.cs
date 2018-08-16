using System;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using System.Collections.Generic;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM
{
    public class TransporteRelatorio_003_DadosCadastrais
    {
        public string NomeCliente { get; set; }

        public string Estado { get; set; }

        public string Cidade { get; set; }

        public string Tipo { get; set; }

        public string DataUltimaOperacao { get; set; }

        public string DataDeCadastro { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Telefone { get; set; }

        public string Ramal { get; set; }

        public string Celular1 { get; set; }

        public string Celular2 { get; set; }

        public string Email { get; set; }

        public TransporteRelatorio_003_DadosCadastrais(ResumoDoClienteDadosCadastraisInfo pParametro)
        {
            this.Estado = pParametro.DsUF.ToUpper();
            this.Cidade = pParametro.NmCidade.ToStringFormatoNome();
            this.Tipo = pParametro.DsTipoCliente;
            this.DataUltimaOperacao = pParametro.DtUltimaOperacao == DateTime.MinValue ? "" : pParametro.DtUltimaOperacao.ToString("dd/MM/yyyy");
            this.DataDeCadastro = pParametro.DtCriacao == DateTime.MinValue ? "" : pParametro.DtCriacao.ToString("dd/MM/yyyy");
            this.NomeCliente = pParametro.NmCliente.ToStringFormatoNome();
            this.Logradouro = pParametro.NmLogradouro.ToStringFormatoNome();
            this.Numero = pParametro.NrPredio;
            this.Complemento = pParametro.NmComplementoEndereco;
            this.Bairro = pParametro.NmBairro;
            this.Telefone = string.Format("({0}) {1}", pParametro.CDDddTel, pParametro.NrTelefone.ToTelefoneString());
            this.Ramal = pParametro.NrRamal;
            this.Celular1 = string.Format("({0}) {1}", pParametro.CdDddCelular1, pParametro.NrCelular1.ToTelefoneString());
            this.Celular2 = string.Format("({0}) {1}", pParametro.CdDddCelular2, pParametro.NrCelular2.ToTelefoneString());
            this.Email = pParametro.NmEmail.ToLower();
        }
    }

    public class TransporteRelatorio_003_Corretagem
    {
        public string VolumeEm12Meses { get; set; }

        public string VolumeMediaNoAno { get; set; }

        public string VolumeNoMes { get; set; }

        public string CorretagemEm12Meses { get; set; }

        public string CorretagemMediaNoAno { get; set; }

        public string CorretagemNoMes { get; set; }

        public string ContaCorrenteDisponivel { get; set; }

        public TransporteRelatorio_003_Corretagem(ResumoDoClienteCorretagemInfo pParametro)
        {
            if (null != pParametro)
            {
                this.CorretagemNoMes = pParametro.VlCorretagemMes.ToString("N2");
                this.CorretagemMediaNoAno = pParametro.VlCorretagemMediaAno.ToString("N2");
                this.CorretagemEm12Meses = pParametro.VlCorretagemEm12Meses.ToString("N2");
                this.VolumeNoMes = pParametro.VlVolumeMes.ToString("N2");
                this.VolumeMediaNoAno = pParametro.VlVolumeMediaAno.ToString("N2");
                this.VolumeEm12Meses = pParametro.VlVolumeEm12Meses.ToString("N2");
                this.ContaCorrenteDisponivel = pParametro.VlDisponivel.ToString("N2");
            }
            else
            {
                this.CorretagemMediaNoAno =
                this.ContaCorrenteDisponivel = this.CorretagemNoMes =
                    this.CorretagemEm12Meses = this.VolumeEm12Meses =
                    this.VolumeMediaNoAno = this.VolumeNoMes = "0,00";
            }
        }
    }

    public class TransporteRelatorio_003_Carteira
    {
        public string Carteira { get; set; }

        public string Valor { get; set; }

        public string Quantidade { get; set; }

        public List<TransporteRelatorio_003_Carteira> TraduzirLista(List<ResumoDoClienteCarteiraInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_003_Carteira>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(rcc =>
                {
                    lRetorno.Add(new TransporteRelatorio_003_Carteira()
                    {
                        Carteira = rcc.DsCarteira.ToUpper(),
                        Quantidade = rcc.QtQuantidade.ToString("N0"),
                        Valor = rcc.VlCotacao.ToString("N2"),
                    });
                });

            return lRetorno;
        }
    }
}