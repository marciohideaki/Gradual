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
    /// Classe de auxilio 
    /// </summary>
    public class ListaPermissoesAssociadasHelper
    {
        /// <summary>
        /// Lista das permissoes por tipo
        /// </summary>
        public Dictionary<Type, PermissaoBase> ListaPorTipo { get; set; }

        /// <summary>
        /// Lista das permissoes por código
        /// </summary>
        public Dictionary<string, PermissaoBase> ListaPorCodigo { get; set; }

        /// <summary>
        /// Lista de permissoes negadas
        /// </summary>
        public List<Type> PermissoesNegadas { get; set; }

        /// <summary>
        /// Indica se esta lista está representando a lista do administrador
        /// </summary>
        public bool EhAdministrador { get; set; }

        /// <summary>
        /// Contem a lista de permissoes para que as traduções possam ser realizadas
        /// </summary>
        public ListaPermissoesHelper ListaPermissoes { get; set; }
        
        /// <summary>
        /// Construtor default
        /// </summary>
        public ListaPermissoesAssociadasHelper()
        {
            this.ListaPorCodigo = new Dictionary<string, PermissaoBase>();
            this.ListaPorTipo = new Dictionary<Type, PermissaoBase>();
            this.PermissoesNegadas = new List<Type>();
            
            // Recebe a lista de permissoes
            IServicoSeguranca servicoSeguranca = Ativador.Get<IServicoSeguranca>();
            this.ListaPermissoes = 
                new ListaPermissoesHelper(
                    servicoSeguranca.ListarPermissoes(
                        new ListarPermissoesRequest()).Permissoes);
        }
        
        /// <summary>
        /// Adiciona a lista informada
        /// </summary>
        public void AdicionarPermissoes(List<PermissaoAssociadaInfo> permissoesAssociadas)
        {
            // Varre as permissoes a serem adicionadas aplicando a regra de associacao
            foreach (PermissaoAssociadaInfo permissaoAssociada in permissoesAssociadas)
                this.AdicionarPermissao(permissaoAssociada);
        }

        public void AdicionarPermissao(PermissaoAssociadaInfo permissaoAssociada)
        {
            // Acha a permissao base e o tipo
            PermissaoBase permissao = this.ListaPermissoes.ListaPorCodigo[permissaoAssociada.CodigoPermissao];
            Type tipoPermissao = permissao.GetType();
            
            // Se já existe e é negação, remove
            bool contemPermissao = this.ListaPorTipo.ContainsKey(tipoPermissao);
            if (contemPermissao && permissaoAssociada.Status == PermissaoAssociadaStatusEnum.Negado)
            {
                this.ListaPorTipo.Remove(tipoPermissao);
            }
            // Se não existe e é permissão, insere
            else if (!contemPermissao && permissaoAssociada.Status == PermissaoAssociadaStatusEnum.Permitido && !this.PermissoesNegadas.Contains(tipoPermissao))
            {
                this.ListaPorTipo.Add(tipoPermissao, permissao);
                this.ListaPorCodigo.Add(permissao.PermissaoInfo.CodigoPermissao, permissao);
            }

            // Se for permissao negada, tem que contar na lista de negação
            if (permissaoAssociada.Status == PermissaoAssociadaStatusEnum.Negado && !this.PermissoesNegadas.Contains(tipoPermissao))
                this.PermissoesNegadas.Add(tipoPermissao);
        }

        /// <summary>
        /// Consulta se o usuário tem a pemissão informada válida
        /// Overload que consulta por tipo da permissao
        /// </summary>
        /// <param name="sessao"></param>
        /// <returns></returns>
        public bool ConsultarPermissao(Type tipoPermissao)
        {
            if (this.EhAdministrador)
                return true;
            else if (this.ListaPorTipo.ContainsKey(tipoPermissao))
                return this.ListaPorTipo[tipoPermissao].ValidarPermissao();
            else
                return false;
        }

        /// <summary>
        /// Consulta se o usuário tem a pemissão informada válida.
        /// Overload que consulta por código da permissao
        /// ** Precisa criar os overloads recebendo sessao para repassar 
        /// ** para o método ValidarPermissao
        /// </summary>
        /// <param name="sessao"></param>
        /// <returns></returns>
        public bool ConsultarPermissao(string codigoPermissao)
        {
            if (this.ListaPorTipo.ContainsKey(typeof(PermissaoAdministrador)))
                return true;
            else if (this.ListaPorCodigo.ContainsKey(codigoPermissao))
                return this.ListaPorCodigo[codigoPermissao].ValidarPermissao();
            else
                return false;
        }
    }
}
