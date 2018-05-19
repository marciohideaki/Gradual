using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Interface.Dados
{
    /// <summary>
    /// Representa um comando de interface.
    /// Indica qual o tipo deve ser criado para qual plataforma na execução de comandos.
    /// A interface web pode, por exemplo, implementar um comando chamado ExecutarComandoWEB. 
    /// Este comando pode receber como parâmetros um link de página para ser redirecionado.
    /// </summary>
    [Serializable]
    public class ComandoInterfaceExecucaoInfo
    {
        /// <summary>
        /// Objeto de configurações do comando
        /// </summary>
        public object Config { get; set; }

        /// <summary>
        /// Tipo da plataforma a que a execução se refere
        /// </summary>
        public InterfacePlataformaEnum Plataforma { get; set; }

        /// <summary>
        /// Tipo da execução de comando
        /// </summary>
        [XmlIgnore]
        public Type TipoComandoExecucao 
        {
            get { return Type.GetType(this.TipoComandoExecucaoString); }
            set { this.TipoComandoExecucaoString = value.FullName + ", " + value.Assembly.FullName; }
        }

        /// <summary>
        /// Tipo string da execução de comando.
        /// Utilizado para serialização
        /// </summary>
        [XmlElement("TipoComandoExecucao")]
        public string TipoComandoExecucaoString { get; set; }
    }
}
