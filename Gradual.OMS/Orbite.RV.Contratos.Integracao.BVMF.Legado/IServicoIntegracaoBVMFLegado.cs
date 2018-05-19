using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.Integracao.BVMF.Legado.Mensagens;

namespace Orbite.RV.Contratos.Integracao.BVMF.Legado
{
    /// <summary>
    /// Interface para o serviço de integração com o sistema legado de conversão de arquivos.
    /// </summary>
    public interface IServicoIntegracaoBVMFLegado
    {
        /// <summary>
        /// Operação para realizar a conversão do arquivo legado no arquivo atual.
        /// Converte os arquivos utilizando o serviço de persistencia dos layouts BVMF, ou seja,
        /// o arquivo que será salvo está configurado no serviço de persistencia BVMF.
        /// </summary>
        /// <param name="caminhoArquivoLegado">Caminho do arquivo a ser convertido</param>
        ConverterBVMFLegadoResponse ConverterArquivo(ConverterBVMFLegadoRequest parametros);
    }
}
