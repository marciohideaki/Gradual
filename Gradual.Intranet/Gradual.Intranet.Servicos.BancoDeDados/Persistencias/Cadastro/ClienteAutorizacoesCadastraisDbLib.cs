using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Dados.ControleDeOrdens;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Negocio;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using Gradual.Generico.Dados;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        public static ReceberObjetoResponse<ClienteAutorizacaoInfo> ReceberAutorizacoesCadastrais(ReceberEntidadeRequest<ClienteAutorizacaoInfo> pParametros)
        {
            try
            {
                var resposta = new ReceberObjetoResponse<ClienteAutorizacaoInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_autorizacoes_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            resposta.Objeto = (CriarAutorizacaoInfo(lDataTable.Rows[i]));
                        }
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

        public static ClienteAutorizacaoInfo ReceberAutorizacoesCadastrais(int IdCliente)
        {
            var resposta = new ClienteAutorizacaoInfo();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_autorizacoes_sel_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "id_cliente", DbType.Int32, IdCliente);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        resposta = CriarAutorizacaoInfo(lDataTable.Rows[i]);
                    }
                }
            }

            return resposta;
        }

        public static ConsultarObjetosResponse<ClienteAutorizacaoInfo> ConsultarAutorizacoesCadastrais(ConsultarEntidadeRequest<ClienteAutorizacaoInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteAutorizacaoInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_autorizacoes_lst_sp"))
                {
                    if (pParametros.Objeto != null)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "st_autorizado", DbType.Int32, pParametros.Objeto.StAutorizado);
                    }

                    if (pParametros.Objeto != null)
                    {
                        if (pParametros.Objeto.DataAutorizacao_D1.HasValue)
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "dt_de", DbType.DateTime, pParametros.Objeto.DataAutorizacao_D1.Value);
                        }

                        if (pParametros.Objeto.DataAutorizacao_D2.HasValue)
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "dt_ate", DbType.DateTime, pParametros.Objeto.DataAutorizacao_D2.Value);
                        }
                    }

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            resposta.Resultado.Add(CriarAutorizacaoInfo(lDataTable.Rows[i]));
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

        public static SalvarEntidadeResponse<ClienteAutorizacaoInfo> SalvarAutorizacaoCadastral(SalvarObjetoRequest<ClienteAutorizacaoInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteAutorizacaoInfo> lRetorno = new SalvarEntidadeResponse<ClienteAutorizacaoInfo>();

            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_autorizacoes_ins_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@tipo", DbType.Int16, pParametros.Objeto.IdLogin_D1);  //sempre esse campo para o tipo
                lAcessaDados.AddInParameter(lDbCommand, "@numero", DbType.Int16, pParametros.Objeto.IdLogin_D2);  //sempre esse campo para o numero
                lAcessaDados.AddInParameter(lDbCommand, "@id_login_autorizador", DbType.Int32, pParametros.Objeto.IdLogin_P1);  //sempre esse campo para o login auth
                lAcessaDados.AddOutParameter(lDbCommand, "@status_final", DbType.String, 1);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                lRetorno.Objeto = pParametros.Objeto;

                object lStat = lDbCommand.Parameters[4].Value;

                if(lStat != DBNull.Value)
                {
                    lRetorno.Objeto.StAutorizado = Convert.ToString(lDbCommand.Parameters[4].Value);
                }
            }

            return lRetorno;
        }

        public static ClienteAutorizacaoInfo SalvarAutorizacaoCadastral(ClienteAutorizacaoInfo pParametros)
        {
            var lRetorno = new ClienteAutorizacaoInfo();

            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_autorizacoes_ins_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente",              DbType.Int32, pParametros.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@tipo",                    DbType.Int16, pParametros.IdLogin_D1);  //sempre esse campo para o tipo
                lAcessaDados.AddInParameter(lDbCommand, "@numero",                  DbType.Int16, pParametros.IdLogin_D2);  //sempre esse campo para o numero
                lAcessaDados.AddInParameter(lDbCommand, "@id_login_autorizador",    DbType.Int32, pParametros.IdLogin_P1);  //sempre esse campo para o login auth
                lAcessaDados.AddOutParameter(lDbCommand, "@status_final",           DbType.String, 1);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                lRetorno = pParametros;

                object lStat = lDbCommand.Parameters[4].Value;

                if (lStat != DBNull.Value)
                {
                    lRetorno.StAutorizado = Convert.ToString(lDbCommand.Parameters[4].Value);
                }
            }

            return lRetorno;
        }

        public static ClienteAutorizacaoInfo SalvarAutorizacaoCadastralImportacao(DbTransaction lTrans,ClienteAutorizacaoInfo pParametros)
        {
            var lRetorno = new ClienteAutorizacaoInfo();

            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (var lDbCommand = lAcessaDados.CreateCommand(lTrans, CommandType.StoredProcedure, "cliente_autorizacoes_ins_importacao_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand,     "@id_cliente",        DbType.Int32,      pParametros.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand,     "@id_login_d1",       DbType.Int32,      pParametros.IdLogin_D1);
                lAcessaDados.AddInParameter(lDbCommand,     "@dt_autorizacao_d1", DbType.DateTime,   pParametros.DataAutorizacao_D1);
                lAcessaDados.AddInParameter(lDbCommand,     "@id_login_d2",       DbType.Int32,      pParametros.IdLogin_D2);
                lAcessaDados.AddInParameter(lDbCommand,     "@dt_autorizacao_d2", DbType.DateTime,   pParametros.DataAutorizacao_D2);
                lAcessaDados.AddInParameter(lDbCommand,     "@id_login_p1",       DbType.Int32,      pParametros.IdLogin_P1);
                lAcessaDados.AddInParameter(lDbCommand,     "@dt_autorizacao_p1", DbType.DateTime,   pParametros.DataAutorizacao_P1);
                lAcessaDados.AddInParameter(lDbCommand,     "@id_login_p2",       DbType.Int32,      pParametros.IdLogin_P2);
                lAcessaDados.AddInParameter(lDbCommand,     "@dt_autorizacao_p2", DbType.DateTime,   pParametros.DataAutorizacao_P2);
                lAcessaDados.AddInParameter(lDbCommand,     "@id_login_t1",       DbType.Int32,      pParametros.IdLogin_T1);
                lAcessaDados.AddInParameter(lDbCommand,     "@dt_autorizacao_t1", DbType.DateTime,   pParametros.DataAutorizacao_T1);
                lAcessaDados.AddInParameter(lDbCommand,     "@id_login_t2",       DbType.Int32,      pParametros.IdLogin_T2);
                lAcessaDados.AddInParameter(lDbCommand,     "@dt_autorizacao_t2", DbType.DateTime,   pParametros.DataAutorizacao_T2);
                lAcessaDados.AddInParameter(lDbCommand,     "@st_autorizado",     DbType.String,     pParametros.StAutorizado);
                
                lAcessaDados.ExecuteNonQuery(lDbCommand, lTrans);

                lRetorno = pParametros;
            }

            return lRetorno;
        }

        public static ClienteAutorizacaoInfo CriarAutorizacaoInfo(DataRow pRow)
        {
            ClienteAutorizacaoInfo lRetorno = new ClienteAutorizacaoInfo();

            lRetorno.IdAutorizacao  = pRow["id_autorizacao"].DBToInt32();
            lRetorno.NomeCliente    = pRow["ds_nome"].DBToString();
            lRetorno.IdCliente      = pRow["id_cliente"].DBToInt32();
            lRetorno.CodigoBov      = pRow["cd_codigo"].DBToString();
            lRetorno.CPF            = pRow["ds_cpfcnpj"].DBToString();
            lRetorno.Passo          = pRow["st_passo"].DBToString();
            lRetorno.DataExportacao = pRow["dt_ultimaexportacao"].DBToDateTime();
            lRetorno.StAutorizado   = pRow["st_autorizado"].DBToString();

            if (pRow["id_login_d1"] != DBNull.Value)
            {
                lRetorno.IdLogin_D1 = pRow["id_login_d1"].DBToInt32();
                lRetorno.Nome_D1 = pRow["nome_d1"].DBToString();
                lRetorno.Email_D1 = pRow["email_d1"].DBToString();
                lRetorno.DataAutorizacao_D1 = pRow["dt_autorizacao_d1"].DBToDateTime();
            }

            if (pRow["id_login_d2"] != DBNull.Value)
            {
                lRetorno.IdLogin_D2 = pRow["id_login_d2"].DBToInt32();
                lRetorno.Nome_D2 = pRow["nome_d2"].DBToString();
                lRetorno.Email_D2 = pRow["email_d2"].DBToString();
                lRetorno.DataAutorizacao_D2 = pRow["dt_autorizacao_d2"].DBToDateTime();
            }

            if (pRow["id_login_p1"] != DBNull.Value)
            {
                lRetorno.IdLogin_P1 = pRow["id_login_p1"].DBToInt32();
                lRetorno.Nome_P1 = pRow["nome_p1"].DBToString();
                lRetorno.Email_P1 = pRow["email_p1"].DBToString();
                lRetorno.DataAutorizacao_P1 = pRow["dt_autorizacao_p1"].DBToDateTime();
            }

            if (pRow["id_login_p2"] != DBNull.Value)
            {
                lRetorno.IdLogin_P2 = pRow["id_login_p2"].DBToInt32();
                lRetorno.Nome_P2 = pRow["nome_p2"].DBToString();
                lRetorno.Email_P2 = pRow["email_p2"].DBToString();
                lRetorno.DataAutorizacao_P2 = pRow["dt_autorizacao_p2"].DBToDateTime();
            }

            if (pRow["id_login_t1"] != DBNull.Value)
            {
                lRetorno.IdLogin_T1 = pRow["id_login_t1"].DBToInt32();
                lRetorno.Nome_T1 = pRow["nome_t1"].DBToString();
                lRetorno.Email_T1 = pRow["email_t1"].DBToString();
                lRetorno.DataAutorizacao_T1 = pRow["dt_autorizacao_t1"].DBToDateTime();
            }

            if (pRow["id_login_t2"] != DBNull.Value)
            {
                lRetorno.IdLogin_T2 = pRow["id_login_t2"].DBToInt32();
                lRetorno.Nome_T2 = pRow["nome_t2"].DBToString();
                lRetorno.Email_T2 = pRow["email_t2"].DBToString();
                lRetorno.DataAutorizacao_T2 = pRow["dt_autorizacao_t2"].DBToDateTime();
            }

            return lRetorno;
        }

    }
}
