﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gradual.OMS.TesouroDireto.WSTDConectividade {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.cblc.com.br", ConfigurationName="WSTDConectividade.CTestaConectividadeSoap")]
    public interface CTestaConectividadeSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cblc.com.br/EnviarSinal", ReplyAction="*")]
        bool EnviarSinal();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CTestaConectividadeSoapChannel : Gradual.OMS.TesouroDireto.WSTDConectividade.CTestaConectividadeSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CTestaConectividadeSoapClient : System.ServiceModel.ClientBase<Gradual.OMS.TesouroDireto.WSTDConectividade.CTestaConectividadeSoap>, Gradual.OMS.TesouroDireto.WSTDConectividade.CTestaConectividadeSoap {
        
        public CTestaConectividadeSoapClient() {
        }
        
        public CTestaConectividadeSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CTestaConectividadeSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CTestaConectividadeSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CTestaConectividadeSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool EnviarSinal() {
            return base.Channel.EnviarSinal();
        }
    }
}
