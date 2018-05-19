using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.Intranet.Contratos.Dados;
using System.Data.Common;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        #region | Métodos CRUD
        public static void SalvarImportacaoSuitability(int idClienteAntigo, int idClienteNovo, DbTransaction pTransaction)
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_suitability_swap_ids_upd_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_antigo", DbType.Int32, idClienteAntigo);
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_novo", DbType.Int32, idClienteNovo);

                lAcessaDados.ExecuteNonQuery(lDbCommand, pTransaction);
            }
        }

        public static ConsultarObjetosResponse<ClienteSuitabilityInfo> ConsultarClienteSuitability(ConsultarEntidadeRequest<ClienteSuitabilityInfo> pParametros)
        {
            try{
            ConsultarObjetosResponse<ClienteSuitabilityInfo> lResposta = new ConsultarObjetosResponse<ClienteSuitabilityInfo>();
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_suitability_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente == 0 ? (object)System.DBNull.Value : pParametros.Objeto.IdCliente);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);


                if (null != lDataTable && lDataTable.Rows.Count.CompareTo(0).Equals(1))
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistro(lDataTable.Rows[i]));
            }

            return lResposta;
            }
            catch (Exception ex)
            {
                                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar,ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ClienteSuitabilityInfo> SalvarClienteSuitabilityArquivoCiencia(SalvarObjetoRequest<ClienteSuitabilityInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                SalvarEntidadeResponse<ClienteSuitabilityInfo> lResponse = new SalvarEntidadeResponse<ClienteSuitabilityInfo>();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_suitability_ciencia_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome_arquivo", DbType.String, pParametros.Objeto.ds_arquivo_ciencia);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    return lResponse;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir,ex);

                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ClienteSuitabilityInfo> SalvarClienteSuitability(DbTransaction pTrans,SalvarObjetoRequest<ClienteSuitabilityInfo> pParametros) {
            return Salvar(pTrans, pParametros);
        }

        public static SalvarEntidadeResponse<ClienteSuitabilityInfo> SalvarClienteSuitability(SalvarObjetoRequest<ClienteSuitabilityInfo> pParametros)
        {
            if (pParametros.Objeto.IdClienteSuitability.HasValue)
                throw new NotImplementedException("Id do Suitability enviado, porém o suitability não deve ser atualizado, um novo deve ser incluído");
            else
                return Salvar(pParametros);
        }

        private static SalvarEntidadeResponse<ClienteSuitabilityInfo> Salvar(SalvarObjetoRequest<ClienteSuitabilityInfo> pParametros) 
        {
            SalvarEntidadeResponse<ClienteSuitabilityInfo> lRetorno;

            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            try
            {
                if (pParametros.DescricaoUsuarioLogado == "UPD_SUITABILITY")
                {
                    lRetorno = SalvarClienteSuitabilityArquivoCiencia(pParametros);
                    trans.Rollback(); //não é necessária
                }
                else
                {
                    lRetorno = Salvar(trans, pParametros);
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
                if (!ConnectionState.Closed.Equals(conn.State)) conn.Close();
                conn.Dispose();
                conn = null;
            }
            return lRetorno;
        }

        private static SalvarEntidadeResponse<ClienteSuitabilityInfo> Salvar(DbTransaction pTrans,  SalvarObjetoRequest<ClienteSuitabilityInfo> pParametros)
        {
            try{
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            SalvarEntidadeResponse<ClienteSuitabilityInfo> lResponse = new SalvarEntidadeResponse<ClienteSuitabilityInfo>();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_suitability_integracao_ins_sp"))
            {
                if (pParametros.Objeto.CdCblc.HasValue)
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cblc", DbType.Int32, pParametros.Objeto.CdCblc);
                }

                if (pParametros.Objeto.IdCliente.HasValue)
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                }

                lAcessaDados.AddInParameter(lDbCommand, "@ds_perfil",                DbType.String,   pParametros.Objeto.ds_perfil);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_status",                DbType.String,   pParametros.Objeto.ds_status);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_realizacao",            DbType.Date,     pParametros.Objeto.dt_realizacao);
                lAcessaDados.AddInParameter(lDbCommand, "@st_preenchidopelocliente", DbType.Boolean,  pParametros.Objeto.st_preenchidopelocliente);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_loginrealizado",        DbType.String,   pParametros.Objeto.ds_loginrealizado);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_fonte",                 DbType.String,   pParametros.Objeto.ds_fonte);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_respostas",             DbType.String,   pParametros.Objeto.ds_respostas.Replace("|",""));

                lAcessaDados.AddOutParameter(lDbCommand, "@id_cliente_suitability", DbType.Int32, 8);

                lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                lResponse.Codigo = lDbCommand.Parameters["@id_cliente_suitability"].Value.DBToInt32();
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);
                return lResponse;
            }
            }
            catch (Exception ex)
            {
                               LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir,ex);
                throw ex;
            }
        }
        
        #endregion

        #region | Métodos apoio
        
        private static ClienteSuitabilityInfo CriarRegistro(DataRow pRow)
        {
            return new ClienteSuitabilityInfo()
            {
                IdCliente                = pRow["id_cliente"].DBToInt32(),
                IdClienteSuitability     = pRow["id_cliente_suitability"].DBToInt32(),
                ds_perfil                = pRow["ds_perfil"].DBToString(),
                ds_fonte                 = pRow["ds_fonte"].DBToString(),
                ds_loginrealizado        = pRow["ds_loginrealizado"].DBToString(),
                ds_respostas             = pRow["ds_respostas"].DBToString(),
                ds_status                = pRow["ds_status"].DBToString(),
                st_preenchidopelocliente = pRow["st_preenchidopelocliente"].DBToBoolean(),
                dt_realizacao            = pRow["dt_realizacao"].DBToDateTime(),
                ds_arquivo_ciencia       = pRow["ds_arquivo_ciencia"].DBToString(),
                dt_arquivo_upload        = pRow["dt_arquivo_upload"].DBToDateTime()
            };
        }

        #endregion


    }
}
