using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        #region | CRUD

        /// <summary>
        /// Lista os clientes cadastrados em um certo período
        /// </summary>
        /// <param name="pParametros">Entidade do tipo ClienteCadastradoPeriodoInfo</param>
        /// <returns>Retorna uma lista de clientes cadastrados em um certo período</returns>
        public static ConsultarObjetosResponse<ClienteCadastradoPeriodoInfo> ConsultarClienteCadastradoPeriodo(ConsultarEntidadeRequest<ClienteCadastradoPeriodoInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteCadastradoPeriodoInfo> lResposta = 
                new ConsultarObjetosResponse<ClienteCadastradoPeriodoInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_cliente_cadastrado_periodo_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, pParametros.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte", DbType.DateTime, pParametros.Objeto.DtAte);
                lAcessaDados.AddInParameter(lDbCommand, "@cpfcnpj", DbType.AnsiString, pParametros.Objeto.DsCpfCnpj);
                lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa", DbType.AnsiString, pParametros.Objeto.TipoPessoa);

                if (!0.Equals(pParametros.Objeto.CodigoAssessor))
                    lAcessaDados.AddInParameter(lDbCommand, "@codigoAssessor", DbType.Int32, pParametros.Objeto.CodigoAssessor);

                lDbCommand.CommandTimeout = 200;
                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroClienteCadastradoPeriodo(lDataTable.Rows[i]));
                
            }
            return lResposta;
        }

        #endregion

        #region | Métodos Apoio

        /// <summary>
        /// Método de apoio para criação/preenchimento de entidade do tipo ClienteCadastradoPeriodoInfo
        /// </summary>
        /// <param name="linha">DataRow do relatório de clientes cadastrados por período</param>
        /// <returns>Retorna uma entidade do tipo ClienteCadastradoPeriodoInfo preenchida</returns>
        private static ClienteCadastradoPeriodoInfo CriarRegistroClienteCadastradoPeriodo(DataRow linha)
        {
            return new ClienteCadastradoPeriodoInfo()
            {
                IdCliente           = linha["id_cliente"].DBToInt32(),
                DsNomeCliente       = linha["ds_nome"].DBToString(),
                DsCpfCnpj           = linha["ds_cpfcnpj"].DBToString(),
                BlnExportado        = Convert.ToBoolean(linha["blnExportado"]),
                CodigoAssessor      = linha["cd_assessor"].DBToInt32(),
                TipoPessoa          = linha["tp_pessoa"].DBToString(),
                DtCadastro          = linha["dtcadastro"].DBToDateTime(),
                DsTelefone          = linha["ds_telefone"].DBToString(),
                DsDDD               = linha["ds_ddd"].DBToString(),
                DsRamal             = linha["ds_ramal"].DBToString(),
                DtUltimaAtualizacao = linha["dt_ultimaatualizacao"].DBToDateTime(),
                CodigoBmf           = linha["cd_bmf"].DBToInt32(),
                CodigoBovespa       = linha["cd_bovespa"].DBToInt32(),
                DsEmail             = linha["ds_email"].DBToString(),
                PassoAtual          = linha["st_passo"].DBToString()
                , DsDesejaOperarEm  = linha["tp_deseja_aplicar"].DBToString()
		        , CodigoTipoOperacao= linha["CodigoTipoOperacaoCliente"].DBToString()
            };
        }

        #endregion
    }
}
