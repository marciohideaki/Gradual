using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;

namespace Gradual.OMS.Sistemas.Ordens
{
    /// <summary>
    /// Implementação do serviço de proxy entre ordens e MSMQ.
    /// Faz conexão com o MQ e assina os eventos do serviço de ordens.
    /// Quando chega algo do MQ, repassa para ordens. Quando chega algo 
    /// de ordens, repassa para o MQ.
    /// </summary>
    public class ServicoOrdensProxyMQ : IServicoOrdensProxyMQ
    {
        private MessageQueue _queueEntrada = null;
        private MessageQueue _queueSaida = null;
        private IServicoOrdens _servicoOrdens = null;
        private ServicoOrdensProxyMQConfig _config = null;
        private ServicoStatus _status = ServicoStatus.Parado;
        private bool _finalizando = false;
        
        #region IServicoControlavel Members

        /// <summary>
        /// Inicia o serviço
        /// </summary>
        public void IniciarServico()
        {
            // Sinaliza
            _finalizando = false;
            
            // Pega configuracoes
            _config = GerenciadorConfig.ReceberConfig<ServicoOrdensProxyMQConfig>();

            // Assina os eventos de ordens
            _servicoOrdens = Ativador.Get<IServicoOrdens>();
            _servicoOrdens.EventoSinalizacao += new EventHandler<SinalizarEventArgs>(_servicoOrdens_EventoSinalizacao);

            // Conecta fila de saida do MQ
            _queueSaida = new MessageQueue(_config.CaminhoFilaSaida);
            _queueSaida.Formatter = new BinaryMessageFormatter();

            // Conecta fila de entrada do MQ
            _queueEntrada = new MessageQueue(_config.CaminhoFilaEntrada);
            _queueEntrada.Formatter = new BinaryMessageFormatter();
            _queueEntrada.ReceiveCompleted += new ReceiveCompletedEventHandler(_queueEntrada_ReceiveCompleted);
            _queueEntrada.BeginReceive();

        }

        /// <summary>
        /// Para o serviço
        /// </summary>
        public void PararServico()
        {
            // Sinaliza 
            _finalizando = true;

            // Para fila de entrada do MQ
            _queueEntrada.Close();

            // Para fila de saida do MQ
            _queueSaida.Close();

            // Remove assinatura dos eventos de ordens
            _servicoOrdens.EventoSinalizacao -= new EventHandler<SinalizarEventArgs>(_servicoOrdens_EventoSinalizacao);
        }

        /// <summary>
        /// Retorna o status do serviço
        /// </summary>
        /// <returns></returns>
        public Gradual.OMS.Library.Servicos.ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        #endregion

        /// <summary>
        /// Recebimento de evento do sistema de ordens
        /// Faz a conversao da mensagem e coloca na fila de saida MQ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _servicoOrdens_EventoSinalizacao(object sender, SinalizarEventArgs e)
        {
            // Se estiver finalizando, ignora
            if (_finalizando)
                return;

            // Serializa a classe

            // Envia para o MQ
            
        }

        /// <summary>
        /// Recebimento da mensagem do sistema de conexões
        /// Faz a conversão da mensagem e chama o sistema de ordens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _queueEntrada_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            // Indica mensagem recebida
            _queueEntrada.EndReceive(e.AsyncResult);

            // Verifica o tipo da mensagem

            // Desserializa

            // Envia para o sistema de ordens

            // Espera próxima mensagem
            _queueEntrada.BeginReceive();
        }
    }
}
