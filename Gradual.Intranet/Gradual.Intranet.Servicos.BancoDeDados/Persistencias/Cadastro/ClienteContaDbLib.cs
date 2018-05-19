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
        public static ConsultarObjetosResponse<ClienteContaInfo> ConsultarClienteConta(ConsultarEntidadeRequest<ClienteContaInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteContaInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_conta_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarClienteConta(lDataTable.Rows[i]));
                }
                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteContaInfo CriarClienteConta(DataRow linha)
        {
            ClienteContaInfo lClienteContaInfo = new ClienteContaInfo();

            lClienteContaInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteContaInfo.IdClienteConta = linha["id_cliente_conta"].DBToInt32();
            lClienteContaInfo.StAtiva = bool.Parse(linha["st_ativa"].ToString());
            lClienteContaInfo.StContaInvestimento = bool.Parse(linha["st_containvestimento"].ToString());
            lClienteContaInfo.StPrincipal = bool.Parse(linha["st_principal"].ToString());
            lClienteContaInfo.CdCodigo = linha["cd_codigo"].DBToInt32();
            lClienteContaInfo.CdAssessor = linha["cd_assessor"].DBToInt32();

            string lSis = (linha["cd_sistema"]).DBToString();
            
            if (lSis == "BOL")
                lClienteContaInfo.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BOL;
            else if (lSis == "BMF")
                lClienteContaInfo.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BMF;
            else if (lSis == "CUS")
                lClienteContaInfo.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CUS;
            else
                lClienteContaInfo.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CC;

            return lClienteContaInfo;

        }

        public static ReceberObjetoResponse<ClienteContaInfo> ReceberClienteConta(ReceberEntidadeRequest<ClienteContaInfo> pParametros)
        {
            try
            {
                var resposta = new ReceberObjetoResponse<ClienteContaInfo>();

                resposta.Objeto = new ClienteContaInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_conta_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_conta", DbType.Int32, pParametros.Objeto.IdClienteConta);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Int32, pParametros.Objeto.StPrincipal);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.CdAssessor = (lDataTable.Rows[0]["cd_assessor"]).DBToInt32();
                        resposta.Objeto.CdCodigo = (lDataTable.Rows[0]["cd_codigo"]).DBToInt32();
                        resposta.Objeto.IdCliente = lDataTable.Rows[0]["id_cliente"].DBToInt32();
                        resposta.Objeto.IdClienteConta = lDataTable.Rows[0]["id_cliente_conta"].DBToInt32();
                        resposta.Objeto.StAtiva = lDataTable.Rows[0]["st_principal"].DBToBoolean();
                        resposta.Objeto.StContaInvestimento = lDataTable.Rows[0]["st_containvestimento"].DBToBoolean();
                        resposta.Objeto.StPrincipal = lDataTable.Rows[0]["st_principal"].DBToBoolean();
                      
                        string lSis = (lDataTable.Rows[0]["cd_sistema"]).DBToString();
                   
                        if (lSis == "BOL")
                            resposta.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BOL;
                        else if (lSis == "BMF")
                            resposta.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BMF;
                        else if (lSis == "CUS")
                            resposta.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CUS;
                        else
                            resposta.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CC;
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

        public static RemoverObjetoResponse<ClienteContaInfo> RemoverClienteConta(RemoverEntidadeRequest<ClienteContaInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_conta_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_conta", DbType.Int32, pParametros.Objeto.IdClienteConta);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClienteContaInfo> response = new RemoverEntidadeResponse<ClienteContaInfo>()
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

        public static SalvarEntidadeResponse<ClienteContaInfo> SalvarClienteConta(DbTransaction pTrans, SalvarObjetoRequest<ClienteContaInfo> pParametros)
        {
            return Salvar(pTrans, pParametros);
        }

        public static SalvarEntidadeResponse<ClienteContaInfo> SalvarClienteConta(SalvarObjetoRequest<ClienteContaInfo> pParametros)
        {
            if (pParametros.Objeto.IdClienteConta > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<ClienteContaInfo> Salvar(SalvarObjetoRequest<ClienteContaInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteContaInfo> lRetorno;

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

        private static SalvarEntidadeResponse<ClienteContaInfo> Salvar(DbTransaction pTrans, SalvarObjetoRequest<ClienteContaInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_conta_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, pParametros.Objeto.CdAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_sistema", DbType.AnsiString, pParametros.Objeto.CdSistema.ToString());
                    lAcessaDados.AddInParameter(lDbCommand, "@st_containvestimento", DbType.Boolean, pParametros.Objeto.StContaInvestimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Boolean, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ativa", DbType.Boolean, pParametros.Objeto.StAtiva);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_cliente_conta", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    SalvarEntidadeResponse<ClienteContaInfo> response = new SalvarEntidadeResponse<ClienteContaInfo>()
                    {
                        Codigo = pParametros.Objeto.IdClienteConta
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

        private static SalvarEntidadeResponse<ClienteContaInfo> Atualizar(DbTransaction pTrans, SalvarObjetoRequest<ClienteContaInfo> pParametros) { 
        
         try
            {
                //LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_conta_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, pParametros.Objeto.CdAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_sistema", DbType.AnsiString, pParametros.Objeto.CdSistema.ToString());
                    lAcessaDados.AddInParameter(lDbCommand, "@st_containvestimento", DbType.Boolean, pParametros.Objeto.StContaInvestimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Boolean, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ativa", DbType.Boolean, pParametros.Objeto.StAtiva);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_conta", DbType.Int32, pParametros.Objeto.IdClienteConta);

                    lAcessaDados.ExecuteNonQuery(lDbCommand,pTrans);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteContaInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        
        }

        private static SalvarEntidadeResponse<ClienteContaInfo> AtualizarAtivaInativa(DbTransaction pTrans, SalvarObjetoRequest<ClienteContaInfo> pParametros)
        {

            try
            {
                //LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_conta_ativarinativar_upd_sp"))
                {
                  
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_sistema", DbType.AnsiString, pParametros.Objeto.CdSistema.ToString());
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ativa", DbType.Boolean, pParametros.Objeto.StAtiva);
                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteContaInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }

        }

        private static SalvarEntidadeResponse<ClienteContaInfo> Atualizar(SalvarObjetoRequest<ClienteContaInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteContaInfo> lRetorno;

            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            try
            {
                lRetorno = Atualizar(trans, pParametros);
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

        private static ClienteContaInfo GetClienteContaPrincipal(ClienteInfo pCliente)
        {
            ClienteContaInfo resposta = new ClienteContaInfo();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_conta_pri_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pCliente.IdCliente);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    resposta.CdAssessor = (lDataTable.Rows[0]["cd_assessor"]).DBToInt32();
                    resposta.CdCodigo = (lDataTable.Rows[0]["cd_codigo"]).DBToInt32();
                    string lSis = (lDataTable.Rows[0]["cd_sistema"]).DBToString();
                    if (lSis == "BOL")
                        resposta.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BOL;
                    else if (lSis == "BMF")
                        resposta.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BMF;
                    else if (lSis == "CUS")
                        resposta.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CUS;
                    else
                        resposta.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CC;
                    resposta.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                    resposta.IdClienteConta = (lDataTable.Rows[0]["id_cliente_conta"]).DBToInt32();
                    resposta.StAtiva = bool.Parse((lDataTable.Rows[0]["st_principal"].ToString()));
                    resposta.StContaInvestimento = bool.Parse(lDataTable.Rows[0]["st_containvestimento"].ToString());
                    resposta.StPrincipal = bool.Parse((lDataTable.Rows[0]["st_principal"].ToString()));
                }
            }

            return resposta;
        }

        private static void LogarModificacao(ClienteContaInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteContaInfo> lEntrada = new ReceberEntidadeRequest<ClienteContaInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteContaInfo> lRetorno = ReceberClienteConta(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }
    }
}
