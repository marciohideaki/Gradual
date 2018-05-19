using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Core.Spider.Monitoring.Lib.Entities;

namespace Gradual.Core.Spider.Monitor.Teste
{
    public class TesteUtil
    {

        public FixSessionInfo GetFixSessionInfo(FixSessionInfo item)
        {
            Random teste = new Random();
            item.ActiveUsers = teste.Next(1, 100);
            item.OrderCount = teste.Next(1, 1000);
            item.LastMessage = DateTime.Now;
            return item;
        }

    }
}
