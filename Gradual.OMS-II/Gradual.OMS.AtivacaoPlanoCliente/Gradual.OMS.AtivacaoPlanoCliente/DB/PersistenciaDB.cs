#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.Common;
using System.Data;
using Gradual.Generico.Dados;
using Gradual.OMS.AtivacaoPlanoCliente.Lib;
using Gradual.OMS.PlanoCliente.Lib;
using Gradual.OMS.PlanoCliente.Lib.Util;
#endregion

namespace Gradual.OMS.AtivacaoPlanoCliente
{
    /// <summary>
    /// Classe para acesso a dados
    /// </summary>
    public class PersistenciaDB
    {
        #region Prpopriedades
        private const string _ConnStringSinacor        = "SINACOR";
        private const string _ConnStringControleAcesso = "ControleAcesso";
        private const string _ConnstringCorrWin        = "CORRWIN";
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Construtores
        public PersistenciaDB()
        {
            log4net.Config.XmlConfigurator.Configure();


        }
        #endregion

        #region Métodos
        /// <summary>
        /// Seleciona os clientes inativo para futura vefificação da ativa~çao no sinacor
        /// </summary>
        /// <returns></returns>
        public List<PlanoClienteInfo> SelecionaClientesEmEspera()
        {
            List<PlanoClienteInfo> lRetorno = new List<PlanoClienteInfo>();

            AcessaDados lAcesso = new AcessaDados();

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_ClientesProdutoNaoAtivo_sel"))
            {
                lAcesso.AddInParameter(cmd, "@st_situacao", DbType.AnsiString, 'E');

                lAcesso.AddInParameter(cmd, "@ID_PRODUTO_PLANO", DbType.Int32, 1);

                DataTable table = lAcesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(CriarRegistroListarClientesInfo(dr));
            }

            return lRetorno;
        }

        /// <summary>
        /// Seleciona os clientes inativo para futura vefificação da ativa~çao no sinacor
        /// </summary>
        /// <returns></returns>
        public List<PlanoClienteInfo> SelecionaClientesAssessorDirectEmEspera()
        {
            List<PlanoClienteInfo> lRetorno = new List<PlanoClienteInfo>();

            AcessaDados lAcesso = new AcessaDados();

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_ClientesAssessorDirectProdutoNaoAtivo_sel"))
            {
                lAcesso.AddInParameter(cmd, "@st_situacao", DbType.AnsiString, 'E');

                DataTable table = lAcesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(CriarRegistroListarClientesInfo(dr));
            }

            return lRetorno;
        }
        /// <summary>
        /// Cria a entidade com o registro recuperado da tabela
        /// </summary>
        /// <param name="row">Data row preenchida que vem do banco</param>
        /// <returns>Retorna uma entidade preenchida</returns>
        private PlanoClienteInfo CriarRegistroListarClientesInfo(DataRow row)
        {
            return new PlanoClienteInfo()
            {
                DtOperacao     = row["dt_operacao"].DBToDateTime(),
                DsEmail        = row["ds_email"].DBToString(),
                NomeCliente    = row["ds_nome"].DBToString(),
                DsCpfCnpj      = row["ds_cpfcnpj"].DBToString(),
                IdProdutoPlano = row["id_produto_plano"].DBToInt32(),
                CdCblc         = row["cd_cblc"].DBToInt32(),
            };
        }

        /// <summary>
        /// Verifica clientes exportados para o sinacor
        /// </summary>
        /// <param name="pRequest">Lista de planos de clientes</param>
        /// <returns>Retorna uma lista de planos de clientes</returns>
        public List<PlanoClienteInfo> VerificaClientesExportadosSinacor(List<PlanoClienteInfo> pRequest)
        {
            List<PlanoClienteInfo> lRetorno = new List<PlanoClienteInfo>();

            try
            {
                AcessaDados lAcesso = new AcessaDados("Retorno");

                lAcesso.ConnectionStringName = _ConnStringSinacor;

                if (pRequest.Count > 0)
                {
                    using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTESEXPORTADOS_LST"))
                    {
                        foreach (PlanoClienteInfo plano in pRequest)
                        {
                            cmd.Parameters.Clear();

                            lAcesso.AddInParameter(cmd, "pCpfCnpj", DbType.AnsiString, plano.DsCpfCnpj);

                            logger.InfoFormat("Buscando cpfcnpj {0}", plano.DsCpfCnpj);

                            DataTable table = lAcesso.ExecuteOracleDataTable(cmd);

                            if (table.Rows.Count > 0)
                                lRetorno.Add(CriarRegistroListarClientesCblcInfo(table.Rows[0]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error em VerificaClientesExportadosSinacor - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);

                throw ex;
            }

            return lRetorno;
        }

        /// <summary>
        /// Cria entidade com o registro recuperado da tabela
        /// </summary>
        /// <param name="dr">DataRow do registro</param>
        /// <returns></returns>
        private PlanoClienteInfo CriarRegistroListarClientesCblcInfo(DataRow dr)
        {
            return new PlanoClienteInfo()
            {
                CdCblc    = dr["CD_CLIENTE"].DBToInt32(),
                DtAdesao  = DateTime.Now,
                DsCpfCnpj = dr["CD_CPFCGC"].DBToString(),
            };
        }

        /// <summary>
        /// Inserri planos selecionados pelo cliente
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public List<PlanoClienteInfo> AtualizaPlanoClienteExistente(List<PlanoClienteInfo> pRequest)
        {
            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = _ConnStringControleAcesso;

            List<PlanoClienteInfo>  lRetorno = new List<PlanoClienteInfo>();

            lRetorno.AddRange(pRequest);

            logger.Info("Passou da linha lRetorno.LstPlanoCliente = pRequest.LstPlanoCliente;");

            acesso.Conexao._ConnectionStringName = _ConnStringControleAcesso;

            var lDbConnection = acesso.Conexao.CreateIConnection();

            lDbConnection.Open();

            DbTransaction lTrans = lDbConnection.BeginTransaction();

            try
            {
                using (DbCommand cmdins = acesso.CreateCommand(lTrans, CommandType.StoredProcedure, "prc_ClienteExistenteProduto_upd"))
                {
                    foreach (PlanoClienteInfo info in lRetorno)
                    {
                        cmdins.Parameters.Clear();

                        acesso.AddInParameter(cmdins, "@ds_cpfcnpj",       DbType.AnsiString,     info.DsCpfCnpj);
                        acesso.AddInParameter(cmdins, "@st_situacao",      DbType.String,         info.StSituacao);
                        acesso.AddInParameter(cmdins, "@id_produto_plano", DbType.Int32,          info.IdProdutoPlano);
                        acesso.AddInParameter(cmdins, "@cd_cblc",          DbType.Int32,          info.CdCblc);
                        acesso.AddInParameter(cmdins, "@dt_adesao",        DbType.DateTime,       info.DtAdesao);

                        this.AtualizaPlanoCorretagemClienteSinacor(info);

                        acesso.ExecuteNonQuery(cmdins);
                    }
                }

                lTrans.Commit();
            }
            catch (Exception ex)
            {
                lTrans.Rollback();

                logger.ErrorFormat("Error em AtualizaPlanoClienteExistente- {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);

                throw ex;
            }

            return lRetorno;
        }

        /// <summary>
        /// Atualiza o tipo de corretagem do cliente no sinacor TSCCLIBOL
        /// </summary>
        private void AtualizaPlanoCorretagemClienteSinacor(PlanoClienteInfo pRequest)
        {
            try
            {
                AcessaDados acesso = new AcessaDados();

                acesso.ConnectionStringName = _ConnstringCorrWin;

                string lQuery = string.Format("UPDATE TSCCLIBOL SET IN_TIPO_CORRET = 1 WHERE CD_CLIENTE = {0}", pRequest.CdCblc);

                using (DbCommand cmdUpd = acesso.CreateCommand(CommandType.Text, lQuery))
                {
                    acesso.ExecuteNonQuery(cmdUpd);
                }

                //string lQueryCus = string.Format("UPDATE TSCCLICUS SET TP_CUSTODIA = 351 WHERE CD_CLIENTE = {0}", pRequest.CdCblc);

                //using (DbCommand cmdUpd = acesso.CreateCommand(CommandType.Text, lQueryCus))
                //{
                //    acesso.ExecuteNonQuery(cmdUpd);
                //}
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em AtualizaPlanoCorretagemClienteSinacor- {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        
        #endregion
    }
}