using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class SinacorListaInfo : ICodigoEntidade
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
        
        public SinacorListaInfo() { }
        
        public SinacorListaInfo(eInformacao pInformacao) 
        {
            this.Informacao = pInformacao;
        }

        #endregion
    }
}
