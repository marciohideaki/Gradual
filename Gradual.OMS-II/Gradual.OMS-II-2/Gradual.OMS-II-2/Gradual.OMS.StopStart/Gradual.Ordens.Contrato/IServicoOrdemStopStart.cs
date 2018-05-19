using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;

namespace Gradual.OMS.Ordens.StartStop.Lib
{
    /// <summary>
    /// Rafael Sanches Garcia
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoOrdemStopStart
    {
        //[OperationContract]
        //ArmarStartStopResponse ArmarStopMovel(ArmarStartStopRequest req, bool pReenviando);

        ///// <summary>
        ///// Registra um Stop Simultaneo no MDS Server.
        ///// </summary>
        ///// <param name="req">Atributos de StopStart Request </param>
        ///// <param name="pReenviando">Flag de aviso se estiver reenviando o StopStart</param>
        ///// <returns>Id do stopstart </returns>
        //[OperationContract]
        //ArmarStartStopResponse ArmarStopSimultaneo(ArmarStartStopRequest req, bool pReenviando = false);

        ///// <summary>
        ///// Registra um Stop Loss no MDS Server.
        ///// </summary>
        ///// <param name="TemplateOrder">Atributos da Ordem </param>
        ///// <param name="pReenviando">Flag de aviso se estiver reenviando o StopStart</param>
        ///// <returns>Id do stopstart </returns>
        //[OperationContract]
        //ArmarStartStopResponse ArmarStopLoss(ArmarStartStopRequest req, bool pReenviando = false);

        ///// <summary>
        ///// Registra um Stop Gain no MDS Server.
        ///// </summary>
        ///// <param name="req">Atributos de StopStart Request </param>
        ///// <param name="pReenviando">Flag de aviso se estiver reenviando o StopStart</param>
        ///// <returns>Id do stopstart </returns>
        //[OperationContract]
        //ArmarStartStopResponse ArmarStopGain(ArmarStartStopRequest req, bool pReenviando = false);

        ///// <summary>
        ///// Registra um start de compra.
        ///// </summary>
        ///// <param name="req">Atributos de StopStart Request </param>
        ///// <param name="pReenviando">Flag de aviso se estiver reenviando o StopStart</param>
        ///// <returns>Id do stopstart </returns>
        //[OperationContract]
        //ArmarStartStopResponse ArmarStartCompra(ArmarStartStopRequest req, bool pReenviando = false);

        /// <summary>
        /// Cancela um ordem que esta em aberto no MDS
        /// </summary>
        /// <param name="Instrument">Código do Instrumento</param>
        /// <param name="id_stopstart">Código da Ordem a ser cancelada </param>
        /// <param name="id_stopstart_status"> Status da ordem</param>
        [OperationContract]
        CancelarStartStopOrdensResponse CancelaOrdemStopStart(CancelarStartStopOrdensRequest req);

        /// <summary>
        /// Registra um StopStart Geral
        /// </summary>
        /// <param name="req">Atributos de StopStart Request </param>
        /// <param name="pReenviando">Flag de aviso se estiver reenviando o StopStart</param>
        /// <returns>Id do stopstart </returns>
        [OperationContract]
        ArmarStartStopResponse ArmarStopStartGeral(ArmarStartStopRequest req);
    }
}
