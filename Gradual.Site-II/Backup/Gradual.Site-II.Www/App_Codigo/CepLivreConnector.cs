using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Gradual.Site.Www
{
    public static class CepLivreConnector
    {
        #region Propriedades

        public static string CepLivreChave { get; set; }

        public static Dictionary<string, CepLivreEndereco> Cache { get; set; }

        #endregion

        #region Métodos Públicos

        public static CepLivreEndereco BuscarCEP(string pCEP)
        {
            if (string.IsNullOrEmpty(CepLivreConnector.CepLivreChave))
            {
                CepLivreConnector.CepLivreChave = ConfiguracoesValidadas.CepLivreChave;
            }

            if (CepLivreConnector.Cache == null)
                CepLivreConnector.Cache = new Dictionary<string, CepLivreEndereco>();

            CepLivreEndereco lRetorno = new CepLivreEndereco();

            if (!pCEP.Contains("-") && pCEP.Length > 5)
            {
                pCEP = pCEP.Insert(5, "-");
            }

            string lURL = string.Format("http://ceplivre.com.br/consultar/cep/{0}/{1}/json", CepLivreConnector.CepLivreChave, pCEP);

            if (string.IsNullOrEmpty(pCEP)) 
            {
                return lRetorno;
            }

            if (pCEP.Length < 3)
            {
                lRetorno.cep = pCEP;

                return lRetorno;
            }
            
            if (pCEP.Length > 10)
            {
                throw new Exception( string.Format("CEP muito longo: [{0}]", pCEP) );
            }

            if (CepLivreConnector.Cache.ContainsKey(pCEP))
            {
                return CepLivreConnector.Cache[pCEP];
            }

            WebRequest lRequest = WebRequest.Create( lURL );

            try
            {
                HttpWebResponse lResponse = (HttpWebResponse)lRequest.GetResponse();

                Stream lResponseStream = lResponse.GetResponseStream();

                StreamReader lReader = new StreamReader(lResponseStream);

                string lContent = lReader.ReadToEnd();

                try
                {
                    CepLivreRetorno lLista = JsonConvert.DeserializeObject<CepLivreRetorno>(lContent);

                    if (lLista.cep.Count > 0)
                    {
                        lRetorno = lLista.cep[0];

                        CepLivreConnector.Cache.Add(pCEP, lRetorno);
                    }
                    else
                    {
                        lRetorno.logradouro = "n/d";
                    }
                }
                catch (Exception convEx)
                {
                    throw new Exception( string.Format("Erro ao converter resposta [{0}] em JSON", lContent));
                }
            }
            catch (Exception webEx)
            {
                throw new Exception( string.Format("Erro ao buscar request para o CEP Livre em [{0}]: [{1}]\r\n{2}", lURL, webEx.Message, webEx.StackTrace));
            }

            return lRetorno;
        }

        #endregion
    }

    public class CepLivreRetorno
    {
        public List<CepLivreEndereco> cep { get; set; }
    }

    public class CepLivreEndereco
    {
        #region Propriedades
        
        /*
         { "cep": [
         *           {"tp_logradouro": "Rua"
         *          , "tp_logradouro_id": "11"
         *          , "logradouro": "dos Girassois"
         *          , "bairro": "Jardim das Indústrias"
         *          , "cidade": "São José dos Campos"
         *          , "uf_sigla": "SP"
         *          , "ufnome": "São Paulo"
         *          , "id_estado_ibge": "35"
         *          , "cep": "12240-360"
         *          , "muncoddv": "3549904"
         *          , "ddd": "12"
         *          , "altitude": "600"
         *          , "latitude": "-23.179"
         *          , "longitude": "-45.887"
         *          , "area": "1099.613"
         *          , "capital": "N"
         *        }]}
        */
        
        public string tp_logradouro    { get; set; }
        public string tp_logradouro_id { get; set; }
        public string logradouro       { get; set; }
        public string bairro           { get; set; }
        public string cidade           { get; set; }
        public string uf_sigla         { get; set; }
        public string ufnome           { get; set; }
        public string id_estado_ibge   { get; set; }
        public string cep              { get; set; }
        public string muncoddv         { get; set; }
        public string ddd              { get; set; }
        public decimal altitude        { get; set; }
        public decimal latitude        { get; set; }
        public decimal longitude       { get; set; }
        public decimal area            { get; set; }
        public string capital          { get; set; }

        #endregion
    }
}