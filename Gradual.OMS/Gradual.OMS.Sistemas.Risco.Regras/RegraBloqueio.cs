using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Contratos.Risco.Regras;
using Gradual.OMS.Contratos.Risco.Regras.Criticas;
using Gradual.OMS.Contratos.Risco.Regras.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Risco.Regras
{
    [Regra(
        CodigoRegra = "36B54183-D2EE-461d-A0F8-6E2F71CC4CC2",
        NomeRegra = "Bloqueio de Operação",
        DescricaoRegra = "Indica que a operação não pode ser efetuada.",
        TipoConfig = null,
        RegraDeUsuario = true)]
    [Serializable]
    public class RegraBloqueio : RegraRiscoBase
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="regraInfo"></param>
        public RegraBloqueio(RegraRiscoInfo regraInfo) : base(regraInfo)
        {
        }

        /// <summary>
        /// Executa a lógica de validação
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns></returns>
        protected override bool OnValidar(ContextoValidacaoInfo contexto)
        {
            // Adiciona crítica
            contexto.Criticas.Add(
                new CriticaInfo() 
                { 
                    Descricao = "Operação não permitida por regra de bloqueio",
                    Status = CriticaStatusEnum.ErroNegocio
                });
            
            // Retorna que operação não pode ser executada
            return false;
        }

        public override string ToString()
        {
            return this.NomeRegra;
        }
    }
}
