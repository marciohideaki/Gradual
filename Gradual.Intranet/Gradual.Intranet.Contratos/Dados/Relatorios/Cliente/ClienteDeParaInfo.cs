using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Cliente
{
    public class ClienteDeParaInfo : Gradual.OMS.Library.ICodigoEntidade
    {
        public Nullable<int>    CodigoGradual    { get; set; }
        public Nullable<int>    CodigoExterno    { get; set; }
        public Nullable<int>    DigitoPlural     { get; set; }
        public string           CodigoAssessor   { get; set; }
        public string           Nome             { get; set; }

        string OMS.Library.ICodigoEntidade.ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
