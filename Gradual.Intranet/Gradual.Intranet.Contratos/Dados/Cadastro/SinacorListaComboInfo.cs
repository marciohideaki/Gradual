using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Dados
{
    public class SinacorListaComboInfo : ICodigoEntidade
    {
        #region Propriedades

        /// <summary>
        /// Informação(Tabela) a ser pesquisada
        /// </summary>
        public eInformacao Informacao { get; set; }

        /// <summary>
        /// Código
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Filtro para pesquisa.
        /// </summary>
        public string Filtro { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        public string Value { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Construtor

        public SinacorListaComboInfo() { }

        public SinacorListaComboInfo(eInformacao pInformacao)
        {
            this.Informacao = pInformacao;
        }

        #endregion
    }
}
