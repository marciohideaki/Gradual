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
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    public partial class CadastroPerfilRisco : XtraUserControl, IControle
    {
        private Controle _item = null;
        private InterfaceContextoOMS _contexto = null;
        private CadastroPerfilRiscoParametros _parametros = new CadastroPerfilRiscoParametros();

        public CadastroPerfilRisco()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroPerfilRisco_Load);
        }

        private void CadastroPerfilRisco_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo design
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Carrega lista de usuários
                carregarLista();

                // Carrega os layouts dos controles
                _parametros.LayoutsDevExpress.RecuperarLayouts(this);

                // Captura eventos
                grdv.DoubleClick += new EventHandler(grdv_DoubleClick);
            }
        }

        private void grdv_DoubleClick(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            PerfilRiscoInfo info = (PerfilRiscoInfo)this.grdv.GetFocusedRow();
            if (info != null)
            {
                // Mostra detalhe do usuario e salva se ok
                PerfilRiscoDetalhe controleDetalhe = new PerfilRiscoDetalhe(info);
                controleDetalhe.CarregarParametros(_parametros.ParametrosPerfilRiscoDetalhe, EventoManipulacaoParametrosEnum.Persistencia);
                FormDialog frm = new FormDialog(controleDetalhe, FormDialogTipoEnum.OkCancelar);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Salva informações do perfil de risco
                    salvar(info);

                    // Pede para o controle salvar demais informações
                    controleDetalhe.SalvarTabs();
                }
                
                // Salva layout da tela de detalhe
                _parametros.ParametrosPerfilRiscoDetalhe = 
                    (PerfilRiscoDetalheParametros)
                        controleDetalhe.SalvarParametros(EventoManipulacaoParametrosEnum.Persistencia);
            }
        }

        private void carregarLista()
        {
            // Guarda eventual selecao
            int selecao = grdv.FocusedRowHandle;

            // Referencia ao servico de seguranca
            IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();

            // Pede a lista
            List<PerfilRiscoInfo> lista =
                ((ListarPerfisRiscoResponse)
                    servicoRisco.ListarPerfisRisco(
                        new ListarPerfisRiscoRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao
                        })).Resultado;

            // Associa ao grid
            grd.DataSource = lista;

            // Mantem selecao anterior
            grdv.FocusedRowHandle = selecao;
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
            _item = controle;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Salva layouts
            _parametros.LayoutsDevExpress.SalvarLayout(this.grdv);

            // Retorna
            return _parametros;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            // Seta a classe de parametros. Será utilizada no load do controle
            CadastroPerfilRiscoParametros parametros2 = parametros as CadastroPerfilRiscoParametros;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion

        private void salvar(PerfilRiscoInfo info)
        {
            // Referencia ao servico de seguranca
            IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();

            // Salva o usuário
            servicoRisco.SalvarPerfilRisco(
                new SalvarPerfilRiscoRequest()
                {
                    CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                    PerfilRiscoInfo = info
                });

            // Atualiza a lista
            carregarLista();
        }

        private void cmdAdicionar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Cria novo perfil
            PerfilRiscoInfo info = new PerfilRiscoInfo();

            // Mostra tela de detalhe e salva se ok
            PerfilRiscoDetalhe controleDetalhe = new PerfilRiscoDetalhe(info);
            controleDetalhe.CarregarParametros(_parametros.ParametrosPerfilRiscoDetalhe, EventoManipulacaoParametrosEnum.Persistencia);
            FormDialog frm = new FormDialog(controleDetalhe, FormDialogTipoEnum.OkCancelar);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Salva objeto
                salvar(info);
                
                // Salva layout da tela de detalhe
                _parametros.ParametrosPerfilRiscoDetalhe =
                    (PerfilRiscoDetalheParametros)
                        controleDetalhe.SalvarParametros(EventoManipulacaoParametrosEnum.Persistencia);
            }
        }

        private void cmdRemover_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Tenta pegar o objeto selecionado
            PerfilRiscoInfo info = (PerfilRiscoInfo)this.grdv.GetFocusedRow();
            if (info != null)
            {
                // Referencia ao servico de risco
                IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();

                // Solicita remoção
                servicoRisco.RemoverPerfilRisco(
                    new RemoverPerfilRiscoRequest()
                    {
                        CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                        CodigoPerfilRisco = info.CodigoPerfilRisco
                    });

                // Atualiza a lista
                carregarLista();
            }
        }
    }
}
