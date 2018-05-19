using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.ContaCorrente;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.ContaCorrente.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Seguranca.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Custodia
{
    public partial class TabContaCorrente : XtraUserControl, IControle
    {
        /// <summary>
        /// Conta corrente que está sendo trabalhada
        /// </summary>
        private ContaCorrenteInfo _contaCorrente = null;

        /// <summary>
        /// Contexto da interface
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Construtor default
        /// </summary>
        public TabContaCorrente()
        {
            InitializeComponent();
        }

        private InterfaceContextoOMS receberInterfaceContextoOMS()
        {
            // Verifica se o contexto está carregado
            if (_contexto == null)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();
            }

            // Retorna
            return _contexto;
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            return null;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            // Pega o tipo da mensagem
            Type tipoMensagem = parametros.GetType();

            // De acordo com o tipo da mensagem
            if (tipoMensagem == typeof(SinalizarInicializarUsuarioRequest))
            {
                // Pega a mensagem com o tipo correto
                SinalizarInicializarUsuarioRequest parametros2 = (SinalizarInicializarUsuarioRequest)parametros;

                // Pega o contexto do usuário
                ContextoOMSInfo contextoOMS = parametros2.Usuario.Complementos.ReceberItem<ContextoOMSInfo>();
                if (contextoOMS != null && contextoOMS.CodigoContaCorrente != null)
                {
                    // Pede a conta corrente
                    ContaCorrenteInfo contaCorrente =
                        Ativador.Get<IServicoContaCorrente>().ReceberContaCorrente(
                            new ReceberContaCorrenteRequest()
                            {
                                CodigoSessao = receberInterfaceContextoOMS().SessaoInfo.CodigoSessao,
                                CodigoContaCorrente = contextoOMS.CodigoContaCorrente
                            }).ContaCorrenteInfo;

                    // Achou?
                    if (contaCorrente != null)
                    {
                        // Salva referencia da custodia
                        _contaCorrente = contaCorrente;

                        // Pede a inicializacao
                        inicializar(contaCorrente);
                    }
                }
            }
            else if (tipoMensagem == typeof(SinalizarSalvarUsuarioRequest))
            {
                // Salva as alterações
                salvar();
            }

            // Retorna
            return null;
        }

        #endregion

        /// <summary>
        /// Carrega as informações da conta corrente
        /// </summary>
        /// <param name="custodia"></param>
        private void inicializar(ContaCorrenteInfo contaCorrente)
        {
            // Carrega o grid
            ppg.SelectedObject = contaCorrente;
        }

        private void salvar()
        {
            // Se tem conta corrente, salva
            if (_contaCorrente != null)
                Ativador.Get<IServicoContaCorrente>().SalvarContaCorrente(
                    new SalvarContaCorrenteRequest()
                    {
                        CodigoSessao = receberInterfaceContextoOMS().SessaoInfo.CodigoSessao,
                        ContaCorrenteInfo = _contaCorrente
                    });
        }
    }
}
