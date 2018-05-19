using System;
using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        #region | Métodos CRUD

        public static ConsultarObjetosResponse<ConfiguracaoInfo> ConsultarConfiguracao(ConsultarEntidadeRequest<ConfiguracaoInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ConfiguracaoInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "configuracao_lst_sp"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroConfiguracao(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ConfiguracaoInfo> ReceberConfiguracaoPorDescricao(ReceberEntidadeRequest<ConfiguracaoInfo> pParametros)
        {
            try
            {
                var lResposta = new ReceberObjetoResponse<ConfiguracaoInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "configuracao_bus_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_configuracao", DbType.String, pParametros.Objeto.Configuracao.ToString().ToLower());

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        lResposta.Objeto = CriarRegistroConfiguracao(lDataTable.Rows[0]);
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ConfiguracaoInfo> ReceberConfiguracaoPorId(ReceberEntidadeRequest<ConfiguracaoInfo> pParametros)
        {
            try
            {
                var lResposta = new ReceberObjetoResponse<ConfiguracaoInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "configuracao_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_configuracao", DbType.Int32, pParametros.Objeto.IdConfiguracao);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        lResposta.Objeto = CriarRegistroConfiguracao(lDataTable.Rows[0]);
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ConfiguracaoInfo> SalvarConfiguracao(SalvarObjetoRequest<ConfiguracaoInfo> pParametros)
        {
            if (pParametros.Objeto.IdConfiguracao > 0)
                return Atualizar(pParametros);
            else
                return Salvar(pParametros);
        }

        private static SalvarEntidadeResponse<ConfiguracaoInfo> Atualizar(SalvarObjetoRequest<ConfiguracaoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "configuracao_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_configuracao", DbType.Int32, pParametros.Objeto.IdConfiguracao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_configuracao", DbType.String, pParametros.Objeto.Configuracao.ToString().ToLower());
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_valor", DbType.String, pParametros.Objeto.Valor);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                return new SalvarEntidadeResponse<ConfiguracaoInfo>();
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<ConfiguracaoInfo> Salvar(SalvarObjetoRequest<ConfiguracaoInfo> pParametros)
        {
            try
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "configuracao_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_configuracao", DbType.String, pParametros.Objeto.Configuracao.ToString().ToLower());
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_valor", DbType.String, pParametros.Objeto.Valor);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_configuracao", DbType.Int32, 8);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    var response = new SalvarEntidadeResponse<ConfiguracaoInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_configuracao"].Value)
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

        public static RemoverObjetoResponse<ConfiguracaoInfo> RemoverConfiguracao(RemoverEntidadeRequest<ConfiguracaoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "configuracao_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_configuracao", DbType.Int32, pParametros.Objeto.IdConfiguracao);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                var response = new RemoverEntidadeResponse<ConfiguracaoInfo>()
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

        #endregion

        #region | Métodos apoio

        private static ConfiguracaoInfo CriarRegistroConfiguracao(DataRow linha)
        {
            return new ConfiguracaoInfo()
            {
                Configuracao = ConfiguracaoInfo.TraduzirEnum(linha["ds_configuracao"]),
                IdConfiguracao = linha["id_configuracao"].DBToInt32(),
                Valor = linha["ds_valor"].DBToString()
            };
        }

        #endregion

        private static void LogarModificacao(ConfiguracaoInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ConfiguracaoInfo> lEntrada = new ReceberEntidadeRequest<ConfiguracaoInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ConfiguracaoInfo> lRetorno = ReceberConfiguracaoPorId(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

    }
}
