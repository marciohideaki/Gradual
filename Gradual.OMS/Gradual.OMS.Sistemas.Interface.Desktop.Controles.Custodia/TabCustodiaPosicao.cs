using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Custodia;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Custodia.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Custodia.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Seguranca.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Custodia
{
    /// <summary>
    /// Tab de lista de posições de custódia
    /// </summary>
    public partial class TabCustodiaPosicao : XtraUserControl, IControle
    {
        /// <summary>
        /// Custódia que está sendo trabalhada
        /// </summary>
        private CustodiaInfo _custodia = null;
        
        /// <summary>
        /// Contexto da interface
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Construtor default
        /// </summary>
        public TabCustodiaPosicao()
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
            if (tipoMensagem == typeof(SinalizarInicializarCustodiaRequest))
            {
                // Pega a mensagem com o tipo correto
                SinalizarInicializarCustodiaRequest parametros2 = (SinalizarInicializarCustodiaRequest)parametros;

                // Salva referencia da custodia
                _custodia = parametros2.Custodia;

                // Pede a inicializacao
                inicializar(parametros2.Custodia);
            }
            else if (tipoMensagem == typeof(SinalizarSalvarCustodiaRequest))
            {
                // Salva as alterações
                salvar();
            } 
            else if (tipoMensagem == typeof(SinalizarInicializarUsuarioRequest))
            {
                // Pega a mensagem com o tipo correto
                SinalizarInicializarUsuarioRequest parametros2 = (SinalizarInicializarUsuarioRequest)parametros;

                // Pega o contexto do usuário
                ContextoOMSInfo contextoOMS = parametros2.Usuario.Complementos.ReceberItem<ContextoOMSInfo>();
                if (contextoOMS != null && contextoOMS.CodigoCustodia != null)
                {
                    // Pede a custodia
                    CustodiaInfo custodia =
                        Ativador.Get<IServicoCustodia>().ReceberCustodia(
                            new ReceberCustodiaRequest()
                            {
                                CodigoSessao = receberInterfaceContextoOMS().SessaoInfo.CodigoSessao,
                                CodigoCustodia = contextoOMS.CodigoCustodia,
                                CarregarCotacoes = true
                            }).CustodiaInfo;

                    // Achou?
                    if (custodia != null)
                    {
                        // Salva referencia da custodia
                        _custodia = custodia;

                        // Pede a inicializacao
                        inicializar(custodia);
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
        /// Carrega as informações necessárias da custódia
        /// </summary>
        /// <param name="custodia"></param>
        private void inicializar(CustodiaInfo custodia)
        {
            // Carrega o grid
            grd.DataSource = custodia.Posicoes;
        }

        private void salvar()
        {
            // Se tem custodia, salva
            if (_custodia != null)
                Ativador.Get<IServicoCustodia>().SalvarCustodia(
                    new SalvarCustodiaRequest()
                    {
                        CodigoSessao = receberInterfaceContextoOMS().SessaoInfo.CodigoSessao,
                        CustodiaInfo = _custodia
                    });
        }

        private void cmdAdicionar_Click(object sender, EventArgs e)
        {
            _custodia.Posicoes.Add(new CustodiaPosicaoInfo());
            grdv.RefreshData();
        }

        private void cmdRemover_Click(object sender, EventArgs e)
        {
            CustodiaPosicaoInfo custodiaPosicao = grdv.GetFocusedRow() as CustodiaPosicaoInfo;
            if (custodiaPosicao != null)
            {
                _custodia.Posicoes.Remove(custodiaPosicao);
                grdv.RefreshData();
            }
        }
    }
}
