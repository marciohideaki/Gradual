using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe de Mutex do cliente onde é armazenado cada
    /// algumas informações como, intervalo de recalculo, 
    /// Primeira hora de procesamento,
    /// </summary>
    public class ClienteMutexInfo
    {
        /// <summary>
        /// Incremental para identificador
        /// </summary>
        public int SomeValue;

        /// <summary>
        /// Timer Reference é usado para 
        /// </summary>
        public System.Threading.Timer TimerReference;

        /// <summary>
        /// 
        /// </summary>
        public bool TimerCanceled;

        /// <summary>
        /// 
        /// </summary>
        public int IdCliente;

        /// <summary>
        /// 
        /// </summary>
        public Mutex _Mutex = new Mutex();

        /// <summary>
        /// 
        /// </summary>
        public EnumProcessamento StatusProcessando { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int Intervalo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FirstTimeProcessed { get; set; }
    }

   
}
