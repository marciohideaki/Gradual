using System;
using System.Collections.Generic;
using System.Data;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Text;
using System.ComponentModel;
using Gradual.Generico.Geral;

namespace Gradual.Cadastro.Negocios
{
    public class NPendencia
    {
        /// <summary>
        /// Verifica se um cliente possui pendencia cadastral
        /// </summary>
        /// <param name="IdCliente">Id do Cliente</param>
        /// <returns>Retorna True se existir alguma pendência e False se não existir nenhuma pendência cadastral</returns>
        public bool PendenciaCadastral(int IdCliente)
        {

            try
            {
                EPendencia _EPendencia = new EPendencia();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT * ");    
                sbSQL.Append(" FROM Pendencia ");
                sbSQL.Append(" where  ID_Cliente = " + IdCliente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EPendencia.CertidaoCasamento = Conversao.ToChar(_table.Rows[0]["CertidaoCasamento"]).Value;
                    _EPendencia.ComprovanteEndereco = Conversao.ToChar(_table.Rows[0]["ComprovanteEndereco"]).Value;
                    _EPendencia.ComprovanteRenda = Conversao.ToChar(_table.Rows[0]["ComprovanteRenda"]).Value;
                    _EPendencia.CPF = Conversao.ToChar(_table.Rows[0]["CPF"]).Value;
                    _EPendencia.Documento = Conversao.ToChar(_table.Rows[0]["Documento"]).Value;
                    _EPendencia.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _EPendencia.ID_Pendencia = Conversao.ToInt(_table.Rows[0]["ID_Pendencia"]).Value;
                    _EPendencia.Procuracao = Conversao.ToChar(_table.Rows[0]["Procuracao"]).Value;

                    _EPendencia.Contrato = Conversao.ToChar(_table.Rows[0]["Contrato"]).Value;
                    _EPendencia.DataCadastro = Conversao.ToDateTime(_table.Rows[0]["DataCadastro"]).Value;
                    _EPendencia.DataResolucao = Conversao.ToDateTime(_table.Rows[0]["DataResolucao"]);
                    _EPendencia.Descricao = Conversao.ToString(_table.Rows[0]["Descricao"]);
                    _EPendencia.Serasa = Conversao.ToChar(_table.Rows[0]["Serasa"]).Value;
                }


                if ( _EPendencia.CertidaoCasamento == 'S'   ||
                     _EPendencia.ComprovanteEndereco == 'S' ||
                     _EPendencia.ComprovanteRenda == 'S'    ||
                     _EPendencia.CPF == 'S'                 ||
                     _EPendencia.Documento == 'S'           ||
                     _EPendencia.Serasa == 'S'   )       
                    
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        /// <summary>
        /// Lista as pendências cadastrais de um cliente
        /// </summary>
        /// <param name="_ID_Cliente">Id do Cliente</param>
        /// <returns>Entidade contendo as pendências do cliente</returns>
        public EPendencia Listar(int _ID_Cliente)
        {
            try
            {
                EPendencia _EPendencia = new EPendencia();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Pendencia ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,Documento ");
                sbSQL.Append(" ,CPF ");
                sbSQL.Append(" ,CertidaoCasamento ");
                sbSQL.Append(" ,ComprovanteEndereco ");
                sbSQL.Append(" ,Procuracao ");
                sbSQL.Append(" ,ComprovanteRenda ");
                sbSQL.Append(" ,WTR ");
                sbSQL.Append(" ,Contrato  ");
                sbSQL.Append(" ,DataCadastro  ");
                sbSQL.Append(" ,DataResolucao  ");
                sbSQL.Append(" ,Descricao  ");
                sbSQL.Append(" ,Serasa  ");
                sbSQL.Append(" FROM Pendencia ");
                sbSQL.Append(" where  ID_Cliente = " + _ID_Cliente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EPendencia.CertidaoCasamento = Conversao.ToChar(_table.Rows[0]["CertidaoCasamento"]).Value;
                    _EPendencia.ComprovanteEndereco = Conversao.ToChar(_table.Rows[0]["ComprovanteEndereco"]).Value;
                    _EPendencia.ComprovanteRenda = Conversao.ToChar(_table.Rows[0]["ComprovanteRenda"]).Value;
                    _EPendencia.CPF = Conversao.ToChar(_table.Rows[0]["CPF"]).Value;
                    _EPendencia.Documento = Conversao.ToChar(_table.Rows[0]["Documento"]).Value;
                    _EPendencia.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _EPendencia.ID_Pendencia = Conversao.ToInt(_table.Rows[0]["ID_Pendencia"]).Value;
                    _EPendencia.Procuracao = Conversao.ToChar(_table.Rows[0]["Procuracao"]).Value;
                    _EPendencia.WTR = Conversao.ToChar(_table.Rows[0]["WTR"]).Value;
                    _EPendencia.Contrato = Conversao.ToChar(_table.Rows[0]["Contrato"]).Value;
                    _EPendencia.DataCadastro = Conversao.ToDateTime(_table.Rows[0]["DataCadastro"]).Value;
                    _EPendencia.DataResolucao = Conversao.ToDateTime(_table.Rows[0]["DataResolucao"]);
                    _EPendencia.Descricao = Conversao.ToString(_table.Rows[0]["Descricao"]);
                    _EPendencia.Serasa = Conversao.ToChar(_table.Rows[0]["Serasa"]).Value;
                }

                return _EPendencia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Seleciona uma pendência cadastral
        /// </summary>
        /// <param name="_ID_Pendencia">Id da Pendência Cadastral</param>
        /// <returns>Entidade contendo a pendência cadastral</returns>
        public EPendencia Selecionar(int _ID_Pendencia)
        {
            try
            {
                EPendencia _EPendencia = new EPendencia();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Pendencia ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,Documento ");
                sbSQL.Append(" ,CPF ");
                sbSQL.Append(" ,CertidaoCasamento ");
                sbSQL.Append(" ,ComprovanteEndereco ");
                sbSQL.Append(" ,Procuracao ");
                sbSQL.Append(" ,ComprovanteRenda ");
                sbSQL.Append(" ,Contrato  ");
                sbSQL.Append(" ,DataCadastro  ");
                sbSQL.Append(" ,DataResolucao  ");
                sbSQL.Append(" ,Descricao  ");
                sbSQL.Append(" ,Serasa  ");
                sbSQL.Append(" FROM Pendencia ");
                sbSQL.Append(" where  ID_Pendencia = " + _ID_Pendencia.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {

                    _EPendencia.CertidaoCasamento = Conversao.ToChar(_table.Rows[0]["CertidaoCasamento"]).Value;
                    _EPendencia.ComprovanteEndereco = Conversao.ToChar(_table.Rows[0]["ComprovanteEndereco"]).Value;
                    _EPendencia.ComprovanteRenda = Conversao.ToChar(_table.Rows[0]["ComprovanteRenda"]).Value;
                    _EPendencia.CPF = Conversao.ToChar(_table.Rows[0]["CPF"]).Value;
                    _EPendencia.Documento = Conversao.ToChar(_table.Rows[0]["Documento"]).Value;
                    _EPendencia.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _EPendencia.ID_Pendencia = Conversao.ToInt(_table.Rows[0]["ID_Pendencia"]).Value;
                    _EPendencia.Procuracao = Conversao.ToChar(_table.Rows[0]["Procuracao"]).Value;
                    _EPendencia.Contrato = Conversao.ToChar(_table.Rows[0]["Contrato"]).Value;
                    _EPendencia.DataCadastro = Conversao.ToDateTime(_table.Rows[0]["DataCadastro"]).Value;
                    _EPendencia.DataResolucao = Conversao.ToDateTime(_table.Rows[0]["DataResolucao"]).Value;
                    _EPendencia.Descricao = Conversao.ToString(_table.Rows[0]["Descricao"]);
                    _EPendencia.Serasa = Conversao.ToChar(_table.Rows[0]["Serasa"]).Value;
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _EPendencia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Altera uma pendência Cadastral
        /// </summary>
        /// <param name="pendencia">Entidade contendo todos os dados da Pendência cadastral a ser alterada</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Alterar(EPendencia pendencia)
        {
            try
            {

                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                //verifica se existe registro para fazer alteração ou inclusão 
                DbCommand _DbCommandCount = _AcessaDados.CreateCommand(CommandType.Text, "select count(*) from pendencia where id_cliente=" + pendencia.ID_Cliente.ToString());
                int qtd = Conversao.ToInt( _AcessaDados.ExecuteScalar(_DbCommandCount)).Value;


                StringBuilder sbSQL = new StringBuilder();

                if (qtd > 0)
                {
                    //alterar
                    sbSQL.Append(" UPDATE Pendencia set ");
                    sbSQL.Append(" WTR = '" + pendencia.WTR.ToString() + "'");
                    sbSQL.Append(" ,Documento = '" + pendencia.Documento.ToString() + "'");
                    sbSQL.Append(" ,CPF = '" + pendencia.CPF.ToString() + "'");
                    sbSQL.Append(" ,CertidaoCasamento = '" + pendencia.CertidaoCasamento.ToString() + "'");
                    sbSQL.Append(" ,ComprovanteEndereco = '" + pendencia.ComprovanteEndereco.ToString() + "'");
                    sbSQL.Append(" ,Procuracao = '" + pendencia.Procuracao.ToString() + "'");
                    sbSQL.Append(" ,ComprovanteRenda = '" + pendencia.ComprovanteRenda.ToString() + "'");
                    sbSQL.Append(" ,Contrato = '" + pendencia.Contrato.ToString() + "'");
                    sbSQL.Append(" ,DataCadastro = '" + pendencia.DataCadastro.ToString("dd-MMM-yy") + "'");
                    if (pendencia.DataResolucao != null)
                        sbSQL.Append(" ,DataResolucao = '" + pendencia.DataResolucao.Value.ToString("dd-MMM-yy") + "'");
                    else
                        sbSQL.Append(" ,DataResolucao = NULL ");
                    sbSQL.Append(" ,Descricao  = '" + pendencia.Descricao + "'");
                    sbSQL.Append(" ,Serasa  = '" + pendencia.Serasa + "'");
                    sbSQL.Append(" WHERE ID_Cliente = " + pendencia.ID_Cliente.ToString());
                }
                else { 
                //inclui
                    sbSQL.Append(" insert into Pendencia ( ");
                    sbSQL.Append(" id_pendencia ");
                    sbSQL.Append(" ,WTR ");
                    sbSQL.Append(" ,Documento ");
                    sbSQL.Append(" ,CPF ");
                    sbSQL.Append(" ,CertidaoCasamento ");
                    sbSQL.Append(" ,ComprovanteEndereco ");
                    sbSQL.Append(" ,Procuracao ");
                    sbSQL.Append(" ,ComprovanteRenda ");
                    sbSQL.Append(" ,Contrato ");
                    sbSQL.Append(" ,DataCadastro ");
                    sbSQL.Append(" ,DataResolucao ");
                    sbSQL.Append(" ,Descricao  ");
                    sbSQL.Append(" ,Serasa  ");
                    sbSQL.Append(" ,ID_Cliente ) ");
                    sbSQL.Append(" values ( ");
                    sbSQL.Append(" seqPendencia.nextval, ");
                    sbSQL.Append("'" + pendencia.WTR.ToString() + "',");
                    sbSQL.Append("'" + pendencia.Documento.ToString() + "',");
                    sbSQL.Append("'" + pendencia.CPF.ToString() + "',");
                    sbSQL.Append("'" + pendencia.CertidaoCasamento.ToString() + "',");
                    sbSQL.Append("'" + pendencia.ComprovanteEndereco.ToString() + "',");
                    sbSQL.Append("'" + pendencia.Procuracao.ToString() + "',");
                    sbSQL.Append("'" + pendencia.ComprovanteRenda.ToString() + "',");
                    sbSQL.Append("'" + pendencia.Contrato.ToString() + "',");
                    sbSQL.Append("'" + pendencia.DataCadastro.ToString("dd-MMM-yy") + "',");
                    if (pendencia.DataResolucao != null)
                        sbSQL.Append("'" + pendencia.DataResolucao.Value.ToString("dd-MMM-yy") + "',");
                    else
                        sbSQL.Append(" NULL, ");
                    sbSQL.Append("'" + pendencia.Descricao + "',");
                    sbSQL.Append("'" + pendencia.Serasa + "',");
                    sbSQL.Append( pendencia.ID_Cliente.ToString());
                    sbSQL.Append(" ) ");
                }
                    
                    DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Exclui uma pendência cadastral
        /// </summary>
        /// <param name="_ID_Pendencia">Id da Pendência Cadastral</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Excluir(int _ID_Pendencia)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("delete from Pendencia ");
                sbSQL.Append("where ID_Pendencia = " + _ID_Pendencia.ToString());


                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
