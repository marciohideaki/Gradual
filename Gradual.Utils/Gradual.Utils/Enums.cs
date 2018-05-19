using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Utils
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

        public enum TipoSinal
        {
            CotacaoRapida,
            LivroOfertas,
            LivroNegocios,
            RankingCorretoras,
            Destaques,
            Acompanhamento,
            TopoLivroOfertas,
            NegociosDestaque,
            AcompanhamentoLeilao,
            ResumoCorretoras,
            Noticias,
            LivroOfertasAgregado
        }
    }
}
