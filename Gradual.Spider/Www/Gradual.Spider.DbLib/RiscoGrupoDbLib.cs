using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.Lib.Dados;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.Spider.DbLib
{
    public class RiscoGrupoDbLib
    {
        private const string NomeConexaoSpider = "GradualSpider";

        public RiscoGrupoInfo ReceberEntidadeRiscoGrupoInfo(RiscoGrupoInfo pParametros)
        {
            var lRetorno = new RiscoGrupoInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametros.CodigoGrupo);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno = (this.CarregarEntidadeRiscoGrupoInfo(lDataTable.Rows[i]));
                    }
                }
            }

            return lRetorno;
        }

        private RiscoGrupoInfo CarregarEntidadeRiscoGrupoInfo(DataRow dr)
        {
            var lRetorno         = new RiscoGrupoInfo();
            
            lRetorno.CodigoGrupo = (int)dr["id_grupo"];
            lRetorno.NomeDoGrupo = dr["ds_grupo"].DBToString();

            return lRetorno;
        }
    }
}
