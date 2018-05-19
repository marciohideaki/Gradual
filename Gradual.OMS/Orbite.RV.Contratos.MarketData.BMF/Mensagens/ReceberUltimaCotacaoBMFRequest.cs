﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Orbite.RV.Contratos.MarketData.BMF.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de última cotação de ativos BMF
    /// </summary>
    public class ReceberUltimaCotacaoBMFRequest : MensagemRequestBase
    {
        /// <summary>
        /// Lista de ativos que se deseja a última cotação
        /// </summary>
        public List<string> Ativos { get; set; }

        /// <summary>
        /// Ativo que se deseja a ultima cotação
        /// </summary>
        public string Ativo { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberUltimaCotacaoBMFRequest()
        {
            this.Ativos = new List<string>();
        }
    }
}
