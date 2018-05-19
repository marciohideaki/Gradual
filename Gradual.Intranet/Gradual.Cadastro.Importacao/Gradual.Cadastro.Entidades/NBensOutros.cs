using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;
using System.ComponentModel;
using System.Data.Common;
using System.Data;

namespace Gradual.Cadastro.Negocios
{
    public class NBensOutros
    {
        /// <summary>
        /// Lista todos os Bens Outros de um determinado Cliente
        /// </summary>
        /// <param name="IdCliente">Id do Cliente</param>
        /// <returns>Lista com todos os Bens Outros do cliente passado como parâmetro</returns>
        public BindingList<EBensOutros> Listar(int IdCliente)
        {
            try
            {
                BindingList<EBensOutros> lstBensOutros = new BindingList<EBensOutros>();
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_BensOutros ");
                sbSQL.Append(",ID_Cliente ");
                sbSQL.Append(",Tipo ");
                sbSQL.Append(",Descricao ");
                sbSQL.Append(",Valor ");
                sbSQL.Append("FROM BensOutros ");
                sbSQL.Append("where ID_Cliente = " + IdCliente.ToString());

                DbCommand cmd = acessaBD.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable tbBensOutros = acessaBD.ExecuteDbDataTable(cmd);

                foreach (DataRow item in tbBensOutros.Rows)
                {
                    EBensOutros eBensOutros = new EBensOutros();
                    eBensOutros.ID_BensOutros = Conversao.ToInt(item["ID_BensOutros"]).Value;
                    eBensOutros.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]).Value;
                    eBensOutros.Tipo = Conversao.ToInt(item["Tipo"]).Value;
                    eBensOutros.Descricao = Conversao.ToString(item["Descricao"]);
                    eBensOutros.Valor = Conversao.ToDecimal(item["Valor"]).Value;
                    lstBensOutros.Add(eBensOutros);
                }

                return lstBensOutros;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Seleciona um Bem
        /// </summary>
        /// <param name="IdBensOutros">Id do Bem</param>
        /// <returns>Entidade contendo todos os dados do Bem passado como parâmetro</returns>
        public EBensOutros Selecionar(int IdBensOutros)
        {
            try
            {
                EBensOutros eBensOutros = new EBensOutros();
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_BensOutros ");
                sbSQL.Append(",ID_Cliente ");
                sbSQL.Append(",Tipo ");
                sbSQL.Append(",Descricao ");
                sbSQL.Append(",Valor ");
                sbSQL.Append("FROM BensOutros ");
                sbSQL.Append("where ID_BensOutros = " + IdBensOutros.ToString());

                DbCommand cmd = acessaBD.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable tbBensOutros = acessaBD.ExecuteDbDataTable(cmd);

                if (tbBensOutros.Rows.Count > 0)
                {
                    eBensOutros.ID_BensOutros = Conversao.ToInt(tbBensOutros.Rows[0]["ID_BensOutros"]).Value;
                    eBensOutros.ID_Cliente = Conversao.ToInt(tbBensOutros.Rows[0]["ID_Cliente"]).Value;
                    eBensOutros.Tipo = Conversao.ToInt(tbBensOutros.Rows[0]["Tipo"]).Value;
                    eBensOutros.Descricao = Conversao.ToString(tbBensOutros.Rows[0]["Descricao"]);
                    eBensOutros.Valor = Conversao.ToDecimal(tbBensOutros.Rows[0]["Valor"]).Value;
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return eBensOutros;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inseri um Bem
        /// </summary>
        /// <param name="eBensOutros">Entidade contendo todos os dados do Bem a ser inserido</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Inserir(EBensOutros eBensOutros)
        {
            try
            {
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" INSERT INTO BensOutros ");
                sbSQL.Append(" (ID_BensOutros ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,Tipo ");
                sbSQL.Append(" ,Descricao ");
                sbSQL.Append(" ,Valor) ");
                sbSQL.Append(" VALUES ");
                sbSQL.Append(" ( seqBensOutros.nextval ");
                sbSQL.Append(" ," + eBensOutros.ID_Cliente.ToString());
                sbSQL.Append(" ," + eBensOutros.Tipo.ToString());
                sbSQL.Append(" , '" + eBensOutros.Descricao + "'");
                sbSQL.Append(" ," + Conversao.ToDecimalOracle(eBensOutros.Valor) + ") ");

                DbCommand cmd = acessaBD.CreateCommand(CommandType.Text, sbSQL.ToString());

                return acessaBD.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Altera um Bem
        /// </summary>
        /// <param name="eBensOutros">Entidade contendo todos os dados do Bem a ser alterado</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Alterar(EBensOutros eBensOutros)
        {
            try
            {
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" UPDATE BensOutros ");
                sbSQL.Append(" SET ");
                sbSQL.Append(" Tipo = " + eBensOutros.Tipo.ToString());
                sbSQL.Append(" ,Descricao = '" + eBensOutros.Descricao + "'");
                sbSQL.Append(" ,Valor = " + Conversao.ToDecimalOracle(eBensOutros.Valor));
                sbSQL.Append("  WHERE ID_BensOutros = " + eBensOutros.ID_BensOutros.ToString());

                DbCommand cmd = acessaBD.CreateCommand(CommandType.Text, sbSQL.ToString());

                return acessaBD.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui um Bem
        /// </summary>
        /// <param name="IdBensOutros">Id do Bem a ser excluido</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Excluir(int IdBensOutros)
        {
            try
            {
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" delete from BensOutros ");
                sbSQL.Append("  WHERE ID_BensOutros = " + IdBensOutros.ToString());

                DbCommand cmd = acessaBD.CreateCommand(CommandType.Text, sbSQL.ToString());

                return acessaBD.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
