using System;
using Gradual.OMS.Library;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Dados
{
    public class MigracaoClienteAssessorInfo : ICodigoEntidade
    {
        #region Enum Ação

        #endregion

        #region | Propriedades
        /// <summary>
        /// Código do assessor de origem
        /// </summary>
        public int IdAssessorOrigem { get; set; }

        /// <summary>
        /// Código de assessor destino
        /// </summary>
        public int IdAssessorDestino { get; set; }

        /// <summary>
        /// Código do cliente a ser migrado
        /// </summary>
        public int IdCliente { get; set; }

        public string CdBmfBovespaCliente { get; set; }

        /// <summary>
        /// Códigos de Clientes separados por virgula
        /// </summary>
        public string DsClientes { get; set; }

        /// <summary>
        /// Lista de códigos do Cliente
        /// </summary>
        public List<int> IdsClientes { get; set; }

        public List<string> CdBmfBovespaClientes { get; set; }

        public List<string> CdSistema { get; set; }

        /// <summary>
        /// Enum 
        /// </summary>
        public MigracaoClienteAssessorAcao Acao { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
