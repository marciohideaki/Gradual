using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Dados;
using Gradual.Servico.FichaCadastral.Lib;
using log4net;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace Gradual.Servico.FichaCadastral
{
    public class FichaCadastral_PF
    {
        #region | Atributos

        private static bool lEhCambio = false;

        private int _IdCliente;

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private double LinhaAtual = 20;
        private Root.Reports.Page PaginaPDF;
        private Root.Reports.FontProp FontProp;
        private Root.Reports.Report ReportCadastro;
        private Root.Reports.FontDef FontDefin;

        //TODO: colocar os parametros no arquivo de configuracoes
        private string pathVirtualPDF = "../ReportPDF";
        private string pathVirtualImages = "../../Images/";
        private string pathVirtualImagesCadastro = "../../images/cadastro/";

        private string fileNameFichaCadastral;
        private string fileNameFichaCadastralCambio;

        private FichaCadastralInfo gFichaCadastralInfo = null;

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

        private int gPagina = 1;

        /// <summary>
        /// Retorna o Id do Login
        /// </summary>
        //private int GetIdLogin
        //{
        //    get;
        //    set;
        //}

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
                    var lFichaCadastral = FichaCadastralDbLib.ReceberFichaCadastral(new ReceberEntidadeRequest<Gradual.Intranet.Contratos.Dados.FichaCadastralInfo>()
                    {
                        Objeto = new FichaCadastralInfo() { IdCliente = this.GetIdCliente, }
                    });

                    if (null != lFichaCadastral)
                    {
                        this.gFichaCadastralInfo = lFichaCadastral.Objeto;

                        if (lFichaCadastral.Objeto.ClienteResponse.Objeto.TpDesejaAplicar.Equals("CAMBIO") || lFichaCadastral.Objeto.ClienteResponse.Objeto.TpDesejaAplicar.Equals("BOV/CAM") || lFichaCadastral.Objeto.ClienteResponse.Objeto.TpDesejaAplicar.Equals("FUN/CAM") || lFichaCadastral.Objeto.ClienteResponse.Objeto.TpDesejaAplicar.ToUpper().Equals("TODAS") || lFichaCadastral.Objeto.ClienteResponse.Objeto.TpDesejaAplicar.ToUpper().Equals("TODOS"))
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

        public SistemaOrigem SitemaOrigem { get; set; }
        
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

        #region | Construtores

        public FichaCadastral_PF() { }

        #endregion

        #region | Métodos Servico

        public ReceberObjetoResponse<FichaCadastralGradualInfo> GerarFichaCadastral_PF(ReceberEntidadeRequest<FichaCadastralGradualInfo> pParametro)
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

        public DocumentResponse GerarFichaCadastral_PF_Bytes(ReceberEntidadeRequest<FichaCadastralGradualInfo> pParametro, TipoFicha pTipoFicha)
        {
            try
            {
                var ficha = new FichaCadastral_PF().GerarFichaCadastral_PF(pParametro);

                string fileName = String.Empty;
                byte[] pdfBytes = new byte[0];

                if (pTipoFicha == TipoFicha.FichaCadastral)
                {
                    fileName = Path.GetFileName(ficha.Objeto.PathFichaCadastral);
                    if (!fileName.ToUpper().Contains("FICHA"))
                        fileName = "FichaCadastral_" + pParametro.Objeto.IdCliente + ".pdf";

                    pdfBytes = File.ReadAllBytes(this.GetEnderecoArquivoPortal + "\\" + ficha.Objeto.PathFichaCadastral);
                }
                else
                {
                    fileName = Path.GetFileName(ficha.Objeto.PathFichaCadastralCambio);
                    if (!fileName.ToUpper().Contains("FICHA"))
                        fileName = "FichaCadastralCambio_" + pParametro.Objeto.IdCliente + ".pdf";

                    pdfBytes = File.ReadAllBytes(this.GetEnderecoArquivoPortal + "\\" + ficha.Objeto.PathFichaCadastralCambio);
                }

                File.Delete(this.GetEnderecoArquivoPortal + "\\" + ficha.Objeto.PathFichaCadastral);
                File.Delete(this.GetEnderecoArquivoPortal + "\\" + ficha.Objeto.PathFichaCadastralCambio);

                DocumentResponse docResp = new DocumentResponse()
                {
                    FileName = fileName,
                    PdfBase64 = Convert.ToBase64String(pdfBytes),
                    StatusGeracao = "SUCESSO",
                    MensagemGeracao = ""
                };

                return docResp;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex.ToString(), ex);

                DocumentResponse docResp = new DocumentResponse()
                {
                    FileName = "",
                    PdfBase64 = "",
                    StatusGeracao = "ERRO",
                    MensagemGeracao = "Erro ao gerar documento: " + ex.Message
                };

                return docResp;
            }
        }

        #endregion

        #region | TriggersRelatorios

        private void CriaFichaDuc()
        {
            this.LinhaAtual = 25;
            this.MontaFichaCadastralCliente();
            this.IsFinalPagina(0);
            this.MontaInformacoesComerciais();
            this.IsFinalPagina(0);
            this.MontaFontesReferenciaBancaria();
            this.IsFinalPagina(0);
            this.MontaDadosResponsavel();
            this.IsFinalPagina(0);
            this.MontaInformacoesPatrimoniais();
            this.IsFinalPagina(0);
            //this.MontaBensImoveis();
            //this.IsFinalPagina(0);
            this.MontaOutrosBensDireitos();
            this.IsFinalPagina(0);
            this.DeclaracoesAutorizacoesCliente();
            this.IsFinalPagina(0);
            this.NovaPagina(0);
            this.MontaDeclaracaoResponsavelNaCorretora();
        }

        private void CriaFichaCambio()
        {
            this.LinhaAtual = 25;
            this.MontaFichaCadastralCliente(true);
            this.IsFinalPagina(0);
            this.MontaInformacoesComerciais();
            this.IsFinalPagina(0);
            this.MontaFontesReferenciaBancaria();
            this.IsFinalPagina(0);
            this.MontaDadosResponsavel();
            this.IsFinalPagina(0);
            this.MontaInformacoesPatrimoniais(true);
            this.IsFinalPagina(0);
            this.MontaComoConheceu();
            this.IsFinalPagina(0);
            this.MontaDeclaracaoResponsavelNaCorretora_Cambio();
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

            #region Ficha Cadastral
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

            #endregion

            #region Ficha Cadastral Câmbio
            if (lEhCambio)
            {
                g = Guid.NewGuid();

                this.ReportCadastro = new Root.Reports.Report(new Root.Reports.PdfFormatter());
                this.FontDefin = new Root.Reports.FontDef(this.ReportCadastro, Root.Reports.FontDef.StandardFont.Helvetica);
                this.FontProp = new Root.Reports.FontProp(this.FontDefin, 6);
                this.NovaPagina(0);
                this.CriaFichaCambio();
                //this.fileNameFichaCadastralCambio = string.Format("FichaCadastralCambio-{0}.pdf", this.GetIdCliente.ToString());
                this.fileNameFichaCadastralCambio = string.Format("{0}", g.ToString());

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

            if (gPagina == 1)
            {
                gPagina = gPagina + 1;  //na primeira página ignora o rodapé
            }
            else
            {
                Root.Reports.FontProp fontProp = new Root.Reports.FontProp(this.FontDefin, 5);
                fontProp.bBold = true;

                this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, "GRADUAL CCTVM S/A"));
                fontProp.bBold = false;
                string tx = "Av. Presidente Juscelino Kubitschek, 1909, 19° andar, conj. 191 B, Torre Norte - Vila Nova Conceição São Paulo – SP - 04543-904";
                linha = linha + 3;
                this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, tx));
                tx = "Tel (11)3372-8300 - FAX (11)3372-8301  |  www.gradualinvestimentos.com.br";
                linha = linha + 3;
                this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, tx));
                fontProp.bUnderline = false;
                linha = linha + 3;
                this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, "Ouvidoria/SAC: 0800 655 1466"));
                fontProp.bUnderline = false;
            }
        }

        /// <summary>
        /// Gera informações da declaração responsável
        /// </summary>
        private void MontaDeclaracaoResponsavelNaCorretora()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "DECLARAÇÃO DO RESPONSÁVEL NA CORRETORA PELO CADASTRAMENTO DO CLIENTE");
                this.LinhaAtual += 3;
            }

            string texto0 = "Responsabilizo-me pela exatidão das informações prestadas, a vista dos originais dos documentos de Identidade, CPF/CNPJ, e outros comprobató-",
                   texto1 = "rios dos demais elementos de informação apresentados, sob pena de aplicação do disposto no artigo 64 da Lei nº 8383 de 30 de dezembro de 1991.";

            double fontAnt = this.FontProp.rSize;

            this.FontProp.rSize = 6;

            this.PaginaPDF.AddLT_MM(8, LinhaAtual, new Root.Reports.RepString(this.FontProp, texto0));
            this.LinhaAtual += 4;
            this.PaginaPDF.AddLT_MM(8, LinhaAtual, new Root.Reports.RepString(this.FontProp, texto1));
            
            this.FontProp.rSize = fontAnt;

            this.LinhaAtual += 10;

            this.InsereLinhaContinua(20, this.LinhaAtual, 80, 10, true);
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(20, LinhaAtual + 6, new Root.Reports.RepString(this.FontProp, "Assinatura do RESPONSÁVEL NA GRADUAL CCTVM S/A"));
            this.FontProp.bBold = false;

            this.InsereLinhaContinua(115, this.LinhaAtual, 80, 10, true);
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(135, LinhaAtual + 6, new Root.Reports.RepString(this.FontProp, "Assinatura do ASSESSOR"));
            this.FontProp.bBold = false;

            this.QuebraLinhaDupla();
            this.QuebraLinhaDupla();

            this.FontProp.bBold = true;

            this.PaginaPDF.AddLT_MM(8, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, "Obs.: As fichas cadastrais de clientes devem ter em anexo cópias:"));
            
            this.FontProp.rSize = 6;
            this.FontProp.bBold = false;
            this.LinhaAtual += 4;
            this.PaginaPDF.AddLT_MM(8, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, "- Doc. de Identidade, do CPF e comprovante de endereço atualizado."));
            
            this.LinhaAtual += 20;  //para dar mais espaço pras assianturas

            this.FontProp.rSize = 7;

            this.TextoValorItem(LinhaAtual + 2, 115, "Local e data", string.Empty);
            this.InsereLinhaContinua(135, this.LinhaAtual, 60, 10, true);
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

        private void MontaFichaCadastralCliente()
        {
            MontaFichaCadastralCliente(false);
        }
        
        private void MontaFichaCadastralCliente(bool CabecalhoCambio)
        {
            var entCliente = this.GetFichaCadastralInfo.ClienteResponse.Objeto;
            var entLogin = this.GetFichaCadastralInfo.ClienteLoginResponse.Objeto;
            var entConta = this.GetFichaCadastralInfo.ClienteContaResponse.Resultado.Find(delegate(ClienteContaInfo cci) { return eAtividade.BOL.Equals(cci.CdSistema) || eAtividade.BMF.Equals(cci.CdSistema); });
            var entEndereco = this.GetFichaCadastralInfo.ClienteEnderecoResponse.Resultado.Find(delegate(ClienteEnderecoInfo cei) { return cei.IdTipoEndereco == 2; });
            var entTelefoneRes = this.GetFichaCadastralInfo.ClienteTelefoneReponse.Resultado.Find(delegate(ClienteTelefoneInfo cti) { return cti.IdTipoTelefone == 1; });
            var entTelefoneCel = this.GetFichaCadastralInfo.ClienteTelefoneReponse.Resultado.Find(delegate(ClienteTelefoneInfo cti) { return cti.IdTipoTelefone == 3; });

            //--> Definindo a barra de título
            this.LinhaAtual += 5;
            string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");
            this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);

            string lTitulo = String.Empty;
            if (CabecalhoCambio)
            {
                lTitulo = "FICHA CADASTRAL DE CLIENTE - CÂMBIO (PESSOA FÍSICA)";
            }
            else
            {
                lTitulo = "FICHA CADASTRAL DE CLIENTE (PESSOA FÍSICA)";
            }

            this.MontaBarraTitulo(LinhaAtual, lTitulo);
            this.LinhaAtual += 20;

            this.LinhaAtual = this.LinhaAtual - 15;

            this.TextoValorItem(LinhaAtual, 8, "Nome Completo", entCliente.DsNome);

            this.TextoValorItem(LinhaAtual, 158, "Assessor", entCliente.IdAssessorInicial.DBToString());

            this.TextoValorItem(LinhaAtual, 185, "Cód. Cliente", null != entConta ? entConta.CdCodigo.ToCodigoClienteFormatado() : string.Empty);

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Data de Nasc.", (null != entCliente.DtNascimentoFundacao) ? Convert.ToDateTime(entCliente.DtNascimentoFundacao.Value, new System.Globalization.CultureInfo("pt-BR")).ToString("dd/MM/yyyy") : string.Empty);

            this.TextoValorItem(LinhaAtual, 45, "Nacionalidade", this.RecuperarDadosDeNacionalidade(entCliente.CdNacionalidade));

            this.TextoValorItem(LinhaAtual, 80, "Local de Nascimento", entCliente.DsNaturalidade);

            this.TextoValorItem(LinhaAtual, 120, "UF", entCliente.CdUfNascimento);

            this.TextoValorItem(LinhaAtual, 130, "Estado Civil", this.RecuperarDadosDeEstadoCivil(entCliente.CdEstadoCivil));

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "CPF", entCliente.DsCpfCnpj.ToCpfCnpjString());

            this.TextoValorItem(LinhaAtual, 35, "Nº documento de identidade", entCliente.DsNumeroDocumento);

            this.TextoValorItem(LinhaAtual, 80, "Data de emissão", (null != entCliente.DtEmissaoDocumento || DateTime.MinValue.Equals(entCliente.DtEmissaoDocumento)) ?  Convert.ToDateTime(entCliente.DtEmissaoDocumento.Value, new System.Globalization.CultureInfo("pt-BR")).ToString("dd/MM/yyyy") : string.Empty);

            this.TextoValorItem(LinhaAtual, 130, "Órgão emissor", entCliente.CdOrgaoEmissorDocumento);

            this.TextoValorItem(LinhaAtual, 158, "UF Emissor", entCliente.CdUfEmissaoDocumento);

            this.TextoValorItem(LinhaAtual, 185, "Sexo", (null != entCliente.CdSexo) ? entCliente.CdSexo.DBToString() : string.Empty);

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Nome do(a) Cônjuge/Companheiro(a)", entCliente.DsConjugue);

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Filiação (Nomes dos Pais, Tutor ou Curador)", string.Format("{0} / {1}", entCliente.DsNomeMae, entCliente.DsNomePai));

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Endereço residencial (Logradouro)", (null != entEndereco) ? entEndereco.DsLogradouro : string.Empty);

            this.TextoValorItem(LinhaAtual, 130, "Número", (null != entEndereco) ? entEndereco.DsNumero : string.Empty);

            this.TextoValorItem(LinhaAtual, 158, "Complemento", (null != entEndereco) ? entEndereco.DsComplemento : string.Empty);

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Bairro", (null != entEndereco) ? entEndereco.DsBairro : string.Empty);

            this.TextoValorItem(LinhaAtual, 60, "CEP", (null != entEndereco) ? string.Format("{0}-{1}", entEndereco.NrCep.DBToString(), entEndereco.NrCepExt.DBToString().PadLeft(3, '0')) : string.Empty);

            this.TextoValorItem(LinhaAtual, 80, "Cidade", (null != entEndereco) ? entEndereco.DsCidade : string.Empty);

            this.TextoValorItem(LinhaAtual, 130, "Estado", (null != entEndereco) ? entEndereco.CdUf : string.Empty);

            this.TextoValorItem(LinhaAtual, 158, "País", (null != entEndereco) ? entEndereco.CdPais : string.Empty);

            this.QuebraLinhaDupla();

            this.TextoValorItem(LinhaAtual, 8, "Telefone (DDD + número)", (null != entTelefoneRes) ? string.Concat(entTelefoneRes.DsDdd, "-", entTelefoneRes.DsNumero.ToTelefoneString()) : string.Empty);

            this.TextoValorItem(LinhaAtual, 60, "Celular (DDD + número)", (null != entTelefoneCel) ? string.Concat(entTelefoneCel.DsDdd, "-", entTelefoneCel.DsNumero.ToTelefoneString()) : string.Empty);

            this.TextoValorItem(LinhaAtual, 130, "E-Mail (endereço eletrônico)", entLogin.DsEmail);
        }

        /// <summary>
        /// Método para montar outros Bens Direitos
        /// </summary>
        private void MontaOutrosBensDireitos()
        {
            string xii = null;

            Root.Reports.FontProp fontPropPeq = new Root.Reports.FontProp(this.FontDefin, 5);
            fontPropPeq.bBold = false;

            var entCliente = this.GetFichaCadastralInfo.ClienteSituacaoFinanceiraPatrimonialResponse.Resultado;
            var lcsfp = entCliente.Count > 0 ? entCliente[0] : new ClienteSituacaoFinanceiraPatrimonialInfo();
            
            CultureInfo lInfo = new CultureInfo("pt-BR");

            decimal lValor = 0;

            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "FICHA DE CLIENTE (PESSOA FÍSICA)");
                this.PaginaPDF.AddLT_MM(220, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Informções patrimoniais"));
                this.LinhaAtual += 7;
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "OUTROS BENS E DIREITOS");
                this.LinhaAtual += 5;
            }
            
            if(lcsfp.VlTotalBensMoveis.HasValue)
                lValor = lcsfp.VlTotalBensMoveis.Value;

            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Total de Bens Móveis"));
            this.PaginaPDF.AddLT_MM(90, this.LinhaAtual, new Root.Reports.RepString(fontPropPeq, "Considerar o total de Bens Móveis que possua o registro de propriedade: Carro, moto, navio, etc."));
            this.FontProp.bBold = false;
            this.PaginaPDF.AddLT_MM(43, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, lValor.ToString("C2", lInfo)));
            this.LinhaAtual += 5;

            lValor = 0;

            if(lcsfp.VlTotalBensImoveis.HasValue)
                lValor = lcsfp.VlTotalBensImoveis.Value;
            
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Total de Bens Imóveis"));
            this.PaginaPDF.AddLT_MM(90, this.LinhaAtual, new Root.Reports.RepString(fontPropPeq, "Considerar o total de Bens Imóveis que possua o registro de propriedade: Casa, Apto, Terreno, etc."));
            this.FontProp.bBold = false;
            this.PaginaPDF.AddLT_MM(43, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, lValor.ToString("C2", lInfo)));

        }

        private void DeclaracoesAutorizacoesCliente()
        {
            this.LinhaAtual += 10;

            double padding = 10;

            var entCliente = this.GetFichaCadastralInfo.ClienteResponse.Objeto;
            var entEndereco = this.GetFichaCadastralInfo.ClienteEnderecoResponse.Resultado;
            var entRepresentante = this.GetFichaCadastralInfo.ClienteProcuradorRepresentanteResponse.Resultado;
            var entClienteNaoOpera = this.GetFichaCadastralInfo.ClienteNaoOperaPorContaPropriaResponse.Objeto;

            this.FontProp.rSize = 5;

            string xii = "(As informações acima são obrigatórias, decorrentes da Lei nº 9.613, da Circular nº 2.852 e da Carta Circular nº 2826 do Banco Central do Brasil e da Instrução nº 505 da",
                   xiii = "Comissão de valores mobiliários e serão mantidos confidencialmente. Declaro, na forma da lei, que são verdadeiras as informações abaixo descritas, estando ciente de que",
                   xix = "será usado para fins de atualização patrimonial e limite operacional).";
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, xii));
            this.LinhaAtual += 4;
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, xiii));
            this.LinhaAtual += 4;
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, xix));
            this.LinhaAtual += 7;

            this.FontProp.rSize = 6;

            string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");
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
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 1.25, 21, filePath, 7, 7); //--> CheckBox Sim do item 1

                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 1.25, 42, filePath, 7, 7);//--> CheckBox Não do item 2
                }
                else
                {
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 1.25, 21, filePath, 7, 7); //--> CheckBox Sim do item 1

                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 1.25, 42, filePath, 7, 7);//--> CheckBox Não do item 2

                    var nomeRepresentante = (null != entClienteNaoOpera) ? entClienteNaoOpera.DsNomeClienteRepresentado : string.Empty;

                    this.PaginaPDF.AddLT_MM(72, LinhaAtual - 1.0, new Root.Reports.RepString(this.FontProp, nomeRepresentante));
                }

                this.InsereLinhaContinua(72, LinhaAtual - 2, 80, 8, false);
            }

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "2", "Autoriza a transmissão de ordens por procurador ou representante?", ".");

            {   //--> CheckBox Sim e Não do item 2
                this.PaginaPDF.AddLT_MM(106, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Sim"));
                this.PaginaPDF.AddLT_MM(124, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Não"));

                if (entRepresentante != null && entRepresentante.Count > 0)
                {
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 0.25, 102.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.25, 120.25, filePath, 7, 7); //--> CheckBox Não do item 2
                }
                else
                {
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.25, 102.5, filePath, 7, 7); //--> CheckBox Sim do item 2

                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 0.25, 120.25, filePath, 7, 7); //--> CheckBox Não do item 2
                }
            }

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S2a = "(Em caso positivo anexar a procuração ou documento específico, comprometendo-se a informar por escrito à Corretora no caso de revogação",
                   S2b = "de mandato).";
            this.LinhaSubTopico(LinhaAtual, 14, S2a);
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 14, S2b);
            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "3", "É pessoa vinculada à corretora (contato definido pela instrução CVM nº 505/2011)?", ".");
            
            this.PaginaPDF.AddLT_MM(126, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Sim, à Gradual"));
            this.PaginaPDF.AddLT_MM(153.75, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Sim, à outra corretora"));
            this.PaginaPDF.AddLT_MM(187.75, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Não"));

            {   //--> CheckBox Sim e Não do item 3
                // para pessoas vinculadas: 0: "não" 1: "sim, à outras corretoras; 2: "Sim, à Gradual"
                if (entCliente.StPessoaVinculada == 0)
                {
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 123.25, filePath, 7, 7);
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 150.75, filePath, 7, 7);
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 184.75, filePath, 7, 7);
                }
                else if (entCliente.StPessoaVinculada == 1)
                {
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 123.25, filePath, 7, 7);
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 150.75, filePath, 7, 7);
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 184.75, filePath, 7, 7);
                }
                else
                {
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 123.25, filePath, 7, 7);
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 150.75, filePath, 7, 7);
                    filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                    this.InsereImagem(LinhaAtual - 0.5, 184.75, filePath, 7, 7);
                }
            }

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S4a = "É cliente na condição de Pessoa Politicamente Exposta, conforme Circular 3.461/09 do Banco Central? ";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "4", S4a, ".");

            this.PaginaPDF.AddLT_MM(154, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Sim"));
            this.PaginaPDF.AddLT_MM(170, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Não"));

            //--> CheckBox Sim e Não do item 3
            if (null != entCliente.StPPE && entCliente.StPPE.Value)
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.5, 150.75, filePath, 7, 7);
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.5, 165.5, filePath, 7, 7);
            }
            else
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.5, 150.75, filePath, 7, 7);
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.5, 165.5, filePath, 7, 7);
            }

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "5", "Você é US Person? (Confirme Lei FACTA - Foreign Account Tax Compliance Act", ".");
            
            this.PaginaPDF.AddLT_MM(154, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Sim"));
            this.PaginaPDF.AddLT_MM(170, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Não"));

            //--> CheckBox Sim e Não do item 3
            if (entCliente.StUSPerson.HasValue && entCliente.StUSPerson.Value == true)
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.5, 150.75, filePath, 7, 7);
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.5, 165.5, filePath, 7, 7);
            }
            else
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                this.InsereImagem(LinhaAtual - 0.5, 150.75, filePath, 7, 7);
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                this.InsereImagem(LinhaAtual - 0.5, 165.5, filePath, 7, 7);
            }

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "6", "Concordo que a carteira própria da Corretora ou pessoas a ela vinculadas podem atuar na contraparte das operações que ordenar", ".");

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S5a = "(Caso a opção seja: não concordo ou concordo sob consulta, providenciar correspondência assinada explicando a opção)";

            this.LinhaSubTopico(LinhaAtual, 14, S5a);

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S5b = "(Esta declaração é obrigatória somente quando se tratar de clientes cuja carteira individual é administrada pela corretora)";

            this.LinhaSubTopico(LinhaAtual, 14, S5b);

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S6a = "São consideradas válidas ordens transmitidas verbalmente ou por escrito, conforme determina o documento Regras e Parâmetros de Atuação",
                   S6b = "da Gradual CCTVM S/A. (Caso a opção seja: Considerar válidas as odens transmitidas exclusivamente por escrito, encaminhar correspondência",
                   S6c = "com assinatura e firma reconhecida, solicitando o aceite pela corretora, que a protocolará, tornando-a parte integrante do cadastro).";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "7", S6a, ".");
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 14, S6b);
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 14, S6c);

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "8", "Não estou impedido de operar no mercado de valores mobiliários.", ".");

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S8a = "Tenho conhecimento do disposto nas instruções CVM nº 505 e 506/2011, das Regras e Parâmetros de Atualção da Corretora e das normas",
                   S8b = "Operacionais, do Fundo de Garantia das Bolsas, e das normas operacionais editadas pelas Bolsas e pelas Câmaras de Compensação e",
                   S8c = "Liquidação, as quais estão disponíveis nos respectivos sites.";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "9", S8a, ".");
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 14, S8b);
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 14, S8c);
            
            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "10", " Declaro, conforme determina a ICVM 553/2014, que a natureza e o propósito da relação de negócios com esta Corretora se dará com o seguinte fim:", ".");
            
            this.LinhaAtual += 4;

            //this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto //--> Verifica se a página deve ser quebrada neste ponto

            //this.LinhaSubTopico(this.LinhaAtual, 14, !string.IsNullOrEmpty(entCliente.DsPropositoGradual) ? entCliente.DsPropositoGradual : "______________________________________________________________________________________________________________________");


            string sLinha1 = "Investimento em BOVESPA       Investimento em BM&F        Fundo de Investimento       Aluguel Ações (BTC)",
                   sLinha2 = "Derivativos      Renda Fixa      Operações de mercado de Cambio      Outros:";
            
            this.LinhaSubTopico(LinhaAtual, 14, sLinha1);
        
            filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");

            this.InsereImagem(LinhaAtual - 0.3, 51.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 88.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 124.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 158.0, filePath, 7, 7);

            this.LinhaAtual += 4;
            
            this.LinhaSubTopico(LinhaAtual, 14, sLinha2);

            this.InsereImagem(LinhaAtual - 0.3, 29.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 49.0, filePath, 7, 7);
            this.InsereImagem(LinhaAtual - 0.3, 100.0, filePath, 7, 7);

            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "11", " Declaro, conforme determina a ICVM 553/2014, que concedo a autorização prévia para negociação de cotas de fundos de investimentos", ".");
            
            
            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaTopico(this.LinhaAtual, 8, 1, "12", " Declaro, conforme determina a ICVM 553/2014, que recebi e estou ciente dos seguintes documentos:", ".");
            
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

            this.InsereImagem(LinhaAtual - 0.3, 36.0, filePath, 7, 7);

            //--> CheckBox prospecto
            if (entCliente.StCienteDocumentos == 7 || entCliente.StCienteDocumentos == 6 || entCliente.StCienteDocumentos == 2)
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
            }
            else
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
            }

            this.InsereImagem(LinhaAtual - 0.3, 53.50, filePath, 7, 7);

            //--> CheckBox lâmina      
            if (entCliente.StCienteDocumentos == 7 || entCliente.StCienteDocumentos == 3 || entCliente.StCienteDocumentos == 1)
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
            }
            else
            {
                filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
            }

            this.InsereImagem(LinhaAtual - 0.3, 71.0, filePath, 7, 7);

            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 14, S12b);
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 14, S12c);
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 14, S12d);



            this.LinhaAtual += 4;

            string S9a = "Tenho conhecimento de que as operações realizadas no sistema de negociação de títulos e valores mobiliários mantidos pela SOMA não constam",
                   S9b = "contam com a proteção de fundo de garantia.";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "13", S9a, ".");
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 14, S9b);
            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S10a = " Estou ciente de que não devo entregar ou receber, por qualquer razão, numerário, títulos ou valores mobiliários, ou quaisquer outros valores por",
                   S10b = "meio de Agente Autônomo de Investimentos ou Prespostos da Corretora, bem como de que eles não poderão ser meus procuradores;";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "14", S10a, ".");
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 15, S10b);
            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S11a = " Autorizo a Corretora, caso existam débitos pendentes em meu nome, a liquidar, em Bolsa ou em Câmaras de Compensação e de Liquidação, os",
                   S11b = "contratos, direitos e ativos adquiridos por minha conta e ordem, bem como a executar bens e direitos dados em garantia de minhas operações,",
                   S11c = "ou que estejam em poder da Corretora, aplicando o produto da venda no pagamento dos débitos pendentes, independente de notificação judicial",
                   S11d = "ou extrajudicial";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "15", S11a, ".");
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 15, S11b);
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 15, S11c);
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 15, S11d);
            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S16a = " Mediante este documento, adiro aos termos do contrato de prestação de serviços de Custódia Fungível dos Ativos da CBLC, firmado por esta",
                   S16b = "Corretora, outorgando à CBLC poderes para, na qualidade de proprietário fiduciário, transferir para o seu nome, nas companhias emitentes,",
                   S16c = "os ativos de minha propriedade;";

            this.LinhaTopico(this.LinhaAtual, 8, 1, "16", S16a, ".");
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 15, S16b);
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 15, S16c);
            this.LinhaAtual += 4;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S13a = " Estou ciente e concordo que minhas conversas com os representantes da Corretora acerca de quisquer assuntos relativos às minhas operações",
                   S13b = "poderão ser gravadas, podendo, ainda, o conteúdo ser usado como prova no esclarecimento de questões relacionadas à minha conta e às",
                   S13c = "minhas operações nesta Corretora.";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "17", S13a, ".");
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 15, S13b);
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 15, S13c);
            this.LinhaAtual += 5;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            string S14a = " São Verdadeira as informações fornecidas para o preenchimento deste cadastro, e me comprometo a informar no prazo de 10 (dez) dias",
                   S14b = "quaisquer alterações que virem a ocorrer nos meus dados cadastrais;";
            this.LinhaTopico(this.LinhaAtual, 8, 1, "18", S14a, ".");
            this.LinhaAtual += 4;
            this.LinhaSubTopico(LinhaAtual, 15, S14b);

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            this.LinhaAtual += 5;
            this.LinhaTopico(this.LinhaAtual, 8, 1, "19", " Endereço para recebimento de correspondência, emitidas pela Corretora e pelas Bolsas de Valores e/ou Futuros:", ".");
            this.LinhaAtual += 5;

            {   //--> CheckBox Residencial, Comercial e Outro do item 3
                this.PaginaPDF.AddLT_MM(18, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Residencial"));
                this.PaginaPDF.AddLT_MM(50, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Comercial"));
                this.PaginaPDF.AddLT_MM(78, LinhaAtual, new Root.Reports.RepString(this.FontProp, "Outro informar:"));
                this.InsereLinhaContinua(100, this.LinhaAtual, 100, this.FontProp.rSize, false);

                int count = default(int);

                entEndereco = entEndereco.FindAll(cei => cei.StPrincipal); //--> Busca o endereço principal do cliente

                while (entEndereco != null && entEndereco.Count.CompareTo(count).Equals(1))
                {
                    if (entEndereco[count].IdTipoEndereco == 2) //--> Residencial
                    {
                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 14.40, filePath, 7, 7); //--> CheckBox Residencial

                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 46.43, filePath, 7, 7); //--> CheckBox Comercial

                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 74.5, filePath, 7, 7); //--> CheckBox Outro
                    }
                    if (entEndereco[count].IdTipoEndereco == 1) //--> Comercial
                    {
                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 14.40, filePath, 7, 7); //--> CheckBox Residencial

                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 46.43, filePath, 7, 7); //--> CheckBox Comercial

                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 74.5, filePath, 7, 7); //--> CheckBox Outro
                        
                        string endereco;

                        if (string.IsNullOrEmpty(entEndereco[count].DsComplemento))
                            endereco = string.Format("{0}, nº{1} {2}-{3} CEP: {4}", entEndereco[count].DsLogradouro, entEndereco[count].DsNumero, entEndereco[count].DsCidade, entEndereco[count].CdUf, entEndereco[count].NrCep);
                        else
                            endereco = string.Format("{0}, nº {1} Comp.: {2} CEP: {5} {3}-{4} ", entEndereco[count].DsLogradouro, entEndereco[count].DsNumero, entEndereco[count].DsComplemento, entEndereco[count].DsCidade, entEndereco[count].CdUf, entEndereco[count].NrCep);

                        this.FontProp.rSize -= 1;
                        this.PaginaPDF.AddLT_MM(98.5, LinhaAtual - 0.25, new Root.Reports.RepString(this.FontProp, endereco));
                        this.FontProp.rSize += 2;
                    }
                    else if (entEndereco[count].IdTipoEndereco == 3) //--> Outros
                    {
                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 14.40, filePath, 7, 7); //--> CheckBox Residencial

                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 46.43, filePath, 7, 7); //--> CheckBox Comercial

                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                        this.InsereImagem(LinhaAtual - 0.2, 74.5, filePath, 7, 7); //--> CheckBox Outro

                        string endereco;

                        if (string.IsNullOrEmpty(entEndereco[count].DsComplemento))
                            endereco = string.Format("{0}, nº{1} {2}-{3} CEP: {4}", entEndereco[count].DsLogradouro, entEndereco[count].DsNumero, entEndereco[count].DsCidade, entEndereco[count].CdUf, entEndereco[count].NrCep);
                        else
                            endereco = string.Format("{0}, nº {1} Comp.: {2} CEP: {5} {3}-{4} ", entEndereco[count].DsLogradouro, entEndereco[count].DsNumero, entEndereco[count].DsComplemento, entEndereco[count].DsCidade, entEndereco[count].CdUf, entEndereco[count].NrCep);

                        this.FontProp.rSize -= 1;
                        this.PaginaPDF.AddLT_MM(98.5, LinhaAtual - 0.25, new Root.Reports.RepString(this.FontProp, endereco));
                        this.FontProp.rSize += 2;
                    }
                    count++;
                }
            }

            this.QuebraLinhaDupla();
            var tamAnt = this.FontProp.rSize;
            this.FontProp.rSize = 5;
            string v1 = "(Alterações de endereço de correspondência somente serão atendidas quando do recebimento de correspondência formal e cópia do comprovante de endereço atualizado)";
            this.PaginaPDF.AddCB_MM(LinhaAtual, new Root.Reports.RepString(this.FontProp, v1));
            this.FontProp.rSize = tamAnt;

            this.IsFinalPagina(padding); //--> Verifica se a página deve ser quebrada neste ponto

            //this.LinhaAtual += 5;
            //this.LinhaTopico(this.LinhaAtual, 8, 0, "16", " Plano escolhido pelo cliente:", ".");
            //this.LinhaAtual += 5;

            //this.PaginaPDF.AddLT_MM(18, LinhaAtual, new Root.Reports.RepString(this.FontProp, "GRADUAL DIRECT"));
            //this.PaginaPDF.AddLT_MM(50, LinhaAtual, new Root.Reports.RepString(this.FontProp, "GRADUAL ASSESSORIA"));

            //{   //--> CheckBox Sim e Não do item 3
            //    if (null != entCliente.IdAssessorInicial && entCliente.IdAssessorInicial == 438)
            //    {
            //        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
            //        this.InsereImagem(LinhaAtual - 0.5, 14.75, filePath, 7, 7);
            //        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
            //        this.InsereImagem(LinhaAtual - 0.5, 46.43, filePath, 7, 7);
            //    }
            //    else
            //    {
            //        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
            //        this.InsereImagem(LinhaAtual - 0.5, 14.75, filePath, 7, 7);
            //        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
            //        this.InsereImagem(LinhaAtual - 0.5, 46.43, filePath, 7, 7);
            //    }
            //}

            this.QuebraLinhaDupla();

            this.InsereLinhaContinua(20, this.LinhaAtual, 80, 10, true);
            this.TextoValorItem(LinhaAtual + 2, 110, "Local e data", string.Empty);
            this.InsereLinhaContinua(130, this.LinhaAtual, 65, 10, true);
            this.LinhaAtual += 5;
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(45, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, "Assinatura do CLIENTE"));
            this.FontProp.bBold = false;
        }

        /// <summary>
        /// Método para capturar os Bens Imóveis
        /// </summary>
        private void MontaBensImoveis()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "BENS IMÓVEIS");
                this.LinhaAtual += 5;
            }

            //this.FontProp.bBold = true;
            //this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Tipo"));
            //this.PaginaPDF.AddLT_MM(38, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Endereço"));
            //this.PaginaPDF.AddLT_MM(83, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Cidade"));
            //this.PaginaPDF.AddLT_MM(123, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "UF"));
            //this.PaginaPDF.AddLT_MM(138, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Valor Atual (R$)"));
            //this.FontProp.bBold = false;
            //this.LinhaAtual += 5;
            
            CultureInfo lInfo = new CultureInfo("pt-BR");

            decimal lValor = 0;

            if(this.GetSituacaoFinanceiraPatrimonial.VlTotalBensImoveis.HasValue)
                lValor = this.GetSituacaoFinanceiraPatrimonial.VlTotalBensImoveis.Value;

            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(118, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "TOTAL"));
            this.FontProp.bBold = false;
            this.PaginaPDF.AddLT_MM(131, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, lValor.ToString("C2", lInfo)));

            //foreach (EBensImoveis _bensImoveis in new NBensImoveis().Listar(this.GetIdCliente))
            //{
            //    this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, NDadosSinacor.Selecionar(_bensImoveis.Tipo.ToString(), NDadosSinacor.eTabela.BensImoveis)));
            //    this.PaginaPDF.AddLT_MM(38, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, _bensImoveis.Endereco));
            //    this.PaginaPDF.AddLT_MM(83, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, _bensImoveis.Cidade));
            //    this.PaginaPDF.AddLT_MM(123, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, _bensImoveis.UF));
            //    this.PaginaPDF.AddLT_MM(138, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, Gradual.Generico.Geral.Conversao.ToCurrency(_bensImoveis.Valor.ToString())));
            //    this.LinhaAtual += 3;
            //}

            //this.FontProp.rSize = fontAnt;
        }

        private void MontaInformacoesPatrimoniais()
        {
            MontaInformacoesPatrimoniais(false);
        }

        private void MontaInformacoesPatrimoniais(bool CabecalhoCambio)
        {
            var entCliente = this.GetFichaCadastralInfo.ClienteSituacaoFinanceiraPatrimonialResponse.Resultado; // new NCliente().Listar(this.GetIdCliente);
            
            Root.Reports.FontProp fontPropPeq = new Root.Reports.FontProp(this.FontDefin, 5);
            fontPropPeq.bBold = false;

            string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");

            {   //--> Definindo a barra de título
                this.LinhaAtual += 10;
                
                if (!CabecalhoCambio)
                {
                    this.InsereImagem(LinhaAtual - 3, 8, filePath, 552, 15);
                    this.MontaBarraTitulo(this.LinhaAtual, "INFORMAÇÕES PATRIMONIAIS");
                    this.LinhaAtual += 8;
                }
                
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "RENDIMENTOS");
                this.LinhaAtual += 5;
            }

            var lcsfp = entCliente.Count > 0 ? entCliente[0] : new ClienteSituacaoFinanceiraPatrimonialInfo();

            //decimal salario = entCliente.Count > 0 ? entCliente[0].VlTotalSalarioProLabore.Value : 0M; // Conversao.ToInt64(entCliente.Salario);
            //decimal outrosRendimentosValor = entCliente.Count > 0 ? entCliente[0].VlTotalOutrosRendimentos.Value : 0M;

            CultureInfo lInfo = new CultureInfo("pt-BR");

            decimal lValor;

            if (!CabecalhoCambio)
            {
                this.FontProp.bBold = true;
                this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Rendimentos Mensais"));
                this.PaginaPDF.AddLT_MM(43, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Valor Atual R$"));
                this.FontProp.bBold = false;
                this.LinhaAtual += 5;

                this.FontProp.bBold = true;
                this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Salário/Pró-labore"));
                this.PaginaPDF.AddLT_MM(90, this.LinhaAtual, new Root.Reports.RepString(fontPropPeq, "O salário deve ser informado por quem mantém vínculo empregatício, aposentados, pensionistas, etc."));
                this.FontProp.bBold = false;


                lValor = 0;

                if (lcsfp.VlTotalSalarioProLabore.HasValue)
                    lValor = lcsfp.VlTotalSalarioProLabore.Value;

                this.PaginaPDF.AddLT_MM(43, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, lValor.ToString("C2", lInfo)));
                this.LinhaAtual += 5;

                this.FontProp.bBold = true;
                this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Outros Rendimentos"));
                this.PaginaPDF.AddLT_MM(90, this.LinhaAtual, new Root.Reports.RepString(fontPropPeq, "Outros rendimentos é destinado a rendas extras, tais como: Alguém, mesada de estudante, etc."));
                this.FontProp.bBold = false;

                lValor = 0;

                if (lcsfp.VlTotalOutrosRendimentos.HasValue)
                    lValor = lcsfp.VlTotalOutrosRendimentos.Value;

                this.PaginaPDF.AddLT_MM(43, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, lValor.ToString("C2", lInfo)));
                this.LinhaAtual += 5;

                this.InsereImagem(LinhaAtual, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual + 4, "APLICAÇÕES FINANCEIRAS");
                this.LinhaAtual += 7;

                this.FontProp.bBold = true;
                this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Aplicações Financeiras"));
                this.PaginaPDF.AddLT_MM(90, this.LinhaAtual, new Root.Reports.RepString(fontPropPeq, "Aplicações em fundos, clubes de investimentos, carteiras de ações, poupança, etc."));
                this.FontProp.bBold = false;

                lValor = 0;

                if (lcsfp.VlTotalAplicacaoFinanceira.HasValue)
                    lValor = lcsfp.VlTotalAplicacaoFinanceira.Value;

                this.PaginaPDF.AddLT_MM(43, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, lValor.ToString("C2", lInfo)));
                this.LinhaAtual += 5;
            }
            else
            {
                lValor = 0;

                this.FontProp.bBold = true;
                this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Renda Mensal"));
                //this.PaginaPDF.AddLT_MM(90, this.LinhaAtual, new Root.Reports.RepString(fontPropPeq, ""));
                this.FontProp.bBold = false;

                lValor = 0;

                if (lcsfp.VlTotalSalarioProLabore.HasValue)
                    lValor = lcsfp.VlTotalSalarioProLabore.Value;

                if (lcsfp.VlTotalOutrosRendimentos.HasValue)
                    lValor += lcsfp.VlTotalOutrosRendimentos.Value;

                this.PaginaPDF.AddLT_MM(43, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, lValor.ToString("C2", lInfo)));
                this.LinhaAtual += 5;
            }
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

        private void MontaInformacoesRendimentosCambio()
        {
            var entCliente = this.GetFichaCadastralInfo.ClienteSituacaoFinanceiraPatrimonialResponse.Resultado; // new NCliente().Listar(this.GetIdCliente);

            Root.Reports.FontProp fontPropPeq = new Root.Reports.FontProp(this.FontDefin, 5);
            fontPropPeq.bBold = false;

            string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");

            {   //--> Definindo a barra de título
                this.LinhaAtual += 10;
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "RENDIMENTOS");
                this.LinhaAtual += 5;
            }

            var lcsfp = entCliente.Count > 0 ? entCliente[0] : new ClienteSituacaoFinanceiraPatrimonialInfo();

            CultureInfo lInfo = new CultureInfo("pt-BR");

            decimal lValor;
            this.FontProp.bBold = false;

            lValor = 0;

            this.PaginaPDF.AddLT_MM(43, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, lValor.ToString("C2", lInfo)));
            this.LinhaAtual += 5;

            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Outros Rendimentos"));
            this.PaginaPDF.AddLT_MM(90, this.LinhaAtual, new Root.Reports.RepString(fontPropPeq, "Outros rendimentos é destinado a rendas extras, tais como: Alguém, mesada de estudante, etc."));
            this.FontProp.bBold = false;

            lValor = 0;

            if (lcsfp.VlTotalSalarioProLabore.HasValue)
                lValor = lcsfp.VlTotalSalarioProLabore.Value;

            if (lcsfp.VlTotalOutrosRendimentos.HasValue)
                lValor += lcsfp.VlTotalOutrosRendimentos.Value;

            this.PaginaPDF.AddLT_MM(43, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, lValor.ToString("C2", lInfo)));
            this.LinhaAtual += 5;
        }

        private void MontaDadosResponsavel()
        {
            var entCliente = this.GetFichaCadastralInfo.ClienteResponse.Objeto;
            var entTelefoneFix = this.GetFichaCadastralInfo.ClienteTelefoneReponse.Resultado.Find(delegate(ClienteTelefoneInfo cti) { return cti.IdTipoTelefone == 1; });
            var entTelefoneCel = this.GetFichaCadastralInfo.ClienteTelefoneReponse.Resultado.Find(delegate(ClienteTelefoneInfo cti) { return cti.IdTipoTelefone == 3; });

            bool _menorDeIdade = DateTime.Compare(entCliente.DtNascimentoFundacao.Value.AddYears(18), DateTime.Now).Equals(1);
            bool _emancipado = entCliente.StEmancipado != null && entCliente.StEmancipado.Value;

            if ((_menorDeIdade) || (_emancipado))
            {
                string filePath = null;

                {   //--> Definindo a barra de título
                    this.LinhaAtual += 15;
                    filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");
                    this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                    this.MontaBarraTitulo(this.LinhaAtual, "DADOS DO RESPONSÁVEL (QUANDO APLICÁVEL)");
                    this.LinhaAtual += 5;
                }

                this.FontProp.bBold = true;
                this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Situação legal do Cliente"));
                this.FontProp.bBold = false;
                this.LinhaAtual += 5;

                var entRepresentante = (this.GetFichaCadastralInfo.ClienteProcuradorRepresentanteResponse.Resultado != null
                                    && (this.GetFichaCadastralInfo.ClienteProcuradorRepresentanteResponse.Resultado.Count > 0))
                                     ? (this.GetFichaCadastralInfo.ClienteProcuradorRepresentanteResponse.Resultado[0])
                                     : (new ClienteProcuradorRepresentanteInfo());

                {   //--> Checkbox Situação Legal

                    if (_emancipado)
                    {
                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                        this.InsereImagem(this.LinhaAtual - 0.17, 13, filePath, 7, 7);

                        this.InsereImagem(this.LinhaAtual - 0.17, 57.75, filePath, 7, 7);

                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                        this.InsereImagem(this.LinhaAtual - 0.17, 30.75, filePath, 7, 7);
                    }
                    else if (_menorDeIdade)
                    {
                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                        this.InsereImagem(this.LinhaAtual - 0.17, 13, filePath, 7, 7);

                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                        this.InsereImagem(this.LinhaAtual - 0.17, 30.75, filePath, 7, 7);

                        this.InsereImagem(this.LinhaAtual - 0.17, 57.75, filePath, 7, 7);
                    }
                    else //--> Outros
                    {
                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_none.jpg");
                        this.InsereImagem(this.LinhaAtual - 0.17, 13, filePath, 7, 7);
                        this.InsereImagem(this.LinhaAtual - 0.17, 30.75, filePath, 7, 7);

                        filePath = string.Concat(pathVirtualImagesCadastro, "checbox_full.jpg");
                        this.InsereImagem(this.LinhaAtual - 0.17, 57.75, filePath, 7, 7);
                    }

                    this.FontProp.bBold = true;
                    this.PaginaPDF.AddLT_MM(16.25, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Menor"));
                    this.PaginaPDF.AddLT_MM(34, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Emancipado"));
                    this.PaginaPDF.AddLT_MM(61, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Outros"));
                    this.FontProp.bBold = false;
                }

                this.LinhaAtual += 4;

                if (null == entRepresentante || null == entRepresentante.NrCpfCnpj)
                    return;

                this.InsereLinhaContinua(8, this.LinhaAtual, 194, this.FontProp.rSize, true);

                this.LinhaAtual += 4;

                this.FontProp.bBold = true;
                this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Dados do Responsável pelo Cliente menor ou incapaz"));
                this.FontProp.bBold = false;

                this.LinhaAtual += 1;

                this.InsereLinhaContinua(8, this.LinhaAtual, 194, this.FontProp.rSize, true);

                this.LinhaAtual += 5;

                this.TextoValorItem(this.LinhaAtual, 8, "Nome do Responsável", entRepresentante.DsNome.ToStringFormatoNome());
                this.TextoValorItem(this.LinhaAtual, 130, "CPF", entRepresentante.NrCpfCnpj.ToCpfCnpjString());
                //this.TextoValorItem(this.LinhaAtual, 170, "Sexo", Conversao.ToString(entRepresentante[0].Sexo));

                this.QuebraLinhaDupla();

                this.FontProp.bBold = true;

                string situacao = this.RecuperarSituacaoLegal(entRepresentante.TpSituacaoLegal);

                this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, string.Concat("Situação Legal do Responsável: ", situacao)));
                this.FontProp.bBold = false;

                this.LinhaAtual += 5;

                /*
                var lClienteTelefoneInfo = this.RecuperarTelefone(entRepresentante.IdProcuradorRepresentante);

                var lRepresentanteFoneFix = lClienteTelefoneInfo.Find(delegate(ClienteTelefoneInfo cti) { return cti.IdTipoTelefone == 1; });
                var lRepresentanteFoneCel = lClienteTelefoneInfo.Find(delegate(ClienteTelefoneInfo cti) { return cti.IdTipoTelefone == 3; });

                this.TextoValorItem(this.LinhaAtual, 8, "DDD", (null != lRepresentanteFoneFix) ? lRepresentanteFoneFix.DsDdd : string.Empty);
                this.TextoValorItem(this.LinhaAtual, 30, "Telefone", (null != lRepresentanteFoneFix) ? lRepresentanteFoneFix.DsNumero.ToTelefoneString() : string.Empty);
                this.TextoValorItem(this.LinhaAtual, 60, "DDD", (null != lRepresentanteFoneCel) ? lRepresentanteFoneCel.DsDdd : string.Empty);
                this.TextoValorItem(this.LinhaAtual, 70, "Telefone", (null != lRepresentanteFoneCel) ? lRepresentanteFoneCel.DsNumero.ToTelefoneString() : string.Empty);

                this.QuebraLinha(14);
                */

                this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Anexar documento comprobatório (RG e CPF)"));
            }
        }

        private void MontaFontesReferenciaBancaria()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "FONTES DE REFERÊNCIA BANCÁRIA");
                this.LinhaAtual += 5;
            }

            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Banco"));
            this.PaginaPDF.AddLT_MM(78, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Agência"));
            this.PaginaPDF.AddLT_MM(108, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Conta"));
            this.PaginaPDF.AddLT_MM(128, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Tipo"));
            this.FontProp.bBold = false;

            this.LinhaAtual += 1;

            this.GetFichaCadastralInfo.ClienteBancoResponse.Resultado.ForEach(
                delegate(ClienteBancoInfo entConta)
                {
                    this.LinhaAtual += 4.5;

                    this.PaginaPDF.AddLT_MM(8, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, this.RecuperarNomeBanco(entConta.CdBanco)));

                    this.PaginaPDF.AddLT_MM(78, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, string.Concat(entConta.DsAgencia, string.IsNullOrWhiteSpace(entConta.DsAgenciaDigito) ? string.Empty : "-", entConta.DsAgenciaDigito)));

                    if (string.IsNullOrEmpty(entConta.DsContaDigito))
                        this.PaginaPDF.AddLT_MM(108, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, entConta.DsConta));
                    else
                        this.PaginaPDF.AddLT_MM(108, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, string.Concat(entConta.DsConta, string.IsNullOrWhiteSpace(entConta.DsContaDigito) ? string.Empty : "-", entConta.DsContaDigito)));

                    switch (entConta.TpConta)
                    {
                        case "CI":
                            this.PaginaPDF.AddLT_MM(128, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Investimento"));
                            break;
                        case "CC":
                            this.PaginaPDF.AddLT_MM(128, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Conta Corrente"));
                            break;
                        case "CP":
                            this.PaginaPDF.AddLT_MM(128, this.LinhaAtual, new Root.Reports.RepString(this.FontProp, "Poupança"));
                            break;
                    }
                });

            //this.FontProp.rSize = fontAnt;
        }

        private void MontaInformacoesComerciais()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(pathVirtualImages, "pixel_black.jpg");
                this.InsereImagem(this.LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "INFORMAÇÕES COMERCIAIS");
                this.LinhaAtual += 5;
            }

            var entCliente = this.GetFichaCadastralInfo.ClienteResponse.Objeto;
            var entEndereco = this.GetFichaCadastralInfo.ClienteEnderecoResponse.Resultado.Find(delegate(ClienteEnderecoInfo cei) { return cei.IdTipoEndereco == 1; });
            var entTelefoneCom = this.GetFichaCadastralInfo.ClienteTelefoneReponse.Resultado.Find(delegate(ClienteTelefoneInfo cti) { return cti.IdTipoTelefone == 2; });
            var entTelefoneFax = this.GetFichaCadastralInfo.ClienteTelefoneReponse.Resultado.Find(delegate(ClienteTelefoneInfo cti) { return cti.IdTipoTelefone == 4; });

            this.TextoValorItem(LinhaAtual, 8, "Instituição em que trabalha", entCliente.DsEmpresa);
            this.QuebraLinhaDupla();
            this.TextoValorItem(LinhaAtual, 8, "Profissão", this.RecuperarProfissao(entCliente.CdProfissaoAtividade));
            this.TextoValorItem(LinhaAtual, 130, "Cargo atual ou função", entCliente.DsCargo);
            /*
            //Removendo para diminuir espaço na ficha
            this.QuebraLinhaDupla();
            this.TextoValorItem(LinhaAtual, 8, "Endereço comercial (rua, avenida, etc.)", (null != entEndereco) ? entEndereco.DsLogradouro : string.Empty);
            this.TextoValorItem(LinhaAtual, 130, "Número", (null != entEndereco) ? entEndereco.DsNumero : string.Empty);
            this.TextoValorItem(LinhaAtual, 160, "Complemento", (null != entEndereco) ? entEndereco.DsComplemento : string.Empty);
            this.QuebraLinhaDupla();
            this.TextoValorItem(LinhaAtual, 8, "Bairro", (null != entEndereco) ? entEndereco.DsBairro : string.Empty);
            this.TextoValorItem(LinhaAtual, 55, "CEP", (null != entEndereco) ? string.Format("{0}-{1}", entEndereco.NrCep.DBToString(), entEndereco.NrCepExt.DBToString().PadLeft(3, '0')) : string.Empty);
            this.TextoValorItem(LinhaAtual, 75, "Cidade", (null != entEndereco) ? entEndereco.DsCidade : string.Empty);
            this.TextoValorItem(LinhaAtual, 130, "Estado", (null != entEndereco) ? entEndereco.CdUf : string.Empty);
            this.TextoValorItem(LinhaAtual, 170, "País", (null != entEndereco) ? this.RecuperarPais(entEndereco.CdPais) : string.Empty);
            this.QuebraLinhaDupla();
            this.TextoValorItem(LinhaAtual, 8, "Telefone (DDD + número)", (entTelefoneCom != null) ? string.Concat(entTelefoneCom.DsDdd, "-", entTelefoneCom.DsNumero.ToTelefoneString()) : string.Empty);
            this.TextoValorItem(LinhaAtual, 55, "FAX (DDD + número)", (entTelefoneFax != null) ? string.Concat(entTelefoneFax.DsDdd, "-", entTelefoneFax.DsNumero.ToTelefoneString()) : string.Empty);
            this.TextoValorItem(LinhaAtual, 130, "E-Mail (endereço eletrônico)", entCliente.DsEmailComercial);
             * */
        }

        #endregion

        #region | Controles de texto

        private void MontaCabecalho()
        {
            string filePath = string.Concat(pathVirtualImages, "logo_gradual.jpg");
            this.InsereImagem(6, 8, filePath, 50, 50);
        }

        private void TextoValorItem(double linha, double coluna, string Campo, string valor)
        {
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(coluna, linha, new Root.Reports.RepString(this.FontProp, Campo));
            this.FontProp.bBold = false;
            this.PaginaPDF.AddLT_MM(coluna, linha + 4, new Root.Reports.RepString(this.FontProp, valor));
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
            this.FontProp.bBold = true;
            this.PaginaPDF.AddLT_MM(coluna + LeftPad, linha, new Root.Reports.RepString(this.FontProp, string.Concat(topicoId, caractereSeparador)));
            this.FontProp.bBold = false;
            this.PaginaPDF.AddLT_MM(coluna + espaco, linha, new Root.Reports.RepString(this.FontProp, texto));
        }

        private void LinhaSubTopico(double linha, int coluna, string texto)
        {
            this.PaginaPDF.AddLT_MM(coluna, linha, new Root.Reports.RepString(this.FontProp, texto));
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
            if (this.LinhaAtual.CompareTo(255).Equals(1))
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

        //private string ServerMapPath(string objectPath)
        //{
        //    return string.Concat(this._ServerMapPath, "\\", objectPath);
        //}

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
                        Informacao = eInformacao.SituacaoLegalRepresentante
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
                        Informacao = eInformacao.ProfissaoPF
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
