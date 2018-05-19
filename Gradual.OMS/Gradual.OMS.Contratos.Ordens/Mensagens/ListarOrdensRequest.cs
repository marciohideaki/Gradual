using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de lista de ordens.
    /// Esta função é implementada pelos serviços de persistencia. 
    /// Caso o cliente queira executar a funçao de lista, esta mensagem é repassada pelo 
    /// serviço de ordens para o serviço de persistencia.
    /// </summary>
    [Mensagem(TipoMensagemResponse = typeof(ListarOrdensResponse))]
    public class ListarOrdensRequest : MensagemOrdemRequestBase
    {
        /// <summary>
        /// Filtrar operações por código do cliente.
        /// </summary>
        public string FiltroCodigoCliente { get; set; }
        
        /// <summary>
        /// Filtrar operações por código da sessão.
        /// </summary>
        public string FiltroCodigoSessao { get; set; }

        /// <summary>
        /// Filtrar operações com data de execução maior ou igual à data informada
        /// </summary>
        public DateTime? FiltroDataMaiorIgual { get; set; }

        /// <summary>
        /// Filtrar operações com data de execução menor que a data informada
        /// </summary>
        public DateTime? FiltroDataMenor { get; set; }

        /// <summary>
        /// Filtrar operações com data de última alteração maior que a data informada
        /// </summary>
        public DateTime? FiltroDataUltimaAlteracaoMaior { get; set; }

        /// <summary>
        /// Filtrar operações com o status informado
        /// </summary>
        public OrdemStatusEnum? FiltroStatus { get; set; }

        /// <summary>
        /// Permite filtrar por código da bolsa
        /// </summary>
        public string FiltroCodigoBolsa { get; set; }

        /// <summary>
        /// Permite filtrar por instrumento
        /// </summary>
        public string FiltroInstrumento { get; set; }

        /// <summary>
        /// Filtro por código externo
        /// </summary>
        public string FiltroCodigoExterno { get; set; }

        /// <summary>
        /// Permite filtrar ordens cujo código externo seja nulo.
        /// Esta opção excluiria as ordens enviadas pelo, por exemplo, sistema de start e stop.
        /// </summary>
        public bool FiltroCodigoExternoNulo { get; set; }

        /// <summary>
        /// Filtro por código do sistema cliente
        /// </summary>
        public string FiltroCodigoSistemaCliente { get; set; }

        /// <summary>
        /// Filtro por código do canal
        /// </summary>
        public string FiltroCodigoCanal { get; set; }
    }
}
