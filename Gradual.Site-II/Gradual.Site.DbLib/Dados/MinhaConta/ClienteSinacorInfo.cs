using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Dados.MinhaConta
{
    /// <summary>
    /// Classe de Info de gerenciamento de cliente com informações do sinacor.
    /// </summary>
    [Serializable]
    public class ClienteSinacorInfo : ICodigoEntidade
    {
        /// <summary>
        /// Codigo do Cliente que solicitou a oferta publica
        /// </summary>
        public int CodigoCliente { get; set; }

        /// <summary>
        /// Email do assessor do cliente
        /// </summary>
        public string EmailAssessor { get; set; }

        /// <summary>
        /// Email do cliente
        /// </summary>
        public string EmailCliente { get; set; }

        /// <summary>
        /// Código do assessor do cliente
        /// </summary>
        public int CodigoAssessor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
