using System;
using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using System.Data.Common;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        /// <summary>
        /// Realiza a atualização do assessor também no Sinacor.
        /// </summary>
        private static void MigrarClientesNoSinacor(MigracaoClienteAssessorInfo pParametro)
        {
            var lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            Conexao._ConnectionStringName = gNomeConexaoSinacor;
            DbConnection lDbConnection = Conexao.CreateIConnection();
            lDbConnection.Open();
            DbTransaction lDbTransaction = lDbConnection.BeginTransaction();

            try
            {
                int lCdCdBmfBovespaCliente = default(int);

                for (int i = 0; i < pParametro.CdBmfBovespaClientes.Count; i++)
                {
                    if (int.TryParse(pParametro.CdBmfBovespaClientes[i], out lCdCdBmfBovespaCliente))
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, "prc_exp_cc"))
                        {   //--> Atualiza a conta corrente
                            lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESSOR", DbType.Int32, pParametro.IdAssessorDestino);
                            lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, lCdCdBmfBovespaCliente);

                            lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                        }

                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, "prc_exp_cus"))
                        {   //--> Atualiza a conta de custódia
                            lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESSOR", DbType.Int32, pParametro.IdAssessorDestino);
                            lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, lCdCdBmfBovespaCliente);

                            lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                        }

                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, "prc_exp_basag"))
                        {   //--> Atualiza a contas de BM&F e Bovespa
                            lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESSOR", DbType.Int32, pParametro.IdAssessorDestino);
                            lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, lCdCdBmfBovespaCliente);

                            lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                        }
                    }
                }
                lDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                lDbTransaction.Rollback();

                throw ex;
            }
            finally
            {
                if (lDbConnection.State == ConnectionState.Open)
                    lDbConnection.Close();
            }
        }

        public static SalvarEntidadeResponse<MigracaoClienteAssessorInfo> MigracaoClienteAssessor(SalvarObjetoRequest<MigracaoClienteAssessorInfo> pParametros)
        {
            try
            {
                SalvarEntidadeResponse<MigracaoClienteAssessorInfo> resposta = null;
                switch (pParametros.Objeto.Acao)
                {
                    case MigracaoClienteAssessorAcao.MigrarClienteParcial:
                        {
                            string strTemp = string.Empty;
                            pParametros.Objeto.IdsClientes.ForEach(delegate(int id_cliente) { strTemp += id_cliente + ","; });
                            pParametros.Objeto.DsClientes = strTemp.Remove(strTemp.LastIndexOf(','));
                            resposta = MigrarClientesParcialAssessor(pParametros);
                        }
                        break;
                    case MigracaoClienteAssessorAcao.MigrarClienteTodos:
                        resposta = MigrarTodosClientesAssessor(pParametros);
                        break;
                    case MigracaoClienteAssessorAcao.MigrarClienteUnico:
                        resposta = MigrarClienteAssessor(pParametros);

                        break;
                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<MigracaoClienteAssessorInfo> MigrarTodosClientesAssessor(SalvarObjetoRequest<MigracaoClienteAssessorInfo> pParametros)
        {
            DbConnection lDbConnection;
            DbTransaction lDbTransaction;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            lDbConnection = Conexao.CreateIConnection();
            lDbConnection.Open();
            lDbTransaction = lDbConnection.BeginTransaction();

            try
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_assessor_migracao_todos_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor_destino", DbType.Int32, pParametros.Objeto.IdAssessorDestino);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor_origem", DbType.Int32, pParametros.Objeto.IdAssessorOrigem);

                    MigrarClientesNoSinacor(pParametros.Objeto);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lDbTransaction.Commit();

                return new SalvarEntidadeResponse<MigracaoClienteAssessorInfo>();
            }
            catch (Exception ex)
            {
                if (lDbConnection.State == ConnectionState.Open)
                    lDbConnection.Close();

                lDbTransaction.Rollback();

                throw ex;
            }
            finally
            {
                if (lDbConnection.State == ConnectionState.Open)
                    lDbConnection.Close();
            }
        }

        private static SalvarEntidadeResponse<MigracaoClienteAssessorInfo> MigrarClienteAssessor(SalvarObjetoRequest<MigracaoClienteAssessorInfo> pParametros)
        {
            DbConnection lDbConnection;
            DbTransaction lDbTransaction;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            lDbConnection = Conexao.CreateIConnection();
            lDbConnection.Open();
            lDbTransaction = lDbConnection.BeginTransaction();

            try
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_assessor_migracao_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor_destino", DbType.Int32, pParametros.Objeto.IdAssessorDestino);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor_origem", DbType.Int32, pParametros.Objeto.IdAssessorOrigem);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.String, pParametros.Objeto.IdCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                MigrarClientesNoSinacor(pParametros.Objeto);

                lDbTransaction.Commit();

                return new SalvarEntidadeResponse<MigracaoClienteAssessorInfo>();
            }
            catch (Exception ex)
            {
                if (lDbConnection.State == ConnectionState.Open)
                    lDbConnection.Close();

                lDbTransaction.Rollback();

                throw ex;
            }
            finally
            {
                if (lDbConnection.State == ConnectionState.Open)
                    lDbConnection.Close();
            }
        }

        private static SalvarEntidadeResponse<MigracaoClienteAssessorInfo> MigrarClientesParcialAssessor(SalvarObjetoRequest<MigracaoClienteAssessorInfo> pParametros)
        {
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            DbConnection lDbConnection = Conexao.CreateIConnection();
            lDbConnection.Open();
            DbTransaction lDbTransaction = lDbConnection.BeginTransaction();

            try
            {
                var lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, "cliente_parcial_assessor_migracao_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor_destino", DbType.Int32, pParametros.Objeto.IdAssessorDestino);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor_origem", DbType.Int32, pParametros.Objeto.IdAssessorOrigem);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_clientes", DbType.String, pParametros.Objeto.DsClientes);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                MigrarClientesNoSinacor(pParametros.Objeto);

                lDbTransaction.Commit();

                return new SalvarEntidadeResponse<MigracaoClienteAssessorInfo>();
            }
            catch (Exception ex)
            {
                if (lDbConnection.State == ConnectionState.Open)
                    lDbConnection.Close();

                lDbTransaction.Rollback();

                throw ex;
            }
            finally
            {
                if (lDbConnection.State == ConnectionState.Open)
                    lDbConnection.Close();
            }
        }
    }
}
