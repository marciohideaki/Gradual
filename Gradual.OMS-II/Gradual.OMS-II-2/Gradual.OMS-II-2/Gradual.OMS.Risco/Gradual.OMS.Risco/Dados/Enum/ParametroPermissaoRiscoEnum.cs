using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Risco.Enum
{
    public enum ParametroPermissaoRiscoEnum
    {
        OPERAR_BOVESPA = 1,
        OPERAR_BMF = 2,
        OPERAR_NO_MERCADO_AVISTA = 22,
        OPERAR_NO_MERCADO_OPCOES = 29,
        OPERAR_INSTRUMENTO = 23,
        OPERAR_STOPSTART = 35,
        UTILIZAR_CONTAMARGEM = 24,
        UTILIZAR_LIMITE_OPERAR_MERCADO_AVISTA = 26,
        LIMITE_MERCADO_AVISTA = 12,
        LIMITE_VENDA_DESCOBERTO_AVISTA = 5,
        LIMITE_MERCADO_OPCOES = 13,
        LIMITE_DESCOBERTO_OPCOES = 7
    }
}
