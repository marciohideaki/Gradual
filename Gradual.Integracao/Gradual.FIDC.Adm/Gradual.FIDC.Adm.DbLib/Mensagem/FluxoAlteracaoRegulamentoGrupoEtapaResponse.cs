using System.Collections.Generic;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class FluxoAlteracaoRegulamentoGrupoEtapaResponse : MensagemResponseBase
    {
        public IList<FluxoAlteracaoRegulamentoGrupoEtapaInfo> ListaEtapas { get; set; }
    }
}
