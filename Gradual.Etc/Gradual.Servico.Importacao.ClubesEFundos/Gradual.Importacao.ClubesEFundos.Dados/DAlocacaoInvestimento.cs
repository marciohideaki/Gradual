using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using Gradual.MinhaConta.Entidade;

namespace Gradual.MinhaConta.Dados
{
    public class DAlocacaoInvestimento : DBase
    {
        /// <summary>
        /// Lista a Alocação de investimentos de um cliente específico
        /// </summary>
        /// <param name="cd_cliente">Códigos do cliente</param>
        /// <param name="cpf">CPFCGC</param>
        /// <returns>Retorna uma lista com os dados consolidados dos clientes filtrados pelos códigos dos assessores</returns>
        public BindingList<EAlocacaoInvestimentos> ListarAlocacaoInvestimentos(Int64 cd_cliente, Int64 cpf)
        {
            EAlocacaoInvestimentos _objEntidade = null;
            BindingList<EAlocacaoInvestimentos> lstAlocacao = new BindingList<EAlocacaoInvestimentos>();

            AcessaDados.ConnectionStringName = base.ConexaoSinacor;

            DbCommand _DbCommand = AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ALOCACAO_INVEST_SEL");
            AcessaDados.AddInParameter(_DbCommand, "cd_cliente", DbType.Int64, cd_cliente);
            AcessaDados.AddInParameter(_DbCommand, "cpf", DbType.Int64, cpf);
            
            AcessaDados.CursorRetorno = "ACCOUNT";

            DataTable dtDados = AcessaDados.ExecuteOracleDataTable(_DbCommand);

            foreach (DataRow item in dtDados.Rows)
            {
                _objEntidade                   = new EAlocacaoInvestimentos();

                _objEntidade.Alocacao          = item["alocacao"].DBToString();
                _objEntidade.ContaDeposito     = item["ContaDeposito"].DBToDecimal();
                _objEntidade.ContaInvestimento = item["ContaInvestimento"].DBToDecimal();
                _objEntidade.Total             = _objEntidade.ContaDeposito + _objEntidade.ContaInvestimento;
                lstAlocacao.Add(_objEntidade);
            }

            return lstAlocacao;
        }
    }
}
