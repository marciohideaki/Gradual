using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {

        public static SalvarEntidadeResponse<ClienteEmitenteInfo> SalvarClienteEmitente(DbTransaction pTrans, SalvarObjetoRequest<ClienteEmitenteInfo> pParametros)
        {
            return Salvar(pTrans, pParametros);
        }

        public static SalvarEntidadeResponse<ClienteEmitenteInfo> SalvarClienteEmitente(SalvarObjetoRequest<ClienteEmitenteInfo> pParametros)
        {
            if (pParametros.Objeto.IdPessoaAutorizada > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<ClienteEmitenteInfo> Salvar(SalvarObjetoRequest<ClienteEmitenteInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteEmitenteInfo> lRetorno;

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

        private static SalvarEntidadeResponse<ClienteEmitenteInfo> Salvar(DbTransaction pTrans, SalvarObjetoRequest<ClienteEmitenteInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_emitente_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.AnsiString, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.NrCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Boolean, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimento", DbType.DateTime, pParametros.Objeto.DtNascimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numerodocumento", DbType.AnsiString, pParametros.Objeto.DsNumeroDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_sistema", DbType.AnsiString, pParametros.Objeto.CdSistema);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.AnsiString, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_data", DbType.DateTime, pParametros.Objeto.DsData);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_pessoaautorizada", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    SalvarEntidadeResponse<ClienteEmitenteInfo> response = new SalvarEntidadeResponse<ClienteEmitenteInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_pessoaautorizada"].Value)
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

        private static SalvarEntidadeResponse<ClienteEmitenteInfo> Atualizar(SalvarObjetoRequest<ClienteEmitenteInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_emitente_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pessoaautorizada", DbType.Int32, pParametros.Objeto.IdPessoaAutorizada);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.AnsiString, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.NrCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numerodocumento", DbType.AnsiString, pParametros.Objeto.DsNumeroDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_sistema", DbType.AnsiString, pParametros.Objeto.CdSistema);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Boolean, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.AnsiString, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_data", DbType.DateTime, pParametros.Objeto.DsData);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimento", DbType.DateTime, pParametros.Objeto.DtNascimento);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteEmitenteInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        public static RemoverObjetoResponse<ClienteEmitenteInfo> RemoverClienteEmitente(RemoverEntidadeRequest<ClienteEmitenteInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_emitente_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pessoaautorizada", DbType.Int32, pParametros.Objeto.IdPessoaAutorizada);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                }

                RemoverEntidadeResponse<ClienteEmitenteInfo> response = new RemoverEntidadeResponse<ClienteEmitenteInfo>()
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

        private static ClienteEmitenteInfo CriarRegistroClienteEmitente(DataRow linha)
        {
            ClienteEmitenteInfo lClienteEmitente = new ClienteEmitenteInfo();

            lClienteEmitente.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteEmitente.DsNome = linha["ds_nome"].DBToString();
            lClienteEmitente.NrCpfCnpj = linha["ds_cpfcnpj"].DBToString();
            lClienteEmitente.StPrincipal = Boolean.Parse(linha["st_principal"].ToString());
            lClienteEmitente.DsNumeroDocumento = linha["ds_numerodocumento"].DBToString();
            lClienteEmitente.CdSistema = linha["cd_sistema"].DBToString();
            lClienteEmitente.DsEmail = linha["ds_email"].DBToString();
            lClienteEmitente.DsData = linha["ds_data"].DBToDateTime();
            lClienteEmitente.IdPessoaAutorizada = linha["id_pessoaautorizada"].DBToInt32();
            lClienteEmitente.DtNascimento = linha["dt_nascimento"].DBToDateTime();


            return lClienteEmitente;

        }

        public static ReceberObjetoResponse<ClienteEmitenteInfo> ReceberClienteEmitente(ReceberEntidadeRequest<ClienteEmitenteInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteEmitenteInfo> resposta =
                    new ReceberObjetoResponse<ClienteEmitenteInfo>();

                resposta.Objeto = new ClienteEmitenteInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_emitente_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pessoaautorizada", DbType.Int32, pParametros.Objeto.IdPessoaAutorizada);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {

                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.DsNome = (lDataTable.Rows[0]["ds_nome"]).DBToString();
                        resposta.Objeto.NrCpfCnpj = (lDataTable.Rows[0]["ds_cpfcnpj"]).DBToString();
                        resposta.Objeto.DtNascimento = (lDataTable.Rows[0]["dt_nascimento"]).DBToDateTime();
                        resposta.Objeto.StPrincipal = Boolean.Parse(lDataTable.Rows[0]["st_principal"].ToString());
                        resposta.Objeto.DsNumeroDocumento = (lDataTable.Rows[0]["ds_numerodocumento"]).DBToString(); ;
                        resposta.Objeto.CdSistema = (lDataTable.Rows[0]["cd_sistema"]).DBToString();
                        resposta.Objeto.DsEmail = (lDataTable.Rows[0]["ds_email"]).DBToString();
                        resposta.Objeto.DsData = (lDataTable.Rows[0]["ds_data"]).DBToDateTime();

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

        public static List<ClienteEmitenteInfo> GetClienteEmitentePorIdCliente(ClienteInfo pParametros)
        {

            List<ClienteEmitenteInfo> resposta = new List<ClienteEmitenteInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_emitente_lst_porcliente_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.IdCliente);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                foreach (DataRow item in lDataTable.Rows)
                {
                    ClienteEmitenteInfo lEmitente = new ClienteEmitenteInfo();
                    lEmitente.IdCliente = (item["id_cliente"]).DBToInt32();
                    lEmitente.DsNome = (item["ds_nome"]).DBToString();
                    lEmitente.NrCpfCnpj = (item["ds_cpfcnpj"]).DBToString();
                    lEmitente.DtNascimento = (item["dt_nascimento"]).DBToDateTime();
                    lEmitente.StPrincipal = Boolean.Parse(item["st_principal"].ToString());
                    lEmitente.DsNumeroDocumento = (item["ds_numerodocumento"]).DBToString(); ;
                    lEmitente.CdSistema = (item["cd_sistema"]).DBToString();
                    lEmitente.DsEmail = (item["ds_email"]).DBToString();
                    lEmitente.DsData = (item["ds_data"]).DBToDateTime();
                    resposta.Add(lEmitente);
                }
            }

            return resposta;
        }

        public static ConsultarObjetosResponse<ClienteEmitenteInfo> ConsultarClienteEmitente(ConsultarEntidadeRequest<ClienteEmitenteInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteEmitenteInfo> resposta =
                    new ConsultarObjetosResponse<ClienteEmitenteInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_emitente_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);

                        DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {

                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                DataRow linha = lDataTable.NewRow();

                                linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                                linha["ds_nome"] = (lDataTable.Rows[i]["ds_nome"]).DBToString();
                                linha["ds_cpfcnpj"] = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();
                                linha["st_principal"] = Boolean.Parse(lDataTable.Rows[i]["st_principal"].ToString());
                                linha["ds_numerodocumento"] = (lDataTable.Rows[i]["ds_numerodocumento"]).DBToString();
                                linha["cd_sistema"] = (lDataTable.Rows[i]["cd_sistema"]).DBToString();
                                linha["ds_email"] = (lDataTable.Rows[i]["ds_email"]).DBToString();
                                linha["ds_data"] = (lDataTable.Rows[i]["ds_data"]).DBToDateTime();
                                linha["id_pessoaautorizada"] = (lDataTable.Rows[i]["id_pessoaautorizada"]).DBToInt32();
                                linha["dt_nascimento"] = (lDataTable.Rows[i]["dt_nascimento"]).DBToDateTime();

                                resposta.Resultado.Add(CriarRegistroClienteEmitente(linha));
                            }

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


        private static void LogarModificacao(ClienteEmitenteInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteEmitenteInfo> lEntrada = new ReceberEntidadeRequest<ClienteEmitenteInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteEmitenteInfo> lRetorno = ReceberClienteEmitente(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

    }
}

