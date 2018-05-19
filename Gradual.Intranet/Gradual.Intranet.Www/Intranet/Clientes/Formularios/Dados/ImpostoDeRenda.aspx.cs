using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Microsoft.Reporting.WebForms;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class ImpostoDeRenda : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int GetAno
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["Ano"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetCpfCnpj
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["cpfcnpj"]))
                    return null;

                return this.Request["cpfcnpj"].Replace(".", "").Replace("-", "");
            }
        }

        private string GetNome
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["nome"]))
                    return null;

                return this.Request["nome"].ToStringFormatoNome();
            }
        }

        private int GetIdCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["idcliente"], out lRetorno);

                return lRetorno;
            }
        }

        private bool GetGerarRelatorio
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request["GerarRelatorio"], out lRetorno);

                return lRetorno;
            }
        }

        private TransporteImpostoDeRenda GetTransporteImpostoDeRenda
        {
            get
            {
                return new TransporteImpostoDeRenda()
                {
                    Ano = this.GetAno.ToString(),
                    CpfCnpj = this.GetCpfCnpj,
                    IdCliente = this.GetIdCliente.ToString(),
                    Nome = this.GetNome,
                    NomeAcao = base.Acao
                };
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                     , "GerarComprovanteDayTrade"
                                                     , "GerarComprovanteOperacoes"
                                                     , "GerarComprovanteTesouroDireto"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderCarregarHtmlComDados
                                                     , this.ResponderGerarComprovanteDayTrade
                                                     , this.ResponderGerarComprovanteOperacoes
                                                     , this.ResponderGerarComprovanteTesouroDireto
                                                     });
        }

        #endregion

        #region | Métodos

        private string ResponderCarregarHtmlComDados()
        {
            var lListaAnos = new List<KeyValuePair<string, string>>();

            for (int y = DateTime.Now.Year - 1; y > DateTime.Now.Year - 6; y--)
                lListaAnos.Add(new KeyValuePair<string, string>(y.ToString(), y.ToString()));

            this.rptGradIntra_Clientes_ImpostoDeRenda_AnoVigencia.DataSource = lListaAnos;
            this.rptGradIntra_Clientes_ImpostoDeRenda_AnoVigencia.DataBind();

            return string.Empty;
        }

        private string ResponderGerarComprovanteDayTrade()
        {
            var lRetorno = string.Empty;

            try
            {
                this.GerarInforme("IRPF8468.rdlc", 8468);

                lRetorno = base.RetornarSucessoAjax(this.GetTransporteImpostoDeRenda, "Relatório gerado com sucesso.");
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax(ex.Message);
            }

            return lRetorno;
        }

        private string ResponderGerarComprovanteOperacoes()
        {
            var lRetorno = string.Empty;

            try
            {
                this.GerarInforme("IRPF5557.rdlc", 5557);

                lRetorno = base.RetornarSucessoAjax(this.GetTransporteImpostoDeRenda, "Relatório gerado com sucesso.");
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax(ex.Message);
            }

            return lRetorno;
        }

        private string ResponderGerarComprovanteTesouroDireto()
        {
            var lRetorno = string.Empty;

            try
            {
                this.GerarInforme8053("IRPF8053.rdlc");

                lRetorno = base.RetornarSucessoAjax(this.GetTransporteImpostoDeRenda, "Relatório gerado com sucesso.");
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax(ex.Message);
            }

            return lRetorno;
        }

        private bool GerarInforme(string pNomeArquivo, int pIdRelatorio)
        {
            int lCondicaoDeDependente = 1;
            var lClienteInfo = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(new ReceberEntidadeCadastroRequest<ClienteInfo>()
            {
                EntidadeCadastro = new ClienteInfo { IdCliente = this.GetIdCliente }
            });

            DateTime lDataNascimento = lClienteInfo.EntidadeCadastro.DtNascimentoFundacao.Value;

            var lEnderecoInfo = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteEnderecoInfo>(new ConsultarEntidadeCadastroRequest<ClienteEnderecoInfo>()
            {
                EntidadeCadastro = new ClienteEnderecoInfo() { IdCliente = this.GetIdCliente, StPrincipal = true, }
            });

            var lEnderecoPrincipal = lEnderecoInfo.Resultado.Find(end => { return end.StPrincipal; });

            var lInformeRendimentos = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<InformeRendimentosInfo>(new ConsultarEntidadeCadastroRequest<InformeRendimentosInfo>()
            {
                EntidadeCadastro = new InformeRendimentosInfo()
                {
                    ConsultaCondicaoDeDependente = lCondicaoDeDependente,
                    ConsultaCpfCnpj = this.GetCpfCnpj,
                    ConsultaDataFim = new DateTime(this.GetAno, 12, 31),
                    ConsultaDataInicio = new DateTime(this.GetAno, 1, 1),
                    ConsultaDataNascimento = lDataNascimento,
                    ConsultaTipoInforme = pIdRelatorio,
                }
            });

            if (null != lInformeRendimentos && null != lInformeRendimentos.Resultado && lInformeRendimentos.Resultado.Count > 0)
            {
                if (this.GetGerarRelatorio)
                {
                    string lNomeDoRelatorio = string.Format("IRPF_{0}_{1}", this.GetAno.ToString(), pIdRelatorio.ToString());
                    this.GerarRelatorio(this.GetAno, pNomeArquivo, lNomeDoRelatorio, lInformeRendimentos.Resultado, lEnderecoPrincipal);
                }
                return true;
            }
            else
            {
                if (pIdRelatorio == 5557)
                    throw new Exception("Você não possui Comprovante de Operações.");
                else
                    throw new Exception("Você não possui Comprovante de Day Trade.");
            }
        }

        private bool GerarInforme8053(string pNomeArquivo)
        {
            int lCondicaoDeDependente = 1;
            int lIdRelatorio = 8053;
            var lClienteInfo = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(new ReceberEntidadeCadastroRequest<ClienteInfo>()
            {
                EntidadeCadastro = new ClienteInfo { IdCliente = this.GetIdCliente }
            });

            DateTime lDataNascimento = lClienteInfo.EntidadeCadastro.DtNascimentoFundacao.Value;

            var lEnderecoCustodia = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteEnderecoDeCustodiaInfo>(new ReceberEntidadeCadastroRequest<ClienteEnderecoDeCustodiaInfo>()
            {
                EntidadeCadastro = new ClienteEnderecoDeCustodiaInfo()
                {
                    ConsultaCondicaoDeDePendente = lCondicaoDeDependente,
                    ConsultaCpfCnpj = this.GetCpfCnpj,
                    ConsultaDataDeNascimento = lDataNascimento
                }
            });

            var lInformeRendimentos = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<InformeRendimentosInfo>(new ConsultarEntidadeCadastroRequest<InformeRendimentosInfo>()
            {
                EntidadeCadastro = new InformeRendimentosInfo()
                {
                    ConsultaCondicaoDeDependente = lCondicaoDeDependente,
                    ConsultaCpfCnpj = this.GetCpfCnpj,
                    ConsultaDataFim = new DateTime(this.GetAno, 12, 31),
                    ConsultaDataInicio = new DateTime(this.GetAno, 1, 1),
                    ConsultaDataNascimento = lDataNascimento,
                    ConsultaTipoInforme = lIdRelatorio,
                }
            });

            var lInformeRendimentosTesouroDireto = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<InformeRendimentosTesouroDiretoInfo>(new ConsultarEntidadeCadastroRequest<InformeRendimentosTesouroDiretoInfo>()
            {
                EntidadeCadastro = new InformeRendimentosTesouroDiretoInfo()
                {
                    ConsultaAno = new DateTime(this.GetAno, 12, 31),
                    ConsultaAnoAnterior = new DateTime(this.GetAno - 1, 12, 31),
                    ConsultaCondicaoDeDependente = lCondicaoDeDependente,
                    ConsultaCpfCnpj = this.GetCpfCnpj,
                    ConsultaDataNascimento = lDataNascimento,
                }
            });

            if (lInformeRendimentosTesouroDireto.Resultado.Count > 0 || lInformeRendimentos.Resultado.Count > 0)
            {
                if (this.GetGerarRelatorio)
                {
                    string lNomeDoRelatorio = string.Format("IRPF_{0}_{1}", this.GetAno.ToString(), lIdRelatorio.ToString());

                    this.GerarRelatorio(this.GetAno, pNomeArquivo, lNomeDoRelatorio, lInformeRendimentos.Resultado, lInformeRendimentosTesouroDireto.Resultado, new ClienteEnderecoInfo(lEnderecoCustodia.EntidadeCadastro));
                }
                return true;
            }
            else
            {
                throw new Exception("Você não possui Comprovante de Rendimentos no Tesouro Direto.");
            }
        }

        private void GerarRelatorio(int ano, string pathReport, string fileName, List<InformeRendimentosInfo> pInformeRendimentos, ClienteEnderecoInfo pEndereco)
        {
            this.GerarRelatorio(ano, pathReport, fileName, pInformeRendimentos, null, pEndereco);
        }

        private void GerarRelatorio(int ano, string pathReport, string pNomeArquivo, List<InformeRendimentosInfo> pInformeRendimentos, List<InformeRendimentosTesouroDiretoInfo> pRendimentoTesouro, ClienteEnderecoInfo pEndereco)
        {
            LocalReport lLocalReport = new LocalReport();
            //Endereço
            lLocalReport.ReportPath = this.Server.MapPath(string.Concat(@"~\Extras\RelatoriosIR\", pathReport));
            //Parametro
            List<ReportParameter> parametros = new List<ReportParameter>();

            parametros.Add(new ReportParameter("pAno", ano.ToString()));

            parametros.Add(new ReportParameter("pNome", this.GetNome));

            parametros.Add(new ReportParameter("pCpf", this.GetCpfCnpj));

            parametros.Add(new ReportParameter("pCep", string.Format("{0}-{1}", pEndereco.NrCep, pEndereco.NrCepExt)));

            parametros.Add(new ReportParameter("pEndereco", string.Format("{0}           {1}           {2}", pEndereco.DsLogradouro, pEndereco.DsNumero, pEndereco.DsComplemento)));

            parametros.Add(new ReportParameter("pBairro", string.Format("{0}           {1}           {2}", pEndereco.DsBairro, pEndereco.DsCidade, pEndereco.CdUf)));
            //rpt.SetParameters(parametros); em baixo...
            //Source
            //rpt.DataSources.Clear();

            ReportDataSource rds = new ReportDataSource("InformeRendimentosInfo", pInformeRendimentos);
            lLocalReport.DataSources.Add(rds);

            if (pathReport.Contains("8053"))
            {
                ReportDataSource rdsTesouro = new ReportDataSource("InformeRendimentosTesouroDiretoInfo", pRendimentoTesouro);
                lLocalReport.DataSources.Add(rdsTesouro);

                parametros.Add(new ReportParameter("pAnoAtual", string.Concat("31/12/", ano.ToString())));
                parametros.Add(new ReportParameter("pAnoAnterior", string.Concat("31/12/", (ano - 1).ToString())));
            }

            lLocalReport.SetParameters(parametros);
            //rpt.DataBind();

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string lCaminhoArquivo = this.Server.MapPath(string.Concat(@"~\Extras\RelatoriosIR\", pNomeArquivo));

            //The DeviceInfo settings should be changed based on the reportType //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            //string deviceInfo =
            //"<deviceinfo> <outputformat>PDF</OutputFormat> <PageWidth>8.5in</PageWidth> <PageHeight>11in</PageHeight> <MarginTop>0.5in</MarginTop> <MarginLeft>1in</MarginLeft> <MarginRight>1in</MarginRight> <MarginBottom>0.5in</MarginBottom> </DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes = lLocalReport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Clear the response stream and write the bytes to the outputstream  //Set content-disposition to "attachment" so that user is prompted to take an action  //on the file (open or save)

            base.RegistrarLogConsulta(new LogIntranetInfo() { IdClienteAfetado = this.GetIdCliente, DsObservacao = string.Concat("Gerado para ação: ", base.Acao) });

            this.Response.Buffer = true;
            this.Response.Clear();
            this.Response.ContentType = mimeType;
            this.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.{1}", lCaminhoArquivo, fileNameExtension));
            this.Response.BinaryWrite(renderedBytes);
            this.Response.Flush();
            this.Response.End();
        }

        #endregion
    }
}