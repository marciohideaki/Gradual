using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Classe de informações para uma regra
    /// </summary>
    [Serializable]
    public class RegraInfo
    {
        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public RegraInfo()
        {
            this.Habilitado = true;
        }

        #endregion

        #region Informações relevantes da regra

        /// <summary>
        /// Indica o tipo da regra
        /// </summary>
        [XmlIgnore]
        public Type TipoRegra 
        {
            get { return this.TipoRegraString != null ? Type.GetType(this.TipoRegraString) : null; }
            set { this.TipoRegraString = value.FullName + ", " + value.Assembly.FullName; }
        }

        /// <summary>
        /// Tipo da regra. Propriedade utilizada para serialização do tipo da regra.
        /// </summary>
        [XmlElement("TipoRegra")]
        public string TipoRegraString { get; set; }

        /// <summary>
        /// Contém configurações a serem passadas para a regra.
        /// </summary>
        public object Config { get; set; }

        /// <summary>
        /// Indica se a regra está habilitada.
        /// </summary>
        public bool Habilitado { get; set; }

        #endregion

        #region Informações (apenas leitura) copiadas do RegraAttribute para servir de auxilio (por exemplo em cadastro)

        /// <summary>
        /// Chave primária da regra.
        /// Apenas leitura.
        /// </summary>
        public string CodigoRegra 
        { 
            get 
            {
                RegraAttribute attr = receberRegraAttribute();
                if (attr != null)
                    return attr.CodigoRegra;
                else
                    return null;
            } 
        }

        /// <summary>
        /// Nome da regra
        /// Apenas leitura.
        /// </summary>
        public string NomeRegra
        {
            get
            {
                RegraAttribute attr = receberRegraAttribute();
                if (attr != null)
                    return attr.NomeRegra;
                else
                    return null;
            }
        }

        /// <summary>
        /// Descrição da regra
        /// Apenas leitura.
        /// </summary>
        public string DescricaoRegra
        {
            get
            {
                RegraAttribute attr = receberRegraAttribute();
                if (attr != null)
                    return attr.DescricaoRegra;
                else
                    return null;
            }
        }

        /// <summary>
        /// Tipo do config utilizado pela regra.
        /// Apenas leitura.
        /// </summary>
        public Type TipoConfig
        {
            get
            {
                RegraAttribute attr = receberRegraAttribute();
                if (attr != null)
                    return attr.TipoConfig;
                else
                    return null;
            }
        }

        /// <summary>
        /// Indica se esta é uma regra que pode ser editada pelo
        /// usuário. Existem regras, como por exemplo a que trata
        /// o login e logout (no sistema de risco) que não pode 
        /// ser utilizada pelos usuários finais.
        /// Apenas leitura.
        /// </summary>
        public bool RegraDeUsuario
        {
            get
            {
                RegraAttribute attr = receberRegraAttribute();
                if (attr != null)
                    return attr.RegraDeUsuario;
                else
                    return false;
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Conversao para string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            RegraAttribute attr = receberRegraAttribute();
            if (attr != null)
                return attr.NomeRegra;
            else
                return Serializador.TransformarEmString(this);
        }

        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Recebe o tipo informado na classe e pega o atributo da regra
        /// </summary>
        /// <returns></returns>
        private RegraAttribute receberRegraAttribute()
        {
            // Tem o tipo?
            if (this.TipoRegra != null)
            {
                // Pega os atributos de regra
                object[] attrs =
                    this.TipoRegra.GetCustomAttributes(
                        typeof(RegraAttribute), true);

                // Se achou, retorna 
                if (attrs.Length > 0)
                    return (RegraAttribute)attrs[0];
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
