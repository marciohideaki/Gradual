using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

using DevExpress.XtraEditors;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Ordens
{
    /// <summary>
    /// Controle de boleto de operações.
    /// </summary>
    public partial class OperacaoBoleto : XtraUserControl, IControle
    {
        private Controle _item = null;

        private OperacaoBoletoParametros _parametros = new OperacaoBoletoParametros();
        
        public OperacaoBoleto()
        {
            InitializeComponent();

            this.Load += new EventHandler(OperacaoBoleto_Load);
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
            _item = controle;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Salva layouts
            _parametros.LayoutsDevExpress.SalvarLayout(this.layoutControl);

            // Retorna
            return _parametros;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            // Seta a classe de parametros. Será utilizada no load do controle
            OperacaoBoletoParametros parametros2 = parametros as OperacaoBoletoParametros;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            // Verifica o tipo da mensagem
            if (parametros.GetType() == typeof(CarregarOperacaoBoletoRequest))
            {
                // Carrega informações na tela
                CarregarOperacaoBoletoRequest parametros2 = (CarregarOperacaoBoletoRequest)parametros;
                if (parametros2.Bolsa != null) txtBolsa.Text = parametros2.Bolsa;
                if (parametros2.Cliente != null) txtCliente.Text = parametros2.Cliente;
                if (parametros2.Direcao.HasValue) optDirecao.EditValue = parametros2.Direcao.Value;
                if (parametros2.Papel != null) txtPapel.Text = parametros2.Papel;
                if (parametros2.Preco.HasValue) txtValor.EditValue = parametros2.Preco.Value;
                if (parametros2.Quantidade.HasValue) txtQuantidade.EditValue = parametros2.Quantidade.Value;
                if (parametros2.Tipo.HasValue) cmbOrdemTipo.EditValue = parametros2.Tipo.Value;
                if (parametros2.Validade.HasValue) cmbOrdemValidade.EditValue = parametros2.Validade.Value;
                
                // Verifica se deve ativar esta janela
                if (parametros2.AtivarJanela)
                {
                    // Pega referencia ao servico de interface e pede ativacao da janela
                    IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                    servicoInterface.AtivarJanela(_item.Janela.Info.Id);
                }
            }

            // Retorna
            return null;
        }

        #endregion

        private void cmdEnviar_Click(object sender, EventArgs e)
        {
            // Faz referencia ao servico de interface e pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Cria a mensagem da ordem
            ExecutarOrdemRequest request = 
                new ExecutarOrdemRequest() 
                { 
                    CodigoUsuarioDestino = txtCliente.Text,
                    CodigoBolsa = txtBolsa.Text,
                    CodigoSessao = contexto.SessaoInfo.CodigoSessao,
                    DataReferencia = DateTime.Now,
                    OrderQty = Convert.ToDouble(txtQuantidade.Value),
                    OrdType = OrdemTipoEnum.Limitada,
                    Price = Convert.ToDouble(txtValor.Value),
                    Side = (OrdemDirecaoEnum)optDirecao.EditValue,
                    Symbol = txtPapel.Text
                };

            // Faz o envio da ordem
            ExecutarOrdemResponse response = 
                (ExecutarOrdemResponse)
                    contexto.ServicoOrdensServidor.ProcessarMensagem(request);

            // Caso tenha ocorrido erro, mostra
            if (response.StatusResposta != MensagemResponseStatusEnum.OK)
                new FormMensagem("Erro no envio: " + response.DescricaoResposta, response).ShowDialog();
            else
                new FormMensagem("Ordem enviada com sucesso!", null).ShowDialog();
        }

        private void OperacaoBoleto_Load(object sender, EventArgs e)
        {
            // Carrega os layouts dos controles
            _parametros.LayoutsDevExpress.RecuperarLayouts(this);
        }

        private void cmdSalvarLayoutDefault_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _item.SalvarDefault();
        }

        private void cmdValidar_Click(object sender, EventArgs e)
        {
            // Faz referencia ao servico de interface e pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Cria a mensagem da ordem
            ExecutarOrdemRequest request =
                new ExecutarOrdemRequest()
                {
                    CodigoUsuarioDestino = txtCliente.Text,
                    CodigoBolsa = txtBolsa.Text,
                    CodigoSessao = contexto.SessaoInfo.CodigoSessao,
                    DataReferencia = DateTime.Now,
                    OrderQty = Convert.ToDouble(txtQuantidade.Value),
                    OrdType = OrdemTipoEnum.Limitada,
                    Price = Convert.ToDouble(txtValor.Value),
                    Side = (OrdemDirecaoEnum)optDirecao.EditValue,
                    Symbol = txtPapel.Text
                };

            // Executa
            IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();
            ValidarOperacaoResponse validarResponse = 
                servicoRisco.ValidarOperacao(                
                    new ValidarOperacaoRequest() 
                    { 
                        CodigoSessao = contexto.SessaoInfo.CodigoSessao,
                        Mensagem = request 
                    });

            // Mostra ao usuário
            if (validarResponse.StatusResposta == MensagemResponseStatusEnum.ErroPrograma)
                new FormMensagem("Validação", validarResponse.Validacao).ShowDialog();
            else
                new FormValidacao(validarResponse).ShowDialog();
        }

    }
}
