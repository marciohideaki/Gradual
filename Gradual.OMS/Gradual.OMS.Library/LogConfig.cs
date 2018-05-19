using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Classe de configurações do utilitário de log
    /// </summary>
    public class LogConfig
    {
        /// <summary>
        /// Indica o tipo que irá conter as origens de log que 
        /// podem ser utilizadas pelos sistemas
        /// </summary>
        [XmlIgnore]
        public Type TipoComOrigensDeLog
        {
            get { return this.TipoComOrigensDeLogString != null ? Type.GetType(this.TipoComOrigensDeLogString) : null; }
            set { this.TipoComOrigensDeLogString = value.FullName + ", " + value.Assembly.FullName; }
        }

        /// <summary>
        /// Classe utilizada para serialização
        /// </summary>
        [XmlElement("TipoComOrigensDeLog")]
        public string TipoComOrigensDeLogString { get; set; }
        
        /// <summary>
        /// Indica se deve efetuar o log em console
        /// </summary>
        public bool LogarEmConsole { get; set; }

        /// <summary>
        /// Indica se deve efetuar o log no event viewer
        /// </summary>
        public bool LogarEmEventViewer { get; set; }

        /// <summary>
        /// Indica se deve mostrar erros no console
        /// </summary>
        public bool LogarErrosNoConsole { get; set; }

        /// <summary>
        /// Indica se deve logar erros no event viewer
        /// </summary>
        public bool LogarErrosNoEventViewer { get; set; }

        /// <summary>
        /// Indica se deve mostrar mensagens de passagem no console
        /// </summary>
        public bool LogarPassagemNoConsole { get; set; }

        /// <summary>
        /// Indica se deve logar mensagens de passagem no event viewer
        /// </summary>
        public bool LogarPassagemNoEventViewer { get; set; }
        
        /// <summary>
        /// Indica o nome da aplicação default que deve ser registrada no event viewer
        /// </summary>
        public string NomeOrigemLogDefault { get; set; }

        /// <summary>
        /// Construtor default.
        /// </summary>
        public LogConfig()
        {
            this.NomeOrigemLogDefault = "OMS - Ordens";
            this.LogarEmEventViewer = true;
            this.LogarErrosNoEventViewer = true;
            this.LogarPassagemNoEventViewer = true;
            this.LogarEmConsole = true;
            this.LogarErrosNoConsole = true;
            this.LogarPassagemNoConsole = true;
        }
    }
}
