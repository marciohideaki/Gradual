using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Gradual.Generico.Dados;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Enum;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;
using log4net;

namespace Gradual.OMS.WsIntegracao.Arena.Services
{
    public class ContaCorrenteServico :ServicoBase
    {
        #region Propriedades
        public IServicoContaCorrente gServicoContaCorrente;
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Construtores
        public ContaCorrenteServico()
        {
            gServicoContaCorrente = Ativador.Get<IServicoContaCorrente>();
        }
        #endregion

        #region Métodos
        public ContaCorrenteInfo GetSaldoContaCorrenteCliente(int CodigoCliente)
        {
            var lRetorno = new ContaCorrenteInfo();
            try
            {
                SaldoContaCorrenteRequest lRequest = new SaldoContaCorrenteRequest();

                lRequest.IdCliente = CodigoCliente;

                lRetorno = this.ObterSaldoContaCorrente(CodigoCliente); //gServicoContaCorrente.ObterSaldoContaCorrente(lRequest);
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
            }

            return lRetorno;
        }

        public ContaCorrenteInfo ObterSaldoContaCorrente(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            ContaCorrenteInfo _SaldoContaCorrente = new ContaCorrenteInfo();
            ContaCorrenteInfo lRetorno = new ContaCorrenteInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDO_ABERTURA_OMS"))
                {
                    lRetorno.IdClienteSinacor = CodigoCliente;

                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, CodigoCliente);

                    gLogger.Info("Inicia a consulta de saldos no banco de dados");

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        lRetorno.SaldoD2 = (lDataTable.Rows[0]["VL_PROJET2"]).DBToDecimal();
                    }

                    lRetorno.SaldoD0 = this.ObterSaldoD0(CodigoCliente);
                    lRetorno.SaldoD1 = this.ObterSaldoD1(CodigoCliente);
                    lRetorno.SaldoD3 = this.ObterSaldoD3(CodigoCliente);
                    lRetorno.SaldoContaMargem = this.ObtemSaldoContaMargem(CodigoCliente);
                    lRetorno.SaldoBloqueado = this.ObtemSaldoBloqueado(CodigoCliente);

                    gLogger.Info("Consulta de saldos no banco de dados carregadas com sucesso");

                    this.ConsultarLimiteOperacionalDisponivel(lRetorno);
                    //this.ConsultarLimiteOperacionalTotal(lRetorno);
                    /*
                    _SaldoContaCorrente.Objeto = lRetorno;
                    _SaldoContaCorrente.DescricaoResposta = string.Format("Saldo de conta corrente do cliente: {0} carregado com sucesso", pParametro.IdCliente.DBToString());
                    _SaldoContaCorrente.StatusResposta = CriticaMensagemEnum.OK;
                     * */
                }
            }
            catch (Exception ex)
            {

                gLogger.Info("Ocorreu um erro ao consultar a posição de conta corrente do cliente no banco de dados");
                gLogger.Info("Descição do erro          :" + ex.Message);
                /*
                _SaldoContaCorrente.DescricaoResposta = ex.Message;
                _SaldoContaCorrente.StackTrace = ex.StackTrace;
                _SaldoContaCorrente.StatusResposta = CriticaMensagemEnum.Exception;
                _SaldoContaCorrente.Objeto = null;
                 * */
            }


            return lRetorno;
        }
        /// <summary>
        /// Saldo disponivel D0
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        private decimal ObterSaldoD0(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_saldo_negociavel_oms"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["vl_disponivel"]).DBToDecimal();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta Corrente (D3)do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
        }

        /// <summary>
        /// Saldo D+1
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        private decimal ObterSaldoD1(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoD1 = 0;
            decimal SaldoAberturaOpcoes = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOD1_NOTA"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, IdCliente);

                    gLogger.Info("Inicia a consulta de saldos no banco de dados");

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        SaldoD1 = (lDataTable.Rows[0]["Opecacoes_D1"]).DBToDecimal();
                    }

                    SaldoAberturaOpcoes = this.AberturaDiaOpcoes(IdCliente);
                }

                return (SaldoAberturaOpcoes + SaldoD1);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta Corrente (D3)do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
        }

        /// <summary>
        /// SALDO DIA
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        private decimal ObterSaldoD3(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDO_DIA_OMS"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["vl_projet3"]).DBToDecimal();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta Corrente (D3)do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
        }


        /// <summary>
        /// Obtem o saldo de conta margem do cliente
        /// </summary>
        /// <param name="IdCliente">Código do cliente</param>
        /// <returns>Saldo em conta margem do cliente</returns>
        private Nullable<decimal> ObtemSaldoContaMargem(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            SaldoContaCorrenteResponse<ContaCorrenteInfo> _SaldoContaCorrente = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoContaMargem;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_contamargem"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["VL_LIMITE"]).DBToDecimal();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta margem do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
        }

        /// <summary>
        /// Obtem o saldo bloqueado em CC do cliente.
        /// </summary>
        /// <param name="IdCliente">Código do cliente</param>
        /// <returns>Saldo Bloqueado em CC</returns>
        private Nullable<decimal> ObtemSaldoBloqueado(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoRisco;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_saldo_bloqueado_cliente"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@Account", DbType.AnsiString, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["SaldoBloqueado"]).DBToDecimal();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta margem do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }

        }

        /// <summary>
        /// Busca o limite operacional disponível para o cliente.
        /// </summary>
        private void ConsultarLimiteOperacionalDisponivel(ContaCorrenteInfo pParametro)
        {
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = gNomeConexaoRisco;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_limites_cliente_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.IdClienteSinacor);

                gLogger.Info("Inicia a consulta de limites operacionais no banco de dados");

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    DataRow dr = null;
                    int id_parametro;

                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        dr = lDataTable.Rows[i];

                        if (!DBNull.Value.Equals(dr["id_parametro"]))
                        {
                            id_parametro = (int)dr["id_parametro"];

                            switch (id_parametro)
                            {
                                case 5:  //--> Limite descoberto no mercado a vista
                                    pParametro.LimiteOperacioalDisponivelAVistaVenda = lDataTable.Rows[i]["valor"].DBToDecimal();
                                    break;
                                case 7:  //--> Limite descoberto no mercado de opcoes
                                    pParametro.LimiteOperacioalDisponivelOpcaoVenda = lDataTable.Rows[i]["valor"].DBToDecimal();
                                    break;
                                case 12: //--> Limite para compra mercado a vista         
                                    pParametro.LimiteOperacioalDisponivelAVista = lDataTable.Rows[i]["valor"].DBToDecimal();
                                    break;
                                case 13: //--> Limite para compra no mercado de opções 
                                    pParametro.LimiteOperacioalDisponivelOpcao = lDataTable.Rows[i]["valor"].DBToDecimal();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private decimal AberturaDiaOpcoes(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoAberturaD1 = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOD1_ABERTURA"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, IdCliente);

                    gLogger.Info("Inicia a consulta de saldos no banco de dados");

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        SaldoAberturaD1 = (lDataTable.Rows[0]["Abertura_D1"]).DBToDecimal();
                    }
                }

                return SaldoAberturaD1;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta Corrente (D3)do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
        }
        #endregion
    }
}