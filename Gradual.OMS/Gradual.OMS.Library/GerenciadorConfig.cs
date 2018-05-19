using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Fornece as configurações referentes ao objeto e id solicitados.
    /// </summary>
    public static class GerenciadorConfig
    {
        public static T ReceberConfig<T>()
        {
            return (T)ConfigurationManager.GetSection(typeof(T).FullName);
        }

        public static object ReceberConfig(Type tipo)
        {
            return ConfigurationManager.GetSection(tipo.FullName);
        }

        public static T ReceberConfig<T>(string id)
        {
            if (id != null)
                return (T)ConfigurationManager.GetSection(typeof(T).FullName + "-" + id);
            else
                return ReceberConfig<T>();
        }

        public static object ReceberConfig(Type tipo, string id)
        {
            if (id != null)
                return ConfigurationManager.GetSection(tipo.FullName + "-" + id);
            else
                return ReceberConfig(tipo);
        }
    }
}
