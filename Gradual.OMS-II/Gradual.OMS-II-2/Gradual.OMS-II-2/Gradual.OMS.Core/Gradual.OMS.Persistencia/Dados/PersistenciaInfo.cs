using System;
using System.Collections.Generic;

using System.Xml.Serialization;


namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Contem informações sobre uma persistencia.
    /// Armazena o tipo da persistencia em questão e as 
    /// informações para inicializá-la
    /// </summary>
    [Serializable]
    public class PersistenciaInfo
    {
        /// <summary>
        /// Tipo da persistencia a criar
        /// </summary>
        [XmlIgnore]
        public Type TipoPersistencia 
        {
            get { return Type.GetType(this.TipoPersistenciaString); }
            set { this.TipoPersistenciaString = value.FullName + ", " + value.Assembly.FullName; } 
        }

        /// <summary>
        /// Propriedade auxiliar para serialização
        /// </summary>
        [XmlElement("TipoPersistencia")]
        public string TipoPersistenciaString { get; set; }

        /// <summary>
        /// Objeto de configuracao a ser passado para a persistencia
        /// </summary>
        public ObjetoSerializado Config { get; set; }

        /// <summary>
        /// Tipo dos objetos a serem passados para essa persistencia
        /// </summary>
        [XmlIgnore]
        public Type[] TipoObjetos 
        {
            get 
            {
                List<Type> retorno = new List<Type>();
                if (ListaTipoObjeto != null)
                    foreach (string tipoObjeto in this.ListaTipoObjeto)
                        retorno.Add(Type.GetType(tipoObjeto));
                return retorno.ToArray(); 
            }
            set
            {
                List<string> tiposString = new List<string>();
                if (value != null)
                    foreach (Type tipo in value)
                        tiposString.Add(tipo.FullName + ", " + tipo.Assembly.FullName);
                this.ListaTipoObjeto = tiposString;
            }
        }

        /// <summary>
        /// Propriedade auxiliar para serialização
        /// </summary>
        public List<string> ListaTipoObjeto { get; set; }

        /// <summary>
        /// Cria a relação com esta persistencia para todos os objetos do namespace 
        /// informado. O namespace é no formato namespace, assembly.
        /// </summary>
        public string[] NamespacesObjetos { get; set; }

        /// <summary>
        /// Indica se esta é a persistencia default para os 
        /// objetos que não tem persistencia específica
        /// </summary>
        public bool Default { get; set; }
    }
}
