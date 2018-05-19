using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Enumerador de status possíveis de críticas
    /// </summary>
    public enum CriticaStatusEnum
    {
        /// <summary>
        /// Indica que um status não se aplica à critica
        /// </summary>
        NaoSeAplica,

        /// <summary>
        /// Indica que é uma crítica informativa, não indicando nenhum tipo
        /// de validade
        /// </summary>
        Informativo,

        /// <summary>
        /// Indica que é uma crítica relativa a uma violação de regra de negócio
        /// </summary>
        ErroNegocio,

        /// <summary>
        /// Indica que é uma crítica relativa a um erro de programa
        /// </summary>
        ErroPrograma,

        /// <summary>
        /// Indica que é uma crítica de uma regra indicando que uma validação foi feita com sucesso
        /// </summary>
        Validado
    }
}
