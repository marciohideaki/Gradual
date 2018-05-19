using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data.Common;
using System;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {

        #region | Métodos CRUD

        public static ConsultarObjetosResponse<ClienteTelefoneInfo> ConsultarClienteTelefone(ConsultarEntidadeRequest<ClienteTelefoneInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteTelefoneInfo> resposta = new ConsultarObjetosResponse<ClienteTelefoneInfo>();
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_telefone_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente == 0 ? (object)System.DBNull.Value : pParametros.Objeto.IdCliente);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (null != lDataTable && lDataTable.Rows.Count.CompareTo(0).Equals(1))
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroTelefone(lDataTable.Rows[i]));
                }
                return resposta;
            }
            catch (Exception ex)
            {
                                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar,ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ClienteTelefoneInfo> ReceberClienteTelefone(ReceberEntidadeRequest<ClienteTelefoneInfo> pParametros)
        {
            ReceberObjetoResponse<ClienteTelefoneInfo> resposta = new ReceberObjetoResponse<ClienteTelefoneInfo>();
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_telefone_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_telefone", DbType.Int32, pParametros.Objeto.IdTelefone);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        resposta.Objeto = CriarRegistroTelefone(lDataTable.Rows[0]);
                }

                return resposta;
            }
            catch (Exception ex)
            {
                               LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber,ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ClienteTelefoneInfo> SalvarClienteTelefone(DbTransaction pTrans, SalvarObjetoRequest<ClienteTelefoneInfo> pParametros)
        {
            return Salvar(pTrans, pParametros);
        }

        public static SalvarEntidadeResponse<ClienteTelefoneInfo> SalvarClienteTelefone(SalvarObjetoRequest<ClienteTelefoneInfo> pParametros)
        {
            if (pParametros.Objeto.IdTelefone > 0)
                return Atualizar(pParametros);
            else
                return Salvar(pParametros);
        }

        private static void LogarModificacao(ClienteTelefoneInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteTelefoneInfo> lEntrada = new ReceberEntidadeRequest<ClienteTelefoneInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteTelefoneInfo> lRetorno = ReceberClienteTelefone(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

        private static SalvarEntidadeResponse<ClienteTelefoneInfo> Atualizar(SalvarObjetoRequest<ClienteTelefoneInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
              
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_telefone_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_telefone", DbType.Int32, pParametros.Objeto.IdTelefone);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_telefone", DbType.Int32, pParametros.Objeto.IdTipoTelefone);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Byte, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_ddd", DbType.String, pParametros.Objeto.DsDdd);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_ramal", DbType.String, pParametros.Objeto.DsRamal);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numero", DbType.String, pParametros.Objeto.DsNumero);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                return new SalvarEntidadeResponse<ClienteTelefoneInfo>();
            }
            catch (Exception ex)
            {
                             LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar,ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<ClienteTelefoneInfo> Salvar(SalvarObjetoRequest<ClienteTelefoneInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteTelefoneInfo> lRetorno;

            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            try
            {
                lRetorno = Salvar(trans, pParametros);
                trans.Commit();
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

        private static SalvarEntidadeResponse<ClienteTelefoneInfo> Salvar(DbTransaction pTrans, SalvarObjetoRequest<ClienteTelefoneInfo> pParametros)
        {
            try
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_telefone_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_telefone", DbType.Int32, pParametros.Objeto.IdTipoTelefone);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Byte, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_ddd", DbType.String, pParametros.Objeto.DsDdd);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_ramal", DbType.String, pParametros.Objeto.DsRamal);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numero", DbType.String, pParametros.Objeto.DsNumero);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_telefone", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    var response = new SalvarEntidadeResponse<ClienteTelefoneInfo>()
                    {
                        Codigo = lDbCommand.Parameters["@id_telefone"].Value.DBToInt32()
                    };
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);
              
                    return response;
                }
            }
            catch (Exception ex)
            {
                              LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir,ex);
                throw ex;
            }
        }

        public static RemoverObjetoResponse<ClienteTelefoneInfo> RemoverClienteTelefone(RemoverEntidadeRequest<ClienteTelefoneInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
               
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_telefone_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_telefone", DbType.Int32, pParametros.Objeto.IdTelefone);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                var response = new RemoverEntidadeResponse<ClienteTelefoneInfo>()
                {
                    lStatus = true
                };
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir);

                return response;
            }
            catch (Exception ex)
            {
                               LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir,ex);
                throw ex;
            }
        }

        #endregion

        #region | Métodos apoio

        public static ClienteTelefoneInfo GetClienteTelefonePrincipal(ClienteInfo pCliente)
        {

            ClienteTelefoneInfo lResposta = new ClienteTelefoneInfo();
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_telefone_pri_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pCliente.IdCliente);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    lResposta = CriarRegistroTelefone(lDataTable.Rows[0]);
            }

            return lResposta;
        }

        private static ClienteTelefoneInfo CriarRegistroTelefone(DataRow linha)
        {
            return new ClienteTelefoneInfo()
            {
                DsDdd = linha["ds_ddd"].DBToString(),
                DsNumero = linha["ds_numero"].DBToString(),
                DsRamal = linha["ds_ramal"].DBToString(),
                IdCliente = linha["id_cliente"].DBToInt32(),
                IdTelefone = linha["id_telefone"].DBToInt32(),
                IdTipoTelefone = linha["id_tipo_telefone"].DBToInt32(),
                StPrincipal = linha["st_principal"].DBToBoolean()
            };
        }

        #endregion
    }
}
