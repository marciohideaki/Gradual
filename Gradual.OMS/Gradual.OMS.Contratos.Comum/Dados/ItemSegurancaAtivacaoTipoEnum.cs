using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Indica a regra para descobrir se o acesso ao item de 
    /// segurança é ou não permitido.
    /// </summary>
    public enum ItemSegurancaAtivacaoTipoEnum
    {
        /// <summary>
        /// Não faz a validação de permissões, ou seja, o 
        /// item é valido para todos
        /// </summary>
        NaoValidar,

        /// <summary>
        /// Indica que a ativação será feita quando qualquer uma das condições seja satisfeita.
        /// </summary>
        QualquerCondicao,

        /// <summary>
        /// Indica que a ativação será feita quando todas as condições forem satisfeitas.
        /// </summary>
        TodasAsCondicoes
    }
}
