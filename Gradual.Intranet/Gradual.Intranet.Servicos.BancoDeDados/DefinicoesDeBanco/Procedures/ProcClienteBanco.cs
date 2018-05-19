using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Servicos.BancoDeDados.DefinicoesDeBanco.Procedures
{
    public class ProcClienteBanco : ProcedureBase
    {
        #region Propriedades

        public Parametro Informacao { get; set; }



        public eTipoProc eTipoProcedure { get; set; }

        #endregion

        #region Construtor
        public ProcClienteBanco()
        {
            //this.NomeProcedure = "prc_ListaComboSinacor";
            this.Parametros = new List<Parametro>();
            this.Informacao = new Parametro();
            this.Informacao.Direcao = System.Data.ParameterDirection.Input;
            this.Informacao.Nome = "Informacao";
            this.Informacao.Tipo = System.Data.DbType.Int16;
        }
        #endregion

        #region Metodos Publicos
        public override void Formatar()
        {
            base.Parametros.Add(Informacao);
        }
        #endregion
    }
}
