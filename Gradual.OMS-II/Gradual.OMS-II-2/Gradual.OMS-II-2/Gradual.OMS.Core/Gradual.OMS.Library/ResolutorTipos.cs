using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Resolve tipos com nomes parciais.
    /// Pode-se, por exemplo, informar a string Log e ter o tipo resolvido para Gradual.OMS.Library.Log
    /// </summary>
    public static class ResolutorTipos
    {
        /// <summary>
        /// Configurações do Resolutor
        /// </summary>
        private static ResolutorTiposConfig _config = GerenciadorConfig.ReceberConfig<ResolutorTiposConfig>();

        /// <summary>
        /// Lista de tipos conhecidos
        /// </summary>
        private static List<Type> _tiposConhecidos = null;

        /// <summary>
        /// Tenta fazer a resolução do tipo
        /// </summary>
        /// <param name="nomeParcial"></param>
        /// <returns></returns>
        public static Type Resolver(string nomeParcial)
        {
            // Deve inicializar a lista?
            if (_tiposConhecidos == null)
                inicializarListaTipos();

            // Varre a lista tentando resolver
            foreach (Type tipo in _tiposConhecidos)
                if (tipo.Name == nomeParcial)
                    return tipo;

            // Retorna
            return null;
        }

        /// <summary>
        /// Inicializa a lista com os tipos conhecidos
        /// </summary>
        private static void inicializarListaTipos()
        {
            // Cria a lista 
            _tiposConhecidos = new List<Type>();
            
            // Adiciona namespaces
            foreach (string nsx in _config.IncluirNamespaces)
            {
                // Pega o assembly
                string[] ns = nsx.Split(',');
                string ns0 = ns[0].Trim();
                Assembly assembly = Assembly.Load(ns[1].Trim());

                // Faz o filtro e inclui
                _tiposConhecidos.AddRange(
                    from t in assembly.GetTypes()
                    where (t.Namespace == ns0 ||
                           (_config.AprofundarNamespaces && t.Namespace.StartsWith(ns0)))
                           && !t.IsGenericType
                    select t);
            }

            // Adiciona tipos
            foreach (string tipoStr in _config.IncluirTipos)
            {
                // Pega o tipo
                Type tipo = Type.GetType(tipoStr);

                // Inclui
                _tiposConhecidos.Add(tipo);
            }

            // Exclui tipos
            foreach (string tipoStr in _config.ExcluirTipos)
            {
                // Pega o tipo
                Type tipo = Type.GetType(tipoStr);

                // Exclui
                if (_tiposConhecidos.Contains(tipo))
                    _tiposConhecidos.Remove(tipo);
            }
        }
    }
}
