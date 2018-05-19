using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;

namespace Gradual.BackOffice.BrokerageProcessor.Account
{
    public class AccountDigit
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private static AccountDigit _me = null;
        private static object _objTrava = new object();
        bool _accStripDigit;

        public static string BOVESPA = "BOVESPA";
        public static string BMF = "BMF";

        public static AccountDigit Instance
        {
            get
            {
                lock (_objTrava)
                {
                    if (_me == null)
                    {
                        _me = new AccountDigit();
                    }
                }
                return _me;
            }
        }

        public AccountDigit()
        {
            try
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AccountStripDigit"))
                {
                    _accStripDigit = Convert.ToBoolean(ConfigurationManager.AppSettings["AccountStripDigit"].ToString());
                }
                else
                    _accStripDigit = true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao buscar o parametro AccountDigit: " + ex.Message);
                _accStripDigit = true;
            }
        }

        public int GetAccount(int acc, string exchange)
        {
            try
            {
                int ret;
                if (!_accStripDigit)
                {
                    ret = acc;
                }
                else
                {
                    string strAcc = acc.ToString();
                    if (exchange.Equals(BOVESPA, StringComparison.CurrentCultureIgnoreCase))
                        strAcc = strAcc.Remove(strAcc.Length - 1);
                    ret = Convert.ToInt32(strAcc);
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao parsear a conta: " + ex.Message);
                return 0;
            }
        }

        public int GetAccount(string acc, string exchange)
        {
            int xx;
            if (int.TryParse(acc, out xx))
                return this.GetAccount(xx, exchange);
            return 0;

        }

        public int CalculaDV(int CodigoCorretora, int CodigoCliente)
        {

            int valor = 0;
            valor = (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            valor = valor % 11;

            if (valor == 0 || valor == 1)
            {
                valor = 0;
            }
            else
            {
                valor = 11 - valor;
            }

            return int.Parse(string.Format("{0}{1}", CodigoCliente, valor));

        }

        public int GetAccountWithDigit(int account, string exchange)
        {
            try
            {
                int ret = 0;
                if (!_accStripDigit)
                {
                    ret = account;
                }
                else
                {
                    if (exchange.Equals(BOVESPA, StringComparison.CurrentCultureIgnoreCase))
                        ret = this.CalculaDV(227, account);
                    else
                        ret = account;
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao parsear a conta CalculatingDigit: " + ex.Message, ex);
                return 0;
            }
        }

        public int GetAccountWithDigit(string acc, string exchange)
        {
            int xx;
            if (int.TryParse(acc, out xx))
                return this.GetAccountWithDigit(xx, exchange);
            return 0;
        } 
    }
}
