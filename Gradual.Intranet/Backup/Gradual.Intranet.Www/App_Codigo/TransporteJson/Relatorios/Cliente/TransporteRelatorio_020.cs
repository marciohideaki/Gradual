using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    public class TransporteRelatorio_020
    {
        #region Members

        public string NomeAssessor { get; set; }

        public string CodigoAssessor { get; set; }

        public string TotalClientes { get; set; }

        public string DataCadastro { get; set; }
        #endregion

        #region Construtor

        public TransporteRelatorio_020() { }

        public TransporteRelatorio_020(TotalClienteCadastradoAssessorPeriodoInfo pInfo) 
        {
            this.CodigoAssessor = pInfo.CodigoAssessor.ToString();
            this.NomeAssessor   = pInfo.DsNomeAssessor;
            this.TotalClientes  = pInfo.TotalCliente.ToString();
            this.DataCadastro   = pInfo.DataCadastro;
        }

        #endregion
    }
}