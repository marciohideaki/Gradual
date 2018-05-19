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
    public class TCPReplayBMF : FixInitiator
    {
        private static TCPReplayBMF _me = null;
        private static Object _mutex = new Object();

        #region ctor
        public TCPReplayBMF(MarketIncrementalProcessor mktIncProc, ChannelUMDFConfig channelConfig, string templateFile, Queue<UdpPacket> qUdpPkt, Object replayLockObject)
            : base(mktIncProc, channelConfig, templateFile, qUdpPkt, replayLockObject)
        {
        }
        #endregion //ctor

        public static TCPReplayBMF GetInstance(MarketIncrementalProcessor mktIncProc, ChannelUMDFConfig channelConfig, string templateFile, Queue<UdpPacket> qUdpPkt, Object replayLockObject)
        {
            if (_me == null)
            {
                lock (_mutex)
                {
                    if (_me == null)
                    {
                        _me = new TCPReplayBMF(mktIncProc, channelConfig, templateFile, qUdpPkt, replayLockObject);
                    }
                }
            }
            return _me;
        }
    }
}
