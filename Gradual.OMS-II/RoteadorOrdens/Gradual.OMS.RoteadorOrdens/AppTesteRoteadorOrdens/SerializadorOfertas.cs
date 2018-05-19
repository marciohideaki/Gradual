using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace AppTesteRoteadorOrdens
{
    public class SerializadorOfertas
    {
        public static void SaveOfertas(List<OrdemInfo> lista)
        {
            Stream stream = null;
            string path = ConfigurationManager.AppSettings["ArquivoOrdens"].ToString();

            stream = File.Open(path, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            
            long qtdeitens = lista.Count;
            bformatter.Serialize(stream, qtdeitens);

            for (int i = 0; i < qtdeitens; i++)
            {
                bformatter.Serialize(stream, lista[i]);
            }

            stream.Close();
            stream = null;
        }


        public static List<OrdemInfo> LoadOfertas()
        {
            Stream stream = null;
            List<OrdemInfo> lista = new List<OrdemInfo>();
            string path = ConfigurationManager.AppSettings["ArquivoOrdens"].ToString();

            try
            {

                stream = File.Open(path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                long qtdeitens = (long)bformatter.Deserialize(stream);

                for (int i = 0; i < qtdeitens; i++)
                {
                    OrdemInfo info = (OrdemInfo)bformatter.Deserialize(stream);
                    lista.Add(info);
                }

                stream.Close();
                stream = null;
            }
            catch (Exception ex)
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return lista;
        }

    }
}
