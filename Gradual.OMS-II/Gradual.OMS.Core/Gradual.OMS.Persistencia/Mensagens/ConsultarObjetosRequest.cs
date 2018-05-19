using System.Collections.Generic;
using Gradual.OMS.Library;


namespace Gradual.OMS.Persistencia
{
    public class ConsultarObjetosRequest<T> : MensagemRequestBase
    {
        /// <summary>
        /// Permite carregar, opcionalmente, o código da sessão
        /// </summary>
        public string CodigoSessao { get; set; }

        /// <summary>
        /// Lista de condições para o filtro
        /// </summary>
        public List<CondicaoInfo> Condicoes { get; set; }

        /// <summary>
        /// Objeto responsável pelos parametros da mensagem.
        /// </summary>
        public T Objeto { set; get; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ConsultarObjetosRequest()
        {
            this.Condicoes = new List<CondicaoInfo>();
        }

    }
}
