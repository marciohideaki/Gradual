using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Risco.Regras
{
    [Regra(
        CodigoRegra = "9A7645A5-8F71-4b34-9EC7-66A346332514",
        NomeRegra = "Inicializa o cliente",
        DescricaoRegra = "Faz a inicialização do cliente. Sobe as regras do cliente.",
        TipoConfig = null,
        RegraDeUsuario = false)]
    [Serializable]
    public class RegraInicializaCliente : RegraRiscoBase
    {
        public RegraInicializaCliente(RegraRiscoInfo regraInfo) : base(regraInfo)
        {
        }

        protected override bool OnValidar(ContextoValidacaoInfo contexto)
        {
            //// Pede inicializacao do cliente
            //Ativador.Get<IServicoRisco>().InicializarCliente(
            //    new InicializarClienteRequest()
            //    {
            //    });
            
            // Retorna ok
            return base.OnValidar(contexto);
        }

        public override string ToString()
        {
            return this.NomeRegra;
        }
    }
}
