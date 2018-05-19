using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum.Permissoes;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe que contem informações e funções relativas ao usuário
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Informações do usuário
        /// </summary>
        public UsuarioInfo UsuarioInfo { get; set; }

        /// <summary>
        /// Lista de grupos a que o usuário pertence.
        /// A chave da lista é o código do usuario grupo
        /// </summary>
        public Dictionary<string, UsuarioGrupoInfo> UsuariosGrupo { get; set; }

        /// <summary>
        /// Lista de perfis que o usuário possui.
        /// A chave da lista é o código do perfil
        /// </summary>
        public Dictionary<string, PerfilInfo> Perfis { get; set; }

        /// <summary>
        /// Lista de permissões associadas ao usuario direta ou indiretamente
        /// </summary>
        public ListaPermissoesAssociadasHelper PermissoesAssociadas { get; set; }

        /// <summary>
        /// Construtor recebe as informações do usuario
        /// </summary>
        public Usuario(UsuarioInfo usuarioInfo)
        {
            // Inicializa colecoes
            this.PermissoesAssociadas = new ListaPermissoesAssociadasHelper();
            this.Perfis = new Dictionary<string, PerfilInfo>();
            this.UsuariosGrupo = new Dictionary<string, UsuarioGrupoInfo>();
            
            // Inicializa
            this.UsuarioInfo = usuarioInfo;

            // Pede para carregar as informações do usuário
            this.AtualizarInformacoes();
        }

        /// <summary>
        /// Carrega os grupos, perfis e permissoes associadas a todos esses elementos
        /// </summary>
        public void AtualizarInformacoes()
        {
            // Referencia para o servico de persistencia
            IServicoSegurancaPersistencia segurancaPersistencia = Ativador.Get<IServicoSegurancaPersistencia>();

            // Limpa as listas
            this.Perfis.Clear();
            this.PermissoesAssociadas.ListaPorTipo.Clear();
            this.UsuariosGrupo.Clear();

            // Carrega lista de grupos
            foreach (string codigoUsuarioGrupo in this.UsuarioInfo.Grupos)
            {
                // Recebe
                UsuarioGrupoInfo usuarioGrupo =
                    segurancaPersistencia.ReceberUsuarioGrupo(
                        new ReceberUsuarioGrupoRequest()
                        {
                            CodigoUsuarioGrupo = codigoUsuarioGrupo
                        }).UsuarioGrupo;

                // Adiciona na colecao
                this.UsuariosGrupo.Add(usuarioGrupo.CodigoUsuarioGrupo, usuarioGrupo);

                // Adiciona as pemissoes do usuario grupo
                this.PermissoesAssociadas.AdicionarPermissoes(usuarioGrupo.Permissoes);
            }

            // Carrega lista de perfis
            foreach (string codigoPerfil in this.UsuarioInfo.Perfis)
            {
                // Recebe
                PerfilInfo perfil =
                    segurancaPersistencia.ReceberPerfil(
                        new ReceberPerfilRequest()
                        {
                            CodigoPerfil = codigoPerfil
                        }).Perfil;

                // Adiciona na colecao
                this.Perfis.Add(perfil.CodigoPerfil, perfil);

                // Adiciona as pemissoes do usuario grupo
                this.PermissoesAssociadas.AdicionarPermissoes(perfil.Permissoes);
            }

            // Carrega a lista de permissoes do usuario
            this.PermissoesAssociadas.AdicionarPermissoes(this.UsuarioInfo.Permissoes);

            // Verifica se é admin
            this.PermissoesAssociadas.EhAdministrador = 
                this.PermissoesAssociadas.ConsultarPermissao(
                    typeof(PermissaoAdministrador));
        }
    }   
}
