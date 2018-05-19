using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.Generico.Dados;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco
{
    public partial class RiscoDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoGrupoInfo> ConsultarRiscoGrupos(ConsultarEntidadeRequest<RiscoGrupoInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoGrupoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoNovoOMS;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametros.Objeto.IdGrupo == 0 ? new Nullable<int>() : pParametros.Objeto.IdGrupo);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_grupo", DbType.String, pParametros.Objeto.DsGrupo);
                lAcessaDados.AddInParameter(lDbCommand, "@tp_grupo", DbType.Int32, (int)pParametros.Objeto.TipoGrupo);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoGrupoInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoGrupoInfo CarregarEntidadeRiscoGrupoInfo(DataRow pLinha)
        {
            return new RiscoGrupoInfo()
            {
                DsGrupo = pLinha["ds_grupo"].DBToString(),
                IdGrupo = pLinha["id_grupo"].DBToInt32(),
                TipoGrupo = (EnumRiscoRegra.TipoGrupo)pLinha["tp_grupo"].DBToInt32(),
            };
        }

        #endregion
    }
}
