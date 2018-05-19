using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Gradual.OMS.ConsolidadorRelatorioCC.Dados.Shared
{
    public static class ColecaoObjetos
    {
        /// <summary>
        /// Objeto responsavel por armezar o saldo em conta corrente de todos os clientes
        /// </summary>
        private static Hashtable htSaldoContaCorrente = new Hashtable();

        /// <summary>
        /// Representa a data e a hora da última atualização.
        /// </summary>
        public static DateTime DataHoraUltimaAtualizacao
        {
            get;
            set;
        }

        /// <summary>
        /// Adiciona um novo item na Hash
        /// </summary>
        /// <param name="chave"></param>
        /// <param name="valor"></param>
        public static void AdicionarItem(object chave, object valor)
        {
            lock (htSaldoContaCorrente)
            {
                htSaldoContaCorrente.Add(chave, valor);
            }
        }

        public static bool ContemItem(object key)
        {
            lock (htSaldoContaCorrente)
            {
                return htSaldoContaCorrente.ContainsKey(key);
            }
        }

        /// <summary>
        /// Pesquisa um determinado elemento da Hash
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object RetornarItem(object key)
        {
            lock (htSaldoContaCorrente)
            {
                return htSaldoContaCorrente[key];
            }
        }

        /// <summary>
        /// Remove um determinado item da Hash
        /// </summary>
        /// <param name="key"></param>
        public static void Remover(object key)
        {
            lock(htSaldoContaCorrente){
                htSaldoContaCorrente.Remove(key);
            }
        }

        /// <summary>
        /// Limpa todos os items da Hash.
        /// </summary>
        public static void Remover()
        {
            lock (htSaldoContaCorrente)
            {
                htSaldoContaCorrente = new Hashtable();
                htSaldoContaCorrente.Clear();
            }
        }

    }
}
