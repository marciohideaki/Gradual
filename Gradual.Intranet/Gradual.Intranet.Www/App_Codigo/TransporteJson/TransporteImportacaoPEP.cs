using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteImportacaoPEP
    {
        #region Propriedades
        
        public string IdDaImportacao { get; set; }
        
        /// <summary>
        /// Status "Em Processamento", "Finalizada", "Com Erro"
        /// </summary>
        public string StatusDaImportacao { get; set; }

        public int RegistrosImportadosComSucesso { get; set; }

        public int RegistrosComErro { get; set; }

        public int RegistrosJaExistentes { get; set; }

        public int RegistrosParaImportar { get; set; }

        /// <summary>
        /// Mensagem quando a exportação termina, com erro ou não.
        /// </summary>
        public string MensagemDeFinalizacao { get; set; }

        public DateTime DataDeInicio { get; set; }

        #endregion

        #region Construtores

        public TransporteImportacaoPEP()
        {
            this.DataDeInicio = DateTime.Now;

            this.StatusDaImportacao = "Em Processamento";
        }

        public TransporteImportacaoPEP(string pIdDaImportacao) : this()
        {
            this.IdDaImportacao = pIdDaImportacao;
        }

        #endregion
    }
}