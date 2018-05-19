using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Servicos.Contratos.TesteWCF.Mensagens;
using System.ServiceModel;

namespace Gradual.Servicos.Contratos.TesteWCF
{
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoTesteWcf
    {
        ReceberMensagemDeTextoResponse ReceberMensagemTexto(ReceberMensagemDeTextoRequest pRequest);

        ListarMensagensDeTextoResponse ListarMensagensDeTexto(ListarMensagensDeTextoRequest pRequest);

        SalvarMensagemDeTextoResponse SalvarMensagemDeTexto(SalvarMensagemDeTextoRequest pRequest);
    }
}
