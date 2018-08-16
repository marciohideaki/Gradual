using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Risco.Regra.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteGrupoDeAlavancagem
    {
        public string ContaCorrente { get; set; }

        public string Custodia { get; set; }

        public string CompraAVista { get; set; }

        public string VendaAVista { get; set; }

        public string CompraOpcao{ get; set; }

        public string VendaOpcao { get; set; }

        public bool UtilizarCarteira23 { get; set; }

        public bool UtilizarCarteira27 { get; set; }

        public TransporteGrupoDeAlavancagem(ParametroAlavancagemInfo pParametroAlavancagemInfo)
        {
            this.ContaCorrente = pParametroAlavancagemInfo.PercentualContaCorrente.ToString("N2");
            this.Custodia = pParametroAlavancagemInfo.PercentualCustodia.ToString("N2");
            this.CompraAVista = pParametroAlavancagemInfo.PercentualAlavancagemCompraAVista.ToString("N2");
            this.VendaAVista = pParametroAlavancagemInfo.PercentualAlavancagemVendaAVista.ToString("N2");
            this.CompraOpcao = pParametroAlavancagemInfo.PercentualAlavancagemCompraOpcao.ToString("N2");
            this.VendaOpcao = pParametroAlavancagemInfo.PercentualAlavancagemVendaOpcao.ToString("N2");
            this.UtilizarCarteira23 = pParametroAlavancagemInfo.StCarteiraGarantiaPrazo.DBToBoolean();
            this.UtilizarCarteira27 = pParametroAlavancagemInfo.StCarteiraOpcao.DBToBoolean();
        }
    }
}