using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

using Orbite.Comum;
using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    /// <summary>
    /// Implementacao do servico de persistencia para persistencia em diretório.
    /// </summary>
    public class ServicoIntegracaoBVMFPersistenciaDiretorio : IServicoIntegracaoBVMFPersistencia
    {
        private ServicoIntegracaoBVMFPersistenciaDiretorioConfig _config = 
            GerenciadorConfig.ReceberConfig<ServicoIntegracaoBVMFPersistenciaDiretorioConfig>();

        #region IServicoIntegracaoBVMFPersistencia Members

        public List<ArquivoBVMFInfo> ReceberListaArquivos()
        {
            // Pega o servico para retornar a lista
            IServicoIntegracaoBVMFArquivos servicoIntegracaoArquivos = Ativador.Get<IServicoIntegracaoBVMFArquivos>();

            // Retorna a lista de arquivos
            return servicoIntegracaoArquivos.ListarDiretorio(_config.Diretorio); ;
        }

        public void PersistirArquivo(ArquivoBVMFInfo arquivoInfo, Stream arquivo)
        {
            // Pega o nome do arquivo a persistir
            string nomeArquivo = criarNomeArquivo(arquivoInfo);

            // Prepara leitor
            BinaryReader reader = new BinaryReader(arquivo);

            // Salva o arquivo no diretorio da persistencia
            File.WriteAllBytes(nomeArquivo, reader.ReadBytes((int)arquivo.Length));
        }

        public void RemoverArquivo(ArquivoBVMFInfo arquivoInfo)
        {
            // Pega o nome do arquivo a remover
            string nomeArquivo = criarNomeArquivo(arquivoInfo);

            // Remove
            if (File.Exists(nomeArquivo))
                File.Delete(nomeArquivo);
        }

        public Stream ReceberArquivo(ArquivoBVMFInfo arquivoInfo)
        {
            // Se nao informou o nome do arquivo, descobre pelo layout e data de movimento
            if (arquivoInfo.NomeArquivo == null)
            {
                // Pega o diretorio
                List<ArquivoBVMFInfo> diretorio = this.ReceberListaArquivos();

                // Tenta encontrar 
                ArquivoBVMFInfo arquivoEncontrado = (from a in diretorio
                                                     where a.DataMovimento == arquivoInfo.DataMovimento &&
                                                           a.Layout.Nome == arquivoInfo.Layout.Nome
                                                     select a).FirstOrDefault();

                // Apenas se encontrou
                if (arquivoEncontrado != null)
                    arquivoInfo = arquivoEncontrado;
            }
            
            // Pega o nome do arquivo a abrir
            string nomeArquivo = criarNomeArquivo(arquivoInfo);

            // Existe o arquivo?
            if (File.Exists(nomeArquivo))
                return new FileStream(nomeArquivo, FileMode.Open);
            else
                return null;
        }

        #endregion

        private string criarNomeArquivo(ArquivoBVMFInfo arquivoInfo)
        {
            // Monta o nome do arquivo
            FileInfo fi = new FileInfo(arquivoInfo.NomeArquivo);
            DirectoryInfo di = new DirectoryInfo(_config.Diretorio);
            return di.FullName + @"\" + fi.Name;
        }
    }
}
