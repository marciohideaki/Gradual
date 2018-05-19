using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Fix;

using QuickFix;
using QuickFix42;

namespace Gradual.OMS.Sistemas.CanaisNegociacao.CanalBovespa
{
    public class CanalNegociacaoBovespa : CanalNegociacaoBase
    {
        private HostFix<AplicacaoFixClienteBovespa> _host = null;
        private SessionID _session = null;
        private CanalNegociacaoBVMFConfig _config = null;

        public CanalNegociacaoBVMFConfig Config
        {
            get { return _config; }
        }

        protected override void OnIniciar()
        {
            // Carrega configurações
            _config =
                GerenciadorConfig.ReceberConfig<CanalNegociacaoBVMFConfig>(this.Codigo);

            // Sobe o host fix
            _host = new HostFix<AplicacaoFixClienteBovespa>(_config.ArquivoConfig, true);
            _host.AplicacaoFix.CanalBovespa = this;
            _host.Iniciar();

            // Cria sessao que será usada para mandar as mensagens
            _session =
                new SessionID(
                    new BeginString(_config.BeginString),
                    new SenderCompID(_config.SenderCompID),
                    new TargetCompID(_config.TargetCompID));
        }

        protected override void OnParar()
        {
            // Faz o log
            Gradual.OMS.Library.Log.EfetuarLog("Finalizando canal Bovespa", LogTipoEnum.Passagem, ModulosOMS.ModuloCanais);

            // Finaliza o host
            _host.Parar();
        }

        protected override void OnProcessarMensagem(MensagemRequestBase mensagem)
        {
            // Faz a tradução da mensagem
            QuickFix.Message mensagemFix = CanalNegociacaoBovespaTradutor.Traduzir(this, mensagem);

            // Valida
            if (mensagemFix == null)
            {
                // Envia mensagem de erro para o cliente
                // TODO: CanalNegociacaoTeste - OnProcessarMensagem: Retornar erro indicando mensagem inválida
                this.ServicoOrdens.ProcessarMensagem(
                    new SinalizarMensagemInvalidaRequest()
                    {
                        CodigoCanal = this.Codigo,
                        CodigoMensagemOrigem = mensagem.CodigoMensagem,
                        Descricao = "CanalNegociacaoTeste.OnProcessarMensagem: Tipo de mensagem inválida (" + mensagem.GetType().FullName + ")"
                    });
            }
            else
            {
                // Envia a mensagem para o servidor Fix
                Session.sendToTarget(mensagemFix, _session);
            }
        }

    }
}
