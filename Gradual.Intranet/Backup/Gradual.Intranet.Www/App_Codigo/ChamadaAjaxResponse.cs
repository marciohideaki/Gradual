using System;

namespace Gradual.Intranet
{
    /// <summary>
    /// Classe que representa uma resposta a chamada ajax, que é serializada em json para retornar pro javascript
    /// </summary>
    public class ChamadaAjaxResponse
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
                if (value.Length <= 100 || value.Contains(System.Configuration.ConfigurationManager.AppSettings["FileLoader"]))
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

        public ChamadaAjaxResponse(bool pTemErro, string pMensagem)
        {
            this.TemErro = pTemErro;
            this.Mensagem = pMensagem;
        }

        public ChamadaAjaxResponse(bool pTemErro, string pMensagem, params object[] pParams) : this(pTemErro, string.Format(pMensagem, pParams)) { }

        public ChamadaAjaxResponse(string pMensagem, params object[] pParams) : this(false, pMensagem, pParams) { }

        public ChamadaAjaxResponse(string pMensagem, Exception pErro, params object[] pParams) : this(true, pMensagem, pParams) 
        {
            this.MensagemExtendida = string.Format("{0}\r\n\r\n{1}", pErro.Message, pErro.StackTrace);
        }

        #endregion
    }
}
