#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.Common;
using System.Data;
using Gradual.Generico.Dados;
using Gradual.OMS.ClientePremium.Lib;
using System.Data.OracleClient;
using System.Configuration;
#endregion

namespace Gradual.OMS.ClientePremium
{
    /// <summary>
    /// Classe para acesso a dados
    /// </summary>
    public class PersistenciaDB
    {
        #region Prpopriedades
        private const string _ConnStringTrade          = "TRADE";
        private const string _ConnStringControleAcesso = "ControleAcesso";
        private const string _ConnstringCorrWin        = "CORRWIN";
        private const string _ConnStringRisco          = "Risco";

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string StringConexaoSinacor
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CORRWIN"].ToString();
            }
        }
        #endregion

        #region Construtores
        public PersistenciaDB()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        #region Métodos Atualização Sinacor
        /// <summary>
        /// Atualiza a tabela de Corretagem do sinacor dos clientes
        /// </summary>
        /// <param name="pListClientes">List<ClienteGradualPremiumInfo> lista de clientes para serem atualizados</param>
        /// <param name="pTiraCorretagem">Booleano para configurar a corretagem os clientes</param>
        /// <param name="pTipoCorretagem">Código para a tabela do sinacor usar ou não usar a corretagem </param>
        public void AtualizaCorretagemSinacor(List<ClienteGradualPremiumInfo> pListClientes, bool pTiraCorretagem, int pTipoCorretagem)
        {
            OracleConnection lConn;
            OracleCommand lCommand;
            OracleTransaction lTrans = null;

            lConn = new OracleConnection(StringConexaoSinacor);

            try 
            {	        
                lConn.Open();

                lTrans = lConn.BeginTransaction();

                lCommand = new OracleCommand();

                lCommand.Connection = lConn;

                lCommand.Transaction = lTrans;

                lCommand.CommandType = CommandType.Text;

                foreach (ClienteGradualPremiumInfo lCliente in pListClientes)
                {
                    if (pTiraCorretagem)
                    {
                        logger.InfoFormat("Retirando (Zerando, Inicio de Mês) Corretagem do  cliente : {0}", lCliente.CdCliente);
                    }
                    else
                    {
                        logger.InfoFormat("Reativando Corretagem do cliente : {0}, o cliente já emitiu {1} no mês", lCliente.CdCliente, lCliente.QuantidadeOrdens);
                    }
                    
                    lCommand.CommandText = string.Format("UPDATE TSCCLIBOL SET IN_TIPO_CORRET = {0} WHERE CD_CLIENTE = {1}", pTipoCorretagem, lCliente.CdCliente);

                    lCommand.ExecuteNonQuery();
                }

                lTrans.Commit();

                logger.InfoFormat("! **************************************************************************************** !");

                logger.InfoFormat("! ****************** Atualização de corretagem efetuada com sucesso ********************** !");

                logger.InfoFormat("! **************************************************************************************** !");
            }
            catch (Exception ex)
            {
                lTrans.Rollback();

                logger.ErrorFormat("Erro em AtualizaCorretagemSinacor - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);
            }
            finally
            {
                lConn.Close();

                logger.InfoFormat("Fechando a conexão no método AtualizaCorretagemSinacor");
            }
        }

        /// <summary>
        /// Atualiza a tabela de Custódia do sinacor dos clientes
        /// </summary>
        /// <param name="pListClientes">List<ClienteGradualPremiumInfo> pListClientes - Lista de Clientes a serem atualiados </param>
        /// <param name="pTiraCustodia">Booleano para retirar ou inserir custódia nos cliente (351 - tirar a custódia, 151 - insere a custódia)</param>
        public void AtualizaCustodiaSinacor(List<ClienteGradualPremiumInfo> pListClientes, bool pTiraCustodia)
        {

            OracleConnection lConn;
            OracleCommand lCommand;
            OracleTransaction lTrans = null;

            lConn = new OracleConnection(StringConexaoSinacor);

            try
            {
                lConn.Open();

                lTrans = lConn.BeginTransaction();

                lCommand = new OracleCommand();

                lCommand.Connection = lConn;

                lCommand.Transaction = lTrans;

                lCommand.CommandType = CommandType.Text;

                string lQueryCus = string.Empty;

                foreach (ClienteGradualPremiumInfo lCliente in pListClientes)
                {
                    if (pTiraCustodia)
                    {
                        lQueryCus = string.Format("UPDATE TSCCLICUS SET TP_CUSTODIA = 351 WHERE CD_CLIENTE = {0}", lCliente.CdCliente);

                        logger.InfoFormat("Inativando Custódia do cliente : {0}", lCliente.CdCliente);
                    }
                    else
                    {
                        lQueryCus = string.Format("UPDATE TSCCLICUS SET TP_CUSTODIA = 151 WHERE CD_CLIENTE = {0}", lCliente.CdCliente);

                        logger.InfoFormat("Ativando Custódia do cliente : {0}", lCliente.CdCliente);
                    }

                    lCommand.CommandText = string.Format(lQueryCus);

                    lCommand.ExecuteNonQuery();

                    
                }

                lTrans.Commit();

                logger.InfoFormat("! **************************************************************************************** !");

                logger.InfoFormat("! ****************** Atualização de custodia efetuada com sucesso ************************ !");

                logger.InfoFormat("! **************************************************************************************** !");

            }
            catch (Exception ex)
            {
                lTrans.Rollback();

                logger.ErrorFormat("Erro em AtualizaCustodiaSinacor - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);
            }
            finally
            {
                lConn.Close();

                logger.InfoFormat("Fechando a conexão no método AtualizaCustodiaSinacor");
            }
        }
        #endregion

        #region Seleciona clientes no sql para configuração de custódia e corretagem
        
        /// <summary>
        /// Seleciona do clientes para retirar a taxa de custódia dos clientes que já efetuaram ordens
        /// </summary>
        /// <returns>Retorno uma lista com os clientes que já enviaram ordens</returns>
        public List<ClienteGradualPremiumInfo> SelecionarClienteParaTirarCustodia()
        {
            List<ClienteGradualPremiumInfo> lRetorno = new List<ClienteGradualPremiumInfo>();

            try
            {
                AcessaDados lAcesso = new AcessaDados();

                lAcesso.ConnectionStringName = _ConnStringRisco;

                using (DbCommand cmdSel = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_cliente_para_remover_custodia_sel"))
                {
                    DataTable lTable = lAcesso.ExecuteDbDataTable(cmdSel);

                    lRetorno.Clear();

                    if (lTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in lTable.Rows)
                        {
                            lRetorno.Add(this.CriarRegistroListarCliente(row));

                            this.InserirRegistroClienteCustodiaRemovida(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em SelecionarClienteParaTirarCustodia- {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Seleciona do clientes para configurar a corretagem dos clientes que já efetuaram mais que 5     ordens
        /// </summary>
        /// <returns>Retorno uma lista com os clientes que já enviaram ordens</returns>
        public List<ClienteGradualPremiumInfo> SelecionarClienteParaAtivarCorretagem()
        {
            List<ClienteGradualPremiumInfo> lRetorno = new List<ClienteGradualPremiumInfo>();

            try
            {
                AcessaDados lAcesso = new AcessaDados();

                lAcesso.ConnectionStringName = _ConnStringRisco;

                using (DbCommand cmdSel = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_cliente_para_ativar_corretagem_sel"))
                {
                    DataTable lTable = lAcesso.ExecuteDbDataTable(cmdSel);

                    lRetorno.Clear();

                    if (lTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in lTable.Rows)
                        {
                            lRetorno.Add(this.CriarRegistroListarCliente(row));

                            //this.InserirRegistroClienteCorretagemAtivada(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em SelecionarClienteParaAtivarCorretagem - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }
        #endregion

        #region Seleciona e reativa corretagem e custódia normal
        /// <summary>
        /// Essa rotina deve ser rodada ás 23h do ultimo dia do mês para 
        /// selecionar os clientes para reativar a custódia normal dos clientes que enviaram as ordens
        /// </summary>
        /// <returns>Retorna uma lista com os clientes que irão ter a custódia reativado às 23h do ultimo dia do mês</returns>
        public List<ClienteGradualPremiumInfo> SelecionarClienteParaReativarCustodia()
        {
            List<ClienteGradualPremiumInfo> lRetorno = new List<ClienteGradualPremiumInfo>();

            try
            {
                AcessaDados lAcesso = new AcessaDados();

                lAcesso.ConnectionStringName = _ConnStringControleAcesso;

                using (DbCommand cmdSel = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_cliente_custodia_alterada_sel"))
                {
                    DataTable lTable = lAcesso.ExecuteDbDataTable(cmdSel);

                    lRetorno.Clear();

                    if (lTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in lTable.Rows)
                        {
                            lRetorno.Add(this.CriarRegistroListarClienteCorretagem(row));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em SelecionarClienteParaReativarCorretagem - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Essa rotina deve ser rodada ás 23h do ultimo dia do mês para 
        /// selecionar os clientes para reativar a corretagem dos clientes que enviaram as ordens
        /// </summary>
        /// <returns>Retorna uma lista com os clientes que irão ter a corretagem reativado às 23h do ultimo dia do mês</returns>
        public List<ClienteGradualPremiumInfo> SelecionarClienteParaInativarCorretagem()
        {
            List<ClienteGradualPremiumInfo> lRetorno = new List<ClienteGradualPremiumInfo>();

            try
            {
                AcessaDados lAcesso = new AcessaDados();

                lAcesso.ConnectionStringName = _ConnStringControleAcesso;

                using (DbCommand cmdSel = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_cliente_corretagem_alterada_sel"))
                {
                    DataTable lTable = lAcesso.ExecuteDbDataTable(cmdSel);

                    lRetorno.Clear();

                    if (lTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in lTable.Rows)
                        {
                            lRetorno.Add(this.CriarRegistroListarClienteCorretagem(row));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em SelecionarClienteParaReativarCorretagem - {0} - Stacktrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }
        #endregion

        #region Insere logs de corretagem e custódia
        /// <summary>
        /// Insere na tabela de log os clientes que 
        /// tiveram a corretagem removida 
        /// </summary>
        /// <param name="lRow">DataRow lRow - Row com os dados de códidgo de cliente e quantidade</param>
        private void InserirRegistroClienteCorretagemAtivada(DataRow lRow)
        {
            AcessaDados lAcesso = new AcessaDados();

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            using (DbCommand cmdins = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_cliente_log_corretagem_ins"))
            {
                lAcesso.AddInParameter(cmdins, "@cd_cliente", DbType.Int32, lRow["account"].DBToInt32());
                lAcesso.AddInParameter(cmdins, "@quantidade", DbType.Int32, lRow["quantidade"].DBToInt32());

                lAcesso.ExecuteNonQuery(cmdins);
            }
        }

        /// <summary>
        /// Insere na tabela de log os clientes 
        /// que tiveram a custódia removida
        /// </summary>
        /// <param name="lRow">DataRow lRow - Row com os dados de códidgo de cliente e quantidade</param>
        private void InserirRegistroClienteCustodiaRemovida(DataRow lRow)
        {
            AcessaDados lAcesso = new AcessaDados();

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            using (DbCommand cmdins = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_cliente_log_custodia_ins"))
            {
                lAcesso.AddInParameter(cmdins, "@cd_cliente", DbType.Int32, lRow["account"].DBToInt32());

                lAcesso.ExecuteNonQuery(cmdins);
            }
        }
        #endregion

        #region Cria registro 
        /// <summary>
        /// Criar o registro de cliente com os dados que vem do banco
        /// </summary>
        /// <param name="pRow">Row com os dados que vem do banco </param>
        /// <returns>Retorno um objeto ClienteGradualPremiumInfo</returns>
        private ClienteGradualPremiumInfo CriarRegistroListarCliente(DataRow pRow)
        {
            ClienteGradualPremiumInfo lRetorno = new ClienteGradualPremiumInfo();

            lRetorno.CdCliente        = pRow["account"].DBToInt32();
            lRetorno.QuantidadeOrdens = pRow["quantidade"].DBToInt32();

            return lRetorno;
        }

        /// <summary>
        /// Criar o registro de cliente com os dados que vem do banco
        /// </summary>
        /// <param name="pRow">ROw com os dados que vem do banco</param>
        /// <returns>Retorno um objeto ClienteGradualPremiumInfo</returns>
        private ClienteGradualPremiumInfo CriarRegistroListarClienteCorretagem(DataRow pRow)
        {
            ClienteGradualPremiumInfo lRetorno = new ClienteGradualPremiumInfo();

            lRetorno.CdCliente        = pRow["cd_cliente"].DBToInt32();

            return lRetorno;
        }
        #endregion

        #region Tests
        private void AnularAlteracaoCustodiaCorretagem(bool bMaisDeOrdens)
        {

            //AtualizaCorretagemSinacor(List<ClienteGradualPremiumInfo> pListClientes, bool pTiraCorretagem, int pTipoCorretagem)

            //public void AtualizaCustodiaSinacor(List<ClienteGradualPremiumInfo> pListClientes, bool pTiraCustodia)

            //SelecionarClienteParaReativarCustodia()

            //SelecionarClienteParaInativarCorretagem()

            //SelecionarClienteParaAtivarCorretagem()

            //SelecionarClienteParaTirarCustodia()
        }

        #endregion
    }
}