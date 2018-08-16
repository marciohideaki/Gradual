using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class SincronizarComSinacor : PaginaBaseAutenticada
    {
        #region | Atributos

        private const string gResultadoErroCadastro = "Cliente não Exportado por estar com pendências cadastrais. Veja os detalhes abaixo.";
        private const string gResultadoErroClieExterno = "Cliente não Exportado, pois a entrada de dados não foi aceita pelo Sinacor. Veja os detalhes abaixo.";
        private const string gResultadoOk = "Cliente Exportado com Sucesso!";
        private const string gResultadoOkParcial = "Cliente Exportado parcialmente para o Sinacor. Siga as instruções abaixo para completar a Exportação.";
        private const string gResultadoErroAtualizacao = "Os dados do cliente não foram atualizados corretamente no Sistema de Cadastro. Favor informar a equipe de sistemas para realizar a atualização manualmente.";
        private const string gResultadoErroComplementos = "Não foi possível realizar todas as atualizações no Sinacor. Os itens na tabela abaixo devem ser cadastrados manualmente no Sinacor.";
        private const string gResultadoErroRisco = "Os dados do cliente não foram atualizados corretamente no Sistema de Risco ou Controle de Acesso. Favor informar a equipe de sistemas para realizar a atualização manualmente.";
        private const string gTipoPendencia = "Pendência Cadastral";
        private const string gTipoCliExterno = "Entrada no Sinacor";
        private const string gTipoAtualizacao = "Atualização no Sistema de Cadastro";
        private const string gTipoComplementos = "Exportação de Complementos";
        private const string gTipoRisco = "Risco e Controle de Acesso";

        #endregion

        #region | Propriedades

        private int GetIdCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["Id"], out lRetorno);

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                , "SincronizarComSinacor"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados
                                                , ResponderSincronizarComSinacor
                                                });
        }

        #endregion

        #region | Métodos

        private string ResponderCarregarHtmlComDados()
        {
            var lPendencias = string.Empty;

            var lEntradaPendencia = new ReceberEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>();
            var lRetornoPendencia = new ReceberEntidadeCadastroResponse<TipoDePendenciaCadastralInfo>();

            var lEntradaPendenciaCliente = new ConsultarEntidadeCadastroRequest<ClientePendenciaCadastralInfo>()
            {
                EntidadeCadastro = new ClientePendenciaCadastralInfo()
                {
                    IdCliente = this.GetIdCliente
                }
            };

            var lRetornoPendenciaCliente = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePendenciaCadastralInfo>(lEntradaPendenciaCliente);

            if (lRetornoPendenciaCliente.StatusResposta != MensagemResponseStatusEnum.OK)
                return RetornarErroAjax("Erro ao verificar pendências cadastrais", lRetornoPendenciaCliente.DescricaoResposta);

            foreach (ClientePendenciaCadastralInfo item in lRetornoPendenciaCliente.Resultado)
            {   // Pegar apenas as sem baixa
                if (null == item.DtResolucao || item.DtResolucao.Value == DateTime.MinValue)
                {
                    lEntradaPendencia = new ReceberEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>();
                    lEntradaPendencia.EntidadeCadastro = new TipoDePendenciaCadastralInfo();
                    lEntradaPendencia.EntidadeCadastro.IdTipoPendencia = item.IdTipoPendencia;

                    lRetornoPendencia = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<TipoDePendenciaCadastralInfo>(lEntradaPendencia);

                    if (lRetornoPendencia.StatusResposta != MensagemResponseStatusEnum.OK)
                        return RetornarErroAjax("Erro ao verificar pendências cadastrais", lRetornoPendencia.DescricaoResposta);

                    lPendencias += string.Concat(lRetornoPendencia.EntidadeCadastro.DsPendencia, "<br/>");
                }
            }

            if (lPendencias.Trim().Length > 0)
            {
                //Exibir mensagem em Vermelho
                this.spnClientes_SincronizarComSinacor_PendenciaCadastral.InnerHtml = string.Concat("A exportação de clientes com pendências pode acarretar em problemas de auditoria.<br/>O cliente possui as pendências cadastrais descritas abaixo.<br />", lPendencias);
            }
            else
            {
                this.spnClientes_SincronizarComSinacor_PendenciaCadastral.InnerHtml = "";
            }

            return string.Empty;
        }

        private string ResponderSincronizarComSinacor()
        {
            var lResposta = string.Empty;
            var lPrimeira = true;
            var lCodigo = new Nullable<int>();

            //Pegar Cliente
            var lEntradaCliente = new ReceberEntidadeCadastroRequest<ClienteInfo>() { };
            var lRetornoCliente = new ReceberEntidadeCadastroResponse<ClienteInfo>();

            lEntradaCliente.EntidadeCadastro = new ClienteInfo();
            lEntradaCliente.EntidadeCadastro.IdCliente = this.GetIdCliente;
            lEntradaCliente.IdUsuarioLogado = base.UsuarioLogado.Id;
            lEntradaCliente.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

            lRetornoCliente = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro(lEntradaCliente);

            if (lRetornoCliente.EntidadeCadastro.StPasso < 3)
            {
                lResposta = RetornarErroAjax("É preciso gerar a ficha cadastral antes de exportar o cliente.");
            }
            else
            {
                //Passo e Conta
                if (lRetornoCliente.EntidadeCadastro.StPasso == 4)
                {
                    lPrimeira = false;

                    //Pegar o Códio Principal
                    var lEntradaConta = new ConsultarEntidadeCadastroRequest<ClienteContaInfo>();
                    var lRetornoConta = new ConsultarEntidadeCadastroResponse<ClienteContaInfo>();

                    lEntradaConta.EntidadeCadastro = new ClienteContaInfo();
                    lEntradaConta.EntidadeCadastro.IdCliente = lRetornoCliente.EntidadeCadastro.IdCliente.Value;
                    lEntradaConta.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                    lEntradaConta.IdUsuarioLogado = base.UsuarioLogado.Id;
                    lRetornoConta = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteContaInfo>(lEntradaConta);

                    List<ClienteContaInfo> lListaConta = lRetornoConta.Resultado;

                    foreach (ClienteContaInfo lConta in lListaConta)
                    {
                        if (null != lConta.StPrincipal && lConta.StPrincipal.Value)
                        {
                            lCodigo = lConta.CdCodigo;
                            break;
                        }
                    }
                }

                //Exportação
                var lEntradaExportacao = new SalvarEntidadeCadastroRequest<SinacorExportarInfo>();
                var lRetornoExportacao = new SalvarEntidadeCadastroResponse();

                lEntradaExportacao.EntidadeCadastro = new SinacorExportarInfo();
                lEntradaExportacao.EntidadeCadastro.Entrada = new SinacorExportacaoEntradaInfo();
                lEntradaExportacao.EntidadeCadastro.Entrada.IdCliente = lRetornoCliente.EntidadeCadastro.IdCliente.Value;
                lEntradaExportacao.EntidadeCadastro.Entrada.CdCodigo = lCodigo;
                lEntradaExportacao.EntidadeCadastro.Entrada.PrimeiraExportacao = lPrimeira;
                lEntradaExportacao.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                lEntradaExportacao.IdUsuarioLogado = base.UsuarioLogado.Id;
                lRetornoExportacao = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<SinacorExportarInfo>(lEntradaExportacao);

                //Retorno da Exportação
                var lTransporte = new TransporteSincronizacaoComSinacor()
                {
                    GridErro = new List<GridSincronizacao>()
                };

                if (lRetornoExportacao.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    SinacorExportarInfo lRetorno = (SinacorExportarInfo)lRetornoExportacao.Objeto;
                    GridSincronizacao lGrig;

                    if (!lRetorno.Retorno.DadosClienteOk)
                    {   //Erro na validação do Cliente
                        lTransporte.Resultado = gResultadoErroCadastro;
                        foreach (var item in lRetorno.Retorno.DadosClienteMensagens)
                        {
                            lGrig = new GridSincronizacao();
                            lGrig.Tipo = gTipoPendencia;
                            lGrig.Descricao = item.Mensagem;
                            lTransporte.GridErro.Add(lGrig);
                        }
                    }
                    else
                    {
                        if (!lRetorno.Retorno.ExportacaoSinacorOk)
                        {   //Erro no processo CliExterno
                            lTransporte.Resultado = gResultadoErroClieExterno;
                            foreach (var item in lRetorno.Retorno.ExportacaoSinacorMensagens)
                            {
                                lGrig = new GridSincronizacao();
                                lGrig.Tipo = gTipoCliExterno;
                                lGrig.Descricao = string.Concat(item.DS_OBS, " - ", item.DS_AUX);
                                lTransporte.GridErro.Add(lGrig);
                            }
                        }
                        else
                        {
                            //Cliente Exportado
                            //Verificação do Resultado:
                            if (lRetorno.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Retorno.ExportacaoComplementosOk)//Tudo OK
                                lTransporte.Resultado = gResultadoOk;
                            else //Com resalvas
                            {
                                lTransporte.Resultado = gResultadoOkParcial;

                                if (!lRetorno.Retorno.ExportacaoAtualizarCadastroOk) //Atualizacao OK
                                    lTransporte.Mensagens.Add(gResultadoErroAtualizacao);

                                if (!lRetorno.Retorno.ExportacaoRiscoOk) //Risco OK
                                    lTransporte.Mensagens.Add(gResultadoErroRisco);

                                if (!lRetorno.Retorno.ExportacaoComplementosOk) //Complementos OK
                                    lTransporte.Mensagens.Add(gResultadoErroComplementos);
                            }

                            //Montando o Grid
                            if (!lRetorno.Retorno.ExportacaoAtualizarCadastroOk)
                            {
                                foreach (var item in lRetorno.Retorno.ExportacaoAtualizarCadastroMensagens)
                                {
                                    lGrig = new GridSincronizacao();
                                    lGrig.Tipo = gTipoAtualizacao;
                                    lGrig.Descricao = item.Mensagem;
                                    lTransporte.GridErro.Add(lGrig);
                                }
                            }

                            if (!lRetorno.Retorno.ExportacaoRiscoOk)
                            {
                                foreach (var item in lRetorno.Retorno.ExportacaoRiscoMensagens)
                                {
                                    lGrig = new GridSincronizacao();
                                    lGrig.Tipo = gTipoRisco;
                                    lGrig.Descricao = item.Mensagem;
                                    lTransporte.GridErro.Add(lGrig);
                                }
                            }

                            if (!lRetorno.Retorno.ExportacaoComplementosOk)
                            {
                                foreach (var item in lRetorno.Retorno.ExportacaoComplementosMensagens)
                                {
                                    lGrig = new GridSincronizacao();
                                    lGrig.Tipo = gTipoComplementos;
                                    lGrig.Descricao = item.Mensagem;
                                    lTransporte.GridErro.Add(lGrig);
                                }
                            }

                            this.EnviarEmailCodigo(lRetorno, lRetornoCliente.EntidadeCadastro);

                        } // Fim Cliente Exportado
                    }
                }
                else
                {
                    lTransporte.Resultado = lRetornoExportacao.DescricaoResposta;
                    lResposta = RetornarErroAjax(lRetornoExportacao.DescricaoResposta);
                }

                lResposta = RetornarSucessoAjax(lTransporte, "Comunicação com o Sinacor realizada.");

                base.RegistrarLogAlteracao(new Contratos.Dados.Cadastro.LogIntranetInfo()
                {
                    IdClienteAfetado = this.GetIdCliente,
                    DsObservacao = string.Concat("Cliente sincronizado: id_cliente: ", this.GetIdCliente)
                });
            }
            return lResposta;
        }

        private int RetornaCodigoCliente(int CodigoCorretora, int CodigoCliente)
        {
            var valor = default(int);
            
            valor = (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5)
                  + (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4)
                  + (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3)
                  + (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2)
                  + (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9)
                  + (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8)
                  + (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7)
                  + (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6)
                  + (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5)
                  + (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4)
                  + (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3)
                  + (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            valor = valor % 11;

            if (valor == 0 || valor == 1)
                valor = 0;
            else
                valor = 11 - valor;

            return int.Parse(string.Format("{0}{1}", CodigoCliente, valor));
        }

        private void EnviarEmailCodigo(SinacorExportarInfo pRequestSinacor, ClienteInfo pRequestCliente)
        {
            if (pRequestSinacor.Retorno.ExportacaoSinacorOk && pRequestCliente.StPasso == 3)
            {   //--> Primeira exportação
                var lNome = pRequestCliente.DsNome; //--> Pegar Nome Cliente

                var lEntradaLogin = new ReceberEntidadeCadastroRequest<LoginInfo>()
                {   //--> Pegar Login
                    EntidadeCadastro = new LoginInfo()
                    {
                        IdLogin = pRequestCliente.IdLogin,
                    }
                };

                string lEmailDestinatario = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro(lEntradaLogin).EntidadeCadastro.DsEmail;

                if (lEmailDestinatario.Trim().Length > 0
                && (lEmailDestinatario.Trim().IndexOf('@') > -1)
                && (lNome.Trim().Length > 0)
                && (lEmailDestinatario.Trim().ToLower() != "a@a.a"))
                {
                    var dicVariaveis = new Dictionary<string, string>();

                    {   //--> Enviando e-mail para o cliente.
                        dicVariaveis.Add("@Nome", lNome);
                        dicVariaveis.Add("@Codigo", pRequestSinacor.Entrada.CdCodigo.ToString());

                        var lNomeAssessorInicial = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaComboInfo>(
                            new ConsultarEntidadeCadastroRequest<SinacorListaComboInfo>()
                            {   //-->Pegando o Assessor para ver se é TBC
                                IdUsuarioLogado = base.UsuarioLogado.Id,
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                EntidadeCadastro = new SinacorListaComboInfo()
                                {
                                    Informacao = eInformacao.Assessor,
                                    Filtro = pRequestCliente.IdAssessorInicial.Value.ToString()
                                }
                            });

                        var lTextoEmail = "CadastroPasso4-PrimeiraExportacao.htm";

                        if (MensagemResponseStatusEnum.OK.Equals(lNomeAssessorInicial.StatusResposta)
                        && (null != lNomeAssessorInicial)
                        && (null != lNomeAssessorInicial.Resultado)
                        && (lNomeAssessorInicial.Resultado.Count > 0)
                        && (null != lNomeAssessorInicial.Resultado[0])
                        && lNomeAssessorInicial.Resultado[0].Value.ToUpper().Contains("TBC"))
                        {
                            lTextoEmail = "CadastroPrimeiraExportacaoTBC.htm";
                        }
                        else if (MensagemResponseStatusEnum.OK.Equals(lNomeAssessorInicial.StatusResposta)
                             && (null != lNomeAssessorInicial)
                             && (null != lNomeAssessorInicial.Resultado)
                             && (lNomeAssessorInicial.Resultado.Count > 0)
                             && (null != lNomeAssessorInicial.Resultado[0])
                             && lNomeAssessorInicial.Resultado[0].Value.ToUpper().Contains("ESPACO DELA"))
                        {
                            lTextoEmail = "CadastroPrimeiraExportacaoEspacoDela.htm";
                        }

                        if(pRequestCliente.TpDesejaAplicar != "FUNDOS")
                        {
                            base.EnviarEmail(lEmailDestinatario, "Dados da sua conta na Gradual", lTextoEmail, dicVariaveis, eTipoEmailDisparo.Assessor);
                        }
                    }

                    {   //--> Enviando notificação para o assessor do cliente.
                        var lAssessor = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<AssessorDoClienteInfo>(
                            new ReceberEntidadeCadastroRequest<AssessorDoClienteInfo>()
                            {   //--> Enviando email para o Assessor
                                IdUsuarioLogado = base.UsuarioLogado.Id,
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                EntidadeCadastro = new AssessorDoClienteInfo()
                                {
                                    IdCliente = pRequestCliente.IdCliente.Value
                                }
                            });

                        dicVariaveis = new Dictionary<string, string>();
                        dicVariaveis.Add("@NomeCliente", lNome);
                        dicVariaveis.Add("@Codigo", pRequestSinacor.Entrada.CdCodigo.ToString());//.ToCodigoClienteFormatado());
                        dicVariaveis.Add("@Nome", lAssessor.EntidadeCadastro.NomeAssessor);

                        var lListaEmailAssessor = base.ConsultarListaEmailAssessor(lAssessor.EntidadeCadastro.CodigoAssessor).ListaEmailAssessor;

                        if (null != lListaEmailAssessor)
                            lListaEmailAssessor.ForEach(lDestinatario =>
                            {
                                base.EnviarEmail(lDestinatario, "Dados do seu cliente na Gradual", "CadastroPrimeiraExportacaoAssessor.htm", dicVariaveis, eTipoEmailDisparo.Assessor);
                            });
                    }
                }
            }
        }

        #endregion
    }
}
