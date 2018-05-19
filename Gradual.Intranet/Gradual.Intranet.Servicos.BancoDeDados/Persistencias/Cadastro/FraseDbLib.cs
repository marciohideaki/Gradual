using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<FrasesInfo> ConsultarFrase(ConsultarEntidadeRequest<FrasesInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<FrasesInfo> resposta =
                    new ConsultarObjetosResponse<FrasesInfo>();

                pParametros.Condicoes.Add(new CondicaoInfo());

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "frases_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                DataRow linha = lDataTable.NewRow();

                                linha["id_frase"] = (lDataTable.Rows[i]["id_frase"]).DBToInt32();
                                linha["ds_frase"] = (lDataTable.Rows[i]["ds_frase"]).DBToString();
                                resposta.Resultado.Add(CriarRegistroFrases(linha));
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

        public static ReceberObjetoResponse<FrasesInfo> ReceberFrase(ReceberEntidadeRequest<FrasesInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<FrasesInfo> resposta =
                    new ReceberObjetoResponse<FrasesInfo>();

                resposta.Objeto = new FrasesInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "frases_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_frase", DbType.Int32, pParametros.Objeto.IdFrase);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdFrase = (lDataTable.Rows[0]["id_frase"]).DBToInt32();
                        resposta.Objeto.DsFrase = (lDataTable.Rows[0]["ds_frase"]).DBToString();
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

        private static FrasesInfo CriarRegistroFrases(DataRow linha)
        {
            FrasesInfo lFrasesInfo = new FrasesInfo();

            lFrasesInfo.IdFrase = linha["id_frase"].DBToInt32();
            lFrasesInfo.DsFrase = linha["ds_frase"].DBToString();

            return lFrasesInfo;

        }

        public static RemoverObjetoResponse<FrasesInfo> RemoverFrase(RemoverEntidadeRequest<FrasesInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "frases_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_frase", DbType.Int32, pParametros.Objeto.IdFrase);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<FrasesInfo> response = new RemoverEntidadeResponse<FrasesInfo>()
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

        public static SalvarEntidadeResponse<FrasesInfo> SalvarFrase(SalvarObjetoRequest<FrasesInfo> pParametros)
        {
            if (pParametros.Objeto.IdFrase > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<FrasesInfo> Salvar(SalvarObjetoRequest<FrasesInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "frases_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_frase", DbType.String, pParametros.Objeto.DsFrase);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_frase", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<FrasesInfo> response = new SalvarEntidadeResponse<FrasesInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_frase"].Value)
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

        private static SalvarEntidadeResponse<FrasesInfo> Atualizar(SalvarObjetoRequest<FrasesInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "frases_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_frase", DbType.String, pParametros.Objeto.DsFrase);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_frase", DbType.Int32, pParametros.Objeto.IdFrase);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<FrasesInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(FrasesInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<FrasesInfo> lEntrada = new ReceberEntidadeRequest<FrasesInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<FrasesInfo> lRetorno = ReceberFrase(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

    }
}
