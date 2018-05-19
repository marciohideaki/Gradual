using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string address          = System.Configuration.ConfigurationManager.AppSettings["UrlClient"];
        string pageIntegration  = System.Configuration.ConfigurationManager.AppSettings["PageIntegration"];
        string page             = System.Configuration.ConfigurationManager.AppSettings["Page"];

        if (System.Configuration.ConfigurationManager.AppSettings["TokenType"].Equals("Integration1"))
        {
            string data = Gradual.Integration.HomeBroker.Cipher.Encrypt("TesteTesteTesteTesteTesteTeste", "hb.gradualinvestimentos.com.br");

            var client = new HttpClient();

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Host", "hb.gradualinvestimentos.com.br"),
                new KeyValuePair<string, string>("TokenType", "Integration1"),
                new KeyValuePair<string, string>("Token", data)

            };

            var content = new FormUrlEncodedContent(pairs);

            var response = client.PostAsync(address + pageIntegration, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Response.Redirect(address + page);
            }
        }

        if (System.Configuration.ConfigurationManager.AppSettings["TokenType"].Equals("Integration2"))
        {
            Gradual.Integration.HomeBroker.User user = new Gradual.Integration.HomeBroker.User();

            user.Codigo         = 9999;
            user.Login          = "Teste";
            user.Nome           = "Testerson do Teste";
            user.DataHora       = DateTime.Now;
            user.ValidadeToken  = Int32.Parse(ConfigurationManager.AppSettings.Get("TokenExpiration"));

            using (FileStream fsSource = new FileStream(@ConfigurationManager.AppSettings.Get("PublicKey"), FileMode.Open, FileAccess.Read))
            {
                byte[] obj = ObjectToByteArray(user);
                byte[] objCrypt = Gradual.Integration.HomeBroker.Crypto.Encrypt(obj, Gradual.Integration.HomeBroker.Crypto.ReadPublicKey(fsSource), true, true);

                Token.Value = Encoding.ASCII.GetString(objCrypt);
            }

            #region Testes
            //var client = new HttpClient();

            //var pairs = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("Host", "hb.gradualinvestimentos.com.br"),
            //    new KeyValuePair<string, string>("TokenType", "Integration2"),
            //    new KeyValuePair<string, string>("Token", Encoding.ASCII.GetString(objCrypt))
            //};

            //var content = new FormUrlEncodedContent(pairs);



            #region Option0
            //using (var wclient = new System.Net.WebClient())
            //{
            //    var values = new System.Collections.Specialized.NameValueCollection();
            //    //values["thing1"] = "hello";
            //    //values["thing2"] = "world";
            //    values["Host"] = "hb.gradualinvestimentos.com.br";
            //    values["TokenType"] = "Integration2";
            //    values["Token"] = Encoding.ASCII.GetString(objCrypt);


            //    var response = wclient.UploadValues(address + pageIntegration, values);

            //    var responseString = Encoding.Default.GetString(response);
            //}

            //using (var wclient = new System.Net.WebClient())
            //{
            //    var responseString = wclient.DownloadString(address + pageIntegration);
            //}

            #endregion

            #region Option1 (Funciona)

            //var response = client.PostAsync(address + pageIntegration, content).Result;

            //if (response.IsSuccessStatusCode)
            //{
            //}

            #endregion

            #region Option2
            //// Create a request using a URL that can receive a post. 
            //System.Net.WebRequest request = System.Net.WebRequest.Create(address + pageIntegration);
            //// Set the Method property of the request to POST.
            //request.Method = "POST";
            //// Create POST data and convert it to a byte array.
            ////string postData = "This is a test that posts this string to a Web server.";
            ////byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            //byte[] byteArray = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(objCrypt));

            //// Set the ContentType property of the WebRequest.
            //request.ContentType = "application/x-www-form-urlencoded";
            //// Set the ContentLength property of the WebRequest.
            //request.ContentLength = byteArray.Length;
            //// Get the request stream.
            //Stream dataStream = request.GetRequestStream();
            //// Write the data to the request stream.
            //dataStream.Write(byteArray, 0, byteArray.Length);
            //// Close the Stream object.
            //dataStream.Close();
            //// Get the response.
            //System.Net.WebResponse response = request.GetResponse();
            //// Display the status.
            //Console.WriteLine(((System.Net.HttpWebResponse)response).StatusDescription);
            //// Get the stream containing content returned by the server.
            //dataStream = response.GetResponseStream();
            //// Open the stream using a StreamReader for easy access.
            //StreamReader reader = new StreamReader(dataStream);
            //// Read the content.
            //string responseFromServer = reader.ReadToEnd();
            //// Display the content.
            //Console.WriteLine(responseFromServer);
            //// Clean up the streams.
            //reader.Close();
            //dataStream.Close();
            //response.Close();
            #endregion

            #region Option3
            //string addresToRedirect = String.Format("{0}{1}?TokenType={2}&Host={3}&Token={4}", address, pageIntegration, "Integration2", "hb.gradualinvestimentos.com.br", Encoding.UTF8.GetString(objCrypt).Replace("-----BEGIN PGP MESSAGE-----\r\nVersion: BCPG C# v1.8.1.0\r\n\r\n", "").Replace("\r\n-----END PGP MESSAGE-----\r\n","").Replace("\r\n",".r.n"));
            //Response.Redirect(addresToRedirect);
            #endregion

            #endregion
        }
    }

    private static byte[] ObjectToByteArray(Object obj)
    {
        if (obj == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        //return ms.ToString();
        return ms.ToArray();

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Gradual.Integration.WebService.IntegrationLog integrationLog = new Gradual.Integration.WebService.IntegrationLog();
    }
}