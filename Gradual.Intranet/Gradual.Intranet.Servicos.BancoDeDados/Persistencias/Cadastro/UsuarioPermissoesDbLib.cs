using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Cadastro
{
    /// <summary>
    /// Classe para acesso ao banco para gerenciar informações de usuários 
    /// e suas devidas permissões
    /// </summary>
    public class UsuarioPermissoesDbLib
    {
        #region Atributos
        /// <summary>
        /// Atributo com o nome da conexão que a classe irá usar para acessar o banco de controle de
        /// acesso
        /// </summary>
        private const string gNomeConexaoControleAcesso = "Seguranca";
        #endregion
        
        /// <summary>
        /// Método que busca no banco de dados informações de usuario vinculado com suas permissões
        /// </summary>
        /// <param name="pParametro">Parametro de request com o Código de usuário do para
        /// buscar suas permissões no banco de dados de controle de acesso</param>
        /// <returns>Retorna o objeto preechido com Código de usuário e a lista de permissões</returns>
        public ReceberUsuarioResponse ListarIntranetPermissoesUsuario(UsuarioPermissaoInfo pParametro)
        {
            var lRetorno = new ReceberUsuarioResponse();

            try
            {
                var lAcessaDados = new AcessaDados();
                
                lAcessaDados.ConnectionStringName = gNomeConexaoControleAcesso;

                lRetorno.Usuario = new UsuarioInfo();

                lRetorno.Usuario.CodigoUsuario = pParametro.CodigoUsuario.ToString();

                lRetorno.Usuario.Permissoes = new List<PermissaoAssociadaInfo>();

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_PermissoesPorUsuario_Intranet_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoUsuario", DbType.Int32, pParametro.CodigoUsuario);
                    
                    DataTable lTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lTable != null && lTable.Rows.Count > 0)
                    {
                        for (int i=0; i < lTable.Rows.Count; i++)
                        {
                            DataRow lRow = lTable.Rows[i];

                            var lPermissao = new PermissaoAssociadaInfo();

                            lPermissao.CodigoPermissao = lRow["CodigoPermissao"].DBToString();

                            lRetorno.Usuario.Permissoes.Add(lPermissao);
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
    }
}
