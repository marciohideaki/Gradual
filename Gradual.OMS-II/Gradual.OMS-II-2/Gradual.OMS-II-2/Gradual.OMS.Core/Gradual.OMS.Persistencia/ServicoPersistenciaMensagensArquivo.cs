using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Implementação do serviço de persistencia de mensagens em Arquivo
    /// </summary>
    public class ServicoPersistenciaMensagensArquivo : IServicoPersistenciaMensagens
    {
        #region Variaveis Locais

        /// <summary>
        /// Lista interna de mensagens.
        /// Dicionário que tem como chave o código da mensagem.
        /// </summary>
        private Dictionary<string, MensagemBase> _mensagens = new Dictionary<string, MensagemBase>();

        /// <summary>
        /// Objeto de configurações do servico de persistencia
        /// </summary>
        private ServicoPersistenciaMensagensArquivoConfig _config = GerenciadorConfig.ReceberConfig<ServicoPersistenciaMensagensArquivoConfig>();

        #endregion

        #region Construtor e Destrutor

        /// <summary>
        /// Construtor. Carrega o arquivo de persistencia se existir.
        /// </summary>
        public ServicoPersistenciaMensagensArquivo()
        {
            // Carrega o arquivo, se existir
            if (File.Exists(_config.ArquivoPersistencia))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = File.OpenRead(_config.ArquivoPersistencia);
                _mensagens = (Dictionary<string, MensagemBase>)formatter.Deserialize(stream);
                stream.Close();
            }
        }

        /// <summary>
        /// Destrutor. Salva o arquivo de persistencia.
        /// </summary>
        ~ServicoPersistenciaMensagensArquivo()
        {
            // Salva o arquivo
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_config.ArquivoPersistencia, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, _mensagens);
            stream.Close();
        }

        #endregion

        #region IServicoPersistenciaMensagens Members

        /// <summary>
        /// Salva a mensagem.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarMensagemResponse SalvarMensagem(SalvarMensagemRequest parametros)
        {
            // Faz o clone da mensagem para não ter problemas de referencia
            MensagemBase mensagem = parametros.Mensagem.ClonarObjeto();

            // Salva a mensagem na coleção interna ou atualiza o objeto
            if (_mensagens.ContainsKey(mensagem.CodigoMensagem))
                _mensagens[mensagem.CodigoMensagem] = mensagem;
            else
                _mensagens.Add(mensagem.CodigoMensagem, mensagem);
            
            // Retorna
            return new SalvarMensagemResponse();
        }

        /// <summary>
        /// Retorna uma mensagem.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberMensagemResponse ReceberMensagem(ReceberMensagemRequest parametros)
        {
            // Inicializa
            MensagemBase mensagem = null;

            // Pesquisa mensagem na coleção
            if (_mensagens.ContainsKey(parametros.CodigoMensagem))
                mensagem = _mensagens[parametros.CodigoMensagem];

            // Retorna
            return
                new ReceberMensagemResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Mensagem = mensagem
                };
        }

        /// <summary>
        /// Lista as mensagens.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarMensagensResponse ListarMensagens(ListarMensagensRequest parametros)
        {
            // Por enquanto retorna a lista completa
            List<MensagemBase> retorno = null;
            retorno = (from m in _mensagens
                       select m.Value).ToList<MensagemBase>();

            // Retorna
            return
                new ListarMensagensResponse()
                {
                    Mensagens = retorno 
                };
        }

        #endregion
    }
}
