using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.BackOffice.BrokerageProcessor.Db
{
    public class DbEmailOracle
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        public DbEmailOracle()
        {

        }


        public Dictionary<int, List<string>> BuscarClienteBovespa()
        {
            Dictionary<int, List<string>> ret = new Dictionary<int, List<string>>();
            try
            {
                AcessaDados acesso = new AcessaDados("Retorno");
                acesso.ConnectionStringName = "TRADE";
                // Buscar Posicao de cliente bovespa
                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_OBTER_CLI_EMAIL_BOVESPA"))
                {
                    DataTable table = acesso.ExecuteOracleDataTable(cmd);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow dataRow = table.Rows[i];
                        List<string> lstEmail = null;
                        int account = dataRow["CD_CLIENTE"].DBToInt32();
                        if (ret.TryGetValue(account, out lstEmail))
                        {
                            string mail = dataRow["NM_E_MAIL"].DBToString();
                            string aux = lstEmail.FirstOrDefault(x => x.Equals(mail, StringComparison.CurrentCultureIgnoreCase));
                            if (string.IsNullOrEmpty(aux))
                                lstEmail.Add(mail);
                        }
                        else
                        {
                            List<string> lst = new List<string>();
                            lst.Add(dataRow["NM_E_MAIL"].DBToString());
                            ret.Add(account, lst);
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na consulta de Emails de cliente Bovespa: " + ex.Message, ex);
                return null;
            }
        }

        public Dictionary<int, List<string>> BuscarClienteBmf()
        {
            Dictionary<int, List<string>> ret = new Dictionary<int, List<string>>();
            try
            {
                AcessaDados acesso = new AcessaDados("Retorno");
                acesso.ConnectionStringName = "TRADE";
                // Buscar Posicao de cliente bovespa
                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_OBTER_CLI_EMAIL_BMF"))
                {
                    DataTable table = acesso.ExecuteOracleDataTable(cmd);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow dataRow = table.Rows[i];
                        List<string> lstEmail = null;
                        int account = dataRow["CD_CLIENTE"].DBToInt32();
                        if (ret.TryGetValue(account, out lstEmail))
                        {
                            string mail = dataRow["NM_E_MAIL"].DBToString();
                            string aux = lstEmail.FirstOrDefault(x => x.Equals(mail, StringComparison.CurrentCultureIgnoreCase));
                            if (string.IsNullOrEmpty(aux))
                                lstEmail.Add(mail);
                        }
                        else
                        {
                            List<string> lst = new List<string>();
                            lst.Add(dataRow["NM_E_MAIL"].DBToString());
                            ret.Add(account, lst);
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na consulta de Emails de cliente Bovespa: " + ex.Message, ex);
                return null;
            }
        }
    }
}
