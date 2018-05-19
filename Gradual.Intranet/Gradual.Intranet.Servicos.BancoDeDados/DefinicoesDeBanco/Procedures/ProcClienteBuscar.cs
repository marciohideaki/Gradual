using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Servicos.BancoDeDados.DefinicoesDeBanco.Procedures
{
    public class ProcClienteBuscar : ProcedureBase
    {
        #region Propriedades

        public Parametro Informacao { get; set; }

        #endregion

        #region Construtor

        public ProcClienteBuscar()
        {
            this.NomeProcedure      = "cliente_resumido_sel_sp";

            this.Parametros         = new List<Parametro>();

            this.Informacao         = new Parametro();
            this.Informacao.Direcao = System.Data.ParameterDirection.Input;
            this.Informacao.Nome    = "Informacao";
            this.Informacao.Tipo    = System.Data.DbType.Int16;
        }
        
        #endregion

        #region Métodos Públicos

        public override void Formatar()
        {
            //base.Parametros.Add(Informacao);
        }
                
        #endregion
    }
}
