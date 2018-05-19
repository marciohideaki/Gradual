using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Interface.Dados
{
    /// <summary>
    /// Representa um comando de interface.
    /// Permite representar uma hierarquia de comandos.
    /// </summary>
    [Serializable]
    public class ComandoInterfaceInfo
    {
        /// <summary>
        /// Código do comando de interface.
        /// Chave primária.
        /// </summary>
        [XmlAttribute]
        public string CodigoComandoInterface { get; set; }

        /// <summary>
        /// Nome de sistema.
        /// Permite que as funcionalidades sejam agrupadas por sistema.
        /// </summary>
        [XmlAttribute]
        public string CodigoSistema { get; set; }

        /// <summary>
        /// Lista dos comandos de execução por plataforma.
        /// </summary>
        public List<ComandoInterfaceExecucaoInfo> Execucoes { get; set; }

        /// <summary>
        /// Nome do comando.
        /// </summary>
        [XmlAttribute]
        public string Nome { get; set; }

        /// <summary>
        /// Lista de comandos filhos
        /// </summary>
        public List<ComandoInterfaceInfo> Filhos { get; set; }

        /// <summary>
        /// Contém informações sobre quando este comando será ativado, ou seja, 
        /// para que combinação de grupos, perfis e permissões ele estará presente.
        /// </summary>
        public ItemSegurancaInfo Seguranca { get; set; }

        /// <summary>
        /// Propriedade de uso geral.
        /// Na web, pode ser o endereço da página
        /// </summary>
        [XmlAttribute]
        public string Tag { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ComandoInterfaceInfo()
        {
            this.CodigoComandoInterface = Guid.NewGuid().ToString();
            this.Execucoes = new List<ComandoInterfaceExecucaoInfo>();
            this.Seguranca = new ItemSegurancaInfo();
            this.Filhos = new List<ComandoInterfaceInfo>();
        }
    }
}
