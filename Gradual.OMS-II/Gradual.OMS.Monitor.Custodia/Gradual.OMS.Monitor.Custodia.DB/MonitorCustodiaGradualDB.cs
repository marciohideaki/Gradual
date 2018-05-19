using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using System.Collections;

namespace Gradual.OMS.Monitor.Custodia.DB
{
    /// <summary>
    /// Classe de banco de dados para gerenciamento das funcionalidades dados de ranking da gradual
    /// </summary>
    public class MonitorCustodiaGradualDB
    {
        #region Atributos
        
        /// <summary>
        /// Atributo do Log4Net
        /// </summary>
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Conexão com o banco de dados do risco
        /// </summary>
        private static string gNomeConexaoGradualOMS = "GradualOMS";

        /// <summary>
        /// Conexão com o banco de dados do Spider
        /// </summary>
        private static string gNomeConexaoGradualSpider2 = "GradualSpider2";
        #endregion

        #region Construtores
        public MonitorCustodiaGradualDB() { }
        #endregion

        #region Métodos

        /// <summary>
        /// Método que atualiza a base de dados do gradualOMS com os dados de posição global da corretora.
        /// Procedure: prc_atualiza_posicao_custodia_gradual
        /// </summary>
        /// <param name="pParametro">Objeto encapsulado da posição global da gradual diaria</param>
        public void AtualizaPosicaoCustodiaGradual(MonitorPositionGlobalGradulaInfo pParametro)
        {
            var lDados = new AcessaDados();

            try
            {
                lDados.ConnectionStringName = gNomeConexaoGradualOMS;

                foreach (var info in pParametro.ListColumn)
                {
                    DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_atualiza_posicao_custodia_gradual");

                    lDados.AddInParameter(lCommand, "@name_line",   DbType.String,  info.NameLine);
                    lDados.AddInParameter(lCommand, "@total_buy",   DbType.Int32,   info.TotalBuy);
                    lDados.AddInParameter(lCommand, "@total_sell",  DbType.Int32,   info.TotalSell);
                    lDados.AddInParameter(lCommand, "@volume",      DbType.Decimal, info.Volume);
                    lDados.AddInParameter(lCommand, "@net",         DbType.Decimal, info.Net);
                    lDados.AddInParameter(lCommand, "@qty_order",   DbType.Decimal, info.QtyOrder);

                    // Executa a operação no banco.
                    lDados.ExecuteNonQuery(lCommand);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Consulta as ordens de opções de intraday do banco de dados do spider 2
        /// </summary>
        /// <returns>Retorna o objeto encpsulado do banco de dados com os dados de ranking da gradual</returns>
        public MonitorIntradayOrderInfo ConsultarOrdensBovespaIntraday()
        {
            var lDados = new AcessaDados();

            var lRetorno = new MonitorIntradayOrderInfo();

            try
            {
                lDados.ConnectionStringName = gNomeConexaoGradualSpider2;

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_monitor_ordens_intraday_bovespa");

                var lDataTable = lDados.ExecuteDbDataTable(lCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        var lMonitor = new MonitorOrderInfo();

                        var lRow = lDataTable.Rows[i];

                        lMonitor.Symbol   = lRow["symbol"].ToString();
                        lMonitor.Side     = int.Parse( lRow["Side"].ToString());
                        lMonitor.QtyOrder =( int.Parse(lRow["OrderQty"].ToString()) - int.Parse(lRow["OrderQtyRemaining"].ToString()));

                        lRetorno.ListOrders.Add(lMonitor);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);

            }
            return lRetorno;
        }
        
        /// <summary>
        /// Consulta listagem de cadastro de papel no banco de dados
        /// </summary>
        /// <returns>Retorna a listagem de papel que e seu segmento em uma hashtable para ser usada para consulta no serviço 
        /// de monitoramento de custódia.</returns>
        public Hashtable ConsultarListaCadastroPapelOpcoes()
        {
            var lDados = new AcessaDados();

            var lRetorno = new Hashtable();

            try
            {
                lDados.ConnectionStringName = gNomeConexaoGradualOMS;

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_cadastro_papel_opcoes_sel");

                var lDataTable = lDados.ExecuteDbDataTable(lCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        var lRow = lDataTable.Rows[i];

                        string lSymbol          = lRow["CodigoInstrumento"].ToString();
                        decimal lPrecoExercicio = Convert.ToDecimal(lRow["PrecoExercicio"].ToString());

                        lRetorno.Add(lSymbol, lPrecoExercicio);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);

            }
            return lRetorno;
        }
        #endregion
    }
}
