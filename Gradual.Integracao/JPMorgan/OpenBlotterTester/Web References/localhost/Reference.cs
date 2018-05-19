﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.1.
// 
#pragma warning disable 1591

namespace OpenBlotterTester.localhost {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSTradeInterfaceSoap", Namespace="http://tempuri.org/AttivoTradeInterface/WSTradeInterface")]
    public partial class WSTradeInterface : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback QueryTradesStrOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSTradeInterface() {
            this.Url = global::OpenBlotterTester.Properties.Settings.Default.OpenBlotterTester_localhost_WSTradeInterface;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event QueryTradesStrCompletedEventHandler QueryTradesStrCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AttivoTradeInterface/WSTradeInterface/QueryTradesStr", RequestNamespace="http://tempuri.org/AttivoTradeInterface/WSTradeInterface", ResponseNamespace="http://tempuri.org/AttivoTradeInterface/WSTradeInterface", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string QueryTradesStr(string pUser, string pPassword, int pInstitutionID, System.DateTime pInitialDate, System.DateTime pFinalDate, ref System.DateTime pTradeID, string pTradeStatus, string pProduct, string pAfterHour, string pTraderID) {
            object[] results = this.Invoke("QueryTradesStr", new object[] {
                        pUser,
                        pPassword,
                        pInstitutionID,
                        pInitialDate,
                        pFinalDate,
                        pTradeID,
                        pTradeStatus,
                        pProduct,
                        pAfterHour,
                        pTraderID});
            pTradeID = ((System.DateTime)(results[1]));
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void QueryTradesStrAsync(string pUser, string pPassword, int pInstitutionID, System.DateTime pInitialDate, System.DateTime pFinalDate, System.DateTime pTradeID, string pTradeStatus, string pProduct, string pAfterHour, string pTraderID) {
            this.QueryTradesStrAsync(pUser, pPassword, pInstitutionID, pInitialDate, pFinalDate, pTradeID, pTradeStatus, pProduct, pAfterHour, pTraderID, null);
        }
        
        /// <remarks/>
        public void QueryTradesStrAsync(string pUser, string pPassword, int pInstitutionID, System.DateTime pInitialDate, System.DateTime pFinalDate, System.DateTime pTradeID, string pTradeStatus, string pProduct, string pAfterHour, string pTraderID, object userState) {
            if ((this.QueryTradesStrOperationCompleted == null)) {
                this.QueryTradesStrOperationCompleted = new System.Threading.SendOrPostCallback(this.OnQueryTradesStrOperationCompleted);
            }
            this.InvokeAsync("QueryTradesStr", new object[] {
                        pUser,
                        pPassword,
                        pInstitutionID,
                        pInitialDate,
                        pFinalDate,
                        pTradeID,
                        pTradeStatus,
                        pProduct,
                        pAfterHour,
                        pTraderID}, this.QueryTradesStrOperationCompleted, userState);
        }
        
        private void OnQueryTradesStrOperationCompleted(object arg) {
            if ((this.QueryTradesStrCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.QueryTradesStrCompleted(this, new QueryTradesStrCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void QueryTradesStrCompletedEventHandler(object sender, QueryTradesStrCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class QueryTradesStrCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal QueryTradesStrCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public System.DateTime pTradeID {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.DateTime)(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591