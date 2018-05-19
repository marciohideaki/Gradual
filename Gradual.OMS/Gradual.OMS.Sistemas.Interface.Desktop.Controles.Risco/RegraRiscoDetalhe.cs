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
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    /// <summary>
    /// Controle de detalhe de regra de risco
    /// </summary>
    public partial class RegraRiscoDetalhe : XtraUserControl
    {
        /// <summary>
        /// Contexto da interface. Necessário para repassar a
        /// informação da sessão.
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Regra de risco que está sendo alterada
        /// </summary>
        private RegraRiscoInfo _regraRiscoInfo = null;

        /// <summary>
        /// Dicionário com os configs já criados para evitar perda de informações
        /// caso o usuário navegue de uma regra para outra
        /// </summary>
        private Dictionary<Type, object> _configsCriados = new Dictionary<Type, object>();

        /// <summary>
        /// Construtor. Recebe a regra de risco a ser trabalhada
        /// </summary>
        /// <param name="regraRiscoInfo"></param>
        public RegraRiscoDetalhe(RegraRiscoInfo regraRiscoInfo)
        {
            InitializeComponent();

            // Inicializa
            _regraRiscoInfo = regraRiscoInfo;

            // Adiciona o config no dicionario de configs, caso exista
            if (_regraRiscoInfo.Config != null)
                _configsCriados.Add(_regraRiscoInfo.Config.GetType(), _regraRiscoInfo.Config);
            
            // Pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Captura os eventos
            this.Load += new EventHandler(RegraRiscoDetalhe_Load);
        }

        private void RegraRiscoDetalhe_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                carregarTela();
                this.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
            }
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Salva informações
            _regraRiscoInfo.Agrupamento.CodigoAtivo = txtCodigoAtivo.Text != "" ? txtCodigoAtivo.Text : null;
            _regraRiscoInfo.Agrupamento.CodigoAtivoBase = txtCodigoAtivoBase.Text != "" ? txtCodigoAtivoBase.Text : null;
            _regraRiscoInfo.Agrupamento.CodigoBolsa = txtCodigoBolsa.Text != "" ? txtCodigoBolsa.Text : null;
            _regraRiscoInfo.Agrupamento.CodigoPerfilRisco = txtCodigoPerfilRisco.Text != "" ? txtCodigoPerfilRisco.Text : null;
            _regraRiscoInfo.Agrupamento.CodigoSistemaCliente = txtCodigoSistemaCliente.Text != "" ? txtCodigoSistemaCliente.Text : null;
            _regraRiscoInfo.Agrupamento.CodigoUsuario = txtCodigoUsuario.Text != "" ? txtCodigoUsuario.Text : null;
            _regraRiscoInfo.Habilitado = chkHabilitado.Checked;
        }

        private void carregarTela()
        {
            // Carrega lista de regras disponiveis
            IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();
            ListarRegrasDisponiveisResponse listarRegrasResponse = 
                servicoRisco.ListarRegrasDisponiveis(
                    new ListarRegrasDisponiveisResponse());
            foreach (RegraInfo regraInfo in listarRegrasResponse.Regras)
                cmbRegra.Properties.Items.Add(regraInfo);

            // Faz a seleção do combo
            for (int i = 0; i < cmbRegra.Properties.Items.Count; i++)
                if (((RegraInfo)cmbRegra.Properties.Items[i]).TipoRegra == _regraRiscoInfo.TipoRegra)
                    cmbRegra.SelectedIndex = i;

            // Carrega informações
            txtCodigoAtivo.Text = _regraRiscoInfo.Agrupamento.CodigoAtivo;
            txtCodigoAtivoBase.Text = _regraRiscoInfo.Agrupamento.CodigoAtivoBase;
            txtCodigoBolsa.Text = _regraRiscoInfo.Agrupamento.CodigoBolsa;
            txtCodigoPerfilRisco.Text = _regraRiscoInfo.Agrupamento.CodigoPerfilRisco;
            txtCodigoSistemaCliente.Text = _regraRiscoInfo.Agrupamento.CodigoSistemaCliente;
            txtCodigoUsuario.Text = _regraRiscoInfo.Agrupamento.CodigoUsuario;
            chkHabilitado.EditValue = _regraRiscoInfo.Habilitado;

            // Carrega elementos da regra de risco
            ppgRegra.SelectedObject = _regraRiscoInfo;
        }

        private void cmbRegra_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Se tem objeto do propertygrid de config, verifica se deve colocá-lo no dicionário
            if (ppgConfig.SelectedObject != null)
                if (!_configsCriados.ContainsKey(ppgConfig.SelectedObject.GetType()))
                    _configsCriados.Add(ppgConfig.SelectedObject.GetType(), ppgConfig.SelectedObject);

            // Pega o item selecionado
            RegraInfo regra = (RegraInfo)cmbRegra.SelectedItem;

            // Informa na regraRisco
            _regraRiscoInfo.TipoRegra = regra.TipoRegra;

            // Apenas se tem config
            if (regra.TipoConfig != null)
            {
                // Se o tipo do config consta no dicionario, utiliza, senao cria
                if (!_configsCriados.ContainsKey(regra.TipoConfig))
                    _regraRiscoInfo.Config = Activator.CreateInstance(regra.TipoConfig);
                else
                    _regraRiscoInfo.Config = _configsCriados[regra.TipoConfig];

                // Mostra item
                ppgConfig.SelectedObject = _regraRiscoInfo.Config;
                ppgConfig.RetrieveFields();
            }
            else
            {
                // Não mostra nada no config
                ppgConfig.SelectedObject = null;
            }

            // Expande primeira linha do config
            if (ppgConfig.Rows.Count > 0)
                ppgConfig.FullExpandRow(ppgConfig.Rows[0]);

            // Faz refresh da regra
            ppgRegra.SelectedObject = null;
            ppgRegra.SelectedObject = _regraRiscoInfo;
        }
    }
}
