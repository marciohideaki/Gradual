using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradualForm
{
    public class Enums
    {
        public enum TipoVisualizacao
        {
            Incremental = 0,
            Agregado = 1
        };

        public enum TipoAbertura
        {
            Quantidade,
            Preco
        }

        public enum AberturaPor
        {
            VAZIO = 0,
            LOTE_PADRAO = 1,
            QUANTIDADE_DEFINIDA = 2
        }
    }
}
