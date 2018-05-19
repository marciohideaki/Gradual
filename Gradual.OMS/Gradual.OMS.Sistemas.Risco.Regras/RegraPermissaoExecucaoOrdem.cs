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
    /// <summary>
    /// Verifica se o usuário que está solicitando a execução da ordem tem as permissões para tal.
    /// Verifica se o CBLC da execução da ordem pode ser feito pelo usuário solicitante.
    /// </summary>
    [Regra(
        CodigoRegra = "69D34D7B-90B1-41be-935E-67359515CF5C",
        NomeRegra = "Verificação de Permissão de Execução",
        DescricaoRegra = "Verifica se o usuário solicitante tem permissão para executar a ordem solicitada.",
        TipoConfig = typeof(RegraPermissaoExecucaoOrdemConfig),
        RegraDeUsuario = false)]
    [Serializable]
    public class RegraPermissaoExecucaoOrdem : RegraRiscoBase
    {
        /// <summary>
        /// Objeto de configuração da regra
        /// </summary>
        private RegraPermissaoExecucaoOrdemConfig _config = null;

        /// <summary>
        /// Referencia para objeto de informações da regra
        /// </summary>
        private RegraRiscoInfo _regraRiscoInfo = null;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="regraInfo"></param>
        public RegraPermissaoExecucaoOrdem(RegraRiscoInfo regraInfo) : base(regraInfo)
        {
            // Seta referencias iniciais
            _regraRiscoInfo = regraInfo;
            _config = (RegraPermissaoExecucaoOrdemConfig)_regraRiscoInfo.Config;
        }

        /// <summary>
        /// Executa a lógica de validação
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns></returns>
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
