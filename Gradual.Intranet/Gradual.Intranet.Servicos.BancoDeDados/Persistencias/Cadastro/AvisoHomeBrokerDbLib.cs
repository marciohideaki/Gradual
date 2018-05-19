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

        public static ConsultarObjetosResponse<AvisoHomeBrokerInfo> ConsultarAvisosHomeBroker(ConsultarEntidadeRequest<AvisoHomeBrokerInfo> pParametros)
        {
            try
            {
                var lResposta = new ConsultarObjetosResponse<AvisoHomeBrokerInfo>();
                var lDataTable = new DataTable();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                //@id_sistema

                DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "aviso_lst_sp");

                if (pParametros.Objeto != null && pParametros.Objeto.IdSistema > 0)
                {
                    lAcessaDados.AddInParameter(lDbCommand, "id_sistema", DbType.Int32, pParametros.Objeto.IdSistema);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "id_sistema", DbType.Int32, DBNull.Value);
                }

                lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null)
                {
                    foreach (DataRow lRow in lDataTable.Rows)
                    {
                        lResposta.Resultado.Add(CriarRegistroAvisoHomeBroker(lRow));
                    }
                }

                lDbCommand.Dispose();

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static AvisoHomeBrokerInfo CriarRegistroAvisoHomeBroker(DataRow pRow)
        {
            AvisoHomeBrokerInfo lRetorno = new AvisoHomeBrokerInfo();

            lRetorno.IdAviso = pRow["id_aviso"].DBToInt32();
            lRetorno.IdSistema = pRow["id_sistema"].DBToInt32();

            lRetorno.DsAviso = pRow["ds_aviso"].DBToString();
            lRetorno.DtEntrada = pRow["dt_entrada"].DBToDateTime();
            lRetorno.DtSaida = pRow["dt_saida"].DBToDateTime();

            if(pRow["ds_cblcs"] != DBNull.Value)
                lRetorno.DsCBLCs = pRow["ds_cblcs"].DBToString();

            lRetorno.StAtivacaoManual = pRow["st_ativacaomanual"].DBToString().ToUpper();

            return lRetorno;
        }

        public static ReceberObjetoResponse<AvisoHomeBrokerInfo> ReceberAvisoHomebroker(ReceberEntidadeRequest<AvisoHomeBrokerInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<AvisoHomeBrokerInfo> lResposta = new ReceberObjetoResponse<AvisoHomeBrokerInfo>();

                lResposta.Objeto = new AvisoHomeBrokerInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "aviso_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_aviso", DbType.Int32, pParametros.Objeto.IdAviso);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lResposta.Objeto = CriarRegistroAvisoHomeBroker(lDataTable.Rows[0]);
                    }
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        
        /*
        public static RemoverObjetoResponse<AvisoHomeBrokerInfo> RemoverAtividadesIlicitas(RemoverEntidadeRequest<AvisoHomeBrokerInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "atividades_ilicitas_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_atividadeilicita", DbType.Int32, pParametros.Objeto.IdAtividadeIlicita);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<AvisoHomeBrokerInfo> response = new RemoverEntidadeResponse<AvisoHomeBrokerInfo>()
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
        */
        public static SalvarEntidadeResponse<AvisoHomeBrokerInfo> SalvarAvisoHomebroker(SalvarObjetoRequest<AvisoHomeBrokerInfo> pParametros)
        {
            if (pParametros.Objeto.IdAviso > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<AvisoHomeBrokerInfo> Salvar(SalvarObjetoRequest<AvisoHomeBrokerInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "aviso_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_sistema", DbType.Int32, pParametros.Objeto.IdSistema);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_aviso",   DbType.String, pParametros.Objeto.DsAviso);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cblcs",   DbType.String, pParametros.Objeto.DsCBLCs);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_entrada", DbType.DateTime, pParametros.Objeto.DtEntrada);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_saida",   DbType.DateTime, pParametros.Objeto.DtSaida);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ativacaomanual", DbType.String, pParametros.Objeto.StAtivacaoManual);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_aviso", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<AvisoHomeBrokerInfo> lResponse = new SalvarEntidadeResponse<AvisoHomeBrokerInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_aviso"].Value)
                    };

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    return lResponse;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }
        
        private static SalvarEntidadeResponse<AvisoHomeBrokerInfo> Atualizar(SalvarObjetoRequest<AvisoHomeBrokerInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "aviso_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_aviso", DbType.Int32, pParametros.Objeto.IdAviso);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_sistema", DbType.Int32, pParametros.Objeto.IdSistema);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_aviso", DbType.String, pParametros.Objeto.DsAviso);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cblcs", DbType.String, pParametros.Objeto.DsCBLCs);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_entrada", DbType.DateTime, pParametros.Objeto.DtEntrada);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_saida", DbType.DateTime, pParametros.Objeto.DtSaida);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ativacaomanual", DbType.String, pParametros.Objeto.StAtivacaoManual);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<AvisoHomeBrokerInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);

                throw ex;
            }
        }
        

        private static void LogarModificacao(AvisoHomeBrokerInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            //ReceberEntidadeRequest<AvisoHomeBrokerInfo> lEntrada = new ReceberEntidadeRequest<AvisoHomeBrokerInfo>();
            //lEntrada.Objeto = pParametro;
            //ReceberObjetoResponse<AvisoHomeBrokerInfo> lRetorno = ReceberAtividadesIlicitas(lEntrada);
            //LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
