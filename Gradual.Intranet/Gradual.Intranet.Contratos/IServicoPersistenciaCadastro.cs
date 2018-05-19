using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos
{
    public interface IServicoPersistenciaCadastro
    {
        ConsultarEntidadeCadastroResponse<T> ConsultarEntidadeCadastro<T>(ConsultarEntidadeCadastroRequest<T> parametros) where T : ICodigoEntidade;
        /// <summary>
        /// Insere ou Atualiza um registro de entidades do tipo ICodigoEntidade
        /// </summary>
        /// <typeparam name="T">Qualquer tipo que implemente ICodigoEntidade.</typeparam>
        /// <param name="parametros">Objeto que contendo os dados a serem gravados no banco.</param>
        /// <returns>Objeto de resposta</returns>
        SalvarEntidadeCadastroResponse SalvarEntidadeCadastro<T>(SalvarEntidadeCadastroRequest<T> parametros) where T : ICodigoEntidade;
        ReceberEntidadeCadastroResponse<T> ReceberEntidadeCadastro<T>(ReceberEntidadeCadastroRequest<T> parametros) where T : ICodigoEntidade;
        RemoverEntidadeCadastroResponse RemoverEntidadeCadastro<T>(RemoverEntidadeCadastroRequest<T> parametros) where T : ICodigoEntidade;
    }
}
