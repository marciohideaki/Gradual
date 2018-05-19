using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Classe que fornece serviços de serialização e clonagem através de serialização
    /// </summary>
    public static class Serializador
    {
        /// <summary>
        /// Faz a serialização de um objeto.
        /// A serialização realizada é XML e coloca o tipo do objeto no inicio da string
        /// XML para permitir a desserialização sem conhecer o tipo previamente.
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public static string SerializaParametro(object parametro)
        {
            Type type = parametro.GetType();
            XmlSerializer serializer = new XmlSerializer(type);
            System.IO.MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, parametro);
            ms.Position = 0;
            System.IO.StreamReader reader = new StreamReader(ms);
            return type.FullName + reader.ReadToEnd();
        }

        /// <summary>
        /// Faz a desserialização de um objeto.
        /// Utiliza a string do inicio do XML para descobrir o tipo do objeto a ser desserializado.
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public static object DesserializaParametro(string parametro)
        {
            int posIni = parametro.IndexOf('<');
            string stringType = parametro.Substring(0, posIni);
            string parametro2 = parametro.Remove(0, posIni);

            Type t = Type.GetType(stringType);
            if (t == null)
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    if ((t = assembly.GetType(stringType)) != null)
                        break;

            XmlSerializer serializer = new XmlSerializer(t);
            System.IO.StringReader stream = new StringReader(parametro2);
            return serializer.Deserialize(stream);
        }

        /// <summary>
        /// Faz o deep clone de um objeto utilizando serialização binária.
        /// Representado como um 'extension method'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalObject"></param>
        /// <returns></returns>
        public static T ClonarObjeto<T>(this T originalObject)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, originalObject);
                stream.Position = 0;
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Retorna uma string representando o objeto com as suas propriedades
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string TransformarEmString(object obj)
        {
            // Inicializa
            StringBuilder retorno = new StringBuilder();
            Type tipo = obj.GetType();

            // Insere nome do objeto
            retorno.Append(tipo.Name + " { ");

            // Loop para as propriedades
            foreach (PropertyInfo propriedade in tipo.GetProperties())
            {
                if (!propriedade.PropertyType.IsArray)
                {
                    // Pega o valor da propriedade
                    object valor = propriedade.GetValue(obj, null);
                    
                    // Se nao for nulo...
                    if (valor != null)
                    {
                        // Se for um char em branco, pula
                        if (valor.GetType() == typeof(char) && (char)valor == '\0')
                            continue;

                        // Adiciona na string
                        retorno.Append(propriedade.Name + ": " + valor.ToString() + "; ");
                    }
                }
            }

            // Finaliza
            retorno.Append("}");

            // Retorna
            return retorno.ToString();
        }
    }
}
