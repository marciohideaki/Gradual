using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using System.Data;
using Gradual.Generico.Dados;
using System.Data.Common;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<TipoDePendenciaCadastralInfo> ConsultarTipoPendencia(ConsultarEntidadeRequest<TipoDePendenciaCadastralInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<TipoDePendenciaCadastralInfo> resposta =
                    new ConsultarObjetosResponse<TipoDePendenciaCadastralInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_pendenciacadastral_lst_sp"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["id_tipo_pendencia"] = (lDataTable.Rows[i]["id_tipo_pendencia"]).DBToInt32();
                            linha["ds_pendencia"] = (lDataTable.Rows[i]["ds_pendencia"]).DBToString();
                            linha["st_automatica"] =  (lDataTable.Rows[i]["st_automatica"]).DBToBoolean();
                            resposta.Resultado.Add(CriarRegistroTipoPendencia(linha));
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


        public static ReceberObjetoResponse<TipoDePendenciaCadastralInfo> ReceberTipoPendencia(ReceberEntidadeRequest<TipoDePendenciaCadastralInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<TipoDePendenciaCadastralInfo> resposta =
                    new ReceberObjetoResponse<TipoDePendenciaCadastralInfo>();

                resposta.Objeto = new TipoDePendenciaCadastralInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_pendenciacadastral_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_pendencia", DbType.Int32, pParametros.Objeto.IdTipoPendencia);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdTipoPendencia = Convert.ToInt32(lDataTable.Rows[0]["id_tipo_pendencia"]);
                        resposta.Objeto.DsPendencia = Convert.ToString(lDataTable.Rows[0]["ds_pendencia"]);
                        resposta.Objeto.StAutomatica = Convert.ToBoolean(lDataTable.Rows[0]["St_Automatica"]);

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

        private static TipoDePendenciaCadastralInfo CriarRegistroTipoPendencia(DataRow linha)
        {
            TipoDePendenciaCadastralInfo lTipoDependencia = new TipoDePendenciaCadastralInfo();

            lTipoDependencia.IdTipoPendencia = linha["id_tipo_pendencia"].DBToInt32();
            lTipoDependencia.DsPendencia = linha["ds_pendencia"].DBToString();
            lTipoDependencia.StAutomatica = linha["st_automatica"].DBToBoolean();
            return lTipoDependencia;

        }

        public static RemoverObjetoResponse<TipoDePendenciaCadastralInfo> RemoverTipoPendencia(RemoverEntidadeRequest<TipoDePendenciaCadastralInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_pendenciacadastral_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_pendencia", DbType.Int32, pParametros.Objeto.IdTipoPendencia);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                }

                RemoverEntidadeResponse<TipoDePendenciaCadastralInfo> response = new RemoverEntidadeResponse<TipoDePendenciaCadastralInfo>()
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

        public static SalvarObjetoResponse<TipoDePendenciaCadastralInfo> SalvarTipoPendencia(SalvarObjetoRequest<TipoDePendenciaCadastralInfo> pParametros)
        {
            if (pParametros.Objeto.IdTipoPendencia > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarObjetoResponse<TipoDePendenciaCadastralInfo> Salvar(SalvarObjetoRequest<TipoDePendenciaCadastralInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_pendenciacadastral_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_pendencia", DbType.String, pParametros.Objeto.DsPendencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@St_Automatica", DbType.Boolean, pParametros.Objeto.StAutomatica);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_tipo_pendencia", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<TipoDePendenciaCadastralInfo> response = new SalvarEntidadeResponse<TipoDePendenciaCadastralInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_tipo_pendencia"].Value)
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

        private static SalvarObjetoResponse<TipoDePendenciaCadastralInfo> Atualizar(SalvarObjetoRequest<TipoDePendenciaCadastralInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_pendenciacadastral_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_pendencia", DbType.String, pParametros.Objeto.DsPendencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_pendencia", DbType.Int32, pParametros.Objeto.IdTipoPendencia);
                    lAcessaDados.AddInParameter(lDbCommand, "@St_Automatica", DbType.Boolean, pParametros.Objeto.StAutomatica);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<TipoDePendenciaCadastralInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(TipoDePendenciaCadastralInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<TipoDePendenciaCadastralInfo> lEntrada = new ReceberEntidadeRequest<TipoDePendenciaCadastralInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<TipoDePendenciaCadastralInfo> lRetorno = ReceberTipoPendencia(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
