using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Representa um nó da árvore.
    /// </summary>
    /// <typeparam name="T">Tipo da chave do item</typeparam>
    /// <typeparam name="U">Tipo do item</typeparam>
    public class ArvoreNo<T, U>
    {
        /// <summary>
        /// Nó default. Representa uma ausencia de valor para a chave considerada
        /// </summary>
        public ArvoreNo<T, U> Default { get; set; }

        /// <summary>
        /// Contem a lista de nós das chaves filhas
        /// </summary>
        public Dictionary<T, ArvoreNo<T, U>> Filhos { get; set; }

        /// <summary>
        /// Indica a chave deste nó
        /// </summary>
        public T Chave { get; set; }

        /// <summary>
        /// Indica o elemento que este nó carrega
        /// </summary>
        public U Item { get; set; }

        /// <summary>
        /// Indica o nó pai
        /// </summary>
        public ArvoreNo<T, U> Pai { get; set; }

        /// <summary>
        /// Retorna o nível do nó na árvore
        /// </summary>
        public int Nivel
        {
            get 
            {
                if (this.Pai == null)
                    return 0;
                else
                    return this.Pai.Nivel + 1;
            }
        }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ArvoreNo()
        {
            this.Filhos = new Dictionary<T, ArvoreNo<T, U>>();
        }

        /// <summary>
        /// Retorna o caminho do nó separado por ';'
        /// </summary>
        /// <returns></returns>
        public string ReceberCaminho()
        {
            return ReceberCaminho(";");
        }
        
        /// <summary>
        /// Retorna o caminho do nó separado pelo separador
        /// </summary>
        /// <param name="separador"></param>
        /// <returns></returns>
        public string ReceberCaminho(string separador)
        {
            // Se nao tem pai, o caminho é vazio, pois é o primeiro nó
            if (Pai == null)
                return "";

            // Pega o caminho do pai
            string caminho = this.Pai.ReceberCaminho(separador);
            if (caminho != "")
                caminho += separador;

            // Adiciona este nó
            if (this.Chave == null)
                caminho += "{null}";
            else
                caminho += this.Chave.ToString();

            // Retorna
            return caminho;
        }
    }
}
