using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Orbite.RV.Contratos.MarketData
{
    /// <summary>
    /// Classe de auxilio para o WCF.
    /// Contem o método que retorna os tipos possíveis para serem inclusos na lista de serialização.
    /// </summary>
    [Serializable]
    public static class MarketDataTiposHelper
    {
        /// <summary>
        /// Variável local para fazer o cache dos tipos. Evita que a cada chamada seja recriada a lista
        /// </summary>
        private static List<Type> _cache = null;

        /// <summary>
        /// Método para retornar a lista de tipos conhecidos. Retorna os tipos de todos as classes de 
        /// dados e de mensagens deste assembly.
        /// </summary>
        /// <returns></returns>
        public static List<Type> RetornarTipos(ICustomAttributeProvider provider)
        {
            // Se ainda nao tem o cache, cria um com todos os tipos deste assembly
            if (_cache == null)
            {
                // Inicializa
                _cache = new List<Type>();

                // Pega exemplo de dados e de mensagens
                Type tipoDados = typeof(Orbite.RV.Contratos.MarketData.Dados.AtivoAcaoInfo);
                Type tipoMensagens = typeof(Orbite.RV.Contratos.MarketData.Mensagens.AssinarBookRequest);

                // Adiciona todos os dados e mensagens de ordens
                _cache.AddRange(
                            from t in Assembly.GetExecutingAssembly().GetTypes()
                            where (t.Namespace == tipoDados.Namespace || t.Namespace == tipoMensagens.Namespace) && !t.IsGenericType
                            select t);
                _cache.Add(typeof(Dados.SerieElementosInfo<Dados.TickInfo>));
            }

            // Retorna
            return _cache;
        }
    }
}
