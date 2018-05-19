using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;

using System.Collections;
using System.Linq;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        #region | Propriedades

        private static Hashtable LstClientesHash = new Hashtable();

        #endregion

        #region | Métodos Servico



        public static ConsultarObjetosResponse<ClienteSemLoginInfo> ConsultarClienteSemLogin(ConsultarEntidadeRequest<ClienteSemLoginInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteSemLoginInfo> lResposta = new ConsultarObjetosResponse<ClienteSemLoginInfo>();

      
            int lIdCliente = default(int);
            DataTable lDataTable = new DataTable();
            DataTable lDataTableSql = new DataTable();
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();


            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_sem_login")) //"prc_sel_cliente_sem_email"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, pParametros.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte", DbType.DateTime, pParametros.Objeto.DtAte);
                lAcessaDados.AddInParameter(lDbCommand, "@CdAssessor", DbType.Int32, pParametros.Objeto.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@DsCpfCnpj", DbType.Int32, pParametros.Objeto.DsCpfCnpj);
                lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa", DbType.AnsiString, pParametros.Objeto.TipoPessoa);

                lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);
            }

            if (null != lDataTable && lDataTable.Rows.Count > 0)
                for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                {
                    lResposta.Resultado.Add(CriarRegistroClienteSemLogin(lDataTable.Rows[i], lIdCliente));
                }

            return lResposta;
        }

        #endregion

        #region | Métodos Apoio

        /// <summary>
        /// Método de apoio para criação/preenchimento de entidade do tipo ClienteSemEmailInfo
        /// </summary>
        /// <param name="linha">DataRow do relatório de clientes sem email</param>
        /// <returns>Retorna uma entidade do tipo ClienteSemEmailInfo preenchida</returns>
        private static ClienteSemLoginInfo CriarRegistroClienteSemLogin(DataRow linha, int pIdCliente)
        {
            return new ClienteSemLoginInfo()
            {
                IdCliente = linha["id_cliente"].DBToInt32(),
                DsNomeCliente = linha["ds_nome"].DBToString(),
                CodigoAssessor = linha["cd_assessor"].DBToInt32(),
                TipoPessoa = linha["TP_PESSOA"].DBToString(),
                DtCadastro = linha["dtcadastro"].DBToDateTime(),
                DsCpfCnpj = linha["ds_cpfcnpj"].DBToString(),
                CdBovespa = linha["cd_bovespa"].DBToString(),
                DsEmail = linha["ds_email"].DBToString()
            };
        }

        /// <summary>
        /// Método de apoio para preenchimento 
        /// </summary>
        /// <param name="pParametros"></param>
        private static void ListarClienteCadastradoPeriodo(ClienteSemLoginInfo pParametros)
        {
            LstClientesHash.Clear();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_cliente_cad_periodo_assessor_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, pParametros.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte", DbType.DateTime, pParametros.DtAte);
                lAcessaDados.AddInParameter(lDbCommand, "@CdAssessor", DbType.Int32, pParametros.CodigoAssessor);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        if (!LstClientesHash.Contains(lDataTable.Rows[i]["ds_cpfcnpj"].ToString().PadLeft(15, '0')))
                            LstClientesHash.Add(lDataTable.Rows[i]["ds_cpfcnpj"].ToString().PadLeft(15, '0'), lDataTable.Rows[i]["id_cliente"]);
                    }
            }
        }

        #endregion
    }
}
