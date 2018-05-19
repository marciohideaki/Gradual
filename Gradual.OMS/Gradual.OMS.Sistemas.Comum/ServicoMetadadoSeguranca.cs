using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Implementação do serviço de metadados de segurança
    /// </summary>
    public class ServicoMetadadoSeguranca : IServicoMetadadoSeguranca
    {
        /// <summary>
        /// Referencia para a classe de configurações de segurança
        /// </summary>
        private ServicoSegurancaConfig _config = GerenciadorConfig.ReceberConfig<ServicoSegurancaConfig>();

        /// <summary>
        /// Coleção com as permissões do sistema
        /// </summary>
        private ListaPermissoesHelper _permissoes = new ListaPermissoesHelper();

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoMetadadoSeguranca()
        {
            // Cria a lista de permissões
            _permissoes.CarregarPermissoes(_config.NamespacesPermissoes);
        }
        
        #region IServicoMetadadoSeguranca Members

        /// <summary>
        /// Solicita a geração de metadados de segurança
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public GerarMetadadoSegurancaResponse GerarMetadadoSeguranca(GerarMetadadoSegurancaRequest parametros)
        {
            // Cria lista de permissaoInfo
            List<PermissaoInfo> permissoes =
                (from p in _permissoes.ListaPorCodigo
                 select p.Value.PermissaoInfo).ToList();

            // Retorna
            return 
                new GerarMetadadoSegurancaResponse() 
                { 
                    Permissoes = permissoes
                };
        }

        #endregion
    }
}
