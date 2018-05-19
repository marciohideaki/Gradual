using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Contratos.Automacao.Ordens
{
    [ServiceContract(Namespace = "http://gradual")]
    [ServiceKnownType(typeof(AutomacaoInfo))]
    [ServiceKnownType(typeof(AutomacaoOrdensInfo))]
    [ServiceKnownType(typeof(AutomacaoPrecosTaxasInfo))]
    [ServiceKnownType(typeof(ItemAutomacaoOrdemHistoricoInfo))]
    [ServiceKnownType(typeof(ItemAutomacaoOrdemInfo))]
    [ServiceKnownType(typeof(ItemAutomacaoStatusEnum))]
    [ServiceKnownType(typeof(ItemAutomacaoTipoEnum))]
    [ServiceKnownType(typeof(ItemPrazoExecucaoEnum))]
    [ServiceKnownType(typeof(List<ItemAutomacaoOrdemHistoricoInfo>))]
    public interface IServicoAutomacaoOrdens : IServicoControlavel
    {
        //[OperationContract]
        //ArmarStartStopResponse ArmarStopMovel(ArmarStartStopRequest req);

        ///// <summary>
        ///// Registra um Stop Simultaneo no MDS Server.
        ///// </summary>
        ///// <param name="TemplateOrder">Atributos da Ordem </param>
        ///// <returns>Id do stopstart </returns>
        //[OperationContract]
        //ArmarStartStopResponse ArmarStopSimultaneo(ArmarStartStopRequest req);

        ///// <summary>
        ///// Registra um Stop Loss no MDS Server.
        ///// </summary>
        ///// <param name="TemplateOrder">Atributos da Ordem </param>
        ///// <returns>Id do stopstart </returns>
        //[OperationContract]
        //ArmarStartStopResponse ArmarStopLoss(ArmarStartStopRequest req);

        ///// <summary>
        ///// Registra um Stop Gain no MDS Server.
        ///// </summary>
        ///// <param name="TemplateOrder">Atributos da Ordem </param>
        ///// <returns>Id do stopstart </returns>
        //[OperationContract]
        //ArmarStartStopResponse ArmarStopGain(ArmarStartStopRequest req);

        ///// <summary>
        ///// Registra um start de compra.
        ///// </summary>
        ///// <param name="TemplateOrder">Atributos da Ordem </param>
        ///// <returns>Id do stopstart </returns>
        //[OperationContract]
        //ArmarStartStopResponse ArmarStartCompra(ArmarStartStopRequest req);

        AtualizaOrdemStartStopResponse AtualizaOrdemStartStop(AtualizaOrdemStartStopRequest req); 

        /// <summary>
        /// Cancela um ordem que esta em aberto no MDS
        /// </summary>
        /// <param name="Instrument">Código do Instrumento</param>
        /// <param name="id_stopstart">Código da Ordem a ser cancelada </param>
        /// <param name="id_stopstart_status"> Status da ordem</param>
        [OperationContract]
        CancelarStartStopOrdensResponse CancelaOrdemStopStart(CancelarStartStopOrdensRequest req);

        [OperationContract]
        SelecionarAutomacaoOrdemResponse SelecionarAutomacaoOrdem(SelecionarAutomacaoOrdemRequest req);

        [OperationContract]
        ListarItensAutomacaoOrdemResponse ListarItemsAutomacaoOrdem(ListarItensAutomacaoOrdemRequest req);

        [OperationContract]
        ReceberItemAutomacaoOrdemResponse ReceberItemAutomacaoOrdem(ReceberItemAutomacaoOrdemRequest req);

        [OperationContract]
        ExecutarAutomacaoOrdemResponse ExecutarAutomacaoOrdem(ExecutarAutomacaoOrdemRequest req);
    }
}
