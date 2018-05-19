using System;

namespace Gradual.Intranet.Contratos
{
    public class Erro
    {
        #region Propriedades

        public string Mensagem { get; set; }

        public string Fonte { get; set; }

        public string Pilha { get; set; }

        #endregion

        #region Construtor

        public Erro() { }

        public Erro(Exception pException)
        {
            this.Mensagem = pException.Message;
            this.Fonte = pException.Source;
            this.Pilha = pException.StackTrace;
        }

        #endregion
    }
}
