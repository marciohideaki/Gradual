using System;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using System.ServiceModel;
using System.Collections.Generic;

namespace Gradual.OMS.Contratos.CadastroPapeis.Mensagens
{
    [Serializable]
    public class ConsultarPapelNegociadoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Ativos a serem consultados
        /// </summary>
        public List<string> LstAtivos;

        /// <summary>
        /// Tipo de Mercado
        /// </summary>
        public int TipoMercado{ get; set; }

        /// <summary>
        /// Descrição do tipo de mercado
        /// </summary>
        public string DescTipoMercado { get; set; }

        /// <summary>
        /// Data de Vencimento
        /// </summary>
        public Nullable<DateTime> DataVencimento { get; set; }
    }
}
