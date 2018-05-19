using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    public class RiscoClienteBloqueioGrupoRelDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoClienteBloqueioGrupoRelInfo> ConsultarClienteBloqueioGrupo(ConsultarEntidadeRequest<RiscoClienteBloqueioGrupoRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoClienteBloqueioGrupoRelInfo>();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            lRetorno.Resultado = new List<RiscoClienteBloqueioGrupoRelInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relatorio_cliente_parametro_grupo_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.String, pParametros.Objeto.CdAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.String, pParametros.Objeto.IdParametro);
                lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.String, pParametros.Objeto.IdGrupo);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoClienteBloqueioGrupo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoClienteBloqueioGrupoRelInfo CarregarEntidadeRiscoClienteBloqueioGrupo(DataRow lLinha)
        {
            return new RiscoClienteBloqueioGrupoRelInfo()
            {
                CdAssessor = lLinha["cd_assessor"].DBToInt32(),
                CdCodigo = lLinha["cd_codigo"].DBToString(),
                DsCpfCnpj = lLinha["ds_cpfcnpj"].DBToString(),
                DsGrupo = lLinha["ds_grupo"].DBToString(),
                DsNome = lLinha["ds_nome"].DBToString(),
                DsParametro = lLinha["ds_parametro"].DBToString(),
            };
        }

        #endregion
    }
}
