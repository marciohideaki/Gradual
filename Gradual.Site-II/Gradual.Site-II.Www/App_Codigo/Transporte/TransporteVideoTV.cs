using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Gradual.Site.Www
{
    public class TransporteVideoTV
    {
        #region Propriedades

        public string ID { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string UrlDaImagem { get; set; }

        public string Autor { get; set; }

        public string Duracao { get; set; }

        public string Categoria { get; set; }

        #endregion

        #region Constructors

        public TransporteVideoTV() { }

        public TransporteVideoTV(XmlNode pNoXml, string pCategoria = null)
        {
            this.ID          = pNoXml["media_id"].InnerText;
            this.Titulo      = pNoXml["title"].InnerText;
            this.Descricao   = pNoXml["description"].InnerText;
            this.UrlDaImagem = pNoXml["thumbnail_url"].InnerText;
            this.Autor       = pNoXml["author"].InnerText;
            this.Duracao     = pNoXml["video_length"].InnerText;
            this.Categoria   = pCategoria;
        }

        #endregion
    }
}