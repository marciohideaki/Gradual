using Gradual.OMS.Contratos.CadastroPapeis.Mensagens;
using Gradual.OMS.Contratos.Comum.Mensagens;
using System.ServiceModel;

namespace Gradual.OMS.Contratos.CadastroPapeis
{
    [ServiceContract(Namespace="http://gradual")]
    public interface IServicoCadastroPapeis
    {
        /// <summary>
        /// Função para os dados do papel negociado
        /// </summary>
        /// <param name="Request">Mensagem de request com o ativo a ser consultado</param>
        /// <returns></returns>
        [OperationContract]
        ConsultarPapelNegociadoResponse ConsultarPapelNegociado(ConsultarPapelNegociadoRequest Request);
    }
}
