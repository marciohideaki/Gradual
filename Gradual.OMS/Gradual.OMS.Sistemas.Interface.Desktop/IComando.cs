using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    public interface IComando 
    {
        void Inicializar(Comando comando);
        void Executar();
        MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros);
    }
}
