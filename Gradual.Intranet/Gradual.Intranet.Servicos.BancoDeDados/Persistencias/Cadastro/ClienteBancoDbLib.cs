using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<ClienteBancoInfo> ConsultarClienteBanco(ConsultarEntidadeRequest<ClienteBancoInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteBancoInfo>();

                var info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_banco_lst_sp"))
                {
                    DataTable lDataTable;

                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);
                        lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                                resposta.Resultado.Add(CriarRegistroClienteBanco(lDataTable.Rows[i]));
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ClienteBancoInfo> ReceberClienteBanco(ReceberEntidadeRequest<ClienteBancoInfo> pParametros)
        {
            try
            {
                var resposta = new ReceberObjetoResponse<ClienteBancoInfo>();

                resposta.Objeto = new ClienteBancoInfo();

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_banco_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_banco", DbType.Int32, pParametros.Objeto.IdBanco);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdBanco = (lDataTable.Rows[0]["id_banco"]).DBToInt32();
                        resposta.Objeto.CdBanco = (lDataTable.Rows[0]["cd_banco"]).DBToString();
                        resposta.Objeto.DsAgencia = (lDataTable.Rows[0]["ds_agencia"]).DBToString();
                        resposta.Objeto.DsAgenciaDigito = (lDataTable.Rows[0]["ds_agencia_digito"]).DBToString();
                        resposta.Objeto.DsConta = (lDataTable.Rows[0]["ds_conta"]).DBToString();
                        resposta.Objeto.DsContaDigito = (lDataTable.Rows[0]["ds_conta_digito"]).DBToString();
                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.DsNomeTitular = Convert.ToString(lDataTable.Rows[0]["ds_nometitular"]);
                        resposta.Objeto.DsCpfTitular =  Convert.ToString(lDataTable.Rows[0]["ds_cpftitular"]);
                        resposta.Objeto.StPrincipal = Boolean.Parse(lDataTable.Rows[0]["st_principal"].ToString());
                        resposta.Objeto.TpConta = (lDataTable.Rows[0]["tp_conta"]).DBToString();
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        private static ClienteBancoInfo CriarRegistroClienteBanco(DataRow linha)
        {
            ClienteBancoInfo lClienteBancoInfo = new ClienteBancoInfo();

            lClienteBancoInfo.CdBanco = linha["cd_banco"].DBToString();
            lClienteBancoInfo.DsAgencia = linha["ds_agencia"].DBToString();
            lClienteBancoInfo.DsAgenciaDigito = linha["ds_agencia_digito"].DBToString();
            lClienteBancoInfo.DsConta = linha["ds_conta"].DBToString();
            lClienteBancoInfo.DsContaDigito = linha["ds_conta_digito"].DBToString();
            lClienteBancoInfo.IdBanco = linha["id_banco"].DBToInt32();
            lClienteBancoInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteBancoInfo.StPrincipal = Boolean.Parse(linha["st_principal"].ToString());
            lClienteBancoInfo.TpConta = linha["tp_conta"].DBToString();
            
            lClienteBancoInfo.DsNomeTitular = Convert.ToString(linha["ds_nometitular"]);
            lClienteBancoInfo.DsCpfTitular =  Convert.ToString(linha["ds_cpftitular"]);

            return lClienteBancoInfo;
        }

        public static RemoverObjetoResponse<ClienteBancoInfo> RemoverClienteBanco(RemoverEntidadeRequest<ClienteBancoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_banco_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_banco", DbType.Int32, pParametros.Objeto.IdBanco);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClienteBancoInfo> response = new RemoverEntidadeResponse<ClienteBancoInfo>()
                {
                    lStatus = true
                };

                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir);

                return response;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ClienteBancoInfo> SalvarClienteBanco(DbTransaction pTrans, SalvarObjetoRequest<ClienteBancoInfo> pParametros)
        {
            return Salvar(pTrans, pParametros);
        }

        public static SalvarEntidadeResponse<ClienteBancoInfo> SalvarClienteBanco(SalvarObjetoRequest<ClienteBancoInfo> pParametros)
        {
            if (pParametros.Objeto.IdBanco > 0)
                return Atualizar(pParametros);
            else
                return Salvar(pParametros);
        }

        private static SalvarEntidadeResponse<ClienteBancoInfo> Salvar(SalvarObjetoRequest<ClienteBancoInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteBancoInfo> lRetorno;
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

        private static SalvarEntidadeResponse<ClienteBancoInfo> Salvar(DbTransaction pTrans, SalvarObjetoRequest<ClienteBancoInfo> pParametros)
        {
            try
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_banco_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_agencia", DbType.AnsiString, pParametros.Objeto.DsAgencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_agencia_digito", DbType.AnsiString, pParametros.Objeto.DsAgenciaDigito);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_conta", DbType.AnsiString, pParametros.Objeto.DsConta);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_conta_digito", DbType.AnsiString, pParametros.Objeto.DsContaDigito);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Boolean, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_conta", DbType.AnsiString, pParametros.Objeto.TpConta);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_banco", DbType.AnsiString, pParametros.Objeto.CdBanco);
                    
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nometitular", DbType.AnsiString, pParametros.Objeto.DsNomeTitular);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpftitular",  DbType.AnsiString, pParametros.Objeto.DsCpfTitular);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_banco", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    var response = new SalvarEntidadeResponse<ClienteBancoInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_banco"].Value)
                    };

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<ClienteBancoInfo> Atualizar(SalvarObjetoRequest<ClienteBancoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_banco_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_agencia", DbType.String, pParametros.Objeto.DsAgencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_agencia_digito", DbType.String, pParametros.Objeto.DsAgenciaDigito);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_conta", DbType.String, pParametros.Objeto.DsConta);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_conta_digito", DbType.String, pParametros.Objeto.DsContaDigito);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Boolean, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_conta", DbType.String, pParametros.Objeto.TpConta);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_banco", DbType.String, pParametros.Objeto.CdBanco);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_banco", DbType.String, pParametros.Objeto.IdBanco);
                    
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nometitular", DbType.String, pParametros.Objeto.DsNomeTitular);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpftitular",  DbType.String, pParametros.Objeto.DsCpfTitular);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteBancoInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        public static ClienteBancoInfo GetClienteBancoPrincipal(ClienteInfo pCliente)
        {
            try
            {
                var lRresposta = new ClienteBancoInfo();

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_banco_pri_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_Cliente", DbType.Int32, pCliente.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRresposta.IdBanco = (lDataTable.Rows[0]["id_banco"]).DBToInt32();
                        lRresposta.CdBanco = (lDataTable.Rows[0]["cd_banco"]).DBToString();
                        lRresposta.DsAgencia = (lDataTable.Rows[0]["ds_agencia"]).DBToString();
                        lRresposta.DsAgenciaDigito = (lDataTable.Rows[0]["ds_agencia_digito"]).DBToString();
                        lRresposta.DsConta = (lDataTable.Rows[0]["ds_conta"]).DBToString();
                        lRresposta.DsContaDigito = (lDataTable.Rows[0]["ds_conta_digito"]).DBToString();
                        lRresposta.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        lRresposta.StPrincipal = Boolean.Parse(lDataTable.Rows[0]["st_principal"].ToString());
                        lRresposta.TpConta = (lDataTable.Rows[0]["tp_conta"]).DBToString();

                        try
                        {
                            lRresposta.DsNomeTitular = Convert.ToString(lDataTable.Rows[0]["ds_nometitular"]);
                            lRresposta.DsCpfTitular  = Convert.ToString(lDataTable.Rows[0]["ds_cpftitular"]);
                        }
                        catch { }
                    }
                }

                return lRresposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pCliente, 0, string.Empty, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        private static void LogarModificacao(ClienteBancoInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            var lEntrada = new ReceberEntidadeRequest<ClienteBancoInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteBancoInfo> lRetorno = ReceberClienteBanco(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }
    }
}
