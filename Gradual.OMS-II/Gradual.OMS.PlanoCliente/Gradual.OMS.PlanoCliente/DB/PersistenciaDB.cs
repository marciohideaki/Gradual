using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.OMS.PlanoCliente.Lib;
using Gradual.OMS.PlanoCliente.Lib.Util;
using log4net;
using System.Configuration;

namespace Gradual.OMS.PlanoCliente
{
    public class PersistenciaDB
    {
        #region | Atributos

        private const string _ConnectionStringName = "OMS";
        private const string _ConnectionStringSinacor = "SINACOR";
        private const string _ConnstringCorrWin = "CORRWIN";
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private enum TipoProduto
        {
            PlanoDirect = 1,
            PlanoIRAberto = 4,
            PlanoIRFechado = 5,
        };

        #endregion

        #region | Propriedades

        private string GetEmailPadraoCliente
        {
            get 
            {
                var lRetorno = "codigo@gradual.com.br";

                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailCalculadoraIR"]))
                    lRetorno = ConfigurationManager.AppSettings["EmailCalculadoraIR"];

                return lRetorno;
            }
        }

        #endregion

        #region | Métodos

        /// <summary>
        /// Consultar planos de cliente com filtro de relatório
        /// </summary>
        /// <param name="pRequest">Objeto do tipo ListarProdutosClienteRequest</param>
        /// <returns>Retorna uma lsita com filro efetuado por request</returns>
        public List<PlanoClienteInfo> ConsultarPlanoClientesFiltrado(ListarProdutosClienteRequest pRequest)
        {
            List<PlanoClienteInfo> lRetorno = new List<PlanoClienteInfo>();

            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = _ConnectionStringName;

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "prc_FiltrarClienteProduto_sel"))
            {
                acesso.AddInParameter(cmd, "@dt_de", DbType.DateTime, pRequest.De);
                acesso.AddInParameter(cmd, "@dt_ate", DbType.DateTime, pRequest.Ate);
                acesso.AddInParameter(cmd, "@id_produto", DbType.Int32, pRequest.IdProduto);
                acesso.AddInParameter(cmd, "@cd_cblc", DbType.Int32, pRequest.CdCblc);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(CriarRegistroFiltrarPlanoClientesInfo(dr));
            }

            return lRetorno;
        }

        /// <summary>
        /// Consultar planos para um cliente onde o intervalo de datas passadas não exista no banco.
        /// </summary>
        /// <param name="pRequest">Retorna uma lsita com filro efetuado por request</param>
        /// <returns>Retorna uma lsita com filro efetuado por request</returns>
        public List<PlanoClienteInfo> ConsultarPlanoClientesRengeDatas(ListarProdutosClienteRequest pRequest)
        {
            List<PlanoClienteInfo> lRetorno = new List<PlanoClienteInfo>();

            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = _ConnectionStringName;

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_EXISTENTE_POR_RANGER_DATA"))
            {
                acesso.AddInParameter(cmd, "@DT_INICIAL", DbType.DateTime, pRequest.De);
                acesso.AddInParameter(cmd, "@DT_FIM", DbType.DateTime, pRequest.Ate);
                acesso.AddInParameter(cmd, "@id_produto", DbType.Int32, pRequest.IdProduto);
                acesso.AddInParameter(cmd, "@DS_CPFCNPJ", DbType.String, pRequest.DsCpfCnpj);
                acesso.AddInParameter(cmd, "@ST_SITUACAO", DbType.String, pRequest.StSituacao);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(CriarRegistroFiltrarPlanoClientesInfo(dr));
            }

            return lRetorno;
        }

        /// <summary>
        /// Consultar planos vigentes para clientes num determinado plano
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns>Retorna uma lsita com filro efetuado por request</returns>
        public List<PlanoClienteInfo> ConsultarPlanoClientesVigente(ListarProdutosClienteRequest pRequest)
        {
            List<PlanoClienteInfo> lRetorno = new List<PlanoClienteInfo>();

            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = _ConnectionStringName;

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_EXISTENTE_VIGENTE"))
            {
                acesso.AddInParameter(cmd, "@id_produto", DbType.Int32, pRequest.IdProduto);
                acesso.AddInParameter(cmd, "@DS_CPFCNPJ", DbType.String, pRequest.DsCpfCnpj);
                acesso.AddInParameter(cmd, "@ST_SITUACAO", DbType.String, pRequest.StSituacao);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(CriarRegistroFiltrarPlanoClientesInfo(dr));
            }

            return lRetorno;
        }

        /// <summary>
        /// Listar os produtos aderidos de um cliente específico
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public List<PlanoClienteInfo> ConsultarPlanoClientes(ListarProdutosClienteRequest pRequest)
        {
            List<PlanoClienteInfo> lRetorno = new List<PlanoClienteInfo>();

            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = "Seguranca";

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "prc_ClienteProduto_sel"))
            {
                acesso.AddInParameter(cmd, "@ds_cpfcnpj", DbType.AnsiString, pRequest.DsCpfCnpj);

                //if (pRequest.StSituacao != null)
                acesso.AddInParameter(cmd, "@st_situacao", DbType.AnsiString, pRequest.StSituacao);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(CriarRegistroPlanoClientesInfo(dr));
            }

            return lRetorno;
        }

        /// <summary>
        /// Listar os Produtos da da tabela
        /// </summary>
        /// <returns>Retorna uma lista de produtos</returns>
        public List<ProdutoInfo> ListarProdutos()
        {
            List<ProdutoInfo> lRetorno = new List<ProdutoInfo>();

            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = "Seguranca";

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "prc_Produtos_lst"))
            {
                DataTable table = acesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(CriarRegistroProdutosInfo(dr));
            }

            return lRetorno;
        }

        /// <summary>
        /// Inserri planos selecionados pelo cliente
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public InserirProdutosClienteResponse AtualizaPlanoClienteExistente(InserirProdutosClienteRequest pRequest)
        {
            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = _ConnectionStringName;

            InserirProdutosClienteResponse lRetorno = new InserirProdutosClienteResponse();

            lRetorno.LstPlanoCliente = new List<PlanoClienteInfo>();

            lRetorno.LstPlanoCliente.AddRange(pRequest.LstPlanoCliente);

            logger.Info("Passou da linha lRetorno.LstPlanoCliente = pRequest.LstPlanoCliente;");

            acesso.Conexao._ConnectionStringName = _ConnectionStringName;

            var lDbConnection = acesso.Conexao.CreateIConnection();

            lDbConnection.Open();

            DbTransaction lTrans = lDbConnection.BeginTransaction();

            try
            {

                using (DbCommand cmdins = acesso.CreateCommand(lTrans, CommandType.StoredProcedure, "prc_ClienteExistenteProduto_upd"))
                {
                    foreach (PlanoClienteInfo info in lRetorno.LstPlanoCliente)
                    {
                        cmdins.Parameters.Clear();

                        acesso.AddInParameter(cmdins, "@ds_cpfcnpj", DbType.AnsiString, info.DsCpfCnpj);
                        acesso.AddInParameter(cmdins, "@st_situacao", DbType.String, info.StSituacao);
                        acesso.AddInParameter(cmdins, "@id_produto_plano", DbType.Int32, info.IdProdutoPlano);
                        acesso.AddInParameter(cmdins, "@cd_cblc", DbType.Int32, info.CdCblc);
                        acesso.AddInParameter(cmdins, "@dt_adesao", DbType.DateTime, info.DtAdesao);

                        acesso.ExecuteNonQuery(cmdins);
                    }
                }

                lTrans.Commit();
            }
            catch (Exception ex)
            {
                lTrans.Rollback();

                logger.ErrorFormat("Error - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);

                throw ex;
            }
            finally
            {
                if (lDbConnection.State != ConnectionState.Closed)
                {
                    lDbConnection.Close();
                    lDbConnection.Dispose();
                }
            }

            return lRetorno;
        }

        public AtualizarProdutosClienteResponse AtualizarPlanoCliente(AtualizarProdutosClienteRequest pRequest)
        {
            var lRetorno = new AtualizarProdutosClienteResponse() { LstPlanoCliente = new List<PlanoClienteInfo>() };
           

            lRetorno.LstPlanoCliente.AddRange(pRequest.LstPlanoCliente);

            try
            {

                foreach (PlanoClienteInfo planoCliente in pRequest.LstPlanoCliente)
                {
                    //atualiza a tabela de produtos somente se NÃO for o plano IR Fechado, pois o plano fechado já foi incluido e cancelado no mesmo dia.
                    if (planoCliente.IdProdutoPlano != (int)Gradual.OMS.PlanoCliente.PersistenciaDB.TipoProduto.PlanoIRFechado)
                    {
                        this.AtualizarPlanoClienteSql(planoCliente);
                    }

                    //atualiza a integração com o mycapital somente para os planos IR aberto.
                    if (planoCliente.IdProdutoPlano == (int)Gradual.OMS.PlanoCliente.PersistenciaDB.TipoProduto.PlanoIRAberto)
                    {
                        this.AtualizarPlanoClienteOracle(planoCliente);
                    }
                    //Exclui a corretagem do cliente no sinacor para clientes do plano direct.
                    if (planoCliente.IdProdutoPlano == (int)Gradual.OMS.PlanoCliente.PersistenciaDB.TipoProduto.PlanoDirect)
                    {
                        this.ExcluiAdesaoCorretagemClienteSinacor(planoCliente);
                    }

                }
            }
               
            catch (Exception ex)
            {
                logger.ErrorFormat("Error - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);

                throw ex;
            }
                      


            return lRetorno;
        }

        /// <summary>
        /// Inserri planos selecionados pelo cliente, usado pela intranet, exclui antes de inserir.
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public InserirProdutosClienteResponse InserirPlanoCliente(InserirProdutosClienteRequest pRequest)
        {
            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = "Seguranca";

            InserirProdutosClienteResponse lRetorno = new InserirProdutosClienteResponse();

            lRetorno.LstPlanoCliente = new List<PlanoClienteInfo>();

            lRetorno.LstPlanoCliente.AddRange(pRequest.LstPlanoCliente);

            logger.Info("Passou da linha lRetorno.LstPlanoCliente = pRequest.LstPlanoCliente;");

            acesso.Conexao._ConnectionStringName = "Seguranca";

            var lDbConnection = acesso.Conexao.CreateIConnection();

            lDbConnection.Open();

            DbTransaction lTrans = lDbConnection.BeginTransaction();

            try
            {

                using (DbCommand cmddel = acesso.CreateCommand(lTrans, CommandType.StoredProcedure, "prc_ClienteProduto_del"))
                {
                    cmddel.Parameters.Clear();

                    acesso.AddInParameter(cmddel, "@ds_cpfcnpj", DbType.AnsiString, (string)pRequest.LstPlanoCliente[0].DsCpfCnpj);

                    acesso.ExecuteNonQuery(cmddel);
                }

                using (DbCommand cmdins = acesso.CreateCommand(lTrans, CommandType.StoredProcedure, "prc_ClienteProduto_ins"))
                {
                    foreach (PlanoClienteInfo info in lRetorno.LstPlanoCliente)
                    {
                        cmdins.Parameters.Clear();

                        acesso.AddInParameter(cmdins, "@ds_cpfcnpj", DbType.AnsiString, info.DsCpfCnpj);
                        acesso.AddInParameter(cmdins, "@dt_operacao", DbType.DateTime, info.DtOperacao);
                        acesso.AddInParameter(cmdins, "@st_situacao", DbType.String, info.StSituacao);
                        acesso.AddInParameter(cmdins, "@id_produto_plano", DbType.Int32, info.IdProdutoPlano);
                        acesso.AddInParameter(cmdins, "@cd_cblc", DbType.Int32, info.CdCblc);

                        acesso.ExecuteNonQuery(cmdins);
                    }
                }

                lTrans.Commit();
            }
            catch (Exception ex)
            {
                lTrans.Rollback();

                logger.ErrorFormat("Error - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);

                throw ex;
            }

            finally
            {
                if (lDbConnection.State != ConnectionState.Closed)
                {
                    lDbConnection.Close();
                    lDbConnection.Dispose();
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Insere planos selecionados pelo cliente, inserindo também o código cblc, data adesão  - usado pelo portal, só inseri e NÃO exlui antes de inserir.
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public InserirProdutosClienteResponse InserirPlanoClienteExistente(InserirProdutosClienteRequest pRequest)
        {
            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = _ConnectionStringName;

            InserirProdutosClienteResponse lRetorno = new InserirProdutosClienteResponse();

            lRetorno.LstPlanoCliente = new List<PlanoClienteInfo>();

            lRetorno.LstPlanoCliente.AddRange(pRequest.LstPlanoCliente);

            logger.Info("Passou da linha lRetorno.LstPlanoCliente = pRequest.LstPlanoCliente;");

            acesso.Conexao._ConnectionStringName = _ConnectionStringName;

            var lDbConnection = acesso.Conexao.CreateIConnection();

            lDbConnection.Open();

            DbTransaction lTrans = lDbConnection.BeginTransaction();

            try
            {
                using (DbCommand cmdins = acesso.CreateCommand(lTrans, CommandType.StoredProcedure, "prc_ClienteExistenteProduto_ins"))
                {
                    foreach (PlanoClienteInfo info in lRetorno.LstPlanoCliente)
                    {
                        cmdins.Parameters.Clear();

                        acesso.AddInParameter(cmdins, "@ds_cpfcnpj"         , DbType.AnsiString , info.DsCpfCnpj                            );
                        acesso.AddInParameter(cmdins, "@dt_operacao"        , DbType.String     , info.DtOperacao.ToString("yyyyMMdd")      );
                        acesso.AddInParameter(cmdins, "@dt_adesao"          , DbType.String     , info.DtAdesao.Value.ToString("yyyyMMdd")  );
                        acesso.AddInParameter(cmdins, "@st_situacao"        , DbType.String     , info.StSituacao                           );
                        acesso.AddInParameter(cmdins, "@id_produto_plano"   , DbType.Int32      , info.IdProdutoPlano                       );
                        acesso.AddInParameter(cmdins, "@cd_cblc"            , DbType.Int32      , info.CdCblc                               );
                        acesso.AddInParameter(cmdins, "@dt_fim_adesao"      , DbType.DateTime   , info.DtFimAdesao                          );


                        acesso.ExecuteNonQuery(cmdins);
                    }
                }

                lTrans.Commit();
            }
            catch (Exception ex)
            {
                lTrans.Rollback();

                logger.ErrorFormat("Error - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);

                throw ex;
            }
            finally
            {
                if (lDbConnection.State != ConnectionState.Closed)
                {
                    lDbConnection.Close();
                    lDbConnection.Dispose();
                }
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

                string lQueryCus = string.Format("UPDATE TSCCLICUS SET TP_CUSTODIA = 351 WHERE CD_CLIENTE = {0}", pRequest.CdCblc);

                using (DbCommand cmdUpd = acesso.CreateCommand(CommandType.Text, lQueryCus))
                {
                    acesso.ExecuteNonQuery(cmdUpd);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em AtualizaPlanoCorretagemClienteSinacor- {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Atualiza a adesão do cliente para a situação normal, sem plano de corretagem especial
        /// </summary>
        /// <param name="pRequest">Request com dados do cliente</param>
        public void ExcluiAdesaoCorretagemClienteSinacor(PlanoClienteInfo pRequest)
        {
            try
            {
                AcessaDados acesso = new AcessaDados();

                acesso.ConnectionStringName = _ConnstringCorrWin;

                string lQuery = string.Format("UPDATE TSCCLIBOL SET IN_TIPO_CORRET = 0 WHERE CD_CLIENTE = {0}", pRequest.CdCblc);

                using (DbCommand cmdUpd = acesso.CreateCommand(CommandType.Text, lQuery))
                {
                    acesso.ExecuteNonQuery(cmdUpd);
                }

                string lQueryCus = string.Format("UPDATE TSCCLICUS SET TP_CUSTODIA = 151 WHERE CD_CLIENTE = {0}", pRequest.CdCblc);

                using (DbCommand cmdUpd = acesso.CreateCommand(CommandType.Text, lQueryCus))
                {
                    acesso.ExecuteNonQuery(cmdUpd);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em ExcluiAdesaoCorretagemClienteSinacor - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        #endregion

        #region | Métodos de apoio

        public string ToCodigoClienteComDigito(object pObject)
        {
            int lDigito = 0;

            int lCodigoCorretora = 227;

            lDigito = (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            lDigito = lDigito % 11;

            if (lDigito == 0 || lDigito == 1)
            {
                lDigito = 0;
            }

            else
            {
                lDigito = 11 - lDigito;
            }

            return string.Format("{0}{1}", pObject.ToString(), lDigito);
        }

        private void AtualizarPlanoClienteSql(PlanoClienteInfo pRequest)
        {

            var lRetorno = new IntegracaoIRInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "Seguranca";
            try
            {

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ClienteProduto_upd"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj"       , DbType.AnsiString , pRequest.DsCpfCnpj        );
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_fim_adesao"    , DbType.DateTime   , pRequest.DtFimAdesao      );
                    lAcessaDados.AddInParameter(lDbCommand, "@st_situacao"      , DbType.String     , pRequest.StSituacao       );
                    lAcessaDados.AddInParameter(lDbCommand, "@id_produto_plano" , DbType.Int32      , pRequest.IdProdutoPlano   );

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro ao Atualizar Plano cliente no SQL Sever - Error - {0} ", ex.Message);

                throw ex;
            }
                      

            
        }

        private void AtualizarPlanoClienteOracle(PlanoClienteInfo pRequest)
        {
            
            var lIntegracaoIR = new IntegracaoIRInfo();

            
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "MyCapital";
            try
            {

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "INS_MYC_INTEGRACAO"))
                {
                
                        lDbCommand.Parameters.Clear();

                        lIntegracaoIR = this.CriarRegistroIntegracaoIRInfo(pRequest);

                        lAcessaDados.AddInParameter(lDbCommand, "P_CODIGOBOVESPACLIENTE"        , DbType.Int32      , lIntegracaoIR.IdBovespa               );
                        lAcessaDados.AddInParameter(lDbCommand, "P_EMAILUSUARIO"                , DbType.String     , lIntegracaoIR.Email                   );
                        lAcessaDados.AddInParameter(lDbCommand, "P_CIDADEUSUARIO"               , DbType.String     , lIntegracaoIR.Cidade                  );
                        lAcessaDados.AddInParameter(lDbCommand, "P_ESTADOUSUARIO"               , DbType.String     , lIntegracaoIR.Estado                  );
                        lAcessaDados.AddInParameter(lDbCommand, "P_DTAINICIOLANCAMENTOUSUARIO"  , DbType.DateTime   , lIntegracaoIR.dataFim                 );
                        lAcessaDados.AddInParameter(lDbCommand, "P_STABLOQUEADO"                , DbType.String     , lIntegracaoIR.EstadoBloqueado         );
                        lAcessaDados.AddInParameter(lDbCommand, "P_CODIGOEVENTO"                , DbType.Int32      , IntegracaoIRInfo.CodigoEvento.CANCELAR);
                        lAcessaDados.AddInParameter(lDbCommand, "P_DESCRICAO"                   , DbType.String     , lIntegracaoIR.Descricao               );
                        lAcessaDados.AddInParameter(lDbCommand, "P_TPOPRODUTO"                  , DbType.Int32      , IntegracaoIRInfo.TipoProduto.BOVESPA  );

                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro ao Atualizar Plano cliente no MyCapital - Error - {0} ", ex.Message);

                throw ex;
            }

            
        }

        private IntegracaoIRInfo CriarRegistroIntegracaoIRInfo(PlanoClienteInfo pPlanoCliente)
        {
            var lRetorno = new IntegracaoIRInfo();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "Cadastro";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_endereco_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa", DbType.Int32, pPlanoCliente.CdCblc);
                lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Int32, 1);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Cidade = lDataTable.Rows[0]["ds_cidade"].DBToString();
                    lRetorno.Estado = lDataTable.Rows[0]["cd_uf"].DBToString();
                }
            }

            lRetorno.IdBovespa  = Convert.ToInt32(this.ToCodigoClienteComDigito(pPlanoCliente.CdCblc)); //coloca o digito no código CBLC
            lRetorno.Email      = this.GetEmailPadraoCliente;
            lRetorno.dataFim    = pPlanoCliente.DtFimAdesao;
            lRetorno.EstadoBloqueado = "S";

            return lRetorno;
        }

        private ProdutoInfo CriarRegistroProdutosInfo(DataRow dr)
        {
            return new ProdutoInfo()
            {
                DsProduto = dr["ds_produto"].DBToString(),
                IdProduto = dr["id_produto_plano"].DBToInt32()
            };
        }

        private PlanoClienteInfo CriarRegistroFiltrarPlanoClientesInfo(DataRow dr)
        {
            return new PlanoClienteInfo()
            {
                DtOperacao = dr["dt_operacao"].DBToDateTime(),
                IdProdutoPlano = dr["id_produto_plano"].DBToInt32(),
                NomeCliente = dr["ds_nomecliente"].DBToString(),
                DsCpfCnpj = dr["ds_cpfcnpj"].DBToString(),
                StSituacao = dr["st_situacao"].DBToChar(),
                NomeProduto = dr["ds_produto"].DBToString(),
                DtFimAdesao = dr["dt_fim_adesao"].DBToDateTime()
            };
        }

        private PlanoClienteInfo CriarRegistroPlanoClientesInfo(DataRow dr)
        {
            return new PlanoClienteInfo()
            {
                DtOperacao = dr["dt_operacao"].DBToDateTime(),
                IdProdutoPlano = dr["id_produto_plano"].DBToInt32(),
                NomeCliente = dr["ds_nome_cliente"].DBToString(),
                StSituacao = dr["st_situacao"].DBToChar(),
                NomeProduto = dr["ds_produto"].DBToString(),
                DtAdesao = dr["dt_adesao"].DBToDateTime(),
                DtUltimaOperacao = dr["dt_ultima_cobranca"].DBToDateTime(),
                DtUltimaCobranca = dr["dt_ultima_cobranca"].DBToDateTime(),
                CdCblc = dr["cd_cblc"].DBToInt32(),
                DsCpfCnpj = dr["ds_cpfcnpj"].DBToString(),
                DtFimAdesao = dr["dt_fim_adesao"].DBToDateTime(),
                VlCobrado = dr["vl_cobrado"].DBToDecimal()
            };
        }

        #endregion
    }
}
