using System;
using System.ServiceModel;
using Gradual.Intranet.Contratos.Mensagens;
using System.Collections.Generic;

namespace Gradual.Intranet.Contratos
{
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoCliente
    {
        [OperationContract]
        List<SinacorComboResponse> SinacorListar(SinacorComboRequest pParametros);

        [OperationContract]
        SinacorComboResponse SinacorSelecionar(SinacorComboRequest pParametros);

        [OperationContract]
        BuscarClientesResponse BuscarClientes(BuscarClientesRequest pParametros);

        [OperationContract]
        CadastrarEntidadeFilhaResponse CadastrarEntidadeFilha(CadastrarEntidadeFilhaRequest pParametros);

        [OperationContract]
        ExcluirEntidadeFilhaResponse ExcluirEntidadeFilha(ExcluirEntidadeFilhaRequest pParametros);

        [OperationContract]
        ListarEntidadeFilhaResponse ListarEntidadeFilha(ListarEntidadeFilhaRequest pParametros);

        [OperationContract]
        SelecionarEntidadeFilhaResponse SelecionarEntidadeFilha(SelecionarEntidadeFilhaRequest pParametros);
       
    }
}
