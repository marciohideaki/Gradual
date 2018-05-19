using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using log4net;
using System.Data.Common;
using System.Data;

namespace Gradual.OMS.RelatoriosFinanc.Posicao
{
    public class MonitorRiscoDbLib
    {
        #region Atributos
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string gNomeConexaoOracle = "Sinacor";
        #endregion

        #region Propriedades
        private MonitorRiscoResponse gRetorno { get; set;}
        #endregion

        #region Métodos
        public MonitorRiscoResponse ConsultarDadosMonitorRisco(MonitorRiscoRequest pParametros)
        {
            gRetorno = new MonitorRiscoResponse();

            this.ConsultarMargemRequeridaBMF(pParametros);

            this.ConsultarGarantiaBovespa(pParametros);

            gRetorno.DescricaoResposta = string.Format("Posição em Monitor de Risco do cliente: {0} carregado com sucesso", pParametros.CodigoCliente.DBToString());
            gRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

            return gRetorno;
        }

        private void ConsultarMargemRequeridaBMF(MonitorRiscoRequest pParametros)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOCLIENTE_BMF"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, pParametros.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        gRetorno.Relatorio.ValorMargemRequerida = lDataTable.Rows[i]["VL_TOTMAR"].DBToDecimal();
                    }
                }
            }
        }

        private void ConsultarGarantiaBovespa(MonitorRiscoRequest pParametros)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_GARANTIA_BOV"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, pParametros.CodigoCliente);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        gRetorno.Relatorio.ValorGarantiaDeposito = lDataTable.Rows[i]["VALO_GARN_DEPO"].DBToDecimal();
                        gRetorno.Relatorio.DataMovimentoGarantia = lDataTable.Rows[i]["DATA_MVTO"].DBToDateTime();
                    }
                }
            }
        }

        #endregion
    }
}
