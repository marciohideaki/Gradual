using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.PosicaoBTC.Lib;
using log4net;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.OMS.PosicaoBTC
{
    public class ServicoPosicaoBTC : IServicoPosicaoBTC
    {
        #region Globais

        private readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Propriedades

        public static string ConexaoSinacor { get { return "SINACOR"; } }

        #endregion
        
        #region Métodos Públicos

        public BuscarBTCLiquidadoResponse BuscarBTCLiquidado(BuscarBTCLiquidadoRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            BuscarBTCLiquidadoResponse lResponse = new BuscarBTCLiquidadoResponse();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPosicaoBTC.ConexaoSinacor;

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_liquidacao_btc"))
                {
                    lAcessaDados.AddInParameter(lCommand, "IdCliente", DbType.Int32, pRequest.IdCliente);

                    DataTable lTable = lAcessaDados.ExecuteOracleDataTable(lCommand);

                    foreach (DataRow lRow in lTable.Rows)
                    {
                        lResponse.ListaDeMovimentos.Add(new BTCLiquidadoInfo(lRow));
                    }
                }
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        public BuscarMovimentoNoDiaResponse BuscarMovimentoNoDia(BuscarMovimentoNoDiaRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            BuscarMovimentoNoDiaResponse lResponse = new BuscarMovimentoNoDiaResponse();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPosicaoBTC.ConexaoSinacor;

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_movimento_btc"))
                {
                    lAcessaDados.AddInParameter(lCommand, "CodigoCliente", DbType.Int32, pRequest.IdCliente);

                    DataTable lTable = lAcessaDados.ExecuteOracleDataTable(lCommand);

                    foreach (DataRow lRow in lTable.Rows)
                    {
                        lResponse.ListaDeMovimentos.Add(new MovimentoNoDiaInfo(lRow));
                    }
                }
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        public BuscarPosicaoConsolidadaNoDiaResponse BuscarPosicaoConsolidadaNoDia(BuscarPosicaoConsolidadaNoDiaRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            BuscarPosicaoConsolidadaNoDiaResponse lResponse = new BuscarPosicaoConsolidadaNoDiaResponse();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPosicaoBTC.ConexaoSinacor;

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_posicaoclient_btc"))
                {
                    lAcessaDados.AddInParameter(lCommand, "IdCliente", DbType.Int32, pRequest.IdCliente);

                    DataTable lTable = lAcessaDados.ExecuteOracleDataTable(lCommand);

                    foreach (DataRow lRow in lTable.Rows)
                    {
                        lResponse.ListaDeMovimentos.Add(new PosicaoConsolidadaNoDiaInfo(lRow));
                    }
                }
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        #endregion
    }
}
