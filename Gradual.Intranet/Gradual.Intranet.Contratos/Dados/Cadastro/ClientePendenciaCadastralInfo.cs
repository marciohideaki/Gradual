using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClientePendenciaCadastralInfo : ICodigoEntidade
    {
        #region Propriedades

        /// <summary>
        /// Código do cliente pendencia
        /// </summary>
        public Nullable<int> IdPendenciaCadastral { set; get; }

        /// <summary>
        /// Código da pendencia
        /// </summary>
        public int IdTipoPendencia { set; get; }

        /// <summary>
        /// Descrição do Tipo da Pendencia
        /// </summary>
        public string DsTipoPendencia { set; get; }

        /// <summary>
        /// Código do cliente
        /// </summary>
        public Nullable<int> IdCliente { set; get; }

        /// <summary>
        /// Descrição da pendencia cadastral
        /// </summary>
        public string DsPendencia { set; get; }

        /// <summary>
        /// Data de cadastro da pendencia cadastral
        /// </summary>
        public Nullable<DateTime> DtPendencia { set; get; }

        /// <summary>
        /// Data de resolução da pendencia cadastral
        /// </summary>
        public Nullable<DateTime> DtResolucao { set; get; }

        /// <summary>
        /// Descrição da resolução da pendencia cadastral
        /// </summary>
        public string DsResolucao { set; get; }

        /// <summary>
        /// Id do usuário que resolveu
        /// </summary>
        public Nullable<int> IdLoginRealizacao { get; set; }

        /// <summary>
        /// Nome do usuário que resolveu
        /// </summary>
        public string DsLoginRealizacao { get; set; }
        
        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
