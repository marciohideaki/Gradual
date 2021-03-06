﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gradual.OMS.TesouroDireto.WSTDConsultas {


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
    [System.Web.Services.WebServiceBindingAttribute(Name = "consultasSoap", Namespace = "http://www.cblc.com.br/")]
    public partial class consultas : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private hdSeguranca hdSegurancaValueField;

        private System.Threading.SendOrPostCallback ConsultasConsExtratMensalOperationCompleted;

        private System.Threading.SendOrPostCallback ConsultasConsExtratoRentabilidadeOperationCompleted;

        private System.Threading.SendOrPostCallback ConsultasConsTaxaProtocoloOperationCompleted;

        private System.Threading.SendOrPostCallback ConsultasConsCestaOperationCompleted;

        private System.Threading.SendOrPostCallback ConsultasConsTipoTituloOperationCompleted;

        private System.Threading.SendOrPostCallback ConsultasConsTipoIndexadorOperationCompleted;

        private System.Threading.SendOrPostCallback ConsultasConsMercadoOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public consultas()
        {
            this.Url = global::  Gradual.OMS.TesouroDireto.Properties.Settings.Default.Gradual_OMS_TesouroDireto_WSTDConsultas_consultas;
            if ((this.IsLocalFileSystemWebService(this.Url) == true))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public hdSeguranca hdSegurancaValue
        {
            get
            {
                return this.hdSegurancaValueField;
            }
            set
            {
                this.hdSegurancaValueField = value;
            }
        }

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                            && (this.useDefaultCredentialsSetExplicitly == false))
                            && (this.IsLocalFileSystemWebService(value) == false)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        /// <remarks/>
        public event ConsultasConsExtratMensalCompletedEventHandler ConsultasConsExtratMensalCompleted;

        /// <remarks/>
        public event ConsultasConsExtratoRentabilidadeCompletedEventHandler ConsultasConsExtratoRentabilidadeCompleted;

        /// <remarks/>
        public event ConsultasConsTaxaProtocoloCompletedEventHandler ConsultasConsTaxaProtocoloCompleted;

        /// <remarks/>
        public event ConsultasConsCestaCompletedEventHandler ConsultasConsCestaCompleted;

        /// <remarks/>
        public event ConsultasConsTipoTituloCompletedEventHandler ConsultasConsTipoTituloCompleted;

        /// <remarks/>
        public event ConsultasConsTipoIndexadorCompletedEventHandler ConsultasConsTipoIndexadorCompleted;

        /// <remarks/>
        public event ConsultasConsMercadoCompletedEventHandler ConsultasConsMercadoCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("hdSegurancaValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.cblc.com.br/ConsultasConsExtratMensal", RequestNamespace = "http://www.cblc.com.br/", ResponseNamespace = "http://www.cblc.com.br/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConsultasConsExtratMensal(string negociador_cpf)
        {
            object[] results = this.Invoke("ConsultasConsExtratMensal", new object[] {
                        negociador_cpf});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void ConsultasConsExtratMensalAsync(string negociador_cpf)
        {
            this.ConsultasConsExtratMensalAsync(negociador_cpf, null);
        }

        /// <remarks/>
        public void ConsultasConsExtratMensalAsync(string negociador_cpf, object userState)
        {
            if ((this.ConsultasConsExtratMensalOperationCompleted == null))
            {
                this.ConsultasConsExtratMensalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultasConsExtratMensalOperationCompleted);
            }
            this.InvokeAsync("ConsultasConsExtratMensal", new object[] {
                        negociador_cpf}, this.ConsultasConsExtratMensalOperationCompleted, userState);
        }

        private void OnConsultasConsExtratMensalOperationCompleted(object arg)
        {
            if ((this.ConsultasConsExtratMensalCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultasConsExtratMensalCompleted(this, new ConsultasConsExtratMensalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("hdSegurancaValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.cblc.com.br/ConsultasConsExtratoRentabilidade", RequestNamespace = "http://www.cblc.com.br/", ResponseNamespace = "http://www.cblc.com.br/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConsultasConsExtratoRentabilidade(string negociador_cpf, int cd_titulo_publico)
        {
            object[] results = this.Invoke("ConsultasConsExtratoRentabilidade", new object[] {
                        negociador_cpf,
                        cd_titulo_publico});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void ConsultasConsExtratoRentabilidadeAsync(string negociador_cpf, int cd_titulo_publico)
        {
            this.ConsultasConsExtratoRentabilidadeAsync(negociador_cpf, cd_titulo_publico, null);
        }

        /// <remarks/>
        public void ConsultasConsExtratoRentabilidadeAsync(string negociador_cpf, int cd_titulo_publico, object userState)
        {
            if ((this.ConsultasConsExtratoRentabilidadeOperationCompleted == null))
            {
                this.ConsultasConsExtratoRentabilidadeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultasConsExtratoRentabilidadeOperationCompleted);
            }
            this.InvokeAsync("ConsultasConsExtratoRentabilidade", new object[] {
                        negociador_cpf,
                        cd_titulo_publico}, this.ConsultasConsExtratoRentabilidadeOperationCompleted, userState);
        }

        private void OnConsultasConsExtratoRentabilidadeOperationCompleted(object arg)
        {
            if ((this.ConsultasConsExtratoRentabilidadeCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultasConsExtratoRentabilidadeCompleted(this, new ConsultasConsExtratoRentabilidadeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("hdSegurancaValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.cblc.com.br/ConsultasConsTaxaProtocolo", RequestNamespace = "http://www.cblc.com.br/", ResponseNamespace = "http://www.cblc.com.br/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConsultasConsTaxaProtocolo(int codigo_titulo, int codigo_cesta)
        {
            object[] results = this.Invoke("ConsultasConsTaxaProtocolo", new object[] {
                        codigo_titulo,
                        codigo_cesta});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void ConsultasConsTaxaProtocoloAsync(int codigo_titulo, int codigo_cesta)
        {
            this.ConsultasConsTaxaProtocoloAsync(codigo_titulo, codigo_cesta, null);
        }

        /// <remarks/>
        public void ConsultasConsTaxaProtocoloAsync(int codigo_titulo, int codigo_cesta, object userState)
        {
            if ((this.ConsultasConsTaxaProtocoloOperationCompleted == null))
            {
                this.ConsultasConsTaxaProtocoloOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultasConsTaxaProtocoloOperationCompleted);
            }
            this.InvokeAsync("ConsultasConsTaxaProtocolo", new object[] {
                        codigo_titulo,
                        codigo_cesta}, this.ConsultasConsTaxaProtocoloOperationCompleted, userState);
        }

        private void OnConsultasConsTaxaProtocoloOperationCompleted(object arg)
        {
            if ((this.ConsultasConsTaxaProtocoloCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultasConsTaxaProtocoloCompleted(this, new ConsultasConsTaxaProtocoloCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("hdSegurancaValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.cblc.com.br/ConsultasConsCesta", RequestNamespace = "http://www.cblc.com.br/", ResponseNamespace = "http://www.cblc.com.br/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConsultasConsCesta(int mercado, string negociador_cpf, byte situacao, byte tipo, int codigo_cesta, string data_compra, int codigo_titulo, int cliente)
        {
            object[] results = this.Invoke("ConsultasConsCesta", new object[] {
                        mercado,
                        negociador_cpf,
                        situacao,
                        tipo,
                        codigo_cesta,
                        data_compra,
                        codigo_titulo,
                        cliente});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void ConsultasConsCestaAsync(int mercado, string negociador_cpf, byte situacao, byte tipo, int codigo_cesta, string data_compra, int codigo_titulo, int cliente)
        {
            this.ConsultasConsCestaAsync(mercado, negociador_cpf, situacao, tipo, codigo_cesta, data_compra, codigo_titulo, cliente, null);
        }

        /// <remarks/>
        public void ConsultasConsCestaAsync(int mercado, string negociador_cpf, byte situacao, byte tipo, int codigo_cesta, string data_compra, int codigo_titulo, int cliente, object userState)
        {
            if ((this.ConsultasConsCestaOperationCompleted == null))
            {
                this.ConsultasConsCestaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultasConsCestaOperationCompleted);
            }
            this.InvokeAsync("ConsultasConsCesta", new object[] {
                        mercado,
                        negociador_cpf,
                        situacao,
                        tipo,
                        codigo_cesta,
                        data_compra,
                        codigo_titulo,
                        cliente}, this.ConsultasConsCestaOperationCompleted, userState);
        }

        private void OnConsultasConsCestaOperationCompleted(object arg)
        {
            if ((this.ConsultasConsCestaCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultasConsCestaCompleted(this, new ConsultasConsCestaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("hdSegurancaValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.cblc.com.br/ConsultasConsTipoTitulo", RequestNamespace = "http://www.cblc.com.br/", ResponseNamespace = "http://www.cblc.com.br/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConsultasConsTipoTitulo()
        {
            object[] results = this.Invoke("ConsultasConsTipoTitulo", new object[0]);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void ConsultasConsTipoTituloAsync()
        {
            this.ConsultasConsTipoTituloAsync(null);
        }

        /// <remarks/>
        public void ConsultasConsTipoTituloAsync(object userState)
        {
            if ((this.ConsultasConsTipoTituloOperationCompleted == null))
            {
                this.ConsultasConsTipoTituloOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultasConsTipoTituloOperationCompleted);
            }
            this.InvokeAsync("ConsultasConsTipoTitulo", new object[0], this.ConsultasConsTipoTituloOperationCompleted, userState);
        }

        private void OnConsultasConsTipoTituloOperationCompleted(object arg)
        {
            if ((this.ConsultasConsTipoTituloCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultasConsTipoTituloCompleted(this, new ConsultasConsTipoTituloCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("hdSegurancaValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.cblc.com.br/ConsultasConsTipoIndexador", RequestNamespace = "http://www.cblc.com.br/", ResponseNamespace = "http://www.cblc.com.br/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConsultasConsTipoIndexador()
        {
            object[] results = this.Invoke("ConsultasConsTipoIndexador", new object[0]);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void ConsultasConsTipoIndexadorAsync()
        {
            this.ConsultasConsTipoIndexadorAsync(null);
        }

        /// <remarks/>
        public void ConsultasConsTipoIndexadorAsync(object userState)
        {
            if ((this.ConsultasConsTipoIndexadorOperationCompleted == null))
            {
                this.ConsultasConsTipoIndexadorOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultasConsTipoIndexadorOperationCompleted);
            }
            this.InvokeAsync("ConsultasConsTipoIndexador", new object[0], this.ConsultasConsTipoIndexadorOperationCompleted, userState);
        }

        private void OnConsultasConsTipoIndexadorOperationCompleted(object arg)
        {
            if ((this.ConsultasConsTipoIndexadorCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultasConsTipoIndexadorCompleted(this, new ConsultasConsTipoIndexadorCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("hdSegurancaValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.cblc.com.br/ConsultasConsMercado", RequestNamespace = "http://www.cblc.com.br/", ResponseNamespace = "http://www.cblc.com.br/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConsultasConsMercado()
        {
            object[] results = this.Invoke("ConsultasConsMercado", new object[0]);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void ConsultasConsMercadoAsync()
        {
            this.ConsultasConsMercadoAsync(null);
        }

        /// <remarks/>
        public void ConsultasConsMercadoAsync(object userState)
        {
            if ((this.ConsultasConsMercadoOperationCompleted == null))
            {
                this.ConsultasConsMercadoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultasConsMercadoOperationCompleted);
            }
            this.InvokeAsync("ConsultasConsMercado", new object[0], this.ConsultasConsMercadoOperationCompleted, userState);
        }

        private void OnConsultasConsMercadoOperationCompleted(object arg)
        {
            if ((this.ConsultasConsMercadoCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultasConsMercadoCompleted(this, new ConsultasConsMercadoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if (((url == null)
                        || (url == string.Empty)))
            {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0)))
            {
                return true;
            }
            return false;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cblc.com.br/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.cblc.com.br/", IsNullable = false)]
    public partial class hdSeguranca : System.Web.Services.Protocols.SoapHeader
    {

        private string strContratoHashField;

        private string strContratoSenhaField;

        private string strLoginNomeField;

        private string strLoginSenhaField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        public string strContratoHash
        {
            get
            {
                return this.strContratoHashField;
            }
            set
            {
                this.strContratoHashField = value;
            }
        }

        /// <remarks/>
        public string strContratoSenha
        {
            get
            {
                return this.strContratoSenhaField;
            }
            set
            {
                this.strContratoSenhaField = value;
            }
        }

        /// <remarks/>
        public string strLoginNome
        {
            get
            {
                return this.strLoginNomeField;
            }
            set
            {
                this.strLoginNomeField = value;
            }
        }

        /// <remarks/>
        public string strLoginSenha
        {
            get
            {
                return this.strLoginSenhaField;
            }
            set
            {
                this.strLoginSenhaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ConsultasConsExtratMensalCompletedEventHandler(object sender, ConsultasConsExtratMensalCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultasConsExtratMensalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ConsultasConsExtratMensalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ConsultasConsExtratoRentabilidadeCompletedEventHandler(object sender, ConsultasConsExtratoRentabilidadeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultasConsExtratoRentabilidadeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ConsultasConsExtratoRentabilidadeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ConsultasConsTaxaProtocoloCompletedEventHandler(object sender, ConsultasConsTaxaProtocoloCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultasConsTaxaProtocoloCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ConsultasConsTaxaProtocoloCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ConsultasConsCestaCompletedEventHandler(object sender, ConsultasConsCestaCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultasConsCestaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ConsultasConsCestaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ConsultasConsTipoTituloCompletedEventHandler(object sender, ConsultasConsTipoTituloCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultasConsTipoTituloCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ConsultasConsTipoTituloCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ConsultasConsTipoIndexadorCompletedEventHandler(object sender, ConsultasConsTipoIndexadorCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultasConsTipoIndexadorCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ConsultasConsTipoIndexadorCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ConsultasConsMercadoCompletedEventHandler(object sender, ConsultasConsMercadoCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultasConsMercadoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ConsultasConsMercadoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}
