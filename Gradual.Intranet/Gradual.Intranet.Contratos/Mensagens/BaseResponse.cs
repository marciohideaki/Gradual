using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class BaseResponse
    {
        #region Globais

        private StringBuilder gTraceBuilder;

        #endregion

        #region Propriedades

        public bool Sucesso { get; set; }

        public Erro Erro { get; set; }

        #endregion

        #region Construtor

        public BaseResponse()
        {
            this.Sucesso = true;
            this.Erro = new Erro();
        }

        #endregion

        #region Métodos Públicos

        public void Trace(string pMensagem, params object[] pParams)
        {
            if (gTraceBuilder == null)
                gTraceBuilder = new StringBuilder();

            gTraceBuilder.AppendLine(
                string.Format("{0}:> {1}",
                                DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"),
                                string.Format(pMensagem, pParams)));
        }

        public string BuscarTrace()
        {
            if (gTraceBuilder != null)
                return gTraceBuilder.ToString();

            return null;
        }

        public void AssumirErro(Exception pException)
        {
            this.Erro = new Erro(pException);

            this.Sucesso = false;
        }

        #endregion
    }
}
