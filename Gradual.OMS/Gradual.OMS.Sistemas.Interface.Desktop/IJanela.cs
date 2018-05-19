using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    public interface IJanela
    {
        void Inicializar(Janela janela);
        void AdicionarControle(Controle controle);
        void AdicionarComando(Comando comando);
        void MostrarJanela();
        void EsconderJanela();
        void MostrarConfiguracoes();
        void SetarTitulo(string titulo);
        object SalvarParametros(EventoManipulacaoParametrosEnum evento);
        void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento);
        void Fechar();
        void Ativar();
        MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros);
    }
}
