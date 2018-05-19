using System;
using System.Data;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteTelefoneDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteTelefoneInfo> ConsultarClienteTelefone(ConsultarEntidadeRequest<ClienteTelefoneInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteTelefoneInfo> resposta = new ConsultarObjetosResponse<ClienteTelefoneInfo>();
            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_telefone_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente == 0 ? (object)System.DBNull.Value : pParametros.Objeto.IdCliente);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (null != lDataTable && lDataTable.Rows.Count.CompareTo(0).Equals(1))
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroTelefone(lDataTable.Rows[i]));
                }
                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteTelefoneInfo CriarRegistroTelefone(DataRow linha)
        {
            return new ClienteTelefoneInfo()
            {
                DsDdd = linha["ds_ddd"].DBToString(),
                DsNumero = linha["ds_numero"].DBToString(),
                DsRamal = linha["ds_ramal"].DBToString(),
                IdCliente = linha["id_cliente"].DBToInt32(),
                IdTelefone = linha["id_telefone"].DBToInt32(),
                IdTipoTelefone = linha["id_tipo_telefone"].DBToInt32(),
                StPrincipal = linha["st_principal"].DBToBoolean()
            };
        }
    }
}
