using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe de auxilio para manter a lista de permissões
    /// </summary>
    public class ListaPermissoesHelper
    {
        /// <summary>
        /// Lista de permissoes por código da permissao
        /// </summary>
        public Dictionary<string, PermissaoBase> ListaPorCodigo { get; set; }

        /// <summary>
        /// Lista de permissoes por tipo da permissao
        /// </summary>
        public Dictionary<Type, PermissaoBase> ListaPorTipo { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListaPermissoesHelper()
        {
            this.ListaPorCodigo = new Dictionary<string, PermissaoBase>();
            this.ListaPorTipo = new Dictionary<Type, PermissaoBase>();
        }

        /// <summary>
        /// Adiciona uma lista de permissoes na colecao
        /// </summary>
        /// <param name="permissao"></param>
        public void AdicionarPermissoes(List<PermissaoBase> permissoes)
        {
            // Varre a lista adicionando
            foreach (PermissaoBase permissao in permissoes)
                this.AdicionarPermissao(permissao);
        }

        /// <summary>
        /// Adiciona uma nova permissao nas coleções
        /// </summary>
        /// <param name="permissao"></param>
        public void AdicionarPermissao(PermissaoBase permissao)
        {
            Type tipoPermissao = permissao.GetType();

            this.ListaPorCodigo.Add(permissao.PermissaoInfo.CodigoPermissao, permissao);
            this.ListaPorTipo.Add(tipoPermissao, permissao);
        }

        /// <summary>
        /// Carrega as pemissoes encontradas nos namespaces informados.
        /// A string do namespace deve estar no formato namespace, assembly
        /// </summary>
        /// <param name="namespaces"></param>
        public void CarregarPermissoes(List<string> namespaces)
        {
            // Varre a lista
            foreach (string ns1 in namespaces)
            {
                // Separa o nome do namespace e o nome do assembly
                string[] ns2 = ns1.Split(',');
                string ns = ns2[0].Trim();
                string nomeAssembly = ns2[1].Trim();

                // Pega referencia ao assembly
                Assembly assembly = Assembly.Load(nomeAssembly);

                // Faz a varredura em cima dos tipos adicionando os que fazem parte do namespace informado
                foreach (Type tipo in assembly.GetTypes())
                    if (tipo.IsSubclassOf(typeof(PermissaoBase)) && tipo.Namespace.StartsWith(ns))
                        this.AdicionarPermissao(
                            (PermissaoBase)
                                Activator.CreateInstance(tipo));
            }
        }

        /// <summary>
        /// Construtor que recebe a lista de permissoes iniciais a serem carregadas
        /// </summary>
        /// <param name="permissoes"></param>
        public ListaPermissoesHelper(List<PermissaoBase> permissoes) : this()
        {
            this.AdicionarPermissoes(permissoes);
        }
    }
}
