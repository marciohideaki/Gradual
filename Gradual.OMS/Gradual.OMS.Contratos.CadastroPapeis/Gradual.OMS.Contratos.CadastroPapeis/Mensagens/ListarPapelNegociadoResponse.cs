using System;
using System.Collections.Generic;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.CadastroPapeis.Dados;
using System.Collections;

namespace Gradual.OMS.Contratos.CadastroPapeis.Mensagens
{
    [Serializable]
    public class ConsultarPapelNegociadoResponse : MensagemResponseBase
    {
        public List<PapelNegociadoBovespaInfo> LstPapelBovespaInfo { get; set; }

        public List<PapelNegociadoBmfInfo> LstPapelBmfInfo { get; set; }

        public Hashtable LstPapelInfo { get; set; }
    }
}
