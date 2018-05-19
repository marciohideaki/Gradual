using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;

namespace OpenBlotterLib
{
    public class OracleConvert
    {
        public static int _IndexCampo(string strNomeCampo, OracleDataReader objDataReader)
        {
            return objDataReader.GetOrdinal(strNomeCampo);
        }

        public static char GetChar(string strNomeCampo, OracleDataReader objDataReader)
        {
            try
            {
                char[] Caracter = new char[1];
                objDataReader.GetChars(_IndexCampo(strNomeCampo, objDataReader), 0, Caracter, 0, 1);
                return Caracter[0];
            }
            catch { }
            return (char)0;
        }

        public static string GetString(string strNomeCampo, OracleDataReader objDataReader)
        {
            try
            {
                int i = _IndexCampo(strNomeCampo, objDataReader);
                if (!objDataReader.IsDBNull(i))
                    return objDataReader.GetString(i);
            }
            catch { }
            return "";
        }

        public static int GetInt(string strNomeCampo, OracleDataReader objDataReader)
        {
            try
            {
                return objDataReader.GetInt32(_IndexCampo(strNomeCampo, objDataReader));
            }
            catch { }
            return 0;
        }

        public static long GetLong(string strNomeCampo, OracleDataReader objDataReader)
        {
            try
            {
                int i = _IndexCampo(strNomeCampo, objDataReader);
                if (!objDataReader.IsDBNull(i))
                    return objDataReader.GetInt64(i);
            }
            catch { }
            return 0;
        }

        public static DateTime GetDateTime(string strNomeCampo, OracleDataReader objDataReader)
        {
            try
            {
                return objDataReader.GetDateTime(_IndexCampo(strNomeCampo, objDataReader));
            }
            catch { }
            return DateTime.MinValue;
        }

        public static double GetDouble(string strNomeCampo, OracleDataReader objDataReader)
        {
            try
            {
                return objDataReader.GetDouble(_IndexCampo(strNomeCampo, objDataReader));
            }
            catch { }
            return (double)0;
        }

        public static bool GetBool(string strNomeCampo, OracleDataReader objDataReader)
        {
            try
            {
                return objDataReader.GetBoolean(_IndexCampo(strNomeCampo, objDataReader));
            }
            catch { }
            return false;
        }

        public static decimal GetNumber(string strNomeCampo, OracleDataReader objDataReader)
        {
            try
            {
                return objDataReader.GetOracleNumber(_IndexCampo(strNomeCampo, objDataReader)).Value;
            }
            catch { }
            return (decimal)0;
        }
    }
}
