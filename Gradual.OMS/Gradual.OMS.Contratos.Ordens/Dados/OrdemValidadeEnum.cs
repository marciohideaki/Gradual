using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Enumerador de tipos de validade de ordens
    /// </summary>
    public enum OrdemValidadeEnum
    {
        NaoInformado,
        NaoImplementado,
        ExecutaIntegralOuCancela,
        ExecutaIntegralParcialOuCancela,
        ValidaParaODia,
        ValidaAteSerCancelada,
        ValidaParaAberturaDoMercado,
        ValidoAteDeterminadaData
    }
}
