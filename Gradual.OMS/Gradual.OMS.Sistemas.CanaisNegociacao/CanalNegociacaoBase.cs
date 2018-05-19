using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.CanaisNegociacao;
using Gradual.OMS.Contratos.Ordens;

namespace Gradual.OMS.Sistemas.CanaisNegociacao
{
    /// <summary>
    /// Classe base para implementação de um canal de negociação
    /// </summary>
    public class CanalNegociacaoBase
    {
        public string Codigo { get; set; }
        public IServicoOrdens ServicoOrdens { get; set; }

        /// <summary>
        /// Inicia o canal
        /// </summary>
        public void Iniciar()
        {
            OnIniciar();
        }

        /// <summary>
        /// Método virtual para iniciar o canal
        /// </summary>
        protected virtual void OnIniciar()
        {
        }

        /// <summary>
        /// Para o canal
        /// </summary>
        public void Parar()
        {
            OnParar();
        }

        /// <summary>
        /// Método virtual para parar o canal
        /// </summary>
        protected virtual void OnParar()
        {
        }

        /// <summary>
        /// Pede para o canal processar a mensagem
        /// </summary>
        /// <param name="mensagem"></param>
        public void ProcessarMensagem(MensagemRequestBase mensagem)
        {
            OnProcessarMensagem(mensagem);
        }

        /// <summary>
        /// Método virtual para o processamento da mensagem
        /// </summary>
        /// <param name="mensagem"></param>
        protected virtual void OnProcessarMensagem(MensagemRequestBase mensagem)
        {
        }

        /// <summary>
        /// Evento para sinalizar mensagens recebidas pelo canal
        /// </summary>
        public event EventHandler<MensagemRecebidaEventArgs> EventoMensagemRecebida;

    }
}
