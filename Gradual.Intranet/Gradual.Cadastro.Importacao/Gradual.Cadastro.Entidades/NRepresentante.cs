using System;
using System.Collections.Generic;
using System.Data;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.ComponentModel;
using Gradual.Generico.Geral;
using System.Text;

namespace Gradual.Cadastro.Negocios
{
    public class NRepresentante{
        /// <summary>
        /// Método para listar o representante do cliente.
        /// </summary>
        /// <param name="Id_cliente">Código do cliente</param>
        /// <returns>Representante</returns>
        public ERepresentante Listar(int Id_cliente)
        {
            DataTable _DtDados;
            try
            {

                List<ERepresentante> lstRepresentante = new List<ERepresentante>();
                ERepresentante _Representante = new ERepresentante();

                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("select * from representante where id_cliente = "); 
                sbQuery.Append(Id_cliente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());

                _DtDados = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                int intX = 0;

                _Representante = new ERepresentante();

                //if (_DtDados.Rows.Count == 0)
                //{   //--> Esta exceção gera erro em execução e impede a ficha DUC de ser criada.
                //    throw new Exception(CFormatacao.REGISTRONAOENCONTRADO);
                //}
                //else
                if (_DtDados.Rows.Count > 0)
                {
                    _Representante.ID_Representante = Conversao.ToInt(_DtDados.Rows[intX]["ID_Representante"]);

                    _Representante.Email = Conversao.ToString(_DtDados.Rows[intX]["Email"]);
                    _Representante.Nome = Conversao.ToString(_DtDados.Rows[intX]["Nome"]);
                    _Representante.CPF = Conversao.ToString(_DtDados.Rows[intX]["CPF"]);
                    _Representante.DataNascimento = Conversao.ToDateTime(_DtDados.Rows[intX]["DataNascimento"]);
                    _Representante.Sexo = Conversao.ToChar(_DtDados.Rows[intX]["Sexo"]);
                    _Representante.Nacionalidade = Conversao.ToInt(_DtDados.Rows[intX]["Nacionalidade"]);
                    _Representante.UFNascimento = Conversao.ToString(_DtDados.Rows[intX]["UFNascimento"]);
                    _Representante.Naturalidade = Conversao.ToString(_DtDados.Rows[intX]["Naturalidade"]);
                    _Representante.EstadoCivil = Conversao.ToInt(_DtDados.Rows[intX]["EstadoCivil"]);
                    _Representante.Conjugue = Conversao.ToString(_DtDados.Rows[intX]["Conjugue"]);
                    _Representante.Profissao = Conversao.ToInt(_DtDados.Rows[intX]["Profissao"]);
                    _Representante.NomeMae = Conversao.ToString(_DtDados.Rows[intX]["NomeMae"]);
                    _Representante.NomePai = Conversao.ToString(_DtDados.Rows[intX]["NomePai"]);
                    _Representante.DDDTelefone = Conversao.ToString(_DtDados.Rows[intX]["DDDTelefone"]);
                    _Representante.Telefone = Conversao.ToString(_DtDados.Rows[intX]["Telefone"]);
                    _Representante.DDDCelular = Conversao.ToString(_DtDados.Rows[intX]["DDDCelular"]);
                    _Representante.Celular = Conversao.ToString(_DtDados.Rows[intX]["Celular"]);
                    _Representante.CEP = Conversao.ToString(_DtDados.Rows[intX]["CEP"]);
                    _Representante.Logradouro = Conversao.ToString(_DtDados.Rows[intX]["Logradouro"]);
                    _Representante.Numero = Conversao.ToString(_DtDados.Rows[intX]["Numero"]);
                    _Representante.Complemento = Conversao.ToString(_DtDados.Rows[intX]["Complemento"]);
                    _Representante.Bairro = Conversao.ToString(_DtDados.Rows[intX]["Bairro"]);
                    _Representante.Cidade = Conversao.ToString(_DtDados.Rows[intX]["Cidade"]);
                    _Representante.Estado = Conversao.ToString(_DtDados.Rows[intX]["Estado"]);
                    _Representante.Pais = Conversao.ToString(_DtDados.Rows[intX]["Pais"]);
                    _Representante.TipoDocumento = Conversao.ToString(_DtDados.Rows[intX]["TipoDocumento"]);
                    _Representante.OrgaoEmissorDocumento = Conversao.ToString(_DtDados.Rows[intX]["OrgaoEmissorDocumento"]);
                    _Representante.NumeroDocumento = Conversao.ToString(_DtDados.Rows[intX]["NumeroDocumento"]);
                    _Representante.DataEmissaoDocumento = Conversao.ToDateTime(_DtDados.Rows[intX]["DataEmissaoDocumento"]);
                    _Representante.UFEmissaoDocumento = Conversao.ToString(_DtDados.Rows[intX]["UFEmissaoDocumento"]);
                    _Representante.PaisNascimento = Conversao.ToString(_DtDados.Rows[intX]["PaisNascimento"]);
                    _Representante.PaisNascimento = Conversao.ToString(_DtDados.Rows[intX]["PaisNascimento"]);
                    _Representante.UFNascimentoEstrangeiro = Conversao.ToString(_DtDados.Rows[intX]["UFNascimentoEstrangeiro"]);
                    _Representante.SituacaoLegal = Conversao.ToInt32(_DtDados.Rows[intX]["SituacaoLegal"]);
                    
                    if (_DtDados.Rows.Count - 1 != intX) { }
                }
                return _Representante;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// Método para listar todos os representantes
        /// </summary>
        /// <returns>Representantes</returns>
        public BindingList<ERepresentante> Listar()
        {
            DataTable _DtDados;
            try
            {

                ERepresentante _Representante = new ERepresentante();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                BindingList<ERepresentante> lstRepresentante = new BindingList<ERepresentante>();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("select * from representante");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());

                _DtDados = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_DtDados.Rows.Count > 0)
                {
                    for (int intX = 0; intX <= _DtDados.Rows.Count - 1; intX++)
                    {
                        _Representante.ID_Representante = Conversao.ToInt(_DtDados.Rows[intX]["ID_Representante"]);

                        _Representante.Email = Conversao.ToString(_DtDados.Rows[intX]["Email"]);
                        _Representante.Nome = Conversao.ToString(_DtDados.Rows[intX]["Nome"]);
                        _Representante.CPF = Conversao.ToString(_DtDados.Rows[intX]["CPF"]);
                        _Representante.DataNascimento = Conversao.ToDateTime(_DtDados.Rows[intX]["DataNascimento"]);
                        _Representante.Sexo = Conversao.ToChar(_DtDados.Rows[intX]["Sexo"]);
                        _Representante.Nacionalidade = Conversao.ToInt(_DtDados.Rows[intX]["Nacionalidade"]);
                        _Representante.UFNascimento = Conversao.ToString(_DtDados.Rows[intX]["UFNascimento"]);
                        _Representante.Naturalidade = Conversao.ToString(_DtDados.Rows[intX]["Naturalidade"]);
                        _Representante.EstadoCivil = Conversao.ToChar(_DtDados.Rows[intX]["EstadoCivil"]);
                        _Representante.Conjugue = Conversao.ToString(_DtDados.Rows[intX]["Conjugue"]);
                        _Representante.Profissao = Conversao.ToInt(_DtDados.Rows[intX]["Profissao"]);
                        _Representante.NomeMae = Conversao.ToString(_DtDados.Rows[intX]["NomeMae"]);
                        _Representante.NomePai = Conversao.ToString(_DtDados.Rows[intX]["NomePai"]);
                        _Representante.DDDTelefone = Conversao.ToString(_DtDados.Rows[intX]["DDDTelefone"]);
                        _Representante.Telefone = Conversao.ToString(_DtDados.Rows[intX]["Telefone"]);
                        _Representante.DDDCelular = Conversao.ToString(_DtDados.Rows[intX]["DDDCelular"]);
                        _Representante.Celular = Conversao.ToString(_DtDados.Rows[intX]["Celular"]);
                        _Representante.TipoDocumento = Conversao.ToString(_DtDados.Rows[intX]["TipoDocumento"]);
                        _Representante.OrgaoEmissorDocumento = Conversao.ToString(_DtDados.Rows[intX]["OrgaoEmissorDocumento"]);
                        _Representante.NumeroDocumento = Conversao.ToString(_DtDados.Rows[intX]["NumeroDocumento"]);
                        _Representante.DataEmissaoDocumento = Conversao.ToDateTime(_DtDados.Rows[intX]["DataEmissaoDocumento"]);
                        _Representante.UFEmissaoDocumento = Conversao.ToString(_DtDados.Rows[intX]["UFEmissaoDocumento"]);
                        _Representante.PaisNascimento = Conversao.ToString(_DtDados.Rows[intX]["PaisNascimento"]);
                        _Representante.UFNascimentoEstrangeiro = Conversao.ToString(_DtDados.Rows[intX]["UFNascimentoEstrangeiro"]);
                        _Representante.SituacaoLegal = Conversao.ToInt32(_DtDados.Rows[intX]["SituacaoLegal"]);
                   
                        /*
                        _Representante.Bairro = Conversao.ToString(_DtDados.Rows[intX]["Bairro"]);
                        _Representante.CEP = Conversao.ToString(_DtDados.Rows[intX]["CEP"]);
                        _Representante.Cidade = Conversao.ToString(_DtDados.Rows[intX]["Cidade"]);
                        _Representante.Complemento = Conversao.ToString(_DtDados.Rows[intX]["Complemento"]);
                        _Representante.Estado = Conversao.ToString(_DtDados.Rows[intX]["Estado"]);
                        _Representante.Logradouro = Conversao.ToString(_DtDados.Rows[intX]["Logradouro"]);
                    
                        _Representante.Pais = Conversao.ToString(_DtDados.Rows[intX]["Pais"]);
                        _Representante.Numero = Conversao.ToString(_DtDados.Rows[intX]["Numero"]);
                        */
                    }
                }

                return lstRepresentante;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Método para cadastrar um representante
        /// </summary>
        /// <param name="representante">Dados do representante</param>
        /// <returns>Número de linhas afetadas</returns>
        public int Inserir(ERepresentante representante)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("INSERT INTO REPRESENTANTE ");
                sbSQL.Append("(ID_Representante, ");
                sbSQL.Append("ID_Cliente, ");
                sbSQL.Append("Email, ");
                sbSQL.Append("Nome, ");
                sbSQL.Append("CPF, ");
                sbSQL.Append("DataNascimento, ");
                sbSQL.Append("Sexo, ");
                sbSQL.Append("Nacionalidade, ");
                sbSQL.Append("UFNascimento, ");
                sbSQL.Append("EstadoCivil , ");
                sbSQL.Append("Conjugue, ");
                sbSQL.Append("Profissao , ");
                sbSQL.Append("NomePai , ");
                sbSQL.Append("NomeMae , ");
                sbSQL.Append("CEP, ");
                sbSQL.Append("Logradouro , ");
                sbSQL.Append("Numero , ");
                sbSQL.Append("Complemento , ");
                sbSQL.Append("Bairro , ");
                sbSQL.Append("Cidade , ");
                sbSQL.Append("Estado , ");
                sbSQL.Append("Pais , ");
                sbSQL.Append("DDDTelefone , ");
                sbSQL.Append("Telefone , ");
                sbSQL.Append("DDDCelular , ");
                sbSQL.Append("Celular , ");
                sbSQL.Append("TipoDocumento , ");
                sbSQL.Append("OrgaoEmissorDocumento , ");
                sbSQL.Append("NumeroDocumento , ");
                sbSQL.Append("DataEmissaoDocumento , ");
                sbSQL.Append("Naturalidade , ");
                sbSQL.Append("UFEmissaoDocumento,  ");
                sbSQL.Append("PaisNascimento,  ");
                sbSQL.Append("SituacaoLegal, ");
                sbSQL.Append("UFNascimentoEstrangeiro  ) ");
                sbSQL.Append("VALUES ( ");
                sbSQL.Append("seqRepresentante.nextval , ");
                sbSQL.Append("" + representante.ID_Cliente + ",");
                sbSQL.Append("'" + representante.Email + "',");
                sbSQL.Append("'" + representante.Nome + "',");
                sbSQL.Append("'" + representante.CPF + "',");
                sbSQL.Append(Conversao.ToDateOracle(representante.DataNascimento) + ", ");
                sbSQL.Append("'" + representante.Sexo + "',");
                sbSQL.Append( valida(representante.Nacionalidade.ToString()) + ",");
                sbSQL.Append("'" + representante.UFNascimento + "',");
                sbSQL.Append( valida(representante.EstadoCivil.ToString()) + ",");
                sbSQL.Append("'" + representante.Conjugue + "',");
                sbSQL.Append( valida(representante.Profissao.ToString()) + ",");
                sbSQL.Append("'" + representante.NomePai + "',");
                sbSQL.Append("'" + representante.NomeMae + "',");
                sbSQL.Append("'" + representante.CEP + "',");
                sbSQL.Append("'" + representante.Logradouro + "',");
                sbSQL.Append("'" + representante.Numero + "',");
                sbSQL.Append("'" + representante.Complemento + "',");
                sbSQL.Append("'" + representante.Bairro + "',");
                sbSQL.Append("'" + representante.Cidade + "',");
                sbSQL.Append("'" + representante.Estado + "',");
                sbSQL.Append("'" + representante.Pais + "',");
                sbSQL.Append("'" + representante.DDDTelefone + "',");
                sbSQL.Append("'" + representante.Telefone + "',");
                sbSQL.Append("'" + representante.DDDCelular + "',");
                sbSQL.Append("'" + representante.Celular + "',");
                sbSQL.Append("'" + representante.TipoDocumento + "',");
                sbSQL.Append("'" + representante.OrgaoEmissorDocumento + "',");
                sbSQL.Append("'" + representante.NumeroDocumento + "', ");
                sbSQL.Append(Conversao.ToDateOracle(representante.DataEmissaoDocumento)+ ", ");
                sbSQL.Append("'" + representante.Naturalidade + "', ");
                sbSQL.Append("'" + representante.UFEmissaoDocumento + "', ");
                sbSQL.Append("'" + representante.PaisNascimento + "', ");
                sbSQL.Append( valida(representante.SituacaoLegal.ToString()) + ", ");
                sbSQL.Append("'" + representante.UFNascimentoEstrangeiro + "')");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Método para validar se o parâmetro não é Nulo.
        /// </summary>
        /// <param name="valor">Conteúdo</param>
        /// <returns>Retorna o valor/NULL</returns>
        private string valida(string valor)
        {
            if (valor.Length > 0)
                return valor;
            else
                return "NULL";
        
        }
        /// <summary>
        /// Método para alterar um representante
        /// </summary>
        /// <param name="representante">Dados do representante a ser alterado</param>
        /// <returns>Número de linhas afetadas</returns>
        public int Alterar(ERepresentante representante)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("UPDATE REPRESENTANTE SET ");
                sbQuery.Append("Email = '" + representante.Email + "',");
                sbQuery.Append("Nome = '" + representante.Nome + "',");
                sbQuery.Append("CPF = '" + representante.CPF + "',");
                sbQuery.Append("DataNascimento = " + Conversao.ToDateOracle(representante.DataNascimento)+ ",");
                sbQuery.Append("Sexo = '" + representante.Sexo + "',");
                sbQuery.Append("Nacionalidade = '" + representante.Nacionalidade + "',");
                sbQuery.Append("Naturalidade = '" + representante.Naturalidade + "',");
                sbQuery.Append("EstadoCivil = " + ValidaInt(Conversao.ToInt(representante.EstadoCivil)) + ",");
                sbQuery.Append("Conjugue = '" + representante.Conjugue + "',");
                sbQuery.Append("Profissao = " + ValidaInt(Conversao.ToInt(representante.Profissao)) + ",");
                sbQuery.Append("NomePai = '" + representante.NomePai + "',");
                sbQuery.Append("NomeMae = '" + representante.NomeMae + "',");
                sbQuery.Append("CEP = '" + representante.CEP + "',");
                sbQuery.Append("Logradouro = '" + representante.Logradouro + "',");
                sbQuery.Append("Numero = '" + representante.Numero + "',");
                sbQuery.Append("Complemento = '" + representante.Complemento + "',");
                sbQuery.Append("Bairro = '" + representante.Bairro + "',");
                sbQuery.Append("Estado = '" + representante.Estado + "',");
                sbQuery.Append("Cidade = '" + representante.Cidade + "',");
                sbQuery.Append("Pais = '" + representante.Pais + "',");
                sbQuery.Append("DDDTelefone = '" + representante.DDDTelefone + "',");
                sbQuery.Append("Telefone = '" + representante.Telefone + "',");
                sbQuery.Append("DDDCelular = '" + representante.DDDCelular + "',");
                sbQuery.Append("Celular = '" + representante.Celular + "',");
                sbQuery.Append("TipoDocumento = '" + representante.TipoDocumento + "',");
                sbQuery.Append("OrgaoEmissorDocumento = '" + representante.OrgaoEmissorDocumento + "',");
                sbQuery.Append("NumeroDocumento = '" + representante.NumeroDocumento + "',");
                sbQuery.Append("DataEmissaoDocumento = " + Conversao.ToDateOracle(representante.DataEmissaoDocumento)+ ",");
                sbQuery.Append("UFEmissaoDocumento = '" + representante.UFEmissaoDocumento + "', ");
                sbQuery.Append("PaisNascimento = '" + representante.PaisNascimento + "', ");
                sbQuery.Append("UFNascimentoEstrangeiro = '" + representante.UFNascimentoEstrangeiro + "', ");
                sbQuery.Append("SituacaoLegal = '" + representante.SituacaoLegal + "'");
                sbQuery.Append(" where id_representante =" + representante.ID_Representante.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Valida se um valor é numérico(INT).
        /// </summary>
        /// <param name="valor">Conteúdo</param>
        /// <returns>Retorna o Valor/NULL</returns>
        private string ValidaInt(int? valor) {
            if (null == valor)
                return "NULL";
            else
                return valor.ToString();
        
        }
        /// <summary>
        /// Método para excluir um representante
        /// </summary>
        /// <param name="id_representante">Código do representante</param>
        /// <returns>Número de linhas afetadas</returns>
        public int Excluir(int id_representante)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("Delete from representante where id_representante = " + id_representante);

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Verifica se um cliente já possui representante.
        /// </summary>
        /// <param name="Id_Cliente">Código do cliente</param>
        /// <returns>Retorna o código do representante</returns>
        public int? VerificarExistencia(int Id_Cliente)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.Append("SELECT ID_Representante FROM Representante WHERE ");
            sbSQL.Append("ID_Cliente = " + Id_Cliente.ToString());

            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
            int? Result = Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand));

            return Result;
        }
    }
}
