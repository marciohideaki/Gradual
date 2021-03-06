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

namespace Gradual.GeradorBasePontual.intra.gradual.financial {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="PosicaoCotistaWSSoap", Namespace="http://tempuri.org/")]
    public partial class PosicaoCotistaWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private ValidateLogin validateLoginValueField;
        
        private System.Threading.SendOrPostCallback ExportaOperationCompleted;
        
        private System.Threading.SendOrPostCallback ExportaPorDataAplicacaoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PosicaoCotistaWS() {
            this.Url = global::Gradual.GeradorBasePontual.Properties.Settings.Default.Gradual_GeradorBasePontual_intra_gradual_financial_PosicaoCotistaWS;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public ValidateLogin ValidateLoginValue {
            get {
                return this.validateLoginValueField;
            }
            set {
                this.validateLoginValueField = value;
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
        public event ExportaCompletedEventHandler ExportaCompleted;
        
        /// <remarks/>
        public event ExportaPorDataAplicacaoCompletedEventHandler ExportaPorDataAplicacaoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ValidateLoginValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Exporta", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public PosicaoCotistaViewModel[] Exporta([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> IdPosicao, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> IdCotista, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> IdCarteira) {
            object[] results = this.Invoke("Exporta", new object[] {
                        IdPosicao,
                        IdCotista,
                        IdCarteira});
            return ((PosicaoCotistaViewModel[])(results[0]));
        }
        
        /// <remarks/>
        public void ExportaAsync(System.Nullable<int> IdPosicao, System.Nullable<int> IdCotista, System.Nullable<int> IdCarteira) {
            this.ExportaAsync(IdPosicao, IdCotista, IdCarteira, null);
        }
        
        /// <remarks/>
        public void ExportaAsync(System.Nullable<int> IdPosicao, System.Nullable<int> IdCotista, System.Nullable<int> IdCarteira, object userState) {
            if ((this.ExportaOperationCompleted == null)) {
                this.ExportaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExportaOperationCompleted);
            }
            this.InvokeAsync("Exporta", new object[] {
                        IdPosicao,
                        IdCotista,
                        IdCarteira}, this.ExportaOperationCompleted, userState);
        }
        
        private void OnExportaOperationCompleted(object arg) {
            if ((this.ExportaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExportaCompleted(this, new ExportaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ValidateLoginValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ExportaPorDataAplicacao", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public PosicaoCotistaViewModel[] ExportaPorDataAplicacao(System.DateTime DataInicial, System.DateTime DataFinal) {
            object[] results = this.Invoke("ExportaPorDataAplicacao", new object[] {
                        DataInicial,
                        DataFinal});
            return ((PosicaoCotistaViewModel[])(results[0]));
        }
        
        /// <remarks/>
        public void ExportaPorDataAplicacaoAsync(System.DateTime DataInicial, System.DateTime DataFinal) {
            this.ExportaPorDataAplicacaoAsync(DataInicial, DataFinal, null);
        }
        
        /// <remarks/>
        public void ExportaPorDataAplicacaoAsync(System.DateTime DataInicial, System.DateTime DataFinal, object userState) {
            if ((this.ExportaPorDataAplicacaoOperationCompleted == null)) {
                this.ExportaPorDataAplicacaoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExportaPorDataAplicacaoOperationCompleted);
            }
            this.InvokeAsync("ExportaPorDataAplicacao", new object[] {
                        DataInicial,
                        DataFinal}, this.ExportaPorDataAplicacaoOperationCompleted, userState);
        }
        
        private void OnExportaPorDataAplicacaoOperationCompleted(object arg) {
            if ((this.ExportaPorDataAplicacaoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExportaPorDataAplicacaoCompleted(this, new ExportaPorDataAplicacaoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.2022")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/", IsNullable=false)]
    public partial class ValidateLogin : System.Web.Services.Protocols.SoapHeader {
        
        private string usernameField;
        
        private string passwordField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        public string Username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.2022")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class PosicaoCotistaViewModel {
        
        private int idPosicaoField;
        
        private System.Nullable<int> idOperacaoField;
        
        private int idCotistaField;
        
        private int idCarteiraField;
        
        private decimal valorAplicacaoField;
        
        private System.DateTime dataAplicacaoField;
        
        private System.DateTime dataConversaoField;
        
        private decimal cotaAplicacaoField;
        
        private decimal cotaDiaField;
        
        private decimal valorBrutoField;
        
        private decimal valorLiquidoField;
        
        private decimal quantidadeInicialField;
        
        private decimal quantidadeField;
        
        private decimal quantidadeBloqueadaField;
        
        private System.DateTime dataUltimaCobrancaIRField;
        
        private decimal valorIRField;
        
        private decimal valorIOFField;
        
        private decimal valorPerformanceField;
        
        private decimal valorIOFVirtualField;
        
        private decimal quantidadeAntesCortesField;
        
        private decimal valorRendimentoField;
        
        private System.Nullable<System.DateTime> dataUltimoCortePfeeField;
        
        private string posicaoIncorporadaField;
        
        private string codigoAnbimaField;
        
        /// <remarks/>
        public int IdPosicao {
            get {
                return this.idPosicaoField;
            }
            set {
                this.idPosicaoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> IdOperacao {
            get {
                return this.idOperacaoField;
            }
            set {
                this.idOperacaoField = value;
            }
        }
        
        /// <remarks/>
        public int IdCotista {
            get {
                return this.idCotistaField;
            }
            set {
                this.idCotistaField = value;
            }
        }
        
        /// <remarks/>
        public int IdCarteira {
            get {
                return this.idCarteiraField;
            }
            set {
                this.idCarteiraField = value;
            }
        }
        
        /// <remarks/>
        public decimal ValorAplicacao {
            get {
                return this.valorAplicacaoField;
            }
            set {
                this.valorAplicacaoField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime DataAplicacao {
            get {
                return this.dataAplicacaoField;
            }
            set {
                this.dataAplicacaoField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime DataConversao {
            get {
                return this.dataConversaoField;
            }
            set {
                this.dataConversaoField = value;
            }
        }
        
        /// <remarks/>
        public decimal CotaAplicacao {
            get {
                return this.cotaAplicacaoField;
            }
            set {
                this.cotaAplicacaoField = value;
            }
        }
        
        /// <remarks/>
        public decimal CotaDia {
            get {
                return this.cotaDiaField;
            }
            set {
                this.cotaDiaField = value;
            }
        }
        
        /// <remarks/>
        public decimal ValorBruto {
            get {
                return this.valorBrutoField;
            }
            set {
                this.valorBrutoField = value;
            }
        }
        
        /// <remarks/>
        public decimal ValorLiquido {
            get {
                return this.valorLiquidoField;
            }
            set {
                this.valorLiquidoField = value;
            }
        }
        
        /// <remarks/>
        public decimal QuantidadeInicial {
            get {
                return this.quantidadeInicialField;
            }
            set {
                this.quantidadeInicialField = value;
            }
        }
        
        /// <remarks/>
        public decimal Quantidade {
            get {
                return this.quantidadeField;
            }
            set {
                this.quantidadeField = value;
            }
        }
        
        /// <remarks/>
        public decimal QuantidadeBloqueada {
            get {
                return this.quantidadeBloqueadaField;
            }
            set {
                this.quantidadeBloqueadaField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime DataUltimaCobrancaIR {
            get {
                return this.dataUltimaCobrancaIRField;
            }
            set {
                this.dataUltimaCobrancaIRField = value;
            }
        }
        
        /// <remarks/>
        public decimal ValorIR {
            get {
                return this.valorIRField;
            }
            set {
                this.valorIRField = value;
            }
        }
        
        /// <remarks/>
        public decimal ValorIOF {
            get {
                return this.valorIOFField;
            }
            set {
                this.valorIOFField = value;
            }
        }
        
        /// <remarks/>
        public decimal ValorPerformance {
            get {
                return this.valorPerformanceField;
            }
            set {
                this.valorPerformanceField = value;
            }
        }
        
        /// <remarks/>
        public decimal ValorIOFVirtual {
            get {
                return this.valorIOFVirtualField;
            }
            set {
                this.valorIOFVirtualField = value;
            }
        }
        
        /// <remarks/>
        public decimal QuantidadeAntesCortes {
            get {
                return this.quantidadeAntesCortesField;
            }
            set {
                this.quantidadeAntesCortesField = value;
            }
        }
        
        /// <remarks/>
        public decimal ValorRendimento {
            get {
                return this.valorRendimentoField;
            }
            set {
                this.valorRendimentoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> DataUltimoCortePfee {
            get {
                return this.dataUltimoCortePfeeField;
            }
            set {
                this.dataUltimoCortePfeeField = value;
            }
        }
        
        /// <remarks/>
        public string PosicaoIncorporada {
            get {
                return this.posicaoIncorporadaField;
            }
            set {
                this.posicaoIncorporadaField = value;
            }
        }
        
        /// <remarks/>
        public string CodigoAnbima {
            get {
                return this.codigoAnbimaField;
            }
            set {
                this.codigoAnbimaField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ExportaCompletedEventHandler(object sender, ExportaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExportaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ExportaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public PosicaoCotistaViewModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((PosicaoCotistaViewModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ExportaPorDataAplicacaoCompletedEventHandler(object sender, ExportaPorDataAplicacaoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExportaPorDataAplicacaoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ExportaPorDataAplicacaoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public PosicaoCotistaViewModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((PosicaoCotistaViewModel[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591