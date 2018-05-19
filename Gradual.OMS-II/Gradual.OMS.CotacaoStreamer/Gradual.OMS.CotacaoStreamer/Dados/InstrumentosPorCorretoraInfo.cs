using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    /// <summary>
    /// Dados de Instrumentos armazenados para cada Corretora
    /// </summary>
    [Serializable]
    public class InstrumentosPorCorretoraInfo
    {
        /// <summary>
        /// Dicionario dos maiores volumes de Instrumentos
        /// </summary>
        [DataMember]
        public SortedDictionary<CorretorasInfo, string> DictMaioresVolumes { get; set; }
        public SortedDictionary<string, CorretorasInfo> DictMaioresVolumesPorInstrumento { get; set; }
    }
}

