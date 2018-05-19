using System;
using System.Collections.Generic;
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

        public static SalvarEntidadeResponse<ClienteDiretorInfo> SalvarClienteDiretor(DbTransaction pTrans, SalvarObjetoRequest<ClienteDiretorInfo> pParametros)
        {
            return Salvar(pTrans, pParametros);
        }

        public static SalvarEntidadeResponse<ClienteDiretorInfo> SalvarClienteDiretor(SalvarObjetoRequest<ClienteDiretorInfo> pParametros)
        {
            if (pParametros.Objeto.IdClienteDiretor > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<ClienteDiretorInfo> Atualizar(SalvarObjetoRequest<ClienteDiretorInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_diretor_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_identidade", DbType.String, pParametros.Objeto.DsIdentidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.NrCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_diretor", DbType.Int32, pParametros.Objeto.IdClienteDiretor);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_uforgaoemissordocumento", DbType.String, pParametros.Objeto.CdUfOrgaoEmissor);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_orgaoemissor", DbType.String, pParametros.Objeto.CdOrgaoEmissor);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
             
                    return new SalvarEntidadeResponse<ClienteDiretorInfo>();
                }
            }
            catch (Exception ex)
            {
                              LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar,ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<ClienteDiretorInfo> Salvar(SalvarObjetoRequest<ClienteDiretorInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteDiretorInfo> lRetorno;

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

        private static SalvarEntidadeResponse<ClienteDiretorInfo> Salvar(DbTransaction pTrans, SalvarObjetoRequest<ClienteDiretorInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_diretor_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_identidade", DbType.String, pParametros.Objeto.DsIdentidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.NrCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_uforgaoemissordocumento", DbType.String, pParametros.Objeto.CdUfOrgaoEmissor);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_orgaoemissor", DbType.String, pParametros.Objeto.CdOrgaoEmissor);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_cliente_diretor", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    SalvarEntidadeResponse<ClienteDiretorInfo> response = new SalvarEntidadeResponse<ClienteDiretorInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_cliente_diretor"].Value)
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


        public static RemoverObjetoResponse<ClienteDiretorInfo> RemoverClienteDiretor(RemoverEntidadeRequest<ClienteDiretorInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_diretor_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_diretor", DbType.Int32, pParametros.Objeto.IdClienteDiretor);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClienteDiretorInfo> response = new RemoverEntidadeResponse<ClienteDiretorInfo>()
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

        public static ReceberObjetoResponse<ClienteDiretorInfo> ReceberClienteDiretor(ReceberEntidadeRequest<ClienteDiretorInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteDiretorInfo> resposta =
                    new ReceberObjetoResponse<ClienteDiretorInfo>();

                resposta.Objeto = new ClienteDiretorInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_diretor_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_diretor", DbType.Int32, pParametros.Objeto.IdClienteDiretor);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.NrCpfCnpj = (lDataTable.Rows[0]["ds_cpfcnpj"]).DBToString();
                        resposta.Objeto.DsIdentidade = (lDataTable.Rows[0]["ds_identidade"]).DBToString();
                        resposta.Objeto.DsNome = (lDataTable.Rows[0]["ds_nome"]).DBToString();
                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.IdClienteDiretor = (lDataTable.Rows[0]["id_cliente_diretor"]).DBToInt32();
                        resposta.Objeto.CdOrgaoEmissor = (lDataTable.Rows[0]["cd_orgaoemissor"]).DBToString();
                        resposta.Objeto.CdUfOrgaoEmissor = (lDataTable.Rows[0]["cd_uforgaoemissordocumento"]).DBToString();
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                               LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber,ex);
                throw ex;
            }
        }

        public static List<ClienteDiretorInfo> GetClienteDiretorPorIdCliente(ClienteInfo pParametros)
        {

            List<ClienteDiretorInfo> resposta = new List<ClienteDiretorInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_diretor_lst_porcliente_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.IdCliente);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                foreach (DataRow item in lDataTable.Rows)
                {
                    ClienteDiretorInfo lDiretor = new ClienteDiretorInfo();
                    lDiretor.NrCpfCnpj = (item["ds_cpfcnpj"]).DBToString();
                    lDiretor.DsIdentidade = (item["ds_identidade"]).DBToString();
                    lDiretor.DsNome = (item["ds_nome"]).DBToString();
                    lDiretor.IdCliente = (item["id_cliente"]).DBToInt32();
                    lDiretor.IdClienteDiretor = (item["id_cliente_diretor"]).DBToInt32();
                    lDiretor.CdOrgaoEmissor = (item["cd_orgaoemissor"]).DBToString();
                    lDiretor.CdUfOrgaoEmissor = (item["cd_uforgaoemissordocumento"]).DBToString();
                    resposta.Add(lDiretor);
                }
            }

            return resposta;
        }

        public static ConsultarObjetosResponse<ClienteDiretorInfo> ConsultarClienteDiretor(ConsultarEntidadeRequest<ClienteDiretorInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteDiretorInfo> resposta =
                    new ConsultarObjetosResponse<ClienteDiretorInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_diretor_lst_sp"))
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

                                linha["ds_cpfcnpj"] = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();
                                linha["ds_identidade"] = (lDataTable.Rows[i]["ds_identidade"]).DBToString();
                                linha["ds_nome"] = (lDataTable.Rows[i]["ds_nome"]).DBToString();
                                linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                                linha["id_cliente_diretor"] = (lDataTable.Rows[i]["id_cliente_diretor"]).DBToInt32();
                                linha["cd_orgaoemissor"] = (lDataTable.Rows[i]["cd_orgaoemissor"]).DBToString();
                                linha["cd_uforgaoemissordocumento"] = (lDataTable.Rows[i]["cd_uforgaoemissordocumento"]).DBToString();

                                resposta.Resultado.Add(CriarRegistroClienteDiretorInfo(linha));
                            }

                        }
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                               LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar,ex);
                throw ex;
            }
        }


        private static ClienteDiretorInfo CriarRegistroClienteDiretorInfo(DataRow linha)
        {
            ClienteDiretorInfo lClienteDiretorInfo = new ClienteDiretorInfo();

            lClienteDiretorInfo.NrCpfCnpj = linha["ds_cpfcnpj"].DBToString();
            lClienteDiretorInfo.DsIdentidade = linha["ds_identidade"].DBToString();
            lClienteDiretorInfo.DsNome = linha["ds_nome"].DBToString();
            lClienteDiretorInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteDiretorInfo.IdClienteDiretor = linha["id_cliente_diretor"].DBToInt32();
            lClienteDiretorInfo.CdUfOrgaoEmissor = linha["cd_uforgaoemissordocumento"].DBToString();
            lClienteDiretorInfo.CdOrgaoEmissor = linha["cd_orgaoemissor"].DBToString();
            return lClienteDiretorInfo;

        }


        private static void LogarModificacao(ClienteDiretorInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteDiretorInfo> lEntrada = new ReceberEntidadeRequest<ClienteDiretorInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteDiretorInfo> lRetorno = ReceberClienteDiretor(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
