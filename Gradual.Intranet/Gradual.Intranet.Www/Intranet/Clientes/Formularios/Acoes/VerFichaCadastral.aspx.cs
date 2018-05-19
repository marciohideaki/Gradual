using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Views;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.Servico.FichaCadastral.Lib;
using System.Configuration;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class VerFichaCadastral : PaginaBaseAutenticada
    {
        #region | Propriedades

        private Boolean gAtualizaPasso;

        private int GetIdCliente
        {
            get
            {
                var lRetorno = default(int);

                if(!int.TryParse(this.Request.Form["Id"], out lRetorno))
                    int.TryParse(this.Request["Id"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodigoBovespa
        {
            get
            {
                var lRetorno = default(int);

                if (int.TryParse(this.Request.Form["CodBov"], out lRetorno))
                    return lRetorno;

                int.TryParse(this.Request.Form["CodBMF"], out lRetorno);

                return lRetorno;
            }
        }

        private bool TemFichaCambio {get;set; }
        private string DesejaAplicar { get; set; }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                , "ImprimirFichaDuc_PF"
                                                , "ImprimirFichaDuc_PJ"
                                                , "VisualizarTermo_PF"
                                                , "VisualizarTermo_PJ"
                                                , "ImprimirFichaCambio_PF"
                                                , "ImprimirFichaCambio_PJ"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados 
                                                , ImprimirFichaDucPF
                                                , ImprimirFichaDucPJ
                                                , ResponderVisualizarTermo_PF
                                                , ResponderVisualizarTermo_PJ
                                                , ImprimirFichaCambioPF
                                                , ImprimirFichaCambioPJ
                                                });
        }

        #endregion

        #region | Métodos
        private string ResponderVisualizarTermo_PF()
        {
            string lRetorno = string.Empty;

            try
            {
                if (this.GetIdCliente != 0)
                {
                    string lNomeArquivoFichaCadastral = string.Empty;

                    IServicoFichaCadastral lServico = Ativador.Get<IServicoFichaCadastral>();

                    string lURL = "../../Extras/Files/Adesao_Contrato_Intermediacao_20jun13.doc";// "../../Resc/PDFs/Adesao_Contrato_Intermediacao_20jun13.doc";

                    ReceberEntidadeRequest<TermoAdesaoGradualInfo> lRequestTermo = new ReceberEntidadeRequest<TermoAdesaoGradualInfo>();
                    OMS.Persistencia.ReceberObjetoResponse<TermoAdesaoGradualInfo> lResponseTermo;

                    lRequestTermo.Objeto = new TermoAdesaoGradualInfo();

                    lRequestTermo.Objeto.IdCliente = GetIdCliente;
                    lRequestTermo.Objeto.SitemaOrigem = SistemaOrigem.Intranet;

                    lResponseTermo = lServico.GerarTermoDeAdesao(lRequestTermo);

                    if (lResponseTermo.Objeto != null)
                    {
                        //if (this.Request.Url.OriginalString.ToLower().Contains("192.168") || this.Request.Url.OriginalString.ToLower().Contains("localhost")) //--> Desenvolvimento
                        //    lURL = string.Format("{0}://{1}/Intranet/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResponseTermo.Objeto.PathDownloadPdf);
                        //else                                                                         //---> Produção
                        //    lURL = string.Format("{0}://{1}/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResponseTermo.Objeto.PathDownloadPdf);
                        
                        if (this.Request.Url.OriginalString.ToLower().Contains("192.168") || this.Request.Url.OriginalString.ToLower().Contains("localhost")) //--> Desenvolvimento
                            lURL = string.Format("{0}://{1}/Intranet/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResponseTermo.Objeto.PathDownloadPdf);
                        else                                                                         //---> Produção
                            lURL = string.Format("{0}://{1}/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResponseTermo.Objeto.PathDownloadPdf);
                        
                        Session["ArquivoTermo"] = lURL;
                    }


                    lRetorno = RetornarSucessoAjax(1, string.Format("{0}", lURL));
                }
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro em ResponderSalvarPasso4()", ex);
            }

            return lRetorno;
        }

        private string ResponderVisualizarTermo_PJ()
        {
            string lRetorno = string.Empty;

            try
            {
                if (this.GetIdCliente != 0)
                {
                    IServicoFichaCadastral lServico = Ativador.Get<IServicoFichaCadastral>();

                    ReceberEntidadeRequest<FichaCadastralGradualInfo> lRequest = new ReceberEntidadeRequest<FichaCadastralGradualInfo>();
                    OMS.Persistencia.ReceberObjetoResponse<FichaCadastralGradualInfo> lResponse;

                    lRequest.Objeto = new FichaCadastralGradualInfo();

                    lRequest.Objeto.IdCliente = this.GetIdCliente;
                    lRequest.Objeto.SitemaOrigem = SistemaOrigem.Portal;

                    lResponse = lServico.GerarFichaCadastralPF(lRequest);

                    Session["ArquivoFicha"] = Session["ArquivoTermo"] = "";

                    string lNomeArquivoFichaCadastral = string.Empty;

                    if (lResponse.Objeto != null)
                    {
                        lNomeArquivoFichaCadastral = string.Format("~/Extras/Files/{1}", lResponse.Objeto.PathFichaCadastral);

                        Session["ArquivoFicha"] = string.Format("~/Resc/PDFs/FichaCadastral/{0}", lResponse.Objeto.PathFichaCadastral);
                    }

                    string lURL = "~/Extras/Files/Adesao_Contrato_Intermediacao_20jun13.doc";// "../../Resc/PDFs/Adesao_Contrato_Intermediacao_20jun13.doc";

                    ReceberEntidadeRequest<TermoAdesaoGradualInfo> lRequestTermo = new ReceberEntidadeRequest<TermoAdesaoGradualInfo>();
                    OMS.Persistencia.ReceberObjetoResponse<TermoAdesaoGradualInfo> lResponseTermo;

                    lRequestTermo.Objeto = new TermoAdesaoGradualInfo();

                    lRequestTermo.Objeto.IdCliente = GetIdCliente;
                    lRequestTermo.Objeto.SitemaOrigem = SistemaOrigem.Intranet;

                    lResponseTermo = lServico.GerarTermoDeAdesao(lRequestTermo);

                    if (lResponseTermo.Objeto != null)
                    {
                        
                        //if (this.Request.Url.OriginalString.ToLower().Contains("192.168")) //--> Desenvolvimento
                        //    lURL = string.Format("{0}://{1}/../Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResponseTermo.Objeto.PathDownloadPdf);
                        //else                                                                         //---> Produção
                        //    lURL = string.Format("{0}://{1}/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResponseTermo.Objeto.PathDownloadPdf);

                        if (this.Request.Url.OriginalString.ToLower().Contains("192.168") || this.Request.Url.OriginalString.ToLower().Contains("localhost")) //--> Desenvolvimento
                            lURL = string.Format("{0}://{1}/Intranet/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResponseTermo.Objeto.PathDownloadPdf);
                        else                                                                         //---> Produção
                            lURL = string.Format("{0}://{1}/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResponseTermo.Objeto.PathDownloadPdf);
                        //lURL = string.Format("~/Extras/Files/{0}", lResponseTermo.Objeto.PathDownloadPdf);

                        //Session["ArquivoTermo"] = string.Format("~/Resc/PDFs/FichaCadastral/{0}", lResponseTermo.Objeto.PathDownloadPdf);
                        Session["ArquivoTermo"] = lURL;
                    }

                    //EnviarEmailPasso4();

                    lRetorno = RetornarSucessoAjax(1, string.Format("{0},{1}", lNomeArquivoFichaCadastral, lURL));
                }
                //else
                //{
                //    lRetorno = RetornarErroAjax("");
                //}
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro em ResponderSalvarPasso4()", ex);
            }

            return lRetorno;
        }

        private string ResponderCarregarHtmlComDados()
        {
            var req = new ReceberEntidadeCadastroRequest<ViewFichaCadastralCompletaInfo>()
            {
                EntidadeCadastro = new ViewFichaCadastralCompletaInfo()
                {
                    IdDoCliente = this.GetIdCliente
                },
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            ViewFichaCadastralCompletaInfo lFicha = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ViewFichaCadastralCompletaInfo>(req).EntidadeCadastro;

            bool blnPessoaFisica = lFicha.DadosBasicos_TipoPessoa.Equals("F") ? true : false;

            this.ControleVisualizacaoFisicaJuridica(blnPessoaFisica);

            this.lblDadosBasicos_NomeCompleto.Text = lFicha.DadosBasicos_NomeCompleto;
            this.lblDadosBasicos_Email.Text = lFicha.DadosBasicos_Email;
            this.lblDadosBasicos_CodigoDUC.Text = lFicha.DadosBasicos_CodigoDUC;
            this.lblDadosBasicos_DataDoCadastro.Text = lFicha.DadosBasicos_DataDoCadastro;
            this.lblDadosBasicos_DataDeNascimento.Text = lFicha.DadosBasicos_DataDeNascimento;
            this.lblDadosBasicos_EstadoDeNascimento.Text = lFicha.DadosBasicos_EstadoDeNascimento;
            this.lblDadosBasicos_CidadeDeNascimento.Text = lFicha.DadosBasicos_CidadeDeNascimento;
            this.lblDadosBasicos_NomeDoPai.Text = lFicha.DadosBasicos_NomeDoPai;
            this.lblDadosBasicos_NomeDaMae.Text = lFicha.DadosBasicos_NomeDaMae;
            this.lblDadosBasicos_Sexo.Text = lFicha.DadosBasicos_Sexo;
            this.lblDadosBasicos_EstadoCivil.Text = lFicha.DadosBasicos_EstadoCivil;
            this.lblDadosBasicos_Conjuge.Text = lFicha.DadosBasicos_Conjuge;
            this.lblDadosBasicos_CPF.Text = lFicha.DadosBasicos_CPF;
            this.lblDadosBasicos_TipoDeDocumento.Text = lFicha.DadosBasicos_TipoDeDocumento;
            this.lblDadosBasicos_OrgaoUfDeEmissao.Text = lFicha.DadosBasicos_OrgaoUfDeEmissao;
            this.lblDadosBasicos_DesejaAplicar.Text = lFicha.DadosBasicos_DesejaAplicar;
            this.lblDadosBasicos_Numero.Text = lFicha.DadosBasicos_Numero;
            this.lblDadosBasicos_DataDeEmissao.Text = lFicha.DadosBasicos_DataDeEmissao;
            this.lblDados_Contas.Text = lFicha.Dados_Contas;
            this.lblInformacoesComerciais_Empresa.Text = lFicha.InformacoesComerciais_Empresa;
            this.lblInformacoesComerciais_Profissao.Text = lFicha.InformacoesComerciais_Profissao;
            this.lblInformacoesComerciais_CargoAtualOuFuncao.Text = lFicha.InformacoesComerciais_CargoAtualOuFuncao;
            this.lblInformacoesComerciais_Email.Text = lFicha.InformacoesComerciais_Email;
            this.lblDadosParaContato_Enderecos.Text = lFicha.DadosParaContato_Enderecos;//.Replace(Environment.NewLine, "<br />");
            this.lblDadosParaContato_Telefones.Text = lFicha.DadosParaContato_Telefones;
            this.lblDadosBancarios_Contas.Text = lFicha.DadosBancarios_Contas;
            this.lblInformacoesPatrimoniais_Salario.Text = lFicha.InformacoesPatrimoniais_Salario;
            this.lblInformacoesPatrimoniais_OutrosRendimentos.Text = lFicha.InformacoesPatrimoniais_OutrosRendimentos;
            this.lblInformacoesPatrimoniais_TotalDeOutrosRendimentos.Text = lFicha.InformacoesPatrimoniais_TotalDeOutrosRendimentos;
            this.lblInformacoesPatrimoniais_BensImoveis.Text = lFicha.InformacoesPatrimoniais_BensImoveis;
            this.lblInformacoesPatrimoniais_BensMoveis.Text = lFicha.InformacoesPatrimoniais_BensMoveis;
            this.lblInformacoesPatrimoniais_Aplicacoes.Text = lFicha.InformacoesPatrimoniais_Aplicacoes;
            this.lblInformacoesPatrimoniais_DeclaracoesEAutorizacoes.Text = lFicha.InformacoesPatrimoniais_DeclaracoesEAutorizacoes;
            this.lblProcuradores_Nome.Text = lFicha.DadosBasicos_Representantes;
            this.lblDiretores_Nome.Text = lFicha.DadosBasicos_Diretor;
            this.lblControladores_Nome.Text = lFicha.DadosBasicos_Controladora;
            //lblDadosBasicos_LiberadoParaOperar.Text = lFicha.DadosBasicos_LiberadoParaOperar;

            if (lFicha.DadosBasicos_DesejaAplicar == "FUNDOS")
            {
                pnlBotoes.Visible = false;
            }

            return string.Empty;
        }

        private string ValidaClienteCompleto()
        {
            var lRetorno = new StringBuilder();
            int lIdCliente = this.GetIdCliente;

            //Ver assessor Inicial
            ReceberEntidadeCadastroResponse<ClienteInfo> lCliente =
                this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(
                    new ReceberEntidadeCadastroRequest<ClienteInfo>()
                    {
                        EntidadeCadastro = new ClienteInfo() { IdCliente = lIdCliente }
                    });
            
            if(lCliente.EntidadeCadastro.TpDesejaAplicar.ToUpper().Contains("CAM") || lCliente.EntidadeCadastro.TpDesejaAplicar.ToUpper().Contains("TODAS") || lCliente.EntidadeCadastro.TpDesejaAplicar.ToUpper().Contains("TODOS"))
            {
                TemFichaCambio = true;
            }

            DesejaAplicar = lCliente.EntidadeCadastro.TpDesejaAplicar.ToUpper();

            if (lCliente.EntidadeCadastro.StPasso < 4 && (lCliente.EntidadeCadastro.IdAssessorInicial == null || lCliente.EntidadeCadastro.IdAssessorInicial == 0))
                lRetorno.Append("\n\tO Cliente Precisa ter um Assessor Inicial Cadastrado;");

            ClienteBancoInfo lBancoPrincipal = Gradual.Intranet.Servicos.BancoDeDados.Persistencias.ClienteDbLib.GetClienteBancoPrincipal(lCliente.EntidadeCadastro);

            ClienteEnderecoInfo lEnderecoPrincipal = Gradual.Intranet.Servicos.BancoDeDados.Persistencias.ClienteDbLib.GetClienteEnderecoPrincipal(lCliente.EntidadeCadastro);

            ClienteTelefoneInfo lTelefonePrincipal = Gradual.Intranet.Servicos.BancoDeDados.Persistencias.ClienteDbLib.GetClienteTelefonePrincipal(lCliente.EntidadeCadastro);

            //Telefone
            if (null == lTelefonePrincipal || null == lTelefonePrincipal.DsNumero || lTelefonePrincipal.DsNumero.Trim().Length == 0)
                lRetorno.Append("\n\tÉ Necessário Cadastrar um Telefone Principal para o Cliente;");

            //Endereço
            if (null == lEnderecoPrincipal || null == lEnderecoPrincipal.DsLogradouro || lEnderecoPrincipal.DsLogradouro.Trim().Length == 0)
                lRetorno.Append("\n\tÉ Necessário Cadastrar um Endereço Principal para o Cliente;");

            //Conta Bancária
            if (null == lBancoPrincipal || null == lBancoPrincipal.DsAgencia || lBancoPrincipal.DsAgencia.Trim().Length == 0)
                lRetorno.Append("\n\tÉ Necessário Cadastrar uma Conta Bancária Principal para o Cliente.");

            return lRetorno.ToString();
        }

        private string ImprimirFichaCambioPF()
        {
            return ImprimirFichaDucPF(true);
        }

        private string ImprimirFichaDucPF()
        {
            return ImprimirFichaDucPF(false);
        }
        
        private string ImprimirFichaDucPF(bool EhCambio)
        {
            try
            {
                //Sempre haverá pendêcias, pois o Serasa ainda não foi consultado, o comprovante de endereço não foi enviado, etc...
                //if (this.PossuiPendenciasCadastrais())
                //    return base.RetornarErroAjax("Não é possível gerar a ficha cadastral, pois o cliente possui pendências cadastrais ativas.");

                string lValidaCliente = this.ValidaClienteCompleto();
                if (lValidaCliente.Trim().Length > 0)
                {
                    return base.RetornarErroAjax(string.Concat("Existem Pendências Cadastrais:\n", lValidaCliente));
                }

                var lAtivador = Ativador.Get<IServicoFichaCadastral>();

                var lResposta = lAtivador.GerarFichaCadastralPF(new ReceberEntidadeRequest<FichaCadastralGradualInfo>()
                {
                    Objeto = new FichaCadastralGradualInfo()
                    {
                        IdCliente = this.GetIdCliente,
                        SitemaOrigem = SistemaOrigem.Intranet,
                    }
                });

                //var lResposta =
                //    this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteFichaDucPFInfo>(
                //        new ReceberEntidadeCadastroRequest<ClienteFichaDucPFInfo>()
                //        {
                //            EntidadeCadastro = new ClienteFichaDucPFInfo()
                //            {
                //                IdCliente = this.GetIdCliente,
                //                PathImages = @"..\..\Skin\Default\Img\",
                //                PathImagesCadastro = @"..\..\Skin\Default\Img\",
                //                PathPDF = this.Server.MapPath("../../../../Extras/FichaDuc"),
                //                ServerMapPath = this.Server.MapPath("../../../../Extras/FichaDuc"),
                //                CdBovespa = this.GetCodigoBovespa,
                //            },
                //            DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                //            IdUsuarioLogado = base.UsuarioLogado.Id
                //        });

                if (null == lResposta)// || MensagemResponseStatusEnum.ErroPrograma.Equals(lResposta.StatusResposta))
                    return base.RetornarErroAjax("Houve um erro ao tentar gerar a ficha cadastral. Tente novamente em alguns instantes.");

                var lLinkPdf = string.Empty;

                if (EhCambio)
                {
                    //if (this.Request.Url.OriginalString.ToLower().Contains("192.168")) //--> Desenvolvimento
                    //    lLinkPdf = string.Format("{0}://{1}/Intranet/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResposta.Objeto.PathFichaCadastralCambio);
                    //else                                                                         //---> Produção
                    //    lLinkPdf = string.Format("{0}://{1}/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResposta.Objeto.PathFichaCadastralCambio);

                    if (this.Request.Url.OriginalString.ToLower().Contains("192.168") || this.Request.Url.OriginalString.ToLower().Contains("localhost")) //--> Desenvolvimento
                        lLinkPdf = string.Format("{0}://{1}/Intranet/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResposta.Objeto.PathFichaCadastralCambio);
                    else                                                                         //---> Produção
                        lLinkPdf = string.Format("{0}://{1}/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResposta.Objeto.PathFichaCadastralCambio);
                }
                else
                {
                    //if (this.Request.Url.OriginalString.ToLower().Contains("192.168")) //--> Desenvolvimento
                    //    lLinkPdf = string.Format("{0}://{1}/Intranet/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResposta.Objeto.PathFichaCadastral);
                    //else                                                                         //---> Produção
                    //    lLinkPdf = string.Format("{0}://{1}/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResposta.Objeto.PathFichaCadastral);

                    if (this.Request.Url.OriginalString.ToLower().Contains("192.168") || this.Request.Url.OriginalString.ToLower().Contains("localhost")) //--> Desenvolvimento
                        lLinkPdf = string.Format("{0}://{1}/Intranet/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResposta.Objeto.PathFichaCadastral);
                    else                                                                         //---> Produção
                        lLinkPdf = string.Format("{0}://{1}/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResposta.Objeto.PathFichaCadastral);
                }


                if (!String.IsNullOrEmpty(lResposta.Objeto.PathFichaCadastralCambio))
                {
                    this.AtualizarPassoCadastral(lResposta.Objeto.PathFichaCadastral, lResposta.Objeto.PathFichaCadastralCambio, eTipoPessoa.Fisica);
                }
                else
                {
                    this.AtualizarPassoCadastral(lResposta.Objeto.PathFichaCadastral, eTipoPessoa.Fisica);
                }

                base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo()
                {   //--> Registrando o Log
                    IdClienteAfetado = this.GetIdCliente,
                    DsObservacao = "Ficha Cadastral PF"
                });

                return base.RetornarSucessoAjax(new { StatusAtualizaPasso = gAtualizaPasso }, lLinkPdf);
            }
            catch (Exception ex)
            {
                return base.RetornarErroAjax(ex.ToString());
            }
        }

        private string ImprimirFichaCambioPJ()
        {
            return ImprimirFichaDucPJ(true);
        }

        private string ImprimirFichaDucPJ()
        {
            return ImprimirFichaDucPJ(false);
        }

        private string ImprimirFichaDucPJ(bool EhCambio)
        {
            try
            {
                //Sempre haverá pendêcias, pois o Serasa ainda não foi consultado, o comprovante de endereço não foi enviado, etc...
                //if (this.PossuiPendenciasCadastrais())
                //    return base.RetornarErroAjax("Não é possível gerar a ficha cadastral, pois o cliente possui pendências cadastrais ativas.");
                string lValidaCliente = this.ValidaClienteCompleto();
                if (lValidaCliente.Trim().Length > 0)
                {
                    return base.RetornarErroAjax("Existem Pendências Cadastrais:\n" + lValidaCliente);
                }

                var lResposta = Ativador.Get<IServicoFichaCadastral>().GerarFichaCadastralPJ(new ReceberEntidadeRequest<FichaCadastralGradualInfo>()
                {
                    Objeto = new FichaCadastralGradualInfo()
                    {
                        IdCliente = this.GetIdCliente,
                        SitemaOrigem = SistemaOrigem.Intranet
                    }
                });

                //var lResposta = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<FichaCadastralGradualInfo>(
                //        new ReceberEntidadeCadastroRequest<FichaCadastralGradualInfo>()
                //        {
                //            EntidadeCadastro = new FichaCadastralGradualInfo()
                //            {
                //                IdCliente = this.GetIdCliente,
                //                PathImages = @"..\..\Skin\Default\Img\",
                //                PathImagesCadastro = @"..\..\Skin\Default\Img\",
                //                PathPDF = this.Server.MapPath("../../../../Extras/FichaDuc"),
                //                ServerMapPath = this.Server.MapPath("../../../../Extras/FichaDuc"),
                //                CdBovespa = this.GetCodigoBovespa,
                //            },
                //            DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                //            IdUsuarioLogado = base.UsuarioLogado.Id
                //        });

                if (null == lResposta) // || MensagemResponseStatusEnum.ErroPrograma.Equals(lResposta.Objeto.StatusResposta))
                    return base.RetornarErroAjax("Houve um erro ao tentar gerar a ficha cadastral. Tente novamente em alguns instantes.");

                var lLinkPdf = string.Empty;


                if (EhCambio)
                {
                    //if (this.Request.Url.OriginalString.ToLower().Contains("192.168")) //--> Desenvolvimento
                    //    lLinkPdf = string.Format("{0}://{1}/Intranet/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResposta.Objeto.PathFichaCadastralCambio);
                    //else                                                                         //---> Produção
                    //    lLinkPdf = string.Format("{0}://{1}/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResposta.Objeto.PathFichaCadastralCambio);

                    if (this.Request.Url.OriginalString.ToLower().Contains("192.168") || this.Request.Url.OriginalString.ToLower().Contains("localhost")) //--> Desenvolvimento
                        lLinkPdf = string.Format("{0}://{1}/Intranet/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResposta.Objeto.PathFichaCadastralCambio);
                    else                                                                         //---> Produção
                        lLinkPdf = string.Format("{0}://{1}/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResposta.Objeto.PathFichaCadastralCambio);
                }
                else
                {
                    //if (this.Request.Url.OriginalString.ToLower().Contains("192.168")) //--> Desenvolvimento
                    //    lLinkPdf = string.Format("{0}://{1}/Intranet/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResposta.Objeto.PathFichaCadastral);
                    //else                                                                         //---> Produção
                    //    lLinkPdf = string.Format("{0}://{1}/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResposta.Objeto.PathFichaCadastral);

                    //if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["FichaDeHomologacao"]))
                    //    lLinkPdf = string.Format("{0}://{1}/Intranet/Extras/FichaDuc/{2}", this.Request.Url.Scheme, this.Request.Url.Authority, lResposta.Objeto.PathFichaCadastral);

                    if (this.Request.Url.OriginalString.ToLower().Contains("192.168") || this.Request.Url.OriginalString.ToLower().Contains("localhost")) //--> Desenvolvimento
                        lLinkPdf = string.Format("{0}://{1}/Intranet/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResposta.Objeto.PathFichaCadastral);
                    else                                                                         //---> Produção
                        lLinkPdf = string.Format("{0}://{1}/{2}?Param={3}", this.Request.Url.Scheme, this.Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings["FileLoader"], lResposta.Objeto.PathFichaCadastral);
                }

                //this.AtualizarPassoCadastral(lResposta.Objeto.PathFichaCadastral, eTipoPessoa.Juridica);
                if (!String.IsNullOrEmpty(lResposta.Objeto.PathFichaCadastralCambio))
                {
                    this.AtualizarPassoCadastral(lResposta.Objeto.PathFichaCadastral, lResposta.Objeto.PathFichaCadastralCambio, eTipoPessoa.Juridica);
                }
                else
                {
                    this.AtualizarPassoCadastral(lResposta.Objeto.PathFichaCadastral, eTipoPessoa.Juridica);
                }

                base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = this.GetIdCliente, DsObservacao = "Ficha Cadastral PJ" });

                return base.RetornarSucessoAjax(new { StatusAtualizaPasso = gAtualizaPasso }, lLinkPdf);
            }
            catch (Exception ex)
            {
                return base.RetornarErroAjax(ex.ToString());
            }
        }

        private void AtualizarPassoCadastral(string pNomeFicha, eTipoPessoa pTipoPessoa)
        {
            AtualizarPassoCadastral(pNomeFicha, String.Empty, pTipoPessoa);
        }

        private void AtualizarPassoCadastral(string pNomeFicha, string pNomeFichaCambio, eTipoPessoa pTipoPessoa)
        {
            gAtualizaPasso = false;

            var lCliente = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(
                    new ReceberEntidadeCadastroRequest<ClienteInfo>()
                    {
                        EntidadeCadastro = new ClienteInfo()
                        {
                            IdCliente = this.GetIdCliente,
                        },
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    });

            if (lCliente.EntidadeCadastro.StPasso < 3)
            {
                gAtualizaPasso = true;

                lCliente.EntidadeCadastro.StPasso = 3;

                base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteInfo>(
                    new SalvarEntidadeCadastroRequest<ClienteInfo>()
                    {
                        EntidadeCadastro = lCliente.EntidadeCadastro,
                        IdUsuarioLogado = base.UsuarioLogado.Id,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome
                    });

                //Enviar a ficha e o contrato por email

                string lNomeContrato;
                if (pTipoPessoa == eTipoPessoa.Fisica)
                    lNomeContrato = "ContratoPF.PDF";
                else
                    lNomeContrato = "ContratoPJ-1501.pdf";

                if (!String.IsNullOrEmpty(pNomeFichaCambio))
                {
                    EnviarEmail(lCliente.EntidadeCadastro, pNomeFicha, pNomeFichaCambio, lNomeContrato);
                }
                else
                {
                    EnviarEmail(lCliente.EntidadeCadastro, pNomeFicha, lNomeContrato);
                }
            }
        }

        private void EnviarEmail(ClienteInfo pcliente, string pNomeFicha, string pNomeContrato)
        {
            EnviarEmail(pcliente, pNomeFicha, String.Empty, pNomeContrato);
        }

        private void EnviarEmail(ClienteInfo pcliente, string pNomeFicha, string pNomeFichaCambio, string pNomeContrato)
        {
            string PathFicha = this.Server.MapPath("../../../../Extras/FichaDuc/" + pNomeFicha);
            string PathContrato = this.Server.MapPath("../../../../Extras/Contratos/" + pNomeContrato);

            if (!string.Empty.Equals(pcliente.DsEmail))
            {
                var dicVariaveis = new Dictionary<string, string>();

                dicVariaveis.Add("@NomeCliente", pcliente.DsNome);

                var lEmail = "CadastroPasso3.htm";

                List<Gradual.OMS.Email.Lib.EmailAnexoInfo> lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();

                if (!DesejaAplicar.Equals("CAMBIO"))
                {
                    Gradual.OMS.Email.Lib.EmailAnexoInfo lFicha = new Gradual.OMS.Email.Lib.EmailAnexoInfo()
                    {
                        Nome = pNomeFicha,
                        Arquivo = File.ReadAllBytes(PathFicha)
                    };

                    lAnexos.Add(lFicha);
                }

                if (!String.IsNullOrEmpty(pNomeFichaCambio))
                {
                    string PathFichaCambio = this.Server.MapPath("../../../../Extras/FichaDuc/" + pNomeFichaCambio);

                    Gradual.OMS.Email.Lib.EmailAnexoInfo lContratoCambio = new Gradual.OMS.Email.Lib.EmailAnexoInfo()
                    {
                        Nome = pNomeFichaCambio,
                        Arquivo = File.ReadAllBytes(PathFichaCambio)
                    };

                    lAnexos.Add(lContratoCambio);
                }

                if (!DesejaAplicar.Equals("CAMBIO"))
                {

                    Gradual.OMS.Email.Lib.EmailAnexoInfo lContrato = new Gradual.OMS.Email.Lib.EmailAnexoInfo()
                    {
                        Nome = pNomeContrato,
                        Arquivo = File.ReadAllBytes(PathContrato)
                    };

                    lAnexos.Add(lContrato);
                }

                base.EnviarEmail(pcliente.DsEmail, "Complete seu cadastro na Gradual", lEmail, dicVariaveis, eTipoEmailDisparo.Assessor, lAnexos);
            }
        }

        private void ControleVisualizacaoFisicaJuridica(bool visible)
        {
            this.trNomeDoPai.Visible = visible;
            this.trNomeMae.Visible = visible;
            this.trSexo.Visible = visible;
            this.trEstadoCivil.Visible = visible;
            this.trConjugue.Visible = visible;
            this.trTipoDocumento.Visible = visible;
            this.trOrgaoEmissor.Visible = visible;
            this.trNumeroDocumento.Visible = visible;
            this.trDataEmissaoDocumento.Visible = visible;
        }

        #endregion

        public object IServicoFichaCadastral { get; set; }
    }
}