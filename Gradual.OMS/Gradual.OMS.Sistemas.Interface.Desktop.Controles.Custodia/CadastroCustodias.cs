using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Custodia;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Custodia.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Custodia
{
    public partial class CadastroCustodias : XtraUserControl, IControle
    {
        private Controle _item = null;
        private InterfaceContextoOMS _contexto = null;

        public CadastroCustodias()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroCustodias_Load);
        }

        private void CadastroCustodias_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo design
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Carrega lista de usuários
                carregarListaCustodias();

                // Captura eventos
                grdv.DoubleClick += new EventHandler(grdv_DoubleClick);
            }
        }

        private void grdv_DoubleClick(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            CustodiaInfo custodiaInfo = (CustodiaInfo)this.grdv.GetFocusedRow();
            if (custodiaInfo != null)
            {
                // Faz a solicitação do objeto de detalhe
                custodiaInfo = 
                    Ativador.Get<IServicoCustodia>().ReceberCustodia(
                        new ReceberCustodiaRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                            CodigoCustodia = custodiaInfo.CodigoCustodia,
                            CarregarCotacoes = true
                        }).CustodiaInfo;

                // Mostra detalhe da custodia e salva se ok
                CustodiaDetalhe custodiaDetalhe = new CustodiaDetalhe(custodiaInfo);
                FormDialog frm = new FormDialog(custodiaDetalhe, FormDialogTipoEnum.OkCancelar);
                if (frm.ShowDialog() == DialogResult.OK)
                    salvar(custodiaInfo);
            }
        }

        private void carregarListaCustodias()
        {
            // Referencia ao servico de seguranca
            IServicoCustodia servicoCustodia = Ativador.Get<IServicoCustodia>();

            // Pede a lista
            List<CustodiaInfo> custodias =
                ((ConsultarCustodiasResponse)
                    servicoCustodia.ConsultarCustodias(
                        new ConsultarCustodiasRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao
                        })).Custodias;

            // Associa ao grid
            grd.DataSource = custodias;
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
            _item = controle;
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
            return null;
        }

        #endregion

        private void salvar(CustodiaInfo custodiaInfo)
        {
            // Referencia ao servico de seguranca
            IServicoCustodia servicoCustodia = Ativador.Get<IServicoCustodia>();

            // Salva o usuário
            servicoCustodia.SalvarCustodia(
                new SalvarCustodiaRequest()
                {
                    CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                    CustodiaInfo = custodiaInfo
                });

            // Atualiza a lista
            carregarListaCustodias();
        }

        private void cmdAtualizarLista_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Recarrega a lista
            carregarListaCustodias();
        }

        private void cmdAdicionar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Cria nova custodia
            CustodiaInfo info = new CustodiaInfo();

            // Mostra tela de detalhe e salva se ok
            CustodiaDetalhe controleDetalhe = new CustodiaDetalhe(info);
            FormDialog frm = new FormDialog(controleDetalhe, FormDialogTipoEnum.OkCancelar);
            if (frm.ShowDialog() == DialogResult.OK)
                salvar(info);
        }
    }
}
