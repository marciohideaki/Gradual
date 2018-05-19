using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;

namespace Gradual.OMS.ServicoRoteador
{
    // Implementa uma fila simples persistida em arquivo
    [Serializable]
    public class PersistentQueue<T> 
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Queue<T> queue = new Queue<T>();


        //public  PersistentQueue<T>(string persistencefile)
        //{
        //}

        //public PersistentQueue<T>()
        //{
        //}

        /// <summary>
        /// PersistenceFile - Path completo do arquivo de persistencia
        /// </summary>
        public string PersistenceFile { get; set; }


        /// <summary>
        /// Retorna o numero de itens contidos na fila
        /// </summary>
        public int Count
        {
            get { return queue.Count; }
        }


        /// <summary>
        /// Enqueue() - Insere um objeto na fila
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Desenfileira um objeto da fila
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            T item = default(T);
            lock (queue)
            {
                if ( queue.Count > 0 )
                    item = queue.Dequeue();
            }
            return item;
        }

        /// <summary>
        /// Remove todos os elementos da fila
        /// </summary>
        public void Clear()
        {
            lock (queue)
            {
                while ( queue.Count > 0 )
                {
                    T item = queue.Dequeue();
                }
            }
        }

        /// <summary>
        /// Retorna um List&lt;T&gt; com todos os elementos da fila
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            return queue.ToList();
        }

        /// <summary>
        /// LoadQueue() - Carrega a fila a partir de um arquivo.
        /// </summary>
        /// <returns>qtde de mensagems</returns>
        public long LoadQueue()
        {
            long qtdeMsgs = 0;

            if (PersistenceFile == null || PersistenceFile.Length == 0)
            {
                throw new NullReferenceException("A propriedade PersistenceFile deve ser definido");
            }

            try
            {
                lock (queue)
                {
                    if (System.IO.File.Exists(PersistenceFile))
                    {
                        FileStream fs = File.Open(PersistenceFile, FileMode.Open, FileAccess.Read);
                        BinaryFormatter bformatter = new BinaryFormatter();

                        qtdeMsgs = (long)bformatter.Deserialize(fs);

                        logger.Info("Carregando " + qtdeMsgs + " msgs enfileiradas");

                        for (long i = 0; i < qtdeMsgs; i++)
                        {
                            T item = (T)bformatter.Deserialize(fs);
                            queue.Enqueue(item);
                        }

                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em SaveQueue():" + ex.Message, ex);
            }

            return qtdeMsgs;
        }

        /// <summary>
        /// SaveQueue() - Serializa a fila de mensagens em arquivo
        /// </summary>
        public void SaveQueue()
        {
            long qtdeMsgs = 0;

            if (PersistenceFile == null || PersistenceFile.Length == 0)
            {
                throw new NullReferenceException("A propriedade PersistenceFile deve ser definido");
            }

            try
            {
                FileStream fs = File.Open(PersistenceFile, FileMode.OpenOrCreate, FileAccess.Write);
                BinaryFormatter bformatter = new BinaryFormatter();

                lock (queue)
                {
                    qtdeMsgs = queue.Count();

                    logger.Info("Salvando " + qtdeMsgs + " msgs enfileiradas");

                    bformatter.Serialize(fs, qtdeMsgs);

                    for (long i = 0; i < qtdeMsgs; i++)
                    {
                        T item = (T)queue.Dequeue();
                        bformatter.Serialize(fs, item);
                    }

                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em SaveQueue():" + ex.Message, ex);
            }
        }

    }
}
