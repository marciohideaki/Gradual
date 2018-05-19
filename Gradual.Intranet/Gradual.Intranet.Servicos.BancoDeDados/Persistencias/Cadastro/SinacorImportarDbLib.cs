using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Negocios;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados.Cadastro;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public class SinacorImportarDbLib
    {
        #region Strings de Conexão

        private const string gNomeConexaoOracle = "SinacorConsulta";
        private const string gNomeConexaoSql = "Cadastro";
        private const string gNomeConexaoRisco = "RISCO";
        private const string gNomeConexaoControleAcesso = "Seguranca";
        private const string gNomeConexaoCadastroDuc = "CadastroDuc";
        private const string gConexaoDUC = "DUC";

        #endregion

        #region Contrutores

        public SinacorImportarDbLib(SinacorChaveClienteInfo pChaveCliente)
        {
            this._lChaveCliente = pChaveCliente;
        }

        public SinacorImportarDbLib()
        {

        }

        #endregion

        private Hashtable gHtLoginCliente = new Hashtable();

        private SinacorChaveClienteInfo _lChaveCliente;
        public SinacorChaveClienteInfo lChaveCliente
        {
            get
            {
                if (null == _lChaveCliente)
                    throw new Exception(gErroFaltaChave);

                return _lChaveCliente;
            }
            set
            {
                _lChaveCliente = value;
            }
        }

        private int gContaPrincipal;
        private const string gErroFaltaChave = "É necessário informar a chave do cliente (CPF/CNPJ, Data de Nascimento/Fundação e Condição de Dependente)";

        /// <summary>
        /// Lista todos os clientes do Sinacor para Importação
        /// </summary>
        /// <DataCriacao>14/05/2010</DataCriacao>
        /// <Autor>Gustavo Malta Guimarães</Autor>
        /// <Alteracao>
        ///     <DataAlteracao></DataAlteracao>
        ///     <Autor></Autor>
        ///     <Motivo></Motivo>
        /// </Alteracao>
        /// <param name="pParametros">New SinacorChaveClienteInfo</param>
        /// <returns>Lista com a chave de todos os clientes do Sinacor</returns>
        public ConsultarObjetosResponse<SinacorChaveClienteInfo> SinacorListarTodos(ConsultarEntidadeRequest<SinacorChaveClienteInfo> pParametros)
        {
            ConsultarObjetosResponse<SinacorChaveClienteInfo> lRetorno = new ConsultarObjetosResponse<SinacorChaveClienteInfo>();

            foreach (SinacorChaveClienteInfo item in ListarTodos())
            {
                lRetorno.Resultado.Add(item);
            }



            return lRetorno;
        }


        /// <summary>
        /// Método que será reaproveitado para outros sistemas
        /// </summary>
        /// <DataCriacao>06/05/2010</DataCriacao>
        /// <Autor>Gustavo Malta Guimarães</Autor>
        /// <Alteracao>
        ///     <DataAlteracao></DataAlteracao>
        ///     <Autor></Autor>
        ///     <Motivo></Motivo>
        /// </Alteracao>
        /// <param name="pParametros">SinacorClienteInfo a Chave Preenchida</param>
        /// <returns>Clinete na Estrutura do Sinacor</returns>
        public ReceberObjetoResponse<SinacorClienteInfo> SinacorBuscarCliente(ReceberEntidadeRequest<SinacorClienteInfo> pParametros)
        {
            ReceberObjetoResponse<SinacorClienteInfo> lRetorno = new ReceberObjetoResponse<SinacorClienteInfo>();

            lRetorno.Objeto = new SinacorClienteInfo();

            try
            {
                lRetorno.Objeto.ChaveCliente = pParametros.Objeto.ChaveCliente;

                lRetorno.Objeto.ClienteGeral = GetClienteGeral();

                lRetorno.Objeto.ClienteComplemento = GetClienteComplemento();

                lRetorno.Objeto.Emitentes = GetEmitentes();

                lRetorno.Objeto.Diretor = GetDiretor();

                lRetorno.Objeto.Cc = GetCc();

                lRetorno.Objeto.Enderecos = GetEnderecos();

                lRetorno.Objeto.Telefones = GetTelefones();

                lRetorno.Objeto.SituacaoFinaceiraPatrimoniais = GetSituacaoFinaceiraPatrimoniais();

                lRetorno.Objeto.Bovespas = GetBovespas();

                lRetorno.Objeto.AtividadesCc = GetAtividadesCc();

                lRetorno.Objeto.AtividadesCustodia = GetAtividadesCustodia();

                lRetorno.Objeto.Bmfs = GetBmfs();

                //Deve ser depois de carregar os códigos Bovespa e BM&F
                lRetorno.Objeto.ContasBancarias = GetContasBancarias(lRetorno.Objeto);

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto.ChaveCliente, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Importar, ex);
                throw ex;
            }
        }

        public Hashtable ObterLoginClienteSistemaDuc()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = gConexaoDUC;
            List<SinacorMigracaoLoginInfo> lRetorno = new List<SinacorMigracaoLoginInfo>();
            Hashtable lHtLogin = new Hashtable();
            string lKey = string.Empty;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_login_migracao"))
            {
                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow linha in lDataTable.Rows)
                    {
                        lKey = string.Format("{0}-{1}", linha["CPF"].DBToString(), linha["DATANASCIMENTO"].DBToDateTime().ToString("ddMMyyyy"));

                        if (!lHtLogin.Contains(lKey))
                        {
                            lHtLogin.Add(lKey,
                                (new SinacorMigracaoLoginInfo()
                                {
                                    DsAssinatura = linha["ASSINATURA"].DBToString(),
                                    DsCpf = linha["CPF"].DBToString(),
                                    DsEmail = linha["EMAIL"].DBToString(),
                                    DsNome = linha["NOME"].DBToString(),
                                    DsSenha = linha["SENHA"].DBToString(),
                                    DtNascimento = linha["DATANASCIMENTO"].DBToDateTime(),
                                    IdAssessor = linha["ID_ASSESSOR"].DBToInt32()
                                }));
                        }
                    }
            }

            return lHtLogin;
        }

        /// <summary>
        /// Define a conta Principal
        /// </summary>
        /// <DataCriacao>13/05/2010</DataCriacao>
        /// <Autor>Gustavo Malta Guimarães</Autor>
        /// <Alteracao>
        ///     <DataAlteracao></DataAlteracao>
        ///     <Autor></Autor>
        ///     <Motivo></Motivo>
        /// </Alteracao>
        /// <param name="pChaveCliente">Chave do Cliente no Sinacor</param>
        /// <param name="pContas">Lista de Contas</param>
        /// <returns>Lesta de Contas com a Principal definida</returns>
        private List<ClienteContaInfo> SetContaPrincipal(SinacorChaveClienteInfo pChaveCliente, List<ClienteContaInfo> pContas)
        {
            if (pContas.Count == 1)
            {
                pContas[0].StPrincipal = true;
                gContaPrincipal = pContas[0].CdCodigo.DBToInt32();
                return pContas;
            }

            this.lChaveCliente = pChaveCliente;

            Int64 pContaPrincipal = this.GetContaPrincipal();
            Boolean lExisteBol = false;

            foreach (ClienteContaInfo item in pContas)
            {
                if (item.CdSistema == Contratos.Dados.Enumeradores.eAtividade.BOL && item.CdCodigo == pContaPrincipal)
                {
                    item.StPrincipal = true;
                    gContaPrincipal = item.CdCodigo.DBToInt32();
                    lExisteBol = true;
                }
            }

            if (!lExisteBol)
            {
                foreach (ClienteContaInfo item in pContas)
                {
                    if (item.CdSistema == Contratos.Dados.Enumeradores.eAtividade.BMF && item.CdCodigo == pContaPrincipal)
                    {
                        item.StPrincipal = true;
                        gContaPrincipal = item.CdCodigo.DBToInt32();
                        lExisteBol = true;
                    }
                }
            }

            return pContas;
        }

        /// <summary>
        /// Método específico para o Sistema de Cadastro, que recebe os dados na estrutura do Sinacor e Salva o cliente no Cadastro
        /// </summary>
        /// <param name="pCliente">Entidade Cliente do Sinacor</param>
        public SalvarEntidadeResponse<SinacorClienteInfo> SinacorSalvarCliente(SalvarObjetoRequest<SinacorClienteInfo> pParametros)
        {

            LoginInfo lLogin;
            ClienteInfo lCliente;
            List<ClienteTelefoneInfo> lTelefone;
            List<ClienteEnderecoInfo> lEndereco;
            List<ClienteBancoInfo> lContaBancaria;
            List<ClienteContaInfo> lConta;
            List<ClienteDiretorInfo> lDiretor;
            List<ClienteEmitenteInfo> lEmitente;
            ClienteSituacaoFinanceiraPatrimonialInfo lSfp;

            try
            {
                SinacorClienteInfo pCliente = pParametros.Objeto;

                lCliente = ConversaoSinacor.ConverterCliente(pCliente.ChaveCliente, pCliente.ClienteGeral, pCliente.ClienteComplemento, pCliente.Cc, pCliente.Diretor);

                string lCpfCnpj = string.Empty;

                if (pParametros.Objeto.ChaveCliente.CD_CPFCGC.ToString().Length <= 11)
                {
                    lCpfCnpj = pParametros.Objeto.ChaveCliente.CD_CPFCGC.ToString().PadLeft(11, '0');
                }
                else
                {
                    if (pParametros.Objeto.ChaveCliente.CD_CPFCGC.ToString().Length > 11)
                    {
                        lCpfCnpj = pParametros.Objeto.ChaveCliente.CD_CPFCGC.ToString().PadLeft(15, '0');
                    }
                }

                string lKey = string.Format("{0}-{1}", lCpfCnpj, pParametros.Objeto.ChaveCliente.DT_NASC_FUND.ToString("ddMMyyyy"));

                if (ObjetosCompartilhados.lHashClientes.Contains(lKey))
                {
                    SinacorMigracaoLoginInfo lSinacorMigracaoLoginInfo = (SinacorMigracaoLoginInfo)(ObjetosCompartilhados.lHashClientes[lKey]);

                    lLogin = new LoginInfo();

                    lLogin.CdAssinaturaEletronica = lSinacorMigracaoLoginInfo.DsAssinatura;
                    lLogin.CdSenha = lSinacorMigracaoLoginInfo.DsSenha;
                    lLogin.DsEmail = lSinacorMigracaoLoginInfo.DsEmail;
                    lLogin.DsRespostaFrase = string.Empty;
                    lLogin.DtUltimaExpiracao = DateTime.Now;
                    lLogin.IdFrase = null;
                    lLogin.IdLogin = null;
                    lLogin.NrTentativasErradas = 0;
                    lLogin.TpAcesso = eTipoAcesso.Cliente;
                    lLogin.CdAssessor = lSinacorMigracaoLoginInfo.IdAssessor;
                }
                else
                {
                    lLogin = new LoginInfo();
                    string lSenhaMigracao = ConfigurationManager.AppSettings["SenhaMigracao"].ToString();

                    lLogin.CdAssinaturaEletronica = lSenhaMigracao;
                    lLogin.CdSenha = lSenhaMigracao;
                    lLogin.DsEmail = "a@a.a";
                    lLogin.DsRespostaFrase = string.Empty;
                    lLogin.DtUltimaExpiracao = DateTime.Now;
                    lLogin.IdFrase = null;
                    lLogin.IdLogin = null;
                    lLogin.NrTentativasErradas = 0;
                    lLogin.TpAcesso = eTipoAcesso.Cliente;
                    lLogin.CdAssessor = null;
                }


                lTelefone = ConversaoSinacor.ConverterTelefone(pCliente.Telefones);

                lEndereco = ConversaoSinacor.ConverterEndereco(pCliente.Enderecos);

                lContaBancaria = ConversaoSinacor.ConverterContaBancaria(pCliente.ContasBancarias);

                lConta = ConversaoSinacor.ConverterConta(pCliente.Bovespas, pCliente.Bmfs, pCliente.AtividadesCc, pCliente.AtividadesCustodia);

                lConta = this.SetContaPrincipal(pCliente.ChaveCliente, lConta);

                var AssessorContaPrincipal = (from cp in lConta
                                              where cp.StPrincipal == true
                                              select new { cp.CdAssessor }).ToList();

                if (null == lCliente.IdAssessorInicial)
                    lCliente.IdAssessorInicial = AssessorContaPrincipal[0].CdAssessor;

                lDiretor = ConversaoSinacor.ConverterDiretor(pCliente.Diretor);

                lEmitente = ConversaoSinacor.ConverterEmitente(pCliente.Emitentes);

                lSfp = ConversaoSinacor.ConverterSFP(pCliente.SituacaoFinaceiraPatrimoniais);

                SalvarEntidadeResponse<SinacorClienteInfo> lRetorno = new SalvarEntidadeResponse<SinacorClienteInfo>();

                pCliente.Enderecos.ForEach(ende => {
                    if (ende.IN_TIPO_ENDE.HasValue && ende.IN_TIPO_ENDE == 'C')
                    {
                        lCliente.DsEmailComercial = ende.DS_EMAIL_COMERCIAL;
                    }
                    });

                lRetorno.Codigo =
                    this.InserirCliente(
                    pParametros.Objeto.StReimportacao,
                    lLogin,
                    lCliente,
                    lTelefone,
                    lEndereco,
                    lContaBancaria,
                    lConta,
                    lDiretor,
                    lEmitente,
                    lSfp
                );

                LogCadastro.Logar(pParametros.Objeto.ChaveCliente, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Importar);

                //Atualizar Ativação da informação referenta à CliGer e acesso ao HB
                Boolean stAtivoCliGer = pCliente.ClienteGeral.IN_SITUAC.Value == 'A' ? true : false;

                ReceberObjetoResponse<ClienteAtivarInativarInfo> RetornoClienteAtivar = new ReceberObjetoResponse<ClienteAtivarInativarInfo>();
                ReceberEntidadeRequest<ClienteAtivarInativarInfo> EntradaClienteAtivar = new ReceberEntidadeRequest<ClienteAtivarInativarInfo>() { DescricaoUsuarioLogado = "Processo de Exportação" };
                EntradaClienteAtivar.Objeto = new ClienteAtivarInativarInfo() { IdCliente = lRetorno.Codigo.Value };
                EntradaClienteAtivar.DescricaoUsuarioLogado = "Processo de Importação";
                RetornoClienteAtivar = ClienteDbLib.ReceberClienteAtivarInativar(EntradaClienteAtivar);

                SalvarEntidadeRequest<ClienteAtivarInativarInfo> EntradaSalvar = new SalvarEntidadeRequest<ClienteAtivarInativarInfo>();
                EntradaSalvar.Objeto = RetornoClienteAtivar.Objeto;
                EntradaSalvar.Objeto.StClienteGeralAtivo = stAtivoCliGer;
                EntradaSalvar.Objeto.StHbAtivo = stAtivoCliGer;
                EntradaSalvar.DescricaoUsuarioLogado = "Processo de Importação";

                SalvarEntidadeResponse<ClienteAtivarInativarInfo> RetornoSalvar = ClienteDbLib.SalvarClienteAtivarInativar(EntradaSalvar);

                return lRetorno;
            }
            catch(Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto.ChaveCliente, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Importar, ex);
                throw ex;
            }
            
        }

        /// <summary>
        /// Método responsável por Inserir Cliente com todos os dados
        /// </summary>
        /// <DataCriacao>14/05/2010</DataCriacao>
        /// <Autor>Gustavo Malta Guimarães</Autor>
        /// <Alteracao>
        ///     <DataAlteracao></DataAlteracao>
        ///     <Autor></Autor>
        ///     <Motivo></Motivo>
        /// </Alteracao>
        /// <param name="pLogin">Login</param>
        /// <param name="pCliente">Cliente</param>
        /// <param name="pTelefone">Lista de Telefones</param>
        /// <param name="pEndereco">Lista de Endereços</param>
        /// <param name="pContaBancaria">Lista de Contas Bancárias</param>
        /// <param name="pConta">Lista de Contas</param>
        /// <param name="pDiretor">Lista de Diretores</param>
        /// <param name="pEmitente">Lista de Emitentes</param>
        /// <param name="pSfp">Situação Financeira Patrimonial</param>
        public int InserirCliente
        (Boolean pStReimportacao
        , LoginInfo pLogin
        , ClienteInfo pCliente
        , List<ClienteTelefoneInfo> pTelefone
        , List<ClienteEnderecoInfo> pEndereco
        , List<ClienteBancoInfo> pContaBancaria
        , List<ClienteContaInfo> pConta
        , List<ClienteDiretorInfo> pDiretor
        , List<ClienteEmitenteInfo> pEmitente
        , ClienteSituacaoFinanceiraPatrimonialInfo pSfp)
        {
            //Criar Transação
            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoSql;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();

            try
            {
                Nullable<int> lIdClienteAntigo = null;

                Nullable<int> lIdLoginAntigo = null;

                ClienteNaoOperaPorContaPropriaInfo lClienteOperaContaPropria = null;

                List<ClientePendenciaCadastralInfo> lPendenciasCad = null;

                List<ClienteContratoInfo> lContratosCliente = null;

                List<LogIntranetInfo> lLogsIntranet = null;

                LoginInfo lLoginReimportacao = null;

                ClienteAutorizacaoInfo lAutorizacao = null;

                if (pStReimportacao)
                {   
                    //-->> Consultar pendencias cadastrais do cliente.
                    ClienteDbLib.SelecionaClientePorDataNacCpfCnpj(new RemoverEntidadeRequest<ClienteInfo>() { Objeto = pCliente });

                    lIdClienteAntigo = pCliente.IdCliente;

                    lIdLoginAntigo = pLogin.IdLogin;

                    //-->> Recupera as informações de contratos de clientes
                    lContratosCliente = ClienteDbLib.ConsultarClienteContrato(pCliente.IdCliente.Value);

                    //--> consulta pendencias cadastrais para posteriormente inserir novamente
                    lPendenciasCad =  ClienteDbLib.ListarClientePendenciaCadastral(pCliente.IdCliente.Value);

                    //--> consulta para ver se o cliente opera por conta Própria
                    lClienteOperaContaPropria = ClienteDbLib.ConsultarClienteNaoOperaPorContaPropria(pCliente.IdCliente.Value);

                    if (lClienteOperaContaPropria.IdCliente  == 0)
                    {
                        lClienteOperaContaPropria = null;
                    }

                    //-->> Recupera as informações de log na intranet para depois ser inseridas novamente com o id_login novo
                    lLogsIntranet = ClienteDbLib.ConsultarLogsClientes(pCliente.IdLogin.Value);

                    //-->> Recupera as informações de login do usuario reimportado
                    lLoginReimportacao = ClienteDbLib.ReceberLogin(pCliente.DsCpfCnpj, pCliente.DtNascimentoFundacao.Value);

                    //-->> Recupera as informações de autorizações do cliente, caso haja alguma
                    lAutorizacao = ClienteDbLib.ReceberAutorizacoesCadastrais(pCliente.IdCliente.Value);

                    //--> Verifica a necessidade de exclusão do cliente antes de iserrir
                    ClienteDbLib.RemoverCliente(trans, new RemoverEntidadeRequest<ClienteInfo>() { Objeto = pCliente });
                }

                //Inserir Login
                SalvarObjetoRequest<LoginInfo> lLogin = new SalvarObjetoRequest<LoginInfo>();

                lLogin.Objeto = (pStReimportacao) ? lLoginReimportacao : pLogin;
                pLogin.IdLogin = ClienteDbLib.SalvarLogin(trans, lLogin, false).Codigo;

                //Colocar idLogin no cliente
                pCliente.IdLogin = pLogin.IdLogin.Value;

                //Inserir Cliente
                SalvarObjetoRequest<ClienteInfo> lCliente = new SalvarObjetoRequest<ClienteInfo>();
                lCliente.Objeto = pCliente;

                if (lClienteOperaContaPropria != null)
                {
                    lCliente.Objeto.DadosClienteNaoOperaPorContaPropria = lClienteOperaContaPropria;
                }
                else
                {
                    lCliente.Objeto.DadosClienteNaoOperaPorContaPropria = null; //--> Este dado não existe no Sinacor, portanto não deve ser informado.
                }
                
                

                pCliente.IdCliente = ClienteDbLib.SalvarCliente(trans, lCliente, false, pStReimportacao).Codigo;

                //Faz o swap de cliente no suitability
                if (pStReimportacao && lIdClienteAntigo != null)
                {
                    ClienteDbLib.SalvarImportacaoSuitability(lIdClienteAntigo.Value, pCliente.IdCliente.Value, trans);

                    if (lPendenciasCad != null && lPendenciasCad.Count > 0)
                    {
                        lPendenciasCad.ForEach(pend =>
                        {
                            ClientePendenciaCadastralInfo lPendencia = new ClientePendenciaCadastralInfo();

                            lPendencia = pend;

                            lPendencia.IdCliente = pCliente.IdCliente;

                            lPendencia.IdPendenciaCadastral = null;

                            lPendencia.IdLoginRealizacao = null;

                            ClienteDbLib.SalvarClientePendenciaCadastral(trans, lPendencia);
                        });
                    }
                }

                //--> Verifica se o cliente preencheu o contrato de 
                //--> legislação anteriormente e salva com o código novo do cliente
                if (lContratosCliente != null && lContratosCliente.Count > 0)
                {
                    lContratosCliente.ForEach(contrato =>
                    {
                        ClienteContratoInfo lContrato = new ClienteContratoInfo();

                        lContrato.IdCliente   = pCliente.IdCliente;

                        lContrato.IdContrato  = contrato.IdContrato;

                        lContrato.DtAssinatura = contrato.DtAssinatura;

                        ClienteDbLib.SalvarImportacaoClienteContrato(trans, lContrato);
                    });
                }

                //--> Verifica se há logs na intranet do cliente para gravar novamente
                if (lLogsIntranet != null && lLogsIntranet.Count > 0)
                {
                    lLogsIntranet.ForEach(log => {
                        
                        LogIntranetInfo lLog = new LogIntranetInfo();

                        lLog = log;

                        lLog.IdLogin = pCliente.IdLogin.Value;

                        lLog.DsObservacao += string.Concat(" ",lLog.IdLogin);

                        ClienteDbLib.RegistrarLog(trans, lLog);
                    });
                }

                //--> Insere novamente a autorização, se houver 
                if (lAutorizacao != null && lAutorizacao.StAutorizado != null)
                {
                    lAutorizacao.IdCliente = pCliente.IdCliente.Value;
                    ClienteDbLib.SalvarAutorizacaoCadastralImportacao( trans, lAutorizacao );
                }

                //colocar idCliente nas outras entidades
                //Inserir outras entidades
                SalvarObjetoRequest<ClienteTelefoneInfo> lTelefone;
                foreach (ClienteTelefoneInfo item in pTelefone)
                {
                    item.IdCliente = pCliente.IdCliente.Value;
                    lTelefone = new SalvarObjetoRequest<ClienteTelefoneInfo>();
                    lTelefone.Objeto = item;
                    ClienteDbLib.SalvarClienteTelefone(trans, lTelefone);
                }

                SalvarObjetoRequest<ClienteEnderecoInfo> lEndereco;
                foreach (ClienteEnderecoInfo item in pEndereco)
                {
                    item.IdCliente = pCliente.IdCliente.Value;
                    lEndereco = new SalvarObjetoRequest<ClienteEnderecoInfo>();
                    lEndereco.Objeto = item;
                    ClienteDbLib.SalvarClienteEndereco(trans, lEndereco);
                }

                SalvarObjetoRequest<ClienteBancoInfo> lBanco;
                foreach (ClienteBancoInfo item in pContaBancaria)
                {
                    item.IdCliente = pCliente.IdCliente.Value;
                    lBanco = new SalvarObjetoRequest<ClienteBancoInfo>();
                    lBanco.Objeto = item;
                    ClienteDbLib.SalvarClienteBanco(trans, lBanco);
                }

                SalvarObjetoRequest<ClienteContaInfo> lConta;
                foreach (ClienteContaInfo item in pConta)
                {
                    item.IdCliente = pCliente.IdCliente.Value;
                    lConta = new SalvarObjetoRequest<ClienteContaInfo>();
                    lConta.Objeto = item;
                    ClienteDbLib.SalvarClienteConta(trans, lConta);
                }

                SalvarObjetoRequest<ClienteDiretorInfo> lDiretor;
                foreach (ClienteDiretorInfo item in pDiretor)
                {
                    item.IdCliente = pCliente.IdCliente.Value;
                    lDiretor = new SalvarObjetoRequest<ClienteDiretorInfo>();
                    lDiretor.Objeto = item;
                    ClienteDbLib.SalvarClienteDiretor(trans, lDiretor);
                }

                SalvarObjetoRequest<ClienteEmitenteInfo> lEmitente;
                foreach (ClienteEmitenteInfo item in pEmitente)
                {
                    item.IdCliente = pCliente.IdCliente.Value;
                    lEmitente = new SalvarObjetoRequest<ClienteEmitenteInfo>();
                    lEmitente.Objeto = item;
                    ClienteDbLib.SalvarClienteEmitente(trans, lEmitente);
                }

                pSfp.IdCliente = pCliente.IdCliente.Value;
                SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo> lSfp = new SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo>();
                lSfp.Objeto = pSfp;
                ClienteDbLib.SalvarClienteSituacaoFinanceiraPatrimonial(trans, lSfp);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoControleAcesso;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_UsuariosPermissoes_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoItem", DbType.Int32, pLogin.IdLogin.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@Status", DbType.Int32, 0);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoPermissao", DbType.String, "22FF518C-C7D3-4ff0-A0CB-96F2476068BB");
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoRisco;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_permissoes_default_cliente"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, gContaPrincipal);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                trans.Commit();

                return pCliente.IdCliente.Value;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
                if (!ConnectionState.Closed.Equals(conn.State)) conn.Close();
                conn.Dispose();
                conn = null;
            }
        }

        private Int64 GetContaPrincipal()
        {
            ConexaoDbHelper lConexaoDbHelper = new ConexaoDbHelper();
            lConexaoDbHelper.ConnectionStringName = gNomeConexaoOracle;
            Int64 lRetorno = 0;
            using (DbCommand lDbCommand = lConexaoDbHelper.CreateCommand(CommandType.StoredProcedure, "prc_sel_codigo_principal"))
            {
                lConexaoDbHelper.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, this.lChaveCliente.CD_CPFCGC);
                lConexaoDbHelper.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, this.lChaveCliente.DT_NASC_FUND);
                lConexaoDbHelper.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int16, this.lChaveCliente.CD_CON_DEP);
                lConexaoDbHelper.AddOutParameter(lDbCommand, "pCD_CLIENTE", DbType.Int64, 8);
                lConexaoDbHelper.ExecuteNonQuery(lDbCommand);
                lRetorno = lDbCommand.Parameters["pCD_CLIENTE"].Value.DBToInt32();
            }
            return lRetorno;
        }

        private DataTable ExecuteDataTable(string pProc)
        {
            ConexaoDbHelper lConexaoDbHelper = new ConexaoDbHelper();

            DataTable lRetorno;

            lConexaoDbHelper.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lConexaoDbHelper.CreateCommand(CommandType.StoredProcedure, pProc))
            {

                lConexaoDbHelper.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, this._lChaveCliente.CD_CPFCGC);
                lConexaoDbHelper.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, this._lChaveCliente.DT_NASC_FUND);
                lConexaoDbHelper.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int16, this._lChaveCliente.CD_CON_DEP);

                lRetorno = lConexaoDbHelper.ExecuteOracleDataTable(lDbCommand);
            }
            return lRetorno;
        }

        private List<SinacorChaveClienteInfo> ListarTodos()
        {
            List<SinacorChaveClienteInfo> lRetorno = new List<SinacorChaveClienteInfo>();
            AcessaDados lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;
           // using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_todos"))
            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_clientefundos"))
            {
                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                SinacorChaveClienteInfo lLinha;

                int count = lDataTable.Rows.Count;

                foreach (DataRow item in lDataTable.Rows)
                {
                    lLinha = new SinacorChaveClienteInfo();

                    lLinha.CD_CPFCGC = item["CD_CPFCGC"].DBToInt64();
                    lLinha.DT_NASC_FUND = item["DT_NASC_FUND"].DBToDateTime();
                    lLinha.CD_CON_DEP = item["CD_CON_DEP"].DBToInt32();

                    lRetorno.Add(lLinha);
                }
            }
            return lRetorno;
        }

        private SinacorClienteGeralInfo GetClienteGeral()
        {
            SinacorClienteGeralInfo lRetorno = new SinacorClienteGeralInfo();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tsccliger");

            if (lDataTable != null && lDataTable.Rows.Count == 1)
            {
                lRetorno.DT_CRIACAO = lDataTable.Rows[0]["DT_CRIACAO"].DBToDateTime(eDateNull.Permite);
                lRetorno.IN_PESS_VINC = lDataTable.Rows[0]["IN_PESS_VINC"].DBToChar();
                lRetorno.IN_POLITICO_EXP = lDataTable.Rows[0]["IN_POLITICO_EXP"].DBToChar();
                lRetorno.IN_SITUAC = lDataTable.Rows[0]["IN_SITUAC"].DBToChar();
                lRetorno.NM_CLIENTE = lDataTable.Rows[0]["NM_CLIENTE"].DBToString();
                lRetorno.TP_CLIENTE = lDataTable.Rows[0]["TP_CLIENTE"].DBToInt32();
                lRetorno.TP_PESSOA = lDataTable.Rows[0]["TP_PESSOA"].DBToChar();
            }
            else
            {
                throw new Exception("Cliente não encontrado no Sinacor");
            }

            lDataTable = ExecuteDataTable("prc_sel_tscclibol");

            if (lDataTable != null && lDataTable.Rows.Count > 0)
            {
                lRetorno.OPERA_CONTA_PROPRIA = lDataTable.Rows[0]["IN_CART_PROP"].DBToChar() == 'S' ? true : false;
            }

            return lRetorno;
        }

        private SinacorClienteComplementoInfo GetClienteComplemento()
        {
            SinacorClienteComplementoInfo lRetorno = new SinacorClienteComplementoInfo();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tscclicomp");

            if (lDataTable != null && lDataTable.Rows.Count == 1)
            {
                lRetorno.CD_ATIV = lDataTable.Rows[0]["CD_ATIV"].DBToInt32();
                lRetorno.CD_EST_CIVIL = lDataTable.Rows[0]["CD_EST_CIVIL"].DBToInt32();
                lRetorno.CD_NACION = lDataTable.Rows[0]["CD_NACION"].DBToInt32();
                lRetorno.ID_SEXO = lDataTable.Rows[0]["ID_SEXO"].DBToChar();
                lRetorno.NM_CONJUGE = lDataTable.Rows[0]["NM_CONJUGE"].DBToString();
                lRetorno.NM_EMPRESA = lDataTable.Rows[0]["NM_EMPRESA"].DBToString();
                lRetorno.NM_LOC_NASC = lDataTable.Rows[0]["NM_LOC_NASC"].DBToString();
                lRetorno.NM_MAE = lDataTable.Rows[0]["NM_MAE"].DBToString();
                lRetorno.NM_PAI = lDataTable.Rows[0]["NM_PAI"].DBToString();
                lRetorno.SG_ESTADO_EMIS = lDataTable.Rows[0]["SG_ESTADO_EMIS"].DBToString();
                lRetorno.SG_ESTADO_NASC = lDataTable.Rows[0]["SG_ESTADO_NASC"].DBToString();
                lRetorno.SG_PAIS = lDataTable.Rows[0]["SG_PAIS"].DBToString();
                lRetorno.CD_ESCOLARIDADE = lDataTable.Rows[0]["CD_ESCOLARIDADE"].DBToInt32();
                lRetorno.CD_NIRE = lDataTable.Rows[0]["CD_NIRE"].DBToInt32();
                lRetorno.DS_CARGO = lDataTable.Rows[0]["DS_CARGO"].DBToString();
                //Documentos
                //* Outros
                //CD_DOC_IDENT - Número - VARCHAR2(16)   
                lRetorno.CD_DOC_IDENT = lDataTable.Rows[0]["CD_DOC_IDENT"].DBToString();
                //CD_TIPO_DOC - Tipo - VARCHAR2(2)   
                lRetorno.CD_TIPO_DOC = lDataTable.Rows[0]["CD_TIPO_DOC"].DBToString();
                //CD_ORG_EMIT - Órgão - VARCHAR2(4)   
                lRetorno.CD_ORG_EMIT = lDataTable.Rows[0]["CD_ORG_EMIT"].DBToString();
                //SG_ESTADO_EMIS - UF - VARCHAR2(4)
                lRetorno.SG_ESTADO_EMIS = lDataTable.Rows[0]["SG_ESTADO_EMIS"].DBToString();
                //DT_DOC_IDENT - Data - DATE
                lRetorno.DT_DOC_IDENT = lDataTable.Rows[0]["DT_DOC_IDENT"].DBToDateTime(eDateNull.Permite);
                //* RG 
                //NR_RG - Número - VARCHAR2(16)     
                lRetorno.NR_RG = lDataTable.Rows[0]["NR_RG"].DBToString();
                //SG_ESTADO_EMISS_RG - UF - VARCHAR2(4)
                lRetorno.SG_ESTADO_EMISS_RG = lDataTable.Rows[0]["SG_ESTADO_EMISS_RG"].DBToString();
                //DT_EMISS_RG - Data - DATE 
                lRetorno.DT_EMISS_RG = lDataTable.Rows[0]["DT_EMISS_RG"].DBToDateTime(eDateNull.Permite);
                //CD_ORG_EMIT_RG - Órgão - VARCHAR2(4) 
                lRetorno.CD_ORG_EMIT_RG = lDataTable.Rows[0]["CD_ORG_EMIT_RG"].DBToString();
            }
            else
            {
                throw new Exception("Cliente não encontrado no Sinacor");
            }
            return lRetorno;
        }

        private List<SinacorEmitenteInfo> GetEmitentes()
        {
            List<SinacorEmitenteInfo> lRetorno = new List<SinacorEmitenteInfo>();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tscemitordem");
            SinacorEmitenteInfo lEmitente;

            foreach (DataRow item in lDataTable.Rows)
            {
                lEmitente = new SinacorEmitenteInfo();

                lEmitente.CD_CPFCGC_EMIT    = item["CD_CPFCGC_EMIT"].DBToInt64();
                lEmitente.CD_DOC_IDENT_EMIT = item["CD_DOC_IDENT_EMIT"].DBToString();
                lEmitente.CD_SISTEMA        = item["CD_SISTEMA"].DBToString();
                lEmitente.IN_PRINCIPAL      = item["IN_PRINCIPAL"].DBToChar();
                lEmitente.NM_EMIT_ORDEM     = item["NM_EMIT_ORDEM"].DBToString();
                lEmitente.NM_E_MAIL         = item["NM_E_MAIL"].DBToString();
                lEmitente.TM_STAMP          = item["TM_STAMP"].DBToDateTime(eDateNull.Permite);
                lRetorno.Add(lEmitente);
            }

            return lRetorno;
        }

        private SinacorDiretorInfo GetDiretor()
        {
            SinacorDiretorInfo lRetorno = new SinacorDiretorInfo();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tsccvm220");

            if (lDataTable != null && lDataTable.Rows.Count == 1)
            {
                lRetorno.CD_CPFCGC_DIR1    = lDataTable.Rows[0]["CD_CPFCGC_DIR1"].DBToInt64();
                lRetorno.CD_CPFCGC_DIR2    = lDataTable.Rows[0]["CD_CPFCGC_DIR2"].DBToInt64();
                lRetorno.CD_CPFCGC_DIR3    = lDataTable.Rows[0]["CD_CPFCGC_DIR3"].DBToInt64();
                lRetorno.CD_DOC_IDENT_DIR1 = lDataTable.Rows[0]["CD_DOC_IDENT_DIR1"].DBToString();
                lRetorno.CD_DOC_IDENT_DIR2 = lDataTable.Rows[0]["CD_DOC_IDENT_DIR2"].DBToString();
                lRetorno.CD_DOC_IDENT_DIR3 = lDataTable.Rows[0]["CD_DOC_IDENT_DIR3"].DBToString();
                lRetorno.NM_DIRETOR_1      = lDataTable.Rows[0]["NM_DIRETOR_1"].DBToString();
                lRetorno.NM_DIRETOR_2      = lDataTable.Rows[0]["NM_DIRETOR_2"].DBToString();
                lRetorno.NM_DIRETOR_3      = lDataTable.Rows[0]["NM_DIRETOR_3"].DBToString();
                lRetorno.DS_FORMACAO       = lDataTable.Rows[0]["DS_FORMACAO"].DBToString();
                lRetorno.NR_INSCRICAO      = lDataTable.Rows[0]["NR_INSCRICAO"].DBToString();
            }
            return lRetorno;
        }

        private SinacorCcInfo GetCc()
        {
            SinacorCcInfo lRetorno = new SinacorCcInfo();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tscclicc");

            if (lDataTable != null && lDataTable.Rows.Count == 1)
            {
                lRetorno.CD_ASSESSOR = lDataTable.Rows[0]["CD_ASSESSOR"].DBToInt32();
                lRetorno.IN_CARPRO   = lDataTable.Rows[0]["IN_CARPRO"].DBToChar();
            }
            return lRetorno;
        }

        private List<SinacorEnderecoInfo> GetEnderecos()
        {
            List<SinacorEnderecoInfo> lRetorno = new List<SinacorEnderecoInfo>();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tscende");
            SinacorEnderecoInfo lEndereco;

            foreach (DataRow item in lDataTable.Rows)
            {
                lEndereco = new SinacorEnderecoInfo();

                lEndereco.CD_CEP        = item["CD_CEP"].DBToInt32();
                lEndereco.CD_CEP_EXT    = item["CD_CEP_EXT"].DBToInt32();
                lEndereco.IN_ENDE_CORR  = item["IN_ENDE_CORR"].DBToChar();
                lEndereco.IN_TIPO_ENDE  = item["IN_TIPO_ENDE"].DBToChar();
                lEndereco.NM_BAIRRO     = item["NM_BAIRRO"].DBToString();
                lEndereco.NM_CIDADE     = item["NM_CIDADE"].DBToString();
                lEndereco.NM_COMP_ENDE  = item["NM_COMP_ENDE"].DBToString();
                lEndereco.NM_LOGRADOURO = item["NM_LOGRADOURO"].DBToString();
                lEndereco.NR_PREDIO     = item["NR_PREDIO"].DBToString();
                lEndereco.SG_ESTADO     = item["SG_ESTADO"].DBToString();
                lEndereco.SG_PAIS       = item["SG_PAIS"].DBToString();

                if (lEndereco.IN_TIPO_ENDE.HasValue && lEndereco.IN_TIPO_ENDE.Value == 'C')
                {
                    lEndereco.DS_EMAIL_COMERCIAL = item["NM_E_MAIL"].DBToString();
                }
                else if (lEndereco.IN_TIPO_ENDE.HasValue && lEndereco.IN_TIPO_ENDE.Value == 'R')
                {
                    lEndereco.DS_EMAIL = item["NM_E_MAIL"].DBToString();
                }

                lRetorno.Add(lEndereco);
            }
            return lRetorno;
        }

        private List<SinacorTelefoneInfo> GetTelefones()
        {
            List<SinacorTelefoneInfo> lRetorno = new List<SinacorTelefoneInfo>();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tscende");
            SinacorTelefoneInfo lTelefone;

            foreach (DataRow item in lDataTable.Rows)
            {
                lTelefone = new SinacorTelefoneInfo();

                lTelefone.CD_DDD_CELULAR1 = item["CD_DDD_CELULAR1"].DBToInt32();
                lTelefone.CD_DDD_CELULAR2 = item["CD_DDD_CELULAR2"].DBToInt32();
                lTelefone.CD_DDD_TEL      = item["CD_DDD_TEL"].DBToInt32();
                lTelefone.IN_ENDE_OFICIAL = item["IN_ENDE_OFICIAL"].DBToChar();
                lTelefone.IN_TIPO_ENDE    = item["IN_TIPO_ENDE"].DBToChar();
                lTelefone.NR_CELULAR1     = item["NR_CELULAR1"].DBToInt64();
                lTelefone.NR_CELULAR2     = item["NR_CELULAR2"].DBToInt64();
                lTelefone.NR_RAMAL        = item["NR_RAMAL"].DBToInt32();
                lTelefone.NR_TELEFONE     = item["NR_TELEFONE"].DBToInt64();
                lTelefone.CD_DDD_FAX      = item["CD_DDD_FAX"].DBToInt32();
                lTelefone.NR_FAX          = item["NR_FAX"].DBToInt64();
                lTelefone.CD_DDD_FAX2     = item["CD_DDD_FAX2"].DBToInt32();
                lTelefone.NR_FAX2         = item["NR_FAX2"].DBToInt64();

                lRetorno.Add(lTelefone);
            }

            return lRetorno;
        }

        private List<SinacorSituacaoFinanceiraPatrimonialInfo> GetSituacaoFinaceiraPatrimoniais()
        {
            List<SinacorSituacaoFinanceiraPatrimonialInfo> lRetorno = new List<SinacorSituacaoFinanceiraPatrimonialInfo>();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tscsfp");
            SinacorSituacaoFinanceiraPatrimonialInfo lSFP;

            foreach (DataRow item in lDataTable.Rows)
            {
                lSFP = new SinacorSituacaoFinanceiraPatrimonialInfo();

                lSFP.CD_SFPGRUPO = item["CD_SFPGRUPO"].DBToInt32();
                lSFP.CD_SFPSUBGRUPO = item["CD_SFPSUBGRUPO"].DBToInt32();
                lSFP.DS_BEN = item["DS_BEN"].DBToString();
                lSFP.DT_VENCIMENTO = item["DT_VENCIMENTO"].DBToDateTime(eDateNull.Permite);
                lSFP.IN_ONUS = item["IN_ONUS"].DBToChar();
                lSFP.PC_LIMITE = item["PC_LIMITE"].DBToDecimal();
                lSFP.SG_ESTADO = item["SG_ESTADO"].DBToString();
                lSFP.VL_BEN = item["VL_BEN"].DBToDecimal();
                lSFP.VL_DEVEDOR = item["VL_DEVEDOR"].DBToDecimal();

                lRetorno.Add(lSFP);
            }

            return lRetorno;
        }

        private List<SinacorBovespaInfo> GetBovespas()
        {
            List<SinacorBovespaInfo> lRetorno = new List<SinacorBovespaInfo>();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tscclibol");
            SinacorBovespaInfo lBovespa;

            foreach (DataRow item in lDataTable.Rows)
            {
                lBovespa = new SinacorBovespaInfo();

                lBovespa.CD_ASSESSOR         = item["CD_ASSESSOR"].DBToInt32();
                lBovespa.CD_CLIENTE          = item["CD_CLIENTE"].DBToInt32();
                lBovespa.IN_CONTA_INV        = item["IN_CONTA_INV"].DBToChar();
                lBovespa.IN_SITUAC           = item["IN_SITUAC"].DBToChar();
                
                lRetorno.Add(lBovespa);
            }

            return lRetorno;
        }

        private List<SinacorAtividadeCcInfo> GetAtividadesCc()
        {
            List<SinacorAtividadeCcInfo> lRetorno = new List<SinacorAtividadeCcInfo>();
            DataTable lDataTable = ExecuteDataTable("prc_sel_ativ_tscclicc");
            SinacorAtividadeCcInfo lBovespa;

            foreach (DataRow item in lDataTable.Rows)
            {
                lBovespa = new SinacorAtividadeCcInfo();

                lBovespa.CD_ASSESSOR = item["CD_ASSESSOR"].DBToInt32();
                lBovespa.CD_CLIENTE = item["CD_CLIENTE"].DBToInt32();
                lBovespa.IN_CONTA_INV = item["IN_CONTA_INV"].DBToChar();
                lBovespa.IN_SITUAC = item["IN_SITUAC"].DBToChar();

                lRetorno.Add(lBovespa);
            }

            return lRetorno;
        }

        private List<SinacorAtividadeCustodiaInfo> GetAtividadesCustodia()
        {
            List<SinacorAtividadeCustodiaInfo> lRetorno = new List<SinacorAtividadeCustodiaInfo>();
            DataTable lDataTable = ExecuteDataTable("prc_sel_ativ_tscclicus");
            SinacorAtividadeCustodiaInfo lBovespa;

            foreach (DataRow item in lDataTable.Rows)
            {
                lBovespa = new SinacorAtividadeCustodiaInfo();

                lBovespa.CD_ASSESSOR = item["CD_ASSESSOR"].DBToInt32();
                lBovespa.CD_CLIENTE = item["CD_CLIENTE"].DBToInt32();
                lBovespa.IN_CONTA_INV = item["IN_CONTA_INV"].DBToChar();
                lBovespa.IN_SITUAC = item["IN_SITUAC"].DBToChar();

                lRetorno.Add(lBovespa);
            }

            return lRetorno;
        }

        private List<SinacorBmfInfo> GetBmfs()
        {
            List<SinacorBmfInfo> lRetorno = new List<SinacorBmfInfo>();
            DataTable lDataTable = ExecuteDataTable("prc_sel_tscclibmf");
            SinacorBmfInfo lBmf;

            foreach (DataRow item in lDataTable.Rows)
            {
                lBmf = new SinacorBmfInfo();

                lBmf.CODASS = item["CODASS"].DBToInt32();
                lBmf.CODCLI = item["CODCLI"].DBToInt32();
                lBmf.IN_CONTA_INV = item["IN_CONTA_INV"].DBToChar();
                lBmf.STATUS = item["STATUS"].DBToChar();

                lRetorno.Add(lBmf);
            }

            return lRetorno;
        }

        private List<SinacorContaBancariaInfo> GetContasBancarias(SinacorClienteInfo pCliente)
        {
            List<SinacorContaBancariaInfo> lRetorno = new List<SinacorContaBancariaInfo>();
            AcessaDados lAcessaDados = new AcessaDados();
            DataTable lDataTable = new DataTable();
            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            //A conta bancário pode estar relacionada com Bovespa ou BMF
            //Foi usado um Hash para não deixar duplicar as contas, poi cliente possuem o mesmo código para conta Bovespa e BMF
            Hashtable lContas = new Hashtable();
            foreach (SinacorBovespaInfo item in pCliente.Bovespas)
                lContas.Add(item.CD_CLIENTE.Value, item.CD_CLIENTE.Value);

            foreach (SinacorBmfInfo item in pCliente.Bmfs)
                if (!lContas.Contains(item.CODCLI.Value))
                    lContas.Add(item.CODCLI.Value, item.CODCLI.Value);

            SinacorContaBancariaInfo lConta;

            foreach (DictionaryEntry item in lContas)
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_tscclicta"))
                {
                    lDataTable.Rows.Clear();
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, item.Value);
                    lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    foreach (DataRow linha in lDataTable.Rows)
                    {
                        lConta = new SinacorContaBancariaInfo();

                        lConta.CD_AGENCIA = linha["CD_AGENCIA"].DBToString();
                        lConta.DV_AGENCIA = linha["DV_AGENCIA"].DBToString();
                        lConta.CD_BANCO = linha["CD_BANCO"].DBToString();
                        lConta.DV_CONTA = linha["DV_CONTA"].DBToString(); ;
                        lConta.IN_PRINCIPAL = linha["IN_PRINCIPAL"].DBToChar();
                        lConta.NR_CONTA = linha["NR_CONTA"].DBToString();
                        lConta.TP_CONTA = linha["TP_CONTA"].DBToString();

                        lRetorno.Add(lConta);
                    }
                }
            }
            return lRetorno;
        }
    }
}
