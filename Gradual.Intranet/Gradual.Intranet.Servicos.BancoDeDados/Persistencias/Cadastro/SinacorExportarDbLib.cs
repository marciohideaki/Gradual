using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public class SinacorExportarDbLib
    {
        #region | Atributos

        private const string gConexaoRisco = "Risco";
        private const string gConexaoCadastro = "Cadastro";
        private const string gConexaoControleAcesso = "Seguranca";
        private const string gConexaoSinacorConsulta = "SinacorConsulta";
        private const string gConexaoSinacorCorrwin = "SinacorExportacao";

        private LoginInfo gLogin;
        private ClienteInfo gCliente;
        private ClienteBancoInfo gBancoPrincipal;
        private ClienteContaInfo gContaExportacao;
        private List<ClienteDiretorInfo> glDiretor;
        private List<ClienteEmitenteInfo> gEmitente;
        private ClienteTelefoneInfo gTelefonePrincipal;
        private ClienteEnderecoInfo gEnderecoPrincipal;
        private ClienteSituacaoFinanceiraPatrimonialInfo gSfp;
        private SinacorExportarDadosClienteInfo gClienteSinacor;
        private ConsultarObjetosResponse<ClienteBancoInfo> gClienteContasBancarias;
        private SinacorExportarEnderecoInfo gEnderecoPrincipalSinacor = new SinacorExportarEnderecoInfo();
        private List<SinacorExportarEnderecoInfo> gOutrosEnderecosSinacor = new List<SinacorExportarEnderecoInfo>();

        private int? gCodigo;
        private int gAssessor;
        private Boolean gPrimeiraExportacao;

        #endregion

        #region | Enums

        private enum DataType
        {
            NUMBER,
            VARCHAR2,
            CHAR,
            DATE
        }

        #endregion

        #region | Propriedades

        private int GetCdUsuario
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(ConfigurationManager.AppSettings["CD_USUARIO_SINACOR"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetParametroGrupoAlavancagem
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(ConfigurationManager.AppSettings["ExportacaoGrupoAlavancagem"], out lRetorno);

                return lRetorno;
            }
        }

        #endregion

        #region | Structs

        public struct sMantemValoresBovespa
        {
            public string PC_CORCOR_PRIN { get; set; }
            public string COD_AGCT { get; set; }
            public string COD_CLI_AGCT { get; set; }
            public string CD_AGENTE_COMP { get; set; }
            public string CD_CLIENTE_COMP { get; set; }
        }

        private struct sGeraInsert
        {
            public string Campo;
            public object Valor;
            public SinacorExportarDbLib.DataType Tipo;
            public Boolean TiraPonto;
            public sGeraInsert(string sCampo, object oValor, SinacorExportarDbLib.DataType dtTipo)
            {
                Campo = sCampo;
                Valor = oValor;
                Tipo = dtTipo;
                TiraPonto = true;
            }
            public sGeraInsert(string sCampo, object oValor, SinacorExportarDbLib.DataType dtTipo, Boolean bTiraPonto)
            {
                Campo = sCampo;
                Valor = oValor;
                Tipo = dtTipo;
                TiraPonto = bTiraPonto;
            }
        }

        #endregion

        #region | Métodos Exportacao

        public SalvarEntidadeResponse<SinacorExportarInfo> SinacorExportarCliente(SalvarObjetoRequest<SinacorExportarInfo> pParametros)
        {
            var lRetorno = new SalvarEntidadeResponse<SinacorExportarInfo>();

            try
            {
                lRetorno.Objeto = new SinacorExportarInfo();

                gPrimeiraExportacao = pParametros.Objeto.Entrada.PrimeiraExportacao;
                gCodigo = pParametros.Objeto.Entrada.CdCodigo;

                lRetorno.Objeto = pParametros.Objeto;
                lRetorno.Objeto.Retorno = new SinacorExportacaoRetornoInfo();

                GetDadosCliente(pParametros.Objeto);

                lRetorno.Objeto.Retorno.DadosClienteMensagens = this.ValidarClienteCompleto();

                if (null != lRetorno.Objeto.Retorno.DadosClienteMensagens && lRetorno.Objeto.Retorno.DadosClienteMensagens.Count > 0)
                {
                    lRetorno.Objeto.Retorno.DadosClienteOk = false;
                    LogCadastro.Logar(lRetorno.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Exportar);
                    return lRetorno;
                }
                else
                {
                    lRetorno.Objeto.Retorno.DadosClienteOk = true;
                }

                SetDadosSinacor();

                lRetorno.Objeto.Retorno.ExportacaoSinacorMensagens = this.Exportar();
                if (null != lRetorno.Objeto.Retorno.ExportacaoSinacorMensagens && lRetorno.Objeto.Retorno.ExportacaoSinacorMensagens.Count > 0)
                {
                    lRetorno.Objeto.Retorno.ExportacaoSinacorOk = false;
                    LogCadastro.Logar(lRetorno.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Exportar);
                    return lRetorno;
                }
                else
                    lRetorno.Objeto.Retorno.ExportacaoSinacorOk = true;

                lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroMensagens = this.AtualizaCliente();
                if (null != lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroMensagens && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroMensagens.Count > 0)
                    lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk = false;
                else
                    lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk = true;

                lRetorno.Objeto.Retorno.ExportacaoComplementosMensagens = this.ExportarComplementos();
                if (null != lRetorno.Objeto.Retorno.ExportacaoComplementosMensagens && lRetorno.Objeto.Retorno.ExportacaoComplementosMensagens.Count > 0)
                    lRetorno.Objeto.Retorno.ExportacaoComplementosOk = false;
                else
                    lRetorno.Objeto.Retorno.ExportacaoComplementosOk = true;

                lRetorno.Objeto.Retorno.ExportacaoRiscoMensagens = this.ExportarRiscoEPermissao();
                if (null != lRetorno.Objeto.Retorno.ExportacaoRiscoMensagens && lRetorno.Objeto.Retorno.ExportacaoRiscoMensagens.Count > 0)
                    lRetorno.Objeto.Retorno.ExportacaoRiscoOk = false;
                else
                    lRetorno.Objeto.Retorno.ExportacaoRiscoOk = true;

                if (lRetorno.Objeto.Entrada.PrimeiraExportacao)
                    lRetorno.Objeto.Entrada.CdCodigo = gCodigo.Value;

                return lRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Preenche as entidades do Cadastro Para Validar o Cliente
        /// </summary>
        /// <param name="pParametro"></param>
        private void GetDadosCliente(SinacorExportarInfo pParametro)
        {
            int lIdCliente = pParametro.Entrada.IdCliente;

            ReceberEntidadeRequest<ClienteInfo> lEntidadeCliente = new ReceberEntidadeRequest<ClienteInfo>();
            lEntidadeCliente.Objeto = new ClienteInfo();
            lEntidadeCliente.Objeto.IdCliente = lIdCliente;
            gCliente = ClienteDbLib.ReceberCliente(lEntidadeCliente).Objeto;

            ReceberEntidadeRequest<LoginInfo> lEntidadeLogin = new ReceberEntidadeRequest<LoginInfo>();
            lEntidadeLogin.Objeto = new LoginInfo();
            lEntidadeLogin.Objeto.IdLogin = gCliente.IdLogin;
            gLogin = ClienteDbLib.ReceberLogin(lEntidadeLogin).Objeto;

            gBancoPrincipal = ClienteDbLib.GetClienteBancoPrincipal(gCliente);

            gClienteContasBancarias = ClienteDbLib.ConsultarClienteBanco(new ConsultarEntidadeRequest<ClienteBancoInfo>() { Objeto = new ClienteBancoInfo() { IdCliente = lIdCliente } });

            gEnderecoPrincipal = ClienteDbLib.GetClienteEnderecoPrincipal(gCliente);

            gTelefonePrincipal = ClienteDbLib.GetClienteTelefonePrincipal(gCliente);

            gSfp = ClienteDbLib.GetClienteSituacaoFinanceiraPatrimonialPorIdCliente(gCliente);

            glDiretor = ClienteDbLib.GetClienteDiretorPorIdCliente(gCliente);
            gEmitente = ClienteDbLib.GetClienteEmitentePorIdCliente(gCliente);
        }

        /// <summary>
        /// Valida se o Cliente está completo para a exportação
        /// </summary>
        /// <returns></returns>
        private List<SinacorExportacaoRetornoFalhaDadosClienteInfo> ValidarClienteCompleto()
        {
            List<SinacorExportacaoRetornoFalhaDadosClienteInfo> lRetorno = new List<SinacorExportacaoRetornoFalhaDadosClienteInfo>();

            try
            {
                if (gPrimeiraExportacao)
                {
                    //TODO: comentar quando for gerar para o Rocket
                    //if (gCliente.StPasso == 4)
                    //{
                    //    SinacorExportacaoRetornoFalhaDadosClienteInfo lFalha = new SinacorExportacaoRetornoFalhaDadosClienteInfo();
                    //    lFalha.Mensagem = "Cliente Passo 4 Não Pode ser Exportado pela Primeira Vez";
                    //    lFalha.Tabela = "TB_CLIENTE";
                    //    lRetorno.Add(lFalha);
                    //}

                    if (gCliente.IdAssessorInicial == null || gCliente.IdAssessorInicial == 0)
                    {
                        SinacorExportacaoRetornoFalhaDadosClienteInfo lFalha = new SinacorExportacaoRetornoFalhaDadosClienteInfo();
                        lFalha.Mensagem = "Para a Primeira Exportação, o Cliente Precisa ter um Assessor Inicial Cadastrado ";
                        lFalha.Tabela = "TB_CLIENTE";
                        lRetorno.Add(lFalha);
                    }
                }
                else
                {
                    if (null == gCodigo)
                    {
                        SinacorExportacaoRetornoFalhaDadosClienteInfo lFalha = new SinacorExportacaoRetornoFalhaDadosClienteInfo();
                        lFalha.Mensagem = "Para re-exportar o Cliente, é Obrigatório Informar o Código";
                        lFalha.Tabela = "TB_CLIENTE";
                        lRetorno.Add(lFalha);
                    }

                    ConsultarEntidadeRequest<ClienteContaInfo> lConta = new ConsultarEntidadeRequest<ClienteContaInfo>();
                    lConta.Objeto = new ClienteContaInfo();
                    lConta.Objeto.IdCliente = gCliente.IdCliente.Value;
                    List<ClienteContaInfo> lContas = new List<ClienteContaInfo>();
                    lContas = ClienteDbLib.ConsultarClienteConta(lConta).Resultado;
                    Boolean lContaOk = false;

                    foreach (ClienteContaInfo item in lContas)
                    {
                        if (item.CdCodigo == gCodigo.Value)
                        {
                            lContaOk = true;
                            gContaExportacao = item;
                        }
                    }

                    if (!lContaOk)
                    {
                        SinacorExportacaoRetornoFalhaDadosClienteInfo lFalha = new SinacorExportacaoRetornoFalhaDadosClienteInfo();
                        lFalha.Mensagem = "Para re-exportar o Cliente, é Obrigatório Informar um Código Válido do Cliente";
                        lFalha.Tabela = "TB_CLIENTE_CONTA";
                        lRetorno.Add(lFalha);
                    }
                }

                //Telefone
                if (null == gTelefonePrincipal || null == gTelefonePrincipal.DsNumero || gTelefonePrincipal.DsNumero.Trim().Length == 0)
                {
                    SinacorExportacaoRetornoFalhaDadosClienteInfo lFalha = new SinacorExportacaoRetornoFalhaDadosClienteInfo();
                    lFalha.Mensagem = "É Necessário Cadastrar um Telefone Principal para o Cliente";
                    lFalha.Tabela = "TB_CLIENTE_TELEFONE";
                    lRetorno.Add(lFalha);
                }
                //Endereço
                if (null == gEnderecoPrincipal || null == gEnderecoPrincipal.DsLogradouro || gEnderecoPrincipal.DsLogradouro.Trim().Length == 0)
                {
                    SinacorExportacaoRetornoFalhaDadosClienteInfo lFalha = new SinacorExportacaoRetornoFalhaDadosClienteInfo();
                    lFalha.Mensagem = "É Necessário Cadastrar um Endereço Principal para o Cliente";
                    lFalha.Tabela = "TB_CLIENTE_ENDERECO";
                    lRetorno.Add(lFalha);
                }

                //Conta Bancária
                if (null == gBancoPrincipal || null == gBancoPrincipal.DsAgencia || gBancoPrincipal.DsAgencia.Trim().Length == 0)
                {
                    SinacorExportacaoRetornoFalhaDadosClienteInfo lFalha = new SinacorExportacaoRetornoFalhaDadosClienteInfo();
                    lFalha.Mensagem = "É Necessário Cadastrar uma Conta Bancária Principal para o Cliente";
                    lFalha.Tabela = "TB_CLIENTE_BANCO";
                    lRetorno.Add(lFalha);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Preenche dados no formato de Exportação para o Sinacor
        /// </summary>
        private void SetDadosSinacor()
        {
            try
            {
                SetEnredecosParaExportacao(gCliente.IdCliente.Value);

                gClienteSinacor = new SinacorExportarDadosClienteInfo();

                gClienteSinacor.TP_CLIENTE = gCliente.TpCliente.ToString();
                gClienteSinacor.TP_Registro = "I";
                gClienteSinacor.CD_CPFCGC = gCliente.DsCpfCnpj;
                gClienteSinacor.DT_CRIACAO = DateTime.Now.ToString();
                gClienteSinacor.DT_ATUALIZ = DateTime.Now.ToString();
                gClienteSinacor.DT_NASC_FUND = gCliente.DtNascimentoFundacao.ToString();
                gClienteSinacor.CD_CON_DEP = "1";
                gClienteSinacor.TP_PESSOA = gCliente.TpPessoa.ToString();
                gClienteSinacor.NM_CLIENTE = gCliente.DsNome.ToUpper();

                gClienteSinacor.ID_SEXO = gCliente.CdSexo.ToString();
                gClienteSinacor.CD_EST_CIVIL = gCliente.CdEstadoCivil.ToString();
                var lVinc = new ConsultarEntidadeRequest<ClientePessoaVinculadaPorClienteInfo>();
                lVinc.Objeto = new ClientePessoaVinculadaPorClienteInfo();
                lVinc.Objeto.IdCliente = gCliente.IdCliente.Value;
                List<ClientePessoaVinculadaPorClienteInfo> lPessoaVinculada = ClienteDbLib.ConsultarPessoaVinculadaPorCliente(lVinc).Resultado;

                if (null != lPessoaVinculada && lPessoaVinculada.Count > 0)
                    gClienteSinacor.IN_PESS_VINC = "S";

                else
                    gClienteSinacor.IN_PESS_VINC = "N";

                gClienteSinacor.NM_CONJUGE = gCliente.DsConjugue;
                gClienteSinacor.CD_CAPAC = string.Empty;

                gClienteSinacor.CD_CODQUA = string.Empty;
                gClienteSinacor.CD_ATIV = (gCliente.TpPessoa == 'F') ? gCliente.CdProfissaoAtividade.ToString() : gCliente.CdAtividadePrincipal.ToString();
                gClienteSinacor.CD_NACION = gCliente.CdNacionalidade.ToString();
                gClienteSinacor.SG_PAIS = gCliente.CdPaisNascimento;

                if (gPrimeiraExportacao)
                {
                    gClienteSinacor.CD_CLIENTE = this.GerarBovespa();
                    gClienteSinacor.PC_CORCOR_PRIN = "0";
                    gClienteSinacor.CD_ASSESSOR = gCliente.IdAssessorInicial.ToString();
                    gClienteSinacor.CD_AGENTE_COMP = "";
                    gClienteSinacor.CD_CLIENTE_COMP = "";
                    gClienteSinacor.COD_AGCT = "";
                    gClienteSinacor.COD_CLI_AGCT = "";
                }
                else
                {
                    gClienteSinacor.CD_CLIENTE = gContaExportacao.CdCodigo.ToString();

                    sMantemValoresBovespa lValoresAtuais = GetDadosAtuaisBovespa(gClienteSinacor.CD_CLIENTE);

                    gClienteSinacor.PC_CORCOR_PRIN = lValoresAtuais.PC_CORCOR_PRIN;
                    if (gClienteSinacor.PC_CORCOR_PRIN.Trim().Length == 0)
                        gClienteSinacor.PC_CORCOR_PRIN = "0";
                    gClienteSinacor.COD_AGCT = lValoresAtuais.COD_AGCT;
                    gClienteSinacor.COD_CLI_AGCT = lValoresAtuais.COD_CLI_AGCT;
                    gClienteSinacor.CD_AGENTE_COMP = lValoresAtuais.CD_AGENTE_COMP;
                    gClienteSinacor.CD_CLIENTE_COMP = lValoresAtuais.CD_CLIENTE_COMP;

                    gClienteSinacor.CD_ASSESSOR = gContaExportacao.CdAssessor.ToString();

                }
                //Tratados com return null para numérico
                gClienteSinacor.CD_AGENTE_COMP = "";
                gClienteSinacor.CD_CLIENTE_COMP = "";

                gCodigo = int.Parse(gClienteSinacor.CD_CLIENTE);
                gAssessor = int.Parse(gClienteSinacor.CD_ASSESSOR);

                gClienteSinacor.DV_CLIENTE = this.GeraDigito(gClienteSinacor.CD_CLIENTE);

                gClienteSinacor.CD_BANCO_CLICTA_PRINCIPAL = gBancoPrincipal.CdBanco;
                gClienteSinacor.CD_AGENCIA_CLICTA_PRINCIPAL = gBancoPrincipal.DsAgencia;
                gClienteSinacor.DV_AGENCIA_CLICTA_PRINCIPAL = gBancoPrincipal.DsAgenciaDigito;
                gClienteSinacor.NR_CONTA_CLICTA_PRINCIPAL = gBancoPrincipal.DsConta;
                gClienteSinacor.DV_CONTA_CLICTA_PRINCIPAL = gBancoPrincipal.DsContaDigito == "" ? " " : gBancoPrincipal.DsContaDigito;

                gClienteSinacor.DS_COMNOM = string.Empty;
                gClienteSinacor.IN_REC_DIVI = "S";
                gClienteSinacor.IN_IRSDIV = "3";  //Tipo De Imposto de Renda

                //Endereço e Telefone
                gClienteSinacor.CD_CEP = gEnderecoPrincipalSinacor.CD_CEP;
                gClienteSinacor.CD_DDD_CELULAR1 = gEnderecoPrincipalSinacor.CD_DDD_CELULAR1;
                gClienteSinacor.CD_DDD_CELULAR2 = gEnderecoPrincipalSinacor.CD_DDD_CELULAR2;
                gClienteSinacor.CD_DDD_FAX = gEnderecoPrincipalSinacor.CD_DDD_FAX;
                gClienteSinacor.CD_DDD_TEL = gEnderecoPrincipalSinacor.CD_DDD_TEL;
                gClienteSinacor.IN_ENDE = gEnderecoPrincipalSinacor.IN_ENDE;
                gClienteSinacor.NM_BAIRRO = gEnderecoPrincipalSinacor.NM_BAIRRO;
                gClienteSinacor.NM_CIDADE = gEnderecoPrincipalSinacor.NM_CIDADE;
                gClienteSinacor.NM_COMP_ENDE = gEnderecoPrincipalSinacor.NM_COMP_ENDE;
                gClienteSinacor.NM_LOGRADOURO = gEnderecoPrincipalSinacor.NM_LOGRADOURO;
                gClienteSinacor.NR_CELULAR1 = gEnderecoPrincipalSinacor.NR_CELULAR1;
                gClienteSinacor.NR_CELULAR2 = gEnderecoPrincipalSinacor.NR_CELULAR2;
                gClienteSinacor.NR_FAX = gEnderecoPrincipalSinacor.NR_FAX;
                gClienteSinacor.NR_PREDIO = gEnderecoPrincipalSinacor.NR_PREDIO;
                gClienteSinacor.NR_RAMAL = gEnderecoPrincipalSinacor.NR_RAMAL;
                gClienteSinacor.NR_TELEFONE = gEnderecoPrincipalSinacor.NR_TELEFONE;
                gClienteSinacor.SG_ESTADO = gEnderecoPrincipalSinacor.SG_ESTADO;
                gClienteSinacor.SG_PAIS_ENDE1 = gEnderecoPrincipalSinacor.SG_PAIS_ENDE1;

                gClienteSinacor.IN_CART_PROP = gCliente.StCarteiraPropria.Value ? "S" : "N";
                gClienteSinacor.IN_EMITE_NOTA = null;

                gClienteSinacor.IN_SITUAC = "A";
                gClienteSinacor.NM_MAE = gCliente.DsNomeMae;
                gClienteSinacor.NM_PAI = gCliente.DsNomePai;
                gClienteSinacor.NM_APELIDO = string.Empty;

                gClienteSinacor.IN_CORRENTISTA = "S";

                gClienteSinacor.NM_LOC_NASC = gCliente.DsNaturalidade;
                gClienteSinacor.NM_EMPRESA = gCliente.DsEmpresa;

                gClienteSinacor.CD_ORIGEM = "1";

                gClienteSinacor.SG_ESTADO_NASC = gCliente.CdUfNascimento;
                gClienteSinacor.TP_CONTA = "N";
                gClienteSinacor.CD_VINCULO = null;
                gClienteSinacor.NM_E_MAIL = gLogin.DsEmail == "" ? "a@a.a" : gLogin.DsEmail;
                gClienteSinacor.NM_E_MAIL2 = string.IsNullOrWhiteSpace(gCliente.DsEmailComercial) ? string.Empty : gCliente.DsEmailComercial;

                gClienteSinacor.TP_REGCAS = "";

                if (gCliente.TpPessoa == 'F')
                {
                    //Tipo de Investidor
                    gClienteSinacor.TP_INVESTIDOR = "101";
                    gClienteSinacor.CD_COSIF = "21";
                }
                else if ('J'.Equals(gCliente.TpPessoa))
                {
                    gClienteSinacor.TP_INVESTIDOR = gCliente.CdProfissaoAtividade.DBToString();
                    gClienteSinacor.CD_COSIF = "22";
                }
                else
                {
                    if (gCliente.TpCliente == 8)
                    {
                        gClienteSinacor.TP_INVESTIDOR = "701";
                        gClienteSinacor.CD_COSIF = "8";
                    }
                    else if (gCliente.TpCliente == 18)
                    {
                        gClienteSinacor.TP_INVESTIDOR = "202";
                        gClienteSinacor.CD_COSIF = "22";
                    }
                    else
                    {
                        gClienteSinacor.TP_INVESTIDOR = "101";
                        gClienteSinacor.CD_COSIF = "22";
                    }
                }
                gClienteSinacor.TP_CLIENTE_BOL = gCliente.TpCliente.ToString();

                //COSIF para conta de Investimento
                gClienteSinacor.CD_COSIF_CI = "10";
                //Cargo
                gClienteSinacor.DS_CARGO = gCliente.DsCargo;

                gClienteSinacor.IN_INTEGRA_CC = "U";

                //Documentos
                gClienteSinacor.CD_DOC_IDENT = gCliente.DsNumeroDocumento == null ? "" : gCliente.DsNumeroDocumento;
                gClienteSinacor.CD_TIPO_DOC = gCliente.TpDocumento;
                gClienteSinacor.CD_ORG_EMIT = gCliente.CdOrgaoEmissorDocumento;
                gClienteSinacor.DT_DOC_IDENT = gCliente.DtEmissaoDocumento.ToString();
                gClienteSinacor.SG_ESTADO_EMIS = gCliente.CdUfEmissaoDocumento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exporta cliente via CliExterno
        /// </summary>
        /// <returns></returns>
        private List<SinacorExportacaoRetornoFalhaBovespaInfo> Exportar()
        {
            List<SinacorExportacaoRetornoFalhaBovespaInfo> lRetorno = new List<SinacorExportacaoRetornoFalhaBovespaInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gConexaoSinacorCorrwin;
            Conexao._ConnectionStringName = gConexaoSinacorCorrwin;
            using (DbConnection conn = Conexao.CreateIConnection())
            {
                conn.Open();
                DbTransaction lTransaction = conn.BeginTransaction();
                try
                {

                    #region [Passo1]

                    string strLimpaTscimpclih = "DELETE FROM TSCIMPCLIH";
                    DbCommand lDbCommandClienteHistorico = lAcessaDados.CreateCommand(CommandType.Text, strLimpaTscimpclih);
                    lAcessaDados.ExecuteNonQuery(lDbCommandClienteHistorico);
                    string strLimpaTscerroh = "DELETE FROM TSCERROH";
                    DbCommand lDbCommandClienteErro = lAcessaDados.CreateCommand(CommandType.Text, strLimpaTscerroh);
                    lAcessaDados.ExecuteNonQuery(lDbCommandClienteErro);

                    #endregion

                    #region [Passo2]

                    #region [Trata Endereço nas Atividades]

                    //Exportação do Endereço
                    if (!gPrimeiraExportacao)
                    {
                        // Tratamento de endereço de Custódia e CC
                        DbCommand lDbCommand1stExport = null;

                        //ver se existe o 1
                        int existeEndereco1 = 0;

                        lDbCommand1stExport = lAcessaDados.CreateCommand(lTransaction, CommandType.StoredProcedure, "prc_existe_endereco");
                        lAcessaDados.AddInParameter(lDbCommand1stExport, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                        lAcessaDados.AddInParameter(lDbCommand1stExport, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND);
                        lAcessaDados.AddInParameter(lDbCommand1stExport, "pCD_CON_DEP", DbType.Int32, 1);
                        lAcessaDados.AddOutParameter(lDbCommand1stExport, "pCOUNT", DbType.Int32, 8);
                        lDbCommand1stExport.Transaction = lTransaction;
                        lAcessaDados.ExecuteNonQuery(lDbCommand1stExport, lTransaction);
                        existeEndereco1 = lDbCommand1stExport.Parameters["pCOUNT"].Value.DBToInt32();

                        //criar o 1 se não existe
                        if (existeEndereco1 == 0)
                        {
                            lDbCommand1stExport.Parameters.Clear();
                            lDbCommand1stExport = lAcessaDados.CreateCommand(lTransaction, CommandType.StoredProcedure, "prc_exp_endereco_temp");
                            lAcessaDados.AddInParameter(lDbCommand1stExport, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                            lAcessaDados.AddInParameter(lDbCommand1stExport, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                            lAcessaDados.AddInParameter(lDbCommand1stExport, "pCD_CON_DEP", DbType.Int32, 1);
                            lDbCommand1stExport.Transaction = lTransaction;
                            lAcessaDados.ExecuteNonQuery(lDbCommand1stExport, lTransaction);
                        }

                        ////mover endereço das atividades para o 1 e limpa outros endereços
                        lDbCommand1stExport.Parameters.Clear();
                        lDbCommand1stExport = lAcessaDados.CreateCommand(lTransaction, CommandType.StoredProcedure, "prc_move1_endAtiv_del_outros");
                        lAcessaDados.AddInParameter(lDbCommand1stExport, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                        lAcessaDados.AddInParameter(lDbCommand1stExport, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        lAcessaDados.AddInParameter(lDbCommand1stExport, "pCD_CON_DEP", DbType.Int32, 1);
                        lDbCommand1stExport.Transaction = lTransaction;
                        lAcessaDados.ExecuteNonQuery(lDbCommand1stExport, lTransaction);
                    }

                    #endregion

                    string comando;

                    #region [Insert]

                    List<sGeraInsert> campos = new List<sGeraInsert>();

                    /*
                    COD_AGCT - CD_CUSTODIANTE
                    COD_CLI_AGCT - CD_CLIE_CUST
                    CD_AGENTE_COMP - CD_AGENTE_COMP
                    CD_CLIENTE_COMP - CD_USUA_COMP
                    */

                    campos.Add(new sGeraInsert("TP_REGISTRO", "I", DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_AGENTE_COMP", gClienteSinacor.CD_AGENTE_COMP, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_CLIENTE_COMP", gClienteSinacor.CD_CLIENTE_COMP, DataType.NUMBER));
                    campos.Add(new sGeraInsert("COD_AGCT", gClienteSinacor.COD_AGCT, DataType.NUMBER));
                    campos.Add(new sGeraInsert("COD_CLI_AGCT", gClienteSinacor.COD_CLI_AGCT, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_CPFCGC", gClienteSinacor.CD_CPFCGC, DataType.NUMBER));
                    campos.Add(new sGeraInsert("DT_CRIACAO", string.Format("TO_DATE('{0}', 'dd/mm/yyyy hh24:mi:ss')", gClienteSinacor.DT_CRIACAO), DataType.DATE));
                    campos.Add(new sGeraInsert("DT_ATUALIZ", string.Format("TO_DATE('{0}', 'dd/mm/yyyy hh24:mi:ss')", gClienteSinacor.DT_ATUALIZ), DataType.DATE));
                    campos.Add(new sGeraInsert("DT_NASC_FUND", string.Format("TO_DATE('{0}', 'dd/mm/yyyy hh24:mi:ss')", gClienteSinacor.DT_NASC_FUND), DataType.DATE));
                    campos.Add(new sGeraInsert("CD_CON_DEP", gClienteSinacor.CD_CON_DEP, DataType.NUMBER));
                    campos.Add(new sGeraInsert("TP_PESSOA", gClienteSinacor.TP_PESSOA, DataType.CHAR));
                    campos.Add(new sGeraInsert("NM_CLIENTE", gClienteSinacor.NM_CLIENTE, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_DOC_IDENT", gClienteSinacor.CD_DOC_IDENT, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_TIPO_DOC", gClienteSinacor.CD_TIPO_DOC, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_ORG_EMIT", gClienteSinacor.CD_ORG_EMIT, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("ID_SEXO", gClienteSinacor.ID_SEXO, DataType.CHAR));
                    campos.Add(new sGeraInsert("CD_EST_CIVIL", gClienteSinacor.CD_EST_CIVIL, DataType.NUMBER));
                    campos.Add(new sGeraInsert("IN_PESS_VINC", gClienteSinacor.IN_PESS_VINC, DataType.CHAR));
                    campos.Add(new sGeraInsert("NM_CONJUGE", gClienteSinacor.NM_CONJUGE, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_CAPAC", gClienteSinacor.CD_CAPAC, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NM_PAI", gClienteSinacor.NM_PAI, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_CODQUA", gClienteSinacor.CD_CODQUA, DataType.NUMBER));
                    campos.Add(new sGeraInsert("TP_CLIENTE", gClienteSinacor.TP_CLIENTE, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_ATIV", gClienteSinacor.CD_ATIV, DataType.NUMBER));
                    campos.Add(new sGeraInsert("IN_IRMARG", gClienteSinacor.IN_IRMARG, DataType.CHAR));
                    campos.Add(new sGeraInsert("IN_IOFMAR", gClienteSinacor.IN_IOFMAR, DataType.CHAR));
                    campos.Add(new sGeraInsert("IN_IRSDIV", gClienteSinacor.IN_IRSDIV, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_NACION", gClienteSinacor.CD_NACION, DataType.NUMBER));
                    campos.Add(new sGeraInsert("SG_PAIS", gClienteSinacor.SG_PAIS, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_CLIENTE", gClienteSinacor.CD_CLIENTE, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("DV_CLIENTE", gClienteSinacor.DV_CLIENTE, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_CLIENTE_BMF", gClienteSinacor.CD_CLIENTE_BMF, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_ADMIN_CVM", gClienteSinacor.CD_ADMIN_CVM, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_BOLSA", gClienteSinacor.CD_BOLSA, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_CORR_OUTR_BOLSA", gClienteSinacor.CD_CORR_OUTR_BOLSA, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_CLIE_OUTR_BOLSA", gClienteSinacor.CD_CLIE_OUTR_BOLSA, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_BANCO", gClienteSinacor.CD_BANCO, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_AGENCIA", gClienteSinacor.CD_AGENCIA, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NM_CONTA", gClienteSinacor.NM_CONTA, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_ASSESSOR", gClienteSinacor.CD_ASSESSOR, DataType.NUMBER));
                    campos.Add(new sGeraInsert("DS_COMNOM", gClienteSinacor.DS_COMNOM, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("IN_REC_DIVI", gClienteSinacor.IN_REC_DIVI, DataType.CHAR));
                    campos.Add(new sGeraInsert("NM_LOGRADOURO", gClienteSinacor.NM_LOGRADOURO, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NR_PREDIO", gClienteSinacor.NR_PREDIO, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_COMP_ENDE", gClienteSinacor.NM_COMP_ENDE, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_BAIRRO", gClienteSinacor.NM_BAIRRO, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_CIDADE", gClienteSinacor.NM_CIDADE, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("SG_ESTADO", gClienteSinacor.SG_ESTADO, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_CEP", gClienteSinacor.CD_CEP, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_DDD_TEL", gClienteSinacor.CD_DDD_TEL, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_TELEFONE", gClienteSinacor.NR_TELEFONE, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_RAMAL", gClienteSinacor.NR_RAMAL, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_DDD_FAX", gClienteSinacor.CD_DDD_FAX, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_FAX", gClienteSinacor.NR_FAX, DataType.NUMBER));
                    campos.Add(new sGeraInsert("PC_CORCOR_PRIN", gClienteSinacor.PC_CORCOR_PRIN, DataType.NUMBER));
                    campos.Add(new sGeraInsert("IN_CART_PROP", gClienteSinacor.IN_CART_PROP, DataType.CHAR));
                    campos.Add(new sGeraInsert("IN_EMITE_NOTA", gClienteSinacor.IN_EMITE_NOTA, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_USUA_INST", gClienteSinacor.CD_USUA_INST, DataType.NUMBER));
                    campos.Add(new sGeraInsert("DV_USUA_INST", gClienteSinacor.DV_USUA_INST, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_CLIE_INST", gClienteSinacor.CD_CLIE_INST, DataType.NUMBER));
                    campos.Add(new sGeraInsert("DV_CLIE_INST", gClienteSinacor.DV_CLIE_INST, DataType.NUMBER));
                    campos.Add(new sGeraInsert("DT_DATINA", gClienteSinacor.DT_DATINA, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("TP_CLIENTE_BOL", gClienteSinacor.TP_CLIENTE_BOL, DataType.NUMBER));
                    campos.Add(new sGeraInsert("IN_SITUAC", gClienteSinacor.IN_SITUAC, DataType.CHAR));
                    campos.Add(new sGeraInsert("NM_MAE", gClienteSinacor.NM_MAE, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_APELIDO", gClienteSinacor.NM_APELIDO, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("IN_ENDE", gClienteSinacor.IN_ENDE, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NR_CELULAR1", gClienteSinacor.NR_CELULAR1, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_CELULAR2", gClienteSinacor.NR_CELULAR2, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_DDD_CELULAR1", gClienteSinacor.CD_DDD_CELULAR1, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_DDD_CELULAR2", gClienteSinacor.CD_DDD_CELULAR2, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_CONTA_COMPL", gClienteSinacor.NR_CONTA_COMPL, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("IN_CORRENTISTA", gClienteSinacor.IN_CORRENTISTA, DataType.CHAR));
                    campos.Add(new sGeraInsert("DT_DOC_IDENT", string.Format("TO_DATE('{0}', 'dd/mm/yyyy hh24:mi:ss')", gClienteSinacor.DT_DOC_IDENT), DataType.DATE));
                    campos.Add(new sGeraInsert("SG_ESTADO_EMIS", gClienteSinacor.SG_ESTADO_EMIS, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_LOC_NASC", gClienteSinacor.NM_LOC_NASC, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_EMPRESA", gClienteSinacor.NM_EMPRESA, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("DS_CARGO", gClienteSinacor.DS_CARGO, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_COSIF", gClienteSinacor.CD_COSIF, DataType.NUMBER));
                    campos.Add(new sGeraInsert("TP_REGCAS", gClienteSinacor.TP_REGCAS, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_ORIGEM", gClienteSinacor.CD_ORIGEM, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_SITUAC", gClienteSinacor.CD_SITUAC, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NM_LOGRADOURO2", gClienteSinacor.NM_LOGRADOURO2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NR_PREDIO2", gClienteSinacor.NR_PREDIO2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_COMP_ENDE2", gClienteSinacor.NM_COMP_ENDE2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_BAIRRO2", gClienteSinacor.NM_BAIRRO2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_CIDADE2", gClienteSinacor.NM_CIDADE2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("SG_ESTADO2", gClienteSinacor.SG_ESTADO2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("SG_PAIS_ENDE2", gClienteSinacor.SG_PAIS_ENDE2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_CEP2", gClienteSinacor.CD_CEP2, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_DDD_TEL2", gClienteSinacor.CD_DDD_TEL2, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_TELEFONE2", gClienteSinacor.NR_TELEFONE2, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_RAMAL2", gClienteSinacor.NR_RAMAL2, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_DDD_FAX2", gClienteSinacor.CD_DDD_FAX2, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_FAX2", gClienteSinacor.NR_FAX2, DataType.NUMBER));
                    campos.Add(new sGeraInsert("IN_TIPO_ENDE2", gClienteSinacor.IN_TIPO_ENDE2, DataType.CHAR));
                    campos.Add(new sGeraInsert("CD_DDD_FAX22", gClienteSinacor.CD_DDD_FAX22, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_FAX22", gClienteSinacor.NR_FAX22, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NM_CONTATO12", gClienteSinacor.NM_CONTATO12, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NM_CONTATO22", gClienteSinacor.NM_CONTATO22, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NR_CELULAR12", gClienteSinacor.NR_CELULAR12, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NR_CELULAR22", gClienteSinacor.NR_CELULAR22, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_DDD_CELULAR12", gClienteSinacor.CD_DDD_CELULAR12, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_DDD_CELULAR22", gClienteSinacor.CD_DDD_CELULAR22, DataType.NUMBER));
                    campos.Add(new sGeraInsert("IN_SITUAC_QUALI", gClienteSinacor.IN_SITUAC_QUALI, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_CNPJ_CVM", gClienteSinacor.CD_CNPJ_CVM, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_CVM", gClienteSinacor.CD_CVM, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("SG_ESTADO_NASC", gClienteSinacor.SG_ESTADO_NASC, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("TP_CONTA", gClienteSinacor.TP_CONTA, DataType.CHAR));
                    campos.Add(new sGeraInsert("CD_VINCULO", gClienteSinacor.CD_VINCULO, DataType.NUMBER));
                    campos.Add(new sGeraInsert("NM_E_MAIL", gClienteSinacor.NM_E_MAIL, DataType.VARCHAR2, false));
                    campos.Add(new sGeraInsert("SOCEFE", gClienteSinacor.SOCEFE, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CLINST", gClienteSinacor.CLINST, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("PERTAX", gClienteSinacor.PERTAX, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CORMAX", gClienteSinacor.CORMAX, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("IN_NAO_RESIDE", gClienteSinacor.IN_NAO_RESIDE, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("IN_INTEGRA_CC", gClienteSinacor.IN_INTEGRA_CC, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("INDPLD", gClienteSinacor.INDPLD, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("INDBRO", gClienteSinacor.INDBRO, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("IN_INTEGRA_CORR", gClienteSinacor.IN_INTEGRA_CORR, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("IN_LIMINAR_IR_OPER", gClienteSinacor.IN_LIMINAR_IR_OPER, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("IN_TRIBUT_ESPECIAL", gClienteSinacor.IN_TRIBUT_ESPECIAL, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("IN_COBRA_MC", gClienteSinacor.IN_COBRA_MC, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("INTLIQ", gClienteSinacor.INTLIQ, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("TP_CLIENTE_BMF", gClienteSinacor.TP_CLIENTE_BMF, DataType.NUMBER));
                    campos.Add(new sGeraInsert("CD_OPERAC_CVM", gClienteSinacor.CD_OPERAC_CVM, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_BANCO_CLICTA_PRINCIPAL", gClienteSinacor.CD_BANCO_CLICTA_PRINCIPAL, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_AGENCIA_CLICTA_PRINCIPAL", gClienteSinacor.CD_AGENCIA_CLICTA_PRINCIPAL, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NR_CONTA_CLICTA_PRINCIPAL", gClienteSinacor.NR_CONTA_CLICTA_PRINCIPAL, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("DV_CONTA_CLICTA_PRINCIPAL", gClienteSinacor.DV_CONTA_CLICTA_PRINCIPAL, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_BANCO_CLICTA_A", gClienteSinacor.CD_BANCO_CLICTA_A, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_AGENCIA_CLICTA_A", gClienteSinacor.CD_AGENCIA_CLICTA_A, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NR_CONTA_CLICTA_A1", gClienteSinacor.NR_CONTA_CLICTA_A1, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("DV_CONTA_CLICTA_A1", gClienteSinacor.DV_CONTA_CLICTA_A1, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NR_CONTA_CLICTA_A2", gClienteSinacor.NR_CONTA_CLICTA_A2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("DV_CONTA_CLICTA_A2", gClienteSinacor.DV_CONTA_CLICTA_A2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_BANCO_CLICTA_B", gClienteSinacor.CD_BANCO_CLICTA_B, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_AGENCIA_CLICTA_B", gClienteSinacor.CD_AGENCIA_CLICTA_B, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NR_CONTA_CLICTA_B1", gClienteSinacor.NR_CONTA_CLICTA_B1, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("DV_CONTA_CLICTA_B1", gClienteSinacor.DV_CONTA_CLICTA_B1, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("NR_CONTA_CLICTA_B2", gClienteSinacor.NR_CONTA_CLICTA_B2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("DV_CONTA_CLICTA_B2", gClienteSinacor.DV_CONTA_CLICTA_B2, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_AGENTE", gClienteSinacor.CD_AGENTE, DataType.NUMBER));
                    campos.Add(new sGeraInsert("SG_PAIS_ENDE1", gClienteSinacor.SG_PAIS_ENDE1, DataType.VARCHAR2));
                    campos.Add(new sGeraInsert("CD_COSIF_CI", gClienteSinacor.CD_COSIF_CI, DataType.NUMBER));
                    campos.Add(new sGeraInsert("TP_INVESTIDOR", gClienteSinacor.TP_INVESTIDOR, DataType.NUMBER));

                    #endregion

                    comando = GeraInsert("TSCIMPCLIH", campos);
                    DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, comando.Replace(Environment.NewLine, ""));
                    lDbCommand.Transaction = lTransaction;
                    lAcessaDados.ExecuteNonQuery(lDbCommand, conn);

                    #endregion

                    #region [Passo3]

                    using (DbCommand lDbCommandEXECCONH = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PROC_CLIECOH_V2.EXECCONH"))
                    {
                        lAcessaDados.AddInParameter(lDbCommandEXECCONH, "V_PARAM", DbType.Byte, 'S');
                        lDbCommandEXECCONH.Transaction = lTransaction;
                        lAcessaDados.ExecuteNonQuery(lDbCommandEXECCONH, conn);
                    }

                    #endregion

                    #region [Passo4]

                    using (DbCommand lDbCommandEXECIMPH = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PROC_IMPCLIH_V2.EXECIMPH"))
                    {
                        lAcessaDados.AddInParameter(lDbCommandEXECIMPH, "CD_EMPRESA", DbType.Int32, 227);
                        lAcessaDados.AddInParameter(lDbCommandEXECIMPH, "CD_USUARIO", DbType.Int32, 1);
                        lAcessaDados.AddInParameter(lDbCommandEXECIMPH, "TP_OCORRENCIA", DbType.AnsiString, "I");
                        lAcessaDados.AddInParameter(lDbCommandEXECIMPH, "CD_CLIENTE_PADRAO", DbType.Int32, 55555);
                        lDbCommandEXECIMPH.Transaction = lTransaction;
                        lAcessaDados.ExecuteNonQuery(lDbCommandEXECIMPH, conn);
                    }

                    #endregion

                    #region [Atualiza Endereços das Atividades]

                    if (!gPrimeiraExportacao)
                    {
                        ///Antes Refazia perfeitamente os relacionamentos das atividades com os endereços
                        ///Agora Faz com que os endereços nas atividades seja o nr_seq_ende =1

                        lDbCommand.Parameters.Clear();
                        lDbCommand = lAcessaDados.CreateCommand(lTransaction, CommandType.StoredProcedure, "prc_relaciona_endereco_cus");
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, 1);
                        lDbCommand.Transaction = lTransaction;
                        lAcessaDados.ExecuteNonQuery(lDbCommand, lTransaction);

                        lDbCommand.Parameters.Clear();
                        lDbCommand = lAcessaDados.CreateCommand(lTransaction, CommandType.StoredProcedure, "prc_relaciona_endereco_cc");
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, 1);
                        lDbCommand.Transaction = lTransaction;
                        lAcessaDados.ExecuteNonQuery(lDbCommand, lTransaction);

                        lDbCommand.Parameters.Clear();
                        lDbCommand = lAcessaDados.CreateCommand(lTransaction, CommandType.StoredProcedure, "prc_relaciona_endereco_bol");
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, 1);
                        lDbCommand.Transaction = lTransaction;
                        lAcessaDados.ExecuteNonQuery(lDbCommand, lTransaction);

                        lDbCommand.Parameters.Clear();
                        lDbCommand = lAcessaDados.CreateCommand(lTransaction, CommandType.StoredProcedure, "prc_relaciona_endereco_bmf");
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, 1);
                        lDbCommand.Transaction = lTransaction;
                        lAcessaDados.ExecuteNonQuery(lDbCommand, lTransaction);

                        #region Código Antigo para Endereço

                        /// ver se os endereços que eram das atividades foram recadastrados
                        //int? nrseqCustodia = null;
                        //int? nrseqcc = null;
                        //int? nrseqbol = null;
                        //int? nrseqbmf = null;

                        //if (null != gEnderecoCustodia & null != gEnderecoCustodia.CD_CEP && gEnderecoCustodia.CD_CEP.Trim().Length != 0)
                        //{
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_nr_endereco_atividades");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP", DbType.UInt32, gEnderecoCustodia.CD_CEP.Substring(0, 5).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP_EXT", DbType.UInt32, gEnderecoCustodia.CD_CEP.Substring(5, 3).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_LOGRADOURO", DbType.AnsiString, gEnderecoCustodia.NM_LOGRADOURO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_PREDIO", DbType.AnsiString, gEnderecoCustodia.NR_PREDIO);
                        //        lAcessaDados.AddOutParameter(lDbCommand, "pNR_SEQ_ENDE", DbType.UInt32, 8);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //        nrseqCustodia = lDbCommand.Parameters["pNR_SEQ_ENDE"].Value.DBToInt32();

                        //    if (null != nrseqCustodia || 0 == nrseqCustodia)
                        //    {
                        //        //Refaz o relacionamento
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_relaciona_endereco_cus");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, nrseqCustodia);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //    }
                        //    else
                        //    {
                        //        // ReCadastra e refaz o relacionamento
                        //        seqendereco++;
                        //        //Inserir endereco
                        //        SinacorExportarEnderecoInfo item = gEnderecoCustodia;
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_exp_endereco");

                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP", DbType.UInt32, item.CD_CEP.Substring(0, 5).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP_EXT", DbType.UInt32, item.CD_CEP.Substring(5, 3).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR1", DbType.UInt32, item.CD_DDD_CELULAR1.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR2", DbType.UInt32, item.CD_DDD_CELULAR2.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_FAX", DbType.UInt32, item.CD_DDD_FAX.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_TEL", DbType.UInt32, item.CD_DDD_TEL.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pIN_ENDE", DbType.AnsiString, item.IN_ENDE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_BAIRRO", DbType.AnsiString, item.NM_BAIRRO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR1", DbType.UInt32, item.NR_CELULAR1.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR2", DbType.UInt32, item.NR_CELULAR2.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CIDADE", DbType.AnsiString, item.NM_CIDADE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_COMP_ENDE", DbType.AnsiString, item.NM_COMP_ENDE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_FAX", DbType.UInt32, item.NR_FAX.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO1", DbType.AnsiString, item.NM_CONTATO1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO2", DbType.AnsiString, item.NM_CONTATO2);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_LOGRADOURO", DbType.AnsiString, item.NM_LOGRADOURO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_PREDIO", DbType.AnsiString, item.NR_PREDIO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_RAMAL", DbType.UInt32, item.NR_RAMAL.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_TELEFONE", DbType.UInt32, item.NR_TELEFONE.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pSG_ESTADO", DbType.AnsiString, item.SG_ESTADO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pSG_PAIS_ENDE1", DbType.AnsiString, item.SG_PAIS_ENDE1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pseqendereco", DbType.UInt32, seqendereco.DBToInt32());

                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);

                        //        //Refaz o relacionamento
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_relaciona_endereco_cus");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, seqendereco);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);

                        //    }
                        //}

                        //if (null != gEnderecoCC & null != gEnderecoCC.CD_CEP && gEnderecoCC.CD_CEP.Trim().Length != 0)
                        //{
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_nr_endereco_atividades");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP", DbType.UInt32, gEnderecoCC.CD_CEP.Substring(0, 5).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP_EXT", DbType.UInt32, gEnderecoCC.CD_CEP.Substring(5, 3).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_LOGRADOURO", DbType.AnsiString, gEnderecoCC.NM_LOGRADOURO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_PREDIO", DbType.AnsiString, gEnderecoCC.NR_PREDIO);
                        //        lAcessaDados.AddOutParameter(lDbCommand, "pNR_SEQ_ENDE", DbType.UInt32, 8);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //        nrseqcc = lDbCommand.Parameters["pNR_SEQ_ENDE"].Value.DBToInt32();

                        //    if (null != nrseqcc || 0 == nrseqcc)
                        //    {
                        //        //Refaz o relacionamento
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_relaciona_endereco_cc");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, nrseqcc);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //    }
                        //    else
                        //    {
                        //        seqendereco++;
                        //        //Inserir endereco
                        //        SinacorExportarEnderecoInfo item = gEnderecoCC;
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_exp_endereco");

                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP", DbType.UInt32, item.CD_CEP.Substring(0, 5).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP_EXT", DbType.UInt32, item.CD_CEP.Substring(5, 3).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR1", DbType.UInt32, item.CD_DDD_CELULAR1.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR2", DbType.UInt32, item.CD_DDD_CELULAR2.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_FAX", DbType.UInt32, item.CD_DDD_FAX.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_TEL", DbType.UInt32, item.CD_DDD_TEL.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pIN_ENDE", DbType.AnsiString, item.IN_ENDE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_BAIRRO", DbType.AnsiString, item.NM_BAIRRO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR1", DbType.UInt32, item.NR_CELULAR1.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR2", DbType.UInt32, item.NR_CELULAR2.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CIDADE", DbType.AnsiString, item.NM_CIDADE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_COMP_ENDE", DbType.AnsiString, item.NM_COMP_ENDE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_FAX", DbType.UInt32, item.NR_FAX.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO1", DbType.AnsiString, item.NM_CONTATO1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO2", DbType.AnsiString, item.NM_CONTATO2);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_LOGRADOURO", DbType.AnsiString, item.NM_LOGRADOURO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_PREDIO", DbType.AnsiString, item.NR_PREDIO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_RAMAL", DbType.UInt32, item.NR_RAMAL.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_TELEFONE", DbType.UInt32, item.NR_TELEFONE.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pSG_ESTADO", DbType.AnsiString, item.SG_ESTADO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pSG_PAIS_ENDE1", DbType.AnsiString, item.SG_PAIS_ENDE1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pseqendereco", DbType.UInt32, seqendereco.DBToInt32());

                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);

                        //        //Refaz o relacionamento
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_relaciona_endereco_cc");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, seqendereco);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //    }
                        //}

                        //if (null != gEnderecoBol & null != gEnderecoBol.CD_CEP && gEnderecoBol.CD_CEP.Trim().Length != 0)
                        //{
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_nr_endereco_atividades");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP", DbType.UInt32, gEnderecoBol.CD_CEP.Substring(0, 5).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP_EXT", DbType.UInt32, gEnderecoBol.CD_CEP.Substring(5, 3).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_LOGRADOURO", DbType.AnsiString, gEnderecoBol.NM_LOGRADOURO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_PREDIO", DbType.AnsiString, gEnderecoBol.NR_PREDIO);
                        //        lAcessaDados.AddOutParameter(lDbCommand, "pNR_SEQ_ENDE", DbType.UInt32, 8);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //        nrseqbol = lDbCommand.Parameters["pNR_SEQ_ENDE"].Value.DBToInt32();

                        //    if (null != nrseqbol || 0 == nrseqbol)
                        //    {
                        //        //Refaz o relacionamento
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_relaciona_endereco_bol");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, nrseqbol);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //    }
                        //    else
                        //    {
                        //        seqendereco++;
                        //        //Inserir endereco
                        //        SinacorExportarEnderecoInfo item = gEnderecoBol;
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_exp_endereco");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP", DbType.UInt32, item.CD_CEP.Substring(0, 5).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP_EXT", DbType.UInt32, item.CD_CEP.Substring(5, 3).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR1", DbType.UInt32, item.CD_DDD_CELULAR1.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR2", DbType.UInt32, item.CD_DDD_CELULAR2.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_FAX", DbType.UInt32, item.CD_DDD_FAX.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_TEL", DbType.UInt32, item.CD_DDD_TEL.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pIN_ENDE", DbType.AnsiString, item.IN_ENDE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_BAIRRO", DbType.AnsiString, item.NM_BAIRRO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR1", DbType.UInt32, item.NR_CELULAR1.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR2", DbType.UInt32, item.NR_CELULAR2.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CIDADE", DbType.AnsiString, item.NM_CIDADE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_COMP_ENDE", DbType.AnsiString, item.NM_COMP_ENDE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_FAX", DbType.UInt32, item.NR_FAX.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO1", DbType.AnsiString, item.NM_CONTATO1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO2", DbType.AnsiString, item.NM_CONTATO2);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_LOGRADOURO", DbType.AnsiString, item.NM_LOGRADOURO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_PREDIO", DbType.AnsiString, item.NR_PREDIO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_RAMAL", DbType.UInt32, item.NR_RAMAL.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_TELEFONE", DbType.UInt32, item.NR_TELEFONE.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pSG_ESTADO", DbType.AnsiString, item.SG_ESTADO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pSG_PAIS_ENDE1", DbType.AnsiString, item.SG_PAIS_ENDE1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pseqendereco", DbType.UInt32, seqendereco.DBToInt32());

                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);

                        //        //Refaz o relacionamento
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_relaciona_endereco_bol");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, seqendereco);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //    }
                        //}

                        //if (null != gEnderecoBmf & null != gEnderecoBmf.CD_CEP && gEnderecoBmf.CD_CEP.Trim().Length != 0)
                        //{
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_nr_endereco_atividades");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP", DbType.UInt32, gEnderecoBmf.CD_CEP.Substring(0, 5).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP_EXT", DbType.UInt32, gEnderecoBmf.CD_CEP.Substring(5, 3).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_LOGRADOURO", DbType.AnsiString, gEnderecoBmf.NM_LOGRADOURO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_PREDIO", DbType.AnsiString, gEnderecoBmf.NR_PREDIO);
                        //        lAcessaDados.AddOutParameter(lDbCommand, "pNR_SEQ_ENDE", DbType.UInt32, 8);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //        nrseqbmf = lDbCommand.Parameters["pNR_SEQ_ENDE"].Value.DBToInt32();

                        //    if (null != nrseqbmf || 0 == nrseqbmf)
                        //    {
                        //        //Refaz o relacionamento
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_relaciona_endereco_bmf");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, nrseqbmf);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //    }
                        //    else
                        //    {
                        //        seqendereco++;
                        //        //Inserir endereco
                        //        SinacorExportarEnderecoInfo item = gEnderecoBmf;
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_exp_endereco");

                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP", DbType.UInt32, item.CD_CEP.Substring(0, 5).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP_EXT", DbType.UInt32, item.CD_CEP.Substring(5, 3).DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR1", DbType.UInt32, item.CD_DDD_CELULAR1.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR2", DbType.UInt32, item.CD_DDD_CELULAR2.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_FAX", DbType.UInt32, item.CD_DDD_FAX.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_TEL", DbType.UInt32, item.CD_DDD_TEL.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pIN_ENDE", DbType.AnsiString, item.IN_ENDE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_BAIRRO", DbType.AnsiString, item.NM_BAIRRO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR1", DbType.UInt32, item.NR_CELULAR1.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR2", DbType.UInt32, item.NR_CELULAR2.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CIDADE", DbType.AnsiString, item.NM_CIDADE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_COMP_ENDE", DbType.AnsiString, item.NM_COMP_ENDE);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_FAX", DbType.UInt32, item.NR_FAX.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO1", DbType.AnsiString, item.NM_CONTATO1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO2", DbType.AnsiString, item.NM_CONTATO2);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNM_LOGRADOURO", DbType.AnsiString, item.NM_LOGRADOURO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_PREDIO", DbType.AnsiString, item.NR_PREDIO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_RAMAL", DbType.UInt32, item.NR_RAMAL.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_TELEFONE", DbType.UInt32, item.NR_TELEFONE.DBToInt32());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pSG_ESTADO", DbType.AnsiString, item.SG_ESTADO);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pSG_PAIS_ENDE1", DbType.AnsiString, item.SG_PAIS_ENDE1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pseqendereco", DbType.UInt32, seqendereco.DBToInt32());

                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);

                        //        //Refaz o relacionamento
                        //        lDbCommand.Parameters.Clear();
                        //        lDbCommand = lAcessaDados.CreateCommand(_Transaction, CommandType.StoredProcedure, "prc_relaciona_endereco_bmf");
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, lClienteSinacor.CD_CPFCGC);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, lClienteSinacor.DT_NASC_FUND.DBToDateTime());
                        //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                        //        lAcessaDados.AddInParameter(lDbCommand, "pNR_SEQ_ENDE_CORRESP", DbType.UInt32, seqendereco);
                        //        lDbCommand.Transaction = _Transaction;
                        //        lAcessaDados.ExecuteNonQuery(lDbCommand, _Transaction);
                        //    }
                        //}

                        #endregion
                    }

                    #endregion

                    #region [Atualiza Bovespa]

                    if (gPrimeiraExportacao)
                    {
                        string cmd = string.Format("UPDATE TSCPARM SET CD_USUARIO = {0}, CD_CLIE_BASE = CD_CLIE_BASE + 1", GetCdUsuario.ToString());
                        DbCommand lDbCommandAtualizaBovespa = lAcessaDados.CreateCommand(CommandType.Text, cmd);
                        lDbCommandAtualizaBovespa.Transaction = lTransaction;
                        lAcessaDados.ExecuteNonQuery(lDbCommandAtualizaBovespa, conn);
                    }

                    #endregion

                    // efetuar rollback se tiver erro...
                    #region [Passo5]

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append("SELECT * FROM TSCERROH");
                    DbCommand lDbCommandError = lAcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                    lDbCommandError.Transaction = lTransaction;
                    DataTable dtError = lAcessaDados.ExecuteDbDataTable(lDbCommandError, conn);

                    if (dtError.Rows.Count > 0)
                    {
                        lTransaction.Rollback();
                        lTransaction.Dispose();
                        SinacorExportacaoRetornoFalhaBovespaInfo lLinhaErro;

                        foreach (DataRow item in dtError.Rows)
                        {
                            lLinhaErro = new SinacorExportacaoRetornoFalhaBovespaInfo();

                            lLinhaErro.CD_CLIENTE = item["CD_CLIENTE"].DBToInt32();
                            lLinhaErro.CD_CPFCGC = item["CD_CPFCGC"].DBToInt64();
                            lLinhaErro.CD_EXTERNO = item["CD_EXTERNO"].DBToString();
                            lLinhaErro.DS_AUX = item["DS_AUX"].DBToString();
                            lLinhaErro.DS_OBS = item["DS_OBS"].DBToString();
                            lLinhaErro.DT_IMPORTA = item["DT_IMPORTA"].DBToDateTime();
                            lLinhaErro.DT_NASC_FUND = item["DT_NASC_FUND"].DBToDateTime();
                            lLinhaErro.NM_CLIENTE = item["NM_CLIENTE"].DBToString();

                            lRetorno.Add(lLinhaErro);
                        }
                    }
                    else
                    {
                        lTransaction.Commit();
                        lTransaction.Dispose();
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    lTransaction.Rollback();
                    lTransaction.Dispose();
                    throw (ex);
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Atualiza cliente no sistema de cadastro após exportação
        /// </summary>
        /// <returns>Erros que ocorreram na atualização</returns>
        private List<SinacorExportacaoRetornoFalhaSistemaCadastroInfo> AtualizaCliente()
        {
            int pIdCliente = gCliente.IdCliente.Value;
            int pCdCodigo = gCodigo.Value;
            int pCdAssessor = gAssessor;
            int lContErroTransaction = 0;
            var lRetorno = new List<SinacorExportacaoRetornoFalhaSistemaCadastroInfo>();

            try
            {
                SalvarObjetoRequest<ClienteInfo> lClienteAntes = new SalvarObjetoRequest<ClienteInfo>();

                lClienteAntes.DescricaoUsuarioLogado = "Processo de Exportação";

                lClienteAntes.Objeto = gCliente;

                if (gPrimeiraExportacao)
                {
                    lClienteAntes.Objeto.DtPrimeiraExportacao = DateTime.Now;
                }

                lClienteAntes.Objeto.StPasso = 4;

                lClienteAntes.Objeto.DtUltimaExportacao = DateTime.Now;
                lClienteAntes.Objeto.DadosClienteNaoOperaPorContaPropria = null;

                ClienteDbLib.SalvarCliente(lClienteAntes);
            }
            catch (Exception ex)
            {
                var lFalha = new SinacorExportacaoRetornoFalhaSistemaCadastroInfo();
                lFalha.Mensagem = ex.Message;
                lFalha.Tabela = "TB_CLIENTE";
                lRetorno.Add(lFalha);
                lContErroTransaction++;
            }

            try
            {
                if (gPrimeiraExportacao)
                {
                    SalvarObjetoRequest<ClienteContaInfo> lContaNova = new SalvarObjetoRequest<ClienteContaInfo>();

                    lContaNova.DescricaoUsuarioLogado = "Processo de Exportação";

                    lContaNova.Objeto = new ClienteContaInfo();

                    lContaNova.Objeto.CdAssessor = gAssessor;
                    lContaNova.Objeto.CdCodigo = gCodigo.Value;
                    lContaNova.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BOL;
                    lContaNova.Objeto.IdCliente = gCliente.IdCliente.Value;
                    lContaNova.Objeto.StAtiva = true;
                    lContaNova.Objeto.StContaInvestimento = false;
                    lContaNova.Objeto.StPrincipal = true;

                    ClienteDbLib.SalvarClienteConta(lContaNova);

                    lContaNova.Objeto = new ClienteContaInfo();

                    lContaNova.Objeto.CdAssessor = gAssessor;
                    lContaNova.Objeto.CdCodigo = gCodigo.Value;
                    lContaNova.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CC;
                    lContaNova.Objeto.IdCliente = gCliente.IdCliente.Value;
                    lContaNova.Objeto.StAtiva = true;
                    lContaNova.Objeto.StContaInvestimento = false;
                    lContaNova.Objeto.StPrincipal = false;

                    ClienteDbLib.SalvarClienteConta(lContaNova);

                    lContaNova.Objeto = new ClienteContaInfo();

                    lContaNova.Objeto.CdAssessor = gAssessor;
                    lContaNova.Objeto.CdCodigo = gCodigo.Value;
                    lContaNova.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CUS;
                    lContaNova.Objeto.IdCliente = gCliente.IdCliente.Value;
                    lContaNova.Objeto.StAtiva = true;
                    lContaNova.Objeto.StContaInvestimento = false;
                    lContaNova.Objeto.StPrincipal = false;

                    ClienteDbLib.SalvarClienteConta(lContaNova);
                }
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaSistemaCadastroInfo lFalha = new SinacorExportacaoRetornoFalhaSistemaCadastroInfo();
                lFalha.Mensagem = ex.Message;
                lFalha.Tabela = "TB_CLIENTE_CONTA";
                lRetorno.Add(lFalha);
                lContErroTransaction++;
            }

            try
            {
                if (gPrimeiraExportacao)
                {
                    SalvarObjetoRequest<ClienteAtivarInativarInfo> lAtivarCliGer = new SalvarObjetoRequest<ClienteAtivarInativarInfo>();
                    lAtivarCliGer.DescricaoUsuarioLogado = "Porcesso de Exportação";
                    lAtivarCliGer.Objeto = new ClienteAtivarInativarInfo();
                    lAtivarCliGer.Objeto.IdCliente = gCliente.IdCliente.Value;
                    lAtivarCliGer.Objeto.StClienteGeralAtivo = true;
                    lAtivarCliGer.Objeto.StLoginAtivo = true;
                    lAtivarCliGer.Objeto.StHbAtivo = true;

                    ClienteDbLib.SalvarClienteAtivarInativar(lAtivarCliGer);
                }
            }
            catch (Exception ex)
            {
                var lFalha = new SinacorExportacaoRetornoFalhaSistemaCadastroInfo();
                lFalha.Mensagem = ex.Message;
                lFalha.Tabela = "TB_CLIENTE_CONTA";
                lRetorno.Add(lFalha);
                lContErroTransaction++;
            }

            Conexao._ConnectionStringName = gConexaoSinacorCorrwin;
            using (DbConnection conn = Conexao.CreateIConnection())
            {
                var lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gConexaoSinacorCorrwin;

                conn.Open();
                DbTransaction lDbTransaction = conn.BeginTransaction();

                try
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, "prc_cliente_email_upd"))
                    {
                        lAcessaDados.ClearParameters(lDbCommand);
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gCliente.DsCpfCnpj.DBToInt64());
                        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gCliente.DtNascimentoFundacao.Value.DBToDateTime());
                        lAcessaDados.AddInParameter(lDbCommand, "pNM_E_MAIL", DbType.String, gCliente.DsEmail);

                        lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                    }
                }
                catch (Exception ex)
                {
                    var lFalha = new SinacorExportacaoRetornoFalhaSistemaCadastroInfo();
                    lFalha.Mensagem = ex.Message;
                    lFalha.Tabela = "TSCENDE";
                    lRetorno.Add(lFalha);
                    lContErroTransaction++;
                }

                try
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, "prc_tipo_investidor_upd"))
                    {
                        lAcessaDados.ClearParameters(lDbCommand);

                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gCliente.DsCpfCnpj.DBToInt64());
                        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gCliente.DtNascimentoFundacao.Value.DBToDateTime());
                        lAcessaDados.AddInParameter(lDbCommand, "pTP_INVESTIDOR", DbType.Int64, gClienteSinacor.TP_INVESTIDOR);

                        lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                    }
                }
                catch (Exception ex)
                {
                    var lFalha = new SinacorExportacaoRetornoFalhaSistemaCadastroInfo();
                    lFalha.Mensagem = ex.Message;
                    lFalha.Tabela = "TSCCLIGER";
                    lRetorno.Add(lFalha);
                    lContErroTransaction++;
                }

                try
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, "prc_cliente_nire_upd"))
                    {
                        lAcessaDados.ClearParameters(lDbCommand);

                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gCliente.DsCpfCnpj.DBToInt64());
                        lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gCliente.DtNascimentoFundacao.Value.DBToDateTime());
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_NIRE", DbType.Int64, gCliente.CdNire);

                        lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                    }
                }
                catch (Exception ex)
                {
                    var lFalha = new SinacorExportacaoRetornoFalhaSistemaCadastroInfo();
                    lFalha.Mensagem = ex.Message;
                    lFalha.Tabela = "TSCCLICOMP";
                    lRetorno.Add(lFalha);
                    lContErroTransaction++;
                }

                if (lContErroTransaction == 0)
                    lDbTransaction.Commit();
                else
                    lDbTransaction.Rollback();
            }

            return lRetorno;
        }

        /// <summary>
        /// Exporta dados não exportados pelo processo CliExterno
        /// </summary>
        /// <returns></returns>
        private List<SinacorExportacaoRetornoFalhaComplementosInfo> ExportarComplementos()
        {
            var lRetorno = new List<SinacorExportacaoRetornoFalhaComplementosInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gConexaoSinacorConsulta;
            DbCommand lDbCommand = null;

            #region [ISS]
            //Altera ISS - Precisa estar na mesma transação da PROC_IMPCLIH.EXECIMPH

            try
            {
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_iss");
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, gCodigo.Value);
                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Não foi possível alterar a forma de cobrança de ISS";
                erro.Procedure = "prc_exp_iss";
                erro.Tabela = "tscclibol";
                lRetorno.Add(erro);
            }
            #endregion

            #region [SPF]

            int SFPTotOutrosRendimentos = -1;
            int SFPTotBensImoveis = -1;
            int SFPTotBensMoveis = -1;
            int SFPTotAplicacoesFinanceiras = -1;
            int SFPTotSalarioProLabore = -1;
            int SFPTotCapitalSocial = -1;
            int SFPTotPatrimonioLiquido = -1;

            try
            {
                SFPTotOutrosRendimentos = System.Configuration.ConfigurationManager.AppSettings["SFPTotOutrosRendimentos"].DBToInt32();
                SFPTotBensImoveis = System.Configuration.ConfigurationManager.AppSettings["SFPTotBensImoveis"].DBToInt32();
                SFPTotBensMoveis = System.Configuration.ConfigurationManager.AppSettings["SFPTotBensMoveis"].DBToInt32();
                SFPTotAplicacoesFinanceiras = System.Configuration.ConfigurationManager.AppSettings["SFPTotAplicacoesFinanceiras"].DBToInt32();
                SFPTotSalarioProLabore = System.Configuration.ConfigurationManager.AppSettings["SFPTotSalarioProLabore"].DBToInt32();
                SFPTotCapitalSocial = System.Configuration.ConfigurationManager.AppSettings["SFPTotCapitalSocial"].DBToInt32();
                SFPTotPatrimonioLiquido = System.Configuration.ConfigurationManager.AppSettings["SFPTotPatrimonioLiquido"].DBToInt32();
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Não foi possível ler os tipos de Situação Financeira Patrimonial no arquivo de Configuração";
                erro.Procedure = "WEB.CONFIG";
                erro.Tabela = "WEB.CONFIG";
                lRetorno.Add(erro);
            }

            try
            {
                lDbCommand.Parameters.Clear();
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_del_sfp");
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Não foi possível excluir os registros de Situação Financeira Patrimonial";
                erro.Procedure = "prc_exp_del_sfp";
                erro.Tabela = "tscsfp";
                lRetorno.Add(erro);
            }

            if (null != gSfp && null != gSfp.VlTotalBensImoveis && gSfp.VlTotalBensImoveis > 0)
            {
                // vl_totalbensimoveis - 23
                try
                {
                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_sfp");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_SFPGRUPO", DbType.Int32, SFPTotBensImoveis);
                    lAcessaDados.AddInParameter(lDbCommand, "pVL_BEN", DbType.Decimal, gSfp.VlTotalBensImoveis);
                    lAcessaDados.AddInParameter(lDbCommand, "pTM_STAMP", DbType.DateTime, gSfp.DtAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pDS_BEN", DbType.AnsiString, "Total de Bens imoveis".ToUpper());
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao tentar incluir Total de Bens Imóveis na Situação Financeira Patrimonial";
                    erro.Procedure = "prc_exp_sfp";
                    erro.Tabela = "tscsfp";
                    lRetorno.Add(erro);
                }
            }

            if (null != gSfp && null != gSfp.VlTotalBensMoveis && gSfp.VlTotalBensMoveis > 0)
            {
                //vl_totalbensmoveis - 25
                try
                {
                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_sfp");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_SFPGRUPO", DbType.Int32, SFPTotBensMoveis);
                    lAcessaDados.AddInParameter(lDbCommand, "pVL_BEN", DbType.Decimal, gSfp.VlTotalBensMoveis);
                    lAcessaDados.AddInParameter(lDbCommand, "pTM_STAMP", DbType.DateTime, gSfp.DtAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pDS_BEN", DbType.AnsiString, "Total de Bens Moveis".ToUpper());


                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao tentar incluir Total de Bens Móveis na Situação Financeira Patrimonial";
                    erro.Procedure = "prc_exp_sfp";
                    erro.Tabela = "tscsfp";
                    lRetorno.Add(erro);
                }
            }

            if (null != gSfp && null != gSfp.VlTotalAplicacaoFinanceira && gSfp.VlTotalAplicacaoFinanceira > 0)
            {
                // vl_totalaplicacaofinanceira - 26
                try
                {
                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_sfp");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_SFPGRUPO", DbType.Int32, SFPTotAplicacoesFinanceiras);
                    lAcessaDados.AddInParameter(lDbCommand, "pVL_BEN", DbType.Decimal, gSfp.VlTotalAplicacaoFinanceira);
                    lAcessaDados.AddInParameter(lDbCommand, "pTM_STAMP", DbType.DateTime, gSfp.DtAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pDS_BEN", DbType.AnsiString, "Total de Aplicacoes Financeiras".ToUpper());

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao tentar incluir Total de Aplicações Financeiras na Situação Financeira Patrimonial ";
                    erro.Procedure = "prc_exp_sfp";
                    erro.Tabela = "tscsfp";
                    lRetorno.Add(erro);
                }
            }

            if (null != gSfp && null != gSfp.VlTotalSalarioProLabore && gSfp.VlTotalSalarioProLabore > 0)
            {
                // vl_totalsalarioprolabore - 27
                try
                {
                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_sfp");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_SFPGRUPO", DbType.Int32, SFPTotSalarioProLabore);
                    lAcessaDados.AddInParameter(lDbCommand, "pVL_BEN", DbType.Decimal, gSfp.VlTotalSalarioProLabore);
                    lAcessaDados.AddInParameter(lDbCommand, "pTM_STAMP", DbType.DateTime, gSfp.DtAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pDS_BEN", DbType.AnsiString, "Total de Salario/Pro-labore".ToUpper());

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao tentar incluir Total de Salário/Prolabore na Situação Financeira Patrimonial";
                    erro.Procedure = "prc_exp_sfp";
                    erro.Tabela = "tscsfp";
                    lRetorno.Add(erro);
                }
            }

            if (null != gSfp && null != gSfp.VlTotalOutrosRendimentos && gSfp.VlTotalOutrosRendimentos > 0)
            {
                //   vl_totaloutrosrendimentos - 24
                try
                {
                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_sfp");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_SFPGRUPO", DbType.Int32, SFPTotOutrosRendimentos);
                    lAcessaDados.AddInParameter(lDbCommand, "pVL_BEN", DbType.Decimal, gSfp.VlTotalOutrosRendimentos);
                    lAcessaDados.AddInParameter(lDbCommand, "pTM_STAMP", DbType.DateTime, gSfp.DtAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pDS_BEN", DbType.AnsiString, "Total de Outros Rendimentos".ToUpper());
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao tentar incluir Total de Outros Rendimentos na Situação Financeira Patrimonial";
                    erro.Procedure = "prc_exp_sfp";
                    erro.Tabela = "tscsfp";
                    lRetorno.Add(erro);
                }
            }

            if (null != gSfp && null != gSfp.VTotalCapitalSocial && gSfp.VTotalCapitalSocial > 0)
            {
                //vl_capitalsocial - 28
                //dt_capitalsocial
                try
                {
                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_sfp");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_SFPGRUPO", DbType.Int32, SFPTotCapitalSocial);
                    lAcessaDados.AddInParameter(lDbCommand, "pVL_BEN", DbType.Decimal, gSfp.VTotalCapitalSocial);
                    lAcessaDados.AddInParameter(lDbCommand, "pTM_STAMP", DbType.DateTime, gSfp.DtAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pDS_BEN", DbType.AnsiString, "Total de Capital Social em: ".ToUpper() + gSfp.DtCapitalSocial.Value.ToString("dd/MM/yyyy"));

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao tentar incluir Total de Outros Rendimentos na Situação Financeira Patrimonial";
                    erro.Procedure = "prc_exp_sfp";
                    erro.Tabela = "tscsfp";
                    lRetorno.Add(erro);
                }
            }

            if (null != gSfp && null != gSfp.VlTotalPatrimonioLiquido && gSfp.VlTotalPatrimonioLiquido > 0)
            {
                //VlTotalPatrimonioLiquido 
                try
                {
                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_sfp");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_SFPGRUPO", DbType.Int32, SFPTotPatrimonioLiquido);
                    lAcessaDados.AddInParameter(lDbCommand, "pVL_BEN", DbType.Decimal, gSfp.VlTotalPatrimonioLiquido);
                    lAcessaDados.AddInParameter(lDbCommand, "pTM_STAMP", DbType.DateTime, gSfp.DtAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pDS_BEN", DbType.AnsiString, "Patrimonio Liquido em: ".ToUpper() + gSfp.DtPatrimonioLiquido.Value.ToString("dd/MM/yyyy"));

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao tentar incluir Total de Patrimônio Liquido na Situação Financeira Patrimonial";
                    erro.Procedure = "prc_exp_sfp";
                    erro.Tabela = "tscsfp";
                    lRetorno.Add(erro);
                }
            }

            #endregion

            #region [TSCCLIBOL]

            //try
            //{
            //    lDbCommand.Parameters.Clear();
            //    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_bol");
            //    lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, gCodigo.Value);
            //    lAcessaDados.AddInParameter(lDbCommand, "pTP_INVESTIDOR", DbType.Int32, int.Parse(gClienteSinacor.TP_INVESTIDOR));
            //    lAcessaDados.ExecuteNonQuery(lDbCommand);

            //}
            //catch (Exception ex)
            //{
            //    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
            //    erro.Excessao = ex.Message;
            //    erro.Mensagem = "Erro ao Inserir dados na Atividade Bolsa.";
            //    erro.Procedure = "prc_exp_bol";
            //    erro.Tabela = "tscclibol";
            //    lRetorno.Add(erro);
            //}

            #endregion

            #region [TSCCBASAG]

            try
            {
                lDbCommand.Parameters.Clear();
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_basag");
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, gCodigo.Value);
                lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESSOR", DbType.Int32, int.Parse(gClienteSinacor.CD_ASSESSOR));
                lAcessaDados.ExecuteNonQuery(lDbCommand);

            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Erro ao Gravar Assessor na Atividade Bolsa.";
                erro.Procedure = "prc_exp_basag";
                erro.Tabela = "tscclibol";
                lRetorno.Add(erro);
            }

            #endregion

            #region [Inativar Conta - TSCCLICTA]

            //if (gPrimeiraExportacao)
            //{
            //    try
            //    {
            //        lDbCommand.Parameters.Clear();
            //        lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_cta");
            //        lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, gCodigo.Value);
            //        lAcessaDados.ExecuteNonQuery(lDbCommand);
            //    }
            //    catch (Exception ex)
            //    {
            //        SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
            //        erro.Excessao = ex.Message;
            //        erro.Mensagem = "Erro ao Inativar a Conta Corrente.";
            //        erro.Procedure = "prc_exp_cta";
            //        erro.Tabela = "tscclicta";
            //        lRetorno.Add(erro);
            //    }
            //}

            #endregion

            #region [Incrementar Contas - TSCCLICTA]

            using (DbCommand lDbCommandDadosBancariosExclusao = lAcessaDados.CreateCommand(CommandType.Text, string.Concat("DELETE FROM tscclicta WHERE cd_cliente = ", gClienteSinacor.CD_CLIENTE.DBToString())))
            {
                lAcessaDados.ExecuteNonQuery(lDbCommandDadosBancariosExclusao);
            }

            gClienteContasBancarias.Resultado.ForEach(delegate(ClienteBancoInfo cci)
            {
                using (DbCommand lDbCommandDadosBancarios = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_conta_ins"))
                {
                    lAcessaDados.ClearParameters(lDbCommand);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pCD_CLIENTE", DbType.Int32, gClienteSinacor.CD_CLIENTE);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pCD_BANCO", DbType.String, cci.CdBanco);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pCD_AGENCIA", DbType.String, cci.DsAgencia);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pDV_AGENCIA", DbType.String, cci.DsAgenciaDigito);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pNR_CONTA", DbType.String, cci.DsConta);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pDV_CONTA", DbType.String, cci.DsContaDigito);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pIN_PRINCIPAL", DbType.String, cci.StPrincipal ? "S" : "N");
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pIN_INATIVA", DbType.String, "N");
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pCD_EMPRESA", DbType.Int32, 222);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pCD_USUARIO", DbType.Int32, this.GetCdUsuario);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pTP_OCORRENCIA", DbType.String, "H");
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pTP_CONTA", DbType.String, cci.TpConta);
                    lAcessaDados.AddInParameter(lDbCommandDadosBancarios, "pIN_CONJUNTA", DbType.String, "N");
                    lAcessaDados.ExecuteNonQuery(lDbCommandDadosBancarios);
                }
            });

            #endregion

            #region [Assessor da Conta Corrente - TSCCLICC]

            try
            {
                lDbCommand.Parameters.Clear();
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_cc");
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, gCodigo.Value);
                lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESSOR", DbType.Int32, int.Parse(gClienteSinacor.CD_ASSESSOR));
                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Erro ao Alterar Assessor na Atividade Conta Corrente.";
                erro.Procedure = "tscclicc";
                erro.Tabela = "tscclicta";
                lRetorno.Add(erro);
            }

            #endregion

            #region [CliSis]

            if (gPrimeiraExportacao)
            {
                try
                {
                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_sis");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, gCodigo.Value);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao Inserir Referência na Atividade Conta Corrente.";
                    erro.Procedure = "prc_exp_sis";
                    erro.Tabela = "tscclisis";
                    lRetorno.Add(erro);
                }
            }

            #endregion

            #region [Email na TSCENDE]

            //try
            //{
            //    //--> 
            //    var lEmailCliente = ("R".Equals(gEnderecoPrincipalSinacor.IN_ENDE))
            //                      ? gClienteSinacor.NM_E_MAIL   //--> Email pessoal
            //                      : gClienteSinacor.NM_E_MAIL2; //--> Email comercial

            //    lDbCommand.Parameters.Clear();
            //    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_ende");
            //    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
            //    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
            //    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
            //    lAcessaDados.AddInParameter(lDbCommand, "pNM_E_MAIL", DbType.AnsiString, lEmailCliente);
            //    lAcessaDados.ExecuteNonQuery(lDbCommand);
            //}
            //catch (Exception ex)
            //{
            //    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
            //    erro.Excessao = ex.Message;
            //    erro.Mensagem = "Erro ao Alterar o Email no Cadastro Geral.";
            //    erro.Procedure = "prc_exp_ende";
            //    erro.Tabela = "tscende";
            //    lRetorno.Add(erro);
            //}

            #endregion

            #region [Inclui demais Endereços]

            try
            {
                int lNrSequencialEndereco = 1;

                //--> 
                var lEmailCliente = string.Empty;

                foreach (SinacorExportarEnderecoInfo item in gOutrosEnderecosSinacor)
                {
                    lNrSequencialEndereco++;

                    lEmailCliente = ("R".Equals(item.IN_ENDE))
                                  ? gClienteSinacor.NM_E_MAIL   //--> Email pessoal
                                  : gClienteSinacor.NM_E_MAIL2; //--> Email comercial

                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_endereco");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP", DbType.UInt32, item.CD_CEP.Substring(0, 5).DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CEP_EXT", DbType.UInt32, item.CD_CEP.Substring(5, 3).DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR1", DbType.UInt32, item.CD_DDD_CELULAR1.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_CELULAR2", DbType.UInt32, item.CD_DDD_CELULAR2.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_FAX", DbType.UInt32, item.CD_DDD_FAX.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_DDD_TEL", DbType.UInt32, item.CD_DDD_TEL.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pIN_ENDE", DbType.AnsiString, item.IN_ENDE);
                    lAcessaDados.AddInParameter(lDbCommand, "pNM_BAIRRO", DbType.AnsiString, item.NM_BAIRRO);
                    lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR1", DbType.UInt32, item.NR_CELULAR1.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pNR_CELULAR2", DbType.UInt32, item.NR_CELULAR2.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pNM_CIDADE", DbType.AnsiString, item.NM_CIDADE);
                    lAcessaDados.AddInParameter(lDbCommand, "pNM_COMP_ENDE", DbType.AnsiString, item.NM_COMP_ENDE);
                    lAcessaDados.AddInParameter(lDbCommand, "pNR_FAX", DbType.UInt32, item.NR_FAX.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO1", DbType.AnsiString, item.NM_CONTATO1);
                    lAcessaDados.AddInParameter(lDbCommand, "pNM_CONTATO2", DbType.AnsiString, item.NM_CONTATO2);
                    lAcessaDados.AddInParameter(lDbCommand, "pNM_LOGRADOURO", DbType.AnsiString, item.NM_LOGRADOURO);
                    lAcessaDados.AddInParameter(lDbCommand, "pNR_PREDIO", DbType.AnsiString, item.NR_PREDIO);
                    lAcessaDados.AddInParameter(lDbCommand, "pNR_RAMAL", DbType.UInt32, item.NR_RAMAL.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pNR_TELEFONE", DbType.UInt32, item.NR_TELEFONE.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "pSG_ESTADO", DbType.AnsiString, item.SG_ESTADO);
                    lAcessaDados.AddInParameter(lDbCommand, "pSG_PAIS_ENDE1", DbType.AnsiString, item.SG_PAIS_ENDE1);
                    lAcessaDados.AddInParameter(lDbCommand, "pNM_E_MAIL", DbType.AnsiString, lEmailCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pseqendereco", DbType.UInt32, lNrSequencialEndereco.DBToInt32());
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Erro ao Inserir os Endereços no Sinacor.";
                erro.Procedure = "prc_exp_endereco";
                erro.Tabela = "tscende";
                lRetorno.Add(erro);
            }

            #endregion

            #region [Assessor na TSCCLICUS]

            try
            {
                lDbCommand.Parameters.Clear();
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_cus");
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, gCodigo.Value);
                lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESSOR", DbType.Int32, int.Parse(gClienteSinacor.CD_ASSESSOR));
                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Erro ao Alterar o Assessor da Atividade Custódia.";
                erro.Procedure = "prc_exp_cus";
                erro.Tabela = "tscclicus";
                lRetorno.Add(erro);
            }

            #endregion

            #region [Assessor na TSCCLIGER]

            try
            {
                lDbCommand.Parameters.Clear();
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_ger");
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESSOR", DbType.Int32, int.Parse(gClienteSinacor.CD_ASSESSOR));
                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Erro ao Alterar no Cadastro Geral.";
                erro.Procedure = "prc_exp_ger";
                erro.Tabela = "tsccliger";
                lRetorno.Add(erro);
            }

            #endregion

            #region [Documentos]

            try
            {
                char CHEK_COMPROVANTES = 'S';
                char CHEK_BALANCO = 'N';
                char IN_PROCUR = 'N';
                char IN_VAL_MANDIR = 'N';
                Nullable<DateTime> DT_VAL_PROCUR = null;
                Nullable<DateTime> DT_BAL_PATRIMONIAL = null;
                Nullable<DateTime> DT_VAL_MANDIR = null;

                if (gCliente.TpPessoa == 'J')
                {
                    CHEK_COMPROVANTES = 'N';
                    CHEK_BALANCO = 'S';
                    DT_BAL_PATRIMONIAL = null == gSfp.DtPatrimonioLiquido ? new Nullable<DateTime>() : gSfp.DtPatrimonioLiquido.Value.AddYears(1);
                }

                ConsultarEntidadeRequest<ClienteProcuradorRepresentanteInfo> lEntradaProcurador = new ConsultarEntidadeRequest<ClienteProcuradorRepresentanteInfo>();
                lEntradaProcurador.Objeto = new ClienteProcuradorRepresentanteInfo() { IdCliente = gCliente.IdCliente };
                var lRetornoProcurador = ClienteDbLib.ConsultarClienteProcuradorRepresentante(lEntradaProcurador);

                foreach (ClienteProcuradorRepresentanteInfo item in lRetornoProcurador.Resultado)
                {
                    if (item.TpProcuradorRepresentante == TipoProcuradorRepresentante.Procurador)
                    {
                        IN_PROCUR = 'S';
                        DT_VAL_PROCUR = item.DtValidade;
                    }
                    else if ((item.TpProcuradorRepresentante == TipoProcuradorRepresentante.Administrador)
                         || ((item.TpProcuradorRepresentante == TipoProcuradorRepresentante.Procurador)))
                    {
                        IN_VAL_MANDIR = 'S';
                        DT_VAL_MANDIR = item.DtValidade;
                    }
                }

                lDbCommand.Parameters.Clear();
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_docs");
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);

                lAcessaDados.AddInParameter(lDbCommand, "pCHEK_COMPROVANTES", DbType.AnsiStringFixedLength, CHEK_COMPROVANTES);
                lAcessaDados.AddInParameter(lDbCommand, "pCHEK_BALANCO", DbType.AnsiStringFixedLength, CHEK_BALANCO);
                lAcessaDados.AddInParameter(lDbCommand, "pDT_BAL_PATRIMONIAL", DbType.Date, DT_BAL_PATRIMONIAL);
                lAcessaDados.AddInParameter(lDbCommand, "pIN_PROCUR", DbType.AnsiStringFixedLength, IN_PROCUR);
                lAcessaDados.AddInParameter(lDbCommand, "pDT_VAL_PROCUR", DbType.Date, DT_VAL_PROCUR);
                lAcessaDados.AddInParameter(lDbCommand, "pIN_VAL_MANDIR", DbType.AnsiStringFixedLength, IN_VAL_MANDIR);
                lAcessaDados.AddInParameter(lDbCommand, "pDT_VAL_MANDIR", DbType.Date, DT_VAL_MANDIR);
                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Erro ao Incluir Documentos.";
                erro.Procedure = "prc_exp_docs";
                erro.Tabela = "tscdocs";
                lRetorno.Add(erro);
            }

            #endregion

            #region [Diretores/Emitentes]

            try
            {
                lDbCommand.Parameters.Clear();
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_del_diretor_emitente");
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Não foi possível excluir os registros de Diretores e Emitentes";
                erro.Procedure = "prc_exp_del_diretor_emitente";
                erro.Tabela = "tsccliemitordem,tscemitordem,tsccvm220";
                lRetorno.Add(erro);
            }

            try
            {
                //PF exporta para a tabela tsccvm, sem diretor, pois PF pode ter emitente e emitente faz relacionamento com a tsccvm
                lDbCommand.Parameters.Clear();
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_diretor");
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);

                if (gCliente.TpPessoa == 'J')
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pDS_FORMACAO", DbType.String, gCliente.DsFormaConstituicao);
                    lAcessaDados.AddInParameter(lDbCommand, "pNR_INSCRICAO", DbType.String, gCliente.NrInscricaoEstadual);
                }

                int count = 0;

                foreach (ClienteDiretorInfo item in glDiretor)
                {
                    count++;
                    if (count == 1)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "pNM_DIRETOR_1", DbType.String, item.DsNome);
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC_DIR1", DbType.Int64, item.NrCpfCnpj);
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_DOC_IDENT_DIR1", DbType.String, item.DsIdentidade);
                    }
                    else if (count == 2)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "pNM_DIRETOR_2", DbType.String, item.DsNome);
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC_DIR2", DbType.Int64, item.NrCpfCnpj);
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_DOC_IDENT_DIR2", DbType.String, item.DsIdentidade);
                    }
                    else if (count == 3)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "pNM_DIRETOR_3", DbType.String, item.DsNome);
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC_DIR3", DbType.Int64, item.NrCpfCnpj);
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_DOC_IDENT_DIR3", DbType.String, item.DsIdentidade);
                    }
                }

                if (count > 0 || gEmitente.Count > 0) //Sempre exportar diretor, pois para poder ter emitente é necessário ter diretor
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
            }
            catch (Exception ex)
            {
                SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                erro.Excessao = ex.Message;
                erro.Mensagem = "Erro ao Incluir Diretor";
                erro.Procedure = "prc_exp_diretor";
                erro.Tabela = "tsccvm220";
                lRetorno.Add(erro);
            }

            foreach (ClienteEmitenteInfo item in gEmitente)
            {
                try
                {
                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_exp_emitente");
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, gClienteSinacor.CD_CPFCGC);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, gClienteSinacor.DT_NASC_FUND.DBToDateTime());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);

                    lAcessaDados.AddInParameter(lDbCommand, "pNM_EMIT_ORDEM", DbType.String, item.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC_EMIT", DbType.Int64, item.NrCpfCnpj.ToCpfCnpjSemPontuacao());
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_DOC_IDENT_EMIT", DbType.String, item.DsNumeroDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "pIN_PRINCIPAL", DbType.Int16, item.StPrincipal ? 1 : 0);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_SISTEMA", DbType.String, item.CdSistema);
                    lAcessaDados.AddInParameter(lDbCommand, "pNM_E_MAIL", DbType.String, item.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "pTM_STAMP", DbType.Date, item.DsData);

                    //lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int64, lCodigo.Value);


                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao incluir Emitente";
                    erro.Procedure = "prc_exp_emitente";
                    erro.Tabela = "tscemitordem";
                    lRetorno.Add(erro);
                }
            }

            #endregion

            #region [Cobrança de Custódia]

            if (gPrimeiraExportacao)
            {
                string lProc = "prc_exp_ver_dia_feriado";
                string lTabela = "tgeferia";

                try
                {
                    int diaCobranca = int.Parse(ConfigurationManager.AppSettings["DiaCobrancaCustodia"].ToString());
                    bool diaCobrancaOk = false;

                    //Colocar o dia aqui, pois se somar mes, alterar o ano e se somar dia, alterar o mês quando necessário
                    DateTime lDataAux = DateTime.Now.AddMonths(-1);
                    DateTime lDataCobrancaCustodia = new DateTime(lDataAux.Year, lDataAux.Month, diaCobranca);

                    while (!diaCobrancaOk)
                    {
                        //lDataCobrancaCustodia.AddDays(1);
                        //sábado ou domingo
                        if (lDataCobrancaCustodia.DayOfWeek == DayOfWeek.Saturday || lDataCobrancaCustodia.DayOfWeek == DayOfWeek.Sunday)
                        {
                            lDataCobrancaCustodia = lDataCobrancaCustodia.AddDays(1);
                        }
                        else
                        {
                            //ver feriado
                            //se sim então   data.AddDays(1);
                            //senão
                            //diaCobrancaOk = true;

                            lDbCommand.Parameters.Clear();
                            lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, lProc);
                            lAcessaDados.AddInParameter(lDbCommand, "pDATA", DbType.Date, lDataCobrancaCustodia);
                            lAcessaDados.AddOutParameter(lDbCommand, "pFERIADO", DbType.Int32, 8);
                            lAcessaDados.ExecuteNonQuery(lDbCommand);

                            int lFeriado = lDbCommand.Parameters["pFERIADO"].Value.DBToInt32();

                            if (lFeriado > 0)
                                lDataCobrancaCustodia = lDataCobrancaCustodia.AddDays(1);
                            else
                                diaCobrancaOk = true;
                        }
                    }

                    lProc = "prc_exp_cobrcus";
                    lTabela = "tscclicus";

                    lDbCommand.Parameters.Clear();
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, lProc);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, gCodigo.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_ULT_COBRANCA", DbType.Date, lDataCobrancaCustodia);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao Data de cobrança de Custódia.";
                    erro.Procedure = lProc;
                    erro.Tabela = lTabela;
                    lRetorno.Add(erro);
                }
            }

            #endregion

            #region [Atualizar Ativação nas Atividades]

            if (!gPrimeiraExportacao)
            {
                try
                {
                    ReceberObjetoResponse<ClienteAtivarInativarInfo> RetornoClienteAtivar = new ReceberObjetoResponse<ClienteAtivarInativarInfo>();
                    ReceberEntidadeRequest<ClienteAtivarInativarInfo> EntradaClienteAtivar = new ReceberEntidadeRequest<ClienteAtivarInativarInfo>() { DescricaoUsuarioLogado = "Processo de Exportação" };
                    EntradaClienteAtivar.Objeto = new ClienteAtivarInativarInfo() { IdCliente = gCliente.IdCliente.Value };

                    EntradaClienteAtivar.DescricaoUsuarioLogado = "Processo de Exportação";

                    RetornoClienteAtivar = ClienteDbLib.ReceberClienteAtivarInativar(EntradaClienteAtivar);

                    SalvarEntidadeRequest<ClienteAtivarInativarInfo> EntradaSalvar = new SalvarEntidadeRequest<ClienteAtivarInativarInfo>();

                    EntradaSalvar.Objeto = RetornoClienteAtivar.Objeto;

                    EntradaSalvar.DescricaoUsuarioLogado = "Processo de Exportação";

                    SalvarEntidadeResponse<ClienteAtivarInativarInfo> RetornoSalvar = ClienteDbLib.SalvarClienteAtivarInativar(EntradaSalvar);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaComplementosInfo erro = new SinacorExportacaoRetornoFalhaComplementosInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Erro ao Alterar a Ativação do Cliente na tabela ClienteGeral, Atividade Bolsa, Atividade Custódia ou Atividade Conta Corrente.";
                    erro.Procedure = "prc_AtivarInativar_CliGer_upd";
                    erro.Tabela = "tscliger,tescclibol,tscclicus,tscclicc";
                    lRetorno.Add(erro);
                }
            }

            #endregion

            //#region [Associar Cliente Grupo Alavancagem]

            //try
            //{
            //    var lAcessaDadosRisco = new ConexaoDbHelper() { ConnectionStringName = "Risco" };

            //    using (DbCommand lDbCommandRisco = lAcessaDadosRisco.CreateCommand(CommandType.StoredProcedure, "prc_ins_cliente_grupo_alavancagem"))
            //    {
            //        lAcessaDadosRisco.AddInParameter(lDbCommandRisco, "@id_cliente", DbType.Int32, gCodigo);
            //        lAcessaDadosRisco.AddInParameter(lDbCommandRisco, "@id_parametro_grupo_alavancagem", DbType.Int32, this.GetParametroGrupoAlavancagem);

            //        //lAcessaDadosRisco.ExecuteNonQuery(lDbCommandRisco);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    lRetorno.Add(new SinacorExportacaoRetornoFalhaComplementosInfo()
            //    {
            //        Excessao = ex.ToString(),
            //        Mensagem = "Associação do cliente ao grupo de alavancagem.",
            //        Procedure = "prc_ins_cliente_grupo_alavancagem",
            //        Tabela = "tb_parametro_alavancagem_cliente",
            //    });
            //}

            //#endregion

            return lRetorno;
        }

        private List<SinacorExportacaoRetornoFalhaRiscoInfo> ExportarRiscoEPermissao()
        {
            List<SinacorExportacaoRetornoFalhaRiscoInfo> lRetorno = new List<SinacorExportacaoRetornoFalhaRiscoInfo>();
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gConexaoRisco;
            DbCommand lDbCommand = null;

            if (gPrimeiraExportacao)
            {
                try
                {
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_permissoes_default_cliente");
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, gCodigo.Value);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaRiscoInfo erro = new SinacorExportacaoRetornoFalhaRiscoInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Não foi possível atualizar os parâmetros de Risco do cliente";
                    erro.Procedure = "prc_ins_permissoes_default_cliente";
                    erro.Tabela = "TB_CLIENTE_PERMISSAO";
                    lRetorno.Add(erro);
                }

                lAcessaDados.ConnectionStringName = gConexaoControleAcesso;
                lDbCommand = null;

                try
                {
                    lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_UsuariosPermissoes_ins");
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoItem", DbType.Int32, gLogin.IdLogin.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@Status", DbType.Int32, 0);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoPermissao", DbType.String, "22FF518C-C7D3-4ff0-A0CB-96F2476068BB");
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                catch (Exception ex)
                {
                    SinacorExportacaoRetornoFalhaRiscoInfo erro = new SinacorExportacaoRetornoFalhaRiscoInfo();
                    erro.Excessao = ex.Message;
                    erro.Mensagem = "Não foi possível atualizar as permissões no Controle de Acesso";
                    erro.Procedure = "PRC_USUARIOSPERMISSOES_INS";
                    erro.Tabela = "TB_USUARIOSPERMISSOES";
                    lRetorno.Add(erro);
                }
            }

            return lRetorno;
        }

        private string GetTipoDeEndereco(int pIdTipoEndereco)
        {
            if (pIdTipoEndereco == 1)
                return "C";
            else
                return "R";
        }

        /// <summary>
        /// Tratamento de endereço principal e mapeamento de telefone X endereço
        /// </summary>
        /// <param name="pIdCliente"></param>
        private void SetEnredecosParaExportacao(int pIdCliente)
        {
            //Listar Todos os Telefones
            var lTelefones = ClienteDbLib.ConsultarClienteTelefone(new ConsultarEntidadeRequest<ClienteTelefoneInfo>() { Objeto = new ClienteTelefoneInfo(pIdCliente.DBToString()) }).Resultado;

            //Listar Todos os Endereços
            var lEnderecos = ClienteDbLib.ConsultarClienteEndereco(new ConsultarEntidadeRequest<ClienteEnderecoInfo>() { Objeto = new ClienteEnderecoInfo(pIdCliente.DBToString()) }).Resultado;

            if (null != lEnderecos && lEnderecos.Count > 0
            && (null != lTelefones && lTelefones.Count > 0))
            {
                var lTelefonePrincipal = new ClienteTelefoneInfo();
                var lPrimeiroCelular = new ClienteTelefoneInfo();
                var lSegundoCelular = new ClienteTelefoneInfo();
                var lFax = new ClienteTelefoneInfo();

                var lEnderecoOutros = lEnderecos.FindAll(end => { return !end.StPrincipal; });   //--> Selecionar os Demais Endereços
                var lEnderecoPrincipal = lEnderecos.Find(end => { return end.StPrincipal; }); //--> Selecionar Endereço Principal
                var lTelefoneCelulares = lTelefones.FindAll(tel => { return tel.IdTipoTelefone == 3; }); //--> Selencionar os fones Celulares cadastrados

                lPrimeiroCelular = (null != lTelefoneCelulares && lTelefoneCelulares.Count > 0) ? lTelefoneCelulares[0] : new ClienteTelefoneInfo();
                lSegundoCelular = (null != lTelefoneCelulares && lTelefoneCelulares.Count > 1) ? lTelefoneCelulares[1] : new ClienteTelefoneInfo();
                lFax = lTelefones.Find(tel => { return tel.IdTipoTelefone == 4; });

                {   //--> Definindo o endereço Principal
                    lTelefones.ForEach(itemTelefone =>
                    {
                        if ((lEnderecoPrincipal.IdTipoEndereco == 1 && itemTelefone.IdTipoTelefone == 2)   //--> Endereco 1 = Comercial   e Telefone 2 = Comercial
                        || ((lEnderecoPrincipal.IdTipoEndereco == 2 && itemTelefone.IdTipoTelefone == 1))  //--> Endereco 2 = Residencial e Telefone 1 = Residencial
                        || ((lEnderecoPrincipal.IdTipoEndereco == 3 && itemTelefone.IdTipoTelefone == 3))) //--> Endereco 3 = Outros      e Telefone 3 = Celular
                        {
                            lTelefonePrincipal = itemTelefone;
                        }
                    });

                    gEnderecoPrincipalSinacor.CD_DDD_TEL = lTelefonePrincipal.DsDdd.DBToString();
                    gEnderecoPrincipalSinacor.NR_TELEFONE = lTelefonePrincipal.DsNumero.DBToString();
                    gEnderecoPrincipalSinacor.NR_RAMAL = lTelefonePrincipal.DsRamal.DBToString();
                    gEnderecoPrincipalSinacor.CD_DDD_CELULAR1 = lPrimeiroCelular.DsDdd.DBToString();
                    gEnderecoPrincipalSinacor.NR_CELULAR1 = lPrimeiroCelular.DsNumero.DBToString();
                    gEnderecoPrincipalSinacor.CD_DDD_CELULAR2 = lSegundoCelular.DsDdd.DBToString();
                    gEnderecoPrincipalSinacor.NR_CELULAR2 = lSegundoCelular.DsNumero.DBToString();
                    gEnderecoPrincipalSinacor.CD_DDD_FAX = (null != lFax) ? lFax.DsDdd.DBToString() : string.Empty;
                    gEnderecoPrincipalSinacor.NR_FAX = (null != lFax) ? lFax.DsNumero.DBToString() : string.Empty;

                    gEnderecoPrincipalSinacor.CD_CEP = string.Concat(lEnderecoPrincipal.NrCep.DBToString().PadLeft(5, '0'), lEnderecoPrincipal.NrCepExt.DBToString().PadLeft(3, '0'));
                    gEnderecoPrincipalSinacor.IN_ENDE = GetTipoDeEndereco(lEnderecoPrincipal.IdTipoEndereco);
                    gEnderecoPrincipalSinacor.NM_BAIRRO = lEnderecoPrincipal.DsBairro.DBToString();
                    gEnderecoPrincipalSinacor.NM_CIDADE = lEnderecoPrincipal.DsCidade.DBToString();
                    gEnderecoPrincipalSinacor.NM_COMP_ENDE = lEnderecoPrincipal.DsComplemento.DBToString();
                    gEnderecoPrincipalSinacor.NM_CONTATO1 = string.Empty;
                    gEnderecoPrincipalSinacor.NM_CONTATO2 = string.Empty;
                    gEnderecoPrincipalSinacor.NM_LOGRADOURO = lEnderecoPrincipal.DsLogradouro.DBToString();
                    gEnderecoPrincipalSinacor.NR_PREDIO = lEnderecoPrincipal.DsNumero.DBToString();
                    gEnderecoPrincipalSinacor.SG_ESTADO = lEnderecoPrincipal.CdUf.DBToString();
                    gEnderecoPrincipalSinacor.SG_PAIS_ENDE1 = lEnderecoPrincipal.CdPais.DBToString();

                    gEnderecoPrincipalSinacor.CD_USUARIO = this.GetCdUsuario.ToString();
                    gEnderecoPrincipalSinacor.TP_OCORRENCIA = "I";
                    gEnderecoPrincipalSinacor.DT_ATUALIZ_CCLI = DateTime.Now.ToString();
                }

                if (null != lEnderecoOutros && lEnderecoOutros.Count > 0)
                {
                    var lOutroTelefone = new ClienteTelefoneInfo();
                    var lOutroEndereco = new SinacorExportarEnderecoInfo();

                    lEnderecoOutros.ForEach(itemEndereco =>
                    {
                        lOutroTelefone = new ClienteTelefoneInfo();
                        lOutroEndereco = new SinacorExportarEnderecoInfo();

                        lTelefones.ForEach(itemTelefone =>
                        {
                            if ((itemEndereco.IdTipoEndereco == 1 && itemTelefone.IdTipoTelefone == 2)   //--> Endereco 1 = Comercial   e Telefone 2 = Comercial
                            || ((itemEndereco.IdTipoEndereco == 2 && itemTelefone.IdTipoTelefone == 1))  //--> Endereco 2 = Residencial e Telefone 1 = Residencial
                            || ((itemEndereco.IdTipoEndereco == 3 && itemTelefone.IdTipoTelefone == 3))) //--> Endereco 3 = Outros      e Telefone 3 = Celular
                            {
                                lOutroTelefone = itemTelefone;
                            }
                        });

                        lOutroEndereco.CD_CEP = string.Concat(itemEndereco.NrCep.DBToString().PadLeft(5, '0'), itemEndereco.NrCepExt.DBToString().PadLeft(3, '0'));
                        lOutroEndereco.CD_DDD_CELULAR1 = lPrimeiroCelular.DsDdd.DBToString();
                        lOutroEndereco.NR_CELULAR1 = lPrimeiroCelular.DsNumero.DBToString();
                        lOutroEndereco.CD_DDD_CELULAR2 = lSegundoCelular.DsDdd.DBToString();
                        lOutroEndereco.NR_CELULAR2 = lSegundoCelular.DsNumero.DBToString();
                        lOutroEndereco.CD_DDD_FAX = (null != lFax) ? lFax.DsDdd.DBToString() : string.Empty;
                        lOutroEndereco.NR_FAX = (null != lFax) ? lFax.DsNumero.DBToString() : string.Empty;

                        lOutroEndereco.CD_DDD_TEL = lOutroTelefone.DsDdd.DBToString();
                        lOutroEndereco.IN_ENDE = GetTipoDeEndereco(itemEndereco.IdTipoEndereco);
                        lOutroEndereco.NM_BAIRRO = itemEndereco.DsBairro.DBToString();
                        lOutroEndereco.NM_CIDADE = itemEndereco.DsCidade.DBToString();
                        lOutroEndereco.NM_COMP_ENDE = itemEndereco.DsComplemento.DBToString();
                        lOutroEndereco.NM_CONTATO1 = string.Empty;
                        lOutroEndereco.NM_CONTATO2 = string.Empty;
                        lOutroEndereco.NM_LOGRADOURO = itemEndereco.DsLogradouro.DBToString();
                        lOutroEndereco.NR_PREDIO = itemEndereco.DsNumero.DBToString();
                        lOutroEndereco.NR_RAMAL = lOutroTelefone.DsRamal.DBToString();
                        lOutroEndereco.NR_TELEFONE = lOutroTelefone.DsNumero.DBToString();
                        lOutroEndereco.SG_ESTADO = itemEndereco.CdUf.DBToString();
                        lOutroEndereco.SG_PAIS_ENDE1 = itemEndereco.CdPais.DBToString();

                        lOutroEndereco.CD_USUARIO = this.GetCdUsuario.DBToString();
                        lOutroEndereco.TP_OCORRENCIA = "U";
                        lOutroEndereco.DT_ATUALIZ_CCLI = DateTime.Now.ToString();

                        gOutrosEnderecosSinacor.Add(lOutroEndereco);
                    });
                }
            }
        }

        /// <summary>
        /// Formata endereço para a estrutura do Sinacor
        /// </summary>
        /// <param name="pDdados"></param>
        /// <returns></returns>
        private SinacorExportarEnderecoInfo FormataEndereco(DataTable pDdados)
        {
            SinacorExportarEnderecoInfo lEndereco = new SinacorExportarEnderecoInfo();
            if (pDdados.Rows.Count > 0)
            {
                try
                {
                    lEndereco.CD_CEP = pDdados.Rows[0]["CD_CEP"].DBToString().PadLeft(5, '0') + pDdados.Rows[0]["CD_CEP_EXT"].DBToString().PadLeft(3, '0');
                    lEndereco.CD_DDD_CELULAR1 = pDdados.Rows[0]["CD_DDD_CELULAR1"].DBToString();
                    lEndereco.CD_DDD_CELULAR2 = pDdados.Rows[0]["CD_DDD_CELULAR2"].DBToString();
                    lEndereco.CD_DDD_FAX = pDdados.Rows[0]["CD_DDD_FAX"].DBToString();
                    lEndereco.CD_DDD_TEL = pDdados.Rows[0]["CD_DDD_TEL"].DBToString();
                    lEndereco.IN_ENDE = pDdados.Rows[0]["IN_TIPO_ENDE"].DBToString();
                    lEndereco.NM_BAIRRO = pDdados.Rows[0]["NM_BAIRRO"].DBToString();
                    lEndereco.NR_CELULAR1 = pDdados.Rows[0]["NR_CELULAR1"].DBToString();
                    lEndereco.NR_CELULAR2 = pDdados.Rows[0]["NR_CELULAR2"].DBToString();
                    lEndereco.NM_CIDADE = pDdados.Rows[0]["NM_CIDADE"].DBToString();
                    lEndereco.NM_COMP_ENDE = pDdados.Rows[0]["NM_COMP_ENDE"].DBToString();
                    lEndereco.NR_FAX = pDdados.Rows[0]["NR_FAX"].DBToString();
                    lEndereco.NM_CONTATO1 = pDdados.Rows[0]["NM_CONTATO1"].DBToString();
                    lEndereco.NM_CONTATO2 = pDdados.Rows[0]["NM_CONTATO2"].DBToString();
                    lEndereco.NM_LOGRADOURO = pDdados.Rows[0]["NM_LOGRADOURO"].DBToString();
                    lEndereco.NR_PREDIO = pDdados.Rows[0]["NR_PREDIO"].DBToString();
                    lEndereco.NR_RAMAL = pDdados.Rows[0]["NR_RAMAL"].DBToString();
                    lEndereco.NR_TELEFONE = pDdados.Rows[0]["NR_TELEFONE"].DBToString();
                    lEndereco.SG_ESTADO = pDdados.Rows[0]["SG_ESTADO"].DBToString();
                    lEndereco.SG_PAIS_ENDE1 = pDdados.Rows[0]["SG_PAIS"].DBToString();
                    lEndereco.CD_USUARIO = this.GetCdUsuario.ToString();
                    lEndereco.TP_OCORRENCIA = "U";
                    lEndereco.DT_ATUALIZ_CCLI = DateTime.Now.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return new SinacorExportarEnderecoInfo();
            }

            return lEndereco;
        }

        /// <summary>
        /// Método para gerar um novo Código Bovespa
        /// </summary>
        /// <returns>Código Gerado</returns>
        private string GerarBovespa()
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gConexaoSinacorConsulta;
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("SELECT (CD_CLIE_BASE + 1) as CD_CLIE_BASE FROM TSCPARM");
            DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
            object Bovespa = lAcessaDados.ExecuteScalar(lDbCommand);
            return Bovespa.ToString();
        }

        /// <summary>
        /// Gera dígito do código
        /// </summary>
        /// <param name="Bovespa">Código CBLC</param>
        /// <returns>Retorna o dígito gerado</returns>
        private string GeraDigito(string Bovespa)
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gConexaoSinacorCorrwin;
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select proc_func.GERA_DIGITO(227," + Bovespa + ") as Digito  from dual");
            DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
            object digito = lAcessaDados.ExecuteScalar(lDbCommand);
            return digito.ToString();
        }

        /// <summary>
        /// Retorna a porcentagem de corretagem, para manter o valor na re-exportação
        /// </summary>
        /// <param name="cd_cliente">Código do Cliente no Sinacor</param>
        /// <returns>Porcentagem de Corretagem</returns>
        private sMantemValoresBovespa GetDadosAtuaisBovespa(string cd_cliente)
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gConexaoSinacorConsulta;
            DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, "select PC_CORCOR_PRIN,CD_CUSTODIANTE,CD_CLIE_CUST,CD_AGENTE_COMP,CD_USUA_COMP from TSCCLIBOL where CD_CLIENTE = " + cd_cliente);
            DataTable lDados = lAcessaDados.ExecuteDbDataTable(lDbCommand);
            sMantemValoresBovespa lRetorno = new sMantemValoresBovespa() { CD_AGENTE_COMP = "", CD_CLIENTE_COMP = "", COD_AGCT = "", COD_CLI_AGCT = "", PC_CORCOR_PRIN = "" };

            if (lDados.Rows.Count > 0)
            {
                lRetorno.PC_CORCOR_PRIN = lDados.Rows[0]["PC_CORCOR_PRIN"].ToString();
                lRetorno.CD_AGENTE_COMP = lDados.Rows[0]["CD_AGENTE_COMP"].ToString();
                lRetorno.CD_CLIENTE_COMP = lDados.Rows[0]["CD_USUA_COMP"].ToString();
                lRetorno.COD_AGCT = lDados.Rows[0]["CD_CUSTODIANTE"].ToString();
                lRetorno.COD_CLI_AGCT = lDados.Rows[0]["CD_CLIE_CUST"].ToString();
            }
            return lRetorno;
        }

        /// <summary>
        /// Formata os conteúdos
        /// </summary>
        /// <param name="myBuffer">Dado</param>
        /// <param name="myData">Tipo de dados</param>
        /// <returns>Dado formatado de acordo com o tipo</returns>
        private string FormataInformacoes(string myBuffer, DataType myData)
        {
            if ((myBuffer == null) || (myBuffer == ""))
                return "null";

            if (myBuffer.ToString().ToUpper() == "NULL")
                return "Null";

            string strBuffer = string.Empty;

            if (myBuffer == null)
                return myBuffer;

            switch (myData)
            {
                case DataType.NUMBER:
                    if (myBuffer == "" || myBuffer == string.Empty)
                        return "Null";
                    strBuffer = myBuffer.Replace(",", ".");
                    break;
                case DataType.DATE:
                    strBuffer = myBuffer;
                    break;
                default:
                    strBuffer = "'" + myBuffer + "'";
                    break;
            }
            return strBuffer.ToUpper();
        }

        private string GeraInsert(string Tabela, List<sGeraInsert> Comando)
        {
            //string retorno;
            string strQ1, strQ2 = "";

            strQ1 = "Insert Into " + Tabela + " (";
            foreach (sGeraInsert item in Comando)
            {
                strQ1 += Environment.NewLine + item.Campo + ",";

                if (item.Tipo == DataType.DATE)
                    strQ2 += Environment.NewLine + item.Valor.ToString() + ",";
                else
                    strQ2 += Environment.NewLine + FormataInformacoes(SinacorExportarDbLib.TiraAcento(item.Valor, item.TiraPonto), item.Tipo) + ",";
            }
            //retira a virgula do ultimo
            strQ1 = strQ1.Substring(0, strQ1.Length - 1);
            strQ1 += ") values (";
            //retira a virgula do ultimo
            strQ2 = strQ2.Substring(0, strQ2.Length - 1);
            strQ2 += ")";
            return strQ1 + strQ2;
        }

        #endregion

        #region | Métodos de apoio

        public static string TiraAcento(string pTexto)
        {
            return TiraAcento(pTexto, true);
        }

        public static string TiraAcento(object Texto, Boolean tiraPonto)
        {
            string pTexto;
            if (null == Texto)
                return "Null";
            else
                pTexto = Texto.ToString();
            if (pTexto != null)
            {
                pTexto = pTexto.Replace("á", "a");

                pTexto = pTexto.Replace("é", "e");

                pTexto = pTexto.Replace("í", "i");

                pTexto = pTexto.Replace("ó", "o");

                pTexto = pTexto.Replace("ú", "u");

                pTexto = pTexto.Replace("à", "a");

                pTexto = pTexto.Replace("è", "e");

                pTexto = pTexto.Replace("ì", "i");

                pTexto = pTexto.Replace("ò", "o");

                pTexto = pTexto.Replace("ù", "u");

                pTexto = pTexto.Replace("â", "a");

                pTexto = pTexto.Replace("ê", "e");

                pTexto = pTexto.Replace("î", "i");

                pTexto = pTexto.Replace("ô", "o");

                pTexto = pTexto.Replace("û", "u");

                pTexto = pTexto.Replace("ä", "a");

                pTexto = pTexto.Replace("ë", "e");

                pTexto = pTexto.Replace("ï", "i");

                pTexto = pTexto.Replace("ö", "o");

                pTexto = pTexto.Replace("ü", "u");

                pTexto = pTexto.Replace("ã", "a");

                pTexto = pTexto.Replace("õ", "o");

                pTexto = pTexto.Replace("ñ", "n");

                pTexto = pTexto.Replace("ç", "c");

                pTexto = pTexto.Replace("Á", "A");

                pTexto = pTexto.Replace("É", "E");

                pTexto = pTexto.Replace("Í", "I");

                pTexto = pTexto.Replace("Ó", "O");

                pTexto = pTexto.Replace("Ú", "U");

                pTexto = pTexto.Replace("À", "A");

                pTexto = pTexto.Replace("È", "E");

                pTexto = pTexto.Replace("Ì", "I");

                pTexto = pTexto.Replace("Ò", "O");

                pTexto = pTexto.Replace("Ù", "U");

                pTexto = pTexto.Replace("Â", "A");

                pTexto = pTexto.Replace("Ê", "E");

                pTexto = pTexto.Replace("Î", "I");

                pTexto = pTexto.Replace("Ô", "O");

                pTexto = pTexto.Replace("Û", "U");

                pTexto = pTexto.Replace("Ä", "A");

                pTexto = pTexto.Replace("Ë", "E");

                pTexto = pTexto.Replace("Ï", "I");

                pTexto = pTexto.Replace("Ö", "O");

                pTexto = pTexto.Replace("Ü", "U");

                pTexto = pTexto.Replace("Ã", "A");

                pTexto = pTexto.Replace("Õ", "O");

                pTexto = pTexto.Replace("Ñ", "N");

                pTexto = pTexto.Replace("Ç", "C");

                pTexto = pTexto.Replace("'", " ");

                if (tiraPonto)
                    pTexto = pTexto.Replace(".", "");

                return pTexto.ToUpper();
            }

            else
                return pTexto;
        }

        #endregion
    }
}