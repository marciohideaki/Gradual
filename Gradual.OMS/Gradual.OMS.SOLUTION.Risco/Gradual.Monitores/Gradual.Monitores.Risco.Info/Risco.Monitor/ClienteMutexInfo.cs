using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Gradual.Monitores.Risco.Lib
{
    public class ClienteMutexInfo
    {

        public int SomeValue;
        public System.Threading.Timer TimerReference;
        public bool TimerCanceled;
        public int IdCliente;
        public Mutex _Mutex = new Mutex();
        public EnumProcessamento StatusProcessando { set; get; }
    }

   
}
