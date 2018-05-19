using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using Gradual.MinhaConta.Entidade;

namespace Gradual.MinhaConta.Dados
{
    public class DFundos : DBase
    {
        public void AtualizarFundos(List<EFundos> lstFundos)
        {
            //EFundos _objEntidade = null;
            AcessaDados.ConnectionStringName = base.ConexaoOMS;

            foreach (EFundos eFundos in lstFundos)
            {
                using (DbCommand dbCommand = AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_atualiza_fundos"))
                {
                    AcessaDados.AddInParameter(dbCommand, "@ds_cpfcgc", DbType.String, eFundos.CpfCnpj);
                    AcessaDados.AddInParameter(dbCommand, "@cd_carteira", DbType.Int64, eFundos.Carteira);
                    AcessaDados.AddInParameter(dbCommand, "@id_cliente", DbType.Int64, eFundos.Cliente);
                    AcessaDados.AddInParameter(dbCommand, "@ds_nome_fundo", DbType.String, eFundos.NomeFundo);
                    AcessaDados.AddInParameter(dbCommand, "@ds_nome_cliente", DbType.String, eFundos.NomeCliente);
                    AcessaDados.AddInParameter(dbCommand, "@vl_cota", DbType.Decimal, eFundos.Cota);
                    AcessaDados.AddInParameter(dbCommand, "@vl_quantidade", DbType.Decimal, eFundos.Quantidade);
                    AcessaDados.AddInParameter(dbCommand, "@vl_bruto", DbType.Decimal, eFundos.ValorBruto);
                    AcessaDados.AddInParameter(dbCommand, "@vl_ir", DbType.Decimal, eFundos.IR);
                    AcessaDados.AddInParameter(dbCommand, "@vl_iof", DbType.Int64, eFundos.IOF);
                    AcessaDados.AddInParameter(dbCommand, "@vl_liquido", DbType.Decimal, eFundos.ValorLiquido);
                    //AcessaDados.AddInParameter(dbCommand, "DataAtu"         , DbType.DateTime, eFundos.DataAtu);
                    AcessaDados.ExecuteNonQuery(dbCommand);
                }
            }
        }

        public BindingList<EFundos> ConsultarFundos(string _CPFCNPJ)
        {
            base.AcessaDados.ConnectionStringName = base.ConexaoOMS;
            var fundos = new BindingList<EFundos>();
            var fundo = new EFundos();
            var dados = new DataTable();

            using (var _DbCommand = AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_consulta_fundos"))
            {
                AcessaDados.AddInParameter(_DbCommand, "@ds_cpfcgc", DbType.String, _CPFCNPJ.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace(@"\", string.Empty));

                dados = AcessaDados.ExecuteOracleDataTable(_DbCommand);
            }
            
            foreach (DataRow item in dados.Rows)
            {
                fundo = new EFundos();
                fundo.CpfCnpj = item["CPFCGC"].DBToString();
                fundo.Carteira = item["CARTEIRA"].DBToInt32();
                fundo.Cliente = item["CLIENTE"].DBToInt32();
                fundo.NomeFundo = item["NOMEFUNDO"].DBToString();
                fundo.NomeCliente = item["NOMECLIENTE"].DBToString();
                fundo.Cota = item["COTA"].DBToDecimal();
                fundo.Quantidade = item["QUANTIDADE"].DBToDecimal();
                fundo.ValorBruto = item["VALORBRUTO"].DBToDecimal();
                fundo.IR = item["IR"].DBToDecimal();
                fundo.IOF = item["IOF"].DBToDecimal();
                fundo.ValorLiquido = item["VALORLIQUIDO"].DBToDecimal();
                fundo.DataAtu = item["DATAATU"].DBToDateTime();
                fundos.Add(fundo);
            }
            return fundos;
        }
    }
}