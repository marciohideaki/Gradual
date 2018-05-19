using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Coleção baseada em RelacaoValorValorInfo que mantem dois indices, uma pela chave 1 e 
    /// outro pela chave 2, permitindo que os itens sejam acessados tanto pela chave 1
    /// quanto pela 2.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class RelacaoValorValorHelper<T, U>
    {
        /// <summary>
        /// Dicionario para traduzir 1 em 2
        /// </summary>
        public Dictionary<T, RelacaoValorValorInfo<T, U>> DicionarioDe1Para2 { get; set; }

        /// <summary>
        /// Dicionario para traduzir 2 em 1
        /// </summary>
        public Dictionary<U, RelacaoValorValorInfo<T, U>> DicionarioDe2Para1 { get; set; }

        /// <summary>
        /// Construtor. Inicializa as coleções
        /// </summary>
        public RelacaoValorValorHelper()
        {
            this.DicionarioDe1Para2 = new Dictionary<T, RelacaoValorValorInfo<T, U>>();
            this.DicionarioDe2Para1 = new Dictionary<U, RelacaoValorValorInfo<T, U>>();
        }

        /// <summary>
        /// Adiciona um novo elemento nas duas coleções
        /// </summary>
        /// <param name="elementoExterno"></param>
        /// <param name="elementoInterno"></param>
        public void Adicionar(T valor1, U valor2)
        {
            RelacaoValorValorInfo<T, U> elemento = new RelacaoValorValorInfo<T, U>();
            elemento.Valor1 = valor1;
            elemento.Valor2 = valor2;

            this.DicionarioDe1Para2.Add(elemento.Valor1, elemento);
            this.DicionarioDe2Para1.Add(elemento.Valor2, elemento);
        }

        /// <summary>
        /// Remove um elemento pelo valor1
        /// </summary>
        /// <param name="elementoExterno"></param>
        public void RemoverPorValor1(T valor1)
        {
            RelacaoValorValorInfo<T, U> elemento = this.DicionarioDe1Para2[valor1];
            this.DicionarioDe1Para2.Remove(elemento.Valor1);
            this.DicionarioDe2Para1.Remove(elemento.Valor2);
        }

        /// <summary>
        /// Remove um elemento pelo valor2
        /// </summary>
        /// <param name="elementoInterno"></param>
        public void RemoverPorValor2(U valor2)
        {
            RelacaoValorValorInfo<T, U> elemento = this.DicionarioDe2Para1[valor2];
            this.DicionarioDe1Para2.Remove(elemento.Valor1);
            this.DicionarioDe2Para1.Remove(elemento.Valor2);
        }
    }
}
