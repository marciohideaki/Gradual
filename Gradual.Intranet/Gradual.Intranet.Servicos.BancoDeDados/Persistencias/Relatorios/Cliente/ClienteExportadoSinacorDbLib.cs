#region Includes
using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
#endregion

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        #region ConsultarClienteCadastradoPeriodo
        /// <summary>
        /// Relatório de clientes exportados para o sinacor
        /// </summary>
        /// <param name="pParametros">Entidade do tipo ClientesExportadosSinacorInfo</param>
        /// <returns>Retorna uma lista de clientes cadastrados e já exportados para o sinacor de acordo com o filtro</returns>
        public static ConsultarObjetosResponse<ClientesExportadosSinacorInfo> ConsultarClienteExportadoSinacor(ConsultarEntidadeRequest<ClientesExportadosSinacorInfo> pParametros)
        {
            ConsultarObjetosResponse<ClientesExportadosSinacorInfo> lResposta =
                new ConsultarObjetosResponse<ClientesExportadosSinacorInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_cliente_exportado_sinacor_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe",        DbType.DateTime, pParametros.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte",       DbType.DateTime, pParametros.Objeto.DtAte);
                lAcessaDados.AddInParameter(lDbCommand, "@CdAssessor",  DbType.Int32,    pParametros.Objeto.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@DsCpfCnpj",   DbType.String,   pParametros.Objeto.DsCpfCnpj.Trim());
                lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa",  DbType.String,   pParametros.Objeto.TipoPessoa);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroClienteExportadoSinacor(lDataTable.Rows[i]));

            }
            return lResposta;
        }
        #endregion

        #region | Métodos Apoio

        private static ClientesExportadosSinacorInfo CriarRegistroClienteExportadoSinacor(DataRow linha)
        {
            return new ClientesExportadosSinacorInfo()
            {
                IdCliente           = linha["id_cliente"].DBToInt32(),
                DsNomeCliente       = linha["ds_nome"].DBToString(),
                DsCpfCnpj           = linha["ds_cpfcnpj"].DBToString(),
                CodigoAssessor      = linha["cd_assessor"].DBToInt32(),
                TipoPessoa          = linha["tp_pessoa"].DBToString(),
                DtCadastro          = linha["dtcadastro"].DBToDateTime(),
                DsTelefone          = linha["ds_telefone"].DBToString(),
                DsDDD               = linha["ds_ddd"].DBToString(),
                DsRamal             = linha["ds_ramal"].DBToString(),
                DtPrimeiraExportacao = linha["Dt_PrimeiraExportacao"].DBToDateTime(),
                DtUltimaExportacao = linha["Dt_UltimaExportacao"].DBToDateTime(),
                CodigoBovespa       = linha["cd_codigo"].DBToInt32(),
                      };
        }

        #endregion
    }
}
