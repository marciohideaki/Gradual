using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    public partial class CadastroRegrasRiscoBase : XtraUserControl
    {
        /// <summary>
        /// Parametros da tela
        /// </summary>
        private CadastroRegrasRiscoBaseParametros _parametros = new CadastroRegrasRiscoBaseParametros();

        /// <summary>
        /// Evento disparado quando se salva um elemento
        /// </summary>
        public event EventHandler<CadastroRegrasRiscoBaseEventArgs> EventoSalvar;

        /// <summary>
        /// Evento disparado quando se remove um elemento
        /// </summary>
        public event EventHandler<CadastroRegrasRiscoBaseEventArgs> EventoRemover;

        /// <summary>
        /// Contexto da interface. Informações da sessão
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Referencia externa para o grid
        /// </summary>
        public GridControl Grid { get { return grd; } }
        
        /// <summary>
        /// Referencia externa para o gridview
        /// </summary>
        public GridView GridView { get { return grdv; } }

        /// <summary>
        /// Indica o agrupamento base, ou modelo, para as regras
        /// criadas pelo controle.
        /// </summary>
        public RiscoGrupoInfo AgrupamentoBase { get; set; }

        /// <summary>
        /// Lista das regras de risco
        /// </summary>
        public List<RegraRiscoInfo> RegrasRisco = new List<RegraRiscoInfo>();

        /// <summary>
        /// Construtor default
        /// </summary>
        public CadastroRegrasRiscoBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento de inicialização do controle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Inicializar()
        {
            // Apenas se não estiver em modo design
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Captura eventos
                grdv.DoubleClick += new EventHandler(grdv_DoubleClick);

                // Cria as colunas
                grdv.OptionsBehavior.AutoPopulateColumns = false;
                grdv.Columns.Clear();
                grdv.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { Caption = "Código Regra", FieldName = "CodigoRegraRisco", Visible = true });
                grdv.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { Caption = "Tipo da Regra", FieldName = "TipoRegra", Visible = true });
                grdv.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { Caption = "Bolsa", FieldName = "Agrupamento.CodigoBolsa", Visible = true });
                grdv.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { Caption = "Ativo", FieldName = "Agrupamento.CodigoAtivo", Visible = true });
                grdv.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { Caption = "AtivoBase", FieldName = "Agrupamento.CodigoAtivoBase", Visible = true });
                grdv.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { Caption = "Usuário", FieldName = "Agrupamento.CodigoUsuario", Visible = true });
                grdv.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { Caption = "Perfil de Risco", FieldName = "Agrupamento.CodigoPerfilRisco", Visible = true });
                grdv.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { Caption = "Sistema Cliente", FieldName = "Agrupamento.CodigoSistemaCliente", Visible = true });
                grdv.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { Caption = "Habilitado", FieldName = "Habilitado", Visible = true });

                // Refresh
                object temp = grd.DataSource;
                grd.DataSource = null;
                grd.DataSource = temp;

                // Carrega os layouts dos controles
                _parametros.LayoutsDevExpress.RecuperarLayouts(this);
            }
        }

        /// <summary>
        /// Duplo clique do grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdv_DoubleClick(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            RegraRiscoInfo info = (RegraRiscoInfo)this.grdv.GetFocusedRow();
            if (info != null)
            {
                // Faz o clone da instancia para passar para a edição
                RegraRiscoInfo infoClone = info.ClonarObjeto<RegraRiscoInfo>();

                // Mostra detalhe e salva se ok
                RegraRiscoDetalhe controleDetalhe = new RegraRiscoDetalhe(infoClone);
                FormDialog frm = new FormDialog(controleDetalhe, FormDialogTipoEnum.OkCancelar);
                frm.Width = 500;
                frm.Height = 600;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Faz o merge das informacoes
                    info.Agrupamento = infoClone.Agrupamento;
                    info.Config = infoClone.Config;
                    info.DataCriacao = infoClone.DataCriacao;
                    info.DataVencimento = infoClone.DataVencimento;
                    info.Habilitado = infoClone.Habilitado;
                    info.TipoRegra = infoClone.TipoRegra;

                    // Dispara o evento
                    if (this.EventoSalvar != null)
                        this.EventoSalvar(
                            this, 
                            new CadastroRegrasRiscoBaseEventArgs() 
                            { 
                                RegraRisco = info
                            });
                }
            }
        }

        /// <summary>
        /// Salva parametros
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Salva layouts
            _parametros.LayoutsDevExpress.SalvarLayout(this.grdv);

            // Retorna
            return _parametros;
        }

        /// <summary>
        /// Carrega parametros
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="evento"></param>
        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            // Seta a classe de parametros. Será utilizada no load do controle
            CadastroRegrasRiscoBaseParametros parametros2 = parametros as CadastroRegrasRiscoBaseParametros;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        /// <summary>
        /// Faz a atualização do grid através da lista
        /// </summary>
        private void atualizarGrid()
        {
            // Guarda eventual selecao
            int selecao = grdv.FocusedRowHandle;

            // Associa ao grid
            grd.DataSource = null;
            grd.DataSource = this.RegrasRisco;

            // Mantem selecao anterior
            grdv.FocusedRowHandle = selecao;
        }
        
        /// <summary>
        /// Carrega a lista de regras informada
        /// </summary>
        /// <param name="lista"></param>
        public void CarregarLista(List<RegraRiscoInfo> lista)
        {
            // Salva lista
            this.RegrasRisco = lista;
            
            // Atualiza
            atualizarGrid();
        }

        public void CriarRegraRisco()
        {
            // Cria nova regra
            RegraRiscoInfo info = new RegraRiscoInfo();

            // Se tem agrupamento base, informa
            if (this.AgrupamentoBase != null)
                info.Agrupamento = this.AgrupamentoBase;

            // Mostra tela de detalhe e salva usuario se ok
            RegraRiscoDetalhe controleDetalhe = new RegraRiscoDetalhe(info);

            // Mostra tela de cadastro
            FormDialog frm = new FormDialog(controleDetalhe, FormDialogTipoEnum.OkCancelar);
            frm.Width = 500;
            frm.Height = 600;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Se tem modelo, garante que as informações do modelo estão corretas
                if (this.AgrupamentoBase != null)
                {
                    if (this.AgrupamentoBase.CodigoAtivo != null) info.Agrupamento.CodigoAtivo = this.AgrupamentoBase.CodigoAtivo;
                    if (this.AgrupamentoBase.CodigoAtivoBase != null) info.Agrupamento.CodigoAtivoBase = this.AgrupamentoBase.CodigoAtivoBase;
                    if (this.AgrupamentoBase.CodigoBolsa != null) info.Agrupamento.CodigoBolsa = this.AgrupamentoBase.CodigoBolsa;
                    if (this.AgrupamentoBase.CodigoPerfilRisco != null) info.Agrupamento.CodigoPerfilRisco = this.AgrupamentoBase.CodigoPerfilRisco;
                    if (this.AgrupamentoBase.CodigoSistemaCliente != null) info.Agrupamento.CodigoSistemaCliente = this.AgrupamentoBase.CodigoSistemaCliente;
                    if (this.AgrupamentoBase.CodigoUsuario != null) info.Agrupamento.CodigoUsuario = this.AgrupamentoBase.CodigoUsuario;
                }
                
                // Insere novo elemento na lista
                this.RegrasRisco.Add(info);

                // Dispara o evento
                if (this.EventoSalvar != null)
                    this.EventoSalvar(
                        this,
                        new CadastroRegrasRiscoBaseEventArgs()
                        {
                            RegraRisco = info
                        });

                // Atualiza o grid
                atualizarGrid();
            }
        }

        public void RemoverRegraRiscoSelecionada()
        {
            // Tenta pegar o objeto selecionado
            RegraRiscoInfo info = (RegraRiscoInfo)this.grdv.GetFocusedRow();
            if (info != null)
            {
                // Remove da lista
                this.RegrasRisco.Remove(info);

                // Dispara o evento
                if (this.EventoRemover != null)
                    this.EventoRemover(
                        this,
                        new CadastroRegrasRiscoBaseEventArgs()
                        {
                            RegraRisco = info
                        });

                // Atualiza a lista
                atualizarGrid();
            }
        }
    }
}
