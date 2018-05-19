using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Dados.ControleDeOrdens;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Negocio;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using Gradual.Generico.Dados;
using log4net;
namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region | Propriedades Conexao


        public static string gNomeConexaoCadastro
        {
            get { return "Cadastro"; }
        }

        public static string gNomeFilialAssessor
        {
            get { return "Filial"; }
        }

        public static string gNomeConexaoRendaFixa
        {
            get { return "RendaFixa"; }
        }

        public static string gNomeConexaoSinacor
        {
            get { return "SinacorConsulta"; }
        }

        public static string gNomeConexaoSinacorConsulta
        {
            get { return "SinacorConsulta"; }
        }

        public static string gNomeComexaoOMS
        {
            get { return "OMS"; }
        }

        public static string gNomeConexaoGradualOMS
        {
            get { return "GradualOMS"; }
        }

        public static string gNomeConexaoRisco
        {
            get { return "RISCO"; }
        }

        public static string gNomeConexaoRiscoNovoOMS
        {
            get { return "RISCO_GRADUALOMS"; }
        }

        public static string gNomeConexaoRiscoSpider
        {
            get { return "GradualSpider"; }
        }

        public static string gNomeConexaoSinacorTrade
        {
            get { return "SINACOR"; }
        }

        public static string gNomeConexaoClubesSisfinance
        {
            get { return "Clubes"; }
        }

        public static string gNomeConexaoFundos
        {
            get { return "PlataformaInviXX"; }
        }
        #endregion

        #region | Atributos

        private const string gCpfCnpjInvalido = "CPF/CNPJ Inválido";
        private const string gCpfCnpjJaCadastrado = "CPF/CNPJ já cadastrado para outro Cliente";
        private const string gEmailJaCadastrado = "E-mail já cadastrado para outro cliente";

        #endregion

        #region | ClienteResumidoInfo

        public static ConsultarObjetosResponse<ClienteResumidoInfo> ConsultarClienteResumido_ViaAtributosDoCliente(ConsultarEntidadeRequest<ClienteResumidoInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteResumidoInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_resumido_sel_sp"))
                {
                    MontarParametrosConsultaClienteResumido(ref lAcessaDados, lDbCommand, pParametros.Objeto);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroClienteResumido(lDataTable.Rows[i], false));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ConsultarObjetosResponse<ClienteResumidoMigracaoInfo> ConsultarClienteResumidoMigracao_ViaIdDoAssessor(ConsultarEntidadeRequest<ClienteResumidoMigracaoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteResumidoMigracaoInfo> lResposta = new ConsultarObjetosResponse<ClienteResumidoMigracaoInfo>();
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
                int lTotalRegistros = 0;

                //using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_assessor_lst_sp"))
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_assessor_paginada_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@pid_assessor", DbType.Int32, pParametros.Objeto.TermoDeBusca.DBToInt32());
                    lAcessaDados.AddInParameter(lDbCommand, "@pPaginaCorrente", DbType.Int32, pParametros.Objeto.PaginaCorrente);

                    System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter();
                    sqlParameter.Direction = ParameterDirection.Output;
                    sqlParameter.DbType = DbType.Int32;
                    sqlParameter.ParameterName = "@pTotalRegistros";

                    lDbCommand.Parameters.Add(sqlParameter);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lTotalRegistros = Convert.ToInt32(lDbCommand.Parameters["@pTotalRegistros"].Value.ToString()) ;
                    }

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            lResposta.Resultado.Add(CriarRegistroClienteResumidoMigracao(lDataTable.Rows[i], true, lTotalRegistros));
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ConsultarObjetosResponse<ClienteResumidoInfo> ConsultarClienteResumido_ViaIdDoAssessor(ConsultarEntidadeRequest<ClienteResumidoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteResumidoInfo> lResposta = new ConsultarObjetosResponse<ClienteResumidoInfo>();
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_assessor_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, pParametros.Objeto.TermoDeBusca.DBToInt32());

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            lResposta.Resultado.Add(CriarRegistroClienteResumido(lDataTable.Rows[i], true));
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ConsultarObjetosResponse<ClienteResumidoInfo> ConsultarClienteResumido_DadosBasicos(ConsultarEntidadeRequest<ClienteResumidoInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteResumidoInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_resumido_dados_basicos_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa", DbType.String, OpcoesBuscarPor.CodBovespa.Equals(pParametros.Objeto.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pParametros.Objeto.TermoDeBusca) ? (pParametros.Objeto.TermoDeBusca.Trim().Length > 8) ? (object)pParametros.Objeto.TermoDeBusca.Trim().Substring(0, 8) : (object)pParametros.Objeto.TermoDeBusca.Trim() : System.DBNull.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, OpcoesBuscarPor.CpfCnpj.Equals(pParametros.Objeto.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pParametros.Objeto.TermoDeBusca) ? (object)pParametros.Objeto.TermoDeBusca.Trim().Replace(".", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty) : System.DBNull.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, OpcoesBuscarPor.NomeCliente.Equals(pParametros.Objeto.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pParametros.Objeto.TermoDeBusca) ? (object)pParametros.Objeto.TermoDeBusca.Trim() : System.DBNull.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, OpcoesBuscarPor.Email.Equals(pParametros.Objeto.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pParametros.Objeto.TermoDeBusca) ? (object)pParametros.Objeto.TermoDeBusca.Trim() : System.DBNull.Value);


                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroClienteResumidoDadosBasicos(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ConsultarObjetosResponse<ClienteResumidoInfo> ConsultarClienteResumido_DadosBasicosPorAssessor(ConsultarEntidadeRequest<ClienteResumidoInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteResumidoInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_resumido_dados_basicos_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa", DbType.String, OpcoesBuscarPor.CodBovespa.Equals(pParametros.Objeto.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pParametros.Objeto.TermoDeBusca) ? (pParametros.Objeto.TermoDeBusca.Trim().Length > 8) ? (object)pParametros.Objeto.TermoDeBusca.Trim().Substring(0, 8) : (object)pParametros.Objeto.TermoDeBusca.Trim() : System.DBNull.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, OpcoesBuscarPor.CpfCnpj.Equals(pParametros.Objeto.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pParametros.Objeto.TermoDeBusca) ? (object)pParametros.Objeto.TermoDeBusca.Trim().Replace(".", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty) : System.DBNull.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, OpcoesBuscarPor.NomeCliente.Equals(pParametros.Objeto.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pParametros.Objeto.TermoDeBusca) ? (object)pParametros.Objeto.TermoDeBusca.Trim() : System.DBNull.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, OpcoesBuscarPor.Email.Equals(pParametros.Objeto.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pParametros.Objeto.TermoDeBusca) ? (object)pParametros.Objeto.TermoDeBusca.Trim() : System.DBNull.Value);


                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroClienteResumidoDadosBasicos(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        #endregion

        #region | Portal

        public static ConsultarObjetosResponse<ClienteEducacionalInfo> ConsultarClienteEducacional(ConsultarEntidadeRequest<ClienteEducacionalInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteEducacionalInfo> resposta = new ConsultarObjetosResponse<ClienteEducacionalInfo>();
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_educacional_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);


                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(Converter(CriarRegistroCliente(lDataTable.Rows[i])));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<SessaoPortalInfo> ReceberSessaoPortal(ReceberEntidadeRequest<SessaoPortalInfo> pParametros)
        {
            try
            {

                ReceberObjetoResponse<SessaoPortalInfo> lRetorno = new ReceberObjetoResponse<SessaoPortalInfo>();
                lRetorno.Objeto = pParametros.Objeto;

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_sessaoportal_sel_2_sp"))
                {
                    if (pParametros.Objeto.IdCliente != 0)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@Id_Cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    }

                    if (pParametros.Objeto.IdLogin != 0)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@Id_Loginp", DbType.Int32, pParametros.Objeto.IdLogin);
                    }
                    
                    lAcessaDados.AddOutParameter(lDbCommand, "@Id_Login", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Id_ClienteRetorno", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Ds_Nome", DbType.String, 80);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Ds_EmailRetorno", DbType.String, 80);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Ds_CpfCnpj", DbType.String, 15);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Cd_CodigoPrincipal", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Cd_AssessorPrincipal", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@St_Passo", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Dt_NascimentoFundacao", DbType.Date, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Tp_Pessoa", DbType.String, 1);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Dt_Passo1", DbType.Date, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Dt_UltimoLogin", DbType.Date, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@CodigoTipoOperacaocliente", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.Objeto.IdLogin                     = lDbCommand.Parameters["@Id_Login"].Value.DBToInt32();
                    lRetorno.Objeto.IdCliente                   = lDbCommand.Parameters["@Id_ClienteRetorno"].Value.DBToInt32();
                    lRetorno.Objeto.DsNome                      = lDbCommand.Parameters["@Ds_Nome"].Value.DBToString();
                    lRetorno.Objeto.DsEmailRetorno              = lDbCommand.Parameters["@Ds_EmailRetorno"].Value.DBToString();
                    lRetorno.Objeto.DsCpfCnpj                   = lDbCommand.Parameters["@Ds_CpfCnpj"].Value.DBToString();
                    lRetorno.Objeto.CdCodigoPrincipal           = lDbCommand.Parameters["@Cd_CodigoPrincipal"].Value.DBToInt32();
                    lRetorno.Objeto.CdAssessorPrincipal         = lDbCommand.Parameters["@Cd_AssessorPrincipal"].Value.DBToInt32();
                    lRetorno.Objeto.StPasso                     = lDbCommand.Parameters["@St_Passo"].Value.DBToInt32();
                    lRetorno.Objeto.DtNascimentoFundacao        = lDbCommand.Parameters["@Dt_NascimentoFundacao"].Value.DBToDateTime();
                    lRetorno.Objeto.TpPessoa                    = lDbCommand.Parameters["@Tp_Pessoa"].Value.DBToString();
                    lRetorno.Objeto.DtPasso1                    = lDbCommand.Parameters["@Dt_Passo1"].Value.DBToDateTime();
                    lRetorno.Objeto.DtUltimoLogin               = lDbCommand.Parameters["@Dt_UltimoLogin"].Value.DBToDateTime();
                    lRetorno.Objeto.CodigoTipoOperacaoCliente   = lDbCommand.Parameters["@CodigoTipoOperacaocliente"].Value.DBToInt32();

                }

                lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_novo_hb"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente", DbType.Int32, lRetorno.Objeto.CdCodigoPrincipal);
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                    
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRetorno.Objeto.NovoHB = true;
                        if (lDataTable.Rows[0]["AlterarAssinatura"].Equals(true))
                        {
                            lRetorno.Objeto.AlterarAssinatura = true;
                        }
                        else
                        {
                            lRetorno.Objeto.AlterarAssinatura = false;
                        }
                    }
                    else
                    {
                        lRetorno.Objeto.NovoHB = false;
                    }

                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static SalvarEntidadeResponse<Passo1Info> SalvarPasso1(SalvarObjetoRequest<Passo1Info> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                SalvarEntidadeResponse<Passo1Info> lRetorno = new SalvarEntidadeResponse<Passo1Info>();

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_passo1_ins_sp"))
                {
                    pParametros.Objeto.DsCpfCnpj = LimparCpfCnpj(pParametros.Objeto.DsCpfCnpj);

                    if (pParametros.Objeto.DsCpfCnpj.Length < 11)
                        pParametros.Objeto.DsCpfCnpj = pParametros.Objeto.DsCpfCnpj.PadLeft(11, '0');

                    ValidarCpfCnpj(pParametros.Objeto.DsCpfCnpj);

                    ClienteInfo lClienteCpfEmail = new ClienteInfo();
                    lClienteCpfEmail.IdCliente = 0;
                    lClienteCpfEmail.DsCpfCnpj = pParametros.Objeto.DsCpfCnpj;
                    ValidaCpfCnpJaExiste(lClienteCpfEmail);

                    lClienteCpfEmail.DsEmail = pParametros.Objeto.DsEmail;
                    ValidaEmailJaExisteCliente(lClienteCpfEmail);


                    //Login
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@Cd_Senha", DbType.String, Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenha));
                    lAcessaDados.AddInParameter(lDbCommand, "@Cd_AssinaturaEletronica", DbType.String, Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaEletronica));

                    //Cliente
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_CpfCnpj", DbType.String, LimparCpfCnpj(pParametros.Objeto.DsCpfCnpj));
                    lAcessaDados.AddInParameter(lDbCommand, "@Dt_NascimentoFundacao", DbType.Date, pParametros.Objeto.DtNascimentoFundacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_Nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@Id_AssessorInicial", DbType.Int32, pParametros.Objeto.IdAssessorInicial);
                    lAcessaDados.AddInParameter(lDbCommand, "@Cd_Sexo", DbType.String, pParametros.Objeto.CdSexo);
                    lAcessaDados.AddInParameter(lDbCommand, "@Tp_Pessoa", DbType.String, pParametros.Objeto.TpPessoa);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_Comoconheceu", DbType.String, pParametros.Objeto.ComoConheceu);
                    lAcessaDados.AddInParameter(lDbCommand, "@Tp_Cliente", DbType.Int32, pParametros.Objeto.TpCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoTipoOperacaoCliente", DbType.Int32, pParametros.Objeto.CodigoTipoOperacaoCliente);

                    //Celular
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_CelNumero", DbType.String, pParametros.Objeto.DsCelNumero);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_CelDdd", DbType.String, pParametros.Objeto.DsCelDdd);

                    
                    //Telefone
                    lAcessaDados.AddInParameter(lDbCommand, "@Id_Tipo_Telefone", DbType.Int32, pParametros.Objeto.IdTipoTelefone);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_Numero", DbType.String, pParametros.Objeto.DsNumero);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_Ddd", DbType.String, pParametros.Objeto.DsDdd);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_Ramal", DbType.String, pParametros.Objeto.DsRamal);
                    

                    ////public int StPrincipal { get { return 1; }}


                    lAcessaDados.AddOutParameter(lDbCommand, "@Id_Cliente", DbType.Int32, 8);


                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.Codigo = lDbCommand.Parameters["@Id_Cliente"].Value.DBToInt32();
                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);
                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static ConsultarObjetosResponse<ClienteResumidoInfo> ConsultarClienteResumido(ConsultarEntidadeRequest<ClienteResumidoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteResumidoInfo> lResposta = null;

                switch (pParametros.Objeto.TipoDeConsulta)
                {
                    case TipoDeConsultaClienteResumidoInfo.Clientes:
                        lResposta = ConsultarClienteResumido_ViaAtributosDoCliente(pParametros);
                        break;
                    case TipoDeConsultaClienteResumidoInfo.ClientesPorAssessor:
                        lResposta = ConsultarClienteResumido_ViaIdDoAssessor(pParametros);
                        break;
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ConsultarObjetosResponse<ClientePassoContaInfo> ConsultarPassoEContasDoCliente(ConsultarEntidadeRequest<ClientePassoContaInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClientePassoContaInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_passo_conta_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroClientePassoConta(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ConsultarObjetosResponse<ClienteContaConsultaInfo> ConsultarCodigoCliente(ConsultarEntidadeRequest<ClienteContaConsultaInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteContaConsultaInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                resposta.Resultado = new List<ClienteContaConsultaInfo>();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_conta_sel_parametrizado_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_sistema", DbType.String, "bol");
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int64, pParametros.Objeto.IdLogin);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        resposta.Resultado.Add(new ClienteContaConsultaInfo() { CdCodigo = lDataTable.Rows[0]["cd_codigo"].DBToInt32() });
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        #endregion

        #region | ClienteInfo

        /// <summary>
        /// Exclui o cliente e todas as suas dependências. Ex. Telefone, endereço, etc...
        /// </summary>
        /// <param name="pCliente">Precisa apenas do idCLiente preenchido.</param>
        /// <DataCriacao>14/05/2010</DataCriacao>
        /// <Autor>Gustavo Malta Guimarães</Autor>
        /// <Alteracao>
        ///     <DataAlteracao></DataAlteracao>
        ///     <Autor></Autor>
        ///     <Motivo></Motivo>
        /// </Alteracao>
        public static RemoverObjetoResponse<ClienteInfo> ExcluirComDependencias(RemoverEntidadeRequest<ClienteInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "clientecompleto_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClienteInfo> response = new RemoverEntidadeResponse<ClienteInfo>()
                {
                    lStatus = true
                };
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir);

                return response;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<ClienteInfo> Atualizar(SalvarObjetoRequest<ClienteInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteInfo> lRetorno;

            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            try
            {
                lRetorno = Atualizar(trans, pParametros);
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
            return lRetorno;
        }

        private static SalvarEntidadeResponse<ClienteInfo> Atualizar(DbTransaction pTrans, SalvarObjetoRequest<ClienteInfo> pParametros, Boolean pSalvarLogin = true)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                if (pSalvarLogin)
                {
                    if (0.Equals(pParametros.Objeto.IdLogin) && !string.IsNullOrEmpty(pParametros.Objeto.DsEmail.Trim()))
                    {
                        var lLoginInfo = new SalvarObjetoRequest<LoginInfo>()
                        {
                            Objeto = new LoginInfo()
                            {
                                DtUltimaExpiracao = DateTime.Now,
                                NrTentativasErradas = default(int),
                                DsEmail = pParametros.Objeto.DsEmail,
                            },
                            DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado,
                            IdUsuarioLogado = pParametros.IdUsuarioLogado
                        };

                        var lIncluirrLoginInfo = Salvar(pTrans, lLoginInfo);

                        pParametros.Objeto.IdLogin = lIncluirrLoginInfo.Codigo;
                    }
                    else
                    {   //--> Salvando as alterações de e-mail
                        var lLoginInfo = new SalvarObjetoRequest<LoginInfo>()
                        {
                            Objeto = new LoginInfo()
                            {
                                IdLogin = pParametros.Objeto.IdLogin,
                                DsEmail = pParametros.Objeto.DsEmail,
                            },
                            DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado,
                            IdUsuarioLogado = pParametros.IdUsuarioLogado
                        };

                        var lAtualizarLoginInfo = AtualizarEmail(pTrans, lLoginInfo);
                    }
                }

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ultimaatualizacao", DbType.DateTime, pParametros.Objeto.DtUltimaAtualizacao == DateTime.MinValue ? (object)System.DBNull.Value : pParametros.Objeto.DtUltimaAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, LimparCpfCnpj(pParametros.Objeto.DsCpfCnpj));
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_passo1", DbType.DateTime, pParametros.Objeto.DtPasso1 == DateTime.MinValue ? (object)System.DBNull.Value : pParametros.Objeto.DtPasso1);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_passo2", DbType.DateTime, (pParametros.Objeto.DtPasso2 == null || pParametros.Objeto.DtPasso2 == DateTime.MinValue) ? (object)System.DBNull.Value : pParametros.Objeto.DtPasso2);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_passo3", DbType.DateTime, (pParametros.Objeto.DtPasso3 == null || pParametros.Objeto.DtPasso3 == DateTime.MinValue) ? (object)System.DBNull.Value : pParametros.Objeto.DtPasso3);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_primeiraexportacao", DbType.DateTime, (pParametros.Objeto.DtPrimeiraExportacao == null || pParametros.Objeto.DtPrimeiraExportacao == DateTime.MinValue) ? (object)System.DBNull.Value : pParametros.Objeto.DtPrimeiraExportacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ultimaexportacao", DbType.DateTime, (pParametros.Objeto.DtUltimaExportacao == null || pParametros.Objeto.DtUltimaExportacao == DateTime.MinValue) ? (object)System.DBNull.Value : pParametros.Objeto.DtUltimaExportacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_origemcadastro", DbType.String, pParametros.Objeto.DsOrigemCadastro);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_pessoa", DbType.String, pParametros.Objeto.TpPessoa);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_cliente", DbType.Int32, pParametros.Objeto.TpCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_passo", DbType.Int32, pParametros.Objeto.StPasso);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_sexo", DbType.String, pParametros.Objeto.CdSexo);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nacionalidade", DbType.Int32, pParametros.Objeto.CdNacionalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_paisnascimento", DbType.AnsiString, pParametros.Objeto.CdPaisNascimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ufnascimento", DbType.AnsiString, pParametros.Objeto.CdUfNascimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_ufnascimentoestrangeuro", DbType.String, pParametros.Objeto.DsUfnascimentoEstrangeiro);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_estadocivil", DbType.Int32, pParametros.Objeto.CdEstadoCivil);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_conjugue", DbType.String, pParametros.Objeto.DsConjugue);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_documento", DbType.String, pParametros.Objeto.TpDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimentofundacao", DbType.DateTime, pParametros.Objeto.DtNascimentoFundacao == DateTime.MinValue ? (object)System.DBNull.Value : pParametros.Objeto.DtNascimentoFundacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_orgaoemissordocumento", DbType.String, pParametros.Objeto.CdOrgaoEmissorDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ufemissaodocumento", DbType.String, pParametros.Objeto.CdUfEmissaoDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_profissaoatividade", DbType.Int32, pParametros.Objeto.CdProfissaoAtividade);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cargo", DbType.String, pParametros.Objeto.DsCargo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_empresa", DbType.String, pParametros.Objeto.DsEmpresa);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ppe", DbType.Boolean, pParametros.Objeto.StPPE);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_carteirapropria", DbType.Boolean, pParametros.Objeto.StCarteiraPropria);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_autorizadooperar", DbType.String, pParametros.Objeto.DsAutorizadoOperar);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_cvm387", DbType.Boolean, pParametros.Objeto.StCVM387);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_emancipado", DbType.Boolean, pParametros.Objeto.StEmancipado);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_cadastroportal", DbType.Boolean, pParametros.Objeto.StCadastroPortal);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_escolaridade", DbType.Int32, pParametros.Objeto.CdEscolaridade);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nomefantasia", DbType.String, pParametros.Objeto.DsNomeFantasia);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nire", DbType.Int64, pParametros.Objeto.CdNire);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_formaconstituicao", DbType.String, pParametros.Objeto.DsFormaConstituicao);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_interdito", DbType.Boolean, pParametros.Objeto.StInterdito);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_situacaolegaloutros", DbType.Boolean, pParametros.Objeto.StSituacaoLegalOutros);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_NomePai", DbType.String, pParametros.Objeto.DsNomePai);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_NomeMae", DbType.String, pParametros.Objeto.DsNomeMae);
                    lAcessaDados.AddInParameter(lDbCommand, "@Dt_EmissaoDocumento", DbType.Date, pParametros.Objeto.DtEmissaoDocumento == null || pParametros.Objeto.DtEmissaoDocumento.Equals(DateTime.MinValue) ? System.DBNull.Value : (object)pParametros.Objeto.DtEmissaoDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ds_Naturalidade", DbType.String, pParametros.Objeto.DsNaturalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_atividadePrincipal", DbType.String, pParametros.Objeto.CdAtividadePrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@Nr_InscricaoEstadual", DbType.String, pParametros.Objeto.NrInscricaoEstadual);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_pessoavinculada", DbType.Int32, pParametros.Objeto.StPessoaVinculada);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numerodocumento", DbType.String, pParametros.Objeto.DsNumeroDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_emailcomercial", DbType.String, pParametros.Objeto.DsEmailComercial);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_comoconheceu", DbType.String, pParametros.Objeto.DsComoConheceu);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_deseja_aplicar", DbType.String, pParametros.Objeto.TpDesejaAplicar);
                    
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_proposito_gradual", DbType.String, pParametros.Objeto.DsPropositoGradual);

                    lAcessaDados.AddInParameter(lDbCommand, "@st_usperson", DbType.String, pParametros.Objeto.StUSPerson);

                    if (!string.IsNullOrEmpty(pParametros.Objeto.DsUSPersonPJDetalhes))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@ds_usperson_pj_detalhes", DbType.String, pParametros.Objeto.DsUSPersonPJDetalhes);
                    }

                    if (pParametros.Objeto.StCienteDocumentos.HasValue)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@st_ciente_documentos", DbType.Int16, pParametros.Objeto.StCienteDocumentos.Value);
                    }
                    else
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@st_ciente_documentos", DbType.Int16, DBNull.Value);
                    }

                    if (pParametros.Objeto.IdAssessorInicial != null)
                        lAcessaDados.AddInParameter(lDbCommand, "@id_assessorinicial", DbType.String, pParametros.Objeto.IdAssessorInicial);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    if (null != pParametros.Objeto.DadosClienteNaoOperaPorContaPropria)
                    {   //--> Definindo os dados de cliente nao opera por conta propria
                        pParametros.Objeto.DadosClienteNaoOperaPorContaPropria.IdCliente = pParametros.Objeto.IdCliente.Value;
                        SalvarClienteNaoOperaPorContaPropria(pParametros.Objeto, pTrans);
                    }

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                    return new SalvarEntidadeResponse<ClienteInfo>() { Objeto = pParametros.Objeto };
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ClienteDesejaAplicarInfo> AtualizarDesejaAplicar(SalvarObjetoRequest<ClienteDesejaAplicarInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteDesejaAplicarInfo> lRetorno = new SalvarEntidadeResponse<ClienteDesejaAplicarInfo>();

            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_deseja_aplicar_upd_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@tp_deseja_aplicar", DbType.String, pParametros.Objeto.TpDesejaAplicar);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return lRetorno;
        }

        public static ConsultarObjetosResponse<ClienteInfo> ConsultarCliente(ConsultarEntidadeRequest<ClienteInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_lst_sp"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroCliente(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ConsultarObjetosResponse<ClienteInfo> ConsultarClientePorAssessor(ConsultarEntidadeRequest<ClienteInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_assessor_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, pParametros.Objeto.IdAssessorInicial);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable) foreach (DataRow linha in lDataTable.Rows)
                            resposta.Resultado.Add(CriarRegistroCliente(linha));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ClienteInfo> ReceberCliente(ReceberEntidadeRequest<ClienteInfo> pParametros)
        {
            try
            {
                gLogger.Info("Receber Cliente");
                
                var resposta = new ReceberObjetoResponse<ClienteInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                resposta.Objeto = new ClienteInfo();
                gLogger.Info("Cria Conexao");
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
                
                //atualizado para, caso tenha somente id_login, use outra proc para buscar os dados do cliente

                string lProc = "cliente_sel_sp";
                string lParam = "@id_cliente";
                int lValue;

                if (!pParametros.Objeto.IdCliente.HasValue && pParametros.Objeto.IdLogin.HasValue)
                {
                    gLogger.Info("Chama por Login");
                    lProc = "cliente_sel_login_sp";
                    lParam = "@id_login";
                    lValue = pParametros.Objeto.IdLogin.Value;
                }
                else
                {
                    gLogger.Info("Chama por ID");
                    lValue = pParametros.Objeto.IdCliente.Value;
                }

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, lProc))
                {
                    lAcessaDados.AddInParameter(lDbCommand, lParam, DbType.Int32, lValue);
                    gLogger.Info("Executar");
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto = CriarRegistroCliente(lDataTable.Rows[0]);

                        pParametros.Objeto.IdCliente = resposta.Objeto.IdCliente;
                    }
                }

                if (resposta.Objeto.StCarteiraPropria != null && !resposta.Objeto.StCarteiraPropria.Value)
                {
                    var lCarteiraPropria = ConsultarClienteNaoOperaPorContaPropria(new ReceberEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo>() { Objeto = new ClienteNaoOperaPorContaPropriaInfo() { IdCliente = pParametros.Objeto.IdCliente.Value } });
                    resposta.Objeto.DadosClienteNaoOperaPorContaPropria.DsCpfCnpjClienteRepresentado = lCarteiraPropria.Objeto.DsCpfCnpjClienteRepresentado;
                    resposta.Objeto.DadosClienteNaoOperaPorContaPropria.DsNomeClienteRepresentado = lCarteiraPropria.Objeto.DsNomeClienteRepresentado;
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        public static RemoverObjetoResponse<ClienteInfo> RemoverCliente(RemoverEntidadeRequest<ClienteInfo> pParametros)
        {
            //Alterado por Gustavo em 19/05/2010
            //para excluir primeiro as dependências
            return ExcluirComDependencias(pParametros);

        }

        public static void SelecionaClientePorDataNacCpfCnpj(RemoverEntidadeRequest<ClienteInfo> pParametros)
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand  = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_login_id_sel_cpfcnpj_dtnasc"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, LimparCpfCnpj(pParametros.Objeto.DsCpfCnpj));
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimentofundacao", DbType.DateTime, pParametros.Objeto.DtNascimentoFundacao == DateTime.MinValue ? (object)System.DBNull.Value : pParametros.Objeto.DtNascimentoFundacao);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        pParametros.Objeto.IdCliente = lDataTable.Rows[0]["id_cliente"].DBToInt32();
                        pParametros.Objeto.IdLogin   = lDataTable.Rows[0]["id_login"].DBToInt32();
                    }
                }
        }

        public static RemoverObjetoResponse<ClienteInfo> RemoverCliente(DbTransaction pTrans, RemoverEntidadeRequest<ClienteInfo> pParametros)
        {
            var lRetorno = new RemoverObjetoResponse<ClienteInfo>();

            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, LimparCpfCnpj(pParametros.Objeto.DsCpfCnpj));
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimentofundacao", DbType.DateTime, pParametros.Objeto.DtNascimentoFundacao == DateTime.MinValue ? (object)System.DBNull.Value : pParametros.Objeto.DtNascimentoFundacao);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir);
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }

            return lRetorno;
        }

        public static SalvarEntidadeResponse<ClienteInfo> SalvarCliente(SalvarObjetoRequest<ClienteInfo> pParametros)
        {
            ValidarCpfCnpj(pParametros.Objeto.DsCpfCnpj);
            ValidaCpfCnpJaExiste(pParametros.Objeto);
            ValidaEmailJaExisteCliente(pParametros.Objeto);
            if (pParametros.Objeto.IdCliente > 0)
                return Atualizar(pParametros);
            else
                return Salvar(pParametros);
        }

        public static SalvarEntidadeResponse<ClienteInfo> SalvarCliente(DbTransaction pTrans, SalvarObjetoRequest<ClienteInfo> pParametros, Boolean pSalvarLogin = true, Boolean pStReimportacao = false)
        {
            ValidarCpfCnpj(pParametros.Objeto.DsCpfCnpj);
            if (!pStReimportacao) ValidaCpfCnpJaExiste(pParametros.Objeto);
            return Salvar(pTrans, pParametros, pSalvarLogin);
        }

        private static SalvarEntidadeResponse<ClienteInfo> Salvar(DbTransaction pTrans, SalvarObjetoRequest<ClienteInfo> pParametros, Boolean pSalvarLogin = true)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                if (pSalvarLogin)
                {   //--> Salvando o registro de e-mail.
                    string lSenha = pParametros.Objeto.DsSenhaGerada;

                    var lLoginInfo = new SalvarObjetoRequest<LoginInfo>()
                    {
                        Objeto = new LoginInfo()
                        {
                            CdSenha = lSenha,
                            CdAssinaturaEletronica = lSenha,
                            NrTentativasErradas = 0,
                            DtUltimaExpiracao = DateTime.Now,
                            DsEmail = pParametros.Objeto.DsEmail,
                        },
                        IdUsuarioLogado = pParametros.IdUsuarioLogado,
                        DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado
                    };

                    var lSalvarLoginInfo = Salvar(pTrans, lLoginInfo);

                    pParametros.Objeto.IdLogin = lSalvarLoginInfo.Codigo;
                    pParametros.Objeto.CdSenha = lSalvarLoginInfo.Objeto.CdSenha;
                }

                using (var lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ultimaatualizacao", DbType.DateTime, pParametros.Objeto.DtUltimaAtualizacao == DateTime.MinValue ? (object)System.DBNull.Value : pParametros.Objeto.DtUltimaAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, LimparCpfCnpj(pParametros.Objeto.DsCpfCnpj));
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_passo1", DbType.DateTime, pParametros.Objeto.DtPasso1 == DateTime.MinValue ? (object)System.DBNull.Value : pParametros.Objeto.DtPasso1);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_passo2", DbType.DateTime, (pParametros.Objeto.DtPasso2 == null || pParametros.Objeto.DtPasso2 == DateTime.MinValue) ? (object)System.DBNull.Value : pParametros.Objeto.DtPasso2);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_passo3", DbType.DateTime, (pParametros.Objeto.DtPasso3 == null || pParametros.Objeto.DtPasso3 == DateTime.MinValue) ? (object)System.DBNull.Value : pParametros.Objeto.DtPasso3);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_primeiraexportacao", DbType.DateTime, (pParametros.Objeto.DtPrimeiraExportacao == null || pParametros.Objeto.DtPrimeiraExportacao == DateTime.MinValue) ? (object)System.DBNull.Value : pParametros.Objeto.DtPrimeiraExportacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ultimaexportacao", DbType.DateTime, (pParametros.Objeto.DtUltimaExportacao == null || pParametros.Objeto.DtUltimaExportacao == DateTime.MinValue) ? (object)System.DBNull.Value : pParametros.Objeto.DtUltimaExportacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_origemcadastro", DbType.String, pParametros.Objeto.DsOrigemCadastro);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_pessoa", DbType.String, pParametros.Objeto.TpPessoa);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_cliente", DbType.Int32, pParametros.Objeto.TpCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_passo", DbType.Int32, pParametros.Objeto.StPasso);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_sexo", DbType.String, pParametros.Objeto.CdSexo);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nacionalidade", DbType.Int32, pParametros.Objeto.CdNacionalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_paisnascimento", DbType.AnsiString, pParametros.Objeto.CdPaisNascimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ufnascimento", DbType.AnsiString, pParametros.Objeto.CdUfNascimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_ufnascimentoestrangeuro", DbType.String, pParametros.Objeto.DsUfnascimentoEstrangeiro);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_estadocivil", DbType.Int32, pParametros.Objeto.CdEstadoCivil);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_conjugue", DbType.String, pParametros.Objeto.DsConjugue);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_documento", DbType.String, pParametros.Objeto.TpDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimentofundacao", DbType.DateTime, pParametros.Objeto.DtNascimentoFundacao == DateTime.MinValue ? (object)System.DBNull.Value : pParametros.Objeto.DtNascimentoFundacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_orgaoemissordocumento", DbType.String, pParametros.Objeto.CdOrgaoEmissorDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ufemissaodocumento", DbType.String, pParametros.Objeto.CdUfEmissaoDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_profissaoatividade", DbType.Int32, pParametros.Objeto.CdProfissaoAtividade);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cargo", DbType.String, pParametros.Objeto.DsCargo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_empresa", DbType.String, pParametros.Objeto.DsEmpresa);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ppe", DbType.Boolean, pParametros.Objeto.StPPE);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_carteirapropria", DbType.Boolean, pParametros.Objeto.StCarteiraPropria);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_autorizadooperar", DbType.String, pParametros.Objeto.DsAutorizadoOperar);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_cvm387", DbType.Boolean, pParametros.Objeto.StCVM387);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_emancipado", DbType.Boolean, pParametros.Objeto.StEmancipado);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_cadastroportal", DbType.Boolean, pParametros.Objeto.StCadastroPortal);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessorinicial", DbType.Int32, pParametros.Objeto.IdAssessorInicial);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_escolaridade", DbType.Int32, pParametros.Objeto.CdEscolaridade);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nomefantasia", DbType.String, pParametros.Objeto.DsNomeFantasia);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nire", DbType.Int64, pParametros.Objeto.CdNire);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_formaconstituicao", DbType.String, pParametros.Objeto.DsFormaConstituicao);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_interdito", DbType.Boolean, pParametros.Objeto.StInterdito);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_situacaolegaloutros", DbType.Boolean, pParametros.Objeto.StSituacaoLegalOutros);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nomepai", DbType.String, pParametros.Objeto.DsNomePai);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nomemae", DbType.String, pParametros.Objeto.DsNomeMae);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_emissaodocumento", DbType.Date, pParametros.Objeto.DtEmissaoDocumento == null || DateTime.MinValue.Equals(pParametros.Objeto.DtEmissaoDocumento) ? System.DBNull.Value : (object)pParametros.Objeto.DtEmissaoDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_naturalidade", DbType.String, pParametros.Objeto.DsNaturalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_atividadeprincipal", DbType.String, pParametros.Objeto.CdAtividadePrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@nr_inscricaoestadual", DbType.String, pParametros.Objeto.NrInscricaoEstadual);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numerodocumento", DbType.String, pParametros.Objeto.DsNumeroDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_emailcomercial", DbType.String, pParametros.Objeto.DsEmailComercial);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_pessoavinculada", DbType.Int32, pParametros.Objeto.StPessoaVinculada);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_comoconheceu", DbType.Int32, pParametros.Objeto.DsComoConheceu);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_deseja_aplicar", DbType.String, pParametros.Objeto.TpDesejaAplicar);
                    
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_proposito_gradual", DbType.String, pParametros.Objeto.DsPropositoGradual);

                    lAcessaDados.AddInParameter(lDbCommand, "@st_usperson", DbType.String, pParametros.Objeto.StUSPerson);

                    if (!string.IsNullOrEmpty(pParametros.Objeto.DsUSPersonPJDetalhes))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@ds_usperson_pj_detalhes", DbType.String, pParametros.Objeto.DsUSPersonPJDetalhes);
                    }

                    if (pParametros.Objeto.StCienteDocumentos.HasValue)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@st_ciente_documentos", DbType.Int16, pParametros.Objeto.StCienteDocumentos.Value);
                    }
                    else
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@st_ciente_documentos", DbType.Int16, DBNull.Value);
                    }

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_cliente", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    if (null != pParametros.Objeto.DadosClienteNaoOperaPorContaPropria)
                    {   //--> Definindo os dados de cliente nao opera por conta propria  //--> Este dado não existe no Sinacor, portanto não deve ser informado.
                        pParametros.Objeto.DadosClienteNaoOperaPorContaPropria.IdCliente = lDbCommand.Parameters["@id_cliente"].Value.DBToInt32();
                        SalvarClienteNaoOperaPorContaPropria(pParametros.Objeto, pTrans);
                    }

                    return new SalvarEntidadeResponse<ClienteInfo>()
                    {
                        Codigo = lDbCommand.Parameters["@id_cliente"].Value.DBToInt32(),
                        Objeto = pParametros.Objeto, //--> Informando à variável de retorno dados de IdLogin, e-mail e senha.
                    };
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<ClienteInfo> Salvar(SalvarObjetoRequest<ClienteInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteInfo> lRetorno;

            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            try
            {
                lRetorno = Salvar(trans, pParametros);
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
            return lRetorno;
        }

        private static void LogarModificacao(ClienteInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteInfo> lEntrada = new ReceberEntidadeRequest<ClienteInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteInfo> lRetorno = ReceberCliente(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

        #endregion

        #region | Métodos Apoio

        private static void MontarParametrosConsultaClienteResumido(ref ConexaoDbHelper pAcessaDados, DbCommand pDbCommand, ClienteResumidoInfo pClienteResumidoInfo)
        {
            pAcessaDados.AddInParameter(pDbCommand, "@cd_bovespa", DbType.Int32, OpcoesBuscarPor.CodBovespa.Equals(pClienteResumidoInfo.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pClienteResumidoInfo.TermoDeBusca) ? (pClienteResumidoInfo.TermoDeBusca.Trim().Length > 8) ? (object)pClienteResumidoInfo.TermoDeBusca.Trim().Substring(0, 8) : (object)pClienteResumidoInfo.TermoDeBusca.Trim() : System.DBNull.Value);
            pAcessaDados.AddInParameter(pDbCommand, "@ds_cpfcnpj", DbType.String, OpcoesBuscarPor.CpfCnpj.Equals(pClienteResumidoInfo.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pClienteResumidoInfo.TermoDeBusca) ? (object)pClienteResumidoInfo.TermoDeBusca.Trim().Replace(".", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty) : System.DBNull.Value);
            pAcessaDados.AddInParameter(pDbCommand, "@ds_nome", DbType.String, OpcoesBuscarPor.NomeCliente.Equals(pClienteResumidoInfo.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pClienteResumidoInfo.TermoDeBusca) ? (object)pClienteResumidoInfo.TermoDeBusca.Trim() : System.DBNull.Value);
            pAcessaDados.AddInParameter(pDbCommand, "@ds_email", DbType.String, OpcoesBuscarPor.Email.Equals(pClienteResumidoInfo.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pClienteResumidoInfo.TermoDeBusca) ? (object)pClienteResumidoInfo.TermoDeBusca.Trim() : System.DBNull.Value);

            //pAcessaDados.AddInParameter(pDbCommand, "@tp_cliente", DbType.Byte, OpcoesTipo.ClientePF.Equals(pClienteResumidoInfo.OpcaoTipo) ? 1 : 2);

            if (null != pClienteResumidoInfo.CodAssessor)
            {
                pAcessaDados.AddInParameter(pDbCommand, "@cd_assessor", DbType.Int32, pClienteResumidoInfo.CodAssessor);
            }

            {   //--> Status (Ativo/Inativo)
                pAcessaDados.AddInParameter(pDbCommand, "@st_ativo", DbType.Byte, pClienteResumidoInfo.OpcaoStatus.Equals(OpcoesStatus.Ativo) ? 1 : 0);
                pAcessaDados.AddInParameter(pDbCommand, "@st_inativo", DbType.Byte, pClienteResumidoInfo.OpcaoStatus.Equals(OpcoesStatus.Inativo) ? 0 : 1);
            }

            {   //--> Pesquisa por Passo
                if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Visitante))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_cadastrado", DbType.Byte, 0);
                    pAcessaDados.AddInParameter(pDbCommand, "@st_exportadosinacor", DbType.Byte, 0);
                }
                else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Cadastrado))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_visitante", DbType.Byte, 0);
                    pAcessaDados.AddInParameter(pDbCommand, "@st_exportadosinacor", DbType.Byte, 0);
                }
                else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Exportado))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_visitante", DbType.Byte, 0);
                    pAcessaDados.AddInParameter(pDbCommand, "@st_cadastrado", DbType.Byte, 0);
                }
                else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Visitante | OpcoesPasso.Cadastrado))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_exportadosinacor", DbType.Byte, 0);
                }
                else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Visitante | OpcoesPasso.Exportado))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_cadastrado", DbType.Byte, 0);
                }
                else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Cadastrado | OpcoesPasso.Exportado))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_visitante", DbType.Byte, 0);
                }
                //else
                //{
                //    pAcessaDados.AddInParameter(pDbCommand, "@st_visitante", DbType.Byte, 1);
                //    pAcessaDados.AddInParameter(pDbCommand, "@st_cadastrado", DbType.Byte, 1);
                //    pAcessaDados.AddInParameter(pDbCommand, "@st_exportadosinacor", DbType.Byte, 1);
                //}
            }

            if ("PJ".Equals(pClienteResumidoInfo.TipoCliente))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@tp_pessoa", DbType.String, "J");
            }
            else if ("PF".Equals(pClienteResumidoInfo.TipoCliente))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@tp_pessoa", DbType.String, "F");
            }

            if (pClienteResumidoInfo.OpcaoPendencia != 0)
            {
                if (pClienteResumidoInfo.OpcaoPendencia.Equals(OpcoesPendencia.ComPendenciaCadastral))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_compendenciacadastral", DbType.Byte, 1);
                    pAcessaDados.AddInParameter(pDbCommand, "@st_comsolicitacaoalteracao", DbType.Byte, 0);
                }
                else if (pClienteResumidoInfo.OpcaoPendencia.Equals(OpcoesPendencia.ComSolicitacaoAlteracao))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_compendenciacadastral", DbType.Byte, 0);
                    pAcessaDados.AddInParameter(pDbCommand, "@st_comsolicitacaoalteracao", DbType.Byte, 1);
                }
                //else if (pClienteResumidoInfo.OpcaoPendencia.Equals(OpcoesPendencia.ComPendenciaCadastral | OpcoesPendencia.ComSolicitacaoAlteracao))
                //{
                //    pAcessaDados.AddInParameter(pDbCommand, "@st_compendenciacadastral", DbType.Byte, 1);
                //    pAcessaDados.AddInParameter(pDbCommand, "@st_comsolicitacaoalteracao", DbType.Byte, 1);
                //}
            }
            //else
            //{
            //    pAcessaDados.AddInParameter(pDbCommand, "@st_compendenciacadastral", DbType.Byte, 0);
            //    pAcessaDados.AddInParameter(pDbCommand, "@st_comsolicitacaoalteracao", DbType.Byte, 0);
            //}
        }

        public static string LimparCpfCnpj(string pCpfCnpj)
        {
            string lCpfCnpj = pCpfCnpj.Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", "").Replace(@"\", string.Empty).Replace(" ", string.Empty);
            return lCpfCnpj;
        }

        public static void ValidarCpfCnpj(string pCpfCnpj)
        {
            Validacao.ValidaCpfCnpj(LimparCpfCnpj(pCpfCnpj), gCpfCnpjInvalido);
        }

        /// <summary>
        /// Verifica se CPF já está cadastrado
        /// </summary>
        /// <param name="pCliente">Entidade Cliente</param>
        private static void ValidaCpfCnpJaExiste(ClienteInfo pCliente)
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
            int lRetorno = 0;
            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_verificaCpfCnpj_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pCliente.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, LimparCpfCnpj(pCliente.DsCpfCnpj));
                lAcessaDados.AddOutParameter(lDbCommand, "@count", DbType.Int32, 8);
                lAcessaDados.ExecuteNonQuery(lDbCommand);
                lRetorno = lDbCommand.Parameters["@count"].Value.DBToInt32();

            }

            if (lRetorno > 0)
                throw new Exception(gCpfCnpjJaCadastrado);
        }

        /// <summary>
        /// Verifica se o emailjá está cadastrado
        /// </summary>
        /// <param name="pCliente">Entidade Cliente</param>
        public static void ValidaEmailJaExisteCliente(ClienteInfo pCliente)
        {
            var lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
            int lRetorno = 0;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_verificaEmail_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pCliente.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pCliente.DsEmail.Trim());
                lAcessaDados.AddOutParameter(lDbCommand, "@count", DbType.Int32, 8);
                lAcessaDados.ExecuteNonQuery(lDbCommand);
                lRetorno = lDbCommand.Parameters["@count"].Value.DBToInt32();
            }

            if (lRetorno > 0)
                throw new Exception(gEmailJaCadastrado);
        }

        private static ClienteEducacionalInfo Converter(ClienteInfo pParametro)
        {
            var lRetorno = new ClienteEducacionalInfo();

            lRetorno.IdCliente = pParametro.IdCliente;
            lRetorno.DsNome = pParametro.DsNome;
            lRetorno.IdLogin = pParametro.IdLogin;
            lRetorno.DtUltimaAtualizacao = pParametro.DtUltimaAtualizacao;
            lRetorno.DsCpfCnpj = pParametro.DsCpfCnpj;
            lRetorno.DtPasso1 = pParametro.DtPasso1;
            lRetorno.DtPasso2 = pParametro.DtPasso2;
            lRetorno.DtPasso3 = pParametro.DtPasso3;
            lRetorno.DtPrimeiraExportacao = pParametro.DtPrimeiraExportacao;
            lRetorno.DtUltimaExportacao = pParametro.DtUltimaExportacao;
            lRetorno.DsOrigemCadastro = pParametro.DsOrigemCadastro;
            lRetorno.TpPessoa = pParametro.TpPessoa;
            lRetorno.TpCliente = pParametro.TpCliente;
            lRetorno.StPasso = pParametro.StPasso;
            lRetorno.CdSexo = pParametro.CdSexo;
            lRetorno.CdNacionalidade = pParametro.CdNacionalidade;
            lRetorno.CdPaisNascimento = pParametro.CdPaisNascimento;
            lRetorno.CdUfNascimento = pParametro.CdUfNascimento;
            lRetorno.DsUfnascimentoEstrangeiro = pParametro.DsUfnascimentoEstrangeiro;
            lRetorno.CdEstadoCivil = pParametro.CdEstadoCivil;
            lRetorno.DsConjugue = pParametro.DsConjugue;
            lRetorno.TpDocumento = pParametro.TpDocumento;
            lRetorno.DtNascimentoFundacao = pParametro.DtNascimentoFundacao;
            lRetorno.CdOrgaoEmissorDocumento = pParametro.CdOrgaoEmissorDocumento;
            lRetorno.CdUfEmissaoDocumento = pParametro.CdUfEmissaoDocumento;
            lRetorno.CdProfissaoAtividade = pParametro.CdProfissaoAtividade;
            lRetorno.StPessoaVinculada = pParametro.StPessoaVinculada;
            lRetorno.DsEmpresa = pParametro.DsEmpresa;
            lRetorno.StPPE = pParametro.StPPE;
            lRetorno.StCarteiraPropria = pParametro.StCarteiraPropria;
            lRetorno.DsAutorizadoOperar = pParametro.DsAutorizadoOperar;
            lRetorno.StCVM387 = pParametro.StCVM387;
            lRetorno.StEmancipado = pParametro.StEmancipado;
            lRetorno.IdAssessorInicial = pParametro.IdAssessorInicial;
            lRetorno.CdEscolaridade = pParametro.CdEscolaridade;
            lRetorno.StCadastroPortal = pParametro.StCadastroPortal;
            lRetorno.DsNomeFantasia = pParametro.DsNomeFantasia;
            lRetorno.CdNire = pParametro.CdNire;
            lRetorno.DsFormaConstituicao = pParametro.DsFormaConstituicao;
            lRetorno.StInterdito = pParametro.StInterdito;
            lRetorno.StSituacaoLegalOutros = pParametro.StSituacaoLegalOutros;
            lRetorno.DsNumeroDocumento = pParametro.DsNumeroDocumento;
            lRetorno.CdAtividadePrincipal = pParametro.CdAtividadePrincipal;
            lRetorno.DsEmail = pParametro.DsEmail;
            lRetorno.DsEmailComercial = pParametro.DsEmailComercial;
            lRetorno.CdSenha = pParametro.CdSenha;
            lRetorno.DsCargo = pParametro.DsCargo;
            lRetorno.DsNomePai = pParametro.DsNomePai;
            lRetorno.DsNomeMae = pParametro.DsNomeMae;
            lRetorno.DtEmissaoDocumento = pParametro.DtEmissaoDocumento;
            lRetorno.DsNaturalidade = pParametro.DsNaturalidade;
            lRetorno.NrInscricaoEstadual = pParametro.NrInscricaoEstadual;
            lRetorno.StAtivo = pParametro.StAtivo;
            lRetorno.DtAtivacaoInativacao = pParametro.DtAtivacaoInativacao;

            return lRetorno;
        }

        private static ClienteInfo CriarRegistroCliente(DataRow linha)
        {
            ClienteInfo lRetorno = new ClienteInfo()
            {
                StCadastroPortal                    = linha["st_cadastroportal"].DBToBoolean(),
                DsCargo                             = linha["ds_cargo"].DBToString(),
                StCarteiraPropria                   = linha["st_carteirapropria"].DBToBoolean(),
                DsAutorizadoOperar                  = linha["ds_autorizadooperar"].DBToString(),
                StCVM387                            = linha["st_cvm387"].DBToBoolean(),
                DsConjugue                          = linha["ds_conjugue"].DBToString(),
                DsCpfCnpj                           = linha["ds_cpfcnpj"].ToString(),
                DtNascimentoFundacao                = linha["dt_nascimentofundacao"].DBToDateTime(),
                DtPasso1                            = linha["dt_passo1"].DBToDateTime(),
                DtPasso2                            = linha["dt_passo2"].DBToDateTime(),
                DtPasso3                            = linha["dt_passo3"].DBToDateTime(),
                DtPrimeiraExportacao                = linha["dt_primeiraexportacao"].DBToDateTime(),
                DtUltimaAtualizacao                 = linha["dt_ultimaatualizacao"].DBToDateTime(),
                DtUltimaExportacao                  = linha["dt_ultimaexportacao"].DBToDateTime(),
                StEmancipado                        = linha["st_emancipado"].DBToBoolean(),
                DsEmpresa                           = linha["ds_empresa"].DBToString(),
                CdEscolaridade                      = linha["cd_escolaridade"].DBToInt32(),
                CdEstadoCivil                       = linha["cd_estadocivil"].DBToInt32(),
                DsFormaConstituicao                 = linha["ds_formaconstituicao"].DBToString(),
                IdAssessorInicial                   = linha["id_assessorinicial"].DBToInt32(),
                IdCliente                           = linha["id_cliente"].DBToInt32(),
                IdLogin                             = linha["id_login"].DBToInt32(),
                CdNacionalidade                     = linha["cd_nacionalidade"].DBToInt32(),
                CdNire                              = linha["cd_nire"].DBToInt64(),
                DsNome                              = linha["ds_nome"].DBToString(),
                DsNomeFantasia                      = linha["ds_nomefantasia"].DBToString(),
                CdOrgaoEmissorDocumento             = linha["cd_orgaoemissordocumento"].DBToString(),
                DsOrigemCadastro                    = linha["ds_origemcadastro"].DBToString(),
                CdPaisNascimento                    = linha["cd_paisnascimento"].DBToString(),
                StPasso                             = linha["st_passo"].DBToInt32(),
                StPPE                               = linha["st_ppe"].DBToBoolean(),
                CdProfissaoAtividade                = linha["cd_profissaoatividade"].DBToInt32(),
                CdAtividadePrincipal                = linha["cd_atividadePrincipal"].DBToInt32(),
                CdSexo                              = linha["cd_sexo"].DBToChar(),
                TpCliente                           = linha["tp_cliente"].DBToInt32(),
                TpDocumento                         = linha["tp_documento"].DBToString(),
                TpPessoa                            = linha["tp_pessoa"].DBToChar(),
                CdUfEmissaoDocumento                = linha["cd_ufemissaodocumento"].DBToString(),
                CdUfNascimento                      = linha["cd_ufnascimento"].DBToString(),
                DsUfnascimentoEstrangeiro           = linha["ds_ufnascimentoestrangeuro"].DBToString(),
                StInterdito                         = linha["st_interdito"].DBToBoolean(),
                StSituacaoLegalOutros               = linha["st_situacaolegaloutros"].DBToBoolean(),
                DsNomePai                           = linha["Ds_NomePai"].DBToString(),
                DsNomeMae                           = linha["Ds_NomeMae"].DBToString(),
                DtEmissaoDocumento                  = linha["Dt_EmissaoDocumento"].DBToDateTime(),
                DsNaturalidade                      = linha["Ds_Naturalidade"].DBToString(),
                NrInscricaoEstadual                 = linha["Nr_InscricaoEstadual"].DBToString(),
                DsNumeroDocumento                   = linha["ds_numerodocumento"].DBToString(),
                DsEmailComercial                    = linha["ds_emailcomercial"].DBToString(),
                DsEmail                             = linha["ds_email"].DBToString(),
                StPessoaVinculada                   = linha["st_pessoavinculada"].DBToInt32(),
                DsComoConheceu                      = linha["ds_comoconheceu"].DBToString(),
                DsPropositoGradual                  = linha["ds_proposito_gradual"].DBToString(),
                DsUSPersonPJDetalhes                = linha["ds_usperson_pj_detalhes"].DBToString(),
                DadosClienteNaoOperaPorContaPropria = new ClienteNaoOperaPorContaPropriaInfo(),
            };

            try
            {
                lRetorno.TpDesejaAplicar = linha["tp_deseja_aplicar"].DBToString();
            }
            catch { }

            try
            {
                if(linha["st_usperson"] != DBNull.Value)
                    lRetorno.StUSPerson = linha["st_usperson"].DBToBoolean();
            }
            catch { }

            try
            {
                if (linha["st_ciente_documentos"] != DBNull.Value)
                {
                    lRetorno.StCienteDocumentos = Convert.ToInt16(linha["st_ciente_documentos"]);
                }
            }
            catch { }

            return lRetorno;
        }

        private static ClienteResumidoInfo CriarRegistroClienteResumidoDadosBasicos(DataRow pRow)
        {
            var lRetorno = new ClienteResumidoInfo();

            lRetorno.CPF = pRow["ds_cpfcnpj"].DBToString().ToCpfCnpjString();
            lRetorno.NomeCliente = pRow["ds_nome"].DBToString();
            lRetorno.Email = pRow["ds_email"].DBToString();
            lRetorno.CodBovespa = pRow["cd_codigo"].DBToString();

            return lRetorno;
        }

        private static ClienteResumidoInfo CriarRegistroClienteResumido(DataRow pRow)
        {
            return CriarRegistroClienteResumido(pRow, false);
        }

        private static ClienteResumidoMigracaoInfo CriarRegistroClienteResumidoMigracao(DataRow pRow, bool pSomenteTabelaCliente, int TotalRegistros)
        {
            var lRetorno = new ClienteResumidoMigracaoInfo();
            try
            {
                lRetorno.Status = (pRow["st_status"].DBToBoolean() ? "Ativo" : "Inativo");
                lRetorno.TipoCliente = string.Concat("P", pRow["tp_pessoa"].DBToString());
                lRetorno.CPF = pRow["ds_cpfcnpj"].DBToString().ToCpfCnpjString();
                lRetorno.Sexo = pRow["cd_sexo"].DBToString() == "1" ? "F" : "M";
                lRetorno.IdCliente = pRow["id_cliente"].DBToInt32();
                lRetorno.Passo = pRow["st_passo"].DBToString();
                lRetorno.NomeCliente = pRow["ds_nome"].DBToString();
                lRetorno.Cise = CriticaCISE(pRow["ds_cpfcnpj"].DBToString());

                lRetorno.TotalRegistros = TotalRegistros;

                if (pRow.Table.Columns.Contains("cd_codigo"))
                    lRetorno.CodBovespa = pRow["cd_codigo"].DBToString();

                if (pRow.Table.Columns.Contains("cd_sistema"))
                    lRetorno.CodSistema = pRow["cd_sistema"].DBToString();

                ClienteContaAux lConta = new ClienteContaAux();

                ClienteContaAux lContaBmf = new ClienteContaAux();

                if (pRow["cd_bovespa"] != null && !string.IsNullOrEmpty(pRow["cd_bovespa"].ToString()))
                {
                    lConta = ObterDadosCliente(int.Parse(pRow["cd_bovespa"].ToString()));
                }

                if (pRow["cd_bmf"] != null && !string.IsNullOrEmpty(pRow["cd_bmf"].DBToString()))
                {
                    lContaBmf = ObterContaBMF(int.Parse(pRow["cd_bmf"].ToString()));
                }

                if (!pSomenteTabelaCliente)
                {
                    lRetorno.FlagPendencia = pRow["st_pendencia"].DBToString();

                    lRetorno.DataNascimento = pRow["dt_nascimento"].DBToDateTime();

                    lRetorno.DataCadastro = pRow["dt_cadastro"].DBToDateTime();

                    lRetorno.CodGradual = pRow["cd_bovespa"].DBToString();

                    lRetorno.CodBMF = pRow["cd_bmf"].DBToString();

                    lRetorno.CodBovespa = pRow["cd_bovespa"].DBToString();

                    lRetorno.CodBMFAtiva = lContaBmf.StatusBmf == "A" ? lContaBmf.StatusBmf : "I"; //pRow["cd_bmf_ativa"].DBToBoolean() ? "A" : "I";

                    lRetorno.CodBovespaAtiva = lConta.StatusBovespa == "A" ? lConta.StatusBovespa : "I"; // pRow["cd_bovespa_ativa"].DBToBoolean() ? "A" : "I";

                    lRetorno.Email = pRow["ds_email"].DBToString();

                    lRetorno.CodAssessor = pRow["cd_assessor"].DBToInt32();
                }
                else
                {
                    lRetorno.DataNascimento = pRow["dt_nascimentofundacao"].DBToDateTime();
                    lRetorno.DataCadastro = pRow["dt_passo1"].DBToDateTime();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lRetorno;
        }

        private static ClienteResumidoInfo CriarRegistroClienteResumido(DataRow pRow, bool pSomenteTabelaCliente)
        {
            var lRetorno = new ClienteResumidoInfo();
            try
            {
                lRetorno.Status = (pRow["st_status"].DBToBoolean() ? "Ativo" : "Inativo");
                lRetorno.TipoCliente = string.Concat("P", pRow["tp_pessoa"].DBToString());
                lRetorno.CPF = pRow["ds_cpfcnpj"].DBToString().ToCpfCnpjString();
                lRetorno.Sexo = pRow["cd_sexo"].DBToString() == "1" ? "F" : "M";
                lRetorno.IdCliente = pRow["id_cliente"].DBToInt32();
                lRetorno.Passo = pRow["st_passo"].DBToString();
                lRetorno.NomeCliente = pRow["ds_nome"].DBToString();
                lRetorno.Cise = CriticaCISE(pRow["ds_cpfcnpj"].DBToString());

                if (pRow.Table.Columns.Contains("cd_codigo"))
                    lRetorno.CodBovespa = pRow["cd_codigo"].DBToString();

                if (pRow.Table.Columns.Contains("cd_sistema"))
                    lRetorno.CodSistema = pRow["cd_sistema"].DBToString();

                ClienteContaAux lConta = new ClienteContaAux();

                ClienteContaAux lContaBmf = new ClienteContaAux();

                if (pRow["cd_bovespa"] != null && !string.IsNullOrEmpty(pRow["cd_bovespa"].ToString()))
                {
                    lConta = ObterDadosCliente(int.Parse(pRow["cd_bovespa"].ToString()));
                }

                if (pRow["cd_bmf"] != null && !string.IsNullOrEmpty(pRow["cd_bmf"].DBToString()))
                {
                    lContaBmf = ObterContaBMF(int.Parse(pRow["cd_bmf"].ToString()));
                }

                if (!pSomenteTabelaCliente)
                {
                    lRetorno.FlagPendencia   = pRow["st_pendencia"].DBToString();

                    lRetorno.DataNascimento  = pRow["dt_nascimento"].DBToDateTime();

                    lRetorno.DataCadastro    = pRow["dt_cadastro"].DBToDateTime();

                    lRetorno.CodGradual      = pRow["cd_bovespa"].DBToString();

                    lRetorno.CodBMF          = pRow["cd_bmf"].DBToString();

                    lRetorno.CodBovespa      = pRow["cd_bovespa"].DBToString();

                    lRetorno.CodBMFAtiva     = lContaBmf.StatusBmf == "A" ? lContaBmf.StatusBmf : "I"; //pRow["cd_bmf_ativa"].DBToBoolean() ? "A" : "I";

                    lRetorno.CodBovespaAtiva = lConta.StatusBovespa == "A" ? lConta.StatusBovespa : "I"; // pRow["cd_bovespa_ativa"].DBToBoolean() ? "A" : "I";

                    lRetorno.Email           = pRow["ds_email"].DBToString();

                    lRetorno.CodAssessor     = pRow["cd_assessor"].DBToInt32();
                }
                else
                {
                    lRetorno.DataNascimento = pRow["dt_nascimentofundacao"].DBToDateTime();
                    lRetorno.DataCadastro = pRow["dt_passo1"].DBToDateTime();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lRetorno;
        }

        public class ClienteContaAux
        {
            public string CodigoAssessor { get; set; }
            public string NomeAssessor { get; set; }
            public string NomeCliente { get; set; }
            public string StatusBovespa { get; set; }
            public int CodigoBovespa { get; set; }
            public int CodigoBmf { get; set; }
            public string StatusBmf { get; set; }
        }

        public static ClienteContaAux ObterContaBMF(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            ClienteContaAux lRetorno = new ClienteContaAux();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_OBTEM_COD_BMF_INTRA"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        lRetorno.CodigoBmf = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                        lRetorno.StatusBmf = (lDataTable.Rows[i]["Status"]).DBToString();
                    }
                }
            }

            return lRetorno;
        }

        public static ClienteContaAux ObterDadosCliente(int? CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            ClienteContaAux lRetorno = new ClienteContaAux();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacorTrade;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obtem_cliente_asse_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string TipoCliente = (lDataTable.Rows[i]["Tipo"]).DBToString();

                            lRetorno.CodigoAssessor = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            lRetorno.NomeCliente    = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            lRetorno.NomeAssessor   = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();

                            if (TipoCliente == "BOVESPA")
                            {
                                lRetorno.CodigoBovespa = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                lRetorno.StatusBovespa = (lDataTable.Rows[i]["situac"]).DBToString();
                            }
                            else
                            {
                                lRetorno.CodigoBmf = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                lRetorno.StatusBmf = (lDataTable.Rows[i]["situac"]).DBToString();
                            }

                            lRetorno.CodigoAssessor = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            lRetorno.NomeCliente    = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lRetorno;
        }

        private static ClientePassoContaInfo CriarRegistroClientePassoConta(DataRow linha)
        {
            return new ClientePassoContaInfo()
            {
                IdCliente = linha["id_cliente"].DBToInt32(),
                StPasso = linha["st_passo"].DBToInt32(),
                IdClienteConta = linha["id_cliente_conta"].DBToInt32(),
                CdAssessor = linha["cd_assessor"].DBToInt32(),
                CdSistema = (eAtividade)linha["cd_sistema"].DBToInt32(),
                StPrincipal = linha["st_principal"].DBToBoolean(),
            };
        }

        private static string CriticaCISE(string pCpfCnpj)
        {
            var lRetorno = string.Empty;

            if (!string.IsNullOrWhiteSpace(pCpfCnpj))
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_cise_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCpfCnpj", DbType.Int64, LimparCpfCnpj(pCpfCnpj));

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        lRetorno = lDataTable.Rows[0]["DS_COMENT"].ToString();
                }
                
            }

            return lRetorno;
        }

        private static void SalvarClienteNaoOperaPorContaPropria(ClienteInfo pCliente, DbTransaction pDbTransaction)
        {
            if (null == pCliente.StCarteiraPropria || !pCliente.StCarteiraPropria.Value)
            {    // inclui / altera
                SalvarClienteNaoOperaPorContaPropria(new SalvarObjetoRequest<ClienteNaoOperaPorContaPropriaInfo>()
                {
                    Objeto = new ClienteNaoOperaPorContaPropriaInfo()
                    {
                        DsCpfCnpjClienteRepresentado = pCliente.DadosClienteNaoOperaPorContaPropria.DsCpfCnpjClienteRepresentado,
                        DsNomeClienteRepresentado = pCliente.DadosClienteNaoOperaPorContaPropria.DsNomeClienteRepresentado,
                        IdCliente = pCliente.DadosClienteNaoOperaPorContaPropria.IdCliente,
                    },
                }, pDbTransaction);
            }
            else
            {  // exclui
                ExcluirClienteNaoOperaPorContaPropria(new RemoverEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo>()
                {
                    Objeto = new ClienteNaoOperaPorContaPropriaInfo()
                    {
                        IdCliente = pCliente.DadosClienteNaoOperaPorContaPropria.IdCliente,
                    }
                }, pDbTransaction);
            }
        }

        #endregion
    }
}
