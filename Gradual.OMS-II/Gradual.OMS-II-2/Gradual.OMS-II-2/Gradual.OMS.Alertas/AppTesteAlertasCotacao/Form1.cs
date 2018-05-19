using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Alertas.Lib;
using Gradual.OMS.Alertas.Lib.Dados;
using Gradual.OMS.Alertas.Lib.Mensagens;


namespace AppTesteAlertasCotacao
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radbtnPreco.Checked = true;
            radbtnMaiorIgual.Checked = true;

            disableNewAlertaTxts();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            IServicoAlertas servicoAlerta = Ativador.Get<IServicoAlertas>();

            AlertaInfo alerta = new AlertaInfo();
            alerta.IdAlerta = "0";
            alerta.IdCliente = txtIdCliente.Text;
            alerta.Instrumento = txtInstrumento.Text;
            
            if (radbtnPreco.Checked)
                alerta.TipoOperando = Operando.Preco;
            else if (radbtnOscilacao.Checked)
                alerta.TipoOperando = Operando.Oscilacao;
            else if (radbtnMaximo.Checked)
                alerta.TipoOperando = Operando.Maximo;
            else if (radbtnMinimo.Checked)
                alerta.TipoOperando = Operando.Minimo;

            if (radbtnMenorIgual.Checked)
                alerta.TipoOperador = Operador.MenorIgual;
            else if (radbtnMaiorIgual.Checked)
                alerta.TipoOperador = Operador.MaiorIgual;
            else if (radbtnAlcancado.Checked)
                alerta.TipoOperador = Operador.Atingido;

            alerta.Valor = Convert.ToDecimal(txtValor.Text);

            CadastrarAlertaRequest reqCadastrarAlerta = new CadastrarAlertaRequest();
            reqCadastrarAlerta.Alerta = alerta;

            CadastrarAlertaResponse respCadastrarAlerta = servicoAlerta.CadastrarAlerta(reqCadastrarAlerta);

            if (respCadastrarAlerta.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
            {
                txtIdAlerta.Text = respCadastrarAlerta.IdAlerta;
            }
            else
            {
                txtIdAlerta.Text = "Cadastro Não OK. Desc=[" + respCadastrarAlerta.DescricaoErro + "]";
            }
            disableNewAlertaTxts();
        }

        private void btnNovoAlerta_Click(object sender, EventArgs e)
        {
            enableNewAlertaTxts();
        }

        private void disableNewAlertaTxts()
        {
            txtIdAlerta.Enabled = false;
            txtIdCliente.Enabled = false;
            txtInstrumento.Enabled = false;
            txtValor.Enabled = false;
            btnCadastrar.Enabled = false;
            pnlOperador.Enabled = false;
            pnlOperando.Enabled = false;
        }

        private void enableNewAlertaTxts()
        {
            txtIdAlerta.Enabled = true;
            txtIdCliente.Enabled = true;
            txtInstrumento.Enabled = true;
            txtValor.Enabled = true;
            btnCadastrar.Enabled = true;
            pnlOperador.Enabled = true;
            pnlOperando.Enabled = true;
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            if (txtIdClienteListar.Text.Length == 0)
                return;

            btnListar.Enabled = false;
            lstboxAlertas.Items.Clear();
            lstboxAlertas.Refresh();

            ListarAlertasRequest request = new ListarAlertasRequest();
            request.IdCliente = txtIdClienteListar.Text;

            IServicoAlertas servicoAlerta = Ativador.Get<IServicoAlertas>();

            ListarAlertasResponse response = servicoAlerta.ListarAlertas(request);

            if (response.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (response.Alertas.Count == 0)
                {
                    lstboxAlertas.Items.Add("Não há alertas cadastrados para o Cliente [" + request.IdCliente + "]");
                    lstboxAlertas.Refresh();
                }
                else
                {
                    foreach (AlertaInfo alerta in response.Alertas)
                    {
                        StringBuilder info = new StringBuilder();
                        info.Append("IdAlerta=[" + alerta.IdAlerta + "]");
                        info.Append(" Instrumento=[" + alerta.Instrumento + "]");
                        info.Append(" TipoOperador=[" + alerta.TipoOperador + "]");
                        info.Append(" TipoOperando=[" + alerta.TipoOperando + "]");
                        info.Append(" Valor=[" + alerta.Valor + "]");
                        info.Append(" DataCadastro=[" + alerta.DataCadastro + "]");
                        info.Append(" Atingido=[" + (alerta.Atingido ? "Sim" : "Não") + "]");
                        info.Append(" Exibido=[" + (alerta.Exibido ? "Sim" : "Não") + "]");

                        lstboxAlertas.Items.Add(info.ToString());
                    }
                }

                btnListar.Enabled = true;
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (txtIdAlertaExcluir.Text.Length == 0)
                return;

            if (txtIdClienteExcluir.Text.Length == 0)
                return;

            ExcluirAlertaRequest request = new ExcluirAlertaRequest();
            request.IdAlerta = txtIdAlertaExcluir.Text;
            request.IdCliente = txtIdClienteExcluir.Text;
            
            IServicoAlertas servicoAlerta = Ativador.Get<IServicoAlertas>();

            ExcluirAlertaResponse response = servicoAlerta.ExcluirAlerta(request);

            MessageBox.Show("Exclusão executada.", "Exclusão" ,
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            if (txtIdClienteVerificar.Text.Length == 0)
                return;

            btnVerificar.Enabled = false;
            lstboxVerificar.Items.Clear();
            lstboxVerificar.Refresh();

            VerificarAlertasRequest request = new VerificarAlertasRequest();
            request.IdCliente = txtIdClienteVerificar.Text;

            IServicoAlertas servicoAlerta = Ativador.Get<IServicoAlertas>();

            VerificarAlertasResponse response = servicoAlerta.VerificarAlertas(request);

            if (response.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (response.Alertas.Count == 0)
                {
                    lstboxVerificar.Items.Add("Não há alertas cadastrados para o Cliente [" + request.IdCliente + "]");
                    lstboxVerificar.Refresh();
                }
                else
                {
                    foreach (AlertaInfo alerta in response.Alertas)
                    {
                        StringBuilder info = new StringBuilder();
                        info.Append("IdAlerta=[" + alerta.IdAlerta + "]");
                        info.Append(" Instrumento=[" + alerta.Instrumento + "]");
                        info.Append(" TipoOperador=[" + alerta.TipoOperador + "]");
                        info.Append(" TipoOperando=[" + alerta.TipoOperando + "]");
                        info.Append(" Valor=[" + alerta.Valor + "]");
                        info.Append(" DataCadastro=[" + alerta.DataCadastro + "]");
                        info.Append(" Atingido=[" + (alerta.Atingido ? "Sim" : "Não") + "]");
                        info.Append(" Exibido=[" + (alerta.Exibido ? "Sim" : "Não") + "]"); 
                        info.Append(" Cotação=[" + alerta.Cotacao + "]");

                        lstboxVerificar.Items.Add(info.ToString());
                    }
                }

                btnVerificar.Enabled = true;
            }

        }

        private void btnMarcarExibida_Click(object sender, EventArgs e)
        {
            if (txtIdAlertaMarcar.Text.Length == 0)
                return;

            if (txtIdClienteMarcar.Text.Length == 0)
                return;

            MarcarComoExibidoRequest request = new MarcarComoExibidoRequest();
            request.IdCliente = txtIdClienteMarcar.Text;
            List<String> listaAlertas = new List<String>();
            listaAlertas.Add(txtIdAlertaMarcar.Text);
            request.listaIdAlerta = listaAlertas;

            IServicoAlertas servicoAlerta = Ativador.Get<IServicoAlertas>();

            MarcarComoExibidoResponse response = servicoAlerta.MarcarComoExibido(request);

            MessageBox.Show("Ação executada.", "Marcar como exibida",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

    }
}
