using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
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
    /// Controle para listar as operações realizadas e o status delas.
    /// Age tanto como histórico quanto como controle de operações pendentes.
    /// </summary>
    public partial class OperacaoHistorico : XtraUserControl, IControle
    {
        /// <summary>
        /// Referencia para o objeto de controle
        /// </summary>
        private Controle _controle = null;

        /// <summary>
        /// Lista de sinalizações já ocorridas
        /// </summary>
        private List<SinalizarExecucaoOrdemRequest> _sinalizacoes = new List<SinalizarExecucaoOrdemRequest>();

        /// <summary>
        /// Parametros do controle
        /// </summary>
        private OperacaoHistoricoParametros _parametros = new OperacaoHistoricoParametros();

        /// <summary>
        /// Construtor default
        /// </summary>
        public OperacaoHistorico()
        {
            InitializeComponent();

            this.Load += new EventHandler(OperacaoHistorico_Load);
            this.grdvOperacoes.MouseUp += new MouseEventHandler(grdvOperacoes_MouseUp);
            this.grdvOperacoes.DoubleClick += new EventHandler(grdvOperacoes_DoubleClick);
            this.grdvMensagens.DoubleClick += new EventHandler(grdvMensagens_DoubleClick);
            this.cmdConfigurar.ItemClick += new ItemClickEventHandler(cmdConfigurar_ItemClick);
        }

        /// <summary>
        /// Evento de load do controle.
        /// Carrega as operações realizadas e monitora os eventos de ordens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OperacaoHistorico_Load(object sender, EventArgs e)
        {
            // Inicializa PopupMenu
            inicializarPopupMenu();

            // Faz referencias aos servicos de interface e ordens e ao contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Lista as ordens
            listarOrdens();

            // Lista as mensagens
            listarMensagens();

            // Carrega os layouts dos grids
            carregarLayouts();

            // Assina o evento de sinalização
            contexto.CallbackEvento.Evento += new EventHandler<EventoEventArgs>(CallbackEvento_Evento);
        }

        /// <summary>
        /// Delegate utilizado para efetuar as chamadas de invoke para os elementos de tela
        /// </summary>
        private delegate void InvokeDelegate();
        
        /// <summary>
        /// Evento recebido para sinalizar alguma execução de ordem.
        /// Atualiza a lista de ordens interna.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallbackEvento_Evento(object sender, EventoEventArgs e)
        {
            // Controle de fluxo
            try
            {
                // Usa o invoke para 
                this.Invoke(
                    new InvokeDelegate(
                        delegate()
                        {
                            // Pega a mensagem
                            object mensagem = e.EventoInfo.ObjetoDesserializar();

                            // Verifica se é mensagem de sinalização
                            if (mensagem.GetType() == typeof(SinalizarExecucaoOrdemRequest))
                            {
                                // Atualiza informações da ordem informada
                                _sinalizacoes.Add((SinalizarExecucaoOrdemRequest)mensagem);

                                // Atualiza o grid
                                grdMensagens.DataSource = null;
                                grdMensagens.DataSource = _sinalizacoes;

                                // Pede novamente a lista de ordens
                                listarOrdens();
                            }
                        }));
            }
            catch (Exception ex)
            {
                // Aqui apenas efetua log, não pode retornar erro
                Log.EfetuarLog(ex, null, ModulosOMS.ModuloInterfaceDesktop);
            }
        }

        #region IControle Members

        /// <summary>
        /// Chamada de inicialização do controle.
        /// </summary>
        /// <param name="controle"></param>
        public void Inicializar(Controle controle)
        {
            _controle = controle;
        }

        /// <summary>
        /// Solicitação para salvar os parâmetros do controle.
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Salva layouts
            _parametros.LayoutsDevExpress.SalvarLayout(this.grdvOperacoes);
            _parametros.LayoutsDevExpress.SalvarLayout(this.grdvMensagens);
            _parametros.LayoutsDevExpress.SalvarLayout(this.layoutControl1);

            // Retorna
            return _parametros;
        }

        /// <summary>
        /// Solicitação para carregar os parâmetros do controle.
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="evento"></param>
        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            // Seta a classe de parametros. Será utilizada no load do controle
            OperacaoHistoricoParametros parametros2 = parametros as OperacaoHistoricoParametros;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        /// <summary>
        /// Solicitação de processamento de mensagens.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion

        #region Métodos Privados

        private void listarOrdens()
        {
            // Faz referencias aos servicos de interface e ordens e ao contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();
            IServicoOrdensServidor servicoOrdensServidor = contexto.ServicoOrdensServidor;

            // Pede a lista de ordens realizadas no dia
            ListarOrdensResponse listarOrdensResponse =
                (ListarOrdensResponse)
                    servicoOrdensServidor.ProcessarMensagem(
                        new ListarOrdensRequest()
                        {
                            CodigoSessao = contexto.SessaoOrdensInfo.CodigoSessao
                        });

            // Mostra no grid
            grdOperacoes.DataSource = null;
            grdOperacoes.DataSource = listarOrdensResponse.Ordens;
        }

        private void listarMensagens()
        {
            // Faz referencias aos servicos de interface e ordens e ao contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();
            IServicoOrdensServidor servicoOrdensServidor = contexto.ServicoOrdensServidor;

            // Pede lista de mensagens
            ListarMensagensResponse listarMensagensResponse =
                (ListarMensagensResponse)
                    servicoOrdensServidor.ProcessarMensagem(
                        new ListarMensagensRequest()
                        {
                        });

            // Adiciona na coleção de sinalizações
            _sinalizacoes.AddRange(
                from m in listarMensagensResponse.Mensagens
                where m.GetType() == typeof(SinalizarExecucaoOrdemRequest)
                select (SinalizarExecucaoOrdemRequest)m);

            // Mostra no grid
            grdMensagens.DataSource = null;
            grdMensagens.DataSource = _sinalizacoes;
        }

        private void cancelarOrdem(OrdemInfo ordem)
        {
            // Faz referencias aos servicos de interface e ordens e ao contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            InterfaceContextoOMS contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();
            IServicoOrdensServidor servicoOrdensServidor = contexto.ServicoOrdensServidor;

            // Pede a lista de ordens realizadas no dia
            CancelarOrdemResponse cancelarOrdemResponse =
                (CancelarOrdemResponse)
                    servicoOrdensServidor.ProcessarMensagem(
                        new CancelarOrdemRequest()
                        {
                            CodigoSessao = contexto.SessaoOrdensInfo.CodigoSessao,
                            CodigoCliente = ordem.CodigoCliente,
                            OrigClOrdID = ordem.CodigoOrdem
                        });

            // Informa
            new FormMensagem("Cancelamento de Ordem enviado com sucesso", ordem).ShowDialog();
        }

        private void inicializarPopupMenu()
        {
            // Item Cancelamento
            ToolStripItem itemCancelar = new ToolStripButton()
            { 
                Text = "Cancelar"
            };
            itemCancelar.Click += 
                delegate(object sender, EventArgs e)
                {
                    OrdemInfo ordemInfo = (OrdemInfo)this.grdvOperacoes.GetFocusedRow();
                    if (ordemInfo != null)
                        cancelarOrdem(ordemInfo);
                };
            popupMenu.Items.Add(itemCancelar);
        }

        private void carregarLayouts()
        {
            // Carrega os layouts
            _parametros.LayoutsDevExpress.RecuperarLayouts(this);

            // Cria os estilos condicionais para OrdemStatusEnum
            criarEstilos(
                this.grdvOperacoes, 
                colOperacaoStatus, 
                typeof(OrdemStatusEnum));

            // Estilos de ExecType
            criarEstilos(
                this.grdvMensagens, 
                this.grdvMensagens.Columns.ColumnByFieldName("ExecType"), 
                typeof(OrdemTipoExecucaoEnum));

            // Estilos de OrdRejReason
            criarEstilos(
                this.grdvMensagens,
                this.grdvMensagens.Columns.ColumnByFieldName("OrdRejReason"),
                typeof(OrdemMotivoRejeicaoEnum));

            // Estilos de OrdStatus
            criarEstilos(
                this.grdvMensagens,
                this.grdvMensagens.Columns.ColumnByFieldName("OrdStatus"),
                typeof(OrdemStatusEnum));

            // Carrega as cores
            carregarCoresGridOperacoes();
            carregarCoresGridMensagens();
        }

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

        private void carregarCoresGridOperacoes()
        {
            // Seta as cores informadas
            foreach (StyleFormatCondition estilo in this.grdvOperacoes.FormatConditions)
            {
                // Considera apenas estilos da coluna de status
                if (estilo.Column == colOperacaoStatus)
                {
                    // Verifica se tem o item em parametros
                    CorEnumeradorInfo<OrdemStatusEnum> cor =
                        (from c in _parametros.CoresDosStatusOperacoes
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

        private void carregarCoresGridMensagens()
        {
            // Seta as cores informadas
            foreach (StyleFormatCondition estilo in this.grdvMensagens.FormatConditions)
            {
                // Considera apenas estilos da coluna de status
                if (estilo.Column == this.grdvMensagens.Columns.ColumnByFieldName("ExecType"))
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
                else if (estilo.Column == this.grdvMensagens.Columns.ColumnByFieldName("OrdRejReason"))
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
                else if (estilo.Column == this.grdvMensagens.Columns.ColumnByFieldName("OrdStatus"))
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
        
        private void cmdConfigurar_ItemClick(object sender, ItemClickEventArgs e)
        {
            new FormMensagem("Configurar parâmetros de histórico de operações", _parametros).ShowDialog();
            carregarCoresGridOperacoes();
            carregarCoresGridMensagens();
        }

        private void grdvOperacoes_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                popupMenu.Show(Cursor.Position);
        }

        private void grdvOperacoes_DoubleClick(object sender, EventArgs e)
        {
            OrdemInfo ordemInfo = (OrdemInfo)this.grdvOperacoes.GetFocusedRow();
            //if (ordemInfo != null)
            //    new FormMensagem("Detalhe da Ordem", ordemInfo).ShowDialog();

            // Pede para mostrar o histórico da ordem
            if (ordemInfo != null)
            {
                FormDialog frm = 
                    new FormDialog(                        
                        new ControleInfo() 
                        {  
                            TipoInstancia = typeof(OperacaoDetalheHistorico),
                            Titulo = "Histórico da Operação"
                        }, FormDialogTipoEnum.Fechar);
                frm.Controle.Instancia.ProcessarMensagem(
                    new InicializarHistoricoOperacaoRequest() 
                    { 
                        Historico = ordemInfo.Historico
                    });
                frm.ShowDialog();
            }
        }

        private void grdvMensagens_DoubleClick(object sender, EventArgs e)
        {
            SinalizarExecucaoOrdemRequest mensagem = (SinalizarExecucaoOrdemRequest)this.grdvMensagens.GetFocusedRow();
            if (mensagem != null)
                new FormMensagem("Detalhe da Mensagem", mensagem).ShowDialog();
        }

        #endregion

        private void cmdSalvarLayoutDefault_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Para este tipo de controle, salva os parametros default desta instancia
            _controle.SalvarDefault();
        }

        private void cmdTeste_ItemClick(object sender, ItemClickEventArgs e)
        {
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            servicoInterface.EnviarMensagemParaControle(
                new EnviarMensagemParaControleRequest()
                {
                    ControleTipo = typeof(OperacaoBoleto),
                    CriarCasoNaoEncontrado = true,
                    TituloJanela = "Janela Teste",
                    MensagemRequest =
                        new CarregarOperacaoBoletoRequest()
                        {
                            Bolsa = "BOLSA",
                            Cliente = "CLIENTE",
                            Preco = 999,
                            Quantidade = 999,
                            AtivarJanela = true,
                            Papel = "ABCD",
                            Direcao = OrdemDirecaoEnum.Compra
                        }
                });
            
            
            
            //JanelaInfo janelaInfo = new JanelaInfo();
            //servicoInterface.CriarJanela(janelaInfo);
            //ControleInfo controleInfo = new ControleInfo() 
            //{ 
            //    TipoInstancia = typeof(OperacaoBoleto)
            //};
            //servicoInterface.AdicionarControle(janelaInfo.Id, controleInfo);
        }

    }
}
