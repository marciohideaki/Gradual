using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de response de Titulos Liquidados do banco de dados
    /// </summary>
    public class TitulosLiquidadosRequest : MensagemRequestBase
    {
        public int? CodigoFundo          { get; set; }

        public DateTime DataDe          { get; set; }

        public DateTime DataAte         { get; set; }

        public string DownloadPendente  { get; set; }

        public decimal ValorLiquidacao  { get; set; }
    }
}
