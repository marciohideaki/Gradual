using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    /// <summary>
    /// Classe base para as classes de permissões do sistema.
    /// Não é uma classe de dados, e sim a base para uma classe de negócios.
    /// </summary>
    public abstract class PermissaoBase
    {
        /// <summary>
        /// Referencia para o atributo de descrição da permissao
        /// </summary>
        private PermissaoAttribute _permissaoAttribute = null;

        /// <summary>
        /// Contem informações da permissão, herdadas do atributo
        /// </summary>
        public PermissaoInfo PermissaoInfo { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public PermissaoBase()
        {
            // Pega o atributo de permissão
            _permissaoAttribute = 
                (PermissaoAttribute)
                    this.GetType().GetCustomAttributes(
                        typeof(PermissaoAttribute), true)[0];

            // Cria o permissao info
            this.PermissaoInfo = 
                new PermissaoInfo() 
                { 
                    CodigoPermissao = _permissaoAttribute.CodigoPermissao,
                    DescricaoPermissao = _permissaoAttribute.DescricaoPermissao,
                    NomePermissao = _permissaoAttribute.NomePermissao
                };
        }

        /// <summary>
        /// Solicita validação da permissão, passando a sessão como contexto
        /// </summary>
        /// <returns></returns>
        public bool ValidarPermissao()
        {
            return OnValidarPermissao();
        }

        /// <summary>
        /// Método virtual, para ser sobescrito pelas permissões que desejam
        /// um maior controle sobre o permitido ou não permitido.
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnValidarPermissao()
        {
            return true;
        }

        /// <summary>
        /// Overload para converter em string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Pega o atributo
            PermissaoAttribute atributo =
                (PermissaoAttribute)
                    this.GetType().GetCustomAttributes(typeof(PermissaoAttribute), true)[0];

            // Retorna
            if (atributo != null)
                return atributo.NomePermissao;
            else
                return base.ToString();
        }
    }
}
