using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.Integracao.BVMF.Dados;

namespace Orbite.RV.Contratos.Integracao.BVMF
{
    /// <summary>
    /// Responsável por persistir os arquivos BVMF
    /// </summary>
    public interface IServicoIntegracaoBVMFPersistencia
    {
        /// <summary>
        /// Retorna a lista de arquivos contidos no repositório
        /// </summary>
        /// <returns></returns>
        List<ArquivoBVMFInfo> ReceberListaArquivos();

        /// <summary>
        /// Realiza a persistencia de um arquivo
        /// </summary>
        /// <param name="arquivoInfo">Informações do arquivo</param>
        /// <param name="arquivo">Stream do arquivo</param>
        void PersistirArquivo(ArquivoBVMFInfo arquivoInfo, Stream arquivo);

        /// <summary>
        /// Remove um arquivo da persistencia
        /// </summary>
        /// <param name="arquivoInfo"></param>
        void RemoverArquivo(ArquivoBVMFInfo arquivoInfo);

        /// <summary>
        /// Retorna um arquivo da persistencia
        /// </summary>
        /// <param name="arquivoInfo"></param>
        /// <returns></returns>
        Stream ReceberArquivo(ArquivoBVMFInfo arquivoInfo);
    }
}
