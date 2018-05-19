using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Interface.Desktop.Mensagens
{
    /// <summary>
    /// Solicitação de envio de mensagem para controle
    /// </summary>
    public class EnviarMensagemParaControleRequest : MensagemInterfaceRequestBase
    {
        /// <summary>
        /// Quando o id do controle estiver presente, irá utilizá-lo
        /// para encontrar o controle
        /// </summary>
        public string IdControle { get; set; }

        /// <summary>
        /// Mensagem de solicitação a ser enviada para o controle
        /// </summary>
        public MensagemInterfaceRequestBase MensagemRequest { get; set; }

        /// <summary>
        /// Tipo do controle no qual a mensagem deve ser enviada.
        /// </summary>
        [XmlIgnore]
        public Type ControleTipo 
        {
            get { return Type.GetType(this.ControleTipoString); }
            set { this.ControleTipoString = value.FullName + ", " + value.Assembly.FullName; }
        }

        /// <summary>
        /// String do tipo do controle no qual a mensagem deve ser enviada.
        /// Esta propriedade é necessário para serialização.
        /// </summary>
        public string ControleTipoString { get; set; }

        /// <summary>
        /// Id do desktop no qual o controle deve ser procurado.
        /// Se não for informado, assume o desktop ativo.
        /// </summary>
        public string IdDesktop { get; set; }

        /// <summary>
        /// Indica se deve criar o controle, caso não seja 
        /// encontrada nenhuma instância criada.
        /// </summary>
        public bool CriarCasoNaoEncontrado { get; set; }

        /// <summary>
        /// Caso um novo controle tenha que ser criado, informa qual
        /// será o título da janela
        /// </summary>
        public string TituloJanela { get; set; }
    }
}
