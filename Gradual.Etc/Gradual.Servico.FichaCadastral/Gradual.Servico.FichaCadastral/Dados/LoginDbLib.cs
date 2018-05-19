using System;
using System.Data;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class LoginDbLib : DbLibBase
    {
        public ReceberObjetoResponse<LoginInfo> ReceberLogin(ReceberEntidadeRequest<LoginInfo> pParametros)
        {
            try
            {
                var lRetorno = new ReceberObjetoResponse<LoginInfo>();
                var lAcessaDados = new AcessaDados();

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
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        private static LoginInfo CriarRegistroLogin(DataRow linha)
        {
            return new LoginInfo()
            {
                CdAssinaturaEletronica = linha["cd_assinaturaeletronica"].DBToString(),
                CdSenha = linha["cd_senha"].DBToString(),
                DsEmail = linha["ds_email"].DBToString(),
                DsRespostaFrase = linha["ds_respostafrase"].DBToString(),
                DtUltimaExpiracao = linha["dt_ultimaexpiracao"].DBToDateTime(),
                IdFrase = linha["id_frase"].DBToInt32(),
                IdLogin = linha["id_login"].DBToInt32(),
                NrTentativasErradas = linha["nr_tentativaserradas"].DBToInt32(),
                CdAssessor = ((DBNull.Value == linha["cd_assessor"]) ? new Nullable<int>() : linha["cd_assessor"].DBToInt32()),
                TpAcesso = (Gradual.Intranet.Contratos.Dados.Enumeradores.eTipoAcesso)linha["tp_acesso"].DBToInt32(),
                DsNome = linha["ds_nome"].DBToString()
            };
        }
    }
}
