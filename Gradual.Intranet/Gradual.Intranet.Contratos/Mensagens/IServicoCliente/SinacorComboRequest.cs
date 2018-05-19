using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class SinacorComboRequest : BaseRequest
    {


        /// <summary>
        /// Necessário apenas no Filtro
        /// </summary>
        public string Filtro { get; set; }

        public eInformacao Informacao { get; set; }
    }
}
