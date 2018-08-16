using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    /// <summary>
    /// 
    /// </summary>
    public class TransporteRelatorio_029
    {
        /// <summary>
        /// Data de do filtro
        /// </summary>
        public Nullable<DateTime> DtDe   { get; set; }

        /// <summary>
        /// Data "Até" do filtro
        /// </summary>
        public Nullable<DateTime> DtAte  { get; set; }

        /// <summary>
        /// CodigoCliente do filtro e da resposta
        /// </summary>
        public int? CodigoCliente { get; set; }

        public string NomeCliente      { get; set; }
        public string DataMovimento    { get; set; }
        public string NumeroLancamento { get; set; }
        public string Descricao        { get; set; }
        public string Valor            { get; set; }

        public TransporteRelatorio_029(LancamentoTEDInfo pInfo)
        {
            this.DataMovimento              = pInfo.DataMovimento.ToString("dd/MM/yyyy");
            this.CodigoCliente              = pInfo.CodigoCliente;
            this.NomeCliente                = pInfo.NomeCliente;
            this.NumeroLancamento           = pInfo.NumeroLancamento;
            this.Descricao                  = pInfo.Descricao;
            this.Valor                      = pInfo.Valor.ToString("N2");
        }

    }
}
