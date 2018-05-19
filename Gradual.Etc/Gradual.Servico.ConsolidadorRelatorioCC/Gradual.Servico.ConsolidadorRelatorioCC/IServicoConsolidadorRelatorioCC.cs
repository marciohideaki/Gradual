using System.ComponentModel;
using System.ServiceModel;
using Servico.Contratos.ConsolidadorRelatorioCC.Mensageria;

namespace Servico.Contratos.ConsolidadorRelatorioCC
{
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoConsolidadorRelatorioCC
    {
        //[OperationContract]
        //[Category("ContaCorrente")]
        //void AlimentarConsultaDNDelegate(object pParametro);
    }
}
