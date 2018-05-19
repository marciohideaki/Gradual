using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text;
using Gradual.Cadastro.Entidades;
using Gradual.Cadastro.Negocios;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados;


namespace Gradual.Cadastro.Importacao
{
    class Program
    {
        public Program()
        {
            ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");
        }

        public IServicoPersistenciaCadastro ServicoPersistenciaCadastro
        {
            get
            {
                return Ativador.Get<IServicoPersistenciaCadastro>();
            }
        }

        static void Main(string[] args)
        {

            Program p = new Program();
            try
            {
                p.Importar();
                Console.WriteLine("Programa finalizado. Pressiona uma tecla...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        public void Importar()
        {

            ///Excluir Cliente
            //ExcluirClientes();

            ///Importar Logins
            ///
            
           /* ImportarAdministradores();
            ImportarAtendimento();
            ImportarTeleMarketing();*/
            ImportarAssessores();
            
           // Console.WriteLine("Primeiro passo da importacao concluida");

           //// ImportarTodosClientesSinacor();

           // Console.WriteLine("Clientes do Sinacor importados com sucesso");


           // Console.WriteLine("Iniciado");
           //// ImportarLoginsAlteracoesPasso4();
           //// ImportarClientesPasso123();

           // Console.WriteLine("Clientes passo 1234 finalizdo");

           // /*
            //InativarLogins()

            //ImportarSuitability();

            //Console.WriteLine("Suitability finalizado");

            //ExportarCliente();

            //Importar1DoSinacor();

            //SelecionarAtivacao();
           

            ///TODO Gustavo - Importar atividades
        }

        public void SelecionarAtivacao()
        {
            ReceberEntidadeCadastroResponse<ClienteAtivarInativarInfo> RetornoClienteAtivar = new ReceberEntidadeCadastroResponse<ClienteAtivarInativarInfo>();
            ReceberEntidadeCadastroRequest<ClienteAtivarInativarInfo> EntradaClienteAtivar = new ReceberEntidadeCadastroRequest<ClienteAtivarInativarInfo>();
            EntradaClienteAtivar.EntidadeCadastro = new ClienteAtivarInativarInfo() { IdCliente = 71089 };

            RetornoClienteAtivar = ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteAtivarInativarInfo>(EntradaClienteAtivar);

            SalvarEntidadeCadastroRequest<ClienteAtivarInativarInfo> EntradaSalvar = new SalvarEntidadeCadastroRequest<ClienteAtivarInativarInfo>();

            EntradaSalvar.EntidadeCadastro = RetornoClienteAtivar.EntidadeCadastro;

            //Testar Alterações
            SalvarEntidadeCadastroResponse RetornoSalvar = ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteAtivarInativarInfo>(EntradaSalvar);

        }

        public void CarregarAtividades()
        {
            ///TODO Gustavo - Importar atividades

        }

        /* Importação de Solicitação de Alteração(clientes 123 e importação de LoginAlteracaoPasso4) joga a realização para o login 1 
         
        SET IDENTITY_INSERT tb_login ON 

INSERT INTO tb_login
           (cd_senha,cd_assinaturaeletronica,nr_tentativaserradas,id_frase,ds_respostafrase,dt_ultimaexpiracao,ds_email,tp_acesso,cd_assessor,ds_nome,id_login)
     VALUES
           ('senha','Assinatura',0,1,'',getdate(),'usuarioimportacao@gradualinvestimentos.com.br',1,null,'Usuario de Importação',1)

        SET IDENTITY_INSERT tb_login OFF 

         */

        public void ImportarLoginsAlteracoesPasso4()
        {
            //****************************************************************************************************************************
            //****************************************************************************************************************************
            //****************************************************************************************************************************
            //****************************************************************************************************************************
            ///TODO OBS. Pegar o login do Administrador do cadastro
            int IdLoginAlteracao = 65859;
            //****************************************************************************************************************************
            //****************************************************************************************************************************

            //pegar todos os clientes no sistema novo
            //para cada cliente
            //Se passo = 4
            //Pegar o Login no sistema antido apartir do CPF
            //Salvar todas as Solicitações de alteração
            //salvar o Login no sistema Novo

            ConsultarEntidadeCadastroRequest<ClienteInfo> lEntradaCliente = new ConsultarEntidadeCadastroRequest<ClienteInfo>();
            ConsultarEntidadeCadastroResponse<ClienteInfo> lRetornoCliente = null;
            lEntradaCliente.EntidadeCadastro = new ClienteInfo();

            lRetornoCliente = ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteInfo>(lEntradaCliente);

            string Ok = "";
            string erro = "";
            int iok = 0;
            int ierro = 0;
            ReceberEntidadeCadastroRequest<LoginInfo> lEntradaLoginNovo;
            ReceberEntidadeCadastroResponse<LoginInfo> lRetornoLoginNovo = null;
            ELogin lLoginAntigo;
            SalvarObjetoRequest<LoginInfo> lEntradaSalvarLogin;

            foreach (ClienteInfo lCliente in lRetornoCliente.Resultado)
            {
                if (lCliente.StPasso == 4)
                {
                    try
                    {
                        //Pegar Login no novo
                        lEntradaLoginNovo = new ReceberEntidadeCadastroRequest<LoginInfo>();
                        lEntradaLoginNovo.EntidadeCadastro = new LoginInfo();
                        lEntradaLoginNovo.EntidadeCadastro.IdLogin = lCliente.IdLogin.Value;
                        lRetornoLoginNovo = ServicoPersistenciaCadastro.ReceberEntidadeCadastro<LoginInfo>(lEntradaLoginNovo);

                        //Pegar Login no Antigo apartir do CPF
                        lLoginAntigo = new NLogin().SelecionarPorCPF(lCliente.DsCpfCnpj);

                        //Sobreacrever o Login novo com os dados do antigo
                        lRetornoLoginNovo.EntidadeCadastro.CdAssessor = null;
                        lRetornoLoginNovo.EntidadeCadastro.CdAssinaturaEletronica = lLoginAntigo.Assinatura;
                        lRetornoLoginNovo.EntidadeCadastro.CdSenha = lLoginAntigo.Senha;
                        lRetornoLoginNovo.EntidadeCadastro.DsEmail = lLoginAntigo.Email;
                        lRetornoLoginNovo.EntidadeCadastro.DsNome = "";//já está no cliente
                        lRetornoLoginNovo.EntidadeCadastro.DsRespostaFrase = "";
                        lRetornoLoginNovo.EntidadeCadastro.DtUltimaExpiracao = DateTime.Now;
                        lRetornoLoginNovo.EntidadeCadastro.IdFrase = null;
                        lRetornoLoginNovo.EntidadeCadastro.NrTentativasErradas = 0;
                        lRetornoLoginNovo.EntidadeCadastro.TpAcesso = 0;

                        CarregarSoliciacaoAlteracao(lCliente.IdCliente.Value, lCliente.DsCpfCnpj, IdLoginAlteracao);

                        //Salvar o Login novo 
                        lEntradaSalvarLogin = new SalvarObjetoRequest<LoginInfo>();
                        lEntradaSalvarLogin.Objeto = lRetornoLoginNovo.EntidadeCadastro;
                        //lRetornoSalvarLogin = ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LoginInfo>(lEntradaSalvarLogin);
                        ClienteDbLib.AtualizarPorImportacao(lEntradaSalvarLogin);

                        Ok += lCliente.DsCpfCnpj + Environment.NewLine;
                        iok++;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message != "CPF não encontrado no Sistema Antigo!")
                        {
                            erro += lCliente.DsCpfCnpj + ": " + ex.Message + " - " + ex.StackTrace + Environment.NewLine;
                            ierro++;
                        }
                    }
                }
            }
        }

        private void CarregarSoliciacaoAlteracao(int pIdCliente, string pCPF, int pIdLoginAlteracao)
        {
            ///TODO tratar login realização, pois foi criado posteriormente

            BindingList<EAlteracao> lAlteracaoAntiga = new NAlteracao().Listar(pCPF);
            ClienteAlteracaoInfo lAlteracao;
            SalvarObjetoRequest<ClienteAlteracaoInfo> lEntradaAlteracao;
            DbConnection conn;
            DbTransaction trans;

            foreach (EAlteracao item in lAlteracaoAntiga)
            {
                lAlteracao = new ImportacaoDuc().GetAlteracao(item, pIdLoginAlteracao);
                lAlteracao.IdCliente = pIdCliente;

                if (null != lAlteracao.IdLoginRealizacao)
                    lAlteracao.IdLoginRealizacao = pIdLoginAlteracao;

                //TODO adicionar ALteração
                lEntradaAlteracao = new SalvarObjetoRequest<ClienteAlteracaoInfo>();
                lEntradaAlteracao.Objeto = lAlteracao;


                Conexao._ConnectionStringName = "Cadastro";
                conn = Conexao.CreateIConnection();
                conn.Open();
                trans = conn.BeginTransaction();
                try
                {
                    ClienteDbLib.SalvarClienteAlteracaoImportacao(trans, lEntradaAlteracao);
                    trans.Commit();
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
        }

        #region Importar do DUC

        public void ImportarClientesPasso123()
        {
            //****************************************************************************************************************************
            //****************************************************************************************************************************
            //****************************************************************************************************************************
            //****************************************************************************************************************************
            ///TODO OBS. Pegar o login do Administrador do cadastro
            int IdLoginAlteracao = 65859;
            //****************************************************************************************************************************
            //****************************************************************************************************************************

            //carregar antigo
            //converter para novo
            //salvar com transação
            //pegar próximo


            string ok = "";
            string erro = "";
            int iok = 0;
            int ierro = 0;

            ImportacaoDuc lImportar = new ImportacaoDuc();
            List<int> lIdClientesDuc = lImportar.GetIdClientes();
            ClienteAntigo lClienteDuc;
            ClienteNovo lClienteNovo;
            foreach (int lIdCliente in lIdClientesDuc)
            {
                try
                {
                    lClienteDuc = lImportar.GetClienteDuc(lIdCliente);
                    lClienteNovo = lImportar.Conversao(lClienteDuc, IdLoginAlteracao);
                    lImportar.ImportarCliente(lClienteNovo);
                    ok += lIdCliente + Environment.NewLine;
                    iok++;
                }
                catch (Exception ex)
                {
                    erro += lIdCliente + ": " + ex.Message + Environment.NewLine;
                    ierro++;
                }
            }

            string xerro = erro;
            string xok = ok;
            int iiok = iok;
            int iierro = ierro;
        }

        public void InativarLogins()
        {
            ////script oracle para rodar no SQL
            //select 'update tb_cliente set st_ativo = 0 where ds_cpfcnpj = "' || cpf || '" ;' 
            //from cliente,login 
            //where cliente.id_login = login.id_login and login.ativo = 'N'

        }



        #endregion

        #region Exportar Cliente

        private void ExportarCliente()
        {
            SalvarEntidadeCadastroRequest<SinacorExportarInfo> EntradaExportacao = new SalvarEntidadeCadastroRequest<SinacorExportarInfo>();
            SalvarEntidadeCadastroResponse RetornoExportacao = new SalvarEntidadeCadastroResponse();
            EntradaExportacao.EntidadeCadastro = new SinacorExportarInfo();
            EntradaExportacao.EntidadeCadastro.Entrada = new SinacorExportacaoEntradaInfo();
            EntradaExportacao.EntidadeCadastro.Entrada.IdCliente = 37179;
            EntradaExportacao.EntidadeCadastro.Entrada.PrimeiraExportacao = false;
            EntradaExportacao.EntidadeCadastro.Entrada.CdCodigo = 4213;

            RetornoExportacao = ServicoPersistenciaCadastro.SalvarEntidadeCadastro<SinacorExportarInfo>(EntradaExportacao);
            VerificaErro(RetornoExportacao);
        }

        #endregion

        #region Importar do Sinacor

        private void Importar1DoSinacor()
        {
            ReceberEntidadeCadastroResponse<SinacorClienteInfo> RetornoClienteSinacor = new ReceberEntidadeCadastroResponse<SinacorClienteInfo>();
            ReceberEntidadeCadastroRequest<SinacorClienteInfo> EntradaClienteSinacor = new ReceberEntidadeCadastroRequest<SinacorClienteInfo>();
            EntradaClienteSinacor.EntidadeCadastro = new SinacorClienteInfo();
            EntradaClienteSinacor.EntidadeCadastro.ChaveCliente = new SinacorChaveClienteInfo()
            {
                CD_CON_DEP = 1,
                DT_NASC_FUND = new DateTime(1965, 08, 24),
                CD_CPFCGC = 11137855860
            };
            //Pegando o cliente completo do Sinacor
            RetornoClienteSinacor = ServicoPersistenciaCadastro.ReceberEntidadeCadastro<SinacorClienteInfo>(EntradaClienteSinacor);

            SalvarEntidadeCadastroRequest<SinacorClienteInfo> EntradaCliente = new SalvarEntidadeCadastroRequest<SinacorClienteInfo>();
            SalvarEntidadeCadastroResponse RetornoCliente = new SalvarEntidadeCadastroResponse();
            EntradaCliente.EntidadeCadastro = RetornoClienteSinacor.EntidadeCadastro;

            //Salvando no Cadastro
            RetornoCliente = ServicoPersistenciaCadastro.SalvarEntidadeCadastro<SinacorClienteInfo>(EntradaCliente);

            VerificaErro(RetornoCliente);
        }

        private void ImportarTodosClientesSinacor()
        {
            //SinacorChaveClienteInfo lChave = new SinacorChaveClienteInfo();
            //EntidadeCadastro = lChave
            string ok = "";
            string erro = "";

            try
            {

                //Listar Todas as chaves
                #region "Listar Chaves do Sinacor"

                SinacorImportarDbLib lib = new SinacorImportarDbLib();

                ObjetosCompartilhados.lHashClientes = lib.ObterLoginClienteSistemaDuc();

                ConsultarEntidadeCadastroRequest<SinacorChaveClienteInfo> EntradaChaveSinacor = new ConsultarEntidadeCadastroRequest<SinacorChaveClienteInfo>();
                ConsultarEntidadeCadastroResponse<SinacorChaveClienteInfo> RetornoChaveSinacor = new ConsultarEntidadeCadastroResponse<SinacorChaveClienteInfo>();
                EntradaChaveSinacor.EntidadeCadastro = new SinacorChaveClienteInfo() { CD_CPFCGC = 33414115859, CD_CON_DEP = 1, DT_NASC_FUND = new DateTime(1985, 10, 2) };
                RetornoChaveSinacor = ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorChaveClienteInfo>(EntradaChaveSinacor);

                #endregion

                //Para cada chave, pegar o Cliente na estrutura do Sinacor
                //System.Collections.Generic.List<SinacorClienteInfo> lTodosClienteSinacorCompleto = new System.Collections.Generic.List<SinacorClienteInfo>();
                #region "Listar Clientes do Sinacor a partir das Chaves"
                ReceberEntidadeCadastroResponse<SinacorClienteInfo> RetornoClienteSinacor;
                ReceberEntidadeCadastroRequest<SinacorClienteInfo> EntradaClienteSinacor;
                SalvarEntidadeCadastroRequest<SinacorClienteInfo> EntradaCliente;
                SalvarEntidadeCadastroResponse RetornoCliente;

                foreach (SinacorChaveClienteInfo item in RetornoChaveSinacor.Resultado)
                {
                    string lKey = string.Format("{0}-{1}", item.CD_CPFCGC, item.DT_NASC_FUND.ToString("ddMMyyyy"));


                    RetornoClienteSinacor = new ReceberEntidadeCadastroResponse<SinacorClienteInfo>();
                    EntradaClienteSinacor = new ReceberEntidadeCadastroRequest<SinacorClienteInfo>();
                    EntradaClienteSinacor.EntidadeCadastro = new SinacorClienteInfo();
                    EntradaClienteSinacor.EntidadeCadastro.ChaveCliente = item;

                    RetornoClienteSinacor = ServicoPersistenciaCadastro.ReceberEntidadeCadastro<SinacorClienteInfo>(EntradaClienteSinacor);

                    //antes adidionava em um list, agora inclui
                    //lTodosClienteSinacorCompleto.Add(lClienteSinacor.EntidadeCadastro);
                    EntradaCliente = new SalvarEntidadeCadastroRequest<SinacorClienteInfo>();
                    RetornoCliente = new SalvarEntidadeCadastroResponse();
                    EntradaCliente.EntidadeCadastro = RetornoClienteSinacor.EntidadeCadastro;
                    EntradaCliente.EntidadeCadastro.StReimportacao = true;

                    try
                    {
                        RetornoCliente = ServicoPersistenciaCadastro.SalvarEntidadeCadastro<SinacorClienteInfo>(EntradaCliente);
                        VerificaErro(RetornoCliente);
                        ok += EntradaCliente.EntidadeCadastro.ChaveCliente.CD_CPFCGC + " - " + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        erro += EntradaCliente.EntidadeCadastro.ChaveCliente.CD_CPFCGC + " - " + ex.Message + " - " + Environment.NewLine;
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            string lOk = ok;
            string lErro = erro;

        }

        #endregion

        #region Excluir Cliente

        private void ExcluirClientes()
        {
            RemoverEntidadeCadastroRequest<ClienteInfo> EntradaCliente;
            RemoverEntidadeCadastroResponse RetornoCliente;

            for (int i = 51818; i <= 61816; i++)
            {
                EntradaCliente = new RemoverEntidadeCadastroRequest<ClienteInfo>();
                RetornoCliente = new RemoverEntidadeCadastroResponse();
                EntradaCliente.EntidadeCadastro = new ClienteInfo();
                EntradaCliente.EntidadeCadastro.IdCliente = i;

                RetornoCliente = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteInfo>(EntradaCliente);
            }
        }

        #endregion

        #region Importar Logins

        public void ImportarAdministradores()
        {
            string ok = "";
            string erro = "";
                    SalvarObjetoRequest<LoginInfo> lLogin;

            List<ELogin> Logins = new NLogin().Listar(NLogin.eTipo.Administrador);
            foreach (ELogin item in Logins)
            {
                try
                {
                    lLogin = new SalvarObjetoRequest<LoginInfo>();
                    lLogin.Objeto = new LoginInfo();
                    lLogin.Objeto.CdAssessor = null;
                    lLogin.Objeto.CdAssinaturaEletronica = item.Assinatura;
                    lLogin.Objeto.CdSenha = item.Senha;
                    lLogin.Objeto.DsEmail = item.Email.ToLower();
                    lLogin.Objeto.DsRespostaFrase = "";
                    lLogin.Objeto.DtUltimaExpiracao = DateTime.Now;
                    lLogin.Objeto.IdFrase = null;
                    lLogin.Objeto.NrTentativasErradas = 0;
                    lLogin.Objeto.DsNome = item.Nome;
                    lLogin.Objeto.TpAcesso = Intranet.Contratos.Dados.Enumeradores.eTipoAcesso.Cadastro;

                    ClienteDbLib.Salvar(lLogin, false);
                    ok += item.Email + " - " + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    erro += item.Email + " - " + ex.Message + " - " + Environment.NewLine;
                }
            }

            string xerro = erro;
            string xok = ok;
        }

        public void ImportarAssessores()
        {
            string ok = "";
            string erro = "";

            List<ELogin> Logins = new NLogin().Listar(NLogin.eTipo.Assessor);
                    SalvarObjetoRequest<LoginInfo> lLogin;
            foreach (ELogin item in Logins)
            {
                try
                {
                    lLogin = new SalvarObjetoRequest<LoginInfo>();
                    lLogin.Objeto = new LoginInfo();
                    lLogin.Objeto.CdAssessor = int.Parse(item.Nome);
                    lLogin.Objeto.CdAssinaturaEletronica = item.Assinatura;
                    lLogin.Objeto.CdSenha = item.Senha;
                    lLogin.Objeto.DsEmail = item.Email;
                    lLogin.Objeto.DsRespostaFrase = "";
                    lLogin.Objeto.DtUltimaExpiracao = DateTime.Now;
                    lLogin.Objeto.IdFrase = null;
                    lLogin.Objeto.NrTentativasErradas = 0;
                    //lLogin.Objeto.DsNome = item.Nome;
                    lLogin.Objeto.TpAcesso = Intranet.Contratos.Dados.Enumeradores.eTipoAcesso.Assessor;

                    ClienteDbLib.Salvar(lLogin, false);
                    ok += item.Email + " - " + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    erro += item.Email + " - " + ex.Message + " - " + Environment.NewLine;
                }
            }

            string xerro = erro;
            string xok = ok;
        }

        public void ImportarAtendimento()
        {
            string ok = "";
            string erro = "";

            List<ELogin> Logins = new NLogin().Listar(NLogin.eTipo.Atendimento);
            SalvarObjetoRequest<LoginInfo> lLogin;
            foreach (ELogin item in Logins)
            {
                try
                {
                    lLogin = new SalvarObjetoRequest<LoginInfo>();
                    lLogin.Objeto = new LoginInfo();
                    lLogin.Objeto.CdAssessor = null;
                    lLogin.Objeto.CdAssinaturaEletronica = item.Assinatura;
                    lLogin.Objeto.CdSenha = item.Senha;
                    lLogin.Objeto.DsEmail = item.Email;
                    lLogin.Objeto.DsRespostaFrase = "";
                    lLogin.Objeto.DtUltimaExpiracao = DateTime.Now;
                    lLogin.Objeto.IdFrase = null;
                    lLogin.Objeto.NrTentativasErradas = 0;
                    lLogin.Objeto.DsNome = item.Nome;
                    lLogin.Objeto.TpAcesso = Intranet.Contratos.Dados.Enumeradores.eTipoAcesso.Atendimento;

                    ClienteDbLib.Salvar(lLogin, false);
                    ok += item.Email + " - " + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    erro += item.Email + " - " + ex.Message + " - " + Environment.NewLine;
                }
            }

            string xerro = erro;
            string xok = ok;
        }

        public void ImportarTeleMarketing()
        {
            string ok = "";
            string erro = "";

            List<ELogin> Logins = new NLogin().Listar(NLogin.eTipo.Telemarketing);
            SalvarObjetoRequest<LoginInfo> lLogin;
            foreach (ELogin item in Logins)
            {
                try
                {
                    lLogin = new SalvarObjetoRequest<LoginInfo>();
                    lLogin.Objeto = new LoginInfo();
                    lLogin.Objeto.CdAssessor = null;
                    lLogin.Objeto.CdAssinaturaEletronica = item.Assinatura;
                    lLogin.Objeto.CdSenha = item.Senha;
                    lLogin.Objeto.DsEmail = item.Email;
                    lLogin.Objeto.DsRespostaFrase = "";
                    lLogin.Objeto.DtUltimaExpiracao = DateTime.Now;
                    lLogin.Objeto.IdFrase = null;
                    lLogin.Objeto.NrTentativasErradas = 0;
                    lLogin.Objeto.DsNome = item.Nome;
                    lLogin.Objeto.TpAcesso = Intranet.Contratos.Dados.Enumeradores.eTipoAcesso.TeleMarketing;

                    ClienteDbLib.Salvar(lLogin, false);
                    ok += item.Email + " - " + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    erro += item.Email + " - " + ex.Message + " - " + Environment.NewLine;
                }
            }

            string xerro = erro;
            string xok = ok;
        }

        #endregion

        #region Importar Suitability

        public void ImportarSuitability()
        {
            try
            {
                ConexaoDbHelper lAcessaDadosSuitability = new ConexaoDbHelper();
                lAcessaDadosSuitability.ConnectionStringName = "Suitability";

                ConexaoDbHelper lAcessaDadosCadastro = new ConexaoDbHelper();
                lAcessaDadosCadastro.ConnectionStringName = "Cadastro";

                using (DbCommand lDbCommand = lAcessaDadosSuitability.CreateCommand(CommandType.Text, "select * from Suitability"))
                {
                    DataTable dtSuitability = lAcessaDadosSuitability.ExecuteDbDataTable(lDbCommand);

                    string pCpf;
                    Int64? id_cliente;
                    string ds_perfil;
                    string ds_status;
                    DateTime dt_realizacao;
                    bool st_preenchidopelocliente;
                    string ds_loginrealizado;
                    string ds_fonte;
                    StringBuilder cmd;

                    foreach (DataRow item in dtSuitability.Rows)
                    {
                        pCpf = Convert.ToString(item["CPF"]);
                        ds_perfil = Convert.ToString(item["Profile"]);   //Perfil
                        ds_status = Convert.ToString(item["Status"]);//Finalizado;não agora
                        dt_realizacao = Convert.ToDateTime(item["DateLastAccess"]);//Data
                        st_preenchidopelocliente = Convert.ToBoolean(item["FillCustomer"]);//Feito pelo cliente
                        ds_loginrealizado = Convert.ToString(item["LastLogin"]);
                        ds_fonte = Convert.ToString(item["LastSource"]);//Origem

                        DbCommand lDbCommandCadastro = lAcessaDadosCadastro.CreateCommand(CommandType.Text, "select id_cliente from tb_cliente where convert(bigint,ds_cpfcnpj) = convert(bigint," + pCpf + ") ");
                        id_cliente = ConvertInt(lAcessaDadosCadastro.ExecuteScalar(lDbCommandCadastro));

                        if (null != id_cliente)
                        {
                            cmd = new StringBuilder();

                            cmd.Append(" INSERT INTO tb_cliente_suitability");
                            cmd.Append("  ([id_cliente]");
                            cmd.Append("   ,[ds_perfil]");
                            cmd.Append("   ,[ds_status]");
                            cmd.Append("   ,[dt_realizacao]");
                            cmd.Append("   ,[st_preenchidopelocliente]");
                            cmd.Append("   ,[ds_loginrealizado]");
                            cmd.Append("   ,[ds_fonte]");
                            cmd.Append("   ,[ds_respostas])");
                            cmd.Append("    VALUES ( ");
                            cmd.Append(id_cliente);
                            cmd.Append("   ,'" + ds_perfil + "'");
                            cmd.Append("   ,'" + ds_status + "'");
                            cmd.Append("   ,'" + dt_realizacao.ToString("yyyy-MM-d HH:mm:ss") + "'");
                            cmd.Append("   ,'" + st_preenchidopelocliente.ToString() + "'");
                            cmd.Append("   ,'" + ds_loginrealizado.Replace("'", "").Replace("?", "") + "'");
                            cmd.Append("   ,'" + ds_fonte + "'");
                            cmd.Append("   ,null)");


                            lDbCommandCadastro = lAcessaDadosCadastro.CreateCommand(CommandType.Text, cmd.ToString());
                            lAcessaDadosCadastro.ExecuteNonQuery(lDbCommandCadastro);
                        }
                    }
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        #endregion

        #region Tratamanto de Excessão

        private const char ERRONEGOCIO = '|';


        public void VerificaErro(MensagemResponseStatusEnum pStatus, String pMensagem)
        {
            if (pStatus != MensagemResponseStatusEnum.OK)
            {
                string[] x = pMensagem.Split(ERRONEGOCIO);
                if (x.Length > 2)
                    throw new Exception(x[1]);
                else
                    throw new Exception(pMensagem);
            }
        }

        public void VerificaErro(SalvarEntidadeCadastroResponse pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<PrimeiroAcessoValidaCpfInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<PrimeiroAcessoValidaInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<VerificaNomeInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<EsqueciSenhaInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<EsqueciAssinaturaEletronicaInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<ClienteEducacionalInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<ClienteSituacaoFinanceiraPatrimonialInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<ClienteEmitenteInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<ClienteDiretorInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<SessaoPortalInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<ClienteSuitabilityInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }


        public void VerificaErro(ReceberEntidadeCadastroResponse<LoginInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<ClienteProcuradorRepresentanteInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<ArquivoContratoInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<ArquivoContratoInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<TipoTelefoneInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<MensagemAjudaInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<TipoTelefoneInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<ContratoInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<SinacorListaComboInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(RemoverEntidadeCadastroResponse pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<SinacorListaInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<ClienteTelefoneInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<ClienteInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }


        public void VerificaErro(ConsultarEntidadeCadastroResponse<TipoEnderecoInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<EfetuarLoginInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ReceberEntidadeCadastroResponse<TipoEnderecoInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }

        public void VerificaErro(ConsultarEntidadeCadastroResponse<ClienteBancoInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }
        public void VerificaErro(ConsultarEntidadeCadastroResponse<ClienteEnderecoInfo> pObjeto)
        {
            VerificaErro(pObjeto.StatusResposta, pObjeto.DescricaoResposta);
        }
        #endregion

        #region Aux

        public Int64? ConvertInt(object valor)
        {
            Int64 retorno = default(Int64);

            if (valor != null && Int64.TryParse(valor.ToString(), out retorno))
                return retorno;
            else
                return null;
        }

        #endregion
    }
}
