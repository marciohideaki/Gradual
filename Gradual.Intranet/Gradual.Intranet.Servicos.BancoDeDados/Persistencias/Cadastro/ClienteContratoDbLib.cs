using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using System.Collections.Generic;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static List<ClienteContratoInfo> ConsultarClienteContrato (int id_Cliente)
        {
            try
            {
                List<ClienteContratoInfo> resposta = new List<ClienteContratoInfo>();

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_contrato_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, id_Cliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["id_cliente"]    = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["id_contrato"]   = (lDataTable.Rows[i]["id_contrato"]).DBToInt32();
                            linha["dt_assinatura"] = (lDataTable.Rows[i]["dt_assinatura"]).DBToDateTime();

                            resposta.Add(CriarClienteContrato(linha));
                        }
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }

        }

        public static ConsultarObjetosResponse<ClienteContratoInfo> ConsultarClienteContrato(ConsultarEntidadeRequest<ClienteContratoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteContratoInfo> resposta = new ConsultarObjetosResponse<ClienteContratoInfo>();

                string lProcedure = "cliente_contrato_lst_sp";

                if (pParametros.Objeto.CodigoBovespaCliente == null)
                {
                    CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);

                    pParametros.Condicoes.Add(info);
                }
                else
                {
                    CondicaoInfo info = new CondicaoInfo("@cod_bovespa_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.CodigoBovespaCliente);

                    pParametros.Condicoes.Add(info);

                    lProcedure = "cliente_contrato_lst_bov_sp";
                }

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, lProcedure))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);
                    }

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["id_contrato"] = (lDataTable.Rows[i]["id_contrato"]).DBToInt32();
                            linha["dt_assinatura"] = (lDataTable.Rows[i]["dt_assinatura"]).DBToDateTime();

                            resposta.Resultado.Add(CriarClienteContrato(linha));
                        }
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

        private static ClienteContratoInfo CriarClienteContrato(DataRow linha)
        {
            ClienteContratoInfo lClienteContrato = new ClienteContratoInfo();

            lClienteContrato.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteContrato.IdContrato = linha["id_contrato"].DBToInt32();
            lClienteContrato.DtAssinatura = linha["dt_assinatura"].DBToDateTime();

            return lClienteContrato;

        }

        public static ReceberObjetoResponse<ClienteContratoInfo> ReceberClienteContrato(ReceberEntidadeRequest<ClienteContratoInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteContratoInfo> resposta =
                    new ReceberObjetoResponse<ClienteContratoInfo>();

                resposta.Objeto = new ClienteContratoInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_contrato_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contrato", DbType.Int32, pParametros.Objeto.IdContrato);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.IdContrato = (lDataTable.Rows[0]["id_contrato"]).DBToInt32();
                        resposta.Objeto.DtAssinatura = (lDataTable.Rows[0]["dt_assinatura"]).DBToDateTime();
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

        public static RemoverObjetoResponse<ClienteContratoInfo> RemoverClienteContrato(RemoverEntidadeRequest<ClienteContratoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_contrato_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contrato", DbType.Int32, pParametros.Objeto.IdContrato);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClienteContratoInfo> response = new RemoverEntidadeResponse<ClienteContratoInfo>()
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

        private static void RemoverClienteContrato(DbTransaction pTrans, int pIdCliente)
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_contrato_del_por_cliente_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pIdCliente);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }
        }

        public static ClienteContratoInfo SalvarImportacaoClienteContrato(DbTransaction pTrans, ClienteContratoInfo pContrato)
        {
            try
            {
                //LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                //RemoverClienteContrato(pTrans, Convert.ToInt32(pParametros.IdCliente));

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                int lCodigoCliente = 0;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_contrato_ins_sp"))
                {
                    lDbCommand.Parameters.Clear();

                    lCodigoCliente = pContrato.IdCliente.Value;

                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pContrato.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contrato", DbType.Int32, pContrato.IdContrato);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_assinatura", DbType.DateTime, pContrato.DtAssinatura);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    ClienteContratoInfo response = new ClienteContratoInfo()
                    {
                        IdCliente = lCodigoCliente
                    };
                    
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ClienteContratoInfo> SalvarClienteContrato(DbTransaction pTrans, SalvarObjetoRequest<ClienteContratoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                RemoverClienteContrato(pTrans, Convert.ToInt32(pParametros.Objeto.IdCliente));

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                string lCommandName = "cliente_contrato_ins_sp";
                string lIdParamName = "@id_cliente";

                Nullable<int> lIdValue = pParametros.Objeto.IdCliente;

                if (pParametros.Objeto.CodigoBovespaCliente.HasValue)
                {
                    lCommandName = "cliente_contrato_ins_bov_sp";

                    lIdParamName = "@cod_bovespa_cliente";

                    lIdValue = pParametros.Objeto.CodigoBovespaCliente.Value;
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, lCommandName))
                {
                    foreach (int idContrato in pParametros.Objeto.LstIdContrato)
                    {
                        lDbCommand.Parameters.Clear();
                        lAcessaDados.AddInParameter(lDbCommand, lIdParamName, DbType.Int32, lIdValue);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_contrato", DbType.Int32, idContrato);
                        lAcessaDados.AddInParameter(lDbCommand, "@dt_assinatura", DbType.DateTime, pParametros.Objeto.DtAssinatura);

                        lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);
                    }

                    SalvarEntidadeResponse<ClienteContratoInfo> response = new SalvarEntidadeResponse<ClienteContratoInfo>()
                    {
                        Codigo = pParametros.Objeto.IdCliente
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

        public static SalvarEntidadeResponse<ClienteContratoInfo> SalvarClienteContrato(SalvarObjetoRequest<ClienteContratoInfo> pParametros)
        {
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            DbConnection lConn = Conexao.CreateIConnection();
            lConn.Open();
            DbTransaction lTrans = lConn.BeginTransaction();

            try
            {
                SalvarEntidadeResponse<ClienteContratoInfo> lRetorno = SalvarClienteContrato(lTrans, pParametros);
                lTrans.Commit();
                return lRetorno;

            }
            catch (Exception ex)
            {
                lTrans.Rollback();
                throw ex;
            }
            finally
            {
                lTrans.Dispose();
                lTrans = null;
                if (!ConnectionState.Closed.Equals(lConn.State))
                    lConn.Close();
                lConn.Dispose();
                lConn = null;
            }
        }

        private static void LogarModificacao(ClienteContratoInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteContratoInfo> lEntrada = new ReceberEntidadeRequest<ClienteContratoInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteContratoInfo> lRetorno = ReceberClienteContrato(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }
    }
}
