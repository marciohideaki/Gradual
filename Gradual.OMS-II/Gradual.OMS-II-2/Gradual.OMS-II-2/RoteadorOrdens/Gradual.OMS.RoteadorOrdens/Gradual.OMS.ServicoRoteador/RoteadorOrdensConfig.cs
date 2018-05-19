using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.ServicoRoteador
{
    [Serializable]
    public class CanalConfig
    {
        /// <summary>
        /// Codigo da bolsa (BOVESPA/BMF)
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Codigo do operador (300,302,etc)
        /// </summary>
        public string ChannelID { get; set; }

        /// <summary>
        /// Endereco do endpoint do servicos (metodos de envio/cancelamento/modificacao)
        /// </summary>
        public string EndPointRoteador { get; set; }

        /// <summary>
        /// Endereco do endpoint dos eventos
        /// </summary>
        public string EndPointAssinatura { get; set; }

        /// <summary>
        /// Endereco do endpoint do servicos administrativos
        /// </summary>
        public string EndPointRoteadorAdm { get; set; }
    }

    [Serializable]
    public class RoteadorOrdensConfig
    {
        /// <summary>
        /// Lista das informacoes dos canais disponiveis
        /// </summary>
        public List<CanalConfig> Canais { get; set; }


        /// <summary>
        /// Path para o arquivo de serializacao da fila de eventos de acompanhamento de ordens
        /// </summary>
        public string PathFilaOrdens { get; set; }
    }
}
