using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Persistencia
{
    public partial class CadastroPersistencia : DevExpress.XtraEditors.XtraUserControl, IControle
    {
        private IServicoPersistencia _servicoPersistencia = Ativador.Get<IServicoPersistencia>();

        private Controle _item = null;

        private CadastroPersistenciaParametros _parametros = new CadastroPersistenciaParametros();

        public CadastroPersistencia()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroPersistencia_Load);
        }

        private void CadastroPersistencia_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                // Carrega os layouts dos controles
                _parametros.LayoutsDevExpress.RecuperarLayouts(this);

                // Inicializa o controle
                inicializarControle();
            }
        }

        private void inicializarControle()
        {
            // Carrega os tipos de objetos
            carregarTipoObjetos();

            // Lista tipos de comparacao
            foreach (string tipoComparacao in Enum.GetNames(typeof(CondicaoTipoEnum)))
                cmbTipoComparacao.Properties.Items.Add(tipoComparacao);
        }

        private void carregarTipoObjetos()
        {
            // Apenas se tem o serviço de persistencia aqui
            if (_servicoPersistencia != null)
            {
                // Lista os tipos de objetos da persistencia
                cmbTipoObjeto.Properties.Items.Clear();
                List<Type> tipos = _servicoPersistencia.ListarTipos(new ListarTiposRequest()).Resultado;
                foreach (Type tipo in tipos)
                    cmbTipoObjeto.Properties.Items.Add(tipo);
            }
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
            CadastroPersistenciaParametros parametros2 = parametros as CadastroPersistenciaParametros;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion

        private void cmdFiltroListar_Click(object sender, EventArgs e)
        {
            consultar();
        }

        private void consultar()
        {
            // Se houver, guarda o objeto selecionado
            int itemSelecionado = lstResultado.SelectedIndex;

            // Pega o tipo do objeto selecionado
            Type tipoObjeto = (Type)cmbTipoObjeto.EditValue;
            Type tipoMensagem = typeof(ConsultarObjetosRequest<>);
            Type tipoMensagemG = tipoMensagem.MakeGenericType(tipoObjeto);
            Type tipoServicoPersistencia = typeof(IPersistencia);
            MethodInfo miConsultarObjetos = tipoServicoPersistencia.GetMethod("ConsultarObjetos");
            MethodInfo miConsultarObjetosG = miConsultarObjetos.MakeGenericMethod(tipoObjeto);

            // Cria filtro
            List<CondicaoInfo> condicoes = new List<CondicaoInfo>();
            foreach (object item in lstFiltro.Items)
                condicoes.Add((CondicaoInfo)item);

            // Cria mensagem e seta o parametro do objeto
            object mensagem = Activator.CreateInstance(tipoMensagemG);
            tipoMensagemG.InvokeMember("Condicoes", BindingFlags.SetProperty, null, mensagem, new object[] { condicoes });

            // Faz a chamada
            object retorno = miConsultarObjetosG.Invoke(_servicoPersistencia, new object[] { mensagem });

            // Mostra na lista
            lstResultado.DataSource = retorno.GetType().InvokeMember("Resultado", BindingFlags.GetProperty, null, retorno, null);

            // Se havia selecao anterior, tenta selecionar novamente
            if (itemSelecionado >= 0)
                lstResultado.SelectedIndex = itemSelecionado;
        }

        private void lstResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            ppg.SelectedObject = lstResultado.SelectedItem;
            ppg.RetrieveFields();
        }

        private void cmdObjetoSalvar_Click(object sender, EventArgs e)
        {
            // Pega o objeto selecionado e o tipo
            object objeto = ppg.SelectedObject;
            Type tipoObjeto = (Type)cmbTipoObjeto.SelectedItem;

            // Pega o método consultar normal 
            Type tipoMensagem = typeof(SalvarObjetoRequest<>);
            Type tipoMensagemG = tipoMensagem.MakeGenericType(tipoObjeto);
            Type tipoServicoPersistencia = typeof(IPersistencia);
            MethodInfo miSalvarObjeto = tipoServicoPersistencia.GetMethod("SalvarObjeto");
            MethodInfo miSalvarObjetoG = miSalvarObjeto.MakeGenericMethod(tipoObjeto);

            // Cria mensagem e seta o parametro do objeto
            object mensagem = Activator.CreateInstance(tipoMensagemG);
            tipoMensagemG.InvokeMember("Objeto", BindingFlags.SetProperty, null, mensagem, new object[] { objeto });

            // Faz a chamada
            object retorno = miSalvarObjetoG.Invoke(_servicoPersistencia, new object[] { mensagem });

            // Recarrega a lista
            consultar();
        }

        private void cmdFiltroNovo_Click(object sender, EventArgs e)
        {
            novoFiltro();
        }

        private void novoFiltro()
        {
            lstFiltro.SelectedIndex = -1;

            cmbPropriedade.SelectedIndex = -1;
            cmbTipoComparacao.SelectedIndex = -1;
            txtValor.Text = "";
        }

        private void cmbTipoObjeto_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarPropriedades();

            lstFiltro.Items.Clear();
            novoFiltro();
        }

        private void carregarPropriedades()
        {
            cmbPropriedade.Properties.Items.Clear();
            Type tipoObjeto = (Type)cmbTipoObjeto.SelectedItem;
            foreach (PropertyInfo pi in tipoObjeto.GetProperties())
                cmbPropriedade.Properties.Items.Add(pi.Name);
        }

        private void cmdFiltroSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                CondicaoInfo ci = null;
                if (lstFiltro.SelectedItem != null)
                    ci = (CondicaoInfo)lstFiltro.SelectedItem;
                else
                    ci = new CondicaoInfo();

                ci.Propriedade = (string)cmbPropriedade.SelectedItem;
                ci.TipoCondicao = (CondicaoTipoEnum)Enum.Parse(typeof(CondicaoTipoEnum), (string)cmbTipoComparacao.SelectedItem);

                Type tipoObjeto = (Type)cmbTipoObjeto.SelectedItem;
                PropertyInfo pi = tipoObjeto.GetProperty(ci.Propriedade);
                ci.Valores = new object[] { Convert.ChangeType(txtValor.Text, pi.PropertyType) };

                if (lstFiltro.SelectedItem == null)
                    lstFiltro.Items.Add(ci);

                novoFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro ao salvar filtro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CondicaoInfo ci = (CondicaoInfo)lstFiltro.SelectedItem;

            if (ci != null)
            {
                cmbPropriedade.SelectedItem = ci.Propriedade;
                cmbTipoComparacao.SelectedItem = ci.TipoCondicao.ToString();
                txtValor.Text = ci.Valores[0].ToString();
            }
        }

        private void cmdFiltroRemover_Click(object sender, EventArgs e)
        {
            if (lstFiltro.SelectedIndex >= 0)
                lstFiltro.Items.RemoveAt(lstFiltro.SelectedIndex);
        }

        private void cmdResultadoNovo_Click(object sender, EventArgs e)
        {
            lstResultado.SelectedIndex = -1;

            Type tipoObjeto = (Type)cmbTipoObjeto.SelectedItem;
            ppg.SelectedObject = Activator.CreateInstance(tipoObjeto);
        }

        private void cmdResultadoRemover_Click(object sender, EventArgs e)
        {
            if (lstResultado.SelectedItem != null)
                if (MessageBox.Show("Confirma a exclusão deste objeto?", "Exclusão de objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Pega o objeto selecionado e o tipo
                    object objeto = ppg.SelectedObject;
                    Type tipoObjeto = (Type)cmbTipoObjeto.SelectedItem;

                    // Pega o método consultar normal 
                    Type tipoMensagem = typeof(RemoverObjetoRequest<>);
                    Type tipoMensagemG = tipoMensagem.MakeGenericType(tipoObjeto);
                    Type tipoServicoPersistencia = typeof(IPersistencia);
                    MethodInfo miRemoverObjeto = tipoServicoPersistencia.GetMethod("RemoverObjeto");
                    MethodInfo miRemoverObjetoG = miRemoverObjeto.MakeGenericMethod(tipoObjeto);

                    // Cria mensagem e seta o parametro do objeto
                    object mensagem = Activator.CreateInstance(tipoMensagemG);
                    tipoMensagemG.InvokeMember("CodigoObjeto", BindingFlags.SetProperty, null, mensagem, new object[] { ((ICodigoEntidade)objeto).ReceberCodigo() });

                    // Faz a chamada
                    object retorno = miRemoverObjetoG.Invoke(_servicoPersistencia, new object[] { mensagem });

                    // Recarrega a lista
                    consultar();
                }
        }

        private void cmdTipoObjetoAtualizar_Click(object sender, EventArgs e)
        {
            carregarTipoObjetos();
        }
    }
}
