using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Tipos de eventos da operacao
    /// </summary>
    public enum ProcessarOperacaoTipoEventoEnum
    {
        /// <summary>
        /// Evento nao informado
        /// </summary>
        NaoInformado,
        
        /// <summary>
        /// Faz a provisao do valor informado. 
        /// É um estado temporário. Quando o sistema
        /// iniciar, serão cancelados todas as parcelas
        /// em estado de provisao
        /// </summary>
        Provisionar,

        /// <summary>
        /// Faz a reserva do valor.
        /// </summary>
        Reservar,

        /// <summary>
        /// Faz a alteração do valor reservado
        /// </summary>
        Alterar,

        /// <summary>
        /// Remove a reserva do valor
        /// </summary>
        Remover
    }
}
