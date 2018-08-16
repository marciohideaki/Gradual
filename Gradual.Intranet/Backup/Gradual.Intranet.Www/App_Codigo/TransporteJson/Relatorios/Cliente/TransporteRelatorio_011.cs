using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.PlanoCliente.Lib;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    public class TransporteRelatorio_011
    {
        #region Propriedades
        public string CodBolsa { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCnpj { get; set; }
        public string Produto { get; set; }
        public string DataAdesao { get; set; }
        public string CodAssessor { get; set; }
        #endregion

        #region Constructors
        public TransporteRelatorio_011() { }

        public TransporteRelatorio_011(ClienteDirectInfo pInfo)
        {
            this.CodBolsa    = pInfo.CdCblc.DBToString();
            this.NomeCliente = pInfo.NomeCliente.ToUpper().DBToString();
            this.CpfCnpj     = pInfo.DsCpfCnpj;
            this.Produto     = pInfo.NomeProduto.DBToString();
            this.DataAdesao = (pInfo.DtAdesao.HasValue && pInfo.DtAdesao.Value.ToString("dd/MM/yyyy") != "01/01/0001") ? pInfo.DtAdesao.Value.ToString("dd/MM/yyyy") : "Aguardando Ativação";
            this.CodAssessor = pInfo.CdAssessor.DBToString();

        }
        #endregion
    }
}