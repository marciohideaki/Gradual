using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    public class DadosFilaAcompanhamentoLeilao
    {
        public Socket socketClient { get; set; }
        public AcompanhamentoLeilaoInfo acompanhamentoLeilaoInfo { get; set; }
    }

    public class AcompanhamentoLeilaoInfo
    {
        /// <summary>
        /// Nome do Instrumento
        /// </summary>
        [DataMember]
        public string instrumento { get; set; }

        /// <summary>
        /// Dados do cabecalho
        /// </summary>
        [DataMember]
        public CabecalhoInfo cabecalho { get; set; }

        /// <summary>
        /// Lista de ocorrencias do Acompanhamento de Leilao
        /// </summary>
        [DataMember]
        public List<LeilaoInfo> negocio { get; set; }
    }
}
