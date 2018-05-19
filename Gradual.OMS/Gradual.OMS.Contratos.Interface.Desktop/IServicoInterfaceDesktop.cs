using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Interface.Desktop
{
    /// <summary>
    /// Interface para o serviço de Interfaces visuais.
    /// Contém funções para criação de janelas, com conceito de múltiplos desktops, serialização, entre outros.
    /// </summary>
    public interface IServicoInterfaceDesktop
    {
        /// <summary>
        /// Lista de desktops da interface
        /// </summary>
        List<DesktopInfo> Desktops { get; set; }

        /// <summary>
        /// Lista de janelas abertas na interface.
        /// Não mantém a instância, apenas o Info, para permitir que as janelas estejam em 
        /// processos ou appDomais separados.
        /// </summary>
        List<JanelaInfo> Janelas { get; set; }

        /// <summary>
        /// Info do launcher. É a janela especial do sistema que tem funções de ativação, desativação
        /// de janelas, salvar o arquivo de configuração de janelas, visualizar os desktops, etc.
        /// </summary>
        JanelaInfo Launcher { get; set; }

        /// <summary>
        /// Indica qual o desktop está ativo no momento.
        /// Na criação de uma nova janela, se o desktop não for informado será utilizado o desktop ativo.
        /// </summary>
        DesktopInfo DesktopAtivo { get; set; }

        /// <summary>
        /// Coleção com parâmetros default por tipo de controle.
        /// Contém nome do tipo do objeto, e os parametros do objeto.
        /// </summary>
        Dictionary<string, object> ParametrosDefault { get; set; }

        /// <summary>
        /// Variável genérica para ser utilizada pela aplicação cliente.
        /// </summary>
        ColecaoTipoInstancia Contexto { get; set; }

        /// <summary>
        /// Método para iniciar a janela de launcher.
        /// Utiliza arquivo de configuração para saber qual a instancia da janela a ser criada
        /// </summary>
        void IniciarLauncher();

        /// <summary>
        /// Cria uma nova janela de acordo com as características passadas no janelaInfo
        /// </summary>
        /// <param name="janelaInfo"></param>
        void CriarJanela(JanelaInfo janelaInfo);
        void CriarJanela(JanelaInfo janelaInfo, object parametros);

        /// <summary>
        /// Remove uma janela existente do estado interno da interface
        /// </summary>
        /// <param name="idJanela"></param>
        void RemoverJanela(string idJanela);

        /// <summary>
        /// Fecha uma janela existente.
        /// O processo de fechamento finaliza o elemento visual e depois pede a remoção da janela através do RemoverJanela
        /// </summary>
        /// <param name="idJanela"></param>
        void FecharJanela(string idJanela);

        /// <summary>
        /// Ativa a janela informada
        /// </summary>
        /// <param name="idJanela"></param>
        void AtivarJanela(string idJanela);

        /// <summary>
        /// Adiciona um controle em uma janela previamente criada.
        /// O controle possui as características informadas em controleInfo
        /// </summary>
        /// <param name="idJanela"></param>
        /// <param name="controleInfo"></param>
        void AdicionarControle(string idJanela, ControleInfo controleInfo);

        /// <summary>
        /// Mostra um desktop específico. Primeiramente esconde todas as janelas do desktop ativo e depois mostra as
        /// janelas do desktop solicitado.
        /// </summary>
        /// <param name="idDesktop"></param>
        void MostrarDesktop(string idDesktop);

        /// <summary>
        /// Esconde todas as janelas do desktop ativo
        /// </summary>
        void EsconderDesktopAtivo();

        /// <summary>
        /// Remove o desktop informado e todas as janelas associadas a ele
        /// </summary>
        /// <param name="idDesktop"></param>
        void RemoverDesktop(string idDesktop);

        /// <summary>
        /// Recebe a instância da janela launcher.
        /// Utilizado, por exemplo, na utilização de janelas tipo MDI, sendo o launcher o MDI parent.
        /// </summary>
        /// <returns></returns>
        object ReceberJanelaLauncher();

        /// <summary>
        /// Salva as configurações da interface para o arquivo informado no config
        /// </summary>
        void SalvarConfiguracoes();

        /// <summary>
        /// Salva as configurações da interface para o arquivo informado
        /// </summary>
        /// <param name="nomeArquivo"></param>
        void SalvarConfiguracoes(string nomeArquivo);

        /// <summary>
        /// Salva as configurações default para o arquivo informado no config
        /// </summary>
        void SalvarConfiguracoesDefault();

        /// <summary>
        /// Salva as configurações default para o arquivo informado
        /// </summary>
        /// <param name="nomeArquivo"></param>
        void SalvarConfiguracoesDefault(string nomeArquivo);

        /// <summary>
        /// Carrega as configurações da interface do arquivo informado no config
        /// </summary>
        void CarregarConfiguracoes();

        /// <summary>
        /// Carrega as configurações da interface do arquivo informado
        /// </summary>
        /// <param name="nomeArquivo"></param>
        void CarregarConfiguracoes(string nomeArquivo);

        /// <summary>
        /// Carrega as configurações default do arquivo informado no config
        /// </summary>
        void CarregarConfiguracoesDefault();

        /// <summary>
        /// Carrega as configurações default do arquivo informado
        /// </summary>
        /// <param name="nomeArquivo"></param>
        void CarregarConfiguracoesDefault(string nomeArquivo);

        /// <summary>
        /// Sinaliza abertura de nova janela. Irá disparar o evento EventoJanelaAbrindo
        /// </summary>
        /// <param name="janelaInfo"></param>
        void SinalizarJanelaAbrindo(JanelaInfo janelaInfo);

        /// <summary>
        /// Sinaliza fechamento de janela. Irá disparar o evento EventoJanelaFechando
        /// </summary>
        /// <param name="janelaInfo"></param>
        void SinalizarJanelaFechando(JanelaInfo janelaInfo);

        /// <summary>
        /// Evento que sinaliza janela abrindo
        /// </summary>
        event EventHandler<EventoJanelaEventArgs> EventoJanelaAbrindo;

        /// <summary>
        /// Evento que sinaliza janela fechando
        /// </summary>
        event EventHandler<EventoJanelaEventArgs> EventoJanelaFechando;

        /// <summary>
        /// Envia mensagem para o controle do tipo especificado.
        /// Caso o controle não seja encontrado, permite que uma janela
        /// seja criada com o controle para que a mensagem seja enviada
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="controleTipo">String no formato tipo, assembly</param>
        /// <param name="criarCasoNaoEncontrado"></param>
        /// <returns></returns>
        EnviarMensagemParaControleResponse EnviarMensagemParaControle(EnviarMensagemParaControleRequest parametros);
    }
}
