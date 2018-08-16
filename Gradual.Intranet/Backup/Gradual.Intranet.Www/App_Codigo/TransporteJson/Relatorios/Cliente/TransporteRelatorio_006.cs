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
    /// Linha do Relatório Clientes > "Clientes que efetuaram Suitability"
    /// </summary>
    public class TransporteRelatorio_006
    {


        public int Id { get; set; }

        public string Nome { get; set; }

        public string CodigoBovespa { get; set; }

        public string CpfCnpj { get; set; }

        public string Assessor { get; set; }

        public string UltimaAlteracaoSuitability { get; set; }

        public string ResultadoDaAnalise { get; set; }

        public string Local { get; set; }

        public string RealizadoPeloCliente { get; set; }

        public string RealizadoPor { get; set; }

        public string Status { get; set; }
        
        public string ArquivoCiencia { get; set; }
        
        public string ArquivoCienciaData { get; set; }

        public string PrefixoRaiz { get; set; }

        public string Respostas { get; set; }

        public string Peso { get; set; }

        public string ArquivoCienciaLink
        {
            get
            {
                return string.IsNullOrEmpty(this.ArquivoCiencia) ? "" : string.Format("<a href='{3}{0}' title='{1}' target='_blank' style='color:blue'>{2}</a>"
                        , this.ArquivoCiencia
                        , this.ArquivoCiencia.Substring(this.ArquivoCiencia.LastIndexOf('/') + 1)
                        , this.ArquivoCienciaData
                        , this.PrefixoRaiz);
            }
        }

        private string GetPreenchidoCliente(System.Nullable<Boolean> pStPreenchidoPeloCliente) {
            if (null == pStPreenchidoPeloCliente)
                return "";
            else if (pStPreenchidoPeloCliente.Value)
                return "Sim";
            else
                return "Não";
        }

        public TransporteRelatorio_006(ClienteSuitabilityEfetuadoInfo pInfo, string pPrefixoRaiz)
        {
            this.Id                         = pInfo.IdCliente.Value;
            this.Nome                       = pInfo.DsNomeCliente;
            this.CpfCnpj                    = pInfo.DsCpfCnpj;
            this.CodigoBovespa              = pInfo.CodigoBovespa.ToString();
            this.Assessor                   = pInfo.CodigoAssessor.ToString();
            this.UltimaAlteracaoSuitability = pInfo.DtRealizacao==DateTime.MinValue?"":pInfo.DtRealizacao.ToString("dd/MM/yyyy HH:mm:ss");
            this.ResultadoDaAnalise         = pInfo.DsPerfil;
            this.Local                      = pInfo.DsFonte;
            this.RealizadoPeloCliente       = GetPreenchidoCliente(pInfo.StPreenchidoPeloCliente);
            this.RealizadoPor               = pInfo.DsLoginRealizado;
            this.Status                     = pInfo.DsStatus;
            this.ArquivoCiencia             = pInfo.DsArquivoCiencia.Replace("~", "");
            this.ArquivoCienciaData         = pInfo.DtArquivoCiencia;
            this.PrefixoRaiz                = pPrefixoRaiz;
            this.Respostas                  = pInfo.DsRespostas.ToString();
            this.Peso                       = pInfo.Peso.ToString();
        }

    }
}
