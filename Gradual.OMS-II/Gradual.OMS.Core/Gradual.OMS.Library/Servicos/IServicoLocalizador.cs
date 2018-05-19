using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Servico que implementa um diretório de serviços.
    /// Fornece transparencia na ativação dos serviços.
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoLocalizador 
    {
        [OperationContract]
        List<ServicoInfo> Consultar();

        [OperationContract(Name="ConsultarComServInt")]
        ServicoInfo Consultar(string servicoInterface);

        [OperationContract(Name = "ConsultarComServIntEID")]
        ServicoInfo Consultar(string servicoInterface, string id);

        [OperationContract]
        void Registrar(ServicoInfo servico);

        [OperationContract]
        void Remover(string servicoInterface);

        [OperationContract(Name = "RemoverComID")]
        void Remover(string servicoInterface, string id);
    }

    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoLocalizadorReplicacao
    {
        [OperationContract]
        Dictionary<string, Dictionary<string, ServicoInfo>> ObterListaServicos();

        [OperationContract]
        void ReplicarLista(Dictionary<string, Dictionary<string, ServicoInfo>> lista);

        [OperationContract]
        void ReplicarRegistro(ServicoInfo servico);

        [OperationContract]
        void ReplicarRemocao(string servicoInterface);

        [OperationContract]
        void ReplicarRemocaoID(string servicoInterface, string id);
    }
}
