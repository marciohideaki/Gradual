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
        public static ConsultarObjetosResponse<PaisesBlackListInfo> ConsultarPaisesBlackList(ConsultarEntidadeRequest<PaisesBlackListInfo> pParametros)
        {
            try
            {
                var lResposta = new ConsultarObjetosResponse<PaisesBlackListInfo>();

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "paises_blacklist_lst_sp"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable) for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            lResposta.Resultado.Add(CriarRegistroPaisesBlackList(lDataTable.Rows[i]));
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<PaisesBlackListInfo> ReceberPaisesBlackList(ReceberEntidadeRequest<PaisesBlackListInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<PaisesBlackListInfo> resposta =
                    new ReceberObjetoResponse<PaisesBlackListInfo>();

                resposta.Objeto = new PaisesBlackListInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "paises_blacklist_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pais_blacklist", DbType.Int32, pParametros.Objeto.IdPaisBlackList);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdPaisBlackList = (lDataTable.Rows[0]["id_pais_blacklist"]).DBToInt32();
                        resposta.Objeto.CdPais = (lDataTable.Rows[0]["cd_pais"]).DBToString();
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

        private static PaisesBlackListInfo CriarRegistroPaisesBlackList(DataRow linha)
        {
            return new PaisesBlackListInfo()
            {
                IdPaisBlackList = linha["id_pais_blacklist"].DBToInt32(),
                CdPais = linha["cd_pais"].DBToString(),
            };
        }

        public static RemoverObjetoResponse<PaisesBlackListInfo> RemoverPaisesBlackList(RemoverEntidadeRequest<PaisesBlackListInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "paises_blacklist_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pais_blacklist", DbType.Int32, pParametros.Objeto.IdPaisBlackList);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<PaisesBlackListInfo> response = new RemoverEntidadeResponse<PaisesBlackListInfo>()
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

        public static SalvarEntidadeResponse<PaisesBlackListInfo> SalvarPaisesBlackList(SalvarObjetoRequest<PaisesBlackListInfo> pParametros)
        {
            if (pParametros.Objeto.IdPaisBlackList > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<PaisesBlackListInfo> Salvar(SalvarObjetoRequest<PaisesBlackListInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "paises_blacklist_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_pais", DbType.String, pParametros.Objeto.CdPais);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_pais_blacklist", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<PaisesBlackListInfo> response = new SalvarEntidadeResponse<PaisesBlackListInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_pais_blacklist"].Value)
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

        private static SalvarEntidadeResponse<PaisesBlackListInfo> Atualizar(SalvarObjetoRequest<PaisesBlackListInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "paises_blacklist_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_pais", DbType.String, pParametros.Objeto.CdPais);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pais_blacklist", DbType.Int32, pParametros.Objeto.IdPaisBlackList);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<PaisesBlackListInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(PaisesBlackListInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<PaisesBlackListInfo> lEntrada = new ReceberEntidadeRequest<PaisesBlackListInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<PaisesBlackListInfo> lRetorno = ReceberPaisesBlackList(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

    }
}
