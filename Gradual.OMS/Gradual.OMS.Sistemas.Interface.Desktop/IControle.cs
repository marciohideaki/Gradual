using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    public interface IControle
    {
        void Inicializar(Controle controle);
        object SalvarParametros(EventoManipulacaoParametrosEnum evento);
        void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento);
        MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros);
    }
}
