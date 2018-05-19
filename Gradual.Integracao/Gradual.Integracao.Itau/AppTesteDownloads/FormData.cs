using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppTesteDownloads
{
    public class FormData
    {
        private List<KeyValuePair<string, string>> lista = new List<KeyValuePair<string, string>>();

        public void Add(string key, string value)
        {
            lista.Add(new KeyValuePair<string, string>(key, value));
        }

        public void Clear()
        {
            lista.Clear();
        }

        public List<KeyValuePair<string, string>> GetAll()
        {
            return lista;
        }
    }
}
