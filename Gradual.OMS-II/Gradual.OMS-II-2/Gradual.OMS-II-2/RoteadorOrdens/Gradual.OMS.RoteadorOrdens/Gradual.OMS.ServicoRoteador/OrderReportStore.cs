using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Gradual.OMS.ServicoRoteador
{
    public class OrderReportStore
    {
        private Queue<OrdemInfo> queue101 = new Queue<OrdemInfo>();
        private Queue<OrdemInfo> queue102 = new Queue<OrdemInfo>();
        private Queue<OrdemInfo> queueER = new Queue<OrdemInfo>();
        private bool bWaitForReport = true;
        private bool b101queued = false;
        private bool b102queued = false;
        private bool bERqueued = false;


        public bool WaitForReport
        {
            get { return bWaitForReport; }
            set { bWaitForReport = value; }
        }

        public bool ShouldFlush
        {
            get
            {
                return b101queued && b102queued && bERqueued;
            }
        }

        public void addReport101(OrdemInfo report)
        {
            lock (queue101)
            {
                queue101.Enqueue(report);
                b101queued = true;
            }
        }

        public void addReport102(OrdemInfo report)
        {
            lock (queue102)
            {
                queue102.Enqueue(report);
                b102queued = true;
            }
        }

        public void addExecutionReport(OrdemInfo report)
        {
            lock (queueER)
            {
                queueER.Enqueue(report);
                bERqueued = true;
            }
        }

        public List<OrdemInfo> GetOrderedReports()
        {
            List<OrdemInfo> retorno = new List<OrdemInfo>();

            lock (queue102)
            {
                retorno.AddRange(queue102.ToArray<OrdemInfo>());
            }

            lock (queue101)
            {
                retorno.AddRange(queue101.ToArray<OrdemInfo>());
            }

            lock (queueER)
            {
                retorno.AddRange(queueER.ToArray<OrdemInfo>());
            }

            return retorno;
        }

        public void Reset()
        {
            b101queued = false;
            b102queued = false;
            bERqueued = false;
            bWaitForReport = true;

            lock (queue102)
            {
                queue102.Clear();
            }

            lock (queue101)
            {
                queue101.Clear();
            }

            lock (queueER)
            {
                queueER.Clear();
            }
        }


    }
}
