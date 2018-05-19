using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.LeitorDeArquivos
{
    public class ResultadoDeImportacao
    {
        #region Propriedades

        public string CaminhoDoArquivoDeDados { get; set; }

        public bool Finalizada { get; set; }

        public string Status { get; set; }

        #endregion

        #region Métodos Públicos

        public void MarcarComoFinalizada(string pStatus)
        {
            this.Finalizada = true;

            this.Status = pStatus;
        }

        #endregion

        #region Construtor

        public ResultadoDeImportacao()
        {
            this.Finalizada = false;
        }
        
        public ResultadoDeImportacao(string pCaminhoDoArquivoDeDados) : this()
        {
            this.CaminhoDoArquivoDeDados = pCaminhoDoArquivoDeDados;
        }

        #endregion
    }
}
