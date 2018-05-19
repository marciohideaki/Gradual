using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Risco
{
    public class RiscoClienteMercadoVistaOpcaoRelInfo : Gradual.OMS.Library.ICodigoEntidade
    {
        #region | Propriedades de consulta

        public string ConsultaDsAtivo { get; set; }
        
        #endregion

        #region | Propriedades de resultado

        public int CodigoAssessor { get; set; }

        public int CodigoCliente { get; set; }

        public string NomeCliente { get; set; }

        public string CodigoNegocio { get; set; }

        public string QtdeTotal { get; set; }

        public string QtdeD1 { get; set; }

        public string QtdeD2 { get; set; }

        public string QtdeD3 { get; set; }

        #endregion

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
