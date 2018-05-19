using System;
using System.Data;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Negocio;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        public static ReceberObjetoResponse<LoginAssinaturaEletronicaInfo> ReceberLoginAssinaturaEletronica(ReceberEntidadeRequest<LoginAssinaturaEletronicaInfo> pParametros)
        {
            try
            {
                var resposta = new ReceberObjetoResponse<LoginAssinaturaEletronicaInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                resposta.Objeto = new LoginAssinaturaEletronicaInfo();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;


                pParametros.Objeto.CdAssinaturaEletronica = Crypto.CalculateMD5Hash(pParametros.Objeto.CdAssinaturaEletronica);

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_sel_email_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.Objeto.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assinaturaeletronica", DbType.String, pParametros.Objeto.CdAssinaturaEletronica);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_senha", DbType.String, pParametros.Objeto.CdSenha);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);

                    if (!0.Equals(pParametros.Objeto.CdCodigo))
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.Int32, pParametros.Objeto.CdCodigo);
                    
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        resposta.Objeto = new LoginAssinaturaEletronicaInfo()
                        {
                             CdAssessor = lDataTable.Rows[0]["cd_assessor"].DBToInt32(),
                             CdAssinaturaEletronica = lDataTable.Rows[0]["cd_assinaturaeletronica"].DBToString(),
                             CdCodigo = lDataTable.Rows[0]["CodigoCBLC"].DBToInt32(),
                             CdSenha = lDataTable.Rows[0]["cd_assinaturaeletronica"].DBToString(),
                             DsEmail = lDataTable.Rows[0]["ds_email"].DBToString(),
                             DsNome = lDataTable.Rows[0]["ds_nome"].DBToString(),
                             DsRespostaFrase = lDataTable.Rows[0]["ds_respostafrase"].DBToString(),
                             DtUltimaExpiracao = lDataTable.Rows[0]["dt_ultimaexpiracao"].DBToDateTime(),
                             IdFrase = lDataTable.Rows[0]["id_frase"].DBToInt32(),
                             IdLogin = lDataTable.Rows[0]["id_login"].DBToInt32(),
                             NrTentativasErradas = lDataTable.Rows[0]["nr_tentativaserradas"].DBToInt32(),
                             TpAcesso = (eTipoAcesso)lDataTable.Rows[0]["tp_acesso"].DBToInt32(),
                        };
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }
    }
}
