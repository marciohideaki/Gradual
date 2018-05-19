///////////////////////////////////////////////////////////
//  ItemAutomacaoOrdemInfo.cs
//  Implementation of the Class ItemAutomacaoOrdemInfo
//  Generated by Enterprise Architect
//  Created on:      23-abr-2010 15:33:13
//  Original author: amiguel
///////////////////////////////////////////////////////////


using System;

using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace Gradual.OMS.Contratos.Automacao.Ordens.Dados
{
    [Serializable]
    [DataContract]
    [ServiceKnownType(typeof(ItemAutomacaoOrdemHistoricoInfo))]
    public class ItemAutomacaoOrdemInfo
    {

        /// <summary>
        /// Cont�m os dados da automa��o que est� sendo executada.
        /// </summary>
        [DataMember]
        public AutomacaoInfo AutomacaoInfo { get; set; }
        [DataMember]
        public string CodigoItemAutomacaoOrdem { get; set; }
        [DataMember]
        public DateTime? DataDisparoOrdem { get; set; }
        [DataMember]
        public DateTime? DataExecucao { get; set; }
        [DataMember]
        public DateTime? DataOrdemEnvio { get; set; }
        [DataMember]
        public DateTime? DataValidade { get; set; }
        [DataMember]
        public ItemPrazoExecucaoEnum PrazoExecucao { get; set; }
        [DataMember]
        public char ItemAtivo { get; set; }
        [DataMember]
        public List<ItemAutomacaoOrdemHistoricoInfo> Historico { get; set; }

        public ItemAutomacaoOrdemInfo()
        {
            AutomacaoInfo = new AutomacaoInfo();
            Historico = new List<ItemAutomacaoOrdemHistoricoInfo>();
        }

    }//end ItemAutomacaoOrdemInfo

}//end namespace Dados