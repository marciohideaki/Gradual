using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Gradual.OMS.Library;

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Mensagem de solicitação de geração de metadado em banco de dados
    /// </summary>
    public class GerarDbMetadadoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Lista de enumeradores a serem sincronizados
        /// </summary>
        [XmlIgnore]
        public Type[] Enumeradores 
        {
            get 
            { 
                // Inicializa
                List<Type> tipos = new List<Type>();

                // Varre a lista de strings gerando os tipos
                if (this.EnumeradoresString != null)
                    foreach (string enumeradorString in this.EnumeradoresString)
                        tipos.Add(Type.GetType(enumeradorString));

                // Retorna
                return tipos.ToArray();
            }
            set
            {
                // Inicializa
                List<string> tiposString = new List<string>();

                // Varre a lista de tipos gerando as strings
                foreach (Type tipo in value)
                    tiposString.Add(tipo.FullName + ", " + tipo.Assembly.FullName);

                // Atribui
                this.EnumeradoresString = tiposString.ToArray();
            }
        }

        /// <summary>
        /// Lista em string dos enumeradores a serem sincronizados.
        /// Cada item deve ter o formato tipo, assembly
        /// </summary>
        [XmlElement("Enumeradores")]
        public string[] EnumeradoresString { get; set; }
    }
}
