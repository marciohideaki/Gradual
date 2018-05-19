using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Servicos.BancoDeDados.DefinicoesDeBanco.Procedures
{
    public class ProcComboSelecionar : ProcedureBase
    {
        #region Propriedades
        
        public Parametro Informacao{ get; set; }
        public Parametro Filtro{ get; set; }

        #endregion

        #region Métodos Públicos
        
        public override void Formatar() 
        {
            Parametros.Add(Informacao);
            Parametros.Add(Filtro);
        }
       
        #endregion

        #region Construtor

        public ProcComboSelecionar()
        {
            this.NomeProcedure = "prc_SelecionaComboSinacor";

            this.Parametros = new List<Parametro>();

            this.Informacao = new Parametro();
            this.Informacao.Direcao = System.Data.ParameterDirection.Input;
            this.Informacao.Nome = "Informacao";
            this.Informacao.Tipo = System.Data.DbType.Int16;
            
            this.Filtro = new Parametro();
            this.Filtro.Direcao = System.Data.ParameterDirection.Input;
            this.Filtro.Nome = "Filtro";
            this.Filtro.Tipo = System.Data.DbType.AnsiString;
        }

        #endregion

    }
}
