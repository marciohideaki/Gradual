using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Regras;

namespace Gradual.OMS.Sistemas.Risco.Regras
{
    /// <summary>
    /// Faz validação de características de operações.
    /// Como, por exemplo, se pode-se operar opções, termos, aluguel, se pode-se ficar descoberto, etc, etc.
    /// </summary>
    [Regra(
        CodigoRegra = "94F253E6-F2CD-44f2-9ED7-4E5283110049",
        NomeRegra = "Características de Operações",
        DescricaoRegra = "Indica as características de operações permitidas de negociação. Por exemplo, se pode operar opções, termo, etc.",
        TipoConfig = typeof(RegraCaracteristicaOperacaoConfig),
        RegraDeUsuario = true)]
    [Serializable]
    public class RegraCaracteristicaOperacao : RegraRiscoBase
    {
        public RegraCaracteristicaOperacao(RegraRiscoInfo regraInfo) : base(regraInfo)
        { 
        }

        protected override bool OnValidar(ContextoValidacaoInfo contexto)
        {
            return base.OnValidar(contexto);
        }

        public override void OnDesfazer(ContextoValidacaoInfo contexto)
        {
            base.OnDesfazer(contexto);
        }

        public override string ToString()
        {
            return this.NomeRegra;
        }
    }
}
