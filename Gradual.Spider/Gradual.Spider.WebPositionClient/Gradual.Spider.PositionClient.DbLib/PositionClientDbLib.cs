using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Spider.PositionClient.Lib;
using Gradual.Spider.PositionClient.Lib.Dados;
using Gradual.Spider.PositionClient.Lib.Messages;
using Gradual.OMS.Library;

namespace Gradual.Spider.PositionClient.DbLib
{
    /// <summary>
    /// Classe de acesso ao banco de dados para consulta no sinacor.
    /// Obtenção das posições de 
    /// </summary>
    public class PositionClientDbLib : PersistenciaBase
    {
        #region Métodos
        /// <summary>
        /// Método que busca no banco de dados as informações de ordens históricas para atribuir ao objeto de trade by trade
        /// </summary>
        /// <param name="lRequest">Objeto de Request de Trade By TRade</param>
        /// <returns>Retorna lista de Trade by Trade do cliente</returns>
        public static TradeByTradeResponse BuscarTradeByTrade(TradeByTradeRequest lRequest )
        {
            var lResposta = new TradeByTradeResponse();
            var lAcessaDados = new ConexaoDbHelper();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSpiderRMS;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_resumido_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_de", DbType.Int32, lRequest.De );
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ate", DbType.Int32, lRequest.Ate);
                    lAcessaDados.AddInParameter(lDbCommand, "@account", DbType.Int32, lRequest.Account);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lResposta.ListaTradeByTrade.Add(CriaRegistroTradeByTrade(lDataTable.Rows[i]));
                            //resposta.Resultado.Add(CriarRegistroClienteResumido(lDataTable.Rows[i], false));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método BuscarTradeByTrade - "+ ex.Message, ex);
                throw ex;
            }

            return lResposta;
        }

        /// <summary>
        /// Método que cria o registro da info trade by trade
        /// </summary>
        /// <param name="pRow">DataRow com os dados de Trade by Trade para montar o objeto</param>
        /// <returns>Retorna uma objeto do tipo Trade by trade com os dados que vem do banco de dados</returns>
        public static TradeByTradeInfo CriaRegistroTradeByTrade(DataRow pRow)
        {
            var lRetorno = new TradeByTradeInfo();

            lRetorno.Account         = pRow["Account"].DBToInt32();
            lRetorno.Ativo           = pRow["Symbol"].DBToString();
            lRetorno.Bolsa           = pRow["Exchange"].DBToString();
            lRetorno.CodCarteira     = pRow[""].DBToInt32();
            lRetorno.CodPapelObjeto  = pRow[""].DBToString();
            lRetorno.DtMovimento     = pRow["TransactTime"].DBToDateTime();
            lRetorno.DtPosicao       = pRow["RegisterTime"].DBToDateTime();
            lRetorno.DtVencimento    = pRow["ExpireDate"].DBToDateTime();
            lRetorno.FinancNet       = pRow[""].DBToDecimal();
            lRetorno.LucroPrej       = pRow[""].DBToDecimal();
            lRetorno.NetAb           = pRow[""].DBToDecimal();
            lRetorno.NetExec         = pRow[""].DBToDecimal();
            lRetorno.PcMedC          = pRow[""].DBToDecimal();
            lRetorno.PcMedV          = pRow[""].DBToDecimal();
            lRetorno.PrecoFechamento = pRow[""].DBToDecimal();
            lRetorno.QtdAbC          = pRow[""].DBToDecimal();
            lRetorno.QtdAbertura     = pRow[""].DBToDecimal();
            lRetorno.QtdAbV          = pRow[""].DBToDecimal();
            lRetorno.QtdD1           = pRow[""].DBToDecimal();
            lRetorno.QtdD2           = pRow[""].DBToDecimal();
            lRetorno.QtdD3           = pRow[""].DBToDecimal();
            lRetorno.QtdExecC        = pRow[""].DBToDecimal();
            lRetorno.QtdExecV        = pRow[""].DBToDecimal();
            lRetorno.SegmentoMercado = pRow[""].DBToString();
            lRetorno.TipoMercado     = pRow[""].DBToString();
            lRetorno.UltPreco        = pRow[""].DBToDecimal();
            lRetorno.Variacao        = pRow[""].DBToDecimal();


            return lRetorno;
        }

        /// <summary>
        /// Método que busca no banco de dados os dados de cadastro de papel
        /// </summary>
        /// <returns>Retorna um objeto preechido do SymbolResponse com a lista de papeis</returns>
        public SymbolResponse BuscarCadastroPapel(SymbolRequest lRequest)
        {
            try
            {
                SymbolResponse lResposta = null;

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoSpiderRMS;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lmt_cadastropapel_sel"))
                {
                    int lDays;

                    if (ConfigurationManager.AppSettings.AllKeys.Contains("DaysSecurityList"))
                    {
                        lDays = Convert.ToInt32(ConfigurationManager.AppSettings["DaysSecurityList"].ToString());
                    }
                    else
                    {
                        lDays = 5;
                    }

                    lAcessaDados.AddInParameter(lDbCommand, "@DataRegistro", DbType.Int32, DateTime.Now.AddDays(lDays * (-1)) );

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            var item = CriaRegistroSymbol(lDataTable.Rows[i]);

                            lResposta.ListaPapel.AddOrUpdate(item.Instrumento, item, (key, oldValue) => item);
                        }
                    }
                }
                
                return lResposta;
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método BuscarCadastroPapel - " + ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Cria registro de cadastro de papel 
        /// </summary>
        /// <param name="pRow">DataRow com os dados de caadstro de papel para montar o objeto</param>
        /// <returns>Retorna um objeto preechido do Symbol com o objeto de Cadastro de papel</returns>
        public SymbolInfo CriaRegistroSymbol(DataRow pRow)
        {
            var lRetorno = new SymbolInfo();

            lRetorno.DtAtualizacao   = pRow["dt_atualizacao"].DBToDateTime();
            lRetorno.DtNegocio       = pRow["dt_negocio"].DBToDateTime();
            lRetorno.FormaCotacao    = pRow["FormaCotacao"].DBToInt32();
            lRetorno.GrupoCotacao    = pRow["GrupoCotacao"].DBToString();
            lRetorno.Instrumento     = pRow["codigoInstrumento"].DBToString();
            lRetorno.LotePadrao      = pRow["LotePadrao"].DBToInt32();

            switch( pRow["SegmentoMercado"].DBToString())
            {
                case "04":
                    lRetorno.SegmentoMercado = "OPCAO";
                    break;
                case "09":
                    lRetorno.SegmentoMercado = "OPCAO";
                    break;
                case "01":
                    lRetorno.SegmentoMercado = "AVISTA";
                    break;
                case "03":
                    lRetorno.SegmentoMercado = "FRACIONARIO";
                    break;
                case "FUT":
                    break;
            }

            lRetorno.VlrFechamento   = pRow["vl_fechamento"].DBToDecimal();
            lRetorno.VlrOscilacao    = pRow["vl_oscilacao"].DBToDecimal();
            lRetorno.VlrUltima       = pRow["vl_ultima"].DBToDecimal();

            return lRetorno;
        }
        #endregion
    }
}
