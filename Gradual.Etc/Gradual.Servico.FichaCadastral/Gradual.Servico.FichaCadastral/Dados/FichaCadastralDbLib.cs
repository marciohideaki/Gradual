using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Views;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class FichaCadastralDbLib
    {
        #region | Atributos

        private static Hashtable LstEstadoCivil = new Hashtable();
        private static Hashtable LstPais = new Hashtable();
        private static Hashtable LstProfissao = new Hashtable();
        private static Hashtable LstAtividade = new Hashtable();
        private static Hashtable LstAssessor = new Hashtable();
        private static Hashtable LstTipoEndereco = new Hashtable();
        private static Hashtable LstContrato = new Hashtable();
        private static Hashtable LstTipoDocumento = new Hashtable();
        private static Hashtable LstTipoTelefone = new Hashtable();

        private static string gNomeConexaoSinacor = "";

        #endregion

        #region | Métodos Públicos

        /// <summary>
        /// Método public que retorna a View com as informações da visualização da ficha cadastral
        /// </summary>
        /// <param name="pParametros"></param>
        /// <returns></returns>
        public  ReceberObjetoResponse<ViewFichaCadastralCompletaInfo> ReceberViewFichaCadastralCompleta(ReceberEntidadeRequest<ViewFichaCadastralCompletaInfo> pParametros)
        {
            try
            {
                PreencheListasSinacor(eInformacao.EstadoCivil);
                PreencheTipoEndereco();
                PreencheContrato();
                PreencheListasSinacor(eInformacao.ProfissaoPF);
                PreencheListasSinacor(eInformacao.AtividadePJ);
                PreencheListasSinacor(eInformacao.TipoDocumento);
                PreencheTipoTelefone();

                return PreencheViewPessoaFisica(ReceberFichaEntidade(pParametros));
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<FichaCadastralInfo> ReceberFichaCadastral(ReceberEntidadeRequest<FichaCadastralInfo> pParametros)
        {
            try
            {
                var lParametro = new ReceberEntidadeRequest<ViewFichaCadastralCompletaInfo>()
                {
                    Objeto = new ViewFichaCadastralCompletaInfo()
                    {
                        IdDoCliente = pParametros.Objeto.IdCliente.DBToInt32()
                    }
                };

                return ReceberFichaEntidade(lParametro);
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        #endregion

        #region | Métodos de apoio

        /// <summary>
        /// Preenche a Lista de tipos de endereços
        /// </summary>
        private static void PreencheTipoEndereco()
        {
            ConsultarObjetosResponse<TipoEnderecoInfo> lRespostaGenerica =
              new TipoEnderecoDbLib().ConsultarTipoEndereco(new ConsultarEntidadeRequest<TipoEnderecoInfo>());
            LstTipoEndereco.Clear();
            lRespostaGenerica.Resultado.ForEach
                (delegate(TipoEnderecoInfo item) { LstTipoEndereco.Add(item.IdTipoEndereco.ToString(), item.DsEndereco.ToString()); }
            );
        }

        private static void PreencheTipoTelefone()
        {
            ConsultarObjetosResponse<TipoTelefoneInfo> lRespostaGenerica =
               new TipoTelefoneDbLib().ConsultarTipoTelefone(new ConsultarEntidadeRequest<TipoTelefoneInfo>());
            LstTipoTelefone.Clear();
            lRespostaGenerica.Resultado.ForEach
                (delegate(TipoTelefoneInfo item) { LstTipoTelefone.Add(item.IdTipoTelefone.ToString(), item.DsTelefone.ToString()); }
            );
        }

        /// <summary>
        /// Preenche contrato a lista de contratos
        /// </summary>
        private static void PreencheContrato()
        {
            ConsultarObjetosResponse<ContratoInfo> lRespostaGenerica = new ContratoDbLib().ConsultarContrato(new ConsultarEntidadeRequest<ContratoInfo>());
            LstContrato.Clear();
            lRespostaGenerica.Resultado.ForEach
                (delegate(ContratoInfo item) { LstContrato.Add(item.IdContrato.ToString(), item.DsContrato.ToString()); }
            );
        }

        /// <summary>
        /// Método de apoio para preenchimento das listas do sinacor
        /// </summary>
        private void PreencheListasSinacor(eInformacao eNum)
        {
            var lRespostaGenerica = ConsultarListaSinacor(
                new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eNum
                    }
                }
            );

            switch (eNum)
            {
                case eInformacao.Pais:
                    {
                        LstPais.Clear();
                        lRespostaGenerica.Resultado.ForEach(delegate(SinacorListaInfo item)
                        {
                            LstPais.Add(item.Id, item.Value);
                        });
                    }
                    break;
                case eInformacao.EstadoCivil:
                    {
                        LstEstadoCivil.Clear();
                        lRespostaGenerica.Resultado.ForEach(delegate(SinacorListaInfo item)
                        {
                            LstEstadoCivil.Add(item.Id, item.Value);
                        });
                    }
                    break;
                case eInformacao.ProfissaoPF:
                    {
                        LstProfissao.Clear();
                        lRespostaGenerica.Resultado.ForEach(delegate(SinacorListaInfo item)
                        {
                            LstProfissao.Add(item.Id, item.Value);
                        });
                    }
                    break;
                case eInformacao.AtividadePJ:
                    {
                        LstAtividade.Clear();
                        lRespostaGenerica.Resultado.ForEach(delegate(SinacorListaInfo item)
                        {
                            LstAtividade.Add(item.Id, item.Value);
                        });
                    }
                    break;
                case eInformacao.Assessor:
                    {
                        LstAssessor.Clear();
                        lRespostaGenerica.Resultado.ForEach(delegate(SinacorListaInfo item)
                        {
                            LstAssessor.Add(item.Id, item.Value);
                        });
                    }
                    break;
                case eInformacao.TipoDocumento:
                    {
                        LstTipoDocumento.Clear();
                        lRespostaGenerica.Resultado.ForEach(delegate(SinacorListaInfo item)
                        {
                            LstTipoDocumento.Add(item.Id, item.Value);
                        });
                    }
                    break;
            }
        }

        /// <summary>
        /// Método de apoio para receber ficha de pessoa física
        /// </summary>
        /// <param name="pParametros"></param>
        /// <returns></returns>
        private static ReceberObjetoResponse<FichaCadastralInfo> ReceberFichaEntidade(ReceberEntidadeRequest<ViewFichaCadastralCompletaInfo> pParametros)
        {
            try
            {
                var lResposta = new ReceberObjetoResponse<FichaCadastralInfo>();
                var lRespostaFinal = new ReceberObjetoResponse<ViewFichaCadastralCompletaInfo>();
                var lClienteInfo = new ReceberEntidadeRequest<ClienteInfo>();
                var lClienteBancoInfo = new ConsultarEntidadeRequest<ClienteBancoInfo>();
                var lClienteContaInfo = new ConsultarEntidadeRequest<ClienteContaInfo>();
                var lClienteEnderecoInfo = new ConsultarEntidadeRequest<ClienteEnderecoInfo>();
                var lClienteProcuradorRepresentanteInfo = new ConsultarEntidadeRequest<ClienteProcuradorRepresentanteInfo>();
                var lClienteSituacaoFinanceiraPatrimonialInfo = new ConsultarEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo>();
                var lClienteLoginInfo = new ReceberEntidadeRequest<LoginInfo>();
                var lClienteTelefoneInfo = new ConsultarEntidadeRequest<ClienteTelefoneInfo>();
                var lClienteEmitenteInfo = new ConsultarEntidadeRequest<ClienteEmitenteInfo>();
                var lContratoInfo = new ConsultarEntidadeRequest<ClienteContratoInfo>();
                var lClienteInvestidorNaoResisdenteInfo = new ConsultarEntidadeRequest<ClienteInvestidorNaoResidenteInfo>();
                var lClienteControladoraInfo = new ConsultarEntidadeRequest<ClienteControladoraInfo>();
                var lClienteDiretorInfo = new ConsultarEntidadeRequest<ClienteDiretorInfo>();
                var lClienteNaoOperaPorContaPropriaInfo = new ReceberEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo>();

                lClienteInfo.Objeto = new ClienteInfo();
                lClienteBancoInfo.Objeto = new ClienteBancoInfo();
                lClienteContaInfo.Objeto = new ClienteContaInfo();
                lClienteEnderecoInfo.Objeto = new ClienteEnderecoInfo();
                lClienteProcuradorRepresentanteInfo.Objeto = new ClienteProcuradorRepresentanteInfo();
                lClienteSituacaoFinanceiraPatrimonialInfo.Objeto = new ClienteSituacaoFinanceiraPatrimonialInfo();
                lClienteLoginInfo.Objeto = new LoginInfo();
                lClienteTelefoneInfo.Objeto = new ClienteTelefoneInfo();
                lClienteEmitenteInfo.Objeto = new ClienteEmitenteInfo();
                lContratoInfo.Objeto = new ClienteContratoInfo();
                lClienteInvestidorNaoResisdenteInfo.Objeto = new ClienteInvestidorNaoResidenteInfo();
                lClienteControladoraInfo.Objeto = new ClienteControladoraInfo();
                lClienteDiretorInfo.Objeto = new ClienteDiretorInfo();
                lClienteNaoOperaPorContaPropriaInfo.Objeto = new ClienteNaoOperaPorContaPropriaInfo();

                {   //--> Atribuindo parâmetro de busca às consultas.                
                    lClienteInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteBancoInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteContaInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteEnderecoInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteProcuradorRepresentanteInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteSituacaoFinanceiraPatrimonialInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteTelefoneInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteEmitenteInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lContratoInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteInvestidorNaoResisdenteInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteControladoraInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteDiretorInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                    lClienteNaoOperaPorContaPropriaInfo.Objeto.IdCliente = pParametros.Objeto.IdDoCliente;
                }

                lResposta.Objeto = new FichaCadastralInfo();

                lResposta.Objeto.ClienteResponse = new ClienteDbLib().ReceberCliente(lClienteInfo);
                lResposta.Objeto.ClienteBancoResponse = new ClienteBancoDbLib().ConsultarClienteBanco(lClienteBancoInfo);
                lResposta.Objeto.ClienteContaResponse = new ContaCorrenteDbLib().ConsultarClienteConta(lClienteContaInfo);
                lResposta.Objeto.ClienteEnderecoResponse = new ClienteEnderecoDbLib().ConsultarClienteEndereco(lClienteEnderecoInfo);
                lResposta.Objeto.ClienteProcuradorRepresentanteResponse = new ClienteProcuradorRepresentanteDbLib().ConsultarClienteProcuradorRepresentante(lClienteProcuradorRepresentanteInfo);
                lResposta.Objeto.ClienteSituacaoFinanceiraPatrimonialResponse = new ClienteSituacaoFinanceiraPatrimonialDbLib().ConsultarClienteSituacaoFinanceiraPatrimonial(lClienteSituacaoFinanceiraPatrimonialInfo);
                lResposta.Objeto.ClienteTelefoneReponse = new ClienteTelefoneDbLib().ConsultarClienteTelefone(lClienteTelefoneInfo);
                lResposta.Objeto.ClienteEmitenteResponse = new ClienteEmitente().ConsultarClienteEmitente(lClienteEmitenteInfo);
                lResposta.Objeto.ClienteContratoResponse = new ClienteContratoDbLib().ConsultarClienteContrato(lContratoInfo);
                lResposta.Objeto.ClienteIvestidorNaoResidenteResponse = new ClienteNaoResidenteDbLib().ConsultarClienteNaoResidente(lClienteInvestidorNaoResisdenteInfo);
                lResposta.Objeto.ClienteControladoraResponse = new ClienteControladoraDbLib().ConsultarClienteControladora(lClienteControladoraInfo);
                lResposta.Objeto.ClienteDiretorResponse = new ClienteDiretorDbLib().ConsultarClienteDiretor(lClienteDiretorInfo);
                lResposta.Objeto.ClienteNaoOperaPorContaPropriaResponse = new ClienteNaoOperaPorContaPropriaDbLib().ConsultarClienteNaoOperaPorContaPropria(lClienteNaoOperaPorContaPropriaInfo);
                //dados de login
                lClienteLoginInfo.Objeto.IdLogin = lResposta.Objeto.ClienteResponse.Objeto.IdLogin;
                lResposta.Objeto.ClienteLoginResponse = new LoginDbLib().ReceberLogin(lClienteLoginInfo);

                return lResposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        private static ReceberObjetoResponse<ViewFichaCadastralCompletaInfo> PreencheViewPessoaFisica(ReceberObjetoResponse<FichaCadastralInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ViewFichaCadastralCompletaInfo> lRetorno1 = new ReceberObjetoResponse<ViewFichaCadastralCompletaInfo>();
                lRetorno1.Objeto = new ViewFichaCadastralCompletaInfo();
                char chrTpPessoa = pParametros.Objeto.ClienteResponse.Objeto.TpPessoa;
                ViewFichaCadastralCompletaInfo lRetorno = new ViewFichaCadastralCompletaInfo();

                lRetorno.DadosBasicos_CidadeDeNascimento = pParametros.Objeto.ClienteResponse.Objeto.DsNaturalidade.ToString();
                lRetorno.DadosBasicos_TipoPessoa = pParametros.Objeto.ClienteResponse.Objeto.TpPessoa.ToString();
                lRetorno.DadosBasicos_CodigoDUC = pParametros.Objeto.ClienteResponse.Objeto.IdCliente == null ? 0.ToString() : pParametros.Objeto.ClienteResponse.Objeto.IdCliente.Value.ToString();
                lRetorno.DadosBasicos_Conjuge = pParametros.Objeto.ClienteResponse.Objeto.DsConjugue;
                lRetorno.DadosBasicos_CPF = pParametros.Objeto.ClienteResponse.Objeto.DsCpfCnpj.ToCpfCnpjString();
                lRetorno.DadosBasicos_DataDeEmissao = pParametros.Objeto.ClienteResponse.Objeto.DtEmissaoDocumento == null ? DateTime.MinValue.ToString("dd/MM/yyyy") : pParametros.Objeto.ClienteResponse.Objeto.DtEmissaoDocumento.Value.ToString("dd/MM/yyyy");
                lRetorno.DadosBasicos_DataDeNascimento = pParametros.Objeto.ClienteResponse.Objeto.DtNascimentoFundacao == null ? DateTime.MinValue.ToString("dd/MM/yyyy") : pParametros.Objeto.ClienteResponse.Objeto.DtNascimentoFundacao.Value.ToString("dd/MM/yyyy");
                lRetorno.DadosBasicos_DataDoCadastro = pParametros.Objeto.ClienteResponse.Objeto.DtPasso1.ToString("dd/MM/yyyy");
                lRetorno.DadosBasicos_EstadoDeNascimento = pParametros.Objeto.ClienteResponse.Objeto.CdUfNascimento;
                lRetorno.DadosBasicos_NomeCompleto = pParametros.Objeto.ClienteResponse.Objeto.DsNome;
                lRetorno.DadosBasicos_NomeDaMae = pParametros.Objeto.ClienteResponse.Objeto.DsNomeMae;
                lRetorno.DadosBasicos_NomeDoPai = pParametros.Objeto.ClienteResponse.Objeto.DsNomePai;
                lRetorno.DadosBasicos_OrgaoUfDeEmissao = pParametros.Objeto.ClienteResponse.Objeto.CdOrgaoEmissorDocumento;
                lRetorno.DadosBasicos_Sexo = pParametros.Objeto.ClienteResponse.Objeto.CdSexo == null ? string.Empty : pParametros.Objeto.ClienteResponse.Objeto.CdSexo.ToString();
                lRetorno.InformacoesComerciais_CargoAtualOuFuncao = pParametros.Objeto.ClienteResponse.Objeto.DsCargo;
                lRetorno.InformacoesComerciais_Empresa = pParametros.Objeto.ClienteResponse.Objeto.DsEmpresa;
                lRetorno.DadosBasicos_LiberadoParaOperar = pParametros.Objeto.ClienteResponse.Objeto.DsAutorizadoOperar;
                lRetorno.DadosBasicos_Numero = pParametros.Objeto.ClienteResponse.Objeto.DsNumeroDocumento;
                lRetorno.DadosBasicos_Escolaridade = pParametros.Objeto.ClienteResponse.Objeto.CdEscolaridade != null && pParametros.Objeto.ClienteResponse.Objeto.CdEscolaridade.HasValue ? pParametros.Objeto.ClienteResponse.Objeto.CdEscolaridade.ToString() : "0";
                lRetorno.DadosBasicos_Email = pParametros.Objeto.ClienteLoginResponse.Objeto != null ? pParametros.Objeto.ClienteLoginResponse.Objeto.DsEmail : string.Empty;
                lRetorno.DadosBasicos_TipoDeDocumento = chrTpPessoa.Equals('F') && !string.IsNullOrWhiteSpace(pParametros.Objeto.ClienteResponse.Objeto.TpDocumento) ? LstTipoDocumento[pParametros.Objeto.ClienteResponse.Objeto.TpDocumento].ToString() : string.Empty;            //sinacor
                lRetorno.DadosBasicos_EstadoCivil = chrTpPessoa.Equals('F') && (null != pParametros.Objeto.ClienteResponse.Objeto.CdEstadoCivil && !0.Equals(pParametros.Objeto.ClienteResponse.Objeto.CdEstadoCivil)) ? LstEstadoCivil[pParametros.Objeto.ClienteResponse.Objeto.CdEstadoCivil.ToString()].ToString() : string.Empty;
                if (null != pParametros.Objeto.ClienteResponse.Objeto.CdProfissaoAtividade && !0.Equals(pParametros.Objeto.ClienteResponse.Objeto.CdProfissaoAtividade))
                    lRetorno.InformacoesComerciais_Profissao = chrTpPessoa.Equals('F') ? LstProfissao[pParametros.Objeto.ClienteResponse.Objeto.CdProfissaoAtividade.Value.ToString()].DBToString() : LstAtividade[pParametros.Objeto.ClienteResponse.Objeto.CdProfissaoAtividade.ToString()].DBToString();   //sinacor

                string strNovaLinha = "<br />";

                //Recuperando dados cadastrais de contas do cliente
                pParametros.Objeto.ClienteContaResponse.Resultado.ForEach(delegate(ClienteContaInfo lconta)
                {
                    lRetorno.Dados_Contas +=
                        string.Concat(" <b>Código: ", lconta.CdCodigo, "</b>", strNovaLinha,
                                      " Assessor: ", lconta.CdAssessor, strNovaLinha,
                                      " Bolsa : ", lconta.CdSistema, strNovaLinha,
                                      " Ativa :", lconta.StAtiva ? "Sim" : "Não", strNovaLinha,
                                      " Conta invetimento: ", lconta.StContaInvestimento ? "Sim" : "Não", strNovaLinha,
                                      " <b>Conta principal: ", (null != lconta.StPrincipal && lconta.StPrincipal.Value) ? "Sim" : "Não", "</b>", strNovaLinha,
                                      strNovaLinha, strNovaLinha);
                }
                    );

                //Recuperando os dados de endereços dos cliente
                pParametros.Objeto.ClienteEnderecoResponse.Resultado.ForEach(delegate(ClienteEnderecoInfo lEndereco)
                {
                    lRetorno.DadosParaContato_Enderecos +=
                        string.Concat(" Logradouro: ", lEndereco.DsLogradouro, strNovaLinha,
                                        " Comp.: ", lEndereco.DsComplemento, strNovaLinha,
                                        " Bairro: ", lEndereco.DsBairro, strNovaLinha,
                                        " Cidade: ", lEndereco.DsCidade, strNovaLinha,
                                        " Número: ", lEndereco.DsNumero, strNovaLinha,
                                        " Estado: ", lEndereco.CdUf, strNovaLinha,
                                        " País: ", lEndereco.CdPais, strNovaLinha,
                                        " <b>Principal: ", (lEndereco.StPrincipal ? "Sim" : "Não"), "</b>", strNovaLinha,
                                        " Tipo Endereço: ", LstTipoEndereco[lEndereco.IdTipoEndereco.ToString()].ToString(), strNovaLinha, strNovaLinha
                                    );
                });

                //Recuperando os dados de telefones do cliente
                pParametros.Objeto.ClienteTelefoneReponse.Resultado.ForEach(delegate(ClienteTelefoneInfo lTel)
                {
                    lRetorno.DadosParaContato_Telefones +=
                        string.Concat(" <b>Tel: ", lTel.DsDdd,
                                      " - ", lTel.DsNumero, strNovaLinha, "</b>",
                                      " Ramal: ", lTel.DsRamal, strNovaLinha,
                                      " <b>Principal: ", (lTel.StPrincipal ? "Sim" : "Não"), "</b>", strNovaLinha,
                                      " Tipo de telefone: ", LstTipoTelefone[lTel.IdTipoTelefone.ToString()].ToString(), strNovaLinha, strNovaLinha);
                });

                pParametros.Objeto.ClienteSituacaoFinanceiraPatrimonialResponse.Resultado.ForEach(delegate(ClienteSituacaoFinanceiraPatrimonialInfo lInfo)
                {
                    lRetorno.InformacoesPatrimoniais_BensImoveis = lInfo.VlTotalBensImoveis.ToString();
                    lRetorno.InformacoesPatrimoniais_OutrosRendimentos = string.Concat(lInfo.DsOutrosRendimentos, " ", lInfo.VlTotalOutrosRendimentos);
                    lRetorno.InformacoesPatrimoniais_Salario = lInfo.VlTotalSalarioProLabore.ToString();
                    lRetorno.InformacoesPatrimoniais_TotalDeOutrosRendimentos = (lInfo.VlTotalBensMoveis + lInfo.VlTotalBensMoveis + lInfo.VlTotalOutrosRendimentos).ToString();
                }
                    );

                //Recuperando dados de representante legal
                pParametros.Objeto.ClienteProcuradorRepresentanteResponse.Resultado.ForEach(delegate(ClienteProcuradorRepresentanteInfo lRep)
                {
                    lRetorno.DadosBasicos_Representantes +=
                        string.Concat(" Nome: ", lRep.DsNome, strNovaLinha,
                                      " Data de Nascimento: ", lRep.DtNascimento, strNovaLinha,
                                      " Número documento: ", lRep.DsNumeroDocumento, strNovaLinha,
                                      " Orgão emissor: ", lRep.CdOrgaoEmissor, strNovaLinha,
                                      " UF Orgão emissor ", lRep.CdUfOrgaoEmissor, strNovaLinha,
                                      " CPF/CPNJ: ", lRep.NrCpfCnpj, strNovaLinha,
                                      " Tipo Documento: ", lRep.TpDocumento, strNovaLinha);
                });

                //Recuperando dados de emitente
                pParametros.Objeto.ClienteEmitenteResponse.Resultado.ForEach(delegate(ClienteEmitenteInfo lEmitente)
                {
                    lRetorno.DadosBasicos_Emitentes +=
                        string.Concat(" Nome: ", lEmitente.DsNome, strNovaLinha,
                                      " E-mail: ", lEmitente.DsEmail, strNovaLinha,
                                      " Data de Nascimento: ", lEmitente.DtNascimento.ToString(), strNovaLinha,
                                      " Bolsa: ", lEmitente.CdSistema, strNovaLinha,
                                      " Documento: ", lEmitente.DsNumeroDocumento, strNovaLinha,
                                      " CPF/CNPJ: ", lEmitente.NrCpfCnpj, strNovaLinha,
                                      " Principal: ", (lEmitente.StPrincipal ? "Sim" : "Não"), strNovaLinha, strNovaLinha
                                      );
                });

                //Recuperando dados bancários
                pParametros.Objeto.ClienteBancoResponse.Resultado.ForEach(delegate(ClienteBancoInfo lBanco)
                {
                    lRetorno.DadosBancarios_Contas +=
                        string.Format("Código do Banco: {1}{0}Agência: {2}-{6}{0}Conta: {3}-{4}{0}Principal: {5}{0}{0}"
                        , strNovaLinha, lBanco.CdBanco, lBanco.DsAgencia, lBanco.DsConta, lBanco.DsContaDigito, (lBanco.StPrincipal ? "Sim" : "Não"), lBanco.DsAgenciaDigito);
                });

                //Recuperando Dados de Contrato
                pParametros.Objeto.ClienteContratoResponse.Resultado.ForEach(delegate(ClienteContratoInfo lContrato)
                {
                    lRetorno.InformacoesPatrimoniais_DeclaracoesEAutorizacoes += string.Concat(" Contrato: ", LstContrato[lContrato.IdContrato.ToString()].ToString());
                });

                //Recuperando dados de Controladora
                pParametros.Objeto.ClienteControladoraResponse.Resultado.ForEach(delegate(ClienteControladoraInfo lControladora)
                {
                    lRetorno.DadosBasicos_Controladora +=
                        string.Concat(" Razão : ", lControladora.DsNomeRazaoSocial, strNovaLinha,
                                      "CPF/CNPJ:", lControladora.DsCpfCnpj, strNovaLinha, strNovaLinha);
                });

                //Recuperando dados de Diretor
                pParametros.Objeto.ClienteDiretorResponse.Resultado.ForEach(delegate(ClienteDiretorInfo lDiretor)
                {
                    lRetorno.DadosBasicos_Diretor +=
                        string.Concat(" Nome : ", lDiretor.DsNome, strNovaLinha,
                                      " CPF/CNPJ:", lDiretor.NrCpfCnpj, strNovaLinha,
                                      " Documento: ", lDiretor.DsIdentidade, strNovaLinha,
                                      " Orgão emissor: ", lDiretor.CdOrgaoEmissor, strNovaLinha,
                                      " UF Orgão Emissor: ", lDiretor.CdUfOrgaoEmissor, strNovaLinha, strNovaLinha);
                });


                lRetorno1.Objeto = lRetorno;
                return lRetorno1;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, 0, "", LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        #endregion

        #region | Métodos de acesso Sinacor

        public static ConsultarObjetosResponse<SinacorListaInfo> ConsultarListaSinacor(ConsultarEntidadeRequest<SinacorListaInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<SinacorListaInfo>();

                var lAcessaDados = new AcessaDados();

                var lInformacao = pParametros.Objeto.Informacao;

                if (Gradual.Intranet.Contratos.Dados.Enumeradores.eInformacao.AssessorPadronizado.Equals(lInformacao))
                    lInformacao = Gradual.Intranet.Contratos.Dados.Enumeradores.eInformacao.Assessor; //--> Corrigindo para realizar a consulta para assesor parametrizado.

                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ListaComboSinacor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "Informacao", DbType.Int32, (int)lInformacao);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        if (Gradual.Intranet.Contratos.Dados.Enumeradores.eInformacao.AssessorPadronizado.Equals(pParametros.Objeto.Informacao))
                            for (int i = 0; i < lDataTable.Rows.Count; i++) //--> ComboInfoNormalizada (id:{id},value:{id}-{descricao})
                                resposta.Resultado.Add(CriarSinacorListaInfoNormalizada(lDataTable.Rows[i]));
                        else
                            for (int i = 0; i < lDataTable.Rows.Count; i++) //--> ComboInfoPadrao (id:{id},value:{descricao})
                                resposta.Resultado.Add(CriarSinacorListaInfo(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static SinacorListaInfo CriarSinacorListaInfoNormalizada(DataRow linha)
        {
            return new SinacorListaInfo()
            {
                Id = linha["id"].DBToString(),
                Value = string.Format("{0} - {1}", linha["id"].DBToString().Trim().PadLeft(4, '0'), linha["Value"].DBToString())
            };
        }

        private static SinacorListaInfo CriarSinacorListaInfo(DataRow linha)
        {
            SinacorListaInfo lSinacorListaInfo = new SinacorListaInfo();

            lSinacorListaInfo.Id = linha["id"].DBToString();
            lSinacorListaInfo.Value = linha["Value"].DBToString();

            return lSinacorListaInfo;
        }

        #endregion
    }
}
