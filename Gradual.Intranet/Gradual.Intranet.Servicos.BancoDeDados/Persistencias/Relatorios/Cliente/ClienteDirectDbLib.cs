using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        #region Atributos

        private static string _ConnectionStringName = "Seguranca";
        
        #endregion

        #region Métodos

        /// <summary>
        /// Consultar planos de cliente com filtro de relatório
        /// </summary>
        /// <param name="pRequest">Objeto do tipo ListarProdutosClienteRequest</param>
        /// <returns>Retorna uma lsita com filro efetuado por request</returns>
        public static ConsultarObjetosResponse<ClienteDirectInfo> ConsultarPlanoClientesFiltrado(ConsultarEntidadeRequest<ClienteDirectInfo> pRequest)
        {
            ConsultarObjetosResponse<ClienteDirectInfo> lRetorno = new ConsultarObjetosResponse<ClienteDirectInfo>();

            ConexaoDbHelper acesso = new ConexaoDbHelper();

            acesso.ConnectionStringName = _ConnectionStringName;

            lRetorno.Resultado = new List<ClienteDirectInfo>();

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "prc_FiltrarClienteDirectProduto_sel"))
            {
                acesso.AddInParameter(cmd, "@dt_de",                DbType.DateTime,    pRequest.Objeto.De);
                acesso.AddInParameter(cmd, "@dt_ate",               DbType.DateTime,    pRequest.Objeto.Ate);
                acesso.AddInParameter(cmd, "@id_produto",           DbType.Int32,       pRequest.Objeto.IdProdutoPlano);
                acesso.AddInParameter(cmd, "@cd_cblc",              DbType.Int32,       pRequest.Objeto.CdCblc);
                acesso.AddInParameter(cmd, "@st_cliente_completo",  DbType.Int32,       pRequest.Objeto.StClienteCompleto);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Resultado.Add(CriarRegistroFiltrarPlanoClientesInfo(dr));
            }

            return lRetorno;
        }

        #endregion

        #region Métodos de Apoio

        private static ClienteDirectInfo CriarRegistroFiltrarPlanoClientesInfo(DataRow dr)
        {
            return new ClienteDirectInfo()
            {
                CdCblc         = dr["cd_cblc"].DBToInt32(),
                DtAdesao       = dr["dt_adesao"].DBToDateTime(),
                DtOperacao     = dr["dt_operacao"].DBToDateTime(),
                IdProdutoPlano = dr["id_produto_plano"].DBToInt32(),
                NomeCliente    = dr["ds_nomecliente"].DBToString(),
                DsCpfCnpj      = dr["ds_cpfcnpj"].DBToString(),
                StSituacao     = dr["st_situacao"].DBToChar(),
                NomeProduto    = dr["ds_produto"].DBToString(),
                CdAssessor     = dr["cd_assessor"].DBToInt32()
            };
        }

        #endregion
    }
}
