using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace Gradual.Site.Www
{
    public class TransporteTickerDeDestaques
    {
        #region Propriedades

        public List<TransporteTickerDeDestaquesMensagem> MaioresAltas { get; set; }

        public List<TransporteTickerDeDestaquesMensagem> MaioresBaixas { get; set; }

        public List<TransporteTickerDeDestaquesMensagem> MaioresVolumes { get; set; }
        
        public List<TransporteTickerDeDestaquesMensagem> MaioresNegociacoes { get; set; }

        #endregion

        #region Construtor

        public TransporteTickerDeDestaques()
        {
            this.MaioresAltas = new List<TransporteTickerDeDestaquesMensagem>();
            this.MaioresBaixas = new List<TransporteTickerDeDestaquesMensagem>();
            this.MaioresVolumes = new List<TransporteTickerDeDestaquesMensagem>();
            this.MaioresNegociacoes = new List<TransporteTickerDeDestaquesMensagem>();
        }

        public TransporteTickerDeDestaques(string pTicker) : this()
        {
            try
            {
                string lMsg = pTicker;
                string[] lMensagens;

                List<TransporteTickerDeDestaquesMensagem> lLista;

                lMsg = lMsg.Substring(42);

                lMensagens = lMsg.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int a = 0; a < lMensagens.Length; a++)
                {
                    try
                    {
                        lLista = new List<TransporteTickerDeDestaquesMensagem>();

                        for (int b = 0; b <= (68 * 4); b+= 68)
                        {
                            if (lMensagens[a].Length > b)
                                lLista.Add(new TransporteTickerDeDestaquesMensagem(lMensagens[a].Substring(b, 68)));
                        }

                        if (a == 0)
                            this.MaioresAltas = lLista;

                        if (a == 1)
                            this.MaioresBaixas = lLista;

                        if (a == 2)
                            this.MaioresVolumes = lLista;

                        if (a == 3)
                            this.MaioresNegociacoes = lLista;
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                ILog lLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                lLogger.ErrorFormat("Erro [{0}] em TransporteTickerDeDestaques()\r\n    >>Parâmetro:\r\n[{1}]\r\n    >>Stack:\r\n{2}"
                                    , ex.Message
                                    , pTicker
                                    , ex.StackTrace);
            }
        }


        #endregion
    }
}