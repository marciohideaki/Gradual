using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class LogEmailInfo : EmailInfo, ICodigoEntidade
    {
        #region | Propriedades

        /// <summary>
        /// Data de envio do email
        /// </summary>
        public DateTime DtEnvio { get; set; }

        /// <summary>
        /// Tipo de E-mail que foi disparado 
        /// 1 - Disparado para assessor 
        /// 2 - Disparado para compliance;
        /// </summary>
        public eTipoEmailDisparo ETipoEmailDisparo { get; set; }

        public int? IdCliente { get; set; }

        public String Perfil { get; set; }

        #endregion

        #region | Construtores

        public LogEmailInfo()
        {
            // TODO: Complete member initialization
        }

        public LogEmailInfo(EmailInfo pParametro, eTipoEmailDisparo pTipoEmailDisparo)
        {
            this.Assunto = pParametro.Assunto;
            this.CorpoMensagem = pParametro.CorpoMensagem;
            this.Destinatarios = pParametro.Destinatarios;
            this.Remetente = pParametro.Remetente;
            this.DtEnvio = DateTime.Now;
            this.ETipoEmailDisparo = pTipoEmailDisparo;
        }

        public LogEmailInfo(EmailInfo pParametro, eTipoEmailDisparo pTipoEmailDisparo, Nullable<int> pIdCliente, String pPerfil)
        {
            this.Assunto = pParametro.Assunto;
            this.CorpoMensagem = pParametro.CorpoMensagem;
            this.Destinatarios = pParametro.Destinatarios;
            this.Remetente = pParametro.Remetente;
            this.DtEnvio = DateTime.Now;
            this.ETipoEmailDisparo = pTipoEmailDisparo;
            this.IdCliente = pIdCliente;
            this.Perfil = pPerfil;
        }

        #endregion

        #region | Implementação de ICodigoEntidade

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
