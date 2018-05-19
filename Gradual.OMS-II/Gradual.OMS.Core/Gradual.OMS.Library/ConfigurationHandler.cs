using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using log4net;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Utilizado para gerenciamento de configurações.
    /// Utiliza o nome da tag utilizada na configuração para determinar a 
    /// classe de configuração correspondente.
    /// Permite utilização de ID´s no nome das tags.
    /// </summary>
    public class ConfigurationHandler : IConfigurationSectionHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IConfigurationSectionHandler Members

        object IConfigurationSectionHandler.Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // Separa id do servico do tipo do servico
            string[] temp1 = section.Name.Split('-');
            string tipoStr = temp1[0];

            // Cria o tipo
            Type tipo = null;
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                tipo = assembly.GetType(tipoStr);
                if (tipo != null)
                    break;
            }

            if (tipo == null)
            {
                logger.Error("Configuration Handler nao encontrado para " + tipoStr);
                return null;
            }

            // Cria o serializador do tipo
            XmlSerializer serializer =
                new XmlSerializer(tipo);

            // Duplica a estrutura para poder passar para o desserializador
            string nodeName = tipoStr.Replace(tipo.Namespace + ".", "");

            XmlNode node = section.OwnerDocument.CreateElement(nodeName);
            foreach (XmlNode childNode in section.ChildNodes)
                node.AppendChild(childNode.Clone());

            // Desserializa
            System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(node);
            object info = serializer.Deserialize(reader);

            // Retorna
            return info;
        }

        #endregion
    }
}
