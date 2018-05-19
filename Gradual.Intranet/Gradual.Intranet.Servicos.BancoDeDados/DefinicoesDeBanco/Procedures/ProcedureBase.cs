using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Gradual.Generico.Dados;

namespace Gradual.Intranet.Servicos.BancoDeDados.DefinicoesDeBanco.Procedures
{
    public class ProcedureBase : IProcedure
    {
        #region Propriedades
        	 
        public string NomeProcedure { get; set; }
        
        public List<Parametro> Parametros { get; set; }
        
        #endregion

        public virtual void Formatar() {}
        
        #region Métodos Públicos

        public void PreencherAcessoDados(ref AcessaDados pAcessaDados, ref DbCommand pComando)
        {
            foreach (Parametro item in this.Parametros)
            {
                pAcessaDados.AddParameter(pComando, item.Nome, item.Tipo, item.Direcao, null, System.Data.DataRowVersion.Current, item.Valor);
            }
        }

        #endregion
    }
}
