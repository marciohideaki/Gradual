using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.MarketData.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Risco.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Seguranca.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    /// <summary>
    /// Controle para a aba de regras de risco do perfil de risco
    /// </summary>
    public partial class TabRegraRisco : XtraUserControl, IControle
    {
        /// <summary>
        /// Parametros da tela
        /// </summary>
        private TabRegraRiscoParametros _parametros = new TabRegraRiscoParametros();

        /// <summary>
        /// Lista inicial de regras de risco
        /// </summary>
        private List<RegraRiscoInfo> _listaInicial = null;

        /// <summary>
        /// Contexto da interface
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Construtor default
        /// </summary>
        public TabRegraRisco()
        {
            InitializeComponent();

            this.Load += new EventHandler(TabRegraRiscoPerfilRisco_Load);
        }

        private void TabRegraRiscoPerfilRisco_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Inicializa o controle base
                cadastroRegrasRiscoBase.Inicializar();
            }
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Salva layouts
            _parametros.ParametrosRegraRiscoBase = cadastroRegrasRiscoBase.SalvarParametros(evento);

            // Retorna
            return _parametros;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            // Seta a classe de parametros. Será utilizada no load do controle
            TabRegraRiscoParametros parametros2 = parametros as TabRegraRiscoParametros;
            if (parametros2 != null)
                cadastroRegrasRiscoBase.CarregarParametros(parametros2.ParametrosRegraRiscoBase, evento);
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
             // Pega o tipo da mensagem
            Type tipoMensagem = parametros.GetType();

            // De acordo com o tipo da mensagem
            if (tipoMensagem == typeof(SinalizarInicializarPerfilRiscoRequest))
            {
                // Pega a mensagem com o tipo correto
                SinalizarInicializarPerfilRiscoRequest parametros2 = (SinalizarInicializarPerfilRiscoRequest)parametros;

                // Pede a inicializacao
                inicializar(
                    new RiscoGrupoInfo() 
                    { 
                        CodigoPerfilRisco = parametros2.PerfilRisco.CodigoPerfilRisco
                    });
            }
            else if (tipoMensagem == typeof(SinalizarSalvarPerfilRiscoRequest))
            {
                // Salva as alterações
                salvar();
            }
            else if (tipoMensagem == typeof(SinalizarInicializarUsuarioRequest))
            {
                // Pega a mensagem com o tipo correto
                SinalizarInicializarUsuarioRequest parametros2 = (SinalizarInicializarUsuarioRequest)parametros;

                // Pede a inicializacao
                inicializar(
                    new RiscoGrupoInfo()
                    {
                        CodigoUsuario = parametros2.Usuario.CodigoUsuario
                    });
            }
            else if (tipoMensagem == typeof(SinalizarSalvarUsuarioRequest))
            {
                // Salva as alterações
                salvar();
            }
            else if (tipoMensagem == typeof(SinalizarInicializarInstrumentoBovespaRequest))
            {
                // Pega a mensagem com o tipo correto
                SinalizarInicializarInstrumentoBovespaRequest parametros2 = (SinalizarInicializarInstrumentoBovespaRequest)parametros;

                // Pede a inicializacao
                inicializar(
                    new RiscoGrupoInfo()
                    {
                        CodigoAtivo = parametros2.InstrumentoBovespa.CodigoNegociacao
                    });
            }
            else if (tipoMensagem == typeof(SinalizarSalvarInstrumentoBovespaRequest))
            {
                // Salva as alterações
                salvar();
            }

            // Retorna
            return null;
        }

        #endregion

        private void inicializar(RiscoGrupoInfo agrupamento)
        {
            // Carrega as regras do perfil
            ListarRegraRiscoResponse respostaListar =
                Ativador.Get<IServicoRisco>().ListarRegraRisco(
                    new ListarRegraRiscoRequest()
                    {
                        FiltroAgrupamento = agrupamento
                    });

            // Salva uma cópia da lista inicial
            _listaInicial = 
                new List<RegraRiscoInfo>(
                    respostaListar.Resultado);

            // Faz a carga no controle
            cadastroRegrasRiscoBase.CarregarLista(respostaListar.Resultado);

            // Seta o modelo de agrupamento
            cadastroRegrasRiscoBase.AgrupamentoBase = agrupamento;
        }

        private void salvar()
        {
            // Se o contexto não estiver carregado, a tab nao foi visitada, ou seja, nao faz nada
            if (_contexto == null)
                return;

            // Varre a lista inicial (consegue achar os alterados e excluidos)
            foreach (RegraRiscoInfo regraRiscoInicial in _listaInicial)
            {
                // Inicializa
                bool excluir = cadastroRegrasRiscoBase.RegrasRisco.Count == 0;

                // Verifica se consta na nova lista
                if (!excluir)
                    excluir =
                        cadastroRegrasRiscoBase.RegrasRisco.Find(
                            r => r.CodigoRegraRisco == regraRiscoInicial.CodigoRegraRisco) == null;

                // Salva ou exclui?
                if (excluir)
                    Ativador.Get<IServicoRisco>().RemoverRegraRisco(
                        new RemoverRegraRiscoRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                            CodigoRegraRisco = regraRiscoInicial.CodigoRegraRisco
                        });
                else
                    Ativador.Get<IServicoRisco>().SalvarRegraRisco(
                        new SalvarRegraRiscoRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                            RegraRiscoInfo = regraRiscoInicial
                        });
            }

            // Varre a nova lista (salva os inseridos)
            foreach (RegraRiscoInfo regraRisco in cadastroRegrasRiscoBase.RegrasRisco)
            {
                // Inicializa
                bool incluir = _listaInicial.Count == 0;

                // Verifica se consta na nova lista
                if (!incluir)
                    incluir =
                        _listaInicial.Find(
                            r => r.CodigoRegraRisco == regraRisco.CodigoRegraRisco) == null;

                // Salva ou exclui?
                if (incluir)
                    Ativador.Get<IServicoRisco>().SalvarRegraRisco(
                        new SalvarRegraRiscoRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                            RegraRiscoInfo = regraRisco
                        });
            }

            // A lista inicial passa a ser a nova lista
            _listaInicial = cadastroRegrasRiscoBase.RegrasRisco;
        }

        private void cmdAdicionar_Click(object sender, EventArgs e)
        {
            cadastroRegrasRiscoBase.CriarRegraRisco();
        }

        private void cmdRemover_Click(object sender, EventArgs e)
        {
            cadastroRegrasRiscoBase.RemoverRegraRiscoSelecionada();
        }
    }
}
