using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Negocio;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados.Portal;
using System.Collections.Generic;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        private const char ERRONEGOCIO = '|';

        #region | Portal

        public static ReceberObjetoResponse<EfetuarLoginInfo> ReceberLogin(ReceberEntidadeRequest<EfetuarLoginInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<EfetuarLoginInfo> lRetorno = new ReceberObjetoResponse<EfetuarLoginInfo>();
                lRetorno.Objeto = pParametros.Objeto;

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_efetuarlogin_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@Cd_Codigo", DbType.Int64, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddInParameter(lDbCommand, "@Cd_Senha", DbType.String, Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenha));

                    lAcessaDados.AddOutParameter(lDbCommand, "@Id_Cliente", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Id_Login", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Ds_Nome", DbType.String, 80);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Ds_EmailRetorno", DbType.String, 80);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Ds_CpfCnpj", DbType.String, 15);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Cd_CodigoPrincipal", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Cd_AssessorPrincipal", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@St_Passo", DbType.Int32, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Dt_NascimentoFundacao", DbType.Date, 8);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Tp_Pessoa", DbType.String, 1);
                    lAcessaDados.AddOutParameter(lDbCommand, "@Dt_Passo1", DbType.Date, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.Objeto.IdCliente = lDbCommand.Parameters["@Id_Cliente"].Value.DBToInt32();
                    lRetorno.Objeto.IdLogin = lDbCommand.Parameters["@Id_Login"].Value.DBToInt32();
                    lRetorno.Objeto.DsNome = lDbCommand.Parameters["@Ds_Nome"].Value.DBToString();
                    lRetorno.Objeto.DsEmailRetorno = lDbCommand.Parameters["@Ds_EmailRetorno"].Value.DBToString();
                    lRetorno.Objeto.DsCpfCnpj = lDbCommand.Parameters["@Ds_CpfCnpj"].Value.DBToString();
                    lRetorno.Objeto.CdCodigoPrincipal = lDbCommand.Parameters["@Cd_CodigoPrincipal"].Value.DBToInt32();
                    lRetorno.Objeto.CdAssessorPrincipal = lDbCommand.Parameters["@Cd_AssessorPrincipal"].Value.DBToInt32();
                    lRetorno.Objeto.StPasso = lDbCommand.Parameters["@St_Passo"].Value.DBToInt32();
                    lRetorno.Objeto.DtNascimentoFundacao = lDbCommand.Parameters["@Dt_NascimentoFundacao"].Value.DBToDateTime();
                    lRetorno.Objeto.TpPessoa = lDbCommand.Parameters["@Tp_Pessoa"].Value.DBToString();
                    lRetorno.Objeto.DtPasso1 = lDbCommand.Parameters["@Dt_Passo1"].Value.DBToDateTime();
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_nrtentativaserradas_sel_sp"))
                {   //--> Definindo o NrTentativasErradas
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.String, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        lRetorno.Objeto.NrTentativasErradas = lDataTable.Rows[0]["nr_tentativaserradas"].DBToInt32();
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static SalvarEntidadeResponse<AlterarAssinaturaEletronicaInfo> AtualizarAssinaturaEletronica(SalvarObjetoRequest<AlterarAssinaturaEletronicaInfo> pParametros)
        {
            try
            {
                if (Integracao_ValidarAssinatura(pParametros))
                {

                    ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                    lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_assinatura_upd_sp"))
                    {
                        //Tratamento para ver se a assinatura atual está ok está na proc
                        pParametros.Objeto.CdAssinaturaNova = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaNova);
                        pParametros.Objeto.CdAssinaturaAntiga = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaAntiga);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturaantiga", DbType.String, pParametros.Objeto.CdAssinaturaAntiga);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturanova", DbType.String, pParametros.Objeto.CdAssinaturaNova);

                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }

                    lAcessaDados = new ConexaoDbHelper();
                    lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_updt_cliente_novo_hb"))
                    {
                        //Tratamento para ver se a assinatura atual está ok está na proc
                        pParametros.Objeto.CdAssinaturaNova = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaNova);
                        pParametros.Objeto.CdAssinaturaAntiga = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaAntiga);
                        lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente", DbType.Int32, pParametros.Objeto.CodigoPrincipal);
                        lAcessaDados.AddInParameter(lDbCommand, "@AlterarAssinatura", DbType.Byte, 0);

                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                    

                    return new SalvarEntidadeResponse<AlterarAssinaturaEletronicaInfo>();
                }
                else
                {
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                    throw new Exception(ERRONEGOCIO.ToString() + "" + ERRONEGOCIO.ToString());
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static SalvarEntidadeResponse<AlterarAssinaturaEletronicaDinamicaInfo> AtualizarAssinaturaEletronicaDinamica(SalvarObjetoRequest<AlterarAssinaturaEletronicaDinamicaInfo> pParametros)
        {
            try
            {
                if (Integracao_ValidarAssinaturaDinamica(pParametros))
                {

                    ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                    lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_assinatura_dinamica_upd_sp"))
                    {
                        //Tratamento para ver se a assinatura atual está ok está na proc
                        pParametros.Objeto.CdAssinaturaNova         = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaNova);
                        pParametros.Objeto.CdAssinaturaAntiga       = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaAntiga);
                        if (pParametros.Objeto.AssinaturaDinamica != null)
                        {
                            pParametros.Objeto.AssinaturaDinamica = pParametros.Objeto.AssinaturaDinamica.AssinaturaHash();
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_assinatura_caractere_1", DbType.String, pParametros.Objeto.AssinaturaDinamica.Caractere1.Opcao1);
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_assinatura_caractere_2", DbType.String, pParametros.Objeto.AssinaturaDinamica.Caractere2.Opcao1);
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_assinatura_caractere_3", DbType.String, pParametros.Objeto.AssinaturaDinamica.Caractere3.Opcao1);
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_assinatura_caractere_4", DbType.String, pParametros.Objeto.AssinaturaDinamica.Caractere4.Opcao1);
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_assinatura_caractere_5", DbType.String, pParametros.Objeto.AssinaturaDinamica.Caractere5.Opcao1);
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_assinatura_caractere_6", DbType.String, pParametros.Objeto.AssinaturaDinamica.Caractere6.Opcao1);
                        }

                        pParametros.Objeto.AssinaturaDinamicaNova   = pParametros.Objeto.AssinaturaDinamicaNova.AssinaturaHash();
                        
                        lAcessaDados.AddInParameter(lDbCommand, "@id_login"                         , DbType.Int32  , pParametros.Objeto.IdLogin);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturaantiga"              , DbType.String , pParametros.Objeto.CdAssinaturaAntiga);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturanova"                , DbType.String , pParametros.Objeto.CdAssinaturaNova);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_assinatura_caractere_1"   , DbType.String , pParametros.Objeto.AssinaturaDinamicaNova.Caractere1.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_assinatura_caractere_2"   , DbType.String , pParametros.Objeto.AssinaturaDinamicaNova.Caractere2.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_assinatura_caractere_3"   , DbType.String , pParametros.Objeto.AssinaturaDinamicaNova.Caractere3.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_assinatura_caractere_4"   , DbType.String , pParametros.Objeto.AssinaturaDinamicaNova.Caractere4.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_assinatura_caractere_5"   , DbType.String , pParametros.Objeto.AssinaturaDinamicaNova.Caractere5.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_assinatura_caractere_6"   , DbType.String , pParametros.Objeto.AssinaturaDinamicaNova.Caractere6.Opcao1);


                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }

                    lAcessaDados = new ConexaoDbHelper();
                    lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_updt_cliente_novo_hb"))
                    {
                        //Tratamento para ver se a assinatura atual está ok está na proc
                        pParametros.Objeto.CdAssinaturaNova = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaNova);
                        pParametros.Objeto.CdAssinaturaAntiga = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaAntiga);
                        lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente", DbType.Int32, pParametros.Objeto.CodigoPrincipal);
                        lAcessaDados.AddInParameter(lDbCommand, "@AlterarAssinatura", DbType.Byte, 0);

                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }


                    return new SalvarEntidadeResponse<AlterarAssinaturaEletronicaDinamicaInfo>();
                }
                else
                {
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                    throw new Exception(ERRONEGOCIO.ToString() + "" + ERRONEGOCIO.ToString());
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        const string INTEGRACAO_OK      = "OK";
        const string INTEGRACAO_SIM     = "SIM";
        const string INTEGRACAO_NAO     = "NAO";
        const string INTEGRACAO_NAOOK   = "NAOOK";

        public static bool Integracao_ValidarAssinaturaDinamica(SalvarObjetoRequest<AlterarAssinaturaEletronicaDinamicaInfo> pParametros)
        {
            bool lRetorno = false;

            SalvarObjetoRequest<AlterarAssinaturaEletronicaInfo> obj = new SalvarObjetoRequest<AlterarAssinaturaEletronicaInfo>();
            obj.Objeto = new AlterarAssinaturaEletronicaInfo();

            obj.Objeto.CodigoPrincipal      = pParametros.Objeto.CodigoPrincipal; 
            obj.Objeto.CdAssinaturaAntiga   = pParametros.Objeto.CdAssinaturaAntiga;
            obj.Objeto.CdAssinaturaNova     = pParametros.Objeto.CdAssinaturaNova;

            lRetorno = Integracao_ValidarAssinatura(obj);

            return lRetorno;
        }

        public static bool Integracao_ValidarAssinatura(SalvarObjetoRequest<AlterarAssinaturaEletronicaInfo> pParametros)
        {
            bool lRetorno           = false;
            String lMensagem        = String.Empty;
            String Login            = pParametros.Objeto.CodigoPrincipal.ToString().PadLeft(7,'0');
            String AssinaturaAntiga = pParametros.Objeto.CdAssinaturaAntiga;
            String AssinaturaNova   = pParametros.Objeto.CdAssinaturaNova;
            var lResposta           = String.Empty;
            br.com.gradualinvestimentos.hb.CadastroHB servico = new br.com.gradualinvestimentos.hb.CadastroHB();

            try
            {
                // Verifica se o usuário possui assintatura cadastrada no ambiente de terceiros
                lResposta = servico.ExisteAssinaturaDigital(Login);

                // Caso o usuário possua assinatura
                if (lResposta.ToUpper().Equals(INTEGRACAO_SIM.ToUpper()))
                {
                    // Verifica se a assinatura está equalizada
                    lResposta = servico.EqualsAssinaturaDigital(Login, AssinaturaNova);

                    // Caso a assinatura esteja equalizada, nenhuma ação é necessária
                    if (lResposta.ToString().ToUpper().Equals(INTEGRACAO_OK.ToUpper()))
                    {
                        //TASK: não é necessário tratamento 
                        lRetorno = true;
                    }

                    // Caso a assinatura não esteja equalizada, será necessário alterar a assinatura
                    if (lResposta.ToString().ToUpper().Equals(INTEGRACAO_NAOOK.ToUpper()))
                    {
                        lResposta = servico.AlterarAssinaturaDigital(Login, AssinaturaNova);

                        // Caso o servico informe que a alteracao foi efetuada, não é necessaria nenhuma acao
                        if (lResposta.ToString().ToUpper().Equals(INTEGRACAO_OK.ToUpper()))
                        {
                            lRetorno = true;
                        }
                        else // Caso o servico informe que a alteracao não foi efetuada
                        {
                            lRetorno = false;

                            //1-Sua Senha é considerada fraca pelo sistema.\\nUse uma senha com no mínimo [8] e no máximo [15] caracteres!
                            if (lResposta.Contains("1-Sua Senha é considerada fraca pelo sistema."))
                            {
                                lMensagem = String.Format("000{0}", lResposta);
                            }
                            //2-Sua Senha é considerada fraca pelo sistema.\\nA Senha não deve ser igual ao Login!
                            if (lResposta.Contains("2-Sua Senha é considerada fraca pelo sistema."))
                            {
                                lMensagem = String.Format("000{0}", lResposta);
                            }
                            //3-Sua Senha é considerada fraca pelo sistema.\\nNão use caracteres iguais em sequência!
                            if (lResposta.Contains("3-Sua Senha é considerada fraca pelo sistema."))
                            {
                                lMensagem = String.Format("000{0}", lResposta);
                            }
                            //4-Sua Senha é considerada fraca pelo sistema.\nNão use sequência de caracteres!
                            if (lResposta.Contains("4-Sua Senha é considerada fraca pelo sistema."))
                            {
                                lMensagem = String.Format("000{0}", lResposta);
                            }
                            //5-Sua Senha é considerada fraca pelo sistema e/ou possui caracteres inválidos.\\nUse uma senha alfanumérica!
                            if (lResposta.Contains("5-Sua Senha é considerada fraca pelo sistema e/ou possui caracteres inválidos."))
                            {
                                lMensagem = String.Format("000{0}", lResposta);
                            }
                            //6-Sua Senha é considerada fraca pelo sistema.\\nSenha já utilizada anteriormente!
                            if (lResposta.Contains("6-Sua Senha é considerada fraca pelo sistema."))
                            {
                                lMensagem = String.Format("000{0}", lResposta);
                            }

                            throw new Exception(ERRONEGOCIO.ToString() + lMensagem + ERRONEGOCIO.ToString());
                        }
                    }
                }
                else
                {
                    lResposta = servico.AlterarAssinaturaDigital(Login, AssinaturaNova);

                    // Caso o servico informe que a alteracao foi efetuada, não é necessaria nenhuma acao
                    if (lResposta.ToString().ToUpper().Equals(INTEGRACAO_OK.ToUpper()))
                    {
                        lRetorno = true;
                    }
                    else // Caso o servico informe que a alteracao não foi efetuada
                    {
                        lRetorno = false;
                        lMensagem = lResposta;

                        //1-Sua Senha é considerada fraca pelo sistema.\\nUse uma senha com no mínimo [8] e no máximo [15] caracteres!
                        if(lResposta.Contains("1-Sua Senha é considerada fraca pelo sistema."))
                        {
                            lMensagem = String.Format("000{0}", lResposta);
                        }
                        //2-Sua Senha é considerada fraca pelo sistema.\\nA Senha não deve ser igual ao Login!
                        if(lResposta.Contains("2-Sua Senha é considerada fraca pelo sistema."))
                        {
                            lMensagem = String.Format("000{0}", lResposta);
                        }
                        //3-Sua Senha é considerada fraca pelo sistema.\\nNão use caracteres iguais em sequência!
                        if(lResposta.Contains("3-Sua Senha é considerada fraca pelo sistema."))
                        {
                            lMensagem = String.Format("000{0}", lResposta);
                        }
                        //4-Sua Senha é considerada fraca pelo sistema.\nNão use sequência de caracteres!
                        if(lResposta.Contains("4-Sua Senha é considerada fraca pelo sistema."))
                        {
                            lMensagem = String.Format("000{0}", lResposta);
                        }
                        //5-Sua Senha é considerada fraca pelo sistema e/ou possui caracteres inválidos.\\nUse uma senha alfanumérica!
                        if(lResposta.Contains("5-Sua Senha é considerada fraca pelo sistema e/ou possui caracteres inválidos."))
                        {
                            lMensagem = String.Format("000{0}", lResposta);
                        }
                        //6-Sua Senha é considerada fraca pelo sistema.\\nSenha já utilizada anteriormente!
                        if(lResposta.Contains("6-Sua Senha é considerada fraca pelo sistema."))
                        {
                            lMensagem = String.Format("000{0}", lResposta);
                        }

                        throw new Exception(ERRONEGOCIO.ToString() + lMensagem + ERRONEGOCIO.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                throw new Exception(ERRONEGOCIO.ToString() + String.Format("{0} [{1}]", lMensagem, ex.Message) + ERRONEGOCIO.ToString());
            }

            return lRetorno;
        }

        public static SalvarEntidadeResponse<AlterarSenhaInfo> AtualizarSenha(SalvarObjetoRequest<AlterarSenhaInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_senha_upd_sp"))
                {
                    //Tratamento para ver se a senha atual está ok está na proc
                    pParametros.Objeto.CdSenhaNova = Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenhaNova);
                    pParametros.Objeto.CdSenhaAntiga = Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenhaAntiga);

                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_senhaantiga", DbType.String, pParametros.Objeto.CdSenhaAntiga);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_senhanova", DbType.String, pParametros.Objeto.CdSenhaNova);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                return new SalvarEntidadeResponse<AlterarSenhaInfo>();
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static SalvarEntidadeResponse<AlterarSenhaDinamicaInfo> AtualizarSenhaDinamica(SalvarObjetoRequest<AlterarSenhaDinamicaInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_senha_dinamica_upd_sp"))
                {
                    //Tratamento para ver se a senha atual está ok está na proc
                    pParametros.Objeto.CdSenhaNova          = Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenhaNova);
                    pParametros.Objeto.CdSenhaAntiga        = Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenhaAntiga);
                    if (pParametros.Objeto.SenhaDinamica != null)
                    {
                        pParametros.Objeto.SenhaDinamica = pParametros.Objeto.SenhaDinamica.SenhaHash();
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_1", DbType.String, pParametros.Objeto.SenhaDinamica.Caractere1.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_2", DbType.String, pParametros.Objeto.SenhaDinamica.Caractere2.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_3", DbType.String, pParametros.Objeto.SenhaDinamica.Caractere3.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_4", DbType.String, pParametros.Objeto.SenhaDinamica.Caractere4.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_5", DbType.String, pParametros.Objeto.SenhaDinamica.Caractere5.Opcao1);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_6", DbType.String, pParametros.Objeto.SenhaDinamica.Caractere6.Opcao1);
                    }
                    else
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_1", DbType.String, null);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_2", DbType.String, null);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_3", DbType.String, null);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_4", DbType.String, null);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_5", DbType.String, null);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_caractere_6", DbType.String, null);
                    }
                    
                    pParametros.Objeto.SenhaDinamicaNova    = pParametros.Objeto.SenhaDinamicaNova.SenhaHash();

                    lAcessaDados.AddInParameter(lDbCommand, "@id_login",                   DbType.Int32,  pParametros.Objeto.IdLogin);
	                lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_antiga",            DbType.String, pParametros.Objeto.CdSenhaAntiga);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_senha_nova",              DbType.String, pParametros.Objeto.CdSenhaNova);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_senha_caractere_1",  DbType.String, pParametros.Objeto.SenhaDinamicaNova.Caractere1.Opcao1);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_senha_caractere_2",  DbType.String, pParametros.Objeto.SenhaDinamicaNova.Caractere2.Opcao1);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_senha_caractere_3",  DbType.String, pParametros.Objeto.SenhaDinamicaNova.Caractere3.Opcao1);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_senha_caractere_4",  DbType.String, pParametros.Objeto.SenhaDinamicaNova.Caractere4.Opcao1);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_senha_caractere_5",  DbType.String, pParametros.Objeto.SenhaDinamicaNova.Caractere5.Opcao1);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_nova_senha_caractere_6",  DbType.String, pParametros.Objeto.SenhaDinamicaNova.Caractere6.Opcao1);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                return new SalvarEntidadeResponse<AlterarSenhaDinamicaInfo>();
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static SalvarEntidadeResponse<PrimeiroAcessoAtualizaInfo> AtualizarPrimeiroAcesso(SalvarObjetoRequest<PrimeiroAcessoAtualizaInfo> pParametros)
        {

            try
            {
                ValidaEmailJaExisteLogin(new LoginInfo() { IdLogin = pParametros.Objeto.IdLogin, DsEmail = pParametros.Objeto.DsEmail });

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_primeiroacesso_upd_sp"))
                {
                    //Tratamento de email duplicado na proc
                    pParametros.Objeto.CdSenha = Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenha);
                    pParametros.Objeto.CdAssinaturaEletronica = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaEletronica);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturaeletronica", DbType.String, pParametros.Objeto.CdAssinaturaEletronica);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_senha", DbType.String, pParametros.Objeto.CdSenha);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                return new SalvarEntidadeResponse<PrimeiroAcessoAtualizaInfo>();
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static ReceberObjetoResponse<EsqueciSenhaInfo> ReceberEsqueciSenha(ReceberEntidadeRequest<EsqueciSenhaInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<EsqueciSenhaInfo> lRetorno = new ReceberObjetoResponse<EsqueciSenhaInfo>();
                lRetorno.Objeto = pParametros.Objeto;

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_sel_esqueci_senha_sp"))
                {
                    string lSenha = GeradorSenhaAleatoria.GerarSenha();

                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_Senha", DbType.String, Crypto.CalculateMD5Hash(lSenha));
                    lAcessaDados.AddInParameter(lDbCommand, "@st_alteracao_funcionario", DbType.String, pParametros.Objeto.StAlteracaoFuncionario ? "1" : "0");

                    if (!pParametros.Objeto.StAlteracaoFuncionario)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@ds_CpfCnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                        lAcessaDados.AddInParameter(lDbCommand, "@dt_NascimentoFundacao", DbType.Date, pParametros.Objeto.DtNascimentoFundacao);
                    }

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    lRetorno.Objeto.CdSenha = lSenha;
                }
                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static ReceberObjetoResponse<EsqueciAssinaturaEletronicaInfo> ReceberEsqueciAssinaturaEletronica(ReceberEntidadeRequest<EsqueciAssinaturaEletronicaInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<EsqueciAssinaturaEletronicaInfo> lRetorno = new ReceberObjetoResponse<EsqueciAssinaturaEletronicaInfo>();
                lRetorno.Objeto = pParametros.Objeto;

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_sel_esqueci_assinatura_sp"))
                {

                    string lAssinatura = GeradorSenhaAleatoria.GerarSenha();

                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.dsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_CpfCnpj", DbType.String, pParametros.Objeto.dsCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_NascimentoFundacao", DbType.Date, pParametros.Objeto.dtNascimentoFundacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_AssinaturaEletronica", DbType.String, Crypto.CalculateMD5Hash(lAssinatura));
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    lRetorno.Objeto.cdAssinaturaEletronica = lAssinatura;
                }
                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static ReceberObjetoResponse<SolicitacaoNovaSenhaInfo> ReceberSolicitacaoNovaSenha(ReceberEntidadeRequest<SolicitacaoNovaSenhaInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<SolicitacaoNovaSenhaInfo> lRetorno = new ReceberObjetoResponse<SolicitacaoNovaSenhaInfo>();
                lRetorno.Objeto = pParametros.Objeto;

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_sel_solicitacao_senha_sp"))
                {
                    string lSenha = GeradorSenhaAleatoria.GerarSenha();

                    lAcessaDados.AddInParameter(lDbCommand  , "@ds_email"               , DbType.String , pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand  , "@cd_Senha"               , DbType.String , Crypto.CalculateMD5Hash(lSenha));
                    lAcessaDados.AddInParameter(lDbCommand  , "@ds_CpfCnpj"             , DbType.String , pParametros.Objeto.DsCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand  , "@dt_NascimentoFundacao"  , DbType.Date   , pParametros.Objeto.DtNascimentoFundacao);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    //if (null != lDataTable && lDataTable.Rows.Count > 0)
                        //lRetorno.Objeto.NrTentativasErradas = lDataTable.Rows[0]["nr_tentativaserradas"].DBToInt32();

                    lRetorno.Objeto.DsEmail             = lDataTable.Rows[0]["ds_email"].DBToString();
                    lRetorno.Objeto.DsCpfCnpj           = lDataTable.Rows[0]["ds_cpfcnpj"].DBToString();
                    lRetorno.Objeto.Login               = lDataTable.Rows[0]["id_login"].DBToString();
                    lRetorno.Objeto.DtNascimentoFundacao= lDataTable.Rows[0]["dt_nascimentofundacao"].DBToDateTime();
                    lRetorno.Objeto.CdSenha             = lDataTable.Rows[0]["cd_senha"].DBToString();
                    lRetorno.Objeto.DataHora            = DateTime.Now;
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static ReceberObjetoResponse<VerificaNomeInfo> ReceberNome(ReceberEntidadeRequest<VerificaNomeInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<VerificaNomeInfo> lRetorno = new ReceberObjetoResponse<VerificaNomeInfo>();
                lRetorno.Objeto = pParametros.Objeto;

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_sel_nome_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddOutParameter(lDbCommand, "@ds_nome", DbType.String, 60);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    lRetorno.Objeto.DsNome = lDbCommand.Parameters["@ds_nome"].Value.DBToString();
                }
                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static ReceberObjetoResponse<PrimeiroAcessoValidaCpfInfo> ReceberValidacaoCpfCnpjPrimeiroAcesso(ReceberEntidadeRequest<PrimeiroAcessoValidaCpfInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<PrimeiroAcessoValidaCpfInfo> lRetorno = new ReceberObjetoResponse<PrimeiroAcessoValidaCpfInfo>();
                lRetorno.Objeto = pParametros.Objeto;

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_primeiroacesso_sel_login_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_login", DbType.String, 32);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    lRetorno.Objeto.IdLogin = lDbCommand.Parameters["@id_login"].Value.DBToInt32();
                }
                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static ReceberObjetoResponse<PrimeiroAcessoValidaInfo> ReceberValidacaoPrimeiroAcesso(ReceberEntidadeRequest<PrimeiroAcessoValidaInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<PrimeiroAcessoValidaInfo> lRetorno = new ReceberObjetoResponse<PrimeiroAcessoValidaInfo>();
                lRetorno.Objeto = pParametros.Objeto;

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_primeiroacesso_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddOutParameter(lDbCommand, "@cd_senha", DbType.String, 32);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    string senha = lDbCommand.Parameters["@cd_senha"].Value.DBToString();
                    if (senha == ConfigurationManager.AppSettings["Migracao"])
                        lRetorno.Objeto.StPrimeiroAcesso = true;
                    else
                        lRetorno.Objeto.StPrimeiroAcesso = false;

                }
                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static ReceberObjetoResponse<LoginReceberQuantidadeTentativasErradasInfo> ReceberQuantidadeTentativasLoginErradas(ReceberEntidadeRequest<LoginReceberQuantidadeTentativasErradasInfo> pParametros)
        {
            try
            {
                var lRetorno = new ReceberObjetoResponse<LoginReceberQuantidadeTentativasErradasInfo>() { Objeto = new LoginReceberQuantidadeTentativasErradasInfo() };

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_nrtentativaserradas_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        lRetorno.Objeto.QtTentativasErradas = lDataTable.Rows[0][0].DBToInt32();
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static SalvarEntidadeResponse<LoginIncrementarTentativasErradasInfo> IncrementarTentativasErradasLogin(SalvarObjetoRequest<LoginIncrementarTentativasErradasInfo> pParametros)
        {
            try
            {
                var lRetorno = new SalvarEntidadeResponse<LoginIncrementarTentativasErradasInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_nrtentativaserradas_incremento_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static SalvarEntidadeResponse<LoginLiberarAcesoTentativasErradasInfo> LiberarAcesoTentativasErradasLogin(SalvarObjetoRequest<LoginLiberarAcesoTentativasErradasInfo> pParametros)
        {
            try
            {
                var lRetorno = new SalvarEntidadeResponse<LoginLiberarAcesoTentativasErradasInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_nrtentativaserradas_liberacaoacesso_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static ConsultarObjetosResponse<HistoricoSenhaInfo> ConsultarHistoricoSenhaPorCliente(ConsultarEntidadeRequest<HistoricoSenhaInfo> pParametros)
        {
            try
            {
                var lRetorno = new ConsultarObjetosResponse<HistoricoSenhaInfo>();
                lRetorno.Resultado = new List<HistoricoSenhaInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_historicosenha_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_senha", DbType.String, Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenha));

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                            lRetorno.Resultado.Add(new HistoricoSenhaInfo()
                            {
                                CdSenha = lDataTable.Rows[i]["cd_senha"].DBToString(),
                                DtAlteracao = lDataTable.Rows[i]["dt_alteracao"].DBToDateTime(),
                                IdHistoricoSenha = lDataTable.Rows[i]["id_historicosenha"].DBToInt32(),
                                IdLogin = lDataTable.Rows[i]["id_historicosenha"].DBToInt32(),
                            });
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        public static SalvarEntidadeResponse<HistoricoSenhaInfo> SalvarHistoricoSenha(SalvarObjetoRequest<HistoricoSenhaInfo> pParametros)
        {
            try
            {
                var lRetorno = new SalvarEntidadeResponse<HistoricoSenhaInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_historicosenha_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_senha", DbType.String, Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenha));

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
        }

        #endregion

        #region | LoginInfo

        public static SalvarEntidadeResponse<LoginInfo> SalvarLogin(DbTransaction pTrans, SalvarObjetoRequest<LoginInfo> pParametros, Boolean pCriptografar = true)
        {
            ValidaEmailJaExisteLogin(pParametros.Objeto);
            return Salvar(pTrans, pParametros, pCriptografar);
        }

        public static SalvarEntidadeResponse<LoginInfo> SalvarLogin(SalvarObjetoRequest<LoginInfo> pParametros)
        {
            ValidaEmailJaExisteLogin(pParametros.Objeto);
            if (pParametros.Objeto.IdLogin > 0)
                //alterar
                return Atualizar(pParametros);
            else
                //incluir
                return Salvar(pParametros);
        }

        public static ConsultarObjetosResponse<LoginInfo> ConsultarLogin(ConsultarEntidadeRequest<LoginInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<LoginInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_lst_sp"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroLogin(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static LoginInfo ReceberLogin(string DsCpfCnpj, DateTime DtNascimentoFundacao)
        {
            try
            {
                var lRetorno = new LoginInfo();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_cliente_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, DsCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimentofundacao", DbType.DateTime, DtNascimentoFundacao);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        lRetorno = CriarRegistroLogin(lDataTable.Rows[0]);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public static ReceberObjetoResponse<LoginInfo> ReceberLogin(ReceberEntidadeRequest<ClienteInfo> pParametros)
        {
            try
            {
                var lRetorno = new ReceberObjetoResponse<LoginInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_cliente_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_nascimentofundacao", DbType.DateTime, pParametros.Objeto.DtNascimentoFundacao);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        lRetorno.Objeto = CriarRegistroLogin(lDataTable.Rows[0]);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<LoginInfo> ReceberLogin(ReceberEntidadeRequest<LoginInfo> pParametros)
        {
            try
            {
                var lRetorno = new ReceberObjetoResponse<LoginInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        lRetorno.Objeto = CriarRegistroLogin(lDataTable.Rows[0]);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeRequest<LoginInfo> AtualizarEmail(DbTransaction pTrans, SalvarObjetoRequest<LoginInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ValidaEmailJaExisteLogin(pParametros.Objeto);

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "login_email_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int64, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);
                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                return new SalvarEntidadeRequest<LoginInfo>()
                {
                    Objeto = pParametros.Objeto,
                };
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<LoginInfo> Atualizar(SalvarObjetoRequest<LoginInfo> pParametros)
        {
            SalvarEntidadeResponse<LoginInfo> lRetorno;

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

        private static SalvarEntidadeResponse<LoginInfo> Atualizar(DbTransaction pTrans, SalvarObjetoRequest<LoginInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@nr_tentativaserradas", DbType.Int32, pParametros.Objeto.NrTentativasErradas);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_frase", DbType.Int32, pParametros.Objeto.IdFrase);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_respostafrase", DbType.String, pParametros.Objeto.DsRespostaFrase);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ultimaexpiracao", DbType.DateTime, pParametros.Objeto.DtUltimaExpiracao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_acesso", DbType.Int32, (int)pParametros.Objeto.TpAcesso);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int64, pParametros.Objeto.CdAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    //Atualização não altera a senha e assinatura eletrônica, devem ser criados métodos específicos para isto
                    //lAcessaDados.AddInParameter(lDbCommand, "@cd_senha", DbType.String, pParametros.Objeto.CdSenha);
                    //lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturaeletronica", DbType.String, pParametros.Objeto.CdAssinaturaEletronica);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);
                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                return new SalvarEntidadeResponse<LoginInfo>();
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Altera Senha e Assinatura e não criptografa
        /// </summary>
        /// <param name="pParametros">Login com a senha já criptografada</param>
        /// <returns></returns>
        public static SalvarEntidadeResponse<LoginInfo> AtualizarPorImportacao(SalvarObjetoRequest<LoginInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ValidaEmailJaExisteLogin(pParametros.Objeto);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_upd_import_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@nr_tentativaserradas", DbType.Int32, pParametros.Objeto.NrTentativasErradas);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_frase", DbType.Int32, pParametros.Objeto.IdFrase);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_respostafrase", DbType.String, pParametros.Objeto.DsRespostaFrase);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ultimaexpiracao", DbType.DateTime, pParametros.Objeto.DtUltimaExpiracao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_acesso", DbType.Int32, (int)pParametros.Objeto.TpAcesso);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int64, pParametros.Objeto.CdAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_senha", DbType.String, pParametros.Objeto.CdSenha);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturaeletronica", DbType.String, pParametros.Objeto.CdAssinaturaEletronica);
                    //lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                return new SalvarEntidadeResponse<LoginInfo>();
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<LoginInfo> Salvar(DbTransaction pTrans, SalvarObjetoRequest<LoginInfo> pParametros, Boolean pCriptografar = true)
        {
            try
            {
                var lSenhaDescrip = pParametros.Objeto.CdSenha;
                var lAssinaturaDescrip = pParametros.Objeto.CdAssinaturaEletronica;

                if (pCriptografar)
                {
                    pParametros.Objeto.CdSenha = Crypto.CalculateMD5Hash(pParametros.Objeto.CdSenha);
                    pParametros.Objeto.CdAssinaturaEletronica = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaEletronica);
                }

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "login_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_senha", DbType.AnsiString, pParametros.Objeto.CdSenha);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturaeletronica", DbType.AnsiString, pParametros.Objeto.CdAssinaturaEletronica);
                    lAcessaDados.AddInParameter(lDbCommand, "@nr_tentativaserradas", DbType.Int32, pParametros.Objeto.NrTentativasErradas);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_frase", DbType.Int32, pParametros.Objeto.IdFrase);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_respostafrase", DbType.String, pParametros.Objeto.DsRespostaFrase);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ultimaexpiracao", DbType.DateTime, pParametros.Objeto.DtUltimaExpiracao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.AnsiString, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_acesso", DbType.Int32, (int)pParametros.Objeto.TpAcesso);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int64, pParametros.Objeto.CdAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_login", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    SalvarEntidadeResponse<LoginInfo> response = new SalvarEntidadeResponse<LoginInfo>()
                    {
                        Codigo = lDbCommand.Parameters["@id_login"].Value.DBToInt32()
                    };

                    response.Objeto = pParametros.Objeto;

                    pParametros.Objeto.CdAssinaturaEletronica = lAssinaturaDescrip;
                    pParametros.Objeto.IdLogin = lDbCommand.Parameters["@id_login"].Value.DBToInt32();
                    pParametros.Objeto.CdSenha = lSenhaDescrip;
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<LoginInfo> Salvar(SalvarObjetoRequest<LoginInfo> pParametros, Boolean pCriptografar = true)
        {
            ValidaEmailJaExisteLogin(pParametros.Objeto);

            SalvarEntidadeResponse<LoginInfo> lRetorno;

            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            try
            {
                lRetorno = Salvar(trans, pParametros, pCriptografar);
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

        public static RemoverObjetoResponse<LoginInfo> RemoverLogin(RemoverEntidadeRequest<LoginInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                var response = new RemoverEntidadeResponse<LoginInfo>()
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

        private static void LogarModificacao(LoginInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<LoginInfo> lEntrada = new ReceberEntidadeRequest<LoginInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<LoginInfo> lRetorno = ReceberLogin(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

        #endregion

        #region | Métodos Apoio

        private static LoginInfo CriarRegistroLogin(DataRow linha)
        {

            LoginInfo lRetorno = new LoginInfo();

            lRetorno.CdAssinaturaEletronica = linha["cd_assinaturaeletronica"].DBToString();
            lRetorno.CdSenha = linha["cd_senha"].DBToString();
            lRetorno.DsEmail = linha["ds_email"].DBToString();
            lRetorno.DsRespostaFrase = linha["ds_respostafrase"].DBToString();
            lRetorno.DtUltimaExpiracao = linha["dt_ultimaexpiracao"].DBToDateTime();
            lRetorno.IdFrase = linha["id_frase"].DBToInt32();
            lRetorno.IdLogin = linha["id_login"].DBToInt32();
            lRetorno.NrTentativasErradas = linha["nr_tentativaserradas"].DBToInt32();
            lRetorno.CdAssessor = ((DBNull.Value == linha["cd_assessor"]) ? new Nullable<int>() : linha["cd_assessor"].DBToInt32());
            lRetorno.TpAcesso = (Contratos.Dados.Enumeradores.eTipoAcesso)linha["tp_acesso"].DBToInt32();
            lRetorno.DsNome = linha["ds_nome"].DBToString();

            try
            {
                if(linha["dt_ultimaalteracaosenha"] == DBNull.Value)
                {
                    lRetorno.DtUltimaAlteracaosSenha = DateTime.Now;
                }
                else
                {
                    lRetorno.DtUltimaAlteracaosSenha = linha["dt_ultimaalteracaosenha"].DBToDateTime();
                }

                if(linha["dt_ultimaalteracaoassinaturaeletronica"] == DBNull.Value)
                {
                    lRetorno.DtUltimaAlteracaoAssinaturaEletronica = DateTime.Now;
                }
                else
                {
                    lRetorno.DtUltimaAlteracaoAssinaturaEletronica = linha["dt_ultimaalteracaoassinaturaeletronica"].DBToDateTime();
                }
            }
            catch
            {
                lRetorno.DtUltimaAlteracaosSenha = DateTime.Now;
                lRetorno.DtUltimaAlteracaoAssinaturaEletronica = DateTime.Now;
            }

            return lRetorno;
        }

        private static void ValidaEmailJaExisteLogin(LoginInfo pLogin)
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
            int lRetorno = 0;
            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_verificaEmail_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pLogin.IdLogin);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pLogin.DsEmail);
                lAcessaDados.AddOutParameter(lDbCommand, "@count", DbType.Int32, 8);
                lAcessaDados.ExecuteNonQuery(lDbCommand);
                lRetorno = lDbCommand.Parameters["@count"].Value.DBToInt32();
            }

            if (lRetorno > 0)
                throw new Exception(gEmailJaCadastrado);

        }

        #endregion
    }
}
