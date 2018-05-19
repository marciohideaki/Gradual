using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.Threading;
using Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO;
using System.Xml.Linq;
using Ionic.Zip;
using System.Configuration;
using Gradual.OMS.InvXX.Fundos.Lib.ITAUUNIBANCO;
using Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO.Info;
using System.Net;
using Gradual.OMS.InvXX.Fundos.Lib.FINANCIAL;
using System.ServiceModel;


namespace Gradual.OMS.InvXX.Fundos.ITAUUNIBANCO
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CotistaPosicaoItau : IServicoControlavel
    {
        #region Atributos
        public bool _bKeepRunningItau = false;
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private WaitOrTimerCallback ThreadReset = null;
        private AutoResetEvent lThreadEvent = new AutoResetEvent(false);
        #endregion

        #region Propriedades
        private string UsuarioFinancial
        {
            get { return ConfigurationManager.AppSettings["UsuarioFinancial"].ToString(); }
        }

        private string SenhaFinancial
        {
            get { return ConfigurationManager.AppSettings["SenhaFinancial"].ToString(); }
        }

        private Int64 IntervalorImportacaoPosicaoItau
        {
            get { return Int64.Parse(ConfigurationManager.AppSettings["intervaloImportacaoPosicaoItau"]); }
        }

        private string GetCodigoGestorItau
        {
            get { return ConfigurationManager.AppSettings["CodigoGestorItau"].ToString(); }
        }

        private string GetUsuarioItau
        {
            get { return ConfigurationManager.AppSettings["UsuarioItau"].ToString(); }
        }

        private string GetSenhaItau
        {
            get { return ConfigurationManager.AppSettings["SenhaItau"].ToString(); }
        }
        private string HorariosImportacaoPosicao
        {
            get { return ConfigurationManager.AppSettings["HorariosImportacaoPosicaoITAU"].ToString(); }
        }

        private ServicoStatus _ServicoStatus { set; get; }
        #endregion

        #region Métodos
        private static XElement FindParameter(XElement element, string type)
        {
            return element.Elements("param").SingleOrDefault(p => (string)p.Attribute("id") == type);
        }

        public void ThreadExportacaoImportacaoCotistaItau()
        {
            try
            {
                int lContaBovespa = 39842;

                ImportacaoItauDbLib lDb = new ImportacaoItauDbLib();

                ClienteCotistaItauInfo lCotista = lDb.SelecionaNovoCotistaItau(lContaBovespa);

                string EBUSINESSID = this.GetUsuarioItau;
                string SENHA       = this.GetSenhaItau;
                string CDBANCLI    = this.GetCodigoGestorItau;
                string AGENCIA     = "0000";
                string CONTA       = "000000000";
                string DAC         = "0";
                string NMCLI       = lCotista.NomeCliente.PadRight(30, ' ');                           //  "RAFAEL SANCHES GARCIA";
                string DTNASCL8    = lCotista.DataNascimento.ToString("yyyyMMdd");                     //  "19851002";
                string PES         = lCotista.CodigoTipoPessoa;                                        //  "F";
                string IDCGCCPF    = lCotista.DsCpfCnpj.PadLeft(15, '0');                              //  "000033414115859";
                string TPDOCIDE    = GetTipoDocumento(lCotista.TipoDocumento).PadRight(5, ' ');        //  "RG";
                string IDDOCLI     = lCotista.NumeroDocumento;                                         //  "308645850";
                string DTDOCEXP    = lCotista.DataExpedicaoDocumento.ToString("yyyyMMdd");             //  "20111221";
                string IDORGEMI    = GetOrgaosEmissoresItau(lCotista.OrgaoEmissorDocumento).PadRight(7, ' ');                    //  "SSP";
                string IDESTEMI    = lCotista.EstadoEmissorDocumento;                                  //  "SP";
                string ICIMP       = GetCodigoTributacao(lCotista.CodigoTipoCliente);                  //  "I"; //--> GetCodigoTributacao
                string SITLEG      = GetSituacaoLegal(lCotista.DataNascimento, lCotista.Emancipado);   //  "L"; //--> GetSituacaoLegal
                string CODSEX      = lCotista.CodigoSexo;                                              //  "M";
                string CDESTCIV    = GetEstadoCivil(lCotista.CodigoEstadoCivil);                       //  "4"; //--> GetEstadoCivil
                string CDRAMOAT    = (lCotista.CodigoTipoPessoa == "J") ? GetPorteEmpresa(lCotista.CodigoAtividadePessoaJuridica) + GetSetorAtuacao(lCotista.CodigoAtividadePessoaJuridica) + "9999000" : "           ";//lCotista.CodigoAtividadePessoaJuridica;                           //  "           " ;//--> GetPorteEmpresa+GetSetorAtuacao;
                string CDPROATI    = (lCotista.CodigoTipoPessoa == "F") ? lCotista.CodigoAtividadePessoaFisica : "   ";                             //  "999"; //--> GetProfissoesPF;
                string CDFCONST    = (lCotista.CodigoTipoPessoa == "J") ? lCotista.CodigoFormaConstituicaoEmpresa : "  ";                          //  "  ";
                string ICLEC       = lCotista.TipoEnderecoCorrespondencia;                             //  "C";
                string CDREMLEC    = lCotista.CodigoTipRemessa;                                        //  "C"; //--> C - Correio R - Retirada
                string DDD         = lCotista.DDD.PadLeft(4, '0');                                      //  "0012";
                string TEL8        = lCotista.NumeroTelResidencial.PadLeft(10, '0');                    //  "0065656500";
                string RAMAL       = lCotista.NUmeroRamal.PadLeft(6, '0');                              //  "000000";
                string EMAIL       = lCotista.Email.PadRight(80, ' ');                                  //  "rsgarcia.desenv@gmail.com";
                string IDASSESS    = lCotista.Assessor.PadLeft(5, '0');                                 //  "00700";
                string IDCLITER    = lCotista.CodigoBovespa.PadRight(25, ' ');                           //  "0000000000000000000031940";
                string ICEXTM      = lCotista.EmissaoExtratoMensal.ToString();                         //  "N";
                string ICCFM       = lCotista.EmissaoAvisoConfirmacaoMovimentacao.ToString();          //  "N";
                string VRENDFAM    = Convert.ToDecimal(lCotista.ValorRendaMensal).ToString("00000000000");                        //  "00000010000";
                string VLPATRIM    = Convert.ToDecimal(lCotista.ValorPatrimonial).ToString("000000000000000");                        //  "000000000100000";
                string CDCLSCLI    = GetTipoCliente(lCotista.CodigoTipoCliente).PadLeft(3, '0');                       //   GetTipoCliente() "99";
                string CDCETIP     = lCotista.CodigoCetip.PadLeft(8, ' ');                              //    "        ";
                string CDDISTRB    = lCotista.CodigoDistribuidor;
                string NOMADMIN    = lCotista.RazaoSocialAdministrador;
                string NMGEST      = lCotista.RazaoSocialGestor;
                string NMCUSTOD    = lCotista.RazaoSocialCustodiante;
                string NMCONTA1    = lCotista.NomePrimeiroContatoCustodiante;
                string DDD1CUST    = lCotista.DDDPrimeiroContatoCustodiante;
                string TEL1CUST    = lCotista.TelefonePrimeiroContatoCustodiante;
                string RML1CUST    = lCotista.RamalPrimeiroContatoCustodiante;
                string EML1CUST    = lCotista.EmailPrimeiroContatoCustodiante;
                string NMCONTA2    = lCotista.NomeSegundoContatoCustodiante;
                string DDD2CUST    = lCotista.DDDSegundoContatoCustodiante;
                string TEL2CUST    = lCotista.TelefoneSegundoContatoCustodiante;
                string RML2CUST    = lCotista.RamalSegundoContatoCustodiante;
                string EML2CUST    = lCotista.EmailSegundoContatoCustodiante;
                string NOLOGRES    = lCotista.EnderecoResidencial;
                string NULOGRES    = (lCotista.NumeroResidencial.Length > 5) ? lCotista.NumeroResidencial.Substring(0, 5) : lCotista.NumeroResidencial;
                string CPLOGRES    = lCotista.ComplementoResidencial;
                string BAIRRRES    = lCotista.BairroResidencial;
                string CEPRESID    = lCotista.CepResidencial;
                string CIDADRES    = lCotista.CidadeResidencial;
                string UFRESID     = lCotista.EstadoResidencial;
                string NOLOGCOM    = lCotista.EnderecoComercial;
                string NULOGCOM    = lCotista.NumeroComercial;
                string CPLOGCOM    = lCotista.ComplementoComercial;
                string BAIRRCOM    = lCotista.BairroComercial;
                string CEPCOMER    = lCotista.CepComercial;
                string CIDADCML    = lCotista.CidadeComercial;
                string UFCOMERC    = lCotista.EstadoComercial;
                string NOLOGALT    = lCotista.EnderecoAlternativo;
                string NULOGALT    = lCotista.NumeroAlternativo;
                string CPLOGALT    = lCotista.ComplementoAlternativo;
                string BAIRRALT    = lCotista.BairroAlternativo;
                string CEPALTER    = lCotista.CepAlternativo;
                string CIDADALT    = lCotista.CidadeAlternativo;
                string UFALTERN    = lCotista.EstadoAlternativo;
                string IDOPEMAC    = "000000";
                string INDCTSAVICD = lCotista.PessoaVinculada == "1" ? "S" : "N";

                string lStringXML = "<itaumsg><parameter>" +
                                    "<param id=\"EBUSINESSID\" value=\"" + EBUSINESSID + "\" />" + //->Código do Usuário
                                    "<param id=\"SENHA      \" value=\"" + SENHA + "\" />" +       //->Senha
                                    "<param id=\"CDBANCLI   \" value=\"" + CDBANCLI + "\" />" +    //->Código do Gestor do Cliente no sistema FJ
                                    "<param id=\"AGENCIA    \" value=\"" + AGENCIA + "\" />" +     //->Código do Agencia
                                    "<param id=\"CONTA      \" value=\"" + CONTA + "\" />" +       //->Código da Conta
                                    "<param id=\"DAC        \" value=\"" + DAC + "\" />" +         //->Dac da Conta
                                    "<param id=\"NMCLI      \" value=\"" + NMCLI + "\" />" +       //->Nome
                                    "<param id=\"DTNASCL8   \" value=\"" + DTNASCL8 + "\" />" +    //->Data de Nascimento
                                    "<param id=\"PES        \" value=\"" + PES + "\" />" +         //->Código do Tipo de Pessoa
                                    "<param id=\"IDCGCCPF   \" value=\"" + IDCGCCPF + "\" />" +    //->Identificador do Cliente Junto à Receita Federal
                                    "<param id=\"TPDOCIDE   \" value=\"" + TPDOCIDE + "\" />" +    //->Tipo de Documento (RG, CM, etc.)
                                    "<param id=\"IDDOCLI    \" value=\"" + IDDOCLI + "\" />" +     //->Número do documento
                                    "<param id=\"DTDOCEXP   \" value=\"" + DTDOCEXP + "\" />" +    //->Data de Expedição
                                    "<param id=\"IDORGEMI   \" value=\"" + IDORGEMI + "\" />" +    //->Orgão Emissor
                                    "<param id=\"IDESTEMI   \" value=\"" + IDESTEMI + "\" />" +    //->Estado Emissor
                                    "<param id=\"ICIMP      \" value=\"" + ICIMP + "\" />" +       //->Indicador de situação tributária do cotista
                                    "<param id=\"SITLEG     \" value=\"" + SITLEG + "\" />" +      //->Código Situação Legal
                                    "<param id=\"CODSEX     \" value=\"" + CODSEX + "\" />" +      //->Código do Sexo
                                    "<param id=\"CDESTCIV   \" value=\"" + CDESTCIV + "\" />" +    //->Código do Estado Civil
                                    "<param id=\"CDRAMOAT   \" value=\"" + CDRAMOAT + "\" />" +    //->Código da atividade cliente pessoa jurídica
                                    "<param id=\"CDPROATI   \" value=\"" + CDPROATI + "\" />" +    //->Código  da atividade cliente pessoa fisica
                                    "<param id=\"CDFCONST   \" value=\"" + CDFCONST + "\" />" +    //->Código da Forma de Constituição de uma Empresa
                                    "<param id=\"ICLEC      \" value=\"" + ICLEC + "\" />" +       //->Indica qual endereço será usado para correspondência
                                    "<param id=\"CDREMLEC   \" value=\"" + CDREMLEC + "\" />" +    //->Código do Tipo de Remessa
                                    "<param id=\"DDD        \" value=\"" + DDD + "\" />" +         //->Código de Discagem Direta à Distância do Telefone Residencial
                                    "<param id=\"TEL8       \" value=\"" + TEL8 + "\" />" +        //->Identificador do Telefone Residencial
                                    "<param id=\"RAMAL      \" value=\"" + RAMAL + "\" />" +       //->Identificador do Ramal
                                    "<param id=\"EMAIL      \" value=\"" + EMAIL + "\" />" +       //->E-Mail
                                    "<param id=\"IDASSESS   \" value=\"" + IDASSESS + "\" />" +    //->Identificador do Assessor
                                    "<param id=\"IDCLITER   \" value=\"" + IDCLITER + "\" />" +    //->Identificador do Cliente Junto ao Gestor
                                    "<param id=\"ICEXTM     \" value=\"" + ICEXTM + "\" />" +      //->Indicador de Emissão do Extrato Mensal
                                    "<param id=\"ICCFM      \" value=\"" + ICCFM + "\" />" +       //->Indicador de Emissão do Aviso de Confirmação de Movimentação
                                    "<param id=\"VRENDFAM   \" value=\"" + VRENDFAM + "\" />" +    //->Valor da Renda Familiar
                                    "<param id=\"VLPATRIM   \" value=\"" + VLPATRIM + "\" />" +    //->Valor do Patrimônio
                                    "<param id=\"CDCLSCLI   \" value=\"" + CDCLSCLI + "\" />" +    //->Classificação do Tipo de Cliente
                                    "<param id=\"CDCETIP    \" value=\"" + CDCETIP + "\" />" +     //->Código cetip
                                    "<param id=\"CDDISTRB   \" value=\"" + CDDISTRB + "\" />" +    //->Código do Distribuidor
                                    "<param id=\"NOMADMIN   \" value=\"" + NOMADMIN + "\" />" +    //->razão social do administrador
                                    "<param id=\"NMGEST     \" value=\"" + NMGEST + "\" />" +      //->razão social do gestor
                                    "<param id=\"NMCUSTOD   \" value=\"" + NMCUSTOD + "\" />" +    //->razão social do custodiante
                                    "<param id=\"NMCONTA1   \" value=\"" + NMCONTA1 + "\" />" +    //->Nome do primeiro contato no custodiante
                                    "<param id=\"DDD1CUST   \" value=\"" + DDD1CUST + "\" />" +    //->ddd do primeiro contato no custodiante
                                    "<param id=\"TEL1CUST   \" value=\"" + TEL1CUST + "\" />" +    //->telefone do primeiro contato no custodiante
                                    "<param id=\"RML1CUST   \" value=\"" + RML1CUST + "\" />" +    //->ramal do primeiro contato no custodiante
                                    "<param id=\"EML1CUST   \" value=\"" + EML1CUST + "\" />" +    //->e-mail do primeiro contato no custodiante
                                    "<param id=\"NMCONTA2   \" value=\"" + NMCONTA2 + "\" />" +    //->Nome do segundo contato no custodiante
                                    "<param id=\"DDD2CUST   \" value=\"" + DDD2CUST + "\" />" +    //->ddd do segundo contato no custodiante
                                    "<param id=\"TEL2CUST   \" value=\"" + TEL2CUST + "\" />" +    //->telefone do segundo contato no custodiante
                                    "<param id=\"RML2CUST   \" value=\"" + RML2CUST + "\" />" +    //->ramal do segundo contato no custodiante
                                    "<param id=\"EML2CUST   \" value=\"" + EML2CUST + "\" />" +    //->e-mail do segundo contato no custodiante
                                    "<param id=\"NOLOGRES   \" value=\"" + NOLOGRES + "\" />" +    //->nome do logradouro residencial
                                    "<param id=\"NULOGRES   \" value=\"" + NULOGRES + "\" />" +    //->número do logradouro residencial
                                    "<param id=\"CPLOGRES   \" value=\"" + CPLOGRES + "\" />" +    //->complemento do endereço residencial
                                    "<param id=\"BAIRRRES   \" value=\"" + BAIRRRES + "\" />" +    //->bairro do endereço residencial
                                    "<param id=\"CEPRESID   \" value=\"" + CEPRESID + "\" />" +    //->CEP do endereço residencial
                                    "<param id=\"CIDADRES   \" value=\"" + CIDADRES + "\" />" +    //->cidade do endereço residencial
                                    "<param id=\"UFRESID    \" value=\"" + UFRESID + "\" />" +     //->estado do endereço residencial
                                    "<param id=\"NOLOGCOM   \" value=\"" + NOLOGCOM + "\" />" +    //->nome do logradouro comercial
                                    "<param id=\"NULOGCOM   \" value=\"" + NULOGCOM + "\" />" +    //->número do logradouro comercial
                                    "<param id=\"CPLOGCOM   \" value=\"" + CPLOGCOM + "\" />" +    //->complemento do endereço comercial
                                    "<param id=\"BAIRRCOM   \" value=\"" + BAIRRCOM + "\" />" +    //->bairro do endereço comercial
                                    "<param id=\"CEPCOMER   \" value=\"" + CEPCOMER + "\" />" +    //->CEP do endereço comercial
                                    "<param id=\"CIDADCML   \" value=\"" + CIDADCML + "\" />" +    //->cidade do endereço comercial
                                    "<param id=\"UFCOMERC   \" value=\"" + UFCOMERC + "\" />" +    //->estado do endereço comercial
                                    "<param id=\"NOLOGALT   \" value=\"" + NOLOGALT + "\" />" +    //->nome do logradouro alternativo
                                    "<param id=\"NULOGALT   \" value=\"" + NULOGALT + "\" />" +    //->número do logradouro do endereço alternativo
                                    "<param id=\"CPLOGALT   \" value=\"" + CPLOGALT + "\" />" +    //->complemento do endereço alternativo
                                    "<param id=\"BAIRRALT   \" value=\"" + BAIRRALT + "\" />" +    //->bairro do endereço alternativo
                                    "<param id=\"CEPALTER   \" value=\"" + CEPALTER + "\" />" +    //->CEP do endereço alternativo
                                    "<param id=\"CIDADALT   \" value=\"" + CIDADALT + "\" />" +    //->cidade do endereço alternativo
                                    "<param id=\"UFALTERN   \" value=\"" + UFALTERN + "\" />" +    //->estado do endereço alternativo
                                    "<param id=\"IDOPEMAC   \" value=\"" + IDOPEMAC + "\" />" +    //->Identificador do Arquivo
                                    "<param id=\"INDCTSAVICD\" value=\"" + INDCTSAVICD + "\" />" +  //->Pessoa Vinculada
                                    "</parameter> </itaumsg>";

                gLogger.Info("Arquivo XML de Envio de cadastro de Cotista");

                gLogger.Info(lStringXML);

                var lResponseHtml = new WebClient().DownloadString("http://www.itaucustodia.com.br/PassivoWebServices/xmlcac.jsp?strXML=" + lStringXML);

                XDocument document = XDocument.Parse(lResponseHtml);

                gLogger.InfoFormat("Recebeu informações de Cadastro de Cliente {0}", lResponseHtml);

                var lCotistaCadastrado = from cotista in document.Root.Elements("parameter")
                                         select new
                                         {
                                             CodigoCotista = FindParameter(cotista, "CODCOTISTA"),
                                             MsgRetorno = FindParameter(cotista, "MSGRETORNO"),
                                         };

                string CodigoCotista = string.Empty;

                foreach (var info in lCotistaCadastrado)
                {
                    if (info.MsgRetorno.Attribute("value").Value == "OK")
                    {
                        gLogger.InfoFormat("Retorno do envio do cadastro de cliente para o Itaú : [{0}]", lResponseHtml);

                        CodigoCotista = info.CodigoCotista.Attribute("value").Value;
                    }
                    else
                    {
                        gLogger.Info(lResponseHtml);
                    }
                }


                ClienteFinancialInfo lCliente = new ClienteFinancialInfo();

                lCliente.NomeCliente       = lCotista.NomeCliente;
                lCliente.Email             = lCotista.Email;
                lCliente.DsCpfCnpj         = lCotista.DsCpfCnpj;
                lCliente.StAtivo           = "S";
                lCliente.CodigoAssessor    = int.Parse(lCotista.Assessor);
                lCliente.CodigoCliente     = int.Parse(lCotista.CodigoBovespa);
                lCliente.CodigoCotistaItau = CodigoCotista;
                lCliente.Telefone          = lCotista.NumeroTelResidencial;
                lCliente.TipoPessoa        = lCotista.CodigoTipoPessoa;

                if (!string.IsNullOrEmpty(CodigoCotista) && CodigoCotista != "00000000000000")
                {
                    gLogger.Info("**********************************************************************************************");
                    gLogger.InfoFormat("Inserindo/Atualizando dados de cliente Nome Cliente [{0}] ",        lCliente.NomeCliente);
                    gLogger.InfoFormat("Inserindo/Atualizando dados de cliente E-mail [{0}] ",              lCliente.Email);
                    gLogger.InfoFormat("Inserindo/Atualizando dados de cliente DsCpfCnpj [{0}] ",           lCliente.DsCpfCnpj);
                    gLogger.InfoFormat("Inserindo/Atualizando dados de cliente StAtivo [{0}] ",             lCliente.StAtivo);
                    gLogger.InfoFormat("Inserindo/Atualizando dados de cliente CodigoAssessor [{0}] ",      lCliente.CodigoAssessor);
                    gLogger.InfoFormat("Inserindo/Atualizando dados de cliente CodigoCliente [{0}] ",       lCliente.CodigoCliente);
                    gLogger.InfoFormat("Inserindo/Atualizando dados de cliente CodigoCotistaItau [{0}] ",   lCliente.CodigoCotistaItau);
                    gLogger.InfoFormat("Inserindo/Atualizando dados de cliente Telefone [{0}] ",            lCliente.Telefone);
                    gLogger.InfoFormat("Inserindo/Atualizando dados de cliente TipoPessoa [{0}] ",          lCliente.TipoPessoa);
                    gLogger.Info("**********************************************************************************************");

                    lDb.AtualizaClienteCotista(lCliente);

                    lCliente = lDb.SelecionaNovoCotistaFinancial(lContaBovespa);

                    this.ExportaCotistaParaFinancial(lCliente);
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ThreadExportacaoImportacaoClienteItau:", ex);
            }
        }

        public void ThreadExportacaoContaCreditoItau(string CodigoCotistaItau, string CodigoBovespa)
        {
            try
            {
                string lPrimeiraContaCredito = "0237023740110787".PadRight(24, ' ');


                string EBUSINESSID = this.GetUsuarioItau;
                string SENHA       = this.GetSenhaItau;
                string CDBANCLI    = this.GetCodigoGestorItau;
                string AGENCIA     = CodigoCotistaItau.Substring(0, 4);
                string CDCTA       = CodigoCotistaItau.Substring(4, 7).PadLeft(9, '0');
                string DAC10       = CodigoCotistaItau.Substring(11, 1);
                string CODMOV      = "I";
                string IDTIPCT1    = "C";
                string BCOAGCT1    = lPrimeiraContaCredito;
                string IDTPCT01    = "C";             //-->>C - Conta de Corretora, F - Conta de Fundo, P - Própria
                string IDCGCCPF    = "033918160000173";
                string IDTTCI01    = "1";
                string IDCLIT01    = CodigoBovespa.PadLeft(25, '0');
                string IDOPEMAC    = "000000";

                string lStringXML = "<itaumsg><parameter>" +
                                    "<param id=\"EBUSINESSID\" value=\"" + EBUSINESSID + "\" />" +
                                    "<param id=\"SENHA\"       value=\"" + SENHA + "\" />" +
                                    "<param id=\"CDBANCLI\"    value=\"" + CDBANCLI + "\" />" +
                                    "<param id=\"AGENCIA\"     value=\"" + AGENCIA + "\" />" +
                                    "<param id=\"CDCTA\"       value=\"" + CDCTA + "\" />" +
                                    "<param id=\"DAC10\"       value=\"" + DAC10 + "\" />" +
                                    "<param id=\"CODMOV\"      value=\"" + CODMOV + "\" />" +
                                    "<param id=\"IDTIPCT1\"    value=\"" + IDTIPCT1 + "\" />" +
                                    "<param id=\"BCOAGCT1\"    value=\"" + BCOAGCT1 + "\" />" +
                                    "<param id=\"IDTPCT01\"    value=\"" + IDTPCT01 + "\" />" +
                                    "<param id=\"IDCGCCPF\"    value=\"" + IDCGCCPF + "\" />" +
                                    "<param id=\"IDTTCI01\"    value=\"" + IDTTCI01 + "\" />" +
                                    "<param id=\"IDCLIT01\"    value=\"" + IDCLIT01 + "\" />" +
                                    "<param id=\"IDOPEMAC\"    value=\"" + IDOPEMAC + "\" />" +
                                    "</parameter> </itaumsg>";

                var lResponseHtml = new WebClient().DownloadString("https://www.itaucustodia.com.br/integrafj/xmlcci.jsp?strXML=" + lStringXML);

                XDocument document = XDocument.Parse(lResponseHtml);

                var lRetornoResgateAplicacao = from conta in document.Root.Elements("parameter")
                                               select new
                                               {
                                                   MsgRetorno = FindParameter(conta, "MSGRETORNO")
                                               };

                string DsMotivoStatus = string.Empty;

                if (lResponseHtml.Contains("OPERACAO+EFETUADA"))
                {
                    foreach (var retorno in lRetornoResgateAplicacao)
                    {
                        DsMotivoStatus = System.Web.HttpUtility.UrlDecode(retorno.MsgRetorno.Attribute("value").Value);
                    }
                }
                else
                {
                    foreach (var retorno in lRetornoResgateAplicacao)
                    {
                        DsMotivoStatus = System.Web.HttpUtility.UrlDecode(retorno.MsgRetorno.Attribute("value").Value);
                    }

                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ThreadExportacaoContaCreditoItau:", ex);
            }
        }

        private List<string> ListaHorarios(string pHorarios)
        {
            List<string> lretorno = new List<string>();

            try
            {
                if (pHorarios.Contains(";"))
                {
                    char[] lchars = { ';' };
                    string[] lhr = pHorarios.Split(lchars, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string hr in lhr) lretorno.Add(hr);
                }
                else if (!string.IsNullOrEmpty(pHorarios))
                {
                    lretorno.Add(pHorarios);
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("ListaHorarios() - ", ex);
            }

            return lretorno;
        }

        public void ThreadImportacaoPosicaoItau()
        {
            DateTime lastRun = DateTime.MinValue;

            while (_bKeepRunningItau)
            {
                TimeSpan interval = DateTime.Now - lastRun;

                DateTime lNow = DateTime.Now;

                List<string> listHorarios = ListaHorarios(this.HorariosImportacaoPosicao);

                string lPosicaoPath = @"c:\ITAU\Arquivos\";

                try
                {
                    if (interval.TotalMilliseconds > IntervalorImportacaoPosicaoItau && listHorarios.Contains(lNow.ToString("HH:mm")))
                    {
                        lastRun = DateTime.Now;

                        br.com.itaucustodia.www.DownloadArquivoServiceService lService = new br.com.itaucustodia.www.DownloadArquivoServiceService();

                        string lRetorno = lService.saldosCotaAberturaD0XML("gradual.op58", "pltfds1", "990686");

                        byte[] zipbytes = Convert.FromBase64String(lRetorno);

                        ZipFile zip = ZipFile.Read(zipbytes);

                        zip.ExtractAll(lPosicaoPath, true);

                        zip.Dispose();

                        System.IO.StreamReader lFile = new System.IO.StreamReader(string.Concat(lPosicaoPath, "DownloadArquivos.xml"));

                        string lContent = lFile.ReadToEnd();

                        lContent = lContent.Replace("</ArrayList id=\"1\">","</ArrayList>");

                        XDocument document = XDocument.Parse(lContent);

                        var lPosicao = from posicao in document.Descendants("SaldosCotaAberturaD0Bean")
                                       select new
                                       {
                                           CDBANC   = posicao.Element("CDBANC").Value,
                                           CDFDO    = posicao.Element("CDFDO").Value,
                                           CDBANCLI = posicao.Element("CDBANCLI").Value,
                                           AGENCIA  = posicao.Element("AGENCIA").Value,
                                           CDCTA    = posicao.Element("CDCTA").Value,
                                           DAC10    = posicao.Element("DAC10").Value,
                                           SUBCONT  = posicao.Element("SUBCONT").Value,
                                           DTLANCT  = posicao.Element("DTLANCT").Value,
                                           LANMOV   = posicao.Element("LANMOV").Value,
                                           IDTIPREG = posicao.Element("IDTIPREG").Value,
                                           CDAPL    = posicao.Element("CDAPL").Value,
                                           DTAPROCE = posicao.Element("DTAPROCE").Value,
                                           VLCOTAP  = posicao.Element("VLCOTAP").Value,
                                           VLORAP   = posicao.Element("VLORAP").Value,
                                           VLUFIRAP = posicao.Element("VLUFIRAP").Value,
                                           QTCOTPAT = posicao.Element("QTCOTPAT").Value,
                                           QTUFIR   = posicao.Element("QTUFIR").Value,
                                           DTACOTA  = posicao.Element("DTACOTA").Value,
                                           VLRCOT   = posicao.Element("VLRCOT").Value,
                                           VLPATBRU = posicao.Element("VLPATBRU").Value,
                                           VLIRRF   = posicao.Element("VLIRRF").Value,
                                           VLRIOF   = posicao.Element("VLRIOF").Value,
                                           VLTXPRF  = posicao.Element("VLTXPRF").Value,
                                           VALTXSAI = posicao.Element("VALTXSAI").Value,
                                           VLPATLIQ = posicao.Element("VLPATLIQ").Value,
                                           LUCRPRE  = posicao.Element("LUCRPRE").Value,
                                           DEBCRE   = posicao.Element("DEBCRE").Value,
                                           DTVCAR   = posicao.Element("DTVCAR").Value,
                                           DTULTRIB = posicao.Element("DTULTRIB").Value,
                                           VLCOTRIB = posicao.Element("VLCOTRIB").Value,
                                           VLIOFUTB = posicao.Element("VLIOFUTB").Value,
                                           QTCOTDIA = posicao.Element("QTCOTDIA").Value,
                                           DTAULTAN = posicao.Element("DTAULTAN").Value,
                                           VLCOTUAN = posicao.Element("VLCOTUAN").Value,
                                           DTPXANI  = posicao.Element("DTPXANI").Value,
                                           SDOPREJ  = posicao.Element("SDOPREJ").Value,
                                           INDACAPL = posicao.Element("INDACAPL").Value,
                                           INDACRSG = posicao.Element("INDACRSG").Value,
                                           MTBLQ    = posicao.Element("MTBLQ").Value,
                                           VLRNDAPL = posicao.Element("VLRNDAPL").Value,
                                           RDIOFPAT = posicao.Element("RDIOFPAT").Value,
                                           RDIRFPAT = posicao.Element("RDIRFPAT").Value,
                                           VLIRCCAC = posicao.Element("VLIRCCAC").Value,
                                           VRNDCCAC = posicao.Element("VRNDCCAC").Value,
                                       };

                        ImportacaoItauDbLib lDb = new ImportacaoItauDbLib();

                        foreach (var dr in lPosicao)
                        {
                            PosicaoClienteItauInfo lPos = new PosicaoClienteItauInfo();
                            
                            //lPos.CodigoCliente   = lDb.SelecionaCodigoCliente( dr.["dsCpfCnpj"].ToString());
                            //lPos.Angencia        = dr.AGENCIA;
                            //lPos.Banco           = dr.CDBANC;
                            //lPos.Conta           = dr.CDCTA;
                            //lPos.DigitoConta     = dr.DAC10;
                            //lPos.DsCpfCnpj       = dr["dsCpfCnpj"].ToString();
                            //lPos.DtProcessamento = Convert.ToDateTime(dr["dtProcessamento"]);
                            //lPos.DtReferencia    = Convert.ToDateTime(dr["dtReferencia"]);
                            //lPos.IdCotista       = dr["idCotista"].ToString();
                            //lPos.IdFundo         = lDb.SelecionaCodigoProduto(Convert.ToInt32(dr["idFundo"]));
                            //lPos.IdMovimento     = Convert.ToInt32(dr["idMovimento"].ToString());
                            //lPos.IdProcessamento = Convert.ToInt32(dr["idProcessamento"].ToString());
                            //lPos.QuantidadeCotas = Convert.ToDecimal(dr["quantidadeCotas"].ToString());
                            //lPos.SubConta        = dr["subConta"].ToString();
                            //lPos.ValorBruto      = Convert.ToDecimal(dr["valorBruto"].ToString());
                            //lPos.ValorCota       = Convert.ToDecimal(dr["valorCota"].ToString());
                            //lPos.ValorIOF        = Convert.ToDecimal(dr["valorIOF"].ToString());
                            //lPos.ValorIR         = Convert.ToDecimal(dr["valorIR"].ToString());
                            //lPos.ValorLiquido    = Convert.ToDecimal(dr["ValorLiquido"].ToString());
                            
                            lDb.ImportaPosicaoClienteFundosITAU(lPos);
                        }
                    }
                }
                catch (Exception ex)
                {
                    gLogger.Error("Ocorreu um erro ao acessar o método ThreadImportacaoItau:" + ex.Message, ex);
                }

                Thread.Sleep(250);
            }
        }

        public void ThreadImportacaoCotistaPosicaoItau()
        {
            try
            {
                DateTime lNow = DateTime.Now;

                List<string> listHorarios = ListaHorarios(this.HorariosImportacaoPosicao);

                if (listHorarios.Contains(lNow.ToString("HH:mm")))
                {
                    gLogger.Info("Entrou na rotina de importação de posição de cotista.");

                    ImportacaoItauDbLib lDb = new ImportacaoItauDbLib();

                    lDb.SelecionaImportaPosicaoClienteFundosITAU();
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ThreadImportacaoCotistaPosicaoItau:" + ex.Message, ex);
            }
        }

        public void StartCotistaPosicaoItau(object state)
        {
            try
            {
                gLogger.Info("StartCotistaPosicaoItau - Iniciando Serviço de Posição de Cotista");

                //thThreadExportacaoImportacaoCotista = new Thread(new ThreadStart(ThreadImportacaoCotistaPosicaoItau));
                //thThreadExportacaoImportacaoCotista.Name = "ThreadImportacaoCotistaPosicaoItau";
                //thThreadExportacaoImportacaoCotista.Start();

                //thThreadMonitorAplicacaoResgate = new Thread(new ThreadStart(ThreadMonitorAplicacaoResgate));
                //thThreadMonitorAplicacaoResgate.Name = "ThreadAplicacaoResgate";
                //thThreadMonitorAplicacaoResgate.Start();

                gLogger.Info("*****************************************************************");
                gLogger.Info("***********Processo de inicialização finalizado******************");
                gLogger.Info("*****************************************************************");
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("StartCotistaPosicaoItau  StackTrace - {0}", ex.StackTrace);
            }
        }

        public void IniciarServico()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                _bKeepRunningItau = true;
                
                _ServicoStatus = ServicoStatus.EmExecucao;

                this.StartCotistaPosicaoItau(null);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("IniciarServico()  StackTrace - {0}", ex.StackTrace);
            }
        }

        public void PararServico()
        {
            try
            {
                _ServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro ao para servicço de Posição de Cotista Itaú - PararServico ", ex);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }
        #endregion

        #region De para cadastro Cotista
        
        private int GetEstadoCivilFinancial(int codigo)
        {
            int lRetorno = 0;

            switch (codigo)
            {
                case 1:
                    lRetorno = CodigoEstadoCivilFinancial.Solteiro;
                    break;
                case 2:
                    lRetorno = CodigoEstadoCivilFinancial.Outros;
                    break;
                case 3:
                    lRetorno = CodigoEstadoCivilFinancial.Viuvo;
                    break;
                case 4:
                    lRetorno = CodigoEstadoCivilFinancial.Divorciado;
                    break;
                case 5:
                case 6:
                case 7:
                    lRetorno = CodigoEstadoCivilFinancial.Casado;
                    break;
                case 8:
                case 9:
                    lRetorno = CodigoEstadoCivilFinancial.Outros;
                    break;
                default:
                    lRetorno = CodigoEstadoCivilFinancial.Nao_Aplicavel;
                    break;
            }

            return lRetorno;
        }

        public void ExportaCotistaParaFinancial(ClienteFinancialInfo pCliente)
        {
            try
            {
                CadastroCotista.CadastroCotistaWSSoapClient lServico = new CadastroCotista.CadastroCotistaWSSoapClient();

                CadastroCotista.ValidateLogin lLogin = new CadastroCotista.ValidateLogin();

                lLogin.Username = UsuarioFinancial;
                lLogin.Password = SenhaFinancial;

                string lIsentoIR    = GetCodigoTributacaoFinancial(pCliente.TipoCliente);
                string lIsentoIOF   = GetCodigoTributacaoFinancial(pCliente.TipoCliente);
                byte lTipoPessoa    = pCliente.TipoPessoa == "F" ? (byte)1 : (byte)2;
                byte TipoCotistaCvm = (byte)GetTipoCotistaFinancial(pCliente.TipoCotistaCvm, pCliente.TipoPessoa);
                byte lEstadoCivil   = (byte)GetEstadoCivilFinancial(pCliente.EstadoCivil);

                gLogger.InfoFormat("Exportando Cotista para a Financial [Cotista] = [{0}]", pCliente.CodigoCliente);

                lServico.Importa(lLogin,
                    pCliente.CodigoCliente,
                    pCliente.NomeCliente,
                    lTipoPessoa,
                    pCliente.DsCpfCnpj,
                    lIsentoIR,
                    lIsentoIOF,
                    1,                       //-->> Ativo
                    (byte)pCliente.StResidentePais,
                    TipoCotistaCvm,
                    "GRADUAL",
                    pCliente.Endereco,
                    pCliente.Numero,
                    pCliente.Complemento,
                    pCliente.Bairro,
                    pCliente.Cidade,
                    pCliente.CEP,
                    pCliente.UF,
                    pCliente.Pais,
                    pCliente.EnderecoCom,
                    pCliente.NumeroCom,
                    pCliente.ComplementoCom,
                    pCliente.BairroCom,
                    pCliente.CidadeCom,
                    pCliente.CEPCom,
                    pCliente.UFCom,
                    pCliente.PaisCom,
                    pCliente.Fone,
                    pCliente.Email,
                    pCliente.FoneCom,
                    pCliente.EmailCom,
                    lEstadoCivil,
                    pCliente.NumeroRG,
                    pCliente.EmissorRG,
                    pCliente.DataEmissaoRG,
                    pCliente.Sexo,
                    pCliente.DataNascimento,
                    pCliente.Profissao
                    );

                gLogger.InfoFormat("Cotista exportado para a Financial [Cotista] [{0}] com sucesso", pCliente.CodigoCliente);
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ExportaCotistaParaFinancial:" + ex.Message, ex);
            }
        }

        public class TipoDocumento
        {
            public const string CEDULA_DE_IDENTIDADE                  = "CI";
            public const string CEDULA_DE_IDENTIDADE_ESTRANGEIRO      = "CIE";
            public const string CEDULA_DE_IDENTIDADE_MILITAR          = "CIM";
            public const string CEDULA_DE_IDENTIDADE_PROFISSIONAL     = "CIP";
            public const string CED_IDENT_PROF_FUNC_ORGAO_PUBLICO     = "CIPF";
            public const string CERTIDAO_DE_NASCIMENTO                = "CN";
            public const string CARTEIRA_NACIONAL_DE_HABILITACAO      = "CNH";
            public const string CARTEIRA_TRABALHO_E_PREV_SOCIAL       = "CTPS";
            public const string IDENTIFICACAO_PRISIONAL               = "IP";
            public const string LAISSEZ_PASSER                        = "LPASS";
            public const string PASSAPORTE                            = "PASSP";
            public const string CEDULA_DE_IDENTIDADE_RG               = "RG";
            public const string DOCUMENTO_DE_IDENTIDADE               = "RI";
            public const string CEDULA_DE_IDENTIDADE_ESTRANGEIRO_RNE  = "RNE";
            public const string INSCRICAO_ESTADUAL                    = "IE";
            public const string NUMERO_IDENTIFICACAO_REGISTRO_EMPRESA = "NIRE";
        }

        public class SituacaoLegal
        {
            public const string Espolio    = "E";
            public const string Interdito  = "I";
            public const string Maior      = "L";
            public const string Menor      = "M";
            public const string Emancipado = "N";
        }

        public class EstadoCivil
        {
            public const string CASADO                = "1";
            public const string SOLTEIRO              = "4";
            public const string DESQUITADO_DIVORCIADO = "5";
            public const string VIUVO                 = "6";
            public const string MARITAL_COMPANHEIRO   = "7";
            public const string DESQUITADO            = "8";
            public const string DIVORCIADO            = "9";
        }

        public class PorteEmpresa
        {
            public const string MICRO_ATE_10000_OTN   = "11";
            public const string MICRO_ACIMA_10000_OTN = "12";
            public const string PEQUENA               = "21";
            public const string MEDIA                 = "31";
            public const string GRANDE                = "41";
            public const string NAO_CADASTRADA        = "91";
        }

        public class SetorAtuacao
        {
            public const string PRIVADO_CAPITAL_NACIONAL           = "11";
            public const string PRIVADO_CAPITAL_MISTO              = "12";
            public const string PRIVADO_CAPITAL_ESTRANGEIRO        = "13";
            public const string PUBLICO_FEDERAL_ADM_DIRETA         = "51";
            public const string PUBLICO_FEDERAL_ADM_INDIRETA       = "52";
            public const string PUBLICO_FEDERAL_ATIV_EMPRESARIAL   = "53";
            public const string PUBLICO_ESTADUAL_ADM_DIRETA        = "61";
            public const string PUBLICO_ESTADUAL_ADM_INDIRETA      = "62";
            public const string PUBLICO_ESTADUAL_ATIV_EMPRESARIAL  = "63";
            public const string PUBLICO_MUNICIPAL_ADM_DIRETA       = "71";
            public const string PUBLICO_MUNICIPAL_ADM_INDIRETA     = "72";
            public const string PUBLICO_MUNICIPAL_ATIV_EMPRESARIAL = "73";
        }

        public class TipoCotista
        {
            public const string Autarquia                                             = "1";
            public const string Cliente_Private                                       = "2";
            public const string Cliente_Varejo                                        = "3";
            public const string Cooperativa                                           = "4";
            public const string Fundo_Capital_Garantido                               = "5";
            public const string Fundo_Direitos_Creditórios                            = "6";
            public const string Fundo_Exclusivo_Fechado                               = "7";
            public const string Fundo_FAC_Generico                                    = "8";
            public const string Fundo_FAC_Generico_Agressivo                          = "9";
            public const string Fundo_FAC_Nao_Referenciado                            = "10";
            public const string Fundo_FAC_Referenciado_Cambial                        = "11";
            public const string Fundo_FAC_Referenciado_DI                             = "12";
            public const string Fundo_FAPI                                            = "13";
            public const string Fundo_FIA_Privatizacao                                = "14";
            public const string Fundo_FICFITVM_ou_FICFIA                              = "15";
            public const string Fundo_FIEX                                            = "16";
            public const string Fundo_FIF_Generico_Derivativos                        = "17";
            public const string Fundo_FIF_Generico_Agressivo_Derivativos              = "18";
            public const string Fundo_FIF_Nao_Referenciado                            = "19";
            public const string Fundo_FIF_Referenciado_Cambial                        = "20";
            public const string Fundo_FIF_Referenciado_DI                             = "21";
            public const string Fundo_FITVM                                           = "22";
            public const string Fundo_FMP_FGTS                                        = "23";
            public const string Fundo_FMP_FGTS_Carteira_Livre                         = "24";
            public const string Fundo_Imobiliario                                     = "25";
            public const string Fundo_Offshore                                        = "26";
            public const string Fundo_Outros                                          = "27";
            public const string Fundo_RF_Cap_Estrangeiro                              = "28";
            public const string Fundo_Pensao_Empresa_Privada                          = "29";
            public const string Fundo_Pensao_Empresa_Publica                          = "30";
            public const string Fundo_Previdencia_Aberto_PF_PGBL_VGBL_etc             = "31";
            public const string Fundo_Previdencia_Aberto_PJ_PGBL_VGBL_etc             = "32";
            public const string Fundo_Previdencia_Fechado_Estados_e_Municipios        = "33";
            public const string Igreja_Instituicao_Religiosa                          = "34";
            public const string Inst_Fin_Capitalizacao                                = "35";
            public const string Inst_Fin_Seguradora                                   = "36";
            public const string Instituicao_Benificente                               = "37";
            public const string Investidor_Qualificado                                = "39";
            public const string Midle_Market_M_200_a_MM_1                             = "40";
            public const string Orgao_Publico                                         = "41";
            public const string Partido_Politico                                      = "42";
            public const string Holding                                               = "43";
            public const string Jornal_Radio_Televisao                                = "44";
            public const string Multinacional                                         = "45";
            public const string Orgao_de_Imprensa                                     = "46";
            public const string Sindicato                                             = "47";
            public const string Investidor_Nao_Residente                              = "50";
            public const string Cotista_Por_Conta_e_Ordem                             = "52";
            public const string Inst_Fin_Corretora                                    = "53";
            public const string Inst_Fin_Distribuidora                                = "54";
            public const string Inst_Fin_Banco_Comercial                              = "55";
            public const string Inst_Fin_Banco_de_Investimento                        = "56";
            public const string Entidade_Aberta_de_Previdencia_Complementar           = "57";
            public const string Entidade_Fechada_de_Previdencia_Complementar          = "58";
            public const string Regime_Proprio_de_Previdencia_dos_Servidores_Publicos = "59";
            public const string Outros                                                = "99";

        }

        public class TipoCotistaFinancial
        {
            public const int pessoa_fisica_private_banking                                      = 0;
            public const int pessoa_fisica_varejo                                               = 1;
            public const int pessoa_juridica_nao_financeira_private_banking                     = 2;
            public const int pessoa_juridica_nao_financeira_varejo                              = 3;
            public const int banco_comercial                                                    = 4;
            public const int corretora_ou_distribuidora                                         = 5;
            public const int outras_pessoas_juridicas_financeiras                               = 6;
            public const int Investidores_nao_residentes                                        = 7;
            public const int entidade_aberta_de_previdencia_complementar                        = 8;
            public const int entidade_fechada_de_previdencia_complementar                       = 9;
            public const int regime_proprio_de_previdencia_dos_servidores_publicos              = 10;
            public const int sociedade_seguradora_ou_resseguradora                              = 11;
            public const int sociedade_de_capitalizacao_e_de_arrendamento_mercantil             = 12;
            public const int fundos_e_clubes_de_Investimento                                    = 13;
            public const int cotistas_de_distribuidores_do_fundo_distribuicao_por_conta_e_ordem = 14;
            public const int outros_tipos_de_cotistas_nao_relacionados                          = 15;

        }

        public class CodigoTributacaoFinancial
        {
            public const string Isentos    = "S";
            public const string Tributados = "N";
        }

        public class CodigoEstadoCivilFinancial
        {
            public const int Nao_Aplicavel = 0;
            public const int Casado        = 1;
            public const int Solteiro      = 2;
            public const int Viuvo         = 3;
            public const int Divorciado    = 4;
            public const int Outros        = 5;
        }

        public class CodigoTributacao
        {
            public const string Isentos                                     = "F";
            public const string Incide_apenas_IOF_IN_264                    = "G";
            public const string Tributados                                  = "I";
            public const string Tributados_com_deposito_em_juizo            = "J";
            public const string Incide_apenas_IOF_IN_341                    = "M";
            public const string Incide_IR_com_deposito_em_juizo             = "P";
            public const string Incide_apenas_IR                            = "R";
            public const string Incide_IOF_IN_341_e_IOF_IN_264              = "T";
            public const string Incide_IR_e_IOF_IN341_com_deposito_em_juizo = "X";
        }

        public class OrgaosEmissoresItau
        {
            public const string ASS_BRASILEIRA_DE_EX_CONGRESSISTAS               = "ABEXCON";
            public const string MIN_DA_AERONAUTICA                               = "AERON";
            public const string ASSEMBLEIA_LEGISLATIVA                           = "ASLEGIS";
            public const string CORPO_DE_BOMBEIROS_MILITAR                       = "CBM";
            public const string CONS_FED_DE_PSICOLOGIA                           = "CFP";
            public const string CORREGEDORIA_GERAL_DA_JUSTICA                    = "CGJ";
            public const string CAMARA_MUNICIPAL_DO_RIO_DE_JANEIRO               = "CMRJ";
            public const string CONF_NACIONAL_DIRIGENTES_LOJISTAS                = "CNDL";
            public const string CONS_NAC_DE_ESTATISTICA                          = "CNE";
            public const string COMISSAO_NACIONAL_ENERGIA_NUCLEAR                = "CNEN";
            public const string CONS_DOS_DETETIVES_DO_BRASIL                     = "CONDEBR";
            public const string CONS_FED_DE_MED_VETERINARIA                      = "CONFEV";
            public const string CONS_FED_DE_DETETIVES_PROF_DO_BRASIL             = "CONFIPA";
            public const string CONS_REG_DE_ESTATISTICA                          = "CONRE";
            public const string CONS_REG_DOS_PROF_DE_RELACOES_PUBLICAS           = "CONRERP";
            public const string CONS_REG_DE_REPR_COMERCIAIS                      = "CORE";
            public const string CONS_REG_DE_ECONOMIA                             = "CORECON";
            public const string CONS_REG_DE_MUSEOLOGIA                           = "COREM";
            public const string CONS_REG_DE_ENFERMAGEM                           = "COREN";
            public const string CONS_REG_DE_ADMINISTRACAO                        = "CRA";
            public const string CONS_REG_DE_ASSISTENTES_SOCIAIS                  = "CRAS";
            public const string CONS_REG_DE_BIBLIOTECONOMIA_OU_BIOLOGIA          = "CRB";
            public const string CONS_REG_DE_BIOMEDICINA                          = "CRBM";
            public const string CONS_REG_DE_CONTABILIDADE                        = "CRC";
            public const string CARTORIO_REG_CIVIL_PESSOAS_NATURAIS              = "CRCPN";
            public const string CONS_REG_DE_ENG_ARQUIT_E_AGRON                   = "CREA";
            public const string CONS_REG_DE_CORRETORES_DE_IMOVEIS                = "CRECI";
            public const string CONS_REG_DE_EDUCACAO_FISICA                      = "CREF";
            public const string CONS_REG_DE_SERVICO_SOCIAL                       = "CRESS";
            public const string CONS_REG_DE_FARMACIA                             = "CRF";
            public const string CONS_REG_DE_FONOAUDIOLOGIA                       = "CRFA";
            public const string CONS_REG_DE_FISIOTERAPIA_E_TER_OCUPACIONAL       = "CRFTO";
            public const string CONS_REG_DE_MEDICINA                             = "CRM";
            public const string CONS_REG_DE_MED_VETERINARIA                      = "CRMV";
            public const string CONS_REG_DE_NUTRICIONISTAS                       = "CRN";
            public const string CONS_REG_DE_ODONTOLOGIA                          = "CRO";
            public const string CONS_REG_DE_PSICOLOGIA                           = "CRP";
            public const string CONS_REG_DE_QUIMICA                              = "CRQ";
            public const string CONS_REG_DOS_TEC_ADMINISTRACAO                   = "CRTA";
            public const string CONS_REG_DE_TEC_DE_RADIOLOGIA                    = "CRTR";
            public const string MIN_DO_TRABALHO_DELEG_REG_DO_TRABALHO            = "CTMTB";
            public const string MIN_DA_DEFESA                                    = "DEF";
            public const string DEPTO_DOS_CORREIOS_E_TELEGRAFOS                  = "DEPCT";
            public const string SECR_DE_EST_DE_JUSTICA_E_INTERIOR                = "DESIPE";
            public const string DEPTO_EST_DE_SEG_PUBLICA                         = "DESP";
            public const string DEPTO_DE_TRANSITO                                = "DETRAN";
            public const string DEPTO_FED_DE_SEG_PUBLICA                         = "DFSP";
            public const string DEFENSORIA_PUBLICA_GERAL_DO_ESTADO               = "DGPC";
            public const string DEPTO_DE_INVESTIGACOES                           = "DI";
            public const string DIR_GERAL_DA_POLICIA_CIVIL                       = "DPF";
            public const string DIR_GERAL_DA_POLICIA_CIVIL2                      = "DPGE";
            public const string DEPTO_DE_POLICIA_TECNICO_CIENTIFICA              = "DPTC";
            public const string DIVISAO_DE_SEG_E_GUARDA                          = "DSG";
            public const string EST_DO_AMAZONAS_PODER_JUDICIARIO                 = "EAPJU";
            public const string ESCOLA_SUPERIOR_DE_GUERRA                        = "ESG";
            public const string MIN_DO_EXERCITO                                  = "EXERCIT";
            public const string FEDERACAO_NACIONAL_DOS_JORNALISTAS               = "FENAJ";
            public const string FUNDACAO_NACIONAL_DO_INDIO                       = "FUNAI";
            public const string GERENCIA_EST_DE_JUSTICA                          = "GEJ";
            public const string GABINETE_DE_INVESTIGACOES_SERV_DE_IDENTIF        = "GISI";
            public const string INST_DE_CRIMINOLOGIA                             = "IC";
            public const string INST_FELIX_PACHECO                               = "IFP";
            public const string INST_DE_IDENT_DR_AROLDO_MENDES_DE_PAIVA          = "IIDAMP";
            public const string INST_DE_IDENT_MEDICO_LEGAL                       = "IIDML";
            public const string INST_DE_IDENT_EST_PARANA                         = "IIEP";
            public const string INST_DE_IDENT_TECNICA_POLICIAL                   = "IITP";
            public const string INST_MED_LEGAL_E_CRIMINALISTICA                  = "IMLC";
            public const string INST_NACIONAL_DE_IDENTIFICACAO                   = "INI";
            public const string INST_PEREIRA_FAUSTINO                            = "IPF";
            public const string INST_DE_POLICIA_TECNICA                          = "IPT";
            public const string INST_TAVARES_BURIL                               = "ITB";
            public const string JUIZADO_DA_INFANCIA_E_DA_JUVENTUDE               = "JUIJ";
            public const string AG_ESPEC_DA_ORG_DAS_NACOES_UNIDAS_ONU            = "LAISSEZ";
            public const string MIN_DA_MARINHA                                   = "MARINHA";
            public const string MIN_DAS_COMUNICACOES                             = "MC";
            public const string MIN_DA_CIENCIA_E_TECNOLOGIA                      = "MCT";
            public const string MIN_DA_CULTURA                                   = "MCU";
            public const string MIN_DA_EDUCACAO_E_DO_DESPORTO                    = "MED";
            public const string MIN_DA_ECONOMIA_FAZENDA_PLANEJAMENTO             = "MEFP";
            public const string MIN_DA_EDUCACAO_UNIVERSIDADE_FEDERAL             = "MEUF";
            public const string MIN_DA_FAZENDA                                   = "MF";
            public const string MIN_DA_GUERRA                                    = "MGUERRA";
            public const string MIN_DO_INTERIOR                                  = "MI";
            public const string MIN_DA_IND_E_DO_COM                              = "MIC";
            public const string MIN_DA_JUSTICA                                   = "MJ";
            public const string MIN_PREVIDENCIA_E_ASSISTENCIA_SOCIAL             = "MPAS";
            public const string MIN_PUBLICO_DISTRITO_FED_TERRITORIOS             = "MPDFT";
            public const string MIN_PUBLICO_DO_EST_DE_MINAS_GERAIS               = "MPEMG";
            public const string MIN_PUBLICO_DO_EST_DE_MATO_GROSSO                = "MPEMT";
            public const string MIN_PUBLICO_DO_EST_DO_PARANA                     = "MPEPR";
            public const string MIN_PUBLICO_DO_EST_DO_RIO_DE_JANEIRO             = "MPERJ";
            public const string MIN_PUBLICO_DO_EST_DE_SAO_PAULO                  = "MPESP";
            public const string MIN_PUBLICO_FEDERAL                              = "MPF";
            public const string MIN_PUBLICO_MILITAR                              = "MPM";
            public const string MIN_PUBLICO_DO_TRABALHO                          = "MPT";
            public const string MIN_DE_RELACOES_EXTERIORES                       = "MRE";
            public const string MIN_DO_SAUDE                                     = "MS";
            public const string MIN_DO_TRABALHO                                  = "MT";
            public const string MIN_DOS_TRANSPORTES                              = "MTRANSP";
            public const string ORDEM_DOS_ADVOGADOS_DO_BRASIL                    = "OAB";
            public const string ORDEM_DOS_MUSICOS_DO_BRASIL_CONS_REG             = "OMBCR";
            public const string SECR_DA_JUSTICA_E_SEG_P                          = "JURIC";
            public const string PAIS_DE_ORIGEM                                   = "PASSPOR";
            public const string POLICIA_CIVIL_INST_IDENT                         = "PCIID";
            public const string PREFEITURA_DA_CIDADE_DO_RJ                       = "PCRJ";
            public const string POLICIA_DO_DISTRITO_FED_INST_DE_IDENT            = "PDFII";
            public const string POLICIA_FERROVIARIA_FEDERAL                      = "PFF";
            public const string PROCURADORIA_GERAL_DO_ESTADO                     = "PGE";
            public const string PROCURADORIA_GERAL_DE_JUSTICA                    = "PGJ";
            public const string PENITENCIARIA_INDL_DE_GUARAPUAVA                 = "PIG";
            public const string PODER_JUDICIARIO_DO_EST_DE_GOIAS                 = "PJEG";
            public const string POLICIA_MILITAR                                  = "PM";
            public const string PREFEITURA_MUNICIPAL_DE_PETROPOLIS               = "PMP";
            public const string REPUBLICA_DO_BRASIL                              = "RBR";
            public const string REPUBLICA_FEDERATIVA_DO_BRASIL                   = "RFBR";
            public const string REDE_FERROVIARIA_FEDERAL                         = "RFF";
            public const string SECR_ADMIN_DEPTO_GERAL_DE_PESSOAL                = "SADGP";
            public const string SUPERINT_ADMINISTRACAO_DE_PESSOAL                = "SAPESS";
            public const string SECR_DA_CULTURA                                  = "SC";
            public const string SIND_DETETIVES_PARTIC_PROFISS_ESPIRITO_SANTO     = "SDPPES";
            public const string SECR_DE_EST_DE_ADMINISTRAÇÃO                     = "SEA";
            public const string SECR_ESPECIAL_DE_CIENCIA_E_TECNOLOGIA            = "SECT";
            public const string SERV_ESTRANG_DEPTO_POL_MARIT_AEREA_FRONT_POL_FED = "SEDPF";
            public const string SECR_DE_EST_DA_DEFESA_SOCIAL                     = "SEDS";
            public const string SECR_DE_EST_DE_EDUCACAO_E_CULTURA                = "SEEC";
            public const string SECR_DE_EST_DA_FAZENDA_CULTURA                   = "SEF";
            public const string SECR_DE_EST_E_JUSTICA_DA_SEG_PUBLICA             = "SEJSP";
            public const string SECR_DE_DE_EST_DA_JUSTIÇA                        = "SEJU";
            public const string SECR_DE_EST_DE_OBRAS_E_MEIO_AMBIENTE             = "SEOMA";
            public const string SECR_DE_EST_DA_POLICIA_CIVIL                     = "SEPC";
            public const string SECR_DE_EST_DA_SEGURANCA                         = "SES";
            public const string SECR_DE_EST_SEG_PUBLICA                          = "SESP";
            public const string SENADO_FEDERAL                                   = "SF";
            public const string SUPERINT_GERAL_DE_POLICIA_JUDICIARIA             = "SGPJ";
            public const string SERVIÇO_DE_IDENT_E_CRIMINALISTICA                = "SIC";
            public const string SECR_DO_INTERIOR_DA_JUSTICA                      = "SIJ";
            public const string SINDICATO_DOS_JORNALISTAS_PROF                   = "SJP";
            public const string SECR_DA_JUSTICA_E_SEG                            = "SJS";
            public const string SECR_DA_JUSTICA_E_SEG_PUBLICA                    = "SJSP";
            public const string SECR_JUSTICA_TRABALHO_CIDADANIA                  = "SJTC";
            public const string SUPERINT_POLICIA_CIVIL                           = "SPC";
            public const string SEG_PUBL_CIDADANIA_INST_IDENT                    = "SPCII";
            public const string SECR_DE_POLICIA_E_SEG_PUBLICA                    = "SPSP";
            public const string SUPERINT_POLICIA_TEC_CIENTIFICA                  = "SPTC";
            public const string SECR_DA_RECEITA_FEDERAL                          = "SRF";
            public const string SECR_DE_SEG_INFORMACOES                          = "SSI";
            public const string SECR_DE_SEG_PUBLICA                              = "SSP";
            public const string SECR_DE_SEG_PUBLICA_E_DEFESA_DA_CIDADANIA        = "SSPDC";
            public const string SUPERIOR_TRIBUNAL_MILITAR                        = "STM";
            public const string TRIBUNAL_DE_CONTAS                               = "TC";
            public const string TRIBUNAL_DE_JUSTIÇA                              = "TJ";
            public const string TRIBUNAL_REG_ELEITORAL                           = "TRE";
            public const string TRIBUNAL_REG_FEDERAL                             = "TRF";
            public const string TRIBUNAL_DE_ALCADA                               = "TRIBALC";
            public const string TRIBUNAL_REG_DO_TRABALHO                         = "TRT";
            public const string TRIBUNAL_SUPERIOR_DO_TRABALHO                    = "TST";
            public const string SECRETARIA_DA_FAZENDA                            = "SEFAZ";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_DISTRITO_FEDERAL    = "JCDE";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_AMAPA               = "JUCAP";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_AMAZONAS            = "JUCEA";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_ACRE                = "JUCEAC";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_ALAGOAS             = "JUCEAL";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DA_BAHIA               = "JUCEB";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_CEARA               = "JUCEC";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_ESPIRITO_SANTO      = "JUCEES";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_GOIAS               = "JUCEG";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_MARANHAO            = "JUCEMA";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_MATO_GROSSO         = "JUCEMAT";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_MINAS_GERAIS        = "JUCEMG";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_MATO_GROSSO_DO_SUL  = "JUCEMS";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DA_PARAIBA             = "JUCEP";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_PARA                = "JUCEPA";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_PARANA              = "JUCEPAR";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_PERNAMBUCO          = "JUCEPE";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_PIAUI               = "JUCEPI";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_RONDONIA            = "JUCER";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_RIO_GRANDE_DO_SUL   = "JUCERGS";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_RIO_DE_JANEIRO      = "JUCERJA";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DO_RIO_GRANDE_DO_NORTE = "JUCERN";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_RORAIMA             = "JUCERR";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_SANTA_CATARINA      = "JUCESC";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_SERGIPE             = "JUCESE";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_SAO_PAULO           = "JUCESP";
            public const string JUNTA_COMERCIAL_DO_ESTADO_DE_TOCANTINS           = "JUCETIN";


        }

        private string GetTipoDocumento(string codigo)
        {
            string lRetorno = string.Empty;

            switch (codigo)
            {
                case "PF":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_MILITAR;
                    break;
                case "AR":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_MILITAR;
                    break;
                case "RN":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_ESTRANGEIRO_RNE;
                    break;
                case "JZ":
                    lRetorno = TipoDocumento.CED_IDENT_PROF_FUNC_ORGAO_PUBLICO;
                    break;
                case "CH":
                    lRetorno = TipoDocumento.CARTEIRA_NACIONAL_DE_HABILITACAO;
                    break;
                case "CN":
                    lRetorno = TipoDocumento.CERTIDAO_DE_NASCIMENTO;
                    break;
                case "CR":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "RP":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "RC":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "AS":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "BT":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "CI":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "MV":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "EN":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "AM":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "CT":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "EC":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "CF":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "EF":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "FA":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "MD":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "NT":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "OD":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "PS":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "KS":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "AE":
                    lRetorno = TipoDocumento.CED_IDENT_PROF_FUNC_ORGAO_PUBLICO;
                    break;
                case "MJ":
                    lRetorno = TipoDocumento.CED_IDENT_PROF_FUNC_ORGAO_PUBLICO;
                    break;
                case "MR":
                    lRetorno = TipoDocumento.CED_IDENT_PROF_FUNC_ORGAO_PUBLICO;
                    break;
                case "ME":
                    lRetorno = TipoDocumento.CED_IDENT_PROF_FUNC_ORGAO_PUBLICO;
                    break;
                case "EX":
                    lRetorno = TipoDocumento.CED_IDENT_PROF_FUNC_ORGAO_PUBLICO;
                    break;
                case "MP":
                    lRetorno = TipoDocumento.CED_IDENT_PROF_FUNC_ORGAO_PUBLICO;
                    break;
                case "AD":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_PROFISSIONAL;
                    break;
                case "PP":
                    lRetorno = TipoDocumento.PASSAPORTE;
                    break;
                case "RG":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_RG;
                    break;
                case "TE":
                    lRetorno = TipoDocumento.CEDULA_DE_IDENTIDADE_RG;
                    break;
            }

            return lRetorno;
        }

        private int GetTipoCotistaFinancial(int pCodigo, string pTipoPessoa)
        {
            int lRetorno = 1;

            switch (pCodigo)
            {
                case 1:
                    {
                        if (pTipoPessoa.Equals("F"))
                        {
                            lRetorno = TipoCotistaFinancial.pessoa_fisica_varejo;
                        }
                        else
                        {
                            lRetorno = TipoCotistaFinancial.pessoa_juridica_nao_financeira_varejo;
                        }
                    }
                    break;
                case 2:
                    lRetorno = TipoCotistaFinancial.outros_tipos_de_cotistas_nao_relacionados;
                    break;
                case 3:
                    lRetorno = TipoCotistaFinancial.Investidores_nao_residentes;
                    break;
                case 4:
                    lRetorno = TipoCotistaFinancial.banco_comercial;

                    break;
                case 6:
                    lRetorno = TipoCotistaFinancial.outras_pessoas_juridicas_financeiras;
                    break;
                case 8:
                    lRetorno = TipoCotistaFinancial.corretora_ou_distribuidora;
                    break;
                case 9:
                    lRetorno = TipoCotistaFinancial.sociedade_seguradora_ou_resseguradora;
                    break;
                case 11:
                    lRetorno = TipoCotistaFinancial.corretora_ou_distribuidora;
                    break;
                case 13:
                    lRetorno = TipoCotistaFinancial.outros_tipos_de_cotistas_nao_relacionados;
                    break;

                case 15:
                    lRetorno = TipoCotistaFinancial.entidade_fechada_de_previdencia_complementar;
                    break;
                case 17:
                    lRetorno = TipoCotistaFinancial.fundos_e_clubes_de_Investimento;
                    break;
                case 18:
                    lRetorno = TipoCotistaFinancial.corretora_ou_distribuidora;
                    break;
                case 20:
                    lRetorno = TipoCotistaFinancial.corretora_ou_distribuidora;
                    break;
                case 21:
                    lRetorno = TipoCotistaFinancial.corretora_ou_distribuidora;
                    break;
                case 23:
                    lRetorno = TipoCotistaFinancial.outras_pessoas_juridicas_financeiras;
                    break;
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                    lRetorno = TipoCotistaFinancial.outros_tipos_de_cotistas_nao_relacionados;
                    break;
                case 99:
                    lRetorno = TipoCotistaFinancial.outros_tipos_de_cotistas_nao_relacionados;
                    break;
            }

            return lRetorno;
        }

        private string GetCodigoTributacaoFinancial(string codigo)
        {
            string lRetorno = string.Empty;
            switch (codigo)
            {
                case "1":
                case "2":
                case "3":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
                case "4":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "5":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
                case "6":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "8":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "11":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "13":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "15":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "17":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "18":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
                case "20":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "21":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "23":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "25":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
                case "26":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
                case "27":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
                case "28":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
                case "29":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
                case "30":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
                case "31":
                    lRetorno = CodigoTributacaoFinancial.Isentos;
                    break;
                case "99":
                    lRetorno = CodigoTributacaoFinancial.Tributados;
                    break;
            }
            return lRetorno;
        }

        private string GetCodigoTributacao(string codigo)
        {
            string lRetorno = string.Empty;
            switch (codigo)
            {
                case "1":
                case "2":
                case "3":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
                case "4":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "5":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
                case "6":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "8":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "11":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "13":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "15":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "17":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "18":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
                case "20":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "21":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "23":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "25":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
                case "26":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
                case "27":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
                case "28":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
                case "29":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
                case "30":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
                case "31":
                    lRetorno = CodigoTributacao.Isentos;
                    break;
                case "99":
                    lRetorno = CodigoTributacao.Tributados;
                    break;
            }
            return lRetorno;
        }

        private string GetSituacaoLegal(DateTime dtnascimento, string emancipado)
        {
            string lRetorno = SituacaoLegal.Maior;

            var anos = (dtnascimento - DateTime.Now).TotalDays;

            if (anos > 216)
            {
                lRetorno = SituacaoLegal.Maior;
            }

            if (emancipado == "1")
            {
                lRetorno = SituacaoLegal.Emancipado;
            }
            return lRetorno;
        }

        private string GetEstadoCivil(string codigo)
        {
            string lRetorno = string.Empty;

            switch (codigo)
            {
                case "5":
                    lRetorno = EstadoCivil.CASADO;
                    break;
                case "6":
                    lRetorno = EstadoCivil.CASADO;
                    break;
                case "7":
                    lRetorno = EstadoCivil.CASADO;
                    break;
                case "2":
                    lRetorno = EstadoCivil.DESQUITADO;
                    break;
                case "4":
                    lRetorno = EstadoCivil.DIVORCIADO;
                    break;
                case "9":
                    lRetorno = EstadoCivil.DESQUITADO_DIVORCIADO;
                    break;
                case "1":
                    lRetorno = EstadoCivil.SOLTEIRO;
                    break;
                case "8":
                    lRetorno = EstadoCivil.MARITAL_COMPANHEIRO;
                    break;
                case "3":
                    lRetorno = EstadoCivil.VIUVO;
                    break;

            }

            return lRetorno;
        }

        private string GetTipoCliente(string codigo)
        {
            string lRetorno = string.Empty;
            //TipoCotista

            switch (codigo)
            {
                case "1":
                    lRetorno = TipoCotista.Cliente_Varejo;
                    break;
                case "2":
                    lRetorno = TipoCotista.Cliente_Varejo;
                    break;
                case "3":
                    lRetorno = TipoCotista.Investidor_Nao_Residente;
                    break;
                case "4":
                    lRetorno = TipoCotista.Inst_Fin_Banco_Comercial;
                    break;
                case "6":
                    lRetorno = TipoCotista.Inst_Fin_Banco_de_Investimento;
                    break;
                case "8":
                    lRetorno = TipoCotista.Outros;
                    break;
                case "9":
                    lRetorno = TipoCotista.Inst_Fin_Seguradora;
                    break;
                case "11":
                    lRetorno = TipoCotista.Inst_Fin_Distribuidora;
                    break;
                case "13":
                    lRetorno = TipoCotista.Orgao_Publico;
                    break;
                case "15":
                    lRetorno = TipoCotista.Fundo_Previdencia_Fechado_Estados_e_Municipios;
                    break;
                case "17":
                    lRetorno = TipoCotista.Fundo_Outros;
                    break;
                case "18":
                    lRetorno = TipoCotista.Cliente_Varejo;
                    break;
                case "20":
                    lRetorno = TipoCotista.Inst_Fin_Corretora;
                    break;
                case "21":
                    lRetorno = TipoCotista.Inst_Fin_Corretora;
                    break;
                case "23":
                    lRetorno = TipoCotista.Outros;
                    break;
                case "25":
                    lRetorno = TipoCotista.Outros;
                    break;
                case "26":
                    lRetorno = TipoCotista.Outros;
                    break;
                case "27":
                    lRetorno = TipoCotista.Outros;
                    break;
                case "28":
                    lRetorno = TipoCotista.Outros;
                    break;
                case "29":
                    lRetorno = TipoCotista.Outros;
                    break;
                case "99":
                    lRetorno = TipoCotista.Outros;
                    break;
            }

            return lRetorno;
        }

        private string GetPorteEmpresa(string codigo)
        {
            string lRetorno = string.Empty;

            switch (codigo)
            {
                case "4":
                    lRetorno = PorteEmpresa.GRANDE;
                    break;
                case "6":
                    lRetorno = PorteEmpresa.GRANDE;
                    break;
                case "8":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "9":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "11":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "13":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "15":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "17":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "18":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "20":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "21":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "23":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "25":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "26":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "27":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "28":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "29":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
                case "99":
                    lRetorno = PorteEmpresa.NAO_CADASTRADA;
                    break;
            }
            return lRetorno;
        }

        private string GetSetorAtuacao(string codigo)
        {
            string lRetorno = string.Empty;

            switch (codigo)
            {
                case "4":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "6":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "8":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "9":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "11":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "13":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "15":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "17":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "18":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "20":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "21":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "23":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "25":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;
                case "26":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_ESTRANGEIRO;
                    break;
                case "27":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_ESTRANGEIRO;
                    break;
                case "28":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_ESTRANGEIRO;
                    break;
                case "29":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_ESTRANGEIRO;
                    break;
                case "99":
                    lRetorno = SetorAtuacao.PRIVADO_CAPITAL_NACIONAL;
                    break;

            }

            return lRetorno;
        }

        private string GetOrgaosEmissoresItau(string codigo)
        {
            string lRetorno = string.Empty;

            switch (codigo)
            {
                case "CART":
                    lRetorno = OrgaosEmissoresItau.CARTORIO_REG_CIVIL_PESSOAS_NATURAIS;
                    break;
                case "RNE ":
                    lRetorno = OrgaosEmissoresItau.SERV_ESTRANG_DEPTO_POL_MARIT_AEREA_FRONT_POL_FED;
                    break;
                case "CFEF":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_EDUCACAO_FISICA;
                    break;
                case "CNT":
                    lRetorno = OrgaosEmissoresItau.DEPTO_DE_TRANSITO;
                    break;
                case "CREA":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_ENG_ARQUIT_E_AGRON;
                    break;
                case "CRTR":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_TEC_DE_RADIOLOGIA;
                    break;
                case "CON":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_CONTABILIDADE;
                    break;
                case "CRA":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_ADMINISTRACAO;
                    break;
                case "CRC":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_CONTABILIDADE;
                    break;
                case "CRCI":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_CORRETORES_DE_IMOVEIS;
                    break;
                case "CRE":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_ECONOMIA;
                    break;
                case "CREM":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_ENFERMAGEM;
                    break;
                case "CREN":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_ENFERMAGEM;
                    break;
                case "CRF":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_ADMINISTRACAO;
                    break;
                case "CRMV":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_MED_VETERINARIA;
                    break;
                case "CRN":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_NUTRICIONISTAS;
                    break;
                case "CRP":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_PSICOLOGIA;
                    break;
                case "CRM":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_MEDICINA;
                    break;
                case "CRO":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_ODONTOLOGIA;
                    break;
                case "CRQ":
                    lRetorno = OrgaosEmissoresItau.CONS_REG_DE_QUIMICA;
                    break;
                case "DPMA":
                    lRetorno = OrgaosEmissoresItau.SECR_DE_EST_DE_OBRAS_E_MEIO_AMBIENTE;
                    break;
                case "DPF":
                    lRetorno = OrgaosEmissoresItau.DIR_GERAL_DA_POLICIA_CIVIL;
                    break;
                case "DPT":
                    lRetorno = OrgaosEmissoresItau.DEPTO_DE_POLICIA_TECNICO_CIENTIFICA;
                    break;
                case "CNH":
                    lRetorno = OrgaosEmissoresItau.DEPTO_DE_TRANSITO;
                    break;
                case "DETR":
                    lRetorno = OrgaosEmissoresItau.DEPTO_DE_TRANSITO;
                    break;
                case "DIC":
                    lRetorno = OrgaosEmissoresItau.DEPTO_DE_TRANSITO;
                    break;
                case "DGPC":
                    lRetorno = OrgaosEmissoresItau.DIR_GERAL_DA_POLICIA_CIVIL;
                    break;
                case "IIRG":
                    lRetorno = OrgaosEmissoresItau.POLICIA_CIVIL_INST_IDENT;
                    break;
                case "SDS":
                    lRetorno = OrgaosEmissoresItau.INST_NACIONAL_DE_IDENTIFICACAO;
                    break;
                case "IIPR":
                    lRetorno = OrgaosEmissoresItau.INST_DE_IDENT_EST_PARANA;
                    break;
                case "IFP":
                    lRetorno = OrgaosEmissoresItau.INST_FELIX_PACHECO;
                    break;
                case "JT":
                    lRetorno = OrgaosEmissoresItau.TRIBUNAL_SUPERIOR_DO_TRABALHO;
                    break;
                case "MAER":
                    lRetorno = OrgaosEmissoresItau.MIN_DA_AERONAUTICA;
                    break;
                case "MJ":
                    lRetorno = OrgaosEmissoresItau.MIN_DA_JUSTICA;
                    break;
                case "MM":
                    lRetorno = OrgaosEmissoresItau.MIN_DA_MARINHA;
                    break;
                case "MRE":
                    lRetorno = OrgaosEmissoresItau.MIN_DE_RELACOES_EXTERIORES;
                    break;
                case "ME":
                    lRetorno = OrgaosEmissoresItau.MIN_DO_EXERCITO;
                    break;
                case "OAB":
                    lRetorno = OrgaosEmissoresItau.ORDEM_DOS_ADVOGADOS_DO_BRASIL;
                    break;
                case "PF":
                    lRetorno = OrgaosEmissoresItau.DIR_GERAL_DA_POLICIA_CIVIL;
                    break;
                case "PM":
                    lRetorno = OrgaosEmissoresItau.POLICIA_MILITAR;
                    break;
                case "SJS":
                    lRetorno = OrgaosEmissoresItau.SECR_DA_JUSTICA_E_SEG_PUBLICA;
                    break;
                case "SEPC":
                    lRetorno = OrgaosEmissoresItau.SECR_DE_EST_DA_POLICIA_CIVIL;
                    break;
                case "SESP":
                    lRetorno = OrgaosEmissoresItau.SECR_DE_EST_SEG_PUBLICA;
                    break;
                case "SEDS":
                    lRetorno = OrgaosEmissoresItau.SECR_DE_EST_DA_DEFESA_SOCIAL;
                    break;
                case "SSP":
                    lRetorno = OrgaosEmissoresItau.SECR_DE_SEG_PUBLICA;
                    break;
                case "SPTC":
                    lRetorno = OrgaosEmissoresItau.SUPERINT_POLICIA_TEC_CIENTIFICA;
                    break;
            }

            return lRetorno;
        }

        #endregion
    }
}
