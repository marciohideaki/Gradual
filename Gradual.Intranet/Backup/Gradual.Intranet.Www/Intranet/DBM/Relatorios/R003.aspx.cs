using System;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.DBM.Relatorios
{
    public partial class R003 : PaginaBaseAutenticada
    {
        #region | Atributos

        private static ReceberEntidadeCadastroResponse<ResumoDoClienteDadosCadastraisInfo> gDadosCadastrais;

        private static ReceberEntidadeCadastroResponse<ResumoDoClienteCorretagemInfo> gDadosCorretagem;

        private static ConsultarEntidadeCadastroResponse<ResumoDoClienteCarteiraInfo> gDadosCarteira;

        #endregion

        #region | Propriedades

        private int GetCodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["CodigoCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetNomeCliente
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["NomeCliente"]))
                    return null;

                return this.Request.Form["nm_cliente"];
            }
        }

        private int? GetCdAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                return null;
            }
        }

        public string DataDeCadastro { get; set; }

        public string DataUltimaOperacao { get; set; }

        public string NomeCliente { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Telefone { get; set; }

        public string Ramal { get; set; }

        public string Celular1 { get; set; }

        public string Celular2 { get; set; }

        public string Email { get; set; }

        public string Tipo { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string CorretagemNoMes { get; set; }

        public string VolumeNoMes { get; set; }

        public string CorretagemMediaNoAno { get; set; }

        public string VolumeMediaNoAno { get; set; }

        public string CorretagemEm12Meses { get; set; }

        public string VolumeEm12Meses { get; set; }

        public string ContaCorrenteDisponivel { get; set; }

        private ReceberEntidadeCadastroResponse<ResumoDoClienteDadosCadastraisInfo> GetDadosCadastrais
        {
            get
            {
                if (null == gDadosCadastrais)
                {
                    var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetCdAssessor);
                    gDadosCadastrais = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoDoClienteDadosCadastraisInfo>(
                    new ReceberEntidadeCadastroRequest<ResumoDoClienteDadosCadastraisInfo>()
                    {
                        EntidadeCadastro = new ResumoDoClienteDadosCadastraisInfo()
                        {
                            ConsultaCodigoAssessor = lListaAssessores,
                            ConsultaCdCliente = this.GetCodigoCliente,
                            ConsultaNmCliente = this.GetNomeCliente,
                        }
                    });
                }

                return gDadosCadastrais;
            }
        }

        private ReceberEntidadeCadastroResponse<ResumoDoClienteCorretagemInfo> GetDadosCorretagem
        {
            get
            {
                if (null == gDadosCorretagem)
                {
                    var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetCdAssessor);

                    gDadosCorretagem = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoDoClienteCorretagemInfo>(
                       new ReceberEntidadeCadastroRequest<ResumoDoClienteCorretagemInfo>()
                       {
                           EntidadeCadastro = new ResumoDoClienteCorretagemInfo()
                           {
                               ConsultaCodigoAssessor = lListaAssessores,
                               ConsultaCdCliente = this.GetCodigoCliente,
                               ConsultaNmCliente = this.GetNomeCliente,
                           }
                       });
                }

                return gDadosCorretagem;
            }
        }

        private ConsultarEntidadeCadastroResponse<ResumoDoClienteCarteiraInfo> GetDadosCarteira
        {
            get
            {
                if (null == gDadosCarteira)
                {
                    var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetCdAssessor);

                    gDadosCarteira = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ResumoDoClienteCarteiraInfo>(
                        new ConsultarEntidadeCadastroRequest<ResumoDoClienteCarteiraInfo>()
                        {
                            EntidadeCadastro = new ResumoDoClienteCarteiraInfo()
                            {
                                ConsultaCodigoAssessor = lListaAssessores,
                                ConsultaCodigoCliente = this.GetCodigoCliente,
                                ConsultaNomeCliente = this.GetNomeCliente,
                            }
                        });
                }

                return gDadosCarteira;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                if (this.Acao == "BuscarItensParaListagemSimples")
                {
                    this.ResponderBuscarItensParaListagemSimples();
                }
                else if (this.Acao == "CarregarComoCSV")
                {
                    this.ResponderArquivoCSV();
                }
                else if (this.Acao == "BuscarParte")
                {
                    this.Response.Clear();

                    string lResponse = base.RetornarSucessoAjax("Dados carregados com sucesso.");

                    this.Response.Write(lResponse);

                    this.Response.End();
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax(ex.ToString());
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            gDadosCorretagem = null;
            gDadosCadastrais = null;
            gDadosCarteira = null;

            this.CarregarDadosCadastrais();

            this.CarregarCorretagem();

            this.CarregarCarteiras();
        }

        private void ResponderArquivoCSV()
        {
            var lConteudoArquivo = new StringBuilder();
            lConteudoArquivo.Append("Nome do cliente\tData de cadastro\tData da última operação\tTipo\te-Mail\tLogradouro\tNúmero\tComplemento\tBairro\tCidade\tEstado\tTelefone\tCelular 1\tCelular 2\n");

            if (this.GetDadosCadastrais.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_003_DadosCadastrais(this.GetDadosCadastrais.EntidadeCadastro);

                lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t"
                    , lTransporte.NomeCliente, lTransporte.DataDeCadastro, lTransporte.DataUltimaOperacao, lTransporte.Tipo, lTransporte.Email, lTransporte.Logradouro
                    , lTransporte.Numero, lTransporte.Complemento, lTransporte.Bairro, lTransporte.Cidade, lTransporte.Estado, lTransporte.Telefone, lTransporte.Celular1, lTransporte.Celular2);
            }

            lConteudoArquivo.Append("\n\rCorretagem no mês (R$)\tVolume no mês (R$)\tCorretagem média no ano (R$)\tVolume média no ano (R$)\tCorretagem em 12 meses (R$)\tVolume em 12 meses (R$)\tDisponível em Conta Corrente\t\r");

            if (this.GetDadosCorretagem.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_003_Corretagem(this.GetDadosCorretagem.EntidadeCadastro);

                lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\r"
                    , lTransporte.CorretagemNoMes, lTransporte.VolumeNoMes, lTransporte.CorretagemMediaNoAno, lTransporte.VolumeMediaNoAno, lTransporte.CorretagemEm12Meses, lTransporte.VolumeEm12Meses, lTransporte.ContaCorrenteDisponivel);
            }

            if (null != this.GetDadosCarteira
            && (null != this.GetDadosCarteira.Resultado)
            && (this.GetDadosCarteira.StatusResposta == MensagemResponseStatusEnum.OK))
            {
                var lTransporte = new TransporteRelatorio_003_Carteira().TraduzirLista(this.GetDadosCarteira.Resultado);

                lConteudoArquivo.AppendLine("\r\nPosição em cateira\t");
                lConteudoArquivo.Append("Carteira\tR$\tQuantidade\t\r");
                lTransporte.ForEach(car =>
                {
                    lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t\r\n", car.Carteira, car.Valor, car.Quantidade);
                });
            }

            this.Response.Clear();

            this.Response.ContentType = "text/xls";

            this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            this.Response.Charset = "iso-8859-1";

            this.Response.AddHeader("content-disposition", "attachment;filename=ResumoDoCliente.xls");

            this.Response.Write(lConteudoArquivo.ToString());

            this.Response.End();
        }

        #endregion

        #region | Métodos de apoio

        private void CarregarDadosCadastrais()
        {
            if (null != this.GetDadosCadastrais
            && (null != this.GetDadosCadastrais.EntidadeCadastro)
            && (!string.IsNullOrWhiteSpace(this.GetDadosCadastrais.EntidadeCadastro.NmCliente))
            && (this.GetDadosCadastrais.StatusResposta == MensagemResponseStatusEnum.OK))
            {
                this.divNenhumClienteEncontrado.Visible = false;
                this.divDadosCadastrais.Visible = true;

                var lTransporte = new TransporteRelatorio_003_DadosCadastrais(this.GetDadosCadastrais.EntidadeCadastro);

                this.Estado = lTransporte.Estado;
                this.Cidade = lTransporte.Cidade;
                this.Tipo = lTransporte.Tipo;
                this.DataUltimaOperacao = lTransporte.DataUltimaOperacao;
                this.DataDeCadastro = lTransporte.DataDeCadastro;
                this.NomeCliente = lTransporte.NomeCliente;
                this.Logradouro = lTransporte.Logradouro;
                this.Numero = lTransporte.Numero;
                this.Complemento = lTransporte.Complemento;
                this.Bairro = lTransporte.Bairro;
                this.Telefone = lTransporte.Telefone;
                this.Ramal = lTransporte.Ramal;
                this.Celular1 = lTransporte.Celular1;
                this.Celular2 = lTransporte.Celular2;
                this.Email = lTransporte.Email;
            }
            else
            {
                this.divNenhumClienteEncontrado.Visible = true;
                this.divDadosCadastrais.Visible = false;
            }
        }

        private void CarregarCorretagem()
        {
            if (this.GetDadosCorretagem.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_003_Corretagem(this.GetDadosCorretagem.EntidadeCadastro);

                this.CorretagemNoMes = lTransporte.CorretagemNoMes;
                this.CorretagemMediaNoAno = lTransporte.CorretagemMediaNoAno;
                this.CorretagemEm12Meses = lTransporte.CorretagemEm12Meses;
                this.VolumeNoMes = lTransporte.VolumeNoMes;
                this.VolumeMediaNoAno = lTransporte.VolumeMediaNoAno;
                this.VolumeEm12Meses = lTransporte.VolumeEm12Meses;
                this.ContaCorrenteDisponivel = lTransporte.ContaCorrenteDisponivel;
            }
        }

        private void CarregarCarteiras()
        {
            if (null != this.GetDadosCarteira && null != this.GetDadosCarteira.Resultado && this.GetDadosCarteira.Resultado.Count > 0)
            {
                this.rptDBM_ResumoDoCliente_Carteira.DataSource = new TransporteRelatorio_003_Carteira().TraduzirLista(this.GetDadosCarteira.Resultado);
                this.rptDBM_ResumoDoCliente_Carteira.DataBind();

                this.divDBM_ResumoDoCliente_Carteira_Nenhum.Visible = false;
                this.divDBM_ResumoDoCliente_Carteira.Visible = true;
            }
            else
            {
                this.divDBM_ResumoDoCliente_Carteira_Nenhum.Visible = true;
                this.divDBM_ResumoDoCliente_Carteira.Visible = false;
            }
        }

        #endregion
    }
}