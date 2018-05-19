using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.OMS.Library;
using System.Data.Common;
using Gradual.OMS.CadastroPapeis.Lib;
using log4net;
using Gradual.Generico.Dados;
using Newtonsoft.Json;

namespace Gradual.OMS.CadastroPapeis.Entidades
{
    class CadastroPapeisMdsDbLib
    {
        #region Properties

        public string _ConnectionStringName = null;

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Métodos
        /// <summary>
        /// Retorna uma lista de todos os papeis cadastrados do MDS
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, CadastroPapelMdsInfo> ListarCadastroPapeisMDS()
        {
            SqlConnection conn = null;
            String procedure = "prc_lst_tbCadastroPapel";

            logger.Debug("Preparando Solicitacao " + procedure);

            Dictionary<string, CadastroPapelMdsInfo> ret = new Dictionary<string, CadastroPapelMdsInfo>();
            SqlCommand command = new SqlCommand();
            try
            {
                conn = new SqlConnection(_ConnectionStringName);
                conn.Open();

                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = procedure;

                DataTable dtDados = new DataTable();
                DateTime dataRegistro = DateTime.Now;

                // Obter todos os instrumentos atualizados de até 5 dias atrás, e não apenas da data atual
                dataRegistro = dataRegistro.AddDays(-5);

                do
                {
                    command.Parameters.AddWithValue("@DataRegistro", dataRegistro);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtDados);

                    if (dtDados.Rows.Count == 0)
                    {
                        command.Parameters.RemoveAt("@DataRegistro");
                        dataRegistro = dataRegistro.AddDays(-1);
                    }
                }
                while (dtDados.Rows.Count == 0);

                foreach (DataRow registro in dtDados.Rows)
                {
                    CadastroPapelMdsInfo info = new CadastroPapelMdsInfo();
                    info.Instrumento = registro["CodigoInstrumento"].ToString();
                    info.RazaoSocial = registro["RazaoSocial"].ToString();
                    info.GrupoCotacao = registro["GrupoCotacao"].ToString();
                    info.FormaCotacao = (registro["FormaCotacao"].ToString().Equals("") ? 0 : Convert.ToInt32(registro["FormaCotacao"].ToString()));
                    info.DataUltimoNegocio = (registro["DataUltimoNegocio"].ToString().Equals("") ? DateTime.MinValue : Convert.ToDateTime(registro["DataUltimoNegocio"].ToString()));
                    info.LotePadrao = (registro["LotePadrao"].ToString().Equals("") ? 0 : Convert.ToInt32(registro["LotePadrao"].ToString()));
                    info.IndicadorOpcao = registro["IndicadorOpcao"].ToString();
                    info.PrecoExercicio = (registro["PrecoExercicio"].ToString().Equals("") ? 0 : Convert.ToDouble(registro["PrecoExercicio"].ToString()));
                    info.DataVencimento = (registro["DataVencimento"].ToString().Equals("") ? DateTime.MinValue : Convert.ToDateTime(registro["DataVencimento"].ToString()));
                    info.CodigoPapelObjeto = registro["CodigoPapelObjeto"].ToString();
                    info.SegmentoMercado = registro["SegmentoMercado"].ToString();
                    info.CoeficienteMultiplicacao = (registro["CoeficienteMultiplicacao"].ToString().Equals("") ? 0 : Convert.ToDouble(registro["CoeficienteMultiplicacao"].ToString()));
                    info.DataRegistro = (registro["DataRegistro"].ToString().Equals("") ? DateTime.MinValue : Convert.ToDateTime(registro["DataRegistro"].ToString()));
                    info.CodigoISIN = registro["CodigoISIN"].ToString();

                    ret.Add(info.Instrumento, info);
                }
            }
            catch (Exception e)
            {
                logger.Error("Erro procedure[" + procedure + "] Mensagem: [" + e.Message + "]", e);
            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }
            logger.Debug("Preparando Resposta " + procedure + " qtdItens[" + ret.Count + "]");
            return ret;
        }

        /// <summary>
        /// Retorna uma lista de todos os indices e suas composições
        /// </summary>
        /// <returns></returns>
        public List<string> ListarComposicaoIndicesMDS()
        {
            SqlConnection conn = null;
            String procedure = "prc_lst_tbCadastroPapel_indices";

            logger.Debug("Preparando Solicitacao " + procedure);

            List<string> ret = new List<string>();
            SqlCommand command = new SqlCommand();
            try
            {
                conn = new SqlConnection(_ConnectionStringName);
                conn.Open();

                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = procedure;

                DataTable dtDados = new DataTable();
                DateTime dataRegistro = DateTime.Now;

                // Obter todos os instrumentos atualizados de até 5 dias atrás, e não apenas da data atual
                dataRegistro = dataRegistro.AddDays(-5);

                do
                {
                    command.Parameters.AddWithValue("@DataRegistro", dataRegistro);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtDados);

                    if (dtDados.Rows.Count == 0)
                    {
                        command.Parameters.RemoveAt("@DataRegistro");
                        dataRegistro = dataRegistro.AddDays(-1);
                    }
                }
                while (dtDados.Rows.Count == 0);

                foreach (DataRow registro in dtDados.Rows)
                {
                    ret.Add(registro["CodigoInstrumento"].ToString());
                }
            }
            catch (Exception e)
            {
                logger.Error("Erro procedure[" + procedure + "] Mensagem: [" + e.Message + "]", e);
            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }
            logger.Debug("Preparando Resposta " + procedure + " qtdItens[" + ret.Count + "]");
            return ret;
        }

        /// <summary>
        /// Retorna um papel cadastrado no MDS
        /// </summary>
        /// <returns></returns>
        public CadastroPapelMdsInfo BuscarPapelCadastradoMds(string instrumento)
        {
            SqlConnection conn = null;
            String procedure = "prc_sel_tbCadastroPapel";

            logger.Debug("Preparando Solicitacao " + procedure + " [" + instrumento + "]");

            CadastroPapelMdsInfo ret = new CadastroPapelMdsInfo();
            SqlCommand command = new SqlCommand();
            try
            {
                conn = new SqlConnection(_ConnectionStringName);
                conn.Open();

                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = procedure;

                DateTime dataRegistro = DateTime.Now;

                // Obter todos os instrumentos atualizados de até 5 dias atrás, e não apenas da data atual
                dataRegistro = dataRegistro.AddDays(-5);

                DataTable dtDados = new DataTable();

                command.Parameters.AddWithValue("@Instrumento", instrumento);
                command.Parameters.AddWithValue("@DataRegistro", dataRegistro);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtDados);

                foreach (DataRow registro in dtDados.Rows)
                {
                    ret.Instrumento = registro["CodigoInstrumento"].ToString();
                    ret.RazaoSocial = registro["RazaoSocial"].ToString();
                    ret.GrupoCotacao = registro["GrupoCotacao"].ToString();
                    ret.FormaCotacao = (registro["FormaCotacao"].ToString().Equals("") ? 0 : Convert.ToInt32(registro["FormaCotacao"].ToString()));
                    ret.DataUltimoNegocio = (registro["DataUltimoNegocio"].ToString().Equals("") ? DateTime.MinValue : Convert.ToDateTime(registro["DataUltimoNegocio"].ToString()));
                    ret.LotePadrao = (registro["LotePadrao"].ToString().Equals("") ? 0 : Convert.ToInt32(registro["LotePadrao"].ToString()));
                    ret.IndicadorOpcao = registro["IndicadorOpcao"].ToString();
                    ret.PrecoExercicio = (registro["PrecoExercicio"].ToString().Equals("") ? 0 : Convert.ToDouble(registro["PrecoExercicio"].ToString()));
                    ret.DataVencimento = (registro["DataVencimento"].ToString().Equals("") ? DateTime.MinValue : Convert.ToDateTime(registro["DataVencimento"].ToString()));
                    ret.CodigoPapelObjeto = registro["CodigoPapelObjeto"].ToString();
                    ret.SegmentoMercado = registro["SegmentoMercado"].ToString();
                    ret.CoeficienteMultiplicacao = (registro["CoeficienteMultiplicacao"].ToString().Equals("") ? 0 : Convert.ToDouble(registro["CoeficienteMultiplicacao"].ToString()));
                    ret.DataRegistro = (registro["DataRegistro"].ToString().Equals("") ? DateTime.MinValue : Convert.ToDateTime(registro["DataRegistro"].ToString()));
                    ret.CodigoISIN = registro["CodigoISIN"].ToString();
                }
            }
            catch (Exception e)
            {
                logger.Error("Erro procedure[" + procedure + "] Mensagem: [" + e.Message + "]", e);
            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }
            logger.Debug("Preparando Resposta " + procedure);
            return ret;
        }
        #endregion
    }
}
