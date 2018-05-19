using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class TotalClienteCadastradoAssessorPeriodoInfo : ICodigoEntidade
    {
        /// <summary>
        /// Data de do filtro
        /// </summary>
        public Nullable<DateTime> DtDe { get; set; }

        /// <summary>
        /// Data "Até" do filtro
        /// </summary>
        public Nullable<DateTime> DtAte { get; set; }

        /// <summary>
        /// Código do Assessor
        /// </summary>
        public Nullable<int> CodigoAssessor{ get; set; }

        /// <summary>
        /// Nome do Cliente
        /// </summary>
        public string DsNomeAssessor { get; set; }

        /// <summary>
        /// Data do cadastro
        /// </summary>
        public string DtCadastro { get; set; }

        /// <summary>
        /// Total de Clientes cadastrados
        /// </summary>
        public int TotalCliente { get; set; }

        /// <summary>
        /// Data de cadastro do cliente
        /// </summary>
        public string DataCadastro { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
