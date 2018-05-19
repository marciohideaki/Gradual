﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.544
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gradual.Integracao.Itau.Downloader.PassivoWebServices {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://service.passivo.itau.com", ConfigurationName="PassivoWebServices.DownloadArquivoService")]
    public interface DownloadArquivoService {
        
        [System.ServiceModel.OperationContractAttribute()]
        void aplicacoesEmAbertoCotasAberturaXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void aplicacoesEmAbertoCotasAberturaXMLNoZIP();
        
        [System.ServiceModel.OperationContractAttribute()]
        void cadastroAssistenteXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void cadastroCoTitularXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void cadastroContaCreditoXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void cadastroFundoXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void cadastroGestorXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void cadastroGrupoXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void cadastroUsuarioXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void controleArquivoGeradoXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void cotacaoXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void extratoCompensacaoPrejuizoXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void historicoAplicacaoResgateEstornoXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void movimentacoesEfetivadasXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void operacoesEmSerPassivoXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void operacoesEmSerPassivoXMLNoZIP();
        
        [System.ServiceModel.OperationContractAttribute()]
        void operacoesEmSerPassivoD1XML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void operacoesEmSerPassivoD1XMLNoZIP();
        
        [System.ServiceModel.OperationContractAttribute()]
        void operacoesEmSerPassivoD2XML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void posicaoResultanteRendimentosACompensarXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void saldosCotaAberturaD0XML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void saldosCotaAberturaD0XMLNoZIP();
        
        [System.ServiceModel.OperationContractAttribute()]
        void saldosCotaFechamentoD0XML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void saldosCotaFechamentoD0XMLNoZIP();
        
        [System.ServiceModel.OperationContractAttribute()]
        void arquivoDeBloqueiosXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void aplicacoesEmAbertoCotasDeFechamentoXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void arquivoDeResgatesTotaisXML();
        
        [System.ServiceModel.OperationContractAttribute()]
        void arquivoDeMovimentacoesPendentesXML();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface DownloadArquivoServiceChannel : Gradual.Integracao.Itau.Downloader.PassivoWebServices.DownloadArquivoService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DownloadArquivoServiceClient : System.ServiceModel.ClientBase<Gradual.Integracao.Itau.Downloader.PassivoWebServices.DownloadArquivoService>, Gradual.Integracao.Itau.Downloader.PassivoWebServices.DownloadArquivoService {
        
        public DownloadArquivoServiceClient() {
        }
        
        public DownloadArquivoServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DownloadArquivoServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DownloadArquivoServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DownloadArquivoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void aplicacoesEmAbertoCotasAberturaXML() {
            base.Channel.aplicacoesEmAbertoCotasAberturaXML();
        }
        
        public void aplicacoesEmAbertoCotasAberturaXMLNoZIP() {
            base.Channel.aplicacoesEmAbertoCotasAberturaXMLNoZIP();
        }
        
        public void cadastroAssistenteXML() {
            base.Channel.cadastroAssistenteXML();
        }
        
        public void cadastroCoTitularXML() {
            base.Channel.cadastroCoTitularXML();
        }
        
        public void cadastroContaCreditoXML() {
            base.Channel.cadastroContaCreditoXML();
        }
        
        public void cadastroFundoXML() {
            base.Channel.cadastroFundoXML();
        }
        
        public void cadastroGestorXML() {
            base.Channel.cadastroGestorXML();
        }
        
        public void cadastroGrupoXML() {
            base.Channel.cadastroGrupoXML();
        }
        
        public void cadastroUsuarioXML() {
            base.Channel.cadastroUsuarioXML();
        }
        
        public void controleArquivoGeradoXML() {
            base.Channel.controleArquivoGeradoXML();
        }
        
        public void cotacaoXML() {
            base.Channel.cotacaoXML();
        }
        
        public void extratoCompensacaoPrejuizoXML() {
            base.Channel.extratoCompensacaoPrejuizoXML();
        }
        
        public void historicoAplicacaoResgateEstornoXML() {
            base.Channel.historicoAplicacaoResgateEstornoXML();
        }
        
        public void movimentacoesEfetivadasXML() {
            base.Channel.movimentacoesEfetivadasXML();
        }
        
        public void operacoesEmSerPassivoXML() {
            base.Channel.operacoesEmSerPassivoXML();
        }
        
        public void operacoesEmSerPassivoXMLNoZIP() {
            base.Channel.operacoesEmSerPassivoXMLNoZIP();
        }
        
        public void operacoesEmSerPassivoD1XML() {
            base.Channel.operacoesEmSerPassivoD1XML();
        }
        
        public void operacoesEmSerPassivoD1XMLNoZIP() {
            base.Channel.operacoesEmSerPassivoD1XMLNoZIP();
        }
        
        public void operacoesEmSerPassivoD2XML() {
            base.Channel.operacoesEmSerPassivoD2XML();
        }
        
        public void posicaoResultanteRendimentosACompensarXML() {
            base.Channel.posicaoResultanteRendimentosACompensarXML();
        }
        
        public void saldosCotaAberturaD0XML() {
            base.Channel.saldosCotaAberturaD0XML();
        }
        
        public void saldosCotaAberturaD0XMLNoZIP() {
            base.Channel.saldosCotaAberturaD0XMLNoZIP();
        }
        
        public void saldosCotaFechamentoD0XML() {
            base.Channel.saldosCotaFechamentoD0XML();
        }
        
        public void saldosCotaFechamentoD0XMLNoZIP() {
            base.Channel.saldosCotaFechamentoD0XMLNoZIP();
        }
        
        public void arquivoDeBloqueiosXML() {
            base.Channel.arquivoDeBloqueiosXML();
        }
        
        public void aplicacoesEmAbertoCotasDeFechamentoXML() {
            base.Channel.aplicacoesEmAbertoCotasDeFechamentoXML();
        }
        
        public void arquivoDeResgatesTotaisXML() {
            base.Channel.arquivoDeResgatesTotaisXML();
        }
        
        public void arquivoDeMovimentacoesPendentesXML() {
            base.Channel.arquivoDeMovimentacoesPendentesXML();
        }
    }
}
