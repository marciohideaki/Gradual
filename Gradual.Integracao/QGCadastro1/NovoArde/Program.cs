using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Configuration;
using log4net;
using System.Globalization;
using System.IO;
using System.Net.Mail;

namespace NovoArde
{
    class Program
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            OracleConnection objORAConnection = new OracleConnection();

            objORAConnection.ConnectionString = ConfigurationManager.ConnectionStrings["SINACOR"].ConnectionString;

            objORAConnection.Open();
            OracleDataReader odr;

            string query1 = "SELECT SUM(A.QTDE_DISP) AS QTDE_DISP, SUM(A.VAL_VIST) AS VAL_VISTA";
            query1 += " FROM HCFPOSI_EXTR_ACAO A, TSCCLIBOL B ";
            query1 += " WHERE A.COD_CLI = B.CD_CLIENTE AND ";
            query1 += " B.CD_OPERAC_CVM = '2253508150708150707' ";
            query1 += " GROUP BY CD_CLIENTE, CD_OPERAC_CVM ";

            string query2 = "SELECT VL_DISPONIVEL AS SALDO FROM TCCSALDO WHERE CD_CLIENTE = 56093";

            logger.Info("Query1 [" + query1 + "]");

            logger.Info("Query2 [" + query2 + "]");

            StringBuilder xmlCvm = new StringBuilder();

            xmlCvm.AppendLine("<?xml version=\"1.0\" encoding=\"windows-1252\"?>");
            xmlCvm.AppendLine("<DOC_ARQ xmlns=\"urn:infmensal\">");
 	        xmlCvm.AppendLine("<CAB_INFORM>");
 	 	    xmlCvm.AppendLine("<COD_DOC>477</COD_DOC>");
 	 	    xmlCvm.AppendLine("<VERSAO>1.0</VERSAO>");
 	 	    xmlCvm.AppendLine("<CD_CVM>227</CD_CVM>");
 	 	    xmlCvm.AppendLine("<DT_COMPT>" + DateTime.Now.ToString("MM/yyyy") + "</DT_COMPT>");
 	 	    xmlCvm.AppendLine("<DT_GERAC_ARQ>"+DateTime.Now.ToString("dd/MM/yyyy")+"</DT_GERAC_ARQ>");
            xmlCvm.AppendLine("</CAB_INFORM>");

            xmlCvm.AppendLine("<LISTA_PARTICIPANTES>");

            xmlCvm.AppendLine("<PARTICIPANTE>");
            xmlCvm.AppendLine("<COD_PARTIC>22535.081507.081507.0-7</COD_PARTIC>");
 	 	 	xmlCvm.AppendLine("<ENTRADA_SAIDA>");
            xmlCvm.AppendLine("<VL_ENTR>0</VL_ENTR>");
            xmlCvm.AppendLine("<VL_SAIDA>0</VL_SAIDA>");
 	 	 	xmlCvm.AppendLine("</ENTRADA_SAIDA>");
            xmlCvm.AppendLine("<MOV_RECURSOS>");
            xmlCvm.AppendLine("<TRANSF_RECURSOS>");
            xmlCvm.AppendLine("<VL_INVEST_CARTEIRA_HAVER_NRES>0</VL_INVEST_CARTEIRA_HAVER_NRES>");
            xmlCvm.AppendLine("<VL_HAVER_NRES_INVEST_CARTEIRA>0</VL_HAVER_NRES_INVEST_CARTEIRA>");
            xmlCvm.AppendLine("<VL_INVEST_CARTEIRA_ADR>0</VL_INVEST_CARTEIRA_ADR>");
            xmlCvm.AppendLine("<VL_ADR_INVEST_CARTEIRA>0</VL_ADR_INVEST_CARTEIRA>");
            xmlCvm.AppendLine("<VL_INVEST_CARTEIRA_ESTRANG_DIRETA>0</VL_INVEST_CARTEIRA_ESTRANG_DIRETA>");
            xmlCvm.AppendLine("<VL_ESTRANG_DIRETA_INVEST_CARTEIRA>0</VL_ESTRANG_DIRETA_INVEST_CARTEIRA>");
            xmlCvm.AppendLine("</TRANSF_RECURSOS>");
            xmlCvm.AppendLine("<RECURSOS_MESMO_INVEST/>");
            xmlCvm.AppendLine("<RECURSOS_OUTRO_INVEST/>");
            xmlCvm.AppendLine("</MOV_RECURSOS>");


            using (OracleCommand objORACommand = objORAConnection.CreateCommand())
            {
                objORACommand.CommandText = query1;
                odr = objORACommand.ExecuteReader(CommandBehavior.CloseConnection);

                xmlCvm.AppendLine("<APLICACOES>");

                if (odr.HasRows)
                {
                    while (odr.Read())
                    {
                        int Qtde = OracleConvert.GetInt("QTDE_DISP", odr);
                        Decimal valor =  Convert.ToDecimal(OracleConvert.GetDouble("VAL_VISTA", odr));
                        //trade.SequentialNumber = OracleConvert.GetInt("NR_SEQORD", odr).ToString();
                        //trade.Papel = OracleConvert.GetString("CD_PAPEL", odr);
                        //trade.MaturityDate = OracleConvert.GetDateTime("DT_VENC", odr);
 	 	 	 	 	    xmlCvm.AppendLine("<CD_APLIC>1</CD_APLIC>");
 	 	 	 	 	    xmlCvm.AppendLine("<VL_MERCADO_APLIC>" + valor.ToString(CultureInfo.CreateSpecificCulture("pt-Br")) + "</VL_MERCADO_APLIC>");
                        xmlCvm.AppendLine("<VL_NOCION_APLIC>0</VL_NOCION_APLIC>");
                    }
                }

                xmlCvm.AppendLine("</APLICACOES>");

                objORACommand.CommandText = query2;
                odr = objORACommand.ExecuteReader(CommandBehavior.CloseConnection);

                if (odr.HasRows)
                {
                    while (odr.Read())
                    {
                        Decimal saldo = Convert.ToDecimal(OracleConvert.GetDouble("SALDO", odr));

                        

                        xmlCvm.AppendLine("<PATRIM_LIQ>" + Math.Truncate(saldo) + "</PATRIM_LIQ>");
                    }
                }

                odr.Close();
                odr.Dispose();
            }

            xmlCvm.AppendLine("</PARTICIPANTE>");
            xmlCvm.AppendLine("</LISTA_PARTICIPANTES>");
            xmlCvm.AppendLine("</DOC_ARQ>");

            string arqXml = ConfigurationManager.AppSettings["ArdeXMLSaida"].ToString();
            File.WriteAllText(arqXml, xmlCvm.ToString());


            logger.Info("Arquivo" + arqXml + " gerado com sucesso!");

            string emails = ConfigurationManager.AppSettings["EmailDestArde"].ToString();

            string subject = "Arquivo" + arqXml + " disponibilizado!";

            List<string> lista = new List<string>();
            lista.Add(arqXml);
            _enviaAviso(emails, subject, lista);

        }

        private static bool _enviaAviso(string emailsDest, string subject, List<string> arquivosMerged)
        {
            try
            {
                string[] emailsTo;
                string[] emailsCC;
                string[] emailsBCC;


                if (ConfigurationManager.AppSettings["EmailRemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailMTARemetente' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                char[] seps = { ';', ',' };
                emailsTo = emailsDest.ToString().Split(seps);

                MailMessage lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailRemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }

                if (ConfigurationManager.AppSettings["EmailBCC"] != null)
                {
                    emailsBCC = ConfigurationManager.AppSettings["EmailBCC"].ToString().Split(seps);

                    for (int i = 0; i < emailsBCC.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(emailsBCC[i]))
                            lMensagem.Bcc.Add(emailsBCC[i]);
                    }
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailReplyTo"].ToString()));
                lMensagem.Subject = subject;


                if (ConfigurationManager.AppSettings["EmTeste"] == null ||
                    ConfigurationManager.AppSettings["EmTeste"].ToString().ToLowerInvariant().Equals("true"))
                {
                    string relatorio = "<b>*** ATENÇÃO !!!!!! ****</b>" + Environment.NewLine;
                    relatorio += "SERVICO EM TESTE, verificar os arquivos anexos antes de submeter a processamento em produção" + Environment.NewLine;
                    relatorio += "Não nos responsabilizamos por eventuais erros ou danos decorrentes do descumprimento do aviso acima" + Environment.NewLine;

                    lMensagem.IsBodyHtml = true;
                    lMensagem.Body = "<html><body style=\"font-family:courier;\">" + relatorio.Replace(" ", "&nbsp;").Replace(Environment.NewLine, "<br>" + Environment.NewLine) + "</body></html>";
                }

                foreach (string arquivoMerged in arquivosMerged)
                    lMensagem.Attachments.Add(new Attachment(arquivoMerged));

                new SmtpClient(ConfigurationManager.AppSettings["EmailHost"].ToString()).Send(lMensagem);

                lMensagem.Dispose();

                logger.Info("Email enviado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("_enviaAviso(): " + ex.Message, ex);
                return false;
            }

            return true;
        }
    }
}
