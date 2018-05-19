using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Gradual.Integration.HomeBroker
{
    public class Integration
    {   
        public Response Integrate(Request request)
        {
            Response response = new Response();

            try
            {
                User user = null;

                switch (request.TokenType)
                {
                    case "Integration1":
                        {
                            string strDecrypted = Cipher.Decrypt(request.Token, request.Host);
                        }
                        break;
                    case "Integration2":
                        {

                            using (FileStream fsPvtSource = new FileStream(request.PublicKeyPath, FileMode.Open, FileAccess.Read))
                            {
                                if (request.Host.Equals("localhost"))
                                {
                                    user = Crypto.Decrypt(Encoding.UTF8.GetBytes(request.Token), fsPvtSource, "hb.gradualinvestimentos.com.br");
                                }
                                else
                                {
                                    user = Crypto.Decrypt(Encoding.UTF8.GetBytes(request.Token), fsPvtSource, request.Host);
                                }
                            }
                        }
                        break;
                }

                if(DateTime.Now > user.DataHora.AddSeconds(user.ValidadeToken))
                {
                    response.Valid = false;
                    response.Message = "Token expirado";
                    response.Except = new Exception("Token expirado");
                }
                else
                {
                    response.User = user;
                    response.Valid = true;
                }
            }
            catch (Exception ex)
            {
                response.Except     = ex;
                response.Message    = ex.Message;
                response.Valid      = false;
            }

            return response;
        }

    }
}
