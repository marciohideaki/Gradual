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
    public class NBensImoveis
    {
        /// <summary>
        /// Lista todos os BensImoveis de um determinado Cliente
        /// </summary>
        /// <param name="IdCliente">Id do Cliente</param>
        /// <returns>Lista contendo todos os BensImoveis do cliente selecionado</returns>
        public BindingList<EBensImoveis> Listar(int IdCliente)
        {
            try
            {
                BindingList<EBensImoveis> lstBensImoveis = new BindingList<EBensImoveis>();
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_BensImoveis ");
                sbSQL.Append(",ID_Cliente ");
                sbSQL.Append(",Tipo ");
                sbSQL.Append(",Endereco ");
                sbSQL.Append(",Cidade ");
                sbSQL.Append(",UF ");
                sbSQL.Append(",Valor ");
                sbSQL.Append("FROM BensImoveis ");
                sbSQL.Append("where ID_Cliente = " + IdCliente.ToString());

                DbCommand cmd = acessaBD.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable tbBensImoveis = acessaBD.ExecuteDbDataTable(cmd);

                foreach (DataRow item in tbBensImoveis.Rows)
                {
                    EBensImoveis eBensImoveis = new EBensImoveis();
                    eBensImoveis.ID_BensImoveis = Conversao.ToInt(item["ID_BensImoveis"]).Value;
                    eBensImoveis.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]).Value;
                    eBensImoveis.Tipo = Conversao.ToInt(item["Tipo"]).Value;
                    eBensImoveis.Endereco = Conversao.ToString(item["Endereco"]);
                    eBensImoveis.Cidade = Conversao.ToString(item["Cidade"]);
                    eBensImoveis.UF = Conversao.ToString(item["UF"]);
                    eBensImoveis.Valor = Conversao.ToDecimal(item["Valor"]).Value;
                    lstBensImoveis.Add(eBensImoveis);
                }

                return lstBensImoveis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Seleciona um Bem Imóvel
        /// </summary>
        /// <param name="IdBensImoveis">Id do Bem</param>
        /// <returns>Entidade contendo todos os dados do Bem passado como parâmetro</returns>
        public EBensImoveis Selecionar(int IdBensImoveis)
        {
            try
            {
                EBensImoveis eBensImoveis = new EBensImoveis();
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_BensImoveis ");
                sbSQL.Append(",ID_Cliente ");
                sbSQL.Append(",Tipo ");
                sbSQL.Append(",Endereco ");
                sbSQL.Append(",Cidade ");
                sbSQL.Append(",UF ");
                sbSQL.Append(",Valor ");
                sbSQL.Append("FROM BensImoveis ");
                sbSQL.Append("where ID_BensImoveis = " + IdBensImoveis.ToString());

                DbCommand cmd = acessaBD.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable tbBensImoveis = acessaBD.ExecuteDbDataTable(cmd);

                if (tbBensImoveis.Rows.Count > 0)
                {
                    eBensImoveis.ID_BensImoveis = Conversao.ToInt(tbBensImoveis.Rows[0]["ID_BensImoveis"]).Value;
                    eBensImoveis.ID_Cliente = Conversao.ToInt(tbBensImoveis.Rows[0]["ID_Cliente"]).Value;
                    eBensImoveis.Tipo = Conversao.ToInt(tbBensImoveis.Rows[0]["Tipo"]).Value;
                    eBensImoveis.Endereco = Conversao.ToString(tbBensImoveis.Rows[0]["Endereco"]);
                    eBensImoveis.Cidade = Conversao.ToString(tbBensImoveis.Rows[0]["Cidade"]);
                    eBensImoveis.UF = Conversao.ToString(tbBensImoveis.Rows[0]["UF"]);
                    eBensImoveis.Valor = Conversao.ToDecimal(tbBensImoveis.Rows[0]["Valor"]).Value;
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return eBensImoveis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inseri um Bem Imóvel
        /// </summary>
        /// <param name="eBensImoveis">Entidade contendo todos os dados do Bem Imóvel a ser inserido</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Inserir(EBensImoveis eBensImoveis)
        {
            try
            {
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" INSERT INTO BensImoveis ");
                sbSQL.Append(" (ID_BensImoveis ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,Tipo ");
                sbSQL.Append(" ,Endereco ");
                sbSQL.Append(" ,Cidade ");
                sbSQL.Append(" ,UF ");
                sbSQL.Append(" ,Valor) ");
                sbSQL.Append(" VALUES ");
                sbSQL.Append(" ( seqBensImoveis.nextval ");
                sbSQL.Append(" ," + eBensImoveis.ID_Cliente.ToString());
                sbSQL.Append(" ," + eBensImoveis.Tipo.ToString());
                sbSQL.Append(" , '" + eBensImoveis.Endereco + "'");
                sbSQL.Append(" , '" + eBensImoveis.Cidade + "'");
                sbSQL.Append(" , '" + eBensImoveis.UF + "'");
                sbSQL.Append(" ," + Conversao.ToDecimalOracle(eBensImoveis.Valor) + ") ");

                DbCommand cmd = acessaBD.CreateCommand(CommandType.Text, sbSQL.ToString());

                return acessaBD.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Altera um Bem Imóvel
        /// </summary>
        /// <param name="eBensImoveis">Entidade contendo todos os dados do Bem Imóvel a ser alterado</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Alterar(EBensImoveis eBensImoveis)
        {
            try
            {
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" UPDATE BensImoveis ");
                sbSQL.Append(" SET ");
                sbSQL.Append(" Tipo = " + eBensImoveis.Tipo.ToString());
                sbSQL.Append(" ,Endereco = '" + eBensImoveis.Endereco + "'");
                sbSQL.Append(" ,Cidade = '" + eBensImoveis.Cidade + "'");
                sbSQL.Append(" ,UF = '" + eBensImoveis.UF + "'");
                sbSQL.Append(" ,Valor = " + Conversao.ToDecimalOracle(eBensImoveis.Valor));
                sbSQL.Append("  WHERE ID_BensImoveis = " + eBensImoveis.ID_BensImoveis.ToString());

                DbCommand cmd = acessaBD.CreateCommand(CommandType.Text, sbSQL.ToString());

                return acessaBD.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui um Bem Imóvel
        /// </summary>
        /// <param name="IdBensImoveis">Id do Bem Imóvel a ser Excluido</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Excluir(int IdBensImoveis)
        {
            try
            {
                AcessaDadosAntigo acessaBD = new AcessaDadosAntigo();
                acessaBD.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" delete from BensImoveis ");
                sbSQL.Append("  WHERE ID_BensImoveis = " + IdBensImoveis.ToString());

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
