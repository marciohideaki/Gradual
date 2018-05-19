using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;



using System.Collections.Generic;namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {

        private struct AssessorAuxiliar
        {
            public int cd_assessor { get; set; }

            public string nm_assessor { get; set; }
        }

        private static List<AssessorAuxiliar> gListaAssessor = null;

        private static List<AssessorAuxiliar> ListaAssessores()
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            
            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            List<AssessorAuxiliar> lRetorno = new List<AssessorAuxiliar>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ListaComboSinacor"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "Informacao", DbType.Int32, 9);

                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        AssessorAuxiliar lAssessor = new AssessorAuxiliar();

                        lAssessor.cd_assessor = lDataTable.Rows[i]["id"].DBToInt32();
                        lAssessor.nm_assessor = lDataTable.Rows[i]["value"].DBToString();

                        lRetorno.Add(lAssessor);
                    }
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Lista o total de clientes cadastrados dos assessores em um certo período
        /// </summary>
        /// <param name="pParametros">Entidade do tipo TotalClienteCadastradoAssessorPeriodoInfo</param>
        /// <returns>Retorna uma lista de total de clientes cadastrados dos assessores em um certo período</returns>
        public static ConsultarObjetosResponse<TotalClienteCadastradoAssessorPeriodoInfo> ConsultarTotalClienteCadastradoAssessorPeriodo(ConsultarEntidadeRequest<TotalClienteCadastradoAssessorPeriodoInfo> pParametros)
        {
            ConsultarObjetosResponse<TotalClienteCadastradoAssessorPeriodoInfo> lResposta =
                new ConsultarObjetosResponse<TotalClienteCadastradoAssessorPeriodoInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_total_cliente_cad_assessor_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, pParametros.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte", DbType.DateTime, pParametros.Objeto.DtAte);

                if (!pParametros.Objeto.CodigoAssessor.Equals(0))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@codigoAssessor", DbType.Int32, pParametros.Objeto.CodigoAssessor);
                }

                if (gListaAssessor == null)
                {
                    gListaAssessor = ListaAssessores();
                }

                lDbCommand.CommandTimeout = 200;
                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        lResposta.Resultado.Add(CriarRegistroTotalClienteCadastradoAssessorPeriodo(lDataTable.Rows[i]));
                    }
                }
            }
            return lResposta;
        }

        #region | Métodos Apoio

        /// <summary>
        /// Método de apoio para criação/preenchimento de entidade do tipo ClienteCadastradoPeriodoInfo
        /// </summary>
        /// <param name="linha">DataRow do relatório de clientes cadastrados por período</param>
        /// <returns>Retorna uma entidade do tipo ClienteCadastradoPeriodoInfo preenchida</returns>
        private static TotalClienteCadastradoAssessorPeriodoInfo CriarRegistroTotalClienteCadastradoAssessorPeriodo(DataRow linha)
        {
            TotalClienteCadastradoAssessorPeriodoInfo lRetorno = new TotalClienteCadastradoAssessorPeriodoInfo();

            lRetorno.CodigoAssessor = linha["cd_assessor"].DBToInt32();

            lRetorno.DataCadastro   = linha["dtcadastro"].DBToString();

            lRetorno.TotalCliente   = linha["totalcliente"].DBToInt32();

            lRetorno.DsNomeAssessor = gListaAssessor.Find(assessor=> assessor.cd_assessor == lRetorno.CodigoAssessor).nm_assessor;

            return lRetorno;
        }

        #endregion

    }
}
