using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.IntegracaoCMRocket.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.IntegracaoCMRocket.Lib.Mensagens;
using System.Reflection;

namespace AppTesteRocket
{
    public partial class FormTeste : Form
    {
        public FormTeste()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Gradual.IntegracaoCMRocket.ServicoIntegracaoCMRocket lServico = new Gradual.IntegracaoCMRocket.ServicoIntegracaoCMRocket();
            Gradual.IntegracaoCMRocket.Lib.Mensagens.ObterEvolucaoProcessoRequest lRequest = new Gradual.IntegracaoCMRocket.Lib.Mensagens.ObterEvolucaoProcessoRequest();
            
            lRequest.IDCapivara = Int32.Parse(txtPesquisa_CodigoCapivara.Text);

            lServico.ObterEvolucaoProcesso(lRequest);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Gradual.IntegracaoCMRocket.ServicoIntegracaoCMRocket lServico = new Gradual.IntegracaoCMRocket.ServicoIntegracaoCMRocket();
            
            ObterRelatorioDetalhadoRequest pParametros = new ObterRelatorioDetalhadoRequest();
            
            lServico.ObterRelatorios(pParametros);
        }

        private void btnEnviarCadastro_Click(object sender, EventArgs e)
        {
            Gradual.IntegracaoCMRocket.Lib.Dados.CMRocketFields lDados = new Gradual.IntegracaoCMRocket.Lib.Dados.CMRocketFields();

            lDados.NOME_COMPLETO        = txtDados_NomeCompleto.Text;
            lDados.SEXO                 = txtDados_Sexo.Text;
            
            lDados.NOME_MAE             = txtDados_NomeMae.Text;
            lDados.NOME_PAI             = txtDados_NomePai.Text;
            lDados.CPF                  = txtDados_CPF.Text;

            lDados.DATA_NASCIMENTO = txtDados_DataNascimento.Text;

            lDados.NUMERO_RG                = txtDados_NumeroRG.Text; ;
            lDados.DATA_EXPEDICAO_RG        = txtDados_DataExpedicaoRg.Text;
            lDados.ORGAO_EMISSOR_RG         = txtDados_EmissorRG.Text;
            lDados.UF_EXPEDICAO_RG          = txtDados_EstadoRG.Text;

            lDados.NUMERO_REGISTRO_CNH      = txtDados_NumeroCNH.Text;
            lDados.NUMERO_SEGURANCA_CNH     = txtDados_SegurancaCNH.Text;

            lDados.LOGRADOURO_COMERCIAL     = txtComercial_Logradouro.Text;
            lDados.NUMERO_COMERCIAL         = txtComercial_Numero.Text;
            lDados.COMPLEMENTO_COMERCIAL    = txtComercial_Complemento.Text;
            lDados.BAIRRO_COMERCIAL         = txtComercial_Bairro.Text;
            lDados.CIDADE_COMERCIAL         = txtComercial_Cidade.Text;
            lDados.ESTADO_COMERCIAL         = txtComercial_UF.Text;
            lDados.CEP_COMERCIAL            = txtComercial_Cep.Text;

            lDados.LOGRADOURO_RESIDENCIAL   = txtResidencial_Logradoruo.Text;
            lDados.NUMERO_RESIDENCIAL       = txtResidencial_Numero.Text;
            lDados.COMPLEMENTO_RESIDENCIAL  = txtResidencial_Complemento.Text;
            lDados.BAIRRO_RESIDENCIAL       = txtResidencial_Bairro.Text;
            lDados.CIDADE_RESIDENCIAL       = txtResidencial_Cidade.Text;
            lDados.ESTADO_RESIDENCIAL       = txtResidencial_UF.Text;
            lDados.CEP_RESIDENCIAL          = txtResidencial_Cep.Text;

            lDados.EMAIL                    = txtContato_Email.Text;

            lDados.DDD_CELULAR              = txtContato_DDDCelular.Text;
            lDados.TELEFONE_CELULAR         = txtContato_NumeroCelular.Text;
            
            lDados.DDD_TELEFONE_RESIDENCIAL = txtContato_DDDResidencial.Text;
            lDados.TELEFONE_RESIDENCIAL     = txtContato_NumeroResidencial.Text;
            
            lDados.DDD_COMERCIAL            = txtContato_DDDComercial.Text;
            lDados.TELEFONE_COMERCIAL       = txtContato_NumeroComercial.Text;
            
            lDados.NUMERO_AGENCIA = "7054";
            lDados.NUMERO_BANCO = "BANCO ITAU";


            Gradual.IntegracaoCMRocket.Lib.Mensagens.ValidarCadastroRequest lRequisicao = new Gradual.IntegracaoCMRocket.Lib.Mensagens.ValidarCadastroRequest();

            lRequisicao.CamposRocket = lDados;

            com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG lDadosEnvio = new com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG();


            CopyPropertiesAsPossible(lDados, lDadosEnvio);

            IServicoIntegracaoCMRocket servico = Ativador.Get<IServicoIntegracaoCMRocket>();

            ValidarCadastroResponse response = servico.ValidarCadastro(lRequisicao);
            
        }

        public static void CopyPropertiesAsPossible(object source, object destination)
        {
            if (source == null || destination == null)
                return;

            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                try
                {
                    foreach (PropertyInfo prop1 in destination.GetType().GetProperties())
                    {
                        if (prop1.Name.Equals(prop.Name) && prop1.PropertyType.Equals(prop.PropertyType))
                        {
                            object xxx = source.GetType().GetProperty(prop1.Name).GetValue(source, null);
                            if (xxx != null)
                                prop1.SetValue(destination, xxx, null);
                        }
                    }

                }
                catch (Exception ex)
                { }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

    }
}
