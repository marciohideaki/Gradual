using System;

namespace Gradual.Spider
{
    #region Classes

    public class RespostaAjax
    {
        #region Propriedades
        
        /// <summary>
        /// (get, set) Flag se a operação teve erro
        /// </summary>
        public bool TemErro { get; set; }

        private string _Mensagem;

        /// <summary>
        /// (get, set) Mensagem de resposta, tendo erro ou não
        /// </summary>
        public string Mensagem 
        { 
            get
            {
                return _Mensagem;
            }

            set
            {
                if (value.Length <= 100)
                {
                    _Mensagem = value;
                }
                else
                {
                    _Mensagem = value.Substring(0, 100) + "(...)";

                    this.MensagemExtendida = value;
                }
            }
        }

        /// <summary>
        /// (get, set) Mensagem extendida, especialmente para casos onde tenha havido exception no server-side
        /// </summary>
        public string MensagemExtendida { get; set; }

        /// <summary>
        /// (get, set) Objeto de retorno da requisição, caso haja. Deve ser serializável em json
        /// </summary>
        public object ObjetoDeRetorno { get; set; }

        #endregion

        #region Construtor

        public RespostaAjax(bool pTemErro, string pMensagem)
        {
            this.TemErro = pTemErro;
            this.Mensagem = pMensagem;
        }

        public RespostaAjax(bool pTemErro, string pMensagem, params object[] pParams) : this(pTemErro, string.Format(pMensagem, pParams)) { }

        public RespostaAjax(string pMensagem, params object[] pParams) : this(false, pMensagem, pParams) { }

        public RespostaAjax(string pMensagem, Exception pErro, params object[] pParams) : this(true, pMensagem, pParams) 
        {
            this.MensagemExtendida = string.Format("{0}\r\n\r\n{1}", pErro.Message, pErro.StackTrace);
        }

        #endregion
    }

    #endregion

}
