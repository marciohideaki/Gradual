using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.Generico.Dados;
using System.Data.Common;
using log4net;
using Gradual.Monitores.Risco.Lib;
using System.Collections;

namespace Gradual.Monitores.Persistencia
{
    public class PersistenciaPLD
    {
        private const string gNomeConexaoSinacor      = "SINACOR";
        private const string gNomeConexaoSQL          = "Risco";
        private const string gNomeConexaoContaMargem  = "CONTAMARGEM";
        private const string gNomeConexaoSQLConfig = "Config";

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PersistenciaPLD()
        {
            //log4net.Config.XmlConfigurator.Configure();
        }

        public List<DateTime> ObterFeriadosDI()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            string procedure = "prc_obter_relacao_feriado_DI";

            List<DateTime> lFeriadoDI= new List<DateTime>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQLConfig;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, procedure))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            //string Instrumento = (lDataTable.Rows[i]["cd_codneg"]).DBToString();
                            DateTime lFeriado = (lDataTable.Rows[i]["DT_feriado"]).DBToDateTime();

                            lFeriadoDI.Add(lFeriado);
                        }
                    }

                }

                return lFeriadoDI;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public Hashtable ObterVencimentosDI()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            string procedure = "prc_obter_relacao_DI";

            Hashtable htVencimentoDI = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, procedure))
                {           
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string Instrumento = (lDataTable.Rows[i]["cd_codneg"]).DBToString();
                            DateTime Vencimento = (lDataTable.Rows[i]["DT_VENC"]).DBToDateTime();

                            htVencimentoDI.Add(Instrumento,Vencimento);
                        }
                    }

                }

                return htVencimentoDI;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public List<PLDOperacaoInfo> ObterOperacoesPLD()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<PLDOperacaoInfo> lstPLDInfo = new List<PLDOperacaoInfo>();
            PLDOperacaoInfo _PLDInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_monitor_pld"))
                {

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                    int ContadorRegistro = 0;

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _PLDInfo = new PLDOperacaoInfo();

                            _PLDInfo.STATUS = (lDataTable.Rows[i]["PLD"]).DBToString().Trim();
                            ContadorRegistro = ContadorRegistro + 1;
                            _PLDInfo.Seq = ContadorRegistro;
                            _PLDInfo.NumeroNegocio = (lDataTable.Rows[i]["NR_NEGOCIO"]).DBToInt32();
                            _PLDInfo.CodigoCliente = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();
                            _PLDInfo.Sentido = (lDataTable.Rows[i]["CD_NATOPE"]).DBToString();
                            _PLDInfo.Contraparte = (lDataTable.Rows[i]["CD_CONTRAPAR"]).DBToInt32();
                            _PLDInfo.HR_NEGOCIO = (lDataTable.Rows[i]["HR_NEGOCIO"]).DBToDateTime();
                            _PLDInfo.IntencaoPLD = (lDataTable.Rows[i]["IN_INT_PLD"]).DBToString();                          
                            _PLDInfo.Intrumento = (lDataTable.Rows[i]["CD_NEGOCIO"]).DBToString();
                            _PLDInfo.QT_CASADA = (lDataTable.Rows[i]["QT_CASADA"]).DBToInt32();
                            _PLDInfo.PrecoNegocio = (lDataTable.Rows[i]["PR_NEGOCIO"]).DBToDecimal();
                            _PLDInfo.Quantidade = (lDataTable.Rows[i]["QT_NEGOCIO"]).DBToInt32();
                            _PLDInfo.TipoRegistro = (lDataTable.Rows[i]["tp_registro"]).DBToInt32();
                            _PLDInfo.UltimaAtualizacao = DateTime.Now;

                            lstPLDInfo.Add(_PLDInfo);

                        }
                    }

                }

                return lstPLDInfo;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return lstPLDInfo;

        }
    }

}