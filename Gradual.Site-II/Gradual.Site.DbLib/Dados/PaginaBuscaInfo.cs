using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class PaginaBuscaInfo
    {
        #region Propriedades

        public int IdPagina { get; set; }

        public int IdEstrutura { get; set; }

        public string ChaveComposta { get; set; }

        public string ResumoHTML { get; set; }

        #endregion

        #region Construtor

        public PaginaBuscaInfo(string pTermo, string pChave, string pConteudoHTML)
        {
            string[] lChaves = pChave.Split('-');

            if (lChaves.Length == 2)
            {
                this.IdPagina    = Convert.ToInt32(lChaves[0]);
                this.IdEstrutura = Convert.ToInt32(lChaves[1]);
            }

            this.ChaveComposta = pChave;


            int lLocalResultado = pConteudoHTML.IndexOf(pTermo);

            int lInicio, lFim;

            lInicio = lLocalResultado - 400;

            if (lInicio < 0)
                lInicio = 0;

            lFim = lLocalResultado + 400;

            if (lFim >= pConteudoHTML.Length)
                lFim = pConteudoHTML.Length - 1;

            this.ResumoHTML = pConteudoHTML.Substring(lInicio, (lFim - lInicio));
        }

        #endregion
    }
}
