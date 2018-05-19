using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Classe estática que contém lista de hosts de serviços.
    /// </summary>
    public class ServicoHostColecao
    {
        private static List<ServicoHost> _hosts = new List<ServicoHost>();
        public static List<ServicoHost> Hosts
        {
            get { return _hosts; }
        }

        public static ServicoHost Default
        {
            get 
            {
                if (_hosts.Count == 0)
                    _hosts.Add(new ServicoHost());
                return _hosts[0];
            }
        }
    }
}
