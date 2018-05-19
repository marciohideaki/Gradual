using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Risco.Enum;

namespace Gradual.Monitores.Risco.Info
{
    /// <summary>
    /// Classe de request da Exposição do cliente.
    /// </summary>
    [Serializable]
    [DataContract]
    public class MonitorLucroPrejuizoRequest
    {        
        /// <summary>
        /// Codigo do Assessor que está fazendo o request.
        /// o Codigo do assessor é necessário para efetuar o filtro de clientes que o assessor pode ver 
        /// na sua barra
        /// </summary>
        [DataMember]
        public int Assessor { set; get; }

        /// <summary>
        /// Codigo de cliente 
        /// </summary>
        [DataMember]
        public int Cliente { set; get; }

        /// <summary>
        /// Código de login do assessor que está fazendo o request.
        /// O Codigo de login do assessor é necessári opara efetuar o filtro de clientes que o assessor 
        /// pode ver na sua barra
        /// </summary>
        [DataMember]
        public int? CodigoLogin { set; get; }

        /// <summary>
        /// Semáforo é usado para efetuar o filtro na exposição dos clientes
        /// </summary>
        [DataMember]
        public EnumSemaforo Semaforo { set; get; }

        /// <summary>
        /// Proporção Prejuízo do filtro sobre a proporção de  da exposição dos clientes
        /// </summary>
        [DataMember]
        public EnumProporcaoPrejuizo ProporcaoPrejuizo { set; get; }

        /// <summary>
        /// Novo range é usado para a paginação dos registros da tela da intranet de monitoramento de risco geral.
        /// Onde vem a exposição de mais de uma cliente. É a variável onde é guardada a página atual que está vindo no resquest.
        /// (IMPORTANTE) a paginação é necessária para o carregamento de todos os registro por vez , de 50 em 50, já que dá erro de wcf pelo 
        /// tamanho da mensagem do retorno da consulta.
        /// </summary>
        [DataMember]
        public int? NovoRange { get; set; }
    }
}
