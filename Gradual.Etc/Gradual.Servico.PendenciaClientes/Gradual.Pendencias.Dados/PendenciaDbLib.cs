using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Pendencias.Entidades;

namespace Gradual.Pendencias.Dados
{
    public class PendenciaDbLib
    {
        #region | Métodos

        private List<AssessorInfo> MontarTodosAssessores()
        {
            List<AssessorInfo> lRetorno = new List<AssessorInfo>();

            //Pegando o Nome dos assessores no Oracle
            Hashtable htNomeAssessores = new Hashtable();
            ConexaoDbHelper lAcessaDadosOracle = new ConexaoDbHelper();

            lAcessaDadosOracle.ConnectionStringName = "SinacorConsulta";

            using (DbCommand lDbCommand = lAcessaDadosOracle.CreateCommand(CommandType.StoredProcedure, "prc_ListaComboSinacor"))
            {
                lAcessaDadosOracle.AddInParameter(lDbCommand, "Informacao", DbType.Int32, 18); // 18 = AssessorPadronizado
                DataTable lDataTable = lAcessaDadosOracle.ExecuteOracleDataTable(lDbCommand);

                foreach (DataRow item in lDataTable.Rows)
                    htNomeAssessores.Add(int.Parse(item["ID"].ToString()), item["Value"].ToString());
            }

            //Pegando o Assessor do SQL
            lRetorno = new List<AssessorInfo>();
            ConexaoDbHelper lAcessaDadosSql = new ConexaoDbHelper();

            lAcessaDadosSql.ConnectionStringName = "Cadastro";

            using (DbCommand lDbCommand = lAcessaDadosSql.CreateCommand(CommandType.StoredProcedure, "assessor_lst_sp"))
            {
                DataTable lDataTable = lAcessaDadosSql.ExecuteDbDataTable(lDbCommand);
                AssessorInfo lAssessor;

                foreach (DataRow item in lDataTable.Rows)
                {
                    lAssessor = new AssessorInfo();
                    lAssessor.IdAssessor = int.Parse(item["Id"].ToString());
                    lAssessor.EmailAssessor = item["Email"].ToString();

                    try // Pegar do Hash, pois veio do Oracle
                    {
                        lAssessor.NomeAssessor = htNomeAssessores[lAssessor.IdAssessor].ToString();
                    }
                    catch
                    {
                        lAssessor.NomeAssessor = string.Empty;
                    }

                    lRetorno.Add(lAssessor);
                }
            }

            return lRetorno;
        }

        private List<ClienteInfo> MontarTodosClientesComPendencias()
        {
            List<ClienteInfo> lRetorno = new List<ClienteInfo>();
            ConexaoDbHelper lAcessaDadosSql = new ConexaoDbHelper();

            lAcessaDadosSql.ConnectionStringName = "Cadastro";
            
            using (DbCommand lDbCommand = lAcessaDadosSql.CreateCommand(CommandType.StoredProcedure, "clientesPendencias_lst_sp"))
            {
                DataTable lDataTable = lAcessaDadosSql.ExecuteDbDataTable(lDbCommand);
                ClienteInfo ClienteAnterior = new ClienteInfo() { CpfCnpjCliente = "-1" };

                foreach (DataRow item in lDataTable.Rows)
                {
                    if (item["cpfcnpj"].ToString() != ClienteAnterior.CpfCnpjCliente) // Apenas para Novo cliente, adicionar cliente
                    {
                        if (ClienteAnterior.CpfCnpjCliente != "-1") // Não pegar antes do primeiro
                            lRetorno.Add(ClienteAnterior);         // Salva o antigo

                        ClienteAnterior = new ClienteInfo();
                        ClienteAnterior.Pendencias = new List<PendenciaInfo>();
                        //Dados do cliente
                        ClienteAnterior.CodigoBovespa = item["codigo"].ToString();
                        ClienteAnterior.CpfCnpjCliente = item["cpfcnpj"].ToString();
                        ClienteAnterior.NomeCliente = item["nome"].ToString();
                        ClienteAnterior.EmailCliente = item["email"].ToString();
                        ClienteAnterior.IdAssessor = int.Parse(item["assessor"].ToString());
                    }

                    ClienteAnterior.Pendencias.Add(new PendenciaInfo()
                    {   //--> Para novos e continuação, adicionar pendencia
                        PendenciaTipo = item["pendencia"].ToString(),
                        PendenciaDescricao = item["pendenciaDescricao"].ToString()
                    }); 
                }
                lRetorno.Add(ClienteAnterior);
            }

            return lRetorno;
        }

        public List<AssessorInfo> GetPendencias()
        {
            List<AssessorInfo> lTodosAssessores = this.MontarTodosAssessores();
            List<ClienteInfo> lClientesComPendencias = this.MontarTodosClientesComPendencias();
            List<AssessorInfo> lRetorno = new List<AssessorInfo>();

            foreach (AssessorInfo itemAssessor in lTodosAssessores)
            {
                itemAssessor.Clientes = new List<ClienteInfo>();

                foreach (ClienteInfo itemCliente in lClientesComPendencias)
                    if (itemAssessor.IdAssessor == itemCliente.IdAssessor)
                        itemAssessor.Clientes.Add(itemCliente);

                if (itemAssessor.Clientes.Count > 0)
                    lRetorno.Add(itemAssessor);
            }

            return lRetorno;
        }

        #endregion
    }
}
