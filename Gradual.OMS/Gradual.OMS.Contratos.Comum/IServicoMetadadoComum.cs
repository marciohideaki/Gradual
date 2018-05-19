using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Interface para o serviço de metadados para bancos de dados.
    /// A motivação inicial é sincronizar os enumeradores com as tabelas
    /// Lista e ListaItem.
    /// </summary>
    public interface IServicoMetadadoComum
    {
        /// <summary>
        /// Faz a geração e/ou sincronismo dos enumeradores informados
        /// com as tabelas Lista e ListaItem
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        GerarDbMetadadoResponse GerarMetadadoComum(GerarDbMetadadoRequest parametros);
    }
}
