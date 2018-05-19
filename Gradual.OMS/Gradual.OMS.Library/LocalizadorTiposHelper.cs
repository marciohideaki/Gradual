using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Classe de auxilio para o WCF.
    /// Contem o método que retorna os tipos possíveis para serem inclusos na lista de serialização, 
    /// de acordo com o tipo chamador
    /// </summary>
    [Serializable]
    public static class LocalizadorTiposHelper
    {
        /// <summary>
        /// Referencia para o objeto de configurações
        /// </summary>
        private static LocalizadorTiposConfig _config = GerenciadorConfig.ReceberConfig<LocalizadorTiposConfig>();

        /// <summary>
        /// Variável local para fazer o cache dos tipos. Evita que a cada chamada seja recriada a lista
        /// </summary>
        private static Dictionary<Type, List<Type>> _cache = new Dictionary<Type, List<Type>>();

        /// <summary>
        /// Método para retornar a lista de tipos conhecidos. Retorna os tipos de todos as classes de 
        /// dados e de mensagens deste assembly.
        /// </summary>
        /// <returns></returns>
        public static List<Type> RetornarTipos(ICustomAttributeProvider provider)
        {
            // Inicializa
            List<Type> retorno = new List<Type>();
            
            // Pega o tipo chamador
            Type tipoChamador = (Type)provider;

            // Verifica se tem a entrada no cache
            if (!_cache.ContainsKey(tipoChamador))
            {
                // Cria item e adiciona no cache
                retorno = new List<Type>();
                _cache.Add(tipoChamador, retorno);

                // Acha o tipo na lista do config
                LocalizadorGrupoTipoInfo grupo = null;
                if (_config != null)
                    grupo = 
                        (from g in _config.Grupos
                         where g.TipoChamador == tipoChamador
                         select g).FirstOrDefault();

                // Achou?
                if (grupo != null)
                {
                    // Varre adicionando os itens
                    foreach (LocalizadorTipoInfo tipoInfo in grupo.Tipos)
                    {
                        // Adiciona assembly?
                        if (tipoInfo.IncluirAssembly != null)
                        {
                            // Pega o assembly
                            Assembly assembly = Assembly.Load(tipoInfo.IncluirAssembly);

                            // Varre os tipos adicionando
                            foreach (Type tipo in assembly.GetTypes())
                                if (!tipo.IsGenericType)
                                    retorno.Add(tipo);
                        }

                        // Adiciona namespace?
                        if (tipoInfo.IncluirNamespace != null)
                        {
                            // Pega o assembly
                            string[] ns = tipoInfo.IncluirNamespace.Split(',');
                            string ns0 = ns[0].Trim();
                            Assembly assembly = Assembly.Load(ns[1].Trim());

                            // Faz o filtro e inclui
                            retorno.AddRange(
                                from t in assembly.GetTypes()
                                where (t.Namespace == ns0 || 
                                       (tipoInfo.AprofundarNamespace && t.Namespace.StartsWith(ns0))) 
                                       && !t.IsGenericType
                                select t);
                        }

                        // Adiciona tipo?
                        if (tipoInfo.IncluirTipo != null)
                        {
                            // Pega o tipo
                            Type tipo = Type.GetType(tipoInfo.IncluirTipo);

                            // Inclui
                            retorno.Add(tipo);
                        }
                    }
                }
            }
            else
            {
                // Pega do cache
                retorno = _cache[tipoChamador];
            }

            // Retorna
            return retorno;
        }
    }
}
