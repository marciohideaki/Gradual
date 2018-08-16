using System;
using System.Globalization;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteImportacao
    {
        public string CPF_CNPJ { get; set; }
        public string DataNascimento { get; set; }
        public string CondicaoDependente { get { return "1"; } }

        public TransporteImportacao() { }

        public TransporteImportacao(SinacorChaveClienteInfo pParametros)
        {
            this.CPF_CNPJ = pParametros.CD_CPFCGC.ToString();
            this.DataNascimento = pParametros.DT_NASC_FUND.ToString("dd/MM/yyyy");
        }

        public SinacorChaveClienteInfo ToSinacorChaveInfo()
        {
            SinacorChaveClienteInfo lRetorno = new SinacorChaveClienteInfo();

            lRetorno.CD_CPFCGC = long.Parse(this.CPF_CNPJ.Replace(".", "").Replace(".", "").Replace(".", "").Replace(".", "").Replace("-", "").Replace("/", ""));
            lRetorno.DT_NASC_FUND = Convert.ToDateTime(this.DataNascimento, new CultureInfo("pt-BR"));
            lRetorno.CD_CON_DEP = int.Parse(this.CondicaoDependente);

            return lRetorno;
        }
    }
}