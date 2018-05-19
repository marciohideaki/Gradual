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

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<TipoTelefoneInfo> ConsultarTipoTelefone(ConsultarEntidadeRequest<TipoTelefoneInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<TipoTelefoneInfo> resposta =
                    new ConsultarObjetosResponse<TipoTelefoneInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_telefone_lst_sp"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["id_tipo_telefone"] = Convert.ToInt32(lDataTable.Rows[i]["id_tipo_telefone"]);
                            linha["ds_telefone"] = Convert.ToString(lDataTable.Rows[i]["ds_telefone"]);
                            resposta.Resultado.Add(CriarRegistroTipoTelefone(linha));
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

        public static ReceberObjetoResponse<TipoTelefoneInfo> ReceberTipoTelefone(ReceberEntidadeRequest<TipoTelefoneInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<TipoTelefoneInfo> resposta =
                    new ReceberObjetoResponse<TipoTelefoneInfo>();

                resposta.Objeto = new TipoTelefoneInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_telefone_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_telefone", DbType.Int32, pParametros.Objeto.IdTipoTelefone);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdTipoTelefone = Convert.ToInt32(lDataTable.Rows[0]["id_tipo_telefone"]);
                        resposta.Objeto.DsTelefone = Convert.ToString(lDataTable.Rows[0]["ds_telefone"]);
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

        private static TipoTelefoneInfo CriarRegistroTipoTelefone(DataRow linha)
        {
            TipoTelefoneInfo lTipoDependencia = new TipoTelefoneInfo();

            lTipoDependencia.IdTipoTelefone = linha["id_tipo_telefone"].DBToInt32();
            lTipoDependencia.DsTelefone = linha["ds_telefone"].DBToString();

            return lTipoDependencia;

        }

        public static RemoverObjetoResponse<TipoTelefoneInfo> RemoverTipoTelefone(RemoverEntidadeRequest<TipoTelefoneInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_telefone_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_telefone", DbType.Int32, pParametros.Objeto.IdTipoTelefone);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                }

                RemoverEntidadeResponse<TipoTelefoneInfo> response = new RemoverEntidadeResponse<TipoTelefoneInfo>()
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


        public static SalvarEntidadeResponse<TipoTelefoneInfo> SalvarTipoTelefone(SalvarObjetoRequest<TipoTelefoneInfo> pParametros)
        {
            if (pParametros.Objeto.IdTipoTelefone > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<TipoTelefoneInfo> Salvar(SalvarObjetoRequest<TipoTelefoneInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_telefone_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_telefone", DbType.String, pParametros.Objeto.DsTelefone);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_tipo_telefone", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<TipoTelefoneInfo> response = new SalvarEntidadeResponse<TipoTelefoneInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_tipo_telefone"].Value)
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

        private static SalvarEntidadeResponse<TipoTelefoneInfo> Atualizar(SalvarObjetoRequest<TipoTelefoneInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_telefone_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_telefone", DbType.String, pParametros.Objeto.DsTelefone);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_telefone", DbType.Int32, pParametros.Objeto.IdTipoTelefone);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<TipoTelefoneInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(TipoTelefoneInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<TipoTelefoneInfo> lEntrada = new ReceberEntidadeRequest<TipoTelefoneInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<TipoTelefoneInfo> lRetorno = ReceberTipoTelefone(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
