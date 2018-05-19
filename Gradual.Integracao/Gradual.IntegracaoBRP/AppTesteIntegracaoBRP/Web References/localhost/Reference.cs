﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.2034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.2034.
// 
#pragma warning disable 1591

namespace AppTesteIntegracaoBRP.localhost {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="InativacaoClientesWSSoap", Namespace="http://tempuri.org/")]
    public partial class InativacaoClientesWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetInactiveAccountsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public InativacaoClientesWS() {
            this.Url = global::AppTesteIntegracaoBRP.Properties.Settings.Default.AppTesteIntegracaoBRP_localhost_InativacaoClientesWS;
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
        public event GetInactiveAccountsCompletedEventHandler GetInactiveAccountsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Gradual.IntegracaoBRP/InativacaoClientes/GetInactiveAccounts", RequestNamespace="http://tempuri.org/Gradual.IntegracaoBRP/InativacaoClientes", ResponseNamespace="http://tempuri.org/Gradual.IntegracaoBRP/InativacaoClientes", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet GetInactiveAccounts(string Cd_Usuario, string Ds_Senha) {
            object[] results = this.Invoke("GetInactiveAccounts", new object[] {
                        Cd_Usuario,
                        Ds_Senha});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void GetInactiveAccountsAsync(string Cd_Usuario, string Ds_Senha) {
            this.GetInactiveAccountsAsync(Cd_Usuario, Ds_Senha, null);
        }
        
        /// <remarks/>
        public void GetInactiveAccountsAsync(string Cd_Usuario, string Ds_Senha, object userState) {
            if ((this.GetInactiveAccountsOperationCompleted == null)) {
                this.GetInactiveAccountsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetInactiveAccountsOperationCompleted);
            }
            this.InvokeAsync("GetInactiveAccounts", new object[] {
                        Cd_Usuario,
                        Ds_Senha}, this.GetInactiveAccountsOperationCompleted, userState);
        }
        
        private void OnGetInactiveAccountsOperationCompleted(object arg) {
            if ((this.GetInactiveAccountsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetInactiveAccountsCompleted(this, new GetInactiveAccountsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void GetInactiveAccountsCompletedEventHandler(object sender, GetInactiveAccountsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetInactiveAccountsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetInactiveAccountsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591