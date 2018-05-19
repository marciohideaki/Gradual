using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net.Mail;
using System.Configuration;
using log4net;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Gradual.OMS.Library
{
    public class Utilities
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Binding GetBinding(string address)
        {
            Binding bind = null;

            if (address.StartsWith("net.pipe"))
            {
                bind = new NetNamedPipeBinding();

                ((NetNamedPipeBinding)bind).MaxReceivedMessageSize = 8000000;
                ((NetNamedPipeBinding)bind).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            }
            else
                if (address.StartsWith("http://"))
                {
                    bind = new BasicHttpBinding();
                    ((BasicHttpBinding)bind).MaxReceivedMessageSize = 8000000;
                    ((BasicHttpBinding)bind).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                }
                else
                    if (address.StartsWith("https://"))
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(
                            delegate(object sender,
                                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; });
                        
                        bind = new BasicHttpBinding();
                        ((BasicHttpBinding)bind).MaxReceivedMessageSize = 8000000;
                        ((BasicHttpBinding)bind).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                        ((BasicHttpBinding)bind).Security.Mode = BasicHttpSecurityMode.Transport;
                        ((BasicHttpBinding)bind).Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    }
                    else
                    {
                        bind = new NetTcpBinding();
                        ((NetTcpBinding)bind).MaxReceivedMessageSize = 8000000;
                        ((NetTcpBinding)bind).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                    }

            return bind;
        }


        public static string GetBindingType(string address)
        {
            if (address.StartsWith("net.pipe://"))
                return "System.ServiceModel.NetNamedPipeBinding";

            if (address.StartsWith("http://"))
                return "System.ServiceModel.BasicHttpBinding";

            if (address.StartsWith("https://"))
                return "System.ServiceModel.WSHttpBinding";

            return "System.ServiceModel.NetTcpBinding";
        }


        /// <summary>
        /// Retorna a data de hoje no formato dd/mm/yyyy 23:59:59
        /// </summary>
        /// <returns></returns>
        public static DateTime MeiaNoiteAindaEhHoje()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        }


        /// <summary>
        /// Envia mensagem de alerta
        /// </summary>
        public static bool EnviarEmail(string subject, string body)
        {
            try
            {
                string[] destinatarios;

                if (ConfigurationManager.AppSettings["EmailAlertaDestinatarios"] == null )
                {
                    logger.Error("AppSetting 'EmailAlertaDestinatarios' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                if (ConfigurationManager.AppSettings["EmailAlertaRemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailAlertaRemetente' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                char[] seps = { ';' };
                destinatarios = ConfigurationManager.AppSettings["EmailAlertaDestinatarios"].ToString().Split(seps);

                var lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailAlertaRemetente"].ToString(), destinatarios[0]);

                for (int i = 1; i < destinatarios.Length; i++)
                {
                    lMensagem.To.Add(destinatarios[i]);
                }


                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailAlertaReplyTo"].ToString()));
                lMensagem.Body = body;
                lMensagem.Subject = subject;

                new SmtpClient(ConfigurationManager.AppSettings["EmailAlertaHost"].ToString()).Send(lMensagem);

                logger.Info("Email enviado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("EnviarEmail(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Carrega uma estrutura do tipo T a partir do binary reader
        /// </summary>
        /// <param name="br">BinaryReader </param>
        /// <returns>estrutura do tipo T preenchida</returns>
        public static T MarshalFromBinaryReaderBlock<T>(BinaryReader br)
        {
            byte[] buff = br.ReadBytes(Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            T s = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return s;
        }

        /// <summary>
        /// Carrega uma estrutura do tipo T a partir de uma string
        /// </summary>
        /// <param name="br">BinaryReader </param>
        /// <returns>estrutura do tipo T preenchida</returns>
        public static T MarshalFromStringBlock<T>(string block)
        {
            byte[] buff = System.Text.Encoding.ASCII.GetBytes(block); 

            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);

            T s = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Copy properties values from source to destination if they have the 
        /// same properties names and types
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyPropertiesAsPossible(object source, object destination)
        {
            if (source == null || destination == null)
                return;

            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                try
                {
                    foreach (PropertyInfo prop1 in destination.GetType().GetProperties())
                    {
                        if (prop1.Name.Equals(prop.Name) && prop1.PropertyType.Equals(prop.PropertyType))
                        {
                            object xxx = source.GetType().GetProperty(prop1.Name).GetValue(source, null );
                            if ( xxx != null )
                                prop1.SetValue(destination, xxx, null);
                        }
                    }

                }
                catch (Exception ex)
                { }
            }
        }
    }
}
