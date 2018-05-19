using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Gradual.OMS.Library.Servicos;
using Gradual.IntegracaoCMRocket.Lib;
using Gradual.IntegracaoCMRocket.Lib.Mensagens;

namespace AppTesteRocket
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG sunda = new com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG();

            // Gambiarra master para preencher todos os campos
            // do car**** do SOAP, mesmo se nao viermos a utilizar
            foreach (PropertyInfo prop in sunda.GetType().GetProperties())
            {
                if (prop.PropertyType.Equals(typeof(String)))
                    prop.SetValue(sunda, String.Empty,null);
            }

            sunda.BAIRRO_COMERCIAL = "Vila Nova Conceicao";
            sunda.BAIRRO_RESIDENCIAL = "Jardins";
            sunda.CEP_COMERCIAL = "09110-160";
            sunda.CEP_RESIDENCIAL = "04634-020";
            sunda.CIDADE_COMERCIAL = "Sao Paulo";
            sunda.CIDADE_RESIDENCIAL = "Santo Andre";
            //sunda.CODIGO_RJ_RG = "1";
            sunda.COMPLEMENTO_COMERCIAL = "CJ 1";
            sunda.COMPLEMENTO_RESIDENCIAL = "Apto 69";
            sunda.CPF = "280.522.228-85";
            sunda.DATA_EXPEDICAO_RG = "01/01/2001";
            sunda.DATA_NASCIMENTO = "01/01/1920";
            sunda.DDD_CELULAR = "011";
            sunda.DDD_COMERCIAL = "012";
            sunda.DDD_TELEFONE_RESIDENCIAL = "013";
            //sunda.DIGITO_RG = "1";
            //sunda.DOCUMENTO_ORIGEM_1_RG = "1";
            //sunda.DOCUMENTO_ORIGEM_2_RG = "1";
            sunda.EMAIL = "abc@bcd.com.br";
            sunda.ESTADO_COMERCIAL = "SP";
            sunda.ESTADO_RESIDENCIAL = "RS";
            //sunda.FLAG_PROC_REPR = "";
            sunda.LOGRADOURO_COMERCIAL = "Av Luis Carlos Berrini";
            sunda.LOGRADOURO_RESIDENCIAL = "Av dos Autonomistas";
            sunda.NOME_COMPLETO = "JESUS CRISTO DA SILVA";
            sunda.NOME_MAE = "MARIA DA SILVA";
            sunda.NOME_PAI = "JOSE DA SILVA";
            sunda.NUMERO_AGENCIA = "1851";
            sunda.NUMERO_BANCO = "BANCO ITAU";
            sunda.NUMERO_COMERCIAL = "1000";
            sunda.NUMERO_REGISTRO_CNH = "4825364782578";
            sunda.NUMERO_RESIDENCIAL = "1001";
            sunda.NUMERO_RG = "123.4567-8";
            //sunda.NUMERO_SEGURANCA_CNH = "";
            sunda.ORGAO_EMISSOR_RG = "SSP";
            sunda.SEXO = "F";
            sunda.TELEFONE_CELULAR = "87854321";
            sunda.TELEFONE_COMERCIAL = "33728300";
            sunda.TELEFONE_RESIDENCIAL = "50317708";
            sunda.UF_EXPEDICAO_RG = "SP";


            com.cmsw.wsrocket.RocketProcessWS rckCli = new com.cmsw.wsrocket.RocketProcessWS();

            rckCli.AllowAutoRedirect = true;
            string yy = rckCli.Url;

            rckCli.Url = "http://wsrocket.cmsw.com/Rocket_33918160000173/services";


            //CMSoftware.Rocket.statusProcess stproc = new CMSoftware.Rocket.statusProcess();

            //stproc.hash = "aaa";
            //stproc.ticket = "ticket";

            com.cmsw.wsrocket.ProcessHeaderVo xxx = new com.cmsw.wsrocket.ProcessHeaderVo();

            xxx.empresa = "33918160000173";
            xxx.fluxo = "WS_VALIDACAO_CADASTRAL_HOMOLOG";
            xxx.senha = "teste";
            xxx.usuario = "teste";
            xxx.hash = "";
            xxx.ticket = "";

            sunda.header = xxx;

            com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOGResponse rsp = rckCli.WS_VALIDACAO_CADASTRAL_HOMOLOG(sunda);

            com.cmsw.wsrocket.rocketWSReturn ret = rsp.retorno;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Gradual.IntegracaoCMRocket.Lib.Dados.CMRocketFields sunda = new Gradual.IntegracaoCMRocket.Lib.Dados.CMRocketFields();

            //sunda.BAIRRO_COMERCIAL = "Vila Nova Conceicao";
            //sunda.BAIRRO_RESIDENCIAL = "Jardins";
            //sunda.CEP_COMERCIAL = "09110-160";
            //sunda.CEP_RESIDENCIAL = "04634-020";
            //sunda.CIDADE_COMERCIAL = "Sao Paulo";
            //sunda.CIDADE_RESIDENCIAL = "Santo Andre";
            //sunda.CODIGO_RJ_RG = "1";
            //sunda.COMPLEMENTO_COMERCIAL = "CJ 1";
            //sunda.COMPLEMENTO_RESIDENCIAL = "Apto 69";
            sunda.CPF = "26505057889";
            sunda.DATA_EXPEDICAO_RG = "01/01/2001";
            sunda.DATA_NASCIMENTO = "01/01/1920";
            sunda.DDD_CELULAR = "011";
            sunda.DDD_COMERCIAL = "012";
            sunda.DDD_TELEFONE_RESIDENCIAL = "013";
            //sunda.DIGITO_RG = "1";
            //sunda.DOCUMENTO_ORIGEM_1_RG = "1";
            //sunda.DOCUMENTO_ORIGEM_2_RG = "1";
            sunda.EMAIL = "abc@bcd.com.br";
            sunda.ESTADO_COMERCIAL = "SP";
            sunda.ESTADO_RESIDENCIAL = "RS";
            //sunda.FLAG_PROC_REPR = "";
            sunda.LOGRADOURO_COMERCIAL = "Av Luis Carlos Berrini";
            sunda.LOGRADOURO_RESIDENCIAL = "Av dos Autonomistas";
            sunda.NOME_COMPLETO = "MARCELO FUJIMORI";
            sunda.NOME_MAE = "MARIA DA SILVA";
            sunda.NOME_PAI = "JOSE DA SILVA";
            sunda.NUMERO_AGENCIA = "1851";
            sunda.NUMERO_BANCO = "BANCO ITAU";
            sunda.NUMERO_COMERCIAL = "1000";
            sunda.NUMERO_REGISTRO_CNH = "4825364782578";
            sunda.NUMERO_RESIDENCIAL = "1001";
            sunda.NUMERO_RG = "123.4567-8";
            //sunda.NUMERO_SEGURANCA_CNH = "";
            sunda.ORGAO_EMISSOR_RG = "SSP";
            sunda.SEXO = "F";
            sunda.TELEFONE_CELULAR = "87854321";
            sunda.TELEFONE_COMERCIAL = "33728300";
            sunda.TELEFONE_RESIDENCIAL = "50317708";
            sunda.UF_EXPEDICAO_RG = "SP";


            //sunda.BAIRRO_COMERCIAL = "Vila Nova Conceicao";
            //sunda.BAIRRO_RESIDENCIAL = "Mirandopolis    ";
            //sunda.CEP_COMERCIAL = "09110-160";
            //sunda.CEP_RESIDENCIAL = "04048-000";
            //sunda.CIDADE_COMERCIAL = "Sao Paulo";
            //sunda.CIDADE_RESIDENCIAL = "Sao Paulo";
            ////sunda.CODIGO_RJ_RG = "1";
            //sunda.COMPLEMENTO_COMERCIAL = "CJ 6";
            //sunda.COMPLEMENTO_RESIDENCIAL = "Apto 13";
            //sunda.CPF = "15095217805";
            //sunda.DATA_EXPEDICAO_RG = "01/01/2001";
            //sunda.DATA_NASCIMENTO = "22/06/1970";
            //sunda.DDD_CELULAR = "011";
            //sunda.DDD_COMERCIAL = "011";
            //sunda.DDD_TELEFONE_RESIDENCIAL = "011";
            ////sunda.DIGITO_RG = "1";
            ////sunda.DOCUMENTO_ORIGEM_1_RG = "1";
            ////sunda.DOCUMENTO_ORIGEM_2_RG = "1";
            //sunda.EMAIL = "aponso@uol.com.br";
            //sunda.ESTADO_COMERCIAL = "SP";
            //sunda.ESTADO_RESIDENCIAL = "SP";
            ////sunda.FLAG_PROC_REPR = "";
            //sunda.LOGRADOURO_COMERCIAL = "Av Presidente Juscelino Kubitscheck";
            //sunda.LOGRADOURO_RESIDENCIAL = "Rua das Rosas";
            //sunda.NOME_COMPLETO = "ALEXANDRE PONSO DE TOLEDO PIZA";
            //sunda.NOME_MAE = "SILVIA PONSO DE TOLEDO PIZA";
            //sunda.NOME_PAI = "LIRAUCIO DE TOLEDO PIZA";
            //sunda.NUMERO_AGENCIA = "7054";
            //sunda.NUMERO_BANCO = "BANCO ITAU";
            //sunda.NUMERO_COMERCIAL = "50";
            //sunda.NUMERO_REGISTRO_CNH = "";
            //sunda.NUMERO_RESIDENCIAL = "95";
            //sunda.NUMERO_RG = "19.204.918-5";
            ////sunda.NUMERO_SEGURANCA_CNH = "";
            //sunda.ORGAO_EMISSOR_RG = "SSP";
            //sunda.SEXO = "M";
            //sunda.TELEFONE_CELULAR = "985337313";
            //sunda.TELEFONE_COMERCIAL = "33728300";
            //sunda.TELEFONE_RESIDENCIAL = "50718031";
            //sunda.UF_EXPEDICAO_RG = "SP";

            Gradual.IntegracaoCMRocket.Lib.Mensagens.ValidarCadastroRequest request = new Gradual.IntegracaoCMRocket.Lib.Mensagens.ValidarCadastroRequest();

            request.CamposRocket = sunda;

            com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG munga = new com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG();


            CopyPropertiesAsPossible(sunda, munga);

            IServicoIntegracaoCMRocket servico = Ativador.Get<IServicoIntegracaoCMRocket>();

            ValidarCadastroResponse response = servico.ValidarCadastro(request);
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
                            object xxx = source.GetType().GetProperty(prop1.Name).GetValue(source, null );
                            if ( xxx != null )
                                prop1.SetValue(destination, xxx, null);
                        }
                    }

                }
                catch (Exception ex)
                { }
            }
        }

        private void btObterEvolucao_Click(object sender, EventArgs e)
        {
            int idCapivara = Convert.ToInt32(txtIDCapivara.Text);

            Gradual.IntegracaoCMRocket.Lib.Mensagens.ObterEvolucaoProcessoRequest request = new ObterEvolucaoProcessoRequest();

            request.IDCapivara = idCapivara;

            IServicoIntegracaoCMRocket servico = Ativador.Get<IServicoIntegracaoCMRocket>();

            ObterEvolucaoProcessoResponse response = servico.ObterEvolucaoProcesso(request);
        }

        private void btDetalhe_Click(object sender, EventArgs e)
        {
            int idCapivara = Convert.ToInt32(txtIDCapivara.Text);

            Gradual.IntegracaoCMRocket.Lib.Mensagens.ObterDetalheRelatorioRequest request = new ObterDetalheRelatorioRequest();

            request.idCapivara = idCapivara;

            IServicoIntegracaoCMRocket servico = Ativador.Get<IServicoIntegracaoCMRocket>();

            ObterDetalheRelatorioResponse response = servico.ObterDetalhamentoRelatorio(request);
        }
    }
}
