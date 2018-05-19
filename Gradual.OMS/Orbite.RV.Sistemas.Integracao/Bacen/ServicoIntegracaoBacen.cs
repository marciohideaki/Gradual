using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Orbite.RV.Contratos.Integracao.Bacen;

namespace Orbite.RV.Sistemas.Integracao.Bacen
{
    public class ServicoIntegracaoBacen : IServicoIntegracaoBacen
    {
        #region IServicoIntegracaoBacen Members

        public DsBCB ReceberSerieLista()
        {
            // Cria a classe que fará a pesquisa
            DsBCB ds = new DsBCB();
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(
                delegate(object param) 
                {
                    object[] param2 = (object[])param;
                    ListaSeries ls = new ListaSeries((DsBCB)param2[0]);
                    ls.Progress.NotificaFimEvent += delegate() 
                    {
                        ((ManualResetEvent)param2[1]).Set();
                        Thread.CurrentThread.Abort();
                    };
                    ls.Start();
                    Thread.Sleep(new TimeSpan(0, 30, 0));
                });
            Thread thread = new Thread(threadStart);
            thread.Start(new object[] {ds, manualResetEvent});

            // Aguarda sinal
            manualResetEvent.WaitOne(new TimeSpan(0, 30, 0));

            // Retorna
            return ds;
        }

        public ListaSeriesProgress ReceberSerieLista(DsBCB ds)
        {
            // Cria a classe que fará a pesquisa
            ListaSeries listaSeries = new ListaSeries(ds);

            // Inicia a estração da lista
            listaSeries.Start();

            // Retorna
            return listaSeries.Progress;
        }

        public DataTable ReceberSerie(long[] series, DateTime dataInicial, DateTime dataFinal)
        {
            WsBCB.FachadaWSSGSService ws = new WsBCB.FachadaWSSGSService();
            //ws.Proxy = new System.Net.WebProxy("mor-isa", 8080);
            WsBCB.ArrayOfflong longs = new WsBCB.ArrayOfflong();
            longs.AddRange(series);
            string ret = ws.getValoresSeriesXML(longs, dataInicial.Date.ToString("dd/MM/yyyy"), dataFinal.Date.ToString("dd/MM/yyyy"));

            MemoryStream ms = new MemoryStream(System.Text.UTF7Encoding.ASCII.GetBytes(ret));

            DataSet ds = new DataSet();
            ds.ReadXml(ms, XmlReadMode.InferSchema);

            DataSetHelper dsHelper = new DataSetHelper(ref ds);
            DataTable tb =
                dsHelper.SelectJoinInto(
                    "ITEM2",
                    ds.Tables["ITEM"],
                    "SERIE_ITEM.ID,DATA,VALOR",
                    null,
                    null);

            DataTable tbDetalhe = ds.Tables["ITEM2"];
            DataTable tbResultado = Utilitarios.Pivot(
                tbDetalhe,
                new DataColumn[] { tbDetalhe.Columns["DATA"], tbDetalhe.Columns["ID"] },
                new DataColumn[] { tbDetalhe.Columns["ID"] },
                null,
                new DataColumn[] { tbDetalhe.Columns["VALOR"] }
            );
            tbResultado.PrimaryKey = new DataColumn[] { tbResultado.Columns["DATA"] };

            return tbResultado;
        }

        #endregion
    }
}
