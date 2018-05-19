using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using log4net;
using System.IO;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.ClientePremium.DB
{
    public class EstornoDB
    {
        #region Propriedades
        private const string _ConnStringSinacor = "SINACOR";
        private const string _ConnStringControleAcesso = "ControleAcesso";
        private readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private string gValorCustodiaEstorno
        {
            get
            {
                return ConfigurationManager.AppSettings["ValorCustodiaEstorno"].ToString();
            }
        }
        #endregion

        #region Construtores
        public EstornoDB()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion
        
        #region Métodos
        
        /// <summary>
        /// Seleciona o próximo dia útil para 
        /// </summary>
        /// <param name="pRange">-1 - Para dia anterior ,0 - para atual, 1 - para Próximo</param>
        /// <returns>Retorno o próximo datetime com o próximo dia útil a partir da data corrente</returns>
        private DateTime BuscarProximoDiaUtil()
        {
            DateTime lRetorno = DateTime.Now;

            AcessaDados acesso = new AcessaDados();

            acesso.CursorRetorno = "Retorno";

            acesso.ConnectionStringName = _ConnStringSinacor;

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_PROXIMO_DIA_UTIL_SEL"))
            {
                //acesso.AddInParameter(cmd, "pDt_operacao", DbType.DateTime, pData);

                DataTable lTable = acesso.ExecuteOracleDataTable(cmd);

                foreach (DataRow row in lTable.Rows)
                {
                    lRetorno = Convert.ToDateTime(row["dProxDia"]);
                }
            }

            return lRetorno;
        }

        #endregion

        #region SetarEstornoEmitidoCliente
        private void SetarEstornoEmitidoCliente(int pCd_cliente, DateTime pDt_insercao)
        {
            AcessaDados lAcesso = new AcessaDados();

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_cliente_seta_custodia_estorno_emitido_upd"))
            {
                lAcesso.AddInParameter(cmd, "@cd_cliente", DbType.Int32, pCd_cliente);

                lAcesso.AddInParameter(cmd, "@dt_insercao", DbType.DateTime, pDt_insercao);

                lAcesso.ExecuteNonQuery(cmd);

                gLogger.InfoFormat("Setando estorno Emitido para o cliente .....{0} - Data inserção {1}", pCd_cliente, pDt_insercao);
            }

        }
        #endregion

        #region MontaDetalheArquivo
        public List<string> MontaDetalheArquivoEstornoCustodia()
        {
            DateTime lDataEstorno = DateTime.Now;

            lDataEstorno = this.BuscarProximoDiaUtil();

            StringBuilder lDetalhe = new StringBuilder();

            AcessaDados lAcesso = new AcessaDados();

            string lValorEstorno = gValorCustodiaEstorno;

            List<string> lRetorno = new List<string>();

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            gLogger.InfoFormat("Montando o arquivo de estorno de custódia");

            using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_cliente_custodia_apos_dia_20_alterada_sel"))
            {
                DataTable table = lAcesso.ExecuteDbDataTable(cmd);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow lRow in table.Rows)
                    {
                        gLogger.InfoFormat("Inserindo cliente ..................{0} no arquivo de estorno.", Convert.ToInt32(lRow["cd_cliente"]));

                        lDetalhe.Length = 0;

                        lDetalhe.Append("01");                                                             //-- Tipo de registro FIXO '01'                                                                  

                        lDetalhe.Append(lDataEstorno.ToString("dd/MM/yyyy"));                              //-- Data vencimento dd/mm/yyyy                                                                  

                        lDetalhe.Append(lRow["cd_cliente"].ToString().PadLeft(7, '0'));                    //-- Código do cliente '7'                                                                  

                        lDetalhe.Append("0152");                                                           //-- Histórico: está com 152 mas será outro número                                                                  

                        lDetalhe.Append(lValorEstorno.PadLeft(15, '0'));                                   //-- Lançamento

                        lDetalhe.Append(string.Empty.PadLeft(94, ' '));

                        lDetalhe.Append(string.Empty.PadLeft(95, ' '));

                        lDetalhe.Append("OUTNOUT 000000000000000");

                        lRetorno.Add(lDetalhe.ToString());

                        this.SetarEstornoEmitidoCliente(Convert.ToInt32(lRow["cd_cliente"]), Convert.ToDateTime(lRow["dt_insercao"]));
                    }
                }
            }

            return lRetorno;
        }
        #endregion
    }
}
