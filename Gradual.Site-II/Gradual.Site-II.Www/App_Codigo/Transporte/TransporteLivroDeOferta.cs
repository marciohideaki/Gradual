using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www
{
    public class TransporteLivroDeOferta
    {
        #region Propriedades

        public string Papel { get; set; }

        public List<TransporteMensagemDeLivroDeOferta> OfertasDeCompra{ get; set; }

        public List<TransporteMensagemDeLivroDeOferta> OfertasDeVenda{ get; set; }

        #endregion

        #region Construtores
        
        public TransporteLivroDeOferta()
        {
            this.OfertasDeCompra = new List<TransporteMensagemDeLivroDeOferta>();
            this.OfertasDeVenda = new List<TransporteMensagemDeLivroDeOferta>();
        }

        public TransporteLivroDeOferta(string pMensagem):this()
        {               
            ProcessarMensagem(pMensagem);
        }

        #endregion

        #region Métodos Públicos

        public void ProcessarMensagem(string pMensagem)
        {
            try
            {
                if (!string.IsNullOrEmpty(pMensagem))
                {
                    this.Papel = pMensagem.Substring(21, 20).Trim();
                    //removendo o header
                    
                    pMensagem = pMensagem.Remove(0, 41);


                    int indexC = pMensagem.IndexOf('C');
                    int indexV = pMensagem.IndexOf('V');

                    string Compra = "";
                    string Venda = "";

                    if (indexC >= 0 && indexV >=0)
                    {
                        Compra = pMensagem.Remove(indexV, (pMensagem.Length - indexV));
                        Venda = pMensagem.Remove(0, indexV);
                    }
                    else
                    if ( indexC==-1 )
                    {
                        Venda = pMensagem;
                    }
                    else
                    {
                        Compra = pMensagem;
                    }

                    TransporteMensagemDeLivroDeOferta lMensagem = null;

                    if (Compra.Length > 0)
                    {
                        string[] arCompra = Compra.Split('C');

                        //Adicionando as mensagens de compra
                        foreach (string itemCompra in arCompra)
                        {
                            if (itemCompra.Length != 0)
                            {
                                lMensagem = new TransporteMensagemDeLivroDeOferta(itemCompra);
                                OfertasDeCompra.Add(lMensagem);
                            }
                        }
                    }

                    if (Venda.Length > 0)
                    {
                        string[] arVenda = Venda.Split('V');
                        //Adicionando as mensagens de VENDA
                        foreach (string itemVenda in arVenda)
                        {
                            if (itemVenda.Length != 0)
                            {
                                lMensagem = new TransporteMensagemDeLivroDeOferta(itemVenda);
                                OfertasDeVenda.Add(lMensagem);
                            }
                        }
                    }
                }
            }
            catch (Exception rter)
            {
                throw rter;
            }

        }

        #endregion
    }
}