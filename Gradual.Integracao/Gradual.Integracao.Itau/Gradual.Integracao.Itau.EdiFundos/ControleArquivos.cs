using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Integracao.Itau.EdiFundos
{
    [Serializable]
    public class ControleArquivos 
    {
        //Hash MD5 do arquivo
        public string MD5Sum { get; set; }

        //Timestamp do ultimo processamento
        public DateTime Timestamp { get; set; }

        //Nome do arquivo
        public string Filename { get; set; }

        // Path completo do arquivo;
        public string Path { get; set; }
    }
}
