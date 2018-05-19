using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Representa uma árvore de decisão
    /// </summary>
    /// <typeparam name="T">Indica o tipo da chave</typeparam>
    /// <typeparam name="U">Indica o tipo do item</typeparam>
    public class Arvore<T, U>
    {
        /// <summary>
        /// Lista dos comparadores para cada nível da árvore
        /// </summary>
        public List<ArvoreComparadorEnum> Comparadores { get; set; }
        
        /// <summary>
        /// Evento para inicialização de um novo nó da árvore
        /// </summary>
        public event EventHandler<ArvoreNoInicializarEventArgs<T, U>> InicializarNo;
        
        /// <summary>
        /// Representa o primeiro nó da árvore
        /// </summary>
        public ArvoreNo<T, U> PrimeiroNo { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public Arvore(ArvoreComparadorEnum[] comparadores)
        {
            // Inicializa
            this.Comparadores = new List<ArvoreComparadorEnum>();
            if (comparadores.Length > 0)
                this.Comparadores.AddRange(comparadores);

            // Cria o primeiro nó
            this.PrimeiroNo = new ArvoreNo<T, U>();
        }

        /// <summary>
        /// Insere um elemento na árvore
        /// </summary>
        /// <param name="chave"></param>
        /// <param name="item"></param>
        public void InserirItem(T[] chave, U item)
        {
            // Cria ou recebe o nó para acomodar o item
            ArvoreNo<T, U> no = this.CriarNo(chave);

            // Atribui o item
            no.Item = item;
        }

        /// <summary>
        /// Retorna o item solicitado, caso exista
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public U ReceberItem(T[] chave)
        {
            // Acha o no da chave informada
            ArvoreNo<T, U> no = this.ReceberNo(chave);

            // Retorna
            if (no != null)
                return no.Item;
            else
                return default(U);
        }

        /// <summary>
        /// Retorna a lista de itens da chave informada.
        /// Considera o caminho da chave e os caminhos defaults. 
        /// Por isso o retorno é uma coleção.
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public List<U> ReceberItens(T[] chave, ArvoreNuloTipoEnum nuloTipo)
        {
            // Recebe os nós
            List<ArvoreNo<T, U>> nos = this.ReceberNos(chave, nuloTipo);

            // Transforma em uma lista de itens
            List<U> retorno =
                (from i in nos
                 select i.Item).ToList();

            // Retorna
            return retorno;
        }

        /// <summary>
        /// Retorna a lista de itens da chave informada.
        /// Overload que considera ArvoreNuloTipoEnum.TratarNuloComoDefault como default
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public List<U> ReceberItens(T[] chave)
        {
            // Retorna
            return this.ReceberItens(chave, ArvoreNuloTipoEnum.TratarNuloComoDefault);
        }

        /// <summary>
        /// Remove o nó solicitado e todos os seus filhos
        /// </summary>
        /// <param name="chave"></param>
        public void RemoverNo(T[] chave)
        {
            // Acha o nó pai
            ArvoreNo<T, U> noPai = this.ReceberNo(chave.Take(chave.Length - 1).ToArray());

            // Achou o pai?
            if (noPai != null)
            {
                // Pega a chave 
                T chaveNo = chave.Last();

                // Remove por nulo ou pelo valor
                if (chaveNo == null && noPai.Default != null)
                    noPai.Default = null;
                else if (chaveNo != null && noPai.Filhos.ContainsKey(chaveNo))
                    noPai.Filhos.Remove(chaveNo);
            }
        }

        /// <summary>
        /// Navega na árvore e retorna o nó indicado.
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public ArvoreNo<T, U> ReceberNo(T[] chave)
        {
            // Navega para achar o no desejado
            return receberNo(this.PrimeiroNo, chave);
        }

        /// <summary>
        /// Navega na árvore retornando a lista de nós solicitada.
        /// Navega pelas chaves informadas e pelos defaults.
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public List<ArvoreNo<T, U>> ReceberNos(T[] chave, ArvoreNuloTipoEnum nuloTipo)
        {
            // Navega para achar a lista de nos desejados
            return receberNosComDefault(this.PrimeiroNo, chave, nuloTipo);
        }

        /// <summary>
        /// Overload que assume ArvoreNuloTipoEnum.TratarNuloComoDefault como default
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public List<ArvoreNo<T, U>> ReceberNos(T[] chave)
        {
            // Navega para achar a lista de nos desejados
            return receberNosComDefault(this.PrimeiroNo, chave, ArvoreNuloTipoEnum.TratarNuloComoDefault);
        }

        /// <summary>
        /// Método de pesquisa de nós usado na recursividade.
        /// </summary>
        /// <param name="noBase"></param>
        /// <param name="chave"></param>
        /// <returns></returns>
        private ArvoreNo<T, U> receberNo(ArvoreNo<T, U> noBase, T[] chave)
        {
            // Se a chave for nula, é este o elemento desejado
            if (chave.Length == 0)
            {
                return noBase;
            }
            // Verifica se o item da chave consta na colecao, para chaves nao nulas
            else if (chave[0] != null && noBase.Filhos.ContainsKey(chave[0]))
            {
                return receberNo(noBase.Filhos[chave[0]], chave.Skip(1).ToArray());
            }
            // Se a chave for nula, vê se tem o item default
            else if (chave[0] == null && noBase.Default != null)
            {
                return receberNo(noBase.Default, chave.Skip(1).ToArray());
            }
            // Nao achou o caminho para o elemento
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Método de pesquisa de nós usado na recursividade.
        /// Considera a chave informada e os caminhos defaults
        /// </summary>
        /// <param name="noBase"></param>
        /// <param name="chave"></param>
        /// <returns></returns>
        private List<ArvoreNo<T, U>> receberNosComDefault(ArvoreNo<T, U> noBase, T[] chave, ArvoreNuloTipoEnum nuloTipo)
        {
            // Retorno
            List<ArvoreNo<T, U>> retorno = new List<ArvoreNo<T, U>>();

            // Se a chave não tem mais elementos, é este o elemento desejado
            if (chave.Length == 0)
            {
                retorno.Add(noBase);
            }
            // Verifica se o item da chave consta na colecao. 
            // Utiliza os dois caminhos: o da chave e o default, caso existam
            else 
            {
                // Se a chave não é nula, adiciona pelo caminho da chave
                if (chave[0] != null)
                {
                    // Acha o comparador
                    ArvoreComparadorEnum comparador = ArvoreComparadorEnum.Igual;
                    if (this.Comparadores.Count >= noBase.Nivel + 1)
                        comparador = this.Comparadores[noBase.Nivel];

                    // Adiciona o caminho do filho correspondente à chave
                    if (comparador == ArvoreComparadorEnum.Igual && noBase.Filhos.ContainsKey(chave[0]))
                    {
                        // Adiciona o item exato
                        retorno.AddRange(
                            receberNosComDefault(
                                noBase.Filhos[chave[0]], chave.Skip(1).ToArray(), nuloTipo));
                    }
                    else if (comparador == ArvoreComparadorEnum.ComecaCom)
                    {
                        // Varre os nós filhos adicionando todos que iniciam com a string
                        foreach (KeyValuePair<T, ArvoreNo<T, U>> item in noBase.Filhos)
                            if (chave[0].ToString().StartsWith(item.Value.Chave.ToString()))
                                retorno.AddRange(
                                    receberNosComDefault(item.Value, chave.Skip(1).ToArray(), nuloTipo));
                    }
                }
                else if (nuloTipo == ArvoreNuloTipoEnum.TratarNuloComoTodos)
                {
                    // Adiciona os caminhos dos filhos
                    foreach (KeyValuePair<T, ArvoreNo<T, U>> item in noBase.Filhos)
                        retorno.AddRange(
                            receberNosComDefault(
                                item.Value, chave.Skip(1).ToArray(), nuloTipo));
                }

                // Adiciona pelo caminho default
                if (noBase.Default != null)
                    retorno.AddRange(
                        receberNosComDefault(
                            noBase.Default, chave.Skip(1).ToArray(), nuloTipo));
            }

            // Retorna
            return retorno;
        }

        /// <summary>
        /// Cria o nó para representar a chave informada.
        /// Se o nó já estiver criado, retorna o nó correspondente
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public ArvoreNo<T, U> CriarNo(T[] chave)
        {
            // Se a chave for vazia, o nó é o primeiro nó
            if (chave == null)
                return this.PrimeiroNo;

            // Acha o nó imediatamente anterior
            T[] chaveAnterior = chave.Take(chave.Length - 1).ToArray();
            ArvoreNo<T, U> noAnterior = this.ReceberNo(chaveAnterior);

            // Se o nó anterior não existir, cria o nó anterior
            if (noAnterior == null)
                noAnterior = this.CriarNo(chaveAnterior);

            // Recebe a chave
            T chaveNo = chave.Last();

            // Referencia para o futuro nó, existente ou novo
            ArvoreNo<T, U> no = null;
            
            // Verifica se o nó já existe ou se precisa criar novo
            bool existente = 
                chaveNo == null ? noAnterior.Default != null : noAnterior.Filhos.ContainsKey(chaveNo);
            if (!existente)
            {
                // Cria o novo nó
                no =
                    new ArvoreNo<T, U>()
                    {
                        Pai = noAnterior,
                        Chave = chaveNo
                    };

                // Pede inicialização do nó
                if (this.InicializarNo != null)
                    this.InicializarNo(
                        this, 
                        new ArvoreNoInicializarEventArgs<T, U>() 
                        { 
                            ArvoreNo = no 
                        });

                // Adiciona na coleção de filhos do nó anterior
                if (chaveNo != null)
                    noAnterior.Filhos.Add(chave.Last(), no);
                else
                    noAnterior.Default = no;
            }
            else
            {
                // Pega o nó atual
                if (chaveNo != null)
                    no = noAnterior.Filhos[chaveNo];
                else
                    no = noAnterior.Default;
            }

            // Retorna
            return no;
        }
    }
}
