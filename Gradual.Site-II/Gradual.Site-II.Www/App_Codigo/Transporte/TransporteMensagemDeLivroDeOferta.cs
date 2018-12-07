using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www
{
    public class TransporteMensagemDeLivroDeOferta
    {
        #region Propriedades

        public int NumeroCorretora { get; set; }

        public string NomeCorretora { get; set; }

        public double Quantidade { get; set; }

        public string QuantidadeAbreviada { get; set; }

        public string Preco { get; set; }

        #endregion

        #region Métodos Públicos

        public void ProcessarMensagem(string pMensagem)
        {
            this.NumeroCorretora = int.Parse(pMensagem.Substring(0, 8));

            this.NomeCorretora = DadosDeAplicacao.NomesDasCorretoras.ContainsKey(this.NumeroCorretora)==true ? DadosDeAplicacao.NomesDasCorretoras[this.NumeroCorretora] : this.NumeroCorretora.ToString();

            this.Preco = pMensagem.Substring(8, 13).Replace('.', ',').TrimStart('0');

            try
            {
                if(!string.IsNullOrEmpty(this.Preco))
                {
                    //2 casas decimais:
                    this.Preco = this.Preco.Substring(0, this.Preco.IndexOf(',') + 3);

                    if (this.Preco.IndexOf(',') == 0) this.Preco = "0" + this.Preco;

                }
            }
            catch { }

            this.Quantidade = double.Parse(pMensagem.Substring(21,12));
        }

        public TransporteMensagemDeLivroDeOferta()
        {
        }

        public TransporteMensagemDeLivroDeOferta(string pMensagem)
        {
            this.ProcessarMensagem(pMensagem);
        }

        #endregion
    }
}