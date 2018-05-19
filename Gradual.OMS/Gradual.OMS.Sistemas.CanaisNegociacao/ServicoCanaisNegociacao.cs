using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.CanaisNegociacao;
using Gradual.OMS.Contratos.CanaisNegociacao.Dados;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.CanaisNegociacao
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoCanaisNegociacao : IServicoCanaisNegociacao
    {
        private ServicoStatus _servicoStatus = ServicoStatus.Parado;
        private Dictionary<string, CanalNegociacaoBase> _canais = new Dictionary<string, CanalNegociacaoBase>();

        #region IServicoControlavel Members

        void IServicoControlavel.IniciarServico()
        {
            // Recebe configuracoes
            ServicoCanaisNegociacaoConfig config = GerenciadorConfig.ReceberConfig<ServicoCanaisNegociacaoConfig>();

            // Pega referencia ao sistema de ordens
            IServicoOrdens servicoOrdens = Ativador.Get<IServicoOrdens>();

            // Cria cada canal
            foreach (CanalInfo canalInfo in config.Canais)
            {
                CanalNegociacaoBase canal = null;

                // Verifica se esta criado
                if (!_canais.ContainsKey(canalInfo.IdCanal))
                {
                    // Cria o canal
                    canal =
                        (CanalNegociacaoBase)
                            Activator.CreateInstance(
                                Type.GetType(canalInfo.TipoCanal));

                    // Seta o id
                    canal.Codigo = canalInfo.IdCanal;
                    canal.ServicoOrdens = servicoOrdens;

                    // Adiciona na colecao
                    _canais.Add(canalInfo.IdCanal, canal);
                }
                else
                {
                    // Pega referencia ao canal
                    canal = _canais[canalInfo.IdCanal];
                }

                // Inicia o canal
                canal.Iniciar();
            }

            // Sinaliza
            _servicoStatus = ServicoStatus.EmExecucao;

            // Efetua log
            Log.EfetuarLog("Serviço de canais iniciado", LogTipoEnum.Passagem, ModulosOMS.ModuloCanais);
        }

        void IServicoControlavel.PararServico()
        {
            // Para cada canal
            foreach (KeyValuePair<string, CanalNegociacaoBase> item in _canais)
                item.Value.Parar();
            
            // Sinaliza
            _servicoStatus = ServicoStatus.Parado;

            // Efetua log
            Log.EfetuarLog("Serviço de canais finalizado", LogTipoEnum.Passagem, ModulosOMS.ModuloCanais);
        }

        ServicoStatus IServicoControlavel.ReceberStatusServico()
        {
            return _servicoStatus;
        }

        #endregion

        #region IServicoCanaisNegociacao Members

        void IServicoCanaisNegociacao.EnviarMensagem(string codigoCanal, MensagemRequestBase mensagem)
        {
            // Repassa a mensagem para o canal solicitado
            _canais[codigoCanal].ProcessarMensagem(mensagem);
        }

        #endregion
    }
}
