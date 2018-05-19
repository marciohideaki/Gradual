using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Core.OMS.DropCopy.Lib;
using log4net;
using Gradual.Core.Spider.AcompanhamentoOrdens.Cache;
using Gradual.Core.OMS.DropCopy.Lib.Mensagens;

namespace Gradual.Core.Spider.AcompanhamentoOrdens.Callback
{
    public class SpiderDropCopyCallback:ISpiderDropCopyCallback
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        OrderCache _parent;

        #endregion

        public SpiderDropCopyCallback(OrderCache parent)
        {
            _parent = parent;
        }

        #region ISpiderDropCopyCallback members
        public void HeartBeat()
        {
            logger.Info("Heartbeat: "  + DateTime.Now);
        }

        public SpiderOrderResponse SendToOrderManager(SpiderOrderRequest req)
        {
            SpiderOrderResponse ret = new SpiderOrderResponse();
            _parent.AddSpiderOrder(req.Order);
            return ret;
        }

        #endregion



    }
}
