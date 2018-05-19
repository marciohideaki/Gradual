using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;
using System.Configuration;
using Gradual.Servico.FichaCadastral.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Root.Reports;

namespace Gradual.Servico.FichaCadastral
{
    public class TermoAdesao_PF
    {
        #region | Atributos

        private int _IdCliente;

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private double LinhaAtual = 20;
        private Root.Reports.Page PaginaPDF;
        private Root.Reports.FontProp FontProp;
        private Root.Reports.Report ReportCadastro;
        private Root.Reports.FontDef FontDefin;

        private string pathVirtualPDF = "../ReportPDF";
        private string pathVirtualImages = "../../Images/";
        private string pathVirtualImagesCadastro = "../../images/cadastro/";
        private string fileNamePDF;

        private Gradual.Intranet.Contratos.Dados.FichaCadastralInfo gFichaCadastralInfo = null;

        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna o Id do cliente
        /// </summary>
        private int GetIdCliente
        {
            get { return this._IdCliente; }
            set { this._IdCliente = value; }
        }

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
                if (gFichaCadastralInfo== null)
                {
                    var lFichaCadastral = FichaCadastralDbLib.ReceberFichaCadastral(new ReceberEntidadeRequest<Gradual.Intranet.Contratos.Dados.FichaCadastralInfo>()
                    {
                        Objeto = new FichaCadastralInfo() { IdCliente = this.GetIdCliente, }
                    });

                    if (lFichaCadastral != null)
                        gFichaCadastralInfo = lFichaCadastral.Objeto;
                }

                return this.gFichaCadastralInfo;
            }

            set
            {
                gFichaCadastralInfo = value;
            }
        }


        public SistemaOrigem SitemaOrigem { get; set; }

        #endregion

        #region Construtores

        public TermoAdesao_PF() { }

        #endregion

        #region | Métodos Servico

        public ReceberObjetoResponse<TermoAdesaoGradualInfo> GerarTermoDeAdesao_PF(ReceberEntidadeRequest<TermoAdesaoGradualInfo> pParametro)
        {
            ReceberObjetoResponse<TermoAdesaoGradualInfo> lResponse = new ReceberObjetoResponse<TermoAdesaoGradualInfo>();

            this.GetIdCliente = pParametro.Objeto.IdCliente;
            this.pathVirtualPDF = ConfigurationManager.AppSettings["pathVirtualPDF"];
            this.pathVirtualImages = ConfigurationManager.AppSettings["pathVirtualImages"];
            this.pathVirtualImagesCadastro = ConfigurationManager.AppSettings["pathVirtualImagesCadastro"];
            this.SitemaOrigem = pParametro.Objeto.SitemaOrigem;

            GerarTermo(); //--> Gerando o relatório em arquivo.

            lResponse.Objeto = new TermoAdesaoGradualInfo();
            lResponse.Objeto.PathDownloadPdf = this.fileNamePDF;

            return lResponse;
        }

        #endregion

        #region Métodos Private - Texto

        private void EscreverTermo()
        {
            this.LinhaAtual = 25;
            this.MontaFichaCadastralCliente();
            this.IsFinalPagina(0);
            //this.MontaInformacoesComerciais();
            this.IsFinalPagina(0);
            //this.MontaFontesReferenciaBancaria();
            this.IsFinalPagina(0);
            //this.MontaDadosResponsavel();
            this.IsFinalPagina(0);
            //this.MontaInformacoesPatrimoniais();
            this.IsFinalPagina(0);
            //this.MontaBensImoveis();
            //this.IsFinalPagina(0);
        }

        /// <summary>
        /// Retorna endereço para responsável para abrir uma janela no Window.Open contendo a Ficha DUC.
        /// </summary>
        public string CaminhoDownLoadRelatorio()
        {
            return string.Concat(pathVirtualPDF, "/", fileNamePDF);
        }

        /// <summary>
        /// Procedimento que dispara a geração dos relatórios.
        /// </summary>
        private void GerarTermo()
        {
            Guid g;
            g = Guid.NewGuid();

            this.ReportCadastro = new Root.Reports.Report(new Root.Reports.PdfFormatter());

            this.FontDefin = new Root.Reports.FontDef(this.ReportCadastro, Root.Reports.FontDef.StandardFont.Helvetica);
            this.FontProp = new Root.Reports.FontProp(this.FontDefin, 7);

            this.NovaPagina(0);

            this.EscreverTermo();

            //this.fileNamePDF = string.Format("TermoAdesao-{0}.pdf", this.GetIdCliente.ToString());
            this.fileNamePDF = g.ToString();

            if (this.SitemaOrigem == SistemaOrigem.Portal)
            {
                this.ReportCadastro.Save(string.Concat(this.GetEnderecoArquivoPortal, "\\", fileNamePDF));
            }
            else
            {
                this.ReportCadastro.Save(string.Concat(this.GetEnderecoArquivoIntranet, "\\", fileNamePDF));
            }
        }

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
            
            this.InsereLinhaContinua(8, linha, 194, 2, true);
            linha = linha + 5;
            fontProp.bBold = true;
            this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, "GRADUAL CORRETORA DE CÂMBIO, TÍTULOS E VALORES MOBILIÁRIOS S/A"));
            fontProp.bBold = false;
            string tx = "C.N.P.J: 33.918.160/0001-73";
            linha = linha + 3;
            this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, tx));
            tx = "Avenida Presidente Juscelino Kubitschek - 50, 6º andar - Vila Nova Conceição - São Paulo - SP - 04543-011";
            linha = linha + 3;
            this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, tx));

            tx = "Ouvidoria/SAC: 0800 655 1466  |  gradualinvestimentos.com.br";
            linha = linha + 3;
            this.PaginaPDF.AddCB_MM(linha, new Root.Reports.RepString(fontProp, tx));
            fontProp.bUnderline = false;
        }

        /// <summary>
        /// Gera informações da declaração responsável
        /// </summary>
        private void MontaDeclaracaoResponsavelNaCorretora()
        {
            {   //--> Definindo a barra de título
                this.LinhaAtual += 15;
                string filePath = string.Concat(pathVirtualImages, "pixel_blue.jpg");
                this.InsereImagem(LinhaAtual - 4, 8, filePath, 552, 15);
                this.MontaBarraTitulo(this.LinhaAtual, "DECLARAÇÃO DO RESPONSÁVEL NA CORRETORA PELO CADASTRAMENTO DO CLIENTE");
                this.LinhaAtual += 3;
            }

            string texto0 = "Responsabilizo-me pela exatidão das informações prestadas, a vista dos originais dos documentos de Identidade, CPF/CNPJ, e outros comprobató-",
                   texto1 = "rios dos demais elementos de informação apresentados, sob pena de aplicação do disposto no artigo 64 da Lei nº 8383 de 30 de dezembro de 1991.";

            double fontAnt = this.FontProp.rSize;
            this.PaginaPDF.AddLT_MM(8, LinhaAtual, new Root.Reports.RepString(this.FontProp, texto0));
            this.LinhaAtual += 4;
            this.PaginaPDF.AddLT_MM(8, LinhaAtual, new Root.Reports.RepString(this.FontProp, texto1));

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
            this.FontProp.bBold = false;
            this.LinhaAtual += 3;
            this.PaginaPDF.AddLT_MM(8, LinhaAtual + 1, new Root.Reports.RepString(this.FontProp, "- Documento de Identidade, do CPF e comprovante de endereço atualizado."));

            this.TextoValorItem(LinhaAtual + 2, 115, "Local e data", string.Empty);
            this.InsereLinhaContinua(135, this.LinhaAtual, 60, 10, true);
            this.LinhaAtual += 5;
        }

        private void MontaFichaCadastralCliente()
        {
            this.LinhaAtual += 5;

            ClienteInfo entCliente = this.GetFichaCadastralInfo.ClienteResponse.Objeto;
            LoginInfo   entLogin = this.GetFichaCadastralInfo.ClienteLoginResponse.Objeto;
            ClienteEnderecoInfo entEndereco = this.GetFichaCadastralInfo.ClienteEnderecoResponse.Resultado.Find(delegate(ClienteEnderecoInfo cei) { return cei.IdTipoEndereco == 2; });  // { return cei.StPrincipal == true; }); pediram pra "sempre colocar o residencial"

            if(entEndereco == null)
                entEndereco = this.GetFichaCadastralInfo.ClienteEnderecoResponse.Resultado.Find(delegate(ClienteEnderecoInfo cei) { return cei.StPrincipal == true; }); //failsafe pra pegar alguma coisa, se não tiver o residencial

            this.TextoCentralizado(LinhaAtual, "TERMO DE ADESÃO AO CONTRATO DE INTERMEDIAÇÃO", 9, true);

            this.LinhaAtual += 10;

            string lTexto;

            /*
            this.QuebraLinhaDupla();

            this.TextoTabuladoEsquerda(this.LinhaAtual, 8, string.Format("Por meio deste instrumento, eu, {0}, {1}, {2}, {3}, ",
                                                                          entCliente.DsNome
                                                                        , RecuperarDadosDeNacionalidade(entCliente.CdNacionalidade)
                                                                        , RecuperarEstadoCivil(entCliente.CdEstadoCivil)
                                                                        , RecuperarProfissao(entCliente.CdProfissaoAtividade)
                                                                        ));
            
            this.QuebraLinha();

            this.TextoTabuladoEsquerda(this.LinhaAtual, 8, string.Format("portador do RG n.º {0}, inscrito no CPF/MF sob o n.º {1}, residente e domiciliado em {2}, {3}, ",
                                                                          FormatarRG(entCliente.DsNumeroDocumento)
                                                                        , FormatarCPF(entCliente.DsCpfCnpj)
                                                                        , entEndereco.DsCidade
                                                                        , entEndereco.CdUf
                                                                        ));

            this.QuebraLinha();

            this.TextoTabuladoEsquerda(this.LinhaAtual, 8, string.Format("na {0}, n.º {1} {2}, CEP {3}, na condição de Cliente, contrato a Gradual Corretora de Câmbio, ",
                                                                          entEndereco.DsLogradouro
                                                                        , entEndereco.DsNumero
                                                                        , entEndereco.DsComplemento
                                                                        , FormatarCEP(entEndereco.NrCep, entEndereco.NrCepExt)
                                                                        ));
            */


            FlowLayoutManager flm = new FlowLayoutManager(null);
            flm.eNewContainer += new FlowLayoutManager.NewContainerEventHandler(flm_eNewContainer);
            flm.NewContainer();

            lTexto = string.Format("Por meio deste instrumento, eu, {0}, nacionalidade {1}, estado civil {2}, profissao {3}, ",
                                                                          entCliente.DsNome
                                                                        , RecuperarDadosDeNacionalidade(entCliente.CdNacionalidade)
                                                                        , RecuperarEstadoCivil(entCliente.CdEstadoCivil)
                                                                        , RecuperarProfissao(entCliente.CdProfissaoAtividade)
                                                                        );

            lTexto +=  string.Format("portador do RG n.º {0}, inscrito no CPF/MF sob o n.º {1}, residente e domiciliado na cidade de {2}, no estado de {3}, ",
                                                                          FormatarRG(entCliente.DsNumeroDocumento)
                                                                        , FormatarCPF(entCliente.DsCpfCnpj)
                                                                        , entEndereco.DsCidade
                                                                        , entEndereco.CdUf
                                                                        );
            
            lTexto +=  string.Format("no endereço {0}, n.º {1} {2}, CEP {3}, na condição de Cliente, contrato a Gradual Corretora de Câmbio, ",
                                                                          entEndereco.DsLogradouro
                                                                        , entEndereco.DsNumero
                                                                        , entEndereco.DsComplemento
                                                                        , FormatarCEP(entEndereco.NrCep, entEndereco.NrCepExt)
                                                                        );

            lTexto += "Títulos e Valores Mobiliários S/A (\"Gradual\"), sociedade com sede na cidade de São Paulo, Estado de São Paulo, na Av. Juscelino Kubitscheck, n.º 50, 5º andar, inscrita no CNPJ/MF sob o n.º 33.918.160/0001-73 e declaro que:";

            flm.Add(new RepString(this.FontProp, lTexto));

            flm.NewLine(24);

            lTexto = "\t\t\t\tI. Preenchi a Ficha Cadastral e reitero as declarações por mim feitas naquele instrumento;";

            flm.Add(new RepString(this.FontProp, lTexto));

            flm.NewLine(24);


            //TODO: verificar a possiblidade de colocar o numero do contrato parametrizavel devido as constantes mudancas / trocas do mesmo
            lTexto = "\t\t\t\tII. Li, compreendi e estou plenamente de acordo com todos os termos e condições do Contrato de Intermediação (“Contrato de Intermediação”), devidamente registrado no 8º Ofício de Registro de Títulos e Documentos da Cidade de São Paulo sob o nº 1418728, que se encontra disponível no website www.gradualinvestimentos.com.br, e do qual este instrumento é parte integrante e indissociável;";

            flm.Add(new RepString(this.FontProp, lTexto));

            flm.NewLine(24);



            lTexto = "\t\t\t\tIII. Li, compreendi e estou plenamente de acordo com os termos das Regras e Parâmetros de Atuação da Gradual, que se encontra disponível no site www.gradualinvestimentos.com.br;";

            flm.Add(new RepString(this.FontProp, lTexto));

            flm.NewLine(24);



            lTexto = "\t\t\t\tIV. Tenho ciência de que o Contrato de Intermediação engloba a possibilidade de realizar operações em diferentes mercados e com os diversos produtos nele especificados, sendo-me facultada a efetiva utilização dos mesmos, quando me convier;";

            flm.Add(new RepString(this.FontProp, lTexto));

            flm.NewLine(24);



            lTexto = "\t\t\t\tV. Estou ciente de que a adesão ao Contrato de Intermediação não configura garantia ou promessa de lucros e ganhos ao meu patrimônio, originados pelos meus investimentos realizados por intermédio pela Gradual;";

            flm.Add(new RepString(this.FontProp, lTexto));

            flm.NewLine(24);



            lTexto = "\t\t\t\tVI. Estou ciente de que os investimentos realizados no mercado de títulos e valores mobiliários, principalmente o de opções e de termo, são caracterizados por serem de riscos, podendo levar a perda total do investimento e de quantias adicionais a ele e, como consequência, ao decréscimo de meu patrimônio; e";

            flm.Add(new RepString(this.FontProp, lTexto));

            flm.NewLine(24);



            lTexto = "\t\t\t\tVII. Reconheço a validade e integridade da assinatura eletrônica como se manuscrita fosse, aceitando os efeitos plenos daí decorrentes, e me responsabilizo, integralmente, pela sua confidencialidade e correta utilização.";

            flm.Add(new RepString(this.FontProp, lTexto));

            flm.NewLine(48);
            

            lTexto = "Local e Data:";

            flm.Add(new RepString(this.FontProp, lTexto));

            flm.NewLine(56);


            lTexto = "Assinatura do Cliente: ___________________________________________________";

            flm.Add(new RepString(this.FontProp, lTexto));



            /*
            this.QuebraLinha();

            this.TextoTabuladoEsquerda(this.LinhaAtual, 8, "Títulos e Valores Imobiliários S/A (\"Gradual\"), sociedade com sede na cidade de São Paulo, Estado de São Paulo, na");

            this.QuebraLinha();

            this.TextoTabuladoEsquerda(this.LinhaAtual, 8, "Av. Juscelino Kubitscheck, n.º 50, 5º, 6º e 7º andares, inscrita no CNPJ/MF sob o n.º 33.918.160/0001-73 e declaro que:   ");
            */


        }

        void flm_eNewContainer(object oSender, FlowLayoutManager.NewContainerEventArgs ea)
        {
            StaticContainer sc = new StaticContainerMM(175, 230);

            this.PaginaPDF.AddMM(16, this.LinhaAtual, sc);
            ea.flm.SetContainer(sc);
        }

        private void MontarTermoDeAdesao()
        {

        }

        #endregion

        #region Controles de texto

        private void MontaCabecalho()
        {
            string filePath = string.Concat(pathVirtualImages, "logo_gradual.jpg");
            this.InsereImagem(6, 96, filePath, 50, 50);
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

        #region Tabulação

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

        #region Métodos Private

        private string RecuperarDadosDeNacionalidade(int? pCdNacionalidade)
        {
            if (pCdNacionalidade == null)
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

        private string RecuperarEstadoCivil(int? pTpEstadoCivil)
        {
            if (pTpEstadoCivil == null)
                return string.Empty;

            var lListaSituacaoLegal = new SinacorDbLib().ConsultarListaSinacor(
                new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.EstadoCivil
                    }
                });

            var lRetorno = lListaSituacaoLegal.Resultado.Find(delegate(SinacorListaInfo sli) { return sli.Id == pTpEstadoCivil.DBToString(); });

            if (lRetorno != null)
                return lRetorno.Value;

            return string.Empty;
        }

        private string RecuperarProfissao(int? pCdProfissao)
        {
            if (pCdProfissao == null)
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

        private string FormatarRG(string pRG)
        {
            string lRetorno = pRG;

            try
            {
                lRetorno = lRetorno.PadLeft(9, ' ').Insert(8, "-").Insert(5, ".").Insert(2, ".");
            }
            catch { }

            return lRetorno;
        }
        
        private string FormatarCPF(string pCPF)
        {
            string lRetorno = pCPF;

            try
            {
                lRetorno = lRetorno.PadLeft(11, ' ').Insert(9, "-").Insert(6, ".").Insert(3, ".");
            }
            catch { }

            return lRetorno;
        }

        private string FormatarCEP(int pParte1, int pParte2)
        {
            string lRetorno = "";

            lRetorno = pParte1.ToString().PadLeft(5, '0') + "-" + pParte2.ToString().PadLeft(3, '0');

            return lRetorno;
        }

        #endregion
    }
}
