using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Interface para implementação do objeto de callback utilizado no modelo WCF.
    /// O mesmo modelo também é utilizado em momentos que não se está utilizando WCF.
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    [ServiceKnownType("RetornarTipos", typeof(LocalizadorTiposHelper))]
    public interface ICallbackEvento
    {
        /// <summary>
        /// Identificador único do callback
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Método para fazer o disparo do evento pelo servidor. Chamado quando o servidor quer
        /// informar algo ao cliente. Na implementação do objeto concreto, está chamada faz o disparo
        /// de um evento.
        /// </summary>
        /// <param name="evento"></param>
        [OperationContract]
        void SinalizarEvento(EventoInfo evento);

        /// <summary>
        /// Método para sinalização de eventos usando o próprio eventargs.
        /// Em teste.
        /// </summary>
        /// <param name="args"></param>
        [OperationContract]
        void SinalizarEvento2(EventArgs args);

        /// <summary>
        /// Método para retornar o Id do callback
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string ReceberId();
    }
}
