using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Utils
{
    public class Fila<T> : System.Collections.Concurrent.ConcurrentQueue<T>
    {
        public object Sync;
        public String Nome { get; set; }
        public Fila()
        {
            Sync = new object();
        }

        public void Enfileira(T item)
        {
            base.Enqueue(item);

            if (this.Count > 0)
            {
                lock (this.Sync)
                {
                    System.Threading.Monitor.Pulse(this.Sync);
                }
            }
        }

        public T TentaDesinfileirar(out T result)
        {
            while (true)
            {
                bool tentativa = base.TryDequeue(out result);
                if (tentativa)
                {
                    return result;
                }
                else
                {
                    lock (this.Sync)
                    {
                        System.Threading.Monitor.Wait(this.Sync);
                    }
                }

            }
        }
    }

}
