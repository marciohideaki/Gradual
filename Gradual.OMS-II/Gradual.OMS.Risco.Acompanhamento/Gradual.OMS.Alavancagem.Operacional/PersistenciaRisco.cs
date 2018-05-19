using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using log4net;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Collections;
using Gradual.OMS.CadastroPapeis.Lib;
using Gradual.OMS.CadastroPapeis.Lib.Mensageria;
using Gradual.OMS.CadastroPapeis.Lib.Info;
using Gradual.OMS.Alavancagem.Operacional;

namespace Gradual.OMS.Alavancagem.Operacional
{
    public class PersistenciaRisco
    {

        private const string gNomeConexaoOracle = "Sinacor";
        private const string gNomeConexaoSQL = "Risco";

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.Threading.Timer gTimerRecalculaPosicao;


        public ContaCorrenteInfo ObterSaldoAbertura(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            ContaCorrenteInfo ContaCorrenteInfo = new ContaCorrenteInfo();
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_saldo_abertura"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {

                        ContaCorrenteInfo.IdCliente = (lDataTable.Rows[0]["CD_CLIENTE"]).DBToInt32();
                        ContaCorrenteInfo.SaldoD0 = (lDataTable.Rows[0]["VL_DISPONIVEL"]).DBToDecimal();
                        ContaCorrenteInfo.SaldoD1 = (lDataTable.Rows[0]["VL_PROJET1"]).DBToDecimal();
                        ContaCorrenteInfo.SaldoD2 = (lDataTable.Rows[0]["VL_PROJET2"]).DBToDecimal();
                        ContaCorrenteInfo.Liquidacoes = (lDataTable.Rows[0]["a_liquidar"]).DBToDecimal();

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return ContaCorrenteInfo;
        }

        //private List<LimitePosicaoInfo> OrganizarLimites(List<LimitePosicaoInfo> Lista)
        //{
        //    LimitePosicaoInfo _LimitePosicaoInfo;
        //    List<LimitePosicaoInfo> ListaOrganizada = new List<LimitePosicaoInfo>();

        //    if (Lista.Count > 0)
        //    {

        //        for (int i = 0; i <= Lista.Count - 1; i++)
        //        {

        //            if (i == 0)
        //            {
        //                ListaOrganizada.Add(Lista[0]);
        //            }
        //            else
        //            {
        //                _LimitePosicaoInfo = new LimitePosicaoInfo();

        //                _LimitePosicaoInfo.IdClienteParametro = Lista[i].IdClienteParametro;
        //                _LimitePosicaoInfo.IdClienteParametroValor = Lista[i].IdClienteParametroValor;

        //                _LimitePosicaoInfo.VlMovimento = (Lista[i].VlMovimento);
        //                _LimitePosicaoInfo.VlAlocado = (ListaOrganizada[i - 1].VlAlocado + Lista[i].VlMovimento);
        //                _LimitePosicaoInfo.VlDisponivel = (ListaOrganizada[i - 1].VlDisponivel - _LimitePosicaoInfo.VlMovimento);

        //                ListaOrganizada.Add(_LimitePosicaoInfo);

        //            }

        //        }
        //    }

        //    return ListaOrganizada;
        //}        

        //public void RecalcularLimiteCliente(string CLOrdID)
        //{
        //    SqlTransaction trans = null;
        //    SqlConnection conn = new SqlConnection();

        //    List<LimitePosicaoInfo> lstPosicaoCliente = new List<LimitePosicaoInfo>();


        //    try
        //    {

        //        LimitePosicaoInfo _LimitePosicaoInfo;

        //        conn.ConnectionString = "Data Source=125.227.220.95;Initial Catalog=DirectTradeRisco;User Id=directtrade;Password=directtrade!1985;";

        //        conn.Open();

        //        trans = conn.BeginTransaction();

        //        SqlCommand CmdCliente = new SqlCommand("prc_recalcula_limite_cliente", conn, trans);

        //        CmdCliente.Transaction = trans;
        //        CmdCliente.CommandType = System.Data.CommandType.StoredProcedure;
        //        CmdCliente.Parameters.Clear();
        //        CmdCliente.Parameters.Add("@CLOrdID", SqlDbType.VarChar).Value = CLOrdID;

        //        DataTable DtPosicao = new DataTable();

        //        SqlDataAdapter Adapter = new SqlDataAdapter(CmdCliente);

        //        Adapter.Fill(DtPosicao);

        //        if (DtPosicao.Rows.Count > 0)
        //        {
        //            for (int i = 0; i <= DtPosicao.Rows.Count - 1; i++)
        //            {

        //                _LimitePosicaoInfo = new LimitePosicaoInfo();

        //                object objeto = (DtPosicao.Rows[i]["vl_movimento"]).DBToDecimal();

        //                _LimitePosicaoInfo.IdClienteParametro = (DtPosicao.Rows[i]["id_cliente_parametro"]).DBToInt32();
        //                _LimitePosicaoInfo.IdClienteParametroValor = (DtPosicao.Rows[i]["id_cliente_parametro_valor"]).DBToInt32();
        //                _LimitePosicaoInfo.VlMovimento = (DtPosicao.Rows[i]["vl_movimento"]).DBToDecimal();
        //                _LimitePosicaoInfo.VlAlocado = (DtPosicao.Rows[i]["vl_alocado"]).DBToDecimal();
        //                _LimitePosicaoInfo.VlDisponivel = (DtPosicao.Rows[i]["vl_disponivel"]).DBToDecimal();

        //                lstPosicaoCliente.Add(_LimitePosicaoInfo);

        //            }

        //            List<LimitePosicaoInfo> ListaRecalculada = OrganizarLimites(lstPosicaoCliente);

        //            SqlCommand cmdLimite = new SqlCommand("prc_atualiza_limitealavancagem", conn, trans);
        //            cmdLimite.Transaction = trans;
        //            cmdLimite.CommandType = System.Data.CommandType.StoredProcedure;

        //            foreach (LimitePosicaoInfo info in ListaRecalculada)
        //            {

        //                cmdLimite.Parameters.Clear();
        //                cmdLimite.Parameters.Add("@IdClienteParametroValor", SqlDbType.Int).Value = info.IdClienteParametroValor;
        //                cmdLimite.Parameters.Add("@VlMovimento", SqlDbType.Decimal).Value = info.VlMovimento.DBToDecimal();
        //                cmdLimite.Parameters.Add("@VlAlocado", SqlDbType.Decimal).Value = info.VlAlocado.DBToDecimal();
        //                cmdLimite.Parameters.Add("@VlDisponivel", SqlDbType.Decimal).Value = info.VlDisponivel.DBToDecimal();


        //                cmdLimite.ExecuteNonQuery();
        //            }


        //        }

        //        trans.Commit();
        //        conn.Close();
        //        conn.Dispose();

        //        logger.Info("Acesso ao banco efetuado com sucesso");

        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        conn.Close();
        //        conn.Dispose();

        //        logger.Info("Ocorreu um erro ao acessar o método: RecalcularLimiteCliente , ordem de referencia: " + CLOrdID);
        //        logger.Info("Descição do erro: " + ex.Message);

        //    }
        //}

        public Hashtable ObterCodigoParametroLimiteCliente(int id_cliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Hashtable hsParametros = new Hashtable();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_codigoLimite_cliente"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, id_cliente);

                    DataTable DtPosicao = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (DtPosicao.Rows.Count > 0)
                    {
                        for (int i = 0; i <= DtPosicao.Rows.Count - 1; i++)
                        {
                            hsParametros.Add(DtPosicao.Rows[i]["id_parametro"].DBToString(), DtPosicao.Rows[i]["id_cliente_parametro"].DBToString());
                        }
                    }

                    return hsParametros;
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao atualizar o limite cadastral do cliente: Descricao: " + ex.Message);
                return hsParametros;
            }

        }

        public bool AtualizaLimiteCliente(int IdClienteParametro, decimal ValorMovimento, string Descricao, string CLOrdIDRef, string Sentido, char Natureza)
        {

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_atualiza_posicao_limite_oms"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_parametro", DbType.Int32, IdClienteParametro);
                    lAcessaDados.AddInParameter(lDbCommand, "@ValorMovimento", DbType.Decimal, ValorMovimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@Descricao", DbType.AnsiString, Descricao);
                    lAcessaDados.AddInParameter(lDbCommand, "@CLordIDRef", DbType.AnsiString, CLOrdIDRef);
                    lAcessaDados.AddInParameter(lDbCommand, "@Sentido", DbType.AnsiString, Sentido);
                    lAcessaDados.AddInParameter(lDbCommand, "@Natureza", DbType.AnsiString, Natureza);

                    logger.Info("Inicia atualização de limite operacional no banco de dados");

                    object Result = lAcessaDados.ExecuteNonQuery(lDbCommand);

                    logger.Info("Limite operacional atualizado com sucesso");

                    return true;

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao atualizar o limite cadastral do cliente: Descricao: " + ex.Message);
                return false;
            }
        }

        public List<string> ObterCodigoCliente()
        {

            AcessaDados lAcessaDados = new AcessaDados();
            List<string> Clientes = new List<string>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                logger.Info("Cria conexão com o banco de dados e tenta executar a procedure {prc_obter_clientes_alavancagem}");

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_clientes_alavancagem")){

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                    logger.Info("Comando executado com sucesso.");

                    if (null != lDataTable && lDataTable.Rows.Count > 0){
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++){
                            int CodigoCliente = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            Clientes.Add(CodigoCliente.ToString());                          
                        }
                    }
                   
                }

                return Clientes;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                logger.Info("Ocorreu um erro ao acessar o banco de dados " + ex.Message);
            }

            return Clientes;
        }

        public int ObterQuantidadeBloqueioOpcao(string PapelBase, int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_custodia_opcao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "PapelBase", DbType.String, PapelBase);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            return (lDataTable.Rows[i]["saldobloqueado"]).DBToInt32();

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return 0;
        }

        public List<CustodiaPapelInfo> ListarOfertasAbertas(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<CustodiaPapelInfo> lstCustodiaPapelIndo = new List<CustodiaPapelInfo>();
            try
            {
                CustodiaPapelInfo lRetorno = null;
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_ORDENS_ABERTAS_OMS"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lRetorno = new CustodiaPapelInfo();

                            lRetorno.Instrumento = (lDataTable.Rows[i]["cd_negocio"]).DBToString();
                            lRetorno.Preco = (lDataTable.Rows[i]["vl_preco"]).DBToDecimal();
                            lRetorno.Quantidade = (lDataTable.Rows[i]["qt_ordem"]).DBToInt32();
                            lRetorno.NatOperacao = (lDataTable.Rows[i]["CD_NATOPE"]).DBToString();

                            lstCustodiaPapelIndo.Add(lRetorno);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lstCustodiaPapelIndo;
        }

        public List<CustodiaPapelInfo> ListarOrdensExecutadas(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<CustodiaPapelInfo> lstCustodiaPapelIndo = new List<CustodiaPapelInfo>();
            try
            {
                CustodiaPapelInfo lRetorno = null;
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_ordens_exec_sinacor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lRetorno = new CustodiaPapelInfo();

                            lRetorno.Quantidade = (lDataTable.Rows[i]["QT_NEGOCIO"]).DBToInt32();
                            lRetorno.Instrumento = (lDataTable.Rows[i]["CD_NEGOCIO"]).DBToString();
                            lRetorno.Preco = (lDataTable.Rows[i]["VL_NEGOCIO"]).DBToDecimal();
                            lRetorno.NatOperacao = (lDataTable.Rows[i]["CD_NATOPE"]).DBToString();
                            lstCustodiaPapelIndo.Add(lRetorno);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lstCustodiaPapelIndo;
        }

        public void RecalcularLimiteClienteCustodia(int CodigoCliente, int IdParametro, List<PosicaoClienteLimiteInfo> lstPosicaoClienteLimite)
        {
            SqlTransaction trans = null;
            SqlConnection conn = new SqlConnection();

            List<LimitePosicaoInfo> lstPosicaoCliente = new List<LimitePosicaoInfo>();

            try
            {

                conn.ConnectionString = "Data Source=125.227.220.95;Initial Catalog=DirectTradeRisco;User Id=directtrade;Password=directtrade!1985;";
               // conn.ConnectionString = "Data Source=192.168.254.14;Initial Catalog=DirectTradeRisco;User Id=directtrade;Password=directtrade!1985;";
                conn.Open();

                trans = conn.BeginTransaction();

                SqlCommand CmdCliente = new SqlCommand("prc_excluir_parametro_cliente", conn, trans);

                CmdCliente.Transaction = trans;
                CmdCliente.CommandType = System.Data.CommandType.StoredProcedure;
                CmdCliente.Parameters.Clear();
                CmdCliente.Parameters.Add("@idCliente", SqlDbType.Int).Value = CodigoCliente;
                CmdCliente.Parameters.Add("@idParametro", SqlDbType.Int).Value = IdParametro;

                CmdCliente.ExecuteNonQuery();


                if (lstPosicaoClienteLimite.Count > 0)
                {
                    SqlCommand cmdLimite = new SqlCommand("prc_consome_limite_cliente_cc", conn, trans);
                    cmdLimite.Transaction = trans;
                    cmdLimite.CommandType = System.Data.CommandType.StoredProcedure;

                    decimal? VolumeAcumulado = 0;

                    for (int i = 0; i <= lstPosicaoClienteLimite.Count - 1; i++)
                    {
                        PosicaoClienteLimiteInfo _PosicaoClienteLimiteInfo = (PosicaoClienteLimiteInfo)(lstPosicaoClienteLimite[i]);

                        VolumeAcumulado += _PosicaoClienteLimiteInfo.Volume;

                        string Historico = "Cliente: " + CodigoCliente.ToString() + " Papel: " + _PosicaoClienteLimiteInfo.Instrumento + " Quantidade: " + _PosicaoClienteLimiteInfo.Quantidade.ToString() + " Preco: " + _PosicaoClienteLimiteInfo.Preco.ToString() + " Data Hora: " + DateTime.Now.ToString();

                        cmdLimite.Parameters.Clear();
                        cmdLimite.Parameters.Add("@idCliente", SqlDbType.Int).Value = CodigoCliente;
                        cmdLimite.Parameters.Add("@idParametro", SqlDbType.Int).Value = IdParametro;
                        cmdLimite.Parameters.Add("@ValorAlocado", SqlDbType.Decimal).Value = VolumeAcumulado;
                        cmdLimite.Parameters.Add("@ValorMovimento", SqlDbType.Decimal).Value = _PosicaoClienteLimiteInfo.Volume;
                        cmdLimite.Parameters.Add("@st_natureza", SqlDbType.Char).Value = "D";
                        cmdLimite.Parameters.Add("@Historico", SqlDbType.VarChar).Value = Historico;

                        cmdLimite.ExecuteNonQuery();

                    }

                }
                else
                {
                    SqlCommand cmdLimite = new SqlCommand("prc_consome_limite_cliente_cc", conn, trans);
                    cmdLimite.Transaction = trans;
                    cmdLimite.CommandType = System.Data.CommandType.StoredProcedure;

                    string Historico = "Cliente: " + CodigoCliente.ToString() + " Parametro auto-populado pela ausencia de ordens abertas";

                    cmdLimite.Parameters.Clear();
                    cmdLimite.Parameters.Add("@idCliente", SqlDbType.Int).Value = CodigoCliente;
                    cmdLimite.Parameters.Add("@idParametro", SqlDbType.Int).Value = IdParametro;
                    cmdLimite.Parameters.Add("@ValorAlocado", SqlDbType.Decimal).Value = 0;
                    cmdLimite.Parameters.Add("@ValorMovimento", SqlDbType.Decimal).Value = 0;
                    cmdLimite.Parameters.Add("@st_natureza", SqlDbType.Char).Value = "D";
                    cmdLimite.Parameters.Add("@Historico", SqlDbType.VarChar).Value = Historico;

                    cmdLimite.ExecuteNonQuery();

                }

                logger.Info("Finaliza transação com sql server e fecha a conexão ");
                trans.Commit();
                conn.Close();
                conn.Dispose();

                logger.Info("Acesso ao banco efetuado com sucesso");

            }
            catch (Exception ex)
            {
                logger.Info("Erro na tentativa de executar a stored procedure no sql.");
                logger.Info("Realiza rollback na transação e fecha as conexões.");

                trans.Rollback();
                conn.Close();
                conn.Dispose();

                logger.Info("Descição do erro gerado: " + ex.Message);


            }
        }

        //public void RecalcularLimiteClienteContaCorrente(int CodigoCliente, int IdParametro, List<PosicaoClienteLimiteInfo> lstPosicaoClienteLimite)
        //{
        //    SqlTransaction trans = null;
        //    SqlConnection conn = new SqlConnection();

        //    List<LimitePosicaoInfo> lstPosicaoCliente = new List<LimitePosicaoInfo>();

        //    try
        //    {

        //        conn.ConnectionString = "Data Source=125.227.220.95;Initial Catalog=DirectTradeRisco;User Id=directtrade;Password=directtrade!1985;";
        //        conn.Open();

        //        trans = conn.BeginTransaction();

        //        SqlCommand CmdCliente = new SqlCommand("prc_excluir_parametro_cliente", conn, trans);

        //        CmdCliente.Transaction = trans;
        //        CmdCliente.CommandType = System.Data.CommandType.StoredProcedure;
        //        CmdCliente.Parameters.Clear();
        //        CmdCliente.Parameters.Add("@idCliente", SqlDbType.Int).Value = CodigoCliente;
        //        CmdCliente.Parameters.Add("@idParametro", SqlDbType.Int).Value = IdParametro;

        //        CmdCliente.ExecuteNonQuery();


        //        if (lstPosicaoClienteLimite.Count > 0)
        //        {
        //            SqlCommand cmdLimite = new SqlCommand("prc_consome_limite_cliente_cc", conn, trans);
        //            cmdLimite.Transaction = trans;
        //            cmdLimite.CommandType = System.Data.CommandType.StoredProcedure;

        //            decimal? VolumeAcumulado = 0;

        //            for (int i = 0; i <= lstPosicaoClienteLimite.Count - 1; i++)
        //            {
        //                PosicaoClienteLimiteInfo _PosicaoClienteLimiteInfo = (PosicaoClienteLimiteInfo)(lstPosicaoClienteLimite[i]);

        //                VolumeAcumulado += _PosicaoClienteLimiteInfo.Volume;

        //                string Historico = "Cliente: " + CodigoCliente.ToString() + " Papel: " + _PosicaoClienteLimiteInfo.Instrumento + " Quantidade: " + _PosicaoClienteLimiteInfo.Quantidade.ToString() + " Preco: " + _PosicaoClienteLimiteInfo.Preco.ToString() + " Data Hora: " + DateTime.Now.ToString();

        //                cmdLimite.Parameters.Clear();
        //                cmdLimite.Parameters.Add("@idCliente", SqlDbType.Int).Value = CodigoCliente;
        //                cmdLimite.Parameters.Add("@idParametro", SqlDbType.Int).Value = IdParametro;
        //                cmdLimite.Parameters.Add("@ValorAlocado", SqlDbType.Decimal).Value = VolumeAcumulado;
        //                cmdLimite.Parameters.Add("@ValorMovimento", SqlDbType.Decimal).Value = _PosicaoClienteLimiteInfo.Volume;
        //                cmdLimite.Parameters.Add("@st_natureza", SqlDbType.Char).Value = "D";
        //                cmdLimite.Parameters.Add("@Historico", SqlDbType.VarChar).Value = Historico;

        //                cmdLimite.ExecuteNonQuery();

        //            }

        //        }
        //        else
        //        {
        //            SqlCommand cmdLimite = new SqlCommand("prc_consome_limite_cliente", conn, trans);
        //            cmdLimite.Transaction = trans;
        //            cmdLimite.CommandType = System.Data.CommandType.StoredProcedure;

        //            string Historico = "Cliente: " + CodigoCliente.ToString() + " Parametro auto-populado pela ausencia de ordens abertas";

        //            cmdLimite.Parameters.Clear();
        //            cmdLimite.Parameters.Add("@idCliente", SqlDbType.Int).Value = CodigoCliente;
        //            cmdLimite.Parameters.Add("@idParametro", SqlDbType.Int).Value = IdParametro;
        //            cmdLimite.Parameters.Add("@ValorAlocado", SqlDbType.Decimal).Value = 0;
        //            cmdLimite.Parameters.Add("@ValorMovimento", SqlDbType.Decimal).Value = 0;
        //            cmdLimite.Parameters.Add("@st_natureza", SqlDbType.Char).Value = "D";
        //            cmdLimite.Parameters.Add("@Historico", SqlDbType.VarChar).Value = Historico;

        //            cmdLimite.ExecuteNonQuery();

        //        }

        //        logger.Info("Finaliza transação com sql server e fecha a conexão ");
        //        trans.Commit();
        //        conn.Close();
        //        conn.Dispose();

        //        logger.Info("Acesso ao banco efetuado com sucesso");

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Info("Erro na tentativa de executar a stored procedure no sql.");
        //        logger.Info("Realiza rollback na transação e fecha as conexões.");

        //        trans.Rollback();
        //        conn.Close();
        //        conn.Dispose();

        //        logger.Info("Descição do erro gerado: " + ex.Message);


        //    }
        //}

        public List<decimal> ObterPosicaoSaldoProjetado(int idCliente, string dataMovimento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<decimal> ProjetadoDia = new List<decimal>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                //using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "proc_obter_valor_projetado"))
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "proc_obter_valor_projetado_geral"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, idCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@datamovimento", DbType.DateTime, dataMovimento);
                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    decimal Volume = 0;

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {

                            Volume = (lDataTable.Rows[i]["VolumeDia"]).DBToDecimal();
                            ProjetadoDia.Add(Volume);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return ProjetadoDia;

        }

        private decimal ObterPosicaoFechamentoCotacao(string Instrumento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal ValorCotacao = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_fechamento_instrumento"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ativo", DbType.AnsiString, Instrumento);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (lDataTable.Rows.Count > 0)
                    {
                        ValorCotacao = (lDataTable.Rows[0]["vl_fechamento"]).DBToDecimal();

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return ValorCotacao;

        }

        public decimal ObterPrecoCotacao(string Instrumento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal ValorCotacao = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obtem_cotacao_atual"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ativo", DbType.AnsiString, Instrumento);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (lDataTable.Rows.Count > 0)
                    {
                        ValorCotacao = (lDataTable.Rows[0]["vl_ultima"]).DBToDecimal();

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return ValorCotacao;

        }


        public int ConsultarPermissaoAlavancagem(int Cliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            int contador = 0;


            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_verifica_permissao_alavancagem"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, Cliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (lDataTable.Rows.Count > 0)
                    {
                        contador = (lDataTable.Rows[0]["valor"]).DBToInt32();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return contador;

        }

        public List<string> ObterInstrumentosMovimentados(int idCliente, string dataMovimento, string TipoMovimento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<string> lInstrumentos = new List<string>();

            string Procedure = string.Empty;

            if (TipoMovimento == "OPC")
            {
                Procedure = "prc_obtem_papeis_movimentados_dia_opcoes";
            }
            else
            {
                Procedure = "prc_obtem_papeis_movimentados_dia_acoes_oms";
            }

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, Procedure))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, idCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@datamovimento", DbType.AnsiString, dataMovimento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string Instrumento = "";

                            Instrumento = (lDataTable.Rows[i]["symbol"]).DBToString();

                            lock (lInstrumentos)
                            {
                                lInstrumentos.Add(Instrumento);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lInstrumentos;

        }

        public List<CustodiaPapelInfo> ObterPosicaoCustodia(int idCliente, string dataMovimento, string TipoMercado)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<CustodiaPapelInfo> lstCustodiaDia = new List<CustodiaPapelInfo>();
            CustodiaPapelInfo CustodiaInfo = null;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_calcula_custodiaCliente"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, idCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@datamovimento", DbType.AnsiString, dataMovimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@TipoMercado", DbType.AnsiString, TipoMercado);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CustodiaInfo = new CustodiaPapelInfo();

                            CustodiaInfo.Instrumento = (lDataTable.Rows[i]["symbol"]).DBToString();

                            if (CustodiaInfo.Instrumento.Substring(CustodiaInfo.Instrumento.Length - 1, 1) == "F")
                            {
                                CustodiaInfo.Instrumento = CustodiaInfo.Instrumento.Remove(CustodiaInfo.Instrumento.Length - 1);
                            }

                            CustodiaInfo.Preco = (lDataTable.Rows[i]["volumedia"]).DBToDecimal();

                            string LadoOferta = (lDataTable.Rows[i]["LadoOferta"]).DBToString();

                            if (LadoOferta == "EXECCOMPRA")
                            {
                                CustodiaInfo.NatOperacao = "C";
                            }
                            else if (LadoOferta == "ABRVENDA")
                            {
                                CustodiaInfo.NatOperacao = "A";
                            }
                            else
                            {
                                CustodiaInfo.NatOperacao = "V";
                            }

                            lstCustodiaDia.Add(CustodiaInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lstCustodiaDia;

        }

        public List<CustodiaPapelInfo> ObterPosicaoCustodiaDorimidaAlavancagem(string dataMovimento)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<CustodiaPapelInfo> lstCustodiaDia = new List<CustodiaPapelInfo>();
            CustodiaPapelInfo CustodiaInfo = null;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_ofertas_dormidas_limite"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dtmovimento", DbType.AnsiString, dataMovimento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CustodiaInfo = new CustodiaPapelInfo();

                            CustodiaInfo.Instrumento   = (lDataTable.Rows[i]["symbol"]).DBToString();
                            CustodiaInfo.CodigoBovespa = (lDataTable.Rows[i]["account"]).DBToInt32();

                            if (CustodiaInfo.Instrumento.Substring(CustodiaInfo.Instrumento.Length - 1, 1) == "F")
                            {
                                CustodiaInfo.Instrumento = CustodiaInfo.Instrumento.Remove(CustodiaInfo.Instrumento.Length - 1);
                            }

                            CustodiaInfo.Preco = (lDataTable.Rows[i]["price"]).DBToDecimal();
                            CustodiaInfo.Quantidade = (lDataTable.Rows[i]["Quantidade"]).DBToInt32();

                            string LadoOferta = (lDataTable.Rows[i]["Side"]).DBToString();

                            if (LadoOferta == "1")
                            {
                                CustodiaInfo.NatOperacao = "C";
                            }
                            else if (LadoOferta == "2")
                            {
                                {
                                    CustodiaInfo.NatOperacao = "V";
                                }
                            }

                            CustodiaInfo.Mercado = (lDataTable.Rows[i]["SegmentoMercado"]).DBToString();
                            
                            CustodiaInfo.TransactTime = (lDataTable.Rows[i]["TransactTime"]).DBToDateTime();

                            lstCustodiaDia.Add(CustodiaInfo);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lstCustodiaDia;

        }

        public List<SaldoLimiteClienteInfo> ObterPosicaoLimiteOperacionalCliente(int idCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<SaldoLimiteClienteInfo> ListaLimiteInfo = new List<SaldoLimiteClienteInfo>();
            SaldoLimiteClienteInfo LimiteInfo;


            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_limite_disponivel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.AnsiString, idCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            LimiteInfo = new SaldoLimiteClienteInfo();

                            LimiteInfo.IdCliente = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            LimiteInfo.ValorDisponivel = (lDataTable.Rows[i]["vl_disponivel"]).DBToDecimal();
                            LimiteInfo.TipoLimite = (lDataTable.Rows[i]["id_parametro"]).DBToString();

                            ListaLimiteInfo.Add(LimiteInfo);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return ListaLimiteInfo;

        }


        public List<string> ObterClientesOperacaoIntraday(string dataMovimento)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<string> lstClientes = new List<string>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_cliente_operacao_intraday"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@datamovimento", DbType.AnsiString, dataMovimento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string CodigoCliente = (lDataTable.Rows[i]["account"]).DBToString();
                            lock (lstClientes)
                            {
                                lstClientes.Add(CodigoCliente);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lstClientes;

        }


        public List<string> ObterClientesOperacaoIntraday()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<string> lstClientes = new List<string>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obtem_clientes_utilizando_limite_dia"))
                {               

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string CodigoCliente = (lDataTable.Rows[i]["id_cliente"]).DBToString();
                            lock (lstClientes)
                            {
                                for (int g = 0; g <= 10; g++)
                                {
                                    lstClientes.Add(CodigoCliente);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObterClientesOperacaoIntraday");
                logger.Info("Descricao do erro: "  + ex.Message);
                logger.Info("StackTrace do erro: " + ex.StackTrace);
                throw (ex);
            }

            return lstClientes;

        }

        public List<CustodiaPapelInfo> ObterPosicaoCustodiaOfertas(int idCliente, string dataMovimento, string TipoMercado)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<CustodiaPapelInfo> lstCustodiaDia = new List<CustodiaPapelInfo>();
            CustodiaPapelInfo CustodiaInfo = null;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_quantidade_ofertas_custodia_dia"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, idCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@datamovimento", DbType.AnsiString, dataMovimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@TipoMercado", DbType.AnsiString, TipoMercado);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CustodiaInfo = new CustodiaPapelInfo();

                            CustodiaInfo.Instrumento = (lDataTable.Rows[i]["symbol"]).DBToString();
                            CustodiaInfo.OrderID = (lDataTable.Rows[i]["orderID"]).DBToInt32();
                            CustodiaInfo.OrderStatusID = (lDataTable.Rows[i]["OrderStatusID"]).DBToInt32();
                            CustodiaInfo.TransactTime = (lDataTable.Rows[i]["TransactTime"]).DBToDateTime();

                            if (CustodiaInfo.Instrumento.Substring(CustodiaInfo.Instrumento.Length - 1, 1) == "F")
                            {
                                CustodiaInfo.Instrumento = CustodiaInfo.Instrumento.Remove(CustodiaInfo.Instrumento.Length - 1);
                            }

                            CustodiaInfo.Preco = (lDataTable.Rows[i]["price"]).DBToDecimal();
                            CustodiaInfo.Quantidade = (lDataTable.Rows[i]["Quantidade"]).DBToInt32();

                            string LadoOferta = (lDataTable.Rows[i]["LadoOferta"]).DBToString();

                            if (LadoOferta == "Compra")
                            {
                                CustodiaInfo.NatOperacao = "C";
                            }
                            else
                            {
                                CustodiaInfo.NatOperacao = "V";
                            }

                            lstCustodiaDia.Add(CustodiaInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lstCustodiaDia;

        }

        public List<CustodiaPapelInfo> ObterPosicaoCustodiaIntraday(int idCliente, string dataMovimento)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<CustodiaPapelInfo> lstCustodiaDia = new List<CustodiaPapelInfo>();
            CustodiaPapelInfo CustodiaInfo = null;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;
           
                //using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_quantidade_custodia_dia"))
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_quantidade_custodia_dia_oms"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, idCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@datamovimento", DbType.AnsiString, dataMovimento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CustodiaInfo = new CustodiaPapelInfo();

                            CustodiaInfo.Instrumento = (lDataTable.Rows[i]["symbol"]).DBToString();

                            if (CustodiaInfo.Instrumento.Substring(CustodiaInfo.Instrumento.Length - 1, 1) == "F")
                            {
                                CustodiaInfo.Instrumento = CustodiaInfo.Instrumento.Remove(CustodiaInfo.Instrumento.Length - 1);
                            }

                            CustodiaInfo.Preco = (lDataTable.Rows[i]["price"]).DBToDecimal();
                            CustodiaInfo.Quantidade = (lDataTable.Rows[i]["Quantidade"]).DBToInt32();
                            int Mercado = (lDataTable.Rows[i]["SegmentoMercado"]).DBToInt32();

                            if ((Mercado == 1))
                            {
                                CustodiaInfo.Mercado = "VIS";
                            }

                            if ((Mercado == 3))
                            {
                                CustodiaInfo.Mercado = "VIS";
                            }


                            if (Mercado == 4)
                            {
                                CustodiaInfo.Mercado = "OPC";
                            }
                           

                            string LadoOferta = (lDataTable.Rows[i]["LadoOferta"]).DBToString();

                            if (LadoOferta == "Compra")
                            {
                                CustodiaInfo.NatOperacao = "C";
                            }
                            else
                            {
                                CustodiaInfo.NatOperacao = "V";
                            }

                            lstCustodiaDia.Add(CustodiaInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lstCustodiaDia;

        }

        public List<CustodiaAberturaInfo> ObterCustodiaAbertura(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<CustodiaAberturaInfo> lstCustodiaInfo = new List<CustodiaAberturaInfo>();
            CustodiaAberturaInfo CustodiaInfo = null;
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_abertura"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CustodiaInfo = new CustodiaAberturaInfo();

                            CustodiaInfo.ID_CLIENTE = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();

                            CustodiaInfo.INSTRUMENTO = (lDataTable.Rows[i]["CD_NEGOCIO"]).DBToString();

                            CustodiaInfo.TIPO_MERCADO = (lDataTable.Rows[i]["TIPO_MERC"]).DBToString();

                            CustodiaInfo.QTDE_DISP = (lDataTable.Rows[i]["QTDE_DISP"]).DBToInt32();
                            CustodiaInfo.COD_CARTEIRA = (lDataTable.Rows[i]["COD_CART"]).DBToInt32();

                            CustodiaInfo.QTDE_PROJ_CPA_D0 = (lDataTable.Rows[i]["QTDE_ALCPA"]).DBToInt32();
                            CustodiaInfo.QTDE_PROJ_CPA_D1 = (lDataTable.Rows[i]["QTDE1_CPA"]).DBToInt32();
                            CustodiaInfo.QTDE_PROJ_CPA_D2 = (lDataTable.Rows[i]["QTDE2_CPA"]).DBToInt32();

                            CustodiaInfo.QTDE_PROJ_VDA_D0 = (lDataTable.Rows[i]["QTDE_ALVDA"]).DBToInt32();
                            CustodiaInfo.QTDE_PROJ_VDA_D1 = (lDataTable.Rows[i]["QTDE1_VDA"]).DBToInt32();
                            CustodiaInfo.QTDE_PROJ_VDA_D2 = (lDataTable.Rows[i]["QTDE2_VDA"]).DBToInt32();

                            CustodiaInfo.VL_FECHAMENTO = ObterPosicaoFechamentoCotacao(CustodiaInfo.INSTRUMENTO);

                            CustodiaInfo.QTDE_PROJ_TOTAL_CPA = (CustodiaInfo.QTDE_PROJ_CPA_D1 + CustodiaInfo.QTDE_PROJ_CPA_D2);
                            CustodiaInfo.QTDE_PROJ_TOTAL_VDA = (CustodiaInfo.QTDE_PROJ_VDA_D1 + CustodiaInfo.QTDE_PROJ_VDA_D2);

                            CustodiaInfo.NET_QTDE_PROJ = (CustodiaInfo.QTDE_PROJ_TOTAL_CPA + CustodiaInfo.QTDE_PROJ_TOTAL_VDA);
                            CustodiaInfo.QTDE_TOTAL_DIA = (CustodiaInfo.QTDE_DISP + CustodiaInfo.QTDE_PROJ_CPA_D0 + CustodiaInfo.QTDE_PROJ_VDA_D0);

                            CustodiaInfo.QTDE_TOTAL_NEGOCIAVEL = (CustodiaInfo.QTDE_TOTAL_DIA + CustodiaInfo.NET_QTDE_PROJ);

                            lstCustodiaInfo.Add(CustodiaInfo);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lstCustodiaInfo;
        }

        public CadastroPapeisResponse<CadastroPapelInfo> ObterInformacoesPapeis(CadastroPapeisRequest pParametro)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            CadastroPapeisResponse<CadastroPapelInfo> CadastroPapelResponse = new CadastroPapeisResponse<CadastroPapelInfo>();

            try
            {

                CadastroPapelInfo lRetorno = null;
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_informacao_papel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CODNEG", DbType.String, pParametro.Instrumento.Trim());

                    logger.Info("Solicitação de consulta de papeis enviada para o banco de dados");

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        logger.Info("Instrumento encontrado com sucesso");

                        lRetorno = new CadastroPapelInfo();
                        lRetorno.Instrumento = (lDataTable.Rows[0]["CD_CODNEG"]).DBToString();
                        lRetorno.PapelObjeto = (lDataTable.Rows[0]["CD_TITOBJ"]).DBToString();
                        lRetorno.LoteNegociacao = (lDataTable.Rows[0]["NR_LOTNEG"]).DBToString();

                        string TipoBolsa = (lDataTable.Rows[0]["BOLSA"]).DBToString();

                        switch (TipoBolsa)
                        {
                            case "BOVESPA":
                                lRetorno.TipoBolsa = Gradual.OMS.CadastroPapeis.Lib.Enum.TipoBolsaEnum.BOVESPA;
                                break;
                            default:
                                lRetorno.TipoBolsa = Gradual.OMS.CadastroPapeis.Lib.Enum.TipoBolsaEnum.BMF;
                                lRetorno.TipoMercado = Gradual.OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.FUTURO;
                                break;
                        }

                        string TipoMercado = (lDataTable.Rows[0]["CD_TPMERC"]).DBToString();

                        switch (TipoMercado)
                        {

                            case "FRA":
                                lRetorno.TipoMercado = Gradual.OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.FRACIONARIO;
                                break;
                            case "VIS":
                                lRetorno.TipoMercado = Gradual.OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.AVISTA;
                                break;
                            case "OPC":
                                lRetorno.TipoMercado = Gradual.OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.OPCAO;
                                break;
                            case "OPV":
                                lRetorno.TipoMercado = Gradual.OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.OPCAO;
                                break;
                            case "LEI":
                                lRetorno.TipoMercado = Gradual.OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.LEILAO;
                                break;

                        }

                        CadastroPapelResponse.Objeto = lRetorno;
                    }
                    else
                    {
                        logger.Info("Instrumento não encontrado");
                    }

                }

                CadastroPapelResponse.DescricaoResposta = "Dados do instrumento <" + pParametro.Instrumento + "> carregados com sucesso !";
                CadastroPapelResponse.StatusResposta = Gradual.OMS.CadastroPapeis.Lib.Enum.CriticaMensagemEnum.OK;

            }
            catch (Exception ex)
            {
                CadastroPapelResponse.DescricaoResposta = ex.Message;
                CadastroPapelResponse.StackTrace = ex.StackTrace;
                CadastroPapelResponse.StatusResposta = Gradual.OMS.CadastroPapeis.Lib.Enum.CriticaMensagemEnum.Exception;
                CadastroPapelResponse.Objeto = null;

                logger.Info("Ocorreu um erro ao efetuar a busca de papeis.");
                logger.Info("Descrição do erro:   " + ex.Message);

            }

            return CadastroPapelResponse;
        }
    }
}

