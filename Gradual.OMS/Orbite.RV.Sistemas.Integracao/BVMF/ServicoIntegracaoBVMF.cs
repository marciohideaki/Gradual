using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Library;

using Orbite.Comum;
using Orbite.RV.Contratos.Integracao.BVMF;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    /// <summary>
    /// Implementação do serviço IServicoIntegracaoBVMF
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoIntegracaoBVMF : IServicoIntegracaoBVMF
    {
        #region IServicoIntegracaoBVMF Members

        /// <summary>
        /// Faz o download do arquivo do período e data informados.
        /// </summary>
        /// <param name="periodo">Periodo do arquivo a ser carregado</param>
        /// <param name="dataReferencia">Data referencia do arquivo. Considera apenas a parte pertinente da data de acordo com o parametro periodo.</param>
        /// <param name="stream">Stream para ser carregado com as informações do arquivo</param>
        public void ReceberArquivoMarketDataBovespa(PeriodoEnum periodo, DateTime dataReferencia, Stream stream)
        {
            // Pega arquivo de configuracao
            ServicoIntegracaoBVMFConfig config = GerenciadorConfig.ReceberConfig<ServicoIntegracaoBVMFConfig>();

            // Trata os erros - pode nao encontrar o arquivo
            try
            {
                // Monta o nome do arquivo
                string nomeArquivo = ((IServicoIntegracaoBVMF)this).ReceberNomeArquivoBovespa(periodo, dataReferencia);
                
                // Pega o arquivo da bovespa
                WebClient req = new WebClient();
                req.Credentials = System.Net.CredentialCache.DefaultCredentials;
                if (config.UtilizarProxy)
                {
                    System.Net.WebProxy proxy = new System.Net.WebProxy(config.ProxyEndereco, config.ProxyPorta);
                    proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    req.Proxy = proxy;
                }
                Stream webStream = req.OpenRead(config.EnderecoMarketDataBovespa + nomeArquivo + ".zip");

                // Salva o arquivo para fazer o unzip
                BinaryReader br = new BinaryReader(webStream);
                FileStream fs = File.Create(config.TempDir + @"\arq.zip");
                BinaryWriter bw = new BinaryWriter(fs);
                Byte[] buffer = new Byte[2048];
                while (true)
                {
                    int qtd = br.Read(buffer, 0, 2048);
                    if (qtd == 0) break;
                    buffer.Initialize();
                    bw.Write(buffer, 0, qtd);
                }
                fs.Close();

                // Descomprime
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.EnableRaisingEvents = false;
                p.StartInfo.FileName = config.CaminhoRar;
                p.StartInfo.Arguments = string.Format(@"x -y {0}\arq.zip {0}\", config.TempDir);
                p.Start();
                p.WaitForExit(60000);

                // Copia o resultado do arquivo para a memoria
                FileStream fs2 = File.Open(string.Format(@"{0}\{1}.txt", config.TempDir, nomeArquivo), FileMode.Open, FileAccess.Read);
                BinaryReader br2 = new BinaryReader(fs2);
                BinaryWriter bw2 = new BinaryWriter(stream);
                Byte[] buffer2 = new Byte[2048];
                while (true)
                {
                    int qtd = br2.Read(buffer2, 0, 2048);
                    if (qtd == 0) break;
                    buffer2.Initialize();
                    bw2.Write(buffer2, 0, qtd);
                }
                bw2.Flush();
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                // Repassa o erro
                throw(ex);
            }
        }

        /// <summary>
        /// Faz o download do arquivo do período e data informados.
        /// Overload para salvar diretamente para um arquivo
        /// </summary>
        /// <param name="periodo">Periodo do arquivo a ser carregado</param>
        /// <param name="dataReferencia">Data referencia do arquivo. Considera apenas a parte pertinente da data de acordo com o parametro periodo.</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser criado</param>
        public void ReceberArquivoMarketDataBovespa(PeriodoEnum periodo, DateTime dataReferencia, string nomeArquivo)
        {
            // Abre o arquivo
            FileStream fs = File.Create(nomeArquivo);

            // Pede o arquivo
            this.ReceberArquivoMarketDataBovespa(periodo, dataReferencia, fs);

            // Finaliza
            fs.Close();
        }

        public string ReceberNomeArquivoBovespa(PeriodoEnum periodo, DateTime dataReferencia)
        {
            // Monta o nome do arquivo
            string nomeArquivo = "";
            switch (periodo)
            {
                case PeriodoEnum.Ano:
                    nomeArquivo = "A" + dataReferencia.ToString("yyyy");
                    break;
                case PeriodoEnum.Mes:
                    nomeArquivo = "M" + dataReferencia.ToString("MMyyyy");
                    break;
                case PeriodoEnum.Dia:
                    nomeArquivo = "D" + dataReferencia.ToString("ddMMyyyy");
                    break;
            }

            // Retorna
            return string.Format(@"COTAHIST_{0}", nomeArquivo);
        }

        #endregion
    }
}
