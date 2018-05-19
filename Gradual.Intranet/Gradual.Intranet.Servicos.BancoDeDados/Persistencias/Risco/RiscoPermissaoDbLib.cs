using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco
{
    public partial class RiscoDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoPermissaoInfo> ConsultarRiscoPermissoes(ConsultarEntidadeRequest<RiscoPermissaoInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoPermissaoInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno.Resultado = new List<RiscoPermissaoInfo>();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoNovoOMS;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_permissao_lst"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoPermissaoInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoPermissaoInfo CarregarEntidadeRiscoPermissaoInfo(DataRow linha)
        {
            return new RiscoPermissaoInfo() 
            { 
                DsPermissao = linha["ds_permissao"].DBToString(), 
                IdPermissao = linha["id_permissao"].DBToInt32(),  
            };
        }

        #endregion
    }
}
