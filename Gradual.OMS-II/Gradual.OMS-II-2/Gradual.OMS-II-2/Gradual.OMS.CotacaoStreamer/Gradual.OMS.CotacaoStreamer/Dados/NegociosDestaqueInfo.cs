using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    public class DadosFilaNegociosDestaque
    {
        public Socket socketClient { get; set; }
        public NegociosDestaqueInfo negociosDestaqueInfo { get; set; }
    }

    public class NegociosDestaqueInfo
    {
        /// <summary>
        /// Tipo de Destaque
        /// (99 - 1o digito eh o filtro de segmento de mercado, 2o digito eh o filtro do tipo de destaque)
        /// 
        /// Segmento de Mercado:
        /// 1: Bovespa - A Vista
        /// 2: Bovespa - Termo
        /// 3: Bovespa - Opcoes
        /// 4: BM&F - Futuro
        /// 5: BM&F - A Vista
        /// 6: BM&F - Opcoes A Vista
        /// 7: BM&F - Opcoes de Futuro
        /// 
        /// Tipo de Destaque:
        /// 1: Maiores Altas
        /// 2: Maiores Baixas
        /// 3: Maiores Volumes
        /// 4: Mais Negociadas
        /// 
        /// </summary>
        [DataMember]
        public string tipo { get; set; }

        /// <summary>
        /// Data e Hora
        /// </summary>
        [DataMember]
        public CabecalhoInfo cabecalho { get; set; }

        /// <summary>
        /// Lista de ocorrencias do destaque
        /// </summary>
        [DataMember]
        public List<DestaqueInfo> destaque { get; set; }
    }
}
