using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        public static SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo> SalvarClienteProcuradorRepresentante(SalvarObjetoRequest<ClienteProcuradorRepresentanteInfo> pParametros)
        {
            if (pParametros.Objeto.IdProcuradorRepresentante > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        public static SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo> SalvarClienteProcuradorRepresentante(DbTransaction pTrans, SalvarObjetoRequest<ClienteProcuradorRepresentanteInfo> pParametros)
        {
            return Salvar(pTrans, pParametros);
        }

        public static SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo> Salvar(SalvarObjetoRequest<ClienteProcuradorRepresentanteInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo> lRetorno;

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

        private static SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo> Salvar(DbTransaction pTrans, SalvarObjetoRequest<ClienteProcuradorRepresentanteInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_procuradorrepresentante_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.NrCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimento", DbType.DateTime, pParametros.Objeto.DtNascimento == DateTime.MinValue ? new Nullable<DateTime>() : pParametros.Objeto.DtNascimento.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numerodocumento", DbType.String, pParametros.Objeto.DsNumeroDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_orgaoemissor", DbType.String, pParametros.Objeto.CdOrgaoEmissor);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_uforgaoemissordocumento", DbType.String, pParametros.Objeto.CdUfOrgaoEmissor);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_documento", DbType.String, pParametros.Objeto.TpDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_situacaoLegal", DbType.Int32, pParametros.Objeto.TpSituacaoLegal);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_validade", DbType.DateTime, pParametros.Objeto.DtValidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_procuradorrepresentante", DbType.Int32, (int)pParametros.Objeto.TpProcuradorRepresentante);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_procuradorrepresentante", DbType.Int32, 16);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo> response = new SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo>()
                    {
                        Codigo = (lDbCommand.Parameters["@id_procuradorrepresentante"].Value).DBToInt32()
                    };
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    //AtualizarOperaPorContaPropria(new ClienteInfo() { IdCliente = pParametros.Objeto.IdCliente });

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo> Atualizar(SalvarObjetoRequest<ClienteProcuradorRepresentanteInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_procuradorrepresentante_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.NrCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimento", DbType.DateTime, pParametros.Objeto.DtNascimento == DateTime.MinValue ? new Nullable<DateTime>() : pParametros.Objeto.DtNascimento.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numerodocumento", DbType.String, pParametros.Objeto.DsNumeroDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_orgaoemissor", DbType.String, pParametros.Objeto.CdOrgaoEmissor);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_uforgaoemissordocumento", DbType.String, pParametros.Objeto.CdUfOrgaoEmissor);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_documento", DbType.String, pParametros.Objeto.TpDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_situacaoLegal", DbType.Int32, pParametros.Objeto.TpSituacaoLegal);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_procuradorrepresentante", DbType.Int32, pParametros.Objeto.IdProcuradorRepresentante);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_validade", DbType.DateTime, pParametros.Objeto.DtValidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_procuradorrepresentante", DbType.Int32, (int)pParametros.Objeto.TpProcuradorRepresentante);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    //AtualizarOperaPorContaPropria(new ClienteInfo() { IdCliente = pParametros.Objeto.IdCliente });

                    return new SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        public static RemoverObjetoResponse<ClienteProcuradorRepresentanteInfo> RemoverClienteProcuradorRepresentante(RemoverEntidadeRequest<ClienteProcuradorRepresentanteInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                var lProcuradorRepresentante = ReceberClienteProcuradorRepresentante(new ReceberEntidadeRequest<ClienteProcuradorRepresentanteInfo>() { Objeto = pParametros.Objeto });

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_procuradorrepresentante_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_procuradorrepresentante", DbType.Int32, pParametros.Objeto.IdProcuradorRepresentante);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClienteProcuradorRepresentanteInfo> response = new RemoverEntidadeResponse<ClienteProcuradorRepresentanteInfo>()
                {
                    lStatus = true
                };
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir);

                //AtualizarOperaPorContaPropria(new ClienteInfo() { IdCliente = lProcuradorRepresentante.Objeto.IdCliente });

                return response;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ClienteProcuradorRepresentanteInfo> ReceberClienteProcuradorRepresentante(ReceberEntidadeRequest<ClienteProcuradorRepresentanteInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteProcuradorRepresentanteInfo> resposta =
                    new ReceberObjetoResponse<ClienteProcuradorRepresentanteInfo>();

                resposta.Objeto = new ClienteProcuradorRepresentanteInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_procuradorrepresentante_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_procuradorrepresentante", DbType.Int32, pParametros.Objeto.IdProcuradorRepresentante);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.CdOrgaoEmissor = (lDataTable.Rows[0]["cd_orgaoemissor"]).DBToString();
                        resposta.Objeto.CdUfOrgaoEmissor = (lDataTable.Rows[0]["cd_uforgaoemissordocumento"]).DBToString();
                        resposta.Objeto.NrCpfCnpj = lDataTable.Rows[0]["ds_cpfcnpj"].DBToString();
                        resposta.Objeto.DsNome = (lDataTable.Rows[0]["ds_nome"]).DBToString();
                        resposta.Objeto.DsNumeroDocumento = (lDataTable.Rows[0]["ds_numerodocumento"]).DBToString();
                        resposta.Objeto.DtNascimento = (lDataTable.Rows[0]["dt_nascimento"]).DBToDateTime();
                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.IdProcuradorRepresentante = (lDataTable.Rows[0]["id_procuradorrepresentante"]).DBToInt32();
                        resposta.Objeto.TpDocumento = (lDataTable.Rows[0]["tp_documento"]).DBToString();
                        resposta.Objeto.TpSituacaoLegal = (lDataTable.Rows[0]["tp_situacaolegal"]).DBToInt32();
                        resposta.Objeto.DtValidade = (lDataTable.Rows[0]["dt_validade"]).DBToDateTime(Contratos.Dados.Enumeradores.eDateNull.Permite);
                        resposta.Objeto.TpProcuradorRepresentante = (TipoProcuradorRepresentante)lDataTable.Rows[0]["tp_procuradorrepresentante"].DBToInt32();
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

        public static ConsultarObjetosResponse<ClienteProcuradorRepresentanteInfo> ConsultarClienteProcuradorRepresentante(ConsultarEntidadeRequest<ClienteProcuradorRepresentanteInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteProcuradorRepresentanteInfo> resposta =
                    new ConsultarObjetosResponse<ClienteProcuradorRepresentanteInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_procuradorrepresentante_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroClienteProcuradorRepresentanteInfo(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteProcuradorRepresentanteInfo CriarRegistroClienteProcuradorRepresentanteInfo(DataRow linha)
        {
            var lClienteProcuradorRepresentanteInfo = new ClienteProcuradorRepresentanteInfo();

            lClienteProcuradorRepresentanteInfo.CdOrgaoEmissor = (linha["cd_orgaoemissor"]).DBToString();
            lClienteProcuradorRepresentanteInfo.CdUfOrgaoEmissor = (linha["cd_uforgaoemissordocumento"]).DBToString();
            lClienteProcuradorRepresentanteInfo.NrCpfCnpj = linha["ds_cpfcnpj"].DBToString();
            lClienteProcuradorRepresentanteInfo.DsNome = linha["ds_nome"].DBToString();
            lClienteProcuradorRepresentanteInfo.DsNumeroDocumento = linha["ds_numerodocumento"].DBToString();
            lClienteProcuradorRepresentanteInfo.DtNascimento = linha["dt_nascimento"].DBToDateTime();
            lClienteProcuradorRepresentanteInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteProcuradorRepresentanteInfo.IdProcuradorRepresentante = linha["id_procuradorrepresentante"].DBToInt32();
            lClienteProcuradorRepresentanteInfo.TpDocumento = linha["tp_documento"].DBToString();
            lClienteProcuradorRepresentanteInfo.TpSituacaoLegal = linha["tp_situacaoLegal"].DBToInt32();
            lClienteProcuradorRepresentanteInfo.DtValidade = linha["dt_validade"].DBToDateTime() == DateTime.MinValue ? new System.Nullable<DateTime>() : linha["dt_validade"].DBToDateTime();
            lClienteProcuradorRepresentanteInfo.TpProcuradorRepresentante = (TipoProcuradorRepresentante)linha["tp_procuradorrepresentante"].DBToInt32();

            return lClienteProcuradorRepresentanteInfo;
        }

        private static void LogarModificacao(ClienteProcuradorRepresentanteInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteProcuradorRepresentanteInfo> lEntrada = new ReceberEntidadeRequest<ClienteProcuradorRepresentanteInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteProcuradorRepresentanteInfo> lRetorno = ReceberClienteProcuradorRepresentante(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

        /// <summary>
        /// Atualiza o estado o opera por carteira própria dependendo se foi definido um representante.
        /// </summary>
        private static void AtualizarOperaPorContaPropria(ClienteInfo pCliente)
        {
            var lRetornoProcuradorRepresentante = ConsultarClienteProcuradorRepresentante(
                new ConsultarEntidadeRequest<ClienteProcuradorRepresentanteInfo>()
                {
                    Objeto = new ClienteProcuradorRepresentanteInfo() { IdCliente = pCliente.IdCliente }
                });

            pCliente = ReceberCliente(new ReceberEntidadeRequest<ClienteInfo>() { Objeto = pCliente }).Objeto;

            var lStOperaCarteiraPropria = (null != lRetornoProcuradorRepresentante.Resultado && lRetornoProcuradorRepresentante.Resultado.Count > 0);

            pCliente.StCarteiraPropria = lStOperaCarteiraPropria;

            SalvarCliente(new SalvarObjetoRequest<ClienteInfo>() { Objeto = pCliente });
        }
    }
}
