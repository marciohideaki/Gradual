using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.MarketData
{
    class NoUnderlyings
    {
        public NoUnderlyings() : base(711, 311,
                new int[] {
                    311, 312, 309, 305, 457, 462, 463, 310, 763, 313, 542,
                    241, 242, 243, 244, 245, 246, 256, 595, 592, 593, 594,
                    247, 316, 941, 317, 436, 435, 308, 306, 362, 363, 307,
                    364, 365, 877, 878, 318, 879, 810, 882, 883, 884, 885,
                    886, 887, 0
                }){}

        public void set(
            QuickFix..fix44.component.UnderlyingInstrument component) {
            setComponent(component);
        }

        public QuickFix.fix44.component.UnderlyingInstrument get(
            QuickFix.fix44.component.UnderlyingInstrument component)
            {
            getComponent(component);

            return component;
        }

        public QuickFix.fix44.component.UnderlyingInstrument getUnderlyingInstrument()
            {
            QuickFix.fix44.component.UnderlyingInstrument component = new QuickFix.fix44.component.UnderlyingInstrument();
            getComponent(component);

            return component;
        }

        public void set(QuickFix.UnderlyingSymbol value) {
            setField(value);
        }

        public QuickFix.UnderlyingSymbol get(
            QuickFix.UnderlyingSymbol value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingSymbol getUnderlyingSymbol()
            {
            QuickFix.UnderlyingSymbol value = new QuickFix.UnderlyingSymbol();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingSymbol field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingSymbol() {
            return isSetField(311);
        }

        public void set(QuickFix.UnderlyingSymbolSfx value) {
            setField(value);
        }

        public QuickFix.UnderlyingSymbolSfx get(
            QuickFix.UnderlyingSymbolSfx value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingSymbolSfx getUnderlyingSymbolSfx()
            {
            QuickFix.UnderlyingSymbolSfx value = new QuickFix.UnderlyingSymbolSfx();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingSymbolSfx field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingSymbolSfx() {
            return isSetField(312);
        }

        public void set(QuickFix.UnderlyingSecurityID value) {
            setField(value);
        }

        public QuickFix.UnderlyingSecurityID get(
            QuickFix.UnderlyingSecurityID value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingSecurityID getUnderlyingSecurityID()
            {
            QuickFix.UnderlyingSecurityID value = new QuickFix.UnderlyingSecurityID();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingSecurityID field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingSecurityID() {
            return isSetField(309);
        }

        public void set(QuickFix.UnderlyingSecurityIDSource value) {
            setField(value);
        }

        public QuickFix.UnderlyingSecurityIDSource get(
            QuickFix.UnderlyingSecurityIDSource value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingSecurityIDSource getUnderlyingSecurityIDSource()
            {
            QuickFix.UnderlyingSecurityIDSource value = new QuickFix.UnderlyingSecurityIDSource();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.UnderlyingSecurityIDSource field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingSecurityIDSource() {
            return isSetField(305);
        }

        public void set(QuickFix.NoUnderlyingSecurityAltID value) {
            setField(value);
        }

        public QuickFix.NoUnderlyingSecurityAltID get(
            QuickFix.NoUnderlyingSecurityAltID value)
            {
            getField(value);

            return value;
        }

        public QuickFix.NoUnderlyingSecurityAltID getNoUnderlyingSecurityAltID()
            {
            QuickFix.NoUnderlyingSecurityAltID value = new QuickFix.NoUnderlyingSecurityAltID();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.NoUnderlyingSecurityAltID field) {
            return isSetField(field);
        }

        public bool isSetNoUnderlyingSecurityAltID() {
            return isSetField(457);
        }

        public void set(QuickFix.UnderlyingProduct value) {
            setField(value);
        }

        public QuickFix.UnderlyingProduct get(
            QuickFix.UnderlyingProduct value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingProduct getUnderlyingProduct()
            {
            QuickFix.UnderlyingProduct value = new QuickFix.UnderlyingProduct();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingProduct field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingProduct() {
            return isSetField(462);
        }

        public void set(QuickFix.UnderlyingCFICode value) {
            setField(value);
        }

        public QuickFix.UnderlyingCFICode get(
            QuickFix.UnderlyingCFICode value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingCFICode getUnderlyingCFICode()
            {
            QuickFix.UnderlyingCFICode value = new QuickFix.UnderlyingCFICode();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingCFICode field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingCFICode() {
            return isSetField(463);
        }

        public void set(QuickFix.UnderlyingSecurityType value) {
            setField(value);
        }

        public QuickFix.UnderlyingSecurityType get(
            QuickFix.UnderlyingSecurityType value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingSecurityType getUnderlyingSecurityType()
            {
            QuickFix.UnderlyingSecurityType value = new QuickFix.UnderlyingSecurityType();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingSecurityType field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingSecurityType() {
            return isSetField(310);
        }

        public void set(QuickFix.UnderlyingSecuritySubType value) {
            setField(value);
        }

        public QuickFix.UnderlyingSecuritySubType get(
            QuickFix.UnderlyingSecuritySubType value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingSecuritySubType getUnderlyingSecuritySubType()
            {
            QuickFix.UnderlyingSecuritySubType value = new QuickFix.UnderlyingSecuritySubType();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingSecuritySubType field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingSecuritySubType() {
            return isSetField(763);
        }

        public void set(QuickFix.UnderlyingMaturityMonthYear value) {
            setField(value);
        }

        public QuickFix.UnderlyingMaturityMonthYear get(
            QuickFix.UnderlyingMaturityMonthYear value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingMaturityMonthYear getUnderlyingMaturityMonthYear()
            {
            QuickFix.UnderlyingMaturityMonthYear value = new QuickFix.UnderlyingMaturityMonthYear();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.UnderlyingMaturityMonthYear field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingMaturityMonthYear() {
            return isSetField(313);
        }

        public void set(QuickFix.UnderlyingMaturityDate value) {
            setField(value);
        }

        public QuickFix.UnderlyingMaturityDate get(
            QuickFix.UnderlyingMaturityDate value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingMaturityDate getUnderlyingMaturityDate()
            {
            QuickFix.UnderlyingMaturityDate value = new QuickFix.UnderlyingMaturityDate();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingMaturityDate field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingMaturityDate() {
            return isSetField(542);
        }

        public void set(QuickFix.UnderlyingCouponPaymentDate value) {
            setField(value);
        }

        public QuickFix.UnderlyingCouponPaymentDate get(
            QuickFix.UnderlyingCouponPaymentDate value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingCouponPaymentDate getUnderlyingCouponPaymentDate()
            {
            QuickFix.UnderlyingCouponPaymentDate value = new QuickFix.UnderlyingCouponPaymentDate();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.UnderlyingCouponPaymentDate field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingCouponPaymentDate() {
            return isSetField(241);
        }

        public void set(QuickFix.UnderlyingIssueDate value) {
            setField(value);
        }

        public QuickFix.UnderlyingIssueDate get(
            QuickFix.UnderlyingIssueDate value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingIssueDate getUnderlyingIssueDate()
            {
            QuickFix.UnderlyingIssueDate value = new QuickFix.UnderlyingIssueDate();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingIssueDate field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingIssueDate() {
            return isSetField(242);
        }

        public void set(
            QuickFix.UnderlyingRepoCollateralSecurityType value) {
            setField(value);
        }

        public QuickFix.UnderlyingRepoCollateralSecurityType get(
            QuickFix.UnderlyingRepoCollateralSecurityType value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingRepoCollateralSecurityType getUnderlyingRepoCollateralSecurityType()
            {
            QuickFix.UnderlyingRepoCollateralSecurityType value = new QuickFix.UnderlyingRepoCollateralSecurityType();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.UnderlyingRepoCollateralSecurityType field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingRepoCollateralSecurityType() {
            return isSetField(243);
        }

        public void set(QuickFix.UnderlyingRepurchaseTerm value) {
            setField(value);
        }

        public QuickFix.UnderlyingRepurchaseTerm get(
            QuickFix.UnderlyingRepurchaseTerm value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingRepurchaseTerm getUnderlyingRepurchaseTerm()
            {
            QuickFix.UnderlyingRepurchaseTerm value = new QuickFix.UnderlyingRepurchaseTerm();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingRepurchaseTerm field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingRepurchaseTerm() {
            return isSetField(244);
        }

        public void set(QuickFix.UnderlyingRepurchaseRate value) {
            setField(value);
        }

        public QuickFix.UnderlyingRepurchaseRate get(
            QuickFix.UnderlyingRepurchaseRate value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingRepurchaseRate getUnderlyingRepurchaseRate()
            {
            QuickFix.UnderlyingRepurchaseRate value = new QuickFix.UnderlyingRepurchaseRate();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingRepurchaseRate field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingRepurchaseRate() {
            return isSetField(245);
        }

        public void set(QuickFix.UnderlyingFactor value) {
            setField(value);
        }

        public QuickFix.UnderlyingFactor get(
            QuickFix.UnderlyingFactor value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingFactor getUnderlyingFactor()
            {
            QuickFix.UnderlyingFactor value = new QuickFix.UnderlyingFactor();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingFactor field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingFactor() {
            return isSetField(246);
        }

        public void set(QuickFix.UnderlyingCreditRating value) {
            setField(value);
        }

        public QuickFix.UnderlyingCreditRating get(
            QuickFix.UnderlyingCreditRating value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingCreditRating getUnderlyingCreditRating()
            {
            QuickFix.UnderlyingCreditRating value = new QuickFix.UnderlyingCreditRating();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingCreditRating field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingCreditRating() {
            return isSetField(256);
        }

        public void set(QuickFix.UnderlyingInstrRegistry value) {
            setField(value);
        }

        public QuickFix.UnderlyingInstrRegistry get(
            QuickFix.UnderlyingInstrRegistry value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingInstrRegistry getUnderlyingInstrRegistry()
            {
            QuickFix.UnderlyingInstrRegistry value = new QuickFix.UnderlyingInstrRegistry();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingInstrRegistry field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingInstrRegistry() {
            return isSetField(595);
        }

        public void set(QuickFix.UnderlyingCountryOfIssue value) {
            setField(value);
        }

        public QuickFix.UnderlyingCountryOfIssue get(
            QuickFix.UnderlyingCountryOfIssue value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingCountryOfIssue getUnderlyingCountryOfIssue()
            {
            QuickFix.UnderlyingCountryOfIssue value = new QuickFix.UnderlyingCountryOfIssue();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingCountryOfIssue field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingCountryOfIssue() {
            return isSetField(592);
        }

        public void set(
            QuickFix.UnderlyingStateOrProvinceOfIssue value) {
            setField(value);
        }

        public QuickFix.UnderlyingStateOrProvinceOfIssue get(
            QuickFix.UnderlyingStateOrProvinceOfIssue value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingStateOrProvinceOfIssue getUnderlyingStateOrProvinceOfIssue()
            {
            QuickFix.UnderlyingStateOrProvinceOfIssue value = new QuickFix.UnderlyingStateOrProvinceOfIssue();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.UnderlyingStateOrProvinceOfIssue field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingStateOrProvinceOfIssue() {
            return isSetField(593);
        }

        public void set(QuickFix.UnderlyingLocaleOfIssue value) {
            setField(value);
        }

        public QuickFix.UnderlyingLocaleOfIssue get(
            QuickFix.UnderlyingLocaleOfIssue value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingLocaleOfIssue getUnderlyingLocaleOfIssue()
            {
            QuickFix.UnderlyingLocaleOfIssue value = new QuickFix.UnderlyingLocaleOfIssue();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingLocaleOfIssue field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingLocaleOfIssue() {
            return isSetField(594);
        }

        public void set(QuickFix.UnderlyingRedemptionDate value) {
            setField(value);
        }

        public QuickFix.UnderlyingRedemptionDate get(
            QuickFix.UnderlyingRedemptionDate value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingRedemptionDate getUnderlyingRedemptionDate()
            {
            QuickFix.UnderlyingRedemptionDate value = new QuickFix.UnderlyingRedemptionDate();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingRedemptionDate field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingRedemptionDate() {
            return isSetField(247);
        }

        public void set(QuickFix.UnderlyingStrikePrice value) {
            setField(value);
        }

        public QuickFix.UnderlyingStrikePrice get(
            QuickFix.UnderlyingStrikePrice value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingStrikePrice getUnderlyingStrikePrice()
            {
            QuickFix.UnderlyingStrikePrice value = new QuickFix.UnderlyingStrikePrice();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingStrikePrice field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingStrikePrice() {
            return isSetField(316);
        }

        public void set(QuickFix.UnderlyingStrikeCurrency value) {
            setField(value);
        }

        public QuickFix.UnderlyingStrikeCurrency get(
            QuickFix.UnderlyingStrikeCurrency value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingStrikeCurrency getUnderlyingStrikeCurrency()
            {
            QuickFix.UnderlyingStrikeCurrency value = new QuickFix.UnderlyingStrikeCurrency();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingStrikeCurrency field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingStrikeCurrency() {
            return isSetField(941);
        }

        public void set(QuickFix.UnderlyingOptAttribute value) {
            setField(value);
        }

        public QuickFix.UnderlyingOptAttribute get(
            QuickFix.UnderlyingOptAttribute value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingOptAttribute getUnderlyingOptAttribute()
            {
            QuickFix.UnderlyingOptAttribute value = new QuickFix.UnderlyingOptAttribute();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingOptAttribute field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingOptAttribute() {
            return isSetField(317);
        }

        public void set(QuickFix.UnderlyingContractMultiplier value) {
            setField(value);
        }

        public QuickFix.UnderlyingContractMultiplier get(
            QuickFix.UnderlyingContractMultiplier value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingContractMultiplier getUnderlyingContractMultiplier()
            {
            QuickFix.UnderlyingContractMultiplier value = new QuickFix.UnderlyingContractMultiplier();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.UnderlyingContractMultiplier field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingContractMultiplier() {
            return isSetField(436);
        }

        public void set(QuickFix.UnderlyingCouponRate value) {
            setField(value);
        }

        public QuickFix.UnderlyingCouponRate get(
            QuickFix.UnderlyingCouponRate value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingCouponRate getUnderlyingCouponRate()
            {
            QuickFix.UnderlyingCouponRate value = new QuickFix.UnderlyingCouponRate();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingCouponRate field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingCouponRate() {
            return isSetField(435);
        }

        public void set(QuickFix.UnderlyingSecurityExchange value) {
            setField(value);
        }

        public QuickFix.UnderlyingSecurityExchange get(
            QuickFix.UnderlyingSecurityExchange value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingSecurityExchange getUnderlyingSecurityExchange()
            {
            QuickFix.UnderlyingSecurityExchange value = new QuickFix.UnderlyingSecurityExchange();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.UnderlyingSecurityExchange field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingSecurityExchange() {
            return isSetField(308);
        }

        public void set(QuickFix.UnderlyingIssuer value) {
            setField(value);
        }

        public QuickFix.UnderlyingIssuer get(
            QuickFix.UnderlyingIssuer value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingIssuer getUnderlyingIssuer()
            {
            QuickFix.UnderlyingIssuer value = new QuickFix.UnderlyingIssuer();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingIssuer field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingIssuer() {
            return isSetField(306);
        }

        public void set(QuickFix.EncodedUnderlyingIssuerLen value) {
            setField(value);
        }

        public QuickFix.EncodedUnderlyingIssuerLen get(
            QuickFix.EncodedUnderlyingIssuerLen value)
            {
            getField(value);

            return value;
        }

        public QuickFix.EncodedUnderlyingIssuerLen getEncodedUnderlyingIssuerLen()
            {
            QuickFix.EncodedUnderlyingIssuerLen value = new QuickFix.EncodedUnderlyingIssuerLen();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.EncodedUnderlyingIssuerLen field) {
            return isSetField(field);
        }

        public bool isSetEncodedUnderlyingIssuerLen() {
            return isSetField(362);
        }

        public void set(QuickFix.EncodedUnderlyingIssuer value) {
            setField(value);
        }

        public QuickFix.EncodedUnderlyingIssuer get(
            QuickFix.EncodedUnderlyingIssuer value)
            {
            getField(value);

            return value;
        }

        public QuickFix.EncodedUnderlyingIssuer getEncodedUnderlyingIssuer()
            {
            QuickFix.EncodedUnderlyingIssuer value = new QuickFix.EncodedUnderlyingIssuer();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.EncodedUnderlyingIssuer field) {
            return isSetField(field);
        }

        public bool isSetEncodedUnderlyingIssuer() {
            return isSetField(363);
        }

        public void set(QuickFix.UnderlyingSecurityDesc value) {
            setField(value);
        }

        public QuickFix.UnderlyingSecurityDesc get(
            QuickFix.UnderlyingSecurityDesc value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingSecurityDesc getUnderlyingSecurityDesc()
            {
            QuickFix.UnderlyingSecurityDesc value = new QuickFix.UnderlyingSecurityDesc();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingSecurityDesc field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingSecurityDesc() {
            return isSetField(307);
        }

        public void set(
            QuickFix.EncodedUnderlyingSecurityDescLen value) {
            setField(value);
        }

        public QuickFix.EncodedUnderlyingSecurityDescLen get(
            QuickFix.EncodedUnderlyingSecurityDescLen value)
            {
            getField(value);

            return value;
        }

        public QuickFix.EncodedUnderlyingSecurityDescLen getEncodedUnderlyingSecurityDescLen()
            {
            QuickFix.EncodedUnderlyingSecurityDescLen value = new QuickFix.EncodedUnderlyingSecurityDescLen();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.EncodedUnderlyingSecurityDescLen field) {
            return isSetField(field);
        }

        public bool isSetEncodedUnderlyingSecurityDescLen() {
            return isSetField(364);
        }

        public void set(QuickFix.EncodedUnderlyingSecurityDesc value) {
            setField(value);
        }

        public QuickFix.EncodedUnderlyingSecurityDesc get(
            QuickFix.EncodedUnderlyingSecurityDesc value)
            {
            getField(value);

            return value;
        }

        public QuickFix.EncodedUnderlyingSecurityDesc getEncodedUnderlyingSecurityDesc()
            {
            QuickFix.EncodedUnderlyingSecurityDesc value = new QuickFix.EncodedUnderlyingSecurityDesc();
            getField(value);

            return value;
        }

        public bool isSet(
            QuickFix.EncodedUnderlyingSecurityDesc field) {
            return isSetField(field);
        }

        public bool isSetEncodedUnderlyingSecurityDesc() {
            return isSetField(365);
        }

        public void set(QuickFix.UnderlyingCPProgram value) {
            setField(value);
        }

        public QuickFix.UnderlyingCPProgram get(
            QuickFix.UnderlyingCPProgram value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingCPProgram getUnderlyingCPProgram()
            {
            QuickFix.UnderlyingCPProgram value = new QuickFix.UnderlyingCPProgram();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingCPProgram field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingCPProgram() {
            return isSetField(877);
        }

        public void set(QuickFix.UnderlyingCPRegType value) {
            setField(value);
        }

        public QuickFix.UnderlyingCPRegType get(
            QuickFix.UnderlyingCPRegType value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingCPRegType getUnderlyingCPRegType()
            {
            QuickFix.UnderlyingCPRegType value = new QuickFix.UnderlyingCPRegType();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingCPRegType field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingCPRegType() {
            return isSetField(878);
        }

        public void set(QuickFix.UnderlyingCurrency value) {
            setField(value);
        }

        public QuickFix.UnderlyingCurrency get(
            QuickFix.UnderlyingCurrency value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingCurrency getUnderlyingCurrency()
            {
            QuickFix.UnderlyingCurrency value = new QuickFix.UnderlyingCurrency();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingCurrency field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingCurrency() {
            return isSetField(318);
        }

        public void set(QuickFix.UnderlyingQty value) {
            setField(value);
        }

        public QuickFix.UnderlyingQty get(
            QuickFix.UnderlyingQty value) {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingQty getUnderlyingQty()
            {
            QuickFix.UnderlyingQty value = new QuickFix.UnderlyingQty();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingQty field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingQty() {
            return isSetField(879);
        }

        public void set(QuickFix.UnderlyingPx value) {
            setField(value);
        }

        public QuickFix.UnderlyingPx get(
            QuickFix.UnderlyingPx value) {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingPx getUnderlyingPx()
            {
            QuickFix.UnderlyingPx value = new QuickFix.UnderlyingPx();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingPx field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingPx() {
            return isSetField(810);
        }

        public void set(QuickFix.UnderlyingDirtyPrice value) {
            setField(value);
        }

        public QuickFix.UnderlyingDirtyPrice get(
            QuickFix.UnderlyingDirtyPrice value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingDirtyPrice getUnderlyingDirtyPrice()
            {
            QuickFix.UnderlyingDirtyPrice value = new QuickFix.UnderlyingDirtyPrice();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingDirtyPrice field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingDirtyPrice() {
            return isSetField(882);
        }

        public void set(QuickFix.UnderlyingEndPrice value) {
            setField(value);
        }

        public QuickFix.UnderlyingEndPrice get(
            QuickFix.UnderlyingEndPrice value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingEndPrice getUnderlyingEndPrice()
            {
            QuickFix.UnderlyingEndPrice value = new QuickFix.UnderlyingEndPrice();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingEndPrice field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingEndPrice() {
            return isSetField(883);
        }

        public void set(QuickFix.UnderlyingStartValue value) {
            setField(value);
        }

        public QuickFix.UnderlyingStartValue get(
            QuickFix.UnderlyingStartValue value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingStartValue getUnderlyingStartValue()
            {
            QuickFix.UnderlyingStartValue value = new QuickFix.UnderlyingStartValue();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingStartValue field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingStartValue() {
            return isSetField(884);
        }

        public void set(QuickFix.UnderlyingCurrentValue value) {
            setField(value);
        }

        public QuickFix.UnderlyingCurrentValue get(
            QuickFix.UnderlyingCurrentValue value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingCurrentValue getUnderlyingCurrentValue()
            {
            QuickFix.UnderlyingCurrentValue value = new QuickFix.UnderlyingCurrentValue();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingCurrentValue field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingCurrentValue() {
            return isSetField(885);
        }

        public void set(QuickFix.UnderlyingEndValue value) {
            setField(value);
        }

        public QuickFix.UnderlyingEndValue get(
            QuickFix.UnderlyingEndValue value)
            {
            getField(value);

            return value;
        }

        public QuickFix.UnderlyingEndValue getUnderlyingEndValue()
            {
            QuickFix.UnderlyingEndValue value = new QuickFix.UnderlyingEndValue();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.UnderlyingEndValue field) {
            return isSetField(field);
        }

        public bool isSetUnderlyingEndValue() {
            return isSetField(886);
        }

        public void set(
            QuickFix.fix44.component.UnderlyingStipulations component) {
            setComponent(component);
        }

        public QuickFix.fix44.component.UnderlyingStipulations get(
            QuickFix.fix44.component.UnderlyingStipulations component)
            {
            getComponent(component);

            return component;
        }

        public QuickFix.fix44.component.UnderlyingStipulations getUnderlyingStipulations()
            {
            QuickFix.fix44.component.UnderlyingStipulations component = new QuickFix.fix44.component.UnderlyingStipulations();
            getComponent(component);

            return component;
        }

        public void set(QuickFix.NoUnderlyingStips value) {
            setField(value);
        }

        public QuickFix.NoUnderlyingStips get(
            QuickFix.NoUnderlyingStips value)
            {
            getField(value);

            return value;
        }

        public QuickFix.NoUnderlyingStips getNoUnderlyingStips()
            {
            QuickFix.NoUnderlyingStips value = new QuickFix.NoUnderlyingStips();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.NoUnderlyingStips field) {
            return isSetField(field);
        }

        public bool isSetNoUnderlyingStips() {
            return isSetField(887);
        }

        public static class NoUnderlyingSecurityAltID extends Group {
            static final long serialVersionUID = 20050617;

            public NoUnderlyingSecurityAltID() {
                super(457, 458, new int[] { 458, 459, 0 });
            }

            public void set(QuickFix.UnderlyingSecurityAltID value) {
                setField(value);
            }

            public QuickFix.UnderlyingSecurityAltID get(
                QuickFix.UnderlyingSecurityAltID value)
                {
                getField(value);

                return value;
            }

            public QuickFix.UnderlyingSecurityAltID getUnderlyingSecurityAltID()
                {
                QuickFix.UnderlyingSecurityAltID value = new QuickFix.UnderlyingSecurityAltID();
                getField(value);

                return value;
            }

            public bool isSet(
                QuickFix.UnderlyingSecurityAltID field) {
                return isSetField(field);
            }

            public bool isSetUnderlyingSecurityAltID() {
                return isSetField(458);
            }

            public void set(
                QuickFix.UnderlyingSecurityAltIDSource value) {
                setField(value);
            }

            public QuickFix.UnderlyingSecurityAltIDSource get(
                QuickFix.UnderlyingSecurityAltIDSource value)
                {
                getField(value);

                return value;
            }

            public QuickFix.UnderlyingSecurityAltIDSource getUnderlyingSecurityAltIDSource()
                {
                QuickFix.UnderlyingSecurityAltIDSource value = new QuickFix.UnderlyingSecurityAltIDSource();
                getField(value);

                return value;
            }

            public bool isSet(
                QuickFix.UnderlyingSecurityAltIDSource field) {
                return isSetField(field);
            }

            public bool isSetUnderlyingSecurityAltIDSource() {
                return isSetField(459);
            }
        }

        public static class NoUnderlyingStips extends Group {
            static final long serialVersionUID = 20050617;

            public NoUnderlyingStips() {
                super(887, 888, new int[] { 888, 889, 0 });
            }

            public void set(QuickFix.UnderlyingStipType value) {
                setField(value);
            }

            public QuickFix.UnderlyingStipType get(
                QuickFix.UnderlyingStipType value)
                {
                getField(value);

                return value;
            }

            public QuickFix.UnderlyingStipType getUnderlyingStipType()
                {
                QuickFix.UnderlyingStipType value = new QuickFix.UnderlyingStipType();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.UnderlyingStipType field) {
                return isSetField(field);
            }

            public bool isSetUnderlyingStipType() {
                return isSetField(888);
            }

            public void set(QuickFix.UnderlyingStipValue value) {
                setField(value);
            }

            public QuickFix.UnderlyingStipValue get(
                QuickFix.UnderlyingStipValue value)
                {
                getField(value);

                return value;
            }

            public QuickFix.UnderlyingStipValue getUnderlyingStipValue()
                {
                QuickFix.UnderlyingStipValue value = new QuickFix.UnderlyingStipValue();
                getField(value);

                return value;
            }

            public bool isSet(QuickFix.UnderlyingStipValue field) {
                return isSetField(field);
            }

            public bool isSetUnderlyingStipValue() {
                return isSetField(889);
            }
        }
    }
}
