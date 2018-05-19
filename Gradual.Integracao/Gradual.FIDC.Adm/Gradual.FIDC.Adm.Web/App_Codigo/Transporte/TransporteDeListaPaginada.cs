using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    public class TransporteDeListaPaginada
    {
        #region Propriedades

        public static int ItensPorPagina = 20;      //TODO: 10 itens por página, fixo. Pra mudar, o code-behind está pronto, mas precisa alterar o js tb

        public static int ItensPorPaginaAcompanhamento = 40;

        public int TotalDePaginas { get; set; }

        public int TotalDeItens { get; set; }

        public int PaginaAtual { get; set; }

        public IEnumerable Itens { get; set; }

        public IEnumerable Itens2 { get; set; }

        #endregion

        #region Construtor

        public TransporteDeListaPaginada() { }

        public TransporteDeListaPaginada(IEnumerable pItens)
            : this()
        {
            this.Itens = pItens;
        }

        public TransporteDeListaPaginada(IEnumerable pItens, IEnumerable pItens2)
            : this()
        {
            this.Itens = pItens;
            this.Itens2 = pItens2;
        }

        #endregion
    }
}