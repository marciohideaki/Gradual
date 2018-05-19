using System;
using System.Collections.Generic;
using Gradual.OMS.Library;


namespace Gradual.OMS.Seguranca
{
    /// <summary>
    /// Configurações para o servico de seguranca
    /// </summary>
    [Serializable]
    public class ServicoSegurancaConfig
    {
        /// <summary>
        /// Indica o nome do usuário administrador, que será criado na primeira
        /// vez que o sistema for executado
        /// </summary>
        public string NomeUsuarioAdministrador { get; set; }

        /// <summary>
        /// Indica a senha do usuário administrador, que será criado na primeira
        /// vez que o sistema for executado
        /// </summary>
        public string SenhaUsuarioAdministrador { get; set; }

        /// <summary>
        /// Indica se o sistema deverá realizar a validação das senhas
        /// </summary>
        public bool ValidarSenhas { get; set; }

        /// <summary>
        /// Lista de strings (no formato namespace, assembly) de onde devem ser carregadas as permissoes
        /// </summary>
        public List<string> NamespacesPermissoes { get; set; }

        /// <summary>
        /// Lista de complementos de autenticação a serem consultados no momento da autenticação
        /// </summary>
        public List<ComplementoAutenticacaoInfo> ComplementosAutenticacao { get; set; }

        /// <summary>
        /// Indica se a garantia do usuário administrador deve sempre ser realizada no início do serviço
        /// </summary>
        public bool InicializarAutomaticamente { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoSegurancaConfig()
        {
            this.NamespacesPermissoes = new List<string>();
            this.ComplementosAutenticacao = new List<ComplementoAutenticacaoInfo>();
            this.ValidarSenhas = true;
            this.InicializarAutomaticamente = true;
        }
    }
}
