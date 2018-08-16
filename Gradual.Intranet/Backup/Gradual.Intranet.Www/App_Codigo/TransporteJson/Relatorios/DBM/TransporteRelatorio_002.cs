using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM
{
    public class TransporteRelatorio_002_Cadastro
    {
        public string QuantidadeTotal { get; set; }
        public string QuantidadeAtivos { get; set; }
        public string QuantidadeInativos { get; set; }
        public string QuantidadeClientesNovos { get; set; }
        public string PercentualTotal { get; set; }
        public string PercentualAtivos { get; set; }
        public string PercentualInativos { get; set; }
        public string QuantidadeVarejo { get; set; }
        public string QuantidadeInstitucional { get; set; }
        public string PercentualOperouNoMes { get; set; }
        public string PercentualComCustodia { get; set; }

        public TransporteRelatorio_002_Cadastro(ResumoDoAssessorCadastroInfo pParametro)
        {
            if (null != pParametro)
            {
                this.QuantidadeTotal = pParametro.QtTotalClientes.ToString("N0");
                this.QuantidadeAtivos = pParametro.QtClientesAtivos.ToString("N0");
                this.QuantidadeInativos = pParametro.QtClientesInativos.ToString("N0");
                this.QuantidadeClientesNovos = pParametro.QtClientesNovos.ToString("N0");
                this.PercentualTotal = "100,00";
                this.PercentualAtivos = pParametro.QtTotalClientes == 0 ? "0,00" : (pParametro.QtClientesAtivos.DBToDecimal() * 100 / pParametro.QtTotalClientes.DBToDecimal()).ToString("N2");
                this.PercentualInativos = pParametro.QtTotalClientes == 0 ? "0,00" : (pParametro.QtClientesInativos.DBToDecimal() * 100 / pParametro.QtTotalClientes.DBToDecimal()).ToString("N2");
                this.QuantidadeVarejo = pParametro.QtClientesNoVarejo.ToString("N0");
                this.QuantidadeInstitucional = pParametro.QtClientesInstitucional.ToString("N0");
                this.PercentualOperouNoMes = pParametro.VlPercentOperouNoMes.ToString("N2");
                this.PercentualComCustodia = pParametro.VlPercenturalComCustodia.ToString("N2");
            }
        }
    }

    public class TransporteRelatorio_002_Receita
    {
        public string BovespaClientes { get; set; }
        public string BovespaValor { get; set; }
        public string BMFClientes { get; set; }
        public string BMFValor { get; set; }
        public string TBCClientes { get; set; }
        public string TBCValor { get; set; }
        public string TesouroClientes { get; set; }
        public string TesouroValor { get; set; }
        public string OutrasClientes { get; set; }
        public string OutrasValor { get; set; }
        public string TotalClientes { get; set; }
        public string TotalValor { get; set; }

        public TransporteRelatorio_002_Receita(ResumoDoAssessorReceitaInfo pParametro, int? pCodigoAssessor)
        {
            if (null != pParametro)
            {
                decimal lValorTotal = pParametro.VlBovespaCorretagem
                                    + pParametro.VlOutrosCorretagem
                                    + pParametro.VlTesouroCorretagem
                                    + pParametro.VlBtcCorretagem
                                    + pParametro.VlBmfCorretagem;

                this.BovespaClientes = pParametro.QtBovespaClientes.ToString("N2");
                this.BovespaValor = pParametro.VlBovespaCorretagem.ToString("N2");
                this.BMFClientes = pParametro.QtBmfClientes.ToString("N2");
                this.BMFValor = pParametro.VlBmfCorretagem.ToString("N2");
                this.TBCClientes = pParametro.QtBtcClientes.ToString("N2");
                this.TBCValor = pParametro.VlBtcCorretagem.ToString("N2");
                this.TesouroClientes = pParametro.QtTesouroClientes.ToString("N2");
                this.TesouroValor = pParametro.VlTesouroCorretagem.ToString("N2");
                this.OutrasClientes = pParametro.QtOutrosClientes.ToString("N2");
                this.OutrasValor = pParametro.VlOutrosCorretagem.ToString("N2");
                this.TotalValor = lValorTotal.ToString("N2");

                if (null == pCodigoAssessor)
                {
                    this.BovespaClientes = this.TotalValor.DBToDecimal() == 0M ? "0,00" : ((pParametro.VlBovespaCorretagem / lValorTotal) * 100).ToString("N2");
                    this.BMFClientes = this.TotalValor.DBToDecimal() == 0M ? "0,00" : ((pParametro.VlBmfCorretagem / lValorTotal) * 100).ToString("N2");
                    this.TBCClientes = this.TotalValor.DBToDecimal() == 0M ? "0,00" : ((pParametro.VlBtcCorretagem / lValorTotal) * 100).ToString("N2");
                    this.TesouroClientes = this.TotalValor.DBToDecimal() == 0M ? "0,00" : ((pParametro.VlTesouroCorretagem / lValorTotal) * 100).ToString("N2");
                    this.OutrasClientes = this.TotalValor.DBToDecimal() == 0M ? "0,00" : ((pParametro.VlOutrosCorretagem / lValorTotal) * 100).ToString("N2");
                }

                this.TotalClientes = (this.BovespaClientes.DBToDecimal()
                                   + this.BMFClientes.DBToDecimal()
                                   + this.TBCClientes.DBToDecimal()
                                   + this.TesouroClientes.DBToDecimal()
                                   + this.OutrasClientes.DBToDecimal()).ToString("N2");
            }
        }
    }

    public class TransporteRelatorio_002_Canal
    {
        public string HbValor { get; set; }
        public string HbPercentual { get; set; }
        public string RepassadorValor { get; set; }
        public string RepassadorPercentual { get; set; }
        public string MesaValor { get; set; }
        public string MesaPercentual { get; set; }
        public string TotalValor { get; set; }
        public string TotalPercentual { get; set; }

        public TransporteRelatorio_002_Canal(ResumoDoAssessorCanalInfo pParametro, int? pCodigoAssessor)
        {
            if (null != pParametro)
            {
                decimal lValorTotal = pParametro.QtMesaValor
                                    + pParametro.QtRepassadorValor
                                    + pParametro.QtHbValor;

                this.HbValor = pParametro.QtHbValor.ToString("N2");
                this.HbPercentual = pParametro.VlHbPercentual.ToString("N2");
                this.RepassadorValor = pParametro.QtRepassadorValor.ToString("N2");
                this.RepassadorPercentual = pParametro.VlRepassadorPercentual.ToString("N2");
                this.MesaValor = pParametro.QtMesaValor.ToString("N2");
                this.MesaPercentual = pParametro.VlMesaPercentual.ToString("N2");

                this.TotalValor = lValorTotal.ToString("N2");

                if (null == pCodigoAssessor)
                {
                    this.HbPercentual = this.TotalValor.DBToDecimal() == 0M ? "0,00" : ((pParametro.QtHbValor / lValorTotal) * 100).ToString("N2");
                    this.RepassadorPercentual = this.TotalValor.DBToDecimal() == 0M ? "0,00" : ((pParametro.QtRepassadorValor / lValorTotal) * 100).ToString("N2");
                    this.MesaPercentual = this.TotalValor.DBToDecimal() == 0M ? "0,00" : ((pParametro.QtMesaValor / lValorTotal) * 100).ToString("N2");
                }

                this.TotalPercentual = (this.HbPercentual.DBToDecimal()
                                     + this.RepassadorPercentual.DBToDecimal()
                                     + this.MesaPercentual.DBToDecimal()).ToString("N2");
            }
        }
    }

    public class TransporteRelatorios_002_Metricas
    {
        public string CorretagemNoDia { get; set; }
        public string CorretagemNoMes { get; set; }
        public string CorretagemNoMesAnterior { get; set; }
        public string CorretagemNoAno { get; set; }
        public string CadastrosNoDia { get; set; }
        public string CadastrosNoMes { get; set; }
        public string CadastrosNoMesAnterior { get; set; }
        public string CadastrosNoAno { get; set; }

        public TransporteRelatorios_002_Metricas(ResumoDoAssessorMetricasInfo pParametro)
        {
            if (null != pParametro)
            {
                this.CorretagemNoDia = pParametro.VlCorretagemDia.ToString("N2");
                this.CorretagemNoMes = pParametro.VlCorretagemMes.ToString("N2");
                this.CorretagemNoMesAnterior = pParametro.VlCorretagemMesAnterior.ToString("N2");
                this.CorretagemNoAno = pParametro.VlCorretagemAno.ToString("N2");
                this.CadastrosNoDia = pParametro.QtCadastrosDia.ToString("N0");
                this.CadastrosNoMes = pParametro.QtCadastrosMes.ToString("N0");
                this.CadastrosNoMesAnterior = pParametro.QtCadastrosMesAnterior.ToString("N0");
                this.CadastrosNoAno = pParametro.QtCadastrosMediaAno.ToString("N2");
            }
        }
    }

    public class TransporteRelatorios_002_Top10
    {
        public string NomeCliente { get; set; }
        public string Corretagem { get; set; }
        public string PercentualTotal { get; set; }
        public string PercentualAcumulado { get; set; }
        public string PercentualDevMedia { get; set; }
        public string Custodia { get; set; }

        public List<TransporteRelatorios_002_Top10> TraduzirLista(List<ResumoDoAssessorTop10Info> pParametros)
        {
            var lRetorno = new List<TransporteRelatorios_002_Top10>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(t10 =>
                {
                    lRetorno.Add(new TransporteRelatorios_002_Top10()
                    {
                        Corretagem = t10.VlCorretagem.ToString("N2"),
                        Custodia = t10.VlCustodia.ToString("N2"),
                        NomeCliente = t10.DsNomeCliente.ToUpper(),
                        PercentualAcumulado = t10.VlPercentualAcumulado.ToString("N2"),
                        PercentualDevMedia = t10.VlPercentualDevMedia.ToString("N2"),
                        PercentualTotal = t10.VlPercentualTotal.ToString("N2")
                    });
                });

            return lRetorno;
        }
    }
}