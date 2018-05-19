using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Persistencia
{
    [Serializable]
    public class ObjetoSerializado : IXmlSerializable
    {
        [XmlAttribute]
        public string TipoObjeto { get; set; }

        public object Objeto { get; set; }

        public ObjetoSerializado()
        {
        }

        public ObjetoSerializado(object obj)
        {
            this.Objeto = obj;
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            // Cria o tipo
            string[] tipoObjetoStr = reader.GetAttribute("TipoObjeto").Split(',');
            reader.ReadStartElement();
            Type tipoObjeto = Assembly.Load(tipoObjetoStr[1]).GetType(tipoObjetoStr[0]);

            // Desserializa
            XmlSerializer serializer = new XmlSerializer(tipoObjeto);
            MemoryStream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms);

            string objetoStr = @"<?xml version=""1.0"" encoding=""utf-8"" ?>" + reader.ReadOuterXml();
            writer.Write(objetoStr);
            writer.Flush();
            ms.Position = 0;

            // Lê fim do elemento
            reader.ReadEndElement();
            
            // Salva valores do objeto
            this.Objeto = serializer.Deserialize(ms);
            this.TipoObjeto = tipoObjeto.FullName;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            // Seta corretamente o tipo do objeto
            Type tipoObjeto = this.Objeto.GetType();
            this.TipoObjeto = tipoObjeto.FullName;

            // Serializa o objeto
            XmlSerializer serializer = new XmlSerializer(tipoObjeto);
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, this.Objeto);
            ms.Position = 0;
            StreamReader reader = new StreamReader(ms);

            // Salva na serializacao
            writer.WriteElementString("TipoObjeto", this.TipoObjeto);
            writer.WriteElementString("Objeto", reader.ReadToEnd());
        }

        #endregion
    }
}
