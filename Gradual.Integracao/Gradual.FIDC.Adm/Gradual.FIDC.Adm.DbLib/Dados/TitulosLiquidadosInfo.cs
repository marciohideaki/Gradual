using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info para gerenciar as informações de Títulos Liquidados
    /// </summary>
    public class TitulosLiquidadosInfo
    {
        public int CodigoFundo      { get; set; }

        public string NomeFundo     { get; set; }

        public DateTime Data        { get; set; }

        public string Status        { get; set; }

        public string DownloadLink  { get; set; }

        public decimal Valor        { get; set; }
    }
}
