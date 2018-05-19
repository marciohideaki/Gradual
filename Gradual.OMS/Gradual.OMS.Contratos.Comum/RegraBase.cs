using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Classe base para uma regra
    /// </summary>
    [Serializable]
    public abstract class RegraBase
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para o atributo de regra
        /// </summary>
        private RegraAttribute _regraAttribute = null;

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public RegraBase(RegraInfo regraInfo)
        {
            // Inicializa
            this.RegraInfo = regraInfo;

            // Pega o atributo de regra
            _regraAttribute =
                (RegraAttribute)
                    this.GetType().GetCustomAttributes(
                        typeof(RegraAttribute), true)[0];
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Informações da regra
        /// </summary>
        public RegraInfo RegraInfo { get; set; }

        #endregion

        #region Propriedades de Leitura, repasse dos atributos

        /// <summary>
        /// Chave primária da regra.
        /// Retorna a informação contida no atributo da classe da regra
        /// </summary>
        public string CodigoRegra
        {
            get { return _regraAttribute.CodigoRegra; }
        }

        /// <summary>
        /// Nome da regra.
        /// Retorna a informação contida no atributo da classe da regra
        /// </summary>
        public string NomeRegra
        {
            get { return _regraAttribute.NomeRegra ; }
        }

        /// <summary>
        /// Descrição da regra.
        /// Retorna a informação contida no atributo da classe da regra
        /// </summary>
        public string DescricaoRegra
        {
            get { return _regraAttribute.DescricaoRegra; }
        }

        /// <summary>
        /// Tipo do config utilizado pela regra.
        /// Retorna a informação contida no atributo da classe da regra
        /// </summary>
        public Type TipoConfig
        {
            get { return _regraAttribute.TipoConfig; }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método para realizar a validação da mensagem.
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns></returns>
        public bool Validar(ContextoValidacaoInfo contexto)
        {
            return OnValidar(contexto);
        }

        /// <summary>
        /// Método virtual a ser sobrescrito pelas implementações das regras.
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns></returns>
        protected virtual bool OnValidar(ContextoValidacaoInfo contexto)
        {
            return false;
        }

        /// <summary>
        /// Método para desfazer uma validação
        /// </summary>
        /// <param name="contexto"></param>
        public void Desfazer(ContextoValidacaoInfo contexto)
        {
            OnDesfazer(contexto);
        }

        /// <summary>
        /// Método virtual a ser sobrescrito pelas implementações de regras.
        /// </summary>
        /// <param name="contexto"></param>
        public virtual void OnDesfazer(ContextoValidacaoInfo contexto)
        {
        }

        #endregion
    }
}
