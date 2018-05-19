using System;
using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using System.Collections.Generic;
using System.Data.Common;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {

        public static SalvarEntidadeResponse<ClientePendenciaCadastralInfo> SalvarClientePendenciaCadastral(SalvarObjetoRequest<ClientePendenciaCadastralInfo> pParametros)
        {
            if (pParametros.Objeto.IdPendenciaCadastral > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }
        public static void SalvarClientePendenciaCadastral(DbTransaction pTrans, ClientePendenciaCadastralInfo pParametros)
        {
            try
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_pendenciacadastral_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_pendencia",   DbType.Int32,       pParametros.IdTipoPendencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente",          DbType.Int32,       pParametros.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_pendencia",        DbType.String,      pParametros.DsPendencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_resolucao",        DbType.DateTime,    pParametros.DtResolucao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_resolucao",        DbType.String,      pParametros.DsResolucao);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login",            DbType.Int32,       pParametros.IdLoginRealizacao);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_pendencia_cadastral", DbType.Int32, 16);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    var response = new SalvarEntidadeResponse<ClientePendenciaCadastralInfo>()
                    {
                        Codigo = (lDbCommand.Parameters["@id_pendencia_cadastral"].Value).DBToInt32()
                    };

                    //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    //return response;
                }
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }
        private static SalvarEntidadeResponse<ClientePendenciaCadastralInfo> Salvar(SalvarObjetoRequest<ClientePendenciaCadastralInfo> pParametros)
        {
            try
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_pendenciacadastral_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_pendencia", DbType.Int32, pParametros.Objeto.IdTipoPendencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_pendencia", DbType.String, pParametros.Objeto.DsPendencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_resolucao", DbType.DateTime, pParametros.Objeto.DtResolucao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_resolucao", DbType.String, pParametros.Objeto.DsResolucao);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLoginRealizacao);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_pendencia_cadastral", DbType.Int32, 16);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    var response = new SalvarEntidadeResponse<ClientePendenciaCadastralInfo>()
                    {
                        Codigo = (lDbCommand.Parameters["@id_pendencia_cadastral"].Value).DBToInt32()
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

        private static SalvarEntidadeResponse<ClientePendenciaCadastralInfo> Atualizar(SalvarObjetoRequest<ClientePendenciaCadastralInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_pendenciacadastral_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_pendencia", DbType.Int32, pParametros.Objeto.IdTipoPendencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_pendencia", DbType.String, pParametros.Objeto.DsPendencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_resolucao", DbType.DateTime, pParametros.Objeto.DtResolucao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_resolucao", DbType.String, pParametros.Objeto.DsResolucao);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLoginRealizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pendencia_cadastral", DbType.Int32, pParametros.Objeto.IdPendenciaCadastral);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClientePendenciaCadastralInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        public static RemoverObjetoResponse<ClientePendenciaCadastralInfo> RemoverClientePendenciaCadastral(RemoverEntidadeRequest<ClientePendenciaCadastralInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_pendenciacadastral_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pendencia_cadastral", DbType.Int32, pParametros.Objeto.IdPendenciaCadastral);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                var response = new RemoverEntidadeResponse<ClientePendenciaCadastralInfo>()
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

        public static ReceberObjetoResponse<ClientePendenciaCadastralInfo> ReceberClientePendenciaCadastral(ReceberEntidadeRequest<ClientePendenciaCadastralInfo> pParametros)
        {
            try
            {
                var resposta = new ReceberObjetoResponse<ClientePendenciaCadastralInfo>();

                resposta.Objeto = new ClientePendenciaCadastralInfo();

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_pendenciacadastral_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pendencia_cadastral", DbType.Int32, pParametros.Objeto.IdPendenciaCadastral);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdPendenciaCadastral = (lDataTable.Rows[0]["id_pendencia_cadastral"]).DBToInt32();
                        resposta.Objeto.IdTipoPendencia = (lDataTable.Rows[0]["id_tipo_pendencia"]).DBToInt32();
                        resposta.Objeto.IdCliente = lDataTable.Rows[0]["id_cliente"].DBToInt32();
                        resposta.Objeto.DtPendencia = (lDataTable.Rows[0]["dt_cadastropendencia"]).DBToDateTime();
                        resposta.Objeto.DtResolucao = (lDataTable.Rows[0]["dt_resolucao"]).DBToDateTime();
                        resposta.Objeto.DsPendencia = (lDataTable.Rows[0]["ds_pendencia"]).DBToString();
                        resposta.Objeto.DsResolucao = (lDataTable.Rows[0]["ds_resolucao"]).DBToString();
                        resposta.Objeto.IdLoginRealizacao = (lDataTable.Rows[0]["id_login"]).DBToInt32();
                        resposta.Objeto.DsLoginRealizacao = (lDataTable.Rows[0]["ds_login"]).DBToString();
                        resposta.Objeto.DsTipoPendencia = (lDataTable.Rows[0]["ds_tipo_pendencia"]).DBToString();
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

        public static ConsultarObjetosResponse<ClientePendenciaCadastralInfo> ConsultarClientePendenciaCadastral(ConsultarEntidadeRequest<ClientePendenciaCadastralInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClientePendenciaCadastralInfo>();

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_pendenciacadastral_lst_sp"))
                {
                    if (pParametros.Objeto.IdCliente > 0)
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroClientePendenciaCadastral(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static List<ClientePendenciaCadastralInfo> ListarClientePendenciaCadastral(int IdCliente)
        {
            try
            {
                var resposta = new List<ClientePendenciaCadastralInfo>();

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_pendenciacadastral_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Add(CriarRegistroClientePendenciaCadastral(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static ClientePendenciaCadastralInfo CriarRegistroClientePendenciaCadastral(DataRow linha)
        {
            return new ClientePendenciaCadastralInfo()
                {
                    DsPendencia = linha["ds_pendencia"].DBToString(),
                    DsResolucao = linha["ds_resolucao"].DBToString(),
                    DtPendencia = linha["dt_cadastropendencia"].DBToDateTime(),
                    DtResolucao = linha["dt_resolucao"].DBToDateTime(Contratos.Dados.Enumeradores.eDateNull.Permite),
                    IdCliente = linha["id_cliente"].DBToInt32(),
                    IdPendenciaCadastral = linha["id_pendencia_cadastral"].DBToInt32(),
                    IdTipoPendencia = linha["id_tipo_pendencia"].DBToInt32(),
                    IdLoginRealizacao = linha["id_login"].DBToInt32(),
                    DsLoginRealizacao = linha["ds_login"].DBToString(),
                    DsTipoPendencia = linha["ds_tipo_pendencia"].DBToString()
                };
        }

        private static void LogarModificacao(ClientePendenciaCadastralInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClientePendenciaCadastralInfo> lEntrada = new ReceberEntidadeRequest<ClientePendenciaCadastralInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClientePendenciaCadastralInfo> lRetorno = ReceberClientePendenciaCadastral(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }
    }
}
