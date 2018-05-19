using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Gradual.RoboDownloadFundos.Lib.Dados;
using Gradual.RoboDownloadFundos.Lib;
using log4net;
using System.Globalization;
using System.Data.OracleClient;

namespace Gradual.RoboDownloadFundos
{
    public class PersistenciaDB
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        CultureInfo ciBr = CultureInfo.CreateSpecificCulture("pt-Br");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origemDnl"></param>
        /// <returns></returns>
        public List<FundoInfo> ObterFundos( OrigemDownloadEnum origemDnl )
        {
            List<FundoInfo> lRetorno = new List<FundoInfo>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            int iOrigDnl = (int)origemDnl;

            string sqlQuery = "SELECT * FROM tbFundos WHERE idOrigem=" + iOrigDnl;

            SqlCommand sqlCmd = new SqlCommand( sqlQuery , sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    FundoInfo info = new FundoInfo();

                    info.CodFundo = table.Rows[i]["codFundo"].ToString();
                    info.DsFundo = table.Rows[i]["dsFundo"].ToString();
                    info.IDFundo = Convert.ToInt32(table.Rows[i]["idFundo"].ToString());
                    info.IDOrigem = Convert.ToInt32(table.Rows[i]["idOrigem"].ToString());
                    info.GrupoCarteira = table.Rows[i]["grpCarteira"].ToString();
                    info.IsSegregada = table.Rows[i]["isSegregada"].ToString();

                    lRetorno.Add(info);
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="origemDnl"></param>
        /// <returns></returns>
        public List<CotistaInfo> ObterCotistas()
        {
            List<CotistaInfo> lRetorno = new List<CotistaInfo>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            string sqlQuery = "SELECT * FROM tbCotista";

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    CotistaInfo info = new CotistaInfo();

                    info.CodCotista = table.Rows[i]["codCotista"].ToString();
                    info.CpfCnpj = table.Rows[i]["cpfCnpj"].ToString();
                    info.FlagIsencaoIR= table.Rows[i]["flgIsencaoIR"].ToString().ToUpperInvariant().Equals("S");
                    info.IDCotista = table.Rows[i]["idCotista"].DBToInt32();
                    info.NomeCotista = table.Rows[i]["nomeCotista"].ToString();
                    info.Operador = table.Rows[i]["dsOperador"].ToString();
                    info.TipoPessoaCotista = table.Rows[i]["tpPessoaCotista"].ToString();

                    lRetorno.Add(info);
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origemDnl"></param>
        /// <returns></returns>
        public Dictionary<int, LogDownloadInfo> ObterLogDownload(OrigemDownloadEnum origemDnl, CategoriaDownloadEnum categoriaDnl, DateTime dataIni)
        {
            Dictionary<int, LogDownloadInfo> lRetorno = new Dictionary<int, LogDownloadInfo>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            int iOrigDnl = (int) origemDnl;
            int iCategDnl = (int) categoriaDnl;

            string sqlQuery = "SELECT * FROM tbArquivoDownload WHERE idOrigem=" + iOrigDnl;
            sqlQuery += " AND idCategoria=" + iCategDnl;
            sqlQuery += " AND DATEADD(D, 0, DATEDIFF(D, 0, dtReferencia)) = DATEADD(D, 0, DATEDIFF(D, 0, @dataRef)) ";

            logger.Debug("sqlQuery = [" + sqlQuery + "] dataRef=" + dataIni.ToString("yyyy/MM/dd HH:mm:ss.fff") );

            //DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.Parameters.AddWithValue("@dataRef", dataIni);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    LogDownloadInfo info = new LogDownloadInfo();

                    info.IDDownloadTransacao = Convert.ToInt32(table.Rows[i]["idDownloadTransacao"].ToString());
                    info.IDCategoria =  Convert.ToInt32(table.Rows[i]["idCategoria"].ToString());
                    info.IDFundo = Convert.ToInt32(table.Rows[i]["idFundo"].ToString());
                    info.cpfCnpj =table.Rows[i]["cpfCnpj"].ToString();
                    info.dtReferencia = table.Rows[i]["dtReferencia"].DBToDateTime();
                    info.dtUltimaTentativa = table.Rows[i]["dtUltimaTentativa"].DBToDateTime();
                    info.stTransacao =table.Rows[i]["stTransacao"].DBToString();
                    info.pathArquivo = table.Rows[i]["pathArquivo"].DBToString();
                    info.IDOrigem = Convert.ToInt32(table.Rows[i]["idOrigem"].ToString());
                    info.numTentativas = Convert.ToInt32(table.Rows[i]["numTentativas"].ToString());

                    lRetorno.Add(info.IDFundo, info);
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }

        public int InserirLogDownload(OrigemDownloadEnum origemDnl, CategoriaDownloadEnum categoriaDnl, DateTime? dataRef, int idFundo, string cpfcnpj=null)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            int iOrigDnl = (int)origemDnl;
            int iCategDnl = (int)categoriaDnl;

            string cpf = String.Empty;

            if (!String.IsNullOrEmpty(cpfcnpj))
                cpf = cpfcnpj;

            string sqlQuery = "INSERT INTO tbArquivoDownload (idOrigem, idCategoria, idFundo, dtReferencia, stTransacao, numTentativas, cpfCnpj) VALUES ";
            sqlQuery += " (" + iOrigDnl.ToString() + ", ";
            sqlQuery += iCategDnl.ToString() + ", ";
            sqlQuery += idFundo.ToString() + ", ";
            if ( dataRef != null )
                sqlQuery += " @dataRef, 'N', 0, ";
            else
                sqlQuery += " GETDATE(), 'N', 0, ";
            sqlQuery += "'" + cpf + "')";
            sqlQuery += "SELECT SCOPE_IDENTITY()";


            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            if ( dataRef != null )
                sqlCmd.Parameters.AddWithValue("@dataRef", dataRef);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            int ident = (int) (decimal) sqlCmd.ExecuteScalar();

            sqlConn.Close();

            sqlConn.Dispose();

            return ident;
        }


        public void AtualizarLogDownload(int idDownload, string pathPDF, bool bRet, string msgErro)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            string sqlQuery = "UPDATE tbArquivoDownload SET";
            sqlQuery += " stTransacao='" + (bRet?"S":"N") + "', ";
            sqlQuery += " pathArquivo='" + pathPDF + "', ";
            sqlQuery += " dtUltimaTentativa=GETDATE(),  ";
            sqlQuery += " numTentativas = numTentativas+1 ";
            if ( !String.IsNullOrEmpty(msgErro) )
                sqlQuery += ", dsMsgErro = '" + msgErro + "'";

            sqlQuery += " WHERE idDownloadTransacao = " + idDownload;

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            sqlCmd.ExecuteNonQuery();

            sqlConn.Close();

            sqlConn.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idFundo"></param>
        /// <param name="total"></param>
        public void InserirTotalTitulosBaixados(int idFundo, decimal total)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            SqlCommand sqlCmd = new SqlCommand("prc_ins_total_titulo_liquidado", sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@idFundo", idFundo);
            sqlCmd.Parameters.AddWithValue("@vlTotalLiquidado", total);
            sqlCmd.Parameters.AddWithValue("@dtReferencia", DateTime.Now);

            sqlCmd.ExecuteNonQuery();

            sqlConn.Close();

            sqlConn.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origemDnl"></param>
        /// <returns></returns>
        public string ObterPathFrontis()
        {
            string lRetorno = String.Empty;

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Frontis"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            string sqlQuery = "SELECT * FROM TB_GLOBAL_CONFIG WHERE ID_GLOBAL_CONFIG=1";

            logger.Debug("sqlQuery = [" + sqlQuery + "]");


            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                lRetorno = table.Rows[0]["DS_PATH_PKCS7"].ToString();
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origemDnl"></param>
        /// <returns></returns>
        public Dictionary<string, FrontisFundoInfo> ObterFundosFrontis()
        {
            Dictionary<string, FrontisFundoInfo> lRetorno = new Dictionary<string,FrontisFundoInfo>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Frontis"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            string sqlQuery = "SELECT * FROM TB_FUNDO";

            logger.Debug("sqlQuery = [" + sqlQuery + "]");


            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    FrontisFundoInfo info = new FrontisFundoInfo();

                    info.CNPJ = table.Rows[i]["NU_CNPJ"].ToString();
                    info.ISIN = table.Rows[i]["CODIGO_ISIN"].ToString();

                    lRetorno.Add(info.CNPJ, info);
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origemDnl"></param>
        /// <returns></returns>
        public List<FrontisDownloadInfo> ObterControleDownloadsFrontis()
        {
            List<FrontisDownloadInfo> lRetorno = new List<FrontisDownloadInfo>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            string sqlQuery = "SELECT * FROM tbControleArquivosFrontis";

            logger.Debug("sqlQuery = [" + sqlQuery + "]");


            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    FrontisDownloadInfo info = new FrontisDownloadInfo();

                    info.Cnpj = table.Rows[i]["cnpj"].ToString();
                    info.NomeArquivo = table.Rows[i]["nmarquivo"].ToString();
                    info.DataDownload = table.Rows[i]["dt_download"].DBToDateTime();

                    lRetorno.Add( info);
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idFundo"></param>
        /// <param name="total"></param>
        public void InserirControleFrontis(string cnpj, string nomearquivo)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            string sqlQuery = "INSERT INTO tbControleArquivosFrontis (cnpj, nmarquivo, dt_download) VALUES ";
            sqlQuery += String.Format("('{0}','{1}',GETDATE())", cnpj, nomearquivo);

            logger.Debug("sQuery [" + sqlQuery + "]");

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandText = sqlQuery;
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.ExecuteNonQuery();

            sqlConn.Close();

            sqlConn.Dispose();
        }

        public List<FundoLaminaPerfil> ObterFundosLaminaPefil()
        {
            List<FundoLaminaPerfil> lRetorno = new List<FundoLaminaPerfil>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            string sqlQuery = "SELECT * FROM tb_rbdnl_fundos_lamina_perfil";

            logger.Debug("sqlQuery = [" + sqlQuery + "]");

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    FundoLaminaPerfil info = new FundoLaminaPerfil();

                    info.CpfCpnj = table.Rows[i]["cnpj_fundo"].DBToString().Trim();
                    info.DsFundo = table.Rows[i]["ds_fundo"].DBToString().Trim();
                    info.IdCarteiraFinancial = table.Rows[i]["id_carteira_financial"].DBToInt32();
                    info.FlagSQL = table.Rows[i]["flg_sql"].DBToInt32() == 1;
                    info.FlagSinacor = table.Rows[i]["flg_sinacor"].DBToInt32() == 1;
                    info.FlagPerfilMensal = table.Rows[i]["flg_perfil_mensal"].DBToInt32() == 1;
                    info.FlagLamina = table.Rows[i]["flg_lamina"].DBToInt32() == 1;

                    lRetorno.Add( info);
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }


        public Dictionary<string, CotistaCarteiraInfo> ObterCotistaFundo(int idCarteiraFinancial, DateTime dataMovto)
        {
            Dictionary<string, CotistaCarteiraInfo> lRetorno = new Dictionary<string, CotistaCarteiraInfo>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["FINANCIAL"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();
    
            string sqlQuery = "";
            sqlQuery += " SELECT A.IDCOTISTA, C.NOME, P.CPFCNPJ, ";
            sqlQuery += " A.IDCARTEIRA, P.IDPESSOA, ";
            sqlQuery += " SUM(QUANTIDADE) AS QTDECOTAS ";
            sqlQuery += " FROM POSICAOCOTISTA A, PESSOA P, COTISTA C";
            sqlQuery += " WHERE ";
            sqlQuery += " A.IDCARTEIRA in (" + idCarteiraFinancial +  ") ";
            sqlQuery += " AND C.IDCOTISTA = A.IDCOTISTA ";
            sqlQuery += " AND P.IDPESSOA = C.IDPESSOA ";
            sqlQuery += " GROUP BY ";
            sqlQuery += " A.IDCOTISTA,C.NOME, P.CPFCNPJ, A.IDCARTEIRA, P.IDPESSOA ";
            sqlQuery += " ORDER BY A.IDCARTEIRA, A.IDCOTISTA ";

            logger.Debug("sqlQuery = [" + sqlQuery + "]");

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@dataHistorica", dataMovto);
            
            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            CotistaCarteiraInfo info = null;
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    info = new CotistaCarteiraInfo();

                    info.IDCotista = row["IDCotista"].DBToInt32();
                    info.DataHistorico = dataMovto;
                    info.IDCarteira = row["IDCarteira"].DBToInt32();
                    info.IDPessoa = row["IDPessoa"].DBToInt32();
                    info.NomeCotista = row["Nome"].DBToString();
                    info.CpfCnpj = row["CpfCnpj"].DBToString();

                    string qtdeCotas = row["QtdeCotas"].DBToDouble().Value.ToString(ciBr).TruncaDecimais(4);

                    info.QtdeCotas = Convert.ToDouble(qtdeCotas, ciBr);

                    if (!lRetorno.ContainsKey(info.CpfCnpj))
                    {
                        lRetorno.Add(info.CpfCnpj, info);
                    }
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }


        public void PopulateCotistaInfoSinacor( ref Dictionary<int, CotistaInfo> dicCotistas ) 
        {
            OracleConnection objORAConnection = new OracleConnection();

            objORAConnection.ConnectionString = ConfigurationManager.ConnectionStrings["TRADE"].ConnectionString;

            objORAConnection.Open();

            string sqlQuery = "";
            sqlQuery += " SELECT DISTINCT(TSCCLIGER.CD_CPFCGC) AS CD_CPFCGC, ";
            sqlQuery += " TSCCLIGER.IN_SITUAC AS IN_SITUAC, ";
            sqlQuery += " TSCCLIGER.NM_CLIENTE AS NM_CLIENTE, ";
            sqlQuery += " TSCCLIGER.TP_PESSOA AS TP_PESSOA, ";
            sqlQuery += " TSCCLIGER.TP_CLIENTE AS TP_CLIENTE, ";
            sqlQuery += " TSCTIPCLI.DS_TIPO_CLIENTE AS DS_TIPO_CLIENTE ";
            sqlQuery += " FROM TSCCLIGER, TSCTIPCLI ";
            sqlQuery += " WHERE TSCCLIGER.TP_CLIENTE = TSCTIPCLI.TP_CLIENTE ";


            Dictionary<string, CotistaInfo> dctPessoasSinacor = new Dictionary<string,CotistaInfo>();

            using (OracleCommand objORACommand = objORAConnection.CreateCommand())
            {
                objORACommand.CommandText = sqlQuery;

                OracleDataReader odr = objORACommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (odr.HasRows)
                {
                    while (odr.Read())
                    {
                        try
                        {
                            CotistaInfo info = new CotistaInfo(); 

                            info.CpfCnpj = OracleConvert.GetNumber("CD_CPFCGC", odr).ToString();
                            info.NomeCotista = OracleConvert.GetString("NM_CLIENTE", odr);
                            info.TipoPessoaCotista = OracleConvert.GetString("TP_PESSOA", odr);
                            info.TipoClienteSinacor = OracleConvert.GetInt("TP_CLIENTE", odr);

                            if (!dctPessoasSinacor.ContainsKey(info.CpfCnpj))
                            {
                                dctPessoasSinacor.Add(info.CpfCnpj, info);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                foreach (CotistaInfo cotista in dicCotistas.Values)
                {
                    if (dctPessoasSinacor.ContainsKey(cotista.CpfCnpj))
                    {
                        cotista.TipoClienteSinacor = dctPessoasSinacor[cotista.CpfCnpj].TipoClienteSinacor;
                        cotista.TipoPessoaCotista = dctPessoasSinacor[cotista.CpfCnpj].TipoPessoaCotista;
                    }
                    else
                    {
                        cotista.TipoClienteSinacor = 99;
                        cotista.TipoPessoaCotista = "INCERTO";
                    }

                }
            }
        }



        public Dictionary<int, TipoPerfilClienteInfo> ObterTiposPerfilCliente()
        {
            Dictionary<int, TipoPerfilClienteInfo> lRetorno = new Dictionary<int, TipoPerfilClienteInfo>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            string sqlQuery = "";
            sqlQuery += "SELECT * FROM tb_rbdnl_tipos_perfil_cliente ";
            sqlQuery += " ORDER BY tp_cliente_sinacor ";

            logger.Debug("sqlQuery = [" + sqlQuery + "]");

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    TipoPerfilClienteInfo info = new TipoPerfilClienteInfo();

                    info.TipoClienteSinacor = table.Rows[i]["tp_cliente_sinacor"].DBToInt32();
                    info.DescPerfil = table.Rows[i]["ds_perfil"].DBToString();
                    info.IDPerfil = table.Rows[i]["id_perfil"].DBToInt32();
                    info.SufixoTagXML = table.Rows[i]["ds_tag_xml_perfil"].DBToString();

                    if (!lRetorno.ContainsKey(info.TipoClienteSinacor))
                        lRetorno.Add(info.TipoClienteSinacor, info);
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }


        public Dictionary<string, CotistaInfo> PopulateCotistaInfoSinacor( ) 
        {
            OracleConnection objORAConnection = new OracleConnection();

            objORAConnection.ConnectionString = ConfigurationManager.ConnectionStrings["TRADE"].ConnectionString;

            objORAConnection.Open();

            string sqlQuery = "";
			sqlQuery += " SELECT DISTINCT(TSCCLIGER.CD_CPFCGC) AS CD_CPFCGC, ";
			sqlQuery += " TSCCLIGER.IN_SITUAC AS IN_SITUAC,                  ";
			sqlQuery += " TSCCLIGER.NM_CLIENTE AS NM_CLIENTE,                ";
			sqlQuery += " TSCCLIGER.TP_PESSOA AS TP_PESSOA,                  ";
			sqlQuery += " TSCCLIGER.TP_CLIENTE AS TP_CLIENTE,                ";
			sqlQuery += " TSCENDE.NM_LOGRADOURO,                             ";
			sqlQuery += " TSCENDE.NR_PREDIO,                                 ";
			sqlQuery += " TSCENDE.NM_COMP_ENDE,                              ";
			sqlQuery += " TSCENDE.NM_BAIRRO,                                 ";
			sqlQuery += " TSCENDE.NM_CIDADE,                                 ";
			sqlQuery += " TSCENDE.SG_ESTADO,                                 ";
			sqlQuery += " TSCENDE.SG_PAIS,                                   ";
			sqlQuery += " TSCENDE.CD_CEP AS CEP,                    ";
			sqlQuery += " TSCENDE.CD_CEP_EXT AS CEPEXT,             ";
			sqlQuery += " TSCTIPCLI.DS_TIPO_CLIENTE AS DS_TIPO_CLIENTE       ";
			sqlQuery += " FROM TSCCLIGER, TSCTIPCLI, TSCENDE                 ";
			sqlQuery += " WHERE TSCCLIGER.TP_CLIENTE = TSCTIPCLI.TP_CLIENTE  ";
			sqlQuery += " AND TSCENDE.CD_CPFCGC = TSCCLIGER.CD_CPFCGC        ";


            Dictionary<string, CotistaInfo> dctPessoasSinacor = new Dictionary<string,CotistaInfo>();

            using (OracleCommand objORACommand = objORAConnection.CreateCommand())
            {
                objORACommand.CommandText = sqlQuery;

                OracleDataReader odr = objORACommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (odr.HasRows)
                {
                    while (odr.Read())
                    {
                        try
                        {
                            CotistaInfo info = new CotistaInfo(); 

                            info.CpfCnpj = OracleConvert.GetNumber("CD_CPFCGC", odr).ToString();
                            info.NomeCotista = OracleConvert.GetString("NM_CLIENTE", odr);
                            info.TipoPessoaCotista = OracleConvert.GetString("TP_PESSOA", odr);
                            info.TipoClienteSinacor = OracleConvert.GetInt("TP_CLIENTE", odr);
                            info.CEP = OracleConvert.GetInt("CEP", odr).ToString("00000") + OracleConvert.GetInt("CEPEXT", odr).ToString("000");
                            info.Endereco = OracleConvert.GetString("NM_LOGRADOURO", odr).Trim() + ", " +
                                OracleConvert.GetInt("NR_PREDIO", odr).ToString() + " - " +
                                OracleConvert.GetString("NM_COMP_ENDE", odr) + " - " +
                                OracleConvert.GetString("NM_BAIRRO", odr) + " - " +
                                OracleConvert.GetString("NM_CIDADE", odr);

                            info.Estado = OracleConvert.GetString("SG_ESTADO", odr);
                            info.Pais = OracleConvert.GetString("SG_PAIS", odr);

                            if (!dctPessoasSinacor.ContainsKey(info.CpfCnpj))
                            {
                                dctPessoasSinacor.Add(info.CpfCnpj, info);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                return dctPessoasSinacor;
            }
        }


        public Dictionary<string, CorrelacaoCarteiraCetipInfo> CarregarCorrelacaoCarteirasCetip()
        {
            Dictionary<string, CorrelacaoCarteiraCetipInfo> lRetorno = new Dictionary<string, CorrelacaoCarteiraCetipInfo>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdmFundos"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            bool EmProducao = false;

            if (ConfigurationManager.AppSettings["ConciliacaoProducao"] != null)
            {
                EmProducao = ConfigurationManager.AppSettings["ConciliacaoProducao"].ToString().ToUpperInvariant().Equals("S");
            }

            string sqlQuery = "";
            sqlQuery += "SELECT * FROM tb_correlacao_cetip_financial ";
            if (EmProducao)
            {
                sqlQuery += " WHERE st_producao = 'S'";
            }
            sqlQuery += " ORDER BY ds_codigo_if ";

            logger.Debug("sqlQuery = [" + sqlQuery + "]");

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    CorrelacaoCarteiraCetipInfo info = new CorrelacaoCarteiraCetipInfo();

                    string codigoIF = table.Rows[i]["ds_codigo_if"].DBToString().Trim();
                    int idCarteira = table.Rows[i]["id_carteira"].DBToInt32();

                    info.InstrumentoFinanceiro = codigoIF;
                    info.IDCarteira = idCarteira;
                    info.StatusProducao = table.Rows[i]["st_producao"].DBToString().Equals("S");
                    info.TipoEscrituracao = table.Rows[i]["ds_tipo_escrit"].DBToString();

                    if (!lRetorno.ContainsKey(codigoIF))
                        lRetorno.Add(codigoIF, info);
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }

        public Dictionary<int, string> CarregarDescricaoCarteirasFinancial( List<int> listaCarteiras )
        {
            Dictionary<int, string> lRetorno = new Dictionary<int, string>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["FINANCIAL"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            string sqlQuery = "";
            sqlQuery += " SELECT * FROM CARTEIRA ";

            logger.Debug("sqlQuery = [" + sqlQuery + "]");

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string nomeCarteira = table.Rows[i]["Nome"].DBToString().Trim();
                    int idCarteira = table.Rows[i]["IdCarteira"].DBToInt32();

                    if (listaCarteiras.Contains(idCarteira))
                    {
                        if (!lRetorno.ContainsKey(idCarteira))
                        {
                            lRetorno.Add(idCarteira, nomeCarteira);
                        }
                    }
                }
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }

        public List<string> ObterFeriadosSinacor()
        {
            List<string> retorno = new List<string>();

            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["TRADE"].ConnectionString);

                StringBuilder sqlQuery = new StringBuilder();

                sqlQuery.AppendLine("SELECT * FROM TGEFERIA WHERE ( CD_PRACA IS NULL OR CD_PRACA = 1 )");

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery.ToString().Replace("\r\n", "\n");
                cmd.CommandType = CommandType.Text;

                conn.Open();

                OracleDataReader objDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (objDataReader.HasRows)
                {
                    while (objDataReader.Read())
                    {
                        retorno.Add(objDataReader.GetDateTime(objDataReader.GetOrdinal("DT_FERIADO")).ToString("yyyy/MM/dd"));
                    }
                }

                cmd.Dispose();

                conn.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("ObterFeriadosSinacor" + ex.Message, ex);
            }

            return retorno;
        }
    }
}
