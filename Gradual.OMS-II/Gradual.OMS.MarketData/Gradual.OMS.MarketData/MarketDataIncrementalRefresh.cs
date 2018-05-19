using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.MarketData
{
    public class MarketDataIncrementalRefresh : QuickFix44.Message
    {
        public const string MSGTYPE = "X";

        public static class NoMDEntries : QuickFix.Group
        {
    
            public NoMDEntries() :
                base(268, 279,
                    new int[] {
                        279, 285, 269, 278, 280, 55, 65, 48, 22, 454, 460, 461, 167,
                        762, 200, 541, 224, 225, 239, 226, 227, 228, 255, 543, 470,
                        471, 472, 240, 202, 947, 206, 231, 223, 207, 106, 348, 349,
                        107, 350, 351, 691, 667, 875, 876, 864, 873, 874, 711, 555,
                        291, 292, 270, 15, 271, 272, 273, 274, 275, 336, 625, 276,
                        277, 282, 283, 284, 286, 59, 432, 126, 110, 18, 287, 37, 299,
                        288, 289, 346, 290, 546, 811, 451, 58, 354, 355, 6032, 326, 6932, 0
                    }) {}
        
            public void set(QuickFix.MDUpdateAction value) 
            {
                setField(value);
            }

            public QuickFix.MDUpdateAction get( QuickFix.MDUpdateAction value)
            {
                getField(value);

                return value;
            }

            public QuickFix.MDUpdateAction getMDUpdateAction()
            {
                QuickFix.MDUpdateAction value = new QuickFix.MDUpdateAction();

                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDUpdateAction field) {
                return isSetField(field);
            }

            public bool isSetMDUpdateAction() {
                return isSetField(279);
            }

            public void set(QuickFix.DeleteReason value) {
                setField(value);
            }

            public QuickFix.DeleteReason get( QuickFix.DeleteReason value)
            {
                getField(value);

                return value;
            }

            public QuickFix.DeleteReason getDeleteReason()
            {
                QuickFix.DeleteReason value = new QuickFix.DeleteReason();

                base.getField(value);

                return value;
            }

            public bool isSet(QuickFix.DeleteReason field) {
                return isSetField(field);
            }

            public bool isSetDeleteReason() {
                return isSetField(285);
            }

            public void set(QuickFix.MDEntryType value) {
                setField(value);
            }

            public QuickFix.MDEntryType get(QuickFix.MDEntryType value){
                getField(value);

                return value;
            }

            public QuickFix.MDEntryType getMDEntryType()
            {
                QuickFix.MDEntryType value = new QuickFix.MDEntryType();

                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntryType field)
            {
                return isSetField(field);
            }

            public bool isSetMDEntryType() {
                return isSetField(269);
            }

            public void set(QuickFix.MDEntryID value)
            {
                setField(value);
            }

            public QuickFix.MDEntryID get(QuickFix.MDEntryID value)
            {
                getField(value);

                return value;
            }

            public QuickFix.MDEntryID getMDEntryID(){

                QuickFix.MDEntryID value = new QuickFix.MDEntryID();

                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntryID field) {
                return isSetField(field);
            }

            public bool isSetMDEntryID() {
                return isSetField(278);
            }

            public void set(QuickFix.MDEntryRefID value) {
                setField(value);
            }

            public QuickFix.MDEntryRefID get( QuickFix.MDEntryRefID value) {
                getField(value);

                return value;
            }

            public QuickFix.MDEntryRefID getMDEntryRefID() {
                QuickFix.MDEntryRefID value = new QuickFix.MDEntryRefID();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntryRefID field) {
                return isSetField(field);
            }

            public bool isSetMDEntryRefID() {
                return isSetField(280);
            }

            public void sunda ()
            {
                QuickFix44
            }

            public void set(Instrument component) {
                setComponent(component);
            }

            public QuickFix.fix44.component.Instrument get(
                QuickFix.fix44.component.Instrument component)
                {
                getComponent(component);

                return component;
            }

            public QuickFix44.IOI getInstrument()
                {
                QuickFix.fix44.component.Instrument component = new QuickFix.fix44.component.Instrument();
                getComponent(component);

                return component;
            }

            public void set(QuickFix.Symbol value) {
                setField(value);
            }

            public QuickFix.Symbol get(QuickFix.Symbol value) {
                getField(value);

                return value;
            }

            public QuickFix.Symbol getSymbol() {
                QuickFix.Symbol value = new QuickFix.Symbol();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.Symbol field) {
                return isSetField(field);
            }

            public bool isSetSymbol() {
                return isSetField(55);
            }

            public void set(QuickFix.SymbolSfx value) {
                setField(value);
            }

            public QuickFix.SymbolSfx get(QuickFix.SymbolSfx value)
            {
                getField(value);

                return value;
            }

            public QuickFix.SymbolSfx getSymbolSfx()
            {
                QuickFix.SymbolSfx value = new QuickFix.SymbolSfx();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.SymbolSfx field) {
                return isSetField(field);
            }

            public bool isSetSymbolSfx() {
                return isSetField(65);
            }

            public void set(QuickFix.SecurityID value) {
                setField(value);
            }

            public QuickFix.SecurityID get(QuickFix.SecurityID value)
                {
                getField(value);

                return value;
            }

            public QuickFix.SecurityID getSecurityID()
            {
                QuickFix.SecurityID value = new QuickFix.SecurityID();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.SecurityID field) {
                return isSetField(field);
            }

            public bool isSetSecurityID() {
                return isSetField(48);
            }

            public void set(QuickFix.SecurityIDSource value) {
                setField(value);
            }

            public QuickFix.SecurityIDSource get(
                QuickFix.SecurityIDSource value) {
                getField(value);

                return value;
            }

            public QuickFix.SecurityIDSource getSecurityIDSource()
                {
                QuickFix.SecurityIDSource value = new QuickFix.SecurityIDSource();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.SecurityIDSource field) {
                return isSetField(field);
            }

            public bool isSetSecurityIDSource() {
                return isSetField(22);
            }

            public void set(QuickFix.NoSecurityAltID value) {
                setField(value);
            }

            public QuickFix.NoSecurityAltID get(
                QuickFix.NoSecurityAltID value) {
                getField(value);

                return value;
            }

            public QuickFix.NoSecurityAltID getNoSecurityAltID()
                {
                QuickFix.NoSecurityAltID value = new QuickFix.NoSecurityAltID();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.NoSecurityAltID field) {
                return isSetField(field);
            }

            public bool isSetNoSecurityAltID() {
                return isSetField(454);
            }

            public void set(QuickFix.Product value) {
                setField(value);
            }

            public QuickFix.Product get(QuickFix.Product value)
                {
                getField(value);

                return value;
            }

            public QuickFix.Product getProduct() {
                QuickFix.Product value = new QuickFix.Product();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.Product field) {
                return isSetField(field);
            }

            public bool isSetProduct() {
                return isSetField(460);
            }

            public void set(QuickFix.CFICode value) {
                setField(value);
            }

            public QuickFix.CFICode get(QuickFix.CFICode value)
                {
                getField(value);

                return value;
            }

            public QuickFix.CFICode getCFICode() {
                QuickFix.CFICode value = new QuickFix.CFICode();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.CFICode field) {
                return isSetField(field);
            }

            public bool isSetCFICode() {
                return isSetField(461);
            }

            public void set(QuickFix.SecurityType value) {
                setField(value);
            }

            public QuickFix.SecurityType get(
                QuickFix.SecurityType value) {
                getField(value);

                return value;
            }

            public QuickFix.SecurityType getSecurityType()
                {
                QuickFix.SecurityType value = new QuickFix.SecurityType();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.SecurityType field) {
                return isSetField(field);
            }

            public bool isSetSecurityType() {
                return isSetField(167);
            }

            public void set(QuickFix.SecuritySubType value) {
                setField(value);
            }

            public QuickFix.SecuritySubType get(
                QuickFix.SecuritySubType value) {
                getField(value);

                return value;
            }

            public QuickFix.SecuritySubType getSecuritySubType()
                {
                QuickFix.SecuritySubType value = new QuickFix.SecuritySubType();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.SecuritySubType field) {
                return isSetField(field);
            }

            public bool isSetSecuritySubType() {
                return isSetField(762);
            }

            public void set(QuickFix.MaturityMonthYear value) {
                setField(value);
            }

            public QuickFix.MaturityMonthYear get(
                QuickFix.MaturityMonthYear value) {
                getField(value);

                return value;
            }

            public QuickFix.MaturityMonthYear getMaturityMonthYear()
                {
                QuickFix.MaturityMonthYear value = new QuickFix.MaturityMonthYear();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MaturityMonthYear field) {
                return isSetField(field);
            }

            public bool isSetMaturityMonthYear() {
                return isSetField(200);
            }

            public void set(QuickFix.MaturityDate value) {
                setField(value);
            }

            public QuickFix.MaturityDate get(
                QuickFix.MaturityDate value) {
                getField(value);

                return value;
            }

            public QuickFix.MaturityDate getMaturityDate()
                {
                QuickFix.MaturityDate value = new QuickFix.MaturityDate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MaturityDate field) {
                return isSetField(field);
            }

            public bool isSetMaturityDate() {
                return isSetField(541);
            }

            public void set(QuickFix.CouponPaymentDate value) {
                setField(value);
            }

            public QuickFix.CouponPaymentDate get(
                QuickFix.CouponPaymentDate value) {
                getField(value);

                return value;
            }

            public QuickFix.CouponPaymentDate getCouponPaymentDate()
                {
                QuickFix.CouponPaymentDate value = new QuickFix.CouponPaymentDate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.CouponPaymentDate field) {
                return isSetField(field);
            }

            public bool isSetCouponPaymentDate() {
                return isSetField(224);
            }

            public void set(QuickFix.IssueDate value) {
                setField(value);
            }

            public QuickFix.IssueDate get(QuickFix.IssueDate value)
                {
                getField(value);

                return value;
            }

            public QuickFix.IssueDate getIssueDate()
                {
                QuickFix.IssueDate value = new QuickFix.IssueDate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.IssueDate field) {
                return isSetField(field);
            }

            public bool isSetIssueDate() {
                return isSetField(225);
            }

            public void set(QuickFix.RepoCollateralSecurityType value) {
                setField(value);
            }

            public QuickFix.RepoCollateralSecurityType get(
                QuickFix.RepoCollateralSecurityType value)
                {
                getField(value);

                return value;
            }

            public QuickFix.RepoCollateralSecurityType getRepoCollateralSecurityType()
                {
                QuickFix.RepoCollateralSecurityType value = new QuickFix.RepoCollateralSecurityType();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.RepoCollateralSecurityType field) {
                return isSetField(field);
            }

            public bool isSetRepoCollateralSecurityType() {
                return isSetField(239);
            }

            public void set(QuickFix.RepurchaseTerm value) {
                setField(value);
            }

            public QuickFix.RepurchaseTerm get(
                QuickFix.RepurchaseTerm value) {
                getField(value);

                return value;
            }

            public QuickFix.RepurchaseTerm getRepurchaseTerm()
                {
                QuickFix.RepurchaseTerm value = new QuickFix.RepurchaseTerm();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.RepurchaseTerm field) {
                return isSetField(field);
            }

            public bool isSetRepurchaseTerm() {
                return isSetField(226);
            }

            public void set(QuickFix.RepurchaseRate value) {
                setField(value);
            }

            public QuickFix.RepurchaseRate get(
                QuickFix.RepurchaseRate value) {
                getField(value);

                return value;
            }

            public QuickFix.RepurchaseRate getRepurchaseRate()
                {
                QuickFix.RepurchaseRate value = new QuickFix.RepurchaseRate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.RepurchaseRate field) {
                return isSetField(field);
            }

            public bool isSetRepurchaseRate() {
                return isSetField(227);
            }

            public void set(QuickFix.Factor value) {
                setField(value);
            }

            public QuickFix.Factor get(QuickFix.Factor value)
                {
                getField(value);

                return value;
            }

            public QuickFix.Factor getFactor() {
                QuickFix.Factor value = new QuickFix.Factor();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.Factor field) {
                return isSetField(field);
            }

            public bool isSetFactor() {
                return isSetField(228);
            }

            public void set(QuickFix.CreditRating value) {
                setField(value);
            }

            public QuickFix.CreditRating get(
                QuickFix.CreditRating value) {
                getField(value);

                return value;
            }

            public QuickFix.CreditRating getCreditRating()
                {
                QuickFix.CreditRating value = new QuickFix.CreditRating();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.CreditRating field) {
                return isSetField(field);
            }

            public bool isSetCreditRating() {
                return isSetField(255);
            }

            public void set(QuickFix.InstrRegistry value) {
                setField(value);
            }

            public QuickFix.InstrRegistry get(
                QuickFix.InstrRegistry value) {
                getField(value);

                return value;
            }

            public QuickFix.InstrRegistry getInstrRegistry()
                {
                QuickFix.InstrRegistry value = new QuickFix.InstrRegistry();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.InstrRegistry field) {
                return isSetField(field);
            }

            public bool isSetInstrRegistry() {
                return isSetField(543);
            }

            public void set(QuickFix.CountryOfIssue value) {
                setField(value);
            }

            public QuickFix.CountryOfIssue get(
                QuickFix.CountryOfIssue value) {
                getField(value);

                return value;
            }

            public QuickFix.CountryOfIssue getCountryOfIssue()
                {
                QuickFix.CountryOfIssue value = new QuickFix.CountryOfIssue();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.CountryOfIssue field) {
                return isSetField(field);
            }

            public bool isSetCountryOfIssue() {
                return isSetField(470);
            }

            public void set(QuickFix.StateOrProvinceOfIssue value) {
                setField(value);
            }

            public QuickFix.StateOrProvinceOfIssue get(
                QuickFix.StateOrProvinceOfIssue value)
                {
                getField(value);

                return value;
            }

            public QuickFix.StateOrProvinceOfIssue getStateOrProvinceOfIssue()
                {
                QuickFix.StateOrProvinceOfIssue value = new QuickFix.StateOrProvinceOfIssue();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.StateOrProvinceOfIssue field) {
                return isSetField(field);
            }

            public bool isSetStateOrProvinceOfIssue() {
                return isSetField(471);
            }

            public void set(QuickFix.LocaleOfIssue value) {
                setField(value);
            }

            public QuickFix.LocaleOfIssue get(
                QuickFix.LocaleOfIssue value) {
                getField(value);

                return value;
            }

            public QuickFix.LocaleOfIssue getLocaleOfIssue()
                {
                QuickFix.LocaleOfIssue value = new QuickFix.LocaleOfIssue();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.LocaleOfIssue field) {
                return isSetField(field);
            }

            public bool isSetLocaleOfIssue() {
                return isSetField(472);
            }

            public void set(QuickFix.RedemptionDate value) {
                setField(value);
            }

            public QuickFix.RedemptionDate get(
                QuickFix.RedemptionDate value) {
                getField(value);

                return value;
            }

            public QuickFix.RedemptionDate getRedemptionDate()
                {
                QuickFix.RedemptionDate value = new QuickFix.RedemptionDate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.RedemptionDate field) {
                return isSetField(field);
            }

            public bool isSetRedemptionDate() {
                return isSetField(240);
            }

            public void set(QuickFix.StrikePrice value) {
                setField(value);
            }

            public QuickFix.StrikePrice get(QuickFix.StrikePrice value)
                {
                getField(value);

                return value;
            }

            public QuickFix.StrikePrice getStrikePrice()
                {
                QuickFix.StrikePrice value = new QuickFix.StrikePrice();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.StrikePrice field) {
                return isSetField(field);
            }

            public bool isSetStrikePrice() {
                return isSetField(202);
            }

            public void set(QuickFix.StrikeCurrency value) {
                setField(value);
            }

            public QuickFix.StrikeCurrency get(
                QuickFix.StrikeCurrency value) {
                getField(value);

                return value;
            }

            public QuickFix.StrikeCurrency getStrikeCurrency()
                {
                QuickFix.StrikeCurrency value = new QuickFix.StrikeCurrency();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.StrikeCurrency field) {
                return isSetField(field);
            }

            public bool isSetStrikeCurrency() {
                return isSetField(947);
            }

            public void set(QuickFix.OptAttribute value) {
                setField(value);
            }

            public QuickFix.OptAttribute get(
                QuickFix.OptAttribute value) {
                getField(value);

                return value;
            }

            public QuickFix.OptAttribute getOptAttribute()
                {
                QuickFix.OptAttribute value = new QuickFix.OptAttribute();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.OptAttribute field) {
                return isSetField(field);
            }

            public bool isSetOptAttribute() {
                return isSetField(206);
            }

            public void set(QuickFix.ContractMultiplier value) {
                setField(value);
            }

            public QuickFix.ContractMultiplier get(
                QuickFix.ContractMultiplier value) {
                getField(value);

                return value;
            }

            public QuickFix.ContractMultiplier getContractMultiplier()
                {
                QuickFix.ContractMultiplier value = new QuickFix.ContractMultiplier();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.ContractMultiplier field) {
                return isSetField(field);
            }

            public bool isSetContractMultiplier() {
                return isSetField(231);
            }

            public void set(QuickFix.CouponRate value) {
                setField(value);
            }

            public QuickFix.CouponRate get(QuickFix.CouponRate value)
                {
                getField(value);

                return value;
            }

            public QuickFix.CouponRate getCouponRate()
                {
                QuickFix.CouponRate value = new QuickFix.CouponRate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.CouponRate field) {
                return isSetField(field);
            }

            public bool isSetCouponRate() {
                return isSetField(223);
            }

            public void set(QuickFix.SecurityExchange value) {
                setField(value);
            }

            public QuickFix.SecurityExchange get(
                QuickFix.SecurityExchange value) {
                getField(value);

                return value;
            }

            public QuickFix.SecurityExchange getSecurityExchange()
                {
                QuickFix.SecurityExchange value = new QuickFix.SecurityExchange();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.SecurityExchange field) {
                return isSetField(field);
            }

            public bool isSetSecurityExchange() {
                return isSetField(207);
            }

            public void set(QuickFix.Issuer value) {
                setField(value);
            }

            public QuickFix.Issuer get(QuickFix.Issuer value)
                {
                getField(value);

                return value;
            }

            public QuickFix.Issuer getIssuer() {
                QuickFix.Issuer value = new QuickFix.Issuer();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.Issuer field) {
                return isSetField(field);
            }

            public bool isSetIssuer() {
                return isSetField(106);
            }

            public void set(QuickFix.EncodedIssuerLen value) {
                setField(value);
            }

            public QuickFix.EncodedIssuerLen get(
                QuickFix.EncodedIssuerLen value) {
                getField(value);

                return value;
            }

            public QuickFix.EncodedIssuerLen getEncodedIssuerLen()
                {
                QuickFix.EncodedIssuerLen value = new QuickFix.EncodedIssuerLen();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.EncodedIssuerLen field) {
                return isSetField(field);
            }

            public bool isSetEncodedIssuerLen() {
                return isSetField(348);
            }

            public void set(QuickFix.EncodedIssuer value) {
                setField(value);
            }

            public QuickFix.EncodedIssuer get(
                QuickFix.EncodedIssuer value) {
                getField(value);

                return value;
            }

            public QuickFix.EncodedIssuer getEncodedIssuer()
                {
                QuickFix.EncodedIssuer value = new QuickFix.EncodedIssuer();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.EncodedIssuer field) {
                return isSetField(field);
            }

            public bool isSetEncodedIssuer() {
                return isSetField(349);
            }

            public void set(QuickFix.SecurityDesc value) {
                setField(value);
            }

            public QuickFix.SecurityDesc get(
                QuickFix.SecurityDesc value) {
                getField(value);

                return value;
            }

            public QuickFix.SecurityDesc getSecurityDesc()
                {
                QuickFix.SecurityDesc value = new QuickFix.SecurityDesc();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.SecurityDesc field) {
                return isSetField(field);
            }

            public bool isSetSecurityDesc() {
                return isSetField(107);
            }

            public void set(QuickFix.EncodedSecurityDescLen value) {
                setField(value);
            }

            public QuickFix.EncodedSecurityDescLen get(
                QuickFix.EncodedSecurityDescLen value)
                {
                getField(value);

                return value;
            }

            public QuickFix.EncodedSecurityDescLen getEncodedSecurityDescLen()
                {
                QuickFix.EncodedSecurityDescLen value = new QuickFix.EncodedSecurityDescLen();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.EncodedSecurityDescLen field) {
                return isSetField(field);
            }

            public bool isSetEncodedSecurityDescLen() {
                return isSetField(350);
            }

            public void set(QuickFix.EncodedSecurityDesc value) {
                setField(value);
            }

            public QuickFix.EncodedSecurityDesc get(
                QuickFix.EncodedSecurityDesc value) {
                getField(value);

                return value;
            }

            public QuickFix.EncodedSecurityDesc getEncodedSecurityDesc()
                {
                QuickFix.EncodedSecurityDesc value = new QuickFix.EncodedSecurityDesc();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.EncodedSecurityDesc field) {
                return isSetField(field);
            }

            public bool isSetEncodedSecurityDesc() {
                return isSetField(351);
            }

            public void set(QuickFix.Pool value) {
                setField(value);
            }

            public QuickFix.Pool get(QuickFix.Pool value)
                {
                getField(value);

                return value;
            }

            public QuickFix.Pool getPool() {
                QuickFix.Pool value = new QuickFix.Pool();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.Pool field) {
                return isSetField(field);
            }

            public bool isSetPool() {
                return isSetField(691);
            }

            public void set(QuickFix.ContractSettlMonth value) {
                setField(value);
            }

            public QuickFix.ContractSettlMonth get(
                QuickFix.ContractSettlMonth value) {
                getField(value);

                return value;
            }

            public QuickFix.ContractSettlMonth getContractSettlMonth()
                {
                QuickFix.ContractSettlMonth value = new QuickFix.ContractSettlMonth();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.ContractSettlMonth field) {
                return isSetField(field);
            }

            public bool isSetContractSettlMonth() {
                return isSetField(667);
            }

            public void set(QuickFix.CPProgram value) {
                setField(value);
            }

            public QuickFix.CPProgram get(QuickFix.CPProgram value)
                {
                getField(value);

                return value;
            }

            public QuickFix.CPProgram getCPProgram()
                {
                QuickFix.CPProgram value = new QuickFix.CPProgram();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.CPProgram field) {
                return isSetField(field);
            }

            public bool isSetCPProgram() {
                return isSetField(875);
            }

            public void set(QuickFix.CPRegType value) {
                setField(value);
            }

            public QuickFix.CPRegType get(QuickFix.CPRegType value)
                {
                getField(value);

                return value;
            }

            public QuickFix.CPRegType getCPRegType()
                {
                QuickFix.CPRegType value = new QuickFix.CPRegType();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.CPRegType field) {
                return isSetField(field);
            }

            public bool isSetCPRegType() {
                return isSetField(876);
            }

            public void set(QuickFix.NoEvents value) {
                setField(value);
            }

            public QuickFix.NoEvents get(QuickFix.NoEvents value)
                {
                getField(value);

                return value;
            }

            public QuickFix.NoEvents getNoEvents() {
                QuickFix.NoEvents value = new QuickFix.NoEvents();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.NoEvents field) {
                return isSetField(field);
            }

            public bool isSetNoEvents() {
                return isSetField(864);
            }

            public void set(QuickFix.DatedDate value) {
                setField(value);
            }

            public QuickFix.DatedDate get(QuickFix.DatedDate value)
                {
                getField(value);

                return value;
            }

            public QuickFix.DatedDate getDatedDate()
                {
                QuickFix.DatedDate value = new QuickFix.DatedDate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.DatedDate field) {
                return isSetField(field);
            }

            public bool isSetDatedDate() {
                return isSetField(873);
            }

            public void set(QuickFix.InterestAccrualDate value) {
                setField(value);
            }

            public QuickFix.InterestAccrualDate get(
                QuickFix.InterestAccrualDate value) {
                getField(value);

                return value;
            }

            public QuickFix.InterestAccrualDate getInterestAccrualDate()
                {
                QuickFix.InterestAccrualDate value = new QuickFix.InterestAccrualDate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.InterestAccrualDate field) {
                return isSetField(field);
            }

            public bool isSetInterestAccrualDate() {
                return isSetField(874);
            }

            public void set(QuickFix.NoUnderlyings value) {
                setField(value);
            }

            public QuickFix.NoUnderlyings get(
                QuickFix.NoUnderlyings value) {
                getField(value);

                return value;
            }

            public QuickFix.NoUnderlyings getNoUnderlyings()
                {
                QuickFix.NoUnderlyings value = new QuickFix.NoUnderlyings();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.NoUnderlyings field) {
                return isSetField(field);
            }

            public bool isSetNoUnderlyings() {
                return isSetField(711);
            }

            public void set(QuickFix.NoLegs value) {
                setField(value);
            }

            public QuickFix.NoLegs get(QuickFix.NoLegs value)
                {
                getField(value);

                return value;
            }

            public QuickFix.NoLegs getNoLegs() {
                QuickFix.NoLegs value = new QuickFix.NoLegs();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.NoLegs field) {
                return isSetField(field);
            }

            public bool isSetNoLegs() {
                return isSetField(555);
            }

            public void set(QuickFix.FinancialStatus value) {
                setField(value);
            }

            public QuickFix.FinancialStatus get(
                QuickFix.FinancialStatus value) {
                getField(value);

                return value;
            }

            public QuickFix.FinancialStatus getFinancialStatus()
                {
                QuickFix.FinancialStatus value = new QuickFix.FinancialStatus();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.FinancialStatus field) {
                return isSetField(field);
            }

            public bool isSetFinancialStatus() {
                return isSetField(291);
            }

            public void set(QuickFix.CorporateAction value) {
                setField(value);
            }

            public QuickFix.CorporateAction get(
                QuickFix.CorporateAction value) {
                getField(value);

                return value;
            }

            public QuickFix.CorporateAction getCorporateAction()
                {
                QuickFix.CorporateAction value = new QuickFix.CorporateAction();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.CorporateAction field) {
                return isSetField(field);
            }

            public bool isSetCorporateAction() {
                return isSetField(292);
            }

            public void set(QuickFix.MDEntryPx value) {
                setField(value);
            }

            public QuickFix.MDEntryPx get(QuickFix.MDEntryPx value)
                {
                getField(value);

                return value;
            }

            public QuickFix.MDEntryPx getMDEntryPx()
                {
                QuickFix.MDEntryPx value = new QuickFix.MDEntryPx();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntryPx field) {
                return isSetField(field);
            }

            public bool isSetMDEntryPx() {
                return isSetField(270);
            }

            public void set(QuickFix.Currency value) {
                setField(value);
            }

            public QuickFix.Currency get(QuickFix.Currency value)
                {
                getField(value);

                return value;
            }

            public QuickFix.Currency getCurrency() {
                QuickFix.Currency value = new QuickFix.Currency();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.Currency field) {
                return isSetField(field);
            }

            public bool isSetCurrency() {
                return isSetField(15);
            }

            public void set(QuickFix.MDEntrySize value) {
                setField(value);
            }

            public QuickFix.MDEntrySize get(QuickFix.MDEntrySize value)
                {
                getField(value);

                return value;
            }

            public QuickFix.MDEntrySize getMDEntrySize()
                {
                QuickFix.MDEntrySize value = new QuickFix.MDEntrySize();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntrySize field) {
                return isSetField(field);
            }

            public bool isSetMDEntrySize() {
                return isSetField(271);
            }

            public void set(QuickFix.MDEntryDate value) {
                setField(value);
            }

            public QuickFix.MDEntryDate get(QuickFix.MDEntryDate value)
                {
                getField(value);

                return value;
            }

            public QuickFix.MDEntryDate getMDEntryDate()
                {
                QuickFix.MDEntryDate value = new QuickFix.MDEntryDate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntryDate field) {
                return isSetField(field);
            }

            public bool isSetMDEntryDate() {
                return isSetField(272);
            }

            public void set(QuickFix.MDEntryTime value) {
                setField(value);
            }

            public QuickFix.MDEntryTime get(QuickFix.MDEntryTime value)
                {
                getField(value);

                return value;
            }

            public QuickFix.MDEntryTime getMDEntryTime()
                {
                QuickFix.MDEntryTime value = new QuickFix.MDEntryTime();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntryTime field) {
                return isSetField(field);
            }

            public bool isSetMDEntryTime() {
                return isSetField(273);
            }

            public void set(QuickFix.TickDirection value) {
                setField(value);
            }

            public QuickFix.TickDirection get(
                QuickFix.TickDirection value) {
                getField(value);

                return value;
            }

            public QuickFix.TickDirection getTickDirection()
                {
                QuickFix.TickDirection value = new QuickFix.TickDirection();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.TickDirection field) {
                return isSetField(field);
            }

            public bool isSetTickDirection() {
                return isSetField(274);
            }

            public void set(QuickFix.MDMkt value) {
                setField(value);
            }

            public QuickFix.MDMkt get(QuickFix.MDMkt value)
                {
                getField(value);

                return value;
            }

            public QuickFix.MDMkt getMDMkt() {
                QuickFix.MDMkt value = new QuickFix.MDMkt();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDMkt field) {
                return isSetField(field);
            }

            public bool isSetMDMkt() {
                return isSetField(275);
            }

            public void set(QuickFix.TradingSessionID value) {
                setField(value);
            }

            public QuickFix.TradingSessionID get(
                QuickFix.TradingSessionID value) {
                getField(value);

                return value;
            }

            public QuickFix.TradingSessionID getTradingSessionID()
                {
                QuickFix.TradingSessionID value = new QuickFix.TradingSessionID();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.TradingSessionID field) {
                return isSetField(field);
            }

            public bool isSetTradingSessionID() {
                return isSetField(336);
            }

            public void set(QuickFix.TradingSessionSubID value) {
                setField(value);
            }

            public QuickFix.TradingSessionSubID get(
                QuickFix.TradingSessionSubID value) {
                getField(value);

                return value;
            }

            public QuickFix.TradingSessionSubID getTradingSessionSubID()
                {
                QuickFix.TradingSessionSubID value = new QuickFix.TradingSessionSubID();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.TradingSessionSubID field) {
                return isSetField(field);
            }

            public bool isSetTradingSessionSubID() {
                return isSetField(625);
            }

            public void set(QuickFix.QuoteCondition value) {
                setField(value);
            }

            public QuickFix.QuoteCondition get(
                QuickFix.QuoteCondition value) {
                getField(value);

                return value;
            }

            public QuickFix.QuoteCondition getQuoteCondition()
                {
                QuickFix.QuoteCondition value = new QuickFix.QuoteCondition();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.QuoteCondition field) {
                return isSetField(field);
            }

            public bool isSetQuoteCondition() {
                return isSetField(276);
            }

            public void set(QuickFix.TradeCondition value) {
                setField(value);
            }

            public QuickFix.TradeCondition get(
                QuickFix.TradeCondition value) {
                getField(value);

                return value;
            }

            public QuickFix.TradeCondition getTradeCondition()
                {
                QuickFix.TradeCondition value = new QuickFix.TradeCondition();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.TradeCondition field) {
                return isSetField(field);
            }

            public bool isSetTradeCondition() {
                return isSetField(277);
            }

            public void set(QuickFix.MDEntryOriginator value) {
                setField(value);
            }

            public QuickFix.MDEntryOriginator get(
                QuickFix.MDEntryOriginator value) {
                getField(value);

                return value;
            }

            public QuickFix.MDEntryOriginator getMDEntryOriginator()
                {
                QuickFix.MDEntryOriginator value = new QuickFix.MDEntryOriginator();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntryOriginator field) {
                return isSetField(field);
            }

            public bool isSetMDEntryOriginator() {
                return isSetField(282);
            }

            public void set(QuickFix.LocationID value) {
                setField(value);
            }

            public QuickFix.LocationID get(QuickFix.LocationID value)
                {
                getField(value);

                return value;
            }

            public QuickFix.LocationID getLocationID()
                {
                QuickFix.LocationID value = new QuickFix.LocationID();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.LocationID field) {
                return isSetField(field);
            }

            public bool isSetLocationID() {
                return isSetField(283);
            }

            public void set(QuickFix.DeskID value) {
                setField(value);
            }

            public QuickFix.DeskID get(QuickFix.DeskID value)
                {
                getField(value);

                return value;
            }

            public QuickFix.DeskID getDeskID() {
                QuickFix.DeskID value = new QuickFix.DeskID();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.DeskID field) {
                return isSetField(field);
            }

            public bool isSetDeskID() {
                return isSetField(284);
            }

            public void set(QuickFix.OpenCloseSettlFlag value) {
                setField(value);
            }

            public QuickFix.OpenCloseSettlFlag get(
                QuickFix.OpenCloseSettlFlag value) {
                getField(value);

                return value;
            }

            public QuickFix.OpenCloseSettlFlag getOpenCloseSettlFlag()
                {
                QuickFix.OpenCloseSettlFlag value = new QuickFix.OpenCloseSettlFlag();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.OpenCloseSettlFlag field) {
                return isSetField(field);
            }

            public bool isSetOpenCloseSettlFlag() {
                return isSetField(286);
            }

            public void set(QuickFix.TimeInForce value) {
                setField(value);
            }

            public QuickFix.TimeInForce get(QuickFix.TimeInForce value)
                {
                getField(value);

                return value;
            }

            public QuickFix.TimeInForce getTimeInForce()
                {
                QuickFix.TimeInForce value = new QuickFix.TimeInForce();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.TimeInForce field) {
                return isSetField(field);
            }

            public bool isSetTimeInForce() {
                return isSetField(59);
            }

            public void set(QuickFix.ExpireDate value) {
                setField(value);
            }

            public QuickFix.ExpireDate get(QuickFix.ExpireDate value)
                {
                getField(value);

                return value;
            }

            public QuickFix.ExpireDate getExpireDate()
                {
                QuickFix.ExpireDate value = new QuickFix.ExpireDate();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.ExpireDate field) {
                return isSetField(field);
            }

            public bool isSetExpireDate() {
                return isSetField(432);
            }

            public void set(QuickFix.ExpireTime value) {
                setField(value);
            }

            public QuickFix.ExpireTime get(QuickFix.ExpireTime value)
                {
                getField(value);

                return value;
            }

            public QuickFix.ExpireTime getExpireTime()
                {
                QuickFix.ExpireTime value = new QuickFix.ExpireTime();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.ExpireTime field) {
                return isSetField(field);
            }

            public bool isSetExpireTime() {
                return isSetField(126);
            }

            public void set(QuickFix.MinQty value) {
                setField(value);
            }

            public QuickFix.MinQty get(QuickFix.MinQty value)
                {
                getField(value);

                return value;
            }

            public QuickFix.MinQty getMinQty() {
                QuickFix.MinQty value = new QuickFix.MinQty();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MinQty field) {
                return isSetField(field);
            }

            public bool isSetMinQty() {
                return isSetField(110);
            }

            public void set(QuickFix.ExecInst value) {
                setField(value);
            }

            public QuickFix.ExecInst get(QuickFix.ExecInst value)
                {
                getField(value);

                return value;
            }

            public QuickFix.ExecInst getExecInst() {
                QuickFix.ExecInst value = new QuickFix.ExecInst();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.ExecInst field) {
                return isSetField(field);
            }

            public bool isSetExecInst() {
                return isSetField(18);
            }

            public void set(QuickFix.SellerDays value) {
                setField(value);
            }

            public QuickFix.SellerDays get(QuickFix.SellerDays value)
                {
                getField(value);

                return value;
            }

            public QuickFix.SellerDays getSellerDays()
                {
                QuickFix.SellerDays value = new QuickFix.SellerDays();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.SellerDays field) {
                return isSetField(field);
            }

            public bool isSetSellerDays() {
                return isSetField(287);
            }

            public void set(QuickFix.OrderID value) {
                setField(value);
            }

            public QuickFix.OrderID get(QuickFix.OrderID value)
                {
                getField(value);

                return value;
            }

            public QuickFix.OrderID getOrderID() {
                QuickFix.OrderID value = new QuickFix.OrderID();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.OrderID field) {
                return isSetField(field);
            }

            public bool isSetOrderID() {
                return isSetField(37);
            }

            public void set(QuickFix.QuoteEntryID value) {
                setField(value);
            }

            public QuickFix.QuoteEntryID get(
                QuickFix.QuoteEntryID value) {
                getField(value);

                return value;
            }

            public QuickFix.QuoteEntryID getQuoteEntryID()
                {
                QuickFix.QuoteEntryID value = new QuickFix.QuoteEntryID();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.QuoteEntryID field) {
                return isSetField(field);
            }

            public bool isSetQuoteEntryID() {
                return isSetField(299);
            }

            public void set(QuickFix.MDEntryBuyer value) {
                setField(value);
            }

            public QuickFix.MDEntryBuyer get(
                QuickFix.MDEntryBuyer value) {
                getField(value);

                return value;
            }

            public QuickFix.MDEntryBuyer getMDEntryBuyer()
                {
                QuickFix.MDEntryBuyer value = new QuickFix.MDEntryBuyer();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntryBuyer field) {
                return isSetField(field);
            }

            public bool isSetMDEntryBuyer() {
                return isSetField(288);
            }

            public void set(QuickFix.MDEntrySeller value) {
                setField(value);
            }

            public QuickFix.MDEntrySeller get(
                QuickFix.MDEntrySeller value) {
                getField(value);

                return value;
            }

            public QuickFix.MDEntrySeller getMDEntrySeller()
                {
                QuickFix.MDEntrySeller value = new QuickFix.MDEntrySeller();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntrySeller field) {
                return isSetField(field);
            }

            public bool isSetMDEntrySeller() {
                return isSetField(289);
            }

            public void set(QuickFix.NumberOfOrders value) {
                setField(value);
            }

            public QuickFix.NumberOfOrders get(
                QuickFix.NumberOfOrders value) {
                getField(value);

                return value;
            }

            public QuickFix.NumberOfOrders getNumberOfOrders()
                {
                QuickFix.NumberOfOrders value = new QuickFix.NumberOfOrders();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.NumberOfOrders field) {
                return isSetField(field);
            }

            public bool isSetNumberOfOrders() {
                return isSetField(346);
            }

            public void set(QuickFix.MDEntryPositionNo value) {
                setField(value);
            }

            public QuickFix.MDEntryPositionNo get(
                QuickFix.MDEntryPositionNo value) {
                getField(value);

                return value;
            }

            public QuickFix.MDEntryPositionNo getMDEntryPositionNo()
                {
                QuickFix.MDEntryPositionNo value = new QuickFix.MDEntryPositionNo();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.MDEntryPositionNo field) {
                return isSetField(field);
            }

            public bool isSetMDEntryPositionNo() {
                return isSetField(290);
            }

            public void set(QuickFix.Scope value) {
                setField(value);
            }

            public QuickFix.Scope get(QuickFix.Scope value)
                {
                getField(value);

                return value;
            }

            public QuickFix.Scope getScope() {
                QuickFix.Scope value = new QuickFix.Scope();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.Scope field) {
                return isSetField(field);
            }

            public bool isSetScope() {
                return isSetField(546);
            }

            public void set(QuickFix.PriceDelta value) {
                setField(value);
            }

            public QuickFix.PriceDelta get(QuickFix.PriceDelta value)
                {
                getField(value);

                return value;
            }

            public QuickFix.PriceDelta getPriceDelta()
                {
                QuickFix.PriceDelta value = new QuickFix.PriceDelta();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.PriceDelta field) {
                return isSetField(field);
            }

            public bool isSetPriceDelta() {
                return isSetField(811);
            }

            public void set(QuickFix.NetChgPrevDay value) {
                setField(value);
            }

            public QuickFix.NetChgPrevDay get(
                QuickFix.NetChgPrevDay value) {
                getField(value);

                return value;
            }

            public QuickFix.NetChgPrevDay getNetChgPrevDay()
                {
                QuickFix.NetChgPrevDay value = new QuickFix.NetChgPrevDay();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.NetChgPrevDay field) {
                return isSetField(field);
            }

            public bool isSetNetChgPrevDay() {
                return isSetField(451);
            }

            public void set(QuickFix.Text value) {
                setField(value);
            }

            public QuickFix.Text get(QuickFix.Text value)
                {
                getField(value);

                return value;
            }

            public QuickFix.Text getText() {
                QuickFix.Text value = new QuickFix.Text();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.Text field) {
                return isSetField(field);
            }

            public bool isSetText() {
                return isSetField(58);
            }

            public void set(QuickFix.EncodedTextLen value) {
                setField(value);
            }

            public QuickFix.EncodedTextLen get(
                QuickFix.EncodedTextLen value) {
                getField(value);

                return value;
            }

            public QuickFix.EncodedTextLen getEncodedTextLen()
                {
                QuickFix.EncodedTextLen value = new QuickFix.EncodedTextLen();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.EncodedTextLen field) {
                return isSetField(field);
            }

            public bool isSetEncodedTextLen() {
                return isSetField(354);
            }

            public void set(QuickFix.EncodedText value) {
                setField(value);
            }

            public QuickFix.EncodedText get(QuickFix.EncodedText value)
                {
                getField(value);

                return value;
            }

            public QuickFix.EncodedText getEncodedText()
                {
                QuickFix.EncodedText value = new QuickFix.EncodedText();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.EncodedText field) {
                return isSetField(field);
            }

            public bool isSetEncodedText() {
                return isSetField(355);
            }

		    public void set(br.com.gradualinvestimentos.mds.QuickFix.UniqueTradeID value)
		    {
			    setField(value);
		    }

		    public Gradual.OMS.MarketData.UniqueTradeID get(Gradual.OMS.MarketData.UniqueTradeID value)
		    {
			    getField(value);
			    return value;
		    }

		    public Gradual.OMS.MarketData.UniqueTradeID getUniqueTradeID()
		    {
			    Gradual.OMS.MarketData.UniqueTradeID value = 
				    new Gradual.OMS.MarketData.UniqueTradeID();

			    getField(value);
			    return value;
		    }

		    public bool isSet(Gradual.OMS.MarketData.UniqueTradeID field)
		    {
			    return isSetField(field);
		    }

		    public bool isSetUniqueTradeID()
		    {
			    return isSetField(6032);
		    }

		    public void set(QuickFix.SecurityTradingStatus value)
		    {
			    setField(value);
		    }

		    public QuickFix.SecurityTradingStatus get( QuickFix.SecurityTradingStatus  value)
		    {
			    getField(value);
			    return value;
		    }

		    public QuickFix.SecurityTradingStatus getSecurityTradingStatus()
		    {
			    QuickFix.SecurityTradingStatus value = 
				    new QuickFix.SecurityTradingStatus();
			    getField(value);
			    return value;
		    }

		    public bool isSet(QuickFix.SecurityTradingStatus field)
		    {
			    return isSetField(field);
		    }

		    public bool isSetSecurityTradingStatus()
		    {
			    return isSetField(326);
		    }

		    public void set(Gradual.OMS.MarketData.NoReferentialPrices value)
		    {
			    setField(value);
		    }

		    public Gradual.OMS.MarketData.NoReferentialPrices 
			    get(Gradual.OMS.MarketData.NoReferentialPrices value)
		    {
			    getField(value);
			    return value;
		    }

		    public Gradual.OMS.MarketData.NoReferentialPrices 
			    getNoReferentialPrices()
		    {
			    Gradual.OMS.MarketData.NoReferentialPrices value = 
				    new Gradual.OMS.MarketData.NoReferentialPrices();
			    getField(value);
			    return value;
		    }

		    public bool isSet( NoReferentialPrices field)
		    {
			    return isSetField(field);
		    }

		    public bool isSetNoReferentialPrices()
		    {
			    return isSetField(6932);
		    }

            public NoSecurityAltID NoSecurityAltID {get;set;}


            public static class NoEvents : QuickFix.Group
            {
                public NoEvents() : base (864, 865, new int[] { 865, 866, 867, 868, 0 }){}

                public void set(QuickFix.EventType value)
                {
                    setField(value);
                }

                public QuickFix.EventType get(QuickFix.EventType value)
                {
                    getField(value);
                    return value;
                }

                public QuickFix.EventType getEventType()
                {
                    QuickFix.EventType value = new QuickFix.EventType();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.EventType field)
                {
                    return isSetField(field);
                }

                public bool isSetEventType()
                {
                    return isSetField(865);
                }

                public void set(QuickFix.EventDate value)
                {
                    setField(value);
                }

                public QuickFix.EventDate get(QuickFix.EventDate value)
                {
                    getField(value);
                    return value;
                }

                public QuickFix.EventDate getEventDate()
                {
                    QuickFix.EventDate value = new QuickFix.EventDate();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.EventDate field)
                {
                    return isSetField(field);
                }

                public bool isSetEventDate() 
                {
                    return isSetField(866);
                }

                public void set(QuickFix.EventPx value)
                {
                    setField(value);
                }

                public QuickFix.EventPx get(QuickFix.EventPx value)
                {
                    getField(value);

                    return value;
                }

                public QuickFix.EventPx getEventPx()
                {
                    QuickFix.EventPx value = new QuickFix.EventPx();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.EventPx field)
                {
                    return isSetField(field);
                }

                public bool isSetEventPx()
                {
                    return isSetField(867);
                }

                public void set(QuickFix.EventText value)
                {
                    setField(value);
                }

                public QuickFix.EventText get(QuickFix.EventText value)
                {
                    getField(value);
                    return value;
                }

                public QuickFix.EventText getEventText()
                {
                    QuickFix.EventText value = new QuickFix.EventText();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.EventText field)
                {
                    return isSetField(field);
                }

                public bool isSetEventText()
                {
                    return isSetField(868);
                }
            }

            public static class NoUnderlyings : QuickFix.Group 

            public static class NoLegs extends Group {
                static final long serialVersionUID = 20050617;

                public NoLegs() {
                    super(555, 600,
                        new int[] {
                            600, 601, 602, 603, 604, 607, 608, 609, 764, 610, 611,
                            248, 249, 250, 251, 252, 253, 257, 599, 596, 597, 598,
                            254, 612, 942, 613, 614, 615, 616, 617, 618, 619, 620,
                            621, 622, 623, 624, 556, 740, 739, 955, 956, 0
                        });
                }

                public void set(QuickFix.fix44.component.InstrumentLeg component) {
                    setComponent(component);
                }

                public QuickFix.fix44.component.InstrumentLeg get(
                    QuickFix.fix44.component.InstrumentLeg component)
                    {
                    getComponent(component);

                    return component;
                }

                QuickFix.Com

                public QuickFix.fix44.component.InstrumentLeg getInstrumentLeg()
                    {
                    QuickFix.fix44.component.InstrumentLeg component = new QuickFix.fix44.component.InstrumentLeg();
                    getComponent(component);

                    return component;
                }

                public void set(QuickFix.LegSymbol value) {
                    setField(value);
                }

                public QuickFix.LegSymbol get(QuickFix.LegSymbol value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegSymbol getLegSymbol()
                    {
                    QuickFix.LegSymbol value = new QuickFix.LegSymbol();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegSymbol field) {
                    return isSetField(field);
                }

                public bool isSetLegSymbol() {
                    return isSetField(600);
                }

                public void set(QuickFix.LegSymbolSfx value) {
                    setField(value);
                }

                public QuickFix.LegSymbolSfx get(
                    QuickFix.LegSymbolSfx value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegSymbolSfx getLegSymbolSfx()
                    {
                    QuickFix.LegSymbolSfx value = new QuickFix.LegSymbolSfx();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegSymbolSfx field) {
                    return isSetField(field);
                }

                public bool isSetLegSymbolSfx() {
                    return isSetField(601);
                }

                public void set(QuickFix.LegSecurityID value) {
                    setField(value);
                }

                public QuickFix.LegSecurityID get(
                    QuickFix.LegSecurityID value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegSecurityID getLegSecurityID()
                    {
                    QuickFix.LegSecurityID value = new QuickFix.LegSecurityID();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegSecurityID field) {
                    return isSetField(field);
                }

                public bool isSetLegSecurityID() {
                    return isSetField(602);
                }

                public void set(QuickFix.LegSecurityIDSource value) {
                    setField(value);
                }

                public QuickFix.LegSecurityIDSource get(
                    QuickFix.LegSecurityIDSource value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegSecurityIDSource getLegSecurityIDSource()
                    {
                    QuickFix.LegSecurityIDSource value = new QuickFix.LegSecurityIDSource();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegSecurityIDSource field) {
                    return isSetField(field);
                }

                public bool isSetLegSecurityIDSource() {
                    return isSetField(603);
                }

                public void set(QuickFix.NoLegSecurityAltID value) {
                    setField(value);
                }

                public QuickFix.NoLegSecurityAltID get(
                    QuickFix.NoLegSecurityAltID value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.NoLegSecurityAltID getNoLegSecurityAltID()
                    {
                    QuickFix.NoLegSecurityAltID value = new QuickFix.NoLegSecurityAltID();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.NoLegSecurityAltID field) {
                    return isSetField(field);
                }

                public bool isSetNoLegSecurityAltID() {
                    return isSetField(604);
                }

                public void set(QuickFix.LegProduct value) {
                    setField(value);
                }

                public QuickFix.LegProduct get(
                    QuickFix.LegProduct value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegProduct getLegProduct()
                    {
                    QuickFix.LegProduct value = new QuickFix.LegProduct();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegProduct field) {
                    return isSetField(field);
                }

                public bool isSetLegProduct() {
                    return isSetField(607);
                }

                public void set(QuickFix.LegCFICode value) {
                    setField(value);
                }

                public QuickFix.LegCFICode get(
                    QuickFix.LegCFICode value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegCFICode getLegCFICode()
                    {
                    QuickFix.LegCFICode value = new QuickFix.LegCFICode();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegCFICode field) {
                    return isSetField(field);
                }

                public bool isSetLegCFICode() {
                    return isSetField(608);
                }

                public void set(QuickFix.LegSecurityType value) {
                    setField(value);
                }

                public QuickFix.LegSecurityType get(
                    QuickFix.LegSecurityType value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegSecurityType getLegSecurityType()
                    {
                    QuickFix.LegSecurityType value = new QuickFix.LegSecurityType();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegSecurityType field) {
                    return isSetField(field);
                }

                public bool isSetLegSecurityType() {
                    return isSetField(609);
                }

                public void set(QuickFix.LegSecuritySubType value) {
                    setField(value);
                }

                public QuickFix.LegSecuritySubType get(
                    QuickFix.LegSecuritySubType value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegSecuritySubType getLegSecuritySubType()
                    {
                    QuickFix.LegSecuritySubType value = new QuickFix.LegSecuritySubType();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegSecuritySubType field) {
                    return isSetField(field);
                }

                public bool isSetLegSecuritySubType() {
                    return isSetField(764);
                }

                public void set(QuickFix.LegMaturityMonthYear value) {
                    setField(value);
                }

                public QuickFix.LegMaturityMonthYear get(
                    QuickFix.LegMaturityMonthYear value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegMaturityMonthYear getLegMaturityMonthYear()
                    {
                    QuickFix.LegMaturityMonthYear value = new QuickFix.LegMaturityMonthYear();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegMaturityMonthYear field) {
                    return isSetField(field);
                }

                public bool isSetLegMaturityMonthYear() {
                    return isSetField(610);
                }

                public void set(QuickFix.LegMaturityDate value) {
                    setField(value);
                }

                public QuickFix.LegMaturityDate get(
                    QuickFix.LegMaturityDate value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegMaturityDate getLegMaturityDate()
                    {
                    QuickFix.LegMaturityDate value = new QuickFix.LegMaturityDate();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegMaturityDate field) {
                    return isSetField(field);
                }

                public bool isSetLegMaturityDate() {
                    return isSetField(611);
                }

                public void set(QuickFix.LegCouponPaymentDate value) {
                    setField(value);
                }

                public QuickFix.LegCouponPaymentDate get(
                    QuickFix.LegCouponPaymentDate value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegCouponPaymentDate getLegCouponPaymentDate()
                    {
                    QuickFix.LegCouponPaymentDate value = new QuickFix.LegCouponPaymentDate();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegCouponPaymentDate field) {
                    return isSetField(field);
                }

                public bool isSetLegCouponPaymentDate() {
                    return isSetField(248);
                }

                public void set(QuickFix.LegIssueDate value) {
                    setField(value);
                }

                public QuickFix.LegIssueDate get(
                    QuickFix.LegIssueDate value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegIssueDate getLegIssueDate()
                    {
                    QuickFix.LegIssueDate value = new QuickFix.LegIssueDate();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegIssueDate field) {
                    return isSetField(field);
                }

                public bool isSetLegIssueDate() {
                    return isSetField(249);
                }

                public void set(QuickFix.LegRepoCollateralSecurityType value) {
                    setField(value);
                }

                public QuickFix.LegRepoCollateralSecurityType get(
                    QuickFix.LegRepoCollateralSecurityType value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegRepoCollateralSecurityType getLegRepoCollateralSecurityType()
                    {
                    QuickFix.LegRepoCollateralSecurityType value = new QuickFix.LegRepoCollateralSecurityType();
                    getField(value);

                    return value;
                }

                public bool isSet(
                    QuickFix.LegRepoCollateralSecurityType field) {
                    return isSetField(field);
                }

                public bool isSetLegRepoCollateralSecurityType() {
                    return isSetField(250);
                }

                public void set(QuickFix.LegRepurchaseTerm value) {
                    setField(value);
                }

                public QuickFix.LegRepurchaseTerm get(
                    QuickFix.LegRepurchaseTerm value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegRepurchaseTerm getLegRepurchaseTerm()
                    {
                    QuickFix.LegRepurchaseTerm value = new QuickFix.LegRepurchaseTerm();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegRepurchaseTerm field) {
                    return isSetField(field);
                }

                public bool isSetLegRepurchaseTerm() {
                    return isSetField(251);
                }

                public void set(QuickFix.LegRepurchaseRate value) {
                    setField(value);
                }

                public QuickFix.LegRepurchaseRate get(
                    QuickFix.LegRepurchaseRate value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegRepurchaseRate getLegRepurchaseRate()
                    {
                    QuickFix.LegRepurchaseRate value = new QuickFix.LegRepurchaseRate();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegRepurchaseRate field) {
                    return isSetField(field);
                }

                public bool isSetLegRepurchaseRate() {
                    return isSetField(252);
                }

                public void set(QuickFix.LegFactor value) {
                    setField(value);
                }

                public QuickFix.LegFactor get(QuickFix.LegFactor value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegFactor getLegFactor()
                    {
                    QuickFix.LegFactor value = new QuickFix.LegFactor();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegFactor field) {
                    return isSetField(field);
                }

                public bool isSetLegFactor() {
                    return isSetField(253);
                }

                public void set(QuickFix.LegCreditRating value) {
                    setField(value);
                }

                public QuickFix.LegCreditRating get(
                    QuickFix.LegCreditRating value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegCreditRating getLegCreditRating()
                    {
                    QuickFix.LegCreditRating value = new QuickFix.LegCreditRating();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegCreditRating field) {
                    return isSetField(field);
                }

                public bool isSetLegCreditRating() {
                    return isSetField(257);
                }

                public void set(QuickFix.LegInstrRegistry value) {
                    setField(value);
                }

                public QuickFix.LegInstrRegistry get(
                    QuickFix.LegInstrRegistry value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegInstrRegistry getLegInstrRegistry()
                    {
                    QuickFix.LegInstrRegistry value = new QuickFix.LegInstrRegistry();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegInstrRegistry field) {
                    return isSetField(field);
                }

                public bool isSetLegInstrRegistry() {
                    return isSetField(599);
                }

                public void set(QuickFix.LegCountryOfIssue value) {
                    setField(value);
                }

                public QuickFix.LegCountryOfIssue get(
                    QuickFix.LegCountryOfIssue value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegCountryOfIssue getLegCountryOfIssue()
                    {
                    QuickFix.LegCountryOfIssue value = new QuickFix.LegCountryOfIssue();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegCountryOfIssue field) {
                    return isSetField(field);
                }

                public bool isSetLegCountryOfIssue() {
                    return isSetField(596);
                }

                public void set(QuickFix.LegStateOrProvinceOfIssue value) {
                    setField(value);
                }

                public QuickFix.LegStateOrProvinceOfIssue get(
                    QuickFix.LegStateOrProvinceOfIssue value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegStateOrProvinceOfIssue getLegStateOrProvinceOfIssue()
                    {
                    QuickFix.LegStateOrProvinceOfIssue value = new QuickFix.LegStateOrProvinceOfIssue();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegStateOrProvinceOfIssue field) {
                    return isSetField(field);
                }

                public bool isSetLegStateOrProvinceOfIssue() {
                    return isSetField(597);
                }

                public void set(QuickFix.LegLocaleOfIssue value) {
                    setField(value);
                }

                public QuickFix.LegLocaleOfIssue get(
                    QuickFix.LegLocaleOfIssue value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegLocaleOfIssue getLegLocaleOfIssue()
                    {
                    QuickFix.LegLocaleOfIssue value = new QuickFix.LegLocaleOfIssue();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegLocaleOfIssue field) {
                    return isSetField(field);
                }

                public bool isSetLegLocaleOfIssue() {
                    return isSetField(598);
                }

                public void set(QuickFix.LegRedemptionDate value) {
                    setField(value);
                }

                public QuickFix.LegRedemptionDate get(
                    QuickFix.LegRedemptionDate value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegRedemptionDate getLegRedemptionDate()
                    {
                    QuickFix.LegRedemptionDate value = new QuickFix.LegRedemptionDate();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegRedemptionDate field) {
                    return isSetField(field);
                }

                public bool isSetLegRedemptionDate() {
                    return isSetField(254);
                }

                public void set(QuickFix.LegStrikePrice value) {
                    setField(value);
                }

                public QuickFix.LegStrikePrice get(
                    QuickFix.LegStrikePrice value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegStrikePrice getLegStrikePrice()
                    {
                    QuickFix.LegStrikePrice value = new QuickFix.LegStrikePrice();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegStrikePrice field) {
                    return isSetField(field);
                }

                public bool isSetLegStrikePrice() {
                    return isSetField(612);
                }

                public void set(QuickFix.LegStrikeCurrency value) {
                    setField(value);
                }

                public QuickFix.LegStrikeCurrency get(
                    QuickFix.LegStrikeCurrency value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegStrikeCurrency getLegStrikeCurrency()
                    {
                    QuickFix.LegStrikeCurrency value = new QuickFix.LegStrikeCurrency();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegStrikeCurrency field) {
                    return isSetField(field);
                }

                public bool isSetLegStrikeCurrency() {
                    return isSetField(942);
                }

                public void set(QuickFix.LegOptAttribute value) {
                    setField(value);
                }

                public QuickFix.LegOptAttribute get(
                    QuickFix.LegOptAttribute value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegOptAttribute getLegOptAttribute()
                    {
                    QuickFix.LegOptAttribute value = new QuickFix.LegOptAttribute();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegOptAttribute field) {
                    return isSetField(field);
                }

                public bool isSetLegOptAttribute() {
                    return isSetField(613);
                }

                public void set(QuickFix.LegContractMultiplier value) {
                    setField(value);
                }

                public QuickFix.LegContractMultiplier get(
                    QuickFix.LegContractMultiplier value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegContractMultiplier getLegContractMultiplier()
                    {
                    QuickFix.LegContractMultiplier value = new QuickFix.LegContractMultiplier();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegContractMultiplier field) {
                    return isSetField(field);
                }

                public bool isSetLegContractMultiplier() {
                    return isSetField(614);
                }

                public void set(QuickFix.LegCouponRate value) {
                    setField(value);
                }

                public QuickFix.LegCouponRate get(
                    QuickFix.LegCouponRate value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegCouponRate getLegCouponRate()
                    {
                    QuickFix.LegCouponRate value = new QuickFix.LegCouponRate();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegCouponRate field) {
                    return isSetField(field);
                }

                public bool isSetLegCouponRate() {
                    return isSetField(615);
                }

                public void set(QuickFix.LegSecurityExchange value) {
                    setField(value);
                }

                public QuickFix.LegSecurityExchange get(
                    QuickFix.LegSecurityExchange value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegSecurityExchange getLegSecurityExchange()
                    {
                    QuickFix.LegSecurityExchange value = new QuickFix.LegSecurityExchange();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegSecurityExchange field) {
                    return isSetField(field);
                }

                public bool isSetLegSecurityExchange() {
                    return isSetField(616);
                }

                public void set(QuickFix.LegIssuer value) {
                    setField(value);
                }

                public QuickFix.LegIssuer get(QuickFix.LegIssuer value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegIssuer getLegIssuer()
                    {
                    QuickFix.LegIssuer value = new QuickFix.LegIssuer();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegIssuer field) {
                    return isSetField(field);
                }

                public bool isSetLegIssuer() {
                    return isSetField(617);
                }

                public void set(QuickFix.EncodedLegIssuerLen value) {
                    setField(value);
                }

                public QuickFix.EncodedLegIssuerLen get(
                    QuickFix.EncodedLegIssuerLen value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.EncodedLegIssuerLen getEncodedLegIssuerLen()
                    {
                    QuickFix.EncodedLegIssuerLen value = new QuickFix.EncodedLegIssuerLen();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.EncodedLegIssuerLen field) {
                    return isSetField(field);
                }

                public bool isSetEncodedLegIssuerLen() {
                    return isSetField(618);
                }

                public void set(QuickFix.EncodedLegIssuer value) {
                    setField(value);
                }

                public QuickFix.EncodedLegIssuer get(
                    QuickFix.EncodedLegIssuer value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.EncodedLegIssuer getEncodedLegIssuer()
                    {
                    QuickFix.EncodedLegIssuer value = new QuickFix.EncodedLegIssuer();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.EncodedLegIssuer field) {
                    return isSetField(field);
                }

                public bool isSetEncodedLegIssuer() {
                    return isSetField(619);
                }

                public void set(QuickFix.LegSecurityDesc value) {
                    setField(value);
                }

                public QuickFix.LegSecurityDesc get(
                    QuickFix.LegSecurityDesc value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegSecurityDesc getLegSecurityDesc()
                    {
                    QuickFix.LegSecurityDesc value = new QuickFix.LegSecurityDesc();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegSecurityDesc field) {
                    return isSetField(field);
                }

                public bool isSetLegSecurityDesc() {
                    return isSetField(620);
                }

                public void set(QuickFix.EncodedLegSecurityDescLen value) {
                    setField(value);
                }

                public QuickFix.EncodedLegSecurityDescLen get(
                    QuickFix.EncodedLegSecurityDescLen value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.EncodedLegSecurityDescLen getEncodedLegSecurityDescLen()
                    {
                    QuickFix.EncodedLegSecurityDescLen value = new QuickFix.EncodedLegSecurityDescLen();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.EncodedLegSecurityDescLen field) {
                    return isSetField(field);
                }

                public bool isSetEncodedLegSecurityDescLen() {
                    return isSetField(621);
                }

                public void set(QuickFix.EncodedLegSecurityDesc value) {
                    setField(value);
                }

                public QuickFix.EncodedLegSecurityDesc get(
                    QuickFix.EncodedLegSecurityDesc value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.EncodedLegSecurityDesc getEncodedLegSecurityDesc()
                    {
                    QuickFix.EncodedLegSecurityDesc value = new QuickFix.EncodedLegSecurityDesc();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.EncodedLegSecurityDesc field) {
                    return isSetField(field);
                }

                public bool isSetEncodedLegSecurityDesc() {
                    return isSetField(622);
                }

                public void set(QuickFix.LegRatioQty value) {
                    setField(value);
                }

                public QuickFix.LegRatioQty get(
                    QuickFix.LegRatioQty value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegRatioQty getLegRatioQty()
                    {
                    QuickFix.LegRatioQty value = new QuickFix.LegRatioQty();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegRatioQty field) {
                    return isSetField(field);
                }

                public bool isSetLegRatioQty() {
                    return isSetField(623);
                }

                public void set(QuickFix.LegSide value) {
                    setField(value);
                }

                public QuickFix.LegSide get(QuickFix.LegSide value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegSide getLegSide()
                    {
                    QuickFix.LegSide value = new QuickFix.LegSide();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegSide field) {
                    return isSetField(field);
                }

                public bool isSetLegSide() {
                    return isSetField(624);
                }

                public void set(QuickFix.LegCurrency value) {
                    setField(value);
                }

                public QuickFix.LegCurrency get(
                    QuickFix.LegCurrency value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegCurrency getLegCurrency()
                    {
                    QuickFix.LegCurrency value = new QuickFix.LegCurrency();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegCurrency field) {
                    return isSetField(field);
                }

                public bool isSetLegCurrency() {
                    return isSetField(556);
                }

                public void set(QuickFix.LegPool value) {
                    setField(value);
                }

                public QuickFix.LegPool get(QuickFix.LegPool value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegPool getLegPool()
                    {
                    QuickFix.LegPool value = new QuickFix.LegPool();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegPool field) {
                    return isSetField(field);
                }

                public bool isSetLegPool() {
                    return isSetField(740);
                }

                public void set(QuickFix.LegDatedDate value) {
                    setField(value);
                }

                public QuickFix.LegDatedDate get(
                    QuickFix.LegDatedDate value) {
                    getField(value);

                    return value;
                }

                public QuickFix.LegDatedDate getLegDatedDate()
                    {
                    QuickFix.LegDatedDate value = new QuickFix.LegDatedDate();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegDatedDate field) {
                    return isSetField(field);
                }

                public bool isSetLegDatedDate() {
                    return isSetField(739);
                }

                public void set(QuickFix.LegContractSettlMonth value) {
                    setField(value);
                }

                public QuickFix.LegContractSettlMonth get(
                    QuickFix.LegContractSettlMonth value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegContractSettlMonth getLegContractSettlMonth()
                    {
                    QuickFix.LegContractSettlMonth value = new QuickFix.LegContractSettlMonth();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegContractSettlMonth field) {
                    return isSetField(field);
                }

                public bool isSetLegContractSettlMonth() {
                    return isSetField(955);
                }

                public void set(QuickFix.LegInterestAccrualDate value) {
                    setField(value);
                }

                public QuickFix.LegInterestAccrualDate get(
                    QuickFix.LegInterestAccrualDate value)
                    {
                    getField(value);

                    return value;
                }

                public QuickFix.LegInterestAccrualDate getLegInterestAccrualDate()
                    {
                    QuickFix.LegInterestAccrualDate value = new QuickFix.LegInterestAccrualDate();
                    getField(value);

                    return value;
                }

                public bool isSet(QuickFix.LegInterestAccrualDate field) {
                    return isSetField(field);
                }

                public bool isSetLegInterestAccrualDate() {
                    return isSetField(956);
                }

                public static class NoLegSecurityAltID extends Group {
                    static final long serialVersionUID = 20050617;

                    public NoLegSecurityAltID() {
                        super(604, 605, new int[] { 605, 606, 0 });
                    }

                    public void set(QuickFix.LegSecurityAltID value) {
                        setField(value);
                    }

                    public QuickFix.LegSecurityAltID get(
                        QuickFix.LegSecurityAltID value)
                        {
                        getField(value);

                        return value;
                    }

                    public QuickFix.LegSecurityAltID getLegSecurityAltID()
                        {
                        QuickFix.LegSecurityAltID value = new QuickFix.LegSecurityAltID();
                        getField(value);

                        return value;
                    }

                    public bool isSet(QuickFix.LegSecurityAltID field) {
                        return isSetField(field);
                    }

                    public bool isSetLegSecurityAltID() {
                        return isSetField(605);
                    }

                    public void set(QuickFix.LegSecurityAltIDSource value) {
                        setField(value);
                    }

                    public QuickFix.LegSecurityAltIDSource get(
                        QuickFix.LegSecurityAltIDSource value)
                        {
                        getField(value);

                        return value;
                    }

                    public QuickFix.LegSecurityAltIDSource getLegSecurityAltIDSource()
                        {
                        QuickFix.LegSecurityAltIDSource value = new QuickFix.LegSecurityAltIDSource();
                        getField(value);

                        return value;
                    }

                    public bool isSet(
                        QuickFix.LegSecurityAltIDSource field) {
                        return isSetField(field);
                    }

                    public bool isSetLegSecurityAltIDSource() {
                        return isSetField(606);
                    }
                }
            }
        }
    }
}
