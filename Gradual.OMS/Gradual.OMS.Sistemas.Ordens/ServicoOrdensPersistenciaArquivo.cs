using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Ordens
{
    public class ServicoOrdensPersistenciaArquivo : IServicoOrdensPersistencia
    {
        // Config
        private ServicoOrdensPersistenciaArquivoConfig _config = GerenciadorConfig.ReceberConfig<ServicoOrdensPersistenciaArquivoConfig>();

        // Lista de ordens
        private Dictionary<string, OrdemInfo> _ordens = new Dictionary<string, OrdemInfo>();

        #region Construtores e Destrutor

        public ServicoOrdensPersistenciaArquivo()
        {
            // Carrega o arquivo, se existir
            if (File.Exists(_config.ArquivoPersistencia))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = File.OpenRead(_config.ArquivoPersistencia);
                _ordens = (Dictionary<string, OrdemInfo>)formatter.Deserialize(stream);
                stream.Close();
            }
        }

        ~ServicoOrdensPersistenciaArquivo()
        {
            // Salva o arquivo
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_config.ArquivoPersistencia, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, _ordens);
            stream.Close();
        }

        #endregion

        #region IServicoOrdensPersistencia Members

        public SalvarOrdemResponse SalvarOrdem(SalvarOrdemRequest parametros)
        {
            // Inicializa
            OrdemInfo ordemInfo = null;

            // Apenas atualiza a coleção, o arquivo será salvo no final
            if (_ordens.ContainsKey(parametros.OrdemInfo.CodigoOrdem))
            {
                ordemInfo = _ordens[parametros.OrdemInfo.CodigoOrdem];
                ordemInfo.Merge(parametros.OrdemInfo);
            }
            else
            {
                ordemInfo = (OrdemInfo)parametros.OrdemInfo.Clone();
                _ordens.Add(ordemInfo.CodigoOrdem, ordemInfo);
            }

            // Adiciona a mensagem de historico
            ordemInfo.Historico.Add(parametros.MensagemSinalizacao);

            // Retorna
            return 
                new SalvarOrdemResponse() 
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        public ReceberOrdemResponse ReceberOrdem(ReceberOrdemRequest parametros)
        {
            // Retorna a ordem da coleção
            return 
                new ReceberOrdemResponse() 
                { 
                    OrdemInfo = _ordens[parametros.ClOrdID]
                };
        }

        public ListarOrdensResponse ListarOrdens(ListarOrdensRequest parametros)
        {
            // Retorna a lista de acordo com os filtros
            return 
                new ListarOrdensResponse() 
                { 
                    Ordens = _ordens.Values.ToList<OrdemInfo>()
                };
        }

        #endregion
    }
}
