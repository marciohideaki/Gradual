using System;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.OMS.Library;
using Gradual.Servico.GerenteHistClienteParamValor.Lib;
using Gradual.Servico.GerenteHistClienteParamValor.Lib.Mensagem;

namespace Gradual.Servico.GerenteHistClienteParamValor.Persistencias
{
    public class GerenteHistoricoRiscoClienteParametroValorDbLib
    {
        private Nullable<int> gIdLog = null;

        public GerenteHistoricoRiscoClienteParametroValorResponse IniciarProcessoGuadarHistorico(GerenteHistoricoRiscoClienteParametroValorRequest pParametro)
        {
            if (null == pParametro.TipoRequisitante)
                return new GerenteHistoricoRiscoClienteParametroValorResponse()
                {   //--> Valida se o requisitante do processo foi informado.
                    DescricaoResposta = "\"TipoRequisitante\" não informado.",
                    StatusResposta = MensagemResponseStatusEnum.ErroPrograma,
                };

            var lRetorno = new GerenteHistoricoRiscoClienteParametroValorResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "Risco";

            DbConnection lDbConnection;
            DbTransaction lDbTransaction;
            {   //--> Criando a transação.
                var lConexao = new Conexao();
                lConexao._ConnectionStringName = "Risco";
                lDbConnection = lConexao.CreateIConnection();
                lDbConnection.Open();
                lDbTransaction = lDbConnection.BeginTransaction();
            }

            try
            {
                this.GuardarLogDoHistorico(pParametro.TipoRequisitante.Value, EstadoUltimoHistorico.Processando, string.Empty);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, "prc_realiza_historico_tb_cliente_parametro_valor"))
                {   //--> Migrando o histórico da tabela 'tb_cliente_parametro_valor'
                    lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, "prc_realiza_historico_tb_cliente_parametro"))
                {   //--> Migrando o histórico da tabela 'tb_cliente_parametro'
                    lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                }

                this.GuardarLogDoHistorico(pParametro.TipoRequisitante.Value, EstadoUltimoHistorico.Concluido, string.Empty);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                lDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                lDbTransaction.Rollback();

                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();

                this.GuardarLogDoHistorico(pParametro.TipoRequisitante.Value, EstadoUltimoHistorico.Erro, ex.ToString());
            }
            finally
            {
                this.gIdLog = null;

                if (null != lDbConnection && lDbConnection.State == System.Data.ConnectionState.Open)
                {
                    lDbConnection.Close();
                }
            }

            return lRetorno;
        }

        public GerenteHistoricoEstadoUltimoHistoricoResponse RecuperaStatusUltimoHistorico(GerenteHistoricoEstadoUltimoHistoricoRequest pParametro)
        {
            var lRetorno = new GerenteHistoricoEstadoUltimoHistoricoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "Risco";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_valor_historico_log_sel"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRetorno.DataHoraUltimoHistorico = lDataTable.Rows[0]["dt_processamento"].DBToDateTime();
                        lRetorno.EstadoUltimoHistorico = (EstadoUltimoHistorico)lDataTable.Rows[0]["st_Historico"].DBToInt32();
                        lRetorno.DescricaoResposta = lDataTable.Rows[0]["ds_historico"].DBToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        private void GuardarLogDoHistorico(TipoRequisitante pTipoRequisitante, EstadoUltimoHistorico pEstadoUltimoHistorico, string pDescricaoHistorico)
        {
            var lRetorno = new GerenteHistoricoEstadoUltimoHistoricoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "Risco";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_valor_historico_log_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@historico_log", DbType.Int32, this.gIdLog);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_Historico", DbType.Int32, (int)pEstadoUltimoHistorico);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_requisitante", DbType.Int32, (int)pTipoRequisitante);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_historico", DbType.String, pDescricaoHistorico);

                    this.gIdLog = lAcessaDados.ExecuteScalar(lDbCommand).DBToInt32();
                }
            }
            catch { }
        }
    }
}
