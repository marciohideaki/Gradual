using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.MinhaConta.Entidade;

namespace Gradual.MinhaConta.Dados
{
    public class DClubes : DBase
    {
        public List<EClubes> ConsultarClubes(string CpfCnpj)
        {
            var eClubes = new EClubes();
            var dataTable = new DataTable();
            var clubeLista = new List<EClubes>();

            base.AcessaDados.ConnectionStringName = base.ConexaoOMS;

            using (var dbCommand = base.AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_consulta_clubes"))
            {
                base.AcessaDados.AddInParameter(dbCommand, "@cpfcgc", DbType.String, CpfCnpj);

                dataTable = base.AcessaDados.ExecuteOracleDataTable(dbCommand);
            }

            foreach (DataRow item in dataTable.Rows)
            {
                eClubes = new EClubes();
                eClubes.Nome_do_Clube = item["Nome_do_Clube"].DBToString();
                eClubes.Codigo_da_Empresa = item["Codigo_da_Empresa"].DBToInt32();
                eClubes.Codigo_Bolsa = item["Codigo_Bolsa"].DBToInt32();
                eClubes.Data = item["Data"].DBToDateTime();
                eClubes.Cotacao = item["Cotacao"].DBToDecimal();
                eClubes.Codigo_Cliente = item["CODIGO_DO_CLIENTE"].DBToInt32();
                eClubes.Nome_do_Cliente = item["Nome_do_Cliente"].DBToString();
                eClubes.Dcquantidade = item["Dcquantidade"].DBToInt32();
                eClubes.Dccotacao = item["Dccotacao"].DBToInt32();
                eClubes.Data_Inicial = item["Data_Inicial"].DBToDateTime();
                eClubes.Saldo_Quantidade = item["Saldo_Quantidade"].DBToDecimal();
                eClubes.Saldo_Bruto = item["Saldo_Bruto"].DBToDecimal();
                eClubes.IR = item["IR"].DBToDecimal();
                eClubes.IOF = item["IOF"].DBToDecimal();
                eClubes.Rendimento = item["Rendimento"].DBToDecimal();
                eClubes.Performance = item["Performance"].DBToDecimal();
                eClubes.Saldo_Liquido = item["Saldo_Liquido"].DBToDecimal();
                eClubes.Saldo_Inicial = item["Saldo_Inicial"].DBToDecimal();
                eClubes.Codigo_do_Agente = item["Codigo_do_Agente"].DBToInt32();
                eClubes.Nome_do_Agente = item["Nome_do_Agente"].DBToString();
                eClubes.CPF_CGC = item["CPFCGC"].DBToString();
                eClubes.Tipo = item["Tipo"].DBToInt32();
                eClubes.Data_Atualizacao = item["Data_Atualizacao"].DBToDateTime();
                clubeLista.Add(eClubes);
            }

            return clubeLista;
        }

        public void AtualizarClubes(List<EClubes> ClubesLista)
        {
            //EFundos _objEntidade = null;
            AcessaDados.ConnectionStringName = base.ConexaoOMS;
            DbCommand dbCommand;
            foreach (EClubes eClubes in ClubesLista)
            {
                using (dbCommand = AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_atualiza_clubes"))
                {
                    base.AcessaDados.AddInParameter(dbCommand, "@ds_cpfcgc", DbType.String, string.IsNullOrEmpty(eClubes.CPF_CGC) ? string.Empty : eClubes.CPF_CGC.Replace(".", "").Replace("/", "").Replace("-", "").Replace(@"\", ""));
                    base.AcessaDados.AddInParameter(dbCommand, "@dt_data", DbType.DateTime, eClubes.Data);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_cotacao", DbType.Decimal, eClubes.Cotacao);
                    base.AcessaDados.AddInParameter(dbCommand, "@cd_empresa", DbType.Int64, eClubes.Codigo_da_Empresa);
                    base.AcessaDados.AddInParameter(dbCommand, "@ds_nome_clube", DbType.String, eClubes.Nome_do_Clube);
                    base.AcessaDados.AddInParameter(dbCommand, "@cd_agente", DbType.Int64, eClubes.Codigo_do_Agente);
                    base.AcessaDados.AddInParameter(dbCommand, "@cd_cliente", DbType.Int64, eClubes.Codigo_Cliente);
                    base.AcessaDados.AddInParameter(dbCommand, "@ds_nome_cliente", DbType.String, eClubes.Nome_do_Cliente);
                    base.AcessaDados.AddInParameter(dbCommand, "@dt_inicial", DbType.DateTime, eClubes.Data_Inicial);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_saldo_inicial", DbType.Decimal, eClubes.Saldo_Inicial);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_saldo_quantidade", DbType.Decimal, eClubes.Saldo_Quantidade);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_saldo_bruto", DbType.Decimal, eClubes.Saldo_Bruto);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_ir", DbType.Decimal, eClubes.IR);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_iof", DbType.Decimal, eClubes.IOF);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_performance", DbType.Decimal, eClubes.Performance);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_rendimento", DbType.Decimal, eClubes.Rendimento);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_saldo_liquido", DbType.Decimal, eClubes.Saldo_Liquido);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_dcquantidade", DbType.Int64, eClubes.Dcquantidade);
                    base.AcessaDados.AddInParameter(dbCommand, "@vl_dccotacao", DbType.Int64, eClubes.Dccotacao);
                    base.AcessaDados.AddInParameter(dbCommand, "@ds_nome_agente", DbType.String, eClubes.Nome_do_Agente);
                    base.AcessaDados.AddInParameter(dbCommand, "@cd_bolsa", DbType.Int64, eClubes.Codigo_Bolsa);
                    base.AcessaDados.AddInParameter(dbCommand, "@tp_tipo", DbType.String, eClubes.Tipo);

                    base.AcessaDados.ExecuteNonQuery(dbCommand);
                }
            }
        }

        public string GetCpf(string Codigo)
        {
            using (var acessaDados = new Gradual.Generico.Dados.AcessaDados())
            {
                acessaDados.ConnectionStringName = base.ConexaoTrade;

                try
                {
                    using (DbCommand dbCommand = acessaDados.CreateCommand(CommandType.StoredProcedure, "prc_get_cpf_by_codigo"))
                    {
                        acessaDados.AddInParameter(dbCommand, "pCodigo", DbType.Int64, Codigo.DBToInt64());
                        acessaDados.AddOutParameter(dbCommand, "pCPF", DbType.Int64, 8);
                        acessaDados.ExecuteNonQuery(dbCommand);
                        return dbCommand.Parameters["pCPF"].Value.ToString();
                    }
                }
                catch (System.Data.OracleClient.OracleException) { return "00000"; }
            }
        }
    }
}
