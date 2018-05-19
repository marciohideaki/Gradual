using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;

namespace Gradual.OMS.Contratos.Interface.Desktop
{
    public interface IServicoInterfaceDesktopHost
    {
        void Inicializar(HostInfo hostInfo);
        void CriarJanela(JanelaInfo janelaInfo);
        void CriarJanela(JanelaInfo janelaInfo, object parametros);
        void RemoverJanela(string idJanela);
        void AdicionarControle(string idJanela, ControleInfo controleInfo, object parametros);
        void MostrarJanela(string idJanela);
        void EsconderJanela(string idJanela);
        void FecharJanela(string idJanela);
        void AtivarJanela(string idJanela);
        //void RegistrarJanela(Janela janela);
        JanelaSerializacaoInfo SerializarJanela(string idJanela);
        JanelaInfo DesserializarJanela(JanelaSerializacaoInfo parametros);
        void CarregarParametrosDeJanela(string idJanela, object parametros);
        object ReceberJanelaInstancia(string idJanela);
        List<ConsultaControlesHelper> ListarControles();
        MensagemInterfaceResponseBase EnviarMensagemParaControle(MensagemInterfaceRequestBase mensagem, string idControle);
    }
}
