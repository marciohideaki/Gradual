using System;
using Gradual.OMS.Library;


namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Interface para serviço de echo.
    /// Serviço para teste de eventos da mensageria
    /// </summary>
    public interface IServicoEcho
    {
        /// <summary>
        /// Solicita a execução do echo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ExecutarEchoResponse ExecutarEcho(ExecutarEchoRequest parametros);

        /// <summary>
        /// Evento de retorno do echo
        /// </summary>
        event EventHandler<EchoEventArgs> EventoEcho;
    }
}
