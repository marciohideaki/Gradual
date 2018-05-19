using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de response dos grupos do fluxo de aprovação de fundos em constituição
    /// </summary>
    public class AlteracaoRegulamentacaoConsultaFundosCarregarListaGruposResponse : MensagemResponseBase
    {
        public List<FluxoAlteracaoRegulamentoGrupoInfo> ListaGrupos { get; set; }
    }
}
