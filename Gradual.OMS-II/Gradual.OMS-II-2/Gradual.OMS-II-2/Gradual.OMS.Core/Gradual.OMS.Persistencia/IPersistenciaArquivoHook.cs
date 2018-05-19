using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Interface para implementações de mediadores para a persistencia de arquivo.
    /// Um mediador pode processar a mensagem antes do servico de persistencia.
    /// </summary>
    public interface IPersistenciaArquivoHook
    {
        /// <summary>
        /// Solicita inicialização do mediador passando a referencia do módulo de persistencia
        /// </summary>
        /// <param name="instancia"></param>
        void Inicializar(PersistenciaArquivo instancia);
    }
}
