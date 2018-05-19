using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    public class DadosFilaResumoCorretoras
    {
        public Socket socketClient { get; set; }
        public ResumoCorretorasInfo resumoCorretorasInfo { get; set; }
    }

    public class ResumoCorretorasInfo
    {
        /// <summary>
        /// Tipo de Segmento de Mercado / Instrumento
        /// 
        /// 00: Todos
        /// 01: Bovespa - A Vista
        /// 02: Bovespa - Termo
        /// 04: Bovespa - Opcoes
        /// 
        /// ou
        /// 
        /// Instrumento
        /// 
        /// </summary>
        [DataMember]
        public string instrumento { get; set; }

        /// <summary>
        /// Data e Hora
        /// </summary>
        [DataMember]
        public CabecalhoInfo cabecalho { get; set; }

        /// <summary>
        /// Lista de ocorrencias da corretora
        /// </summary>
        [DataMember]
        public List<CorretoraInfo> resumo { get; set; }
    }
}
