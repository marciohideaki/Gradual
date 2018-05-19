using System;
using System.Collections.Generic;
using System.Data;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Text;
using Gradual.Generico.Geral;
using System.ComponentModel;

namespace Gradual.Cadastro.Negocios
{
    public class NCliente
    {
        public enum ETipoPremissao { 
            HBAcesso,
            HBSemAcesso,
            MinhaContaAcesso,
            MinhaContaSemAcesso
        }

        public static string NAOENCONTRADO = "Nenhum cliente encontrado!";


        public int GetIdAssessor(int pIdAssessorFilial) {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, "select id_assessorsinacor from assessor,assessorfilial where assessor.id_assessor=assessorfilial.id_assessor and id_assessorfilial = " + pIdAssessorFilial.ToString());
            return Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand)).Value;

        }

        public int GetAssessorSinacor(int _id_Cliente) {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" SELECT id_assessorsinacor ");
            sbSQL.Append(" FROM assessor,assessorfilial,cliente");
            sbSQL.Append(" WHERE assessor.id_assessor = assessorfilial.id_assessor ");
            sbSQL.Append(" and assessorfilial.id_assessorfilial = cliente.id_assessorfilial ");
            sbSQL.Append(" and cliente.id_cliente = "+_id_Cliente.ToString());
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
           return  Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand)).Value;
         
        }

        /// <summary>
        /// Constante contendo a Mensagem de CPF já cadastrado
        /// </summary>
        private const string CPF_Cadastrado = @"O CPF informado já está cadastrado na Gradual. Caso tenha iniciado o seu cadastro na Gradual, faça o seu login e complete o seu cadastro. Em caso de dúvidas, entre em contato com o nosso Atendimento.";

        public int GetQuantidadeAcesso(int TipoCliente, ETipoPremissao TipoPermissao) {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                string query ="";

                switch (TipoPermissao)
                {
                    case ETipoPremissao.HBAcesso:
                        query = "select count(*) from cliente where tipo = " + TipoCliente + " and (PermissaoHB <> 'N' or PermissaoHB is null) ";
                        break;
                    case ETipoPremissao.HBSemAcesso:
                        query = "select count(*) from cliente where tipo = " + TipoCliente + " and PermissaoHB = 'N' ";
                        break;
                    case ETipoPremissao.MinhaContaAcesso:
                        query = "select count(*) from cliente where tipo = " + TipoCliente + " and (PermissaoMinhaConta <> 'N' or PermissaoMinhaConta is null)  ";
                        break;
                    case ETipoPremissao.MinhaContaSemAcesso:
                        query = "select count(*) from cliente where tipo = " + TipoCliente + " and PermissaoMinhaConta = 'N' ";
                        break;
                    default:
                        break;
                }        

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, query);
                return Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand)).Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetAcesso(int TipoCliente,ETipoPremissao TipoPermissao)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                string query = "";

                switch (TipoPermissao)
                {
                    case ETipoPremissao.HBAcesso:
                        query = "update cliente set PermissaoHB='S' where tipo = " + TipoCliente.ToString();
                        break;
                    case ETipoPremissao.HBSemAcesso:
                        query = "update cliente set PermissaoHB='N' where tipo = " + TipoCliente.ToString();
                        break;
                    case ETipoPremissao.MinhaContaAcesso:
                        query = "update cliente set PermissaoMinhaConta='S' where tipo = " + TipoCliente.ToString();
                        break;
                    case ETipoPremissao.MinhaContaSemAcesso:
                        query = "update cliente set PermissaoMinhaConta='N' where tipo = " + TipoCliente.ToString();
                        break;
                    default:
                        break;
                }
          
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, query);
                _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AtualizaPermissaoHB(int id_cliente, Boolean permissao)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                string _permissao = "N";
                if (permissao)
                    _permissao = "S";
                sbQuery.Append("update cliente set permissaohb='" + _permissao + "' where id_cliente = " + id_cliente);
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                return  Conversao.ToInt(_AcessaDados.ExecuteNonQuery(_DbCommand)).Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime GetDataNascimento(string cpf) {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("select datanascimento from cliente where cpf = '" + cpf+"' or cpf = '"+Int64.Parse(cpf).ToString()+"'");
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                DateTime? nasc = Conversao.ToDateTime(_AcessaDados.ExecuteScalar(_DbCommand));
                if (null == nasc)
                {
                    throw new Exception("Cliente não consta no sistema de Cadastro");
                }
                else
                {
                    return nasc.Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetNome(string cpf)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("select nome from login,cliente where login.id_login=cliente.id_login and (cpf = '" + cpf + "' or cpf = '" + Int64.Parse(cpf).ToString() + "')");
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                return Conversao.ToString(_AcessaDados.ExecuteScalar(_DbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AtualizaPermissaoMinhaConta(int id_cliente, Boolean permissao)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                string _permissao = "N";
                if (permissao)
                    _permissao = "S";
                sbQuery.Append("update cliente set permissaominhaconta='" + _permissao + "' where id_cliente = " + id_cliente);
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                return Conversao.ToInt(_AcessaDados.ExecuteNonQuery(_DbCommand)).Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Informa se o CLiente Possui CBLC
        /// </summary>
        /// <param name="id_cliente">Id do Cliente a ser verificado</param>
        /// <returns>True se o cliente possuir CBLC e False se não Possuir</returns>
        public Boolean PossuiCBLC(int id_cliente)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("select codigobovespa from cliente where id_cliente = " + id_cliente);
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                int? cblc = Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand));
                if (null == cblc || 0 == cblc)
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Informa o Código CBLC do Cliente
        /// </summary>
        /// <param name="id_cliente">Id do Cliente</param>
        /// <returns>Código CBLC caso o cliente possua, caso o cliente não possua, retorna null</returns>
        public int? GetCBLC(int id_cliente)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("select codigobovespa from cliente where id_cliente = " + id_cliente);
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                int? cblc = Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand));
                return cblc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int? GetIdCliente(int icblc)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("select id_cliente from cliente where codigobovespa = " + icblc);
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                int? cblc = Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand));
                return cblc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetDigitoInvestidor(string cblc)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("Select DV_CLIENTE from TSCCLIBOL where CD_CLIENTE = ");
                sbQuery.Append(cblc);
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());

                return int.Parse(_AcessaDados.ExecuteScalar(_DbCommand).ToString()); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Seleciona um cliente
        /// </summary>
        /// <param name="id_cliente">Id do Cliente</param>
        /// <param name="Passo">Passo do Cliente</param>
        /// <returns>Entidade contendo os dados constantes no Passo do cliente</returns>
        public ECliente Listar(int id_cliente, int Passo)
        {
            DataTable _DtDados;
            try
            {
                ECliente _ECliente = new ECliente();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("select * from cliente where id_cliente = " + id_cliente);
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                _DtDados = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_DtDados.Rows.Count > 0)
                {
                    _ECliente.ID_Cliente = Conversao.ToInt(_DtDados.Rows[0]["ID_Cliente"]);
                    _ECliente.DataAprovacaoFinal = Conversao.ToDateTime(_DtDados.Rows[0]["DataAprovacaoFinal"]);
                    _ECliente.DataCadastroInicial = Conversao.ToDateTime(_DtDados.Rows[0]["DataCadastroInicial"]);
                    _ECliente.DataProximaAtualizacao = Conversao.ToDateTime(_DtDados.Rows[0]["DataProximaAtualizacao"]);
                    _ECliente.ID_AssessorFilial = Conversao.ToInt(_DtDados.Rows[0]["ID_AssessorFilial"]);
                    _ECliente.ID_Login = Conversao.ToInt(_DtDados.Rows[0]["ID_Login"]);
                    _ECliente.LoginCadastrante = Conversao.ToInt(_DtDados.Rows[0]["LoginCadastrante"]);
                    _ECliente.Liberado = Conversao.ToChar(_DtDados.Rows[0]["Liberado"]);
                    _ECliente.Tipo = Conversao.ToInt(_DtDados.Rows[0]["Tipo"]);
                    if (Passo == 1)
                    {
                        _ECliente.CPF = Conversao.ToString(_DtDados.Rows[0]["CPF"]);
                        _ECliente.Sexo = Conversao.ToChar(_DtDados.Rows[0]["Sexo"]);
                        _ECliente.Conheceu = Conversao.ToInt(_DtDados.Rows[0]["Conheceu"]);
                        _ECliente.ConheceuOutros = Conversao.ToString(_DtDados.Rows[0]["ConheceuOutros"]);
                        _ECliente.Passo = Conversao.ToInt(_DtDados.Rows[0]["Passo"]);
                        _ECliente.DataNascimento = Conversao.ToDateTime(_DtDados.Rows[0]["DataNascimento"]);
                    }
                    else if (Passo == 2)
                    {
                        _ECliente.Nacionalidade = Conversao.ToInt(_DtDados.Rows[0]["Nacionalidade"]);
                        _ECliente.UFNascimento = Conversao.ToString(_DtDados.Rows[0]["UFNascimento"]);
                        _ECliente.Naturalidade = Conversao.ToString(_DtDados.Rows[0]["Naturalidade"]);
                        _ECliente.EstadoCivil = Conversao.ToInt(_DtDados.Rows[0]["EstadoCivil"]);
                        _ECliente.Conjugue = Conversao.ToString(_DtDados.Rows[0]["Conjugue"]);
                        _ECliente.NomeMae = Conversao.ToString(_DtDados.Rows[0]["NomeMae"]);
                        _ECliente.NomePai = Conversao.ToString(_DtDados.Rows[0]["NomePai"]);
                        _ECliente.NumeroDocumento = Conversao.ToString(_DtDados.Rows[0]["NumeroDocumento"]);
                        _ECliente.TipoDocumento = Conversao.ToString(_DtDados.Rows[0]["TipoDocumento"]);
                        _ECliente.OrgaoEmissorDocumento = Conversao.ToString(_DtDados.Rows[0]["OrgaoEmissorDocumento"]);
                        _ECliente.DataEmissaoDocumento = Conversao.ToDateTime(_DtDados.Rows[0]["DataEmissaoDocumento"]);
                        _ECliente.EstadoEmissaoDocumento = Conversao.ToString(_DtDados.Rows[0]["EstadoEmissaoDocumento"]);
                        _ECliente.Profissao = Conversao.ToInt(_DtDados.Rows[0]["Profissao"]);
                        _ECliente.Salario = Conversao.ToDecimal(_DtDados.Rows[0]["Salario"]);
                        _ECliente.OutrosRendimentosDescricao = Conversao.ToString(_DtDados.Rows[0]["OutrosRendimentosDescricao"]);
                        _ECliente.OutrosRendimentosValor = Conversao.ToDecimal(_DtDados.Rows[0]["OutrosRendimentosValor"]);
                        _ECliente.Representante = Conversao.ToChar(_DtDados.Rows[0]["Representante"]);
                        _ECliente.PessoaVinculada = Conversao.ToChar(_DtDados.Rows[0]["PessoaVinculada"]);
                        _ECliente.PPE = Conversao.ToChar(_DtDados.Rows[0]["PPE"]);
                        _ECliente.CarteiraPropria = Conversao.ToChar(_DtDados.Rows[0]["CarteiraPropria"]);
                        _ECliente.CVM387 = Conversao.ToChar(_DtDados.Rows[0]["CVM387"]);
                        _ECliente.EmailComercial = Conversao.ToString(_DtDados.Rows[0]["EmailComercial"]);
                        _ECliente.Empresa = Conversao.ToString(_DtDados.Rows[0]["Empresa"]);
                        _ECliente.PaisNascimento = Conversao.ToString(_DtDados.Rows[0]["PaisNascimento"]);
                        _ECliente.UFNascimentoEstrangeiro = Conversao.ToString(_DtDados.Rows[0]["UFNascimentoEstrangeiro"]);
                        _ECliente.Cargo = Conversao.ToString(_DtDados.Rows[0]["Cargo"]);
                        _ECliente.Emancipado = Conversao.ToString(_DtDados.Rows[0]["Emancipado"]);
                        _ECliente.AutorizaTerceiro = Conversao.ToChar(_DtDados.Rows[0]["AutorizaTerceiro"]);
                    }
                }
                return _ECliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> ListarPasso123() {
            DataTable _DtDados;
            try
            {
                List<int> Retorno = new List<int>();
                
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("select id_cliente from cliente where passo < 4");
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                _DtDados = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _DtDados.Rows)
                {
                    Retorno.Add(Conversao.ToInt(item["ID_Cliente"]).Value);
                }
                return Retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        /// <summary>
        ///  Seleciona um cliente
        /// </summary>
        /// <param name="id_cliente">Id do Cliente</param>
        /// <returns>Entidade contendo os dados do cliente</returns>
        public ECliente Listar(int id_cliente)
        {
            DataTable _DtDados;
            try
            {
                ECliente _ECliente = new ECliente();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("select * from cliente where id_cliente = " + id_cliente);
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                _DtDados = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_DtDados.Rows.Count > 0)
                {
                    _ECliente.ID_Cliente = Conversao.ToInt(_DtDados.Rows[0]["ID_Cliente"]);
                    _ECliente.DataAprovacaoFinal = Conversao.ToDateTime(_DtDados.Rows[0]["DataAprovacaoFinal"]);
                    _ECliente.DataCadastroInicial = Conversao.ToDateTime(_DtDados.Rows[0]["DataCadastroInicial"]);
                    _ECliente.DataProximaAtualizacao = Conversao.ToDateTime(_DtDados.Rows[0]["DataProximaAtualizacao"]);
                    _ECliente.ID_AssessorFilial = Conversao.ToInt(_DtDados.Rows[0]["ID_AssessorFilial"]);
                    _ECliente.ID_Login = Conversao.ToInt(_DtDados.Rows[0]["ID_Login"]);
                    _ECliente.LoginCadastrante = Conversao.ToInt(_DtDados.Rows[0]["LoginCadastrante"]);
                    _ECliente.Liberado = Conversao.ToChar(_DtDados.Rows[0]["Liberado"]);
                    _ECliente.CPF = Conversao.ToString(_DtDados.Rows[0]["CPF"]);
                    _ECliente.Sexo = Conversao.ToChar(_DtDados.Rows[0]["Sexo"]);
                    _ECliente.Conheceu = Conversao.ToInt(_DtDados.Rows[0]["Conheceu"]);
                    _ECliente.ConheceuOutros = Conversao.ToString(_DtDados.Rows[0]["ConheceuOutros"]);
                    _ECliente.Passo = Conversao.ToInt(_DtDados.Rows[0]["Passo"]);
                    _ECliente.DataNascimento = Conversao.ToDateTime(_DtDados.Rows[0]["DataNascimento"]);
                    _ECliente.Nacionalidade = Conversao.ToInt(_DtDados.Rows[0]["Nacionalidade"]);
                    _ECliente.UFNascimento = Conversao.ToString(_DtDados.Rows[0]["UFNascimento"]);
                    _ECliente.Naturalidade = Conversao.ToString(_DtDados.Rows[0]["Naturalidade"]);
                    _ECliente.EstadoCivil = Conversao.ToInt(_DtDados.Rows[0]["EstadoCivil"]);
                    _ECliente.Conjugue = Conversao.ToString(_DtDados.Rows[0]["Conjugue"]);
                    _ECliente.NomeMae = Conversao.ToString(_DtDados.Rows[0]["NomeMae"]);
                    _ECliente.NomePai = Conversao.ToString(_DtDados.Rows[0]["NomePai"]);
                    _ECliente.Emancipado = Conversao.ToString(_DtDados.Rows[0]["Emancipado"]);
                    _ECliente.NumeroDocumento = Conversao.ToString(_DtDados.Rows[0]["NumeroDocumento"]);
                    _ECliente.TipoDocumento = Conversao.ToString(_DtDados.Rows[0]["TipoDocumento"]);
                    _ECliente.OrgaoEmissorDocumento = Conversao.ToString(_DtDados.Rows[0]["OrgaoEmissorDocumento"]);
                    _ECliente.DataEmissaoDocumento = Conversao.ToDateTime(_DtDados.Rows[0]["DataEmissaoDocumento"]);
                    _ECliente.EstadoEmissaoDocumento = Conversao.ToString(_DtDados.Rows[0]["EstadoEmissaoDocumento"]);
                    _ECliente.Profissao = Conversao.ToInt(_DtDados.Rows[0]["Profissao"]);
                    _ECliente.Salario = Conversao.ToDecimal(_DtDados.Rows[0]["Salario"]);
                    _ECliente.OutrosRendimentosDescricao = Conversao.ToString(_DtDados.Rows[0]["OutrosRendimentosDescricao"]);
                    _ECliente.OutrosRendimentosValor = Conversao.ToDecimal(_DtDados.Rows[0]["OutrosRendimentosValor"]);
                    _ECliente.Representante = Conversao.ToChar(_DtDados.Rows[0]["Representante"]);
                    _ECliente.PessoaVinculada = Conversao.ToChar(_DtDados.Rows[0]["PessoaVinculada"]);
                    _ECliente.PPE = Conversao.ToChar(_DtDados.Rows[0]["PPE"]);
                    _ECliente.CarteiraPropria = Conversao.ToChar(_DtDados.Rows[0]["CarteiraPropria"]);
                    _ECliente.CVM387 = Conversao.ToChar(_DtDados.Rows[0]["CVM387"]);
                    _ECliente.EmailComercial = Conversao.ToString(_DtDados.Rows[0]["EmailComercial"]);
                    _ECliente.Empresa = Conversao.ToString(_DtDados.Rows[0]["Empresa"]);
                    _ECliente.PaisNascimento = Conversao.ToString(_DtDados.Rows[0]["PaisNascimento"]);
                    _ECliente.UFNascimentoEstrangeiro = Conversao.ToString(_DtDados.Rows[0]["UFNascimentoEstrangeiro"]);
                    _ECliente.Cargo = Conversao.ToString(_DtDados.Rows[0]["Cargo"]);
                    _ECliente.Tipo = Conversao.ToInt(_DtDados.Rows[0]["Tipo"]);
                    _ECliente.AutorizaTerceiro = Conversao.ToChar(_DtDados.Rows[0]["AutorizaTerceiro"]);
                    _ECliente.CodigoBovespa = Conversao.ToString(_DtDados.Rows[0]["CodigoBovespa"]);
                }
                return _ECliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Altera os dados do Cliente
        /// </summary>
        /// <param name="cliente">Entidade contendo os dados do Cliente</param>
        /// <param name="Passo">Passo do Cliente</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Alterar(ECliente cliente, int Passo)
        {
            try
            {
                int passoAtual = Conversao.ToInt(new NCliente().Listar(cliente.ID_Cliente.Value).Passo).Value;

                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("UPDATE CLIENTE SET ");
                if (Passo == 1)
                {
                    VerificaCPF(cliente.ID_Cliente, cliente.CPF);

                    sbQuery.Append(" CPF = '" + cliente.CPF + "',");
                    sbQuery.Append(" DataNascimento = " + Conversao.ToDateOracle(cliente.DataNascimento) + ",");
                    sbQuery.Append(" SEXO = '" + cliente.Sexo + "',");
                    sbQuery.Append(" CONHECEU = " + cliente.Conheceu + ",");
                    sbQuery.Append(" CONHECEUOUTROS = '" + cliente.ConheceuOutros + "',");
                    sbQuery.Append(" ID_AssessorFilial = " + cliente.ID_AssessorFilial + " ");

                }
                else if (Passo == 2)
                {
                    sbQuery.Append(" Nacionalidade = " + cliente.Nacionalidade + ",");
                    sbQuery.Append(" UFNascimento = '" + cliente.UFNascimento + "',");
                    sbQuery.Append(" Naturalidade = '" + cliente.Naturalidade + "',");
                    sbQuery.Append(" EstadoCivil = " + cliente.EstadoCivil + ",");
                    sbQuery.Append(" Conjugue = '" + cliente.Conjugue + "',");
                    sbQuery.Append(" NomePai = '" + cliente.NomePai + "',");
                    sbQuery.Append(" NomeMae = '" + cliente.NomeMae + "',");
                    sbQuery.Append(" NumeroDocumento = '" + cliente.NumeroDocumento + "',");
                    sbQuery.Append(" TipoDocumento = '" + cliente.TipoDocumento + "',");
                    sbQuery.Append(" OrgaoEmissorDocumento = '" + cliente.OrgaoEmissorDocumento + "',");
                    sbQuery.Append(" DataEmissaoDocumento = " + Conversao.ToDateOracle(cliente.DataEmissaoDocumento) + ",");
                    sbQuery.Append(" EstadoEmissaoDocumento = '" + cliente.EstadoEmissaoDocumento + "',");
                    sbQuery.Append(" Profissao = " + cliente.Profissao + ",");
                    sbQuery.Append(" Salario = " + cliente.SalarioString + ",");
                    sbQuery.Append(" OutrosRendimentosDescricao = '" + cliente.OutrosRendimentosDescricao + "',");
                    sbQuery.Append(" OutrosRendimentosValor = " + cliente.OutrosRendimentosValorString + ",");
                    sbQuery.Append(" Representante = '" + cliente.Representante + "',");
                    sbQuery.Append(" PessoaVinculada = '" + cliente.PessoaVinculada + "',");
                    sbQuery.Append(" PPE = '" + cliente.PPE + "',");
                    sbQuery.Append(" CarteiraPropria  = '" + cliente.CarteiraPropria + "',");
                    sbQuery.Append(" CVM387 = '" + cliente.CVM387 + "',");
                    sbQuery.Append(" EmailComercial = '" + cliente.EmailComercial + "',");
                    sbQuery.Append(" Empresa = '" + cliente.Empresa + "',");
                    sbQuery.Append(" PaisNascimento = '" + cliente.PaisNascimento + "',");
                    sbQuery.Append(" UFNascimentoEstrangeiro = '" + cliente.UFNascimentoEstrangeiro + "',");
                    if (null != cliente.AutorizaTerceiro)
                        sbQuery.Append(" AutorizaTerceiro = '" + cliente.AutorizaTerceiro + "',");
                    if (passoAtual != 4)
                    {
                        if (passoAtual == 1)
                        { sbQuery.Append(" Passo = '2',"); }
                        else
                        {
                            sbQuery.Append(" Passo = '" + Conversao.ToInt(cliente.Passo).ToString() + "',");
                        }
                    }
                    sbQuery.Append(" Emancipado = '" + cliente.Emancipado + "',");
                    sbQuery.Append(" Cargo = '" + cliente.Cargo + "' ");


                }
                else if (Passo == 3)
                {
                    sbQuery.Append(" Representante = '" + cliente.Representante + "',");
                    sbQuery.Append(" PessoaVinculada = '" + cliente.PessoaVinculada + "',");
                    sbQuery.Append(" PPE = '" + cliente.PPE + "',");
                    sbQuery.Append(" CarteiraPropria  = '" + cliente.CarteiraPropria + "',");
                    sbQuery.Append(" CVM387 = '" + cliente.CVM387 + "',");
                    sbQuery.Append(" AutorizaTerceiro = '" + cliente.AutorizaTerceiro + "',");
                    sbQuery.Append(" DataPasso3 = " + Conversao.ToDateOracle(DateTime.Now) + ",");
                    if (passoAtual != 4)
                        sbQuery.Append(" Passo = '" + Conversao.ToInt(cliente.Passo).ToString() + "',");
                    sbQuery.Append(" Emancipado = '" + cliente.Emancipado + "'");

                }

                sbQuery.Append(" WHERE id_cliente = " + cliente.ID_Cliente.ToString());
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                int ret = _AcessaDados.ExecuteNonQuery(_DbCommand);

                //Tratamento de Pendência de certidão de casamento
                if (Passo == 2)
                {
                    if (cliente.EstadoCivil == 1 ||
                        cliente.EstadoCivil == 2 ||
                        cliente.EstadoCivil == 3 ||
                        cliente.EstadoCivil == 4)
                    {
                        sbQuery = new StringBuilder();
                        sbQuery.Append("update pendencia set certidaocasamento='N' where dataresolucao is null and id_cliente = " + cliente.ID_Cliente.Value.ToString());
                        _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                        _AcessaDados.ExecuteNonQuery(_DbCommand);
                    }

                }
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Altera o Cliente para o Passo 3
        /// </summary>
        /// <param name="id_cliente">Id do Cliente</param>
        public void AlterarPasso3(int id_cliente)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("update cliente set passo = '3' , DataPasso3 = " + Conversao.ToDateOracle(DateTime.Now) + " where id_cliente = " + id_cliente.ToString());
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbQuery.ToString());
                _AcessaDados.ExecuteNonQuery(_DbCommand);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Verifica se o CPF/CNPJ já está cadastrado e retorna uma Excessão caso esteja
        /// </summary>
        /// <param name="Id_Cliente">Id do Cliente para não utilizar no filtro</param>
        /// <param name="Cpf">CPF a ser validado</param>
        private void VerificaCPF(int? Id_Cliente, string Cpf)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("SELECT Count(*) FROM Cliente WHERE CPF = '" + Cpf + "'");
            sbSQL.Append(" AND ID_Cliente <> " + Id_Cliente.ToString());
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
            int Result = Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand)).Value;
            if (Result != 0)
                throw new Exception(CPF_Cadastrado);
        }

        public int GetId(int id_Login)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("SELECT id_cliente FROM Cliente WHERE id_login = " + id_Login.ToString() );
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
            return Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand)).Value;
        }

        public BindingList<ECliente> Listar(string cpf, string nome)
        {
            try
            {
                BindingList<ECliente> _Clientes = new BindingList<ECliente>();
                StringBuilder query = new StringBuilder();
                query.Append("select id_cliente,logincliente.nome as nome,cpf,sexo,datanascimento,id_assessorsinacor||' - '||loginassessor.nome||' / '||filial.nome as assessor ");
                query.Append(" from cliente,login loginassessor,login logincliente,assessor,assessorfilial,filial ");
                query.Append(" where assessorfilial.id_assessorfilial=cliente.id_assessorfilial ");
                query.Append(" and filial.id_filial=assessorfilial.id_filial ");
                query.Append(" and assessor.id_assessor=assessorfilial.id_assessor ");
                query.Append(" and loginassessor.id_login=assessor.id_login ");
                query.Append(" and logincliente.id_login=cliente.id_login ");
                if (nome != "")
                    query.Append(" and upper(logincliente.nome) like upper('%" + nome + "%') ");
                if (cpf != "")
                    query.Append(" and (cpf like'%" + cpf + "%' or cpf like'%" + Int64.Parse(cpf).ToString() + "%') ");
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, query.ToString());
                DataTable _DtDados = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_DtDados.Rows.Count == 0)
                    throw new Exception(NAOENCONTRADO);

                foreach (DataRow dr in _DtDados.Rows)
                {
                    ECliente cliente = new ECliente();
                    cliente.NomePai = Conversao.ToString(dr["nome"]);
                    cliente.ID_Cliente = Conversao.ToInt(dr["id_cliente"]);
                    cliente.CPF = Conversao.ToString(dr["cpf"]);
                    cliente.Sexo = Conversao.ToChar(dr["sexo"]);
                    cliente.DataNascimento = Conversao.ToDateTime(dr["datanascimento"]);
                    cliente.NomeMae = Conversao.ToString(dr["assessor"]);


                    _Clientes.Add(cliente);
                }

                return _Clientes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
