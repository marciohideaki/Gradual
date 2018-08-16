using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente
{
    public class TransporteRelatorio_023
    {
        #region Propriedades
        public string NomeFundo         { get; set; }
        public string SaldoBruto        { get; set; } //Saldo bruto na data do 1º aporte 
        public string Entradas          { get; set; } //valor da ultima aplicação
        public string Saida             { get; set; } //valor do ult. Resgate
        public string ImpostoPago       { get; set; }
        public string Rendas            { get; set; } //rentabilidade em valor monetário
        public string SaldoBruto2       { get; set; } //na data da consulta
        public string ProvisaoIR_IOF    { get; set; }
        public string SaldoLiquido      { get; set; } //na data da consulta
        public string Periodo           { get; set; }
        public string Meses03           { get; set; }
        public string Meses06           { get; set; }
        public string Meses12           { get; set; }
        public string Meses24           { get; set; }
        public string Meses36           { get; set; }
        public string DesdeOInicio      { get; set; }
        #endregion

        #region Métodos
        //public List<TransporteRelatorio_023> TraduzirLista(List<PapelPorClienteInfo> pParametros)
        //{
        //    var lRetorno = new List<TransporteRelatorio_023>();

        //    TransporteRelatorio_023 lAcomp = null;

        //    List<PapelPorClienteInfo> lLista = IncluiTotal(pParametros);

        //    lLista.ForEach(papel =>
        //    {
        //        lAcomp = new TransporteRelatorio_023();

        //        lAcomp.NomeFundo       = papel.CodigoCliente.ToString();
        //        lAcomp.SaldoBruto      = papel.CodigoAssessor.ToString();
        //        lAcomp.Entradas        = papel.DataPregao.ToString("dd/MM/yyyy");
        //        lAcomp.Saida           = papel.Papel.ToString();
        //        lAcomp.ImpostoPago     = papel.QtdeCompras.ToString("N0");
        //        lAcomp.Rendas          = papel.QtdeVendas.ToString("N0");
        //        lAcomp.SaldoBruto2     = papel.QtdeLiquida.ToString("N0");
        //        lAcomp.ProvisaoIR_IOF  = papel.Preco.ToString("N6");
        //        lAcomp.SaldoLiquido    = papel.VolCompras.ToString("N2");
        //        lAcomp.Periodo         = papel.VolVendas.ToString("N2");
        //        lAcomp.Meses03         = papel.VolLiquido.ToString("N2");
        //        lAcomp.Meses06         = papel.VlNegocio.ToString("N2");
        //        lAcomp.Meses12         = papel.MostraTotal;
        //        lAcomp.Meses24         = papel.TotalQtdeCompras.ToString("N0");
        //        lAcomp.Meses36         = papel.TotalQtdeVendas.ToString("N0");
        //        lAcomp.DesdeOInicio    = papel.TotalVolCompras.ToString("N2");

        //        lRetorno.Add(lPapelCliente);
        //    });

        //    return lRetorno;
        //}
        #endregion

        
    }
}