using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Orbite.RV.Contratos.Integracao.BVMF.Legado.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de conversão de arquivo de layouts antigo
    /// para a nova estrutura
    /// </summary>
    public class ConverterBVMFLegadoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Caminho do arquivo legado
        /// </summary>
        public string CaminhoArquivo { get; set; }
    }
}
