using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    public partial class RiscoRelDbLib
    {
        #region | Métodos de consulta

        public ConsultarObjetosResponse<RiscoPermissaoRelInfo> ConsultarRiscoPermissao(ConsultarEntidadeRequest<RiscoPermissaoRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoPermissaoRelInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relatorio_permissao_sp"))
            {
                if (null != pParametros.Objeto.ConsultaBolsa && !0.Equals(pParametros.Objeto.ConsultaBolsa))
                    lAcessaDados.AddInParameter(lDbCommand, "@id_bolsa", DbType.Int32, pParametros.Objeto.ConsultaBolsa);

                if (null != pParametros.Objeto.ConsultaPermissao && !0.Equals(pParametros.Objeto.ConsultaPermissao))
                    lAcessaDados.AddInParameter(lDbCommand, "@id_permissao", DbType.Int32, pParametros.Objeto.ConsultaPermissao);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoPermissaoRelInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoPermissaoRelInfo CarregarEntidadeRiscoPermissaoRelInfo(DataRow pLinha)
        {
            return new RiscoPermissaoRelInfo()
            {
                DsBolsa = pLinha["ds_bolsa"].DBToString(),
                DsPermissao = pLinha["ds_permissao"].DBToString(),
            };
        }

        #endregion
    }
}
