using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteDistribuicaoRegionalInfo : ICodigoEntidade
    {
        #region Propriedades
        
        /// <summary>
        /// CPF do Cliente
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Nome do Cliente
        /// </summary>
        public string NM_Cliente { get; set; }
        
        /// <summary>
        /// Nome do Assessor
        /// </summary>
        public string NM_Assessor { get; set; }

        /// <summary>
        /// Sigla do Estado
        /// </summary>
        public string SG_Estado { get; set; }

        /// <summary>
        /// Nome da Cidade
        /// </summary>
        public string NM_Cidade { get; set; }

        /// <summary>
        /// Data de cadastro
        /// </summary>
        public DateTime DT_Criacao { get; set; }

        public string NmLogradouro { get; set; }

        public string NrPredio { get; set; }

        public string DsCompEndereco { get; set; }

        public string NmBairro { get; set; }

        public string CEP { get; set; }

        public string CEPExt { get; set; }

        public int Telefone { get; set; }

        public int TelefoneRamal { get; set; }

        public int Celular1 { get; set; }

        public int Celular1DDD { get; set; }

        public int Celular2 { get; set; }

        public int Celular2DDD { get; set; }

        public string Email { get; set; }

        #endregion
        
        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
