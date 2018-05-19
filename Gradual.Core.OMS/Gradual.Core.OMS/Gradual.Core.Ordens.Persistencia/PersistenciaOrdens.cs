using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;
using Gradual.Core.Ordens.Lib;
using Gradual.Core.Ordens.Lib.Dados.Enum;
using Gradual.Core.Ordens.Lib.Mensageria;
using Gradual.Core.OMS.LimiteBMF.Lib;
using System.Data.SqlClient;
using Gradual.Core.OMS.LimiteBMF.Lib;
using System.Collections;

namespace Gradual.Core.Ordens.Persistencia
{
    public class PersistenciaOrdens
    {

        //private const string gNomeConexao     = "Risco";
        //private const string gNomeConexaoOMS  = "RiscoOMS";
        //private const string gNomeConexaoHomo = "RiscoHomo";

        private const string gCotacao = "Risco";
        private const string gNomeConexao = "RiscoOMS";
        private const string gNomeConexaoProducao = "GRADUALOMS";
        private const string gNomeConexaoOMS = "RiscoOMS";
        private const string gConexaoSinacor = "SINACOR";
        private const string gNomeConexaoHomo = "RiscoOMS";
        private const string gNomeConexaoSpider = "GradualSpider";

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Método responsável por inserir uma nova ordem no banco de dados
        /// </summary>
        /// <param name="_ClienteOrdemInfo">Atributos da ordem do cliente</param>
        /// <returns>bool</returns>
        public bool InserirOrdem(OrdemInfo _ClienteOrdemInfo)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                //Cria DBComand atribuindo a storedprocedure PRC_INS_ORDER_ROUTER_OMS_V2                
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_INS_ORDER_ROUTER_OMS_V3"))
                {
                    logger.Info("PREENCHE PARAMETROS PARA GRAVAR A ORDEM NO BANCO DE DADOS.");
                    // Adiciona os parametros de ordem
                    if (_ClienteOrdemInfo.IdOrdem == 0){
                        lAcessaDados.AddInParameter(lDbCommand, "@OrderID", DbType.Int32, 0);
                    }
                    else{
                        lAcessaDados.AddInParameter(lDbCommand, "@OrderID", DbType.Int32, _ClienteOrdemInfo.IdOrdem);
                    }

                    lAcessaDados.AddInParameter(lDbCommand, "@ClOrdID", DbType.String, _ClienteOrdemInfo.ClOrdID);

                    if (_ClienteOrdemInfo.StopStartID == 0){
                        lAcessaDados.AddInParameter(lDbCommand, "@StopStartID", DbType.Int32, DBNull.Value);
                    }
                    else{
                        lAcessaDados.AddInParameter(lDbCommand, "@StopStartID", DbType.Int32, _ClienteOrdemInfo.StopStartID);
                    }

                    lAcessaDados.AddInParameter(lDbCommand, "@OrigClOrdID", DbType.String, _ClienteOrdemInfo.OrigClOrdID);
                    lAcessaDados.AddInParameter(lDbCommand, "@Account", DbType.Int32, _ClienteOrdemInfo.Account);
                    lAcessaDados.AddInParameter(lDbCommand, "@Symbol", DbType.String, _ClienteOrdemInfo.Symbol.Trim().ToUpper());
                    lAcessaDados.AddInParameter(lDbCommand, "@OrdTypeID", DbType.AnsiString, (int)_ClienteOrdemInfo.OrdType);
                    lAcessaDados.AddInParameter(lDbCommand, "@OrdStatusID", DbType.Int32, (int)_ClienteOrdemInfo.OrdStatus);
                    lAcessaDados.AddInParameter(lDbCommand, "@ExpireDate", DbType.DateTime, _ClienteOrdemInfo.ExpireDate);
                    lAcessaDados.AddInParameter(lDbCommand, "@TimeInForce", DbType.Int32, (int)_ClienteOrdemInfo.TimeInForce);
                    lAcessaDados.AddInParameter(lDbCommand, "@ChannelID", DbType.Int32, _ClienteOrdemInfo.ChannelID);
                    lAcessaDados.AddInParameter(lDbCommand, "@ExecBroker", DbType.String, _ClienteOrdemInfo.ExecBroker);
                    lAcessaDados.AddInParameter(lDbCommand, "@Side", DbType.Int32, (int)_ClienteOrdemInfo.Side);
                    lAcessaDados.AddInParameter(lDbCommand, "@OrderQty", DbType.Int32, _ClienteOrdemInfo.OrderQty);
                    lAcessaDados.AddInParameter(lDbCommand, "@OrderQtyMin", DbType.Single, _ClienteOrdemInfo.MinQty);
                    lAcessaDados.AddInParameter(lDbCommand, "@OrderQtyApar", DbType.Single, _ClienteOrdemInfo.MaxFloor);
                    lAcessaDados.AddInParameter(lDbCommand, "@systemID", DbType.AnsiString, _ClienteOrdemInfo.CompIDOMS);

                    lAcessaDados.AddInParameter(lDbCommand, "@OrderQtyRemaining", DbType.Int32, _ClienteOrdemInfo.OrderQtyRemmaining);
                    lAcessaDados.AddInParameter(lDbCommand, "@Price", DbType.Single, _ClienteOrdemInfo.Price);
                    lAcessaDados.AddInParameter(lDbCommand, "@description", DbType.AnsiString, null);

                    logger.Info("ENVIA SOLICITAÇÃO PARA A STORED PROCEDURE");
                    
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("SOLICITAÇÃO EFETUADA COM SUCESSO.");

                    return true;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }

        public DateTime CarregarVencimentoOpcoes(string symbol)
        {

            AcessaDados lAcessaDados = new AcessaDados();

            DateTime dtVencimento = new DateTime();

            try
            {
                lAcessaDados.ConnectionStringName = gConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_vencimentoOpcao_newoms"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodNeg", DbType.String,symbol) ;

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string Instrumento = lDataTable.Rows[i]["cd_codneg"].ToString();
                            string DataVencimento = lDataTable.Rows[i]["dt_datven"].ToString();

                            return DateTime.Parse(DataVencimento);
                        }

                    }

                    return dtVencimento;
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao carregar a lista de vencimento de opções");

            }


            return dtVencimento;

        }

        /// <summary>
        /// Método responsável por atualizar/gravar um limite operacional 
        /// </summary>
        /// <param name="_ClienteOrdemInfo">Atributos do limite operacional</param>
        /// <returns>bool</returns>
        public InserirLimiteClienteBMFResponse AtualizarLimiteBMF(InserirLimiteClienteBMFRequest InserirLimiteClienteBMFRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            InserirLimiteClienteBMFResponse resposta = new InserirLimiteClienteBMFResponse();
            
            try
			{
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;
                               
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_limite_bmf"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroBMF", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.idClienteParametroBMF);
                    lAcessaDados.AddInParameter(lDbCommand, "@idClientePermissao", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.idClientePermissao);
                    lAcessaDados.AddInParameter(lDbCommand, "@account", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.Account);
                    lAcessaDados.AddInParameter(lDbCommand, "@contrato", DbType.AnsiString, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.Contrato);
                    lAcessaDados.AddInParameter(lDbCommand, "@sentido", DbType.AnsiString, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.Sentido);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtTotal", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.QuantidadeTotal);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtDisponivel", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.QuantidadeDisponivel);
                    lAcessaDados.AddInParameter(lDbCommand, "@stRenovacaoAutomatica", DbType.AnsiString, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.RenovacaoAutomatica);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtValidade", DbType.DateTime, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.DataValidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtMaxOferta", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.QuantidadeMaximaOferta);         

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");              

                     DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                     if (null != lDataTable && lDataTable.Rows.Count > 0)
                     {
                         for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                         {
                             resposta.IdClienteParametroBMF = (lDataTable.Rows[i]["retorno"]).DBToInt32();
                         }
                     }

                    logger.Info("Solicitação executada com sucesso.");

                    resposta.bSucesso = true;

                    return resposta;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }

        /// <summary>
        /// Método responsável por atualizar/gravar um limite operacional 
        /// </summary>
        /// <param name="_ClienteOrdemInfo">Atributos do limite operacional</param>
        /// <returns>bool</returns>
        public InserirLimiteClienteBMFResponse AtualizarSpiderLimiteBMF(InserirLimiteClienteBMFRequest InserirLimiteClienteBMFRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            InserirLimiteClienteBMFResponse resposta = new InserirLimiteClienteBMFResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoSpider;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_limite_bmf"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroBMF", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.idClienteParametroBMF);
                    lAcessaDados.AddInParameter(lDbCommand, "@idClientePermissao", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.idClientePermissao);
                    lAcessaDados.AddInParameter(lDbCommand, "@account", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.Account);
                    lAcessaDados.AddInParameter(lDbCommand, "@contrato", DbType.AnsiString, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.Contrato);
                    lAcessaDados.AddInParameter(lDbCommand, "@sentido", DbType.AnsiString, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.Sentido);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtTotal", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.QuantidadeTotal);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtDisponivel", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.QuantidadeDisponivel);
                    lAcessaDados.AddInParameter(lDbCommand, "@stRenovacaoAutomatica", DbType.AnsiString, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.RenovacaoAutomatica);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtValidade", DbType.DateTime, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.DataValidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtMaxOferta", DbType.Int32, InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.QuantidadeMaximaOferta);

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            resposta.IdClienteParametroBMF = (lDataTable.Rows[i]["retorno"]).DBToInt32();
                        }
                    }

                    logger.Info("Solicitação executada com sucesso.");

                    resposta.bSucesso = true;

                    return resposta;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }
        public decimal ObterSaldoProjetadoCustodia(int CodigoCliente,string CodigoAtivo)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoProjetadoCustodia = 0;

            try
            {

                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_saldo_cons_custodia"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "cd_cliente", DbType.Int32, CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "instrumento", DbType.AnsiString, CodigoAtivo); 

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            SaldoProjetadoCustodia = (lDataTable.Rows[i]["TOTAL_CUSTODIA_FISCA"]).DBToDecimal();
                           
                        }
                    }

                    logger.Info("Solicitação executada com sucesso.");

                    return SaldoProjetadoCustodia;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }

        public decimal ObterSaldoProjetadoContaCorrente(int CodigoCliente)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoProjetadoCustodia = 0;

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_saldo_nota"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, CodigoCliente);                    

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            SaldoProjetadoCustodia = (lDataTable.Rows[i]["TOTAL_CC"]).DBToDecimal();


                        }
                    }

                    logger.Info("Solicitação executada com sucesso.");

                    return SaldoProjetadoCustodia;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }

        private int ObterAssessor(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            int idAssessor = 0;

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_repassador"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, IdCliente);

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            idAssessor = (lDataTable.Rows[i]["cd_assessor"]).DBToInt32();


                        }
                    }

                    logger.Info("Solicitação executada com sucesso.");

                    return idAssessor;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao obter o codigo do assessor no banco de dados.");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }



        }

        public string ObterSiglaRepassador(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            string SiglaRepassador = string.Empty;
            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoOMS;

                int IDAssessor = this.ObterAssessor(IdCliente);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_REPASSADORES"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@IDAssessor", DbType.Int32, IDAssessor);

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            SiglaRepassador = (lDataTable.Rows[i]["dsSigla"]).DBToString();

                        }
                    }

                    logger.Info("Solicitação executada com sucesso.");

                    return SiglaRepassador;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }



        }

        public List<int> ObterRelacaoContaMaster()
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            List<int> lstContaMaster = new List<int>();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoOMS;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_contamaster"))
                {

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            int ContaMaster = (lDataTable.Rows[i]["idContaBroker"]).DBToInt32();
                            {
                                if (lstContaMaster.Contains(ContaMaster) == false)
                                {
                                    lstContaMaster.Add(ContaMaster);
                                }
                            }

                        }
                    }

                    logger.Info("Solicitação executada com sucesso.");


                    return lstContaMaster;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }


        public bool ObterEstadoPregaoBovespa()
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            bool EstadoPregao = false;

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoOMS;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_situacao_bolsa"))
                {

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            int vlEstadoPregao = (lDataTable.Rows[i]["vl_situacao"]).DBToInt32();

                            if (vlEstadoPregao == 1)
                            {
                                EstadoPregao = true;
                            }
                            else
                            {
                                EstadoPregao = false;
                            }
                           

                        }
                    }
                    logger.Info("Solicitação executada com sucesso.");


                    return EstadoPregao;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }


        public Dictionary<int, int> ObterDadosContaMaster(int idCliente)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            Dictionary<int, int> DadosContaBroker = new Dictionary<int, int>();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoOMS;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obterdados_contamaster"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, idCliente);

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            int CodigoCliente = (lDataTable.Rows[i]["idCliente"]).DBToInt32();
                            int CodigoContaMaster = (lDataTable.Rows[i]["idContaBroker"]).DBToInt32();

                            DadosContaBroker.Add(CodigoCliente, CodigoContaMaster);

                            break;
                        }
                    }

                    logger.Info("Solicitação executada com sucesso.");


                    return DadosContaBroker;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }

        public List<int> ObterClientesContaMaster(int ClienteContaMaster)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            List<int> listaContaBroker = new List<int>();      

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoOMS;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_filhobroker"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@ContaMaster", DbType.Int32, ClienteContaMaster);

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                           int CodigoBroker = (lDataTable.Rows[i]["idCliente"]).DBToInt32();

                           listaContaBroker.Add(CodigoBroker);
                        }
                    }

                    logger.Info("Solicitação executada com sucesso.");


                    return listaContaBroker;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }


        public int ObterPortaRoteamentoClienteExcecao(int account)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            int Porta = 0;


            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_porta_oms"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@account", DbType.Int32, account);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            Porta = (lDataTable.Rows[i]["port"]).DBToInt32();

                        }
                    }

                }

                return Porta;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao obter a porta do cliente.");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }

        /// <summary>
        /// Método responsável por atualizar/gravar um limite operacional 
        /// </summary>
        /// <param name="_ClienteOrdemInfo">Atributos do limite operacional</param>
        /// <returns>bool</returns>
        public InserirLimiteBMFInstrumentoResponse AtualizarLimiteInstrumentoBMF(InserirLimiteBMFInstrumentoRequest InserirLimiteBMFInstrumentoRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            InserirLimiteBMFInstrumentoResponse resposta = new InserirLimiteBMFInstrumentoResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_limite_bmf_instrumento"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@IdClienteParametroInstrumento", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.IdClienteParametroInstrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroBMF", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.IdClienteParametroBMF);
                    lAcessaDados.AddInParameter(lDbCommand, "@instrumento", DbType.AnsiString, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.Instrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtTotalContratoPai", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QtTotalContratoPai);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtTotalInstrumento", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QtTotalInstrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtDisponivel", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QtDisponivel);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtMaxOferta", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QuantidadeMaximaOferta);
                    lAcessaDados.AddInParameter(lDbCommand, "@sentido", DbType.AnsiString, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.Sentido);
                    
                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("Solicitação executada com sucesso.");

                    resposta.bSucesso = true;

                    return resposta;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao gravar o limite do instrumento no banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }

        /// <summary>
        /// Método responsável por atualizar/gravar um limite operacional 
        /// </summary>
        /// <param name="_ClienteOrdemInfo">Atributos do limite operacional</param>
        /// <returns>bool</returns>
        public InserirLimiteBMFInstrumentoResponse AtualizarSpiderLimiteInstrumentoBMF(InserirLimiteBMFInstrumentoRequest InserirLimiteBMFInstrumentoRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            InserirLimiteBMFInstrumentoResponse resposta = new InserirLimiteBMFInstrumentoResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoSpider;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_limite_bmf_instrumento"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@IdClienteParametroInstrumento", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.IdClienteParametroInstrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroBMF", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.IdClienteParametroBMF);
                    lAcessaDados.AddInParameter(lDbCommand, "@instrumento", DbType.AnsiString, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.Instrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtTotalContratoPai", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QtTotalContratoPai);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtTotalInstrumento", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QtTotalInstrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtDisponivel", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QtDisponivel);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtMaxOferta", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QuantidadeMaximaOferta);
                    lAcessaDados.AddInParameter(lDbCommand, "@sentido", DbType.AnsiString, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.Sentido);

                    logger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("Solicitação executada com sucesso.");

                    resposta.bSucesso = true;

                    return resposta;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao gravar o limite do instrumento no banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }
        }

        public EnviarClienteFatFingerResponse ObterRegrasFatFingerCliente(EnviarClienteFatFingerRequest pParametroFatFingerRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            ClienteFatFingerInfo ClienteFatFingerInfo = new ClienteFatFingerInfo();
            EnviarClienteFatFingerResponse EnviarClienteFatFingerResponse = new EnviarClienteFatFingerResponse();


            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_fatfinger"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametroFatFingerRequest.CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ClienteFatFingerInfo = new ClienteFatFingerInfo();

                            ClienteFatFingerInfo.account = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            ClienteFatFingerInfo.Mercado = "BOVESPA";
                            ClienteFatFingerInfo.valorRegra = (lDataTable.Rows[i]["vl_maximo"]).DBToDecimal();                           
                            ClienteFatFingerInfo.dtValidadeRegra = (lDataTable.Rows[i]["dt_vencimento"]).DBToDateTime();

                            EnviarClienteFatFingerResponse.lsConfiguracaoFatFinger.Add(ClienteFatFingerInfo);

                        }
                    }

                }

                return EnviarClienteFatFingerResponse;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao obter os limites de exposicao global do cliente");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }
        }

        /// <summary>
        /// Método responsável por obter os parametros globais de exposicao de risco
        /// </summary>
        /// <returns>InserirLimiteBMFInstrumentoResponse</returns>
        public ParametroExposicaoResponse ObterParametrosGlobaisExposicaoRisco(ParametroExposicaoRequest ParametroExposicaoRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            ParametroExposicaoResponse ParametroExposicaoResponse = new ParametroExposicaoResponse();
            ParametroExposicaoInfo ParametroExposicaoInfo = new ParametroExposicaoInfo();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_parametro_exposicao"))
                {

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ParametroExposicaoInfo.OscilacaoMaxima = (lDataTable.Rows[i]["perc_oscilacao"]).DBToDecimal();
                            ParametroExposicaoInfo.PrejuizoMaximo = (lDataTable.Rows[i]["prejuizo_maximo"]).DBToDecimal();
                            ParametroExposicaoInfo.stAtivo = (lDataTable.Rows[i]["st_ativo"]).DBToChar();

                            ParametroExposicaoResponse.ParametroExposicaoInfo = ParametroExposicaoInfo;
                        }
                    }

                }

                return ParametroExposicaoResponse;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao obter os limites de exposicao global do cliente");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }



        public OrdemInfo ObterDadosOrdemOriginal(string OrderID)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            OrdemInfo OrdemInfo = new OrdemInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_ordem_cliente"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@OrderID", DbType.String, OrderID);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        OrdemInfo.Account = (lDataTable.Rows[0]["Account"]).DBToInt32();
                        OrdemInfo.ChannelID = (lDataTable.Rows[0]["ChannelID"]).DBToInt32();
                        OrdemInfo.ClOrdID = (lDataTable.Rows[0]["ClOrdID"]).DBToString();
                        OrdemInfo.CumQty = (lDataTable.Rows[0]["CumQty"]).DBToInt32();
                        OrdemInfo.ExchangeNumberID = (lDataTable.Rows[0]["ExchangeNumberID"]).DBToString();
                        OrdemInfo.ExecBroker = (lDataTable.Rows[0]["ExecBroker"]).DBToString();
                        OrdemInfo.ExpireDate = (lDataTable.Rows[0]["ExpireDate"]).DBToDateTime();
                        OrdemInfo.IdOrdem = (lDataTable.Rows[0]["OrderID"]).DBToInt32();
                        OrdemInfo.MaxFloor = (lDataTable.Rows[0]["MaxFloor"]).DBToDouble();
                        OrdemInfo.MinQty = (lDataTable.Rows[0]["MinQty"]).DBToDouble();
                        OrdemInfo.OrderQty = (lDataTable.Rows[0]["OrderQty"]).DBToInt32();
                        OrdemInfo.OrderQtyRemmaining = (lDataTable.Rows[0]["OrderQtyRemaining"]).DBToInt32();
                        OrdemInfo.OrdStatus = (OrdemStatusEnum)int.Parse((lDataTable.Rows[0]["OrdStatusID"]).DBToString());
                        OrdemInfo.OrdType = (OrdemTipoEnum)int.Parse((lDataTable.Rows[0]["OrdTypeID"]).DBToString());
                        OrdemInfo.OrigClOrdID = (lDataTable.Rows[0]["OrigClOrdID"]).DBToString();
                        OrdemInfo.Price = (lDataTable.Rows[0]["Price"]).DBToDouble();
                        OrdemInfo.RegisterTime = (lDataTable.Rows[0]["RegisterTime"]).DBToDateTime();
                        OrdemInfo.SecurityExchangeID = (lDataTable.Rows[0]["SecurityExchangeID"]).DBToString();
                        OrdemInfo.Side = (OrdemDirecaoEnum)int.Parse((lDataTable.Rows[0]["Side"].DBToString()));
                        OrdemInfo.StopStartID = (lDataTable.Rows[0]["StopStartID"]).DBToInt32();
                        OrdemInfo.Symbol = (lDataTable.Rows[0]["Symbol"]).DBToString();
                        OrdemInfo.TimeInForce = (Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum)int.Parse((lDataTable.Rows[0]["TimeInForce"].DBToString()));
                        OrdemInfo.TransactTime = (lDataTable.Rows[0]["TransactTime"]).DBToDateTime();
                        OrdemInfo.ClOrdID = (lDataTable.Rows[0]["ClOrdID"]).DBToString();

                    }

                }

                return OrdemInfo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public string ObterCodigoBMF(string CodigoBovespa)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            string CodigoBMF = "";

            try
            {
                lAcessaDados.ConnectionStringName = gConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_cli_bmf"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodigoBovespa", DbType.String, CodigoBovespa);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        CodigoBMF = (lDataTable.Rows[0]["codcli"]).DBToString();                       

                    }

                }

                return CodigoBMF;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        /// <summary>
        /// Método responsável por obter os parametros globais de exposicao de risco
        /// </summary>
        /// <returns>InserirLimiteBMFInstrumentoResponse</returns>
        public ZerarPosicaoResponse ZerarPosicaoClientePapel(ZerarPosicaoRequest ZerarPosicaoRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            ZerarPosicaoResponse ZerarPosicaoResponse = new ZerarPosicaoResponse();
            ZerarPosicaoInfo ZerarPosicaoInfo = new ZerarPosicaoInfo();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_calcula_qtde_exec"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@account", DbType.Int32, ZerarPosicaoRequest.ZerarPosicaoInfo.Account);
                    lAcessaDados.AddInParameter(lDbCommand, "@ativo", DbType.AnsiString, ZerarPosicaoRequest.ZerarPosicaoInfo.Instrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ZerarPosicaoInfo = new Lib.ZerarPosicaoInfo();
                            ZerarPosicaoInfo.Account = ZerarPosicaoRequest.ZerarPosicaoInfo.Account;
                            ZerarPosicaoInfo.Instrumento = (lDataTable.Rows[i]["symbol"]).DBToString();
                            ZerarPosicaoInfo.Quantidade = (lDataTable.Rows[i]["qtde"]).DBToInt32();

                            int Sentido = (lDataTable.Rows[i]["side"]).DBToInt32();

                            if (Sentido == 1)
                            {
                                ZerarPosicaoInfo.Sentido = "C";
                            }
                            else
                            {
                                ZerarPosicaoInfo.Sentido = "V";
                            }
               

                            ZerarPosicaoResponse.lstZerarPosicaoInfo.Add(ZerarPosicaoInfo);
                        }
                    }

                }

                return ZerarPosicaoResponse;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao obter os limites de exposicao global do cliente");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }


        /// <summary>
        /// Método responsavel por carregar os instrumentos bloqueados do cliente.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InstrumentoBloqueadoResponse CarregarInstrumentosBloqueadosCliente(InstrumentoBloqueadoRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            InstrumentoBloqueadoResponse InstrumentosBloqueio = new InstrumentoBloqueadoResponse();
            InstrumentoBloqueioInfo      InstrumentoBloqueioInfo = new InstrumentoBloqueioInfo();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoProducao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_bloqueio_instrumento"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, request.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            InstrumentoBloqueioInfo = new Lib.InstrumentoBloqueioInfo();

                            InstrumentoBloqueioInfo.IdCliente   = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            InstrumentoBloqueioInfo.Instrumento = (lDataTable.Rows[i]["cd_ativo"]).DBToString();
                            char Sentido = (lDataTable.Rows[i]["Direcao"].DBToChar());

                            if (Sentido == 'C')
                            {
                                InstrumentoBloqueioInfo.Sentido = SentidoBloqueioEnum.Compra;
                            }
                            else
                                if (Sentido == 'V')
                                {
                                    InstrumentoBloqueioInfo.Sentido = SentidoBloqueioEnum.Venda;
                                }
                                else
                                {
                                    InstrumentoBloqueioInfo.Sentido = SentidoBloqueioEnum.Ambos;
                                }

                            InstrumentosBloqueio.ListaInstrumentoBloqueio.Add(InstrumentoBloqueioInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO CARREGAR O DO INSTRUMENTO BLOQUEADO.");
                throw (ex);
            }

            return InstrumentosBloqueio;

        }

        /// <summary>
        /// METODO RESPONSAVEL POR VALIDAR SE UMA DETERMINADA SERIE DE OPCAO ESTA LIBERADA PARA OPERACAP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidarSerieOpcaoResponse ValidarSerieOpcao(ValidarSerieOpcaoRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            ValidarSerieOpcaoResponse ValidarSerieOpcaoResponse = new ValidarSerieOpcaoResponse();       

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_bloqueio_opcao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@Serie", DbType.AnsiString, request.SerieOpcao);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DateTime dataBloqueio = (lDataTable.Rows[i]["dt_bloqueio"]).DBToDateTime();

                            if (DateTime.Now >= dataBloqueio)
                            {
                                ValidarSerieOpcaoResponse.PermissaoOpcaoEnum = PermissaoOpcaoEnum.NAOPERMITIDA;
                            }
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO OBTER O VENCIMENTO DA SERIE DA OPCAO.");
                throw (ex);
            }

            return ValidarSerieOpcaoResponse;
        }

        public CalcularExposicaoMaximaResponse ObterExposicaoRiscoCliente(CalcularExposicaoMaximaRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            
            CalcularExposicaoMaximaResponse CalcularExposicaoMaximaResponse = new CalcularExposicaoMaximaResponse();
            ClienteExposicaoInfo ClienteExposicaoInfo = new ClienteExposicaoInfo();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_exposicao_risco"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.AnsiString, request.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtMovimento", DbType.DateTime, request.dtMovimento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ClienteExposicaoInfo.IdCliente         = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            ClienteExposicaoInfo.LucroPrejuizo     = (lDataTable.Rows[i]["vl_lucro_prejuizo"]).DBToDecimal();
                            ClienteExposicaoInfo.PatrimonioLiquido = (lDataTable.Rows[i]["vl_patrimonio_liquido"]).DBToDecimal();
                            ClienteExposicaoInfo.DataAtualizacao   = (lDataTable.Rows[i]["dt_atualizacao"]).DBToDateTime();

                            if (ClienteExposicaoInfo.LucroPrejuizo >= 0)
                            {
                                ClienteExposicaoInfo.LucroPrejuizo = 0;
                            }
                            else
                            {
                                ClienteExposicaoInfo.LucroPrejuizo = ClienteExposicaoInfo.LucroPrejuizo * -1;
                            }

                            CalcularExposicaoMaximaResponse.ClienteExposicaoInfo = ClienteExposicaoInfo;
                            CalcularExposicaoMaximaResponse.PosicaoEncontrada = true;
                        }
                    }
                    else
                    {
                        CalcularExposicaoMaximaResponse.PosicaoEncontrada = false;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO OBTER O VENCIMENTO DA SERIE DA OPCAO.");
                throw (ex);
            }

            return CalcularExposicaoMaximaResponse;
        }


        /// <summary>
        /// Método responsavel por carregar os instrumentos bloqueados do cliente.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InstrumentoBloqueadoResponse CarregarInstrumentosBloqueadosGlobal(InstrumentoBloqueadoRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            InstrumentoBloqueadoResponse InstrumentosBloqueio = new InstrumentoBloqueadoResponse();
            InstrumentoBloqueioInfo InstrumentoBloqueioInfo = new InstrumentoBloqueioInfo();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoProducao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_verificar_permissao_ativo"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idAtivo", DbType.AnsiString, request.Instrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            InstrumentoBloqueioInfo = new Lib.InstrumentoBloqueioInfo();
                
                            InstrumentoBloqueioInfo.Instrumento = (lDataTable.Rows[i]["ds_item"]).DBToString();
                            char Sentido = (lDataTable.Rows[i]["sentido"].DBToChar());

                            if (Sentido == 'C')
                            {
                                InstrumentoBloqueioInfo.Sentido = SentidoBloqueioEnum.Compra;
                            }
                            else
                                if (Sentido == 'V')
                                {
                                    InstrumentoBloqueioInfo.Sentido = SentidoBloqueioEnum.Venda;
                                }
                                else
                                {
                                    InstrumentoBloqueioInfo.Sentido = SentidoBloqueioEnum.Ambos;
                                }

                            InstrumentosBloqueio.ListaInstrumentoBloqueio.Add(InstrumentoBloqueioInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO CARREGAR OS INSTRUMENTO BLOQUEADO ( GERAL ).");
                throw (ex);
            }

            return InstrumentosBloqueio;

        }


        /// <summary>
        /// metodo responsavel por verificar se um ativo especifico esta bloqueado dentro de um grupo especifico.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InstrumentoBloqueadoResponse CarregarInstrumentosBloqueadosGrupoCliente(InstrumentoBloqueadoRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            InstrumentoBloqueadoResponse InstrumentosBloqueio = new InstrumentoBloqueadoResponse();
            InstrumentoBloqueioInfo InstrumentoBloqueioInfo = new InstrumentoBloqueioInfo();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoProducao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CLIENTE_BLOQ_GRUPO"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, request.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ativo", DbType.AnsiString, request.Instrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            InstrumentoBloqueioInfo = new Lib.InstrumentoBloqueioInfo();

                            InstrumentoBloqueioInfo.Instrumento = (lDataTable.Rows[i]["ds_item"]).DBToString();
                            char Sentido = (lDataTable.Rows[i]["direcao"].DBToChar());

                            if (Sentido == 'C')
                            {
                                InstrumentoBloqueioInfo.Sentido = SentidoBloqueioEnum.Compra;
                            }
                            else
                                if (Sentido == 'V')
                                {
                                    InstrumentoBloqueioInfo.Sentido = SentidoBloqueioEnum.Venda;
                                }
                                else
                                {
                                    InstrumentoBloqueioInfo.Sentido = SentidoBloqueioEnum.Ambos;
                                }

                            InstrumentosBloqueio.ListaInstrumentoBloqueio.Add(InstrumentoBloqueioInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO CARREGAR OS INSTRUMENTO BLOQUEADO ( GERAL ).");
                throw (ex);
            }

            return InstrumentosBloqueio;

        }

        /// <summary>
        /// Atualização de limite de BMF
        /// </summary>
        /// <param name="AtualizarLimitesBMFRequest"></param>
        /// <returns></returns>
        public AtualizarLimitesBMFResponse AtualizaPosicaoLimiteBMF(AtualizarLimitesBMFRequest AtualizarLimitesBMFRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            AtualizarLimitesBMFResponse AtualizaPosicaoLimiteBMF = new AtualizarLimitesBMFResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                //Cria DBComand atribuindo a storedprocedure 
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_atualiza_contratos"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@account", DbType.Int32, AtualizarLimitesBMFRequest.account);
                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroBMF", DbType.Int32, AtualizarLimitesBMFRequest.idClienteParametroBMF);
                    lAcessaDados.AddInParameter(lDbCommand, "@instrumento", DbType.AnsiString, AtualizarLimitesBMFRequest.instrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@quantidadeSolicitada", DbType.Int32, AtualizarLimitesBMFRequest.quantidadeSolicitada);
                    lAcessaDados.AddInParameter(lDbCommand, "@stUtilizaInstrumento", DbType.AnsiString, AtualizarLimitesBMFRequest.stUtilizaInstrumento);

                    // Executa s Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("Ordem inserida no banco de dados com sucesso");

                    AtualizaPosicaoLimiteBMF.bSucesso = true;
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao atualizar o parametro de BMF no banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                AtualizaPosicaoLimiteBMF.bSucesso = false;

                throw (ex);
            }

            return null;
        }

        /// <summary>
        /// Método responsavel por armazenar a ordem original em caso de alteracao de ordens
        /// </summary>
        /// <param name="_ClienteOrdemInfo"></param>
        /// <returns></returns>
        public bool InserirOrdemBackup(OrdemInfo _ClienteOrdemInfo)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                logger.Info("PREENCHE PARAMETROS PARA SALVAR A ORDEM BKP REFERENTE A ALTERACAO DE ORDENS");

                // ATP
                // Nao eh o melhor lugar pra fazer esse acerto
                if (_ClienteOrdemInfo.TimeInForce == Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada)
                {
                    _ClienteOrdemInfo.ExpireDate = new DateTime(9999,12,31,23,59,59);
                }

                //Cria DBComand atribuindo a storedprocedure 
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_order_updated"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@OrderID", DbType.Int32, _ClienteOrdemInfo.IdOrdem);
                    lAcessaDados.AddInParameter(lDbCommand, "@account", DbType.Int32, _ClienteOrdemInfo.Account);
                    lAcessaDados.AddInParameter(lDbCommand, "@instrumento", DbType.AnsiString, _ClienteOrdemInfo.Symbol);
                    lAcessaDados.AddInParameter(lDbCommand, "@CLOrdID", DbType.String, _ClienteOrdemInfo.ClOrdID);
                    lAcessaDados.AddInParameter(lDbCommand, "@OrdStatusID", DbType.String, (int)_ClienteOrdemInfo.OrdStatus);
                    lAcessaDados.AddInParameter(lDbCommand, "@price", DbType.Decimal, _ClienteOrdemInfo.Price);
                    lAcessaDados.AddInParameter(lDbCommand, "@quantidade", DbType.Int32, _ClienteOrdemInfo.OrderQty);
                    lAcessaDados.AddInParameter(lDbCommand, "@quantidade_exec", DbType.Int32, _ClienteOrdemInfo.CumQty);
                    lAcessaDados.AddInParameter(lDbCommand, "@quantidade_aparente", DbType.Int32, _ClienteOrdemInfo.MaxFloor);
                    lAcessaDados.AddInParameter(lDbCommand, "@quantidade_minima", DbType.Int32, _ClienteOrdemInfo.MinQty);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_validade", DbType.DateTime, _ClienteOrdemInfo.ExpireDate);
                    lAcessaDados.AddInParameter(lDbCommand, "@TimeInForce", DbType.Int32, (int)_ClienteOrdemInfo.TimeInForce);

                    logger.Info("ENVIA A SOLICITAÇAO PARA O BANCO DE DADOS");

                    // Executa s Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("ORDEM INSERIDA COM SUCESSO.");

                    return true;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar alteracao ordem para o banco de dados");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }



        /// <summary>
        /// Método responsavel por armazenar a ordem original em caso de alteracao de ordens
        /// </summary>
        /// <param name="_ClienteOrdemInfo"></param>
        /// <returns></returns>
        public InativarLimiteContratoResponse InativarLimiteCliente(InativarLimiteContratoRequest InativarLimiteContratoRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            InativarLimiteContratoResponse InativarLimiteContratoResponse = new InativarLimiteContratoResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                //Cria DBComand atribuindo a storedprocedure 
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_inativar_limite"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroBMF", DbType.Int32, InativarLimiteContratoRequest.IdClienteParametroBMF);                         
           

                    // Executa s Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("Contrato inativado com sucesso.");

                    InativarLimiteContratoResponse.bSucesso = true;                    

                }
            }
            catch (Exception ex)
            {
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

            return InativarLimiteContratoResponse;

        }

        /// <summary>
        /// Método responsavel por armazenar a ordem original em caso de alteracao de ordens
        /// </summary>
        /// <param name="_ClienteOrdemInfo"></param>
        /// <returns></returns>
        public InativarLimiteContratoResponse InativarSpiderLimiteCliente(InativarLimiteContratoRequest InativarLimiteContratoRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            InativarLimiteContratoResponse InativarLimiteContratoResponse = new InativarLimiteContratoResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoSpider;

                //Cria DBComand atribuindo a storedprocedure 
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_inativar_limite"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroBMF", DbType.Int32, InativarLimiteContratoRequest.IdClienteParametroBMF);


                    // Executa s Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("Contrato inativado com sucesso.");

                    InativarLimiteContratoResponse.bSucesso = true;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

            return InativarLimiteContratoResponse;

        }
        public ListarLimiteBMFResponse ObterLimiteBMFCliente(ListarLimiteBMFRequest lRquest)
        {
            List<ClienteParametroBMFInstrumentoInfo> lstInstrumento = new List<ClienteParametroBMFInstrumentoInfo>();

            AcessaDados lAcessaDados = new AcessaDados();
            ListarLimiteBMFResponse response = new ListarLimiteBMFResponse();
            ClienteParametroLimiteBMFInfo ClienteParametroLimiteBMFInfo = new ClienteParametroLimiteBMFInfo();

            try
            {
                var conn = lAcessaDados.Conexao.AbrirConexao(gNomeConexaoHomo);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)(conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_sel_limites_bmf";

                SqlParameter param = new SqlParameter();
                param.Value = lRquest.Account;
                param.DbType = DbType.Int32;
                param.ParameterName = "@account";

                cmd.Parameters.Add(param);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet dsLimites = new DataSet();
                da.Fill(dsLimites);

                if (dsLimites.Tables.Count > 0)
                {
                    DataTable tableLimite = dsLimites.Tables[0];
                    DataTable tableLimiteInstrumento = dsLimites.Tables[1];

                    if (tableLimite.Rows.Count > 0)
                    {

                        for (int i = 0; i <= tableLimite.Rows.Count - 1; i++)
                        {
                            ClienteParametroLimiteBMFInfo = new ClienteParametroLimiteBMFInfo();

                            ClienteParametroLimiteBMFInfo.Account = (tableLimite.Rows[i]["account"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.idClienteParametroBMF = (tableLimite.Rows[i]["idClienteParametroBMF"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.Contrato = (tableLimite.Rows[i]["Contrato"]).DBToString();
                            ClienteParametroLimiteBMFInfo.Sentido = (tableLimite.Rows[i]["Sentido"]).DBToString();
                            ClienteParametroLimiteBMFInfo.QuantidadeTotal = (tableLimite.Rows[i]["qtTotal"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.QuantidadeDisponivel = (tableLimite.Rows[i]["qtDisponivel"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.DataValidade = (tableLimite.Rows[i]["dtValidade"]).DBToDateTime();
                            ClienteParametroLimiteBMFInfo.DataMovimento = (tableLimite.Rows[i]["dtMovimento"]).DBToDateTime();
                            ClienteParametroLimiteBMFInfo.QuantidadeMaximaOferta = (tableLimite.Rows[i]["qtMaxOferta"]).DBToInt32();                        

                            response.ListaLimites.Add(ClienteParametroLimiteBMFInfo);
                        }
                    }

                    if (tableLimiteInstrumento.Rows.Count > 0)
                    {

                        for (int i = 0; i <= tableLimiteInstrumento.Rows.Count - 1; i++)
                        {
                            ClienteParametroBMFInstrumentoInfo ClienteParametroBMFInstrumentoInfo = new ClienteParametroBMFInstrumentoInfo();

                            ClienteParametroBMFInstrumentoInfo.IdClienteParametroBMF = (tableLimiteInstrumento.Rows[i]["IdClienteParametroBMF"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.IdClienteParametroInstrumento = (tableLimiteInstrumento.Rows[i]["IdClienteParametroInstrumento"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.Instrumento = (tableLimiteInstrumento.Rows[i]["Instrumento"]).DBToString();
                            ClienteParametroBMFInstrumentoInfo.dtMovimento = (tableLimiteInstrumento.Rows[i]["dtMovimento"]).DBToDateTime();
                            ClienteParametroBMFInstrumentoInfo.QtTotalContratoPai = (tableLimiteInstrumento.Rows[i]["QtTotalContratoPai"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.QtTotalInstrumento = (tableLimiteInstrumento.Rows[i]["QtTotalInstrumento"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.QtDisponivel = (tableLimiteInstrumento.Rows[i]["QtDisponivel"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.ContratoBase = (tableLimiteInstrumento.Rows[i]["contrato"]).DBToString();
                            ClienteParametroBMFInstrumentoInfo.QuantidadeMaximaOferta = (tableLimiteInstrumento.Rows[i]["qtMaxOferta"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.Sentido = (tableLimiteInstrumento.Rows[i]["sentido"]).DBToChar();

                            response.ListaLimitesInstrumentos.Add(ClienteParametroBMFInstrumentoInfo);

                        }

                    }

                }

                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ListarLimiteBMFResponse ObterSpiderLimiteBMFCliente(ListarLimiteBMFRequest lRquest)
        {
            List<ClienteParametroBMFInstrumentoInfo> lstInstrumento = new List<ClienteParametroBMFInstrumentoInfo>();

            AcessaDados lAcessaDados = new AcessaDados();
            ListarLimiteBMFResponse response = new ListarLimiteBMFResponse();
            ClienteParametroLimiteBMFInfo ClienteParametroLimiteBMFInfo = new ClienteParametroLimiteBMFInfo();

            try
            {
                var conn = lAcessaDados.Conexao.AbrirConexao(gNomeConexaoSpider);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)(conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_sel_limites_bmf";

                SqlParameter param = new SqlParameter();
                param.Value = lRquest.Account;
                param.DbType = DbType.Int32;
                param.ParameterName = "@account";

                cmd.Parameters.Add(param);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet dsLimites = new DataSet();
                da.Fill(dsLimites);

                if (dsLimites.Tables.Count > 0)
                {
                    DataTable tableLimite = dsLimites.Tables[0];
                    DataTable tableLimiteInstrumento = dsLimites.Tables[1];

                    if (tableLimite.Rows.Count > 0)
                    {

                        for (int i = 0; i <= tableLimite.Rows.Count - 1; i++)
                        {
                            ClienteParametroLimiteBMFInfo = new ClienteParametroLimiteBMFInfo();

                            ClienteParametroLimiteBMFInfo.Account = (tableLimite.Rows[i]["account"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.idClienteParametroBMF = (tableLimite.Rows[i]["idClienteParametroBMF"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.Contrato = (tableLimite.Rows[i]["Contrato"]).DBToString();
                            ClienteParametroLimiteBMFInfo.Sentido = (tableLimite.Rows[i]["Sentido"]).DBToString();
                            ClienteParametroLimiteBMFInfo.QuantidadeTotal = (tableLimite.Rows[i]["qtTotal"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.QuantidadeDisponivel = (tableLimite.Rows[i]["qtDisponivel"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.DataValidade = (tableLimite.Rows[i]["dtValidade"]).DBToDateTime();
                            ClienteParametroLimiteBMFInfo.DataMovimento = (tableLimite.Rows[i]["dtMovimento"]).DBToDateTime();
                            ClienteParametroLimiteBMFInfo.QuantidadeMaximaOferta = (tableLimite.Rows[i]["qtMaxOferta"]).DBToInt32();

                            response.ListaLimites.Add(ClienteParametroLimiteBMFInfo);
                        }
                    }

                    if (tableLimiteInstrumento.Rows.Count > 0)
                    {

                        for (int i = 0; i <= tableLimiteInstrumento.Rows.Count - 1; i++)
                        {
                            ClienteParametroBMFInstrumentoInfo ClienteParametroBMFInstrumentoInfo = new ClienteParametroBMFInstrumentoInfo();

                            ClienteParametroBMFInstrumentoInfo.IdClienteParametroBMF = (tableLimiteInstrumento.Rows[i]["IdClienteParametroBMF"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.IdClienteParametroInstrumento = (tableLimiteInstrumento.Rows[i]["IdClienteParametroInstrumento"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.Instrumento = (tableLimiteInstrumento.Rows[i]["Instrumento"]).DBToString();
                            ClienteParametroBMFInstrumentoInfo.dtMovimento = (tableLimiteInstrumento.Rows[i]["dtMovimento"]).DBToDateTime();
                            ClienteParametroBMFInstrumentoInfo.QtTotalContratoPai = (tableLimiteInstrumento.Rows[i]["QtTotalContratoPai"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.QtTotalInstrumento = (tableLimiteInstrumento.Rows[i]["QtTotalInstrumento"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.QtDisponivel = (tableLimiteInstrumento.Rows[i]["QtDisponivel"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.ContratoBase = (tableLimiteInstrumento.Rows[i]["contrato"]).DBToString();
                            ClienteParametroBMFInstrumentoInfo.QuantidadeMaximaOferta = (tableLimiteInstrumento.Rows[i]["qtMaxOferta"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.Sentido = (tableLimiteInstrumento.Rows[i]["sentido"]).DBToChar();

                            response.ListaLimitesInstrumentos.Add(ClienteParametroBMFInstrumentoInfo);

                        }

                    }

                }

                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        /// <summary>
        /// Seleciona os atributos de uma determinada ordem.
        /// </summary>
        /// <param name="OrderID">Código da ordem</param>
        /// <returns></returns>
        public EnviarInformacoesOrdemResponse ObterInformacoesOrdem(EnviarInformacoesOrdemRequest lRquest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            EnviarInformacoesOrdemResponse _response = new EnviarInformacoesOrdemResponse();
            OrdemInfo OrdemInfo = new OrdemInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_ordem_cliente"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@OrderID", DbType.String, lRquest.NumeroControleOrdem);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        OrdemInfo.Account = (lDataTable.Rows[0]["Account"]).DBToInt32();
                        OrdemInfo.ChannelID = (lDataTable.Rows[0]["ChannelID"]).DBToInt32();
                        OrdemInfo.ClOrdID = (lDataTable.Rows[0]["ClOrdID"]).DBToString();
                        OrdemInfo.CumQty = (lDataTable.Rows[0]["CumQty"]).DBToInt32();
                        OrdemInfo.ExchangeNumberID = (lDataTable.Rows[0]["ExchangeNumberID"]).DBToString();
                        OrdemInfo.ExecBroker = (lDataTable.Rows[0]["ExecBroker"]).DBToString();
                        OrdemInfo.ExpireDate = (lDataTable.Rows[0]["ExpireDate"]).DBToDateTime();
                        OrdemInfo.IdOrdem = (lDataTable.Rows[0]["OrderID"]).DBToInt32();
                        OrdemInfo.MaxFloor = (lDataTable.Rows[0]["MaxFloor"]).DBToDouble();
                        OrdemInfo.MinQty = (lDataTable.Rows[0]["MinQty"]).DBToDouble();
                        OrdemInfo.OrderQty = (lDataTable.Rows[0]["OrderQty"]).DBToInt32();
                        OrdemInfo.OrderQtyRemmaining = (lDataTable.Rows[0]["OrderQtyRemaining"]).DBToInt32();
                        OrdemInfo.OrdStatus = (OrdemStatusEnum)int.Parse((lDataTable.Rows[0]["OrdStatusID"]).DBToString());
                        OrdemInfo.OrdType = (OrdemTipoEnum)int.Parse((lDataTable.Rows[0]["OrdTypeID"]).DBToString());
                        OrdemInfo.OrigClOrdID = (lDataTable.Rows[0]["OrigClOrdID"]).DBToString();

                        int OrderTypeId = (lDataTable.Rows[0]["OrdTypeID"]).DBToInt32();

                        if (OrderTypeId == 52)
                        {
                            OrdemInfo.StopPrice = (lDataTable.Rows[0]["Price"]).DBToDouble();
                        }

                 
                        OrdemInfo.Price = (lDataTable.Rows[0]["Price"]).DBToDouble();
                        OrdemInfo.RegisterTime = (lDataTable.Rows[0]["RegisterTime"]).DBToDateTime();
                        OrdemInfo.SecurityExchangeID = (lDataTable.Rows[0]["SecurityExchangeID"]).DBToString();
                        OrdemInfo.Side = (OrdemDirecaoEnum)int.Parse((lDataTable.Rows[0]["Side"].DBToString()));
                        OrdemInfo.StopStartID = (lDataTable.Rows[0]["StopStartID"]).DBToInt32();
                        OrdemInfo.Symbol = (lDataTable.Rows[0]["Symbol"]).DBToString();
                        OrdemInfo.TimeInForce = ( Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum)int.Parse((lDataTable.Rows[0]["TimeInForce"].DBToString()));
                        OrdemInfo.TransactTime = (lDataTable.Rows[0]["TransactTime"]).DBToDateTime();
                        OrdemInfo.ClOrdID = (lDataTable.Rows[0]["ClOrdID"]).DBToString();

                        _response.OrdemInfo = OrdemInfo;

                    }

                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_preco_ordem_exec"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CLOrdID", DbType.String, _response.OrdemInfo.ClOrdID);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {                        
                        _response.OrdemInfo.Price = (lDataTable.Rows[0]["price"]).DBToDouble();
                        _response.OrdemInfo.CumQty = (lDataTable.Rows[0]["CumQty"]).DBToInt32();                              
                    }

                }


                return _response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        /// <summary>
        /// Seleciona os atributos de uma determinada ordem.
        /// </summary>
        /// <param name="OrderID">Código da ordem</param>
        /// <returns></returns>
        public EnviarInformacoesOrdemResponse ObterInformacoesOrdemRecalculo(EnviarInformacoesOrdemRequest lRquest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            EnviarInformacoesOrdemResponse _response = new EnviarInformacoesOrdemResponse();
            OrdemInfo OrdemInfo = new OrdemInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_ordem_cliente_recalculo"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@OrderID", DbType.String, lRquest.NumeroControleOrdem);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        OrdemInfo.Account = (lDataTable.Rows[0]["Account"]).DBToInt32();                   
                        OrdemInfo.ClOrdID = (lDataTable.Rows[0]["ClOrdID"]).DBToString();                                                                           
                        OrdemInfo.OrderQty = (lDataTable.Rows[0]["quantidade"]).DBToInt32();                                    
                        OrdemInfo.Price = (lDataTable.Rows[0]["Price"]).DBToDouble();                                                            
                        OrdemInfo.Symbol = (lDataTable.Rows[0]["INSTRUMENTO"]).DBToString();           
           
                        _response.OrdemInfo = OrdemInfo;

                    }

                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_preco_ordem_exec"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CLOrdID", DbType.String, _response.OrdemInfo.ClOrdID);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        _response.OrdemInfo.Price = (lDataTable.Rows[0]["price"]).DBToDouble();
                        _response.OrdemInfo.CumQty = (lDataTable.Rows[0]["CumQty"]).DBToInt32();
                    }

                }


                return _response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }



        public SecurityIDResponse ObterSecurityID(SecurityIDRequest pRequest) {

            AcessaDados lAcessaDados = new AcessaDados();
            SecurityIDResponse lResponse = new SecurityIDResponse();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_securityList"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@instrumento", DbType.String, pRequest.Instrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lResponse.Instrumento = pRequest.Instrumento;
                        lResponse.SecurityID = lDataTable.Rows[0]["SecurityID"].ToString();
                    }
                    else
                    {
                        logger.Info("SECURITYID NAO LOCALIZADO PARA O INSTRUMENTO: " + pRequest.Instrumento);
                    }

                }

                return lResponse;
            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO CARREGAR O SECURITYID DO INSTRUMENTO SOLICITADO: " + pRequest.Instrumento);
                throw (ex);
            }
        }

        /// <summary>
        /// Metodo responsavel por obter informacoes referentes ao cadasto de papeis.
        /// </summary>
        /// <param name="request">instrumento.</param>
        /// <returns>informacoes do instrumento.</returns>
        public EnviarCadastroPapelResponse ObterCadastroPapel(EnviarCadastroPapelRequest request)
        {
            EnviarCadastroPapelResponse _response = new EnviarCadastroPapelResponse();
            CadastroPapelInfo _CadastroPapelInfo = new CadastroPapelInfo();

            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoOMS;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cadastropapel_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@instrumento", DbType.String, request.Instrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        _CadastroPapelInfo.Instrumento  = (lDataTable.Rows[0]["codigoInstrumento"]).DBToString();
                        _CadastroPapelInfo.FormaCotacao = (lDataTable.Rows[0]["FormaCotacao"]).DBToInt32();
                        _CadastroPapelInfo.LotePadrao   = (lDataTable.Rows[0]["LotePadrao"]).DBToInt32();

                        #region SegmentoMercado

                        string SegmentoMercado = (lDataTable.Rows[0]["SegmentoMercado"]).DBToString();

                        switch (SegmentoMercado)
                        {
                            case "04":
                                _CadastroPapelInfo.SegmentoMercado = SegmentoMercadoEnum.OPCAO;
                                break;
                            case "09":
                                _CadastroPapelInfo.SegmentoMercado = SegmentoMercadoEnum.OPCAO;
                                break;
                            case "01":
                                _CadastroPapelInfo.SegmentoMercado = SegmentoMercadoEnum.AVISTA;
                                break;
                            case "03":
                                _CadastroPapelInfo.SegmentoMercado = SegmentoMercadoEnum.FRACIONARIO;
                                break;
                            case "FUT":
                                _CadastroPapelInfo.SegmentoMercado = SegmentoMercadoEnum.FUTURO;
                                break;                            

                        #endregion

                        }

                        _response.CadastroPapelInfo = (_CadastroPapelInfo);
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao obter as informacoes do cadastro de papeis no banco de dados.");
                logger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }

        /// <summary>
        /// Obtem os parametros globais de permissoes e parametros dos clientes.
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public ParametrosPermissoesClienteResponse ObterParametrosGlobalRisco(ParametrosPermissoesClienteRequest pParametro)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            ParametrosPermissoesClienteResponse ObjetoParametrosPermissoesClienteResponse = new ParametrosPermissoesClienteResponse();

            string Parametro = "Parametro";
            string Permissao = "Permissao";   

            try
            {
                ClienteParametroPermissaoInfo _ClienteParametroPermissaoInfo = null;
                lAcessaDados.ConnectionStringName = gNomeConexao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "[prc_sel_regras_cliente_oms]"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.CodigoCliente);

                    logger.Info("Solicita consulta de parametros e permissões para o banco de dados");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {                     

                        logger.Info("Parametros e permissões carregados com sucesso.");

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _ClienteParametroPermissaoInfo = new ClienteParametroPermissaoInfo();

                            int ParametroPermissao = (lDataTable.Rows[i]["idParametroPermissao"]).DBToInt32();

                            _ClienteParametroPermissaoInfo.Especie   = (lDataTable.Rows[i]["Especie"]).DBToString();

                            if (_ClienteParametroPermissaoInfo.Especie == Permissao){
                                _ClienteParametroPermissaoInfo.Permissao = (RiscoPermissoesEnum)((lDataTable.Rows[i]["idParametroPermissao"]).DBToInt32());
                            }
                            else{
                                _ClienteParametroPermissaoInfo.Parametro = (RiscoParametrosEnum)((lDataTable.Rows[i]["idParametroPermissao"]).DBToInt32());
                            }

                            _ClienteParametroPermissaoInfo.IdCliente = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            _ClienteParametroPermissaoInfo.IdBolsa   = (lDataTable.Rows[i]["id_bolsa"]).DBToInt32();

                            _ClienteParametroPermissaoInfo.Descricao      = (lDataTable.Rows[i]["ParametroPermissao"]).DBToString();
                            _ClienteParametroPermissaoInfo.ValorParametro = (lDataTable.Rows[i]["valor"]).DBToDecimal();
                            _ClienteParametroPermissaoInfo.ValorAlocado   = (lDataTable.Rows[i]["vl_alocado"]).DBToDecimal();
                            _ClienteParametroPermissaoInfo.DtValidade     = (lDataTable.Rows[i]["dt_validade"]).DBToDateTime();
                            _ClienteParametroPermissaoInfo.DtMovimento    = (lDataTable.Rows[i]["dt_movimento"]).DBToDateTime();

                            ObjetoParametrosPermissoesClienteResponse.lstParametrosPermissoesClienteInfo.Add(_ClienteParametroPermissaoInfo);
                        }
                    }
                    else
                    {
                        logger.Info("Não foram encontrados parametros e permissões para este cliente.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao carregar os parametros e permissões do cliente.",ex);                
              
            }

            return ObjetoParametrosPermissoesClienteResponse;
        }

        /// <summary>
        /// Obtem os parametros globais de permissoes e parametros dos clientes.
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public LimiteOperacionalClienteResponse ObterLimiteCliente(LimiteOperacionalClienteRequest pParametro)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            LimiteOperacionalClienteResponse ObjetoLimitesResponse = new LimiteOperacionalClienteResponse();          

            try
            {
                LimiteOperacionalInfo ObjetoLimiteOperacionalInfo = null;
                lAcessaDados.ConnectionStringName = gNomeConexaoProducao;//gNomeConexaoHomo;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_relacao_limites"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.CodigoCliente);

                    logger.Info("Solicita consulta de parametros e permissões para o banco de dados");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        logger.Info("Parametros e permissões carregados com sucesso.");

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ObjetoLimiteOperacionalInfo = new LimiteOperacionalInfo();
                            
                            ObjetoLimiteOperacionalInfo.CodigoCliente   = pParametro.CodigoCliente;
                            ObjetoLimiteOperacionalInfo.CodigoParametroCliente = (lDataTable.Rows[i]["id_cliente_parametro"]).DBToInt32();
                            ObjetoLimiteOperacionalInfo.DataValidade    = (lDataTable.Rows[i]["dt_validade"]).DBToDateTime();
                            ObjetoLimiteOperacionalInfo.TipoLimite      = (TipoLimiteEnum)((lDataTable.Rows[i]["id_parametro"]).DBToInt32());
                            ObjetoLimiteOperacionalInfo.ValotTotal      = (lDataTable.Rows[i]["vl_parametro"]).DBToDecimal();
                            ObjetoLimiteOperacionalInfo.ValorAlocado    = (lDataTable.Rows[i]["vl_alocado"]).DBToDecimal();
                            ObjetoLimiteOperacionalInfo.ValorDisponivel = (ObjetoLimiteOperacionalInfo.ValotTotal - ObjetoLimiteOperacionalInfo.ValorAlocado);
                         
                            ObjetoLimitesResponse.LimitesOperacionais.Add(ObjetoLimiteOperacionalInfo);
                        }
                    }
                    else
                    {
                        logger.Info("Não foram encontrados limites operacionais para o cliente: " + pParametro.CodigoCliente.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao carregar os limites operacionais do cliente.", ex);

            }

            return ObjetoLimitesResponse;
        }


        /// <summary>
        /// Método responsavel por retornar a cotacao de instrumento requisitado.
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public EnviarCotacaoResponse ObterCotacao(EnviarCotacaoRequest pParametro)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            EnviarCotacaoResponse Response = new EnviarCotacaoResponse();

            try
            {

                logger.Info("Inicia consulta para ober a cotação do instrumento: " + pParametro.Instrumento);
                lAcessaDados.ConnectionStringName = gCotacao;

                CotacaoInfo lRetorno = new CotacaoInfo();

                logger.Info("Chama da procedure prc_sel_ativo_cotacao_oms");
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_ativo_cotacao_oms"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ativo", DbType.AnsiString, pParametro.Instrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    logger.Info("procedure invocada com sucesso.");

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lRetorno = new CotacaoInfo();

                            lRetorno.Ativo       = (lDataTable.Rows[i]["id_ativo"]).DBToString();
                            lRetorno.Ultima      = (lDataTable.Rows[i]["vl_ultima"]).DBToDecimal();
                            lRetorno.Oscilacao   = (lDataTable.Rows[i]["vl_oscilacao"]).DBToDecimal();
                            lRetorno.Fechamento  = (lDataTable.Rows[i]["vl_fechamento"]).DBToDecimal();
                            lRetorno.DataNegocio = (lDataTable.Rows[i]["dt_negocio"]).DBToDateTime();

                            Response.CotacaoInfo = lRetorno;
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao chamar a procedure prc_sel_ativo_cotacao_oms",ex);     

            }

            return Response;

        }

        public AtualizarLimitesResponse AtualizaLimiteCliente(AtualizarLimitesRequest request)
        {

            AcessaDados lAcessaDados = new AcessaDados();
            AtualizarLimitesResponse response = new AtualizarLimitesResponse();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoProducao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_atualiza_posicao_limite_alteracao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_parametro", DbType.Int32, request.LimiteOperacionalInfo.CodigoParametroCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ValorMovimento", DbType.Decimal, request.LimiteOperacionalInfo.ValorAlocado);
                    lAcessaDados.AddInParameter(lDbCommand, "@Descricao", DbType.AnsiString, "ATUALIZACAO LIMITE OPERACIONAL");
                    lAcessaDados.AddInParameter(lDbCommand, "@CLordIDRef", DbType.AnsiString, null);

                    logger.Info("Inicia atualização de limite operacional no banco de dados");

                    object Result = lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("Limite operacional atualizado com sucesso");
                    response.LimiteAtualizado = true;

                    return response;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao atualizar o limite operacional do cliente:" + request.LimiteOperacionalInfo.CodigoCliente.ToString() + ". Descricao: " + ex.Message);
                response.LimiteAtualizado = false;
                return response;
            }
        }

        public ExcluirLimiteBMFResponse InativarLimiteInstrumento(ExcluirLimiteBMFRequest request)
        {

            AcessaDados lAcessaDados = new AcessaDados();
            ExcluirLimiteBMFResponse response = new ExcluirLimiteBMFResponse();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoHomo;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_EXCLUIR_LIMITE_INSTRUMENTO"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroInstrumento", DbType.Int32, request.IdClienteParametroInstrumento);

                    logger.Info("Inicia atualização de limite operacional no banco de dados");

                    object Result = lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("Limite operacional atualizado com sucesso");
                    response.bSucesso = true;

                    return response;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao atualizar o limite de BMF", ex);
                response.bSucesso = false;
                return response;
            }
        }
        public ExcluirLimiteBMFResponse InativarSpiderLimiteInstrumento(ExcluirLimiteBMFRequest request)
        {

            AcessaDados lAcessaDados = new AcessaDados();
            ExcluirLimiteBMFResponse response = new ExcluirLimiteBMFResponse();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoSpider;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_EXCLUIR_LIMITE_INSTRUMENTO"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroInstrumento", DbType.Int32, request.IdClienteParametroInstrumento);

                    logger.Info("Inicia atualização de limite operacional no banco de dados");

                    object Result = lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("Limite operacional atualizado com sucesso");
                    response.bSucesso = true;

                    return response;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao atualizar o limite de BMF", ex);
                response.bSucesso = false;
                return response;
            }
        }
    }
}
