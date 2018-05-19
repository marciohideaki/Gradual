using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe que armazena informações de request referentes a cadastro de fundos
    /// </summary>
    public class FluxoAlteracaoRegulamentoGrupoEtapaRequest : MensagemRequestBase
    {
        public int IdFluxoAlteracaoRegulamentoGrupoEtapa { get; set; }
        public int IdFluxoAlteracaoRegulamentoGrupo { get; set; }
        public string DsFluxoAlteracaoRegulamentoGrupoEtapa { get; set; }

    }
}
