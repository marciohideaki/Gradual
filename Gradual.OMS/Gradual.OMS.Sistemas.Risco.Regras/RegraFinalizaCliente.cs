using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Sistemas.Risco.Regras
{
    [Regra(
        CodigoRegra = "2DAE15DA-0F9D-43c2-B419-4A9E75BB14BC",
        NomeRegra = "Finaliza o cliente",
        DescricaoRegra = "Faz a finalização do cliente. Libera as regras do cliente.",
        TipoConfig = null,
        RegraDeUsuario = false)]
    [Serializable]
    public class RegraFinalizaCliente : RegraRiscoBase
    {
        public RegraFinalizaCliente(RegraRiscoInfo regraInfo) : base(regraInfo)
        {
        }

        protected override bool OnValidar(ContextoValidacaoInfo contexto)
        {
            return base.OnValidar(contexto);
        }

        public override string ToString()
        {
            return this.NomeRegra;
        }
    }
}
