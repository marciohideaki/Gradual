using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Orbite.Comum;

namespace Orbite.RV.Contratos.Integracao.BVMF
{
    /// <summary>
    /// Responsável por pegar os arquivos na BVMF
    /// </summary>
    [ServiceContract]
    public interface IServicoIntegracaoBVMF
    {
        /// <summary>
        /// Faz o download do arquivo do período e data informados.
        /// </summary>
        /// <param name="periodo">Periodo do arquivo a ser carregado</param>
        /// <param name="dataReferencia">Data referencia do arquivo. Considera apenas a parte pertinente da data de acordo com o parametro periodo.</param>
        /// <param name="stream">Stream para ser carregado com as informações do arquivo</param>
        [OperationContract]
        void ReceberArquivoMarketDataBovespa(PeriodoEnum periodo, DateTime dataReferencia, Stream stream);

        /// <summary>
        /// Faz o download do arquivo do período e data informados.
        /// Overload para salvar diretamente para um arquivo
        /// </summary>
        /// <param name="periodo">Periodo do arquivo a ser carregado</param>
        /// <param name="dataReferencia">Data referencia do arquivo. Considera apenas a parte pertinente da data de acordo com o parametro periodo.</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser criado</param>
        [OperationContract]
        void ReceberArquivoMarketDataBovespa(PeriodoEnum periodo, DateTime dataReferencia, string nomeArquivo);

        /// <summary>
        /// Monta o nome do arquivo a ser recuperado na bovespa
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="dataReferencia"></param>
        /// <returns></returns>
        [OperationContract]
        string ReceberNomeArquivoBovespa(PeriodoEnum periodo, DateTime dataReferencia);
    }
}
