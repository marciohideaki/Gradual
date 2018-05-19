using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;

namespace Gradual.OMS.RelatoriosFinanc.Posicao
{
    public class ExtratoOperacaoDbLib
    {
        #region | Atributos

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static List<DateTime> gDataFeriados = new List<DateTime>();

        #endregion

        #region | Propriedades Bovespa
        
        private List<DateTime> GetDataFeriados
        {
            get
            {
                if (gDataFeriados.Count == 0 || gDataFeriados[gDataFeriados.Count - 1].Year != DateTime.Today.Year)
                {
                    var lAcessaDados = new AcessaDados();
                    lAcessaDados.ConnectionStringName = "Sinacor";

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_feriado_por_periodo"))
                    {
                        gDataFeriados = new List<DateTime>();

                        lAcessaDados.AddInParameter(lDbCommand, "pDtInicio", DbType.Date, new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1));
                        lAcessaDados.AddInParameter(lDbCommand, "pDtFim", DbType.Date, new DateTime(DateTime.Now.AddYears(1).Year, 1, 1));

                        var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                        gDataFeriados.Clear();

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                            foreach (DataRow lLinha in lDataTable.Rows)
                                gDataFeriados.Add(lLinha["DT_FERIADO"].DBToDateTime());
                    }
                }

                return gDataFeriados;
            }
        }

        #endregion

        #region | Métodos
        public ExtratoOperacaoResponse ObterExtratoOperacoesBovespa(ExtratoOperacaoRequest lRequest)
        {
            var lRetorno = new ExtratoOperacaoResponse();

            var lAcessaDados = new AcessaDados();

            log4net.Config.XmlConfigurator.Configure();

            try
            {
                lAcessaDados.ConnectionStringName = "CORRWIN";

                this.BuscarExtratoOrdens_Cabecalho(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarExtratoOrdens_Detalhes(ref lRequest, ref lAcessaDados, ref lRetorno);

            }
            catch (Exception ex)
            {
                gLogger.Error("ObterExtratoOperacoesBovespa", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
            }

            return lRetorno;
        }



        private void BuscarExtratoOrdens_Cabecalho(ref ExtratoOperacaoRequest pParametros, ref AcessaDados pAcessoDados, ref ExtratoOperacaoResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_EXT_BOV_CAB"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                plRetorno.RelatorioBovespa.Resultado = new List<ExtratoOperacaoCabecalhoInfo>();

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            var lExtrato = new ExtratoOperacaoCabecalhoInfo();

                            lExtrato.CodigoCliente       = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32();
                            lExtrato.DivCodigoCliente    = lDataTable.Rows[i]["DV_CLIENTE"].DBToInt32();
                            lExtrato.NomeCliente         = lDataTable.Rows[i]["NM_CLIENTE"].DBToString();
                            lExtrato.Companhia           = lDataTable.Rows[i]["NM_NOMPRE"].DBToString();
                            lExtrato.CodigoAssessor      = lDataTable.Rows[i]["CD_ASSESSOR"].DBToInt32();
                            lExtrato.Mercado             = lDataTable.Rows[i]["DS_MERCADO"].DBToString();
                            lExtrato.NomeOperador        = lDataTable.Rows[i]["NM_EMIT_ORDEM"].DBToString();
                            lExtrato.Papel               = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                            lExtrato.PessoaVinculada     = lDataTable.Rows[i]["IN_PESVIN"].DBToString();
                            lExtrato.NumeroOrdem         = lDataTable.Rows[i]["NR_SEQORD"].DBToInt32();
                            lExtrato.Status              = lDataTable.Rows[i]["IN_SITUAC"].DBToString();
                            lExtrato.PrazoVencimento     = lDataTable.Rows[i]["PCO_EXER"].DBToString();
                            lExtrato.Preco               = lDataTable.Rows[i]["VL_PREPAP"].DBToDecimal();
                            lExtrato.PrecoExercicio      = lDataTable.Rows[i]["VL_PREEXE"].DBToDecimal();
                            lExtrato.Quantidade          = lDataTable.Rows[i]["QT_ORDEM"].DBToInt32();
                            lExtrato.QuantidadeExecutada = lDataTable.Rows[i]["QT_ORDEXEC"].DBToInt32();
                            lExtrato.Saldo               = lDataTable.Rows[i]["SALDO"].DBToInt32();
                            lExtrato.TipoBolsa           = "BOVESPA";
                            lExtrato.TipoCompanhia       = lDataTable.Rows[i]["NM_ESPECI"].DBToString(); ;
                            lExtrato.TipoOrdem           = lDataTable.Rows[i]["TP_ORDEM"].DBToString();
                            lExtrato.Vencimento          = lDataTable.Rows[i]["DT_DATVEN"].DBToDateTime();
                            lExtrato.Sentido             = lDataTable.Rows[i]["CD_NATOPE"].DBToString();
                            lExtrato.TipoMercado         = lDataTable.Rows[i]["DS_MERCADO"].DBToString();

                            plRetorno.RelatorioBovespa.Resultado.Add(lExtrato);
                        }
                    }
                }
            }
        }

        private void BuscarExtratoOrdens_Detalhes(ref ExtratoOperacaoRequest pParametros, ref AcessaDados pAcessoDados, ref ExtratoOperacaoResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_EXT_BOV_ORDENS"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData",          DbType.Date,  pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    for (int i = 0;  i < plRetorno.RelatorioBovespa.Resultado.Count; i++)
                    {
                        plRetorno.RelatorioBovespa.Resultado[i].Detalhes = new List<ExtratoOperacaoDetalheInfo>();
                    }

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            var lExtratos = plRetorno.RelatorioBovespa.Resultado.FindAll(ordem =>
                            {
                                return ordem.NumeroOrdem == lDataTable.Rows[i]["NR_SEQORD"].DBToInt32();
                            });

                            ExtratoOperacaoDetalheInfo lInfo = null;

                            for (int j = 0; j < lExtratos.Count; j++)
                            {
                                lInfo = new ExtratoOperacaoDetalheInfo();

                                lInfo.Sentido           = lDataTable.Rows[i]["SENTIDO"].DBToString();
                                lInfo.CodigoContraParte = lDataTable.Rows[i]["CD_CONTRAPARTE"].DBToInt32();
                                lInfo.Data              = lDataTable.Rows[i]["DT_HOREXE"].DBToDateTime();
                                lInfo.NumeroOperacao    = lDataTable.Rows[i]["NR_NEGOCIO"].DBToInt32();
                                lInfo.Preco             = lDataTable.Rows[i]["VL_NEGOCIO"].DBToDecimal();
                                lInfo.Quantidade        = lDataTable.Rows[i]["QT_NEGOCIO"].DBToInt32();
                                lInfo.NumeroOrdem       = lDataTable.Rows[i]["NR_SEQORD"].DBToInt32();
                                
                                lExtratos[j].Detalhes.Add(lInfo);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
