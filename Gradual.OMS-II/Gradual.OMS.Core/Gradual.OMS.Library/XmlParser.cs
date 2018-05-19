using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;

namespace Gradual.OMS.Library
{
    public abstract class XmlParser
    {
        public void Parse(string xml)
        {
            StringReader sr = new StringReader(xml);
            XmlTextReader reader = new XmlTextReader(sr);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Document:
                        StartDocument();
                        break;
                    case XmlNodeType.Element:
                        string namespaceUri = reader.NamespaceURI;
                        string name = reader.Name;
                        Hashtable attributes = new Hashtable();
                        bool hasInlineEnd = reader.IsEmptyElement;

                        if (reader.HasAttributes)
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                attributes.Add(reader.Name, reader.Value);
                            }
                        }
                        StartElement(namespaceUri, name, name, attributes, hasInlineEnd);
                        break;
                    case XmlNodeType.EndElement:
                        EndElement(reader.NamespaceURI, reader.Name, reader.Name);
                        break;
                    case XmlNodeType.CDATA:
                        Characters(reader.Value, 0, reader.Value.Length);
                        break;
                    case XmlNodeType.Text:
                        Characters(reader.Value, 0, reader.Value.Length);
                        break;
                    // There are many other types of nodes, but
                    // we are not interested in them
                }
            }
        }

        protected virtual void Characters(string param1, int param2, int param3) { }
        protected virtual void EndElement(string param1, string param2, string param3) { }
        protected virtual void StartElement(string namespace1, string name, string name3, Hashtable attributes, bool hasInlineEnd) { }
        protected virtual void StartDocument() { }
    }
}

