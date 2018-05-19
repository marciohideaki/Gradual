using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Gradual.Pendencia.Email;
using Gradual.Pendencias.Dados;
using Gradual.Pendencias.Entidades;
using System.Text;

namespace Gradual.Pendencias.Teste
{
    class Program
    {
        #region | Propriedades

        private string GetTextoEmail
        {
            get
            {
                StreamReader ArquivoEmail = File.OpenText(System.IO.Path.GetFullPath("TextoEmail.htm"));
                string TextoEmail = ArquivoEmail.ReadToEnd();
                ArquivoEmail.Close();
                return TextoEmail;
            }
        }

        private string GetSmtp
        {
            get
            { 
                return ConfigurationManager.AppSettings["SMTP"].ToString(); 
            }
        }

        private string GetSenderDisplay
        {
            get
            {
                return ConfigurationManager.AppSettings["SenderDisplay"].ToString();
            }
        }

        private string GetSenderEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["Sender"].ToString();
            }
        }

        private string GetAssunto
        {
            get
            { 
                return ConfigurationManager.AppSettings["Assunto"].ToString(); 
            }
        }

        private string GetDestinatario
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailDestino"].ToString(); 
            }
        }

        private bool IsProducao
        {
            get
            {
                if (null == ConfigurationManager.AppSettings["EmailDestino"])
                    return true;
                else
                    return string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailDestino"].ToString());
            }
        }

        #endregion

        #region | Métodos

        static void Main(string[] args)
        {

            new Gradual.Servico.PendenciaClientes.NotificacaoPendencias().NotificarAssessor();

            /*
            try
            {
                List<AssessorInfo> lListaDeAssessores = new PendenciaDbLib().GetPendencias();
                Program lProgram = new Program();
                StringBuilder lClientesTexto;
                StringBuilder lEmailTexto;
                String lEnderecoDestino;

                foreach (AssessorInfo itemAssessor in lListaDeAssessores)
                {
                    lEmailTexto = new StringBuilder(lProgram.GetTextoEmail);

                    if (lProgram.IsProducao)
                         lEmailTexto = lEmailTexto.Replace("@nome", itemAssessor.NomeAssessor);
                    else lEmailTexto = lEmailTexto.Replace("@nome", string.Format("{0} - {1} - {2}", itemAssessor.NomeAssessor, itemAssessor.IdAssessor.ToString(), itemAssessor.EmailAssessor));
                    
                    lClientesTexto = new StringBuilder();
                    
                    foreach (ClienteInfo itemCliente in itemAssessor.Clientes)
                    {
                        lClientesTexto.AppendFormat("<b>Nome:</b> {0}<br />", itemCliente.NomeCliente);
                        lClientesTexto.AppendFormat("<b>CPF/CNPJ:</b> {0}<br />", itemCliente.CpfCnpjCliente);
                        lClientesTexto.AppendFormat("<b>Email:</b> {0}<br />", itemCliente.EmailCliente);
                        lClientesTexto.AppendFormat("<b>Código:</b> {0}<br/>", itemCliente.CodigoBovespa);
                        lClientesTexto.Append("<b>Pendência(s):</b> ");

                        foreach (PendenciaInfo itemPendencia in itemCliente.Pendencias)
                            lClientesTexto.AppendFormat("{0}, ", itemPendencia.Pendencia);

                        lClientesTexto = lClientesTexto.Remove(lClientesTexto.Length - 2, 2).Append("<br /><br />");
                    }

                    lEmailTexto = lEmailTexto.Replace("@clientes", lClientesTexto.ToString());

                    if (lProgram.IsProducao)
                        lEnderecoDestino = itemAssessor.EmailAssessor;
                    else
                        lEnderecoDestino = lProgram.GetDestinatario;

                    new Email().EnviarEmail(lProgram.GetSenderEmail, lProgram.GetSenderDisplay, lEnderecoDestino, lProgram.GetAssunto, lEmailTexto.ToString(), lProgram.GetSmtp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
             */
        }

        #endregion
    }
}
