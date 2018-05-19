using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.PositionClient.Monitor.Utils
{
    public static class DictionaryExtensions
    {
        /*
        public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>
        (this IEnumerable<TValue> source, Func<TValue, TKey> valueSelector)
        {
            return new ConcurrentDictionary<TKey, TValue>(source.ToDictionary(valueSelector));
        }
        */

        public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            return new ConcurrentDictionary<TKey, TValue>(source);
        }

        public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(
            this IEnumerable<TValue> source, Func<TValue, TKey> keySelector)
        {
            //return new ConcurrentDictionary<TKey, TValue>(source);
            
            return new ConcurrentDictionary<TKey, TValue>(
                from v in source
                select new KeyValuePair<TKey, TValue>(keySelector(v), v));
            
        }
        
         
    }
}
