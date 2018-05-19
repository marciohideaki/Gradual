using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Comandos
{
    /// <summary>
    /// Classe de parametros de comando de iniciar controle
    /// </summary>
    public class ComandoIniciarControleParametro
    {
        /// <summary>
        /// Informacoes do controle a ser criado
        /// </summary>
        public ControleInfo ControleInfo { get; set; }

        /// <summary>
        /// Parametros a serem passados para a janela no momento da criação
        /// </summary>
        public ObjetoSerializado ParametrosJanela { get; set; }
    }
}
