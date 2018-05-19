using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Gradual.OMS.WsIntegracao.Arena.App_Codigo;
using Gradual.OMS.WsIntegracao.Arena.Services;


namespace Gradual.OMS.WsIntegracao.Arena.Models
{
    public class Evento
    {
        #region Properties
        private readonly string gApiKey   = ConfiguracoesValidadas.ArenaKey;// "91065d52fdf24687bded93b99f7499e3ac5cb1c5a1b84c5b89ebcb30632d1f60";

        private readonly string gArenaUrl = ConfiguracoesValidadas.ArenaUri;//  "https://10.97.1.178:3020";

        public string NomeEvento            { get; set; }
        
        public string ApiEnvio              { get; set; }

        public LoginRequestDTO LoginRequest { get; set; }

        public int IdCliente                { get; set; }

        public string UrlHomeBroker         { get; set; }
        #endregion

        #region Métodos
        public async void ShootEventoGet()
        {
            WebRequestHandler handler = new WebRequestHandler();
            handler.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri(gArenaUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", gApiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                switch (NomeEvento)
                {
                    case "RequestDeLogin":

                        var lCliente = ClienteContaServico.ConsultarClienteConta(IdCliente);
                        
                        LoginRequest.Phone                      = lCliente.Phone;
                        LoginRequest.IdClienteGradual           = lCliente.ID;
                        LoginRequest.IsBovespaAllowed           = lCliente.StatusBovespa;
                        LoginRequest.IsBmfAllowed               = lCliente.StatusBmf;
                        LoginRequest.IsSuperQualified           = false;
                        
                        HttpResponseMessage response = await client.GetAsync(ApiEnvio);

                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                        response.EnsureSuccessStatusCode();

                        if (response.IsSuccessStatusCode)
                        {

                            var lRedirectUrl = await response.Content.ReadAsAsync<string>();

                            //return lRedirectUrl;
                            // HTTP PUT
                            //gizmo.Price = 80;   // Update price
                            //response = await client.PutAsJsonAsync(gizmoUrl, gizmo);

                            // HTTP DELETE
                            //response = await client.DeleteAsync(gizmoUrl);
                            //Uri gizmoUrl = response.Headers.Location;

                            //lRetorno = lRedirectUrl;
                        }

                        break;
                }

                //HttpResponseMessage response = await client.GetAsync("api/products/1");
                //HttpResponseMessage response = await client.GetAsync(ApiEnvio);
                //if (response.IsSuccessStatusCode)
                //{
                //    //Product product = await response.Content.ReadAsAsync>Product>();
                //    //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                //}
            }
        }

        public async Task<string> ShootEventPost()
        {
            string lRetorno = string.Empty;

            try
            {
                WebRequestHandler handler = new WebRequestHandler();
                //X509Certificate2 certificate = GetMyX509Certificate();
                //handler.ClientCertificates.Add(certificate);
                //HttpClient client = new HttpClient(handler);
                handler.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(gArenaUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", gApiKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    switch (NomeEvento)
                    {
                        case "RequestDeLogin":

                            var lCliente = ClienteContaServico.ConsultarClienteConta(IdCliente);

                            LoginRequest.Phone            = lCliente.Phone;
                            LoginRequest.IdClienteGradual = lCliente.ID;
                            LoginRequest.IsBovespaAllowed = lCliente.StatusBovespa;
                            LoginRequest.IsBmfAllowed     = lCliente.StatusBmf;
                            LoginRequest.IsSuperQualified = false;

                            HttpResponseMessage response = await client.PostAsJsonAsync(ApiEnvio, LoginRequest);

                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                            
                            response.EnsureSuccessStatusCode();
                           
                            //response

                            if (response.IsSuccessStatusCode)
                            {

                                return await response.Content.ReadAsAsync<string>();

                                //return lRedirectUrl;
                                // HTTP PUT
                                //gizmo.Price = 80;   // Update price
                                //response = await client.PutAsJsonAsync(gizmoUrl, gizmo);

                                // HTTP DELETE
                                //response = await client.DeleteAsync(gizmoUrl);
                                //Uri gizmoUrl = response.Headers.Location;

                                //lRetorno = lRedirectUrl;
                            }

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //gLogger.Error();
            }

            return lRetorno;
        }

        #endregion
    }
}