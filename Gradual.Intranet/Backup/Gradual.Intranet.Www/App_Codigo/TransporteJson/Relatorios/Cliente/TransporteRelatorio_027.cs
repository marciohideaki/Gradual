using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente
{
    public class TransporteRelatorio_027
    {
        public int CodigoGradual    { get; set; }
        public int CodigoExterno    { get; set; }
        public int DigitoExterno    { get; set; }
        public string CodigoAssessor   { get; set; }
        public string Nome          { get; set; }

        public TransporteRelatorio_027(Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo pInfo)
        {
            this.CodigoGradual  = pInfo.CodigoGradual.Value;
            this.CodigoAssessor = pInfo.CodigoAssessor;
            this.CodigoExterno  = pInfo.CodigoExterno.Value;
            this.DigitoExterno  = pInfo.DigitoPlural.Value;
            this.Nome           = pInfo.Nome.ToString();
        }
    }
}