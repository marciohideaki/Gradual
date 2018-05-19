using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum.Permissoes;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados
{
    /// <summary>
    /// Classe de contexto do OMS para o gerenciador de interfaces.
    /// Provê diversas funções relativas às janelas abertas.
    /// Uma das funções é inicilizar e guardar a referencia para o serviço de ordens servidor e monitorar os eventos
    /// que o servidor manda transformando-os em listas mais significativas, como lista de execuções, de ordens, etc.
    /// </summary>
    public class InterfaceContextoOMS
    {
        /// <summary>
        /// Referencia ao servico de ordens servidor
        /// </summary>
        public IServicoOrdensServidor ServicoOrdensServidor { get; set; }

        ///// <summary>
        /// Informacoes da sessao aberta com o servico de ordens servidor
        /// </summary>
        public SessaoOrdensInfo SessaoOrdensInfo { get; set; }

        /// <summary>
        /// Informações da sessao aberta com o serviço de segurança
        /// </summary>
        public SessaoInfo SessaoInfo { get; set; }

        /// <summary>
        /// Objeto para recebimento de mensagens do servidor
        /// </summary>
        public CallbackEvento CallbackEvento { get; set; }

        /// <summary>
        /// Cache de grupos de usuários
        /// </summary>
        public Dictionary<string, UsuarioGrupoInfo> UsuarioGrupos { get; set; }

        /// <summary>
        /// Cache de perfis de usuários
        /// </summary>
        public Dictionary<string, PerfilInfo> Perfis { get; set; }

        /// <summary>
        /// Cache de perfis
        /// </summary>
        public Dictionary<string, PermissaoBase> Permissoes { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public InterfaceContextoOMS(SessaoInfo sessaoInfo)
        {
            // Inicializa
            this.SessaoInfo = sessaoInfo;

            //// Cria informações da sessão com o servidor de ordens
            this.SessaoOrdensInfo = new SessaoOrdensInfo() { CodigoSessao = Guid.NewGuid().ToString() };
            
            // Cria o callback para o serviço de ordens
            this.CallbackEvento = new CallbackEvento();

            // Registra o evento para tratamento das mensagens
            this.CallbackEvento.Evento += new EventHandler<EventoEventArgs>(CallbackEvento_Evento);
            
            // Faz a assinatura no servidor de ordens
            this.ServicoOrdensServidor = Ativador.Get<IServicoOrdensServidor>(this.CallbackEvento, this.SessaoOrdensInfo);

            // Carrega os caches
            this.CarregarPerfis();
            this.CarregarPermissoes();
            this.CarregarUsuarioGrupos();
        }

        /// <summary>
        /// Carrega cache de grupos de usuarios
        /// </summary>
        public void CarregarUsuarioGrupos()
        {
            // Referencia para servico de seguranca
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

            // Cache de grupos
            this.UsuarioGrupos = new Dictionary<string, UsuarioGrupoInfo>();
            ListarUsuarioGruposResponse usuarioGruposResponse =
                (ListarUsuarioGruposResponse)
                    servicoAutenticador.ProcessarMensagem(
                        new ListarUsuarioGruposRequest()
                        {
                            CodigoSessao = this.SessaoInfo.CodigoSessao
                        });
            
            // Carrega a lista
            this.CarregarUsuarioGrupos(usuarioGruposResponse.UsuarioGrupos);
        }

        /// <summary>
        /// Carrega cache de grupos de usuarios.
        /// Overload para carregar atraves de uma lista
        /// </summary>
        public void CarregarUsuarioGrupos(List<UsuarioGrupoInfo> usuarioGrupos)
        {
            // Limpa a lista atual
            this.UsuarioGrupos.Clear();
            
            // Varre a lista carregando
            foreach (UsuarioGrupoInfo usuarioGrupo in usuarioGrupos)
                this.UsuarioGrupos.Add(usuarioGrupo.CodigoUsuarioGrupo, usuarioGrupo);
        }

        /// <summary>
        /// Carrega cache de perfis
        /// </summary>
        public void CarregarPerfis()
        {
            // Referencia para servico de seguranca
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

            // Cache de perfis
            this.Perfis = new Dictionary<string, PerfilInfo>();
            ListarPerfisResponse perfisResponse =
                (ListarPerfisResponse)
                    servicoAutenticador.ProcessarMensagem(
                        new ListarPerfisRequest()
                        {
                            CodigoSessao = this.SessaoInfo.CodigoSessao
                        });
            foreach (PerfilInfo perfil in perfisResponse.Perfis)
                this.Perfis.Add(perfil.CodigoPerfil, perfil);
        }

        /// <summary>
        /// Carrega cache de permissoes
        /// </summary>
        public void CarregarPermissoes()
        {
            // Referencia para servico de seguranca
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

            // Cache de permissoes
            this.Permissoes = new Dictionary<string, PermissaoBase>();
            ListarPermissoesResponse permissoesResponse =
                (ListarPermissoesResponse)
                    servicoAutenticador.ProcessarMensagem(
                        new ListarPermissoesRequest()
                        {
                            CodigoSessao = this.SessaoInfo.CodigoSessao
                        });
            foreach (PermissaoBase permissao in permissoesResponse.Permissoes)
                this.Permissoes.Add(permissao.PermissaoInfo.CodigoPermissao, permissao);
        }

        /// <summary>
        /// Evento disparado quando o servidor envia alguma mensagem para o cliente.
        /// Neste caso, o servidor está enviando alguma mensagem relativa a esta sessão, normalmente um retorno de execução de ordem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallbackEvento_Evento(object sender, EventoEventArgs e)
        {
        }
    }
}
