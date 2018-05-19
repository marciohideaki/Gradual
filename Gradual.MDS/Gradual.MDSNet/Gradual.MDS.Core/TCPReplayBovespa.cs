using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Lib;
using log4net;
using QuickFix;
using OpenFAST;

namespace Gradual.MDS.Core
{
    public class TCPReplayBovespa : FixInitiator
    {
        #region ctor
        public TCPReplayBovespa(MarketIncrementalProcessor mktIncProc, ChannelUMDFConfig channelConfig, string templateFile, Queue<UdpPacket> qUdpPkt, Object replayLockObject) 
            : base(mktIncProc, channelConfig, templateFile, qUdpPkt, replayLockObject)
        {
        }
        #endregion //ctor
    }
}
