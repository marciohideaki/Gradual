using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    public class ConsultarObjetosRequest<T>
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
        /// Construtor default
        /// </summary>
        public ConsultarObjetosRequest()
        {
            this.Condicoes = new List<CondicaoInfo>();
        }
    }
}
