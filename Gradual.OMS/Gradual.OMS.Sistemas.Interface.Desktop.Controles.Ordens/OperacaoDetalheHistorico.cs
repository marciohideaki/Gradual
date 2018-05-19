using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Ordens
{
    /// <summary>
    /// Controle histórico de uma operação. Mostra os eventos que ocorreram
    /// com uma determinada operação.
    /// </summary>
    public partial class OperacaoDetalheHistorico : DevExpress.XtraEditors.XtraUserControl, IControle
    {
        /// <summary>
        /// Referencia para o item de controle
        /// </summary>
        private Controle _item = null;

        /// <summary>
        /// Parametros do controle
        /// </summary>
        private OperacaoDetalheHistoricoParametros _parametros = new OperacaoDetalheHistoricoParametros();

        /// <summary>
        /// Construtor default.
        /// </summary>
        public OperacaoDetalheHistorico()
        {
            InitializeComponent();

            this.Load += new EventHandler(OperacaoDetalheHistorico_Load);
            this.HandleDestroyed += new EventHandler(OperacaoDetalheHistorico_HandleDestroyed);
            this.cmdConfigurar.ItemClick += new ItemClickEventHandler(cmdConfigurar_ItemClick);
        }

        private void cmdConfigurar_ItemClick(object sender, ItemClickEventArgs e)
        {
            new FormMensagem("Configurar parâmetros de detalhe de histórico de operações", _parametros).ShowDialog();
            carregarCoresGrid();
        }

        private void OperacaoDetalheHistorico_HandleDestroyed(object sender, EventArgs e)
        {
            salvarLayout();
        }

        private void OperacaoDetalheHistorico_Load(object sender, EventArgs e)
        {
            carregarParametrosDefault();
            carregarLayouts();
        }

        /// <summary>
        /// Salva o layout do controle
        /// </summary>
        private void salvarLayout()
        {
            // Inicializa
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            Type tipo = this.GetType();

            // Salva layout do grid
            _parametros.LayoutsDevExpress.SalvarLayout(this.grdv);

            // Salva parametros da janela
            _parametros.Largura = this.ParentForm.Width;
            _parametros.Altura = this.ParentForm.Height;
            _parametros.X = this.ParentForm.Left;
            _parametros.Y = this.ParentForm.Top;

            // Salva em parametros default
            if (servicoInterface.ParametrosDefault.ContainsKey(tipo.FullName))
                servicoInterface.ParametrosDefault[tipo.FullName] = _parametros;
            else
                servicoInterface.ParametrosDefault.Add(tipo.FullName, _parametros);
        }

        /// <summary>
        /// Carrega parametros default, caso existam
        /// </summary>
        private void carregarParametrosDefault()
        {
            // Inicializa
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            Type tipo = this.GetType();

            // Verifica tem parametros default para carregar
            if (servicoInterface.ParametrosDefault.ContainsKey(tipo.FullName))
            {
                // Carrega os parametros
                _parametros =
                    (OperacaoDetalheHistoricoParametros)servicoInterface.ParametrosDefault[tipo.FullName];
                _parametros.LayoutsDevExpress.RecuperarLayouts(this);

                // Carrega parametros da janela
                this.ParentForm.Top = _parametros.Y;
                this.ParentForm.Left = _parametros.X;
                this.ParentForm.Width = _parametros.Largura;
                this.ParentForm.Height = _parametros.Altura;
            }
        }

        /// <summary>
        /// Carrega o layout do grid
        /// </summary>
        private void carregarLayouts()
        {
            // Estilos de ExecType
            criarEstilos(
                this.grdv,
                colExecType,
                typeof(OrdemTipoExecucaoEnum));

            // Estilos de OrdRejReason
            criarEstilos(
                this.grdv,
                colOrdRejReason,
                typeof(OrdemMotivoRejeicaoEnum));

            // Estilos de OrdStatus
            criarEstilos(
                this.grdv,
                colOrdStatus,
                typeof(OrdemStatusEnum));

            // Carrega as cores
            carregarCoresGrid();
        }

        #region IControle Members

        /// <summary>
        /// Faz a inicialização do controle
        /// </summary>
        /// <param name="controle"></param>
        public void Inicializar(Controle controle)
        {
            _item = controle;
        }

        /// <summary>
        /// Salva os parâmetros do controle
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            return null;
        }

        /// <summary>
        /// Carrega os parametros do controle
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="evento"></param>
        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
        }

        /// <summary>
        /// Processa mensagens específicas no controle
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            // Verifica o tipo da mensagem que chegou
            if (parametros.GetType() == typeof(InicializarHistoricoOperacaoRequest))
            {
                // Processa mensagem de inicializacao
                InicializarHistoricoOperacaoRequest parametros2 = (InicializarHistoricoOperacaoRequest)parametros;
                grd.DataSource = parametros2.Historico;
            }

            // Retorno
            return null;
        }

        #endregion

        private void criarEstilos(GridView grid, GridColumn column, Type enumerador)
        {
            // Cria os estilos condicionais para OrdemStatusEnum
            foreach (string status in Enum.GetNames(enumerador))
                grid.FormatConditions.Add(
                    new StyleFormatCondition()
                    {
                        ApplyToRow = false,
                        Column = column,
                        Condition = FormatConditionEnum.Equal,
                        Value1 = Enum.Parse(enumerador, status, true)
                    });
        }

        private void carregarCoresGrid()
        {
            // Seta as cores informadas
            foreach (StyleFormatCondition estilo in this.grdv.FormatConditions)
            {
                // Considera apenas estilos da coluna de status
                if (estilo.Column == colExecType)
                {
                    // Verifica se tem o item em parametros
                    CorEnumeradorInfo<OrdemTipoExecucaoEnum> cor =
                        (from c in _parametros.CoresMensagemExecType
                         where c.Valor == (OrdemTipoExecucaoEnum)estilo.Value1
                         select c).FirstOrDefault();

                    // Apenas se tem a cor
                    if (cor != null)
                    {
                        estilo.ApplyToRow = cor.LinhaInteira;
                        estilo.Appearance.BackColor = cor.Cor;
                        estilo.Appearance.Options.UseBackColor = true;
                    }
                    else
                    {
                        estilo.Appearance.Options.UseBackColor = false;
                    }
                }
                else if (estilo.Column == colOrdRejReason)
                {
                    // Verifica se tem o item em parametros
                    CorEnumeradorInfo<OrdemMotivoRejeicaoEnum> cor =
                        (from c in _parametros.CoresMensagemOrdRejReason
                         where c.Valor == (OrdemMotivoRejeicaoEnum)estilo.Value1
                         select c).FirstOrDefault();

                    // Apenas se tem a cor
                    if (cor != null)
                    {
                        estilo.ApplyToRow = cor.LinhaInteira;
                        estilo.Appearance.BackColor = cor.Cor;
                        estilo.Appearance.Options.UseBackColor = true;
                    }
                    else
                    {
                        estilo.Appearance.Options.UseBackColor = false;
                    }
                }
                else if (estilo.Column == colOrdStatus)
                {
                    // Verifica se tem o item em parametros
                    CorEnumeradorInfo<OrdemStatusEnum> cor =
                        (from c in _parametros.CoresMensagemOrdStatus
                         where c.Valor == (OrdemStatusEnum)estilo.Value1
                         select c).FirstOrDefault();

                    // Apenas se tem a cor
                    if (cor != null)
                    {
                        estilo.ApplyToRow = cor.LinhaInteira;
                        estilo.Appearance.BackColor = cor.Cor;
                        estilo.Appearance.Options.UseBackColor = true;
                    }
                    else
                    {
                        estilo.Appearance.Options.UseBackColor = false;
                    }
                }
            }
        }
    
    }
}
