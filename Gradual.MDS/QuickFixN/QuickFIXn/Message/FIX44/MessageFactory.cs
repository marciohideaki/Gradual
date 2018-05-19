// This is a generated file.  Don't edit it directly!

namespace QuickFix
{
    namespace FIX44
    {
        public class MessageFactory : IMessageFactory
        {
            public QuickFix.Message Create(string beginString, string msgType)
            {
                switch (msgType)
                {
                    case QuickFix.FIX44.Heartbeat.MsgType: return new QuickFix.FIX44.Heartbeat();
                    case QuickFix.FIX44.TestRequest.MsgType: return new QuickFix.FIX44.TestRequest();
                    case QuickFix.FIX44.ResendRequest.MsgType: return new QuickFix.FIX44.ResendRequest();
                    case QuickFix.FIX44.Reject.MsgType: return new QuickFix.FIX44.Reject();
                    case QuickFix.FIX44.SequenceReset.MsgType: return new QuickFix.FIX44.SequenceReset();
                    case QuickFix.FIX44.Logout.MsgType: return new QuickFix.FIX44.Logout();
                    case QuickFix.FIX44.Logon.MsgType: return new QuickFix.FIX44.Logon();
                    case QuickFix.FIX44.NetworkStatusResponse.MsgType: return new QuickFix.FIX44.NetworkStatusResponse();
                    case QuickFix.FIX44.BusinessMessageReject.MsgType: return new QuickFix.FIX44.BusinessMessageReject();
                    case QuickFix.FIX44.News.MsgType: return new QuickFix.FIX44.News();
                    case QuickFix.FIX44.MarketDataRequest.MsgType: return new QuickFix.FIX44.MarketDataRequest();
                    case QuickFix.FIX44.MarketDataSnapshotFullRefresh.MsgType: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh();
                    case QuickFix.FIX44.MarketDataIncrementalRefresh.MsgType: return new QuickFix.FIX44.MarketDataIncrementalRefresh();
                    case QuickFix.FIX44.MarketDataRequestReject.MsgType: return new QuickFix.FIX44.MarketDataRequestReject();
                    case QuickFix.FIX44.SecurityListRequest.MsgType: return new QuickFix.FIX44.SecurityListRequest();
                    case QuickFix.FIX44.SecurityList.MsgType: return new QuickFix.FIX44.SecurityList();
                    case QuickFix.FIX44.SecurityStatus.MsgType: return new QuickFix.FIX44.SecurityStatus();
                    case QuickFix.FIX44.LicenseKeyRequest.MsgType: return new QuickFix.FIX44.LicenseKeyRequest();
                    case QuickFix.FIX44.ApplicationMessageRequest.MsgType: return new QuickFix.FIX44.ApplicationMessageRequest();
                    case QuickFix.FIX44.ApplicationMessageRequestAck.MsgType: return new QuickFix.FIX44.ApplicationMessageRequestAck();
                    case QuickFix.FIX44.ApplicationMessageReport.MsgType: return new QuickFix.FIX44.ApplicationMessageReport();
                    case QuickFix.FIX44.ApplicationRawDataReporting.MsgType: return new QuickFix.FIX44.ApplicationRawDataReporting();
                    case QuickFix.FIX44.LicenseKeyResponse.MsgType: return new QuickFix.FIX44.LicenseKeyResponse();
                    case QuickFix.FIX44.LicenseLogoutReport.MsgType: return new QuickFix.FIX44.LicenseLogoutReport();
                    case QuickFix.FIX44.TradeHistoryRequest.MsgType: return new QuickFix.FIX44.TradeHistoryRequest();
                    case QuickFix.FIX44.TradeHistoryResponse.MsgType: return new QuickFix.FIX44.TradeHistoryResponse();
                    case QuickFix.FIX44.MarketTotalsRequest.MsgType: return new QuickFix.FIX44.MarketTotalsRequest();
                    case QuickFix.FIX44.MarketTotalsResponse.MsgType: return new QuickFix.FIX44.MarketTotalsResponse();
                    case QuickFix.FIX44.MarketTotalsBroadcast.MsgType: return new QuickFix.FIX44.MarketTotalsBroadcast();
                    case QuickFix.FIX44.MarketTotalsComposition.MsgType: return new QuickFix.FIX44.MarketTotalsComposition();
                }

                return new QuickFix.Message();
            }


            public Group Create(string beginString, string msgType, int correspondingFieldID)
            {
                if (QuickFix.FIX44.Logon.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoMsgTypes: return new QuickFix.FIX44.Logon.NoMsgTypesGroup();
                    }
                }

                if (QuickFix.FIX44.NetworkStatusResponse.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoCompIDs: return new QuickFix.FIX44.NetworkStatusResponse.NoCompIDsGroup();
                    }
                }

                if (QuickFix.FIX44.News.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoRoutingIDs: return new QuickFix.FIX44.News.NoRoutingIDsGroup();
                        case QuickFix.Fields.Tags.NoRelatedSym: return new QuickFix.FIX44.News.NoRelatedSymGroup();
                        case QuickFix.Fields.Tags.NoSecurityAltID: return new QuickFix.FIX44.News.NoRelatedSymGroup.NoSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoEvents: return new QuickFix.FIX44.News.NoRelatedSymGroup.NoEventsGroup();
                        case QuickFix.Fields.Tags.NoLegs: return new QuickFix.FIX44.News.NoLegsGroup();
                        case QuickFix.Fields.Tags.NoLegSecurityAltID: return new QuickFix.FIX44.News.NoLegsGroup.NoLegSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoUnderlyings: return new QuickFix.FIX44.News.NoUnderlyingsGroup();
                        case QuickFix.Fields.Tags.NoUnderlyingSecurityAltID: return new QuickFix.FIX44.News.NoUnderlyingsGroup.NoUnderlyingSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoUnderlyingStips: return new QuickFix.FIX44.News.NoUnderlyingsGroup.NoUnderlyingStipsGroup();
                        case QuickFix.Fields.Tags.NoLinesOfText: return new QuickFix.FIX44.News.NoLinesOfTextGroup();
                    }
                }

                if (QuickFix.FIX44.MarketDataRequest.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoRelatedSym: return new QuickFix.FIX44.MarketDataRequest.NoRelatedSymGroup();
                        case QuickFix.Fields.Tags.NoSecurityGroups: return new QuickFix.FIX44.MarketDataRequest.NoSecurityGroupsGroup();
                    }
                }

                if (QuickFix.FIX44.MarketDataSnapshotFullRefresh.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoSecurityAltID: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh.NoSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoEvents: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh.NoEventsGroup();
                        case QuickFix.Fields.Tags.NoUnderlyings: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh.NoUnderlyingsGroup();
                        case QuickFix.Fields.Tags.NoUnderlyingSecurityAltID: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh.NoUnderlyingsGroup.NoUnderlyingSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoUnderlyingStips: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh.NoUnderlyingsGroup.NoUnderlyingStipsGroup();
                        case QuickFix.Fields.Tags.NoLegs: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh.NoLegsGroup();
                        case QuickFix.Fields.Tags.NoLegSecurityAltID: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh.NoLegsGroup.NoLegSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoMDEntries: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh.NoMDEntriesGroup();
                        case QuickFix.Fields.Tags.NoReferentialPrices: return new QuickFix.FIX44.MarketDataSnapshotFullRefresh.NoMDEntriesGroup.NoReferentialPricesGroup();
                    }
                }

                if (QuickFix.FIX44.MarketDataIncrementalRefresh.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoMDEntries: return new QuickFix.FIX44.MarketDataIncrementalRefresh.NoMDEntriesGroup();
                        case QuickFix.Fields.Tags.NoSecurityAltID: return new QuickFix.FIX44.MarketDataIncrementalRefresh.NoMDEntriesGroup.NoSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoEvents: return new QuickFix.FIX44.MarketDataIncrementalRefresh.NoMDEntriesGroup.NoEventsGroup();
                        case QuickFix.Fields.Tags.NoUnderlyings: return new QuickFix.FIX44.MarketDataIncrementalRefresh.NoMDEntriesGroup.NoUnderlyingsGroup();
                        case QuickFix.Fields.Tags.NoLegs: return new QuickFix.FIX44.MarketDataIncrementalRefresh.NoMDEntriesGroup.NoLegsGroup();
                        case QuickFix.Fields.Tags.NoLegSecurityAltID: return new QuickFix.FIX44.MarketDataIncrementalRefresh.NoMDEntriesGroup.NoLegsGroup.NoLegSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoReferentialPrices: return new QuickFix.FIX44.MarketDataIncrementalRefresh.NoMDEntriesGroup.NoReferentialPricesGroup();
                    }
                }

                if (QuickFix.FIX44.SecurityList.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoRelatedSym: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup();
                        case QuickFix.Fields.Tags.NoMDFeedTypes: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoMDFeedTypesGroup();
                        case QuickFix.Fields.Tags.NoSecurityAltID: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoEvents: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoEventsGroup();
                        case QuickFix.Fields.Tags.NoInstrAttrib: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoInstrAttribGroup();
                        case QuickFix.Fields.Tags.NoUnderlyings: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoUnderlyingsGroup();
                        case QuickFix.Fields.Tags.NoUnderlyingSecurityAltID: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoUnderlyingsGroup.NoUnderlyingSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoUnderlyingStips: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoUnderlyingsGroup.NoUnderlyingStipsGroup();
                        case QuickFix.Fields.Tags.NoStipulations: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoStipulationsGroup();
                        case QuickFix.Fields.Tags.NoLegs: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoLegsGroup();
                        case QuickFix.Fields.Tags.NoLegSecurityAltID: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoLegsGroup.NoLegSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoLegStipulations: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoLegsGroup.NoLegStipulationsGroup();
                        case QuickFix.Fields.Tags.NoTickRules: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoTickRulesGroup();
                        case QuickFix.Fields.Tags.NoLotTypeRules: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoLotTypeRulesGroup();
                        case QuickFix.Fields.Tags.NoTradingSessionRules: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoTradingSessionRulesGroup();
                        case QuickFix.Fields.Tags.NoOrdTypeRules: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoTradingSessionRulesGroup.NoOrdTypeRulesGroup();
                        case QuickFix.Fields.Tags.NoTimeInForceRules: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoTradingSessionRulesGroup.NoTimeInForceRulesGroup();
                        case QuickFix.Fields.Tags.NoExecInstRules: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoTradingSessionRulesGroup.NoExecInstRulesGroup();
                        case QuickFix.Fields.Tags.NoMatchRules: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoTradingSessionRulesGroup.NoMatchRulesGroup();
                        case QuickFix.Fields.Tags.NoNestedInstrAttrib: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoNestedInstrAttribGroup();
                        case QuickFix.Fields.Tags.NoStrikeRules: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoStrikeRulesGroup();
                        case QuickFix.Fields.Tags.NoMaturityRules: return new QuickFix.FIX44.SecurityList.NoRelatedSymGroup.NoStrikeRulesGroup.NoMaturityRulesGroup();
                    }
                }

                if (QuickFix.FIX44.SecurityStatus.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoSecurityAltID: return new QuickFix.FIX44.SecurityStatus.NoSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoEvents: return new QuickFix.FIX44.SecurityStatus.NoEventsGroup();
                        case QuickFix.Fields.Tags.NoInstrAttrib: return new QuickFix.FIX44.SecurityStatus.NoInstrAttribGroup();
                        case QuickFix.Fields.Tags.NoUnderlyings: return new QuickFix.FIX44.SecurityStatus.NoUnderlyingsGroup();
                        case QuickFix.Fields.Tags.NoUnderlyingSecurityAltID: return new QuickFix.FIX44.SecurityStatus.NoUnderlyingsGroup.NoUnderlyingSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoUnderlyingStips: return new QuickFix.FIX44.SecurityStatus.NoUnderlyingsGroup.NoUnderlyingStipsGroup();
                        case QuickFix.Fields.Tags.NoLegs: return new QuickFix.FIX44.SecurityStatus.NoLegsGroup();
                        case QuickFix.Fields.Tags.NoLegSecurityAltID: return new QuickFix.FIX44.SecurityStatus.NoLegsGroup.NoLegSecurityAltIDGroup();
                    }
                }

                if (QuickFix.FIX44.LicenseKeyRequest.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoPartyIDs: return new QuickFix.FIX44.LicenseKeyRequest.NoPartyIDsGroup();
                        case QuickFix.Fields.Tags.NoPartySubIDs: return new QuickFix.FIX44.LicenseKeyRequest.NoPartyIDsGroup.NoPartySubIDsGroup();
                    }
                }

                if (QuickFix.FIX44.ApplicationMessageRequest.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoApplIDs: return new QuickFix.FIX44.ApplicationMessageRequest.NoApplIDsGroup();
                    }
                }

                if (QuickFix.FIX44.ApplicationMessageRequestAck.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoApplIDs: return new QuickFix.FIX44.ApplicationMessageRequestAck.NoApplIDsGroup();
                    }
                }

                if (QuickFix.FIX44.ApplicationMessageReport.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoApplIDs: return new QuickFix.FIX44.ApplicationMessageReport.NoApplIDsGroup();
                    }
                }

                if (QuickFix.FIX44.ApplicationRawDataReporting.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoApplSeqNums: return new QuickFix.FIX44.ApplicationRawDataReporting.NoApplSeqNumsGroup();
                    }
                }

                if (QuickFix.FIX44.LicenseKeyResponse.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoUserRoleIDs: return new QuickFix.FIX44.LicenseKeyResponse.NoUserRoleIDsGroup();
                        case QuickFix.Fields.Tags.NoPartyIDs: return new QuickFix.FIX44.LicenseKeyResponse.NoPartyIDsGroup();
                        case QuickFix.Fields.Tags.NoPartySubIDs: return new QuickFix.FIX44.LicenseKeyResponse.NoPartyIDsGroup.NoPartySubIDsGroup();
                        case QuickFix.Fields.Tags.NoTradeIDs: return new QuickFix.FIX44.LicenseKeyResponse.NoTradeIDsGroup();
                        case QuickFix.Fields.Tags.NoBroadCastIDs: return new QuickFix.FIX44.LicenseKeyResponse.NoBroadCastIDsGroup();
                        case QuickFix.Fields.Tags.NoNewsAgencyIDs: return new QuickFix.FIX44.LicenseKeyResponse.NoNewsAgencyIDsGroup();
                        case QuickFix.Fields.Tags.NoSupervisedIDs: return new QuickFix.FIX44.LicenseKeyResponse.NoSupervisedIDsGroup();
                    }
                }

                if (QuickFix.FIX44.LicenseLogoutReport.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoPartyIDs: return new QuickFix.FIX44.LicenseLogoutReport.NoPartyIDsGroup();
                        case QuickFix.Fields.Tags.NoPartySubIDs: return new QuickFix.FIX44.LicenseLogoutReport.NoPartyIDsGroup.NoPartySubIDsGroup();
                    }
                }

                if (QuickFix.FIX44.TradeHistoryRequest.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoMDEntryTypes: return new QuickFix.FIX44.TradeHistoryRequest.NoMDEntryTypesGroup();
                        case QuickFix.Fields.Tags.NoRelatedSym: return new QuickFix.FIX44.TradeHistoryRequest.NoRelatedSymGroup();
                    }
                }

                if (QuickFix.FIX44.TradeHistoryResponse.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoSecurityAltID: return new QuickFix.FIX44.TradeHistoryResponse.NoSecurityAltIDGroup();
                        case QuickFix.Fields.Tags.NoEvents: return new QuickFix.FIX44.TradeHistoryResponse.NoEventsGroup();
                        case QuickFix.Fields.Tags.NoMDEntries: return new QuickFix.FIX44.TradeHistoryResponse.NoMDEntriesGroup();
                    }
                }

                if (QuickFix.FIX44.MarketTotalsBroadcast.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoMDEntries: return new QuickFix.FIX44.MarketTotalsBroadcast.NoMDEntriesGroup();
                    }
                }

                if (QuickFix.FIX44.MarketTotalsComposition.MsgType.Equals(msgType))
                {
                    switch (correspondingFieldID)
                    {
                        case QuickFix.Fields.Tags.NoRelatedSym: return new QuickFix.FIX44.MarketTotalsComposition.NoRelatedSymGroup();
                        case QuickFix.Fields.Tags.NoSecurityGroups: return new QuickFix.FIX44.MarketTotalsComposition.NoRelatedSymGroup.NoSecurityGroupsGroup();
                    }
                }

                return null;
            }

        }
    }
}
