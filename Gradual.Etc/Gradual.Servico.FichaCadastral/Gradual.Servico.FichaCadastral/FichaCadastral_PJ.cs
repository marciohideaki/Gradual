using System;
using System.Collections.Generic;
using System.Configuration;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Dados;
using Gradual.Servico.FichaCadastral.Lib;
using log4net;
using Newtonsoft.Json;

namespace Gradual.Servico.FichaCadastral
{
    public class FichaCadastral_PJ
    {
        #region | Atributos

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int _IdCliente;

        double LinhaAtual = 20;
        Root.Reports.Page PaginaPDF;
        Root.Reports.FontProp FontProp;
        Root.Reports.Report ReportCadastro;
        Root.Reports.FontDef FontDefin;

        private string pathVirtualPDF = "../ReportPDF";
        private string pathVirtualImages = "../../Images/";
        private string pathVirtualImagesCadastro = "../../images/cadastro/";
        private string _ServerMapPath;
        private string fileNameFichaCadastral;
        private string fileNameFichaCadastralCambio;
        private FichaCadastralInfo gFichaCadastralInfo = null;

        private static bool lEhCambio = false;

        #endregion

        #region | Propriedades

        /// <summary>
        /// Retorna o Id do cliente
        /// </summary>
        private int GetIdCliente
        {
            get { return this._IdCliente; }
            set { this._IdCliente = value; }
        }

        public SistemaOrigem SitemaOrigem { get; set; }

        private int GetCodigoBovespa { get; set; }

        private string GetEnderecoArquivoIntranet
        {
            get
            {
                return ConfigurationManager.AppSettings["pathVirtualIntranet"];
            }
        }

        private string GetEnderecoArquivoPortal
        {
            get
            {
                return ConfigurationManager.AppSettings["pathVirtualPortal"];
            }
        }

        private string GetEnderecoArquivo
        {
            get
            {
                return ConfigurationManager.AppSettings["pathVirtualPDF"];
            }
        }

        private Gradual.Intranet.Contratos.Dados.FichaCadastralInfo GetFichaCadastralInfo
        {
            get
            {
                if (null == this.gFichaCadastralInfo)
                {
                    var lFichaCadastral = FichaCadastralDbLib.ReceberFichaCadastral(new ReceberEntidadeRequest<FichaCadastralInfo>()
                    {
                        Objeto = new FichaCadastralInfo() { IdCliente = this.GetIdCliente, }
                    });

                    if (null != lFichaCadastral)
                    {
                        this.gFichaCadastralInfo = lFichaCadastral.Objeto;

                        if (lFichaCadastral.Objeto.ClienteResponse.Objeto.TpDesejaAplicar.Equals("CAMBIO") || lFichaCadastral.Objeto.ClienteResponse.Objeto.TpDesejaAplicar.Equals("BOV/CAM") || lFichaCadastral.Objeto.ClienteResponse.Objeto.TpDesejaAplicar.Equals("FUN/CAM"))
                        {
                            lEhCambio = true;
                        }
                    }
                }

                return this.gFichaCadastralInfo;
            }
        }

        private bool StatusPessoaVinculada
        {
            get
            {
                var lRetorno = new PessoaVinculadaDbLib().ConsultarPessoaVinculadaPorCliente(new ConsultarEntidadeRequest<ClientePessoaVinculadaPorClienteInfo>()
                {
                    Objeto = new ClientePessoaVinculadaPorClienteInfo()
                    {
                        IdCliente = this.GetIdCliente
                    }
                });

                return null != lRetorno && lRetorno.Resultado.Count.CompareTo(0).Equals(1);
            }
        }

        private ClienteSituacaoFinanceiraPatrimonialInfo GetSituacaoFinanceiraPatrimonial
        {
            get
            {
                var lRetorno = new ClienteSituacaoFinanceiraPatrimonialDbLib().ReceberClienteSituacaoFinanceiraPatrimonial(
                    new ReceberEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo>()
                    {
                        Objeto = new ClienteSituacaoFinanceiraPatrimonialInfo
                        {
                            IdCliente = this.GetIdCliente
                        }
                    });

                return lRetorno.Objeto;
            }
        }

        #endregion

        #region | Métodos Servico

        public ReceberObjetoResponse<FichaCadastralGradualInfo> GerarFichaCadastral_PJ(ReceberEntidadeRequest<FichaCadastralGradualInfo> pParametro)
        {
            this.GetIdCliente = pParametro.Objeto.IdCliente;
            this.pathVirtualPDF = ConfigurationManager.AppSettings["pathVirtualPDF"];
            this.pathVirtualImages = ConfigurationManager.AppSettings["pathVirtualImages"];
            this.pathVirtualImagesCadastro = ConfigurationManager.AppSettings["pathVirtualImagesCadastro"];
            this.SitemaOrigem = pParametro.Objeto.SitemaOrigem;

            try
            {
                this.GeraRelatorio(); //--> Gerando o relatório em arquivo.
            }
            catch (Exception ex)
            {
                gLogger.Error(ex.ToString(), ex);
                throw ex;
            }

            return new ReceberObjetoResponse<Gradual.Servico.FichaCadastral.Lib.FichaCadastralGradualInfo>()
            {
                Objeto = new Gradual.Servico.FichaCadastral.Lib.FichaCadastralGradualInfo()
                {
                    PathFichaCadastral = string.Concat(this.fileNameFichaCadastral),
                    PathFichaCadastralCambio = lEhCambio ? string.Concat(this.fileNameFichaCadastralCambio) : String.Empty
                }
            };
        }

        #endregion

        #region | TriggersRelatorios

        private void CriaFichaDuc()
        {
            this.LinhaAtual = 25;
            this.MontaFichaCadastralCliente();
            this.IsFinalPagina(0);
            this.MontaNomeControladoresAdmProcuradores();
            this.IsFinalPagina(0);
            this.MontaNomeDasPessoasAutorizadas();
            this.IsFinalPagina(0);
            this.MontaFontesReferenciaBancaria();
            this.IsFinalPagina(0);
            this.MontaInformacoesRendimentosESituacaoFinanceira();
            this.IsFinalPagina(0);
            this.MontaBensImoveis();
            this.IsFinalPagina(0);
            this.MontaInformacoesSocietarias();
            this.IsFinalPagina(0);
            this.DeclaracoesAutorizacoesCliente();
            this.IsFinalPagina(0);

            this.MontaDeclaracaoResponsavelNaCorretora();
            for (int i = 0; i < 3; i++)
            {
                this.IsFinalPagina(0);
                this.MontaTermoDeIdentificacaoDeProcurador();
            }
        }

        private void CriaFichaCambio()
        {
            this.LinhaAtual = 25;
            this.MontaFichaCadastralCliente(true);
            this.IsFinalPagina(0);
            this.MontaNomeControladoresAdmProcuradores();
            //this.IsFinalPagina(0);
            //this.MontaNomeDasPessoasAutorizadas();
            this.IsFinalPagina(0);
            this.MontaFontesReferenciaBancaria();
            this.IsFinalPagina(0);
            this.MontaInformacoesRendimentosESituacaoFinanceira();
            this.IsFinalPagina(0);
            //this.MontaBensImoveis();
            //this.IsFinalPagina(0);
            this.MontaInformacoesSocietarias();
            this.IsFinalPagina(0);
            //this.DeclaracoesAutorizacoesCliente();
            //this.IsFinalPagina(0);
            //this.MontaComoConheceu();
            //this.IsFinalPagina(0);
            this.MontaDeclaracaoResponsavelNaCorretora_Cambio();
            this.IsFinalPagina(0);
        }

        /// <summary>
        /// Retorna endereço para responsável para abrir uma janela no Window.Open contendo a Ficha DUC.
        /// </summary>
        public string CaminhoDownLoadRelatorio()
        {
            return string.Concat(pathVirtualPDF, "/", fileNameFichaCadastral);
        }

        /// <summary>
        /// Procedimento que dispara a geração dos relatórios.
        /// </summary>
        private void GeraRelatorio()
        {
            Guid g;
            g = Guid.NewGuid();

            this.ReportCadastro = new Root.Reports.Report(new Root.Reports.PdfFormatter());

            this.FontDefin = new Root.Reports.FontDef(this.ReportCadastro, Root.Reports.FontDef.StandardFont.Helvetica);
            this.FontProp = new Root.Reports.FontProp(this.FontDefin, 6);

            this.NovaPagina(0);

            this.CriaFichaDuc();

            //this.fileNameFichaCadastral = string.Format("FichaCadastral-{0}.pdf", this.GetIdCliente.ToString());
            this.fileNameFichaCadastral = string.Format("{0}", g.ToString());

            //--> Gravando o arquivo
            if (this.SitemaOrigem == SistemaOrigem.Portal)
                this.ReportCadastro.Save(string.Concat(this.GetEnderecoArquivoPortal, "\\", fileNameFichaCadastral));
            else
                this.ReportCadastro.Save(string.Concat(this.GetEnderecoArquivoIntranet, "\\", fileNameFichaCadastral));

            #region Ficha Cadastral Câmbio
            if (lEhCambio)
            {
                this.ReportCadastro = new Root.Reports.Report(new Root.Reports.PdfFormatter());
                this.FontDefin = new Root.Reports.FontDef(this.ReportCadastro, Root.Reports.FontDef.StandardFont.Helvetica);
                this.FontProp = new Root.Reports.FontProp(this.FontDefin, 6);
                this.NovaPagina(0);
                this.CriaFichaCambio();
                this.fileNameFichaCadastralCambio = string.Format("FichaCadastralCambio-{0}.pdf", this.GetIdCliente.ToString());

                //--> Gravando o arquivo
                if (this.SitemaOrigem == SistemaOrigem.Portal)
                    this.ReportCadastro.Save(string.Concat(this.GetEnderecoArquivoPortal, "\\", fileNameFichaCadastralCambio));
                else
                    this.ReportCadastro.Save(string.Concat(this.GetEnderecoArquivoIntranet, "\\", fileNameFichaCadastralCambio));
            }
            #endregion
        }

        #endregion

        #region | Métodos Ficha Duc

        private void MontaBarraTitulo(double linha, string titulo)
        {
            Root.Reports.FontProp fontProp = new Root.Reports.FontProp(this.FontDefin, 6, System.Drawing.Color.WhiteSmoke);

            this.PaginaPDF.AddCB_MM(105, linha, new Root.Reports.RepString(fontProp, titulo));
        }

        /// <summary>
        /// Método para montar o Rodapé
        /// </summary>
        private void MontaRodape()
        {
            double linha = 270;

            Root.Reports.FontProp fontProp = new Root.Reports.FontProp(this.FontDefin, 5);
            fontProp.bBold = true;

            this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, "GRADUAL CCTVM S/A"));
            fontProp.bBold = false;
            string tx = "Av. Presidente Juscelino Kubitschek 50, 5º e 6º andares - Vila Nova Conceição - São Paulo - SP - 04543-011";
            linha = linha + 3;
            this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, tx));
            tx = "Tel (11)3372-8300 - FAX (11)3372-8301  |  www.gradualinvestimentos.com.br";
            linha = linha + 3;
            this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, tx));
            fontProp.bUnderline = false;
            linha = linha + 3;
            this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, "Ouvidoria/SAC: 0800 723 7444"));
            fontProp.bUnderline = false;
        }

        /// <summary>
        /// Gera informações da declaração responsável
        /// </summary>
        private void MontaDeclaracaoResponsavelNaCorretora()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(ServerMapPath(pathVirtualImages), "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "DECLARAÇÃO DO RESPONSÁVEL NA CORRETORA PELO CADASTRAMENTO DO CLIENTE");
                this.LinhaAtual += 3;
            }

            string texto0 = "\"Responsabilizo-me pela exatidão das informações prestadas, a vista dos originais dos documentos de Identidade, CPF/CNPJ, e outros comprobatórios dos demais elementos de informação apresentados, sob pena de",
                   texto1 = "aplicação do disposto no artigo 64 da Lei nº 8383 de 30 de dezembro de 1991\".";

            double fontAnt = this.FontProp.rSize;
            this.FontProp.rSize = 4;
            this.PaginaPDF.AddLT_MM(8, LinhaAtual, new Root.Reports.RepString(this.FontProp, texto0));
            this.LinhaAtual += 3;
            this.PaginaPDF.AddLT_MM(8, LinhaAtual, new Root.Reports.RepString(this.FontProp, texto1));
            this.FontProp.rSize = fontAnt;

            this.LinhaAtual += 10;

            this.InsereLinhaContinua(20, this.LinhaAtual, 80, 10, true);
            this.TextoValorItem(LinhaAtual + 2, 110, "Local e data", string.Empty);
            this.InsereLinhaContinua(130, this.LinhaAtual, 65, 10, true);
            this.LinhaAtual += 5;
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(25, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, "Assinatura do RESPONSÁVEL NA GRADUAL CCTVM S/A"));
            this.FontProp.bBold = false;

            this.QuebraLinhaDupla();

            this.FontProp.bBold = true;
            this.FontProp.rSize = 4;
            this.PaginaPDF.AddLT_MM(8, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, "Obs.: As fichas cadastrais de clientes devem ter em anexo cópias:"));
            this.FontProp.bBold = false;
            this.LinhaAtual += 3;
            // "",
            string S1a = " Cartão do CNPJ, Último Balanço Auditado, Contrato Social ou Estatuto Social registrado no órgão competente, eventuais alterações contratuais ou Ata de Eleição da Atual Diretoria, Procurações dos Representantes",
                   S1b = " Legais da Empresa, CPF e RG (Identidade) dos emissores de ordens, Controladores, Administradores e Procuradores que assinem a Ficha Cadastral e Contrato.";
            this.PaginaPDF.AddLT_MM(8, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, S1a));
            this.LinhaAtual += 3;
            this.PaginaPDF.AddLT_MM(8, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, S1b));

            this.FontProp.rSize = fontAnt;
        }


        private void MontaFichaCadastralCliente()
        {
            MontaFichaCadastralCliente(false);
        }

        private void MontaFichaCadastralCliente(bool CabecalhoCambio)
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 5;
                string filePath = string.Concat(ServerMapPath(pathVirtualImages), "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);

                string lTitulo = String.Empty;
                if (CabecalhoCambio)
                {
                    lTitulo = "FICHA CADASTRAL DE CLIENTE - CÂMBIO (PESSOA JURÍDICA)";
                }
                else
                {
                    lTitulo = "FICHA CADASTRAL DE CLIENTE (PESSOA JURÍDICA)";
                }

                this.MontaBarraTitulo(LinhaAtual, lTitulo);

                this.MontaBarraTitulo(LinhaAtual, "");
                this.LinhaAtual += 20;
            }

            var entCliente = this.GetFichaCadastralInfo.ClienteResponse.Objeto;
            var entLogin = this.GetFichaCadastralInfo.ClienteLoginResponse.Objeto;
            var entConta = this.GetFichaCadastralInfo.ClienteContaResponse.Resultado.Find(delegate(ClienteContaInfo cci) { return eAtividade.BOL.Equals(cci.CdSistema) || eAtividade.BMF.Equals(cci.CdSistema); });
            var entEndereco = this.GetFichaCadastralInfo.ClienteEnderecoResponse.Resultado.Find(delegate(ClienteEnderecoInfo cei) { return cei.StPrincipal; });
            var entTelefoneCom = this.GetFichaCadastralInfo.ClienteTelefoneReponse.Resultado.Find(delegate(ClienteTelefoneInfo cti) { return cti.StPrincipal; });
            var entTelefoneFax = this.GetFichaCadastralInfo.ClienteTelefoneReponse.Resultado.Find(delegate(ClienteTelefoneInfo cti) { return cti.IdTipoTelefone == 4; });

            this.LinhaAtual = this.LinhaAtual - 15;

            this.TextoValorItem(LinhaAtual, 8, "Razão Social", entCliente.DsNome.ToStringFormatoNome());

            this.TextoValorItem(LinhaAtual, 103, "Código Cliente", null != entConta ? entConta.CdCodigo.ToCodigoClienteFormatado() : string.Empty);

            this.TextoValorItem(LinhaAtual, 140, "Assessor", this.RecuperarDadosDoAssessor(entCliente.IdAssessorInicial));

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Sede Social (endereço)", (null != entEndereco) ? entEndereco.DsLogradouro.ToStringFormatoNome() : string.Empty);

            this.TextoValorItem(LinhaAtual, 103, "Número", (null != entEndereco) ? entEndereco.DsNumero : string.Empty);

            this.TextoValorItem(LinhaAtual, 140, "Complemento", (null != entEndereco) ? entEndereco.DsComplemento : string.Empty);

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Bairro", (null != entEndereco) ? entEndereco.DsBairro.ToStringFormatoNome() : string.Empty);

            this.TextoValorItem(LinhaAtual, 60, "Cidade", (null != entEndereco) ? entEndereco.DsCidade.ToStringFormatoNome() : string.Empty);

            this.TextoValorItem(LinhaAtual, 103, "Estado", (null != entEndereco) ? entEndereco.CdUf : string.Empty);

            this.TextoValorItem(LinhaAtual, 140, "País", (null != entEndereco) ? this.RecuperarPais(entEndereco.CdPais).ToStringFormatoNome() : string.Empty);

            this.TextoValorItem(LinhaAtual, 175, "CEP", (null != entEndereco) ? string.Format("{0}-{1}", entEndereco.NrCep.DBToString().PadLeft(5, '0'), entEndereco.NrCepExt.DBToString().PadLeft(3, '0')) : string.Empty);

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Atividade Principal", this.RecuperarProfissao(entCliente.CdAtividadePrincipal).ToStringFormatoNome());

            this.TextoValorItem(LinhaAtual, 60, "Telefone (DDD + número)", (null != entTelefoneCom) ? string.Concat(entTelefoneCom.DsDdd, "-", entTelefoneCom.DsNumero) : string.Empty);

            this.TextoValorItem(LinhaAtual, 103, "Fax (DDD + número)", (null != entTelefoneFax) ? string.Concat(entTelefoneFax.DsDdd, "-", entTelefoneFax.DsNumero) : string.Empty);

            this.TextoValorItem(LinhaAtual, 140, "E-Mail (endereco eletrônico)", entLogin.DsEmail.ToLower());

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "CNPJ nº", (null != entCliente) ? entCliente.DsCpfCnpj.ToCpfCnpjString() : string.Empty);

            this.TextoValorItem(LinhaAtual, 60, "Data de Constituição na RF", (null != entCliente && null != entCliente.DtNascimentoFundacao) ? entCliente.DtNascimentoFundacao.Value.ToString("dd/MM/yyyy") : string.Empty);

            this.TextoValorItem(LinhaAtual, 103, "Forma de Constiuição", (null != entCliente) ? entCliente.DsFormaConstituicao.TrimEnd('.') : string.Empty);

            this.TextoValorItem(LinhaAtual, 140, "Agente Custódia", string.Empty);

            this.TextoValorItem(LinhaAtual, 175, "Cliente Custódia", string.Empty);
        }

        private void MontaInformacoesSocietarias()
        {
            var filePath = string.Empty;

            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                filePath = string.Concat(ServerMapPath(pathVirtualImages), "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "INFORMAÇÕES SOCIETÁRIAS");
                this.LinhaAtual += 5;
            }

            var entControladora = this.GetFichaCadastralInfo.ClienteControladoraResponse.Resultado;

            this.PaginaPDF.AddLT_MM(15, LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, "Existe pessoa jurídica ou grupo controlador dessa empresa?"));

            this.LinhaAtual += 5;

            this.PaginaPDF.AddLT_MM(25, LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, "Sim"));
            this.PaginaPDF.AddLT_MM(46, LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, "Não"));

            if (null != entControladora && entControladora.Count > 0)
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 1.25, 21, filePath, 7, 7); //--> CheckBox Sim do item 1

                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 1.25, 42, filePath, 7, 7);//--> CheckBox Não do item 2

                this.LinhaAtual += 7;

                this.PaginaPDF.AddLT_MM(15, LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, "Em caso afirmativo, informe a razão social ou a denominação e o CNPJ da(s) empresas(s) controladora(s):"));

                this.LinhaAtual += 4;

                var lControladores = new System.Text.StringBuilder();

                entControladora.ForEach(delegate(ClienteControladoraInfo cci)
                {
                    this.PaginaPDF.AddLT_MM(15, LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, string.Format("{0} - CNPJ: {1};", cci.DsNomeRazaoSocial.ToStringFormatoNome(), cci.DsCpfCnpj.ToCpfCnpjString())));
                    this.LinhaAtual += 5;
                });
            }
            else
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 1.25, 21, filePath, 7, 7); //--> CheckBox Sim do item 1

                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 1.25, 42, filePath, 7, 7);//--> CheckBox Não do item 2
            }
        }

        private void DeclaracoesAutorizacoesCliente()
        {
            this.LinhaAtual += 10;

            double padding = 10;

            var entCliente = this.GetFichaCadastralInfo.ClienteResponse.Objeto;
            var entEndereco = this.GetFichaCadastralInfo.ClienteEnderecoResponse.Resultado;
            var entRepresentante = this.GetFichaCadastralInfo.ClienteProcuradorRepresentanteResponse.Resultado;

            double tamAnt = this.FontProp.rSize;
            this.FontProp.rSize = 4;

            string xii = "(As informações acima são obrigatórias, decorrentes da Lei nº 9.613, da Circular nº 2.852 e da Carta Circular nº 2826 do Banco do Brasil e da Instituição nº 301 da Comissão de valores mobiliários e serão mantidos",
                   xiii = "confidencialmente. Declaro, na forma da lei, que são verdadeiras as informações abaixo descritas, estando ciente de que será usado para fins de atualização patrimonial e limite operacional).";
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, xii));
            this.LinhaAtual += 3;
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, xiii));
            this.LinhaAtual += 7;

            this.FontProp.rSize = tamAnt;

            string filePath = string.Concat(ServerMapPath(pathVirtualImages), "pixel_black.jpg");
            this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
            this.MontaBarraTitulo(this.LinhaAtual, "DECLARAÇÕES E AUTORIZAÇÕES DO CLIENTE");
            this.LinhaAtual += 5;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "1", "Opera por conta própria? Caso negativo, informe o nome para quem pretende operar", ".");
            this.LinhaAtual += 5;

            {   //--> CheckBox Sim e Não do item 1

                this.PaginaPDF.AddLT_MM(25, LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, "Sim"));
                this.PaginaPDF.AddLT_MM(46, LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, "Não, por conta de:"));

                if (null != entCliente.StCarteiraPropria && entCliente.StCarteiraPropria.Value)
                {
                    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 1.25, 21, filePath, 7, 7); //--> CheckBox Sim do item 1

                    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 1.25, 42, filePath, 7, 7);//--> CheckBox Não do item 2
                }
                else
                {
                    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 1.25, 21, filePath, 7, 7); //--> CheckBox Sim do item 1

                    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 1.25, 42, filePath, 7, 7);//--> CheckBox Não do item 2

                    var nomeRepresentante = (entRepresentante != null && entRepresentante.Count > 0) ? entRepresentante[0].DsNome : string.Empty;

                    this.PaginaPDF.AddLT_MM(69, LinhaAtual - 1.0, new Root.Reports.RepString(this.FontProp, nomeRepresentante));
                }

                this.InsereLinhaContinua(67, LinhaAtual - 2, 80, 8, false);
            }

            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "2", "É pessoa vinculada à corretora (contato definido pela instrução CVM nº 505/2011)?", ".");

            this.PaginaPDF.AddLT_MM(120, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Sim"));
            this.PaginaPDF.AddLT_MM(135, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Não"));

            {   //--> CheckBox Sim e Não do item 3
                if (this.StatusPessoaVinculada)
                {
                    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 116.75, filePath, 7, 7);
                    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 131.5, filePath, 7, 7);
                }
                else
                {
                    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 116.75, filePath, 7, 7);
                    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 131.5, filePath, 7, 7);
                }
            }

            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "3", "Autoriza a transmissão de ordens por procurador ou representante?", ".");

            {   //--> CheckBox Sim e Não do item 2
                this.PaginaPDF.AddLT_MM(106, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Sim"));
                this.PaginaPDF.AddLT_MM(124, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Não"));

                if (entCliente.DsAutorizadoOperar == null)
                {
                    if (entRepresentante != null && entRepresentante.Count > 0)
                    {
                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                        this.InsereImagem(LinhaAtual - 0.25, 102.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.25, 120.25, filePath, 7, 7); //--> CheckBox Não do item 2
                    }
                    else
                    {
                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.25, 102.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                        this.InsereImagem(LinhaAtual - 0.25, 120.25, filePath, 7, 7); //--> CheckBox Não do item 2
                    }
                }
                else
                {
                    //if (entCliente.AutorizaTerceiro == 'S')
                    if (entCliente.DsAutorizadoOperar == "N")
                    {
                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                        this.InsereImagem(LinhaAtual - 0.25, 102.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.25, 120.25, filePath, 7, 7); //--> CheckBox Não do item 2
                    }
                    else
                    {
                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.25, 102.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                        this.InsereImagem(LinhaAtual - 0.25, 120.25, filePath, 7, 7); //--> CheckBox Não do item 2
                    }
                }
            }

            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S2 = "(Em caso positivo anexar a procuração ou documento específico, comprometendo-se a informar por escrito à Corretora no caso de revogação de mandato).";
            this.LinhaSubTopico(LinhaAtual, 14, S2);

            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "4", "Concordo que a carteira própria da corretora ou pessoas a ela vinculadas podem atuar na contraparte das operações que ordenar", "."); //--> trocar

            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S5a = "(Caso a opção seja: não concordo ou concordo sob consulta, providenciar correspondência assinada explicando a opção)";

            this.LinhaSubTopico(LinhaAtual, 14, S5a);

            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S5b = "(Esta declaração é obrigatória somente quando se tratar de clientes cuja carteira individual é administrada pela corretora)";

            this.LinhaSubTopico(LinhaAtual, 14, S5b);

            this.LinhaAtual += 3;
             
            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto


            // Parte de "NON US Person"

            TransporteDetalhesUSPersonPJ lDetalhesUS;

            if (string.IsNullOrEmpty(entCliente.DsUSPersonPJDetalhes))
            {
                lDetalhesUS = new TransporteDetalhesUSPersonPJ();
            }
            else
            {
                lDetalhesUS = JsonConvert.DeserializeObject<TransporteDetalhesUSPersonPJ>(entCliente.DsUSPersonPJDetalhes);
            }

            this.LinhaTopico(this.LinhaAtual, 8, 1, "5", "Com base na Regulamentação Tributária dos EUA sobre Pessoa NAO Domiciliada ['Non US Person'] ou Pessoa Domiciliada ['US Person'],", ".");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, "existem Administradores, Diretores ou Representantes Legais nas condições discriminadas a seguir:");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, "É cidadão dos EUA? [nacionalidade dupla ou única]     Sim:           Não:");
            
            string lDetail1 = "";
            string lDetail2 = "";

            //--> CheckBox cidadão dos EUA
            if (lDetalhesUS.Flag_USPersonNacional)
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 80.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 93.25, filePath, 7, 7); //--> CheckBox Não do item 2

                lDetail1 = lDetalhesUS.USPersonNacional_Nome;
                lDetail2 = lDetalhesUS.USPersonNacional_Nacionalidades;
            }
            else
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 80.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 93.25, filePath, 7, 7); //--> CheckBox Não do item 2

                lDetail1 = "__________________________________________________";
                lDetail2 = "_____________________________";
            }

            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, "Nome Completo: " + lDetail1 + "   Relacione todas, se mais de uma: " + lDetail2);
            /*
            //--> CheckBox cidadão dos EUA
            if (lDetalhesUS.Flag_USPersonNacional)
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                lDetail1 = lDetalhesUS.USPersonNacional_Nome;
            }
            else
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                lDetail1 = "______________________________________________________________________________________________";
            }

            this.InsereImagem(LinhaAtual - 0.3, 32.0, filePath, 7, 7);
            */
            
            this.LinhaAtual += 3;

            this.LinhaSubTopico(LinhaAtual, 14, "É residente permanente dos EUA? ['US resident alien'] Sim:           Não:");
            
            //--> CheckBox cidadão dos EUA
            if (lDetalhesUS.Flag_USPersonResidente)
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 80.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 93.25, filePath, 7, 7); //--> CheckBox Não do item 2

                lDetail1 = lDetalhesUS.USPersonResidente_Nome;
            }
            else
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 80.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 93.25, filePath, 7, 7); //--> CheckBox Não do item 2

                lDetail1 = "__________________________________________________";
            }

            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, "Nome Completo: " + lDetail1);
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, "Especificar: É titular de 'Green Card'        ou Atende Requisitos da chamada 'Presença Física Substancial'");

            if (lDetalhesUS.Flag_USPersonGreen)
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 54.5, filePath, 7, 7); //--> CheckBox Sim do item 2
            }
            else
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 54.5, filePath, 7, 7); //--> CheckBox Não do item 2
            }

            if (lDetalhesUS.Flag_USPersonPresenca)
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 130.0, filePath, 7, 7); //--> CheckBox Sim do item 2
            }
            else
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 130.0, filePath, 7, 7); //--> CheckBox Não do item 2
            }
            
            this.LinhaAtual += 3;



            this.LinhaSubTopico(LinhaAtual, 14, "Nasceu nos EUA? Sim:           Não:");

            if (lDetalhesUS.Flag_USPersonNascido)
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 41.5, filePath, 7, 7); //--> CheckBox Sim do item 2
                
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 54.5, filePath, 7, 7); //--> CheckBox Não do item 2
            }
            else
            {
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 41.5, filePath, 7, 7); //--> CheckBox Não do item 2
                
                filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.25, 54.5, filePath, 7, 7); //--> CheckBox Sim do item 2
            }

            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, "Se nasceu nos EUA e julga NÃO ser um 'US Person' indique o motivo e apresente documentação que comprove a renúncia.");
            this.LinhaAtual += 3;

            if (!string.IsNullOrEmpty(lDetalhesUS.USPersonRenuncia_Motivo))
            {
                lDetail1 = lDetalhesUS.USPersonRenuncia_Motivo;
            }
            else
            {
                lDetail1 = "_________________________________________________________";
            }
            
            if (!string.IsNullOrEmpty(lDetalhesUS.USPersonRenuncia_Documento))
            {
                lDetail2 = lDetalhesUS.USPersonRenuncia_Documento;
            }
            else
            {
                lDetail2 = "______________________";
            }

            this.LinhaSubTopico(LinhaAtual, 14, string.Format("Motivo: {0} Tipo Documento em anexo: {1}", lDetail1, lDetail2));
            
            this.LinhaAtual += 3;
            
            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S6a = "São consideradas válidas ordens transmitidas verbalmente ou por escrito, conforme determina o documento Regras e Parâmetros de Atuação da Gradual CCTVM S/A. ";
            string S6b = "(Caso a opção seja: Considerar válidas as ordens transmitidas exclusivamente por escrito, encaminhar correspondeência com assinatura e firma reconhecida, solicitando ";
            string S6c = "aplicando o produto da venda do o aceite pela corretora, que protocolará, tornando-a parte integrante do cadastro).";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "6", S6a, ".");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, S6b);
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, S6c);

            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "7", "Não estou impedido de operar no mercado de valores mobiliários.", ".");

            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S8a = "Tenho conhecimento do disposto nas instruções CVM nº 505 e 506/2011, das Regras e Parâmetros de Atualção da Corretora e das normas Operacionais, do Fundo",
                   S8b = "de Garantia das Bolsas, e das normas operacionais editadas pelas Bolsas e pelas Câmaras de Compensalção e Liquidação, as quais estão disponíveis nos respectivos sites.";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "8", S8a, ".");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, S8b);

            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto //--> Verifica se a página deve ser quebrada neste ponto

            string S9a = "Tenho conhecimento de que as operações realizadas no sistema de negociação de títulos e valores mobiliários mantidos pela SOMA não constam contam com a proteção de ",
                   S9b = "fundo de garantia.";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "9", S9a, ".");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, S9b);
            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 0, "10", " Declaro, conforme determina a ICVM 553/2014, que a natureza e o propósito da relação de negócios com esta Corretora se dará com o seguinte fim:", ".");
            this.LinhaAtual += 3;
            //this.LinhaSubTopico(LinhaAtual, 15, entCliente.DsPropositoGradual);

            string sLinha1 = "Investimento em BOVESPA       Investimento em BM&F        Fundo de Investimento       Aluguel Ações (BTC)",
            sLinha2 = "Derivativos      Renda Fixa      Operações de mercado de Cambio      Outros:";

            this.LinhaSubTopico(LinhaAtual, 14, sLinha1);

            filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");

            this.InsereImagem(LinhaAtual - 0.3, 45.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 76.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 106.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 134.0, filePath, 7, 7);

            this.LinhaAtual += 3;

            this.LinhaSubTopico(LinhaAtual, 14, sLinha2);

            this.InsereImagem(LinhaAtual - 0.3, 27.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 44.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 86.0, filePath, 7, 7);

            this.LinhaAtual += 3;
            
            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto
            
            this.LinhaTopico(this.LinhaAtual, 8, 0, "11", " Declaro, conforme determina a ICVM 553/2014, que concedo a autorização prévia para negociação de cotas de fundos de investimentos", ".");
            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 0, "12", " Declaro, conforme determina a ICVM 553/2014, que recebi e estou ciente dos seguintes documentos:", ".");
            
            this.LinhaAtual += 4;
            
            string S12a = "a. Regulamento      prospecto      ou lâmina      ",
                   S12b = "b. Estou ciente dos riscos envolvidos e da política de investimento;",
                   S12c = "c. Tenho a ciência da possibilidade de ocorrência de patrimônio líquido negativo,",
                   S12d = "d. Declaro que sou responsável por consequentes aportes adicionais de recursos.";

            this.LinhaSubTopico(LinhaAtual, 14, S12a);
            
            //--> CheckBox Regulamento      prospecto      ou lâmina      
            if (entCliente.StCienteDocumentos == 7 || entCliente.StCienteDocumentos == 6 || entCliente.StCienteDocumentos == 4)
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
            }
            else
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
            }

            this.InsereImagem(LinhaAtual - 0.3, 32.0, filePath, 7, 7);

            //--> CheckBox prospecto
            if (entCliente.StCienteDocumentos == 7 || entCliente.StCienteDocumentos == 6 || entCliente.StCienteDocumentos == 2)
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
            }
            else
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
            }

            this.InsereImagem(LinhaAtual - 0.3, 47.5, filePath, 7, 7);

            //--> CheckBox lâmina      
            if (entCliente.StCienteDocumentos == 7 || entCliente.StCienteDocumentos == 3 || entCliente.StCienteDocumentos == 1)
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
            }
            else
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
            }

            this.InsereImagem(LinhaAtual - 0.3, 64.0, filePath, 7, 7);

            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, S12b);
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, S12c);
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 14, S12d);
            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto


            string S11a = "Autorizo a Corretora, caso existam débitos pendentes em meu nome, a liquidar, em Bolsa ou em Câmaras de Compensação e de Liquidação, os contratos, direitos e ativos",
                   S11b = "adquiridos por minha conta em ordem, bem como a executar bens e diretos dados em garantia de minhas operações ou que estejam em poder da Corretora,",
                   S11c = "pagamento dos débitos pendentes, independente de notificação judicial ou extrajudicial;";
            this.LinhaTopico(this.LinhaAtual, 8, 0, "13", S11a, ".");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 15, S11b);
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 15, S11c);
            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S14a = "Mediante este documento, adiro aos termos do contrato de prestação de serviços de Custódia Fungível dos Ativos da CBLC, firmado por esta Corretora, outorgando à CBLC",
                   S14b = "poderes para, na qualidade de proprietário fiduciário, transferir para o seu nome, nas companhias emitentes, os ativos de minha propriedade;";
            this.LinhaTopico(this.LinhaAtual, 8, 0, "14", S14a, ".");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 15, S14b);
            this.LinhaAtual += 3;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S15a = " Estou ciente e concordo que minhas conversas com os representantes da Corretora acerca de quisquer assuntos relativos às minhas operações poderão ser gravadas,",
                   S15b = "podendo, ainda, o conteúdo ser usado como prova no esclarecimento de questões relacionadas à minha conta e às minhas operações nesta Corretora";
            this.LinhaTopico(this.LinhaAtual, 8, 0, "15", S15a, ".");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 15, S15b);
            this.LinhaAtual += 5;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S16a = " São verdadeiras as informações fornecidas para o preenchimento deste cadastro, e me comprometo a informar no prazo de 10 (dez) dias quaisquer alterações que virem a",
                   S16b = "ocorrer nos meus dados cadastrais, apresentando os documentos probatórios;";
            this.LinhaTopico(this.LinhaAtual, 8, 0, "16", S16a, ".");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 15, S16b);

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaAtual += 5;
            this.LinhaTopico(this.LinhaAtual, 8, 0, "17", " Conheço as Normas de Funcionamento do Mercado de Títulos e Valores Mobiliários, bem como os riscos envolvidos nas operações realizadas na Bolsa de Mercadorias", ".");
            this.LinhaAtual += 5;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S18a = " Estou ciente de que a Gradual deverá informar à CVM, conforme a instrução CVM 506, as operações ou movimentações financeiras que configurem ou apresentem indícios",
                   S18b = "de crimes capitulados na Lei 9.613, que dispõe sobre os crimes de \"lavagem de dinheiro\" ou ocultação de bens, direitos e valores;";
            this.LinhaTopico(this.LinhaAtual, 8, 0, "18", S18a, ".");
            this.LinhaAtual += 3;
            this.LinhaSubTopico(LinhaAtual, 15, S18b);

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaAtual += 5;
            this.LinhaTopico(this.LinhaAtual, 8, 0, "19", " Endereço para recebimento de correspondência, emitidas pela Corretora e pelas Bolsas de Valores e/ou Futuros:", ".");
            this.LinhaAtual += 5;

            {   //--> CheckBox Residencial, Comercial e Outro do item 3
                //this.PaginaPDF.AddLT_MM(18, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Residencial"));
                this.PaginaPDF.AddLT_MM(18, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Comercial"));
                this.PaginaPDF.AddLT_MM(48, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Outro, especificar:"));
                this.InsereLinhaContinua(66, this.LinhaAtual, 100, this.FontProp.rSize, false);

                int count = default(int);

                entEndereco = entEndereco.FindAll(cei => cei.StPrincipal); //--> Busca o endereço principal do cliente

                while (entEndereco != null && entEndereco.Count.CompareTo(count).Equals(1))
                {
                    //if (entEndereco[count].IdTipoEndereco == 2) //--> Residencial
                    //{
                    //    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                    //    this.InsereImagem(LinhaAtual - 0.2, 14.40, filePath, 7, 7); //--> CheckBox Residencial

                    //    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                    //    this.InsereImagem(LinhaAtual - 0.2, 44.43, filePath, 7, 7); //--> CheckBox Comercial

                    //    filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                    //    this.InsereImagem(LinhaAtual - 0.2, 74.5, filePath, 7, 7); //--> CheckBox Outro
                    //}
                    if (entEndereco[count].IdTipoEndereco == 1) //--> Comercial
                    {
                        //filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                        //this.InsereImagem(LinhaAtual - 0.2, 14.40, filePath, 7, 7); //--> CheckBox Residencial

                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 14.40, filePath, 7, 7); //--> CheckBox Comercial

                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 44.43, filePath, 7, 7); //--> CheckBox Outro
                    }
                    else if (entEndereco[count].IdTipoEndereco == 3) //--> Outros
                    {
                        //filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                        //this.InsereImagem(LinhaAtual - 0.2, 14.40, filePath, 7, 7); //--> CheckBox Residencial

                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 14.40, filePath, 7, 7); //--> CheckBox Comercial

                        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 44.43, filePath, 7, 7); //--> CheckBox Outro

                        string endereco;

                        if (string.IsNullOrEmpty(entEndereco[count].DsComplemento))
                            endereco = string.Format("{0}, nº{1} {2}-{3} CEP: {4}", entEndereco[count].DsLogradouro, entEndereco[count].DsNumero, entEndereco[count].DsCidade, entEndereco[count].CdUf, entEndereco[count].NrCep);
                        else
                            endereco = string.Format("{0}, nº{1} Complemento: {2} {3}-{4} CEP: {5}", entEndereco[count].DsLogradouro, entEndereco[count].DsNumero, entEndereco[count].DsComplemento, entEndereco[count].DsCidade, entEndereco[count].CdUf, entEndereco[count].NrCep);

                        this.FontProp.rSize -= 1;
                        this.PaginaPDF.AddLT_MM(96.15, LinhaAtual - 0.25, new Root.Reports.RepString(this.FontProp, endereco));
                        this.FontProp.rSize += 2;
                    }
                    count++;
                }
            }

            this.QuebraLinhaDupla();
            tamAnt = this.FontProp.rSize;
            this.FontProp.rSize = 4;
            string v1 = "(Alterações de endereço de correspondência somente serão atendidas quando do recebimento de correspondência formal e cópia do comprovante de endereço atualizado)";
            this.PaginaPDF.AddCB_MM(LinhaAtual, new Root.Reports.RepString(this.FontProp, v1));
            this.FontProp.rSize = tamAnt;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            //this.LinhaAtual += 5;
            //this.LinhaTopico(this.LinhaAtual, 8, 0, "16", " Plano escolhido pelo cliente:", ".");
            //this.LinhaAtual += 5;

            ////this.PaginaPDF.AddLT_MM(18, LinhaAtual, new Root.Reports.RepString(this.FontProp, "GRADUAL DIRECT"));
            //this.PaginaPDF.AddLT_MM(58, LinhaAtual, new Root.Reports.RepString(this.FontProp, "GRADUAL ASSESSORIA"));

            //{   //--> CheckBox Sim e Não do item 3
            //    if (null != entCliente.IdAssessorInicial && entCliente.IdAssessorInicial == 438)
            //    {
            //        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
            //        this.InsereImagem(LinhaAtual - 0.5, 14.75, filePath, 7, 7);
            //        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
            //        this.InsereImagem(LinhaAtual - 0.5, 54.5, filePath, 7, 7);
            //    }
            //    else
            //    {
            //        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
            //        this.InsereImagem(LinhaAtual - 0.5, 14.75, filePath, 7, 7);
            //        filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_full.jpg");
            //        this.InsereImagem(LinhaAtual - 0.5, 54.5, filePath, 7, 7);
            //    }
            //}

            this.QuebraLinhaDupla();

            this.LinhaAtual += 5;
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(90, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, "Assinatura do CLIENTE"));
            this.InsereLinhaContinua(120, this.LinhaAtual, 75, 10, true);
            this.FontProp.bBold = false;
        }

        private void MontaBensImoveis()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(ServerMapPath(pathVirtualImages), "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "INVESTIDOR NÃO RESIDENTE");
                this.LinhaAtual += 5;
            }

            this.TextoValorItem(LinhaAtual, 8, "Representante Legal no País",string.Empty);
            this.TextoValorItem(LinhaAtual, 80, "Representante Corresponsável", string.Empty);
            this.TextoValorItem(LinhaAtual, 150, "País de Origem", string.Empty);

            this.LinhaAtual += 4;

            this.InsereLinhaContinua(8, this.LinhaAtual, 50, 8, false);
            this.InsereLinhaContinua(80, this.LinhaAtual, 50, 8, false);
            this.InsereLinhaContinua(150, this.LinhaAtual, 50, 8, false);

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Representante para Fins Fiscais", "");
            this.TextoValorItem(LinhaAtual, 80, "Custodiante no País", string.Empty);
            this.TextoValorItem(LinhaAtual, 150, "Nº do RDE", string.Empty);

            this.LinhaAtual += 4;

            this.InsereLinhaContinua(8, this.LinhaAtual, 50, 8, false);
            this.InsereLinhaContinua(80, this.LinhaAtual, 50, 8, false);
            this.InsereLinhaContinua(150, this.LinhaAtual, 50, 8, false);

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Administrador de Carteira", string.Empty);
            this.TextoValorItem(LinhaAtual, 150, "Código Operacional CVM", string.Empty);

            this.LinhaAtual += 4;

            this.InsereLinhaContinua(8, this.LinhaAtual, 122, 8, false);
            this.InsereLinhaContinua(150, this.LinhaAtual, 50, 8, false);
            
            
        }

        private void MontaInformacoesRendimentosESituacaoFinanceira()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 10;
                string filePath = string.Concat(ServerMapPath(pathVirtualImages), "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "INFORMAÇÕES ACERCA DOS RENDIMENTOS E DA SITUAÇÃO PATIMONIAL E FINANCEIRA");
                this.LinhaAtual += 5;
            }

            var entSitFinanc = this.GetFichaCadastralInfo.ClienteSituacaoFinanceiraPatrimonialResponse.Resultado;

            if (null != entSitFinanc && entSitFinanc.Count > 0)
            {
                this.TextoValorItem(LinhaAtual, 8, "Capital Social em", entSitFinanc[0].DtCapitalSocial.Value.ToString("dd/MM/yyyy"));
                this.TextoValorItem(LinhaAtual, 40, "Valor em R$", entSitFinanc[0].VTotalCapitalSocial.Value.ToString("N2"));
                this.TextoValorItem(LinhaAtual, 80, "Patrimônio Líquido em", entSitFinanc[0].DtPatrimonioLiquido.Value.ToString("dd/MM/yyyy"));
                this.TextoValorItem(LinhaAtual, 120, "Valor em R$", entSitFinanc[0].VlTotalPatrimonioLiquido.Value.ToString("N2"));
            }
            else
            {
                this.PaginaPDF.AddLT_MM(88, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "[ Não informado ]"));
            }
        }




        private void MontaNomeDasPessoasAutorizadas()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(ServerMapPath(pathVirtualImages), "pixel_black.jpg");
                this.InsereImagem(this.LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "NOME DAS PESSOAS AUTORIZADAS A EMITIR ORDENS");
                this.LinhaAtual += 5;
            }

            var entEmitentes = this.GetFichaCadastralInfo.ClienteEmitenteResponse.Resultado;

            if (null != entEmitentes)
            {
                this.TextoValorItem(LinhaAtual, 8, "Nome", string.Empty);
                this.TextoValorItem(LinhaAtual, 80, "CPF", string.Empty);
                this.TextoValorItem(LinhaAtual, 120, "RG", string.Empty);
                //this.TextoValorItem(LinhaAtual, 150, "Sistema", string.Empty);

                this.LinhaAtual += 2;

                double fontAnt = this.FontProp.rSize;

                this.FontProp.rSize = this.FontProp.rSize - 2;

                entEmitentes.ForEach(delegate(ClienteEmitenteInfo cei)
                {
                    this.LinhaAtual += 2.5;

                    this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, cei.DsNome.ToStringFormatoNome()));
                    this.PaginaPDF.AddLT_MM(80, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, cei.NrCpfCnpj.ToCpfCnpjString()));
                    this.PaginaPDF.AddLT_MM(120, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, cei.DsNumeroDocumento));
                    //this.PaginaPDF.AddLT_MM(150, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "bol".Equals(cei.CdSistema.Trim().ToLower()) ? "Bovespa" : "BM&F"));
                });

                this.FontProp.rSize = fontAnt;
            }
            else
            {
                this.PaginaPDF.AddLT_MM(88, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "[ Não informado ]"));
            }
        }

        private void MontaFontesReferenciaBancaria()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(ServerMapPath(pathVirtualImages), "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "FONTES DE REFERÊNCIA BANCÁRIA");
                this.LinhaAtual += 5;
            }

            var entRefBancarias = this.GetFichaCadastralInfo.ClienteBancoResponse.Resultado;

            if (null != entRefBancarias)
            {
                this.TextoValorItem(LinhaAtual, 8, "Banco", string.Empty);
                this.TextoValorItem(LinhaAtual, 80, "Nº Banco", string.Empty);
                this.TextoValorItem(LinhaAtual, 120, "Agência", string.Empty);
                this.TextoValorItem(LinhaAtual, 150, "Conta", string.Empty);
                

                this.LinhaAtual += 2;

                double fontAnt = this.FontProp.rSize;

                this.FontProp.rSize = this.FontProp.rSize - 2;

                entRefBancarias.ForEach(delegate(ClienteBancoInfo entConta)
                {
                    this.LinhaAtual += 2.5;

                    this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, this.RecuperarNomeBanco(entConta.CdBanco)));

                    this.PaginaPDF.AddLT_MM(80, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, entConta.CdBanco.ToString()));

                    this.PaginaPDF.AddLT_MM(120, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, string.Concat(entConta.DsAgencia, "-", entConta.DsAgenciaDigito)));

                    if (string.IsNullOrEmpty(entConta.DsContaDigito))
                        this.PaginaPDF.AddLT_MM(150, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, entConta.DsConta));
                    else
                        this.PaginaPDF.AddLT_MM(150, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, string.Concat(entConta.DsConta, "-", entConta.DsContaDigito)));

                    //switch (entConta.TpConta)
                    //{
                    //    case "CI":
                    //        this.PaginaPDF.AddLT_MM(150, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Investimento"));
                    //        break;
                    //    case "CC":
                    //        this.PaginaPDF.AddLT_MM(150, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Conta Corrente"));
                    //        break;
                    //    case "CP":
                    //        this.PaginaPDF.AddLT_MM(150, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Poupança"));
                    //        break;
                    //}
                });

                this.FontProp.rSize = fontAnt;
            }
            else
            {
                this.PaginaPDF.AddLT_MM(88, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "[ Não informado ]"));
            }
        }

        private void MontaNomeControladoresAdmProcuradores()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(ServerMapPath(pathVirtualImages), "pixel_black.jpg");
                this.InsereImagem(this.LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "NOME DOS CONTROLADORES, ADMINISTRADORES E PROCURADORES");
                this.LinhaAtual += 5;
            }

            var entProcurador = this.GetFichaCadastralInfo.ClienteProcuradorRepresentanteResponse.Resultado;

            if (null != entProcurador)
            {
                this.TextoValorItem(LinhaAtual, 8, "Nome", string.Empty);
                this.TextoValorItem(LinhaAtual, 80, "CPF", string.Empty);
                this.TextoValorItem(LinhaAtual, 120, "RG", string.Empty);

                this.LinhaAtual += 2;

                double fontAnt = this.FontProp.rSize;

                this.FontProp.rSize = this.FontProp.rSize - 2;

                entProcurador.ForEach(delegate(ClienteProcuradorRepresentanteInfo cpr)
                {
                    this.LinhaAtual += 2.5;

                    this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, cpr.DsNome.ToStringFormatoNome()));
                    this.PaginaPDF.AddLT_MM(80, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, cpr.NrCpfCnpj.ToCpfCnpjString()));
                    this.PaginaPDF.AddLT_MM(120, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, cpr.DsNumeroDocumento));
                });

                this.FontProp.rSize = fontAnt;
            }
            else
            {
                this.PaginaPDF.AddLT_MM(88, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "[ Não informado ]"));
            }
        }

        private void MontaTermoDeIdentificacaoDeProcurador()
        {
            this.LinhaAtual += 7;

            this.InsereLinhaContinua(8, this.LinhaAtual - 4.5, 180, 8, false);

            string filePath = string.Concat(ServerMapPath(pathVirtualImages), "logo_gradual.jpg");
            this.InsereImagem(this.LinhaAtual, 8, filePath, 30, 30);

            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(65, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "TERMO DE IDENTIFICAÇÃO DE PROCURADOR/ADMINISTRADOR/CONTROLADOR"));
            this.FontProp.bBold = false;

            this.LinhaAtual += 5;

            filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
            this.InsereImagem(LinhaAtual - 1, 75, filePath, 7, 7);//--> CheckBox Não do item 2
            this.PaginaPDF.AddLT_MM(80, this.LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, "Procurador"));

            filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
            this.InsereImagem(LinhaAtual - 1, 100, filePath, 7, 7);//--> CheckBox Não do item 2
            this.PaginaPDF.AddLT_MM(105, this.LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, "Administrador"));

            filePath = string.Concat(ServerMapPath(pathVirtualImagesCadastro), "checbox_none.jpg");
            this.InsereImagem(LinhaAtual - 1, 125, filePath, 7, 7);//--> CheckBox Não do item 2
            this.PaginaPDF.AddLT_MM(130, this.LinhaAtual - 1, new Root.Reports.RepString(this.FontProp, "Controlador"));

            this.LinhaAtual += 3;

            this.FontProp.rSize -= 2;

            this.PaginaPDF.AddLT_MM(95, this.LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, "(Documento anexo à ficha cadastral do cliente)"));

            this.FontProp.rSize += 2;

            this.InsereLinhaContinua(8, this.LinhaAtual, 180, 8, false);

            this.QuebraLinhaDupla();

            this.TextoValorItem(this.LinhaAtual, 8, "Nome Completo:", string.Empty);
            this.InsereLinhaContinua(29, this.LinhaAtual, 159, 8, false);

            this.QuebraLinhaDupla();

            this.TextoValorItem(this.LinhaAtual, 8, "Endereço:", string.Empty);
            this.InsereLinhaContinua(21, this.LinhaAtual, 91, 8, false);

            this.TextoValorItem(this.LinhaAtual, 115, "Número:", string.Empty);
            this.InsereLinhaContinua(126, this.LinhaAtual, 17, 8, false);

            this.TextoValorItem(this.LinhaAtual, 145, "Complemento:", string.Empty);
            this.InsereLinhaContinua(163, this.LinhaAtual, 25, 8, false);

            this.QuebraLinhaDupla();

            this.TextoValorItem(this.LinhaAtual, 8, "Bairro:", string.Empty);
            this.InsereLinhaContinua(17, this.LinhaAtual, 45, 8, false);

            this.TextoValorItem(this.LinhaAtual, 66, "Cidade:", string.Empty);
            this.InsereLinhaContinua(76, this.LinhaAtual, 40, 8, false);

            this.TextoValorItem(this.LinhaAtual, 119, "UF:", string.Empty);
            this.InsereLinhaContinua(124, this.LinhaAtual, 7, 8, false);

            this.TextoValorItem(this.LinhaAtual, 133, "País:", string.Empty);
            this.InsereLinhaContinua(141, this.LinhaAtual, 21, 8, false);

            this.TextoValorItem(this.LinhaAtual, 165, "CEP:", string.Empty);
            this.InsereLinhaContinua(172, this.LinhaAtual, 16, 8, false);

            this.QuebraLinhaDupla();

            this.TextoValorItem(this.LinhaAtual, 8, "Profissão:", string.Empty);
            this.InsereLinhaContinua(21, this.LinhaAtual, 27, 8, false);

            this.TextoValorItem(this.LinhaAtual, 51, "Telefone (DDD + número):", string.Empty);
            this.InsereLinhaContinua(82, this.LinhaAtual, 15, 8, false);

            this.TextoValorItem(this.LinhaAtual, 100, "Fax (DDD + número):", string.Empty);
            this.InsereLinhaContinua(125, this.LinhaAtual, 15, 8, false);

            this.TextoValorItem(this.LinhaAtual, 143, "Data de Nascimento:", string.Empty);
            this.InsereLinhaContinua(168, this.LinhaAtual, 20, 8, false);

            this.QuebraLinhaDupla();

            this.TextoValorItem(this.LinhaAtual, 8, "CPF nº:", string.Empty);
            this.InsereLinhaContinua(18, this.LinhaAtual, 23, 8, false);

            this.TextoValorItem(this.LinhaAtual, 44, "Nº Documento Identidade:", string.Empty);
            this.InsereLinhaContinua(76, this.LinhaAtual, 23, 8, false);

            this.TextoValorItem(this.LinhaAtual, 101, "Email:", string.Empty);
            this.InsereLinhaContinua(110, this.LinhaAtual, 78, 8, false);

            this.QuebraLinhaDupla();
            this.QuebraLinhaDupla();

            this.InsereLinhaContinua(8, this.LinhaAtual, 95, 8, false);
            this.InsereLinhaContinua(110, this.LinhaAtual, 78, 8, false);

            this.LinhaAtual += 6;

            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(25, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Assinatura do Procurador / Administrador / Colaborador"));
            this.PaginaPDF.AddLT_MM(140, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Local e Data"));
            this.FontProp.bBold = false;

            this.LinhaAtual += 5;

            this.FontProp.rSize -= 2;
            this.PaginaPDF.AddLT_MM(15, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "(*) Procuração Anexa"));
            this.FontProp.rSize += 2;
        }

        private void MontaComoConheceu()
        {
            var entCliente = this.GetFichaCadastralInfo.ClienteResponse.Objeto;

            this.LinhaAtual += 5;
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Como conheceu a Gradual?"));
            this.PaginaPDF.AddLT_MM(50, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, entCliente.DsComoConheceu));
            this.FontProp.bBold = false;
            this.LinhaAtual += 5;
        }

        /// <summary>
        /// Gera informações da declaração responsável (CÂMBIO)
        /// </summary>
        private void MontaDeclaracaoResponsavelNaCorretora_Cambio()
        {
            //{   //--> Definindo a barra de título
            //    this.LinhaAtual += 15;
            //    string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");
            //    this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
            //    this.MontaBarraTitulo(this.LinhaAtual, "DECLARAÇÃO DO RESPONSÁVEL NA CORRETORA PELO CADASTRAMENTO DO CLIENTE");
            //    this.LinhaAtual += 3;
            //}

            this.LinhaAtual += 15;
            double fontAnt = this.FontProp.rSize;
            this.FontProp.rSize = 4;
            this.FontProp.bBold = true;

            string texto0 = "Declaro para fins de comprovação de endereço, conforme Lei n.º 7.115 de 29/08/1983, que meu domicílio é o informado neste documento  e que as demais informações constantes",
                   texto1 = "nesta Ficha Cadastral são verídicas e por elas assumo a responsabilidade na forma da legislação vigente.";

            this.PaginaPDF.AddCB_MM(LinhaAtual, new Root.Reports.RepString(this.FontProp, texto0));
            this.LinhaAtual += 4;
            this.PaginaPDF.AddCB_MM(LinhaAtual, new Root.Reports.RepString(this.FontProp, texto1));

            this.LinhaAtual += 6;

            this.FontProp.bBold = false;

            string texto2 = "(Alterações de endereço de correspondência somente serão atendidas quando do recebimento de correspondência formal e cópia de comprovante de endereço atualizado)";
            this.PaginaPDF.AddCB_MM(LinhaAtual, new Root.Reports.RepString(this.FontProp, texto2));

            this.LinhaAtual += 15;

            this.InsereLinhaContinua(10, this.LinhaAtual, 80, 10, true);

            this.FontProp.bBold = true;


            this.PaginaPDF.AddLT_MM(40, LinhaAtual + 6, new Root.Reports.RepString(this.FontProp, "Assinatura do Cliente"));
            this.FontProp.bBold = false;

            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(100, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, "Local e Data:"));
            this.FontProp.bBold = false;

            this.InsereLinhaContinua(115, this.LinhaAtual, 80, 10, true);

            this.LinhaAtual += 5;
        }

        #endregion

        #region | Controles de texto

        private void MontaCabecalho()
        {
            string filePath = string.Concat(ServerMapPath(pathVirtualImages), "logo_gradual.jpg");
            this.InsereImagem(4, 96, filePath, 50, 50);
        }

        private void TextoValorItem(double linha, double coluna, string Campo, string valor)
        {
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(coluna, linha, new Root.Reports.RepString(this.FontProp, Campo));
            this.FontProp.bBold = false;
            double fontAnt = this.FontProp.rSize;
            this.FontProp.rSize = this.FontProp.rSize - 2;
            this.PaginaPDF.AddLT_MM(coluna, linha + 4, new Root.Reports.RepString(this.FontProp, valor));
            this.FontProp.rSize = fontAnt;
        }

        private void TextoTabuladoEsquerda(double linha, double coluna, string texto)
        {
            this.PaginaPDF.AddLT_MM(coluna, linha, new Root.Reports.RepString(this.FontProp, texto));
        }

        private void TextoTabuladoEsquerda(double linha, double coluna, string texto, double tamanhoTexto, bool negrito)
        {
            double tamAnt = this.FontProp.rSize;
            this.FontProp.rSize = tamanhoTexto;
            this.FontProp.bBold = negrito;

            this.PaginaPDF.AddLT_MM(coluna, linha, new Root.Reports.RepString(this.FontProp, texto));

            this.FontProp.bBold = false;
            this.FontProp.rSize = tamAnt;
        }

        private void TextoTabuladoEsquerda(double linha, double coluna, string texto, double tamanhoTexto, bool negrito, bool sublinhado, System.Drawing.Color corTexto)
        {
            System.Drawing.Color corAnt = this.FontProp.color;
            double tamAnt = this.FontProp.rSize;
            this.FontProp.rSize = tamanhoTexto;
            this.FontProp.bBold = negrito;
            this.FontProp.color = corTexto;

            this.PaginaPDF.AddLT_MM(coluna, linha, new Root.Reports.RepString(this.FontProp, texto));

            this.FontProp.bBold = false;
            this.FontProp.rSize = tamAnt;
            this.FontProp.color = corAnt;
        }

        private void TextoCentralizado(double linha, string texto, double tamanhoTexto, bool negrito)
        {
            double tamAnt = this.FontProp.rSize;
            this.FontProp.rSize = tamanhoTexto;
            this.FontProp.bBold = negrito;
            this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(this.FontProp, texto));
            this.FontProp.bBold = false;
            this.FontProp.rSize = tamAnt;
        }

        private void LinhaTopico(double linha, int coluna, int LeftPad, string topicoId, string texto, string caractereSeparador)
        {
            int espaco = topicoId.Length + 2 + LeftPad;
            this.FontProp.rSize = 5;
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(coluna + LeftPad, linha, new Root.Reports.RepString(this.FontProp, string.Concat(topicoId, caractereSeparador)));
            this.FontProp.bBold = false;
            this.PaginaPDF.AddLT_MM(coluna + espaco, linha, new Root.Reports.RepString(this.FontProp, texto));
        }

        private void LinhaSubTopico(double linha, int coluna, string texto)
        {
            double tamAnt = this.FontProp.rSize;

            this.FontProp.rSize = 5;
            this.PaginaPDF.AddLT_MM(coluna, linha, new Root.Reports.RepString(this.FontProp, texto));

            this.FontProp.rSize = tamAnt;
        }

        private void InsereLinhaContinua(double posicao, double linha, double comprimento, double tamanhoFonte, bool negrito)
        {
            double tamAnt = this.FontProp.rSize;
            this.FontProp.rSize = tamanhoFonte;
            this.FontProp.bBold = negrito;
            for (int und = 0; und < comprimento; und++)
                this.PaginaPDF.AddLT_MM(posicao + und, linha, new Root.Reports.RepString(this.FontProp, "_"));
            this.FontProp.bBold = false;
            this.FontProp.rSize = tamAnt;
        }

        private void InsereImagem(double linha, double coluna, string caminhoArquivo, double largura, double altura)
        {
            System.IO.FileStream fileStream = System.IO.File.Open(caminhoArquivo, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

            this.PaginaPDF.AddLT_MM(coluna, linha, new Root.Reports.RepImage(fileStream, largura, altura));
        }

        #endregion

        #region | Tabulacao

        private void IsFinalPagina(double padding)
        {
            if (this.LinhaAtual.CompareTo(210).Equals(1))
                this.NovaPagina(padding);
        }

        /// <summary>
        /// Nova linha com espaçamento de 5px
        /// </summary>
        private void QuebraLinha()
        {
            this.LinhaAtual += 5;
        }

        /// <summary>
        /// Nova linha com espaçamento variável
        /// </summary>
        /// <param name="espacamento">Espaçamento em pixel</param>
        private void QuebraLinha(double espacamento)
        {
            this.LinhaAtual += espacamento;
        }

        /// <summary>
        /// Nova linha com espaçamento de 10px
        /// </summary>
        private void QuebraLinhaDupla()
        {
            this.LinhaAtual += 8;
        }

        private void NovaPagina(double padding)
        {
            this.PaginaPDF = new Root.Reports.Page(this.ReportCadastro);
            this.MontaCabecalho();
            this.LinhaAtual = 17 + padding;
            this.MontaRodape();
        }

        #endregion

        #region | Métodos de apoio

        private string ServerMapPath(string objectPath)
        {
            //return string.Concat(this._ServerMapPath, "\\", objectPath);
            return objectPath;
        }

        private List<ClienteTelefoneInfo> RecuperarTelefone(int? pIdCliente)
        {
            if (null == pIdCliente)
                return new List<ClienteTelefoneInfo>();

            var lRetorno = new ClienteTelefoneDbLib().ConsultarClienteTelefone(
                new ConsultarEntidadeRequest<ClienteTelefoneInfo>()
                {
                    Objeto = new ClienteTelefoneInfo()
                    {
                        IdCliente = pIdCliente.Value,
                    }
                });

            return lRetorno.Resultado;
        }

        private string RecuperarDadosDeEstadoCivil(int? pCdEstadoCivil)
        {
            if (null == pCdEstadoCivil)
                return string.Empty;

            var lListaEstadoCivil = new SinacorDbLib().ConsultarListaSinacor(
                new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.EstadoCivil
                    }
                });

            var lRetorno = lListaEstadoCivil.Resultado.Find(delegate(SinacorListaInfo sli) { return sli.Id == pCdEstadoCivil.DBToString(); });

            if (lRetorno != null)
                return lRetorno.Value;

            return null;
        }

        private string RecuperarNomeBanco(string pCdBanco)
        {
            if (string.IsNullOrEmpty(pCdBanco))
                return string.Empty;

            var lListaBanco = new SinacorDbLib().ConsultarListaSinacor(
                new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.Banco
                    }
                });

            var lRetorno = lListaBanco.Resultado.Find(delegate(SinacorListaInfo sli) { return sli.Id == pCdBanco.DBToString(); });

            if (null != lRetorno && !string.IsNullOrWhiteSpace(lRetorno.Value))
                return lRetorno.Value;

            return "Banco não informado";
        }

        private string RecuperarDadosDeNacionalidade(int? pCdNacionalidade)
        {
            if (null == pCdNacionalidade)
                return string.Empty;

            var lListaNacionalidade = new SinacorDbLib().ConsultarListaSinacor(
                new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.Nacionalidade
                    }
                });

            var lRetorno = lListaNacionalidade.Resultado.Find(delegate(SinacorListaInfo sli) { return sli.Id == pCdNacionalidade.DBToString(); });

            if (lRetorno != null)
                return lRetorno.Value;

            return string.Empty;
        }

        private string RecuperarPais(string pCdPais)
        {
            if (null == pCdPais)
                return string.Empty;

            var lListaPaises = new SinacorDbLib().ConsultarListaSinacor(
                new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.Pais
                    }
                });

            var lRetorno = lListaPaises.Resultado.Find(delegate(SinacorListaInfo sli) { return sli.Id == pCdPais.DBToString(); });

            if (lRetorno != null)
                return lRetorno.Value;

            return string.Empty;
        }

        private string RecuperarSituacaoLegal(int? pTpSituacaoLegal)
        {
            if (null == pTpSituacaoLegal)
                return string.Empty;

            var lListaSituacaoLegal = new SinacorDbLib().ConsultarListaSinacor(
                new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.Pais
                    }
                });

            var lRetorno = lListaSituacaoLegal.Resultado.Find(delegate(SinacorListaInfo sli) { return sli.Id == pTpSituacaoLegal.DBToString(); });

            if (lRetorno != null)
                return lRetorno.Value;

            return string.Empty;
        }

        private string RecuperarProfissao(int? pCdProfissao)
        {
            if (null == pCdProfissao)
                return string.Empty;

            var lListaProfissoes = new SinacorDbLib().ConsultarListaSinacor(
                new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.AtividadePFePJ
                    }
                });

            var lRetorno = lListaProfissoes.Resultado.Find(delegate(SinacorListaInfo sli) { return sli.Id == pCdProfissao.DBToString(); });

            if (lRetorno != null)
                return lRetorno.Value;

            return string.Empty;
        }
        
        private string RecuperarDadosDoAssessor(int? pIdAssessor)
        {
            if (null == pIdAssessor)
                return string.Empty;

            var lListaAssesores = new SinacorDbLib().ConsultarListaSinacor(
                new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.Assessor
                    }
                });

            var lRetorno = lListaAssesores.Resultado.Find(delegate(SinacorListaInfo sli) { return sli.Id == pIdAssessor.DBToString(); });

            if (lRetorno != null)
                return string.Concat(pIdAssessor.DBToString(), " - ", lRetorno.Value);

            return string.Empty;
        }

        #endregion
    }
}
