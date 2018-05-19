using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;

namespace TestProjectConsole
{
   public class Program
    {
        /// <summary>
        /// Rafael tHEAD
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {      
            AutoResetEvent autoEvent =
                new AutoResetEvent(false);

            ClasseBancoDeDados DataBase = new ClasseBancoDeDados();
            TimerCallback tcb = DataBase.DisparaMetodoBancoDeDados;

            Timer stateTimer = new Timer(tcb, autoEvent, 0, 5000);

            Console.ReadLine();
        }
    }

   public class ClasseBancoDeDados
    {
        int count = 0;

       [MethodImpl(MethodImplOptions.PreserveSig)]
        public void DisparaMetodoBancoDeDados(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;

            //Chamada de Metodo para o banco de dados
            count++;

            Console.WriteLine(count);

          
        }
    }



}
