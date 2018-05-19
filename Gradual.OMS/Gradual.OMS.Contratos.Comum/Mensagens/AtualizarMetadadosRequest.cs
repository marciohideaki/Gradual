using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de atualização de metadados.
    /// Esta mensagem solicita que a persistencia faça a atualização de 
    /// metadados, por exemplo, inserindo os Lista/ListaItem, a lista
    /// de permissões, etc. Cada persistencia trabalha com os metadados
    /// que necessitar
    /// </summary>
    public class AtualizarMetadadosRequest : MensagemRequestBase
    {
    }
}
