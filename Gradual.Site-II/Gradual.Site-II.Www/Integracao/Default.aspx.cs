using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Integracao
{
    public partial class Default : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string address          = System.Configuration.ConfigurationManager.AppSettings["UrlClient"];
            string pageIntegration  = System.Configuration.ConfigurationManager.AppSettings["PageIntegration"];
            string page             = System.Configuration.ConfigurationManager.AppSettings["Page"];

            if (System.Configuration.ConfigurationManager.AppSettings["TokenType"].Equals("Integration1"))
            {
                string data = Gradual.Integration.HomeBroker.Cipher.Encrypt("TesteTesteTesteTesteTesteTeste", "hb.gradualinvestimentos.com.br");

                var client = new System.Net.Http.HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Host", "hb.gradualinvestimentos.com.br"),
                    new KeyValuePair<string, string>("TokenType", "Integration1"),
                    new KeyValuePair<string, string>("Token", data)

                };

                var content = new System.Net.Http.FormUrlEncodedContent(pairs);

                var response = client.PostAsync(address + pageIntegration, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    Response.Redirect(address + page);
                }
            }

            if (System.Configuration.ConfigurationManager.AppSettings["TokenType"].Equals("Integration2"))
            {
                Gradual.Integration.HomeBroker.User user = new Gradual.Integration.HomeBroker.User();

                user.Codigo = 9999;
                user.Login = "Teste";
                user.Nome = "Testerson do Teste";
                user.DataHora = DateTime.Now;
                user.ValidadeToken = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TokenExpiration"));

                using (System.IO.FileStream fsSource = new System.IO.FileStream(System.Configuration.@ConfigurationManager.AppSettings.Get("PublicKey"), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    //byte[] obj = ObjectToByteArray(user);
                    //byte[] objCrypt = Gradual.Integration.HomeBroker.Crypto.Encrypt(obj, Gradual.Integration.HomeBroker.Crypto.ReadPublicKey(fsSource), true, true);

                    //Token.Value = Encoding.ASCII.GetString(objCrypt);
                }
            }

            RodarJavascriptOnLoad("RedirecionarHB();");
        }


    }
}