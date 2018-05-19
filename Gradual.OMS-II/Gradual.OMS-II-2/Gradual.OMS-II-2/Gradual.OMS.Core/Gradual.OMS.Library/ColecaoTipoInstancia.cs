using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Esta classe tem como finalidade prover funções para armazenamento de objetos, que podem
    /// ser acessados pelo seu tipo. Ou seja, assume que só poderá existir um objeto de cada tipo.
    /// Mesmo conceito de classes de serviços, mas com outro nome.
    /// Utilizado para, por exemplo, permitir que vários objetos de configuração existam ao mesmo
    /// tempo em um mesmo contexto.
    /// O nome é uma referencia à coleção de tipos e instancias.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ColecaoTipoInstancia
    {
        /// <summary>
        /// Dicionario com lista de elementos. A chave é o tipo do elemento
        /// </summary>
        public List<object> Colecao { get; set; }

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ColecaoTipoInstancia()
        {
            this.Colecao = new List<object>();
        }

        /// <summary>
        /// Construtor que permite já adicionar um item
        /// </summary>
        /// <param name="item"></param>
        public ColecaoTipoInstancia(object item)
        {
            this.AdicionarItem(item);
        }

        /// <summary>
        /// Construtor que permite adicionar diversos itens
        /// </summary>
        /// <param name="itens"></param>
        public ColecaoTipoInstancia(object[] itens)
        {
            foreach (object item in itens)
                this.AdicionarItem(item);
        }

        #endregion

        /// <summary>
        /// Adiciona novo item na colecao
        /// </summary>
        /// <param name="item"></param>
        public T AdicionarItem<T>(T item)
        {
            T itemExistente = this.ReceberItem<T>();
            if (itemExistente != null)
                this.RemoverItem<T>();

            this.Colecao.Add(item);

            return item;
        }

        /// <summary>
        /// Recebe um item da colecao
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReceberItem<T>()
        {
            object item =
                (from i in this.Colecao
                 where i != null && i.GetType() == typeof(T)
                 select i).FirstOrDefault();

            // Retorna
            return (T)item;
        }

        /// <summary>
        /// Remove um item da coleção
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoverItem<T>()
        {
            this.Colecao.Remove(this.ReceberItem<T>());
        }
    }
}
