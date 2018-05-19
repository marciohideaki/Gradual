﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Arquivo de dados de informações de serviço.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://gradual")]
    public class ServicoInfo
    {
        /// <summary>
        /// ID da instância do serviço.
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// Nome da interface que está implementando o serviço. Por exemplo: Gradual.OMS.Sistemas.Comum.ServicoPersistencia
        /// </summary>
        [DataMember]
        [XmlElement("NomeInterface")]
        public List<string> NomeInterface { get; set; }

        /// <summary>
        /// Nome do tipo da instância a ser criado. Por exemplo: Gradual.OMS.Contratos.Comum.IServicoPersistencia
        /// </summary>
        [DataMember]
        public string NomeInstancia { get; set; }

        /// <summary>
        /// Indica como deve ser a ativação default do serviço.
        /// Utilizado quando o serviço será ativado via informações contidas no localizador. Caso a ativação default seja 
        /// local, faz com que o ativador primeiro tente achar o serviço localmente, e em caso de falha, faz a ativação via WCF.
        /// </summary>
        [DataMember]
        public ServicoAtivacaoTipo AtivacaoDefaultTipo { get; set; }

        /// <summary>
        /// Indica se deve registrar o serviço no localizador.
        /// </summary>
        [DataMember]
        public bool RegistrarLocalizador { get; set; }

        /// <summary>
        /// Lista de endpoints a serem criados no WCF.
        /// </summary>
        [DataMember]
        public List<ServicoEndPointInfo> EndPoints { get; set; }

        /// <summary>
        /// Lista de endpoints a serem criados no WCF.
        /// </summary>
        [DataMember]
        public bool AtivarWCF { get; set; }

        /// <summary>
        /// Indica se o serviço está habilitado e deve ser iniciado
        /// </summary>
        [DataMember]
        public bool Habilitado { get; set; }

        /// <summary>
        /// WCFAddress - Endereco do endpoint do servico (opcional)
        /// </summary>
        [DataMember]
        [XmlElement("WCFBaseAddress", IsNullable = true)]
        public List<string> WCFBaseAddress { get; set; }

        /// <summary>
        /// Throttle - Numero maximo de chamadas simultaneas (opcional)
        /// </summary>
        [DataMember]
        public Nullable<int> MaxConcurrentCalls { get; set; }

        /// <summary>
        /// Throttle - Numero maximo de instancias simultaneas (opcional)
        /// </summary>
        [DataMember]
        public Nullable<int> MaxConcurrentInstances { get; set; }

        /// <summary>
        /// Throttle - Numero maximo de sessoes simultaneas (opcional)
        /// </summary>
        [DataMember]
        public Nullable<int> MaxConcurrentSessions { get; set; }

        /// <summary>
        /// Construtor. Carrega valores default
        /// </summary>
        public ServicoInfo()
        {
            this.RegistrarLocalizador = false;
            this.AtivacaoDefaultTipo = ServicoAtivacaoTipo.Local;
            this.EndPoints = new List<ServicoEndPointInfo>();
            this.NomeInterface = new List<string>();
            this.WCFBaseAddress = new List<string>();
            this.Habilitado = true;
            this.MaxConcurrentCalls = 0;
            this.MaxConcurrentInstances = 0;
            this.MaxConcurrentSessions = 0;
        }
    }
}
