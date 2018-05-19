using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {

        public static ConsultarObjetosResponse<AtividadeIlicitaInfo> ConsultarAtividadesIlicitas(ConsultarEntidadeRequest<AtividadeIlicitaInfo> pParametros)
        {
            try
            {
                var lResposta = new ConsultarObjetosResponse<AtividadeIlicitaInfo>();
                var lDataTable = new DataTable();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "atividades_ilicitas_lst_sp"))
                {
                    lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable) for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            lResposta.Resultado.Add(CriarRegistroAtividadeIlicita(lDataTable.Rows[i]));
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<AtividadeIlicitaInfo> ReceberAtividadesIlicitas(ReceberEntidadeRequest<AtividadeIlicitaInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<AtividadeIlicitaInfo> resposta =
                    new ReceberObjetoResponse<AtividadeIlicitaInfo>();

                resposta.Objeto = new AtividadeIlicitaInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "atividades_ilicitas_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_atividadeilicita", DbType.Int32, pParametros.Objeto.IdAtividadeIlicita);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.CdAtividade = (lDataTable.Rows[0]["cd_atividade"]).DBToString();
                        resposta.Objeto.IdAtividadeIlicita = (lDataTable.Rows[0]["id_atividadeilicita"]).DBToInt32();
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

        private static AtividadeIlicitaInfo CriarRegistroAtividadeIlicita(DataRow linha)
        {
            return new AtividadeIlicitaInfo()
            {
                CdAtividade = linha["cd_atividade"].DBToString(),
                IdAtividadeIlicita = linha["id_atividadeilicita"].DBToInt32(),
            };
        }

        public static RemoverObjetoResponse<AtividadeIlicitaInfo> RemoverAtividadesIlicitas(RemoverEntidadeRequest<AtividadeIlicitaInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "atividades_ilicitas_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_atividadeilicita", DbType.Int32, pParametros.Objeto.IdAtividadeIlicita);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<AtividadeIlicitaInfo> response = new RemoverEntidadeResponse<AtividadeIlicitaInfo>()
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

        public static SalvarEntidadeResponse<AtividadeIlicitaInfo> SalvarAtividadesIlicitas(SalvarObjetoRequest<AtividadeIlicitaInfo> pParametros)
        {
            if (pParametros.Objeto.IdAtividadeIlicita > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<AtividadeIlicitaInfo> Salvar(SalvarObjetoRequest<AtividadeIlicitaInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "atividades_ilicitas_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_atividade", DbType.String, pParametros.Objeto.CdAtividade);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_atividadeilicita", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<AtividadeIlicitaInfo> response = new SalvarEntidadeResponse<AtividadeIlicitaInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_atividadeilicita"].Value)
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

        private static SalvarEntidadeResponse<AtividadeIlicitaInfo> Atualizar(SalvarObjetoRequest<AtividadeIlicitaInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "atividades_ilicitas_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_atividade", DbType.String, pParametros.Objeto.CdAtividade);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_atividadeilicita", DbType.Int32, pParametros.Objeto.IdAtividadeIlicita);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<AtividadeIlicitaInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(AtividadeIlicitaInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<AtividadeIlicitaInfo> lEntrada = new ReceberEntidadeRequest<AtividadeIlicitaInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<AtividadeIlicitaInfo> lRetorno = ReceberAtividadesIlicitas(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
