using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Mensagens
{
    #region EntidadesCliente
    /// <summary>
    /// Para uso da camada de tabelas filhas da tabela cliente
    /// </summary>
    public enum EntidadesCliente
    { 
        Emitente = 1,
        Diretor  = 2,
        ProcuradorRepresentante = 3,
        Controladora = 4,
        Conta = 5,
        Contrato = 6,
        Endereco = 7,
        Telefone = 8,
        Banco    = 9,
        SituacaoFinanceiraPatrimonial = 10,
        PendeciaCadastral = 11
    }
    #endregion
}
