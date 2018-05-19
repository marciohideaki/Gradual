using System;
using System.Threading;

using Gradual.OMS.Library;
using log4net;

namespace Gradual.OMS.Mensageria
{
    public class ServicoEcho : IServicoEcho
    {
        #region Variáveis Locais
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Mensagem a ser enviada nos eventos do timer
        /// </summary>
        public string _mensagemTimer { get; set; }
        
        /// <summary>
        /// Timer para envio de echos em intervalos
        /// </summary>
        private Timer _timer = null;

        #endregion

        #region IServicoEcho Members

        /// <summary>
        /// Solicita a execução do echo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ExecutarEchoResponse ExecutarEcho(ExecutarEchoRequest parametros)
        {
            // Prepara resposta
            ExecutarEchoResponse resposta =
                new ExecutarEchoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            
            // Executa de acordo com o pedido
            switch (parametros.TipoFuncao)
            {
                case ExecutarEchoTipoFuncaoEnum.EcoarMensagem:
                    if (this.EventoEcho != null)
                        this.EventoEcho(this, new EchoEventArgs() { Mensagem = parametros.Mensagem });
                    break;
                case ExecutarEchoTipoFuncaoEnum.LigarTimer:
                    if (_timer != null)
                    {
                        // Salva mensagem que será informada no timer
                        _mensagemTimer = parametros.Mensagem;

                        // Cria o timer
                        _timer = 
                            new Timer(
                                new TimerCallback(timerCallback), 
                                null, 
                                new TimeSpan(0, 0, parametros.TempoTimer), 
                                new TimeSpan(0, 0, parametros.TempoTimer));
                    }
                    break;
                case ExecutarEchoTipoFuncaoEnum.DesligarTimer:
                    if (_timer != null)
                    {
                        // Finaliza o timer
                        _timer.Dispose();
                        _timer = null;
                    }
                    break;
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Evento de retorno do echo
        /// </summary>
        public event EventHandler<EchoEventArgs> EventoEcho;

        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Callback do timer
        /// </summary>
        /// <param name="param"></param>
        private void timerCallback(object param)
        {
            // Dispara o evento com a mensagem
            if (this.EventoEcho != null)
                this.EventoEcho(
                    this, new EchoEventArgs() { Mensagem = _mensagemTimer });

            // Faz o log
            logger.Info("Echo pelo timer: " + _mensagemTimer);
        }

        #endregion
    }
}
