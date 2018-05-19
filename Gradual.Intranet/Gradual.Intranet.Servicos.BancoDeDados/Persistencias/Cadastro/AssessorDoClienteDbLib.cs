using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using System.Collections.Generic;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ReceberObjetoResponse<AssessorDoClienteInfo> ReceberAssessorDoCliente(ReceberEntidadeRequest<AssessorDoClienteInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<AssessorDoClienteInfo> lResposta = new ReceberObjetoResponse<AssessorDoClienteInfo>();

                lResposta.Objeto = new AssessorDoClienteInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "assessor_do_cliente_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lResposta.Objeto.IdCliente = pParametros.Objeto.IdCliente;
                        lResposta.Objeto.EmailAssessor = lDataTable.Rows[0]["ds_email"].DBToString();
                        lResposta.Objeto.CodigoAssessor = lDataTable.Rows[0]["cd_assessor"].DBToInt32();
                        lResposta.Objeto.NomeAssessor = GetNomeAssessorOracle(lResposta.Objeto.CodigoAssessor);
                    }
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ListaEmailAssessorInfo> ConsultarListaEmailAssessor(ReceberEntidadeRequest<ListaEmailAssessorInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<ListaEmailAssessorInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno.Objeto = new ListaEmailAssessorInfo();
            lRetorno.Objeto.ListaEmailAssessor = new System.Collections.Generic.List<string>();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarEmailAssessor_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, pParametros.Objeto.IdAssessor);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Objeto.ListaEmailAssessor.Add(lLinha["ds_email"].DBToString());
            }

            return lRetorno;
        }

        public static ReceberObjetoResponse<ListaAssessoresVinculadosInfo> ReceberListaCodigoAssessoresVinculados(ReceberEntidadeRequest<ListaAssessoresVinculadosInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<ListaAssessoresVinculadosInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno.Objeto = new ListaAssessoresVinculadosInfo();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarAssessoresVinculados_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, pParametros.Objeto.ConsultaCodigoAssessor);

                if (pParametros.Objeto.CodigoLogin.HasValue)
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_login", DbType.Int32, pParametros.Objeto.CodigoLogin);
                }

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Objeto.ListaCodigoAssessoresVinculados.Add(lLinha["cd_assessor"].DBToInt32());
            }

            return lRetorno;
        }

        public static List<int> ReceberListaClientesAssessoresVinculados(int CodigoAssessor)
        {
            var lRetorno = new List<int>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarClientesAssessoresVinculadosRisco_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, CodigoAssessor);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Add(lLinha["cd_codigo"].DBToInt32());
            }

            return lRetorno;
        }

        public static List<ClienteResumidoInfo> ReceberListaClientesAssessoresVinculadosComCPF(int CodigoAssessor)
        {
            var lRetorno = new List<ClienteResumidoInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarClientesAssessoresVinculadosRisco_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, CodigoAssessor);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0) 
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        var lCliente = new ClienteResumidoInfo();

                        lCliente.CodBovespa  = lLinha["cd_codigo"].ToString();
                        lCliente.NomeCliente = lLinha["ds_nome"].ToString();
                        lCliente.CPF         = lLinha["ds_cpfcnpj"].ToString();
                        lCliente.CodAssessor = lLinha["cd_assessor"].DBToInt32();

                        lRetorno.Add(lCliente);
                    }
            }

            return lRetorno;
        }

        public static string GetNomeAssessorOracle(int pIdAssessor)
        {
            var lAssessor = ConsultarListaComboSinacor(new ConsultarEntidadeRequest<SinacorListaComboInfo>()
            {
                Objeto = new SinacorListaComboInfo()
                {
                    Informacao = Contratos.Dados.Enumeradores.eInformacao.Assessor,
                    Filtro = pIdAssessor.ToString()
                }
            });

            //Tratemento para Retirar o Código
            string lRetorno = "";
            string[] lNomeAssessor = lAssessor.Resultado[0].Value.Split('-');
            for (int i = 0; i < lNomeAssessor.Length; i++)
            {
                if (i > 0)
                {
                    lRetorno += lNomeAssessor[i];
                }
            }
            return lRetorno.Trim().Replace("  ", " - ");

        }

        public static List<ClienteResumidoInfo> ReceberListaClientesDoAssessor(int CodigoAssessor)
        {
            var lRetorno     = new List<ClienteResumidoInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarClientesAssessoresVinculadosRisco_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "cd_assessor", DbType.Int32, CodigoAssessor);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        var lCliente = new ClienteResumidoInfo();

                        lCliente.CodBovespa  = lLinha["cd_codigo"].ToString();
                        lCliente.NomeCliente = lLinha["ds_nome"].ToString();
                        lCliente.CPF         = lLinha["ds_cpfcnpj"].ToString();
                        lCliente.CodAssessor = lLinha["cd_assessor"].DBToInt32();

                        lRetorno.Add(lCliente);
                    }
                }
            }

            return lRetorno;
        }

        public static List<ClienteResumidoInfo> ReceberListaClientesDoAssessor(int CodigoAssessor, Nullable<int> CodigoLogin)
        {
            var lRetorno = new List<ClienteResumidoInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarClientesDadosBasicosAssessoresVinculados_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "id_assessor", DbType.Int32, CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "cd_login", DbType.Int32, CodigoLogin);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        var lCliente = new ClienteResumidoInfo();

                        lCliente.CodBovespa  = lLinha["cd_codigo"].ToString();
                        lCliente.NomeCliente = lLinha["ds_nome"].ToString();
                        lCliente.CPF         = lLinha["ds_cpfcnpj"].ToString();
                        lCliente.CodAssessor = lLinha["cd_assessor"].DBToInt32();
                        lCliente.Email       = lLinha["ds_email"].ToString();

                        lRetorno.Add(lCliente);
                    }
                }
            }

            return lRetorno;
        }

    }
}
