using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www
{
    public class TransporteDadosDoAtivo
    {
        #region Propriedades
        
        public TransporteMensagemDeNegocio DadosDeCotacao { get; set; }

        public TransporteDadosCadastraisDoAtivo DadosCadastrais { get; set; }

        public TransporteLivroDeOferta LivroDeOferta { get; set; }

        #endregion
    }
}