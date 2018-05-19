using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Gradual.Generico.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.WsIntegracao.Arena.Models;
using WebAPI_Test.Models;

namespace Gradual.OMS.WsIntegracao.Arena.Services
{
    public class SegurancaServico : ServicoBase
    {
        #region Propriedades
        public IServicoSeguranca gServicoSeguranca;
        #endregion

        #region Construtores
        public SegurancaServico()
        {
            try
            {
                gServicoSeguranca = Ativador.Get<IServicoSeguranca>();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            
        }
        #endregion

        #region Métodos
        public SegurancaCliente EfetuarLogin(string pUsuario, string pEmail, string pIP, string pCodigoSistemaCliente, string pSenha)
        {
            var lRetorno = new SegurancaCliente();
            lRetorno.Criticas = new List<Criticas>();
            try
            {
                var lResponse = new AutenticarUsuarioResponse();
                
                var lRequest = new AutenticarUsuarioRequest()
                {
                    Email                = pUsuario, 
                    Senha                = Criptografia.CalculateMD5Hash(pSenha), 
                    IP                   = pIP,
                    CodigoSistemaCliente = pCodigoSistemaCliente
                };

                lResponse = gServicoSeguranca.AutenticarUsuario(lRequest);

                if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    var lCritica        = new Criticas();
                    lCritica.DataEvento = DateTime.Now;
                    lCritica.Descricao  = lResponse.DescricaoResposta;
                    lRetorno.Criticas.Add(lCritica);

                    lRetorno.StatusAutenticacaoRequisicao = false;
                    lRetorno.TokenClienteAutenticacao     = "";
                    lRetorno.IdClienteGradual             = 0;

                    return lRetorno;
                }

                var lRetornoSessao = gServicoSeguranca.ReceberSessao(new ReceberSessaoRequest()
                {
                    CodigoSessao          = lRetorno.TokenClienteAutenticacao,
                    CodigoSessaoARetornar = lRetorno.TokenClienteAutenticacao
                });

                lRetorno.StatusAutenticacaoRequisicao = true;
                lRetorno.TokenClienteAutenticacao     = lResponse.Sessao.CodigoSessao;
                lRetorno.ClienteAtributo              = ClienteContaServico.ConsultarClienteContaLogin(lResponse.Sessao.CodigoUsuario.DBToInt32());
                lRetorno.DataAutenticacao             = DateTime.Now;
                lRetorno.Usuario                      = pUsuario;
                lRetorno.IP                           = pIP;
                lRetorno.IdClienteGradual             = lRetorno.ClienteAtributo.ID;
                
                //lRetorno.DataUltimoLogin = lRetornoSessao.Usuario

                return lRetorno;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
            }

            return lRetorno;
        }

        public static AssinaturaEletronica ValidarAssinaturaEletronica(AssinaturaEletronica pParametros)
        {
            var lResposta = new AssinaturaEletronica();
            try
            {
                
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                var lAssinaturaEletronica = Criptografia.CalculateMD5Hash(pParametros.Assinatura);

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_sel_email_sp2"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_gradual",      DbType.String, pParametros.IdClienteGradual);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturaeletronica", DbType.String, lAssinaturaEletronica);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lResposta = new AssinaturaEletronica()
                        {
                            IdClienteGradual      = lDataTable.Rows[0]["id_cliente"].DBToInt32(),
                            Assinatura            = lDataTable.Rows[0]["cd_assinaturaeletronica"].DBToString(),
                            Valida                = true
                            
                            /*
                            CdAssessor             = lDataTable.Rows[0]["cd_assessor"].DBToInt32(),
                            CdAssinaturaEletronica = lDataTable.Rows[0]["cd_assinaturaeletronica"].DBToString(),
                            CdCodigo               = lDataTable.Rows[0]["CodigoCBLC"].DBToInt32(),
                            CdSenha                = lDataTable.Rows[0]["cd_assinaturaeletronica"].DBToString(),
                            DsEmail                = lDataTable.Rows[0]["ds_email"].DBToString(),
                            DsNome                 = lDataTable.Rows[0]["ds_nome"].DBToString(),
                            DsRespostaFrase        = lDataTable.Rows[0]["ds_respostafrase"].DBToString(),
                            DtUltimaExpiracao      = lDataTable.Rows[0]["dt_ultimaexpiracao"].DBToDateTime(),
                            IdFrase                = lDataTable.Rows[0]["id_frase"].DBToInt32(),
                            IdLogin                = lDataTable.Rows[0]["id_login"].DBToInt32(),
                            NrTentativasErradas    = lDataTable.Rows[0]["nr_tentativaserradas"].DBToInt32(),
                            TpAcesso               = (eTipoAcesso)lDataTable.Rows[0]["tp_acesso"].DBToInt32(),
                          * */
                        };
                    }
                    else
                    {
                        lResposta                       = pParametros;
                        lResposta.Assinatura            = "";
                        lResposta.Valida                = false;
                    }
                }

                //return lResposta;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);

                //throw ex;
            }

            return lResposta;
        }
        #endregion
    }
}